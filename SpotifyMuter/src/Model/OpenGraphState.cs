using Newtonsoft.Json;

namespace Model
{
    public class OpenGraphState
    {
        [JsonProperty(PropertyName = "private_session")]
        public bool PrivateSession { get; set; }
    }
}