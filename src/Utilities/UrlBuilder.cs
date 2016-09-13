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

namespace Utilities
{
    public class UrlBuilder : IUrlBuilder
    {
        private const string Port = ":4371";
        private string hostname = new Random(Environment.TickCount).Next(100000, 100000000).ToString();

        public string BuildUrl(string path)
        {
            return "https://" + hostname + ".spotilocal.com" + Port + path;
        }
    }
}