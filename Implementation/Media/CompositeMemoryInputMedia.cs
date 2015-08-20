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
using Implementation.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Implementation.Media
{
    internal sealed unsafe class CompositeMemoryInputMedia : BasicMedia, ICompositeMemoryInputMedia
    {
        Dictionary<int, StreamData> _mStreamData = new Dictionary<int, StreamData>();
        Action<Exception> _mExcHandler;
        bool _mIsComplete = false;

        public CompositeMemoryInputMedia(IntPtr hMediaLib)
            : base(hMediaLib)
        {

        }

        public void StreamAddingComplete()
        {
            _mIsComplete = true;
        }

        public void AddStream(StreamInfo streamInfo, int maxItemsInQueue = 30)
        {
            if (_mIsComplete)
            {
                throw new InvalidOperationException("Stream adding is complete. No more streams allowed");
            }

            _mStreamData[streamInfo.Id] = new StreamData(streamInfo, maxItemsInQueue);
        }

        public void AddFrame(int streamId, FrameData frame)
        {
            var clone = DeepClone(frame);
            _mStreamData[streamId].Queue.Add(clone);
        }

        public void AddFrame(int streamId, byte[] data, long pts, long dts = -1)
        {
            var clone = DeepClone(data);
            clone.Pts = pts;
            clone.Dts = dts;
            _mStreamData[streamId].Queue.Add(clone);
        }

        public void AddFrame(int streamId, Bitmap bitmap, long pts, long dts = -1)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var frame = DeepClone(bmpData.Scan0, bmpData.Stride * bmpData.Height);
            bitmap.UnlockBits(bmpData);
            frame.Pts = pts;
            frame.Dts = dts;
            _mStreamData[streamId].Queue.Add(frame);
        }

        public void AddFrame(int streamId, Sound sound, long dts = -1)
        {
            var clone = DeepClone(sound);
            clone.Dts = dts;
            _mStreamData[streamId].Queue.Add(clone);
        }

        public void SetExceptionHandler(Action<Exception> handler)
        {
            _mExcHandler = handler;
        }

        public int GetPendingFramesCount(int streamId)
        {
            return _mStreamData[streamId].Queue.Count;
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

        private FrameData DeepClone(Sound sound)
        {
            var clone = DeepClone(sound.SamplesData, (int)sound.SamplesSize);
            clone.Pts = sound.Pts;
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

        private class StreamData
        {
            public BlockingCollection<FrameData> Queue;
            public StreamInfo StreamInfo;

            public StreamData(StreamInfo streamInfo, int maxQueueSize)
            {
                StreamInfo = streamInfo;
                Queue = new BlockingCollection<FrameData>(maxQueueSize);
            }
        }

        private int OnImemGet(void* data, char* cookie, long* dts, long* pts, int* flags, uint* dataSize, void** ppData)
        {
            try
            {
                var fdata = GetNextFrameData(cookie);
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

        private FrameData GetNextFrameData(char* cookie)
        {
            var index = (int)*cookie;
            return _mStreamData[index].Queue.Take();
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
    }
}
