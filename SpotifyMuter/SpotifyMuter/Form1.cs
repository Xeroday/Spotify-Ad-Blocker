using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Anotar.NLog;
using AudioSwitcher.AudioApi.CoreAudio;
using SpotifyMuter.Json;

namespace SpotifyMuter
{
    public partial class Main : Form
    {
        private bool _muted;
        private readonly ResumeMessageSender _resumeMessageSender;

        public Main()
        {
            InitializeComponent();
            _resumeMessageSender = new ResumeMessageSender();
        }

        /**
         * Contains the logic for when to mute Spotify
         **/
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                SpotifyStatus result = WebHelperHook.GetStatus();

                if (result.error != null)
                {
                    LogTo.Debug($"Error {result.error.type}: {result.error.message}");
                }
                else
                {
                    if (!result.next_enabled) // Track is ad
                    {
                        if (result.playing)
                        {
                            LogTo.Debug("Ad is playing");

                            if (!_muted)
                            {
                                LogTo.Debug("Muting ad");
                                Mute(true);
                            }
                        }
                        else // Ad is paused
                        {
                            LogTo.Debug("Ad is paused");
                            _resumeMessageSender.Resume(Handle);
                        }
                    }
                    else
                    {
                        if (result.open_graph_state.private_session)
                        {
                            LogTo.Debug("Playing: *Private Session*");
                            MessageBox.Show("Please disable 'Private Session' on Spotify for SpotifyMuter to function properly.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                        }
                        else
                        {
                            if (!result.playing)
                            {
                                LogTo.Debug("Spotify is paused");
                            }
                            else // Song is playing
                            {
                                if (_muted)
                                {
                                    Mute(false);
                                }
                                if (result.track.artist_resource != null)
                                {
                                    LogTo.Debug($"Playing: {result.track.artist_resource.name} - {result.track.track_resource.name}");
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
                    _muted = mute;
                    currentSession.IsMuted = mute;
                }
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
            NlogConfiguration.Configure();

            var webhelperEnabler = new WebhelperEnabler();
            webhelperEnabler.EnableWebhelper();

            Mute(false);

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