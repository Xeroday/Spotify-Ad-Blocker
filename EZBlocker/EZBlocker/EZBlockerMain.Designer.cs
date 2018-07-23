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
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.websiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WebsiteLink = new System.Windows.Forms.LinkLabel();
            this.Heartbeat = new System.Windows.Forms.Timer(this.components);
            this.VolumeMixerButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.BlockBannersCheckbox = new System.Windows.Forms.CheckBox();
            this.StartupCheckbox = new System.Windows.Forms.CheckBox();
            this.SpotifyCheckbox = new System.Windows.Forms.CheckBox();
            this.NotifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTimer
            // 
            this.MainTimer.Interval = 600;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.NotifyIconContextMenu;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "EZBlocker";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.BalloonTipClicked += new System.EventHandler(this.NotifyIcon_BalloonTipClicked);
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // NotifyIconContextMenu
            // 
            this.NotifyIconContextMenu.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.NotifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.websiteToolStripMenuItem,
            this.separatorToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.NotifyIconContextMenu.Name = "NotifyIconContextMenu";
            this.NotifyIconContextMenu.Size = new System.Drawing.Size(132, 82);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(131, 24);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // websiteToolStripMenuItem
            // 
            this.websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
            this.websiteToolStripMenuItem.Size = new System.Drawing.Size(131, 24);
            this.websiteToolStripMenuItem.Text = "Website";
            this.websiteToolStripMenuItem.Click += new System.EventHandler(this.websiteToolStripMenuItem_Click);
            // 
            // separatorToolStripMenuItem
            // 
            this.separatorToolStripMenuItem.Name = "separatorToolStripMenuItem";
            this.separatorToolStripMenuItem.Size = new System.Drawing.Size(128, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(131, 24);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // WebsiteLink
            // 
            this.WebsiteLink.AutoSize = true;
            this.WebsiteLink.Location = new System.Drawing.Point(185, 133);
            this.WebsiteLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(107, 17);
            this.WebsiteLink.TabIndex = 5;
            this.WebsiteLink.TabStop = true;
            this.WebsiteLink.Text = "Report Problem";
            this.WebsiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLink_LinkClicked);
            // 
            // Heartbeat
            // 
            this.Heartbeat.Enabled = true;
            this.Heartbeat.Interval = 60000;
            this.Heartbeat.Tick += new System.EventHandler(this.Heartbeat_Tick);
            // 
            // VolumeMixerButton
            // 
            this.VolumeMixerButton.Location = new System.Drawing.Point(16, 7);
            this.VolumeMixerButton.Margin = new System.Windows.Forms.Padding(4);
            this.VolumeMixerButton.Name = "VolumeMixerButton";
            this.VolumeMixerButton.Size = new System.Drawing.Size(267, 44);
            this.VolumeMixerButton.TabIndex = 7;
            this.VolumeMixerButton.Text = "Open Volume Control";
            this.VolumeMixerButton.UseVisualStyleBackColor = true;
            this.VolumeMixerButton.Click += new System.EventHandler(this.VolumeMixerButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(13, 133);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(71, 17);
            this.StatusLabel.TabIndex = 9;
            this.StatusLabel.Text = "Loading...";
            // 
            // BlockBannersCheckbox
            // 
            this.BlockBannersCheckbox.AutoSize = true;
            this.BlockBannersCheckbox.Location = new System.Drawing.Point(16, 59);
            this.BlockBannersCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.BlockBannersCheckbox.Name = "BlockBannersCheckbox";
            this.BlockBannersCheckbox.Size = new System.Drawing.Size(139, 21);
            this.BlockBannersCheckbox.TabIndex = 10;
            this.BlockBannersCheckbox.Text = "Block Banner Ads";
            this.BlockBannersCheckbox.UseVisualStyleBackColor = true;
            this.BlockBannersCheckbox.Click += new System.EventHandler(this.SkipAdsCheckbox_Click);
            // 
            // StartupCheckbox
            // 
            this.StartupCheckbox.AutoSize = true;
            this.StartupCheckbox.Location = new System.Drawing.Point(16, 83);
            this.StartupCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.StartupCheckbox.Name = "StartupCheckbox";
            this.StartupCheckbox.Size = new System.Drawing.Size(185, 21);
            this.StartupCheckbox.TabIndex = 11;
            this.StartupCheckbox.Text = "Start EZBlocker on Login";
            this.StartupCheckbox.UseVisualStyleBackColor = true;
            this.StartupCheckbox.CheckedChanged += new System.EventHandler(this.StartupCheckbox_CheckedChanged);
            // 
            // SpotifyCheckbox
            // 
            this.SpotifyCheckbox.AutoSize = true;
            this.SpotifyCheckbox.Location = new System.Drawing.Point(16, 108);
            this.SpotifyCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.SpotifyCheckbox.Name = "SpotifyCheckbox";
            this.SpotifyCheckbox.Size = new System.Drawing.Size(201, 21);
            this.SpotifyCheckbox.TabIndex = 12;
            this.SpotifyCheckbox.Text = "Start Spotify with EZBlocker";
            this.SpotifyCheckbox.UseVisualStyleBackColor = true;
            this.SpotifyCheckbox.CheckedChanged += new System.EventHandler(this.SpotifyCheckbox_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 160);
            this.Controls.Add(this.SpotifyCheckbox);
            this.Controls.Add(this.StartupCheckbox);
            this.Controls.Add(this.BlockBannersCheckbox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.VolumeMixerButton);
            this.Controls.Add(this.WebsiteLink);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EZBlocker";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.NotifyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.LinkLabel WebsiteLink;
        private System.Windows.Forms.Timer Heartbeat;
        private System.Windows.Forms.Button VolumeMixerButton;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.CheckBox BlockBannersCheckbox;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.CheckBox StartupCheckbox;
        private System.Windows.Forms.ContextMenuStrip NotifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator separatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem websiteToolStripMenuItem;
        private System.Windows.Forms.CheckBox SpotifyCheckbox;
    }
}

