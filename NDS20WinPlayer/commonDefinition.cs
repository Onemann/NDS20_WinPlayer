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

    // Contents class
    public class clssContents
    {
        public long cntsKey { get; set; }               // 콘텐츠 키
        public string cntsName { get; set; }            // 콘텐츠 명
        public int cntsPlayTime { get; set; }           // 콘텐츠 재생 시간
        public DateTime scheCntsStartDt { get; set; }   //사용기간-시작일
        public DateTime scheCntsEndDt { get; set; }     //사용기간-종료일
        public DateTime scheCntsStartTime { get; set; } //사용시간-시작일
        public DateTime scheCntsEndTime { get; set; }   //사용시간-종료일
    }

    public class clssScheduleFileList
    {
        public string scheType { get; set; }        // [숨김]스케쥴 코드 : 01-일반.기본, 02-일반.이밴트, 03-동기화.기본, 04-동기화.이벤트, 05-사내방송.기본, 06-사내방송.이벤트
        public string scheCategory { get; set; }    // 스케줄 분류 : 일반, 동기화, 사내방송 
        public string scheKind { get; set; }        // 스케줄 종류 : 기본, 이벤트
        public string ctscKey { get; set; }         // [숨김] 스케줄 키
        public string ctscName { get; set; }
        public DateTime ctscStartdate { get; set; }
        public DateTime ctscEnddate { get; set; }
        public string scheFileName { get; set; }    // [숨김] 다운받은 스케줄 파일 명
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