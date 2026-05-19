<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    partial Class MainForm
     Inherits System.Windows.Forms.Form
       '/ <summary>
        '/ Required designer variable.
        '/ </summary>
        Private components As System.ComponentModel.IContainer =  Nothing 
 
        '/ <summary>
        '/ Clean up any resources being used.
        '/ </summary>
        '/ <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '/ <summary>
    '/ Required method for Designer support - do not modify
    '/ the contents of this method with the code editor.
    '/ </summary>
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnStartMulti = New System.Windows.Forms.Button()
        Me.rtbLog = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'btnStartMulti
        '
        Me.btnStartMulti.Location = New System.Drawing.Point(418, 12)
        Me.btnStartMulti.Name = "btnStartMulti"
        Me.btnStartMulti.Size = New System.Drawing.Size(125, 60)
        Me.btnStartMulti.TabIndex = 0
        Me.btnStartMulti.Text = "Start Muti Thread"
        Me.btnStartMulti.UseVisualStyleBackColor = True
        '
        'rtbLog
        '
        Me.rtbLog.Location = New System.Drawing.Point(12, 78)
        Me.rtbLog.Name = "rtbLog"
        Me.rtbLog.Size = New System.Drawing.Size(530, 277)
        Me.rtbLog.TabIndex = 1
        Me.rtbLog.Text = ""
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(555, 367)
        Me.Controls.Add(Me.rtbLog)
        Me.Controls.Add(Me.btnStartMulti)
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnStartMulti As System.Windows.Forms.Button
    Private rtbLog As System.Windows.Forms.RichTextBox
End Class
