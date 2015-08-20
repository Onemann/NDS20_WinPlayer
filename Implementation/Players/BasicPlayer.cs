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
using LibVlcWrapper;
using Implementation.Events;

namespace Implementation.Players
{
    internal class BasicPlayer : DisposableBase, IPlayer, IEventProvider, IReferenceCount, INativePointer
    {
        protected IntPtr MHMediaPlayer;
        protected IntPtr MHMediaLib;
        private readonly EventBroker _mEvents;
        IntPtr _mHEventManager = IntPtr.Zero;
        protected IMedia MCurrentMedia;

        public BasicPlayer(IntPtr hMediaLib)
        {
            MHMediaLib = hMediaLib;
            MHMediaPlayer = LibVlcMethods.libvlc_media_player_new(MHMediaLib);
            AddRef();
            _mEvents = new EventBroker(this);
        }

        #region IPlayer Members

        public virtual void Open(IMedia media)
        {
            MCurrentMedia = media;
            LibVlcMethods.libvlc_media_player_set_media(MHMediaPlayer, ((INativePointer)media).Pointer);
        }

        public virtual void Play()
        {
            LibVlcMethods.libvlc_media_player_play(MHMediaPlayer);
        }

        public void Pause()
        {
            LibVlcMethods.libvlc_media_player_pause(MHMediaPlayer);
        }

        public void Stop()
        {
            LibVlcMethods.libvlc_media_player_stop(MHMediaPlayer);
        }

        public long Time
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_time(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_media_player_set_time(MHMediaPlayer, value);
            }
        }

        public float Position
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_position(MHMediaPlayer);
            }
            set
            {
                LibVlcMethods.libvlc_media_player_set_position(MHMediaPlayer, value);
            }
        }

        public long Length
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_get_length(MHMediaPlayer);
            }
        }

        public IEventBroker Events
        {
            get
            {
                return _mEvents;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return LibVlcMethods.libvlc_media_player_is_playing(MHMediaPlayer) == 1;
            }
        }

        public IMedia CurrentMedia
        {
            get
            {
                return MCurrentMedia;
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        #region IEventProvider Members

        public IntPtr EventManagerHandle
        {
            get
            {
                if (_mHEventManager == IntPtr.Zero)
                {
                    _mHEventManager = LibVlcMethods.libvlc_media_player_event_manager(MHMediaPlayer);
                }

                return _mHEventManager;
            }
        }

        #endregion

        #region IReferenceCount Members

        public void AddRef()
        {
            LibVlcMethods.libvlc_media_player_retain(MHMediaPlayer);
        }

        public void Release()
        {
            try
            {
                LibVlcMethods.libvlc_media_player_release(MHMediaPlayer);
            }
            catch (Exception)
            { }
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

        #region INativePointer Members

        public IntPtr Pointer
        {
            get { return MHMediaPlayer; }
        }

        #endregion

        public override bool Equals(object obj)
        {
            return this.Equals((IPlayer)obj, this);
        }

        public override int GetHashCode()
        {
            return MHMediaPlayer.GetHashCode();
        } 
    }
}
