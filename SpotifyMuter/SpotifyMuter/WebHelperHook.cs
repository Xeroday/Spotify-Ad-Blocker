using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Anotar.NLog;
using SpotifyMuter.Json;

namespace SpotifyMuter
{
    class WebHelperHook
    {
        private const string ua = @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
        private const string port = ":4380";

        private static string oauthToken;
        private static string csrfToken;
        private static string hostname;

        /**
         * Grabs the status of Spotify and returns a SpotifyStatus object.
         **/
        public static SpotifyStatus GetStatus()
        {
            if (oauthToken == null || oauthToken == "null")
            {
                SetOAuth();
            }
            if (csrfToken == null || csrfToken == "null")
            {
                SetCSRF();
            }

            string jsonString = "";
            try
            {
                jsonString = GetPage(GetURL("/remote/status.json" + "?oauth=" + oauthToken + "&csrf=" + csrfToken));
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

        private static void SetOAuth()
        {
            LogTo.Debug("Getting OAuth Token");
            CheckWebHelper();
            String url = "https://open.spotify.com/token";
            String json = GetPage(url);
            LogTo.Debug(json);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            oauthToken = res.t;
        }

        private static void SetCSRF()
        {
            LogTo.Debug("Getting CSRF Token");
            String url = GetURL("/simplecsrf/token.json");
            String json = GetPage(url);
            LogTo.Debug(json);
            if (json.Contains("\"error\":"))
            {
                csrfToken = "";  // Block rest of CSRF calls
                System.Windows.Forms.MessageBox.Show("Error hooking Spotify. Please restart SpotifyMuter after restarting Spotify.", "Error");
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
            return "http://127.0.0.1" + port + path;
            //return "http://" + hostname + ".spotilocal.com" + port + path;
        }

        private static string GetPage(string URL)
        {
            LogTo.Debug("Getting page " + URL);
            WebClient w = new TimedWebClient();
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
}