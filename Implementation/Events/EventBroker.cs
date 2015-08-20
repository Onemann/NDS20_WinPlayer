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
using Declarations.Events;
using Implementation.Media;
using LibVlcWrapper;
using MediaPlayerLengthChanged = Declarations.Events.MediaPlayerLengthChanged;
using MediaPlayerMediaChanged = Declarations.Events.MediaPlayerMediaChanged;
using MediaPlayerPausableChanged = Declarations.Events.MediaPlayerPausableChanged;
using MediaPlayerPositionChanged = Declarations.Events.MediaPlayerPositionChanged;
using MediaPlayerSeekableChanged = Declarations.Events.MediaPlayerSeekableChanged;
using MediaPlayerSnapshotTaken = Declarations.Events.MediaPlayerSnapshotTaken;
using MediaPlayerTimeChanged = Declarations.Events.MediaPlayerTimeChanged;
using MediaPlayerTitleChanged = Declarations.Events.MediaPlayerTitleChanged;

namespace Implementation.Events
{
    internal class EventBroker : EventManager, IEventBroker
    {
        public EventBroker(IEventProvider eventProvider)
            : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaPlayerTimeChanged:
                    RaiseTimeChanged(libvlcEvent.MediaDescriptor.media_player_time_changed.new_time);
                    break;

                case LibvlcEventE.LibvlcMediaPlayerEndReached:
                    RaiseMediaEnded();
                    break;

