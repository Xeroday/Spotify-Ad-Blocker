using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace EZBlocker
{

    public partial class Main : Form
    {
        private string title = string.Empty; // Title of the Spotify window
        private string lastChecked = string.Empty; // Previous artist

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int MEDIA_PLAYPAUSE = 0xE0000;

        private const string ua = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
        private const string website = @"http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/";

        public Main()
        {
            //Console.WriteLine("Checking update");
            //CheckUpdate();
            if (!File.Exists(LiveSettings.nircmdPath))
            {
                Console.WriteLine("Writing nircmd");
                File.WriteAllBytes(LiveSettings.nircmdPath, EZBlocker.Properties.Resources.nircmdc);
            }
            if (!File.Exists(LiveSettings.jsonPath))
            {
                Console.WriteLine("Writing Json");
                File.WriteAllBytes(LiveSettings.jsonPath, EZBlocker.Properties.Resources.Newtonsoft_Json);
            }

            Console.WriteLine("Initializing");
            InitializeComponent();

            LiveSettings.readSettings();
            this.CloseToolStripMenuItem.Checked = LiveSettings.closeTray;
            this.MinimizeToolStripMenuItem.Checked = LiveSettings.minTray;
            this.AutoAddCheckbox.Checked = LiveSettings.autoAdd;
            this.TopMostCheckbox.Checked = LiveSettings.topmost;


            try
            {
                Console.WriteLine("Downloading blocklist");
                WebClient w = new WebClient();
                w.Headers.Add("user-agent", Application.ProductName + " " + Application.ProductVersion + " " + Environment.OSVersion);

                String list = w.DownloadString("http://www.ericzhang.me/dl/?file=blocklist.txt");
                while (list.Length > 0)
                {
                    String entry = list.Substring(0, list.IndexOf(Environment.NewLine));
                    if (!LiveSettings.blocklist.Contains(entry.Trim()))
                    {
                        LiveSettings.blocklist.Add(entry.Trim());
                    }
                    list = list.Substring(entry.Length + Environment.NewLine.Length);
                }
            }
            catch (Exception ex)
            {
                NotifyIcon.ShowBalloonTip(5000, Application.ProductName, ex.Message, ToolTipIcon.None);
            }


            try
            {
                Console.WriteLine("Raising priority");
                System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly
                Console.WriteLine("Starting Spotify");
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe");
            }
            catch
            {
                // Ignore
            }
            Console.WriteLine("Unmuting");
            setVolume(volume.u0nmuted); // Unmute Spotify, if muted
            Console.WriteLine("Reading blocklist");

        }

        /**
         * Contains the logic for when to mute Spotify
         **/
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            UpdateTitle();

            if (IsPlaying() && title.IndexOf("-") + 2 < title.Length)
                Text = "Currently playing: " + this.title.Substring(title.IndexOf("-") + 2);

            if (!IsPlaying())
                return;
            string artist = GetArtist();
            if (lastChecked.Equals(artist))
                return;
            lastChecked = artist;
            if (LiveSettings.autoAdd) // Auto add to block list
            {
                if (!IsInBlocklist(artist) && IsAd(artist))
                {
                    block(artist);
                    Notify("Automatically added " + artist + " to your blocklist.");
                }
            }
            if (IsInBlocklist(artist)) // Should mute
            {
                setVolume(volume.m1uted); // Mute Spotify
                ResumeTimer.Start();
                Console.WriteLine("Muted " + artist);
                // Notify(artist + " is on your blocklist and has been muted.");
            }
            else // Should unmute
            {
                setVolume(volume.u0nmuted); // Unmute Spotify
                ResumeTimer.Stop();
                Console.WriteLine("Unmuted " + artist);
                Notify(artist + " is not on your blocklist. Open " + Application.ProductName + " to add it.");
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
        private bool UpdateTitle()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.Equals("spotify")))
            {
                title = t.MainWindowTitle;
                return true;
            }
            return false;
        }

        /**
         * Gets the Spotify process handle
         **/
        private IntPtr GetHandle()
        {
            foreach (Process t in Process.GetProcesses().Where(t => t.ProcessName.Equals("spotify")))
            {
                return t.Handle;
            }
            return IntPtr.Zero;
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
         * Checks if an artist is in the blocklist (Exact match only)
         **/
        private bool IsInBlocklist(string artist)
        {
            return LiveSettings.blocklist.Contains(artist);
        }

        /**
         * Attempts to check if the current song is an ad
         **/
        private bool IsAd(string artist)
        {
            return (isAdSpotify(artist) && IsAdiTunes(artist));
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
            if (LiveSettings.notify)
                NotifyIcon.ShowBalloonTip(10000, Application.ProductName, message, ToolTipIcon.None);
        }

        /**
         * Checks if the current installation is the latest version. Prompts user if not.
         **/
        private void CheckUpdate()
        {
            int latest = Convert.ToInt32(GetPage("http://www.ericzhang.me/dl/?file=EZBlocker-version.txt", Application.ProductName + " " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " " + System.Environment.OSVersion));
            int current = Convert.ToInt32(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
            if (latest <= current)
                return;
            if (MessageBox.Show("There is a newer version of " + Application.ProductName + " available. Would you like to upgrade?", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(website);
                Application.Exit();
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                this.Visible = !this.Visible;
                this.BringToFront();
            }
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void BlockButton_Click(object sender, EventArgs e)
        {
            if (BlockButton.Text.StartsWith("&Block"))
                block(GetArtist());
            else
                unblock(GetArtist());

            lastChecked = String.Empty; // Reset last checked so we can auto mute
        }

        /**
         * Adds an artist to the blocklist.
         * 
         * Returns false if Spotify is not playing.
         **/
        private bool block(String artist)
        {
            if (!IsPlaying())
                return false;
            BlockButton.TextAlign = ContentAlignment.MiddleRight;
            NotifyIcon.Icon = Properties.Resources.blocked;
            BlockButton.Text = BlockButton.Text.Replace("&Block", "Un&block");
            BlockThisSongToolStripMenuItem.Checked = true;

            if (!LiveSettings.blocklist.Contains(artist))
                LiveSettings.blocklist.Add(artist);

            if (MuteButton.Text.Contains("&Mute"))
            {
                setVolume(volume.m1uted);
                MuteButton.Text = MuteButton.Text.Replace("&Mute", "Un&mute");
                MuteButton.TextAlign = ContentAlignment.MiddleRight;
            }

            return true;
        }


        private void unblock(String artist)
        {
            NotifyIcon.Icon = Properties.Resources.allowed;
            BlockButton.Text = BlockButton.Text.Replace("Un&block", "&Block");
            //uncheck "block this song notify" menu item
            BlockThisSongToolStripMenuItem.Checked = false;
            BlockButton.TextAlign = ContentAlignment.MiddleLeft;
            while (LiveSettings.blocklist.Contains(artist))
                LiveSettings.blocklist.Remove(artist);

            if (MuteButton.Text.Contains("Un&mute"))
            {
                setVolume(volume.u0nmuted);
                MuteButton.Text = MuteButton.Text.Replace("Un&mute", "&Mute");
                MuteButton.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void AutoAddCheck_CheckedChanged(object sender, EventArgs e)
        {
            LiveSettings.autoAdd = AutoAddCheckbox.Checked;
        }

        private void NotifyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LiveSettings.notify = NotifyCheckbox.Checked;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (EditButton.Text == "&Edit Blocklist")
            {
                BlockButton.Enabled = false;
                EditButton.TextAlign = ContentAlignment.MiddleRight;
                blocklistBox.Items.Clear();
                blocklistBox.BeginUpdate(); //prevent flicker
                foreach (String s in LiveSettings.blocklist)
                    blocklistBox.Items.Add(new ListViewItem(s));

                blocklistBox.EndUpdate(); //restore layout

                EditButton.Text = "Finish &Editing";
            }
            else
            {
                BlockButton.Enabled = true;
                EditButton.TextAlign = ContentAlignment.MiddleLeft;
                LiveSettings.blocklist.Clear();
                foreach (ListViewItem item in blocklistBox.Items)
                {
                    LiveSettings.blocklist.Add(item.Text);
                }

                EditButton.Text = "&Edit Blocklist";
            }
        }

        /**
         * Mutes/Unmutes Spotify.
         * 
         * i: 0 = unmute, 1 = mute, 2 = toggle
         **/
        enum volume
        {
            u0nmuted = 0,
            m1uted = 1,
            t2oggle_muted = 2
        };

        private void MuteButton_Click(object sender, EventArgs e)
        {
            if (MuteButton.Text.Contains("Un&mute"))
            {
                setVolume(volume.u0nmuted);
                MuteButton.Text = MuteButton.Text.Replace("Un&mute", "&Mute");
                MuteButton.TextAlign = ContentAlignment.MiddleLeft;
            }
            else
            {
                setVolume(volume.m1uted);
                MuteButton.Text = MuteButton.Text.Replace("&Mute", "Un&mute");
                MuteButton.TextAlign = ContentAlignment.MiddleRight;
            }
        }

        private void setVolume(volume Volume)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C nircmdc muteappvolume spotify.exe " + Volume.ToString().Substring(1, 1);
            process.StartInfo = startInfo;
            process.Start();
        }

        private void WebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(website);
        }

        private void TopMostCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LiveSettings.topmost = TopMostCheckbox.Checked;
            this.TopMost = TopMostCheckbox.Checked;
        }

        private void ButtonRemoveEntry_Click(object sender, EventArgs e)
        {
            ListViewItem[] selected = new ListViewItem[blocklistBox.SelectedItems.Count];

            for (int x = 0; x < blocklistBox.SelectedIndices.Count; x++)
            {
                selected.SetValue(blocklistBox.Items[x], x);
            }

            foreach (ListViewItem item in selected)
                blocklistBox.Items.Remove(item);
        }


        bool exitApp = false;
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            LiveSettings.writeSettings();
            if (LiveSettings.closeTray && !exitApp)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exitApp = true;
            this.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            artistInfoLabel.BackColor = Color.FromArgb(150, Color.White);
        }

        private void BlockThisSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // checked status is althered in block() and unblock()
            BlockButton.PerformClick();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && LiveSettings.minTray)
                this.Hide();
        }

        private void whoAmI_Click(object sender, EventArgs e)
        {
            if (blocklistBox.SelectedItems.Count == 0)
            {
                String artist = GetArtist();
                artistInfoLabel.Text = findDefine(artist);
                artistInfoPicture.LoadAsync(getGGImageFirst(artist + " photo"));
            }
            else
            {
                String corp = blocklistBox.SelectedItems[0].Text;
                artistInfoLabel.Text = findDefine(corp);
                artistInfoPicture.LoadAsync(getGGImageFirst(corp + " logo"));
            }

            //*[@id="imgGrp"]/div[1]
        }

        private String findDefine(String artist)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                String UA = @"Mozilla/5.0 (Linux; Android 4.4; Nexus 5 Build/BuildID) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/30.0.0.0 Mobile Safari/537.36";
                doc.LoadHtml(getHTML("http://en.m.wikipedia.org/wiki/" + artist, UA));
                HtmlAgilityPack.HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
                HtmlAgilityPack.HtmlNode divContentNode = bodyNode.SelectSingleNode("//div[@id='mw-mf-viewport']/div[@id='mw-mf-page-center']/div[@id='content_wrapper']/div[@id='content']/div/div[@id='mw-content-text']/p");
                String removeSpaces = Regex.Replace(divContentNode.InnerText, @"\s", " ");
                return Regex.Replace(removeSpaces, @"\[[0-9]\]", "");
            }
            catch (Exception ex) { this.Text = ex.Message; }

            return "Service unavailable";
        }

        private String getHTML(String url, String UA)
        {
            HttpWebRequest rq = (HttpWebRequest)HttpWebRequest.Create(url);
            rq.UserAgent = UA; WebResponse rs = rq.GetResponse();

            //byte[] bytes = new byte[rs.Length];
            //while (rs.Position < rs.Length)
            //{
            //    bytes[rs.Position] = (byte)rs.ReadByte();
            //}

            StreamReader sr = new StreamReader(rs.GetResponseStream());
            return sr.ReadToEnd();

        }

        private String getGGImageFirst(String keyword)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                String UA = @"Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0; SAMSUNG; SGH-i917)";
                doc.LoadHtml(getHTML("http://www.bing.com/images/search?q=" + keyword, UA));
                HtmlAgilityPack.HtmlNode linknode  = doc.DocumentNode.SelectSingleNode("//body").ChildNodes[3]
                    .ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0];
                HtmlAgilityPack.HtmlAttribute src = linknode.Attributes[3];

                return src.Value;
            }
            catch (Exception ex) { this.Text = ex.Message; }

            return @"http://upload.wikimedia.org/wikipedia/commons/5/52/Unknown.jpg";
        }

        private void CloseToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            LiveSettings.closeTray = CloseToolStripMenuItem.Checked;
        }

        private void MinimizeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            LiveSettings.minTray = MinimizeToolStripMenuItem.Checked;
        }
    }
}
