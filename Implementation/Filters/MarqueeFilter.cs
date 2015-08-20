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
using System.Runtime.InteropServices;

namespace Implementation.Filters
{
   internal class MarqueeFilter : IMarqueeFilter
   {
      IntPtr _mHMediaPlayer;

      public MarqueeFilter(IntPtr hMediaPlayer)
      {
         _mHMediaPlayer = hMediaPlayer;
      }

      #region IMarqueeFilter Members

      public bool Enabled
      {
         get
         {
            return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeEnable) == 1;
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeEnable, Convert.ToInt32(value));
         }
      }

      public string Text
      {
         get
         {
            return GetMarqueeString(LibvlcVideoMarqueeOptionT.LibvlcMarqueeText);
         }
         set
         {
            SetMarqueeString(LibvlcVideoMarqueeOptionT.LibvlcMarqueeText,value);
         }
      }

      public VlcColor Color
      {
         get
         {
            return (VlcColor)GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeColor);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeColor, (int)value);
         }
      }

      public Position Position
      {
         get
         {
            return (Position)GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueePosition);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueePosition, (int)value);
         }
      }

      public int Refresh
      {
         get
         {
            return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeRefresh);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeRefresh, value);
         }
      }

      public int Size
      {
         get
         {
            return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeSize);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeSize, value);
         }
      }

      public int Timeout
      {
         get
         {
            return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeTimeout);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeTimeout, value);
         }
      }

      public int X
      {
         get
         {
            return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeX);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeX, value);
         }
      }

      public int Y
      {
         get
         {
            return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeY);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeY, value);
         }
      }

      public int Opacity
      {
         get
         {
            return GetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeOpacity);
         }
         set
         {
            SetMarquee(LibvlcVideoMarqueeOptionT.LibvlcMarqueeOpacity, value);
         }
      }

      #endregion

      int GetMarquee(LibvlcVideoMarqueeOptionT option)
      {
         return LibVlcMethods.libvlc_video_get_marquee_int(_mHMediaPlayer, option);
      }

      void SetMarquee(LibvlcVideoMarqueeOptionT option, int argument)
      {
         LibVlcMethods.libvlc_video_set_marquee_int(_mHMediaPlayer, option, argument);
      }

      string GetMarqueeString(LibvlcVideoMarqueeOptionT option)
      {
         var pData = LibVlcMethods.libvlc_video_get_marquee_string(_mHMediaPlayer, option);
         return Marshal.PtrToStringAnsi(pData);
      }

      void SetMarqueeString(LibvlcVideoMarqueeOptionT option, string argument)
      {
         LibVlcMethods.libvlc_video_set_marquee_string(_mHMediaPlayer, option, argument.ToUtf8());
      }    
   }
}
