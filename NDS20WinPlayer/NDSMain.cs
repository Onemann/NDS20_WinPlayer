using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Json;
using System.IO;
using System.Threading;

namespace NDS20WinPlayer
{

    public partial class NDSMain : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
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

            arrSubframe = new List<Subframe>();
            arrSchedule = new List<string>();

            assignScheule();

        }

        private void NDSMain_MouseDown(object sender, MouseEventArgs e)
        {
//#if DEBUG
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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
                    Application.Exit();
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
            LogFile.threadWriteLog("====================NDS2.0 Player Opened!!====================", LogType.LOG_INFO);
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            this.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.Location = new System.Drawing.Point(screenLeft, screenTop);

        }

        private void LoadIniFile()
        {
            var AppIniFile = new IniFile();

            AppInfoStrc.DirOfApplication = Environment.CurrentDirectory;
            AppInfoStrc.DirOfSchedule = AppIniFile.Read("DirOfSchedule", "PATH");
            AppInfoStrc.DirOfLog = AppIniFile.Read("DirOfLog", "PATH");           
            
        }
    }
}
