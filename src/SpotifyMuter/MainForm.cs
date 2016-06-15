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
using View;

namespace SpotifyMuter
{
    public partial class MainForm : Form
    {
        private readonly WebHelperHook _webHelperHook;
        private readonly SpotifyStatusProcessor _spotifyStatusProcessor;

        public MainForm()
        {
            InitializeComponent();
            _webHelperHook = new WebHelperHook();
            _spotifyStatusProcessor = new SpotifyStatusProcessor(new Logic.SpotifyMuter());
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            var status = _webHelperHook.GetStatus();
            _spotifyStatusProcessor.ProcessSpotifyStatus(status);
        }

        /// <summary>Close on double click</summary>
        /// <param name="sender">sender who raised the event</param>
        /// <param name="eventArgs">MouseEvent arguments</param>
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs eventArgs)
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
            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", (o, args) => { Close(); });
            trayMenu.MenuItems.Add("About", (o, args) => { new AboutWindow().Show(); });
            NotifyIcon.ContextMenu = trayMenu;
        }
    }
}