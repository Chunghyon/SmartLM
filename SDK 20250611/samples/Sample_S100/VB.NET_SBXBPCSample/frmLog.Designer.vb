<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmLog
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
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents chkAndDelete As System.Windows.Forms.CheckBox
	Public WithEvents cmdEmptySLog As System.Windows.Forms.Button
	Public WithEvents cmdEmptyGLog As System.Windows.Forms.Button
	Public WithEvents chkReadMark As System.Windows.Forms.CheckBox
	Public WithEvents cmdAllGLogData As System.Windows.Forms.Button
	Public WithEvents cmdAllSLogData As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdGlogData As System.Windows.Forms.Button
	Public WithEvents cmdSLogData As System.Windows.Forms.Button
	Public WithEvents gridSLogData As AxMSFlexGridLib.AxMSFlexGrid
	Public WithEvents gridSLogData1 As AxMSFlexGridLib.AxMSFlexGrid
	Public WithEvents gridSLogData2 As AxMSFlexGridLib.AxMSFlexGrid
	Public WithEvents lblEnrollData As System.Windows.Forms.Label
	Public WithEvents LabelTotal As System.Windows.Forms.Label
	Public WithEvents lblMessage As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLog))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkAndDelete = New System.Windows.Forms.CheckBox
        Me.cmdEmptySLog = New System.Windows.Forms.Button
        Me.cmdEmptyGLog = New System.Windows.Forms.Button
        Me.chkReadMark = New System.Windows.Forms.CheckBox
        Me.cmdAllGLogData = New System.Windows.Forms.Button
        Me.cmdAllSLogData = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdGlogData = New System.Windows.Forms.Button
        Me.cmdSLogData = New System.Windows.Forms.Button
        Me.gridSLogData = New AxMSFlexGridLib.AxMSFlexGrid
        Me.gridSLogData1 = New AxMSFlexGridLib.AxMSFlexGrid
        Me.gridSLogData2 = New AxMSFlexGridLib.AxMSFlexGrid
        Me.lblEnrollData = New System.Windows.Forms.Label
        Me.LabelTotal = New System.Windows.Forms.Label
        Me.lblMessage = New System.Windows.Forms.Label
        CType(Me.gridSLogData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridSLogData1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridSLogData2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkAndDelete
        '
        Me.chkAndDelete.BackColor = System.Drawing.SystemColors.Control
        Me.chkAndDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAndDelete.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAndDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAndDelete.Location = New System.Drawing.Point(479, 81)
        Me.chkAndDelete.Name = "chkAndDelete"
        Me.chkAndDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAndDelete.Size = New System.Drawing.Size(110, 19)
        Me.chkAndDelete.TabIndex = 14
        Me.chkAndDelete.Text = "and Delete "
        Me.chkAndDelete.UseVisualStyleBackColor = False
        '
        'cmdEmptySLog
        '
        Me.cmdEmptySLog.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEmptySLog.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEmptySLog.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEmptySLog.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEmptySLog.Location = New System.Drawing.Point(220, 426)
        Me.cmdEmptySLog.Name = "cmdEmptySLog"
        Me.cmdEmptySLog.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEmptySLog.Size = New System.Drawing.Size(94, 43)
        Me.cmdEmptySLog.TabIndex = 11
        Me.cmdEmptySLog.Text = "Empty SLogData"
        Me.cmdEmptySLog.UseVisualStyleBackColor = False
        '
        'cmdEmptyGLog
        '
        Me.cmdEmptyGLog.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEmptyGLog.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEmptyGLog.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEmptyGLog.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEmptyGLog.Location = New System.Drawing.Point(513, 426)
        Me.cmdEmptyGLog.Name = "cmdEmptyGLog"
        Me.cmdEmptyGLog.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEmptyGLog.Size = New System.Drawing.Size(94, 43)
        Me.cmdEmptyGLog.TabIndex = 10
        Me.cmdEmptyGLog.Text = "Empty GLogData"
        Me.cmdEmptyGLog.UseVisualStyleBackColor = False
        '
        'chkReadMark
        '
        Me.chkReadMark.BackColor = System.Drawing.SystemColors.Control
        Me.chkReadMark.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReadMark.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReadMark.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReadMark.Location = New System.Drawing.Point(606, 83)
        Me.chkReadMark.Name = "chkReadMark"
        Me.chkReadMark.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReadMark.Size = New System.Drawing.Size(102, 19)
        Me.chkReadMark.TabIndex = 7
        Me.chkReadMark.Text = "ReadMark"
        Me.chkReadMark.UseVisualStyleBackColor = False
        '
        'cmdAllGLogData
        '
        Me.cmdAllGLogData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAllGLogData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAllGLogData.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAllGLogData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAllGLogData.Location = New System.Drawing.Point(415, 426)
        Me.cmdAllGLogData.Name = "cmdAllGLogData"
        Me.cmdAllGLogData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAllGLogData.Size = New System.Drawing.Size(94, 43)
        Me.cmdAllGLogData.TabIndex = 6
        Me.cmdAllGLogData.Text = "Read All GLogData"
        Me.cmdAllGLogData.UseVisualStyleBackColor = False
        '
        'cmdAllSLogData
        '
        Me.cmdAllSLogData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAllSLogData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAllSLogData.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAllSLogData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAllSLogData.Location = New System.Drawing.Point(122, 426)
        Me.cmdAllSLogData.Name = "cmdAllSLogData"
        Me.cmdAllSLogData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAllSLogData.Size = New System.Drawing.Size(94, 43)
        Me.cmdAllSLogData.TabIndex = 4
        Me.cmdAllSLogData.Text = "Read All SLogData"
        Me.cmdAllSLogData.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(611, 426)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(94, 43)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdGlogData
        '
        Me.cmdGlogData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGlogData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGlogData.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGlogData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGlogData.Location = New System.Drawing.Point(317, 426)
        Me.cmdGlogData.Name = "cmdGlogData"
        Me.cmdGlogData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGlogData.Size = New System.Drawing.Size(94, 43)
        Me.cmdGlogData.TabIndex = 2
        Me.cmdGlogData.Text = "Read GLogData"
        Me.cmdGlogData.UseVisualStyleBackColor = False
        '
        'cmdSLogData
        '
        Me.cmdSLogData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSLogData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSLogData.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSLogData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSLogData.Location = New System.Drawing.Point(24, 426)
        Me.cmdSLogData.Name = "cmdSLogData"
        Me.cmdSLogData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSLogData.Size = New System.Drawing.Size(94, 43)
        Me.cmdSLogData.TabIndex = 1
        Me.cmdSLogData.Text = "Read SLogData"
        Me.cmdSLogData.UseVisualStyleBackColor = False
        '
        'gridSLogData
        '
        Me.gridSLogData.Location = New System.Drawing.Point(24, 104)
        Me.gridSLogData.Name = "gridSLogData"
        Me.gridSLogData.OcxState = CType(resources.GetObject("gridSLogData.OcxState"), System.Windows.Forms.AxHost.State)
        Me.gridSLogData.Size = New System.Drawing.Size(679, 320)
        Me.gridSLogData.TabIndex = 5
        '
        'gridSLogData1
        '
        Me.gridSLogData1.Location = New System.Drawing.Point(24, 211)
        Me.gridSLogData1.Name = "gridSLogData1"
        Me.gridSLogData1.OcxState = CType(resources.GetObject("gridSLogData1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.gridSLogData1.Size = New System.Drawing.Size(679, 214)
        Me.gridSLogData1.TabIndex = 12
        '
        'gridSLogData2
        '
        Me.gridSLogData2.Location = New System.Drawing.Point(24, 318)
        Me.gridSLogData2.Name = "gridSLogData2"
        Me.gridSLogData2.OcxState = CType(resources.GetObject("gridSLogData2.OcxState"), System.Windows.Forms.AxHost.State)
        Me.gridSLogData2.Size = New System.Drawing.Size(679, 107)
        Me.gridSLogData2.TabIndex = 13
        '
        'lblEnrollData
        '
        Me.lblEnrollData.AutoSize = True
        Me.lblEnrollData.BackColor = System.Drawing.SystemColors.Control
        Me.lblEnrollData.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEnrollData.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEnrollData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEnrollData.Location = New System.Drawing.Point(27, 86)
        Me.lblEnrollData.Name = "lblEnrollData"
        Me.lblEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEnrollData.Size = New System.Drawing.Size(73, 19)
        Me.lblEnrollData.TabIndex = 9
        Me.lblEnrollData.Text = "Log Data :"
        '
        'LabelTotal
        '
        Me.LabelTotal.AutoSize = True
        Me.LabelTotal.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTotal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTotal.Location = New System.Drawing.Point(128, 86)
        Me.LabelTotal.Name = "LabelTotal"
        Me.LabelTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTotal.Size = New System.Drawing.Size(47, 19)
        Me.LabelTotal.TabIndex = 8
        Me.LabelTotal.Text = "Total :"
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
        Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMessage.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMessage.Location = New System.Drawing.Point(24, 40)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMessage.Size = New System.Drawing.Size(679, 28)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "Message"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmLog
        '
        Me.AcceptButton = Me.cmdExit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(725, 482)
        Me.Controls.Add(Me.chkAndDelete)
        Me.Controls.Add(Me.cmdEmptySLog)
        Me.Controls.Add(Me.cmdEmptyGLog)
        Me.Controls.Add(Me.chkReadMark)
        Me.Controls.Add(Me.cmdAllGLogData)
        Me.Controls.Add(Me.cmdAllSLogData)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdGlogData)
        Me.Controls.Add(Me.cmdSLogData)
        Me.Controls.Add(Me.gridSLogData)
        Me.Controls.Add(Me.gridSLogData1)
        Me.Controls.Add(Me.gridSLogData2)
        Me.Controls.Add(Me.lblEnrollData)
        Me.Controls.Add(Me.LabelTotal)
        Me.Controls.Add(Me.lblMessage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(321, 209)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLog"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Manage Log Data"
        CType(Me.gridSLogData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridSLogData1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridSLogData2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
End Class