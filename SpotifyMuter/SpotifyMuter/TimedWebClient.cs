using System;
using System.Net;

namespace SpotifyMuter
{
    class TimedWebClient : WebClient
    {
        // Timeout in milliseconds, default = 600,000 msec
        public int Timeout { get; set; }
        private readonly CookieContainer _container = new CookieContainer();

        public TimedWebClient()
        {
            Timeout = 30 * 1000;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = Timeout;
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = _container;
                webRequest.KeepAlive = false;
            }
            return request;
        }
    }
}