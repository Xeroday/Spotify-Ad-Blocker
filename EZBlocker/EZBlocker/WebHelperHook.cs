using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;


namespace EZBlocker
{
    class WebHelperHook
    {
        private const string ua = @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.65 Safari/537.36";
        private const string port = ":4380";

        private static string oauthToken;
        private static string csrfToken;
        private static string hostname;

        /**
         * Checks if currently playing song is an ad.
         * Returns:
         * 0 if not an ad
         * 1 if is an ad
         * 2 if is an ad but paused
         * -1 if not playing
         * -2 if Spotify is not running
         **/
        public static int isAd()
        {
            if (oauthToken == null || oauthToken == "null")
            {
                SetOAuth();
            }
            if (csrfToken == null || csrfToken == "null")
            {
                SetCSRF();
            }
            
            string result = GetPage(GetURL("/remote/status.json" + "?oauth=" + oauthToken + "&csrf=" + csrfToken));
            Console.WriteLine(result);
            
            bool isRunning = false;
            bool isPlaying = false;
            bool isTrackAd = false;

            // Process data
            using (StringReader reader = new StringReader(result))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("\"running\":"))
                    {
                        isRunning = line.Contains("true");
                    }
                    else if (line.Contains("\"track_type\":"))
                    {
                        isTrackAd = line.Contains("\"ad\"");
                    }
                    else if (line.Contains("\"playing\":"))
                    {
                        isPlaying = line.Contains("true");
                    }
                }
            }

            // Ad checking logic
            if (isTrackAd)
            {
                if (isPlaying)
                    return 1;
                else // Spotify is paused
                    return 2;
            }
            if (!isPlaying)
            {
                return -1;
            }
            if (!isRunning)
            {
                return -2;
            }
            return 0;
        }

        private static void CheckWebHelper()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Equals("spotifywebhelper"))) // Check that SpotifyWebHelper.exe is running
            {
                return;
            }
            try
            {
                Console.WriteLine("Starting SpotifyWebHelper");
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe");
            }
            catch {
                MessageBox.Show("Please check 'Allow Spotify to be started from the Web' in your Spotify preferences.", "EZBlocker");
            }

        }

        private static void SetOAuth()
        {
            Console.WriteLine("Getting OAuth Token");
            CheckWebHelper();
            String url = "http://open.spotify.com/token";
            String json = GetPage(url);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            oauthToken = res.t; 
        }

        private static void SetCSRF()
        {
            Console.WriteLine("Getting CSRF Token");
            String url = GetURL("/simplecsrf/token.json");
            String json = GetPage(url);
            CSRF res = JsonConvert.DeserializeObject<CSRF>(json);
            csrfToken = res.token; 
        }

        private static string GetURL(string path)
        {
            if (hostname == null)
            {
                hostname = new Random(Environment.TickCount).Next(100000, 100000000).ToString();
            }
            return "http://" + hostname + ".spotilocal.com" + port + path;
        }

        private static string GetPage(string URL)
        {
            WebClient w = new WebClient();
            w.Headers.Add("user-agent", ua);
            w.Headers.Add("Origin", "https://open.spotify.com");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string s = w.DownloadString(URL);
            return s;
        }
    }
}
