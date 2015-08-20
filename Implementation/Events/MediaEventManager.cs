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
using Implementation.Media;
using LibVlcWrapper;

namespace Implementation.Events
{
    class MediaEventManager : EventManager, IMediaEvents
    {
        public MediaEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {
        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaMetaChanged:
                    if (metaDataChanged != null)
                    {
                        metaDataChanged(MEventProvider, new MediaMetaDataChange((MetaDataType)libvlcEvent.MediaDescriptor.media_meta_changed.meta_type));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaSubItemAdded:
                    if (subItemAdded != null)
                    {
                        var media = new BasicMedia(libvlcEvent.MediaDescriptor.media_subitem_added.new_child, ReferenceCountAction.AddRef);
                        subItemAdded(MEventProvider, new MediaNewSubItem(media));
                        media.Release();
                    }

                    break;
                case LibvlcEventE.LibvlcMediaDurationChanged:
                    if (durationChanged != null)
                    {
                        durationChanged(MEventProvider, new MediaDurationChange(libvlcEvent.MediaDescriptor.media_duration_changed.new_duration));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaParsedChanged:
                    if (parsedChanged != null)
                    {
                        parsedChanged(MEventProvider, new MediaParseChange(Convert.ToBoolean(libvlcEvent.MediaDescriptor.media_parsed_changed.new_status)));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaFreed:
                    if (mediaFreed != null)
                    {
                        mediaFreed(MEventProvider, new MediaFree(libvlcEvent.MediaDescriptor.media_freed.md));
                    }

                    break;
                case LibvlcEventE.LibvlcMediaStateChanged:
                    if (stateChanged != null)
                    {
                        stateChanged(MEventProvider, new MediaStateChange((MediaState)libvlcEvent.MediaDescriptor.media_state_changed.new_state));
                    }

                    break;

                default:
                    break;
            }
        }

        #region IMediaEvents Members

        event EventHandler<MediaMetaDataChange> metaDataChanged;
        public event EventHandler<MediaMetaDataChange> MetaDataChanged
        {
            add
            {
                if (metaDataChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaMetaChanged);
                }
                metaDataChanged += value;
            }
            remove
            {
                if (metaDataChanged != null)
                {
                    metaDataChanged -= value;
                    if (metaDataChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaMetaChanged);
                    }
                }
            }
        }

        event EventHandler<MediaNewSubItem> subItemAdded;
        public event EventHandler<MediaNewSubItem> SubItemAdded
        {
            add
            {
                if (subItemAdded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaSubItemAdded);
                }
                subItemAdded += value;
            }
            remove
            {
                if (subItemAdded != null)
                {
                    subItemAdded -= value;
                    if (subItemAdded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaSubItemAdded);
                    }
                }
            }
        }

        event EventHandler<MediaDurationChange> durationChanged;
        public event EventHandler<MediaDurationChange> DurationChanged
        {
            add
            {
                if (durationChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaDurationChanged);
                }
                durationChanged += value;
            }
            remove
            {
                if (durationChanged != null)
                {
                    durationChanged -= value;
                    if (durationChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaDurationChanged);
                    }
                }
            }
        }

        event EventHandler<MediaParseChange> parsedChanged;
        public event EventHandler<MediaParseChange> ParsedChanged
        {
            add
            {
                if (parsedChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaParsedChanged);
                }
                parsedChanged += value;
            }
            remove
            {
                if (parsedChanged != null)
                {
                    parsedChanged -= value;
                    if (parsedChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaParsedChanged);
                    }
                }
            }
        }

        event EventHandler<MediaFree> mediaFreed;
        public event EventHandler<MediaFree> MediaFreed
        {
            add
            {
                if (mediaFreed == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaFreed);
                }
                mediaFreed += value;
            }
            remove
            {
                if (mediaFreed != null)
                {
                    mediaFreed -= value;
                    if (mediaFreed == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaFreed);
                    }
                }
            }
        }

        event EventHandler<MediaStateChange> stateChanged;
        public event EventHandler<MediaStateChange> StateChanged
        {
            add
            {
                if (stateChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaStateChanged);
                }
                stateChanged += value;
            }
            remove
            {
                if (stateChanged != null)
                {
                    stateChanged -= value;
                    if (stateChanged == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaStateChanged);
                    }
                }
            }
        }

        #endregion
    }
}
