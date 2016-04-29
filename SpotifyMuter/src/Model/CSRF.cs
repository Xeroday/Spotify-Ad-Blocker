using Newtonsoft.Json;

namespace Model
{
    public class CSRF
    {
        [JsonProperty(PropertyName = "error")]
        public Error Error { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
