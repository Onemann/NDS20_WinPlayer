using System;

namespace NDS20WinPlayer
{
    class AppInfoStrc
    {
        public static string DirOfApplication;
        public static string DirOfSchedule;
        public static string DirOfLog;

    }

    enum LogType
    {
        LOG_FATAL,
        LOG_ERROR,
        LOG_WARN,
        LOG_INFO,
        LOG_DEBUG,
        LOG_TRACE
    }
    public struct frameInfoStrc
    {
        public int xPos;
        public int yPos;
        public int width;
        public int height;
        public string contentsFileName;
        public bool mute; //volumeS

    }

    public class scheduleclass
    {
        public string tlclScheduleField { get; set; }
        public string tlclTypeField { get; set; }
        public DateTime tlclStartDateField { get; set; }
        public DateTime tlclEndDateField { get; set; }
        //added
    }

    public class clssScheduleFileList
    {
        public string scheCatagory { get; set; }
        public string scheType { get; set; }
        public string ctscKey { get; set; }
        public string ctscName { get; set; }
        public string ctscStartdate { get; set; }
        public string ctscEnddate { get; set; }
    }

    //Log file class for log grid view with JSON
    public class clssLogFileList
    {
        public string logFileName { get; set; }
    }

    public class clssLogList
    {
        public string logType { get; set; }
        public string logDateTime { get; set; }
        public string logMessage { get; set; }
    }

}