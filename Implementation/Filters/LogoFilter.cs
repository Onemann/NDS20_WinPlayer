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
using Declarations;
using Declarations.Filters;
using LibVlcWrapper;

namespace Implementation.Filters
{
   internal class LogoFilter : ILogoFilter
   {
      IntPtr _mPMediaPlayer;
      private string _mFile;
      
      public LogoFilter(IntPtr hMediaPlayer)
      {
         _mPMediaPlayer = hMediaPlayer;
      }

      #region ILogoFilter Members

      public bool Enabled
      {
         get
         {
            return LibVlcMethods.libvlc_video_get_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoEnable) == 1;
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoEnable, Convert.ToInt32(value));
         }
      }

      public string File
      {
         get
         {
            return _mFile;
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_string(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoFile, value.ToUtf8());
            _mFile = value;
         }
      }

      public int X
      {
         get
         {
            return LibVlcMethods.libvlc_video_get_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoX);
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoX, value);
         }
      }

      public int Y
      {
         get
         {
            return LibVlcMethods.libvlc_video_get_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoY);
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoY, value);
         }
      }

      public int Delay
      {
         get
         {
            return LibVlcMethods.libvlc_video_get_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoDelay);
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoDelay, value);
         }
      }

      public int Repeat
      {
         get
         {
            return LibVlcMethods.libvlc_video_get_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoRepeat);
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoRepeat, value);
         }
      }

      public int Opacity
      {
         get
         {
            return LibVlcMethods.libvlc_video_get_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoOpacity);
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoOpacity, value);
         }
      }

      public Position Position
      {
         get
         {
            return (Position)LibVlcMethods.libvlc_video_get_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoPosition);
         }
         set
         {
            LibVlcMethods.libvlc_video_set_logo_int(_mPMediaPlayer, LibvlcVideoLogoOptionT.LibvlcLogoPosition, (int)value);
         }
      }

      #endregion
   }
}
