Public Class Main
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Exp = GetExpression(TextBox1.Text)
        Exp.Parameters("x") = 0
        Try
            Exp.Evaluate()
            Graph.Expression = Exp
            Me.Hide()
            Graph.ShowDialog()
            Me.Show()
        Catch ex As Exception
            MsgBox("Enter a valid expression.", MsgBoxStyle.Critical, "Error!")
        End Try
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = vbCr Then
            Button1.PerformClick()

        End If
    End Sub
End Class