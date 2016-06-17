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

using System.Net;
using System.Text;
using Anotar.NLog;
using Utilities.Exceptions;

namespace Utilities
{
    public class JsonPageLoader : IJsonPageLoader
    {
        private const string Ua = @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";

        public string GetPage(string url)
        {
            LogTo.Debug("Getting page " + url);
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", Ua);
                client.Headers.Add("Origin", "https://open.spotify.com");
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                try
                {
                    var bytes = Encoding.Default.GetBytes(client.DownloadString(url));
                    return Encoding.UTF8.GetString(bytes);
                }
                catch (WebException exception)
                {
                    var message = $"Getting page {url} failed.";
                    LogTo.DebugException(message, exception);
                    throw new JsonPageLoadingFailedException(message, exception);
                }
            }
        }
    }
}