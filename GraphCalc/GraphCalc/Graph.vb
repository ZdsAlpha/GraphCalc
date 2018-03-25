Imports NCalc

Public Class Graph
    Public Expression As Expression
    Public Origin As New Point(0, 0)
    Private Sub Graph_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Region.Invalidate()
    End Sub
    Private Sub Region_Paint(sender As Object, e As PaintEventArgs) Handles Region.Paint
        DrawAxis(e.Graphics, Region.Size, Origin)
        DrawFunction(e.Graphics, Region.Size, Origin, New Pen(Color.Red, 3), Expression, 3)
    End Sub
    Dim moving As Boolean = False
    Dim o As Point
    Dim point As Point
    Private Sub Region_MouseDown(sender As Object, e As MouseEventArgs) Handles Region.MouseDown
        moving = True
        o = Origin
        point = e.Location
    End Sub
    Private Sub Region_MouseMove(sender As Object, e As MouseEventArgs) Handles Region.MouseMove
        If moving Then
            Origin = o - point + e.Location
            Region.Invalidate()
        End If
    End Sub
    Private Sub Region_MouseUp(sender As Object, e As MouseEventArgs) Handles Region.MouseUp
        moving = False
    End Sub
    Private Sub Graph_Load(sender As Object, e As EventArgs) Handles Me.Load
        Origin = New Point(Region.Width / 2, Region.Height / 2)
    End Sub
    Private Sub Region_MouseWheel(sender As Object, e As MouseEventArgs) Handles Region.MouseWheel
        Unit += e.Delta / 50
        If Unit > 200 Then Unit = 200
        If Unit < 20 Then Unit = 20
        Region.Invalidate()
    End Sub
End Class
