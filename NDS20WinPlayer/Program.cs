using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;

namespace NDS20WinPlayer
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>

        private static void ShowChildForm(RegistPlayer.LoadStyle ls)
        {
            RegistPlayer registPlayer = new RegistPlayer(ls);
            registPlayer.ShowDialog();
        }

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");

            CommonFunctions.LoadIniFile();
            LogFile.ThreadWriteLog("====================NDS2.0 Player Opened!!====================", LogType.LOG_INFO);

            if (AppInfoStrc.PlayerId =="")  // 미등록 플레이어
            {
                ShowChildForm(RegistPlayer.LoadStyle.OnShownDoEvents);
            }

            if (AppInfoStrc.PlayerId != "")  // 등록된 플레이어
            {
                Application.Run(mainForm: new NDSMain());
            }
        }


    }
}
