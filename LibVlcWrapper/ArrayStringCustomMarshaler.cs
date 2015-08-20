using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;

namespace LibVlcWrapper
{
    class ArrayStringCustomMarshaler : ICustomMarshaler
    {
        private readonly Native _mNative = new Native();
        private readonly Managed _mManaged = new Managed();

        public ArrayStringCustomMarshaler(String pstrCookie)
        {

        }

        #region ICustomMarshaler Members

        public void CleanUpManagedData(object managedObj)
        {
            this._mManaged.CleanUpManagedData(managedObj);
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            this._mNative.CleanUpNativeData(pNativeData);
        }

        public int GetNativeDataSize()
        {
            return this._mNative.GetNativeDataSize();
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            return this._mNative.MarshalManagedToNative(managedObj);
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return this._mManaged.MarshalNativeToManaged(pNativeData);
        }

        public static ICustomMarshaler GetInstance(String pstrCookie)
        {
            return new ArrayStringCustomMarshaler(pstrCookie);
        }

        #endregion

        private class Native
        {
            private readonly object _mLockNative;
            private readonly IDictionary<IntPtr, StringArraySizePair> _mNativeData = new Dictionary<IntPtr, StringArraySizePair>();
            private volatile int _mNativeDataSize = 0;

            public Native()
            {
                this._mLockNative = ((ICollection)_mNativeData).SyncRoot ?? new object();
            }

            public void CleanUpNativeData(IntPtr pNativeData)
            {
                if (pNativeData == IntPtr.Zero)
                {
                }
                else
                {
                    lock (this._mLockNative)
                    {
                        var size = this._mNativeData[pNativeData].Size;
                        this._mNativeData.Remove(pNativeData);
                        Marshal.FreeHGlobal(pNativeData);
                        this._mNativeDataSize -= size;
                    }
                }
            }

            public int GetNativeDataSize()
            {
                lock (this._mLockNative)
                {
                    return this._mNativeDataSize;
                }
            }

            public IntPtr MarshalManagedToNative(object managedObj)
            {
                var strs = managedObj as string[];
                if (managedObj != null && strs == null)
                {
                    throw new InvalidCastException("ManagedObj to string[]");
                }

                if (managedObj == null)
                {
                    return IntPtr.Zero;
                }
                else
                {
                    var bytess = new byte[strs.Length][];

                    int nativeDataSize;
                    {
                        var strsNativeDataSize = 0;

                        for (var i = 0; i < strs.Length; ++i)
                        {
                            var str = strs[i];
                            byte[] bytes;
                            if (str == null)
                            {
                                bytes = null;
                            }
                            else
                            {
                                bytes = Encoding.UTF8.GetBytes(str);
                                if (bytes == null)
                                    throw new ApplicationException("Encoding.GetBytes(String) returns null");

                                strsNativeDataSize += bytes.Length + 1;
                            }
                            bytess[i] = bytes;
                        }

                        nativeDataSize = (bytess.Length + 1) * IntPtr.Size + strsNativeDataSize;
                    }
                    var nativeData = Marshal.AllocHGlobal(nativeDataSize);
                    try
                    {
                        {
                            var strNativeData = nativeData + (bytess.Length + 1) * IntPtr.Size;
                            var strPtrNativeData = nativeData;
                            for (var i = 0;
                                 i < bytess.Length;
                                             strNativeData += bytess[i] == null ? 0 : bytess[i].Length + 1, strPtrNativeData += IntPtr.Size, ++i)
                            {
                                if (bytess[i] == null)
                                {
                                    Marshal.WriteIntPtr(strPtrNativeData, IntPtr.Zero);
                                }
                                else
                                {
                                    Marshal.Copy(bytess[i], 0, strNativeData, bytess[i].Length);
                                    Marshal.WriteByte(strNativeData, bytess[i].Length, 0);
                                    Marshal.WriteIntPtr(strPtrNativeData, strNativeData);
                                }
                            }
                            Marshal.WriteIntPtr(strPtrNativeData, IntPtr.Zero);
                        }

                        lock (this._mLockNative)
                        {
                            this._mNativeDataSize += nativeDataSize;
                            try
                            {
                                this._mNativeData.Add(nativeData, new StringArraySizePair(strs, nativeDataSize));
                            }
                            catch
                            {
                                this._mNativeDataSize -= nativeDataSize;
                                throw;
                            }
                        }
                        return nativeData;
                    }
                    catch
                    {
                        Marshal.FreeHGlobal(nativeData);
                        throw;
                    }
                }
            }

