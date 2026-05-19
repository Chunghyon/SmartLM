
Partial Class frmCommParam
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.btnSet = New System.Windows.Forms.Button()
        Me.btnGet = New System.Windows.Forms.Button()
        Me.label1 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label6 = New System.Windows.Forms.Label()
        Me.txtMachineID = New System.Windows.Forms.TextBox()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.txtServerIP = New System.Windows.Forms.TextBox()
        Me.txtServerPort = New System.Windows.Forms.TextBox()
        Me.chkUseDHCP = New System.Windows.Forms.CheckBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtSubnetMask = New System.Windows.Forms.TextBox()
        Me.label7 = New System.Windows.Forms.Label()
        Me.txtGateway = New System.Windows.Forms.TextBox()
        Me.label8 = New System.Windows.Forms.Label()
        Me.label9 = New System.Windows.Forms.Label()
        Me.cmbEventOutType = New System.Windows.Forms.ComboBox()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' txtIP
        ' 
        Me.txtIP.AcceptsReturn = True
        Me.txtIP.BackColor = System.Drawing.SystemColors.Window
        Me.txtIP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIP.Font = New System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtIP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIP.Location = New System.Drawing.Point(93, 93)
        Me.txtIP.MaxLength = 0
        Me.txtIP.Name = "txtIP"
        Me.txtIP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIP.Size = New System.Drawing.Size(113, 26)
        Me.txtIP.TabIndex = 33
        Me.txtIP.Text = "192.168.1.224"
        ' 
        ' btnSet
        ' 
        Me.btnSet.Location = New System.Drawing.Point(146, 357)
        Me.btnSet.Name = "btnSet"
        Me.btnSet.Size = New System.Drawing.Size(81, 33)
        Me.btnSet.TabIndex = 32
        Me.btnSet.Text = "Set"
        Me.btnSet.UseVisualStyleBackColor = True
        ' 
        ' btnGet
        ' 
        Me.btnGet.Location = New System.Drawing.Point(39, 357)
        Me.btnGet.Name = "btnGet"
        Me.btnGet.Size = New System.Drawing.Size(81, 33)
        Me.btnGet.TabIndex = 32
        Me.btnGet.Text = "Get"
        Me.btnGet.UseVisualStyleBackColor = True
        ' 
        ' label1
        ' 
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 27)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(59, 13)
        Me.label1.TabIndex = 34
        Me.label1.Text = "MachineID"
        ' 
        ' label2
        ' 
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(12, 97)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(17, 13)
        Me.label2.TabIndex = 34
        Me.label2.Text = "IP"
        ' 
        ' label3
        ' 
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(12, 128)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(26, 13)
        Me.label3.TabIndex = 34
        Me.label3.Text = "Port"
        ' 
        ' label4
        ' 
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(12, 267)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(51, 13)
        Me.label4.TabIndex = 34
        Me.label4.Text = "Server IP"
        ' 
        ' label5
        ' 
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(12, 297)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(60, 13)
        Me.label5.TabIndex = 34
        Me.label5.Text = "Server Port"
        ' 
        ' label6
        ' 
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(12, 68)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(56, 13)
        Me.label6.TabIndex = 34
        Me.label6.Text = "UseDHCP"
        ' 
        ' txtMachineID
        ' 
        Me.txtMachineID.AcceptsReturn = True
        Me.txtMachineID.BackColor = System.Drawing.SystemColors.Window
        Me.txtMachineID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMachineID.Font = New System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtMachineID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMachineID.Location = New System.Drawing.Point(93, 23)
        Me.txtMachineID.MaxLength = 0
        Me.txtMachineID.Name = "txtMachineID"
        Me.txtMachineID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMachineID.Size = New System.Drawing.Size(113, 26)
        Me.txtMachineID.TabIndex = 33
        Me.txtMachineID.Text = "1"
        ' 
        ' txtPort
        ' 
        Me.txtPort.AcceptsReturn = True
        Me.txtPort.BackColor = System.Drawing.SystemColors.Window
        Me.txtPort.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPort.Font = New System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtPort.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPort.Location = New System.Drawing.Point(93, 124)
        Me.txtPort.MaxLength = 0
        Me.txtPort.Name = "txtPort"
        Me.txtPort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPort.Size = New System.Drawing.Size(113, 26)
        Me.txtPort.TabIndex = 33
        Me.txtPort.Text = "5005"
        ' 
        ' txtServerIP
        ' 
        Me.txtServerIP.AcceptsReturn = True
        Me.txtServerIP.BackColor = System.Drawing.SystemColors.Window
        Me.txtServerIP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtServerIP.Font = New System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtServerIP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtServerIP.Location = New System.Drawing.Point(93, 263)
        Me.txtServerIP.MaxLength = 0
        Me.txtServerIP.Name = "txtServerIP"
        Me.txtServerIP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtServerIP.Size = New System.Drawing.Size(113, 26)
        Me.txtServerIP.TabIndex = 33
        Me.txtServerIP.Text = "192.168.1.200"
        ' 
        ' txtServerPort
        ' 
        Me.txtServerPort.AcceptsReturn = True
        Me.txtServerPort.BackColor = System.Drawing.SystemColors.Window
        Me.txtServerPort.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtServerPort.Font = New System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtServerPort.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtServerPort.Location = New System.Drawing.Point(93, 293)
        Me.txtServerPort.MaxLength = 0
        Me.txtServerPort.Name = "txtServerPort"
        Me.txtServerPort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtServerPort.Size = New System.Drawing.Size(113, 26)
        Me.txtServerPort.TabIndex = 33
        Me.txtServerPort.Text = "4000"
        ' 
        ' chkUseDHCP
        ' 
        Me.chkUseDHCP.AutoSize = True
        Me.chkUseDHCP.Location = New System.Drawing.Point(139, 70)
        Me.chkUseDHCP.Name = "chkUseDHCP"
        Me.chkUseDHCP.Size = New System.Drawing.Size(15, 14)
        Me.chkUseDHCP.TabIndex = 35
        Me.chkUseDHCP.UseVisualStyleBackColor = True
        ' 
        ' groupBox1
        ' 
        Me.groupBox1.Controls.Add(Me.cmbEventOutType)
        Me.groupBox1.Controls.Add(Me.chkUseDHCP)
        Me.groupBox1.Controls.Add(Me.label3)
        Me.groupBox1.Controls.Add(Me.label9)
        Me.groupBox1.Controls.Add(Me.label6)
        Me.groupBox1.Controls.Add(Me.label5)
        Me.groupBox1.Controls.Add(Me.label4)
        Me.groupBox1.Controls.Add(Me.label8)
        Me.groupBox1.Controls.Add(Me.label7)
        Me.groupBox1.Controls.Add(Me.label2)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.txtServerPort)
        Me.groupBox1.Controls.Add(Me.txtServerIP)
        Me.groupBox1.Controls.Add(Me.txtPort)
        Me.groupBox1.Controls.Add(Me.txtMachineID)
        Me.groupBox1.Controls.Add(Me.txtGateway)
        Me.groupBox1.Controls.Add(Me.txtSubnetMask)
        Me.groupBox1.Controls.Add(Me.txtIP)
        Me.groupBox1.Location = New System.Drawing.Point(22, 7)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(226, 333)
        Me.groupBox1.TabIndex = 36
        Me.groupBox1.TabStop = False
        ' 
        ' txtSubnetMask
        ' 
        Me.txtSubnetMask.AcceptsReturn = True
        Me.txtSubnetMask.BackColor = System.Drawing.SystemColors.Window
        Me.txtSubnetMask.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSubnetMask.Font = New System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtSubnetMask.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSubnetMask.Location = New System.Drawing.Point(93, 155)
        Me.txtSubnetMask.MaxLength = 0
        Me.txtSubnetMask.Name = "txtSubnetMask"
        Me.txtSubnetMask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubnetMask.Size = New System.Drawing.Size(113, 26)
        Me.txtSubnetMask.TabIndex = 33
        Me.txtSubnetMask.Text = "255.255.255.0"
        ' 
        ' label7
        ' 
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(12, 159)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(70, 13)
        Me.label7.TabIndex = 34
        Me.label7.Text = "Subnet Mask"
        ' 
        ' txtGateway
        ' 
        Me.txtGateway.AcceptsReturn = True
        Me.txtGateway.BackColor = System.Drawing.SystemColors.Window
        Me.txtGateway.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGateway.Font = New System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Me.txtGateway.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGateway.Location = New System.Drawing.Point(93, 186)
        Me.txtGateway.MaxLength = 0
        Me.txtGateway.Name = "txtGateway"
        Me.txtGateway.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGateway.Size = New System.Drawing.Size(113, 26)
        Me.txtGateway.TabIndex = 33
        Me.txtGateway.Text = "192.168.1.1"
        ' 
        ' label8
        ' 
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(12, 190)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(49, 13)
        Me.label8.TabIndex = 34
        Me.label8.Text = "Gateway"
        ' 
        ' label9
        ' 
        Me.label9.AutoSize = True
        Me.label9.Location = New System.Drawing.Point(12, 233)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(74, 13)
        Me.label9.TabIndex = 34
        Me.label9.Text = "Use EventOut"
        ' 
        ' cmbEventOutType
        ' 
        Me.cmbEventOutType.FormattingEnabled = True
        Me.cmbEventOutType.Location = New System.Drawing.Point(93, 231)
        Me.cmbEventOutType.Name = "cmbEventOutType"
        Me.cmbEventOutType.Size = New System.Drawing.Size(113, 21)
        Me.cmbEventOutType.TabIndex = 36
        ' 
        ' frmCommParam
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(269, 407)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.btnGet)
        Me.Controls.Add(Me.btnSet)
        Me.Name = "frmCommParam"
        Me.Text = "frmCommParam"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WithEvents txtIP As System.Windows.Forms.TextBox
    Private WithEvents btnSet As System.Windows.Forms.Button
    Private WithEvents btnGet As System.Windows.Forms.Button
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label6 As System.Windows.Forms.Label
    Public WithEvents txtMachineID As System.Windows.Forms.TextBox
    Public WithEvents txtPort As System.Windows.Forms.TextBox
    Public WithEvents txtServerIP As System.Windows.Forms.TextBox
    Public WithEvents txtServerPort As System.Windows.Forms.TextBox
    Private WithEvents chkUseDHCP As System.Windows.Forms.CheckBox
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents label9 As System.Windows.Forms.Label
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents label7 As System.Windows.Forms.Label
    Public WithEvents txtGateway As System.Windows.Forms.TextBox
    Public WithEvents txtSubnetMask As System.Windows.Forms.TextBox
    Private WithEvents cmbEventOutType As System.Windows.Forms.ComboBox
End Class
