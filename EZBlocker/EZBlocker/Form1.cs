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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace EZBlocker
{

    public partial class Main : Form
    {
        private String title = String.Empty; // Title of the Spotify window
        private String lastChecked = String.Empty; // Previous artist
        private Boolean autoAdd = true;
        private Boolean notify = true;
        private Boolean muted = false;

        private String blocklistPath = Application.StartupPath + @"\blocklist.txt";
        private String nircmdPath = Application.StartupPath + @"\nircmdc.exe";
        private String jsonPath = Application.StartupPath + @"\Newtonsoft.Json.dll";

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int MEDIA_PLAYPAUSE = 0xE0000;
        
        private const String ua = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
        private const String website = @"http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";

        public Main()
        {
            CheckUpdate();
            if (!File.Exists(nircmdPath))
                File.WriteAllBytes(nircmdPath, EZBlocker.Properties.Resources.nircmdc);
            if (!File.Exists(jsonPath))
                File.WriteAllBytes(jsonPath, EZBlocker.Properties.Resources.Newtonsoft_Json);
            if (!File.Exists(blocklistPath))
            {
                WebClient w = new WebClient();
                w.Headers.Add("user-agent", "EZBlocker " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion);
                w.DownloadFile("http://www.ericzhang.me/dl/?file=blocklist.txt", blocklistPath);
            }
            InitializeComponent();
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
                String artist = GetArtist();
                if (!lastChecked.Equals(artist)) // Song has changed
                {
                    lastChecked = artist;
                    if (autoAdd) // Auto add to block list
                    {
                        if (!IsInBlocklist(artist) && IsAd(artist))
                        {
                            AddToBlockList(artist);
                            Notify("Automatically added " + artist + " to your blocklist.");
                        }
                    }
                    if (IsInBlocklist(artist)) // Should mute
                    {
                        if (!muted)
                            Mute(1); // Mute Spotify
                        ResumeTimer.Start();
                        Console.WriteLine("Muted " + artist);
                        // Notify(artist + " is on your blocklist and has been muted.");
                    }
                    else // Should unmute
                    {
                        if (muted)
                            Mute(0); // Unmute Spotify
                        ResumeTimer.Stop();
                        Console.WriteLine("Unmuted " + artist);
                        Notify(artist + " is not on your blocklist. Open EZBlocker to add it.");
                    }
                }
            }
        }

        /**
         * Will attempt to play ad while muted
         **/
        private void ResumeTimer_Tick(object sender, EventArgs e)
        {
            UpdateTitle();
            if (!IsPlaying())
            {
                SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_PLAYPAUSE); // Play again   
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
         * Gets the Spotify process handle
         **/
        private IntPtr GetHandle()
        {
            Process[] p = Process.GetProcesses();
            for (var i = 0; i < p.Length; i++)
            {
                if (p[i].ProcessName.Equals("spotify"))
                {
                    return p[i].Handle;
                }
            }
            return IntPtr.Zero;
        }

        /**
         * Determines whether or not Spotify is currently playing
         **/
        private Boolean IsPlaying()
        {
            return title.Contains("-");
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
        private Boolean IsInBlocklist(String artist)
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
            String url = "http://itunes.apple.com/search?entity=musicArtist&limit=20&term=" + artist.Replace(" ", "+"); // Ghetto URL encoding for .net 3.5
            String json = GetPage(url, ua);
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            Regex regex = new Regex("[^A-Za-z0-9]");
            while (reader.Read())
            {
                {
                    if (reader.Value != null) 
                    {
                        if (reader.Value.Equals("artistName")) // If key is artistName, read next value
                        {
                            reader.Read();
                            // String readerValue = Encoding.UTF8.GetString(Encoding.Default.GetBytes(reader.Value.ToString())); // Convert result to UTF-8 for people like Beyoncé
                            if (regex.Replace(Convert.ToString(reader.Value), "").ToLower().Equals(regex.Replace(artist, "").ToLower())) return false; // Match alphanumerically, case insensitively
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
        private String GetPage(String URL, String ua)
        {
            WebClient w = new WebClient();
            w.Headers.Add("user-agent", ua);
            String s = w.DownloadString(URL);
            return s;
        }

        private void Notify(String message)
        {
            if (notify)
                NotifyIcon.ShowBalloonTip(10000, "EZBlocker", message, ToolTipIcon.None);
        }

        /**
         * Checks if the current installation is the latest version. Prompts user if not.
         **/
        private void CheckUpdate()
        {
            int latest = Convert.ToInt32(GetPage("http://www.ericzhang.me/dl/?file=EZBlocker-version.txt", "EZBlocker " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion));
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

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.ShowInTaskbar.Equals(false))
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                Notify("EZBlocker is hidden. Double-click this icon to restore.");
            }
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void BlockButton_Click(object sender, EventArgs e)
        {
            AddToBlockList(GetArtist());
            lastChecked = String.Empty; // Reset last checked so we can auto mute
        }

        private void AutoAddCheck_CheckedChanged(object sender, EventArgs e)
        {
            autoAdd = AutoAddCheckbox.Checked;
        }

        private void NotifyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            notify = NotifyCheckbox.Checked;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", blocklistPath);
        }

        private void MuteButton_Click(object sender, EventArgs e)
        {
            Mute(2);
        }

        private void WebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(website);
        }

    }
}
