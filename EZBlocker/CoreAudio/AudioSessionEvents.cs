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
using System.Text;
using CoreAudio.Interfaces;
using System.Runtime.InteropServices;

namespace CoreAudio
{
    internal class AudioSessionEvents : IAudioSessionEvents
    {
        private AudioSessionControl2 _Parent;

        internal AudioSessionEvents(AudioSessionControl2 parent)
        {
            _Parent = parent;
        }

        [PreserveSig]
        public int OnDisplayNameChanged([MarshalAs(UnmanagedType.LPWStr)] string NewDisplayName, Guid EventContext)
        {
            _Parent.FireDisplayNameChanged(NewDisplayName, EventContext);
            return 0;
        }

        [PreserveSig]
        public int OnIconPathChanged([MarshalAs(UnmanagedType.LPWStr)] string NewIconPath, Guid EventContext)
        {
            _Parent.FireOnIconPathChanged(NewIconPath, EventContext);
            return 0;
        }

        [PreserveSig]
        public int OnSimpleVolumeChanged(float NewVolume, bool newMute, Guid EventContext)
        {
            _Parent.FireSimpleVolumeChanged(NewVolume, newMute, EventContext);
            return 0;
        }

        [PreserveSig]
        public int OnChannelVolumeChanged(UInt32 ChannelCount, IntPtr NewChannelVolumeArray, UInt32 ChangedChannel, Guid EventContext)
        {
            _Parent.FireChannelVolumeChanged(ChannelCount, NewChannelVolumeArray, ChangedChannel, EventContext);
            return 0;
        }

        [PreserveSig]
        public int OnGroupingParamChanged(Guid NewGroupingParam, Guid EventContext)
        {
            return 0;
        }

        [PreserveSig]
        public int OnStateChanged(AudioSessionState NewState)
        {
            _Parent.FireStateChanged(NewState);
            return 0;
        }

        [PreserveSig]
        public int OnSessionDisconnected(AudioSessionDisconnectReason DisconnectReason)
        {
            return 0;
        }
    }
}
