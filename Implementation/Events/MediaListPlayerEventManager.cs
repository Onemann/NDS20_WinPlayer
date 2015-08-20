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
using Declarations.Events;
using Implementation.Media;
using LibVlcWrapper;
using MediaListPlayerNextItemSet = Declarations.Events.MediaListPlayerNextItemSet;

namespace Implementation.Events
{
    internal class MediaListPlayerEventManager : EventManager, IMediaListPlayerEvents
    {
        public MediaListPlayerEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaListPlayerPlayed:
                    if (MMediaListPlayerPlayed != null)
                    {
                        MMediaListPlayerPlayed(MEventProvider, EventArgs.Empty);
                    }
                    break;
                case LibvlcEventE.LibvlcMediaListPlayerNextItemSet:
                    if (MMediaListPlayerNextItemSet != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_player_next_item_set.item, ReferenceCountAction.AddRef);
                        MMediaListPlayerNextItemSet(MEventProvider, new MediaListPlayerNextItemSet(media));
                        //media.Release();
                    }
                    break;
                case LibvlcEventE.LibvlcMediaListPlayerStopped:
                    if (MMediaListPlayerStopped != null)
                    {
                        MMediaListPlayerStopped(MEventProvider, EventArgs.Empty);
                    }
                    break;
            }
        }

        #region IMediaListPlayerEvents Members

        event EventHandler MMediaListPlayerPlayed;
        public event EventHandler MediaListPlayerPlayed
        {
            add
            {
                if (MMediaListPlayerPlayed == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListPlayerPlayed);
                }
                MMediaListPlayerPlayed += value;
            }
            remove
            {
                if (MMediaListPlayerPlayed != null)
                {
                    MMediaListPlayerPlayed -= value;
                    if (MMediaListPlayerPlayed == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListPlayerPlayed);
                    }
                }
            }
        }

        event EventHandler<MediaListPlayerNextItemSet> MMediaListPlayerNextItemSet;
        public event EventHandler<MediaListPlayerNextItemSet> MediaListPlayerNextItemSet
        {
            add
            {
                if (MMediaListPlayerNextItemSet == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListPlayerNextItemSet);
                }
                MMediaListPlayerNextItemSet += value;
            }
            remove
            {
                if (MMediaListPlayerNextItemSet != null)
                {
                    MMediaListPlayerNextItemSet -= value;
                    if (MMediaListPlayerNextItemSet == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListPlayerNextItemSet);
                    }
                }
            }
        }

        event EventHandler MMediaListPlayerStopped;
        public event EventHandler MediaListPlayerStopped
        {
            add
            {
                if (MMediaListPlayerStopped == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListPlayerStopped);
                }
                MMediaListPlayerStopped += value;
            }
            remove
            {
                if (MMediaListPlayerStopped != null)
                {
                    MMediaListPlayerStopped -= value;
                    if (MMediaListPlayerStopped == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListPlayerStopped);
                    }
                }
            }
        }

        #endregion
    }
}
