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
using Declarations.Events;
using LibVlcWrapper;

namespace Implementation.Events
{
    class MediaDiscoveryEventManager : EventManager, IMediaDiscoveryEvents
    {
        public MediaDiscoveryEventManager(IEventProvider eventProvider)
            : base(eventProvider)
        {

        }

        protected override void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData)
        {
            switch(libvlcEvent.type)
            {
                case LibvlcEventE.LibvlcMediaDiscovererStarted:

                    break;

                case LibvlcEventE.LibvlcMediaDiscovererEnded:

                    break;
            }
        }

        private event EventHandler MMediaDiscoveryStarted;
        public event EventHandler MediaDiscoveryStarted
        {
            add
            {
                if (MMediaDiscoveryStarted == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaDiscovererStarted);
                }
                MMediaDiscoveryStarted += value;
            }
            remove
            {
                if (MMediaDiscoveryStarted != null)
                {
                    MMediaDiscoveryStarted -= value;
                    if (MMediaDiscoveryStarted == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaDiscovererStarted);
                    }
                }
            }
        }

        private event EventHandler MMediaDiscoveryEnded;
        public event EventHandler MediaDiscoveryEnded
        {
            add
            {
                if (MMediaDiscoveryEnded == null)
                {
                    Attach(LibvlcEventE.LibvlcMediaDiscovererEnded);
                }
                MMediaDiscoveryEnded += value;
            }
            remove
            {
                if (MMediaDiscoveryEnded != null)
                {
                    MMediaDiscoveryEnded -= value;
                    if (MMediaDiscoveryEnded == null)
                    {
                        Dettach(LibvlcEventE.LibvlcMediaDiscovererEnded);
                    }
                }
            }
        }      
    }
}
