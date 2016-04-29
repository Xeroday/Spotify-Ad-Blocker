using Newtonsoft.Json;

namespace Model
{
    public class TrackResource
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}