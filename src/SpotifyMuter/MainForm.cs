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

using System;
using System.Windows.Forms;
using Anotar.NLog;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;

namespace SpotifyMuter
{
    public partial class MainForm : Form
    {
        private readonly NotifyIconManager _notifyIconManager;
        private SpotifyLocalAPI _spotifyLocalApi;
        private readonly Logic.SpotifyMuter _spotifyMuter;

        public MainForm()
        {
            InitializeComponent();
            _spotifyMuter = new Logic.SpotifyMuter();
            _notifyIconManager = new NotifyIconManager();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            _notifyIconManager.AddContextMenu(Close);
            _spotifyLocalApi = SetupSpotifyLocalAPI();
            CheckForAdOnStartup();
            HideWindow();
        }

        private void HideWindow()
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;

            LogTo.Debug("Window was hidden to tray.");
        }

        private void CheckForAdOnStartup()
        {
            var status = _spotifyLocalApi.GetStatus();
            MuteSpotify(status.Track);
        }

        private SpotifyLocalAPI SetupSpotifyLocalAPI()
        {
            var spotifyLocalApi = new SpotifyLocalAPI();
            if (!spotifyLocalApi.Connect())
            {
                throw new InvalidProgramException("Can not connect to Spotify.");
            }

            spotifyLocalApi.OnTrackChange += OnTrackChange;
            spotifyLocalApi.ListenForEvents = true;

            return spotifyLocalApi;
        }

        private void OnTrackChange(object sender, TrackChangeEventArgs e)
        {
            MuteSpotify(e.NewTrack);
        }

        private void MuteSpotify(Track track)
        {
            if (track.IsAd())
            {
                _spotifyMuter.Mute();
                SpotifyMuted();
            }
            else
            {
                _spotifyMuter.Unmute();
                SpotifyUnmuted();
            }
        }

        private void SpotifyMuted()
        {
            _notifyIconManager.SetMutedTrayIcon();
        }

        private void SpotifyUnmuted()
        {
            _notifyIconManager.SetUnmutedTrayIcon();
        }
    }
}