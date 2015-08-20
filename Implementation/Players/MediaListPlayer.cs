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
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation.Events;
using LibVlcWrapper;

namespace Implementation.Players
{
    internal class MediaListPlayer : DisposableBase, IMediaListPlayer, IEventProvider
    {
        private IntPtr _mHMediaListPlayer = IntPtr.Zero;
        private IDiskPlayer _mVideoPlayer;
        private IMediaList _mMediaList;
        private PlaybackMode _mPlaybackMode = PlaybackMode.Default;
        IntPtr _mHEventManager = IntPtr.Zero;
        IMediaListPlayerEvents _mMediaListEvents = null;

        public MediaListPlayer(IntPtr hMediaLib, IMediaList mediaList)
        {
            _mMediaList = mediaList;
            _mHMediaListPlayer = LibVlcMethods.libvlc_media_list_player_new(hMediaLib);
            LibVlcMethods.libvlc_media_list_player_set_media_list(_mHMediaListPlayer, ((INativePointer)_mMediaList).Pointer);
            _mMediaList.Dispose();

            _mVideoPlayer = new DiskPlayer(hMediaLib);
            LibVlcMethods.libvlc_media_list_player_set_media_player(_mHMediaListPlayer, ((INativePointer)_mVideoPlayer).Pointer);
            _mVideoPlayer.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (_mVideoPlayer != null)
            {
                _mVideoPlayer.Dispose();
                _mVideoPlayer = null;
            }
            LibVlcMethods.libvlc_media_list_player_release(_mHMediaListPlayer);
        }

        #region IMediaListPlayer Members

        public void PlayNext()
        {
            LibVlcMethods.libvlc_media_list_player_next(_mHMediaListPlayer);
        }

        public void PlayPrevios()
        {
            LibVlcMethods.libvlc_media_list_player_previous(_mHMediaListPlayer);
        }

        public PlaybackMode PlaybackMode
        {
            get
            {
                return _mPlaybackMode;
            }
            set
            {
                LibVlcMethods.libvlc_media_list_player_set_playback_mode(_mHMediaListPlayer, (LibvlcPlaybackModeT)value);
                _mPlaybackMode = value;
            }
        }

        public void PlayItemAt(int index)
        {
            LibVlcMethods.libvlc_media_list_player_play_item_at_index(_mHMediaListPlayer, index);
        }

        public MediaState PlayerState
        {
            get
            {
                return (MediaState)LibVlcMethods.libvlc_media_list_player_get_state(_mHMediaListPlayer);
            }
        }

        public IDiskPlayer InnerPlayer
        {
            get
            {
                return _mVideoPlayer;
            }
        }

        #endregion

        #region INativePointer Members

        public IntPtr Pointer
        {
            get
            {
                return _mHMediaListPlayer;
            }
        }

        #endregion

        #region IPlayer Members

        public void Play()
        {
            LibVlcMethods.libvlc_media_list_player_play(_mHMediaListPlayer);
        }

        public void Pause()
        {
            LibVlcMethods.libvlc_media_list_player_pause(_mHMediaListPlayer);
        }

        public void Stop()
        {
            LibVlcMethods.libvlc_media_list_player_stop(_mHMediaListPlayer);
        }

        public void Open(IMedia media)
        {
            _mVideoPlayer.Open(media);
        }

        public long Time
        {
            get
            {
                return _mVideoPlayer.Time;
            }
            set
            {
                _mVideoPlayer.Time = value;
            }
        }

        public float Position
        {
            get
            {
                return _mVideoPlayer.Position;
            }
            set
            {
                _mVideoPlayer.Position = value;
            }
        }

        public long Length
        {
            get
            {
                return _mVideoPlayer.Length;
            }
        }

        public IEventBroker Events
        {
            get
            {
                return _mVideoPlayer.Events;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return _mVideoPlayer.IsPlaying;
            }
        }

        public IMedia CurrentMedia
        {
            get
            {
                return _mVideoPlayer.CurrentMedia;
            }
        }

        #endregion

        #region IEventProvider Members

        public IntPtr EventManagerHandle
        {
            get
            {
                if (_mHEventManager == IntPtr.Zero)
                {
                    _mHEventManager = LibVlcMethods.libvlc_media_list_player_event_manager(_mHMediaListPlayer);
                }

                return _mHEventManager;
            }
        }

        #endregion

        #region IMediaListPlayer Members

        public IMediaListPlayerEvents MediaListPlayerEvents
        {
            get
            {
                if (_mMediaListEvents == null)
                {
                    _mMediaListEvents = new MediaListPlayerEventManager(this);
                }
                return _mMediaListEvents;
            }
        }

        #endregion

        #region IEqualityComparer<IPlayer> Members

        public bool Equals(IPlayer x, IPlayer y)
        {
            var x1 = (INativePointer)x;
            var y1 = (INativePointer)y;

            return x1.Pointer == y1.Pointer;
        }

        public int GetHashCode(IPlayer obj)
        {
            return ((INativePointer)obj).Pointer.GetHashCode();
        }

        #endregion
    }
}
