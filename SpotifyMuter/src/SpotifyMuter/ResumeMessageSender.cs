using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Anotar.NLog;

namespace SpotifyMuter
{
    public class ResumeMessageSender
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646275%28v=vs.85%29.aspx
        private const int WM_APPCOMMAND = 0x319;
        private const int MEDIA_PLAYPAUSE = 0xE0000;
        
        /// <summary>
        /// Resumes playing Spotify
        /// </summary>
        public void Resume(IntPtr handle)
        {
            LogTo.Debug("Resuming Spotify");
            SendMessage(GetHandle(), WM_APPCOMMAND, handle, (IntPtr)MEDIA_PLAYPAUSE);
        }

        /// <summary>
        /// Gets the Spotify process handle
        /// </summary>
        private IntPtr GetHandle()
        {
            foreach (Process process in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Contains("spotify")))
            {
                if (process.MainWindowTitle.Length > 1)
                {
                    return process.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }
    }
}