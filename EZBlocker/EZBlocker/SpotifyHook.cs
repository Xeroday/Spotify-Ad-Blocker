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
        private int SpotifyAdTolerance = 0;

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
            return !WindowName.Equals("") && !WindowName.Equals("Drag") && (AudioUtils.GetPeakVolume(VolumeControl) > 0 || WindowName.Contains(" - "));
        }

        public bool IsAdPlaying()
        {
            if (IsPlaying())
            {
                if (WindowName.Equals("Spotify") && SpotifyAdTolerance < 3) // Prevent user pausing Spotify from being detected as ad (PeakVolume needs time to adjust)
                {
                    Debug.WriteLine("Tolerance " + SpotifyAdTolerance);
                    SpotifyAdTolerance++;
                    return false;
                }
                else if (!WindowName.Contains(" - "))
                {
                    return true;
                }
            }
            SpotifyAdTolerance = 0;
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
    }

}
