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
    public partial class ArtistManagerForm : Form
    {

        private readonly ArtistSettingManager _settingManager;

        public ArtistManagerForm()
        {
            InitializeComponent();
            _settingManager = new ArtistSettingManager();
        }

        private void ArtistManagerForm_Load(object sender, EventArgs e)
        {
            foreach (string artist in _settingManager.blockList)
            {
                lstBlockedArtists.Items.Add(artist);
            }
            
        }

        public void SetArtist(string artist)
        {
            if (tbArtist.Text != artist)
            {
                tbArtist.Text = artist;
            }
        }

        private void btnBlockArtist_Click(object sender, EventArgs e)
        {
            string artist = tbArtist.Text;
            if (string.IsNullOrEmpty(artist))  return;
               
            if (_settingManager.TryAddArtist(artist))
            {
                lstBlockedArtists.Items.Add(artist);
            }
            

        }

        private void btnRemoveArtist_Click(object sender, EventArgs e)
        {
            List<string> items = lstBlockedArtists.SelectedItems.Cast<string>().ToList();
            foreach (string item in items)
            {
                if (_settingManager.TryRemoveArtist(item))
                {
                    lstBlockedArtists.Items.Remove(item);
                }
            }
           
        }

       
    }
}
