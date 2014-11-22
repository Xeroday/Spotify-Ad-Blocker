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
    public class PerChannelDbLevel
    {
        public struct LevelRange
        {
            public float minLevel;
            public float maxLevel;
            public float stepping;

            public LevelRange(float minLevel, float maxLevel, float stepping)
            {
                this.minLevel = minLevel;
                this.maxLevel = maxLevel;
                this.stepping = stepping;
            }            
        }

        private IPerChannelDbLevel _PerChannelDbLevel;

        internal PerChannelDbLevel(IPerChannelDbLevel perChannelDbLevel)
        {
            _PerChannelDbLevel = perChannelDbLevel;
        }
        
        public int GetChannelCount
        {
            get
            {
                uint count;
                Marshal.ThrowExceptionForHR(_PerChannelDbLevel.GetChannelCount(out count));
                return (int)count;
            }
        }

        public float GetLevel(int channel)
        {
            System.Threading.Thread.Sleep(5);
            float level = 0;
            try {
                Marshal.ThrowExceptionForHR(_PerChannelDbLevel.GetLevel((uint)channel, out level));
            } catch(Exception) {
                System.Threading.Thread.Sleep(100);
            }
            return level;
        }

        public LevelRange GetLevelRange(int channel)
        {
            float minLevel = 0;
            float maxLevel = 0;
            float stepping = 0;
            System.Threading.Thread.Sleep(5);
            try {
                Marshal.ThrowExceptionForHR(_PerChannelDbLevel.GetLevelRange((uint)channel, out minLevel, out maxLevel, out stepping));
            } catch(Exception) {
                System.Threading.Thread.Sleep(100);
            }
            return new LevelRange(minLevel, maxLevel, stepping);
        }

        public void SetLevel(int channel, float level)
        {
            Guid eventContext;
            Marshal.ThrowExceptionForHR(_PerChannelDbLevel.SetLevel((uint)channel, level, out eventContext));
        }

        public void SetLevelUniform(float level)
        {
            Guid eventContext;
            System.Threading.Thread.Sleep(5);
            try {
                Marshal.ThrowExceptionForHR(_PerChannelDbLevel.SetLevelUniform(level, out eventContext));
            } catch(Exception) {
                System.Threading.Thread.Sleep(100);
            } 
        }
    }
}