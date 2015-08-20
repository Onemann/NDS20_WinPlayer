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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using Declarations;
using Implementation.Utils;
using LibVlcWrapper;

namespace Implementation
{
    internal sealed unsafe class MemoryRendererEx : DisposableBase, IMemoryRendererEx
    {
        IntPtr _mHMediaPlayer;
        NewFrameDataEventHandler _mCallback = null;
        Timer _mTimer = new Timer();
        volatile int _mFrameRate = 0;
        int _mLatestFps;
        object _mLock = new object();
        List<Delegate> _mCallbacks = new List<Delegate>();
        Func<BitmapFormat, BitmapFormat> _mFormatSetupCb = null;
        IntPtr[] _mPlanes = new IntPtr[3];
        BitmapFormat _mFormat;
        Action<Exception> _mExcHandler = null;
        IntPtr _pLockCallback;
        IntPtr _pDisplayCallback;
        IntPtr _pFormatCallback;
        
        PlanarPixelData _mPixelData = default(PlanarPixelData);

        public MemoryRendererEx(IntPtr hMediaPlayer)
        {
            _mHMediaPlayer = hMediaPlayer;

            LockEventHandler leh = OnpLock;
            DisplayEventHandler deh = OnpDisplay;
            VideoFormatCallback formatCallback = OnFormatCallback;
            
            _pFormatCallback = Marshal.GetFunctionPointerForDelegate(formatCallback);
            _pLockCallback = Marshal.GetFunctionPointerForDelegate(leh);
            _pDisplayCallback = Marshal.GetFunctionPointerForDelegate(deh);

            _mCallbacks.Add(leh);
            _mCallbacks.Add(deh);
            _mCallbacks.Add(formatCallback);
           
            _mTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _mTimer.Interval = 1000;
            Sar = AspectRatio.Default;
            LibVlcMethods.libvlc_video_set_format_callbacks(_mHMediaPlayer, _pFormatCallback, IntPtr.Zero);        
            LibVlcMethods.libvlc_video_set_callbacks(_mHMediaPlayer, _pLockCallback, IntPtr.Zero, _pDisplayCallback, IntPtr.Zero);
        }

        private unsafe int OnFormatCallback(void** opaque, char* chroma, int* width, int* height, int* pitches, int* lines)
        {
            var pChroma = new IntPtr(chroma);
            var chromaStr = Marshal.PtrToStringAnsi(pChroma);

            ChromaType type;
            if (!Enum.TryParse<ChromaType>(chromaStr, out type))
            {
                var exc = new ArgumentException("Unsupported chroma type " + chromaStr);
                if (_mExcHandler != null)
                {
                    _mExcHandler(exc);
                    return 0;
                }
                else
                {
                    throw exc;
                }
            }

            _mFormat = new BitmapFormat(*width, *height, type);
            if (_mFormatSetupCb != null)
            {
                _mFormat = _mFormatSetupCb(_mFormat);
            }

            Marshal.Copy(_mFormat.Chroma.ToUtf8(), 0, pChroma, 4);
            *width = _mFormat.Width;
            *height = _mFormat.Height;

            for (var i = 0; i < _mFormat.Planes; i++)
            {
                pitches[i] = _mFormat.Pitches[i];
                lines[i] = _mFormat.Lines[i];
            }

            _mPixelData = new PlanarPixelData(_mFormat.PlaneSizes);

            return _mFormat.Planes;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _mLatestFps = _mFrameRate;
            _mFrameRate = 0;
        }

        unsafe void* OnpLock(void* opaque, void** plane)
        {
            for (var i = 0; i < _mPixelData.Sizes.Length; i++)
            {
                plane[i] = _mPixelData.Data[i];
            }

            return null;
        }

        unsafe void OnpDisplay(void* opaque, void* picture)
        {
            lock (_mLock)
            {
                try
                {
                    _mFrameRate++;
                    for (var i = 0; i < _mPixelData.Sizes.Length; i++)
                    {
                        _mPlanes[i] = new IntPtr(_mPixelData.Data[i]);
                    }

                    if (_mCallback != null)
                    {
                        var pf = GetFrame();
                        _mCallback(pf);
                    }
                }
                catch (Exception ex)
                {
                    if (_mExcHandler != null)
                    {
                        _mExcHandler(ex);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }

        internal void StartTimer()
        {
            _mTimer.Start();
        }

        private PlanarFrame GetFrame()
        {
            return new PlanarFrame(_mPlanes, _mFormat.PlaneSizes);
        }

        #region IMemoryRendererEx Members

        public void SetCallback(NewFrameDataEventHandler callback)
        {
            _mCallback = callback;
        }

        public PlanarFrame CurrentFrame
        {
            get
            {
                lock (_mLock)
                {
                    return GetFrame();
                }
            }
        }

        public void SetFormatSetupCallback(Func<BitmapFormat, BitmapFormat> setupCallback)
        {
            _mFormatSetupCb = setupCallback;
        }

        public int ActualFrameRate
        {
            get
            {
                return _mLatestFps;
            }
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _mExcHandler = handler;
        }

        public AspectRatio Sar { get; set; }

        #endregion

        protected override void Dispose(bool disposing)
        {
            var zero = IntPtr.Zero;
            LibVlcMethods.libvlc_video_set_callbacks(_mHMediaPlayer, zero, zero, zero, zero);

            if (_mPixelData != default(PlanarPixelData))
            {
                _mPixelData.Dispose();
            }

            if (disposing)
            {
                _mTimer.Dispose();
                _mFormatSetupCb = null;
                _mExcHandler = null;
                _mCallback = null;
                _mCallbacks.Clear();
            }         
        }
    }
}
