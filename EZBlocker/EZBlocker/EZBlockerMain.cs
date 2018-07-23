using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private bool muted = false;
        private bool playingAd = false;
        private string lastArtistName = "";
        private ToolTip artistTooltip = new ToolTip();

        private string spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
        private string volumeMixerPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\SndVol.exe";
        private string hostsPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\drivers\etc\hosts";

        private string[] adHosts = { "pubads.g.doubleclick.net", "securepubads.g.doubleclick.net", "www.googletagservices.com", "gads.pubmatic.com", "ads.pubmatic.com", "tpc.googlesyndication.com", "pagead2.googlesyndication.com", "googleads.g.doubleclick.net" };

        private const string website = @"https://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";

        private Analytics a;
        private DateTime lastRequest;

        private SpotifyHook hook;

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
                if (hook.IsRunning())
                {
                    Debug.WriteLine(AudioUtils.GetPeakVolume(hook.Spotify));
                    if (hook.IsAdPlaying())
                    {
                        if (MainTimer.Interval < 1500) MainTimer.Interval = 1500;
                        if (!playingAd) playingAd = true;
                        if (!muted) Mute(true);

                        string artist = hook.GetArtist();
                        if (lastArtistName != artist)
                        {
                            StatusLabel.Text = "Muting: " + Truncate(artist);
                            artistTooltip.SetToolTip(StatusLabel, lastArtistName = artist);
                            LogAction("/mute/" + artist);
                        }
                    }
                    else if (hook.IsPlaying()) // Normal music
                    {
                        if (muted) Mute(false);
                        if (MainTimer.Interval > 600) MainTimer.Interval = 600;
                        if (playingAd) playingAd = false;

                        string artist = hook.GetArtist();
                        if (lastArtistName != artist)
                        {
                            StatusLabel.Text = "Playing: " + Truncate(artist);
                            artistTooltip.SetToolTip(StatusLabel, lastArtistName = artist);
                            LogAction("/play/" + artist);
                        }
                    }
                    else if (playingAd) // If here, means we were in an ad state, but Spotify was paused and ad is no longer playing
                    {
                        AudioUtils.SendPlayPause(hook.Handle);
                    }
                    else
                    {
                        StatusLabel.Text = "Spotify is paused";
                        artistTooltip.SetToolTip(StatusLabel, "");
                    }
                }
                else
                {
                    MainTimer.Interval = 5000;
                    StatusLabel.Text = "Spotify is not running";
                    lastArtistName = "";
                    artistTooltip.SetToolTip(StatusLabel, "");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
       
        /**
         * Mutes/Unmutes Spotify.
         
         * i: false = unmute, true = mute
         **/
        private void Mute(bool mute)
        {
            AudioUtils.SetMute(hook.Spotify, mute);
            muted = AudioUtils.IsMuted(hook.Spotify) != null ? (bool)AudioUtils.IsMuted(hook.Spotify) : false;
        }

        private string Truncate(string name)
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
            try
            {
                WebClient w = new WebClient();
                w.Headers.Add("user-agent", "EZBlocker " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion);
                string s = w.DownloadString("https://www.ericzhang.me/dl/?file=EZBlocker-version.txt");
                int latest = Convert.ToInt32(s);
                int current = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                if (latest <= current)
                    return;
                if (MessageBox.Show("There is a newer version of EZBlocker available. Would you like to upgrade?", "EZBlocker", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(website);
                    Application.Exit();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error checking for update.", "EZBlocker");
            }
        }

        private void LogAction(string action)
        {
            Task.Run(() => a.LogAction(action));
            lastRequest = DateTime.Now;
        }

        /**
         * Send a request every 5 minutes to keep session alive
         **/
        private void Heartbeat_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - lastRequest > TimeSpan.FromMinutes(5))
            {
                LogAction("/heartbeat");
            }
        }


        private void Main_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UpdateSettings) // If true, then first launch of latest EZBlocker
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateSettings = false;
                Properties.Settings.Default.Save();
            }

            // Start Spotify and give EZBlocker higher priority
            try
            {
                if (Properties.Settings.Default.StartSpotify && File.Exists(spotifyPath) && Process.GetProcessesByName("spotify").Length < 1)
                {
                    Process.Start(spotifyPath);
                }
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly
            }
            catch (Exception) {}

            // Set up UI
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
            SpotifyCheckbox.Checked = Properties.Settings.Default.StartSpotify;
            
            // Check .NET
            if (!HasDotNet())
            {
                if (MessageBox.Show("You do not have .NET Framework 4.5 or above. Download now?", "EZBlocker Error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    Process.Start("https://www.microsoft.com/net/download/dotnet-framework-runtime");
                }
                else
                {
                    MessageBox.Show("EZBlocker may not function properly without .NET Framework 4.5 or above.");
                }
            }

            // Set up Analytics
            if (String.IsNullOrEmpty(Properties.Settings.Default.CID))
            {
                Properties.Settings.Default.CID = Analytics.GenerateCID();
                Properties.Settings.Default.Save();
            }
            a = new Analytics(Properties.Settings.Default.CID, Assembly.GetExecutingAssembly().GetName().Version.ToString());

            // Start Spotify hook
            hook = new SpotifyHook();

            Mute(false);

            MainTimer.Enabled = true;

            LogAction("/launch");

            Task.Run(() => CheckUpdate());
        }

        private static bool HasDotNet()
        {
            try
            {
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
                {
                    int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                    if (releaseKey >= 378389) return true; // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed#to-find-net-framework-versions-by-viewing-the-registry-net-framework-45-and-later
                }
            } catch (Exception) {}
            return false;
        }

        private void RestoreFromTray()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }
        
        private void Notify(String message)
        {
            NotifyIcon.ShowBalloonTip(5000, "EZBlocker", message, ToolTipIcon.None);
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.ShowInTaskbar && e.Button == MouseButtons.Left)
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
                Notify("EZBlocker is hidden. Double-click this icon to restore.");
            }
        }

        private void SkipAdsCheckbox_Click(object sender, EventArgs e)
        {
            if (!MainTimer.Enabled) return; // Still setting up UI
            if (!IsUserAnAdmin())
            {
                MessageBox.Show("Enabling/Disabling this option requires Administrator privileges.\n\nPlease reopen EZBlocker with \"Run as Administrator\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BlockBannersCheckbox.Checked = !BlockBannersCheckbox.Checked;
                return;
            }
            try
            {
                if (!File.Exists(hostsPath))
                {
                    File.Create(hostsPath).Close();
                }
                // Always clear hosts
                string[] text = File.ReadAllLines(hostsPath);
                text = text.Where(line => !adHosts.Contains(line.Replace("0.0.0.0 ", "")) && line.Length > 0).ToArray();
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
            if (!MainTimer.Enabled) return; // Still setting up UI
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


        private void SpotifyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!MainTimer.Enabled) return; // Still setting up UI
            Properties.Settings.Default.StartSpotify = SpotifyCheckbox.Checked;
            Properties.Settings.Default.Save();
            LogAction("/settings/startSpotify/" + SpotifyCheckbox.Checked.ToString());
        }

        private void VolumeMixerButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(volumeMixerPath);
                LogAction("/button/volumeMixer");
            }
            catch (Exception)
            {
                MessageBox.Show("Could not open Volume Mixer. This is only available on Windows 7/8/10", "EZBlocker");
            }
        }

        private void WebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please leave a comment clearly describing your problem. \r\n\r\nEg: My audio ads are not muted and banner ads are still visible.", "EZBlocker");
            Process.Start(website);
            LogAction("/button/website");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(website);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!Properties.Settings.Default.UserEducated)
            {
                var result = MessageBox.Show("Spotify ads will not be muted if EZBlocker is not running.\r\n\r\nAre you sure you want to exit?", "EZBlocker",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Warning);

                e.Cancel = (result == DialogResult.No);

                if (result == DialogResult.Yes)
                {
                    Properties.Settings.Default.UserEducated = true;
                    Properties.Settings.Default.Save();
                }
            }
        }

        [DllImport("shell32.dll")]
        public static extern bool IsUserAnAdmin();

    }
}
