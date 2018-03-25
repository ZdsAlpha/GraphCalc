Imports NCalc
Public Module Tools
    Public Property Background As Color = Color.White
    Public Property AxisPen As Pen = New Pen(Color.Black, 2)
    Public Property AxisPen2 As Pen = New Pen(Color.Gray, 0.5)
    Public Property Unit As Single = 30
    Public Property TextSpace As Single = 2
    Public Property TextBrush As Brush = Brushes.Black
    Public Property Font As Font = New Font("Consolas", 8)
    Public Property MaxDelta As Single = 200
    Public Sub DrawAxis(e As Graphics, Size As SizeF, Origin As PointF)
        e.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Clear(Background)
        Dim pi As PointF = PixelToPoint(Origin, New Point(0, 0))
        Dim pf As PointF = PixelToPoint(Origin, New PointF(Size.Width, Size.Height))
        For i = Math.Floor(pi.X) To Math.Ceiling(pf.X)
            Dim P As PointF = PointToPixel(Origin, New PointF(i, 0))
            e.DrawLine(AxisPen2, New Point(P.X, -1), New Point(P.X, Size.Height + 1))
            If i <> 0 Then
                Dim S = e.MeasureString(i.ToString, Font)
                e.FillRectangle(New SolidBrush(Background), New RectangleF(P.X - S.Width / 2, P.Y + TextSpace, S.Width, S.Height))
                e.DrawString(i.ToString, Font, TextBrush, New PointF(P.X - S.Width / 2, P.Y + TextSpace))
            End If
        Next
        For i = Math.Floor(pf.Y) To Math.Ceiling(pi.Y)
            Dim P As PointF = PointToPixel(Origin, New PointF(0, i))
            e.DrawLine(AxisPen2, New Point(-1, P.Y), New Point(Size.Width + 1, P.Y))
            If i <> 0 Then
                Dim S = e.MeasureString(i.ToString, Font)
                e.FillRectangle(New SolidBrush(Background), New RectangleF(P.X - S.Width - TextSpace, P.Y - S.Height / 2, S.Width, S.Height))
                e.DrawString(i.ToString, Font, TextBrush, New PointF(P.X - S.Width - TextSpace, P.Y - S.Height / 2))
            End If
        Next
        e.DrawLine(AxisPen, New Point(Origin.X, -1), New Point(Origin.X, Size.Height + 1))
        e.DrawLine(AxisPen, New Point(-1, Origin.Y), New Point(Size.Width + 1, Origin.Y))
    End Sub
    Public Sub DrawFunction(e As Graphics, Size As SizeF, Origin As PointF, Pen As Pen, F As Func(Of Single, Single), Optional Steps As Single = 10)
        Dim last As Single? = Nothing
        For i = -1 To Size.Width + 1 Step Steps
            Try
                Dim current = PointToPixel(Origin, New PointF(0, F.Invoke(PixelToPoint(Origin, New PointF(i, 0)).X))).Y
                If last IsNot Nothing AndAlso Not Single.IsNaN(last.Value) Then
                    If Math.Abs(last.Value - current) <= MaxDelta Then e.DrawLine(Pen, New PointF(i - Steps, last), New PointF(i, current))
                End If
                last = current
            Catch ex As Exception
                last = Nothing
            End Try
        Next
    End Sub
    Public Sub DrawFunction(e As Graphics, Size As SizeF, Origin As PointF, Pen As Pen, Expression As Expression, Optional Steps As Single = 10)
        DrawFunction(e, Size, Origin, Pen, Function(x)
                                               Expression.Parameters("x") = x
                                               Return Expression.Evaluate()
                                           End Function, Steps)
    End Sub
    Public Function PixelToPoint(Origin As PointF, Pixel As PointF) As PointF
        Return New PointF((Pixel.X - Origin.X) / Unit, (Origin.Y - Pixel.Y) / Unit)
    End Function
    Public Function PointToPixel(Origin As PointF, Point As PointF) As PointF
        Return New PointF(Origin.X + Point.X * Unit, Origin.Y - Point.Y * Unit)
    End Function
    Public Function GetExpression(Raw As String) As Expression
        Dim Expression As New Expression(Raw)
        Expression.Parameters("pi") = Math.PI
        Expression.Parameters("e") = Math.E
        Return Expression
    End Function
End Module