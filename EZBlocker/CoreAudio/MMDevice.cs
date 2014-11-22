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
    public class MMDevice
    {
        #region Variables
        private IMMDevice _RealDevice;
        private PropertyStore _PropertyStore;
        private AudioMeterInformation _AudioMeterInformation;
        private AudioEndpointVolume _AudioEndpointVolume;
        private AudioSessionManager2 _AudioSessionManager2;
        private DeviceTopology _DeviceTopology;

        #endregion

        #region Init
        private void GetPropertyInformation()
        {
            IPropertyStore propstore;
            Marshal.ThrowExceptionForHR(_RealDevice.OpenPropertyStore(EStgmAccess.STGM_READ, out propstore));
            _PropertyStore = new PropertyStore(propstore);
        }

        private void GetAudioSessionManager2()
        {
            object result;
            Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IIDs.IID_IAudioSessionManager2, CLSCTX.ALL, IntPtr.Zero, out result));
            _AudioSessionManager2 = new AudioSessionManager2(result as IAudioSessionManager2);
        }

        private void GetAudioMeterInformation()
        {
            object result;
            Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IIDs.IID_IAudioMeterInformation, CLSCTX.ALL, IntPtr.Zero, out result));
            _AudioMeterInformation = new AudioMeterInformation( result as IAudioMeterInformation);
        }

        private void GetAudioEndpointVolume()
        {
            object result;
            Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IIDs.IID_IAudioEndpointVolume, CLSCTX.ALL, IntPtr.Zero, out result));
            _AudioEndpointVolume = new AudioEndpointVolume(result as IAudioEndpointVolume);
        }

        private void GetDeviceTopology()
        {
            object result;
            Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IIDs.IID_IDeviceTopology, CLSCTX.ALL, IntPtr.Zero, out result));
            _DeviceTopology = new DeviceTopology(result as IDeviceTopology);
        }

        #endregion

        #region Properties

        public AudioSessionManager2 AudioSessionManager2
        {
            get
            {
                if (_AudioSessionManager2 == null) GetAudioSessionManager2();
                return _AudioSessionManager2;
            }
        }

        public AudioMeterInformation AudioMeterInformation
        {
            get
            {
                if (_AudioMeterInformation == null) GetAudioMeterInformation();
                return _AudioMeterInformation;
            }
        }

        public AudioEndpointVolume AudioEndpointVolume
        {
            get
            {
                if (_AudioEndpointVolume == null) GetAudioEndpointVolume();
                return _AudioEndpointVolume;
            }
        }

        public PropertyStore Properties
        {
            get
            {
                if (_PropertyStore == null) GetPropertyInformation();
                return _PropertyStore;
            }
        }

        public DeviceTopology DeviceTopology
        {
            get
            {
                if (_DeviceTopology == null) GetDeviceTopology();
                return _DeviceTopology;
            }
        }

        public string FriendlyName
        {
            get
            {
                if (_PropertyStore == null) GetPropertyInformation();
                if(_PropertyStore.Contains(PKEY.PKEY_DeviceInterface_FriendlyName)) {
                    return (string)_PropertyStore[PKEY.PKEY_DeviceInterface_FriendlyName].Value;
                } else {
                    return "Unknown";
                }
            }
        }


        public string ID
        {
            get
            {
                string Result;
                Marshal.ThrowExceptionForHR(_RealDevice.GetId(out Result));
                return Result;
            }
        }

        public EDataFlow DataFlow
        {
            get
            {
                EDataFlow Result;
                IMMEndpoint ep =  _RealDevice as IMMEndpoint ;
                ep.GetDataFlow(out Result);
                return Result;
            }
        }

        public DEVICE_STATE State
        {
            get
            {
                DEVICE_STATE Result;
                Marshal.ThrowExceptionForHR(_RealDevice.GetState(out Result));
                return Result;

            }
        }

        internal IMMDevice ReadDevice
        {
            get
            {
                return _RealDevice;
            }
        }

        public bool Selected
        {
            get
            {
                return (new MMDeviceEnumerator()).GetDefaultAudioEndpoint(DataFlow, ERole.eMultimedia).ID == ID;
            }
            set
            {
                if (value == true)
                {
                    (new CPolicyConfigVistaClient()).SetDefaultDevie(this.ID);
                    //if(System.Environment.OSVersion.Version.Major==6 && System.Environment.OSVersion.Version.Minor==0)
                    //    (new CPolicyConfigVistaClient()).SetDefaultDevie(this.ID);
                    //else
                    //    (new CPolicyConfigClient()).SetDefaultDevie(this.ID);
                }
            }
        }

        #endregion

        #region Constructor
        internal MMDevice(IMMDevice realDevice)
        {
            _RealDevice = realDevice;
        }
        #endregion

    }
}
