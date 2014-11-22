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

/* Created by Xavier Flix (2010/11/18) */

using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAudio
{
    [Flags]
    public enum AUDCLNT_RETURNFLAGS : uint
    {
        S_OK = 0,
        AUDCLNT_E_NOT_INITIALIZED = 0x001,
        AUDCLNT_E_ALREADY_INITIALIZED = 0x002,
        AUDCLNT_E_WRONG_ENDPOINT_TYPE = 0x003,
        AUDCLNT_E_DEVICE_INVALIDATED = 0x004,
        AUDCLNT_E_NOT_STOPPED = 0x005,
        AUDCLNT_E_BUFFER_TOO_LARGE = 0x006,
        AUDCLNT_E_OUT_OF_ORDER = 0x007,
        AUDCLNT_E_UNSUPPORTED_FORMAT = 0x008,
        AUDCLNT_E_INVALID_SIZE = 0x009,
        AUDCLNT_E_DEVICE_IN_USE = 0x00a,
        AUDCLNT_E_BUFFER_OPERATION_PENDING = 0x00b,
        AUDCLNT_E_THREAD_NOT_REGISTERED = 0x00c,
        AUDCLNT_E_EXCLUSIVE_MODE_NOT_ALLOWED = 0x00e,
        AUDCLNT_E_ENDPOINT_CREATE_FAILED = 0x00f,
        AUDCLNT_E_SERVICE_NOT_RUNNING = 0x010,
        AUDCLNT_E_EVENTHANDLE_NOT_EXPECTED = 0x011,
        AUDCLNT_E_EXCLUSIVE_MODE_ONLY = 0x012,
        AUDCLNT_E_BUFDURATION_PERIOD_NOT_EQUAL = 0x013,
        AUDCLNT_E_EVENTHANDLE_NOT_SET = 0x014,
        AUDCLNT_E_INCORRECT_BUFFER_SIZE = 0x015,
        AUDCLNT_E_BUFFER_SIZE_ERROR = 0x016,
        AUDCLNT_E_CPUUSAGE_EXCEEDED = 0x017,
        AUDCLNT_E_BUFFER_ERROR = 0x018,
        AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED = 0x019,
        AUDCLNT_E_INVALID_DEVICE_PERIOD = 0x020,
        AUDCLNT_S_BUFFER_EMPTY = 0x001,
        AUDCLNT_S_THREAD_ALREADY_REGISTERED = 0x002,
        AUDCLNT_S_POSITION_STALLED = 0x003
    }
}
