using System;
using System.Windows.Forms;
using Anotar.NLog;
using SpotifyMuter.Json;

namespace SpotifyMuter
{
    public partial class Main : Form
    {
        private readonly ResumeMessageSender _resumeMessageSender;
        private readonly SpotifyMuter _spotifyMuter;

        public Main()
        {
            InitializeComponent();
            _resumeMessageSender = new ResumeMessageSender();
            _spotifyMuter = new SpotifyMuter();
        }

        /// <summary>
        /// Contains the logic for when to mute Spotify
        /// </summary>
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                SpotifyStatus result = WebHelperHook.GetStatus();

                if (result.Error != null)
                {
                    LogTo.Debug($"Error {result.Error.Type}: {result.Error.Message}");
                }
                else
                {
                    if (!result.NextEnabled) // Track is ad
                    {
                        if (result.Playing)
                        {
                            LogTo.Debug("Ad is playing");
                            LogTo.Debug("Muting ad");
                            _spotifyMuter.Mute();
                        }
                        else // Ad is paused
                        {
                            LogTo.Debug("Ad is paused");
                            _resumeMessageSender.Resume(Handle);
                        }
                    }
                    else
                    {
                        if (result.OpenGraphState.PrivateSession)
                        {
                            LogTo.Debug("Playing: *Private Session*");
                            MessageBox.Show("Please disable 'Private Session' on Spotify for SpotifyMuter to function properly.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                        }
                        else
                        {
                            if (!result.Playing)
                            {
                                LogTo.Debug("Spotify is paused");
                            }
                            else // Song is playing
                            {
                                _spotifyMuter.UnMute();

                                if (result.Track.ArtistResource != null)
                                {
                                    LogTo.Debug($"Playing: {result.Track.ArtistResource.Name} - {result.Track.TrackResource.Name}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTo.DebugException("Error", ex);
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
            var webhelperEnabler = new WebhelperEnabler();
            webhelperEnabler.EnableWebhelper();

            AddContextMenu();

            WebHelperHook.SetOAuth();
            WebHelperHook.SetCsrf();

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