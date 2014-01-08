using System;
using System.IO; 
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

        private String GetArtist()
        {
            if (!IsPlaying()) return String.Empty;
            Debug.WriteLine("2");
            return title.Substring(10).Split('\u2013')[0].TrimEnd(); // Split at endash
        }

        private void BlockButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("1");
            Debug.WriteLine(GetArtist());
        }

        private void AutoAddCheck_CheckedChanged(object sender, EventArgs e)
        {
            autoAdd = AutoAddCheck.Checked;
        }

    }
}
