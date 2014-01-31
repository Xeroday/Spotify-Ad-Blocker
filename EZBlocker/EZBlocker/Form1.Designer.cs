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
            this.whoAmI = new System.Windows.Forms.Button();
            this.TopMostCheckbox = new System.Windows.Forms.CheckBox();
            this.artistInfoLabel = new System.Windows.Forms.Label();
            this.artistInfoPicture = new System.Windows.Forms.PictureBox();
            this.blocklistBox = new System.Windows.Forms.ListView();
            this.notifyIconMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.artistInfoPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // BlockButton
            // 
            this.BlockButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BlockButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BlockButton.Location = new System.Drawing.Point(12, 117);
            this.BlockButton.Name = "BlockButton";
            this.BlockButton.Size = new System.Drawing.Size(125, 25);
            this.BlockButton.TabIndex = 0;
            this.BlockButton.Text = "&Block this song";
            this.BlockButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BlockButton.UseVisualStyleBackColor = true;
            this.BlockButton.Click += new System.EventHandler(this.BlockButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EditButton.Location = new System.Drawing.Point(12, 179);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(125, 25);
            this.EditButton.TabIndex = 1;
            this.EditButton.Text = "&Edit Blocklist";
            this.EditButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // AutoAddCheckbox
            // 
            this.AutoAddCheckbox.BackColor = System.Drawing.Color.Transparent;
            this.AutoAddCheckbox.Checked = true;
            this.AutoAddCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoAddCheckbox.Location = new System.Drawing.Point(12, 210);
            this.AutoAddCheckbox.Name = "AutoAddCheckbox";
            this.AutoAddCheckbox.Size = new System.Drawing.Size(125, 20);
            this.AutoAddCheckbox.TabIndex = 2;
            this.AutoAddCheckbox.Text = "Help me decide";
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
            this.MuteButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MuteButton.Location = new System.Drawing.Point(12, 148);
            this.MuteButton.Name = "MuteButton";
            this.MuteButton.Size = new System.Drawing.Size(125, 25);
            this.MuteButton.TabIndex = 3;
            this.MuteButton.Text = "&Mute Spotify";
            this.MuteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.BlockThisSongToolStripMenuItem.CheckOnClick = true;
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
            this.CloseToolStripMenuItem.CheckedChanged += new System.EventHandler(this.CloseToolStripMenuItem_CheckedChanged);
            // 
            // MinimizeToolStripMenuItem
            // 
            this.MinimizeToolStripMenuItem.CheckOnClick = true;
            this.MinimizeToolStripMenuItem.Name = "MinimizeToolStripMenuItem";
            this.MinimizeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.MinimizeToolStripMenuItem.Text = "Minimize to tray";
            this.MinimizeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.MinimizeToolStripMenuItem_CheckedChanged);
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
            this.NotifyCheckbox.Location = new System.Drawing.Point(202, 377);
            this.NotifyCheckbox.Name = "NotifyCheckbox";
            this.NotifyCheckbox.Size = new System.Drawing.Size(187, 21);
            this.NotifyCheckbox.TabIndex = 4;
            this.NotifyCheckbox.Text = "Enable taskbar notifications";
            this.NotifyCheckbox.UseVisualStyleBackColor = false;
            this.NotifyCheckbox.CheckedChanged += new System.EventHandler(this.NotifyCheckbox_CheckedChanged);
            // 
            // WebsiteLink
            // 
            this.WebsiteLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WebsiteLink.AutoSize = true;
            this.WebsiteLink.BackColor = System.Drawing.Color.Transparent;
            this.WebsiteLink.Location = new System.Drawing.Point(404, 379);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(99, 17);
            this.WebsiteLink.TabIndex = 5;
            this.WebsiteLink.TabStop = true;
            this.WebsiteLink.Text = "About/Info/FAQ";
            this.WebsiteLink.Visible = false;
            this.WebsiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLink_LinkClicked);
            // 
            // ButtonRemoveEntry
            // 
            this.ButtonRemoveEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonRemoveEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonRemoveEntry.Location = new System.Drawing.Point(465, 353);
            this.ButtonRemoveEntry.Name = "ButtonRemoveEntry";
            this.ButtonRemoveEntry.Size = new System.Drawing.Size(23, 23);
            this.ButtonRemoveEntry.TabIndex = 8;
            this.ButtonRemoveEntry.Text = "-";
            this.ButtonRemoveEntry.UseVisualStyleBackColor = true;
            this.ButtonRemoveEntry.Click += new System.EventHandler(this.ButtonRemoveEntry_Click);
            // 
            // whoAmI
            // 
            this.whoAmI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.whoAmI.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.whoAmI.Location = new System.Drawing.Point(465, 324);
            this.whoAmI.Name = "whoAmI";
            this.whoAmI.Size = new System.Drawing.Size(23, 23);
            this.whoAmI.TabIndex = 9;
            this.whoAmI.Text = "?";
            this.whoAmI.UseVisualStyleBackColor = true;
            this.whoAmI.Click += new System.EventHandler(this.whoAmI_Click);
            // 
            // TopMostCheckbox
            // 
            this.TopMostCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TopMostCheckbox.AutoSize = true;
            this.TopMostCheckbox.BackColor = System.Drawing.Color.Transparent;
            this.TopMostCheckbox.Location = new System.Drawing.Point(12, 377);
            this.TopMostCheckbox.Name = "TopMostCheckbox";
            this.TopMostCheckbox.Size = new System.Drawing.Size(180, 21);
            this.TopMostCheckbox.TabIndex = 10;
            this.TopMostCheckbox.Text = "Stay above other windows";
            this.TopMostCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TopMostCheckbox.UseVisualStyleBackColor = false;
            this.TopMostCheckbox.CheckedChanged += new System.EventHandler(this.TopMostCheckbox_CheckedChanged);
            // 
            // artistInfoLabel
            // 
            this.artistInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.artistInfoLabel.AutoEllipsis = true;
            this.artistInfoLabel.BackColor = System.Drawing.Color.Transparent;
            this.artistInfoLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistInfoLabel.Location = new System.Drawing.Point(143, 12);
            this.artistInfoLabel.Name = "artistInfoLabel";
            this.artistInfoLabel.Size = new System.Drawing.Size(345, 213);
            this.artistInfoLabel.TabIndex = 11;
            this.artistInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // artistInfoPicture
            // 
            this.artistInfoPicture.BackColor = System.Drawing.Color.Transparent;
            this.artistInfoPicture.Location = new System.Drawing.Point(12, 12);
            this.artistInfoPicture.Name = "artistInfoPicture";
            this.artistInfoPicture.Size = new System.Drawing.Size(125, 99);
            this.artistInfoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.artistInfoPicture.TabIndex = 12;
            this.artistInfoPicture.TabStop = false;
            // 
            // blocklistBox
            // 
            this.blocklistBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blocklistBox.BackgroundImage = global::EZBlocker.Properties.Resources.blockedImage;
            this.blocklistBox.Location = new System.Drawing.Point(12, 245);
            this.blocklistBox.Name = "blocklistBox";
            this.blocklistBox.Size = new System.Drawing.Size(447, 131);
            this.blocklistBox.TabIndex = 13;
            this.blocklistBox.UseCompatibleStateImageBehavior = false;
            this.blocklistBox.View = System.Windows.Forms.View.List;
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::EZBlocker.Properties.Resources.background1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(500, 401);
            this.Controls.Add(this.blocklistBox);
            this.Controls.Add(this.artistInfoPicture);
            this.Controls.Add(this.whoAmI);
            this.Controls.Add(this.ButtonRemoveEntry);
            this.Controls.Add(this.BlockButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.TopMostCheckbox);
            this.Controls.Add(this.MuteButton);
            this.Controls.Add(this.WebsiteLink);
            this.Controls.Add(this.NotifyCheckbox);
            this.Controls.Add(this.artistInfoLabel);
            this.Controls.Add(this.AutoAddCheckbox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Opacity = 0.95D;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.notifyIconMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.artistInfoPicture)).EndInit();
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
        private System.Windows.Forms.ContextMenuStrip notifyIconMenu;
        private System.Windows.Forms.ToolStripMenuItem BlockThisSongToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem BehaviorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem MinimizeToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.CheckBox TopMostCheckbox;
        internal System.Windows.Forms.Button whoAmI;
        private System.Windows.Forms.Label artistInfoLabel;
        private System.Windows.Forms.PictureBox artistInfoPicture;
        private System.Windows.Forms.ListView blocklistBox;
    }
}

