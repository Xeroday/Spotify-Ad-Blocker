using Newtonsoft.Json;

namespace Model
{
    public class ArtistResource
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}