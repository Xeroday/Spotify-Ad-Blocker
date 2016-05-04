/* SpotifyMuter - A simple Spotify Ad Muter for Windows
 * Copyright(C) 2016 Maschmi
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see<http://www.gnu.org/licenses/>.*/
using Anotar.NLog;
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