using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using CoreAudio;
using System.Runtime.InteropServices;
using System.Threading;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private bool muted = false;
        private bool spotifyMute = false;
        private float volume = 0.9f;
        private string lastArtistName = "";

        private string nircmdPath = Application.StartupPath + @"\nircmd.exe";
        private string jsonPath = Application.StartupPath + @"\Newtonsoft.Json.dll";
        private string coreaudioPath = Application.StartupPath + @"\CoreAudio.dll";
        private string logPath = Application.StartupPath + @"\EZBlocker-log.txt";

        private string spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
        private string spotifyPrefsPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\prefs";
        private string volumeMixerPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\SndVol.exe";
        private string hostsPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\drivers\etc\hosts";

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("shell32.dll")]
        public static extern bool IsUserAnAdmin();

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646275%28v=vs.85%29.aspx
        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int MEDIA_PLAYPAUSE = 0xE0000;
        private const int MEDIA_NEXTTRACK = 0xB0000;
        
        private string EZBlockerUA = "EZBlocker " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion;
        private const string website = @"https://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";

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
            InitializeComponent();
        }

        /**
         * Contains the logic for when to mute Spotify
         **/
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            WebHelperResult whr = WebHelperHook.GetStatus();

            if (whr.isAd) // Track is ad
            {
                if (whr.isPlaying)
                {
                    Debug.WriteLine("Ad is playing");
                    if (lastArtistName != whr.artistName)
                    {
                        if (!muted) Mute(1);
                        StatusLabel.Text = "Muting ad";
                        lastArtistName = whr.artistName;
                        LogAction("/mute/" + whr.artistName);
                        Debug.WriteLine("Blocked " + whr.artistName);
                    }
                }
                else // Ad is paused
                {
                    Debug.WriteLine("Ad is paused");
                    Resume();
                }
            }
            else if (whr.isPrivateSession)
            {
                lastArtistName = "Private session";
            }
            else if (!whr.isRunning)
            {
                StatusLabel.Text = "Spotify is not running";
                lastArtistName = "";
            }
            else if (!whr.isPlaying)
            {
                StatusLabel.Text = "Spotify is paused";
                lastArtistName = "";
            }
            else // Song is playing
            {
                if (muted) Mute(0);
                if (lastArtistName != whr.artistName)
                {
                    StatusLabel.Text = "Playing: " + ShortenName(whr.artistName);
                    lastArtistName = whr.artistName;
                }
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
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
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
            Debug.WriteLine("Resuming Spotify");
            SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_PLAYPAUSE);
        }

        /**
         *  Plays next track queued on Spotify
         **/
        private void NextTrack()
        {
            Debug.WriteLine("Skipping to next track");
            if (spotifyMute)
            {
                SendMessage(GetHandle(), WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_NEXTTRACK);
            }
            else
            {
                SendMessage(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_NEXTTRACK);
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

        private string ShortenName(string name)
        {
            if (name.Length > 12)
            {
                return name.Substring(0, 12) + "...";
            }
            return name;
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
                int latest = Convert.ToInt32(GetPage("https://www.ericzhang.me/dl/?file=EZBlocker-version.txt", EZBlockerUA));
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

        private void RestoreFromTray()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
                    RestoreFromTray();
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
                RestoreFromTray();
            }
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                Notify("EZBlocker is hidden. Double-click this icon to restore.");
            }
        }

        private void SpotifyMuteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            spotifyMute = SpotifyMuteCheckbox.Checked;
            if (visitorId == null) return; // Still setting up UI
            if (!spotifyMute) MessageBox.Show("You may need to restart Spotify for this to take effect.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LogAction("/settings/spotifyMute/" + spotifyMute.ToString());
            Properties.Settings.Default.SpotifyMute = spotifyMute;
            Properties.Settings.Default.Save();
        }

        private void SkipAdsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (visitorId == null) return; // Still setting up UI
            if (!IsUserAnAdmin())
            {
                MessageBox.Show("Enabling/Disabling this option requires Administrator privilages.\n\nPlease reopen EZBlocker with \"Run as Administrator\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(hostsPath))
            {
                File.Create(hostsPath).Close();
            }
            try
            {
                if (BlockBannersCheckbox.Checked)
                {
                    using (StreamWriter sw = File.AppendText(hostsPath))
                    {
                        sw.WriteLine();
                        sw.WriteLine("0.0.0.0 pubads.g.doubleclick.net");
                        sw.Write("0.0.0.0 securepubads.g.doubleclick.net");
                    }
                }
                else
                {
                    string[] text = File.ReadAllLines(hostsPath);
                    text = text.Where(line => !line.Contains("doubleclick.net")).ToArray();
                    File.WriteAllLines(hostsPath, text);
                }
                MessageBox.Show("You may need to restart Spotify or your computer for this setting to take effect.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAction("/settings/blockBanners/" + BlockBannersCheckbox.Checked.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        
        private void VolumeMixerButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(volumeMixerPath);
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
            LogAction("/launch");
            
            CheckUpdate();

            // Enable web helper
            if (File.Exists(spotifyPrefsPath))
            {
                String[] lines = File.ReadAllLines(spotifyPrefsPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("webhelper.enabled") && lines[i].Contains("false"))
                    {
                        lines[i] = "webhelper.enabled=true";
                        File.WriteAllLines(spotifyPrefsPath, lines);
                        break;
                    }
                }
            }

            // Start Spotify and give EZBlocker higher priority
            try
            {
                if (File.Exists(spotifyPath))
                {
                    if (!FileVersionInfo.GetVersionInfo(spotifyPath).FileVersion.StartsWith("1."))
                    {
                        if (MessageBox.Show("You are using Spotify " + FileVersionInfo.GetVersionInfo(spotifyPath).FileVersion + ".\n\nPlease download EZBlocker v1.4.0.1 or upgrade to the newest Spotify to use EZBlocker v1.5.\n\nClick OK to continue to the EZBlocker website.", "EZBlocker", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            Process.Start(website);
                            Application.Exit();
                        }
                    }
                    else
                    {
                        Process.Start(spotifyPath);
                    }
                }
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // Extract dependencies
            if (!File.Exists(nircmdPath))
            {
                File.WriteAllBytes(nircmdPath, Properties.Resources.nircmd32);
            }
            if (!File.Exists(jsonPath))
            {
                File.WriteAllBytes(jsonPath, Properties.Resources.Newtonsoft_Json);
            }
            if (!File.Exists(coreaudioPath))
            {
                File.WriteAllBytes(coreaudioPath, Properties.Resources.CoreAudio);
            }

            // Set up UI
            SpotifyMuteCheckbox.Checked = Properties.Settings.Default.SpotifyMute;
            if (File.Exists(hostsPath))
            {
                BlockBannersCheckbox.Checked = File.ReadAllText(hostsPath).Contains("doubleclick.net");
            }

            // Google Analytics
            rnd = new Random(Environment.TickCount);
            starttime = DateTime.Now.Ticks;
            if (String.IsNullOrEmpty(Properties.Settings.Default.UID))
            {
                Properties.Settings.Default.UID = rnd.Next(100000000, 999999999).ToString(); // Build unique visitorId;
                Properties.Settings.Default.Save();
            }
            visitorId = Properties.Settings.Default.UID;
            
            Mute(0);

            MainTimer.Enabled = true;
        }
    }
}
