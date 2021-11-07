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
        private readonly Timer RefreshTimer;
        private GlobalSystemMediaTransportControlsSessionManager SessionManager;
        private GlobalSystemMediaTransportControlsSession SpotifyMediaSession;
        public string CurrentArtist { get; private set; }
        public bool IsAdPlaying { get; private set; }
        public bool IsPlaying { get; private set; }

        public MediaHook()
        {
            RefreshTimer = new Timer((e) =>
            {
                _ = UpdateMediaInfo();
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(2000));
        }

        private async Task RegisterSpotifyMediaSession()
        {
            if (SessionManager == null)
            {
                SessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            }
            foreach (GlobalSystemMediaTransportControlsSession session in SessionManager.GetSessions())
            {
                if (session.SourceAppUserModelId == "Spotify.exe")
                {
                    Debug.WriteLine("Registering " + session.GetHashCode());
                    AudioUtils.SetSpotifyMute(false);
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
                await RegisterSpotifyMediaSession();
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
