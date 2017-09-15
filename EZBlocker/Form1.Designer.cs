namespace EZBlocker
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer containerMain = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (containerMain != null))
            {
                containerMain.Dispose();
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
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timerHeartbeat = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnMixer = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnIssue = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.labelSongName = new System.Windows.Forms.Label();
            this.btnMute = new System.Windows.Forms.Button();
            this.appLabel = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.picBoxMain = new System.Windows.Forms.PictureBox();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.linkLabelModder = new System.Windows.Forms.LinkLabel();
            this.panelDown = new System.Windows.Forms.Panel();
            this.panelUp = new System.Windows.Forms.Panel();
            this.picBoxSongName = new System.Windows.Forms.PictureBox();
            this.checkBoxStartup = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockAds = new System.Windows.Forms.CheckBox();
            this.linkLabelDesigner = new System.Windows.Forms.LinkLabel();
            this.linkLabelAuthor = new System.Windows.Forms.LinkLabel();
            this.checkBoxMute = new System.Windows.Forms.CheckBox();
            this.picBoxSpotifyLogo = new System.Windows.Forms.PictureBox();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.contextMenuStrip.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).BeginInit();
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSongName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSpotifyLogo)).BeginInit();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerMain
            // 
            this.timerMain.Interval = 600;
            this.timerMain.Tick += new System.EventHandler(this.TimerMain_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "EZBlocker";
            this.notifyIcon.Visible = true;
            this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.NotifyIcon_BalloonTipClicked);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.contextMenuStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpen,
            this.toolStripMenuSeparator,
            this.toolStripMenuItemExit});
            this.contextMenuStrip.Name = "NotifyIconContextMenu";
            this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip.ShowItemToolTips = false;
            this.contextMenuStrip.Size = new System.Drawing.Size(112, 70);
            // 
            // toolStripMenuItemOpen
            // 
            this.toolStripMenuItemOpen.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.toolStripMenuItemOpen.Size = new System.Drawing.Size(111, 30);
            this.toolStripMenuItemOpen.Text = "Open";
            this.toolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // toolStripMenuSeparator
            // 
            this.toolStripMenuSeparator.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuSeparator.Name = "toolStripMenuSeparator";
            this.toolStripMenuSeparator.Padding = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.toolStripMenuSeparator.Size = new System.Drawing.Size(108, 6);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(111, 30);
            this.toolStripMenuItemExit.Text = "&Exit";
            this.toolStripMenuItemExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripMenuItemExit.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
            // 
            // timerHeartbeat
            // 
            this.timerHeartbeat.Enabled = true;
            this.timerHeartbeat.Interval = 295000;
            this.timerHeartbeat.Tick += new System.EventHandler(this.TimerHeartbeat_Tick);
            // 
            // btnMixer
            // 
            this.btnMixer.BackgroundImage = global::EZBlocker.Properties.Resources.Options_1;
            this.btnMixer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMixer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnMixer.FlatAppearance.BorderSize = 0;
            this.btnMixer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnMixer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnMixer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMixer.Location = new System.Drawing.Point(496, 12);
            this.btnMixer.Name = "btnMixer";
            this.btnMixer.Size = new System.Drawing.Size(45, 50);
            this.btnMixer.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnMixer, "Open mixer");
            this.btnMixer.UseVisualStyleBackColor = true;
            this.btnMixer.Click += new System.EventHandler(this.BtnMixer_Click);
            this.btnMixer.MouseLeave += new System.EventHandler(this.BtnMixer_MouseLeave);
            this.btnMixer.MouseHover += new System.EventHandler(this.BtnMixer_MouseHover);
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackgroundImage = global::EZBlocker.Properties.Resources.Minimize;
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(446, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(50, 30);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.TabStop = false;
            this.toolTip.SetToolTip(this.btnMinimize, "Minimize EZBlocker");
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.BtnMinimize_Click);
            // 
            // btnIssue
            // 
            this.btnIssue.BackgroundImage = global::EZBlocker.Properties.Resources.Issue;
            this.btnIssue.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnIssue.FlatAppearance.BorderSize = 0;
            this.btnIssue.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnIssue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnIssue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIssue.Location = new System.Drawing.Point(496, 0);
            this.btnIssue.Name = "btnIssue";
            this.btnIssue.Size = new System.Drawing.Size(50, 30);
            this.btnIssue.TabIndex = 0;
            this.btnIssue.TabStop = false;
            this.toolTip.SetToolTip(this.btnIssue, "Report issue");
            this.btnIssue.UseVisualStyleBackColor = true;
            this.btnIssue.Click += new System.EventHandler(this.BtnIssue_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::EZBlocker.Properties.Resources.Close;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(546, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.toolTip.SetToolTip(this.btnClose, "Close EZBlocker");
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // labelSongName
            // 
            this.labelSongName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.labelSongName.Location = new System.Drawing.Point(104, 12);
            this.labelSongName.Name = "labelSongName";
            this.labelSongName.Size = new System.Drawing.Size(386, 50);
            this.labelSongName.TabIndex = 9;
            this.labelSongName.Text = "Loading...";
            this.labelSongName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnMute
            // 
            this.btnMute.FlatAppearance.BorderSize = 0;
            this.btnMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMute.Location = new System.Drawing.Point(426, 68);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(120, 97);
            this.btnMute.TabIndex = 4;
            this.btnMute.Text = "Mute/Unmute Spotify";
            this.btnMute.UseVisualStyleBackColor = true;
            this.btnMute.Visible = false;
            this.btnMute.Click += new System.EventHandler(this.BtnMute_Click);
            // 
            // appLabel
            // 
            this.appLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.appLabel.ForeColor = System.Drawing.Color.White;
            this.appLabel.Location = new System.Drawing.Point(0, 0);
            this.appLabel.Name = "appLabel";
            this.appLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.appLabel.Size = new System.Drawing.Size(446, 30);
            this.appLabel.TabIndex = 0;
            this.appLabel.Text = "EZBlocker";
            this.appLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.appLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.appLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.appLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.pnlMain.BackgroundImage = global::EZBlocker.Properties.Resources.EZBlocker_1;
            this.pnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlMain.Controls.Add(this.picBoxMain);
            this.pnlMain.Controls.Add(this.pnlContainer);
            this.pnlMain.Controls.Add(this.panelTitle);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(2, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(596, 396);
            this.pnlMain.TabIndex = 13;
            // 
            // picBoxMain
            // 
            this.picBoxMain.BackColor = System.Drawing.Color.Transparent;
            this.picBoxMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxMain.Image = global::EZBlocker.Properties.Resources.EZBlocker_Logo;
            this.picBoxMain.Location = new System.Drawing.Point(0, 30);
            this.picBoxMain.Name = "picBoxMain";
            this.picBoxMain.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            this.picBoxMain.Size = new System.Drawing.Size(596, 148);
            this.picBoxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxMain.TabIndex = 15;
            this.picBoxMain.TabStop = false;
            this.picBoxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.picBoxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.picBoxMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.linkLabelModder);
            this.pnlContainer.Controls.Add(this.panelDown);
            this.pnlContainer.Controls.Add(this.panelUp);
            this.pnlContainer.Controls.Add(this.btnMute);
            this.pnlContainer.Controls.Add(this.picBoxSongName);
            this.pnlContainer.Controls.Add(this.btnMixer);
            this.pnlContainer.Controls.Add(this.checkBoxStartup);
            this.pnlContainer.Controls.Add(this.checkBoxBlockAds);
            this.pnlContainer.Controls.Add(this.linkLabelDesigner);
            this.pnlContainer.Controls.Add(this.linkLabelAuthor);
            this.pnlContainer.Controls.Add(this.checkBoxMute);
            this.pnlContainer.Controls.Add(this.labelSongName);
            this.pnlContainer.Controls.Add(this.picBoxSpotifyLogo);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlContainer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.pnlContainer.ForeColor = System.Drawing.Color.White;
            this.pnlContainer.Location = new System.Drawing.Point(0, 178);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pnlContainer.Size = new System.Drawing.Size(596, 218);
            this.pnlContainer.TabIndex = 14;
            // 
            // linkLabelModder
            // 
            this.linkLabelModder.ActiveLinkColor = System.Drawing.Color.DimGray;
            this.linkLabelModder.AutoSize = true;
            this.linkLabelModder.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelModder.LinkColor = System.Drawing.Color.Silver;
            this.linkLabelModder.Location = new System.Drawing.Point(17, 181);
            this.linkLabelModder.Name = "linkLabelModder";
            this.linkLabelModder.Size = new System.Drawing.Size(156, 17);
            this.linkLabelModder.TabIndex = 5;
            this.linkLabelModder.TabStop = true;
            this.linkLabelModder.Text = "Modded by: MatrixDJ96";
            this.linkLabelModder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelModder_Click);
            // 
            // panelDown
            // 
            this.panelDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelDown.Location = new System.Drawing.Point(20, 163);
            this.panelDown.Name = "panelDown";
            this.panelDown.Size = new System.Drawing.Size(556, 2);
            this.panelDown.TabIndex = 19;
            // 
            // panelUp
            // 
            this.panelUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelUp.Location = new System.Drawing.Point(53, 68);
            this.panelUp.Name = "panelUp";
            this.panelUp.Size = new System.Drawing.Size(488, 2);
            this.panelUp.TabIndex = 17;
            // 
            // picBoxSongName
            // 
            this.picBoxSongName.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSongName.BackgroundImage = global::EZBlocker.Properties.Resources.Sound;
            this.picBoxSongName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picBoxSongName.Location = new System.Drawing.Point(53, 12);
            this.picBoxSongName.Name = "picBoxSongName";
            this.picBoxSongName.Size = new System.Drawing.Size(45, 50);
            this.picBoxSongName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSongName.TabIndex = 16;
            this.picBoxSongName.TabStop = false;
            // 
            // checkBoxStartup
            // 
            this.checkBoxStartup.AutoSize = true;
            this.checkBoxStartup.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.checkBoxStartup.FlatAppearance.BorderSize = 0;
            this.checkBoxStartup.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.checkBoxStartup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxStartup.Location = new System.Drawing.Point(217, 103);
            this.checkBoxStartup.Name = "checkBoxStartup";
            this.checkBoxStartup.Size = new System.Drawing.Size(177, 21);
            this.checkBoxStartup.TabIndex = 3;
            this.checkBoxStartup.Text = "Start EZBlocker on login";
            this.checkBoxStartup.UseVisualStyleBackColor = true;
            this.checkBoxStartup.CheckedChanged += new System.EventHandler(this.CheckBoxStartup_CheckedChanged);
            // 
            // checkBoxBlockAds
            // 
            this.checkBoxBlockAds.AutoSize = true;
            this.checkBoxBlockAds.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.checkBoxBlockAds.FlatAppearance.BorderSize = 0;
            this.checkBoxBlockAds.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.checkBoxBlockAds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxBlockAds.Location = new System.Drawing.Point(217, 130);
            this.checkBoxBlockAds.Name = "checkBoxBlockAds";
            this.checkBoxBlockAds.Size = new System.Drawing.Size(116, 21);
            this.checkBoxBlockAds.TabIndex = 2;
            this.checkBoxBlockAds.Text = "Disable all ads";
            this.checkBoxBlockAds.UseVisualStyleBackColor = true;
            this.checkBoxBlockAds.Click += new System.EventHandler(this.CheckBoxBlockAds_Click);
            // 
            // linkLabelDesigner
            // 
            this.linkLabelDesigner.ActiveLinkColor = System.Drawing.Color.DimGray;
            this.linkLabelDesigner.AutoSize = true;
            this.linkLabelDesigner.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelDesigner.LinkColor = System.Drawing.Color.Silver;
            this.linkLabelDesigner.Location = new System.Drawing.Point(405, 181);
            this.linkLabelDesigner.Name = "linkLabelDesigner";
            this.linkLabelDesigner.Size = new System.Drawing.Size(171, 17);
            this.linkLabelDesigner.TabIndex = 6;
            this.linkLabelDesigner.TabStop = true;
            this.linkLabelDesigner.Text = "Design by: Bruske Design";
            this.linkLabelDesigner.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.linkLabelDesigner.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelDesigner_Click);
            // 
            // linkLabelAuthor
            // 
            this.linkLabelAuthor.ActiveLinkColor = System.Drawing.Color.DimGray;
            this.linkLabelAuthor.AutoSize = true;
            this.linkLabelAuthor.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelAuthor.LinkColor = System.Drawing.Color.Silver;
            this.linkLabelAuthor.Location = new System.Drawing.Point(214, 181);
            this.linkLabelAuthor.Name = "linkLabelAuthor";
            this.linkLabelAuthor.Size = new System.Drawing.Size(131, 17);
            this.linkLabelAuthor.TabIndex = 6;
            this.linkLabelAuthor.TabStop = true;
            this.linkLabelAuthor.Text = "Made by: Eric Zhan";
            this.linkLabelAuthor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.linkLabelAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelAuthor_Click);
            // 
            // checkBoxMute
            // 
            this.checkBoxMute.AutoSize = true;
            this.checkBoxMute.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.checkBoxMute.FlatAppearance.BorderSize = 0;
            this.checkBoxMute.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.checkBoxMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxMute.Location = new System.Drawing.Point(217, 76);
            this.checkBoxMute.Name = "checkBoxMute";
            this.checkBoxMute.Size = new System.Drawing.Size(132, 21);
            this.checkBoxMute.TabIndex = 1;
            this.checkBoxMute.Text = "Mute only Spotify";
            this.checkBoxMute.UseVisualStyleBackColor = true;
            this.checkBoxMute.CheckedChanged += new System.EventHandler(this.CheckBoxMute_CheckedChanged);
            // 
            // picBoxSpotifyLogo
            // 
            this.picBoxSpotifyLogo.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSpotifyLogo.BackgroundImage = global::EZBlocker.Properties.Resources.Spotify_Logo;
            this.picBoxSpotifyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picBoxSpotifyLogo.Location = new System.Drawing.Point(53, 68);
            this.picBoxSpotifyLogo.Name = "picBoxSpotifyLogo";
            this.picBoxSpotifyLogo.Size = new System.Drawing.Size(103, 97);
            this.picBoxSpotifyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSpotifyLogo.TabIndex = 18;
            this.picBoxSpotifyLogo.TabStop = false;
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.Transparent;
            this.panelTitle.Controls.Add(this.appLabel);
            this.panelTitle.Controls.Add(this.btnMinimize);
            this.panelTitle.Controls.Add(this.btnIssue);
            this.panelTitle.Controls.Add(this.btnClose);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(596, 30);
            this.panelTitle.TabIndex = 12;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.pnlMain);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EZBlocker";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).EndInit();
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSongName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSpotifyLogo)).EndInit();
            this.panelTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMute;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.LinkLabel linkLabelModder;
        private System.Windows.Forms.Timer timerHeartbeat;
        private System.Windows.Forms.CheckBox checkBoxMute;
        private System.Windows.Forms.Button btnMixer;
        private System.Windows.Forms.Label labelSongName;
        private System.Windows.Forms.CheckBox checkBoxBlockAds;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.CheckBox checkBoxStartup;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnIssue;
        private System.Windows.Forms.Label appLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox picBoxSongName;
        private System.Windows.Forms.PictureBox picBoxSpotifyLogo;
        private System.Windows.Forms.Panel panelUp;
        private System.Windows.Forms.Panel panelDown;
        private System.Windows.Forms.LinkLabel linkLabelAuthor;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuSeparator;
        private System.Windows.Forms.LinkLabel linkLabelDesigner;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.PictureBox picBoxMain;
        private System.ComponentModel.IContainer components;
    }
}

