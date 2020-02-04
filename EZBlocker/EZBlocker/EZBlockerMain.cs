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
        private bool? muted = null;
        private string lastMessage = "";
        private ToolTip artistTooltip = new ToolTip();

        private readonly string spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
        private readonly string volumeMixerPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\SndVol.exe";
        private readonly string hostsPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\drivers\etc\hosts";

        private readonly string[] adHosts = { "pubads.g.doubleclick.net", "securepubads.g.doubleclick.net", "www.googletagservices.com", "gads.pubmatic.com", "ads.pubmatic.com", "tpc.googlesyndication.com", "pagead2.googlesyndication.com", "googleads.g.doubleclick.net" };

        public const string website = @"https://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";

        private Analytics a;
        private DateTime lastRequest;
        private string lastAction = "";
        private SpotifyPatcher patcher;
        private SpotifyHook hook;

        public Main()
        {
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
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
                    muted = muted ?? AudioUtils.IsMuted(hook.VolumeControl.Control);

                    if (hook.IsAdPlaying())
                    {
                        if (MainTimer.Interval != 1000) MainTimer.Interval = 1000;
                        if (muted == false) Mute(true);
                        if (!hook.IsPlaying())
                        {
                            AudioUtils.SendNextTrack(hook.Handle == IntPtr.Zero ? Handle : hook.Handle);
                            Thread.Sleep(500);
                        }

                        string artist = hook.GetArtist();
                        string message = Properties.strings.StatusMuting + " " + Truncate(artist);
                        if (lastMessage != message)
                        {
                            lastMessage = message;
                            StatusLabel.Text = message;
                            artistTooltip.SetToolTip(StatusLabel, artist);
                            LogAction("/mute/" + artist);
                        }
                    }
                    else if (hook.IsPlaying() && !hook.WindowName.Equals("Spotify")) // Normal music
                    {
                        if (muted == true)
                        {
                            Thread.Sleep(500); // Give extra time for ad to change out
                            Mute(false);
                        }
                        if (MainTimer.Interval != 400) MainTimer.Interval = 400;

                        string artist = hook.GetArtist();
                        string message = Properties.strings.StatusPlaying + " " + Truncate(artist);
                        if (lastMessage != message)
                        {
                            lastMessage = message;
                            StatusLabel.Text = message;
                            artistTooltip.SetToolTip(StatusLabel, artist);
                            LogAction("/play/" + artist);
                        }
                    }
                    else if (hook.WindowName.Equals("Spotify"))
                    {
                        string message = Properties.strings.StatusPaused;
                        if (lastMessage != message)
                        {
                            lastMessage = message;
                            StatusLabel.Text = message;
                            artistTooltip.SetToolTip(StatusLabel, "");
                        }
                    }
                }
                else
                {
                    if (MainTimer.Interval != 1000) MainTimer.Interval = 1000;
                    string message = Properties.strings.StatusNotFound;
                    if (lastMessage != message)
                    {
                        lastMessage = message;
                        StatusLabel.Text = message;
                        artistTooltip.SetToolTip(StatusLabel, "");
                    };
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
            AudioUtils.SetMute(hook.VolumeControl.Control, mute);
            muted = AudioUtils.IsMuted(hook.VolumeControl.Control);
        }

        private string Truncate(string name)
        {
            if (name.Length > 10)
            {
                return name.Substring(0, 10) + "...";
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
                if (MessageBox.Show(Properties.strings.UpgradeMessageBox, "EZBlocker", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(website);
                    Application.Exit();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Properties.strings.UpgradeErrorMessageBox, "EZBlocker");
            }
        }

        private void LogAction(string action)
        {
            if (lastAction.Equals(action) && DateTime.Now - lastRequest < TimeSpan.FromMinutes(5)) return;
            Task.Run(() => a.LogAction(action));
            lastAction = action;
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
            
            // Set up Analytics
            if (String.IsNullOrEmpty(Properties.Settings.Default.CID))
            {
                Properties.Settings.Default.CID = Analytics.GenerateCID();
                Properties.Settings.Default.Save();
            }
            a = new Analytics(Properties.Settings.Default.CID, Assembly.GetExecutingAssembly().GetName().Version.ToString());

            // Start Spotify hook
            hook = new SpotifyHook();

            /* Start EZBlocker listener
            listener = new Listener();
            Task.Run(() => listener.Listen()); */

            MainTimer.Enabled = true;

            LogAction("/launch");

            Task.Run(() => CheckUpdate());
        }

        private void CheckPatch(bool launch)
        {
            string currentVersion = FileVersionInfo.GetVersionInfo(spotifyPath).FileVersion;
            if (!Properties.Settings.Default.LastPatched.Equals(currentVersion) || launch) // Always attempt to patch on launch
            {
                // MessageBox.Show("EZBlocker needs to modify Spotify.\r\n\r\nTo return to the original, right click the EZBlocker icon in your task tray and choose 'Remove Patch'.", "EZBlocker");
                if (!patcher.Patch())
                {
                    MessageBox.Show(Properties.strings.PatchErrorMessageBox, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Properties.Settings.Default.LastPatched = currentVersion;
                    Properties.Settings.Default.Save();
                }
            }
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
                Notify(Properties.strings.HiddenNotify);
            }
        }

        private void SkipAdsCheckbox_Click(object sender, EventArgs e)
        {
            if (!MainTimer.Enabled) return; // Still setting up UI
            if (!IsUserAnAdmin())
            {
                MessageBox.Show(Properties.strings.BlockBannersUAC, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(Properties.strings.BlockBannersRestart, "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(Properties.strings.VolumeMixerOpenError, "EZBlocker");
            }
        }

        private void WebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(Properties.strings.ReportProblemMessageBox.Replace("{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()).Replace("{1}", FileVersionInfo.GetVersionInfo(spotifyPath).FileVersion), "EZBlocker");
            Clipboard.SetText(Properties.strings.ReportProblemClipboard.Replace("{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()).Replace("{1}", FileVersionInfo.GetVersionInfo(spotifyPath).FileVersion));
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

        private void undoPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastPatched = "";
            Properties.Settings.Default.Save();

            if (patcher.Restore())
            {
                MessageBox.Show(Properties.strings.UndoPatchOKMessageBox, "EZBlocker");
            }
            else
            {
                MessageBox.Show(Properties.strings.UndoPatchFailMessageBox, "EZBlocker");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!MainTimer.Enabled) return; // Still setting up UI
            if (!Properties.Settings.Default.UserEducated)
            {
                var result = MessageBox.Show(Properties.strings.OnExitMessageBox, "EZBlocker",
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
