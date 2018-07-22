using System;
using System.Diagnostics;
using System.Threading;

namespace EZBlocker
{
    class SpotifyHook
    {
        public Process Spotify { get; private set; }
        public string WindowName { get; private set; }
        public IntPtr Handle { get; private set; }

        private readonly Timer RefreshTimer;

        public SpotifyHook()
        {
            RefreshTimer = new Timer((e) =>
            {
                if (IsRunning())
                {
                    WindowName = Spotify.MainWindowTitle;
                    Handle = Spotify.MainWindowHandle;
                }
                else
                {
                    ClearHooks();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(300));
        }

        public bool IsPlaying()
        {
            return !WindowName.Equals("") && !WindowName.Equals("Spotify");
        }

        public bool IsAdPlaying()
        {
            return IsPlaying() && !WindowName.Contains(" - ");
        }

        public bool IsRunning()
        {
            if (Spotify == null && !HookSpotify())
                return false;

            Spotify.Refresh();
            return !Spotify.HasExited;
        }

        public string GetArtist()
        {
            if (IsPlaying())
            {
                if (WindowName.Contains(" - "))
                    return WindowName.Split(new[] { " - " }, StringSplitOptions.None)[0];
                else
                    return WindowName;
            }

            return "";
        }

        private void ClearHooks()
        {
            Spotify = null;
            WindowName = "";
            Handle = IntPtr.Zero;
        }

        private bool HookSpotify()
        {
            foreach (Process p in Process.GetProcessesByName("spotify"))
            {
                if (p.MainWindowTitle.Length > 1)
                {
                    Spotify = p;
                    return true;
                }
            }
            return false;
        }
    }

}
