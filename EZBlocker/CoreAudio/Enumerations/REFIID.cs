/*
  LICENSE
  -------
  Copyright (C) 2007-2010 Ray Molenkamp

  This source code is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this source code or the software it produces.

  Permission is granted to anyone to use this source code for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this source code must not be misrepresented; you must not
     claim that you wrote the original source code.  If you use this source code
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original source code.
  3. This notice may not be removed or altered from any source distribution.
*/

/* Created by Xavier Flix (2010/11/18) */

using System;
using System.Collections.Generic;
#if (NET40) 
using System.Linq;
#endif
using System.Text;

namespace CoreAudio
{
    public class IIDs
    {
        public static Guid IID_IAudioCaptureClient = typeof(Interfaces.IAudioCaptureClient).GUID;
        //public static Guid IID_IAudioClock
        //public static Guid IID_IAudioRenderClient
        //public static Guid IID_IAudioSessionControl
        //public static Guid IID_IAudioStreamVolume
        //public static Guid IID_IChannelAudioVolume
        //public static Guid IID_IMFTrustedOutput

        public static Guid IID_ISimpleAudioVolume = typeof(Interfaces.ISimpleAudioVolume).GUID;
        public static Guid IID_IAudioVolumeLevel = typeof(Interfaces.IAudioVolumeLevel).GUID;
        public static Guid IID_IAudioMute = typeof(Interfaces.IAudioMute).GUID;
        public static Guid IID_IAudioPeakMeter = typeof(Interfaces.IAudioPeakMeter).GUID;
        public static Guid IID_IAudioLoudness = typeof(Interfaces.IAudioLoudness).GUID;

        public static Guid IID_IAudioMeterInformation = typeof(Interfaces.IAudioMeterInformation).GUID;
        public static Guid IID_IAudioEndpointVolume = typeof(Interfaces.IAudioEndpointVolume).GUID;
        public static Guid IID_IAudioSessionManager2 = typeof(Interfaces.IAudioSessionManager2).GUID;
        public static Guid IID_IDeviceTopology = typeof(Interfaces.IDeviceTopology).GUID;

        public static Guid IID_IPart = typeof(Interfaces.IPart).GUID;
    }
}
