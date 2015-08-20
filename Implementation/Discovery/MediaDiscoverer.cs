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
using Declarations.Discovery;
using Declarations.Events;
using Declarations.Media;
using Implementation.Events;
using Implementation.Media;
using LibVlcWrapper;
using System.Runtime.InteropServices;

namespace Implementation.Discovery
{
    internal class MediaDiscoverer : DisposableBase, IMediaDiscoverer, INativePointer, IEventProvider
    {
        private IntPtr _mHDiscovery = IntPtr.Zero;
        private IMediaDiscoveryEvents _mEvents;

        public MediaDiscoverer(IntPtr hMediaLib, string name)
        {
            _mHDiscovery = LibVlcMethods.libvlc_media_discoverer_new_from_name(hMediaLib, name.ToUtf8());
        }

        protected override void Dispose(bool disposing)
        {
            LibVlcMethods.libvlc_media_discoverer_release(_mHDiscovery);
        }

        public bool IsRunning
        {
            get
            {
                return (LibVlcMethods.libvlc_media_discoverer_is_running(_mHDiscovery) == 1);
            }
        }

        public string LocalizedName
        {
            get
            {
                var pData = LibVlcMethods.libvlc_media_discoverer_localized_name(_mHDiscovery);
                return Marshal.PtrToStringAnsi(pData);
            }
        }

        public IMediaList MediaList
        {
            get
            {
                return new MediaList(LibVlcMethods.libvlc_media_discoverer_media_list(_mHDiscovery), ReferenceCountAction.None);
            }
        }

        public IntPtr EventManagerHandle
        {
            get 
            {
                return LibVlcMethods.libvlc_media_discoverer_event_manager(_mHDiscovery);
            }
        }

        public IntPtr Pointer
        {
            get 
            {
                return _mHDiscovery;
            }
        }

        public IMediaDiscoveryEvents Events
        {
            get
            {
                if (_mEvents == null)
                {
                    _mEvents = new MediaDiscoveryEventManager(this);
                }

                return _mEvents;
            }
        }
    }
}
