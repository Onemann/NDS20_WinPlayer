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
using System.Linq;
using System.Text;
using Declarations;
using LibVlcWrapper;
using Declarations.Players;
using Declarations.Enums;
using System.Runtime.InteropServices;

namespace Implementation.Players
{
    internal class DiskPlayer : VideoPlayer, IDiskPlayer
    {
        public DiskPlayer(IntPtr hMediaLib)
            : base(hMediaLib)
        {

        }

        public int AudioTrack
        {
            get
            {
                return LibVlcMethods.libvlc_audio_get_track(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_audio_set_track(MHMediaPlayer, value);
            }
        }

        public int AudioTrackCount
        {
            get
            {
                return LibVlcMethods.libvlc_audio_get_track_count(MHMediaPlayer);
            }
        }

        public IEnumerable<TrackDescription> AudioTracksInfo
        {
            get
            {
                var trackInfo = LibVlcMethods.libvlc_audio_get_track_description(MHMediaPlayer);
                return GetDescription(trackInfo);
            }
        }

        public IEnumerable<TrackDescription> VideoTracksInfo
        {
            get
            {
                var trackInfo = LibVlcMethods.libvlc_video_get_track_description(MHMediaPlayer);
                return GetDescription(trackInfo);
            }
        }

        public IEnumerable<TrackDescription> SubtitleTracksInfo
        {
            get
            {
                var trackInfo = LibVlcMethods.libvlc_video_get_spu_description(MHMediaPlayer);
                return GetDescription(trackInfo);
            }
        }

        public IEnumerable<TrackDescription> TitleInfo
        {
            get
            {
                var trackInfo = LibVlcMethods.libvlc_video_get_title_description(MHMediaPlayer);
                return GetDescription(trackInfo);
            }
        }

        public IEnumerable<TrackDescription> GetChapterDescription(int title)
        {
            var trackInfo = LibVlcMethods.libvlc_video_get_chapter_description(MHMediaPlayer, title);
            return GetDescription(trackInfo);
        }

        private IEnumerable<TrackDescription> GetDescription(IntPtr trackInfo)
        {
            if (trackInfo == IntPtr.Zero)
            {
                yield break;
            }
                
            var trackDesc = (LibvlcTrackDescriptionT)Marshal.PtrToStructure(trackInfo, typeof(LibvlcTrackDescriptionT));
            do
            {
                yield return new TrackDescription()
                {
                    Id = trackDesc.i_id,
                    Name = Marshal.PtrToStringAnsi(trackDesc.psz_name)
                };

                if (trackDesc.p_next != IntPtr.Zero)
                {
                    trackDesc = (LibvlcTrackDescriptionT)Marshal.PtrToStructure(trackDesc.p_next, typeof(LibvlcTrackDescriptionT));
                }
                else
                {
                    break;
                }
            }
            while (true);
            LibVlcMethods.libvlc_track_description_release(trackInfo);
        }
        
        public int SubTitle
        {
            get
            {
                return LibVlcMethods.libvlc_video_get_spu(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_video_set_spu(MHMediaPlayer, value);
            }
        }

        public int SubTitleCount
        {
            get
            {
                return LibVlcMethods.libvlc_video_get_spu_count(MHMediaPlayer);
            }
        }

        public void NextChapter()
        {
            LibVlcMethods.libvlc_media_player_next_chapter(MHMediaPlayer);
        }

        public void PreviousChapter()
        {
            LibVlcMethods.libvlc_media_player_previous_chapter(MHMediaPlayer);
        }

        public int Title
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_title(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_media_player_set_title(MHMediaPlayer, value);
            }
        }

        public int TitleCount
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_title_count(MHMediaPlayer);
            }
        }

        public int GetChapterCountForTitle(int title)
        {
            return LibVlcMethods.libvlc_media_player_get_chapter_count_for_title(MHMediaPlayer, Title);
        }

        public int ChapterCount
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_chapter_count(MHMediaPlayer);
            }
        }

        public int Chapter
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_chapter(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_media_player_set_chapter(MHMediaPlayer, value);
            }
        }

        public int VideoTrackCount
        {
            get
            {
                return LibVlcMethods.libvlc_video_get_track_count(MHMediaPlayer);
            }
        }

        public int VideoTrack
        {
            get
            {
                return LibVlcMethods.libvlc_video_get_track(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_video_set_track(MHMediaPlayer, value);
            }
        }

        public void Navigate(NavigationMode mode)
        {
            LibVlcMethods.libvlc_media_player_navigate(MHMediaPlayer, (LibvlcNavigateModeT)mode);
        }
    }
}
