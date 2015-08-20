using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Json;
using System.IO;
using System.Threading;
using Bauglir.Ex;
using System.Net.NetworkInformation;

namespace NDS20WinPlayer
{

    public partial class NDSMain : Form
    {

        public const int WmNclbuttondown = 0xA1;
        public const int HtCaption = 0x2;

        int _nCountServerConnection = 0;

        System.Threading.Timer _tmrSeverConnection; // Try connect with server if not connected

        static bool _serverConnected = false;
        static bool _tryServerConnecting = false;

        WebSocketClientConnection fConnection = new NDSWebSocketClientConnection(); //Create WebSocket client connection

        public List<Subframe> arrSubframe;
        public List<string> arrSchedule;


        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWind, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public NDSMain()
        {
            InitializeComponent();
            LoadIniFile();
            LogFile.threadWriteLog("====================NDS2.0 Player Opened!!====================", LogType.LOG_INFO);


            arrSubframe = new List<Subframe>();
            arrSchedule = new List<string>();

            assignScheule();

            assignWebSocket();
            
            tmrSeverConnectionStart();
            //startNDSWebSocket();
        }

        #region Start server connection thread
        private void tmrSeverConnectionStart()
        {

            if (_tmrSeverConnection != null) _tmrSeverConnection.Dispose(); // Stop Timer
            _nCountServerConnection = 0;
            System.Threading.TimerCallback callback = TmrSeverConnectionEvent;
            Object data = (Object)200;

            _tmrSeverConnection = new System.Threading.Timer(callback, data, 1000, 5000);
        }
        #endregion

        #region try to connect with server

