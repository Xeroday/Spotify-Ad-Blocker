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
    public class Connector
    {
        private IConnector _Connector;
        private Part _Part;

        internal Connector(IConnector connector)
        {
            _Connector = connector;
        }

        public ConnectorType GetConnectorType
        {
            get
            {
                ConnectorType type;
                Marshal.ThrowExceptionForHR(_Connector.GetType(out type));
                return type;
            }
        }

        public EDataFlow GetDataFlow
        {
            get
            {
                EDataFlow flow;
                Marshal.ThrowExceptionForHR(_Connector.GetDataFlow(out flow));
                return flow;
            }
        }

        public void ConnecTo(Connector connectTo)
        {
            Marshal.ThrowExceptionForHR(_Connector.ConnectTo((IConnector)connectTo));
        }

        public void Disconnect()
        {
            Marshal.ThrowExceptionForHR(_Connector.Disconnect());
        }

        public bool IsConnected
        {
            get
            {
                bool result;
                Marshal.ThrowExceptionForHR(_Connector.IsConnected(out result));
                return result;
            }
        }

        public Connector GetConnectedTo
        {
            get
            {
                IConnector connectedTo;
                Marshal.ThrowExceptionForHR(_Connector.GetConnectedTo(out connectedTo));
                return new Connector(connectedTo);
            }
        }

        public string GetConnectorIdConnectedTo
        {
            get
            {
                string id;
                Marshal.ThrowExceptionForHR(_Connector.GetConnectorIdConnectedTo(out id));
                return id;
            }
        }

        public string GetDeviceIdConnectedTo
        {
            get
            {
                string id;
                Marshal.ThrowExceptionForHR(_Connector.GetDeviceIdConnectedTo(out id));
                return id;
            }
        }

        public Part GetPart
        {
            get
            {
                if (_Part == null)
                {
                    IntPtr pUnk = Marshal.GetIUnknownForObject(_Connector);
                    IntPtr ppv;

                    int res = Marshal.QueryInterface(pUnk, ref IIDs.IID_IPart, out ppv);
                    if (ppv != IntPtr.Zero)
                        _Part = new Part((IPart)Marshal.GetObjectForIUnknown(ppv));
                    else
                        _Part = null;
                }
                return _Part;
            }
        }
    }
}
