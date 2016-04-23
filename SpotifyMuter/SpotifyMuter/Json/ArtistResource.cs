using Newtonsoft.Json;

namespace SpotifyMuter.Json
{
    class ArtistResource
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}