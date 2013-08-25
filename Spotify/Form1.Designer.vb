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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MainTimer = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.ResumeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.AutoAddCheckBox = New System.Windows.Forms.CheckBox()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(238, 29)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "&Add Current Song to Blocklist"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MainTimer
        '
        Me.MainTimer.Enabled = True
        Me.MainTimer.Interval = 750
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "EZBlocker"
        Me.NotifyIcon1.Visible = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 41)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(238, 29)
        Me.Button2.TabIndex = 0
        Me.Button2.Text = "&Open Blacklist"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(12, 76)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(238, 29)
        Me.Button3.TabIndex = 0
        Me.Button3.Text = "&Mute/Unmute Spotify"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'ResumeTimer
        '
        Me.ResumeTimer.Interval = 1500
        '
        'AutoAddCheckBox
        '
        Me.AutoAddCheckBox.AutoSize = True
        Me.AutoAddCheckBox.Checked = True
        Me.AutoAddCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoAddCheckBox.Location = New System.Drawing.Point(12, 111)
        Me.AutoAddCheckBox.Name = "AutoAddCheckBox"
        Me.AutoAddCheckBox.Size = New System.Drawing.Size(169, 17)
        Me.AutoAddCheckBox.TabIndex = 1
        Me.AutoAddCheckBox.Text = "Enable Automatic Ad Blocking"
        Me.AutoAddCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.AutoAddCheckBox.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(215, 112)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(35, 13)
        Me.LinkLabel1.TabIndex = 2
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "About"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(262, 132)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.AutoAddCheckBox)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "EZBlocker"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents MainTimer As System.Windows.Forms.Timer
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ResumeTimer As System.Windows.Forms.Timer
    Friend WithEvents AutoAddCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel

End Class
