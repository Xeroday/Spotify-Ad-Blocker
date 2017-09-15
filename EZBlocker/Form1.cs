using CoreAudio;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private bool muted = false;
        private bool spotifyMute = false;
        private float volume = 0.9f;
        private string lastSongName = "";
        private int exitTolerance = 0;
        private ToolTip songToolTip = new ToolTip();

        private string nircmdPath = Application.StartupPath + @"\nircmd.exe";
        private string jsonPath = Application.StartupPath + @"\Newtonsoft.Json.dll";
        private string coreaudioPath = Application.StartupPath + @"\CoreAudio.dll";

        private string spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
        private string spotifyPrefsPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\prefs";
        private string volumeMixerPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\SndVol.exe";
        private string hostsPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\drivers\etc\hosts";

        private int numAdPatches = 6;
        private string[] adHosts = { "pubads.g.doubleclick.net", "securepubads.g.doubleclick.net", "www.googletagservices.com", "gads.pubmatic.com", "ads.pubmatic.com", "spclient.wg.spotify.com" };

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
        private const string issue_website = @"https://github.com/MatrixDJ96/EZBlocker-Modded/issues";
        private const string update_website = @"https://github.com/MatrixDJ96/EZBlocker-Modded/releases";
        private const string modder_website = @"https://plus.google.com/u/0/+MattiaRombi";
        private const string author_website = @"https://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";
        private const string design_website = @"http://www.bruske.com.br/";

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

        private bool noErrors = false;
        private int hostsPatched = 0; // In final state => 0 - not patched; 1 - something wrong; 2 - patched;
        private string[] appliedPatchesLines = { null, null, null, null, null, null };
        private string[] invalidPatchesLines = { null, null, null, null, null, null };
        private string[] hostsContentLines;
        private string hostsContentText;

        public Main()
        {
            InitializeComponent();
        }

        private bool ExecuteSpotify()
        {
            if (File.Exists(spotifyPath) && Process.GetProcessesByName("spotify").Length < 1)
            {
                Process.Start(spotifyPath);
                return true;
            }
            return false;
        }

        /**
         * Contains the logic for when to mute Spotify
         **/

        bool not_found_notify = false;

        private void TimerMain_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Process.GetProcessesByName("spotify").Length < 1)
                {
                    if (exitTolerance > 20)
                    {
                        if (not_found_notify == false)
                        {
                            Notify("Spotify not found.\r\nExiting EZBlocker.");
                            not_found_notify = true;
                            Application.Exit();
                        }
                    }
                    exitTolerance += 1;
                }
                else
                {
                    exitTolerance = 0;
                }

                WebHelperResult whr = WebHelperHook.GetStatus();

                if (whr.isAd) // Track is Ad
                {
                    timerMain.Interval = 1000;
                    if (whr.isPlaying)
                    {
                        if (lastSongName != whr.songName)
                        {
                            if (!muted)
                            {
                                Mute(1);
                            }
                            lastSongName = whr.songName;
                            labelSongName.Text = "Muting Ad";
                            songToolTip.SetToolTip(labelSongName, "");
                        }
                    }
                    else // Ad is paused
                    {
                        Resume();
                        labelSongName.Text = "Ad is paused";
                        songToolTip.SetToolTip(labelSongName, "");
                    }
                }
                else if (whr.isPrivateSession)
                {
                    if (lastSongName != whr.songName)
                    {
                        lastSongName = whr.songName;
                        labelSongName.Text = "Playing: *Private Session*";
                        songToolTip.SetToolTip(labelSongName, "Please disable 'Private Session' on Spotify for EZBlocker to function properly.");
                    }
                }
                else if (!whr.isRunning)
                {
                    timerMain.Interval = 5000;
                    labelSongName.Text = "Spotify is not running";
                    songToolTip.SetToolTip(labelSongName, "");
                }
                else if (!whr.isPlaying)
                {
                    lastSongName = "";
                    labelSongName.Text = "Spotify is paused";
                    songToolTip.SetToolTip(labelSongName, "");
                }
                else // Song is playing
                {
                    if (muted)
                    {
                        Mute(0);
                    }
                    if (timerMain.Interval > 600) timerMain.Interval = 600;
                    if (lastSongName != whr.songName)
                    {
                        lastSongName = whr.songName;
                        labelSongName.Text = "Playing: " + ShortenName(whr.songName);
                        songToolTip.SetToolTip(labelSongName, "Artist: " + ShortenName(whr.artistName));
                    }
                }
            }
            catch (Exception ex) {}
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
                    }
                }
            }
            else // Mute all of Windows
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = "/C nircmd mutesysvolume " + i.ToString()
                };
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
         *  Plays next track queued on Spotify
         **/
        private void NextTrack()
        {
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

        private string ShortenName(string name)
        {
            if (name.Length > 25)
            {
                return name.Substring(0, 25) + "...";
            }
            return name;
        }

        /**
         * Checks if the current installation is the latest version. Prompts user if not.
         * Need testing
         **/
        private void CheckUpdate()
        {
            int latest = Convert.ToInt32(GetPage("https://raw.githubusercontent.com/MatrixDJ96/EZBlocker/master/Version.txt", EZBlockerUA));
            int current = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            if (latest <= current)
                return;
            if (MessageBox.Show("There is a newer version of EZBlocker available. Would you like to upgrade?", "EZBlocker", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(modder_website);
                Application.Exit();
            }
        }

        /**
         * Send a request every 5 minutes to Google Analytics
         **/
        private void TimerHeartbeat_Tick(object sender, EventArgs e)
        {
            LogAction("/heartbeat");
        }

        private static bool HasNet40()
        {
            try
            {
                using (RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP"))
                {
                    string[] version_names = installed_versions.GetSubKeyNames();
                    //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                    double version = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                    if (version >= 4) return true;
                }
            }
            catch (Exception ex) {}
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
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Normal;
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
                if (ExecuteSpotify() == true)
                {
                    Notify("Restarting Spotify.");
                }
            }
            base.WndProc(ref m);
        }

        private void Notify(string message)
        {
            notifyIcon.ShowBalloonTip(10000, "EZBlocker", message, ToolTipIcon.None);
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
            }
        }

        private string[] GetHostsContentLines()
        {
            return File.ReadAllLines(hostsPath);
        }

        private string GetTextFromLines(string[] lines)
        {
            string newText = "";
            foreach (string line in lines)
            {
                newText += line + "\r\n"; // Windows format
            }
            return newText;
        }

        private void RemoveString(ref string workingString, string stringToRemove)
        {
            int times = 0;
            string newString = workingString.Replace(stringToRemove + "\r\n", "");
            while (newString == workingString)
            {
                if (times == 0)
                {
                    newString = workingString.Replace("\r\n" + stringToRemove, "");
                }
                else if (times == 1)
                {
                    newString = workingString.Replace(stringToRemove, "");
                    break;
                }
                times++;
            }
            workingString = newString;
        }

        private void SetHostsPatches()
        {
            hostsContentLines = GetHostsContentLines();
            hostsContentText = GetTextFromLines(hostsContentLines);
            File.WriteAllText(hostsPath, hostsContentText); // Reformat text to Windows format

            if (checkBoxBlockAds.Checked)
            {
                using (StreamWriter sw = File.AppendText(hostsPath))
                {
                    if (hostsPatched == 0)
                    {
                        sw.WriteLine();
                    }
                    for (int i = 0; i < numAdPatches; i++)
                    {
                        if (!hostsContentText.Contains("0.0.0.0 " + adHosts[i]))
                        {
                            sw.WriteLine("0.0.0.0 " + adHosts[i]);
                        }
                    }
                    hostsPatched = 2;
                }
            }
            else
            {
                hostsContentText += "\r\nRemoving hosts patches...";
                for (int i = 0; i < numAdPatches; i++)
                {
                    if (hostsContentText.Contains("0.0.0.0 " + adHosts[i]))
                    {
                        RemoveString(ref hostsContentText, "0.0.0.0 " + adHosts[i]);
                    }
                }
                RemoveString(ref hostsContentText, "\r\nRemoving hosts patches...");
                hostsPatched = 0;
                File.WriteAllText(hostsPath, hostsContentText);
            }
        }

        private void CheckBoxMute_CheckedChanged(object sender, EventArgs e)
        {
            spotifyMute = checkBoxMute.Checked;
            if (visitorId == null) return; // Still setting up UI
            Properties.Settings.Default.SpotifyMute = spotifyMute;
            Properties.Settings.Default.Save();
        }

        private void CheckBoxBlockAds_Click(object sender, EventArgs e)
        {
            if (visitorId == null) return; // Still setting up UI

            if (!IsUserAnAdmin())
            {
                MessageBox.Show("Enabling/Disabling this option requires Administrator privileges.\r\nPlease restart EZBlocker with \"Run as Administrator\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBoxBlockAds.Checked = !checkBoxBlockAds.Checked;
                return;
            }
            if (!File.Exists(hostsPath))
            {
                File.Create(hostsPath).Close();
            }
            try
            {
                SetHostsPatches();
                Properties.Settings.Default.SkipAds = checkBoxBlockAds.Checked;
                Properties.Settings.Default.Save();
                MessageBox.Show("You may need to restart Spotify or your computer for this setting to take effect.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string option = "applying";
                if (!checkBoxBlockAds.Checked)
                {
                    option = "removing";
                }
                MessageBox.Show("Something went wrong while " + option + " hosts patches...\r\nProbably hosts file is set in read-only mode.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBoxBlockAds.Checked = !checkBoxBlockAds.Checked;
            }
        }

        private void CheckBoxStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (visitorId == null) return; // Still setting up UI
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (checkBoxStartup.Checked)
            {
                startupKey.SetValue("EZBlocker", "\"" + Application.ExecutablePath + "\"");
            }
            else
            {
                startupKey.DeleteValue("EZBlocker");
            }
        }

        private void BtnMixer_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(volumeMixerPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open mixer. This is only available on Windows 7/8/10", "EZBlocker");
            }
        }

        private void BtnMute_Click(object sender, EventArgs e)
        {
            Mute(2);
        }

        private void LinkLabelModder_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(modder_website);
        }

        private void LinkLabelAuthor_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(author_website);
        }

        private void LinkLabelDesigner_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(design_website);
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Normal;
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnIssue_Click(object sender, EventArgs e)
        {
            Process.Start(issue_website);
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void BtnMixer_MouseHover(object sender, EventArgs e)
        {
            btnMixer.BackgroundImage = Properties.Resources.Options_2;
        }

        private void BtnMixer_MouseLeave(object sender, EventArgs e)
        {
            btnMixer.BackgroundImage = Properties.Resources.Options_1;
        }


        private void Main_Load(object sender, EventArgs e)
        {
            // Enable web helper
            if (File.Exists(spotifyPrefsPath))
            {
                string[] lines = File.ReadAllLines(spotifyPrefsPath);
                bool webhelperEnabled = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("webhelper.enabled"))
                    {
                        if (lines[i].Contains("false"))
                        {
                            lines[i] = "webhelper.enabled=true";
                            File.WriteAllLines(spotifyPrefsPath, lines);
                        }
                        webhelperEnabled = true;
                        break;
                    }
                }
                if (!webhelperEnabled)
                {
                    File.AppendAllText(spotifyPrefsPath, "\r\nwebhelper.enabled=true");
                }
            }

            CheckUpdate();

            // Start Spotify and give EZBlocker higher priority
            try
            {
                ExecuteSpotify();
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly
            }
            catch (Exception ex) {}

            // Extract dependencies
            try
            {
                if (File.Exists(nircmdPath)) File.Delete(nircmdPath);
                if (File.Exists(jsonPath)) File.Delete(jsonPath);
                if (File.Exists(coreaudioPath)) File.Delete(coreaudioPath);
                File.WriteAllBytes(nircmdPath, Properties.Resources.nircmd);
                File.WriteAllBytes(jsonPath, Properties.Resources.Newtonsoft_Json);
                File.WriteAllBytes(coreaudioPath, Properties.Resources.CoreAudio);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading EZBlocker dependencies. Please run EZBlocker as administrator or put EZBlocker in a user folder.");
            }

            // Set up UI
            checkBoxMute.Checked = Properties.Settings.Default.SpotifyMute;

            checkBoxBlockAds.Checked = Properties.Settings.Default.SkipAds;
            hostsContentLines = GetHostsContentLines();

            int loadingStuck = -1; // Used to check error and save position from hostsContentLines

            if (File.Exists(hostsPath))
            {
                int posInv = 0;
                foreach (string line in hostsContentLines)
                {
                    if (hostsContentLines[posInv].Contains("open.spotify.com"))
                    {
                        loadingStuck = posInv;
                        break;
                    }
                    posInv++;
                }
                if (loadingStuck == -1)
                {
                    posInv = 0;
                    int posVal = 0;
                    foreach (string line in hostsContentLines)
                    {
                        for (int i = 0; i < numAdPatches; i++)
                        {
                            if (line.Contains(" " + adHosts[i])) // check with space to avoid conflit with (for example) 'pubads.g.doubleclick.net' and 'securepubads.g.doubleclick.net'
                            {
                                if (line.Contains("0.0.0.0 " + adHosts[i]))
                                {
                                    hostsPatched++;
                                    appliedPatchesLines[posVal] = line;
                                    posVal++;
                                }
                                else
                                {
                                    invalidPatchesLines[posInv] = line;
                                    posInv++;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (hostsPatched > 0)
            {
                if (hostsPatched == numAdPatches)
                {
                    hostsPatched = 2; // patched
                }
                else
                {
                    hostsPatched = 1; // something wrong
                }
            }

            if (hostsPatched == 1 || loadingStuck > -1)
            {
                if (IsUserAnAdmin())
                {
                    int error = 0;
                    if (loadingStuck > -1)
                    {
                        try
                        {
                            hostsContentText = GetTextFromLines(hostsContentLines);
                            RemoveString(ref hostsContentText, hostsContentLines[loadingStuck]);
                            File.WriteAllText(hostsPath, hostsContentText);
                        }
                        catch (Exception ex)
                        {
                            error = 1;
                            MessageBox.Show("An error has been detected in your hosts file but it couldn't be fixed.\r\nTry to remove manually from that file this line:\r\n\r\n" + hostsContentLines[loadingStuck], "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    if (hostsPatched == 1)
                    {
                        try
                        {
                            SetHostsPatches();
                            hostsContentText = GetTextFromLines(GetHostsContentLines());
                            foreach (string line in invalidPatchesLines)
                            {
                                if (line != null)
                                {
                                    RemoveString(ref hostsContentText, line);
                                }
                            }
                            File.WriteAllText(hostsPath, hostsContentText);
                        }
                        catch (Exception ex)
                        {
                            error = 1;
                            hostsContentText = GetTextFromLines(GetHostsContentLines());
                            string invalidPatchesText = "\r\n";
                            string toAddPatchesText = "\r\n";
                            for (int i = 0; i < numAdPatches; i++)
                            {
                                if (invalidPatchesLines[i] != null)
                                {
                                    invalidPatchesText += "\r\n" + invalidPatchesLines[i];
                                }
                                if (hostsContentText.Contains("0.0.0.0 " + adHosts[i]))
                                {
                                    invalidPatchesText += "\r\n0.0.0.0 " + adHosts[i];
                                }
                                else
                                {
                                    toAddPatchesText += "\r\n0.0.0.0 " + adHosts[i];
                                }
                            }
                            if (toAddPatchesText != "\r\n")
                            {
                                invalidPatchesText += "\r\n\r\nOr try to change/add manually these lines to enable SkipAds feature:" + toAddPatchesText;
                            }
                            MessageBox.Show("An error has been detected in your hosts file but it couldn't be fixed.\r\nTry to remove manually these lines to disable SkipAds feature:" + invalidPatchesText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                    if (error == 0)
                    {
                        MessageBox.Show("There was/were an error/s in your hosts file and a patch has been applied so you may need to restart Spotify or your computer for this setting to take effect.", "EZBlocker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("An error has been detected in your hosts file but it couldn't be fixed.\r\nPlease run EZBlocker as administrator to try fixing this problem.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            else if ((checkBoxBlockAds.Checked && hostsPatched == 0) || (!checkBoxBlockAds.Checked && hostsPatched == 2))
            {
                checkBoxBlockAds.Checked = !checkBoxBlockAds.Checked;
                Properties.Settings.Default.SkipAds = checkBoxBlockAds.Checked;
                Properties.Settings.Default.Save();

            }

            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (startupKey.GetValue("EZBlocker") != null)
            {
                if (startupKey.GetValue("EZBlocker").ToString() == "\"" + Application.ExecutablePath + "\"")
                {
                    checkBoxStartup.Checked = true;
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
            if (string.IsNullOrEmpty(Properties.Settings.Default.UID))
            {
                Properties.Settings.Default.UID = rnd.Next(100000000, 999999999).ToString(); // Build unique visitorId;
                Properties.Settings.Default.Save();
            }
            visitorId = Properties.Settings.Default.UID;

            if (!HasNet40())
            {
                if (MessageBox.Show("You do not have .NET Framework 4.0. Download now?", "EZBlocker Error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    Process.Start("https://www.microsoft.com/en-US/download/details.aspx?id=17718");
                }
                else
                {
                    MessageBox.Show("EZBlocker may not function properly without .NET Framework 4.0 or above.");
                }
            }

            Mute(0);

            timerMain.Enabled = true;

            if (Process.GetProcessesByName("spotifywebhelper").Length < 1)
            {
                Notify("Please enable 'Allow Spotify to be opened from the web' in your Spotify 'Preferences' -> 'Advanced settings'.");
            }
            

            noErrors = true; // All went fine
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.SpotifyMute && noErrors && !Properties.Settings.Default.SkipAds)
            {
                var result = MessageBox.Show("Spotify ads will not be muted if EZBlocker is not running.\r\n\r\nAre you sure you want to exit?", "EZBlocker",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Warning);

                e.Cancel = (result == DialogResult.No);
            }
        }
    }
}
