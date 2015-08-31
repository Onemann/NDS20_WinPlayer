using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Json;
using System.IO;
using System.Threading;
using Bauglir.Ex;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Timer = System.Threading.Timer;


namespace NDS20WinPlayer
{

    public partial class NDSMain : Form
    {

        public const int WmNclbuttondown = 0xA1;
        public const int HtCaption = 0x2;

        int _nCountServerConnection = 0;
        private readonly PerformanceCounter _pCpu;
        private readonly PerformanceCounter _pMem;

        System.Threading.Timer _tmrSeverConnection; // Try connect with server if not connected

        static bool _serverConnected = false;
        static bool _tryServerConnecting = false;

        public Timer TmrGatherPcInfo { get; private set; }

        public static bool PcInfoGathering { get; set; }


        readonly WebSocketClientConnection _fConnection = new CommonFunctions.NDSWebSocketClientConnection(); //Create WebSocket client connection

        public List<Subframe> arrSubframe;
        public List<string> arrSchedule;


        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWind, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        public NDSMain()
        {
            InitializeComponent();

            //LoadIniFile();
            //LogFile.ThreadWriteLog("====================NDS2.0 Player Opened!!====================", LogType.LOG_INFO);


            AppInfoStrc.PlyrMemTotal = MemoryHelper.GetGlobalMemoryStatusEX() / 1024 / 1024;

            _pCpu = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            _pMem = new PerformanceCounter("Memory", "Available MBytes", true);


            arrSubframe = new List<Subframe>();
            arrSchedule = new List<string>();

            AssignScheule();

            AssignWebSocket();
            TmrSeverConnectionStart();
            TmrGatherPcInfoStart();

        }

        private void TmrGatherPcInfoStart()
        {
            if (TmrGatherPcInfo != null) TmrGatherPcInfo.Dispose();     // Stop timer
            TimerCallback callback = TmrGatherPcInfoEvent;
            Object data = (Object)200;

            TmrGatherPcInfo = new System.Threading.Timer(callback, data, 1000, 1000);
        }

        #region Start server connection thread
        private void TmrSeverConnectionStart()
        {if (_tmrSeverConnection != null) _tmrSeverConnection.Dispose(); // Stop Timer
            _nCountServerConnection = 0;
            System.Threading.TimerCallback callback = TmrSeverConnectionEvent;
            Object data = (Object)200;

            _tmrSeverConnection = new System.Threading.Timer(callback, data, 1000, 5000);
        }
        #endregion

#region Try to gather NDS player system information

        protected void TmrGatherPcInfoEvent(Object obj)
        {
            if(PcInfoGathering) return;

            Invoke(new MethodInvoker(delegate()
            {
                PcInfoGathering = true;
                new Thread(StartGatherPcInfo).Start();
            }
                ));
        }
#endregion


        #region try to connect with server

        protected void TmrSeverConnectionEvent(Object obj)
        {
            if (_tryServerConnecting || _serverConnected) return;
            // try to connect with server by web socket
            Invoke(new MethodInvoker(delegate()
            {
                if (!_serverConnected && !_tryServerConnecting)
                {
                    _tryServerConnecting = true;
                    new Thread(StartNdsWebSocket).Start(); 
                }

                //long totalByteOfMemoryUsed = currentProcess.WorkingSet64 / 1024;
                _nCountServerConnection++;

                //get the physical mem usage
                //var memStat = Win32MemApi.GlobalMemoryStatusEx();

                //var totalByteOfMemoryUsed = memStat.UllTotalPhys /1024;
                //var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                //AppInfoStrc.PlyrMemUsage = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024;

                // calculate CPU Usage %
                //AppInfoStrc.PlyrCpUusage = CurrentCpUusage;

                //lblServerConnectionStatus.Text = (_serverConnected).ToString() + _nCountServerConnection + @" Memory: " +
                //                                 AppInfoStrc.PlyrMemUsage + "K"; // + " CPU:" + CurrentCPUusage;
                //lblServerConnectionStatus.Text = (_serverConnected).ToString() + _nCountServerConnection + " Memory: " + GC.GetTotalMemory(false);
            }
                ));
        }
        #endregion

        private void NDSMain_MouseDown(object sender, MouseEventArgs e)
        {
//#if DEBUG
            ReleaseCapture();
            SendMessage(this.Handle, WmNclbuttondown, HtCaption, 0);
//#endif
        }

