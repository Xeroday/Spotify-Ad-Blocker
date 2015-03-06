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
            this.StatusNameLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
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
            this.WebsiteLink.Location = new System.Drawing.Point(140, 68);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(84, 13);
            this.WebsiteLink.TabIndex = 5;
            this.WebsiteLink.TabStop = true;
            this.WebsiteLink.Text = "About/Info/FAQ";
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
            this.SpotifyMuteCheckbox.Size = new System.Drawing.Size(178, 17);
            this.SpotifyMuteCheckbox.TabIndex = 6;
            this.SpotifyMuteCheckbox.Text = "Mute Only Spotify (Experimental)";
            this.SpotifyMuteCheckbox.UseVisualStyleBackColor = true;
            this.SpotifyMuteCheckbox.CheckedChanged += new System.EventHandler(this.SpotifyMuteCheckBox_CheckedChanged);
            // 
            // VolumeMixerButton
            // 
            this.VolumeMixerButton.Location = new System.Drawing.Point(12, 6);
            this.VolumeMixerButton.Name = "VolumeMixerButton";
            this.VolumeMixerButton.Size = new System.Drawing.Size(212, 36);
            this.VolumeMixerButton.TabIndex = 7;
            this.VolumeMixerButton.Text = "Open Volume Mixer";
            this.VolumeMixerButton.UseVisualStyleBackColor = true;
            this.VolumeMixerButton.Click += new System.EventHandler(this.VolumeMixerButton_Click);
            // 
            // StatusNameLabel
            // 
            this.StatusNameLabel.AutoSize = true;
            this.StatusNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusNameLabel.Location = new System.Drawing.Point(9, 68);
            this.StatusNameLabel.Name = "StatusNameLabel";
            this.StatusNameLabel.Size = new System.Drawing.Size(43, 13);
            this.StatusNameLabel.TabIndex = 8;
            this.StatusNameLabel.Text = "Status: ";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(48, 68);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(54, 13);
            this.StatusLabel.TabIndex = 9;
            this.StatusLabel.Text = "Loading...";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 88);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.StatusNameLabel);
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

        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button MuteButton;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.LinkLabel WebsiteLink;
        private System.Windows.Forms.Timer Heartbeat;
        private System.Windows.Forms.CheckBox SpotifyMuteCheckbox;
        private System.Windows.Forms.Button VolumeMixerButton;
        private System.Windows.Forms.Label StatusNameLabel;
        private System.Windows.Forms.Label StatusLabel;
    }
}

