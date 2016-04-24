using System.Linq;
using AudioSwitcher.AudioApi.CoreAudio;

namespace SpotifyMuter
{
    public class SpotifyMuter
    {
        private bool _isSpotifyMuted;

        public bool IsSpotifyMuted
        {
            set { _isSpotifyMuted = value; }
            get { return _isSpotifyMuted; }
        }

        public void UnMute()
        {
            if (IsSpotifyMuted)
            {
                Mute(false);
            }
        }

        public void Mute()
        {
            if (!IsSpotifyMuted)
            {
                Mute(true);
            }
        }

        private void Mute(bool mute)
        {
            var audioController = new CoreAudioController();
            var defaultDevice = audioController.GetDefaultDevice(AudioSwitcher.AudioApi.DeviceType.Playback, AudioSwitcher.AudioApi.Role.Multimedia);
            var sessions = defaultDevice.SessionController.ActiveSessions();

            for (int sessionId = 0; sessionId < sessions.Count(); sessionId++)
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