using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
         private void btnCancelRegist_Click(object sender, EventArgs e)
        {
            PlayerRegistered = false;
            Close();
        }

        public void ConnectWithServer()
        {

            WebSocketClientConnection fConnection = new NDSMain.NDSWebSocketClientConnection();
            //Create WebSocket client connection
            _serverConnected = fConnection.Start(AppInfoStrc.UrlOfServer, AppInfoStrc.PortOfServer,
                AppInfoStrc.ExtentionOfServer, false);
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
    }
    
}