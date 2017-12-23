using System.Drawing;

namespace EZBlocker
{
    partial class ArtistManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));

            this.lblArtist = new System.Windows.Forms.Label();
            this.lblBlockedArtists = new System.Windows.Forms.Label();
            this.btnRemoveArtist = new System.Windows.Forms.Button();
            this.btnBlockArtist = new System.Windows.Forms.Button();
            this.tbArtist = new System.Windows.Forms.TextBox();
            this.lstBlockedArtists = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.Location = new System.Drawing.Point(13, 32);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(33, 13);
            this.lblArtist.TabIndex = 0;
            this.lblArtist.Text = "Artist:";
            // 
            // lblBlockedArtists
            // 
            this.lblBlockedArtists.AutoSize = true;
            this.lblBlockedArtists.Location = new System.Drawing.Point(12, 90);
            this.lblBlockedArtists.Name = "lblBlockedArtists";
            this.lblBlockedArtists.Size = new System.Drawing.Size(79, 13);
            this.lblBlockedArtists.TabIndex = 1;
            this.lblBlockedArtists.Text = "Blocked artists:";
            // 
            // btnRemoveArtist
            // 
            this.btnRemoveArtist.Location = new System.Drawing.Point(367, 131);
            this.btnRemoveArtist.Name = "btnRemoveArtist";
            this.btnRemoveArtist.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveArtist.TabIndex = 3;
            this.btnRemoveArtist.Text = "Remove";
            this.btnRemoveArtist.UseVisualStyleBackColor = true;
            this.btnRemoveArtist.Click += new System.EventHandler(this.btnRemoveArtist_Click);
            // 
            // btnBlockArtist
            // 
            this.btnBlockArtist.Location = new System.Drawing.Point(367, 30);
            this.btnBlockArtist.Name = "btnBlockArtist";
            this.btnBlockArtist.Size = new System.Drawing.Size(75, 23);
            this.btnBlockArtist.TabIndex = 4;
            this.btnBlockArtist.Text = "Block";
            this.btnBlockArtist.UseVisualStyleBackColor = true;
            this.btnBlockArtist.Click += new System.EventHandler(this.btnBlockArtist_Click);
            // 
            // tbArtist
            // 
            this.tbArtist.Location = new System.Drawing.Point(108, 32);
            this.tbArtist.Name = "tbArtist";
            this.tbArtist.Size = new System.Drawing.Size(247, 20);
            this.tbArtist.TabIndex = 5;
            // 
            // lstBlockedArtists
            // 
            this.lstBlockedArtists.FormattingEnabled = true;
            this.lstBlockedArtists.Location = new System.Drawing.Point(108, 90);
            this.lstBlockedArtists.Name = "lstBlockedArtists";
            this.lstBlockedArtists.Size = new System.Drawing.Size(247, 95);
            this.lstBlockedArtists.TabIndex = 6;
            // 
            // ArtistManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 224);
            this.Controls.Add(this.lstBlockedArtists);
            this.Controls.Add(this.tbArtist);
            this.Controls.Add(this.btnBlockArtist);
            this.Controls.Add(this.btnRemoveArtist);
            this.Controls.Add(this.lblBlockedArtists);
            this.Controls.Add(this.lblArtist);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ArtistManagerForm";
            this.ShowInTaskbar = false;
            this.Text = "Artist Manager";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Load += new System.EventHandler(this.ArtistManagerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Label lblBlockedArtists;
        private System.Windows.Forms.Button btnRemoveArtist;
        private System.Windows.Forms.Button btnBlockArtist;
        private System.Windows.Forms.TextBox tbArtist;
        private System.Windows.Forms.ListBox lstBlockedArtists;
    }
}