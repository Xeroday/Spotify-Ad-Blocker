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
            this.BlockButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.AutoAddCheckbox = new System.Windows.Forms.CheckBox();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.MuteButton = new System.Windows.Forms.Button();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ResumeTimer = new System.Windows.Forms.Timer(this.components);
            this.NotifyCheckbox = new System.Windows.Forms.CheckBox();
            this.WebsiteLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // BlockButton
            // 
            this.BlockButton.Location = new System.Drawing.Point(12, 12);
            this.BlockButton.Name = "BlockButton";
            this.BlockButton.Size = new System.Drawing.Size(212, 36);
            this.BlockButton.TabIndex = 0;
            this.BlockButton.Text = "Add Current to Blocklist";
            this.BlockButton.UseVisualStyleBackColor = true;
            this.BlockButton.Click += new System.EventHandler(this.BlockButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(12, 54);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(212, 36);
            this.OpenButton.TabIndex = 1;
            this.OpenButton.Text = "Open Blocklist";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // AutoAddCheckbox
            // 
            this.AutoAddCheckbox.AutoSize = true;
            this.AutoAddCheckbox.Checked = true;
            this.AutoAddCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoAddCheckbox.Location = new System.Drawing.Point(12, 140);
            this.AutoAddCheckbox.Name = "AutoAddCheckbox";
            this.AutoAddCheckbox.Size = new System.Drawing.Size(211, 17);
            this.AutoAddCheckbox.TabIndex = 2;
            this.AutoAddCheckbox.Text = "AutoAdd Ads to Blocklist (Experimental)";
            this.AutoAddCheckbox.UseVisualStyleBackColor = true;
            this.AutoAddCheckbox.CheckedChanged += new System.EventHandler(this.AutoAddCheck_CheckedChanged);
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
            this.MainTimer.Interval = 666;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // MuteButton
            // 
            this.MuteButton.Location = new System.Drawing.Point(12, 96);
            this.MuteButton.Name = "MuteButton";
            this.MuteButton.Size = new System.Drawing.Size(212, 36);
            this.MuteButton.TabIndex = 3;
            this.MuteButton.Text = "Mute/UnMute Spotify";
            this.MuteButton.UseVisualStyleBackColor = true;
            this.MuteButton.Click += new System.EventHandler(this.MuteButton_Click);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.BalloonTipClicked += new System.EventHandler(this.NotifyIcon_BalloonTipClicked);
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // ResumeTimer
            // 
            this.ResumeTimer.Interval = 1000;
            this.ResumeTimer.Tick += new System.EventHandler(this.ResumeTimer_Tick);
            // 
            // NotifyCheckbox
            // 
            this.NotifyCheckbox.AutoSize = true;
            this.NotifyCheckbox.Checked = true;
            this.NotifyCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NotifyCheckbox.Location = new System.Drawing.Point(12, 163);
            this.NotifyCheckbox.Name = "NotifyCheckbox";
            this.NotifyCheckbox.Size = new System.Drawing.Size(162, 17);
            this.NotifyCheckbox.TabIndex = 4;
            this.NotifyCheckbox.Text = "Enable Taskbar Notifications";
            this.NotifyCheckbox.UseVisualStyleBackColor = true;
            this.NotifyCheckbox.CheckedChanged += new System.EventHandler(this.NotifyCheckbox_CheckedChanged);
            // 
            // WebsiteLink
            // 
            this.WebsiteLink.AutoSize = true;
            this.WebsiteLink.Location = new System.Drawing.Point(9, 185);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(84, 13);
            this.WebsiteLink.TabIndex = 5;
            this.WebsiteLink.TabStop = true;
            this.WebsiteLink.Text = "About/Info/FAQ";
            this.WebsiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLink_LinkClicked);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 207);
            this.Controls.Add(this.WebsiteLink);
            this.Controls.Add(this.NotifyCheckbox);
            this.Controls.Add(this.MuteButton);
            this.Controls.Add(this.AutoAddCheckbox);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.BlockButton);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.RightToLeftLayout = true;
            this.Text = "EZBlocker";
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BlockButton;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.CheckBox AutoAddCheckbox;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button MuteButton;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.Timer ResumeTimer;
        private System.Windows.Forms.CheckBox NotifyCheckbox;
        private System.Windows.Forms.LinkLabel WebsiteLink;
    }
}

