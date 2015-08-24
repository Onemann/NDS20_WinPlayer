﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using Bauglir.Ex;
using DevExpress.XtraBars.Docking2010.Views.Widget;

namespace NDS20WinPlayer
{
    public partial class RegistPlayer : SplashScreen
    {
        private bool _serverConnected = false;
        readonly WebSocketClientConnection _fConnection = new NDSMain.NDSWebSocketClientConnection();

        //public delegate void DoWorkDelegate();
        public enum LoadStyle
        {
            OnLoad,
            OnShown,
            OnShownDoEvents
        }

        private LoadStyle _ls;

        public RegistPlayer(LoadStyle ls)
        {
            InitializeComponent();
            _ls = ls;
        }

        public bool PlayerRegistered { get; set; }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void RegistPlayer_Load(object sender, EventArgs e)
        {
            if (_ls == LoadStyle.OnLoad)
                ConnectWithServer();
        }
        private void RegistPlayer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_serverConnected) _fConnection.Close(WebSocketCloseCode.Normal);
        }

        private void btnCancelRegist_Click(object sender, EventArgs e)
        {
            PlayerRegistered = false;
            Close();
        }
         private void btnRequestRegist_Click(object sender, EventArgs e)
         {
             PlayerRegistered = true;
             AppInfoStrc.PlayerId = "1234";
             Close();
         }

        public void ConnectWithServer()
        {
            AssignWebSocket();
            //Create WebSocket client connection
            _serverConnected = _fConnection.Start(AppInfoStrc.UrlOfServer, AppInfoStrc.PortOfServer,
                AppInfoStrc.ExtentionOfServer, false);
            if (!_serverConnected)
            {
                LogFile.ThreadWriteLog("[NETWORK ERROR-등록]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 실패", LogType.LOG_ERROR);
            }
        }

        private void RegistPlayer_Shown(object sender, EventArgs e)
        {
            if (_ls == LoadStyle.OnShown)
                ConnectWithServer();
            else if (_ls == LoadStyle.OnShownDoEvents)
            {
                Application.DoEvents();
                Application.DoEvents();
                ConnectWithServer();
            }

        }
        private void AssignWebSocket()
        {
            _fConnection.ConnectionClose += ConnectionClose;
            _fConnection.ConnectionRead += ConnectionRead;
            _fConnection.ConnectionWrite += ConnectionWrite;
            _fConnection.ConnectionOpen += ConnectionOpen;
        }

        void ConnectionOpen(WebSocketConnection aConnection)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new WebSocketConnection.ConnectionEvent(ConnectionOpen), new Object[] { aConnection });
            }
            else
            {
                _serverConnected = true;
                LogFile.ThreadWriteLog("[NETWORK-등록]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 성공", LogType.LOG_INFO);
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
                _serverConnected = false;
                LogFile.ThreadWriteLog("[NETWORK-등록]:" + AppInfoStrc.UrlOfServer + AppInfoStrc.ExtentionOfServer + " Web socket 연결 종료", LogType.LOG_INFO);
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
                var inString = Encoding.UTF8.GetString(aData.ToArray());
                var outString = inString.Replace("\"", "'");
                LogFile.ThreadWriteLog(outString, LogType.LOG_INFO);
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

    }
    
}