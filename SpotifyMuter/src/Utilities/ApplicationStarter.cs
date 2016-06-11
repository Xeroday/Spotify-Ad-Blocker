/* SpotifyMuter - A simple Spotify Ad Muter for Windows
 * Copyright(C) 2012-2016 Eric Zhang, 2016 Maschmi
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see<http://www.gnu.org/licenses/>.*/

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Anotar.NLog;

namespace Utilities
{
    /// <summary>Class to start applications</summary>
    public class ApplicationStarter
    {
        private readonly string _appGuid =
            ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;

        /// <summary>Starts an application by executing an action. This will return, when the action returns..</summary>
        /// <param name="actionToRunApplicaton">Action which will start the application.</param>
        public void RunApplicationIfItIsNotAlreadyRunning(Action actionToRunApplicaton)
        {
            string mutexId = $"Local\\{{{_appGuid}}}"; // unique id for local mutex
            using (var mutex = new Mutex(false, mutexId))
            {
                if (mutex.WaitOne(TimeSpan.Zero))
                {
                    actionToRunApplicaton();
                    mutex.ReleaseMutex();
                }
                else
                {
                    LogTo.Debug("Application is already running. Exiting.");
                }
            }
        }
    }
}