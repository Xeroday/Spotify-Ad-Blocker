Public Module LiveSettings

    'this class reads settings from My.Settings at startup 
    'and serve as the major way to provide real-time R/W of
    'settings, it writes settings to My.Settings at exit.


    'behaviors
    Friend closeTray As Boolean = True
    Friend minTray As Boolean = True
    'main form
    Friend autoAdd As Boolean = True
    Friend topmost As Boolean = True
    'paths
    Dim settingPath As String = System.IO.Path.Combine(Application.StartupPath, "settings.ini")
    'Friend blocklist_path As String = System.IO.Path.Combine(Application.StartupPath, "blocklist.txt")
    Friend nircmd_path As String = System.IO.Path.Combine(Application.StartupPath, "nircmd.exe")
    'block-list
    Friend blocklist As List(Of String) = New List(Of String)()

    Public Sub readSettings()
        'create default settings file if doesn't exist
        If (Not System.IO.File.Exists(settingPath)) Then
            writeSettings()
        End If

        Dim sReader As System.IO.StreamReader = New IO.StreamReader(settingPath)

        While (Not sReader.EndOfStream)
            Dim settingsLine As String = sReader.ReadLine()

            If (settingsLine.StartsWith(":BL ")) Then
                blocklist.Add(settingsLine.Substring(4).Trim())
            Else
                settingsLine = sReader.ReadLine.ToLower()
                'settings converted to lower
                If (settingsLine.StartsWith("close to tray")) Then
                    closeTray = parseBool(settingsLine)
                ElseIf (settingsLine.StartsWith("minimize to tray")) Then
                    minTray = parseBool(settingsLine)
                ElseIf (settingsLine.StartsWith("autoblock")) Then
                    autoAdd = parseBool(settingsLine)
                ElseIf (settingsLine.StartsWith("topmost")) Then
                    topmost = parseBool(settingsLine)
                End If
            End if

        End While

        sReader.Close()
    End Sub

    Public Sub writeSettings()
        'will overwrite
        Dim sWriter As System.IO.StreamWriter = New IO.StreamWriter(settingPath, False)

        sWriter.WriteLine(outputBoolSetting("close to tray", closeTray))
        sWriter.WriteLine(outputBoolSetting("minimize to tray", minTray))
        sWriter.WriteLine(outputBoolSetting("autoblock", autoAdd))
        sWriter.WriteLine(outputBoolSetting("topmost", Form1.TopMostCheckBox.Checked))

        For Each entry As String In blocklist
            sWriter.WriteLine("BL: " + entry)
        Next

        sWriter.Close()
    End Sub

    'Precondition: true and false are presented in lowercase
    Private Function parseBool(ByVal str As String) As Boolean
        If str.EndsWith("true") Then
            Return True
        Else : Return False
        End If
    End Function

    Private Function outputBoolSetting(ByVal settingName As String, ByVal settingVal As Boolean) As String
        Return settingName + " := " + settingVal.ToString()
    End Function

End Module