        protected virtual void TmrSeverConnectionEvent(Object obj)
        {
            // try to connect with server by web socket
            this.Invoke(new MethodInvoker(delegate()
            {
                if (!_serverConnected && !_tryServerConnecting)
                {
                    _tryServerConnecting = true;
                    new Thread(StartNdsWebSocket).Start(); 
                }

                //long totalByteOfMemoryUsed = currentProcess.WorkingSet64 / 1024;
                _nCountServerConnection++;

                //get the physical mem usage
                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                var totalByteOfMemoryUsed = currentProcess.PrivateMemorySize64 / 1024;
                lblServerConnectionStatus.Text = (_serverConnected).ToString() + _nCountServerConnection + @" Memory: " + totalByteOfMemoryUsed;
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
        private void openSubframe(JsonObject jsonFrame)
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
                    initMainScreen();
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
        private void assignScheule()
        {
            arrSchedule.Add(
                "{" +
                " \"xPos\": 400," +
                " \"yPos\": 100," +
                " \"width\": 700," +
                " \"height\": 396," +
                " \"fileName\": \"D:/Projects/NDS/Contents/A.avi\"," +
                " \"mute\": true " +
                "}"
                );

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

        }
        #endregion

        private void drawSubFrame()
        {
            int noFrame = arrSchedule.Count;
            for (int i = 0; i < noFrame; i++)
            {
                JsonTextParser parser = new JsonTextParser();
                JsonObject jsonSchedule = parser.Parse(arrSchedule[i]);
                openSubframe(jsonSchedule);
            }

        }

        private void initMainScreen()
        {
            this.BackColor = Color.Black;
            pnlHeader.Visible = false;
            pnlBottom.Visible = false;
            this.BackgroundImage = null;
        }

        // 생성된 폼이 있는지 확인
        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }
            return null;
        }
 
        private void ShowManagerForm()
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

        private void LoadIniFile()
        {
            var appIniFile = new IniFile();

            #region Dir Path
            AppInfoStrc.DirOfApplication = Environment.CurrentDirectory;

            AppInfoStrc.DirOfSchedule = appIniFile.Read("DirOfSchedule", "PATH");
            if (AppInfoStrc.DirOfSchedule == "")
            {
                appIniFile.Write(Key: InitValue.InitDirOfSchedule, Value: "[Path]");
                AppInfoStrc.DirOfSchedule = InitValue.InitDirOfSchedule;
            }

            AppInfoStrc.DirOfLog = appIniFile.Read("DirOfLog", "PATH");
            if (AppInfoStrc.DirOfLog == "")
            {
                appIniFile.Write(Key: InitValue.InitDirOfLog, Value: "PATH");
                AppInfoStrc.DirOfLog = InitValue.InitDirOfLog;
            }
            #endregion

            #region Server connectoion Info
            AppInfoStrc.UrlOfServer = appIniFile.Read("UrlOfServer", "SERVER");
            if (AppInfoStrc.UrlOfServer == "")
            {
                appIniFile.Write(Key: InitValue.InitUrlOfServer, Value: "SERVER");
                AppInfoStrc.UrlOfServer = InitValue.InitUrlOfServer;
            }

            AppInfoStrc.ExtentionOfServer = appIniFile.Read("ExtentionOfServer", "SERVER");
            if (AppInfoStrc.ExtentionOfServer == "")
            {
                appIniFile.Write(Key: InitValue.InitExtensionOfServer, Value: "SERVER");
                AppInfoStrc.ExtentionOfServer = InitValue.InitExtensionOfServer;
            }

            AppInfoStrc.PortOfServer = appIniFile.Read("PortOfServer", "SERVER");
            if (AppInfoStrc.PortOfServer == "")
            {
                appIniFile.Write(Key: InitValue.InitPortOfServer, Value: "SERVER");
                AppInfoStrc.PortOfServer = InitValue.InitPortOfServer;
            }
            #endregion

            AppInfoStrc.PlayerId = appIniFile.Read("PlayerID", "PLAYER");
            
        }

        private void StartNdsWebSocket()
        {
            if (AppInfoStrc.UrlOfServer != "" && AppInfoStrc.PortOfServer != "")
            {
                #region ping test

                Ping x = new Ping();
                PingReply reply = x.Send(AppInfoStrc.UrlOfServer);

                if (reply.Status != IPStatus.Success)
                {
                    LogFile.threadWriteLog("[NETWORK ERROR]:" + AppInfoStrc.UrlOfServer + "로 접속할 수 없습니다.", LogType.LOG_ERROR);
                    _tryServerConnecting = false;
                    return;
                }
                #endregion
                
                /*
                #region TCP client test
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                try
                {
                    client.Connect(System.Net.IPAddress.Parse( AppInfoStrc.UrlOfServer), int.Parse( AppInfoStrc.PortOfServer));
                } catch(System.Net.Sockets.SocketException ex)
                {
                    return;
                }
                finally
                {
                    client.Close();
                }
                #endregion
                */

                //ServerConnected = fConnection.Start(AppInfoStrc.UrlOfServer, AppInfoStrc.PortOfServer, AppInfoStrc.ExtentionOfServer, false);
                _serverConnected = fConnection.Start(AppInfoStrc.UrlOfServer, AppInfoStrc.PortOfServer, "", false);
                if (_serverConnected)
                {
                    LogFile.threadWriteLog("[NETWORK]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 성공", LogType.LOG_INFO);
                }
                else
                {
                    LogFile.threadWriteLog("[NETWORK ERROR]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 실패", LogType.LOG_ERROR);
                }
            }
            else
            {
                LogFile.threadWriteLog("[INI_ERROR]:" + "설정 파일에 서버 접속 정보가 없습니다.", LogType.LOG_ERROR);
            }
            _tryServerConnecting = false;
        }

        private void assignWebSocket()
        {
            fConnection.ConnectionClose += ConnectionClose;
            fConnection.ConnectionRead += ConnectionRead;
            fConnection.ConnectionWrite += ConnectionWrite;
            fConnection.ConnectionOpen += ConnectionOpen;
            ((NDSWebSocketClientConnection)fConnection).ConnectionPing += ConnectionPing;
            ((NDSWebSocketClientConnection)fConnection).ConnectionPong += ConnectionPong;
            ((NDSWebSocketClientConnection)fConnection).ConnectionFramedBinary += ConnectionFramedBinary;
            ((NDSWebSocketClientConnection)fConnection).ConnectionFramedText += ConnectionFramedText;
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
                LogFile.threadWriteLog("[READ]" + Encoding.UTF8.GetString(aData.ToArray()), LogType.LOG_INFO);
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
            }
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
            }
        }

        void ConnectionPing(WebSocketConnection aConnection, string aData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NDSWebSocketClientConnection.ConnectionPingPongEvent(ConnectionPing), new Object[] { aConnection, aData });
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
                this.Invoke(new NDSWebSocketClientConnection.ConnectionPingPongEvent(ConnectionPong), new Object[] { aConnection, aData });
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
                this.Invoke(new NDSWebSocketClientConnection.ConnectionFramedTextEvent(ConnectionFramedText), new Object[] { aConnection, aData });
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
                this.Invoke(new NDSWebSocketClientConnection.ConnectionFramedBinaryEvent(ConnectionFramedBinary), new Object[] { aConnection, aData });
            }
            else
            {
                Image image = Image.FromStream(aData);
                //-pictureBox.Image = image;
                //sendFramesMemo.Text = aData;
            }
        }

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

        private void NDSMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_serverConnected) fConnection.Close(WebSocketCloseCode.Normal);
        }


    }
}
