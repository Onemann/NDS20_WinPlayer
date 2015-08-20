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
using Declarations.Media;
using Implementation.Media;
using Implementation.Utils;
using LibVlcWrapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace Implementation
{
    internal sealed unsafe class MemoryInputMedia : BasicMedia, IMemoryInputMedia
    {
        IntPtr _mPLock, _mPUnlock;
        List<Delegate> _mCallbacks = new List<Delegate>();
        StreamInfo _mStreamInfo;
        BlockingCollection<FrameData> _mQueue;
        Action<Exception> _mExcHandler;
        bool _mInitilaized;
        
        public MemoryInputMedia(IntPtr hMediaLib)
            : base(hMediaLib)
        {           
            ImemGet pLock = OnImemGet;
            ImemRelease pUnlock = OnImemRelease;

            _mPLock = Marshal.GetFunctionPointerForDelegate(pLock);
            _mPUnlock = Marshal.GetFunctionPointerForDelegate(pUnlock);

            _mCallbacks.Add(pLock);
            _mCallbacks.Add(pUnlock);
        }

        public void Initialize(StreamInfo streamInfo, int maxItemsInQueue)
        {
            if (streamInfo == null)
            {
                throw new ArgumentNullException("streamInfo");
            }

            _mStreamInfo = streamInfo;
            AddOptions(MediaOptions.ToList());
            _mQueue = new BlockingCollection<FrameData>(maxItemsInQueue);
            _mInitilaized = true;
        }

        public void AddFrame(FrameData frameData)
        {
            if (!_mInitilaized)
            {
                throw new InvalidOperationException("The instance must be initialized first. Call Initialize method before adding frames");
            }

            if (frameData.Data == IntPtr.Zero)
            {
                throw new ArgumentNullException("frameData.Data");
            }

            if (frameData.DataSize == 0)
            {
                throw new ArgumentException("DataSize value must be greater than zero", "frameData.DataSize");
            }

            if (frameData.Pts < 0)
            {
                throw new ArgumentException("Pts value must be greater than zero", "frameData.PTS");
            }

            _mQueue.Add(DeepClone(frameData));
        }

        public void AddFrame(byte[] data, long pts, long dts)
        {
            if (!_mInitilaized)
            {
                throw new InvalidOperationException("The instance must be initialized first. Call Initialize method before adding frames");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("data buffer size must be greater than zero", "data");
            }

            if (pts <= 0)
            {
                throw new ArgumentException("Pts value must be greater than zero", "pts");
            }

            var frame = DeepClone(data);
            frame.Pts = pts;
            frame.Dts = dts;
            _mQueue.Add(frame);
        }

        public void AddFrame(Bitmap bitmap, long pts, long dts)
        {
            if (!_mInitilaized)
            {
                throw new InvalidOperationException("The instance must be initialized first. Call Initialize method before adding frames");
            }

            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            if (pts < 0)
            {
                throw new ArgumentException("Pts value must be greater than zero", "pts");
            }

            if (bitmap.PixelFormat != PixelFormat.Format24bppRgb &&
                bitmap.PixelFormat != PixelFormat.Format32bppRgb)
            {
                throw new ArgumentException("Supported pixel formats for bitmaps are Format24bppRgb and Format32bppRgb", "bitmap.PixelFormat");
            }

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var frame = DeepClone(bmpData.Scan0, bmpData.Stride * bmpData.Height);
            bitmap.UnlockBits(bmpData);
            frame.Pts = pts;
            frame.Dts = dts;
            _mQueue.Add(frame);
        }

        private FrameData DeepClone(byte[] buffer)
        {
            var clone = new FrameData();
            clone.Data = new IntPtr(MemoryHeap.Alloc(buffer.Length));
            Marshal.Copy(buffer, 0, clone.Data, buffer.Length);
            clone.DataSize = buffer.Length;
            return clone;
        }

        private FrameData DeepClone(FrameData frameData)
        {
            var clone = DeepClone(frameData.Data, frameData.DataSize);
            clone.Dts = frameData.Dts;
            clone.Pts = frameData.Pts;
            return clone;
        }

        private FrameData DeepClone(IntPtr data, int size)
        {
            var clone = new FrameData();
            clone.Data = new IntPtr(MemoryHeap.Alloc(size));
            MemoryHeap.CopyMemory(clone.Data.ToPointer(), data.ToPointer(), size);
            clone.DataSize = size;
            return clone;
        }

        private int OnImemGet(void* data, char* cookie, long* dts, long* pts, int* flags, uint* dataSize, void** ppData)
        {
            try
            {
                var fdata = _mQueue.Take();
                *ppData = fdata.Data.ToPointer();
                *dataSize = (uint)fdata.DataSize;
                *pts = fdata.Pts;
                *dts = fdata.Dts;
                *flags = 0;
                return 0;
            }
            catch (Exception ex)
            {
                if (_mExcHandler != null)
                {
                    _mExcHandler(ex);
                }
                else
                {
                    throw new Exception("imem-get callback failed", ex);
                }
                return 1;
            }           
        }

        private void OnImemRelease(void* data, char* cookie, uint dataSize, void* pData)
        {
            try
            {
                MemoryHeap.Free(pData);
            }
            catch (Exception ex)
            {
                if (_mExcHandler != null)
                {
                    _mExcHandler(ex);
                }
                else
                {
                    throw new Exception("imem-release callback failed", ex);
                }
            }
        }

        private IEnumerable<string> MediaOptions
        {
            get
            {
                yield return string.Format(":imem-get={0}", _mPLock.ToInt64());
                yield return string.Format(":imem-release={0}", _mPUnlock.ToInt64());
                yield return string.Format(":imem-codec={0}", EnumUtils.GetEnumDescription(_mStreamInfo.Codec));
                yield return string.Format(":imem-cat={0}", (int)_mStreamInfo.Category);
                yield return string.Format(":imem-id={0}", _mStreamInfo.Id);
                yield return string.Format(":imem-group={0}", _mStreamInfo.Group);
                yield return string.Format(":imem-fps={0}", _mStreamInfo.Fps);
                yield return string.Format(":imem-width={0}", _mStreamInfo.Width);
                yield return string.Format(":imem-height={0}", _mStreamInfo.Height);
                yield return string.Format(":imem-size={0}", _mStreamInfo.Size);
                yield return string.Format(":imem-channels={0}", _mStreamInfo.Channels);
                yield return string.Format(":imem-samplerate={0}", _mStreamInfo.Samplerate);
                yield return string.Format(":imem-dar={0}", EnumUtils.GetEnumDescription(_mStreamInfo.AspectRatio));
                yield return string.Format(":imem-cookie=3");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _mCallbacks = null;
                if (_mQueue.Count > 0)
                {
                    foreach (var item in _mQueue)
                    {
                        MemoryHeap.Free(item.Data.ToPointer());
                    }
                }
                _mQueue = null;
            }
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _mExcHandler = handler;
        }

        public int PendingFramesCount
        {
            get 
            {
                if (_mQueue == null)
                {
                    return 0;
                }

                return _mQueue.Count;
            }
        }
    }
}
