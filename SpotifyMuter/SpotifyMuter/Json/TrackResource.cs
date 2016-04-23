using Newtonsoft.Json;

namespace SpotifyMuter.Json
{
    class TrackResource
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}