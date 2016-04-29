using System.Net;
using Newtonsoft.Json;
using Anotar.NLog;
using Model;

namespace SpotifyMuter
{
    public class WebHelperHook
    {
        private static string _oauthToken;
        private static string _csrfToken;
        private readonly SpotifyWebHelperStarter _spotifyWebHelperStarter;

        public WebHelperHook()
        {
            _spotifyWebHelperStarter = new SpotifyWebHelperStarter();
        }

        /// <summary>
        /// Grabs the status of Spotify and returns a SpotifyStatus object.
        /// </summary>
        public SpotifyStatus GetStatus()
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

        public void SetOAuth()
        {
            LogTo.Debug("Getting OAuth Token");
            _spotifyWebHelperStarter.StartWebHelper();
            string url = "https://open.spotify.com/token";
            string json = JsonPageLoader.GetPage(url);
            LogTo.Debug(json);
            OAuth res = JsonConvert.DeserializeObject<OAuth>(json);
            _oauthToken = res.Token;
        }

        public void SetCsrf()
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