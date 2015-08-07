using System;
using System.Windows.Forms;

namespace NDS20WinPlayer
{
    class commonFunctions
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


    }

}