using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Anotar.NLog;
using SpotifyMuter.Json;

namespace SpotifyMuter
{
    static class WebHelperHook
    {
        private const string Ua = @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
        private const string Port = ":4380";

        private static string _oauthToken;
        private static string _csrfToken;

        /**
         * Grabs the status of Spotify and returns a SpotifyStatus object.
         **/
        public static SpotifyStatus GetStatus()
        {
            string jsonString = "";
            try
            {
                jsonString = GetPage(GetUrl("/remote/status.json" + "?oauth=" + _oauthToken + "&csrf=" + _csrfToken));
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
            string json = GetPage(url);
            LogTo.Debug(json);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            _oauthToken = res.t;
        }

        public static void SetCsrf()
        {
            LogTo.Debug("Getting CSRF Token");
            string url = GetUrl("/simplecsrf/token.json");
            string json = GetPage(url);
            LogTo.Debug(json);
            CSRF res = JsonConvert.DeserializeObject<CSRF>(json);
            if (res.error != null)
            {
                _csrfToken = "";  // Block rest of CSRF calls
                System.Windows.Forms.MessageBox.Show("Error hooking Spotify. Please restart SpotifyMuter after restarting Spotify.", "Error");
                System.Windows.Forms.Application.Exit();
            }
            _csrfToken = res.token;
        }

        private static string GetUrl(string path)
        {
            return "http://127.0.0.1" + Port + path;
            //return "http://" + hostname + ".spotilocal.com" + port + path;
        }

        private static string GetPage(string url)
        {
            LogTo.Debug("Getting page " + url);
            WebClient w = new WebClient();
            w.Headers.Add("user-agent", Ua);
            w.Headers.Add("Origin", "https://open.spotify.com");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            byte[] bytes = Encoding.Default.GetBytes(w.DownloadString(url));
            return Encoding.UTF8.GetString(bytes);
        }
    }
}