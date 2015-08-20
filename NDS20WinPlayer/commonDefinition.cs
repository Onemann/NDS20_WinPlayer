using System;

namespace NDS20WinPlayer
{
    class AppInfoStrc
    {
        public static string DirOfApplication;
        public static string DirOfSchedule;
        public static string DirOfLog;

        public static string UrlOfServer;       //REST Server URL : ex) 52.50.218.207
        public static string ExtentionOfServer; //REST Server Extension : ex: /socket/json
        public static string PortOfServer;      //REST Server Port : ex) 9090 
        public static string PlayerID;          //Player ID that was registered and permitted by server 
        public static string TextHandlerID = null;     //Assigned and given from server after connecting
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
        public string scheCntsEndTime { get; set; }     //사용시간-종료일
        public string ctscSector { get; set; }          //구간 정보 ex) "1,4,7"

        #region 구간 30 Sectors are more than enough
        public bool sector1 { get; set; }         // [가칭] 1구간
        public bool sector2 { get; set; }         
        public bool sector3 { get; set; }         
        public bool sector4 { get; set; }         
        public bool sector5 { get; set; }         
        public bool sector6 { get; set; }         
        public bool sector7 { get; set; }         
        public bool sector8 { get; set; }         
        public bool sector9 { get; set; }         
        public bool sector10 { get; set; }        

        public bool sector11 { get; set; }        
        public bool sector12 { get; set; }        
        public bool sector13 { get; set; }        
        public bool sector14 { get; set; }        
        public bool sector15 { get; set; }        
        public bool sector16 { get; set; }        
        public bool sector17 { get; set; }        
        public bool sector18 { get; set; }        
        public bool sector19 { get; set; }        
        public bool sector20 { get; set; }        

        public bool sector21 { get; set; }        
        public bool sector22 { get; set; }        
        public bool sector23 { get; set; }        
        public bool sector24 { get; set; }        
        public bool sector25 { get; set; }        
        public bool sector26 { get; set; }        
        public bool sector27 { get; set; }        
        public bool sector28 { get; set; }        
        public bool sector29 { get; set; }        
        public bool sector30 { get; set; }        


        #endregion
   }

    public class clssSchedule
    {
        public string scheType { get; set; }        // [숨김]스케쥴 코드 : 01-일반.기본, 02-일반.이밴트, 03-동기화.기본, 04-동기화.이벤트, 05-사내방송.기본, 06-사내방송.이벤트
        public string scheCategory { get; set; }    // [생성]스케줄 분류 : 일반, 동기화, 사내방송 
        public string scheKind { get; set; }        // [생성]스케줄 종류 : 기본, 이벤트
        public string ctscKey { get; set; }         // [숨김] 스케줄 키
        public string ctscName { get; set; }
        public DateTime ctscStartdate { get; set; }
        public DateTime ctscEnddate { get; set; }
        public int scheTotalSector { get; set; }         // [가칭] 총 구간
        public string scheFileName { get; set; }    // [생성, 숨김] 다운받은 스케줄 파일 명 - 파일명에서 가져옴
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