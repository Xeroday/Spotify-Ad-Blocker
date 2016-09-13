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
    public class SpotifyCsrfRetriever : ISpotifyCsrfRetriever
    {
        private readonly IJsonPageLoader _jsonPageLoader;
        private readonly IUrlBuilder _urlBuilder;

        public SpotifyCsrfRetriever(IJsonPageLoader jsonPageLoader, IUrlBuilder urlBuilder)
        {
            _jsonPageLoader = jsonPageLoader;
            _urlBuilder = urlBuilder;
        }

        public Csrf RetrieveSpotifyCsrf()
        {
            LogTo.Debug("Getting CSRF Token");
            var url = _urlBuilder.BuildUrl("/simplecsrf/token.json");
            string json;

            try
            {
                json = _jsonPageLoader.GetPage(url);
            }
            catch (JsonPageLoadingFailedException exception)
            {
                const string message = "Error hooking Spotify. Please restart SpotifyMuter.";
                LogTo.DebugException(message, exception);
                throw new RetrieveCsrfException(message, exception);
            }

            LogTo.Debug(json);
            return JsonConvert.DeserializeObject<Csrf>(json);
        }
    }
}