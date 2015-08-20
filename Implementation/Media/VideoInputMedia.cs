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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Declarations;
using Declarations.Media;
using Implementation.Utils;
using LibVlcWrapper;

namespace Implementation.Media
{
    [MaxLibVlcVersion("1.1.50", "invmem (fake input)")]
    internal sealed unsafe class VideoInputMedia : BasicMedia, IVideoInputMedia
    {
        BitmapFormat _mFormat;
        PixelData _mData = default(PixelData);
        object _mLock = new object();
        IntPtr _mPLock, _mPUnlock;
        GCHandle _mPData;
        List<Delegate> _mCallbacks = new List<Delegate>();

        public VideoInputMedia(IntPtr hMediaLib)
            : base(hMediaLib)
        {
            CallbackEventHandler pLock = LockCallback;
            CallbackEventHandler pUnlock = UnlockCallback;

            _mPLock = Marshal.GetFunctionPointerForDelegate(pLock);
            _mPUnlock = Marshal.GetFunctionPointerForDelegate(pUnlock);

            _mCallbacks.Add(pLock);
            _mCallbacks.Add(pUnlock);
        }

        #region IVideoInputMedia Members

        public void AddFrame(Bitmap frame)
        {
            Monitor.Enter(_mLock);

            try
            {
                var rect = new Rectangle(0, 0, frame.Width, frame.Height);
                var bmpData = frame.LockBits(rect, ImageLockMode.ReadOnly, frame.PixelFormat);

                var pData = bmpData.Scan0.ToPointer();
                MemoryHeap.CopyMemory(_mData.PPixelData, pData, _mData.Size);

                frame.UnlockBits(bmpData);
            }
            finally
            {
                Monitor.Exit(_mLock);
            }
        }

        public void SetFormat(BitmapFormat format)
        {
            if (_mData == default(PixelData))
            {
                _mFormat = format;
                _mData = new PixelData(_mFormat.ImageSize);
                _mPData = GCHandle.Alloc(_mData, GCHandleType.Pinned);
                InitMedia();
            }
            else
            {
                throw new InvalidOperationException("Bitmap format already set");
            }
        }

        #endregion

        private void InitMedia()
        {
            var options = new List<string>()
            {
               ":codec=invmem",
               string.Format(":invmem-width={0}", _mFormat.Width),
               string.Format(":invmem-height={0}", _mFormat.Height),
               string.Format(":invmem-lock={0}", _mPLock.ToInt64()),
               string.Format(":invmem-unlock={0}", _mPUnlock.ToInt64()),
               string.Format(":invmem-chroma={0}", _mFormat.Chroma),
               string.Format(":invmem-data={0}", _mPData.AddrOfPinnedObject().ToInt64())
            };

            AddOptions(options);
        }

        void* LockCallback(void* data)
        {
            Monitor.Enter(_mLock);
            var pd = (PixelData*)data;
            return pd->PPixelData;
        }

        void* UnlockCallback(void* data)
        {
            Monitor.Exit(_mLock);
            var pd = (PixelData*)data;
            return pd->PPixelData;
        }

        protected override void Dispose(bool disposing)
        {
            _mData.Dispose();
            _mPData.Free();

            if (disposing)
            {
                _mCallbacks = null;
            }

            base.Dispose(disposing);
        }
    }
}
