<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrString
    Inherits System.Windows.Forms.Form

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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtTrString = New System.Windows.Forms.TextBox()
        Me.txtTrNo = New System.Windows.Forms.TextBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.cmdWrite = New System.Windows.Forms.Button()
        Me.cmdRead = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtTrString
        '
        Me.txtTrString.Location = New System.Drawing.Point(114, 121)
        Me.txtTrString.Name = "txtTrString"
        Me.txtTrString.Size = New System.Drawing.Size(249, 22)
        Me.txtTrString.TabIndex = 97
        Me.txtTrString.Text = "Duty On"
        '
        'txtTrNo
        '
        Me.txtTrNo.Location = New System.Drawing.Point(114, 72)
        Me.txtTrNo.Name = "txtTrNo"
        Me.txtTrNo.Size = New System.Drawing.Size(249, 22)
        Me.txtTrNo.TabIndex = 96
        Me.txtTrNo.Text = "0"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.BackColor = System.Drawing.SystemColors.Control
        Me.label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.label2.Location = New System.Drawing.Point(33, 73)
        Me.label2.Name = "label2"
        Me.label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.label2.Size = New System.Drawing.Size(45, 19)
        Me.label2.TabIndex = 94
        Me.label2.Text = "TrNo:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.BackColor = System.Drawing.SystemColors.Control
        Me.label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.label1.Location = New System.Drawing.Point(33, 123)
        Me.label1.Name = "label1"
        Me.label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.label1.Size = New System.Drawing.Size(60, 19)
        Me.label1.TabIndex = 95
        Me.label1.Text = "TrString:"
        '
        'cmdWrite
        '
        Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWrite.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWrite.Location = New System.Drawing.Point(223, 168)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWrite.Size = New System.Drawing.Size(104, 29)
        Me.cmdWrite.TabIndex = 93
        Me.cmdWrite.Text = "Write"
        Me.cmdWrite.UseVisualStyleBackColor = False
        '
        'cmdRead
        '
        Me.cmdRead.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRead.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRead.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRead.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRead.Location = New System.Drawing.Point(74, 168)
        Me.cmdRead.Name = "cmdRead"
        Me.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRead.Size = New System.Drawing.Size(104, 29)
        Me.cmdRead.TabIndex = 92
        Me.cmdRead.Text = "Read"
        Me.cmdRead.UseVisualStyleBackColor = False
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
        Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMessage.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMessage.Location = New System.Drawing.Point(33, 25)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMessage.Size = New System.Drawing.Size(330, 28)
        Me.lblMessage.TabIndex = 91
        Me.lblMessage.Text = "Message"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmTrString
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 227)
        Me.Controls.Add(Me.txtTrString)
        Me.Controls.Add(Me.txtTrNo)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdRead)
        Me.Controls.Add(Me.lblMessage)
        Me.Name = "frmTrString"
        Me.Text = "frmTrString"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents txtTrString As TextBox
    Private WithEvents txtTrNo As TextBox
    Public WithEvents label2 As Label
    Public WithEvents label1 As Label
    Public WithEvents cmdWrite As Button
    Public WithEvents cmdRead As Button
    Public WithEvents lblMessage As Label
End Class
