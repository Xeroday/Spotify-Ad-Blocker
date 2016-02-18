namespace EZBlocker
{
    partial class FrmMain
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
            if (disposing && (components != null))
            {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnMute = new System.Windows.Forms.Button();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblWebSiteLink = new System.Windows.Forms.LinkLabel();
            this.Heartbeat = new System.Windows.Forms.Timer(this.components);
            this.ckbSpotifyMute = new System.Windows.Forms.CheckBox();
            this.btnVolumeMixer = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.ckbBlockBanners = new System.Windows.Forms.CheckBox();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlContent.SuspendLayout();
            this.pnlHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMute
            // 
            this.btnMute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMute.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnMute.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMute.Location = new System.Drawing.Point(50, 114);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(199, 41);
            this.btnMute.TabIndex = 3;
            this.btnMute.Text = "Mute/unmute Spotify";
            this.btnMute.UseVisualStyleBackColor = false;
            this.btnMute.Click += new System.EventHandler(this.MuteButton_Click);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.BalloonTipClicked += new System.EventHandler(this.NotifyIcon_BalloonTipClicked);
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // lblWebSiteLink
            // 
            this.lblWebSiteLink.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblWebSiteLink.LinkColor = System.Drawing.Color.White;
            this.lblWebSiteLink.Location = new System.Drawing.Point(50, 155);
            this.lblWebSiteLink.Margin = new System.Windows.Forms.Padding(0);
            this.lblWebSiteLink.Name = "lblWebSiteLink";
            this.lblWebSiteLink.Size = new System.Drawing.Size(199, 30);
            this.lblWebSiteLink.TabIndex = 5;
            this.lblWebSiteLink.TabStop = true;
            this.lblWebSiteLink.Text = "About ";
            this.lblWebSiteLink.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblWebSiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLink_LinkClicked);
            // 
            // Heartbeat
            // 
            this.Heartbeat.Interval = 295000;
            this.Heartbeat.Tick += new System.EventHandler(this.Heartbeat_Tick);
            // 
            // ckbSpotifyMute
            // 
            this.ckbSpotifyMute.Dock = System.Windows.Forms.DockStyle.Top;
            this.ckbSpotifyMute.Location = new System.Drawing.Point(50, 10);
            this.ckbSpotifyMute.Name = "ckbSpotifyMute";
            this.ckbSpotifyMute.Size = new System.Drawing.Size(199, 30);
            this.ckbSpotifyMute.TabIndex = 6;
            this.ckbSpotifyMute.Text = "Mute only Spotify";
            this.ckbSpotifyMute.CheckedChanged += new System.EventHandler(this.SpotifyMuteCheckBox_CheckedChanged);
            // 
            // btnVolumeMixer
            // 
            this.btnVolumeMixer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnVolumeMixer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnVolumeMixer.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnVolumeMixer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolumeMixer.Location = new System.Drawing.Point(50, 73);
            this.btnVolumeMixer.Name = "btnVolumeMixer";
            this.btnVolumeMixer.Size = new System.Drawing.Size(199, 41);
            this.btnVolumeMixer.TabIndex = 7;
            this.btnVolumeMixer.Text = "Open volume mixer";
            this.btnVolumeMixer.UseVisualStyleBackColor = false;
            this.btnVolumeMixer.Click += new System.EventHandler(this.VolumeMixerButton_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(6, 5);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(187, 25);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Loading...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckbBlockBanners
            // 
            this.ckbBlockBanners.Dock = System.Windows.Forms.DockStyle.Top;
            this.ckbBlockBanners.Location = new System.Drawing.Point(50, 40);
            this.ckbBlockBanners.Name = "ckbBlockBanners";
            this.ckbBlockBanners.Size = new System.Drawing.Size(199, 30);
            this.ckbBlockBanners.TabIndex = 10;
            this.ckbBlockBanners.Text = "Block banner Ads";
            this.ckbBlockBanners.CheckedChanged += new System.EventHandler(this.SkipAdsCheckbox_CheckedChanged);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.btnVolumeMixer);
            this.pnlContent.Controls.Add(this.btnMute);
            this.pnlContent.Controls.Add(this.ckbBlockBanners);
            this.pnlContent.Controls.Add(this.ckbSpotifyMute);
            this.pnlContent.Controls.Add(this.lblWebSiteLink);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 35);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(50, 10, 50, 10);
            this.pnlContent.Size = new System.Drawing.Size(299, 195);
            this.pnlContent.TabIndex = 11;
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.Gray;
            this.pnlHead.Controls.Add(this.lblStatus);
            this.pnlHead.Controls.Add(this.btnMinimize);
            this.pnlHead.Controls.Add(this.btnExit);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pnlHead.Size = new System.Drawing.Size(299, 35);
            this.pnlHead.TabIndex = 11;
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Gray;
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(193, 5);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(50, 25);
            this.btnMinimize.TabIndex = 10;
            this.btnMinimize.Text = "-";
            this.btnMinimize.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Gray;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(243, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 25);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "x";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(299, 230);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHead);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EZBlocker";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.pnlContent.ResumeLayout(false);
            this.pnlHead.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMute;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.LinkLabel lblWebSiteLink;
        private System.Windows.Forms.Timer Heartbeat;
        private System.Windows.Forms.CheckBox ckbSpotifyMute;
        private System.Windows.Forms.Button btnVolumeMixer;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox ckbBlockBanners;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMinimize;
    }
}

