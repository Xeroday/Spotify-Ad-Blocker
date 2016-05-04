/* SpotifyMuter - A simple Spotify Ad Muter for Windows
 * Copyright(C) 2012-2016 Eric Zhang, 2016 Maschmi
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
using Model;

namespace SpotifyMuter
{
    public class SpotifyStatusProcessor
    {
        private readonly SpotifyMuter _spotifyMuter;

        public SpotifyStatusProcessor()
        {
            _spotifyMuter = new SpotifyMuter();
        }

        /// <summary>
        /// Contains the logic for when to mute Spotify
        /// </summary>
        public void ProcessSpotifyStatus(SpotifyStatus status)
        {
            if (status.HasError || status.SpotifyIsInPrivateSession)
            {
                return;
            }

            if (!status.NextEnabled) // Track is ad
            {
                MuteAd(status);
            }
            else
            {
                UnmuteAd(status);
            }
        }

        private void MuteAd(SpotifyStatus result)
        {
            if (result.Playing)
            {
                LogTo.Debug("Ad is playing");
                _spotifyMuter.Mute();
            }
            else
            {
                LogTo.Debug("Ad is paused.");
            }
        }

        private void UnmuteAd(SpotifyStatus result)
        {
            if (result.Playing)
            {
                _spotifyMuter.UnMute();

                if (result.Track.ArtistResource != null)
                {
                    LogTo.Debug($"Playing: {result.Track.ArtistResource.Name} - {result.Track.TrackResource.Name}");
                }
            }
            else
            {
                LogTo.Debug("Spotify is paused");
            }
        }
    }
}