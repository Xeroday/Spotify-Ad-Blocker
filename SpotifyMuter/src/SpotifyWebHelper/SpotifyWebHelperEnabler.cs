/* SpotifyMuter - A simple Spotify Ad Muter for Windows
 * Copyright(C) 2016 Eric Zhang, Maschmi
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
using System.IO;
using Anotar.NLog;

namespace SpotifyWebHelper
{
    public class SpotifyWebHelperEnabler
    {
        private readonly string _spotifyPrefsPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\prefs";

        /// <summary>Enable webhelper in spotify prefs</summary>
        public void EnableWebhelper()
        {
            if (File.Exists(_spotifyPrefsPath))
            {
                string[] lines = File.ReadAllLines(_spotifyPrefsPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("webhelper.enabled") && lines[i].Contains("false"))
                    {
                        lines[i] = "webhelper.enabled=true";
                        File.WriteAllLines(_spotifyPrefsPath, lines);
                        LogTo.Debug("SpotifyWebHelper enabled");
                        break;
                    }
                }
            }
        }
    }
}