            private class StringArraySizePair : Tuple<string[], int>
            {
                public StringArraySizePair(string[] strs, int size)
                    : base(strs, size)
                {
                    if (strs == null) throw new ArgumentNullException("strs");
                }

                public string[] Strs
                {
                    get { return this.Item1; }
                }

                public int Size
                {
                    get { return this.Item2; }
                }
            }
        }

        private class Managed
        {
            private readonly object _mLockManaged;
            private readonly IDictionary<string[], IntPtr> _mManagedData = new Dictionary<string[], IntPtr>(new StringArrayComparer());

            public Managed()
            {
                this._mLockManaged = ((ICollection)_mManagedData).SyncRoot ?? new object();
            }

            public void CleanUpManagedData(object managedObj)
            {
                var strs = managedObj as string[];
                if (managedObj != null && strs == null)
                {
                    throw new InvalidCastException("ManagedObj to string[]");
                }

                if (strs == null)
                {
                }
                else
                {
                    lock (this._mLockManaged)
                    {
                        this._mManagedData.Remove(strs);
                    }
                }
            }

            public object MarshalNativeToManaged(IntPtr pNativeData)
            {
                if (pNativeData != IntPtr.Zero)
                {
                    IntPtr[] ptrs = null;
                    {
                        var size = 0;
                        var offset = 0;
                        for (; /*maxSize < 0 || size < maxSize*/; ++size, offset += IntPtr.Size)
                        {
                            var ptr = Marshal.ReadIntPtr(pNativeData, offset);
                            if (ptr == IntPtr.Zero)
                            {
                                ptrs = new IntPtr[size];
                                break;
                            }
                        }
                        if (ptrs == null)
                        {
                            throw new ArgumentException("String array exceeds maximum limit, probably function returned bad pointer.", "pNativeData");
                        }
                        else
                        {
                            Marshal.Copy(pNativeData, ptrs, 0, size);
                        }
                    }

                    var strs = new string[ptrs.Length];
                    for (var i = 0; i < ptrs.Length; ++i)
                    {
                        var ptr = ptrs[i];
                        string str;
                        if (ptr == IntPtr.Zero)
                        {
                            str = null;
                        }
                        else
                        {
                            var size = 0;
                            byte[] message = null;
                            for (; /*maxSize < 0 || size < maxSize*/; ++size)
                            {
                                var b = Marshal.ReadByte(ptr, size);
                                if (b == 0x0)
                                {
                                    message = new byte[size];
                                    break;
                                }
                            }
                            if (message == null)
                            {
                                throw new ArgumentException("Message exceeds maximum limit, probably function returned bad pointer.", "pNativeData");
                            }
                            else
                            {
                                Marshal.Copy(ptr, message, 0, size);
                                str = Encoding.UTF8.GetString(message);
                            }
                        }
                        strs[i] = str;
                    }

                    lock (this._mLockManaged)
                    {
                        this._mManagedData.Add(strs, pNativeData);
                    }

                    return strs;
                }
                else
                {
                    return null;
                }
            }

            private class StringArrayComparer : IEqualityComparer<string[]>
            {
                public bool Equals(string[] x, string[] y)
                {
                    return object.ReferenceEquals(x, y);
                }

                public int GetHashCode(string[] obj)
                {
                    if (obj == null)
                        return 0;
                    else
                        return obj.GetHashCode();
                }
            }
        }
    }
}