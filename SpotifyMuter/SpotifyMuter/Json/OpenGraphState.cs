using Newtonsoft.Json;

namespace SpotifyMuter.Json
{
    class OpenGraphState
    {
        [JsonProperty(PropertyName = "private_session")]
        public bool PrivateSession { get; set; }
    }
}