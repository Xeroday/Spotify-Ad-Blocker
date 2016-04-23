using Newtonsoft.Json;

namespace SpotifyMuter.Json
{
    class Error
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}