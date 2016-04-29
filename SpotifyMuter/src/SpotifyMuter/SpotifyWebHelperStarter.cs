using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Anotar.NLog;

namespace SpotifyMuter
{
    public class SpotifyWebHelperStarter
    {
        public void StartWebHelper()
        {
            if (SpotifyIsRunning())
            {
                LogTo.Debug("SpotifyWebHelper is already running");
                return;
            }

            LogTo.Debug("Starting SpotifyWebHelper");
            if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe"))
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe");
            }
            else if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe"))
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe");
            }
        }

        private static bool SpotifyIsRunning()
        {
            return Process.GetProcesses().Any(t => t.ProcessName.ToLower().Equals("spotifywebhelper"));
        }
    }
}