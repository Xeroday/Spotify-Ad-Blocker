using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EZBlocker
{
    public partial class CreditsMenu : Form
    {
        public CreditsMenu()
        {
            InitializeComponent();
        }

        private void CreditsText_Enter(object sender, System.EventArgs e)
        {
            CreditsText.SelectionLength = 0;
            CreditsText.SelectionStart = 0;

            ThanksLabel.Focus();
        }

        private void CreditsText_Leave(object sender, System.EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void XerodayLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Xeroday");
        }

        private void Robert3141Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Robert3141");
        }

        private void OpenByteDevLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/OpenByteDev");
        }

        private void cfvescovoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/cfvescovo");
        }

        private void SogolumboLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Sogolumbo");
        }

        private void Ju1jsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Ju1-js");
        }
    }
}
