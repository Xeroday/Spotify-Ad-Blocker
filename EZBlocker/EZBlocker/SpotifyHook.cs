﻿using System;
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

        public SpotifyHook()
        {
            RefreshTimer = new Timer((e) =>
            {
                if (IsRunning())
                {
                    WindowName = Spotify.MainWindowTitle;
                    Handle = Spotify.MainWindowHandle;
                    VolumeControl = AudioUtils.GetVolumeControl(Spotify);
                }
                else
                {
                    ClearHooks();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(300));
        }

        public bool IsPlaying()
        {
            return AudioUtils.GetPeakVolume(VolumeControl) > 0;
        }

        public bool IsAdPlaying()
        {
            return IsPlaying() && !WindowName.Contains(" - ") && !WindowName.Equals("Drag") && !string.IsNullOrWhiteSpace(WindowName);
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
            Marshal.ReleaseComObject(VolumeControl);
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
