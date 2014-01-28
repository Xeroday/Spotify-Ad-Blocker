<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ButtonBlock = New System.Windows.Forms.Button()
        Me.MainTimer = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.notifyIconMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BlockThisSongToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BehaviorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MinimizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonEdit = New System.Windows.Forms.Button()
        Me.ButtonMute = New System.Windows.Forms.Button()
        Me.ResumeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.AutoAddCheckBox = New System.Windows.Forms.CheckBox()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.TopMostCheckBox = New System.Windows.Forms.CheckBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ButtonRemoveEntry = New System.Windows.Forms.Button()
        Me.blocklistBox = New System.Windows.Forms.ListBox()
        Me.StartupTimer = New System.Windows.Forms.Timer(Me.components)
        Me.notifyIconMenu.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonBlock
        '
        Me.ButtonBlock.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonBlock.Location = New System.Drawing.Point(12, 18)
        Me.ButtonBlock.Name = "ButtonBlock"
        Me.ButtonBlock.Size = New System.Drawing.Size(222, 29)
        Me.ButtonBlock.TabIndex = 0
        Me.ButtonBlock.Text = "&Block this song"
        Me.ButtonBlock.UseVisualStyleBackColor = True
        '
        'MainTimer
        '
        Me.MainTimer.Enabled = True
        Me.MainTimer.Interval = 750
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.notifyIconMenu
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "EZBlocker Improved" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Right-click to show menu"
        Me.NotifyIcon1.Visible = True
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
        'ButtonEdit
        '
        Me.ButtonEdit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonEdit.Location = New System.Drawing.Point(12, 114)
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.Size = New System.Drawing.Size(222, 29)
        Me.ButtonEdit.TabIndex = 0
        Me.ButtonEdit.Text = "&Edit Blocklist"
        Me.ButtonEdit.UseVisualStyleBackColor = True
        '
        'ButtonMute
        '
        Me.ButtonMute.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonMute.Location = New System.Drawing.Point(12, 62)
        Me.ButtonMute.Name = "ButtonMute"
        Me.ButtonMute.Size = New System.Drawing.Size(222, 29)
        Me.ButtonMute.TabIndex = 0
        Me.ButtonMute.Text = "&Mute Spotify"
        Me.ButtonMute.UseVisualStyleBackColor = True
        '
        'ResumeTimer
        '
        Me.ResumeTimer.Interval = 1500
        '
        'AutoAddCheckBox
        '
        Me.AutoAddCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AutoAddCheckBox.AutoSize = True
        Me.AutoAddCheckBox.BackColor = System.Drawing.Color.Transparent
        Me.AutoAddCheckBox.Checked = True
        Me.AutoAddCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoAddCheckBox.Location = New System.Drawing.Point(12, 166)
        Me.AutoAddCheckBox.Name = "AutoAddCheckBox"
        Me.AutoAddCheckBox.Size = New System.Drawing.Size(137, 17)
        Me.AutoAddCheckBox.TabIndex = 1
        Me.AutoAddCheckBox.Text = "Automatically block ads"
        Me.AutoAddCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.AutoAddCheckBox.UseVisualStyleBackColor = False
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel1.LinkColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LinkLabel1.Location = New System.Drawing.Point(497, 191)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(35, 13)
        Me.LinkLabel1.TabIndex = 2
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "About"
        '
        'TopMostCheckBox
        '
        Me.TopMostCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TopMostCheckBox.AutoSize = True
        Me.TopMostCheckBox.BackColor = System.Drawing.Color.Transparent
        Me.TopMostCheckBox.Location = New System.Drawing.Point(12, 190)
        Me.TopMostCheckBox.Name = "TopMostCheckBox"
        Me.TopMostCheckBox.Size = New System.Drawing.Size(151, 17)
        Me.TopMostCheckBox.TabIndex = 3
        Me.TopMostCheckBox.Text = "Stay above other windows"
        Me.TopMostCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.TopMostCheckBox.UseVisualStyleBackColor = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.Color.Transparent
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.SplitContainer1.Panel1.BackgroundImage = CType(resources.GetObject("SplitContainer1.Panel1.BackgroundImage"), System.Drawing.Image)
        Me.SplitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonBlock)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonEdit)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonMute)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.SplitContainer1.Panel2.BackgroundImage = CType(resources.GetObject("SplitContainer1.Panel2.BackgroundImage"), System.Drawing.Image)
        Me.SplitContainer1.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.SplitContainer1.Panel2.Controls.Add(Me.ButtonRemoveEntry)
        Me.SplitContainer1.Panel2.Controls.Add(Me.blocklistBox)
        Me.SplitContainer1.Panel2.Enabled = False
        Me.SplitContainer1.Size = New System.Drawing.Size(544, 153)
        Me.SplitContainer1.SplitterDistance = 246
        Me.SplitContainer1.TabIndex = 4
        '
        'ButtonRemoveEntry
        '
        Me.ButtonRemoveEntry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRemoveEntry.Location = New System.Drawing.Point(13, 114)
        Me.ButtonRemoveEntry.Name = "ButtonRemoveEntry"
        Me.ButtonRemoveEntry.Size = New System.Drawing.Size(269, 29)
        Me.ButtonRemoveEntry.TabIndex = 6
        Me.ButtonRemoveEntry.Text = "&Remove"
        Me.ButtonRemoveEntry.UseVisualStyleBackColor = True
        '
        'blocklistBox
        '
        Me.blocklistBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blocklistBox.BackColor = System.Drawing.Color.White
        Me.blocklistBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.blocklistBox.FormattingEnabled = True
        Me.blocklistBox.Location = New System.Drawing.Point(13, 18)
        Me.blocklistBox.Name = "blocklistBox"
        Me.blocklistBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.blocklistBox.Size = New System.Drawing.Size(269, 80)
        Me.blocklistBox.TabIndex = 5
        '
        'StartupTimer
        '
        Me.StartupTimer.Enabled = True
        Me.StartupTimer.Interval = 10000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.EZBlocker.My.Resources.Resources.background1
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(544, 211)
        Me.Controls.Add(Me.TopMostCheckBox)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.AutoAddCheckBox)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(560, 600)
        Me.MinimumSize = New System.Drawing.Size(560, 250)
        Me.Name = "Form1"
        Me.notifyIconMenu.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MainTimer As System.Windows.Forms.Timer
    Friend WithEvents ResumeTimer As System.Windows.Forms.Timer
    Friend WithEvents AutoAddCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TopMostCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents BehaviorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MinimizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents blocklistBox As System.Windows.Forms.ListBox
    Friend WithEvents ButtonRemoveEntry As System.Windows.Forms.Button
    Private WithEvents ButtonBlock As System.Windows.Forms.Button
    Private WithEvents ButtonEdit As System.Windows.Forms.Button
    Private WithEvents ButtonMute As System.Windows.Forms.Button
    Private WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Private WithEvents notifyIconMenu As System.Windows.Forms.ContextMenuStrip
    Private WithEvents BlockThisSongToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartupTimer As System.Windows.Forms.Timer

End Class
