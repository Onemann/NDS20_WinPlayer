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
   internal class DeinterlaceFilter : IDeinterlaceFilter
   {
      IntPtr _mHMediaPlayer;
      bool _mEnabled = false;
      private DeinterlaceMode _mMode;

      public DeinterlaceFilter(IntPtr hMediaPlayer)
      {
         _mHMediaPlayer = hMediaPlayer;
      }

      #region IDeinterlaceFilter Members

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
                  LibVlcMethods.libvlc_video_set_deinterlace(_mHMediaPlayer, Mode.ToString().ToUtf8());
              }
              else
              {
                  LibVlcMethods.libvlc_video_set_deinterlace(_mHMediaPlayer, null);
              }
          }
      }

      public DeinterlaceMode Mode 
      {
          get
          {
              return _mMode;
          }
          set
          {
              _mMode = value;
              LibVlcMethods.libvlc_video_set_deinterlace(_mHMediaPlayer, _mMode.ToString().ToUtf8());
          }
      }

      #endregion
   }
}
