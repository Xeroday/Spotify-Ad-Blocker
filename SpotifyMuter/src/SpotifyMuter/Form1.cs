using System;
using System.Windows.Forms;
using Anotar.NLog;
using Model;

namespace SpotifyMuter
{
    public partial class Main : Form
    {
        private readonly SpotifyMuter _spotifyMuter;
        private readonly WebHelperHook _webHelperHook;

        public Main()
        {
            InitializeComponent();
            _spotifyMuter = new SpotifyMuter();
            _webHelperHook = new WebHelperHook();
        }

        /// <summary>
        /// Contains the logic for when to mute Spotify
        /// </summary>
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            SpotifyStatus result = _webHelperHook.GetStatus();

            if (ResultContainsError(result)
                || SpotifyIsInPrivateSession(result))
            {
                return;
            }

            if (!result.NextEnabled) // Track is ad
            {
                MuteAd(result);
            }
            else
            {
                UnmuteAd(result);
            }
        }

        private static bool ResultContainsError(SpotifyStatus result)
        {
            if (result.HasError)
            {
                LogTo.Debug($"Error {result.Error.Type}: {result.Error.Message}");
                return true;
            }
            return false;
        }

        private static bool SpotifyIsInPrivateSession(SpotifyStatus result)
        {
            if (result.IsPrivateSession)
            {
                LogTo.Debug("Playing: *Private Session*");
                MessageBox.Show("Please disable 'Private Session' on Spotify for SpotifyMuter to function properly.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions) 0x40000);
                return true;
            }
            return false;
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

        /// <summary>Close on double click</summary>
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void HideWindow()
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;

            LogTo.Debug("Window was hidden to tray.");
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var webhelperEnabler = new SpotifyWebHelperEnabler();
            webhelperEnabler.EnableWebhelper();

            AddContextMenu();

            _webHelperHook.SetOAuth();
            _webHelperHook.SetCsrf();

            MainTimer.Enabled = true;

            HideWindow();
        }

        /// <summary>Add ContextMenu to tray</summary>
        private void AddContextMenu()
        {
            ContextMenu trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", (o, args) => { Close(); });
            NotifyIcon.ContextMenu = trayMenu;
        }
    }
}