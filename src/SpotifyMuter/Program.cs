﻿/* SpotifyMuter - A simple Spotify Ad Muter for Windows
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
using Anotar.NLog;
using Utilities;
using System.Windows.Forms;

namespace SpotifyMuter
{
    internal static class Program
    {
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main()
        {
            NlogConfiguration.Configure();

            LogTo.Debug("Starting SpotifyMuter.");

            using (var applicationHandler = new ApplicationExceptionHandler())
            {
                applicationHandler.AddExceptionHandler();

                var applicationStarter = new ApplicationStarter();
                using (var form = new MainForm())
                {
                    applicationStarter.RunApplicationIfItIsNotAlreadyRunning(() => Application.Run(form));
                }
            }

            LogTo.Debug("SpotifyMuter was closed. Exiting");
        }
    }
}