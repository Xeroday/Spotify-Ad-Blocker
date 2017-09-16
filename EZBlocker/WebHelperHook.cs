using System;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;

namespace EZBlocker
{
    class WebHelperHook
    {
        private const string ua = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36)";
        private const string port = ":4380";

        private static string oauthToken;
        private static string csrfToken;
        private static string hostname;

        private static void SetOAuth()
        {
            String url = "https://open.spotify.com/token";
            String json = GetPage(url);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            oauthToken = res.T;
        }

        private static void SetCSRF()
        {
            String url = GetURL("/simplecsrf/token.json");
            String json = GetPage(url);
            if (json.Contains("\"error\":"))
            {
                csrfToken = "";  // Block rest of CSRF calls
                System.Windows.Forms.MessageBox.Show("Error hooking Spotify. Please restart EZBlocker after restarting Spotify.", "Error");
                System.Windows.Forms.Application.Exit();
            }
            CSRF res = JsonConvert.DeserializeObject<CSRF>(json);
            csrfToken = res.Token;
        }

        private static string GetURL(string path)
        {
            if (hostname == null)
            {
                hostname = new Random(Environment.TickCount).Next(100000, 100000000).ToString();
            }
            return "http://" + hostname + ".spotilocal.com" + port + path;
        }

        public static string GetPage(string URL)
        {
            WebClient wc = new WebClient();

            wc.Headers.Add("user-agent", ua);

            if (URL.Contains("spotilocal"))
            {
                wc.Proxy = null;
                wc.Headers.Add("Origin", "https://open.spotify.com");
            }

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string page = wc.DownloadString(URL);
            return page;
        }

        public static WebHelperResult GetStatus()
        {
            if (oauthToken == null || oauthToken == "null")
            {
                SetOAuth();
            }
            if (csrfToken == null || csrfToken == "null")
            {
                SetCSRF();
            }

            string result = "";

            WebHelperResult whr = new WebHelperResult();

            try
            {
                result = GetPage(GetURL("/remote/status.json" + "?oauth=" + oauthToken + "&csrf=" + csrfToken));
            }
            catch (WebException ex)
            {
                whr.isRunning = false;
                return whr;
            }

            // Process data
            using (StringReader reader = new StringReader(result))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("\"running\":"))
                    {
                        whr.isRunning = line.Contains("true");
                    }
                    else if (line.Contains("\"next_enabled\":"))
                    {
                        whr.isAd = line.Contains("false");
                    }
                    else if (line.Contains("\"private_session\":"))
                    {
                        whr.isPrivateSession = line.Contains("true");
                    }
                    else if (line.Contains("\"playing\":"))
                    {
                        whr.isPlaying = line.Contains("true");
                    }
                    else if (line.Contains("\"length\":"))
                    {
                        whr.length = Convert.ToInt32(line.Split(new char[] { ':', ',' })[1]);
                    }
                    else if (line.Contains("\"track_resource\":"))
                    {
                        while ((line = reader.ReadLine()) != null) // Read until we find the "name" field
                        {
                            if (line.Contains("\"name\":"))
                            {
                                whr.songName = (line.Replace("\"name\":", "").Split('"')[1]);
                                break;
                            }
                        }
                    }
                    else if (line.Contains("\"artist_resource\":"))
                    {
                        while ((line = reader.ReadLine()) != null) // Read until we find the "name" field
                        {
                            if (line.Contains("\"name\":"))
                            {
                                whr.artistName = (line.Replace("\"name\":", "").Split('"')[1]);
                                break;
                            }
                        }
                    }
                    else if (line.Contains("Invalid CSRF token"))
                    {
                        SetCSRF();
                    }
                    else if (line.Contains("Invalid OAuth token"))
                    {
                        SetOAuth();
                    }
                    else if (line.Contains("Expired OAuth token"))
                    {
                        SetOAuth();
                    }
                }
            }

            return whr;
        }
    }
}