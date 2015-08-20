using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibVlcWrapper;
using Implementation.Exceptions;
using Implementation.Events;
using Declarations.Events;
using System.Runtime.InteropServices;
using Declarations.VLM;

namespace Implementation
{
    internal class VlmEventManager : EventManager, IVlmEventManager
    {
        public VlmEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch (libvlcEvent.type)
            {

                case LibvlcEventE.LibvlcVlmMediaAdded:
                    if (MMediaAdded != null)
                    {
                        MMediaAdded(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaRemoved:
                    if (MMediaRemoved != null)
                    {
                        MMediaRemoved(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaChanged:
                    if (MMediaChanged != null)
                    {
                        MMediaChanged(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStarted:
                    if (MMediaInstanceStarted != null)
                    {
                        MMediaInstanceStarted(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStopped:
                    if (MMediaInstanceStopped != null)
                    {
                        MMediaInstanceStopped(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusInit:
                    if (MMediaInstanceInit != null)
                    {
                        MMediaInstanceInit(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusOpening:
                    if (MMediaInstanceOpening != null)
                    {
                        MMediaInstanceOpening(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusPlaying:
                    if (MMediaInstancePlaying != null)
                    {
                        MMediaInstancePlaying(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusPause:
                    if (MMediaInstancePause != null)
                    {
                        MMediaInstancePause(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusEnd:
                    if (MMediaInstanceEnd != null)
                    {
                        MMediaInstanceEnd(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                case LibvlcEventE.LibvlcVlmMediaInstanceStatusError:
                    if (MMediaInstanceError != null)
                    {
                        MMediaInstanceError(MEventProvider, new VlmEvent(Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_instance_name), Marshal.PtrToStringAuto(libvlcEvent.MediaDescriptor.vlm_media_event.psz_media_name)));
                    }
                    break;
                default:
                    break;
            }
        }


        private event EventHandler<VlmEvent> MMediaAdded;

        public event EventHandler<VlmEvent> MediaAdded
        {
            add
            {
                if (MMediaAdded == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaAdded);
                }
                MMediaAdded += value;
            }
            remove
            {
                if (MMediaAdded != null)
                {
                    MMediaAdded -= value;
                    if (MMediaAdded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaAdded);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> MMediaRemoved;

        public event EventHandler<VlmEvent> MediaRemoved
        {
            add
            {
                if (MMediaRemoved == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaRemoved);
                }
                MMediaRemoved += value;
            }
            remove
            {
                if (MMediaRemoved != null)
                {
                    MMediaRemoved -= value;
                    if (MMediaRemoved == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaRemoved);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> MMediaChanged;

        public event EventHandler<VlmEvent> MediaChanged
        {
            add
            {
                if (MMediaChanged == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaChanged);
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
                        Dettach(LibvlcEventE.LibvlcVlmMediaChanged);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> MMediaInstanceStarted;

        public event EventHandler<VlmEvent> MediaInstanceStarted
        {
            add
            {
                if (MMediaInstanceStarted == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStarted);
                }
                MMediaInstanceStarted += value;
            }
            remove
            {
                if (MMediaInstanceStarted != null)
                {
                    MMediaInstanceStarted -= value;
                    if (MMediaInstanceStarted == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStarted);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> MMediaInstanceStopped;

        public event EventHandler<VlmEvent> MediaInstanceStopped
        {
            add
            {
                if (MMediaInstanceStopped == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStopped);
                }
                MMediaInstanceStopped += value;
            }
            remove
            {
                if (MMediaInstanceStopped != null)
                {
                    MMediaInstanceStopped -= value;
                    if (MMediaInstanceStopped == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStopped);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> MMediaInstanceInit;

        public event EventHandler<VlmEvent> MediaInstanceInit
        {
            add
            {
                if (MMediaInstanceInit == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusInit);
                }
                MMediaInstanceInit += value;
            }
            remove
            {
                if (MMediaInstanceInit != null)
                {
                    MMediaInstanceInit -= value;
                    if (MMediaInstanceInit == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusInit);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> MMediaInstanceOpening;

        public event EventHandler<VlmEvent> MediaInstanceOpening
        {
            add
            {
                if (MMediaInstanceOpening == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusOpening);
                }
                MMediaInstanceOpening += value;
            }
            remove
            {
                if (MMediaInstanceOpening != null)
                {
                    MMediaInstanceOpening -= value;
                    if (MMediaInstanceOpening == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusOpening);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> MMediaInstancePlaying;

        public event EventHandler<VlmEvent> MediaInstancePlaying
        {
            add
            {
                if (MMediaInstancePlaying == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPlaying);
                }
                MMediaInstancePlaying += value;
            }
            remove
            {
                if (MMediaInstancePlaying != null)
                {
                    MMediaInstancePlaying -= value;
                    if (MMediaInstancePlaying == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPlaying);
                    }
                }
            }
        }


        private event EventHandler<VlmEvent> MMediaInstancePause;

        public event EventHandler<VlmEvent> MediaInstancePause
        {
            add
            {
                if (MMediaInstancePause == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPause);
                }
                MMediaInstancePause += value;
            }
            remove
            {
                if (MMediaInstancePause != null)
                {
                    MMediaInstancePause -= value;
                    if (MMediaInstancePause == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusPause);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> MMediaInstanceEnd;

        public event EventHandler<VlmEvent> MediaInstanceEnd
        {
            add
            {
                if (MMediaInstanceEnd == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusEnd);
                }
                MMediaInstanceEnd += value;
            }
            remove
            {
                if (MMediaInstanceEnd != null)
                {
                    MMediaInstanceEnd -= value;
                    if (MMediaInstanceEnd == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusEnd);
                    }
                }
            }
        }

        private event EventHandler<VlmEvent> MMediaInstanceError;

        public event EventHandler<VlmEvent> MediaInstanceError
        {
            add
            {
                if (MMediaInstanceError == null)
                {
                    Attach(LibvlcEventE.LibvlcVlmMediaInstanceStatusError);
                }
                MMediaInstanceError += value;
            }
            remove
            {
                if (MMediaInstanceError != null)
                {
                    MMediaInstanceError -= value;
                    if (MMediaInstanceError == null)
                    {
                        Dettach(LibvlcEventE.LibvlcVlmMediaInstanceStatusError);
                    }
                }
            }
        }
    }
}
