using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace EZBlocker
{
    class SpotifyHook
    {
        public Process Spotify { get; private set; }
        private HashSet<int> Children;
        public AudioUtils.VolumeControl VolumeControl { get; private set; }
        public string WindowName { get; private set; }
        public IntPtr Handle { get; private set; }

        private readonly Timer RefreshTimer;
        private float peak = 0f;
        private float lastPeak = 0f;

        public SpotifyHook()
        {
            RefreshTimer = new Timer((e) =>
            {
                if (IsRunning())
                {
                    WindowName = Spotify.MainWindowTitle;
                    Handle = Spotify.MainWindowHandle;
                    if (VolumeControl == null)
                    {
                        VolumeControl = AudioUtils.GetVolumeControl(Children);
                    }
                    else
                    {
                        lastPeak = peak;
                        peak = AudioUtils.GetPeakVolume(VolumeControl.Control);
                    }
                }
                else
                {
                    ClearHooks();
                    HookSpotify();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
        }

        public bool IsPlaying()
        {
            return peak > 0 && lastPeak > 0;
        }

        public bool IsAdPlaying()
        {
            if (!WindowName.Equals("") && !WindowName.Equals("Drag") && IsPlaying())
            {
                if (WindowName.Equals("Spotify")) // Prevent user pausing Spotify from being detected as ad (PeakVolume needs time to adjust)
                {
                    Debug.WriteLine("Ad1: " + lastPeak.ToString() + " " + peak.ToString());
                    return true;
                }
                else if (!WindowName.Contains(" - "))
                {
                    Debug.WriteLine("Ad2: " + lastPeak.ToString() + " " + peak.ToString());
                    return true;
                }
            }
            return false;
        }

        public bool IsRunning()
        {
            if (Spotify == null)
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
            if (VolumeControl != null) Marshal.ReleaseComObject(VolumeControl.Control);
            VolumeControl = null;
        }

        private bool HookSpotify()
        {
            Children = new HashSet<int>();

            // Try hooking through window title
            foreach (Process p in Process.GetProcessesByName("spotify"))
            {
                Children.Add(p.Id);
                Spotify = p;
                if (p.MainWindowTitle.Length > 1)
                {
                    return true;
                }
            }

            // Try hooking through audio device
            VolumeControl = AudioUtils.GetVolumeControl(Children);
            if (VolumeControl != null)
            {
                Spotify = Process.GetProcessById(VolumeControl.ProcessId);
                return true;
            }

            return false;
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    }
}
