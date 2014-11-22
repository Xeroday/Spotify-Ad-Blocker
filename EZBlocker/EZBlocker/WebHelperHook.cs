using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using Newtonsoft.Json;
using System.IO;


namespace EZBlocker
{
    class WebHelperHook
    {
        private const string ua = @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.65 Safari/537.36";
        private const string port = ":4380";

        private static string oauthToken;
        private static string csrfToken;

        /**
         * Checks if currently playing song is an ad.
         * Returns 1 if is an ad, 0 if not an ad, -1 if error.
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
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("\"track_type\""))
                    {
                        if (line.Contains("\"ad\""))
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            // If we're here, there was no track_type, so error in query?
            oauthToken = null;
            csrfToken = null;
            return -1;
        }

        private static void SetOAuth()
        {
            String url = "http://open.spotify.com/token";
            String json = GetPage(url);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            oauthToken = res.t; 
        }

        private static void SetCSRF()
        {
            String url = GetURL("/simplecsrf/token.json");
            String json = GetPage(url);
            CSRF res = JsonConvert.DeserializeObject<CSRF>(json);
            csrfToken = res.token; 
        }

        private static string GetURL(string path)
        {
            return "http://" + new Random(Environment.TickCount).Next(100000, 100000000).ToString() + ".spotilocal.com" + port + path;
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
