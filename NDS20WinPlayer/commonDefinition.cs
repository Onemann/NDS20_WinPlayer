using System;

namespace NDS20WinPlayer
{
    public struct frameInfoStrc
    {
        public int xPos;
        public int yPos;
        public int width;
        public int height;
        public string contentsFileName;
        public bool mute;

    }

    public class scheduleclass
    {
        public string tlclScheduleField { get; set; }
        public DateTime tlclStartDateField { get; set; }
        public DateTime tlclEndDateField { get; set; }
        //added
    }

}