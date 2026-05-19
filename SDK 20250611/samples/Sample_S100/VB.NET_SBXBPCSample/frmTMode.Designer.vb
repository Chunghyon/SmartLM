<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTMode
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
        Me.cmbDoorMode = New System.Windows.Forms.ComboBox
        Me.label2 = New System.Windows.Forms.Label
        Me.lstTMode = New System.Windows.Forms.ListBox
        Me.dtEnd = New System.Windows.Forms.DateTimePicker
        Me.label1 = New System.Windows.Forms.Label
        Me.dtStart = New System.Windows.Forms.DateTimePicker
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdWrite = New System.Windows.Forms.Button
        Me.cmdRead = New System.Windows.Forms.Button
        Me.cmdUpdate = New System.Windows.Forms.Button
        Me.lblEnrollData = New System.Windows.Forms.Label
        Me.lblMessage = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmbDoorMode
        '
        Me.cmbDoorMode.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cmbDoorMode.FormattingEnabled = True
        Me.cmbDoorMode.Items.AddRange(New Object() {"Any", "FP&CD", "FP&PWD", "CD&PWD", "FP&CD&PWD"})
        Me.cmbDoorMode.Location = New System.Drawing.Point(220, 95)
        Me.cmbDoorMode.Name = "cmbDoorMode"
        Me.cmbDoorMode.Size = New System.Drawing.Size(226, 27)
        Me.cmbDoorMode.TabIndex = 116
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.BackColor = System.Drawing.SystemColors.Control
        Me.label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.label2.Location = New System.Drawing.Point(215, 65)
        Me.label2.Name = "label2"
        Me.label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.label2.Size = New System.Drawing.Size(85, 19)
        Me.label2.TabIndex = 115
        Me.label2.Text = "Door Mode:"
        '
        'lstTMode
        '
        Me.lstTMode.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.lstTMode.FormattingEnabled = True
        Me.lstTMode.ItemHeight = 19
        Me.lstTMode.Location = New System.Drawing.Point(18, 133)
        Me.lstTMode.Name = "lstTMode"
        Me.lstTMode.Size = New System.Drawing.Size(501, 384)
        Me.lstTMode.TabIndex = 114
        '
        'dtEnd
        '
        Me.dtEnd.CalendarFont = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.dtEnd.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtEnd.Location = New System.Drawing.Point(69, 94)
        Me.dtEnd.Name = "dtEnd"
        Me.dtEnd.Size = New System.Drawing.Size(119, 26)
        Me.dtEnd.TabIndex = 113
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.BackColor = System.Drawing.SystemColors.Control
        Me.label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.label1.Location = New System.Drawing.Point(14, 94)
        Me.label1.Name = "label1"
        Me.label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.label1.Size = New System.Drawing.Size(36, 19)
        Me.label1.TabIndex = 112
        Me.label1.Text = "End:"
        '
        'dtStart
        '
        Me.dtStart.CalendarFont = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.dtStart.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtStart.Location = New System.Drawing.Point(69, 65)
        Me.dtStart.Name = "dtStart"
        Me.dtStart.Size = New System.Drawing.Size(119, 26)
        Me.dtStart.TabIndex = 111
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(542, 472)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(104, 46)
        Me.cmdExit.TabIndex = 110
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdWrite
        '
        Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWrite.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWrite.Location = New System.Drawing.Point(542, 408)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWrite.Size = New System.Drawing.Size(104, 46)
        Me.cmdWrite.TabIndex = 109
        Me.cmdWrite.Text = "Write"
        Me.cmdWrite.UseVisualStyleBackColor = False
        '
        'cmdRead
        '
        Me.cmdRead.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRead.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRead.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRead.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRead.Location = New System.Drawing.Point(542, 343)
        Me.cmdRead.Name = "cmdRead"
        Me.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRead.Size = New System.Drawing.Size(104, 46)
        Me.cmdRead.TabIndex = 108
        Me.cmdRead.Text = "Read"
        Me.cmdRead.UseVisualStyleBackColor = False
        '
        'cmdUpdate
        '
        Me.cmdUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUpdate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpdate.Location = New System.Drawing.Point(542, 134)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpdate.Size = New System.Drawing.Size(104, 46)
        Me.cmdUpdate.TabIndex = 107
        Me.cmdUpdate.Text = "Update"
        Me.cmdUpdate.UseVisualStyleBackColor = False
        '
        'lblEnrollData
        '
        Me.lblEnrollData.AutoSize = True
        Me.lblEnrollData.BackColor = System.Drawing.SystemColors.Control
        Me.lblEnrollData.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEnrollData.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEnrollData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEnrollData.Location = New System.Drawing.Point(14, 65)
        Me.lblEnrollData.Name = "lblEnrollData"
        Me.lblEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEnrollData.Size = New System.Drawing.Size(41, 19)
        Me.lblEnrollData.TabIndex = 106
        Me.lblEnrollData.Text = "Start:"
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
        Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMessage.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMessage.Location = New System.Drawing.Point(14, 16)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMessage.Size = New System.Drawing.Size(632, 28)
        Me.lblMessage.TabIndex = 105
        Me.lblMessage.Text = "Message"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmTMode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(660, 534)
        Me.Controls.Add(Me.cmbDoorMode)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.lstTMode)
        Me.Controls.Add(Me.dtEnd)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.dtStart)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdRead)
        Me.Controls.Add(Me.cmdUpdate)
        Me.Controls.Add(Me.lblEnrollData)
        Me.Controls.Add(Me.lblMessage)
        Me.Name = "frmTMode"
        Me.Text = "frmTMode"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents cmbDoorMode As System.Windows.Forms.ComboBox
    Public WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents lstTMode As System.Windows.Forms.ListBox
    Private WithEvents dtEnd As System.Windows.Forms.DateTimePicker
    Public WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents dtStart As System.Windows.Forms.DateTimePicker
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdWrite As System.Windows.Forms.Button
    Public WithEvents cmdRead As System.Windows.Forms.Button
    Public WithEvents cmdUpdate As System.Windows.Forms.Button
    Public WithEvents lblEnrollData As System.Windows.Forms.Label
    Public WithEvents lblMessage As System.Windows.Forms.Label
End Class
