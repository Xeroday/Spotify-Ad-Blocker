using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace EZBlocker
{
    public partial class Main : Form
    {
        private String title = String.Empty; // Title of the Spotify window
        private Boolean autoAdd = true;

        private String blocklistPath = Application.StartupPath + @"\blocklist.txt";

        public Main()
        {
            InitializeComponent();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            UpdateTitle();
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
         * Determines whether or not Spotify is currently playing
         **/
        private Boolean IsPlaying()
        {
            return title.Contains(" - ");
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
            System.IO.File.AppendAllText(blocklistPath, artist + "\r\n");
            return true;
        }

        /**
         * Checks if an artist is in the blocklist (Exact match only)
         **/
        private Boolean IsBlocked(String artist)
        {
            String[] lines = System.IO.File.ReadAllLines(blocklistPath);
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Equals(artist))
                    return true;
            }
            return false;
        }

        private String GetPage(String URL)
        {
            String s = String.Empty;
            WebClient w = new WebClient();
            return s;
        }

        private void BlockButton_Click(object sender, EventArgs e)
        {
            AddToBlockList(GetArtist());
        }

        private void AutoAddCheck_CheckedChanged(object sender, EventArgs e)
        {
            autoAdd = AutoAddCheck.Checked;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", blocklistPath);
        }

    }
}
