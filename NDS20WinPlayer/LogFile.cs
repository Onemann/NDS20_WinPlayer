using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace NDS20WinPlayer
{
    class LogFile
    {
        public static void threadWriteLog(string strLogMsg, Enum logType)
        {
            new Thread(() => WriteLog(strLogMsg, logType)).Start();
        }

        // Log file
        private static void WriteLog(string strLogMsg, Enum logType)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath;
            logFilePath = AppInfoStrc.DirOfLog + "\\" + "Log-" + System.DateTime.Today.ToString("yyyy-MM-dd") + "." + "log";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();

            string areadyExists = "";
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
                areadyExists = ",";
            }
            log = new StreamWriter(fileStream);

            string strLogType = "";

            switch ((LogType)logType)
            {
                case LogType.LOG_FATAL:
                    strLogType = "FATAL";
                    break;
                case LogType.LOG_ERROR:
                    strLogType = "ERROR";
                    break;
                case LogType.LOG_WARN:
                    strLogType = "WARN";
                    break;
                case LogType.LOG_INFO:
                    strLogType = "INFO";
                    break;
                case LogType.LOG_DEBUG:
                    strLogType = "DEBUG";
                    break;
                case LogType.LOG_TRACE:
                    strLogType = "TRACE";
                    break;
            }
            string strLogTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            log.WriteLine(areadyExists + "{" +
                           "\"logType\":\"" + strLogType + "\"," +
                           "\"logDateTime\":\"" + strLogTime + "\"," +
                            "\"logMessage\":" + "\"" + strLogMsg + "\"" +
                           "}");
            log.Close();
        }   

    }
}
