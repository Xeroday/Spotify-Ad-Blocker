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

using System.Linq;
using Anotar.NLog;
using AudioSwitcher.AudioApi.CoreAudio;

namespace SpotifyMuter
{
    internal class SpotifyMuter
    {
        private bool? _isSpotifyMuted;

        public void UnMute()
        {
            if (!_isSpotifyMuted.HasValue
                || _isSpotifyMuted.Value)
            {
                LogTo.Debug("Unmuting ad");
                Mute(false);
            }
        }

        public void Mute()
        {
            if (!_isSpotifyMuted.HasValue
                || !_isSpotifyMuted.Value)
            {
                LogTo.Debug("Muting ad");
                Mute(true);
            }
        }

        private void Mute(bool mute)
        {
            var audioController = new CoreAudioController();
            var defaultDevice = audioController.GetDefaultDevice(AudioSwitcher.AudioApi.DeviceType.Playback, AudioSwitcher.AudioApi.Role.Multimedia);
            var sessions = defaultDevice.SessionController.ActiveSessions().ToList();

            for (int sessionId = 0; sessionId < sessions.Count; sessionId++)
            {
                var currentSession = sessions.ElementAt(sessionId);
                string displayName = currentSession.DisplayName;
                if (displayName == "Spotify")
                {
                    _isSpotifyMuted = mute;
                    currentSession.IsMuted = mute;
                }
            }
        }
    }
}