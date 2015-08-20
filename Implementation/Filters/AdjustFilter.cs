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
using Declarations.Filters;
using LibVlcWrapper;

namespace Implementation.Filters
{
   internal class AdjustFilter : IAdjustFilter
   {
      IntPtr _mHMediaPlayer;

      public AdjustFilter(IntPtr hMediaPlayer)
      {
         _mHMediaPlayer = hMediaPlayer;
      }

      #region IAdjustFilter Members

      public bool Enabled
      {
         get
         {
            return GetVideoAdjustType<int>(LibvlcVideoAdjustOptionT.LibvlcAdjustEnable) == 1;
         }
         set
         {
            SetVideoAdjustType<int>(LibvlcVideoAdjustOptionT.LibvlcAdjustEnable, Convert.ToInt32(value));
         }
      }

      public float Contrast
      {
         get
         {
            return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustContrast);
         }
         set
         {
            SetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustContrast, value);
         }
      }

      public float Brightness
      {
         get
         {
            return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustBrightness);
         }
         set
         {
            SetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustBrightness, value);
         }
      }

      public int Hue
      {
         get
         {
            return GetVideoAdjustType<int>(LibvlcVideoAdjustOptionT.LibvlcAdjustEnable);
         }
         set
         {
            SetVideoAdjustType<int>(LibvlcVideoAdjustOptionT.LibvlcAdjustEnable, value);
         }
      }

      public float Saturation
      {
         get
         {
            return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustSaturation);
         }
         set
         {
            SetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustSaturation, value);
         }
      }

      public float Gamma
      {
         get
         {
            return GetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustGamma);
         }
         set
         {
            SetVideoAdjustType<float>(LibvlcVideoAdjustOptionT.LibvlcAdjustGamma, value);
         }
      }

      #endregion

      private void SetVideoAdjustType<T>(LibvlcVideoAdjustOptionT adjustType, T value)
      {
         if (typeof(int) == typeof(T))
         {
            LibVlcMethods.libvlc_video_set_adjust_int(_mHMediaPlayer, adjustType, (int)(object)value);
         }
         else
         {
            LibVlcMethods.libvlc_video_set_adjust_float(_mHMediaPlayer, adjustType, (float)(object)value);
         }
      }

      private T GetVideoAdjustType<T>(LibvlcVideoAdjustOptionT adjustType)
      {
         if (typeof(int) == typeof(T))
         {
            return (T)(object)LibVlcMethods.libvlc_video_get_adjust_int(_mHMediaPlayer, adjustType);
         }
         else
         {
            return (T)(object)LibVlcMethods.libvlc_video_get_adjust_float(_mHMediaPlayer, adjustType);
         }
      }
   }

}
