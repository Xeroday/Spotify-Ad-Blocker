using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace EZBlocker
{
    class SpotifyHook
    {
        public Process Spotify { get; private set; }
        public AudioUtils.ISimpleAudioVolume VolumeControl { get; private set; }
        public string WindowName { get; private set; }
        public IntPtr Handle { get; private set; }

        private readonly Timer RefreshTimer;
        private int SpotifyTolerance = 0;

        public SpotifyHook()
        {
            RefreshTimer = new Timer((e) =>
            {
                if (IsRunning())
                {
                    WindowName = Spotify.MainWindowTitle;
                    Handle = Spotify.MainWindowHandle;
                    if (VolumeControl == null) VolumeControl = AudioUtils.GetVolumeControl(Spotify);
                }
                else
                {
                    ClearHooks();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
        }

        public bool IsPlaying()
        {
            if (AudioUtils.GetPeakVolume(VolumeControl) > 0)
            {
                if (WindowName.Equals("Spotify") && SpotifyTolerance < 3)
                {
                    Debug.WriteLine("Tolerance " + SpotifyTolerance);
                    SpotifyTolerance++;
                    return false;
                }
                else
                {
                    SpotifyTolerance = 0;
                    return true;
                }
            }
            SpotifyTolerance = 0;
            return false;
        }

        public bool IsAdPlaying()
        {
            if (!WindowName.Equals("") && !WindowName.Equals("Drag") && IsPlaying())
            {
                if (WindowName.Equals("Spotify") && SpotifyTolerance < 3) // Prevent user pausing Spotify from being detected as ad (PeakVolume needs time to adjust)
                {
                    Debug.WriteLine("Tolerance " + SpotifyTolerance);
                    SpotifyTolerance++;
                    return false;
                }
                else if (!WindowName.Contains(" - "))
                {
                    SpotifyTolerance = 0;
                    return true;
                }
            }
            SpotifyTolerance = 0;
            return false;
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
            if (VolumeControl != null) Marshal.ReleaseComObject(VolumeControl);
            VolumeControl = null;
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

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    }
}
