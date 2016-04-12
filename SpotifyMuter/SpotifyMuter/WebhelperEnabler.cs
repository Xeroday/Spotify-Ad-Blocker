using System;
using System.IO;

namespace SpotifyMuter
{
    public class WebhelperEnabler
    {
        private readonly string _spotifyPrefsPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\prefs";

        /// <summary>Enable webhelper in spotify prefs</summary>
        public void EnableWebhelper()
        {
            if (File.Exists(_spotifyPrefsPath))
            {
                String[] lines = File.ReadAllLines(_spotifyPrefsPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("webhelper.enabled") && lines[i].Contains("false"))
                    {
                        lines[i] = "webhelper.enabled=true";
                        File.WriteAllLines(_spotifyPrefsPath, lines);
                        break;
                    }
                }
            }
        }
    }
}