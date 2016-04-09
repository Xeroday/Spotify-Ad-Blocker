using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AudioSwitcher.AudioApi.CoreAudio;

namespace SpotifyMuter
{
    public partial class Main : Form
    {
        private bool muted = false;
        private string lastArtistName = "";
        
        public static string logPath = Application.StartupPath + @"\SpotifyMuter-log.txt";

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
        
        private string SpotifyMuterUA = "SpotifyMuter " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion;
        private const string website = @"https://www.ericzhang.me/projects/spotify-ad-blocker-SpotifyMuter/";

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
                        StatusLabel.Text = "Playing: *Private Session*";
                        lastArtistName = whr.artistName;
                        MessageBox.Show("Please disable 'Private Session' on Spotify for SpotifyMuter to function properly.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                }
                else if (!whr.isRunning)
                {
                    // Notify("Error connecting to Spotify. Retrying...");
                    File.AppendAllText(logPath, "Not running.\r\n");
                    MainTimer.Interval = 5000;
                    /*
                    MainTimer.Enabled = false;
                    MessageBox.Show("Spotify is not running. Please restart SpotifyMuter after starting Spotify.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    StatusLabel.Text = "Spotify is not running";
                    Application.Exit();
                    */
                }
                else if (!whr.isPlaying)
                {
                    StatusLabel.Text = "Spotify is paused";
                    lastArtistName = "";
                }
                else // Song is playing
                {
                    if (muted) Mute(0);
                    if (MainTimer.Interval > 1000) MainTimer.Interval = 1000;
                    if (lastArtistName != whr.artistName)
                    {
                        StatusLabel.Text = "Playing: " + ShortenName(whr.artistName);
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

           // Mute only Spotify process
           
                var audioController = new CoreAudioController();
                var defaultDevice = audioController.GetDefaultDevice(AudioSwitcher.AudioApi.DeviceType.Playback, AudioSwitcher.AudioApi.Role.Multimedia);
                var sessions = defaultDevice.SessionController.ActiveSessions();

                for (int sessionId = 0; sessionId < sessions.Count(); sessionId++)
                {
                    var currentSession = sessions.ElementAt(sessionId);
                    string displayName = currentSession.DisplayName;
                    if (displayName == "Spotify")
                    {
                        currentSession.IsMuted = muted;
                    }
                }
            
        }

        /**
         * Resumes playing Spotify
         **/
        private void Resume()
        {
            Debug.WriteLine("Resuming Spotify");
            SendMessage(GetHandle(), WM_APPCOMMAND, this.Handle, (IntPtr)MEDIA_PLAYPAUSE);
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

        private string ShortenName(string name)
        {
            if (name.Length > 12)
            {
                return name.Substring(0, 12) + "...";
            }
            return name;
        }

        private void RestoreFromTray()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        
        /**
         * Processes window message and shows SpotifyMuter when attempting to launch a second instance.
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
            NotifyIcon.ShowBalloonTip(10000, "SpotifyMuter", message, ToolTipIcon.None);
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
                Notify("SpotifyMuter is hidden. Double-click this icon to restore.");
            }
        }

        private void SkipAdsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsUserAnAdmin())
            {
                MessageBox.Show("Enabling/Disabling this option requires Administrator privilages.\n\nPlease reopen SpotifyMuter with \"Run as Administrator\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("You may need to restart Spotify or your computer for this setting to take effect.", "SpotifyMuter", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (Exception)
            {
                MessageBox.Show("Could not open Volume Mixer. This is only available on Windows 7/8/10", "SpotifyMuter");
            }
        }

        private void MuteButton_Click(object sender, EventArgs e)
        {
            Mute(2);
        }

        private void WebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(website);
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

            // Start Spotify and give SpotifyMuter higher priority
            try
            {
                if (File.Exists(spotifyPath))
                {
                    Process.Start(spotifyPath);
                }
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure SpotifyMuter runs smoothly
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // Set up UI
            if (File.Exists(hostsPath))
            {
                BlockBannersCheckbox.Checked = File.ReadAllText(hostsPath).Contains("doubleclick.net");
            }
            
            File.AppendAllText(logPath, "-----------\r\n");
            bool unsafeHeaders = WebHelperHook.SetAllowUnsafeHeaderParsing20();
            Debug.WriteLine("Unsafe Headers: " + unsafeHeaders);

            Mute(0);
            
            MainTimer.Enabled = true;
        }
    }
}
