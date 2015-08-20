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
using MediaListItemAdded = Declarations.Events.MediaListItemAdded;
using MediaListItemDeleted = Declarations.Events.MediaListItemDeleted;
using MediaListWillAddItem = Declarations.Events.MediaListWillAddItem;
using MediaListWillDeleteItem = Declarations.Events.MediaListWillDeleteItem;

namespace Implementation.Events
{
    class MediaListEventManager : EventManager, IMediaListEvents
    {
        public MediaListEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {
        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaListItemAdded:
                    if (MItemAdded != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_item_added.item, ReferenceCountAction.AddRef);
                        MItemAdded(MEventProvider, new MediaListItemAdded(media, libvlcEvent.MediaDescriptor.media_list_item_added.index));
                        media.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaListWillAddItem:
                    if (MWillAddItem != null)
                    {
                        var media2 = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_will_add_item.item, ReferenceCountAction.AddRef);
                        MWillAddItem(MEventProvider, new MediaListWillAddItem(media2, libvlcEvent.MediaDescriptor.media_list_will_add_item.index));
                        media2.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaListItemDeleted:
                    if (MItemDeleted != null)
                    {
                        var media3 = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_item_deleted.item, ReferenceCountAction.AddRef);
                        MItemDeleted(MEventProvider, new MediaListItemDeleted(media3, libvlcEvent.MediaDescriptor.media_list_item_deleted.index));
                        media3.Release();
                    }
                    break;

                case LibvlcEventE.LibvlcMediaListWillDeleteItem:
                    if (MWillDeleteItem != null)
                    {
                        var media4 = new BasicMedia(libvlcEvent.MediaDescriptor.media_list_will_delete_item.item, ReferenceCountAction.AddRef);
                        MWillDeleteItem(MEventProvider, new MediaListWillDeleteItem(media4, libvlcEvent.MediaDescriptor.media_list_will_delete_item.index));
                        media4.Release();
                    }
                    break;
            }
        }

        #region IMediaListEvents Members

        event EventHandler<MediaListItemAdded> MItemAdded;
        public event EventHandler<MediaListItemAdded> ItemAdded
        {
            add
            {
                if (MItemAdded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListItemAdded);
                }
                MItemAdded += value;
            }
            remove
            {
                if (MItemAdded != null)
                {
                    MItemAdded -= value;
                    if (MItemAdded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListItemAdded);
                    }
                }
            }
        }

        event EventHandler<MediaListWillAddItem> MWillAddItem;
        public event EventHandler<MediaListWillAddItem> WillAddItem
        {
            add
            {
                if (MWillAddItem != null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListWillAddItem);
                }
                MWillAddItem += value;
            }
            remove
            {
                if (MWillAddItem != null)
                {
                    MWillAddItem -= value;
                    if (MWillAddItem == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListWillAddItem);
                    }
                }
            }
        }

        event EventHandler<MediaListItemDeleted> MItemDeleted;
        public event EventHandler<MediaListItemDeleted> ItemDeleted
        {
            add
            {
                if (MItemDeleted == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListItemDeleted);
                }
                MItemDeleted += value;
            }
            remove
            {
                if (MItemDeleted != null)
                {
                    MItemDeleted -= value;
                    if (MItemDeleted == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListItemDeleted);
                    }
                }
            }
        }

        event EventHandler<MediaListWillDeleteItem> MWillDeleteItem;
        public event EventHandler<MediaListWillDeleteItem> WillDeleteItem
        {
            add
            {
                if (MWillDeleteItem == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaListWillDeleteItem);
                }
                MWillDeleteItem += value;
            }
            remove
            {
                if (MWillDeleteItem != null)
                {
                    MWillDeleteItem -= value;
                    if (MWillDeleteItem == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaListWillDeleteItem);
                    }
                }
            }
        }

        #endregion
    }
}
