﻿//    nVLC
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
using Implementation.Utils;
using System.Runtime.InteropServices;

namespace Implementation
{
    internal unsafe struct PixelData : IDisposable
    {
        public byte* PPixelData;
        public int Size;

        public PixelData(int size)
        {
            this.Size = size;
            this.PPixelData = (byte*)MemoryHeap.Alloc(size);
        }

        #region IDisposable Members

        public void Dispose()
        {
            MemoryHeap.Free(this.PPixelData);
        }

        #endregion

        public static bool operator ==(PixelData pd1, PixelData pd2)
        {
            return (pd1.Size == pd2.Size && pd1.PPixelData == pd2.PPixelData);
        }

        public static bool operator !=(PixelData pd1, PixelData pd2)
        {
            return !(pd1 == pd2);
        }

        public override int GetHashCode()
        {
            return Size.GetHashCode() ^ PPixelData->GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var pd = (PixelData)obj;
            if (pd == null)
            {
                return false;
            }

            return this == pd;
        }
    }
}
