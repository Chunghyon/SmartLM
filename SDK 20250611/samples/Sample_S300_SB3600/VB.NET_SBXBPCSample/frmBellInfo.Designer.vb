<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmBellInfo
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBellInfo))
		Me.txtBellPeriod = New System.Windows.Forms.TextBox()
		Me.txtBellCount = New System.Windows.Forms.TextBox()
		Me.label3 = New System.Windows.Forms.Label()
		Me.chkUsed = New System.Windows.Forms.CheckBox()
		Me.label1 = New System.Windows.Forms.Label()
		Me.label2 = New System.Windows.Forms.Label()
		Me.cmbWeekday = New System.Windows.Forms.ComboBox()
		Me.lstTimeZone = New System.Windows.Forms.ListBox()
		Me.dtStart = New System.Windows.Forms.DateTimePicker()
		Me.cmdUpdate = New System.Windows.Forms.Button()
		Me.lblEnrollData = New System.Windows.Forms.Label()
		Me.cmdExit = New System.Windows.Forms.Button()
		Me.cmdWrite = New System.Windows.Forms.Button()
		Me.cmdRead = New System.Windows.Forms.Button()
		Me.lblMessage = New System.Windows.Forms.Label()
		Me.SuspendLayout()
		'
		'txtBellPeriod
		'
		Me.txtBellPeriod.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtBellPeriod.Location = New System.Drawing.Point(370, 51)
		Me.txtBellPeriod.Name = "txtBellPeriod"
		Me.txtBellPeriod.Size = New System.Drawing.Size(100, 26)
		Me.txtBellPeriod.TabIndex = 104
		Me.txtBellPeriod.Text = "1"
		'
		'txtBellCount
		'
		Me.txtBellCount.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtBellCount.Location = New System.Drawing.Point(131, 52)
		Me.txtBellCount.Name = "txtBellCount"
		Me.txtBellCount.Size = New System.Drawing.Size(100, 26)
		Me.txtBellCount.TabIndex = 105
		Me.txtBellCount.Text = "0"
		'
		'label3
		'
		Me.label3.AutoSize = True
		Me.label3.BackColor = System.Drawing.SystemColors.Control
		Me.label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.label3.Location = New System.Drawing.Point(274, 53)
		Me.label3.Name = "label3"
		Me.label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.label3.Size = New System.Drawing.Size(79, 19)
		Me.label3.TabIndex = 100
		Me.label3.Text = "Bell Period:"
		'
		'chkUsed
		'
		Me.chkUsed.AutoSize = True
		Me.chkUsed.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkUsed.Location = New System.Drawing.Point(39, 110)
		Me.chkUsed.Name = "chkUsed"
		Me.chkUsed.Size = New System.Drawing.Size(60, 23)
		Me.chkUsed.TabIndex = 103
		Me.chkUsed.Text = "Used"
		Me.chkUsed.UseVisualStyleBackColor = True
		'
		'label1
		'
		Me.label1.AutoSize = True
		Me.label1.BackColor = System.Drawing.SystemColors.Control
		Me.label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.label1.Location = New System.Drawing.Point(35, 54)
		Me.label1.Name = "label1"
		Me.label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.label1.Size = New System.Drawing.Size(76, 19)
		Me.label1.TabIndex = 101
		Me.label1.Text = "Bell Count:"
		'
		'label2
		'
		Me.label2.AutoSize = True
		Me.label2.BackColor = System.Drawing.SystemColors.Control
		Me.label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.label2.Location = New System.Drawing.Point(377, 86)
		Me.label2.Name = "label2"
		Me.label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.label2.Size = New System.Drawing.Size(70, 19)
		Me.label2.TabIndex = 102
		Me.label2.Text = "Weekday:"
		'
		'cmbWeekday
		'
		Me.cmbWeekday.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbWeekday.Font = New System.Drawing.Font("Times New Roman", 12.0!)
		Me.cmbWeekday.FormattingEnabled = True
		Me.cmbWeekday.Items.AddRange(New Object() {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Everyday"})
		Me.cmbWeekday.Location = New System.Drawing.Point(381, 110)
		Me.cmbWeekday.Name = "cmbWeekday"
		Me.cmbWeekday.Size = New System.Drawing.Size(154, 27)
		Me.cmbWeekday.TabIndex = 99
		'
		'lstTimeZone
		'
		Me.lstTimeZone.Font = New System.Drawing.Font("Times New Roman", 12.0!)
		Me.lstTimeZone.FormattingEnabled = True
		Me.lstTimeZone.ItemHeight = 19
		Me.lstTimeZone.Location = New System.Drawing.Point(39, 143)
		Me.lstTimeZone.Name = "lstTimeZone"
		Me.lstTimeZone.Size = New System.Drawing.Size(501, 384)
		Me.lstTimeZone.TabIndex = 98
		'
		'dtStart
		'
		Me.dtStart.CalendarFont = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
		Me.dtStart.CustomFormat = "hh:mm:ss"
		Me.dtStart.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right
		Me.dtStart.Font = New System.Drawing.Font("Times New Roman", 12.0!)
		Me.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Time
		Me.dtStart.Location = New System.Drawing.Point(180, 111)
		Me.dtStart.Name = "dtStart"
		Me.dtStart.Size = New System.Drawing.Size(119, 26)
		Me.dtStart.TabIndex = 97
		Me.dtStart.Value = New Date(2011, 10, 12, 10, 44, 0, 0)
		'
		'cmdUpdate
		'
		Me.cmdUpdate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdUpdate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdUpdate.Location = New System.Drawing.Point(563, 144)
		Me.cmdUpdate.Name = "cmdUpdate"
		Me.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdUpdate.Size = New System.Drawing.Size(104, 46)
		Me.cmdUpdate.TabIndex = 96
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
		Me.lblEnrollData.Location = New System.Drawing.Point(176, 86)
		Me.lblEnrollData.Name = "lblEnrollData"
		Me.lblEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEnrollData.Size = New System.Drawing.Size(41, 19)
		Me.lblEnrollData.TabIndex = 95
		Me.lblEnrollData.Text = "Time:"
		'
		'cmdExit
		'
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(563, 490)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(99, 34)
		Me.cmdExit.TabIndex = 93
		Me.cmdExit.Text = "Exit"
		Me.cmdExit.UseVisualStyleBackColor = False
		'
		'cmdWrite
		'
		Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
		Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdWrite.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdWrite.Location = New System.Drawing.Point(563, 450)
		Me.cmdWrite.Name = "cmdWrite"
		Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdWrite.Size = New System.Drawing.Size(99, 34)
		Me.cmdWrite.TabIndex = 92
		Me.cmdWrite.Text = "Write"
		Me.cmdWrite.UseVisualStyleBackColor = False
		'
		'cmdRead
		'
		Me.cmdRead.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRead.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRead.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdRead.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRead.Location = New System.Drawing.Point(563, 410)
		Me.cmdRead.Name = "cmdRead"
		Me.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRead.Size = New System.Drawing.Size(99, 34)
		Me.cmdRead.TabIndex = 91
		Me.cmdRead.Text = "Read"
		Me.cmdRead.UseVisualStyleBackColor = False
		'
		'lblMessage
		'
		Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
		Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMessage.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMessage.Location = New System.Drawing.Point(19, 17)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMessage.Size = New System.Drawing.Size(654, 28)
		Me.lblMessage.TabIndex = 94
		Me.lblMessage.Text = "Message"
		Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter
		'
		'frmBellInfo
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(690, 540)
		Me.Controls.Add(Me.txtBellPeriod)
		Me.Controls.Add(Me.txtBellCount)
		Me.Controls.Add(Me.label3)
		Me.Controls.Add(Me.chkUsed)
		Me.Controls.Add(Me.label1)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.cmbWeekday)
		Me.Controls.Add(Me.lstTimeZone)
		Me.Controls.Add(Me.dtStart)
		Me.Controls.Add(Me.cmdUpdate)
		Me.Controls.Add(Me.lblEnrollData)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.cmdWrite)
		Me.Controls.Add(Me.cmdRead)
		Me.Controls.Add(Me.lblMessage)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Location = New System.Drawing.Point(4, 30)
		Me.Name = "frmBellInfo"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Setting Bell Time"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Private WithEvents txtBellPeriod As TextBox
    Private WithEvents txtBellCount As TextBox
    Public WithEvents label3 As Label
    Private WithEvents chkUsed As CheckBox
    Public WithEvents label1 As Label
    Public WithEvents label2 As Label
    Private WithEvents cmbWeekday As ComboBox
    Private WithEvents lstTimeZone As ListBox
    Private WithEvents dtStart As DateTimePicker
    Public WithEvents cmdUpdate As Button
    Public WithEvents lblEnrollData As Label
    Public WithEvents cmdExit As Button
    Public WithEvents cmdWrite As Button
    Public WithEvents cmdRead As Button
    Public WithEvents lblMessage As Label
#End Region
End Class