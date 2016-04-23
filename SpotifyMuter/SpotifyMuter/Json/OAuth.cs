using Newtonsoft.Json;

namespace SpotifyMuter.Json
{
    class OAuth
    {
        [JsonProperty(PropertyName = "t")]
        public string Token { get; set; } // OAuth token
    }
}
