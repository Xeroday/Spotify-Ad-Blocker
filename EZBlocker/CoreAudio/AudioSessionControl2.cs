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
using CoreAudio.Interfaces;
using System.Runtime.InteropServices;

namespace CoreAudio
{
    public class AudioSessionControl2 
    {
        IAudioSessionControl2 _AudioSessionControl2;
        internal AudioMeterInformation _AudioMeterInformation;
        internal SimpleAudioVolume _SimpleAudioVolume;

        #region events
        public delegate void DisplayNameChangedDelegate(object sender, string newDisplayName);
        public event DisplayNameChangedDelegate OnDisplayNameChanged;

        public delegate void IconPathChangedDelegate(object sender, string newIconPath);
        public event IconPathChangedDelegate OnIconPathChanged;

        public delegate void SimpleVolumeChangedDelegate(object sender, Single newVolume, Boolean newMute);
        public event SimpleVolumeChangedDelegate OnSimpleVolumeChanged;

        public delegate void ChannelVolumeChangedDelegate(object sender, int channelCount, Single[] newVolume, int changedChannel);
        public event ChannelVolumeChangedDelegate OnChannelVolumeChanged;

        public delegate void StateChangedDelegate(object sender, AudioSessionState newState);
        public event StateChangedDelegate OnStateChanged;

        public delegate void SessionDisconnectedDelegate(object sender, AudioSessionDisconnectReason disconnectReason);
        public event SessionDisconnectedDelegate OnSessionDisconnected;
        #endregion

        private AudioSessionEvents _AudioSessionEvents;

        internal AudioSessionControl2(IAudioSessionControl2 realAudioSessionControl2)
        {
            _AudioSessionControl2 = realAudioSessionControl2;

            IAudioMeterInformation _meters = _AudioSessionControl2 as IAudioMeterInformation;
            ISimpleAudioVolume _volume = _AudioSessionControl2 as ISimpleAudioVolume;
            if (_meters != null) _AudioMeterInformation = new CoreAudio.AudioMeterInformation(_meters);
            if (_volume != null) _SimpleAudioVolume = new SimpleAudioVolume(_volume);

            _AudioSessionEvents = new AudioSessionEvents(this);
            Marshal.ThrowExceptionForHR(_AudioSessionControl2.RegisterAudioSessionNotification(_AudioSessionEvents));
        }

        internal void FireDisplayNameChanged([MarshalAs(UnmanagedType.LPWStr)] string NewDisplayName, Guid EventContext)
        {
            if (OnDisplayNameChanged != null) OnDisplayNameChanged(this, NewDisplayName);
        }

        internal void FireOnIconPathChanged([MarshalAs(UnmanagedType.LPWStr)] string NewIconPath, Guid EventContext)
        {
            if (OnIconPathChanged != null) OnIconPathChanged(this, NewIconPath);
        }

        internal void FireSimpleVolumeChanged(float NewVolume, bool newMute, Guid EventContext)
        {
            if (OnSimpleVolumeChanged != null) OnSimpleVolumeChanged(this, NewVolume, newMute);
        }

        internal void FireChannelVolumeChanged(UInt32 ChannelCount, IntPtr NewChannelVolumeArray, UInt32 ChangedChannel, Guid EventContext)
        {
            float[] volume = new float[ChannelCount];
            Marshal.Copy(NewChannelVolumeArray, volume, 0, (int)ChannelCount);
            if (OnChannelVolumeChanged != null) OnChannelVolumeChanged(this, (int)ChannelCount, (Single[])volume, (int)ChangedChannel);
        }

        internal void FireStateChanged(AudioSessionState NewState)
        {
            if (OnStateChanged != null) OnStateChanged(this, NewState);
        }

        internal void FireSessionDisconnected(AudioSessionDisconnectReason DisconnectReason)
        {
            if (OnSessionDisconnected != null) OnSessionDisconnected(this, DisconnectReason);
        }

        public AudioMeterInformation AudioMeterInformation
        {
            get
            {
                return _AudioMeterInformation;
            }
        }

        public SimpleAudioVolume SimpleAudioVolume
        {
            get
            {
                return _SimpleAudioVolume;
            }
        }

        public AudioSessionState State
        {
            get
            {
                AudioSessionState res;
                Marshal.ThrowExceptionForHR(_AudioSessionControl2.GetState(out res));
                return res;
            }
        }

        public string DisplayName
        {
            get
            {
                string str;
                Marshal.ThrowExceptionForHR(_AudioSessionControl2.GetDisplayName(out str));
                return str;
            }
        }

        public string IconPath
        {
            get
            {
                string str;
                Marshal.ThrowExceptionForHR(_AudioSessionControl2.GetIconPath(out str));
                return str;
            }
        }

        public string GetSessionIdentifier
        {
            get
            {
                string str;
                Marshal.ThrowExceptionForHR(_AudioSessionControl2.GetSessionIdentifier(out str));
                return str;
            }
        }

        public string GetSessionInstanceIdentifier
        {
            get
            {
                string str;
                Marshal.ThrowExceptionForHR(_AudioSessionControl2.GetSessionInstanceIdentifier(out str));
                return str;
            }
        }

        public uint GetProcessID
        {
            get
            {
                uint pid;
                Marshal.ThrowExceptionForHR(_AudioSessionControl2.GetProcessId(out pid));
                return pid;
            }
        }

        public bool IsSystemSoundsSession
        {
            get
            {
                return (_AudioSessionControl2.IsSystemSoundsSession() == 0);
            }

        }

        public void Dispose()
        {
            if(_AudioSessionEvents != null)
                Marshal.ThrowExceptionForHR(_AudioSessionControl2.UnregisterAudioSessionNotification(_AudioSessionEvents));
        }

        ~AudioSessionControl2() {
            Dispose();
        }
    }
}
