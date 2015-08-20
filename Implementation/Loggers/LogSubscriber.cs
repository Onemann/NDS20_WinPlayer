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
using LibVlcWrapper;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Implementation.Loggers
{
    internal unsafe sealed class LogSubscriber : DisposableBase
    {
        private IntPtr _mInstance;
        private LogCallback _mCallback;
        private ILogger _mLogger;
        private const int BufferSize = 10240;

        public LogSubscriber(ILogger logger, IntPtr pInstance)
        {
            _mInstance = pInstance;
            _mLogger = logger;
            _mCallback = OnLogCallback;
            var hCallback = Marshal.GetFunctionPointerForDelegate(_mCallback);
            LibVlcMethods.libvlc_log_set(_mInstance, hCallback, IntPtr.Zero);
        }

        private void OnLogCallback(void* data, LibvlcLogLevel level, void* ctx, char* fmt, char* args)
        {
            try
            {
                char* buffer = stackalloc char[BufferSize];
                var len = vsprintf(buffer, fmt, args);
                var msg = Marshal.PtrToStringAnsi(new IntPtr(buffer), len);

                switch (level)
                {
                    case LibvlcLogLevel.LibvlcDebug:
                        _mLogger.Debug(msg);
                        break;
                    case LibvlcLogLevel.LibvlcNotice:
                        _mLogger.Info(msg);
                        break;
                    case LibvlcLogLevel.LibvlcWarning:
                        _mLogger.Warning(msg);
                        break;
                    case LibvlcLogLevel.LibvlcError:
                    default:
                        _mLogger.Error(msg);
                        break;
                }
            }
            catch (Exception ex)
            {
                _mLogger.Error("Failed to handle log callback, reason : " + ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                LibVlcMethods.libvlc_log_unset(_mInstance);
            }
            catch (Exception)
            { }
                      
            if (disposing)
            {
                _mCallback = null;
            }
        }

        [DllImport("msvcrt", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private static extern int vsprintf(char* str, char* format, char* arg);
    }
}