                case LibvlcEventE.LibvlcMediaPlayerMediaChanged:
                    if (MMediaChanged != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_player_media_changed.new_media, ReferenceCountAction.AddRef);
                        MMediaChanged(MEventProvider, new MediaPlayerMediaChanged(media));
                        //media.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerNothingSpecial:
                    if (MNothingSpecial != null)
                    {
                        MNothingSpecial(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerOpening:
                    if (MPlayerOpening != null)
                    {
                        MPlayerOpening(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerBuffering:
                    if (MPlayerBuffering != null)
                    {
                        MPlayerBuffering(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPlaying:
                    if (MPlayerPlaying != null)
                    {
                        MPlayerPlaying(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPaused:
                    if (MPlayerPaused != null)
                    {
                        MPlayerPaused(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerStopped:
                    if (MPlayerStopped != null)
                    {
                        MPlayerStopped(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerForward:
                    if (MPlayerForward != null)
                    {
                        MPlayerForward(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerBackward:
                    if (MPlayerPaused != null)
                    {
                        MPlayerPaused(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerEncounteredError:
                    if (MPlayerEncounteredError != null)
                    {
                        MPlayerEncounteredError(MEventProvider, EventArgs.Empty);
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPositionChanged:
                    if (MPlayerPositionChanged != null)
                    {
                        MPlayerPositionChanged(MEventProvider, new MediaPlayerPositionChanged(libvlcEvent.MediaDescriptor.media_player_position_changed.new_position));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerSeekableChanged:
                    if (MPlayerSeekableChanged != null)
                    {
                        MPlayerSeekableChanged(MEventProvider, new MediaPlayerSeekableChanged(libvlcEvent.MediaDescriptor.media_player_seekable_changed.new_seekable));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerPausableChanged:
                    if (MPlayerPausableChanged != null)
                    {
                        MPlayerPausableChanged(MEventProvider, new MediaPlayerPausableChanged(libvlcEvent.MediaDescriptor.media_player_pausable_changed.new_pausable));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerTitleChanged:
                    if (MPlayerTitleChanged != null)
                    {
                        MPlayerTitleChanged(MEventProvider, new MediaPlayerTitleChanged(libvlcEvent.MediaDescriptor.media_player_title_changed.new_title));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerSnapshotTaken:
                    if (MPlayerSnapshotTaken != null)
                    {
                        MPlayerSnapshotTaken(MEventProvider, new MediaPlayerSnapshotTaken(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.media_player_snapshot_taken.psz_filename)));
                    }
                    break;

                case LibvlcEventE.LibvlcMediaPlayerLengthChanged:
                    if (MPlayerLengthChanged != null)
                    {
                        MPlayerLengthChanged(MEventProvider, new MediaPlayerLengthChanged(libvlcEvent.MediaDescriptor.media_player_length_changed.new_length));
                    }
                    break;
            }
        }

        private void RaiseTimeChanged(long p)
        {
            if (MTimeChanged != null)
            {
                MTimeChanged(MEventProvider, new MediaPlayerTimeChanged(p));
            }
        }

        internal void RaiseMediaEnded()
        {
            if (MMediaEnded != null)
            {
                MMediaEnded.BeginInvoke(MEventProvider, EventArgs.Empty, null, null);
            }
        }

        private event EventHandler<MediaPlayerTimeChanged> MTimeChanged;
        public event EventHandler<MediaPlayerTimeChanged> TimeChanged
        {
            add
            {
                if (MTimeChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerTimeChanged);
                }
                MTimeChanged += value;
            }
            remove
            {
                if (MTimeChanged != null)
                {
                    MTimeChanged -= value;
                    if (MTimeChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerTimeChanged);
                    }
                }
            }
        }

        private event EventHandler MMediaEnded;
        public event EventHandler MediaEnded
        {
            add
            {
                if (MMediaEnded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerEndReached);
                }
                MMediaEnded += value;
            }
            remove
            {
                if (MMediaEnded != null)
                {
                    MMediaEnded -= value;
                    if (MMediaEnded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerEndReached);
                    }
                }
            }
        }

        #region IEventBroker Members

        event EventHandler<MediaPlayerMediaChanged> MMediaChanged;
        public event EventHandler<MediaPlayerMediaChanged> MediaChanged
        {
            add
            {
                if (MMediaChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerMediaChanged);
                }
                MMediaChanged += value;
            }
            remove
            {
                if (MMediaChanged != null)
                {
                    MMediaChanged -= value;
                    if (MMediaChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerMediaChanged);
                    }
                }
            }
        }

        event EventHandler MNothingSpecial;
        public event EventHandler NothingSpecial
        {
            add
            {
                if (MNothingSpecial == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerNothingSpecial);
                }
                MNothingSpecial += value;
            }
            remove
            {
                if (MNothingSpecial != null)
                {
                    MNothingSpecial -= value;
                    if (MNothingSpecial == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerNothingSpecial);
                    }
                }
            }
        }

        event EventHandler MPlayerOpening;
        public event EventHandler PlayerOpening
        {
            add
            {
                if (MPlayerOpening == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerOpening);
                }
                MPlayerOpening += value;
            }
            remove
            {
                if (MPlayerOpening != null)
                {
                    MPlayerOpening -= value;
                    if (MPlayerOpening == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerOpening);
                    }
                }
            }
        }

        event EventHandler MPlayerBuffering;
        public event EventHandler PlayerBuffering
        {
            add
            {
                if (MPlayerBuffering == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerBuffering);
                }
                MPlayerBuffering += value;
            }
            remove
            {
                if (MPlayerBuffering != null)
                {
                    MPlayerBuffering -= value;
                    if (MPlayerBuffering == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerBuffering);
                    }
                }
            }
        }

        event EventHandler MPlayerPlaying;
        public event EventHandler PlayerPlaying
        {
            add
            {
                if (MPlayerPlaying == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPlaying);
                }
                MPlayerPlaying += value;
            }
            remove
            {
                if (MPlayerPlaying != null)
                {
                    MPlayerPlaying -= value;
                    if (MPlayerPlaying == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPlaying);
                    }
                }
            }
        }

        event EventHandler MPlayerPaused;
        public event EventHandler PlayerPaused
        {
            add
            {
                if (MPlayerPaused == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPaused);
                }
                MPlayerPaused += value;
            }
            remove
            {
                if (MPlayerPaused != null)
                {
                    MPlayerPaused -= value;
                    if (MPlayerPaused == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPaused);
                    }
                }
            }
        }

        event EventHandler MPlayerStopped;
        public event EventHandler PlayerStopped
        {
            add
            {
                if (MPlayerStopped == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerStopped);
                }
                MPlayerStopped += value;
            }
            remove
            {
                if (MPlayerStopped != null)
                {
                    MPlayerStopped -= value;
                    if (MPlayerStopped == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerStopped);
                    }
                }
            }
        }

