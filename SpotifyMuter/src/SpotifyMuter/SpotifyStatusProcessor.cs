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