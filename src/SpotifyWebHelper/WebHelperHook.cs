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

using System.Net;
using Anotar.NLog;
using Model;
using Newtonsoft.Json;
using SpotifyWebHelper.Exceptions;
using Utilities;
using Utilities.Exceptions;

namespace SpotifyWebHelper
{
    public class WebHelperHook
    {
        private readonly IJsonPageLoader _jsonPageLoader;
        private readonly IUrlBuilder _urlBuilder;
        private string _oauthToken;
        private string _csrfToken;

        public WebHelperHook(IJsonPageLoader jsonPageLoader, IUrlBuilder urlBuilder)
        {
            _jsonPageLoader = jsonPageLoader;
            _urlBuilder = urlBuilder;
        }

        /// <summary>
        /// Grabs the status of Spotify and returns a SpotifyStatus object.
        /// </summary>
        public SpotifyStatus GetStatus()
        {
            var jsonString = "";
            try
            {
                jsonString = _jsonPageLoader.GetPage(_urlBuilder.GetUrl($"/remote/status.json?oauth={_oauthToken}&csrf={_csrfToken}"));
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
            const string url = "https://open.spotify.com/token";
            var json = _jsonPageLoader.GetPage(url);
            LogTo.Debug(json);
            var res = JsonConvert.DeserializeObject<OAuth>(json);
            _oauthToken = res.Token;
        }

        public void SetCsrf()
        {
            LogTo.Debug("Getting CSRF Token");
            var url = _urlBuilder.GetUrl("/simplecsrf/token.json");
            string json;

            try
            {
                json = _jsonPageLoader.GetPage(url);
                LogTo.Debug(json);
            }
            catch (JsonPageLoadingFailedException exception)
            {
                const string message = "Error hooking Spotify. Make sure you enable 'Allow Spotify to be opened from the web'. Please restart SpotifyMuter.";
                LogTo.DebugException(message, exception);
                throw new SetCsrfException(message, exception);
            }

            var csrf = JsonConvert.DeserializeObject<Csrf>(json);
            _csrfToken = csrf.Token;
        }
    }
}