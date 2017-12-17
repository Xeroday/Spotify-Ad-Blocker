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
using Microsoft.Win32;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private bool muted = false;
        private bool spotifyMute = false;
        private float volume = 0.9f;
        private string lastArtistName = "";
        private int exitTolerance = 0;

        private string nircmdPath = Application.StartupPath + @"\nircmd.exe";
        private string jsonPath = Application.StartupPath + @"\Newtonsoft.Json.dll";
        private string coreaudioPath = Application.StartupPath + @"\CoreAudio.dll";
        public static string logPath = Application.StartupPath + @"\EZBlocker-log.txt";

        private string spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
        private string spotifyPrefsPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\prefs";
        private string volumeMixerPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\SndVol.exe";
        private string hostsPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\drivers\etc\hosts";

        private string[] adHosts = { "pubads.g.doubleclick.net", "securepubads.g.doubleclick.net", "www.googletagservices.com", "gads.pubmatic.com", "ads.pubmatic.com", "spclient.wg.spotify.com"};

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("shell32.dll")]
        public static extern bool IsUserAnAdmin();
        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646275%28v=vs.85%29.aspx
        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int MEDIA_PLAYPAUSE = 0xE0000;
        private const int MEDIA_NEXTTRACK = 0xB0000;
        
        private string EZBlockerUA = "EZBlocker " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion;
        private const string website = @"https://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";
        private const string issueLink = @"https://github.com/Novaki92/Spotify-Ad-Blocker/issues/new";

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
            try {
                if (Process.GetProcessesByName("spotify").Length < 1)
                {
                    if (exitTolerance > 1)
                    {
                        //File.AppendAllText(logPath, "Spotify process not found\r\n");
                        //Notify("Spotify not found, exiting EZBlocker.");
                        Application.Exit();
                    }
                    exitTolerance += 1;
                }
                else
                {
                    exitTolerance = 0;
                }

                WebHelperResult whr = WebHelperHook.GetStatus();

                if (whr.isAd) // Track is ad
                {
                    if (whr.isPlaying)
                    {
                        Debug.WriteLine("Ad is playing");
                        if (lastArtistName != whr.artistName)
                        {
                            if (!muted) Mute(1);
                            ArtistLabel.Text = "Muting ad";
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
                    if (lastArtistName != whr.artistName)
                    {
                        ArtistLabel.Text = "Playing: *Private Session*";
                        lastArtistName = whr.artistName;
                        //MessageBox.Show("Please disable 'Private Session' on Spotify for EZBlocker to function properly.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                }
                else if (!whr.isRunning)
                {
                    ArtistLabel.Text = "Spotify is not running";
                    //Notify("Error connecting to Spotify. Retrying...");
                    File.AppendAllText(logPath, "Not running.\r\n");
                    MainTimer.Interval = 5000;
                    /*
                    MainTimer.Enabled = false;
                    MessageBox.Show("Spotify is not running. Please restart EZBlocker after starting Spotify.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    StatusLabel.Text = "Spotify is not running";
                    Application.Exit();
                    */
                }
                else if (!whr.isPlaying)
                {
                    ArtistLabel.Text = "------------------------------";
                    AlbumLabel.Text = "| Spotify is paused |";
                    SongLabel.Text = "------------------------------";
                }
                else // Song is playing
                {
                    if (muted) Mute(0);
                    if (MainTimer.Interval > 1000) MainTimer.Interval = 600;
                    if (SongLabel.Text != whr.songName)
                    {
                        SongLabel.Text = "Song:  " + ShortenName(whr.songName, 34);
                    }
                    if (AlbumLabel.Text != whr.albumName)
                    {
                        AlbumLabel.Text = "Album: " + ShortenName(whr.albumName, 36);
                    }
                    if (ArtistLabel.Text != whr.artistName)
                    {
                        ArtistLabel.Text = "Artist:   " + ShortenName(whr.artistName, 19);
                        lastArtistName = whr.artistName;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                File.AppendAllText(logPath, ex.Message);
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
                if (t.MainWindowTitle.Length > 1)
                    return t.MainWindowHandle;
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
         * Cuts off text if name is too long. Removes space from end of text if present.
         **/
        private string ShortenName(string name, int length)
        {
            int maxLength = length;
            if (name.Length > maxLength)
            {
                name = name.Substring(0, maxLength);
                if (name.EndsWith(" "))
                {
                    name = name.Substring(0, maxLength - 1);
                    return name + "...";
                }
                else
                {
                    return name + "...";
                }
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
                try
                {
                    if (File.Exists(nircmdPath)) File.Delete(nircmdPath);
                    if (File.Exists(jsonPath)) File.Delete(jsonPath);
                    if (File.Exists(coreaudioPath)) File.Delete(coreaudioPath);
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.UpdateSettings = false;
                    Properties.Settings.Default.UserEducated = false;
                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    MessageBox.Show("There was an error updating EZBlocker. Please run as Administrator to update.");
                }
            }
            //try
            //{
                //int latest = Convert.ToInt32(GetPage("https://www.ericzhang.me/dl/?file=EZBlocker-version.txt", EZBlockerUA));
                //int current = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                //if (latest <= current)
                    //return;
                //if (MessageBox.Show("There is a newer version of EZBlocker available. Would you like to upgrade?", "EZBlocker", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //{
                    //Process.Start(website);
                    //Application.Exit();
                //}
            //}
            //catch
            //{
                //MessageBox.Show("Error checking for update.", "EZBlocker");
            //}
        }

        /**
         * Send a request every 5 minutes to Google Analytics
         **/
        private void Heartbeat_Tick(object sender, EventArgs e)
        {
            LogAction("/heartbeat");
        }

        private static bool hasNet45()
        {
            try
            {
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
                {
                    int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                    if (releaseKey >= 378389) return true;
                }
            } catch (Exception /*ignore*/) {}
            return false;
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
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
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
                Notify("EZBlocker is already open.");
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
                this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
             // Notify("EZBlocker is hidden. Double-click this icon to restore.");
            }
        }

        private void SpotifyMuteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            spotifyMute = SpotifyMuteCheckbox.Checked;
            if (visitorId == null) return; // Still setting up UI
            LogAction("/settings/spotifyMute/" + spotifyMute.ToString());
            Properties.Settings.Default.SpotifyMute = spotifyMute;
            Properties.Settings.Default.Save();
        }

        private void SkipAdsCheckbox_Click(object sender, EventArgs e)
        {
            if (visitorId == null) return; // Still setting up UI
            if (!IsUserAnAdmin())
            {
                MessageBox.Show("Enabling/Disabling this option requires Administrator privileges.\n\nPlease reopen EZBlocker with \"Run as Administrator\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BlockBannersCheckbox.Checked = !BlockBannersCheckbox.Checked;
                return;
            }
            if (!File.Exists(hostsPath))
            {
                File.Create(hostsPath).Close();
            }
            try
            {
                // Always clear hosts
                string[] text = File.ReadAllLines(hostsPath);
                text = text.Where(line => !adHosts.Contains(line.Replace("0.0.0.0 ", "")) && line.Length > 0 && !line.Contains("open.spotify.com")).ToArray();
                File.WriteAllLines(hostsPath, text);

                if (BlockBannersCheckbox.Checked)
                {
                    using (StreamWriter sw = File.AppendText(hostsPath))
                    {
                        sw.WriteLine();
                        foreach (string host in adHosts)
                        {
                            sw.WriteLine("0.0.0.0 " + host);
                        }
                    }
                }
                MessageBox.Show("You may need to restart Spotify or your computer for this setting to take effect.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogAction("/settings/blockBanners/" + BlockBannersCheckbox.Checked.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void StartupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (visitorId == null) return; // Still setting up UI
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (StartupCheckbox.Checked)
            {
                startupKey.SetValue("EZBlocker", "\"" + Application.ExecutablePath + "\"");
            }
            else
            {
                startupKey.DeleteValue("EZBlocker");
            }
            LogAction("/settings/startup/" + StartupCheckbox.Checked.ToString());
        }

        private void VolumeMixerButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(volumeMixerPath);
                LogAction("/button/volumemixer");
            }
            catch (Exception /*ignore*/)
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
            var mb_result = MessageBox.Show("Please leave a comment descibing your issue in as much detail as possible.", "EZBlocker", MessageBoxButtons.OKCancel);
            if (mb_result == DialogResult.OK)
            {
                Process.Start(issueLink);
                LogAction("/button/issueLink");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
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

            CheckUpdate();

            // Start Spotify and give EZBlocker higher priority
            try
            {
                if (File.Exists(spotifyPath) && Process.GetProcessesByName("spotify").Length < 1)
                {
                    Process.Start(spotifyPath);
                }
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly

                // Check for open.spotify.com in hosts
                String hostsContent = File.ReadAllText(hostsPath);
                if (hostsContent.Contains("open.spotify.com"))
                {
                    if (IsUserAnAdmin())
                    {
                        File.WriteAllText(hostsPath, hostsContent.Replace("open.spotify.com", "localhost"));
                        MessageBox.Show("An EZBlocker patch has been applied to your hosts file. If EZBlocker is stuck at 'Loading', please restart your computer.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("EZBlocker has detected an error in your hosts file.\r\n\r\nPlease re-run EZBlocker as Administrator or remove 'open.spotify.com' from your hosts file.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // Extract dependencies
            try {
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
            } catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error loading EZBlocker dependencies. Please run EZBlocker as administrator or put EZBlocker in a user folder.");
            }

            // Set up UI
            SpotifyMuteCheckbox.Checked = Properties.Settings.Default.SpotifyMute;
            if (File.Exists(hostsPath))
            {
                string hostsFile = File.ReadAllText(hostsPath);
                BlockBannersCheckbox.Checked = adHosts.All(host => hostsFile.Contains("0.0.0.0 " + host));
            }
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (startupKey.GetValue("EZBlocker") != null)
            {
                if (startupKey.GetValue("EZBlocker").ToString() == "\"" + Application.ExecutablePath + "\"")
                {
                    StartupCheckbox.Checked = true;
                    this.WindowState = FormWindowState.Minimized;
                }
                else // Reg value exists, but not in right path
                {
                    startupKey.DeleteValue("EZBlocker");
                }
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
            
            File.AppendAllText(logPath, "-----------\r\n");
            bool unsafeHeaders = WebHelperHook.SetAllowUnsafeHeaderParsing20();
            Debug.WriteLine("Unsafe Headers: " + unsafeHeaders);

            if (!hasNet45())
            {
                if (MessageBox.Show("You do not have .NET Framework 4.5. Download now?", "EZBlocker Error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    Process.Start("https://www.microsoft.com/en-us/download/details.aspx?id=30653");
                } else
                {
                    MessageBox.Show("EZBlocker may not function properly without .NET Framework 4.5 or above.");
                }
            }

            Mute(0);
            
            MainTimer.Enabled = true;

            LogAction("/launch");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //if (!Properties.Settings.Default.UserEducated)
            //{
                //var result = MessageBox.Show("Spotify ads will not be muted if EZBlocker is not running.\r\n\r\nAre you sure you want to exit?", "EZBlocker",
                                 //MessageBoxButtons.YesNo,
                                 //MessageBoxIcon.Warning);

                //e.Cancel = (result == DialogResult.No);

                //if (result == DialogResult.Yes)
                //{
                    //Properties.Settings.Default.UserEducated = true;
                    //Properties.Settings.Default.Save();
                //}
            //}
        }
    }
}
