<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BLUI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BLUI))
        Me.AlbumScroll = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.notifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.notifyIconMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BlockThisSongToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BehaviorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MinimizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TopMostCheckBox = New System.Windows.Forms.CheckBox()
        Me.AutoAddCheckBox = New System.Windows.Forms.CheckBox()
        Me.ButtonRemoveEntry = New System.Windows.Forms.Button()
        Me.blocklistBox = New System.Windows.Forms.ListBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ResumeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MainTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ButtonClose = New System.Windows.Forms.PictureBox()
        Me.SingerBox = New System.Windows.Forms.WebBrowser()
        Me.ButtonEdit = New EZBlocker.Flipbutton()
        Me.ButtonMute = New EZBlocker.Flipbutton()
        Me.ButtonBlock = New EZBlocker.Flipbutton()
        Me.notifyIconMenu.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.ButtonClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AlbumScroll
        '
        Me.AlbumScroll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlbumScroll.AutoSize = True
        Me.AlbumScroll.BackColor = System.Drawing.Color.Transparent
        Me.AlbumScroll.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.AlbumScroll.Location = New System.Drawing.Point(12, 71)
        Me.AlbumScroll.Name = "AlbumScroll"
        Me.AlbumScroll.Size = New System.Drawing.Size(118, 15)
        Me.AlbumScroll.TabIndex = 1
        Me.AlbumScroll.Text = "Example song - artist"
        Me.AlbumScroll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(11, 50)
        Me.ProgressBar1.Maximum = 400
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(576, 11)
        Me.ProgressBar1.Step = 1
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar1.TabIndex = 2
        '
        'notifyIcon
        '
        Me.notifyIcon.ContextMenuStrip = Me.notifyIconMenu
        Me.notifyIcon.Icon = CType(resources.GetObject("notifyIcon.Icon"), System.Drawing.Icon)
        Me.notifyIcon.Text = "EZBlocker Improved" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Right-click to show menu"
        Me.notifyIcon.Visible = True
        '
        'notifyIconMenu
        '
        Me.notifyIconMenu.ImageScalingSize = New System.Drawing.Size(21, 21)
        Me.notifyIconMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BlockThisSongToolStripMenuItem, Me.ToolStripSeparator1, Me.BehaviorToolStripMenuItem, Me.ToolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.notifyIconMenu.Name = "notifyIconMenu"
        Me.notifyIconMenu.Size = New System.Drawing.Size(160, 100)
        '
        'BlockThisSongToolStripMenuItem
        '
        Me.BlockThisSongToolStripMenuItem.Image = Global.EZBlocker.My.Resources.Resources.blockedImage
        Me.BlockThisSongToolStripMenuItem.Name = "BlockThisSongToolStripMenuItem"
        Me.BlockThisSongToolStripMenuItem.Size = New System.Drawing.Size(159, 28)
        Me.BlockThisSongToolStripMenuItem.Text = "&Block this song"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(156, 6)
        '
        'BehaviorToolStripMenuItem
        '
        Me.BehaviorToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripMenuItem, Me.MinimizeToolStripMenuItem})
        Me.BehaviorToolStripMenuItem.Name = "BehaviorToolStripMenuItem"
        Me.BehaviorToolStripMenuItem.Size = New System.Drawing.Size(159, 28)
        Me.BehaviorToolStripMenuItem.Text = "Behavior"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.CheckOnClick = True
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.CloseToolStripMenuItem.Text = "Close to tray"
        '
        'MinimizeToolStripMenuItem
        '
        Me.MinimizeToolStripMenuItem.CheckOnClick = True
        Me.MinimizeToolStripMenuItem.Name = "MinimizeToolStripMenuItem"
        Me.MinimizeToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.MinimizeToolStripMenuItem.Text = "Minimize to tray"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(156, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(159, 28)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'TopMostCheckBox
        '
        Me.TopMostCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TopMostCheckBox.AutoSize = True
        Me.TopMostCheckBox.BackColor = System.Drawing.Color.Transparent
        Me.TopMostCheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.TopMostCheckBox.Location = New System.Drawing.Point(22, 429)
        Me.TopMostCheckBox.Name = "TopMostCheckBox"
        Me.TopMostCheckBox.Size = New System.Drawing.Size(165, 19)
        Me.TopMostCheckBox.TabIndex = 6
        Me.TopMostCheckBox.Text = "Stay above other windows"
        Me.TopMostCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.TopMostCheckBox.UseVisualStyleBackColor = False
        '
        'AutoAddCheckBox
        '
        Me.AutoAddCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AutoAddCheckBox.AutoSize = True
        Me.AutoAddCheckBox.BackColor = System.Drawing.Color.Transparent
        Me.AutoAddCheckBox.Checked = True
        Me.AutoAddCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoAddCheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.AutoAddCheckBox.Location = New System.Drawing.Point(22, 404)
        Me.AutoAddCheckBox.Name = "AutoAddCheckBox"
        Me.AutoAddCheckBox.Size = New System.Drawing.Size(153, 19)
        Me.AutoAddCheckBox.TabIndex = 5
        Me.AutoAddCheckBox.Text = "Automatically block ads"
        Me.AutoAddCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.AutoAddCheckBox.UseVisualStyleBackColor = False
        '
        'ButtonRemoveEntry
        '
        Me.ButtonRemoveEntry.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButtonRemoveEntry.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.ButtonRemoveEntry.Location = New System.Drawing.Point(0, 241)
        Me.ButtonRemoveEntry.Name = "ButtonRemoveEntry"
        Me.ButtonRemoveEntry.Size = New System.Drawing.Size(197, 29)
        Me.ButtonRemoveEntry.TabIndex = 10
        Me.ButtonRemoveEntry.Text = "&Remove"
        Me.ButtonRemoveEntry.UseVisualStyleBackColor = False
        '
        'blocklistBox
        '
        Me.blocklistBox.BackColor = System.Drawing.Color.White
        Me.blocklistBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.blocklistBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.blocklistBox.FormattingEnabled = True
        Me.blocklistBox.ItemHeight = 15
        Me.blocklistBox.Location = New System.Drawing.Point(0, 0)
        Me.blocklistBox.Name = "blocklistBox"
        Me.blocklistBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.blocklistBox.Size = New System.Drawing.Size(197, 240)
        Me.blocklistBox.TabIndex = 9
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.blocklistBox)
        Me.Panel1.Controls.Add(Me.ButtonRemoveEntry)
        Me.Panel1.Location = New System.Drawing.Point(389, 125)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(199, 272)
        Me.Panel1.TabIndex = 11
        Me.Panel1.Visible = False
        '
        'ResumeTimer
        '
        Me.ResumeTimer.Interval = 1500
        '
        'MainTimer
        '
        Me.MainTimer.Enabled = True
        Me.MainTimer.Interval = 500
        '
        'ButtonClose
        '
        Me.ButtonClose.BackColor = System.Drawing.Color.Transparent
        Me.ButtonClose.Image = Global.EZBlocker.My.Resources.Resources.close
        Me.ButtonClose.Location = New System.Drawing.Point(556, 12)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(32, 32)
        Me.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.ButtonClose.TabIndex = 12
        Me.ButtonClose.TabStop = False
        '
        'SingerBox
        '
        Me.SingerBox.AllowWebBrowserDrop = False
        Me.SingerBox.IsWebBrowserContextMenuEnabled = False
        Me.SingerBox.Location = New System.Drawing.Point(215, 254)
        Me.SingerBox.MinimumSize = New System.Drawing.Size(20, 20)
        Me.SingerBox.Name = "SingerBox"
        Me.SingerBox.ScriptErrorsSuppressed = True
        Me.SingerBox.ScrollBarsEnabled = False
        Me.SingerBox.Size = New System.Drawing.Size(373, 194)
        Me.SingerBox.TabIndex = 13
        Me.SingerBox.Tag = "nor-found-omg"
        Me.SingerBox.Url = New System.Uri("http://www.mtv.com/ap/error/pageNotFound", System.UriKind.Absolute)
        Me.SingerBox.WebBrowserShortcutsEnabled = False
        '
        'ButtonEdit
        '
        Me.ButtonEdit.BackColor = System.Drawing.Color.White
        Me.ButtonEdit.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEdit.Location = New System.Drawing.Point(22, 297)
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.Size = New System.Drawing.Size(174, 37)
        Me.ButtonEdit.TabIndex = 8
        '
        'ButtonMute
        '
        Me.ButtonMute.BackColor = System.Drawing.Color.White
        Me.ButtonMute.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMute.Location = New System.Drawing.Point(22, 340)
        Me.ButtonMute.Name = "ButtonMute"
        Me.ButtonMute.Size = New System.Drawing.Size(174, 37)
        Me.ButtonMute.TabIndex = 7
        '
        'ButtonBlock
        '
        Me.ButtonBlock.BackColor = System.Drawing.Color.White
        Me.ButtonBlock.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBlock.Location = New System.Drawing.Point(22, 254)
        Me.ButtonBlock.Name = "ButtonBlock"
        Me.ButtonBlock.Size = New System.Drawing.Size(174, 37)
        Me.ButtonBlock.TabIndex = 4
        '
        'BLUI
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.EZBlocker.My.Resources.Resources.background2
        Me.ClientSize = New System.Drawing.Size(600, 460)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.SingerBox)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.ButtonMute)
        Me.Controls.Add(Me.TopMostCheckBox)
        Me.Controls.Add(Me.AutoAddCheckBox)
        Me.Controls.Add(Me.ButtonBlock)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.AlbumScroll)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(600, 460)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 460)
        Me.Name = "BLUI"
        Me.Opacity = 0.95R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "BLUI"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.notifyIconMenu.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.ButtonClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AlbumScroll As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Private WithEvents notifyIcon As System.Windows.Forms.NotifyIcon
    Private WithEvents notifyIconMenu As System.Windows.Forms.ContextMenuStrip
    Private WithEvents BlockThisSongToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BehaviorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MinimizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ButtonBlock As EZBlocker.Flipbutton
    Friend WithEvents TopMostCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AutoAddCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonMute As EZBlocker.Flipbutton
    Friend WithEvents ButtonEdit As EZBlocker.Flipbutton
    Friend WithEvents ButtonRemoveEntry As System.Windows.Forms.Button
    Friend WithEvents blocklistBox As System.Windows.Forms.ListBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ResumeTimer As System.Windows.Forms.Timer
    Friend WithEvents MainTimer As System.Windows.Forms.Timer
    Friend WithEvents ButtonClose As System.Windows.Forms.PictureBox
    Friend WithEvents SingerBox As System.Windows.Forms.WebBrowser
End Class