        // open new sub frame with JSON frame parameter 
        private void OpenSubframe(JsonObject jsonFrame)
        {
            Subframe newSubframe = new Subframe(jsonFrame);
            newSubframe.TopLevel = false;
            newSubframe.Parent = this;
            newSubframe.BackColor = Color.Gold;
            newSubframe.Show();

        }

        private void NDSMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
//#if DEBUG
                case Keys.F3:
                    InitMainScreen();
                    drawSubFrame();
                    break;

//#endif
                case Keys.F7:
                    ShowManagerForm();
                    break;

                case Keys.Escape:
                    Close();
                    //Application.Exit();
                    break;
            }
        }

        #region 테스트 코딩
        private void AssignScheule()
        {
            arrSchedule.Add(
                "{" +
                " \"xPos\": 0," +
                " \"yPos\": 0," +
                " \"width\": 0," +
                " \"height\": 0," +
                " \"fileName\": \"D:/Projects/NDS20_WinPlayer/NDS20WinPlayer/bin/x86/Release/G00001OLC00002.wmv\"," +
                " \"mute\": true " +
                "}"
                );
            /*
            arrSchedule.Add(
                "{" +
                " \"xPos\": 1200," +
                " \"yPos\": 50," +
                " \"width\": 1400," +
                " \"height\": 788," +
                " \"fileName\": \"D:/Projects/NDS/Contents/A.tp\"," +
                " \"mute\": false " +
                "}"
                );

            arrSchedule.Add(
                "{" +
                " \"xPos\": 500," +
                " \"yPos\": 500," +
                " \"width\": 501," +
                " \"height\": 282," +
                " \"fileName\": \"D:/Projects/NDS/Contents/A.mov\"," +
                " \"mute\": true " +
                "}"
                );

            arrSchedule.Add(
                "{" +
                " \"xPos\": 800," +
                " \"yPos\": 600," +
                " \"width\": 600," +
                " \"height\": 350," +
                " \"fileName\": \"D:/Projects/NDS/Contents/A.wmv\"," +
                " \"mute\": true " +
                "}"
                );

            arrSchedule.Add(
                "{" +
                " \"xPos\": 2400," +
                " \"yPos\": 900," +
                " \"width\": 501," +
                " \"height\": 282," +
                " \"fileName\": \"D:/Projects/NDS/Contents/A.ts\"," +
                " \"mute\": true " +
                "}"
                );

            arrSchedule.Add(
                "{" +
                " \"xPos\": 50," +
                " \"yPos\": 50," +
                " \"width\": 500," +
                " \"height\": 900," +
                " \"fileName\": \"D:/Projects/NDS/Contents/B.wmv\"," +
                " \"mute\": true " +
                "}"
                );
            */
        }
        #endregion

        private void drawSubFrame()
        {
            int noFrame = arrSchedule.Count;
            for (int i = 0; i < noFrame; i++)
            {
                JsonTextParser parser = new JsonTextParser();
                JsonObject jsonSchedule = parser.Parse(arrSchedule[i]);
                OpenSubframe(jsonSchedule);
            }

        }

        private void InitMainScreen()
        {
            this.BackColor = Color.Black;
            pnlHeader.Visible = false;
            pnlBottom.Visible = false;
            this.BackgroundImage = null;
        }

        // 생성된 폼이 있는지 확인
        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.GetType() == FormType)
                    return openForm;
            }
            return null;
        }
 
        private static void ShowManagerForm()
        {
            ManagerForm managerForm = null;
            if( (managerForm = (ManagerForm)IsFormAlreadyOpen(typeof(ManagerForm))) == null) //생성된 폼이 없다면
            {
                managerForm = new ManagerForm();
            }
            managerForm.Show();
            managerForm.BringToFront();
            //managerForm.TopLevel = true;
        }

        private void NDSMain_Load(object sender, EventArgs e)
        {
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            this.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.Location = new System.Drawing.Point(screenLeft, screenTop);

        }

        private void SendPlayerStatusInfo()
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            collection.Add(new JsonStringValue(JsonColName.JsonTxtHndId, AppInfoStrc.TextHandlerId));   // textHandlerID
            collection.Add(new JsonStringValue(JsonColName.JsonPlyrId, AppInfoStrc.PlayerId));          // PlayerID
            collection.Add(new JsonStringValue(JsonColName.JsonCmd, JsonCmd.PlayerSysStatus));          // cmd
            collection.Add(new JsonNumericValue(JsonColName.JsonTimestamp, CommonFunctions.ConvertToUnixTimestamp(DateTime.Now))); 

            JsonObjectCollection dataInfo = new JsonObjectCollection();
            //collection.Add(new JsonObjectCollection(JsonColName.JsonData));             // cmd
            dataInfo.Add(new JsonNumericValue(JsonColName.JsonDataCpu, AppInfoStrc.PlyrCpUusage));
            dataInfo.Add(new JsonNumericValue(JsonColName.JsonDataMem, AppInfoStrc.PlyrMemUsage));
            dataInfo.Add(new JsonNumericValue(JsonColName.JsonDataHdd, AppInfoStrc.PlyrAvailableHdd));
            dataInfo.Add(new JsonNumericValue(JsonColName.JsonDataNet, AppInfoStrc.PlyrNetUsage));
            dataInfo.Add(new JsonStringValue(JsonColName.JsonDataVer, Application.ProductVersion));
            JsonObjectCollection subData = new JsonObjectCollection(JsonColName.JsonData, dataInfo);
            collection.Add(subData);




            var jsonText = collection.ToString();

            if (!_fConnection.Closed)
            {
                _fConnection.SendText(jsonText);
            }
        }

        private void StartGatherPcInfo()
        {
            if(!PerformanceCounterCategory.Exists("Processor")) return;
            if (!PerformanceCounterCategory.CounterExists(@"% Processor Time", "Processor")) return;


            AppInfoStrc.PlyrCpUusage = (int)_pCpu.NextValue();
            AppInfoStrc.PlyrAvailableHdd = 20;
            AppInfoStrc.PlyrMemUsage = (int)_pMem.NextValue();

            ManagerForm managerForm = null;

            if ((managerForm = (ManagerForm)NDSMain.IsFormAlreadyOpen(typeof(ManagerForm))) != null) //생성된 폼이 있다면
            {
                managerForm.ShowNdsInfoOnStatusBar();

            }
            PcInfoGathering = false;

        }

        private void SendRequestScheduleSending()
        {
            try
            {
                JsonObjectCollection collection = new JsonObjectCollection();
                collection.Add(new JsonStringValue(JsonColName.JsonTxtHndId, AppInfoStrc.TextHandlerId));   // textHandlerID
                collection.Add(new JsonStringValue(JsonColName.JsonPlyrId, AppInfoStrc.PlayerId));          // PlayerID
                collection.Add(new JsonStringValue(JsonColName.JsonCmd, JsonCmd.SchedueleDown));            // cmd
                collection.Add(new JsonNumericValue(JsonColName.JsonTimestamp,
                    CommonFunctions.ConvertToUnixTimestamp(DateTime.Now)));

                var jsonText = collection.ToString();

                if (!_fConnection.Closed)
                {
                    _fConnection.SendText(jsonText);
                }

            }
            catch (Exception ex)
            {
                LogFile.ThreadWriteLog("[SEND-ERR]" + "SendRequestScheduleSending():" + ex.Message , LogType.LOG_ERROR);
            }
        }

        private  void ActAfterReceivingJson(string jsonText)
        {
            try
            {
                var parser = new JsonTextParser();
                var jsonObj = parser.Parse(jsonText);
                // abstract cmd
                var col = (JsonObjectCollection) jsonObj;
                var cmdValue = (string)col[JsonColName.JsonCmd].GetValue();

                switch (cmdValue)
                {                
                    case JsonCmd.ServerConnected:
                        AppInfoStrc.TextHandlerId = (string) col[JsonColName.JsonTxtHndId].GetValue();
                        SendPlayerStatusInfo(); // send player system status info : CPU, HDD, MEM
                        SendRequestScheduleSending(); // sending request to server for releasing schedule
                        break;
                    case JsonCmd.NewScheduele:
                        SendRequestScheduleSending();
                        break;
                    case JsonCmd.SchedueleDown:
                        JsonArrayCollection sheduleListColl = (JsonArrayCollection) col[JsonColName.JsonschdList];
                                // abstract only schedule JSON text

                        string scheduleColItem = "";
                        for (int idx = 0; idx < sheduleListColl.Count; idx++)
                        {
                            if (scheduleColItem != "") scheduleColItem += ",";
                            scheduleColItem = scheduleColItem + sheduleListColl[idx];
                        }

                        scheduleColItem = "[" + scheduleColItem + "]";
                        string oldScheduleFileText = GetLatestScheuleFileText(); 
                        if (scheduleColItem != oldScheduleFileText.TrimEnd() ) // create new schedule file if any differences. 
                        // if received schedule json is differ from latest schedule file
                        {
                            // Schedule file will be created with filename consist with timestamp for unique naming. 
                            double unixTimeStamp = (double) col[JsonColName.JsonTimestamp].GetValue();
                            DateTime datetimeTimestamp = CommonFunctions.UnixTimeStampToDateTime(unixTimeStamp);
                            string timeStamp = string.Format("{0:yyyyMMdd_HHmmss}", datetimeTimestamp);
                            JsonToTextFile(scheduleColItem, timeStamp);

                        }
                        var beDownloadContentsListJson = CreateNotDownlodedContents(); // for Downloading....

                        if (beDownloadContentsListJson != null)
                        {
                            // Web client download info

                            DoDownloadContents(beDownloadContentsListJson); // begin Downloading.... 

                        }
                        break;
                }

            }
            catch (Exception)
            {
                // Write error log - Received JSON text.
                var outText = jsonText.Replace("\"", "'");
                LogFile.ThreadWriteLog("[RECV-ERR]" + outText, LogType.LOG_ERROR);
            }
        }

        private void DoDownloadContents(JsonArrayCollection beDownloadContentsListJson)
        {
            foreach (JsonObject jsonObject in beDownloadContentsListJson)
            {
                JsonObjectCollection col = (JsonObjectCollection) jsonObject;

                string contentsFileName = col["fileName"].GetValue().ToString();

                double unixTimeStamp = (double)col["cntsUpdateDt"].GetValue();
                DateTime datetimeTimestamp = CommonFunctions.UnixTimeStampToDateTime(unixTimeStamp);
                string contentsUpdateDt = string.Format("{0:yyyyMMdd_HHmmss}", datetimeTimestamp);


                string fullUrl = AppInfoStrc.UrlDownloadWebServer + AppInfoStrc.ExtDownloadWebServer + contentsFileName;
                string fullLocalDir = AppInfoStrc.DirOfContents  +
                                       contentsUpdateDt;

                DirectoryInfo dirIfo = null;
                dirIfo = new DirectoryInfo(fullLocalDir);
                if (!dirIfo.Exists) dirIfo.Create();

                string fullLocalPath = fullLocalDir + @"\" + contentsFileName;

                DownloadFile(fullUrl, fullLocalPath, DownloadStatusChanged, DownloadCompleted);
//                DownloadFile(fullUrl, contentsFileName, DownloadStatusChanged, DownloadCompleted);
            }
        }

        private static string GetLatestScheuleFileText()
        {
            try
            {
                // Get latest schedule file name
                DirectoryInfo schDirectoryInfo = new DirectoryInfo(@AppInfoStrc.DirOfSchedule);
                Current.SchdName =
                    schDirectoryInfo.EnumerateFiles().OrderByDescending(f => f.FullName).FirstOrDefault();

                //load latest schedule file's Json text
                if (Current.SchdName != null)
                {
                    var latestScheduleFullName = Current.SchdName.FullName;
                    return File.ReadAllText(latestScheduleFullName);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                if (Current.SchdName != null) // Previous schedule file exists but wont be read.
                    LogFile.ThreadWriteLog("[SCHD-ERR]" + Current.SchdName.Name + " 읽기 오류", LogType.LOG_WARN);
                return String.Empty;
            }
        }

        #region Create Json array collection for Contents lists were not downloaded
        private static JsonArrayCollection CreateNotDownlodedContents()
        {
            try
            {
                string scheduleText = GetLatestScheuleFileText();
                var parser = new JsonTextParser();
                var jsonObj = parser.Parse(@scheduleText);
                // abstract cmd
                var colArry = (JsonArrayCollection) jsonObj;
                //var col = (JsonObjectCollection)jsonObj;
                var arrColPlayContentsList = new JsonArrayCollection();



                foreach (var o in colArry)
                {
                    var oneJsonSchdObj = (JsonObjectCollection) o;

                    var col = (JsonObjectCollection) o;

                    AppInfoStrc.UrlDownloadWebServer = col[JsonColName.UrlDownloadWebServer].GetValue().ToString();
                    AppInfoStrc.ExtDownloadWebServer = col[JsonColName.ExtDownloadWebServer].GetValue().ToString();
                    AppInfoStrc.BridgePath = col[JsonColName.BridgePath].GetValue().ToString();
                    AppInfoStrc.CurrentScheduleKey = col[JsonColName.JsonSchdKey].GetValue().ToString();

                    var colArrayContentsList = (JsonArrayCollection)oneJsonSchdObj[JsonColName.JsonContentsList];
                    if (colArrayContentsList != null)
                    {
                        foreach (var oc in colArrayContentsList)
                        {
                            var oneJsonContentsObj = (JsonObjectCollection) oc;
                            var jsonContentsObj2 = new JsonObjectCollection();

                            if (!isAreadyExist(arrColPlayContentsList, oneJsonContentsObj["cntsKey"]))
                            {

                                jsonContentsObj2.Add(oneJsonContentsObj["cntsKey"]);
                                jsonContentsObj2.Add(oneJsonContentsObj["cntsUpdateDt"]);
                                jsonContentsObj2.Add(oneJsonContentsObj["fileName"]);

                                arrColPlayContentsList.Add(jsonContentsObj2);
                            }
                        }
                    }
                }
                return arrColPlayContentsList;
            }
            catch (Exception)
            {
                LogFile.ThreadWriteLog("[JSON-ERR]" + Current.SchdName.Name + " 다운받을 콘텐츠 리스트 생성 오류", LogType.LOG_ERROR);
                return null;
            }
        }

        private static bool isAreadyExist(JsonArrayCollection jsonArrayCollection, JsonObject jsonObject) //다운받을 동일한 콘텐츠 파일이 없도록
        {
            foreach (JsonObject o in jsonArrayCollection)
            {
                if (o.ToString().Contains(jsonObject.ToString()))
                    return true;
            }
            return false;
        }
        #endregion

        #region Create Json onject of contents list includes secotr for contents playing
        public static JsonArrayCollection CreateContentsPlayListBySector()
        {
            try
            {  
                string scheduleText = GetLatestScheuleFileText();
                var parser = new JsonTextParser();
                var jsonObj = parser.Parse(@scheduleText);
                // abstract cmd
                var colArry = (JsonArrayCollection)jsonObj;

                foreach (var o in colArry)
                {
                    int fldKeyId = 0;
                    int fldParent = 0;
                    int fldOrder = 0;

                    var oneJsonSchdObj = (JsonObjectCollection) o;
                    var vtotSector = oneJsonSchdObj[JsonColName.JsonSchdTotSec].GetValue(); //총구간
                    int totSector = int.Parse(vtotSector.ToString());
                        // create Json object array of play contents list by sector order
                    if (totSector > 0)
                    {
                        //var jsonPlayContentsListObj = new JsonObjectCollection();
                        var arrColPlayContentsList = new JsonArrayCollection();
                        for (int idx = 0; idx < totSector; ++idx)
                        {
                            var jsonContentsObj = new JsonObjectCollection();

                            #region create parent sector
                            ++fldKeyId;
                            jsonContentsObj.Add(new JsonNumericValue("fldKeyId", fldKeyId)); // Key ID
                            // 구간 Added for displaying Sector field that parent tab in tree list
                            jsonContentsObj.Add(new JsonNumericValue("fldSector", idx + 1));
                            jsonContentsObj.Add(new JsonNumericValue("fldParentKey", 0));
                            fldParent = fldKeyId;

                            arrColPlayContentsList.Add(jsonContentsObj);
                            #endregion
                            //jsonPlayContentsListObj.Add(arrColPlayContentsList);


                            // abstract contents list to Json array collections
                            var colArrayContentsList =
                                (JsonArrayCollection) oneJsonSchdObj[JsonColName.JsonContentsList];

                            foreach (var oc in colArrayContentsList)
                            {
                                var oneJsonContentsObj = (JsonObjectCollection)oc;

                                string cntsSectors = oneJsonContentsObj["cntsSectors"].GetValue().ToString();
                                var arrSectors = cntsSectors.Split(',');

                                if (arrSectors.Contains((idx + 1).ToString())) // if this content is included this sector then create play content list
                                {
                                    //var arrColPlayContentsList2 = new JsonArrayCollection();
                                    var jsonContentsObj2 = new JsonObjectCollection();

                                    ++fldKeyId;
                                    ++fldOrder;

                                    jsonContentsObj2.Add(new JsonNumericValue("fldKeyId", fldKeyId)); // Key ID
                                    jsonContentsObj2.Add(new JsonNumericValue("fldOrder", fldOrder)); // 순서
                                    jsonContentsObj2.Add(new JsonNumericValue("fldParentKey", fldParent)); // ParentKey
                                    // 구간 Added for displaying Sector field that parent tab in tree list
                                    jsonContentsObj2.Add(new JsonNumericValue("fldSector", idx + 1));



                                    jsonContentsObj2.Add(oneJsonContentsObj["cntsKey"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["cntsName"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["cntsUpdateDt"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["cntsPlayTime"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["scheCntsStartDt"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["scheCntsEndDt"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["scheCntsStartTime"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["scheCntsEndTime"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["fileName"]);
                                    jsonContentsObj2.Add(oneJsonContentsObj["mute"]);


                                    arrColPlayContentsList.Add(jsonContentsObj2);
                                    //jsonPlayContentsListObj.Add(arrColPlayContentsList2);
                                }
                            }
                        }
                        return arrColPlayContentsList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                LogFile.ThreadWriteLog("[JSON-ERR]" + Current.SchdName.Name + " 구간별 상영 리스트 생성 오류", LogType.LOG_ERROR);
                return null;
            }
            return null;
        }
        #endregion
        private void JsonToTextFile(string jsonText, string timeStamp)
        {
            try
            {
                //Schedule file creation with schedule JSON text.
                var scheduleFilePath = AppInfoStrc.DirOfSchedule + "\\" + "Schd-" + timeStamp + "." + "sch"; // ex) Schd-20150825_122331.sch
                FileInfo scheduleFileInfo = new FileInfo(scheduleFilePath);
                DirectoryInfo scheduleDirectoryInfo = new DirectoryInfo(scheduleFileInfo.DirectoryName);
                if (!scheduleDirectoryInfo.Exists) scheduleDirectoryInfo.Create();

                if (scheduleFileInfo.Exists) scheduleFileInfo.Delete();
                FileStream scheduleFileStream = scheduleFileInfo.Create();

                StreamWriter scheduleStreamWriter = new StreamWriter(scheduleFileStream);
                scheduleStreamWriter.WriteLine(jsonText);

                scheduleStreamWriter.Close();

            }
            catch (Exception)
            {
                LogFile.ThreadWriteLog("[SCHD-ERR]" + AppInfoStrc.DirOfSchedule + "\\" + "Schd-" + timeStamp + "." + "sch" + " 파일 생성 오류", LogType.LOG_ERROR);
            }

        }

        private void StartNdsWebSocket()
        {
            if (AppInfoStrc.UrlOfServer != "" && AppInfoStrc.PortOfServer != "")
            {
                _serverConnected = _fConnection.Start(AppInfoStrc.UrlOfServer, AppInfoStrc.PortOfServer, AppInfoStrc.ExtentionOfServer, false);
                if (!_serverConnected)
                {
                    LogFile.ThreadWriteLog("[NETWORK ERR]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 실패", LogType.LOG_ERROR);
                }
            }
            else
            {
                LogFile.ThreadWriteLog("[INI_ERR]:" + "설정 파일에 서버 접속 정보가 없습니다.", LogType.LOG_ERROR);
            }
            _tryServerConnecting = false;
        }

        private void AssignWebSocket()
        {
            _fConnection.ConnectionClose += ConnectionClose;
            _fConnection.ConnectionRead += ConnectionRead;
            _fConnection.ConnectionWrite += ConnectionWrite;
            _fConnection.ConnectionOpen += ConnectionOpen;
            ((CommonFunctions.NDSWebSocketClientConnection)_fConnection).ConnectionPing += ConnectionPing;
            ((CommonFunctions.NDSWebSocketClientConnection)_fConnection).ConnectionPong += ConnectionPong;
            ((CommonFunctions.NDSWebSocketClientConnection)_fConnection).ConnectionFramedBinary += ConnectionFramedBinary;
            ((CommonFunctions.NDSWebSocketClientConnection)_fConnection).ConnectionFramedText += ConnectionFramedText;
        }

        void ConnectionOpen(WebSocketConnection aConnection)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new WebSocketConnection.ConnectionEvent(ConnectionOpen), new Object[] { aConnection });
            }
            else
            {
                //LogFile.threadWriteLog(String.Format("ConnectionOpen {0}", aConnection.Index), LogType.LOG_TRACE);
                _serverConnected = true;

                LogFile.ThreadWriteLog("[NETWORK]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 성공", LogType.LOG_INFO);
            }
        }

        void ConnectionClose(WebSocketConnection aConnection, int aCloseCode, string aCloseReason, bool aClosedByPeer)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new WebSocketConnection.ConnectionCloseEvent(ConnectionClose), new Object[] { aConnection, aCloseCode, aCloseReason, aClosedByPeer });
            }
            else
            {
                //LogFile.threadWriteLog(String.Format("ConnectionClose {0}: {1}, {2}, {3} ", aConnection.Index, aCloseCode, aCloseReason, aClosedByPeer ? "closed by peer" : "closed by me"), LogType.LOG_TRACE);
                _serverConnected = false;
                LogFile.ThreadWriteLog("[NETWORK]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 종료", LogType.LOG_INFO);
            }
        }

        void ConnectionRead(WebSocketConnection aConnection, bool aFinal, bool aRes1, bool aRes2, bool aRes3, int aCode, MemoryStream aData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new WebSocketConnection.ConnectionDataEvent(ConnectionRead), new Object[] { aConnection, aFinal, aRes1, aRes2, aRes3, aCode, aData });
            }
            else
            {
                //LogFile.threadWriteLog(String.Format("ConnectionRead {0}: final {1}, ext1 {2}, ext2 {3}, ext1 {4}, code {5}, length {6} ", aConnection.Index, aFinal, aRes1, aRes2, aRes3, aCode, aData.Length), LogType.LOG_TRACE);
                //lastReceivedMemo.Text = Encoding.UTF8.GetString(aData.ToArray(), 0,  (int)aData.Length);
                //-lastReceivedMemo.Text = Encoding.UTF8.GetString(aData.ToArray());
                //LogFile.ThreadWriteLog("[READ]" + Encoding.UTF8.GetString(aData.ToArray()), LogType.LOG_INFO);
                var inString = Encoding.UTF8.GetString(aData.ToArray());


                #region Comment after debugging.
                var outString = inString.Replace("\"", "'");
                LogFile.ThreadWriteLog("[RECV]" + outString, LogType.LOG_INFO);
                #endregion

                ActAfterReceivingJson(inString);
            }
        }

        void ConnectionWrite(WebSocketConnection aConnection, bool aFinal, bool aRes1, bool aRes2, bool aRes3, int aCode, MemoryStream aData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new WebSocketConnection.ConnectionDataEvent(ConnectionWrite), new Object[] { aConnection, aFinal, aRes1, aRes2, aRes3, aCode, aData });
            }
            else
            {
                //LogFile.threadWriteLog(String.Format("ConnectionWrite {0}: final {1}, ext1 {2}, ext2 {3}, ext1 {4}, code {5}, length {6} ", aConnection.Index, aFinal, aRes1, aRes2, aRes3, aCode, aData.Length), LogType.LOG_TRACE);
                //-lastSentMemo.Text = Encoding.UTF8.GetString(aData.ToArray(), 0, (int)aData.Length);
                var inString = Encoding.UTF8.GetString(aData.ToArray());
                var outString = inString.Replace("\"", "'");
                LogFile.ThreadWriteLog("[SEND]" +outString, LogType.LOG_INFO);
            }
        }

        void ConnectionPing(WebSocketConnection aConnection, string aData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CommonFunctions.NDSWebSocketClientConnection.ConnectionPingPongEvent(ConnectionPing), new Object[] { aConnection, aData });
            }
            else
            {
                //LogFile.threadWriteLog(String.Format("ConnectionPing {0}: {1}", aConnection.Index, aData), LogType.LOG_TRACE);
            }
        }

        void ConnectionPong(WebSocketConnection aConnection, string aData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CommonFunctions.NDSWebSocketClientConnection.ConnectionPingPongEvent(ConnectionPong), new Object[] { aConnection, aData });
            }
            else
            {
                //LogFile.threadWriteLog(String.Format("ConnectionPong {0}: {1}", aConnection.Index, aData), LogType.LOG_TRACE);
            }
        }

        void ConnectionFramedText(WebSocketConnection aConnection, string aData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CommonFunctions.NDSWebSocketClientConnection.ConnectionFramedTextEvent(ConnectionFramedText), new Object[] { aConnection, aData });
            }
            else
            {
                //-sendFramesMemo.Text = aData;
            }
        }

        void ConnectionFramedBinary(WebSocketConnection aConnection, MemoryStream aData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CommonFunctions.NDSWebSocketClientConnection.ConnectionFramedBinaryEvent(ConnectionFramedBinary), new Object[] { aConnection, aData });
            }
            else
            {
                Image image = Image.FromStream(aData);
                //-pictureBox.Image = image;
                //sendFramesMemo.Text = aData;
            }
        }
        private void NDSMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_serverConnected) _fConnection.Close(WebSocketCloseCode.Normal);
        }



        private Stopwatch _sw;

        public void DownloadFile(string url, string fileName ,
            Action<string, DownloadProgressChangedEventArgs> DownloadStatusChanged, 
           Action<string, AsyncCompletedEventArgs>  DownloadCompleted)
        {
            //string path = @"C:\DL\";

            Thread bgThread = new Thread(() =>
            {
                _sw = new Stopwatch();
                _sw.Start();
                //ManagerForm managerForm = null;
                //if ((managerForm = (ManagerForm) IsFormAlreadyOpen(typeof (ManagerForm))) != null) //생성된 폼이 있다면
                //{
                    //labelDownloadAudioStatusText.Visibility = Visibility.Visible;
                //}
                fileName += "part";
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged +=
                        (object sender, DownloadProgressChangedEventArgs e) => DownloadStatusChanged(fileName, e);

                    webClient.DownloadFileCompleted +=
                        (object sender, AsyncCompletedEventArgs e) => DownloadCompleted(fileName, e);

                    /*
                    webClient.DownloadFileCompleted +=
                        new AsyncCompletedEventHandler(DownloadCompleted);
                    webClient.DownloadProgressChanged +=
                        new DownloadProgressChangedEventHandler(DownloadStatusChanged);
                     */
                    webClient.DownloadFileAsync(new Uri(url), fileName);
                }
            });

            bgThread.Start();
        }

        private void DownloadStatusChanged(string fileName, DownloadProgressChangedEventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                int percent = 0;

                if (e.ProgressPercentage != percent)
                {
                    percent = e.ProgressPercentage;
                    

                    ManagerForm managerForm = null;
                    if ((managerForm = (ManagerForm)IsFormAlreadyOpen(typeof(ManagerForm))) != null) //생성된 폼이 있다면
                    {
                        //managerForm.ShowDownloadProgressOnStatusBar(percent);
                        //managerForm.MessageOnStatusBar(fileName, LogType.LOG_TRACE);
                        fileName = new FileInfo(fileName).Name.ToString();

                        fileName = CommonFunctions.TrimEndString(fileName, "part");
                        managerForm.ChangeDownloadProgressInGrdContents(fileName, percent);

                    }
                    //progressBarDownloadAudio.Value = percent;

                    //labelDownloadAudioProgress.Content = percent + "%";
                    //labelDownloadAudioDlRate.Content = (Convert.ToDouble(e.BytesReceived) / 1024 /_sw.Elapsed.TotalSeconds).ToString("0.00") + " kb/s";

                    //Thread.Sleep(50);
                }
            });
        }

        private void DownloadCompleted(string fileName, AsyncCompletedEventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                //labelDownloadAudioDlRate.Content = "0 kb/s";
                //labelDownloadAudioStatusText.Visibility = Visibility.Hidden;
                //MessageBox.Show("Download completed");
                /*
                string downloadedLocalFile = fileName;
                ManagerForm managerForm = null;
                if ((managerForm = (ManagerForm)IsFormAlreadyOpen(typeof(ManagerForm))) != null) //생성된 폼이 있다면
                {
                    managerForm.MessageOnStatusBar("[Download completed]" + downloadedLocalFile, LogType.LOG_TRACE);
                }
                 */

                // rename *.extpart to *.ext
                string newFileName = CommonFunctions.TrimEndString(fileName, "part");
                if(File.Exists(newFileName))
                {
                    File.Delete(newFileName);
                }
                File.Move(fileName, newFileName);

            });
        }
    }

}
