//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using Declarations;
using Declarations.Discovery;
using Declarations.Media;
using Declarations.MediaLibrary;
using Declarations.Players;
using Declarations.VLM;
using Implementation.Discovery;
using Implementation.Exceptions;
using Implementation.Loggers;
using Implementation.Media;
using Implementation.MediaLibrary;
using Implementation.Players;
using Implementation.Utils;
using Implementation.VLM;
using LibVlcWrapper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Implementation
{
    /// <summary>
    /// Entry point for the nVLC library.
    /// </summary>
    public class MediaPlayerFactory : DisposableBase, IMediaPlayerFactory, IReferenceCount, INativePointer
    {
        IntPtr _mHMediaLib = IntPtr.Zero;
        IVideoLanManager _mVlm = null;
        NLogger _mLogger = new NLogger();
        LogSubscriber _mLog;
        string _mCurrentDir;

        /// <summary>
        /// Initializes media library with default arguments
        /// </summary>
        /// <param name="findLibvlc"></param>
        /// <param name="useCustomStringMarshaller"></param>
        /// <remarks>Default arguments:
        /// "-I",
        /// "dumy",  
		/// "--ignore-config", 
        /// "--no-osd",
        /// "--disable-screensaver",
        /// "--plugin-path=./plugins"
        /// </remarks>
        public MediaPlayerFactory(bool findLibvlc = false, bool useCustomStringMarshaller = false)
        {
            var args = new string[]
            {
                "-I", 
                "dumy",  
		        "--ignore-config", 
                "--no-osd",
                "--disable-screensaver",
		        "--plugin-path=./plugins" 
            };

            Initialize(args, findLibvlc, useCustomStringMarshaller);
        }

        /// <summary>
        /// Initializes media library with user defined arguments
        /// </summary>
        /// <param name="args">Collection of arguments passed to libVLC library</param>
        /// <param name="findLibvlc">True to find libvlc installation path, False to use libvlc in the executable path</param>
        /// <param name="useCustomStringMarshaller"></param>
        public MediaPlayerFactory(string[] args, bool findLibvlc = false, bool useCustomStringMarshaller = false)
        {
            Initialize(args, findLibvlc, useCustomStringMarshaller);
        }

        private void Initialize(string[] args, bool findLibvlc, bool useCustomStringMarshaller)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            if (findLibvlc)
            {
                TrySetVLCPath();
            }

            try
            {
                if (useCustomStringMarshaller)
                    _mHMediaLib = LibVlcMethods.libvlc_new_custom_marshaller(args.Length, args);
                else
                    _mHMediaLib = LibVlcMethods.libvlc_new(args.Length, args);
            }
            catch (DllNotFoundException ex)
            {
                throw new LibVlcNotFoundException(ex);
            }

            if (_mHMediaLib == IntPtr.Zero)
            {
                throw new LibVlcInitException();
            }

            if (findLibvlc)
            {
                Directory.SetCurrentDirectory(_mCurrentDir);
            }

            TrySetupLogging();
            TryFilterRemovedModules();            
        }

        private void TryFilterRemovedModules()
        {
            try
            {
                const string pattern = @"\d+(\.\d+)+"; // numbers separated by dots
                var versionMatch = Regex.Match(Version, pattern);
                var libVlcVersion = new Version(versionMatch.Value);
                ObjectFactory.FilterRemovedModules(libVlcVersion);
            }
            catch (Exception)
            { }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exc = e.ExceptionObject as Exception;
            if (exc != null)
            {
                var error = MiscUtils.FindNestedException<EntryPointNotFoundException>(exc);
                if (error != null)
                {
                    var ver = MiscUtils.GetMinimalSupportedVersion(error);
                    _mLogger.Error(string.Format("Method {0} supported starting libVLC version {1}", error.TargetSite.Name, ver));
                }
                else
                {
                    var msg = MiscUtils.LogNestedException(exc);
                    _mLogger.Error("Unhandled exception: " + msg);
                }
            }

            if (e.IsTerminating)
            {
                _mLogger.Error("Due to unhandled exception the application will terminate");
            }
        }

        private void TrySetupLogging()
        {
            try
            {
                _mLog = new LogSubscriber(_mLogger, _mHMediaLib);
            }
            catch (EntryPointNotFoundException ex)
            {
                var name = ex.TargetSite.Name;
                var minVersion = MiscUtils.GetMinimalSupportedVersion(ex);
                if (!string.IsNullOrEmpty(minVersion))
                {
                    var msg = string.Format("libVLC logging functinality enabled staring libVLC version {0} while you are using version {1}", minVersion, Version);
                    _mLogger.Warning(msg);
                }
                else
                {
                    _mLogger.Warning(ex.Message);
                }
            }
            catch (Exception ex)
            {
                var msg = string.Format("Failed to setup logging, reason : {0}", ex.Message);
                _mLogger.Error(msg);
            }
        }

        /// <summary>
        /// Creates new instance of player.
        /// </summary>
        /// <typeparam name="T">Type of the player to create</typeparam>
        /// <returns>Newly created player</returns>
        public T CreatePlayer<T>() where T : IPlayer
        {
            return ObjectFactory.Build<T>(_mHMediaLib);
        }

        /// <summary>
        /// Creates new instance of media list player
        /// </summary>
        /// <typeparam name="T">Type of media list player</typeparam>
        /// <param name="mediaList">Media list</param>
        /// <returns>Newly created media list player</returns>
        public T CreateMediaListPlayer<T>(IMediaList mediaList) where T : IMediaListPlayer
        {
            return ObjectFactory.Build<T>(_mHMediaLib, mediaList);
        }

        /// <summary>
        /// Creates new instance of media.
        /// </summary>
        /// <typeparam name="T">Type of media to create</typeparam>
        /// <param name="input">The media input string</param>
        /// <param name="options">Optional media options</param>
        /// <returns>Newly created media</returns>
        public T CreateMedia<T>(string input, params string[] options) where T : IMedia
        {
            var media = ObjectFactory.Build<T>(_mHMediaLib);
            media.Input = input;
            media.AddOptions(options);

            return media;
        }

        /// <summary>
        /// Creates new instance of media list.
        /// </summary>
        /// <typeparam name="T">Type of media list</typeparam>
        /// <param name="mediaItems">Collection of media inputs</param>       
        /// <param name="options"></param>
        /// <returns>Newly created media list</returns>
        public T CreateMediaList<T>(IEnumerable<string> mediaItems, params string[] options) where T : IMediaList
        {
            var mediaList = ObjectFactory.Build<T>(_mHMediaLib);
            foreach (var file in mediaItems)
            {
                mediaList.Add(this.CreateMedia<IMedia>(file, options));
            }

            return mediaList;
        }

        /// <summary>
        /// Creates media list instance with no media items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateMediaList<T>() where T : IMediaList
        {
            return ObjectFactory.Build<T>(_mHMediaLib);
        }

        /// <summary>
        /// Creates media discovery object
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMediaDiscoverer CreateMediaDiscoverer(string name)
        {
            return ObjectFactory.Build<IMediaDiscoverer>(_mHMediaLib, name);
        }

        /// <summary>
        /// Creates media library
        /// </summary>
        /// <returns></returns>
        public IMediaLibrary CreateMediaLibrary()
        {
            return ObjectFactory.Build<IMediaLibrary>(_mHMediaLib);
        }

        /// <summary>
        /// Gets the libVLC version.
        /// </summary>
        public string Version
        {
            get
            {
                var pStr = LibVlcMethods.libvlc_get_version();
                return Marshal.PtrToStringAnsi(pStr);
            }               
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            Release();
        }

        private static class ObjectFactory
        {
            static Dictionary<Type, Type> _objectMap = new Dictionary<Type, Type>();
            static Dictionary<Type, LibVlcRemovedModuleException> _removedModules = new Dictionary<Type, LibVlcRemovedModuleException>();
            
            static ObjectFactory()
            {
                _objectMap.Add(typeof(IMedia), typeof(BasicMedia));
                _objectMap.Add(typeof(IMediaFromFile), typeof(MediaFromFile));
                _objectMap.Add(typeof(IVideoInputMedia), typeof(VideoInputMedia));
                _objectMap.Add(typeof(IScreenCaptureMedia), typeof(ScreenCaptureMedia));
                _objectMap.Add(typeof(IPlayer), typeof(BasicPlayer));
                _objectMap.Add(typeof(IAudioPlayer), typeof(AudioPlayer));
                _objectMap.Add(typeof(IVideoPlayer), typeof(VideoPlayer));
                _objectMap.Add(typeof(IDiskPlayer), typeof(DiskPlayer));
                _objectMap.Add(typeof(IMediaList), typeof(MediaList));
                _objectMap.Add(typeof(IMediaListPlayer), typeof(MediaListPlayer));
                _objectMap.Add(typeof(IVideoLanManager), typeof(VideoLanManager));
                _objectMap.Add(typeof(IMediaDiscoverer), typeof(MediaDiscoverer));
                _objectMap.Add(typeof(IMediaLibrary), typeof(MediaLibraryImpl));
                _objectMap.Add(typeof(IMemoryInputMedia), typeof(MemoryInputMedia));
                _objectMap.Add(typeof(ICompositeMemoryInputMedia), typeof(CompositeMemoryInputMedia));
            }

            public static T Build<T>(params object[] args)
            {
                var t = typeof(T);
                if (_removedModules.ContainsKey(t))
                {
                    throw _removedModules[t];
                }
                if (!_objectMap.ContainsKey(t))
                {
                    throw new ArgumentException("Unregistered type", t.FullName);                  
                }

                return (T)Activator.CreateInstance(_objectMap[t], args);
            }

            public static void FilterRemovedModules(Version currentVersion)
            {
                foreach (var item in _objectMap)
                {
                    var maxVer = (MaxLibVlcVersion)Attribute.GetCustomAttribute(item.Value, typeof(MaxLibVlcVersion));
                    if (maxVer == null)
                    {
                        continue;
                    }
                    
                    var lastSupported = new Version(maxVer.MaxVersion);
                    if (currentVersion > lastSupported)
                    {
                        _removedModules[item.Key] = new LibVlcRemovedModuleException(maxVer.LibVlcModuleName, item.Key.Name, maxVer.MaxVersion);
                    }
                }
            }
        }

        #region IReferenceCount Members

        public void AddRef()
        {
            LibVlcMethods.libvlc_retain(_mHMediaLib);
        }

        public void Release()
        {
            try
            {
                LibVlcMethods.libvlc_release(_mHMediaLib);
            }
            catch (AccessViolationException)
            { }
        }

        #endregion

        #region INativePointer Members

        public IntPtr Pointer
        {
            get
            {
                return _mHMediaLib;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public long Clock
        {
            get
            {
                return LibVlcMethods.libvlc_clock();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public long Delay(long pts)
        {
            return LibVlcMethods.libvlc_delay(pts);
        }

        /// <summary>
        /// Gets list of available audio filters
        /// </summary>
        public IEnumerable<FilterInfo> AudioFilters
        {
            get
            {
                var pList = LibVlcMethods.libvlc_audio_filter_list_get(_mHMediaLib);
                var item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(pList, typeof(LibvlcModuleDescriptionT));

                do
                {
                    yield return GetFilterInfo(item);
                    if (item.p_next != IntPtr.Zero)
                    {
                        item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(item.p_next, typeof(LibvlcModuleDescriptionT));
                    }
                    else
                    {
                        break;
                    }

                }
                while (true);

                LibVlcMethods.libvlc_module_description_list_release(pList);
            }
        }

        /// <summary>
        /// Gets list of available video filters
        /// </summary>
        public IEnumerable<FilterInfo> VideoFilters
        {
            get
            {
                var pList = LibVlcMethods.libvlc_video_filter_list_get(_mHMediaLib);
                if (pList == IntPtr.Zero)
                {
                    yield break;
                }

                var item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(pList, typeof(LibvlcModuleDescriptionT));

                do
                {
                    yield return GetFilterInfo(item);
                    if (item.p_next != IntPtr.Zero)
                    {
                        item = (LibvlcModuleDescriptionT)Marshal.PtrToStructure(item.p_next, typeof(LibvlcModuleDescriptionT));
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);

                LibVlcMethods.libvlc_module_description_list_release(pList);
            }
        }

        private FilterInfo GetFilterInfo(LibvlcModuleDescriptionT item)
        {
            return new FilterInfo()
            {
                Help = Marshal.PtrToStringAnsi(item.psz_help),
                Longname = Marshal.PtrToStringAnsi(item.psz_longname),
                Name = Marshal.PtrToStringAnsi(item.psz_name),
                Shortname = Marshal.PtrToStringAnsi(item.psz_shortname)
            };
        }

        /// <summary>
        /// Gets the VLM instance
        /// </summary>
        public IVideoLanManager VideoLanManager
        {
            get
            {
                if (_mVlm == null)
                {
                    _mVlm = ObjectFactory.Build<IVideoLanManager>(_mHMediaLib);
                }

                return _mVlm;
            }
        } 

        /// <summary>
        /// Gets list of available audio output modules
        /// </summary>
        public IEnumerable<AudioOutputModuleInfo> AudioOutputModules
        {
            get
            {
                var pList = LibVlcMethods.libvlc_audio_output_list_get(_mHMediaLib);
                var pDevice = (LibvlcAudioOutputT)Marshal.PtrToStructure(pList, typeof(LibvlcAudioOutputT));

                do
                {
                    var info = GetDeviceInfo(pDevice);

                    yield return info;
                    if (pDevice.p_next != IntPtr.Zero)
                    {
                        pDevice = (LibvlcAudioOutputT)Marshal.PtrToStructure(pDevice.p_next, typeof(LibvlcAudioOutputT));
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);

                LibVlcMethods.libvlc_audio_output_list_release(pList);
            }
        }

        /// <summary>
        /// Gets list of available audio output devices
        /// </summary>
        public IEnumerable<AudioOutputDeviceInfo> GetAudioOutputDevices(AudioOutputModuleInfo audioOutputModule)
        {
            var i = LibVlcMethods.libvlc_audio_output_device_count(_mHMediaLib, audioOutputModule.Name.ToUtf8());
            for (var j = 0; j < i; j++)
            {
                var d = new AudioOutputDeviceInfo();
                var pName = LibVlcMethods.libvlc_audio_output_device_longname(_mHMediaLib, audioOutputModule.Name.ToUtf8(), j);
                d.Longname = Marshal.PtrToStringAnsi(pName);
                var pId = LibVlcMethods.libvlc_audio_output_device_id(_mHMediaLib, audioOutputModule.Name.ToUtf8(), j);
                d.Id = Marshal.PtrToStringAnsi(pId);

                yield return d;
            }
        }

        private AudioOutputModuleInfo GetDeviceInfo(LibvlcAudioOutputT pDevice)
        {
            return new AudioOutputModuleInfo()
            {
                Name = Marshal.PtrToStringAnsi(pDevice.psz_name),
                Description = Marshal.PtrToStringAnsi(pDevice.psz_description)
            };
        }

        private void TrySetVLCPath()
        {
            try
            {
                _mCurrentDir = Directory.GetCurrentDirectory();
                if (Environment.Is64BitProcess)
                {
                    TrySet64BitPath();
                }
                else
                {
                    TrySetVLCPath("vlc media player");
                }
            }
            catch (Exception ex)
            {
                _mLogger.Error("Failed to set VLC path: " + ex.Message);
            }
        }

        private void TrySet64BitPath()
        {
            using (var rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\VideoLAN\VLC"))
            {
                var vlcDir = rk.GetValue("InstallDir");
                if (vlcDir != null)
                {
                    Directory.SetCurrentDirectory(vlcDir.ToString());
                }
            }
        }

        private void TrySetVLCPath(string vlcRegistryKey)
        {
            using (var rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
            {
                foreach (var skName in rk.GetSubKeyNames())
                {
                    using (var sk = rk.OpenSubKey(skName))
                    {
                        var displayName = sk.GetValue("DisplayName");
                        if (displayName != null)
                        {
                            if (displayName.ToString().ToLower().IndexOf(vlcRegistryKey.ToLower()) > -1)
                            {
                                var vlcDir = sk.GetValue("InstallLocation");

                                if (vlcDir != null)
                                {
                                    Directory.SetCurrentDirectory(vlcDir.ToString());
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets error message for the last LibVLC error in the calling thread
        /// </summary>
        public string LastErrorMsg
        {
            get 
            {
                var pError = LibVlcMethods.libvlc_errmsg();
                return Marshal.PtrToStringAnsi(pError);
            }
        }
    }
}
