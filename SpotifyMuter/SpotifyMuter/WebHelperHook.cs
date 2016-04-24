using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using Anotar.NLog;
using SpotifyMuter.Json;

namespace SpotifyMuter
{
    static class WebHelperHook
    {
        private static string _oauthToken;
        private static string _csrfToken;

        /// <summary>
        /// Grabs the status of Spotify and returns a SpotifyStatus object.
        /// </summary>
        public static SpotifyStatus GetStatus()
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

        public static void CheckWebHelper()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Equals("spotifywebhelper"))) // Check that SpotifyWebHelper.exe is running
            {
                return;
            }
            LogTo.Debug("Starting SpotifyWebHelper");
            if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe"))
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe");
            }
            else if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe"))
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe");
            }
        }

        public static void SetOAuth()
        {
            LogTo.Debug("Getting OAuth Token");
            CheckWebHelper();
            string url = "https://open.spotify.com/token";
            string json = JsonPageLoader.GetPage(url);
            LogTo.Debug(json);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            _oauthToken = res.Token;
        }

        public static void SetCsrf()
        {
            LogTo.Debug("Getting CSRF Token");
            string url = UrlBuilder.GetUrl("/simplecsrf/token.json");
            string json = JsonPageLoader.GetPage(url);
            LogTo.Debug(json);
            CSRF res = JsonConvert.DeserializeObject<CSRF>(json);
            if (res.Error != null)
            {
                _csrfToken = "";  // Block rest of CSRF calls
                System.Windows.Forms.MessageBox.Show("Error hooking Spotify. Please restart SpotifyMuter after restarting Spotify.", "Error");
                System.Windows.Forms.Application.Exit();
            }
            _csrfToken = res.Token;
        }
    }
}