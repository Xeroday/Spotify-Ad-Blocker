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

using System;
using Anotar.NLog;
using Model;

namespace SpotifyMuter.Logic
{
    public class SpotifyStatusProcessor
    {
        private readonly ISpotifyMuter _spotifyMuter;

        public SpotifyStatusProcessor(ISpotifyMuter spotifyMuter)
        {
            _spotifyMuter = spotifyMuter;
        }

        public delegate void SpotifyMutedEventHandler(object sender, EventArgs e);
        public event SpotifyMutedEventHandler SpotifyMuted;

        public delegate void SpotifyUnmutedEventHandler(object sender, EventArgs e);
        public event SpotifyUnmutedEventHandler SpotifyUnmuted;

        /// <summary>
        /// Contains the logic for when to mute Spotify
        /// </summary>
        /// <param name="status">SpotifyStatus to process</param>
        public void ProcessSpotifyStatus(ISpotifyStatus status)
        {
            if (status.HasError || status.SpotifyIsInPrivateSession)
            {
                return;
            }

            if (!status.Playing)
            {
                LogTo.Debug("Spotify is paused");
                return;
            }

            if (!status.NextEnabled) // Track is ad
            {
                MuteAd();
                OnSpotifyMuted();
            }
            else
            {
                UnmuteAd(status);
                OnSpotifyUnmuted();
            }
        }

        private void MuteAd()
        {
            LogTo.Debug("Ad is playing");
            _spotifyMuter.Mute();
        }

        private void UnmuteAd(ISpotifyStatus status)
        {
            _spotifyMuter.Unmute();

            if (status.Track.ArtistResource != null)
            {
                LogTo.Debug($"Playing: {status.Track.ArtistResource.Name} - {status.Track.TrackResource.Name}");
            }
        }

        private void OnSpotifyMuted()
        {
            SpotifyMuted?.Invoke(this, new EventArgs());
        }

        private void OnSpotifyUnmuted()
        {
            SpotifyUnmuted?.Invoke(this, new EventArgs());
        }
    }
}