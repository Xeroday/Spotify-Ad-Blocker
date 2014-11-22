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
    /// <summary>
    /// Property Store class, only supports reading properties at the moment.
    /// </summary>
    public class PropertyStore
    {
        private IPropertyStore _Store;

        public int Count
        {
            get
            {
                int Result;
                Marshal.ThrowExceptionForHR(_Store.GetCount(out Result));
                return Result;
            }
        }

        public PropertyStoreProperty this[int index]
        {
            get
            {
                PropVariant result;
                PROPERTYKEY key = Get(index);
                Marshal.ThrowExceptionForHR(_Store.GetValue(ref key, out result));
                return new PropertyStoreProperty(key, result);
            }
        }

        public bool Contains(PROPERTYKEY testKey)
        {
            for (int i = 0; i < Count; i++)
            {
                PROPERTYKEY key = Get(i);
                if (key.fmtid == testKey.fmtid && key.pid == testKey.pid)
                    return true;
            }
            return false;
        }

        public PropertyStoreProperty this[PROPERTYKEY testKey]
        {
            get
            {
                PropVariant result;
                for (int i = 0; i < Count; i++)
                {
                    PROPERTYKEY key = Get(i);
                    if (key.fmtid == testKey.fmtid && key.pid == testKey.pid)
                    {
                        Marshal.ThrowExceptionForHR(_Store.GetValue(ref key, out result));
                        return new PropertyStoreProperty(key, result);
                    }
                }
                return null;
            }
        }

        public PROPERTYKEY Get(int index)
        {
            PROPERTYKEY key;
            Marshal.ThrowExceptionForHR( _Store.GetAt(index, out key));
            return key;
        }

        public PropVariant GetValue(int index)
        {
            PropVariant result;
            PROPERTYKEY key = Get(index);
            Marshal.ThrowExceptionForHR(_Store.GetValue(ref key, out result));
            return result;
        }

        internal PropertyStore(IPropertyStore store)
        {
            _Store = store;
        }
    }
}
