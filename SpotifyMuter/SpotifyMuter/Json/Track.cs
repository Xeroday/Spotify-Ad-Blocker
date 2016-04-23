using Newtonsoft.Json;

namespace SpotifyMuter.Json
{
    class Track
    {
        [JsonProperty(PropertyName = "artist_resource")]
        public ArtistResource ArtistResource { get; set; }

        [JsonProperty(PropertyName = "track_resource")]
        public TrackResource TrackResource { get; set; }

        [JsonProperty(PropertyName = "track_type")]
        public string TrackType { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
    }
}