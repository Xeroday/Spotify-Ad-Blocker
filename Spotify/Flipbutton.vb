Imports System.Drawing

Public Class Flipbutton
    Dim image As Bitmap

    Private Sub Flipbutton_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub initiate(ByVal text As String, ByVal textPosition As PointF, ByVal iconBitmap As Bitmap)
        Dim bg As Bitmap = New Bitmap(Size.Width, Size.Height)
        Dim g As Graphics = Graphics.FromImage(bg)
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.FillRectangle(Brushes.White, New Rectangle(0, 0, Size.Width, Size.Height))
        g.DrawRectangle(New Pen(Brushes.Green, 2), New Rectangle(0, 0, Size.Width, Size.Height))
        g.DrawString(text, New Font(Me.Font.FontFamily, 17), New SolidBrush(Color.FromArgb(32, 32, 32)), textPosition)

        g.Save()
        g.Dispose()
        Me.BackgroundImage = bg
        image = iconBitmap
    End Sub

    Dim popped As Boolean = False

    Private Sub Flipbutton_MouseHover(sender As Object, e As EventArgs) Handles MyBase.MouseHover
        If (Not popped) Then
            Me.Left += 2
            Me.Top -= 2
            popped = True
        End If
    End Sub

    Private Sub Flipbutton_MouseLeave(sender As Object, e As EventArgs) Handles MyBase.MouseLeave
        If (popped) Then
            Me.Left -= 2
            Me.Top += 2
            popped = False
        End If
    End Sub
End Class
