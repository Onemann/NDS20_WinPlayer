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

using Declarations;
using Declarations.Filters;
using Declarations.Players;
using Implementation.Exceptions;
using Implementation.Filters;
using Implementation.Utils;
using LibVlcWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;

namespace Implementation.Players
{
    internal class VideoPlayer : AudioPlayer, IVideoPlayer
    {
        MemoryRenderer _mMemRender = null;
        MemoryRendererEx _mMemRenderEx = null;
        IAdjustFilter _mAdjust = null;
        ILogoFilter _mLogo = null;
        IMarqueeFilter _mMarquee = null;
        ICropFilter _mCrop = null;
        IDeinterlaceFilter _mDeinterlace = null;

        bool _mKeyInputEnabled = true;
        bool _mMouseInputEnabled = true;
        Dictionary<string, Enum> _mAspectMapper;

        public VideoPlayer(IntPtr hMediaLib)
            : base(hMediaLib)
        {
            _mAspectMapper = EnumUtils.GetEnumMapping(typeof(AspectRatioMode));
        }

        public override void Play()
        {
            base.Play();
            if (_mMemRender != null)
            {
                _mMemRender.StartTimer();
            }

            if (_mMemRenderEx != null)
            {
                _mMemRenderEx.StartTimer();
            }
        }

        #region IVideoPlayer Members

        public IntPtr WindowHandle
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_hwnd(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_media_player_set_hwnd(MHMediaPlayer, value);
            }
        }

        public void TakeSnapShot(uint stream, string path)
        {
            LibVlcMethods.libvlc_video_take_snapshot(MHMediaPlayer, stream, path.ToUtf8(), 0, 0);
        }

        public float PlaybackRate
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_rate(MHMediaPlayer);
            }
            set
            {
                var res = LibVlcMethods.libvlc_media_player_set_rate(MHMediaPlayer, value);
                if (res == -1)
                {
                    throw new LibVlcException();
                }
            }
        }

        public float Fps
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_fps(MHMediaPlayer);
            }
        }

        public void NextFrame()
        {
            LibVlcMethods.libvlc_media_player_next_frame(MHMediaPlayer);
        }

        public Size GetVideoSize(uint stream)
        {
            uint width = 0, height = 0;
            LibVlcMethods.libvlc_video_get_size(MHMediaPlayer, stream, out width, out height);
            return new Size((int)width, (int)height);
        }

        public Point GetCursorPosition(uint stream)
        {
            int px = 0, py = 0;
            LibVlcMethods.libvlc_video_get_cursor(MHMediaPlayer, stream, out px, out py);
            return new Point(px, py);
        }

        public bool KeyInputEnabled
        {
            get
            {
                return _mKeyInputEnabled;
            }
            set
            {
                LibVlcMethods.libvlc_video_set_key_input(MHMediaPlayer, value);
                _mKeyInputEnabled = value;
            }
        }

        public bool MouseInputEnabled
        {
            get
            {
                return _mMouseInputEnabled;
            }
            set
            {
                LibVlcMethods.libvlc_video_set_mouse_input(MHMediaPlayer, value);
                _mMouseInputEnabled = value;
            }
        }

        public float VideoScale
        {
            get
            {
                return LibVlcMethods.libvlc_video_get_scale(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_video_set_scale(MHMediaPlayer, value);
            }
        }

        public AspectRatioMode AspectRatio
        {
            get
            {
                var pData = LibVlcMethods.libvlc_video_get_aspect_ratio(MHMediaPlayer);
                var str = Marshal.PtrToStringAnsi(pData);
                return (AspectRatioMode)_mAspectMapper[str];
            }
            set
            {
                var val = EnumUtils.GetEnumDescription(value);
                LibVlcMethods.libvlc_video_set_aspect_ratio(MHMediaPlayer, val.ToUtf8());
            }
        }

        public void SetSubtitleFile(string path)
        {
            LibVlcMethods.libvlc_video_set_subtitle_file(MHMediaPlayer, path.ToUtf8());
        }

        public int Teletext
        {
            get
            {
                return LibVlcMethods.libvlc_video_get_teletext(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_video_set_teletext(MHMediaPlayer, value);
            }
        }

        public void ToggleTeletext()
        {
            LibVlcMethods.libvlc_toggle_teletext(MHMediaPlayer);
        }

        public bool PlayerWillPlay
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_will_play(MHMediaPlayer);
            }
        }

        public int VoutCount
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_has_vout(MHMediaPlayer);
            }
        }

        public bool IsSeekable
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_is_seekable(MHMediaPlayer);
            }
        }

        public bool CanPause
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_can_pause(MHMediaPlayer);
            }
        }

        public ICropFilter CropGeometry
        {
            get
            {
                if (_mCrop == null)
                {
                    _mCrop = new CropFilter(MHMediaPlayer);
                }

                return _mCrop;
            }
        }

        public IAdjustFilter Adjust
        {
            get
            {
                if (_mAdjust == null)
                {
                    _mAdjust = new AdjustFilter(MHMediaPlayer);
                }

                return _mAdjust;
            }
        }

        public IMemoryRenderer CustomRenderer
        {
            get
            {
                if (_mMemRenderEx != null)
                {
                    throw new InvalidOperationException("CustomRenderer is mutually exclusive with CustomRendererEx");
                }

                if (_mMemRender == null)
                {
                    _mMemRender = new MemoryRenderer(MHMediaPlayer);                 
                }
                return _mMemRender;
            }
        }

        void Events_PlayerPlaying(object sender, EventArgs e)
        {
            try
            {
                var tracksInfo = MCurrentMedia.TracksInfoEx;
                var video = tracksInfo.FirstOrDefault(t => t.TrackType == TrackType.Video) as VideoTrack;
                if (video != null && _mMemRenderEx != null)
                    _mMemRenderEx.Sar = new AspectRatio((int)video.SarNum, (int)video.SarDen);
            }
            catch (EntryPointNotFoundException)
            {
                Events.PlayerPlaying -= Events_PlayerPlaying;
            }
        }

        public IMemoryRendererEx CustomRendererEx
        {
            get
            {
                if (_mMemRender != null)
                {
                    throw new InvalidOperationException("CustomRendererEx is mutually exclusive with CustomRenderer");
                }

                if (_mMemRenderEx == null)
                {
                    _mMemRenderEx = new MemoryRendererEx(MHMediaPlayer);
                    Events.PlayerPlaying += Events_PlayerPlaying;
                }
                return _mMemRenderEx;
            }
        }

        public ILogoFilter Logo
        {
            get
            {
                if (_mLogo == null)
                {
                    _mLogo = new LogoFilter(MHMediaPlayer);
                }
                return _mLogo;
            }
        }

        public IMarqueeFilter Marquee
        {
            get
            {
                if (_mMarquee == null)
                {
                    _mMarquee = new MarqueeFilter(MHMediaPlayer);
                }
                return _mMarquee;
            }
        }

        public IDeinterlaceFilter Deinterlace
        {
            get
            {
                if (_mDeinterlace == null)
                {
                    _mDeinterlace = new DeinterlaceFilter(MHMediaPlayer);
                }
                return _mDeinterlace;
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (_mMemRender != null)
            {
                _mMemRender.Dispose();
                _mMemRender = null;
            }

            if (_mMemRenderEx != null)
            {
                _mMemRenderEx.Dispose();
                _mMemRenderEx = null;
                Events.PlayerPlaying -= Events_PlayerPlaying;
            }

            base.Dispose(disposing);
        }
    }
}
