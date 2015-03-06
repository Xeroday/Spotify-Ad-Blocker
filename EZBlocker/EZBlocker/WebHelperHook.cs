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
         * Returns 1 if is an ad, 2 if is an ad but paused, 0 if not an ad, -1 if error.
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
            using (StringReader reader = new StringReader(result))
            {
                string line;
                Boolean isPlaying = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("\"track_type\":"))
                    {
                        if (line.Contains("\"ad\""))
                        {
                            if (isPlaying)
                                return 1;
                            else
                                return 2;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else if (line.Contains("\"playing\":"))
                    {
                        isPlaying = line.Contains("true");
                    }
                }
            }
            // If we're here, there was no track_type, so error in query?
            oauthToken = null;
            csrfToken = null;
            return -1;
        }

        private static void CheckWebHelper()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Equals("spotifywebhelper"))) // Check that SpotifyWebHelper.exe is running
            {
                return;
            }
            // MessageBox.Show("It is recommended that you enable 'Allow Spotify to be started from the Web' in your Spotify preferences.", "EZBlocker");
            try
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe");
            }
            catch { }

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
