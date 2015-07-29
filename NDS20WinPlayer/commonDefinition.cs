using System;

namespace NDS20WinPlayer
{
    class AppInfoStrc
    {
        public static string DirOfSchedule;

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
        public string 
    }

}