using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using CoreAudio;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Text;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private string lastChecked = string.Empty; // Previous artist
        private bool muted = false;
        private bool spotifyMute = false;
        private float volume = 0.9f;

        private string nircmdPath = Application.StartupPath + @"\nircmd.exe";
        private string jsonPath = Application.StartupPath + @"\Newtonsoft.Json.dll";
        private string coreaudioPath = Application.StartupPath + @"\CoreAudio.dll";

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        

        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int MEDIA_PLAYPAUSE = 0xE0000;

        private const string ua = @"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.65 Safari/537.36";
        private string EZBlockerUA = "EZBlocker " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion;
        private const string website = @"http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";

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
            if (!HasNet35())
                MessageBox.Show(".Net Framework 3.5 not found. EZBlocker may not work properly.", "EZBlocker");
            if (!File.Exists(nircmdPath))
            {
                File.WriteAllBytes(nircmdPath, EZBlocker.Properties.Resources.nircmd32);
            }
            if (!File.Exists(jsonPath))
            {
                File.WriteAllBytes(jsonPath, EZBlocker.Properties.Resources.Newtonsoft_Json);
            }
            if (!File.Exists(coreaudioPath))
            {
                File.WriteAllBytes(coreaudioPath, EZBlocker.Properties.Resources.CoreAudio);
            }
            InitializeComponent();
            CheckUpdate();
            try
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe");
                System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            rnd = new Random(Environment.TickCount);
            starttime = DateTime.Now.Ticks;
            if (String.IsNullOrEmpty(Properties.Settings.Default.UID))
            {
                Properties.Settings.Default.UID = rnd.Next(100000000, 999999999).ToString(); // Build unique visitorId;
                Properties.Settings.Default.Save();
            }
            visitorId = Properties.Settings.Default.UID;
            SpotifyMuteCheckbox.Checked = Properties.Settings.Default.SpotifyMute;

            LogAction("/launch");
        }

        /**
         * Contains the logic for when to mute Spotify
         **/
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                int WebHelperResult = WebHelperHook.isAd();
                Console.WriteLine("WebHelperResult " + WebHelperResult.ToString());
                if (WebHelperResult == 0) // Not an ad
                {
                    if (muted) Mute(0);
                    StatusLabel.Text = "Playing music";
                }
                else if (WebHelperResult == 1) // Is an ad
                {
                    if (!muted) Mute(1); // Mute if not muted
                    StatusLabel.Text = "Playing muted ad";
                }
                else if (WebHelperResult == 2) // Is an ad, paused
                {
                    Resume();
                    StatusLabel.Text = "Playing muted ad";
                }
                else if (WebHelperResult == -1) // Spotify is paused
                {
                    StatusLabel.Text = "Spotify is paused";
                } else if (WebHelperResult == -2) // Spotify is not running
                {
                    StatusLabel.Text = "Spotify not running";
                }
            }
            catch (Exception except)
            {
                Notify("Error occurred trying to connect to ad-detection servers.");
                Console.WriteLine(except);
            }
        }
       
        /**
         * Mutes/Unmutes Spotify.
         
         * i: 0 = unmute, 1 = mute, 2 = toggle
         **/
        private void Mute(int i)
        {
            if (i == 2) // Toggle mute
                i = (muted ? 0 : 1);

            muted = Convert.ToBoolean(i);

            if (spotifyMute) // Mute only Spotify process
            {
                // EZBlocker2.AudioUtilities.SetApplicationMute("spotify", muted);

                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
                MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                AudioSessionManager2 asm = device.AudioSessionManager2;
                SessionCollection sessions = asm.Sessions;
                for (int sid = 0; sid < sessions.Count; sid++)
                {
                    string id = sessions[sid].GetSessionIdentifier;
                    if (id.ToLower().IndexOf("spotify.exe") > -1)
                    {
                        if (muted)
                        {
                            volume = sessions[sid].SimpleAudioVolume.MasterVolume;
                            sessions[sid].SimpleAudioVolume.MasterVolume = 0;
                        }
                        else
                        {
                            sessions[sid].SimpleAudioVolume.MasterVolume = volume;
                        }
                        //sessions[sid].SimpleAudioVolume.Mute = muted;
                    }
                }
            }
            else // Mute all of Windows
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C nircmd mutesysvolume " + i.ToString();
                process.StartInfo = startInfo;
                process.Start();
            }

        }

        /**
         * Resumes playing Spotify
         **/
        private void Resume()
        {
            if (spotifyMute)
            {
                SendMessage(GetHandle(), WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_PLAYPAUSE);
            }
            else
            {
                SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_PLAYPAUSE);
            }
        }

        /**
         * Gets the Spotify process handle
         **/
        private IntPtr GetHandle()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Contains("spotify")))
            {
                return FindWindow(null, "Spotify Free");
            }
            return IntPtr.Zero;
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
                catch { }
            }
            try
            {
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
            catch
            {
                MessageBox.Show("Error checking for update.", "EZBlocker");
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
            catch { /*ignore*/ }
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

        /**
         * Processes window message and shows EZBlocker when attempting to launch a second instance.
         **/
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WindowUtilities.WM_SHOWAPP)
            {
                if (!this.ShowInTaskbar)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                }
                else
                {
                    this.Activate();
                }
            }
            base.WndProc(ref m);
        }

        private void Notify(String message)
        {
            NotifyIcon.ShowBalloonTip(10000, "EZBlocker", message, ToolTipIcon.None);
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.ShowInTaskbar)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                Notify("EZBlocker is hidden. Double-click this icon to restore.");
            }
        }

        private void SpotifyMuteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            spotifyMute = SpotifyMuteCheckbox.Checked;
            if (!spotifyMute) MessageBox.Show("You may need to restart Spotify for this to take affect", "EZBlocker");
            LogAction("/settings/spotifyMute/" + spotifyMute.ToString());
            Properties.Settings.Default.SpotifyMute = spotifyMute;
            Properties.Settings.Default.Save();
        }
        
        private void VolumeMixerButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\System32\SndVol.exe");
            }
            catch (Exception ignore)
            {
                MessageBox.Show("Could not open Volume Mixer. This is only available on Windows 7/8/10", "EZBlocker");
            }
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
            Mute(0); // Unmute Spotify, if muted
        }

    }
}
