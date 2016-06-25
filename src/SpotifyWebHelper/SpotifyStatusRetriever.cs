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
using Utilities;

namespace SpotifyWebHelper
{
    public class SpotifyStatusRetriever
    {
        private readonly IJsonPageLoader _jsonPageLoader;
        private readonly IUrlBuilder _urlBuilder;
        private readonly ISpotifyOAuthRetriever _spotifyOAuthRetriever;
        private readonly ISpotifyCsrfRetriever _spotifyCsrfRetriever;
        private OAuth _oauth;
        private Csrf _csrf;

        public SpotifyStatusRetriever(IJsonPageLoader jsonPageLoader,
                             IUrlBuilder urlBuilder,
                             ISpotifyOAuthRetriever spotifyOAuthRetriever,
                             ISpotifyCsrfRetriever spotifyCsrfRetriever)
        {
            _jsonPageLoader = jsonPageLoader;
            _urlBuilder = urlBuilder;
            _spotifyOAuthRetriever = spotifyOAuthRetriever;
            _spotifyCsrfRetriever = spotifyCsrfRetriever;

            Initialize();
        }

        /// <summary>
        /// Grabs the status of Spotify and returns a SpotifyStatus object.
        /// </summary>
        public SpotifyStatus RetrieveStatus()
        {
            var jsonString = _jsonPageLoader.GetPage(_urlBuilder.BuildUrl($"/remote/status.json?oauth={_oauth.Token}&csrf={_csrf.Token}"));
            LogTo.Debug(jsonString);

            var result = JsonConvert.DeserializeObject<SpotifyStatus>(jsonString);
            return result;
        }

        private void Initialize()
        {
            _oauth = _spotifyOAuthRetriever.RetrieveSpotifyOAuth();
            _csrf = _spotifyCsrfRetriever.RetrieveSpotifyCsrf();
        }
    }
}