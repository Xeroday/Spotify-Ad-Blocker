using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using CoreAudio.Interfaces;

namespace CoreAudio
{
    [ComImport, Guid("294935CE-F637-4E7C-A41B-AB255460B862")]
    internal class _CPolicyConfigVistaClient
    {
    }

    public class CPolicyConfigVistaClient
    {
        private IPolicyConfigVista _policyConfigVistaClient = new _CPolicyConfigVistaClient() as IPolicyConfigVista;
       
        public int SetDefaultDevie(string deviceID)
        {
            _policyConfigVistaClient.SetDefaultEndpoint(deviceID, ERole.eConsole);
            _policyConfigVistaClient.SetDefaultEndpoint(deviceID, ERole.eMultimedia);
            _policyConfigVistaClient.SetDefaultEndpoint(deviceID, ERole.eCommunications);

            return 0;
        }
    }
}
