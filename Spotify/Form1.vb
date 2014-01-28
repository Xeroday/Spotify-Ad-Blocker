Imports System
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Xml

Public Class Form1
    Dim spotifyProcess As Process
    Dim playing As Boolean = False
    Dim titleSplit() As String
    Dim artist As String = "N/A"
    Dim emdash As Char = ChrW(8211)
    Dim muted As Boolean = False
    Dim website As String = "https://github.com/moodspace"

    Private Sub ButtonBlock_Click(sender As System.Object, e As System.EventArgs) Handles ButtonBlock.Click
        If (ButtonBlock.Text.StartsWith("&Block")) Then
            block()
        Else
            unblock()
        End If
    End Sub

    Private Sub block()
        NotifyIcon1.Icon = My.Resources.blocked
        ButtonBlock.Text = ButtonBlock.Text.Replace("&Block", "Un&block")
        BlockThisSongToolStripMenuItem.Checked = True

        If Not blocklist.Contains(artist) Then
            blocklist.Add(artist)
            'can be disabled since notify icon has shown the change
            'NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", artist & " has been added to the blacklist. To delete, open the blacklist and remove the line containing " & artist & ".", ToolTipIcon.None)
            artist = "N/A"
        End If

    End Sub

    Private Sub unblock()
        NotifyIcon1.Icon = My.Resources.allowed
        ButtonBlock.Text = ButtonBlock.Text.Replace("Un&block", "&Block")
        BlockThisSongToolStripMenuItem.Checked = False

        While (blocklist.Contains(artist))
            blocklist.Remove(artist)
        End While

        'can be disabled since notify icon has shown the change
        'NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", artist & " has been added to the blacklist. To delete, open the blacklist and remove the line containing " & artist & ".", ToolTipIcon.None)
        artist = titleSplit(0)
        setVolume(volume.u0nmuted)
    End Sub

    Private Sub MainTimer_Tick(sender As System.Object, e As System.EventArgs) Handles MainTimer.Tick
        titleSplit = GetTitle()
        If playing Then
            If Not Checked() Then ' If not the same artist then:
                If Not muted Then
                    If blocklist.Contains(artist) Then
                        Shell("cmd.exe /c nircmd muteappvolume spotify.exe 1", vbHide) 'Mute Spotify process
                        muted = True
                        ResumeTimer.Start()
                    Else
                        If Not Check() Then ' If no add is autoblocked or autoAdd is disabled:
                            'the balloon can be disabled since the notify icon will alert user
                            'NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", artist & " is currently unblocked. Click this balloon popup to add " & artist & " to the blacklist.", ToolTipIcon.None) 'Artist is not in blacklist
                            NotifyIcon1.Icon = My.Resources.allowed
                            BlockThisSongToolStripMenuItem.Checked = False
                        End If
                    End If
                Else
                    Shell("cmd.exe /c nircmd muteappvolume spotify.exe 0", vbHide) 'Not an ad, unmute
                    ResumeTimer.Stop()
                    muted = False
                    artist = "N/A"
                End If
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'disable due to version conflict
        'checkUpdate()

        Try
            If Not My.Computer.FileSystem.FileExists(nircmd_path) Then ' Extract
                My.Computer.FileSystem.WriteAllBytes(nircmd_path, My.Resources.nircmd, False)
            End If

            ' Start Spotify
            Process.Start(Environment.GetEnvironmentVariable("APPDATA") & "\Spotify\spotify.exe")

            Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal

            'apply product name to caption
            Me.Text = Application.ProductName
            'read settings
            LiveSettings.readSettings()
            Me.CloseToolStripMenuItem.Checked = LiveSettings.closeTray
            Me.MinimizeToolStripMenuItem.Checked = LiveSettings.minTray
            Me.AutoAddCheckBox.Checked = LiveSettings.autoAdd
            Me.TopMostCheckBox.Checked = LiveSettings.topmost

            Me.BringToFront()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub checkBlocklist()
        Try
            Dim _WebClient As New System.Net.WebClient()
            _WebClient.Headers("User-Agent") = Application.ProductName & " " & Application.ProductVersion & " " & My.Computer.Info.OSFullName
            Dim list As String = _WebClient.DownloadString("http://www.ericzhang.me/dl/?file=blocklist.txt")
            While (list.Length > 0)
                Dim entry As String = list.Substring(0, list.IndexOf(Environment.NewLine))
                If (Not blocklist.Contains(entry.Trim())) Then
                    blocklist.Add(entry.Trim())
                End If
                list = list.Substring(entry.Length + Environment.NewLine.Length)
            End While
        Catch ex As Exception
            NotifyIcon1.ShowBalloonTip(5000, Application.ProductName, "Failed to download blocklist", ToolTipIcon.None)
        End Try
    End Sub

    Private Sub checkUpdate()
        Try
            Dim latest As String = GetPage("http://www.ericzhang.me/dl/?file=EZBlocker-version.txt", "EZBlocker " & My.Application.Info.Version.ToString() & " " & My.Computer.Info.OSFullName) 'Query for latest version
            If Double.Parse(latest) > Double.Parse(My.Application.Info.Version.ToString().Replace(".", "")) Then
                If MessageBox.Show("Your EZBlocker is out of date. Would you like to upgrade?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = vbYes Then
                    Process.Start(website)
                    Me.Close()
                End If
            End If
        Catch ex As Exception
            Console.WriteLine("Error checking for updates")
        End Try
    End Sub

    Private Function GetTitle() As String()
        Dim t As String
        Dim ts() As String
        'If Me.WindowState = FormWindowState.Minimized And Me.ShowInTaskbar = True Then 'Hide from system bar
        '    NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", "Click this icon to restore EZBlocker.", ToolTipIcon.None)
        '    Me.ShowInTaskbar = False 'Hide, not remove to keep process priority
        'End If
        For Each Me.spotifyProcess In Process.GetProcessesByName("spotify") 'Hook onto Spotify
            t = spotifyProcess.MainWindowTitle.ToString()
            If t.Contains(" - ") Then 'A song is playing
                playing = True
                t = t.Remove(0, 10).Replace(" " & emdash & " ", "#") 'Remove "Spotify - ", Replace spacer with parceable character
                ts = t.Split(CChar("#")) 'Split Artist and Song Name
                Return ts
            Else
                playing = False
            End If
            Return {"", ""} 'Return here to prevent extra loops
        Next
        Return {"", ""}
    End Function

    Private Function Checked() As Boolean ' See if current artist has been checked
        If artist.Equals(titleSplit(0)) Then
            Return True
        Else
            artist = titleSplit(0)
            Return False
        End If
    End Function

    Public Function GetPage(ByVal URL As String, ByVal UA As String) As String
        Dim S As String = ""
        Dim _WebClient As New System.Net.WebClient()
        _WebClient.Headers("User-Agent") = UA
        S = _WebClient.DownloadString(URL)
        Return S
    End Function

    Private Function Check() As Boolean ' Check to see if an ad is playing and add to block list
        If (LiveSettings.autoAdd) Then
            Try
                Dim rArtist As String = GetPage("http://ws.spotify.com/search/1/artist?q=" & artist, "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)") 'Query server for artist
                Using reader As XmlReader = XmlReader.Create(New StringReader(rArtist))
                    reader.ReadToFollowing("opensearch:totalResults") 'Get number of results
                    If reader.ReadElementContentAsInt() = 0 Then '0 results = ad
                        block()
                        Return True
                    End If
                End Using
            Catch ex As Exception
                Console.WriteLine("Error checking ad")
            End Try
        End If
        Return False
    End Function

    Private Sub wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub

    Private Sub ResumeTimer_Tick(sender As Object, e As EventArgs) Handles ResumeTimer.Tick 'Resumes playing ads when auto stopped due to muting
        Dim title As String = ""
        For Each Me.spotifyProcess In Process.GetProcessesByName("spotify") 'Hook onto Spotify
            title = spotifyProcess.MainWindowTitle.ToString()
        Next
        If Not title.Contains(" - ") Then 'Spotify is paused (Auto-pause for muting during an ad)
            'can be diabled since notify icon will alert user
            'NotifyIcon1.ShowBalloonTip(5000, "EZBlocker", "Playing ad in background.", ToolTipIcon.None)
            Keyboard.SendKey(Keys.MediaPlayPause) 'Resume playing the ad
        End If
    End Sub

    Private Sub ButtonEdit_Click(sender As Object, e As System.EventArgs) Handles ButtonEdit.Click
        If (ButtonEdit.Text = "&Edit Blocklist") Then
            ButtonBlock.Enabled = False

            blocklistBox.Items.Clear()
            blocklistBox.BeginUpdate() 'prevent flicker
            blocklistBox.Items.AddRange(LiveSettings.blocklist.ToArray())
            blocklistBox.EndUpdate() 'restore layout

            ButtonEdit.Text = "Finish &Editing"
            SplitContainer1.Panel2.Enabled = True
        Else
            ButtonBlock.Enabled = True

            blocklist.Clear()
            For Each line As String In blocklistBox.Items
                blocklist.Add(line)
            Next

            ButtonEdit.Text = "&Edit Blocklist"
            SplitContainer1.Panel2.Enabled = False
        End If
    End Sub

    Private Sub ButtonMute_Click(sender As System.Object, e As System.EventArgs) Handles ButtonMute.Click
        If (ButtonMute.Text.Contains("Un&mute")) Then
            setVolume(volume.u0nmuted)
            ButtonMute.Text = ButtonMute.Text.Replace("Un&mute", "&Mute")
        Else
            setVolume(volume.m1uted)
            ButtonMute.Text = ButtonMute.Text.Replace("&Mute", "Un&mute")
        End If
    End Sub

    Enum volume As Integer
        u0nmuted = 0
        m1uted = 1
        t2oggle_muted = 2
    End Enum

    Private Sub setVolume(ByVal volume As volume)
        Shell("cmd.exe /c nircmd muteappvolume spotify.exe " + volume.ToString().Substring(1, 1), vbHide)
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(website)
    End Sub

    Private Sub AutoAddCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles AutoAddCheckBox.CheckedChanged
        LiveSettings.autoAdd = AutoAddCheckBox.Checked
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            Me.Visible = Not Me.Visible
            Me.BringToFront()
        End If
    End Sub

    Private Sub BlockThisSongToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlockThisSongToolStripMenuItem.Click
        block()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            If (LiveSettings.minTray) Then
                Me.Hide()
            End If
        End If
    End Sub

    Private Sub TopMostCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TopMostCheckBox.CheckedChanged
        LiveSettings.topmost = TopMostCheckBox.Checked
        Me.TopMost = TopMostCheckBox.Checked
    End Sub

    Private Sub CloseToolStripMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.CheckedChanged
        LiveSettings.closeTray = CloseToolStripMenuItem.Checked
    End Sub

    Private Sub MinimizeToolStripMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles MinimizeToolStripMenuItem.CheckedChanged
        LiveSettings.minTray = MinimizeToolStripMenuItem.Checked
    End Sub

    Private Sub ButtonRemoveEntry_Click(sender As Object, e As EventArgs) Handles ButtonRemoveEntry.Click
        Dim selected(0 To blocklistBox.SelectedItems.Count) As Object

        For x As Integer = 0 To blocklistBox.SelectedIndices.Count - 1 Step 1
            selected(x) = blocklistBox.Items(x)
        Next

        For Each item As Object In selected
            blocklistBox.Items.Remove(item)
        Next
    End Sub

    'one-time usage (different from LiveSettings.closeTray) if user clicks 'exit'
    Dim exitApp As Boolean = False
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        exitApp = True
        Me.Close()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        LiveSettings.writeSettings()

        If (LiveSettings.closeTray And Not exitApp) Then
            e.Cancel = True
            Me.Hide()
        End If
    End Sub

    Private Sub StartupTimer_Tick(sender As Object, e As EventArgs) Handles StartupTimer.Tick
        'speeds up form loading not to download list at form loading
        checkBlocklist()
        StartupTimer.Enabled = False
    End Sub
End Class
