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
using System.Runtime.InteropServices;


namespace CoreAudio.Interfaces
{
    [Guid("AE2DE0E4-5BCA-4F2D-AA46-5D13F8FDB3A9"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPart
    {
        [PreserveSig]
        int GetName([Out(), MarshalAs(UnmanagedType.LPWStr)] out string name);
        [PreserveSig]
        int GetLocalId(out int id);
        [PreserveSig]
        int GetGlobalId([Out(), MarshalAs(UnmanagedType.LPWStr)] out string globalId);
        [PreserveSig]
        int GetPartType(out PartType partType);
        [PreserveSig]
        int GetSubType(out Guid subType);
        [PreserveSig]
        int GetControlInterfaceCount(out int count);
        [PreserveSig]
        int GetControlInterface(int index, out IControlInterface pInterface);
        [PreserveSig]
        int EnumPartsIncoming(out IPartsList parts);
        [PreserveSig]
        int EnumPartsOutgoing(out IPartsList parts);
        [PreserveSig]
        int GetTopologyObject(out IDeviceTopology topology);
        [PreserveSig]
        int Activate(CLSCTX dwClsContext, ref Guid refiid, [Out(), MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
        [PreserveSig]
        int RegisterControlChangeCallback(ref Guid refiid, IControlChangeNotify notify);
        [PreserveSig]
        int UnregisterControlChangeCallback(IControlChangeNotify notify);
    }
}
