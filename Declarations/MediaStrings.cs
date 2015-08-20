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

namespace Declarations
{
    /// <summary>
    /// String values used to identify media types.
    /// </summary>
    public class MediaStrings
    {
        public const string Dvd = @"dvd://";
        public const string Vcd = @"vcd://";
        public const string Cdda = @"cdda://";
        public const string Bluray = @"bluray://";

        public const string Rtp = @"rtp://";
        public const string Rtsp = @"rtsp://";
        public const string Http = @"http://";
        public const string Udp = @"udp://";
        public const string Mms = @"mms://";

        public const string Dshow = @"dshow://";
        public const string Screen = @"screen://";

        /// <summary>
        /// Fake access module. Should be used with IVideoInputMedia objects.
        /// </summary>
        public const string Fake = @"fake://";

        /// <summary>
        /// imem access module. Should be used with IMemoryInputMedia objects
        /// </summary>
        public const string Imem = @"imem://";
    }
}
