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
        private const string ua = @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.111 Safari/537.36";
        private const string port = ":4380";

        private static string oauthToken;
        private static string csrfToken;
        private static string hostname;
        private static bool warningMessageShown;

        /**
         * Grabs the status of Spotify and returns a WebHelperResult object.
         **/
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

            string result = GetPage(GetURL("/remote/status.json" + "?oauth=" + oauthToken + "&csrf=" + csrfToken));
            Console.WriteLine(result);

            WebHelperResult whr = new WebHelperResult();

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
                    // else if (line.Contains("\"track_type\":"))
                    else if (line.Contains("\"next_enabled\":"))
                    {
                        whr.isAd = line.Contains("false");
                    }
                    else if (line.Contains("\"playing\":"))
                    {
                        whr.isPlaying = line.Contains("true");
                    }
                    /*else if (line.Contains("\"playing_position\":"))
                    {
                        if (!line.Contains("0,")) // Song isn't at 0 position
                            whr.position = Convert.ToSingle(line.Split(new char[] { ':', ',' })[1]);
                    }
                    else if (line.Contains("\"length\":"))
                    {
                        whr.length = Convert.ToInt32(line.Split(new char[] { ':', ',' })[1]);
                    }*/
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
                    else if (line.Contains("Invalid Csrf token"))
                    {
                        csrfToken = null;
                    }
                }
            }

            return whr;
        }

        public static void CheckWebHelper()
        {
            bool running;
            try
            {
                // Check if SpotifyWebHelper.exe is running
                running = Process.GetProcesses().Any(t => t.ProcessName.ToLower().Equals("spotifywebhelper"));
                if (!running)
                {
                    Console.WriteLine("Starting SpotifyWebHelper");
                    string appDataFolder = Environment.GetEnvironmentVariable("APPDATA");
                    if (appDataFolder != null)
                    {
                        string helperExe = Path.Combine(appDataFolder, @"Spotify\Data\SpotifyWebHelper.exe");
                        if (!File.Exists(helperExe))
                        {
                            helperExe = Path.Combine(appDataFolder, @"Spotify\SpotifyWebHelper.exe");
                        }
                      
                        if (File.Exists(helperExe))
                        {
                            Process.Start(helperExe);
                            running = true;
                        }
                    }
                    else if (!warningMessageShown)
                    {
                        warningMessageShown = true;
                        MessageBox.Show("Unable to find App Data folder to start SpotifyWebHelper.exe.", "EZBlocker");
                    }
                }
            }
            catch
            {
                running = false;
            }

            if (!running && !warningMessageShown)
            {
                warningMessageShown = true;
                MessageBox.Show("Please check 'Allow Spotify to be started from the Web' in your Spotify preferences.", "EZBlocker");
            }
        }

        private static void SetOAuth()
        {
            Console.WriteLine("Getting OAuth Token");
            CheckWebHelper();
            String url = "https://open.spotify.com/token";
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
            return "http://127.0.0.1" + port + path;
            //return "http://" + hostname + ".spotilocal.com" + port + path;
        }

        private static string GetPage(string URL)
        {
            WebClient w = new WebClient();
            w.Headers.Add("user-agent", ua);
            w.Headers.Add("Origin", "https://open.spotify.com");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            byte[] bytes = Encoding.Default.GetBytes(w.DownloadString(URL));
            return Encoding.UTF8.GetString(bytes);
        }

        private static string RemoveDiacritics(string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            string normalized = value.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            Encoding nonunicode = Encoding.GetEncoding(850);
            Encoding unicode = Encoding.Unicode;

            byte[] nonunicodeBytes = Encoding.Convert(unicode, nonunicode, unicode.GetBytes(sb.ToString()));
            char[] nonunicodeChars = new char[nonunicode.GetCharCount(nonunicodeBytes, 0, nonunicodeBytes.Length)];
            nonunicode.GetChars(nonunicodeBytes, 0, nonunicodeBytes.Length, nonunicodeChars, 0);

            return new string(nonunicodeChars);
        }
    }

}
