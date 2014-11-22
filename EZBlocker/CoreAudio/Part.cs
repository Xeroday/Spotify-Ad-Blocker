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
using System.Reflection;

namespace CoreAudio
{
    public class Part : IDisposable
    {
        private IPart _Part;

        private AudioVolumeLevel _AudioVolumeLevel;
        private AudioMute _AudioMute;
        private AudioPeakMeter _AudioPeakMeter;
        private AudioLoudness _AudioLoudness;

        public delegate void PartNotificationDelegate(object sender);
        public event PartNotificationDelegate OnPartNotification;

        private ControlChangeNotify _AudioVolumeLevelChangeNotification;
        private ControlChangeNotify _AudioMuteChangeNotification;
        private ControlChangeNotify _AudioPeakMeterChangeNotification;
        private ControlChangeNotify _AudioLoudnessChangeNotification;

        private PartsList partsListIncoming;
        private PartsList partsListOutgoing;

        internal Part(IPart part)
        {
            _Part = part;
        }

        internal void FireNotification(UInt32 dwSenderProcessId, ref Guid pguidEventContext)
        {
            if (OnPartNotification != null) OnPartNotification(this);
        }

        private void GetAudioVolumeLevel()
        {
            object result = null;
            _Part.Activate(CLSCTX.ALL, ref IIDs.IID_IAudioVolumeLevel, out result);
            if (result != null)
            {
                _AudioVolumeLevel = new AudioVolumeLevel(result as IAudioVolumeLevel);
                _AudioVolumeLevelChangeNotification = new ControlChangeNotify(this);
                Marshal.ThrowExceptionForHR(_Part.RegisterControlChangeCallback(ref IIDs.IID_IAudioVolumeLevel, _AudioVolumeLevelChangeNotification));
            }
        }

        private void GetAudioMute()
        {
            object result = null;
            _Part.Activate(CLSCTX.ALL, ref IIDs.IID_IAudioMute, out result);
            if (result != null)
            {
                _AudioMute = new AudioMute(result as IAudioMute);
                _AudioMuteChangeNotification = new ControlChangeNotify(this);
                Marshal.ThrowExceptionForHR(_Part.RegisterControlChangeCallback(ref IIDs.IID_IAudioMute, _AudioMuteChangeNotification));
            }
        }

        private void GetAudioPeakMeter()
        {
            object result = null;
            _Part.Activate(CLSCTX.ALL, ref IIDs.IID_IAudioPeakMeter, out result);
            if (result != null)
            {
                _AudioPeakMeter = new AudioPeakMeter(result as IAudioPeakMeter);
                _AudioPeakMeterChangeNotification = new ControlChangeNotify(this);
                Marshal.ThrowExceptionForHR(_Part.RegisterControlChangeCallback(ref IIDs.IID_IAudioPeakMeter, _AudioPeakMeterChangeNotification));
            }
        }

        private void GetAudioLoudness()
        {
            object result = null;
            _Part.Activate(CLSCTX.ALL, ref IIDs.IID_IAudioLoudness, out result);
            if (result != null)
            {
                _AudioLoudness = new AudioLoudness(result as IAudioLoudness);
                _AudioLoudnessChangeNotification = new ControlChangeNotify(this);
                Marshal.ThrowExceptionForHR(_Part.RegisterControlChangeCallback(ref IIDs.IID_IAudioLoudness, _AudioLoudnessChangeNotification));
            }
        }

        public string GetName
        {
            get
            {
                string name;
                Marshal.ThrowExceptionForHR(_Part.GetName(out name));
                return name;
            }
        }

        public int GetLocalId
        {
            get
            {
                int id;
                Marshal.ThrowExceptionForHR(_Part.GetLocalId(out id));
                return id;
            }
        }

        public string GetGlobalId
        {
            get
            {
                string id;
                Marshal.ThrowExceptionForHR(_Part.GetGlobalId(out id));
                return id;
            }
        }

