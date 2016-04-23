using Newtonsoft.Json;

namespace SpotifyMuter.Json
{
    class CSRF
    {
        [JsonProperty(PropertyName = "error")]
        public Error Error { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
