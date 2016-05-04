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

namespace Utilities
{
    public static class UrlBuilder
    {
        private const string Port = ":4380";

        public static string GetUrl(string path)
        {
            return "http://127.0.0.1" + Port + path;
            //return "http://" + hostname + ".spotilocal.com" + port + path;
        }
    }
}