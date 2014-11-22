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
    public class DeviceTopology
    {
        private IDeviceTopology _DeviceTopology;

        internal DeviceTopology(IDeviceTopology realInterface)
        {
            _DeviceTopology = realInterface;
            
        }

        public int GetConnectorCount
        {
            get
            {
                int count = 0;
                Marshal.ThrowExceptionForHR(_DeviceTopology.GetConnectorCount(out count));
                return count;
            }
        }

        public Connector GetConnector(int index)
        {
            IConnector connector;
            Marshal.ThrowExceptionForHR(_DeviceTopology.GetConnector(index, out connector));
            return new Connector(connector);
        }

        public int GetSubunitCount
        {
            get
            {
                int count = 0;
                Marshal.ThrowExceptionForHR(_DeviceTopology.GetSubunitCount(out count));
                return count;
            }
        }

        public Subunit GetSubunit(int index)
        {
            ISubunit subUnit;
            Marshal.ThrowExceptionForHR(_DeviceTopology.GetSubunit(index, out subUnit));
            return new Subunit(subUnit);
        }

        public Part GetPartById(int id)
        {
            IPart part;
            Marshal.ThrowExceptionForHR(_DeviceTopology.GetPartById(id, out part));
            return new Part(part);
        }

        public string GetDeviceId
        {
            get
            {
                string id;
                Marshal.ThrowExceptionForHR(_DeviceTopology.GetDeviceId(out id));
                return id;
            }
        }

        public PartsList GetSignalPath(Part from, Part to, bool rejectMixedPaths)
        {
            IPartsList partList;
            Marshal.ThrowExceptionForHR(_DeviceTopology.GetSignalPath((IPart)from, (IPart)to, rejectMixedPaths, out partList));
            return new PartsList(partList);
        }
    }
}
