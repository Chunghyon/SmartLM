<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHoliday
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
        Me.txtDays = New System.Windows.Forms.TextBox
        Me.label1 = New System.Windows.Forms.Label
        Me.lstHoliday = New System.Windows.Forms.ListBox
        Me.dtHoliday = New System.Windows.Forms.DateTimePicker
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdWrite = New System.Windows.Forms.Button
        Me.cmdRead = New System.Windows.Forms.Button
        Me.cmdUpdate = New System.Windows.Forms.Button
        Me.lblMessage = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtDays
        '
        Me.txtDays.AcceptsReturn = True
        Me.txtDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDays.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDays.Location = New System.Drawing.Point(209, 60)
        Me.txtDays.MaxLength = 0
        Me.txtDays.Name = "txtDays"
        Me.txtDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDays.Size = New System.Drawing.Size(113, 26)
        Me.txtDays.TabIndex = 113
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.label1.Location = New System.Drawing.Point(151, 62)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(52, 19)
        Me.label1.TabIndex = 112
        Me.label1.Text = "Period:"
        '
        'lstHoliday
        '
        Me.lstHoliday.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.lstHoliday.FormattingEnabled = True
        Me.lstHoliday.ItemHeight = 19
        Me.lstHoliday.Location = New System.Drawing.Point(12, 97)
        Me.lstHoliday.Name = "lstHoliday"
        Me.lstHoliday.Size = New System.Drawing.Size(501, 384)
        Me.lstHoliday.TabIndex = 111
        '
        'dtHoliday
        '
        Me.dtHoliday.CalendarFont = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.dtHoliday.CustomFormat = "MM-dd"
        Me.dtHoliday.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.dtHoliday.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtHoliday.Location = New System.Drawing.Point(12, 56)
        Me.dtHoliday.Name = "dtHoliday"
        Me.dtHoliday.Size = New System.Drawing.Size(97, 26)
        Me.dtHoliday.TabIndex = 110
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(536, 436)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(104, 46)
        Me.cmdExit.TabIndex = 109
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdWrite
        '
        Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWrite.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWrite.Location = New System.Drawing.Point(536, 372)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWrite.Size = New System.Drawing.Size(104, 46)
        Me.cmdWrite.TabIndex = 108
        Me.cmdWrite.Text = "Write"
        Me.cmdWrite.UseVisualStyleBackColor = False
        '
        'cmdRead
        '
        Me.cmdRead.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRead.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRead.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRead.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRead.Location = New System.Drawing.Point(536, 307)
        Me.cmdRead.Name = "cmdRead"
        Me.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRead.Size = New System.Drawing.Size(104, 46)
        Me.cmdRead.TabIndex = 107
        Me.cmdRead.Text = "Read"
        Me.cmdRead.UseVisualStyleBackColor = False
        '
        'cmdUpdate
        '
        Me.cmdUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUpdate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpdate.Location = New System.Drawing.Point(536, 98)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpdate.Size = New System.Drawing.Size(104, 46)
        Me.cmdUpdate.TabIndex = 106
        Me.cmdUpdate.Text = "Update"
        Me.cmdUpdate.UseVisualStyleBackColor = False
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
        Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMessage.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMessage.Location = New System.Drawing.Point(12, 16)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMessage.Size = New System.Drawing.Size(632, 28)
        Me.lblMessage.TabIndex = 105
        Me.lblMessage.Text = "Message"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmHoliday
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(653, 499)
        Me.Controls.Add(Me.txtDays)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.lstHoliday)
        Me.Controls.Add(Me.dtHoliday)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdRead)
        Me.Controls.Add(Me.cmdUpdate)
        Me.Controls.Add(Me.lblMessage)
        Me.Name = "frmHoliday"
        Me.Text = "frmHoliday"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtDays As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents lstHoliday As System.Windows.Forms.ListBox
    Private WithEvents dtHoliday As System.Windows.Forms.DateTimePicker
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdWrite As System.Windows.Forms.Button
    Public WithEvents cmdRead As System.Windows.Forms.Button
    Public WithEvents cmdUpdate As System.Windows.Forms.Button
    Public WithEvents lblMessage As System.Windows.Forms.Label
End Class
