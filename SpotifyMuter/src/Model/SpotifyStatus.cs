﻿using Anotar.NLog;
using Newtonsoft.Json;

namespace Model
{
    public class SpotifyStatus
    {
        [JsonProperty(PropertyName = "error")]
        public Error Error { get; set; }

        [JsonProperty(PropertyName = "running")]
        public bool Running { get; set; }

        [JsonProperty(PropertyName = "playing")]
        public bool Playing { get; set; }

        [JsonProperty(PropertyName = "next_enabled")]
        public bool NextEnabled { get; set; }

        [JsonProperty(PropertyName = "open_graph_state")]
        public OpenGraphState OpenGraphState { get; set; }

        [JsonProperty(PropertyName = "playing_position")]
        public float PlayingPosition { get; set; }

        [JsonProperty(PropertyName = "track")]
        public Track Track { get; set; }

        public bool HasError
        {
            get
            {
                if (Error != null)
                {
                    LogTo.Debug($"Error {Error.Type}: {Error.Message}");
                    return true;
                }
                return false;
            }
        }

        public bool SpotifyIsInPrivateSession
        {
            get
            {
                if (OpenGraphState.PrivateSession)
                {
                    LogTo.Debug("Playing: *Private Session*");
                    return true;
                }
                return false;
            }
        }
    }
}