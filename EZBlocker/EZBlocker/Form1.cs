using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private String title = String.Empty; // Title of the Spotify window
        private Boolean autoAdd = true;
        private Boolean muted = false;

        private String blocklistPath = Application.StartupPath + @"\blocklist.txt";
        private String nircmdPath = Application.StartupPath +@"\nircmdc.exe";

        private String website = @"http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";

        public Main()
        {
            InitializeComponent();
            // CheckUpdate();
            if (!File.Exists(nircmdPath))
                File.WriteAllBytes(nircmdPath, EZBlocker.Properties.Resources.nircmdc);
            if (!File.Exists(blocklistPath))
                new WebClient().DownloadFile("http://www.ericzhang.me/dl/?file=blocklist.txt", blocklistPath);
            try
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe");
            }
            catch (Exception e)
            {
                // Ignore
            }
            Mute(0); // Unmute Spotify, if muted
        }

        /**
         * Contains the logic for when to mute Spotify
         **/
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            UpdateTitle();
            if (IsPlaying())
            {
            }
        }

        /**
         * Updates the title of the Spotify window.
         * 
         * Returns true if title updated successfully, false if otherwise
         **/
        private Boolean UpdateTitle()
        {
            Process[] p = Process.GetProcesses();
            for (var i = 0; i < p.Length; i++)
            {
                if (p[i].ProcessName.Equals("spotify"))
                {
                    title = p[i].MainWindowTitle;
                    return true;
                }
            }
            return false;
        }

        /**
         * Determines whether or not Spotify is currently playing
         **/
        private Boolean IsPlaying()
        {
            return title.Contains(" - ");
        }

        /**
         * Returns the current artist
         **/
        private String GetArtist()
        {
            if (!IsPlaying()) return String.Empty;
            return title.Substring(10).Split('\u2013')[0].TrimEnd(); // Split at endash
        }

        /**
         * Adds an artist to the blocklist.
         * 
         * Returns false if Spotify is not playing.
         **/
        private Boolean AddToBlockList(String artist)
        {
            if (!IsPlaying()) return false;
            File.AppendAllText(blocklistPath, artist + "\r\n");
            return true;
        }

        /**
         * Mutes/Unmutes Spotify.
         * 
         * i: 0 = unmute, 1 = mute, 2 = toggle
         **/
        private void Mute(int i)
        {
            // http://stackoverflow.com/questions/1469764/run-command-prompt-commands
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C nircmdc muteappvolume spotify.exe " + i.ToString();
            process.StartInfo = startInfo;
            process.Start();
            if (i == 1)
                muted = true;
            else if (i == 0)
                muted = false;
            else
                muted = !muted;
        }

        /**
         * Checks if an artist is in the blocklist (Exact match only)
         **/
        private Boolean IsBlocked(String artist)
        {
            String[] lines = File.ReadAllLines(blocklistPath);
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Equals(artist))
                    return true;
            }
            return false;
        }

        /**
         * Attempts to check if the current song is an ad
         * 
         * Checks with iTunes API, can also use http://ws.spotify.com/search/1/artist?q=artist
         **/
        private Boolean IsAd(String artist)
        {
            String json = GetPage("http://itunes.apple.com/search?entity=musicArtist&limit=20&term=" + artist.Replace(" ", "+")); // Ghetto URL encoding for .net 3.5
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                {
                    if (reader.Value != null) 
                    {
                        if (reader.Value.Equals("artistName")) // If key is artistName, read next value
                        {
                            reader.ReadAsString();
                            if (reader.Value.Equals(artist)) return false; // An exact match on the artist == Not an ad
                        } 
                        else if (reader.Value.Equals("resultCount")) // If key is resultCount, read next value
                        {
                            reader.ReadAsInt32();
                            if (reader.Value.Equals(0)) return true; // No results == Ad
                        }
                    }
                }
            }
            return true;
        }

        /**
         * Gets the source of a given URL
         **/
        private String GetPage(String URL)
        {
            WebClient w = new WebClient();
            w.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36");
            String s = w.DownloadString(URL);
            return s;
        }

        /**
         * Checks if the current installation is the latest version. Prompts user if not.
         **/
        private void CheckUpdate()
        {
            int latest = Convert.ToInt32(GetPage("http://www.ericzhang.me/dl/?file=EZBlocker-version.txt"));
            int current = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            if (latest > current)
            {
                if (MessageBox.Show("There is a newer version of EZBlocker available. Would you like to upgrade?", "EZBlocker", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(website);
                    Application.Exit();   
                }
            }
        }

        private void BlockButton_Click(object sender, EventArgs e)
        {
            AddToBlockList(GetArtist());
        }

        private void AutoAddCheck_CheckedChanged(object sender, EventArgs e)
        {
            autoAdd = AutoAddCheck.Checked;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(IsAd(GetArtist()));

            // Process.Start("notepad.exe", blocklistPath);
        }

        private void MuteButton_Click(object sender, EventArgs e)
        {
            Mute(2);
        }

    }
}
