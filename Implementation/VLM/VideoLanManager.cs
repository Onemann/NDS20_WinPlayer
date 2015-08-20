using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibVlcWrapper;
using Implementation.Exceptions;
using Implementation.Events;
using Declarations.Events;
using System.Runtime.InteropServices;
using Declarations.VLM;

namespace Implementation.VLM
{
    internal class VideoLanManager : DisposableBase, IEventProvider, IVideoLanManager
    {
        private IntPtr _mHMediaLib;

        private VlmEventManager _mEventbroker;

        public IVlmEventManager Events
        {
            get
            {
                return _mEventbroker;
            }
        }

        public VideoLanManager(IntPtr pHMediaLib)
        {
            _mHMediaLib = pHMediaLib;

            _mEventbroker = new VlmEventManager(this);
        }

        public IntPtr EventManagerHandle
        {
            get
            {
                return LibVlcMethods.libvlc_vlm_get_event_manager(_mHMediaLib);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                LibVlcMethods.libvlc_vlm_release(_mHMediaLib);
            }
            catch (Exception)
            { }
        }

        public void AddBroadcast(string name, string input, string output, IEnumerable<string> options, bool bEnabled, bool bLoop)
        {
            var optionsNumber = 0;
            string[] optionsArray = null;

            if (options != null)
            {
                optionsNumber = options.Count();
                optionsArray = options.ToArray();
            }

            if (LibVlcMethods.libvlc_vlm_add_broadcast(_mHMediaLib, name.ToUtf8(), input.ToUtf8(), output.ToUtf8(), optionsNumber, optionsArray, bEnabled == true ? 1 : 0, bLoop == true ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void AddVod(string name, string input, IEnumerable<string> options, bool bEnabled, string mux)
        {
            var optionsNumber = 0;
            string[] optionsArray = null;

            if (options != null)
            {
                optionsNumber = options.Count();
                optionsArray = options.ToArray();
            }

            if (LibVlcMethods.libvlc_vlm_add_vod(_mHMediaLib, name.ToUtf8(), input.ToUtf8(), optionsNumber, optionsArray, bEnabled == true ? 1 : 0, mux.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void DeleteMedia(string name)
        {
            if (LibVlcMethods.libvlc_vlm_del_media(_mHMediaLib, name.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetEnabled(string name, bool bEnabled)
        {
            if (LibVlcMethods.libvlc_vlm_set_enabled(_mHMediaLib, name.ToUtf8(), bEnabled == true ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetInput(string name, string input)
        {
            if (LibVlcMethods.libvlc_vlm_set_input(_mHMediaLib, name.ToUtf8(), input.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetOutput(string name, string output)
        {
            if (LibVlcMethods.libvlc_vlm_set_output(_mHMediaLib, name.ToUtf8(), output.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void AddInput(string name, string input)
        {
            if (LibVlcMethods.libvlc_vlm_add_input(_mHMediaLib, name.ToUtf8(), input.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetLoop(string name, bool bLoop)
        {
            if (LibVlcMethods.libvlc_vlm_set_loop(_mHMediaLib, name.ToUtf8(), bLoop == true ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void SetMux(string name, string mux)
        {
            if (LibVlcMethods.libvlc_vlm_set_mux(_mHMediaLib, name.ToUtf8(), mux.ToUtf8()) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void ChangeMedia(string name, string input, string output, IEnumerable<string> options, bool bEnabled, bool bLoop)
        {
            var optionsNumber = 0;
            string[] optionsArray = null;

            if (options != null)
            {
                optionsNumber = options.Count();
                optionsArray = options.ToArray();
            }

            if (LibVlcMethods.libvlc_vlm_change_media(_mHMediaLib, name.ToUtf8(), input.ToUtf8(), output.ToUtf8(), optionsNumber, optionsArray, bEnabled == true ? 1 : 0, bLoop == true ? 1 : 0) != 0)
            {
                throw new LibVlcException();
            }
        }

        public void Play(string name)
        {
            LibVlcMethods.libvlc_vlm_play_media(_mHMediaLib, name.ToUtf8());
        }

        public void Stop(string name)
        {
            LibVlcMethods.libvlc_vlm_stop_media(_mHMediaLib, name.ToUtf8());
        }

        public void Pause(string name)
        {
            LibVlcMethods.libvlc_vlm_pause_media(_mHMediaLib, name.ToUtf8());
        }

        public void Seek(string name, float percentage)
        {
            LibVlcMethods.libvlc_vlm_seek_media(_mHMediaLib, name.ToUtf8(), percentage);
        }

        public float GetMediaPosition(string name)
        {
            return LibVlcMethods.libvlc_vlm_get_media_instance_position(_mHMediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaTime(string name)
        {
            return LibVlcMethods.libvlc_vlm_get_media_instance_time(_mHMediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaLength(string name)
        {
            return LibVlcMethods.libvlc_vlm_get_media_instance_length(_mHMediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaRate(string name)
        {
            return LibVlcMethods.libvlc_vlm_get_media_instance_rate(_mHMediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaTitle(string name)
        {
            return LibVlcMethods.libvlc_vlm_get_media_instance_title(_mHMediaLib, name.ToUtf8(), 0);
        }

        public int GetMediaChapter(string name)
        {
            return LibVlcMethods.libvlc_vlm_get_media_instance_chapter(_mHMediaLib, name.ToUtf8(), 0);
        }

        public bool IsMediaSeekable(string name)
        {
            return LibVlcMethods.libvlc_vlm_get_media_instance_seekable(_mHMediaLib, name.ToUtf8(), 0) == 1;
        }
    }
}
