using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace EZBlocker
{
    class MediaHook
    {
        private readonly Timer RefreshMediaTimer;
        private readonly Timer RefreshSessionTimer;
        private GlobalSystemMediaTransportControlsSessionManager SessionManager;
        private GlobalSystemMediaTransportControlsSession SpotifyMediaSession;
        public string CurrentArtist { get; private set; }
        public bool IsAdPlaying { get; private set; }
        public bool IsPlaying { get; private set; }

        public MediaHook()
        {
            RefreshMediaTimer = new Timer((e) =>
            {
                _ = UpdateMediaInfo();
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(2000));

            RefreshSessionTimer = new Timer((e) =>
            {
                _ = RegisterSpotifyMediaSession(false);
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10000));
        }

        private async Task RegisterSpotifyMediaSession(bool unmute)
        {
            if (SessionManager == null)
            {
                SessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            }
            List<GlobalSystemMediaTransportControlsSession> sessions = new List<GlobalSystemMediaTransportControlsSession>();
            sessions.Add(SessionManager.GetCurrentSession());
            sessions.AddRange(SessionManager.GetSessions());
            foreach (GlobalSystemMediaTransportControlsSession session in sessions)
            {
                if (session != null && ((session.SourceAppUserModelId == "Spotify.exe") || (session.SourceAppUserModelId == "SpotifyAB.SpotifyMusic_zpdnekdrzrea0!Spotify")))
                {
                    Debug.WriteLine("Registering " + session.GetHashCode());
                    if (unmute) AudioUtils.SetSpotifyMute(false);
                    SpotifyMediaSession = session;
                    SpotifyMediaSession.MediaPropertiesChanged += async (s, args) =>
                    {
                        await UpdateMediaInfo();
                    };
                    return;
                }
            }
            SpotifyMediaSession = null;
        }

        private async Task UpdateMediaInfo()
        {
            try
            {
                if (SpotifyMediaSession == null) throw new Exception();
                GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties = await SpotifyMediaSession.TryGetMediaPropertiesAsync();
                GlobalSystemMediaTransportControlsSessionPlaybackControls mediaControls = SpotifyMediaSession.GetPlaybackInfo().Controls;

                CurrentArtist = mediaProperties.Artist.Length > 0 ? mediaProperties.Artist : mediaProperties.Title;
                IsAdPlaying = !mediaControls.IsNextEnabled || mediaProperties.Title == "Advertisement";
                IsPlaying = mediaControls.IsPauseEnabled;
            } catch (Exception e)
            {
                Debug.WriteLine("UpdateMediaInfo exception " + e.ToString());
                CurrentArtist = "N/A";
                IsAdPlaying = false;
                IsPlaying = false;
                await RegisterSpotifyMediaSession(true);
            } 
        }

        public void SendNextTrack()
        {
            _ = SpotifyMediaSession.TryPlayAsync();
        }

        public bool IsRunning()
        {
            return (SpotifyMediaSession != null);
        }
    }
}
