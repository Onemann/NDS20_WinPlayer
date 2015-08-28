using System;
using System.Windows.Forms;
using System.IO;
using System.Net.Json;
using Bauglir.Ex;
using System.Management;
using System.Runtime.InteropServices;

namespace NDS20WinPlayer
{
    public static class MemoryHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct MEMORYSTATUSEX
        {
            internal uint dwLength;
            internal uint dwMemoryLoad;
            internal ulong ullTotalPhys;
            internal ulong ullAvailPhys;
            internal ulong ullTotalPageFile;
            internal ulong ullAvailPageFile;
            internal ulong ullTotalVirtual;
            internal ulong ullAvailVirtual;
            internal ulong ullAvailExtendedVirtual;
        }
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        public static double GetGlobalMemoryStatusEX()
        {
            MEMORYSTATUSEX statEX = new MEMORYSTATUSEX();
            statEX.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            GlobalMemoryStatusEx(ref statEX);

            return (double)statEX.ullTotalPhys;

        }
    }

    class CommonFunctions
    {
        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }
            return null;
        }

        public static void LoadIniFile()
        {
            var appIniFile = new IniFile();

            #region Dir Path
            AppInfoStrc.DirOfApplication = Environment.CurrentDirectory;

            AppInfoStrc.DirOfSchedule = appIniFile.Read("DirOfSchedule", "PATH");
            if (AppInfoStrc.DirOfSchedule == "")
            {
                appIniFile.Write("DirOfSchedule", InitValue.InitDirOfSchedule, "PATH");
                AppInfoStrc.DirOfSchedule = InitValue.InitDirOfSchedule;
            }

            AppInfoStrc.DirOfLog = appIniFile.Read("DirOfLog", "PATH");
            if (AppInfoStrc.DirOfLog == "")
            {
                appIniFile.Write("DirOfLog", InitValue.InitDirOfLog, "PATH");
                AppInfoStrc.DirOfLog = InitValue.InitDirOfLog;
            }

            AppInfoStrc.DirOfContents = appIniFile.Read("DirOfContents", "PATH");
            if (AppInfoStrc.DirOfContents == "")
            {
                appIniFile.Write("DirOfContents", InitValue.DirOfContents, "PATH");
                AppInfoStrc.DirOfContents = InitValue.DirOfContents;
            }
            #endregion

            #region Server connectoion Info
            AppInfoStrc.UrlOfServer = appIniFile.Read("UrlOfServer", "SERVER");
            if (AppInfoStrc.UrlOfServer == "")
            {
                appIniFile.Write("UrlOfServer", InitValue.InitUrlOfServer, "SERVER");
                AppInfoStrc.UrlOfServer = InitValue.InitUrlOfServer;
            }

            AppInfoStrc.ExtentionOfServer = appIniFile.Read("ExtentionOfServer", "SERVER");
            if (AppInfoStrc.ExtentionOfServer == "")
            {
                appIniFile.Write("ExtentionOfServer", InitValue.InitExtensionOfServer, "SERVER");
                AppInfoStrc.ExtentionOfServer = InitValue.InitExtensionOfServer;
            }

            AppInfoStrc.PortOfServer = appIniFile.Read("PortOfServer", "SERVER");
            if (AppInfoStrc.PortOfServer == "")
            {
                appIniFile.Write("PortOfServer", InitValue.InitPortOfServer, "SERVER");
                AppInfoStrc.PortOfServer = InitValue.InitPortOfServer;
            }
            #endregion

            #region Player Info
            AppInfoStrc.PlayerId = appIniFile.Read("PlayerID", "PLAYER");
            if (AppInfoStrc.PlayerId == "")
            {
                appIniFile.Write("PlayerID", "", "PLAYER");
            }
            #endregion

        }

        public static double ConvertToUnixTimestamp(DateTime value)
        {
            var span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return span.TotalSeconds;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            //dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            dtDateTime = dtDateTime.AddSeconds(Math.Round(unixTimeStamp/1000)).ToLocalTime();
            return dtDateTime;
        }

