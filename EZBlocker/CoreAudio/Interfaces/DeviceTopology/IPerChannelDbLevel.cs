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
    [Guid("C2F8E001-F205-4BC9-99BC-C13B1E048CCB"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPerChannelDbLevel
    {
        [PreserveSig]
        int GetChannelCount(out uint count);
        [PreserveSig]
        int GetLevelRange(uint channel, out float minLevel, out float maxLevel, out float stepping);
        [PreserveSig]
        int GetLevel(uint channel, out float level);        
        [PreserveSig]
        int SetLevel(uint channel, float level, out Guid eventContext);
        [PreserveSig]
        int SetLevelUniform(float level, out Guid eventContext);
        [PreserveSig]
        int SetLevelAllChannels(float[] levelsDB, ulong channels, Guid eventContext);
    }
}
