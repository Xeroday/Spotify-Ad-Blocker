using System.Net;
using System.Text;
using Anotar.NLog;

namespace SpotifyMuter
{
    static class JsonPageLoader
    {
        private const string Ua = @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";

        public static string GetPage(string url)
        {
            LogTo.Debug("Getting page " + url);
            WebClient w = new WebClient();
            w.Headers.Add("user-agent", Ua);
            w.Headers.Add("Origin", "https://open.spotify.com");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(w.DownloadString(url));
                return Encoding.UTF8.GetString(bytes);
            }
            catch (WebException exception)
            {
                LogTo.DebugException($"Getting page {url} failed", exception);
                throw;
            }
        }
    }
}