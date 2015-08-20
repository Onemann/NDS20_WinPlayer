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
using System.Runtime.InteropServices;
using Declarations;
using Declarations.Media;
using LibVlcWrapper;
using Implementation.Exceptions;
using System.Collections.Generic;

namespace Implementation.Media
{
    internal class MediaFromFile : BasicMedia, IMediaFromFile
    {
        public MediaFromFile(IntPtr hMediaLib)
            : base(hMediaLib)
        {

        }

        public override string Input
        {
            get
            {
                return MPath;
            }
            set
            {
                MPath = value;
                MHMedia = LibVlcMethods.libvlc_media_new_path(MHMediaLib, MPath.ToUtf8());
            }
        }

        public string GetMetaData(MetaDataType dataType)
        {
            var pData = LibVlcMethods.libvlc_media_get_meta(MHMedia, (LibvlcMetaT)dataType);
            return Marshal.PtrToStringAnsi(pData);
        }

        public void SetMetaData(MetaDataType dataType, string argument)
        {
            LibVlcMethods.libvlc_media_set_meta(MHMedia, (LibvlcMetaT)dataType, argument.ToUtf8());
        }

        public void SaveMetaData()
        {
            LibVlcMethods.libvlc_media_save_meta(MHMedia);
        }

        public long Duration
        {
            get
            {
                return LibVlcMethods.libvlc_media_get_duration(MHMedia);
            }
        }

        [Obsolete]
        public MediaTrackInfo[] TracksInfo
        {
            get
            {
                var pTr = IntPtr.Zero;
                var num = LibVlcMethods.libvlc_media_get_tracks_info(MHMedia, out pTr);

                if (num == 0 || pTr == IntPtr.Zero)
                {
                    throw new LibVlcException();
                }

                var size = Marshal.SizeOf(typeof(LibvlcMediaTrackInfoT));
                var tracks = new LibvlcMediaTrackInfoT[num];
                for (var i = 0; i < num; i++)
                {
                    tracks[i] = (LibvlcMediaTrackInfoT)Marshal.PtrToStructure(pTr, typeof(LibvlcMediaTrackInfoT));
                    pTr = new IntPtr(pTr.ToInt64() + size);
                }

                var mtis = new MediaTrackInfo[num];
                for (var i = 0; i < num; i++)
                {
                    mtis[i] = tracks[i].ToMediaInfo();
                }

                return mtis;
            }
        }
    }
}
