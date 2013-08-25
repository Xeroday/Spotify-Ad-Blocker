Public Class Keyboard
    Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Int32, ByVal dwExtraInfo As Int32)

    Public Shared Sub SendKey(ByVal key As Keys)
        keybd_event(CByte(key), 0, 0, 0)
    End Sub
End Class