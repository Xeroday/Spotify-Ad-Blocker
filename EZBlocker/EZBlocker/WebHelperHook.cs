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

            string result = "";

            WebHelperResult whr = new WebHelperResult();

            try
            {
                result = GetPage(GetURL("/remote/status.json" + "?oauth=" + oauthToken + "&csrf=" + csrfToken));
                Debug.WriteLine(result);
            }
            catch (WebException ex)
            {
                Debug.WriteLine(ex);
                File.AppendAllText(Main.logPath, "WebHelperHook: " + ex.Message + "\r\n");
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
                    // else if (line.Contains("\"track_type\":"))
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
                    /*else if (line.Contains("\"playing_position\":"))
                    {
                        if (!line.Contains("0,")) // Song isn't at 0 position
                            whr.position = Convert.ToSingle(line.Split(new char[] { ':', ',' })[1]);
                    }*/
                    else if (line.Contains("\"length\":"))
                    {
                        whr.length = Convert.ToInt32(line.Split(new char[] { ':', ',' })[1]);
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
                    else if (line.Contains("\"album_resource\":"))
                    {
                        while ((line = reader.ReadLine()) != null) // Read until we find the "name" field
                        {
                            if (line.Contains("\"name\":"))
                            {
                                whr.albumName = (line.Replace("\"name\":", "").Split('"')[1]);
                                break;
                            }
                        }
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
                    else if (line.Contains("Invalid Csrf token"))
                    {
                        Debug.WriteLine("Invalid CSRF token");
                        SetCSRF();
                    }
                    else if (line.Contains("Invalid OAuth token"))
                    {
                        Debug.WriteLine("Invalid OAuth token");
                        SetOAuth();
                    }
                }
            }

            return whr;
        }

        public static void CheckWebHelper()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Equals("spotifywebhelper"))) // Check that SpotifyWebHelper.exe is running
            {
                return;
            }
            Debug.WriteLine("Starting SpotifyWebHelper");
            if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe"))
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe");
            }
            else if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe"))
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe");
            }
        }

        private static void SetOAuth()
        {
            Debug.WriteLine("Getting OAuth Token");
            CheckWebHelper();
            String url = "https://open.spotify.com/token";
            String json = GetPage(url);
            Debug.WriteLine(json);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            oauthToken = res.t;
        }

        private static void SetCSRF()
        {
            Debug.WriteLine("Getting CSRF Token");
            String url = GetURL("/simplecsrf/token.json");
            String json = GetPage(url);
            Debug.WriteLine(json);
            if (json.Contains("\"error\":"))
            {
                csrfToken = "";  // Block rest of CSRF calls
                System.Windows.Forms.MessageBox.Show("Error hooking Spotify. Please restart EZBlocker after restarting Spotify.", "Error");
                System.Windows.Forms.Application.Exit();
            }
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
            Debug.WriteLine("Getting page " + URL);
            WebClient w = new WebClient();

            if (URL.Contains("spotilocal"))
            {
                w.Proxy = null;
            }

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

        public static bool SetAllowUnsafeHeaderParsing20()
        {
            //Get the assembly that contains the internal class
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    class TimedWebClient : WebClient
    {
        // Timeout in milliseconds, default = 600,000 msec
        public int Timeout { get; set; }
        private readonly CookieContainer m_container = new CookieContainer();

        public TimedWebClient()
        {
            this.Timeout = 30 * 1000;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = this.Timeout;
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = m_container;
                webRequest.KeepAlive = false;
            }
            return request;
        }
    }

}
