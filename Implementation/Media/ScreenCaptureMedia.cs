//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Declarations.Media;

namespace Implementation.Media
{
   internal class ScreenCaptureMedia : BasicMedia, IScreenCaptureMedia
   {
      private Rectangle _mCaptureArea;
      private int _mFps;
      private bool _mFollowMouse = false;
      private string _mCursorFile;

      public ScreenCaptureMedia(IntPtr hMediaLib)
         : base(hMediaLib)
      {
         _mCaptureArea = Screen.PrimaryScreen.Bounds;
         _mFps = 1;
      }

      #region IScreenCaptureMedia Members

      public Rectangle CaptureArea
      {
         get
         {
            return _mCaptureArea;
         }
         set
         {
            _mCaptureArea = value;
            UpdateCaptureArea();
         }
      }

      private void UpdateCaptureArea()
      {
         var options = new List<string>()
         {
            string.Format(":screen-top={0}", _mCaptureArea.Top),
            string.Format(":screen-left={0}", _mCaptureArea.Left),
            string.Format(":screen-width={0}", _mCaptureArea.Width),
            string.Format(":screen-height={0}", _mCaptureArea.Height)
         };

         AddOptions(options);
      }

      public int Fps
      {
         get
         {
            return _mFps;
         }
         set
         {
            _mFps = value;
            UpdateFrameRate();
         }
      }

      private void UpdateFrameRate()
      {
         var options = new List<string>()
         {
            string.Format(":screen-fps={0}", _mFps)
         };

         AddOptions(options);
      }

      private void UpdateFollowMouse()
      {
         var options = new List<string>()
         {
            string.Format(":screen-follow-mouse={0}", _mFollowMouse.ToString())
         };

         AddOptions(options);
      }

      public bool FollowMouse
      {
         get
         {
            return _mFollowMouse;
         }
         set
         {
            _mFollowMouse = value;
            UpdateFollowMouse();
         }
      }

      public string CursorFile
      {
         get
         {
            return _mCursorFile;
         }
         set
         {
            _mCursorFile = value;
            UpdateCursorImage();
         }
      }

      private void UpdateCursorImage()
      {
         var options = new List<string>()
         {
            string.Format(":screen-mouse-image={0}", _mCursorFile)
         };

         AddOptions(options);      
      }

      #endregion
   }
}
