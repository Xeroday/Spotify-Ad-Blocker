using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using CoreAudio.Interfaces;

namespace CoreAudio
{
    [ComImport, Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
    internal class _CPolicyConfigClient
    {
    }

    public class CPolicyConfigClient
    {
        private IPolicyConfig _policyConfigClient = new _CPolicyConfigClient() as IPolicyConfig;
       
        public int SetDefaultDevie(string deviceID)
        {
            _policyConfigClient.SetDefaultEndpoint(deviceID, ERole.eConsole);
            _policyConfigClient.SetDefaultEndpoint(deviceID, ERole.eMultimedia);
            _policyConfigClient.SetDefaultEndpoint(deviceID, ERole.eCommunications);

            return 0;
        }
    }
}
