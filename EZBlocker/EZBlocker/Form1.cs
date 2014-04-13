using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Text;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private string title = string.Empty; // Title of the Spotify window
        private string lastChecked = string.Empty; // Previous artist
        private bool autoAdd = false;
        private bool notify = false;
        private bool muted = false;
        private bool spotifyMute = false;
        private uint pid = 0;

        private string blocklistPath = Application.StartupPath + @"\blocklist.txt";
        private string nircmdPath = Application.StartupPath + @"\nircmd.exe";
        private string jsonPath = Application.StartupPath + @"\Newtonsoft.Json.dll";

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        //public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int MEDIA_PLAYPAUSE = 0xE0000;

        private const string ua = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
        private string EZBlockerUA = "EZBlocker " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion;
        private const string website = @"http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";
        private Dictionary<string, int> m_blockList;

        // Google Analytics stuff
        private Random rnd;
        private long starttime, lasttime;
        private string visitorId;
        private int runs = 1;
        private const string domainHash = "69214020";
        private const string source = "EZBlocker";
        private string medium = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private const string sessionNumber = "1";
        private const string campaignNumber = "1";
        private string language = Thread.CurrentThread.CurrentCulture.Name;
        private string screenRes = Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height;
        private const string trackingId = "UA-42480515-3";

        public Main()
        {
            CheckUpdate();
            if (!HasNet35())
                MessageBox.Show(".Net Framework 3.5 not found. EZBlocker may not work properly.", "EZBlocker");
            if (!File.Exists(nircmdPath))
            {
                /*if (getOSArchitecture() == 64)
                    File.WriteAllBytes(nircmdPath, EZBlocker.Properties.Resources.nircmd64);
                else*/
                File.WriteAllBytes(nircmdPath, EZBlocker.Properties.Resources.nircmd32);
            }
            if (!File.Exists(jsonPath))
            {
                File.WriteAllBytes(jsonPath, EZBlocker.Properties.Resources.Newtonsoft_Json);
            }
            if (!File.Exists(blocklistPath))
            {
                WebClient w = new WebClient();
                w.Headers.Add("user-agent", EZBlockerUA);
                w.DownloadFile("http://www.ericzhang.me/dl/?file=blocklist.txt", blocklistPath);
            }
            InitializeComponent();
            try
            {
                // Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe");
                System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Mute(0); // Unmute Spotify, if muted
            ReadBlockList();
            rnd = new Random();
            starttime = DateTime.Now.Ticks;
            if (String.IsNullOrEmpty(Properties.Settings.Default.UID))
            {
                Properties.Settings.Default.UID = rnd.Next(100000000, 999999999).ToString(); // Build unique visitorId;
                Properties.Settings.Default.Save();
            }
            visitorId = Properties.Settings.Default.UID;
            AutoAddCheckbox.Checked = Properties.Settings.Default.AutoAdd;
            NotifyCheckbox.Checked = Properties.Settings.Default.Notifications;
            SpotifyMuteCheckbox.Checked = Properties.Settings.Default.SpotifyMute;
            LogAction("/launch");
        }

        /**
         * Contains the logic for when to mute Spotify
         **/
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (pid == 0) SetProcessId();
            UpdateTitle();
            Console.WriteLine(title);
            if (!IsPlaying()) 
                return;
            string artist = GetArtist();
            if (lastChecked.Equals(artist)) 
                return;
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
                LogAction("/mute/" + artist);
            }
            else // Should unmute
            {
                if (muted)
                    Mute(0); // Unmute Spotify
                ResumeTimer.Stop();
                Notify(artist + " is not on your blocklist. Open EZBlocker to add it.");
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
                SendMessage(GetHandle("SpotifyWindow"), WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_PLAYPAUSE); // Play again   
            }
        }

        /**
         * Updates the title of the Spotify window.
         * 
         * Returns true if title updated successfully, false if otherwise
         **/
        private bool UpdateTitle()
        {
            /*foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.Equals("spotify")))
            {
                title = t.MainWindowTitle;
                return true;
            }
            return false;*/
            /*
             List<string> titles = new List<string>();
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.Equals("spotify")))
            {
                titles.Add(t.MainWindowTitle);
            }
            title = titles.OrderByDescending(s => s.Length).First();
            return true; */
            try
            {
                title = WindowUtilities.GetWindowTitles(false, pid).OrderByDescending(s => s.Length).First();
                return true;
            }
            catch (Exception e)
            {
                return false;
            } 
        }

        /**
         * Gets the Spotify process handle
         **/
        private IntPtr GetHandle(String classname)
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.Equals("spotify")))
            {
                return FindWindowEx(t.MainWindowHandle, new IntPtr(0), classname, null);
            }
            return IntPtr.Zero;
        }

        /**
         * Set's Spotify's process ID
         **/
        private bool SetProcessId()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.Equals("spotify")))
            {
                pid = Convert.ToUInt32(t.Id);
                return true;
            }
            return false;
        }

        /**
         * Determines whether or not Spotify is currently playing
         **/
        private bool IsPlaying()
        {
            return title.Contains("-");
        }

        /**
         * Returns the current artist
         **/
        private string GetArtist()
        {
            if (!IsPlaying()) return string.Empty;
            return title.Substring(10).Split('\u2013')[0].TrimEnd(); // Split at endash
        }

        /**
         * Adds an artist to the blocklist.
         * 
         * Returns false if Spotify is not playing.
         **/
        private bool AddToBlockList(string artist)
        {
            if (!IsPlaying() || IsInBlocklist(artist)) 
                return false;
            m_blockList.Add(artist, 0);
            File.AppendAllText(blocklistPath, artist + "\r\n");
            LogAction("/block/" + artist);
            return true;
        }

        private void ReadBlockList()
        {
            m_blockList = File.ReadAllLines(blocklistPath).Distinct().Select((k, v) => new { Index = k, Value = v }).ToDictionary(v => v.Index, v => v.Value);
        }

        /**
         * Mutes/Unmutes Spotify.
         * 
         * i: 0 = unmute, 1 = mute, 2 = toggle
         **/
        private void Mute(int i)
        {
            if (i == 2) // Toggle mute
            {
                if (muted)
                    i = 0;
                else
                    i = 1;
            }
            if (i == 1)
                muted = true;
            else if (i == 0)
                muted = false;
            else
                muted = !muted;
            System.Diagnostics.Process process = new System.Diagnostics.Process(); // http://stackoverflow.com/questions/1469764/run-command-prompt-commands
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            if (spotifyMute)
            {
                AudioUtilities.SetApplicationMute("spotify", muted);
                /*startInfo.Arguments = "/C nircmd muteappvolume Spotify.exe " + i.ToString();
                process.StartInfo = startInfo;
                process.Start();
                // Run again for some users
                startInfo.Arguments = "/C nircmd muteappvolume spotify.exe " + i.ToString();
                process.StartInfo = startInfo;
                process.Start();*/
            }
            else
            {
                startInfo.Arguments = "/C nircmd mutesysvolume " + i.ToString();
                process.StartInfo = startInfo;
                process.Start();
            }
        }

        /**
         * Checks if an artist is in the blocklist (Exact match only)
         **/
        private bool IsInBlocklist(string artist)
        {
            return m_blockList.ContainsKey(artist);
        }

        /**
         * Attempts to check if the current song is an ad
         **/
        private bool IsAd(string artist)
        {
            try
            {
                return (isAdSpotify(artist) && IsAdiTunes(artist));
            }
            catch (Exception e)
            {
                Notify("Error occurred trying to connect to ad-detection servers.");
                return false;
            }
        }

        /**
         * Checks Spotify Web API to see if artist is an ad
         **/
        private bool isAdSpotify(String artist)
        {
            string url = "http://ws.spotify.com/search/1/artist.json?q=" + FormEncode(artist);
            string json = GetPage(url, ua);
            SpotifyAnswer res = JsonConvert.DeserializeObject<SpotifyAnswer>(json);
            foreach (Artist a in res.artists) 
            {
                if (SimpleCompare(artist, a.name))
                    return false;
            }
            return true;
        }

        /**
         * Checks iTunes Web API to see if artist is an ad
         **/
        private bool IsAdiTunes(String artist)
        {
            String url = "http://itunes.apple.com/search?entity=musicArtist&limit=20&term=" + FormEncode(artist);
            String json = GetPage(url, ua);
            ITunesAnswer res = JsonConvert.DeserializeObject<ITunesAnswer>(json);
            foreach (Result r in res.results)
            {
                if (SimpleCompare(artist, r.artistName))
                    return false;
            }
            return true;
        }

        /**
         * Encodes an artist name to be compatible with web api's
         **/
        private string FormEncode(String param)
        {
            return param.Replace(" ", "+").Replace("&", "");
        }

        /**
         * Compares two strings based on lowercase alphanumeric letters and numbers only.
         **/
        private bool SimpleCompare(String a, String b)
        {
            Regex regex = new Regex("[^a-z0-9]");
            return String.Equals(regex.Replace(a.ToLower(), ""), regex.Replace(b.ToLower(), ""));
        }

        /**
         * Gets the source of a given URL
         **/
        private string GetPage(string URL, string ua)
        {
            WebClient w = new WebClient();
            w.Headers.Add("user-agent", ua);
            string s = w.DownloadString(URL);
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
            if (Properties.Settings.Default.UpdateSettings) // If true, then first launch of latest EZBlocker
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateSettings = false;
                Properties.Settings.Default.Save();
                try
                {
                    File.Delete(nircmdPath);
                    File.Delete(jsonPath);
                }
                catch (Exception ignore) { }
            }
            int latest = Convert.ToInt32(GetPage("http://www.ericzhang.me/dl/?file=EZBlocker-version.txt", EZBlockerUA));
            int current = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            if (latest <= current) 
                return;
            if (MessageBox.Show("There is a newer version of EZBlocker available. Would you like to upgrade?", "EZBlocker", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(website);
                Application.Exit();   
            }
        }

        /**
         * Send a request every 5 minutes to Google Analytics
         **/
        private void Heartbeat_Tick(object sender, EventArgs e)
        {
            LogAction("/heartbeat");
        }

        /**
         * Based off of: http://stackoverflow.com/questions/12851868/how-to-send-request-to-google-analytics-in-non-web-based-app
         * 
         * Logs actions using Google Analytics
         **/
        private void LogAction(string pagename)
        {
            try
            {
                lasttime = DateTime.Now.Ticks;
                string statsRequest = "http://www.google-analytics.com/__utm.gif" +
                    "?utmwv=4.6.5" +
                    "&utmn=" + rnd.Next(100000000, 999999999) +
                    "&utmcs=-" +
                    "&utmsr=" + screenRes +
                    "&utmsc=-" +
                    "&utmul=" + language +
                    "&utmje=-" +
                    "&utmfl=-" +
                    "&utmdt=" + pagename +
                    "&utmp=" + pagename +
                    "&utmac=" + trackingId + // Account number
                    "&utmcc=" +
                        "__utma%3D" + domainHash + "." + visitorId + "." + starttime + "." + lasttime + "." + starttime + "." + (runs++) +
                        "%3B%2B__utmz%3D" + domainHash + "." + lasttime + "." + sessionNumber + "." + campaignNumber + ".utmcsr%3D" + source + "%7Cutmccn%3D(" + medium + ")%7Cutmcmd%3D" + medium + "%7Cutmcct%3D%2Fd31AaOM%3B";
                using (var client = new WebClient())
                {
                    client.DownloadData(statsRequest);
                }
            }
            catch (Exception e)
            {
                // Ignore
            }

        }

        /**
         * http://andrewensley.com/2009/06/c-detect-windows-os-part-1/
         **/
        int getOSArchitecture()
        {
            string pa = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return ((String.IsNullOrEmpty(pa) || String.Compare(pa, 0, "x86", 0, 3, true) == 0) ? 32 : 64);
        }

        bool HasNet35()
        {
            try
            {
                AppDomain.CurrentDomain.Load(
                    "System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.ShowInTaskbar)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
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
            LogAction("/button/block/" + GetArtist());
        }

        private void AutoAddCheck_CheckedChanged(object sender, EventArgs e)
        {
            autoAdd = AutoAddCheckbox.Checked;
            LogAction("/settings/autoAdd/" + autoAdd.ToString());
            Properties.Settings.Default.AutoAdd = autoAdd;
            Properties.Settings.Default.Save();
        }

        private void NotifyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            notify = NotifyCheckbox.Checked;
            LogAction("/settings/notify/" + notify.ToString());
            Properties.Settings.Default.Notifications = notify;
            Properties.Settings.Default.Save();
        }

        private void SpotifyMuteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            spotifyMute = SpotifyMuteCheckbox.Checked;
            if (!spotifyMute) MessageBox.Show("You may need to restart Spotify for this to take affect", "EZBlocker");
            LogAction("/settings/spotifyMute/" + spotifyMute.ToString());
            Properties.Settings.Default.SpotifyMute = spotifyMute;
            Properties.Settings.Default.Save();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            // Process.Start(blocklistPath);
            Blocklist bl = new Blocklist();
            bl.ShowDialog();
            ReadBlockList();
            lastChecked = String.Empty; // Reset last checked so we can auto mute
            LogAction("/button/openBlocklist");
        }

        private void MuteButton_Click(object sender, EventArgs e)
        {
            Mute(2);
            LogAction("/button/mute/" + muted.ToString());
        }

        private void WebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(website);
            LogAction("/button/website");
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }
    }
}