        public PartType GetPartType
        {
            get
            {
                PartType type;
                Marshal.ThrowExceptionForHR(_Part.GetPartType(out type));
                return type;
            }
        }

        public Guid GetSubType
        {
            get
            {
                Guid type;
                Marshal.ThrowExceptionForHR(_Part.GetSubType(out type));
                return type;
            }
        }

        public string GetSubTypeName
        {
            get
            {
                string result;
                Guid subType = this.GetSubType;

                result = FindSubTypeIn(subType, typeof(KSNODETYPE));
                if (result != "") return result;

                result = FindSubTypeIn(subType, typeof(KSCATEGORY));
                if (result != "") return result;

                return "UNDEFINED";
            }
        }

        private string FindSubTypeIn(Guid findGuid, Type inClass)
        {
            FieldInfo[] fields = inClass.GetFields();
            foreach (var field in fields)
            {
                string name = field.Name;
                Guid temp = (Guid)field.GetValue(null);
                if (temp == findGuid)
                {
                    return name;
                }
            }
            return "";
        }

        public int GetControlInterfaceCount
        {
            get
            {
                int count = 0;
                Marshal.ThrowExceptionForHR(_Part.GetControlInterfaceCount(out count));
                return count;
            }
        }

        public ControlInterface GetControlInterface(int index)
        {
            IControlInterface controlInterface;
            Marshal.ThrowExceptionForHR(_Part.GetControlInterface(index, out controlInterface));
            return new ControlInterface(controlInterface);
        }

        public PartsList EnumPartsIncoming
        {
            get
            {
                if (partsListIncoming == null)
                {
                    IPartsList partsList = null;
                    _Part.EnumPartsIncoming(out partsList);
                    if(partsList != null) partsListIncoming = new PartsList(partsList);
                }
                return partsListIncoming;
            }
        }

        public PartsList EnumPartsOutgoing
        {
            get
            {
                if (partsListOutgoing == null)
                {
                    IPartsList partsList = null;
                    _Part.EnumPartsOutgoing(out partsList);
                    if (partsList != null) partsListOutgoing = new PartsList(partsList);
                }
                return partsListOutgoing;
            }
        }

        public DeviceTopology GetTopologyObject
        {
            get
            {
                IDeviceTopology deviceTopology;
                Marshal.ThrowExceptionForHR(_Part.GetTopologyObject(out deviceTopology));
                return new DeviceTopology(deviceTopology);
            }
        }

        public AudioVolumeLevel AudioVolumeLevel
        {
            get
            {
                if (_AudioVolumeLevel == null)
                    GetAudioVolumeLevel();

                return _AudioVolumeLevel;
            }
        }

        public AudioMute AudioMute
        {
            get
            {
                if (_AudioMute == null)
                    GetAudioMute();

                return _AudioMute;
            }
        }

        public AudioPeakMeter AudioPeakMeter
        {
            get
            {
                if (_AudioPeakMeter == null)
                    GetAudioPeakMeter();

                return _AudioPeakMeter;
            }
        }

        public AudioLoudness AudioLoudness
        {
            get
            {
                if (_AudioLoudness == null)
                    GetAudioLoudness();

                return _AudioLoudness;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            DisposeCtrlChangeNotify(ref _AudioLoudnessChangeNotification);
            DisposeCtrlChangeNotify(ref _AudioMuteChangeNotification);
            DisposeCtrlChangeNotify(ref _AudioPeakMeterChangeNotification);
            DisposeCtrlChangeNotify(ref _AudioVolumeLevelChangeNotification);
        }

        private void DisposeCtrlChangeNotify(ref ControlChangeNotify obj)
        {
            if(obj != null) {
                try {
                    ControlChangeNotify cn = (ControlChangeNotify)obj;
                    if(cn.IsAllocated) {
                        Marshal.ThrowExceptionForHR(_Part.UnregisterControlChangeCallback(cn));
                        cn.Dispose();
                    }
                } catch { }
                obj = null;
            }
        }

        ~Part()
        {
            Dispose();
        }

        #endregion
    }
}
