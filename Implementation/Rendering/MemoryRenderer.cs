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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using Declarations;
using Implementation.Utils;
using LibVlcWrapper;

namespace Implementation
{
    internal sealed unsafe class MemoryRenderer : DisposableBase, IMemoryRenderer
    {
        IntPtr _mHMediaPlayer;
        NewFrameEventHandler _mCallback = null;
        BitmapFormat _mFormat;
        Timer _mTimer = new Timer();
        volatile int _mFrameRate = 0;
        int _mLatestFps;
        object _mLock = new object();
        List<Delegate> _mCallbacks = new List<Delegate>();

        IntPtr _pLockCallback;
        IntPtr _pUnlockCallback;
        IntPtr _pDisplayCallback;
        Action<Exception> _mExcHandler = null;
        GCHandle _mPixelDataPtr = default(GCHandle);
        PixelData _mPixelData;
        void* _mPBuffer = null;

        public MemoryRenderer(IntPtr hMediaPlayer)
        {
            _mHMediaPlayer = hMediaPlayer;

            LockEventHandler leh = OnpLock;
            UnlockEventHandler ueh = OnpUnlock;
            DisplayEventHandler deh = OnpDisplay;

            _pLockCallback = Marshal.GetFunctionPointerForDelegate(leh);
            _pUnlockCallback = Marshal.GetFunctionPointerForDelegate(ueh);
            _pDisplayCallback = Marshal.GetFunctionPointerForDelegate(deh);

            _mCallbacks.Add(leh);
            _mCallbacks.Add(deh);
            _mCallbacks.Add(ueh);

            _mTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _mTimer.Interval = 1000;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _mLatestFps = _mFrameRate;
            _mFrameRate = 0;
        }

        unsafe void* OnpLock(void* opaque, void** plane)
        {
            var px = (PixelData*)opaque;
            *plane = px->PPixelData;
            return null;
        }

        unsafe void OnpUnlock(void* opaque, void* picture, void** plane)
        {

        }

        unsafe void OnpDisplay(void* opaque, void* picture)
        {
            lock (_mLock)
            {
                try
                {
                    var px = (PixelData*)opaque;
                    MemoryHeap.CopyMemory(_mPBuffer, px->PPixelData, px->Size);

                    _mFrameRate++;
                    if (_mCallback != null)
                    {
                        using (var frame = GetBitmap())
                        {
                            _mCallback(frame);
                        }
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

        private Bitmap GetBitmap()
        {
            return new Bitmap(_mFormat.Width, _mFormat.Height, _mFormat.Pitch, _mFormat.PixelFormat, new IntPtr(_mPBuffer));
        }

        #region IMemoryRenderer Members

        public void SetCallback(NewFrameEventHandler callback)
        {
            _mCallback = callback;
        }

        public void SetFormat(BitmapFormat format)
        {
            _mFormat = format;

            LibVlcMethods.libvlc_video_set_format(_mHMediaPlayer, _mFormat.Chroma.ToUtf8(), _mFormat.Width, _mFormat.Height, _mFormat.Pitch);
            _mPBuffer = MemoryHeap.Alloc(_mFormat.ImageSize);

            _mPixelData = new PixelData(_mFormat.ImageSize);
            _mPixelDataPtr = GCHandle.Alloc(_mPixelData, GCHandleType.Pinned);
            LibVlcMethods.libvlc_video_set_callbacks(_mHMediaPlayer, _pLockCallback, _pUnlockCallback, _pDisplayCallback, _mPixelDataPtr.AddrOfPinnedObject());
        }

        internal void StartTimer()
        {
            _mTimer.Start();
        }

        public int ActualFrameRate
        {
            get
            {
                return _mLatestFps;
            }
        }

        public Bitmap CurrentFrame
        {
            get
            {
                lock (_mLock)
                {
                    return GetBitmap();
                }
            }
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _mExcHandler = handler;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            var zero = IntPtr.Zero;
            LibVlcMethods.libvlc_video_set_callbacks(_mHMediaPlayer, zero, zero, zero, zero);

            _mPixelDataPtr.Free();
            _mPixelData.Dispose();

            MemoryHeap.Free(_mPBuffer);

            if (disposing)
            {
                _mTimer.Dispose();
                _mCallback = null;
                _mCallbacks.Clear();
            }
        }
    }
}
