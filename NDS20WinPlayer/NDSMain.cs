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
            arrSubframe = new List<Subframe>();
            arrSchedule = new List<string>();

            assignScheule();
        }

        private void NDSMain_MouseDown(object sender, MouseEventArgs e)
        {
#if DEBUG
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
#endif
        }

        // open new sub frame with JSON frame parameter 
        private void openSubframe(JsonObject jsonFrame)
        {
            Subframe newSubframe = new Subframe(jsonFrame);
            newSubframe.TopLevel = false;
            newSubframe.Parent = this;
            newSubframe.BackColor = Color.Blue;
            newSubframe.Show();
        }

        private void NDSMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
#if DEBUG
                case Keys.F3:
                    initMainScreen();
                    drawSubFrame();
                    break;

#endif
                case Keys.F7:

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
                " \"xPos\": 100," +
                " \"yPos\": 0," +
                " \"hLen\": 360," +
                " \"vLen\": 200," +
                " \"fileName\": \"d:/Projects/NDS/Contents/A.avi\"," +
                " \"volume\": 0 " +
                "}"
                );

            arrSchedule.Add(
                "{" +
                " \"xPos\": 400," +
                " \"yPos\": 100," +
                " \"hLen\": 500," +
                " \"vLen\": 280," +
                " \"fileName\": \"d:/Projects/NDS/Contents/A.tp\"," +
                " \"volume\": 0 " +
                "}"
                );

            arrSchedule.Add(
                "{" +
                " \"xPos\": 200," +
                " \"yPos\": 300," +
                " \"hLen\": 500," +
                " \"vLen\": 350," +
                " \"fileName\": \"d:/Projects/NDS/Contents/A.wmv\"," +
                " \"volume\": 100 " +
                "}"
                );
        }

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
    }
        #endregion
}
