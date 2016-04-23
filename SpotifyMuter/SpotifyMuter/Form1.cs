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

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646275%28v=vs.85%29.aspx
        private const int WM_APPCOMMAND = 0x319;
        private const int MEDIA_PLAYPAUSE = 0xE0000;

        public Main()
        {
            InitializeComponent();
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
                            Resume();
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
                                    LogTo.Debug("Playing: {0} - {1}", result.track.artist_resource.name, result.track.track_resource.name);
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

        /**
         * Resumes playing Spotify
         **/
        private void Resume()
        {
            LogTo.Debug("Resuming Spotify");
            SendMessage(GetHandle(), WM_APPCOMMAND, Handle, (IntPtr)MEDIA_PLAYPAUSE);
        }

        /**
         * Gets the Spotify process handle
         **/
        private IntPtr GetHandle()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Contains("spotify")))
            {
                if (t.MainWindowTitle.Length > 1)
                    return t.MainWindowHandle;
            }
            return IntPtr.Zero;
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
            WebHelperHook.SetCSRF();

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