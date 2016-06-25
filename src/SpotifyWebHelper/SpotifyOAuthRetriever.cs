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

using Anotar.NLog;
using Model;
using Newtonsoft.Json;
using SpotifyWebHelper.Exceptions;
using Utilities;
using Utilities.Exceptions;

namespace SpotifyWebHelper
{
    public class SpotifyOAuthRetriever : ISpotifyOAuthRetriever
    {
        private readonly IJsonPageLoader _jsonPageLoader;

        public SpotifyOAuthRetriever(IJsonPageLoader jsonPageLoader)
        {
            _jsonPageLoader = jsonPageLoader;
        }

        public OAuth RetrieveSpotifyOAuth()
        {
            LogTo.Debug("Getting OAuth Token");
            const string url = "https://open.spotify.com/token";
            string json;

            try
            {
                json = _jsonPageLoader.GetPage(url);
            }
            catch (JsonPageLoadingFailedException exception)
            {
                const string message = "Error connecting to spotify.com. Make sure you have internet access.";
                LogTo.DebugException(message, exception);
                throw new RetrieveOAuthException(message, exception);
            }

            LogTo.Debug(json);
            return JsonConvert.DeserializeObject<OAuth>(json);
        }
    }
}