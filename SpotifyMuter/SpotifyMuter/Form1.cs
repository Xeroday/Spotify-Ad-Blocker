﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Anotar.NLog;
using AudioSwitcher.AudioApi.CoreAudio;

namespace SpotifyMuter
{
    public partial class Main : Form
    {
        private bool _muted;
        private string _lastArtistName = "";

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
                WebHelperResult result = WebHelperHook.GetStatus();

                if (result.isAd) // Track is ad
                {
                    if (result.isPlaying)
                    {
                        LogTo.Debug("Ad is playing");
                        if (_lastArtistName != result.artistName)
                        {
                            if (!_muted) Mute(true);
                            LogTo.Debug("Muting ad");
                            _lastArtistName = result.artistName;
                            LogTo.Debug("Blocked " + result.artistName);
                        }
                    }
                    else // Ad is paused
                    {
                        LogTo.Debug("Ad is paused");
                        Resume();
                    }
                }
                else if (result.isPrivateSession)
                {
                    if (_lastArtistName != result.artistName)
                    {
                        LogTo.Debug("Playing: *Private Session*");
                        _lastArtistName = result.artistName;
                        MessageBox.Show("Please disable 'Private Session' on Spotify for SpotifyMuter to function properly.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                }
                else if (!result.isRunning)
                {
                    // Notify("Error connecting to Spotify. Retrying...");
                    LogTo.Debug("Not running.");
                    MainTimer.Interval = 5000;
                    /*
                    MainTimer.Enabled = false;
                    MessageBox.Show("Spotify is not running. Please restart SpotifyMuter after starting Spotify.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    StatusLabel.Text = "Spotify is not running";
                    Application.Exit();
                    */
                }
                else if (!result.isPlaying)
                {
                    LogTo.Debug("Spotify is paused");
                    _lastArtistName = "";
                }
                else // Song is playing
                {
                    if (_muted) Mute(false);
                    if (MainTimer.Interval > 1000) MainTimer.Interval = 1000;
                    if (_lastArtistName != result.artistName)
                    {
                        LogTo.Debug("Playing: " + result.artistName);
                        _lastArtistName = result.artistName;
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

        private void Notify(String message)
        {
            NotifyIcon.ShowBalloonTip(10000, "SpotifyMuter", message, ToolTipIcon.None);
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
            Notify("SpotifyMuter is hidden. Double-click this icon to close it.");

            LogTo.Debug("Window was hidden to tray.");
        }

        private void Main_Load(object sender, EventArgs e)
        {
            NlogConfiguration.Configure();

            var webhelperEnabler = new WebhelperEnabler();
            webhelperEnabler.EnableWebhelper();

            // Give SpotifyMuter higher priority
            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure SpotifyMuter runs smoothly
            }
            catch (Exception ex)
            {
                LogTo.DebugException("Error: ", ex);
            }

            bool unsafeHeaders = WebHelperHook.SetAllowUnsafeHeaderParsing20();
            LogTo.Debug("Unsafe Headers: " + unsafeHeaders);

            Mute(false);

            AddContextMenu();

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