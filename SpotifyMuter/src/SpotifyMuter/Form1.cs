using System;
using System.Windows.Forms;
using Anotar.NLog;
using Model;

namespace SpotifyMuter
{
    public partial class Main : Form
    {
        private readonly WebHelperHook _webHelperHook;
        private readonly SpotifyStatusProcessor _spotifyStatusProcessor;

        public Main()
        {
            InitializeComponent();
            _webHelperHook = new WebHelperHook();
            _spotifyStatusProcessor = new SpotifyStatusProcessor();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            SpotifyStatus status = _webHelperHook.GetStatus();
            _spotifyStatusProcessor.ProcessSpotifyStatus(status);
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