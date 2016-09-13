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
using SpotifyMuter.Logic;
using SpotifyWebHelper;
using Utilities;

namespace SpotifyMuter
{
    public partial class MainForm : Form
    {
        private readonly SpotifyStatusRetriever _spotifyStatusRetriever;
        private readonly SpotifyStatusProcessor _spotifyStatusProcessor;
        private readonly NotifyIconManager _notifyIconManager;

        public MainForm()
        {
            InitializeComponent();
            var pageLoader = new JsonPageLoader();
            var urlBuilder = new UrlBuilder();
            _spotifyStatusRetriever = new SpotifyStatusRetriever(pageLoader, urlBuilder, new SpotifyOAuthRetriever(pageLoader), new SpotifyCsrfRetriever(pageLoader, urlBuilder));
            _spotifyStatusProcessor = new SpotifyStatusProcessor(new Logic.SpotifyMuter());
            _notifyIconManager = new NotifyIconManager();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            var status = _spotifyStatusRetriever.RetrieveStatus();
            _spotifyStatusProcessor.ProcessSpotifyStatus(status);
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
            _notifyIconManager.AddContextMenu(Close);

            SubscribeToStatusProcessorEvents();

            MainTimer.Enabled = true;

            HideWindow();
        }

        private void SubscribeToStatusProcessorEvents()
        {
            _spotifyStatusProcessor.SpotifyMuted += SpotifyMuted;
            _spotifyStatusProcessor.SpotifyUnmuted += SpotifyUnmuted;
        }

        private void SpotifyMuted(object sender, EventArgs e)
        {
            _notifyIconManager.SetMutedTrayIcon();
        }

        private void SpotifyUnmuted(object sender, EventArgs e)
        {
            _notifyIconManager.SetUnmutedTrayIcon();
        }
    }
}