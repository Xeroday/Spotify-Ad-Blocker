/* SpotifyMuter - A simple Spotify Ad Muter for Windows
 * Copyright(C) 2016 Maschmi
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

using Anotar.NLog;
using System;
using System.Windows.Forms;
using Utilities;

namespace SpotifyMuter
{
    /// <summary>Class for application handling</summary>
    public class ApplicationHandler : IDisposable
    {
        private readonly Form _form;
        private readonly ApplicationStarter _applicationStarter;

        public ApplicationHandler(Form form)
        {
            _form = form;
            _applicationStarter = new ApplicationStarter();
        }

        /// <summary>Starts an application by opening the form. This call will return, when the form is closed.</summary>
        public void RunApplicationIfItIsNotAlreadyRunning()
        {
            _applicationStarter.RunApplicationIfItIsNotAlreadyRunning(() => Application.Run(_form));
        }

        public void AddExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            Application.ThreadException += ApplicationThreadException;
        }

        private void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        private void HandleException(Exception e)
        {
            var message = $"An Error occured. SpotifyMuter will be closed.\n\n{e.Message}";
            LogTo.DebugException(message, e);
            MessageBox.Show(message, "SpotifyMuter - Error");
            _form.Close();
        }

        private void RemoveExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainUnhandledException;
            Application.ThreadException -= ApplicationThreadException;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveExceptionHandler();
            }
        }
    }
}