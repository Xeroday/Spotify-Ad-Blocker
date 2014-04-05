using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EZBlocker
{
    public partial class Blocklist : Form
    {
        public Blocklist()
        {
            InitializeComponent();
        }

        private void Blocklist_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (String line in File.ReadAllLines("blocklist.txt").Distinct())
                {
                    AdsList.Items.Add(line);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            AdsList.Items.Remove(AdsList.SelectedItem);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ads = new string[AdsList.Items.Count];
                for (int i = 0; i < AdsList.Items.Count; i++)
                {
                    ads[i] = AdsList.Items[i].ToString();
                }
                File.WriteAllLines("blocklist.txt", ads);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            this.Close();
        }

    }
}
