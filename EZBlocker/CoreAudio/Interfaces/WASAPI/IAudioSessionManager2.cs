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
using System;
using System.Collections.Generic;
#if (NET40) 
using System.Linq;
#endif
using System.Text;
using System.Runtime.InteropServices;

namespace CoreAudio.Interfaces
{
    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionManager2
    {
        [PreserveSig]
        int GetAudioSessionControl(ref Guid AudioSessionGuid, UInt32 StreamFlags, out IAudioSessionControl2 ISessionControl);
        [PreserveSig]
        int GetSimpleAudioVolume(ref Guid AudioSessionGuid, UInt32 StreamFlags, out ISimpleAudioVolume SimpleAudioVolume);
        [PreserveSig]
        int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);
        [PreserveSig]
        int RegisterSessionNotification(IAudioSessionNotification SessionNotification);
        [PreserveSig]
        int UnregisterSessionNotification(IAudioSessionNotification SessionNotification);
        [PreserveSig]
        int RegisterDuckNotification(string sessionID, IAudioSessionNotification IAudioVolumeDuckNotification);
        [PreserveSig]
        int UnregisterDuckNotification(IntPtr IAudioVolumeDuckNotification);
    };
}