        event EventHandler MPlayerForward;
        public event EventHandler PlayerForward
        {
            add
            {
                if (MPlayerForward == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerForward);
                }
                MPlayerForward += value;
            }
            remove
            {
                if (MPlayerForward != null)
                {
                    MPlayerForward -= value;
                    if (MPlayerForward == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerForward);
                    }
                }
            }
        }

        event EventHandler MPlayerBackward;
        public event EventHandler PlayerBackward
        {
            add
            {
                if (MPlayerBackward == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerBackward);
                }
                MPlayerBackward += value;
            }
            remove
            {
                if (MPlayerBackward != null)
                {
                    MPlayerBackward -= value;
                    if (MPlayerBackward == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerBackward);
                    }
                }
            }
        }

        event EventHandler MPlayerEncounteredError;
        public event EventHandler PlayerEncounteredError
        {
            add
            {
                if (MPlayerEncounteredError == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerEncounteredError);
                }
                MPlayerEncounteredError += value;
            }
            remove
            {
                if (MPlayerEncounteredError != null)
                {
                    MPlayerEncounteredError -= value;
                    if (MPlayerEncounteredError == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerEncounteredError);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerPositionChanged> MPlayerPositionChanged;
        public event EventHandler<MediaPlayerPositionChanged> PlayerPositionChanged
        {
            add
            {
                if (MPlayerPositionChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPositionChanged);
                }
                MPlayerPositionChanged += value;
            }
            remove
            {
                if (MPlayerPositionChanged != null)
                {
                    MPlayerPositionChanged -= value;
                    if (MPlayerPositionChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPositionChanged);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerSeekableChanged> MPlayerSeekableChanged;
        public event EventHandler<MediaPlayerSeekableChanged> PlayerSeekableChanged
        {
            add
            {
                if (MPlayerSeekableChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerSeekableChanged);
                }
                MPlayerSeekableChanged += value;
            }
            remove
            {
                if (MPlayerSeekableChanged != null)
                {
                    MPlayerSeekableChanged -= value;
                    if (MPlayerSeekableChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerSeekableChanged);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerPausableChanged> MPlayerPausableChanged;
        public event EventHandler<MediaPlayerPausableChanged> PlayerPausableChanged
        {
            add
            {
                if (MPlayerPausableChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerPausableChanged);
                }
                MPlayerPausableChanged += value;
            }
            remove
            {
                if (MPlayerPausableChanged != null)
                {
                    MPlayerPausableChanged -= value;
                    if (MPlayerPausableChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerPausableChanged);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerTitleChanged> MPlayerTitleChanged;
        public event EventHandler<MediaPlayerTitleChanged> PlayerTitleChanged
        {
            add
            {
                if (MPlayerTitleChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerTitleChanged);
                }
                MPlayerTitleChanged += value;
            }
            remove
            {
                if (MPlayerTitleChanged != null)
                {
                    MPlayerTitleChanged -= value;
                    if (MPlayerTitleChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerTitleChanged);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerSnapshotTaken> MPlayerSnapshotTaken;
        public event EventHandler<MediaPlayerSnapshotTaken> PlayerSnapshotTaken
        {
            add
            {
                if (MPlayerSnapshotTaken == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerSnapshotTaken);
                }
                MPlayerSnapshotTaken += value;
            }
            remove
            {
                if (MPlayerSnapshotTaken != null)
                {
                    MPlayerSnapshotTaken -= value;
                    if (MPlayerSnapshotTaken == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerSnapshotTaken);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerLengthChanged> MPlayerLengthChanged;
        public event EventHandler<MediaPlayerLengthChanged> PlayerLengthChanged
        {
            add
            {
                if (MPlayerLengthChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaPlayerLengthChanged);
                }
                MPlayerLengthChanged += value;
            }
            remove
            {
                if (MPlayerLengthChanged != null)
                {
                    MPlayerLengthChanged -= value;
                    if (MPlayerLengthChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaPlayerLengthChanged);
                    }
                }
            }
        }

        #endregion
    }
}
