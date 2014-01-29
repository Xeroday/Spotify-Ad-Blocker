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
            this.EditButton = new System.Windows.Forms.Button();
            this.AutoAddCheckbox = new System.Windows.Forms.CheckBox();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.MuteButton = new System.Windows.Forms.Button();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BlockThisSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.BehaviorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MinimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResumeTimer = new System.Windows.Forms.Timer(this.components);
            this.NotifyCheckbox = new System.Windows.Forms.CheckBox();
            this.WebsiteLink = new System.Windows.Forms.LinkLabel();
            this.ButtonRemoveEntry = new System.Windows.Forms.Button();
            this.blocklistBox = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.currentSongDisplay = new System.Windows.Forms.Label();
            this.TopMostCheckbox = new System.Windows.Forms.CheckBox();
            this.notifyIconMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BlockButton
            // 
            this.BlockButton.Location = new System.Drawing.Point(10, 41);
            this.BlockButton.Name = "BlockButton";
            this.BlockButton.Size = new System.Drawing.Size(212, 36);
            this.BlockButton.TabIndex = 0;
            this.BlockButton.Text = "&Block this song";
            this.BlockButton.UseVisualStyleBackColor = true;
            this.BlockButton.Click += new System.EventHandler(this.BlockButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(12, 203);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(212, 36);
            this.EditButton.TabIndex = 1;
            this.EditButton.Text = "&Edit Blocklist";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // AutoAddCheckbox
            // 
            this.AutoAddCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoAddCheckbox.AutoSize = true;
            this.AutoAddCheckbox.BackColor = System.Drawing.Color.Transparent;
            this.AutoAddCheckbox.Checked = true;
            this.AutoAddCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoAddCheckbox.Location = new System.Drawing.Point(14, 144);
            this.AutoAddCheckbox.Name = "AutoAddCheckbox";
            this.AutoAddCheckbox.Size = new System.Drawing.Size(211, 17);
            this.AutoAddCheckbox.TabIndex = 2;
            this.AutoAddCheckbox.Text = "AutoAdd Ads to Blocklist (Experimental)";
            this.AutoAddCheckbox.UseVisualStyleBackColor = false;
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
            this.MuteButton.Location = new System.Drawing.Point(12, 90);
            this.MuteButton.Name = "MuteButton";
            this.MuteButton.Size = new System.Drawing.Size(212, 36);
            this.MuteButton.TabIndex = 3;
            this.MuteButton.Text = "&Mute Spotify";
            this.MuteButton.UseVisualStyleBackColor = true;
            this.MuteButton.Click += new System.EventHandler(this.MuteButton_Click);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.notifyIconMenu;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.BalloonTipClicked += new System.EventHandler(this.NotifyIcon_BalloonTipClicked);
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // notifyIconMenu
            // 
            this.notifyIconMenu.ImageScalingSize = new System.Drawing.Size(21, 21);
            this.notifyIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BlockThisSongToolStripMenuItem,
            this.ToolStripSeparator1,
            this.BehaviorToolStripMenuItem,
            this.ToolStripSeparator2,
            this.ExitToolStripMenuItem});
            this.notifyIconMenu.Name = "notifyIconMenu";
            this.notifyIconMenu.Size = new System.Drawing.Size(160, 100);
            // 
            // BlockThisSongToolStripMenuItem
            // 
            this.BlockThisSongToolStripMenuItem.Image = global::EZBlocker.Properties.Resources.blockedImage;
            this.BlockThisSongToolStripMenuItem.Name = "BlockThisSongToolStripMenuItem";
            this.BlockThisSongToolStripMenuItem.Size = new System.Drawing.Size(159, 28);
            this.BlockThisSongToolStripMenuItem.Text = "&Block this song";
            this.BlockThisSongToolStripMenuItem.Click += new System.EventHandler(this.BlockThisSongToolStripMenuItem_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // BehaviorToolStripMenuItem
            // 
            this.BehaviorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseToolStripMenuItem,
            this.MinimizeToolStripMenuItem});
            this.BehaviorToolStripMenuItem.Name = "BehaviorToolStripMenuItem";
            this.BehaviorToolStripMenuItem.Size = new System.Drawing.Size(159, 28);
            this.BehaviorToolStripMenuItem.Text = "Behavior";
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.CheckOnClick = true;
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.CloseToolStripMenuItem.Text = "Close to tray";
            // 
            // MinimizeToolStripMenuItem
            // 
            this.MinimizeToolStripMenuItem.CheckOnClick = true;
            this.MinimizeToolStripMenuItem.Name = "MinimizeToolStripMenuItem";
            this.MinimizeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.MinimizeToolStripMenuItem.Text = "Minimize to tray";
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(156, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(159, 28);
            this.ExitToolStripMenuItem.Text = "E&xit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // ResumeTimer
            // 
            this.ResumeTimer.Interval = 1000;
            this.ResumeTimer.Tick += new System.EventHandler(this.ResumeTimer_Tick);
            // 
            // NotifyCheckbox
            // 
            this.NotifyCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NotifyCheckbox.AutoSize = true;
            this.NotifyCheckbox.BackColor = System.Drawing.Color.Transparent;
            this.NotifyCheckbox.Checked = true;
            this.NotifyCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NotifyCheckbox.Location = new System.Drawing.Point(11, 295);
            this.NotifyCheckbox.Name = "NotifyCheckbox";
            this.NotifyCheckbox.Size = new System.Drawing.Size(162, 17);
            this.NotifyCheckbox.TabIndex = 4;
            this.NotifyCheckbox.Text = "Enable Taskbar Notifications";
            this.NotifyCheckbox.UseVisualStyleBackColor = false;
            this.NotifyCheckbox.CheckedChanged += new System.EventHandler(this.NotifyCheckbox_CheckedChanged);
            // 
            // WebsiteLink
            // 
            this.WebsiteLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WebsiteLink.AutoSize = true;
            this.WebsiteLink.BackColor = System.Drawing.Color.Transparent;
            this.WebsiteLink.Location = new System.Drawing.Point(385, 299);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(84, 13);
            this.WebsiteLink.TabIndex = 5;
            this.WebsiteLink.TabStop = true;
            this.WebsiteLink.Text = "About/Info/FAQ";
            this.WebsiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLink_LinkClicked);
            // 
            // ButtonRemoveEntry
            // 
            this.ButtonRemoveEntry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonRemoveEntry.Location = new System.Drawing.Point(15, 203);
            this.ButtonRemoveEntry.Name = "ButtonRemoveEntry";
            this.ButtonRemoveEntry.Size = new System.Drawing.Size(212, 36);
            this.ButtonRemoveEntry.TabIndex = 8;
            this.ButtonRemoveEntry.Text = "&Remove";
            this.ButtonRemoveEntry.UseVisualStyleBackColor = true;
            this.ButtonRemoveEntry.Click += new System.EventHandler(this.ButtonRemoveEntry_Click);
            // 
            // blocklistBox
            // 
            this.blocklistBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blocklistBox.BackColor = System.Drawing.Color.White;
            this.blocklistBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.blocklistBox.FormattingEnabled = true;
            this.blocklistBox.Location = new System.Drawing.Point(15, 9);
            this.blocklistBox.Name = "blocklistBox";
            this.blocklistBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.blocklistBox.Size = new System.Drawing.Size(212, 184);
            this.blocklistBox.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(1, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.currentSongDisplay);
            this.splitContainer1.Panel1.Controls.Add(this.BlockButton);
            this.splitContainer1.Panel1.Controls.Add(this.EditButton);
            this.splitContainer1.Panel1.Controls.Add(this.MuteButton);
            this.splitContainer1.Panel1.Controls.Add(this.AutoAddCheckbox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.blocklistBox);
            this.splitContainer1.Panel2.Controls.Add(this.ButtonRemoveEntry);
            this.splitContainer1.Size = new System.Drawing.Size(661, 253);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 9;
            // 
            // currentSongDisplay
            // 
            this.currentSongDisplay.AutoSize = true;
            this.currentSongDisplay.Location = new System.Drawing.Point(11, 15);
            this.currentSongDisplay.Name = "currentSongDisplay";
            this.currentSongDisplay.Size = new System.Drawing.Size(90, 13);
            this.currentSongDisplay.TabIndex = 4;
            this.currentSongDisplay.Text = "Currently playing: ";
            // 
            // TopMostCheckbox
            // 
            this.TopMostCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TopMostCheckbox.AutoSize = true;
            this.TopMostCheckbox.BackColor = System.Drawing.Color.Transparent;
            this.TopMostCheckbox.Location = new System.Drawing.Point(11, 272);
            this.TopMostCheckbox.Name = "TopMostCheckbox";
            this.TopMostCheckbox.Size = new System.Drawing.Size(151, 17);
            this.TopMostCheckbox.TabIndex = 10;
            this.TopMostCheckbox.Text = "Stay above other windows";
            this.TopMostCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TopMostCheckbox.UseVisualStyleBackColor = false;
            this.TopMostCheckbox.CheckedChanged += new System.EventHandler(this.TopMostCheckbox_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EZBlocker.Properties.Resources.background1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(481, 321);
            this.Controls.Add(this.TopMostCheckbox);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.WebsiteLink);
            this.Controls.Add(this.NotifyCheckbox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.RightToLeftLayout = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.notifyIconMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BlockButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.CheckBox AutoAddCheckbox;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button MuteButton;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.Timer ResumeTimer;
        private System.Windows.Forms.CheckBox NotifyCheckbox;
        private System.Windows.Forms.LinkLabel WebsiteLink;
        internal System.Windows.Forms.Button ButtonRemoveEntry;
        internal System.Windows.Forms.ListBox blocklistBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label currentSongDisplay;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenu;
        private System.Windows.Forms.ToolStripMenuItem BlockThisSongToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem BehaviorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem MinimizeToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.CheckBox TopMostCheckbox;
    }
}

