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
    Dim autoAdd As Boolean = True ' Auto add to blocklist
    Dim muted As Boolean = False
    Dim stream As StreamWriter
    Dim clicked As Boolean = False
    Dim website As String = "http://www.ericzhang.me/projects/spotify-ad-blocker-ezblocker/"

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        My.Computer.FileSystem.WriteAllText("blocklist.txt", artist & Environment.NewLine, True)
        NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", artist & " has been added to the blacklist. To delete, open the blacklist and remove the line containing " & artist & ".", ToolTipIcon.None)
        artist = "N/A"
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles MainTimer.Tick
        titleSplit = GetTitle()
        If playing Then
            If Not Checked() Then ' If not the same artist then:
                If Not muted Then
                    If My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\blocklist.txt").Contains(artist) Then
                        Shell("cmd.exe /c nircmd muteappvolume spotify.exe 1", vbHide) 'Mute Spotify process
                        muted = True
                        ResumeTimer.Start()
                    Else
                        If Not Check() Then ' If no add is autoblocked or autoAdd is disabled: 
                            NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", artist & " is currently unblocked. Click this balloon popup to add " & artist & " to the blacklist.", ToolTipIcon.None) 'Artist is not in blacklist
                            clicked = False
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
        checkUpdate()
        If Not My.Computer.FileSystem.FileExists(Application.StartupPath & "\nircmd.exe") Then ' Extract
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
        Try ' Start Spotify
            Process.Start(Environment.GetEnvironmentVariable("APPDATA") & "\Spotify\spotify.exe")
        Catch ex As Exception
        End Try
        Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
    End Sub

    Private Sub checkUpdate()
        Try
            Dim latest As String = GetPage("http://www.ericzhang.me/dl/?file=EZBlocker-version.txt", "EZBlocker " & My.Application.Info.Version.ToString & " " & My.Computer.Info.OSFullName) 'Query for latest version
            If Double.Parse(latest) > Double.Parse(My.Application.Info.Version.ToString.Replace(".", "")) Then
                If MessageBox.Show("Your EZBlocker is out of date. Would you like to upgrade?", "EZBlocker", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = vbYes Then
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
        If Me.WindowState = FormWindowState.Minimized And Me.ShowInTaskbar = True Then 'Hide from system bar
            NotifyIcon1.ShowBalloonTip(10000, "EZBlocker", "Click this icon to restore EZBlocker.", ToolTipIcon.None)
            Me.ShowInTaskbar = False 'Hide, not remove to keep process priority
        End If
        For Each Me.spotifyProcess In Process.GetProcessesByName("spotify") 'Hook onto Spotify
            t = spotifyProcess.MainWindowTitle.ToString
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
        If autoAdd Then
            Try
                Dim rArtist As String = GetPage("http://ws.spotify.com/search/1/artist?q=" & artist, "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)") 'Query server for artist
                Using reader As XmlReader = XmlReader.Create(New StringReader(rArtist))
                    reader.ReadToFollowing("opensearch:totalResults") 'Get number of results
                    If reader.ReadElementContentAsInt() = 0 Then '0 results = ad
                        Button1.PerformClick()
                        clicked = True
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

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles ResumeTimer.Tick 'Resumes playing ads when auto stopped due to muting
        Dim title As String = ""
        For Each Me.spotifyProcess In Process.GetProcessesByName("spotify") 'Hook onto Spotify
            title = spotifyProcess.MainWindowTitle.ToString
        Next
        If Not title.Contains(" - ") Then 'Spotify is paused (Auto-pause for muting during an ad)
            NotifyIcon1.ShowBalloonTip(5000, "EZBlocker", "Playing ad in background.", ToolTipIcon.None)
            Keyboard.SendKey(Keys.MediaPlayPause) 'Resume playing the ad
        End If
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

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(website)
    End Sub

    Private Sub AutoAddCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles AutoAddCheckBox.CheckedChanged
        autoAdd = AutoAddCheckBox.Checked
    End Sub
End Class
