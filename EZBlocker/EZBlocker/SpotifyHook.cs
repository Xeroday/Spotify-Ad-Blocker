using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.Media.Control;

namespace EZBlocker
{
    class SpotifyHook
    {
        public Process Spotify { get; private set; }
        private List<int> Children;
        public List<AudioUtils.VolumeControl> VolumeControls { get; private set; }
        public string WindowName { get; private set; }
        public IntPtr Handle { get; private set; }

        private GlobalSystemMediaTransportControlsSession gsmtcs;
        private bool IsNextEnabled = true;

        private readonly Timer RefreshTimer;

        public SpotifyHook()
        {
            RefreshTimer = new Timer((e) =>
            {
                if (IsRunning() && gsmtcs != null)
                {
                    WindowName = Spotify.MainWindowTitle;
                    Handle = Spotify.MainWindowHandle;
                    IsNextEnabled = gsmtcs.GetPlaybackInfo().Controls.IsNextEnabled;
                    Debug.WriteLine("Hook: Next " + IsNextEnabled);
                }
                else
                {
                    RefreshHooks();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(300));
        }

        public bool IsPlaying()
        {
            Debug.WriteLine("Hook: " + gsmtcs.GetPlaybackInfo().PlaybackStatus);
            return gsmtcs.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
        }

        public bool IsAdPlaying()
        {
            if (WindowName.Equals("Advertisement") || !IsNextEnabled && IsPlaying())
            {
                Debug.WriteLine("Hook: " + "IsAdPlaying true");
                return true;
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
            if (VolumeControls != null)
            {
                foreach (AudioUtils.VolumeControl volumeControl in VolumeControls)
                {
                    Marshal.ReleaseComObject(volumeControl.Control);
                }
                VolumeControls = null;
            }
            gsmtcs = null;
        }

        private bool HookSpotify()
        {
            // Hook windows media controller
            try
            {
                gsmtcs = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult().GetCurrentSession();
            } catch (Exception e)
            {
                Debug.WriteLine("Hook: " + e);
            }

            // Hook processes
            Children = new List<int>();
            foreach (Process p in Process.GetProcessesByName("spotify"))
            {
                Children.Add(p.Id);
                if (p.MainWindowTitle.Length > 1 && Spotify == null)
                {
                    Spotify = p;
                }
            }

            // Hook audio controls
            VolumeControls = new List<AudioUtils.VolumeControl>();
            foreach (int child in Children)
            {
                AudioUtils.VolumeControl volumeControl = AudioUtils.GetVolumeControl(child);
                if (volumeControl != null)
                {
                    if (Spotify == null) Spotify = Process.GetProcessById(volumeControl.ProcessId);
                    VolumeControls.Add(volumeControl);
                }
            }

            if (VolumeControls.Count > 0) return true;
            
            return false;
        }

        public void RefreshHooks()
        {
            ClearHooks();
            HookSpotify();
        }
    }
}
