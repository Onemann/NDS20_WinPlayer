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
using System.Runtime.InteropServices;
using Declarations;
using Declarations.Events;
using LibVlcWrapper;

namespace Implementation.Events
{
    internal abstract class EventManager
    {
        protected IEventProvider MEventProvider;
        List<VlcEventHandlerDelegate> _mCallbacks = new List<VlcEventHandlerDelegate>();
        IntPtr _hCallback1;

        protected EventManager(IEventProvider eventProvider)
        {
            MEventProvider = eventProvider;

            VlcEventHandlerDelegate callback1 = MediaPlayerEventOccured;

            _hCallback1 = Marshal.GetFunctionPointerForDelegate(callback1);
            _mCallbacks.Add(callback1);
        }

        protected void Attach(LibvlcEventE eType)
        {
            if (LibVlcMethods.libvlc_event_attach(MEventProvider.EventManagerHandle, eType, _hCallback1, IntPtr.Zero) != 0)
            {
                throw new OutOfMemoryException("Failed to subscribe to event notification");
            }
        }

        protected void Dettach(LibvlcEventE eType)
        {
            LibVlcMethods.libvlc_event_detach(MEventProvider.EventManagerHandle, eType, _hCallback1, IntPtr.Zero);
        }

        protected abstract void MediaPlayerEventOccured(ref LibvlcEventT libvlcEvent, IntPtr userData);
    }
}
