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
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using Anotar.NLog;

namespace SpotifyMuter
{
    static class Program
    {
        private static readonly string AppGuid =
            ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            NlogConfiguration.Configure();

            LogTo.Debug("Starting SpotifyMuter.");

            string mutexId = $"Local\\{{{AppGuid}}}"; // unique id for local mutex
            using (var mutex = new Mutex(false, mutexId))
            {
                if (mutex.WaitOne(TimeSpan.Zero))
                {
                    Application.Run(new Main());
                    mutex.ReleaseMutex();
                }
                else
                {
                    LogTo.Debug("SpotifyMuter is already running. Exiting.");
                }
            }

            LogTo.Debug("SpotifyMuter was closed. Exiting");
        }
    }
}