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
using System.Text;
using System.Threading;
using Declarations;
using LibVlcWrapper;

namespace Implementation
{
    internal class Log : DisposableBase
    {
        Thread _mReader;
        IntPtr _mHLog = IntPtr.Zero;
        volatile bool _doRun;
        ILogger _mLogger;
        bool _mEnabled;
        LogIterator _mLogIterator;

        public Log(IntPtr hLib, ILogger logger)
        {           
            _mLogger = logger;

            LibVlcMethods.libvlc_set_log_verbosity(hLib, 2);
            _mHLog = LibVlcMethods.libvlc_log_open(hLib);
            _mLogIterator = new LogIterator(_mHLog);
            _mReader = new Thread(Retreive);
            _mReader.IsBackground = true;
            _mReader.Name = "Log Thread";

            WriteTimeout = 500;
        }

        public int WriteTimeout { get; set; }

        private void Retreive()
        {
            while (_doRun)
            {
                foreach (var item in _mLogIterator)
                {
                    switch (item.Severity)
                    {
                        case LibvlcLogMessateTSeverity.Info:
                            _mLogger.Info(item.Message);
                            break;

                        case LibvlcLogMessateTSeverity.Warn:
                            _mLogger.Warning(item.Message);
                            break;

                        case LibvlcLogMessateTSeverity.Dbg:
                            _mLogger.Debug(item.Message);
                            break;

                        case LibvlcLogMessateTSeverity.Err:

                        default:
                            _mLogger.Error(item.Message);
                            break;
                    }
                }

                Thread.Sleep(WriteTimeout);
            }
        }

        private void Start()
        {
            _doRun = true;
            _mReader.Start();
        }

        private void Stop()
        {
            _doRun = false;
        }

        public bool Enabled
        {
            get
            {
                return _mEnabled;
            }
            set
            {
                if (_mEnabled == value)
                {
                    return;
                }

                _mEnabled = value;
                if (_mEnabled)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            LibVlcMethods.libvlc_log_close(_mHLog);
        }

        private class LogIterator : IEnumerable<LogMessage>
        {
            IntPtr _mHLog;

            internal LogIterator(IntPtr hLog)
            {
                _mHLog = hLog;
            }

            #region IEnumerable<string> Members

            public IEnumerator<LogMessage> GetEnumerator()
            {
                var i = LibVlcMethods.libvlc_log_get_iterator(_mHLog);

                while (LibVlcMethods.libvlc_log_iterator_has_next(i))
                {
                    var msg = new LibvlcLogMessageT();
                    msg.sizeof_msg = (uint)Marshal.SizeOf(msg);
                    LibVlcMethods.libvlc_log_iterator_next(i, ref msg);

                    yield return GetMessage(msg);
                }

                LibVlcMethods.libvlc_log_iterator_free(i);
                //LibVlcMethods.libvlc_log_clear(m_hLog);
            }

            private LogMessage GetMessage(LibvlcLogMessageT msg)
            {
                var sb = new StringBuilder();
                sb.AppendFormat("{0} ", Marshal.PtrToStringAnsi(msg.psz_header));
                sb.AppendFormat("{0} ", Marshal.PtrToStringAnsi(msg.psz_message));
                sb.AppendFormat("{0} ", Marshal.PtrToStringAnsi(msg.psz_name));
                sb.Append(Marshal.PtrToStringAnsi(msg.psz_type));

                return new LogMessage() { Message = sb.ToString(), Severity = (LibvlcLogMessateTSeverity)msg.i_severity };
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion
        }

        private struct LogMessage
        {
            public LibvlcLogMessateTSeverity Severity;
            public string Message;
        }
    }
}
