using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace EZBlocker
{
    public partial class SettingsMenu : Form
    {
        public SettingsMenu()
        {
            InitializeComponent();
        }

        string ManualSpotifyPath;

        private void FileSelectorButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.InitialDirectory = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\";
                    ofd.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
                    ofd.FilterIndex = 2;
                    ofd.RestoreDirectory = true;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        ManualSpotifyPath = ofd.FileName;

                        //test if it works...
                        Process B = new Process();
                        try
                        {
                            B.StartInfo.UseShellExecute = false;
                            B.StartInfo.FileName = ManualSpotifyPath;
                            B.StartInfo.CreateNoWindow = true;
                            if (B.Start())
                            {
                                // Save Settings
                                AutoClosingMessageBox.Show(global::EZBlocker.Properties.strings.ValidExecutable, "EZBlocker", 3000);
                                Properties.Settings.Default.SpotifyPath = ManualSpotifyPath;
                                Properties.Settings.Default.Save();
                                fileLocation.Text = ManualSpotifyPath;
                            }
                        }
                        catch (Exception)
                        {
                            // Handle exception and do NOT stop A
                            MessageBox.Show(global::EZBlocker.Properties.strings.CouldNotValidateExeError, "EZBlocker");
                        }
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show(Properties.strings.FileSelectorError, "EZBlocker");
            }
        }

        private void DefaultPathButton_Click(object sender, EventArgs e)
        {
            string spotifyPath = Main.GetSpotifyPath();
            if (spotifyPath != "")
            {
                Properties.Settings.Default.SpotifyPath = spotifyPath;
                Properties.Settings.Default.Save();
            }
            else
            {
                spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
                Properties.Settings.Default.SpotifyPath = spotifyPath;
                Properties.Settings.Default.Save();
            }
            ManualSpotifyPath = spotifyPath;
            fileLocation.Text = spotifyPath;
        }

        private void TestSpotifyExeButton_Click(object sender, EventArgs e)
        {
            //test if it works...
            Process B = new Process();
            try
            {
                B.StartInfo.UseShellExecute = false;
                B.StartInfo.FileName = fileLocation.Text;
                B.StartInfo.CreateNoWindow = true;
                if (B.Start())
                {
                    // Save Settings
                    AutoClosingMessageBox.Show(global::EZBlocker.Properties.strings.ValidExecutable, "EZBlocker", 3000);
                    Properties.Settings.Default.SpotifyPath = ManualSpotifyPath;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception)
            {
                // Handle exception and do NOT stop A
                MessageBox.Show(global::EZBlocker.Properties.strings.CouldNotValidateExeError, "EZBlocker");
            }
        }
    }
}
