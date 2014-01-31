Imports System.Xml
Imports System.IO

Public Class BLUI
    Dim spotifyProcess As Process
    Dim playing As Boolean = False
    Dim titleSplit() As String
    Dim artist As String = "N/A"
    Dim emdash As Char = ChrW(8211)
    Dim muted As Boolean = False
    Dim website As String = "https://github.com/moodspace"

    Dim timePlayed As Integer

    Private Sub block()
        notifyIcon.Icon = My.Resources.blocked
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
        notifyIcon.Icon = My.Resources.allowed
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

    Private Sub BLUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ButtonBlock.initiate("Block this song", New PointF(5, 2), New Bitmap(1, 1))
        ButtonBlock.Text = "&Block this song"

        ButtonEdit.initiate("Edit Blocklist", New PointF(15, 2), New Bitmap(1, 1))
        ButtonEdit.Text = "&Edit Blocklist"

        ButtonMute.initiate("Mute Spotify", New PointF(15, 2), New Bitmap(1, 1))
        ButtonMute.Text = "&Mute Spotify"

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
            Me.Text = ex.Message
        End Try
    End Sub

    Private Sub ButtonBlock_Click(sender As Object, e As EventArgs) Handles ButtonBlock.Click
        If (ButtonBlock.Text.StartsWith("&Block")) Then
            block()
        Else
            unblock()
        End If
    End Sub

    Private Sub ButtonMute_Click(sender As Object, e As EventArgs) Handles ButtonMute.Click
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

    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        If (ButtonEdit.Text = "&Edit Blocklist") Then
            ButtonBlock.Enabled = False

            blocklistBox.Items.Clear()
            blocklistBox.BeginUpdate() 'prevent flicker
            blocklistBox.Items.AddRange(LiveSettings.blocklist.ToArray())
            blocklistBox.EndUpdate() 'restore layout

            ButtonEdit.Text = "Finish &Editing"
            Panel1.Enabled = True
            Panel1.Visible = True
            ButtonEdit.Left += 375
            Panel1.Left -= 375
        Else
            ButtonBlock.Enabled = True

            blocklist.Clear()
            For Each line As String In blocklistBox.Items
                blocklist.Add(line)
            Next

            ButtonEdit.Text = "&Edit Blocklist"
            Panel1.Enabled = False
            Panel1.Visible = False
            ButtonEdit.Left -= 375
            Panel1.Left += 375
        End If
    End Sub

    Private Sub ResumeTimer_Tick(sender As Object, e As EventArgs) Handles ResumeTimer.Tick
        timePlayed = 0
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

    Private Function generateSpace(ByVal length As Integer) As String
        If (length = 0) Then
            Return ""
        Else
            Return " " + generateSpace(length - 1)
        End If
    End Function

    Private Sub MainTimer_Tick(sender As Object, e As EventArgs) Handles MainTimer.Tick
        timePlayed += 1

        titleSplit = GetTitle()
        If (AlbumScroll.Left > Me.Width) Then
            AlbumScroll.Left = 0 - AlbumScroll.Width
        Else
            AlbumScroll.Left += 1
        End If

        If (Not SingerBox.Tag.ToString() = titleSplit(0)) Then
            SingerBox.Navigate("http://www.cmt.com/artists/" + titleSplit(0).ToLower().Trim().Replace(" ", "-"))
            SingerBox.Tag = titleSplit(0)
        End If

        Dim bmp As Bitmap = New Bitmap(Me.BackgroundImage)
        For i As Integer = 1 To 235
            Dim c As Color = bmp.GetPixel(timePlayed, i)

            If (c.R > 238 And c.R < 244 And c.G > 238 And c.G < 244 And c.B > 238 And c.B < 244) Then
                bmp.SetPixel(timePlayed, i, Color.FromArgb(150, 200, 100))
            End If

        Next
        Me.BackgroundImage = bmp

        AlbumScroll.Text = titleSplit(0) + "  -  " + titleSplit(1)

        ProgressBar1.PerformStep()
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
                            notifyIcon.Icon = My.Resources.allowed
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


    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Me.Close()
    End Sub
End Class