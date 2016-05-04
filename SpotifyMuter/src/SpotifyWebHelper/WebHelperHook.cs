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
using Anotar.NLog;
using Model;
using Newtonsoft.Json;
using SpotifyWebHelper.Exceptions;
using Utilities;

namespace SpotifyWebHelper
{
    public class WebHelperHook
    {
        private static string _oauthToken;
        private static string _csrfToken;
        private readonly SpotifyWebHelperStarter _spotifyWebHelperStarter;

        public WebHelperHook()
        {
            _spotifyWebHelperStarter = new SpotifyWebHelperStarter();
        }

        /// <summary>
        /// Grabs the status of Spotify and returns a SpotifyStatus object.
        /// </summary>
        public SpotifyStatus GetStatus()
        {
            string jsonString = "";
            try
            {
                jsonString = JsonPageLoader.GetPage(UrlBuilder.GetUrl("/remote/status.json" + "?oauth=" + _oauthToken + "&csrf=" + _csrfToken));
                LogTo.Debug(jsonString);
            }
            catch (WebException ex)
            {
                LogTo.DebugException("WebHelperHook: ", ex);
            }

            var result = JsonConvert.DeserializeObject<SpotifyStatus>(jsonString);
            return result;
        }

        public void SetOAuth()
        {
            LogTo.Debug("Getting OAuth Token");
            _spotifyWebHelperStarter.StartWebHelper();
            string url = "https://open.spotify.com/token";
            string json = JsonPageLoader.GetPage(url);
            LogTo.Debug(json);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            _oauthToken = res.Token;
        }

        public void SetCsrf()
        {
            LogTo.Debug("Getting CSRF Token");
            string url = UrlBuilder.GetUrl("/simplecsrf/token.json");
            string json = JsonPageLoader.GetPage(url);
            LogTo.Debug(json);
            CSRF res = JsonConvert.DeserializeObject<CSRF>(json);
            if (res.HasError)
            {
                throw new SetCsrfException("Error hooking Spotify. Please restart SpotifyMuter after restarting Spotify.");
            }
            _csrfToken = res.Token;
        }
    }
}