#region JSON
        public static  JsonObject StringToJsonObject(string jsonText) // string to JSON object
        {
            try
            {
                JsonTextParser parser = new JsonTextParser();
                return parser.Parse(jsonText);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object GetJsonColValue(JsonObject jsonOject, string colName )
        {
            try
            {
                var col = (JsonObjectCollection) jsonOject;
                return col[colName].GetValue();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string getTextHandlerIdFromJsonText(string jsonText)
        {
            var jsonObj = StringToJsonObject(jsonText);
            if (jsonObj == null) return string.Empty;
            var jsonColValue = GetJsonColValue(jsonObj, JsonColName.JsonCmd);
            if (jsonColValue == null) return string.Empty;
            if ((string)jsonColValue != JsonCmd.ServerConnected) return string.Empty;
            return (string)CommonFunctions.GetJsonColValue(jsonObj, JsonColName.JsonTxtHndId);
        }

        
#endregion

        public class NDSWebSocketClientConnection : WebSocketClientConnection
        {

            string fCachedString = String.Empty;
            MemoryStream fCachedBinary = new MemoryStream();

            /// <summary>
            /// Basic connection ping and pong event
            /// </summary>
            /// <param name="aConnection">connection instance</param>
            /// <param name="aData">ping or pond data</param>
            public delegate void ConnectionPingPongEvent(WebSocketConnection aConnection, string aData);

            /// <summary>
            /// Basic connection framed text
            /// </summary>
            /// <param name="aConnection">connection instance</param>
            /// <param name="aData">text</param>
            public delegate void ConnectionFramedTextEvent(WebSocketConnection aConnection, string aData);

            /// <summary>
            /// Basic connection framed binary
            /// </summary>
            /// <param name="aConnection">connection instance</param>
            /// <param name="aData">minary stream</param>
            public delegate void ConnectionFramedBinaryEvent(WebSocketConnection aConnection, MemoryStream aData);



            /// <summary>
            /// connection ping event
            /// </summary>
            public event ConnectionPingPongEvent ConnectionPing;

            /// <summary>
            /// connection pong event
            /// </summary>
            public event ConnectionPingPongEvent ConnectionPong;

            /// <summary>
            /// connection framed text fully received
            /// </summary>
            public event ConnectionFramedTextEvent ConnectionFramedText;

            /// <summary>
            /// connection framed binary fully received
            /// </summary>
            public event ConnectionFramedBinaryEvent ConnectionFramedBinary;


            protected override void ProcessPing(string aData)
            {
                if (ConnectionPing != null) ConnectionPing(this, aData);
            }

            protected override void ProcessPong(string aData)
            {
                if (ConnectionPong != null) ConnectionPong(this, aData);
            }

            protected override void ProcessStream(bool aReadFinal, bool aRes1, bool aRes2, bool aRes3, MemoryStream aStream)
            {
                fCachedBinary.Write(aStream.ToArray(), 0, (int)aStream.Length);
            }

            protected override void ProcessStreamContinuation(bool aReadFinal, bool aRes1, bool aRes2, bool aRes3, MemoryStream aStream)
            {
                fCachedBinary.Write(aStream.ToArray(), 0, (int)aStream.Length);
                if (aReadFinal)
                {
                    if (ConnectionFramedBinary != null)
                    {
                        ConnectionFramedBinary(this, fCachedBinary);
                    }
                    fCachedBinary.SetLength(0);
                }
            }

            protected override void ProcessText(bool aReadFinal, bool aRes1, bool aRes2, bool aRes3, string aString)
            {
                fCachedString = aString;
            }

            protected override void ProcessTextContinuation(bool aReadFinal, bool aRes1, bool aRes2, bool aRes3, string aString)
            {
                fCachedString += aString;
                if (aReadFinal)
                {
                    if (ConnectionFramedText != null)
                    {
                        ConnectionFramedText(this, fCachedString);
                    }
                    fCachedString = String.Empty;
                }
            }

        }


    }

}