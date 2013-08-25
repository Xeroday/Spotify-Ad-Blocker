Imports System
Imports System.IO
Public Class Form1
    Dim spotifyProcess As Process
    Dim title As String = "N/A"
    Dim artist As String = "N/A"
    Dim titleSplit() As String
    Dim emdash As Char = ChrW(8211)
    Dim checked As Boolean = False
    Dim muted As Boolean = False
    Dim stream As StreamWriter
    Dim clicked As Boolean = False

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        My.Computer.FileSystem.WriteAllText("blocklist.txt", artist & Environment.NewLine, True)
        NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", Environment.NewLine & artist & " has been added to the blacklist. To delete, open the blacklist and remove the line containing " & artist & ".", ToolTipIcon.None)
        artist = "N/A"
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles BlocklistTimer.Tick
        title = getTitle()
        If title.Contains(" - ") Then 'A song is playing
            title = title.Remove(0, 10) 'Remove "Spotify - "
            title = title.Replace(" " & emdash & " ", "#") 'Replace spacer with parceable character
            titleSplit = title.Split(CChar("#")) 'Split Artist and Song Name
            If artist.Equals(titleSplit(0)) Then
                checked = True
            Else
                artist = titleSplit(0)
                checked = False
            End If
            If Not checked Then
                If Not muted Then
                    If My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\blocklist.txt").Contains(artist) Then 'Check blacklist for artist
                        Shell("cmd.exe /c nircmd muteappvolume spotify.exe 1", vbHide) 'Mute Spotify process
                        muted = True
                        ResumeTimer.Start()
                    Else
                        NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", Environment.NewLine & artist & " is currently unblocked. Click this balloon popup to add " & artist & " to the blacklist.", ToolTipIcon.None) 'Artist is not in blacklist
                        clicked = False
                    End If
                Else
                    Shell("cmd.exe /c nircmd muteappvolume spotify.exe 0", vbHide) 'Not an ad, unmute
                    muted = False
                    ResumeTimer.Stop()
                End If
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Not My.Computer.FileSystem.FileExists(Application.StartupPath & "\nircmd.exe") Then
            My.Computer.FileSystem.WriteAllBytes("nircmd.exe", My.Resources.nircmd, False)
        End If
        If Not My.Computer.FileSystem.FileExists(Application.StartupPath & "\blocklist.txt") Then
            Try
                Dim _WebClient As New System.Net.WebClient()
                _WebClient.Headers("User-Agent") = "EZBlocker " & My.Application.Info.Version.ToString & " " & My.Computer.Info.OSFullName
                _WebClient.DownloadFile("http://www.ericzhang.me/dl/?file=blocklist.txt", "blocklist.txt")
            Catch ex As Exception
                MessageBox.Show("Could not download blocklist, EZBlocker will create an empty one.")
                My.Computer.FileSystem.WriteAllText("blocklist.txt", Environment.NewLine, False)
            End Try
        End If
        Try
            Process.Start(Environment.GetEnvironmentVariable("APPDATA") & "\Spotify\spotify.exe")
        Catch ex As Exception
        End Try
        Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
    End Sub

    Private Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        Process.Start("notepad.exe", Application.StartupPath & "\blocklist.txt")
    End Sub

    Private Sub NotifyIcon1_BalloonTipClicked(sender As Object, e As System.EventArgs) Handles NotifyIcon1.BalloonTipClicked
        If Not clicked Then
            Button1.PerformClick()
            clicked = True
        End If
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Shell("cmd.exe /c nircmd muteappvolume spotify.exe 2", vbHide)
    End Sub

    Private Sub NotifyIcon1_Click(sender As Object, e As System.EventArgs) Handles NotifyIcon1.Click
        If Me.ShowInTaskbar = False Then
            Me.ShowInTaskbar = True
            Me.WindowState = FormWindowState.Normal
        Else
            Me.ShowInTaskbar = False
        End If
    End Sub


    Private Sub checkUpdate()

    End Sub

    Private Function getTitle() As String
        If Me.WindowState = FormWindowState.Minimized And Me.ShowInTaskbar = True Then 'Hide from system bar
            NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", "Click this icon to restore EZBlocker.", ToolTipIcon.None)
            Me.ShowInTaskbar = False 'Hide, not remove to keep process priority
        End If

        For Each Me.spotifyProcess In Process.GetProcessesByName("spotify") 'Hook onto Spotify
            Return spotifyProcess.MainWindowTitle.ToString
        Next
        Return ""
    End Function

    Private Sub wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles ResumeTimer.Tick
        For Each Me.spotifyProcess In Process.GetProcessesByName("spotify") 'Hook onto Spotify
            title = spotifyProcess.MainWindowTitle.ToString
        Next
        If Not title.Contains(" - ") Then 'Spotify is paused (Auto-pause for muting during an ad)
            NotifyIcon1.ShowBalloonTip(5000, "EZBlocker", "Playing ad in background.", ToolTipIcon.None)
            Keyboard.SendKey(Keys.MediaPlayPause) 'Resume playing the ad
        End If
    End Sub

    Private Sub AutoblockTimer_Tick(sender As Object, e As EventArgs) Handles AutoblockTimer.Tick

    End Sub
End Class
