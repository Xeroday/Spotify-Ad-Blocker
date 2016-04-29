using Newtonsoft.Json;

namespace Model
{
    public class OAuth
    {
        [JsonProperty(PropertyName = "t")]
        public string Token { get; set; } // OAuth token
    }
}
