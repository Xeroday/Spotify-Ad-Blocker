using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace EZBlocker
{
    class Analytics
    {
        private const string url = "https://www.google-analytics.com/collect";
        private const string tid = "UA-42480515-3";

        private readonly HttpClient client = new HttpClient();
        private readonly string cid;
        private readonly string cm;
        private readonly string ul;
        private readonly string cs;

        public Analytics(string clientId, string version) {
            cid = clientId;
            cm = version;
            ul = CultureInfo.CurrentCulture.Name;
            cs = Environment.OSVersion.ToString();
        }

        public static string GenerateCID()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<bool> LogAction(string action)
        { 
            // https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
            if (!action.StartsWith("/"))
            {
                action = "/" + action;
            }
            var data = new Dictionary<string, string>
            {
                { "v", "1" },
                { "tid", "UA-42480515-3" },
                { "t", "pageview" },
                { "cid", cid }, // client id
                { "cm", cm }, // Campaign medium, EZBlocker version
                { "cn", "EZBlocker Windows" }, // Campaign name
                { "cs", cs}, // Campaign source, OS Version
                { "ul", ul }, // User Language
                { "dh", "ezblocker.ericzhang.me" },
                { "dp", action },
            };

            var content = new FormUrlEncodedContent(data);
            var resp = await client.PostAsync(url, content);

            return resp.IsSuccessStatusCode;
        }
    }
}
