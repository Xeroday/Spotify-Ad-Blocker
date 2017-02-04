using SpotifyMuter.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;
using View;

namespace SpotifyMuter
{
    internal class NotifyIconManager : IDisposable
    {
        bool _disposed;
        private readonly NotifyIcon _notifyIcon;

        public NotifyIconManager()
        {
            _notifyIcon = new NotifyIcon
            {
                Visible = true
            };
            SetUnmutedTrayIcon();
        }

        public void SetMutedTrayIcon()
        {
            SetTrayIcon(Resources.volume_off);
        }

        public void SetUnmutedTrayIcon()
        {
            SetTrayIcon(Resources.volume_up);
        }

        /// <summary>Add ContextMenu to tray</summary>
        /// <param name="closeAction">Action to execute for exit menuitem</param>
        public void AddContextMenu(Action closeAction)
        {
            if (closeAction == null)
            {
                throw new ArgumentNullException(nameof(closeAction));
            }

            _notifyIcon.MouseDoubleClick += (o, args) => closeAction();

            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("About", (o, args) => { new AboutWindow().Show(); });
            trayMenu.MenuItems.Add("Exit", (o, args) => { closeAction(); });
            _notifyIcon.ContextMenu = trayMenu;
        }

        private void SetTrayIcon(Icon icon)
        {
            _notifyIcon.Icon = icon;
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free managed objects here.
                _notifyIcon.Dispose();
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }
        #endregion
    }
}