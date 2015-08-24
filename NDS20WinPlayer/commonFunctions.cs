using System;
using System.Windows.Forms;

namespace NDS20WinPlayer
{
    class CommonFunctions
    {
        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }
            return null;
        }

        public static void LoadIniFile()
        {
            var appIniFile = new IniFile();

            #region Dir Path
            AppInfoStrc.DirOfApplication = Environment.CurrentDirectory;

            AppInfoStrc.DirOfSchedule = appIniFile.Read("DirOfSchedule", "PATH");
            if (AppInfoStrc.DirOfSchedule == "")
            {
                appIniFile.Write("DirOfSchedule", InitValue.InitDirOfSchedule, "PATH");
                AppInfoStrc.DirOfSchedule = InitValue.InitDirOfSchedule;
            }

            AppInfoStrc.DirOfLog = appIniFile.Read("DirOfLog", "PATH");
            if (AppInfoStrc.DirOfLog == "")
            {
                appIniFile.Write("DirOfLog", InitValue.InitDirOfLog, "PATH");
                AppInfoStrc.DirOfLog = InitValue.InitDirOfLog;
            }
            #endregion

            #region Server connectoion Info
            AppInfoStrc.UrlOfServer = appIniFile.Read("UrlOfServer", "SERVER");
            if (AppInfoStrc.UrlOfServer == "")
            {
                appIniFile.Write("UrlOfServer", InitValue.InitUrlOfServer, "SERVER");
                AppInfoStrc.UrlOfServer = InitValue.InitUrlOfServer;
            }

            AppInfoStrc.ExtentionOfServer = appIniFile.Read("ExtentionOfServer", "SERVER");
            if (AppInfoStrc.ExtentionOfServer == "")
            {
                appIniFile.Write("ExtentionOfServer", InitValue.InitExtensionOfServer, "SERVER");
                AppInfoStrc.ExtentionOfServer = InitValue.InitExtensionOfServer;
            }

            AppInfoStrc.PortOfServer = appIniFile.Read("PortOfServer", "SERVER");
            if (AppInfoStrc.PortOfServer == "")
            {
                appIniFile.Write("PortOfServer", InitValue.InitPortOfServer, "SERVER");
                AppInfoStrc.PortOfServer = InitValue.InitPortOfServer;
            }
            #endregion

            #region Player Info
            AppInfoStrc.PlayerId = appIniFile.Read("PlayerID", "PLAYER");
            if (AppInfoStrc.PlayerId == "")
            {
                appIniFile.Write("PlayerID", "", "PLAYER");
            }
            #endregion

        }

    }

}