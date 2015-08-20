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
using System.Drawing;
using Declarations.Filters;
using LibVlcWrapper;
using System.Runtime.InteropServices;

namespace Implementation.Filters
{
   internal class CropFilter : ICropFilter
   {
      IntPtr _mHMediaPlayer;
      bool _mEnabled = false;
      
      public CropFilter(IntPtr hMediaPlayer)
      {
         _mHMediaPlayer = hMediaPlayer;
      }

      #region ICropFilter Members

      public bool Enabled
      {
         get
         {
            return _mEnabled;
         }
         set
         {
            _mEnabled = value;
            if (_mEnabled)
            {
               CropGeometry = CropArea.ToCropFilterString();
            }
            else
            {
               CropGeometry = null;
            }
         }
      }

      public Rectangle CropArea { get; set; }

      #endregion

      string CropGeometry
      {
         get
         {
            var pData = LibVlcMethods.libvlc_video_get_crop_geometry(_mHMediaPlayer);
            return Marshal.PtrToStringAnsi(pData);
         }
         set
         {
            LibVlcMethods.libvlc_video_set_crop_geometry(_mHMediaPlayer, value.ToUtf8());
         }
      }
   }
}
