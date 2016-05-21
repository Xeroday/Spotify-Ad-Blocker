namespace EZBlocker
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.MuteButton = new System.Windows.Forms.Button();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.WebsiteLink = new System.Windows.Forms.LinkLabel();
            this.Heartbeat = new System.Windows.Forms.Timer(this.components);
            this.SpotifyMuteCheckbox = new System.Windows.Forms.CheckBox();
            this.VolumeMixerButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.BlockBannersCheckbox = new System.Windows.Forms.CheckBox();
            this.NextTrackButton = new System.Windows.Forms.Button();
            this.PreviousTrackButton = new System.Windows.Forms.Button();
            this.PlayPauseTrackButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainTimer
            // 
            this.MainTimer.Interval = 1000;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // MuteButton
            // 
            this.MuteButton.Location = new System.Drawing.Point(12, 162);
            this.MuteButton.Name = "MuteButton";
            this.MuteButton.Size = new System.Drawing.Size(59, 27);
            this.MuteButton.TabIndex = 3;
            this.MuteButton.Text = "Mute/UnMute Spotify";
            this.MuteButton.UseVisualStyleBackColor = true;
            this.MuteButton.Visible = false;
            this.MuteButton.Click += new System.EventHandler(this.MuteButton_Click);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.BalloonTipClicked += new System.EventHandler(this.NotifyIcon_BalloonTipClicked);
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // WebsiteLink
            // 
            this.WebsiteLink.AutoSize = true;
            this.WebsiteLink.Location = new System.Drawing.Point(144, 94);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(80, 13);
            this.WebsiteLink.TabIndex = 5;
            this.WebsiteLink.TabStop = true;
            this.WebsiteLink.Text = "Report Problem";
            this.WebsiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLink_LinkClicked);
            // 
            // Heartbeat
            // 
            this.Heartbeat.Enabled = true;
            this.Heartbeat.Interval = 295000;
            this.Heartbeat.Tick += new System.EventHandler(this.Heartbeat_Tick);
            // 
            // SpotifyMuteCheckbox
            // 
            this.SpotifyMuteCheckbox.AutoSize = true;
            this.SpotifyMuteCheckbox.Location = new System.Drawing.Point(12, 48);
            this.SpotifyMuteCheckbox.Name = "SpotifyMuteCheckbox";
            this.SpotifyMuteCheckbox.Size = new System.Drawing.Size(109, 17);
            this.SpotifyMuteCheckbox.TabIndex = 6;
            this.SpotifyMuteCheckbox.Text = "Mute Only Spotify";
            this.SpotifyMuteCheckbox.UseVisualStyleBackColor = true;
            this.SpotifyMuteCheckbox.CheckedChanged += new System.EventHandler(this.SpotifyMuteCheckBox_CheckedChanged);
            // 
            // VolumeMixerButton
            // 
            this.VolumeMixerButton.Location = new System.Drawing.Point(12, 6);
            this.VolumeMixerButton.Name = "VolumeMixerButton";
            this.VolumeMixerButton.Size = new System.Drawing.Size(206, 36);
            this.VolumeMixerButton.TabIndex = 7;
            this.VolumeMixerButton.Text = "Open Volume Mixer";
            this.VolumeMixerButton.UseVisualStyleBackColor = true;
            this.VolumeMixerButton.Click += new System.EventHandler(this.VolumeMixerButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(9, 94);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(54, 13);
            this.StatusLabel.TabIndex = 9;
            this.StatusLabel.Text = "Loading...";
            // 
            // BlockBannersCheckbox
            // 
            this.BlockBannersCheckbox.AutoSize = true;
            this.BlockBannersCheckbox.Location = new System.Drawing.Point(12, 71);
            this.BlockBannersCheckbox.Name = "BlockBannersCheckbox";
            this.BlockBannersCheckbox.Size = new System.Drawing.Size(165, 17);
            this.BlockBannersCheckbox.TabIndex = 10;
            this.BlockBannersCheckbox.Text = "Disable All Ads (Experimental)";
            this.BlockBannersCheckbox.UseVisualStyleBackColor = true;
            this.BlockBannersCheckbox.Click += new System.EventHandler(this.SkipAdsCheckbox_Click);
            // 
            // NextTrackButton
            // 
            this.NextTrackButton.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextTrackButton.Location = new System.Drawing.Point(193, 44);
            this.NextTrackButton.Name = "NextTrackButton";
            this.NextTrackButton.Size = new System.Drawing.Size(25, 23);
            this.NextTrackButton.TabIndex = 11;
            this.NextTrackButton.Text = "⏩";
            this.NextTrackButton.UseVisualStyleBackColor = true;
            this.NextTrackButton.Click += new System.EventHandler(this.NextTrackButton_Click);
            // 
            // PreviousTrackButton
            // 
            this.PreviousTrackButton.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousTrackButton.Location = new System.Drawing.Point(127, 44);
            this.PreviousTrackButton.Name = "PreviousTrackButton";
            this.PreviousTrackButton.Size = new System.Drawing.Size(25, 23);
            this.PreviousTrackButton.TabIndex = 12;
            this.PreviousTrackButton.Text = "⏪ ";
            this.PreviousTrackButton.UseVisualStyleBackColor = true;
            this.PreviousTrackButton.Click += new System.EventHandler(this.PreviousTrackButton_Click);
            // 
            // PlayPauseTrackButton
            // 
            this.PlayPauseTrackButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayPauseTrackButton.Location = new System.Drawing.Point(158, 44);
            this.PlayPauseTrackButton.Name = "PlayPauseTrackButton";
            this.PlayPauseTrackButton.Size = new System.Drawing.Size(29, 23);
            this.PlayPauseTrackButton.TabIndex = 13;
            this.PlayPauseTrackButton.Text = "❚❚";
            this.PlayPauseTrackButton.UseVisualStyleBackColor = true;
            this.PlayPauseTrackButton.Click += new System.EventHandler(this.PlayPauseTrackButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 113);
            this.Controls.Add(this.PlayPauseTrackButton);
            this.Controls.Add(this.PreviousTrackButton);
            this.Controls.Add(this.NextTrackButton);
            this.Controls.Add(this.BlockBannersCheckbox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.VolumeMixerButton);
            this.Controls.Add(this.SpotifyMuteCheckbox);
            this.Controls.Add(this.WebsiteLink);
            this.Controls.Add(this.MuteButton);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.RightToLeftLayout = true;
            this.Text = "EZBlocker";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MuteButton;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.LinkLabel WebsiteLink;
        private System.Windows.Forms.Timer Heartbeat;
        private System.Windows.Forms.CheckBox SpotifyMuteCheckbox;
        private System.Windows.Forms.Button VolumeMixerButton;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.CheckBox BlockBannersCheckbox;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button NextTrackButton;
        private System.Windows.Forms.Button PreviousTrackButton;
        private System.Windows.Forms.Button PlayPauseTrackButton;
    }
}

