<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNetworkSetting
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
        Me.groupBox3 = New System.Windows.Forms.GroupBox()
        Me.chkEther_ManualDNS = New System.Windows.Forms.CheckBox()
        Me.chkEther_DHCP = New System.Windows.Forms.CheckBox()
        Me.btnGetEthernetSetting = New System.Windows.Forms.Button()
        Me.label9 = New System.Windows.Forms.Label()
        Me.label10 = New System.Windows.Forms.Label()
        Me.label11 = New System.Windows.Forms.Label()
        Me.label14 = New System.Windows.Forms.Label()
        Me.label15 = New System.Windows.Forms.Label()
        Me.btnSetEthernetSetting = New System.Windows.Forms.Button()
        Me.txtEther_SecondaryDNSServer = New System.Windows.Forms.TextBox()
        Me.txtEther_DefaultGateway = New System.Windows.Forms.TextBox()
        Me.txtEther_PrimaryDNSServer = New System.Windows.Forms.TextBox()
        Me.txtEther_Subnet = New System.Windows.Forms.TextBox()
        Me.txtEther_IP = New System.Windows.Forms.TextBox()
        Me.groupBox4 = New System.Windows.Forms.GroupBox()
        Me.label12 = New System.Windows.Forms.Label()
        Me.label13 = New System.Windows.Forms.Label()
        Me.label8 = New System.Windows.Forms.Label()
        Me.btnGetCommSetting = New System.Windows.Forms.Button()
        Me.label17 = New System.Windows.Forms.Label()
        Me.label16 = New System.Windows.Forms.Label()
        Me.btnSetCommSetting = New System.Windows.Forms.Button()
        Me.txtP2P_Port = New System.Windows.Forms.TextBox()
        Me.txtP2P_Server = New System.Windows.Forms.TextBox()
        Me.txtDeviceID = New System.Windows.Forms.TextBox()
        Me.txtCommPwd = New System.Windows.Forms.TextBox()
        Me.txtTcpPort = New System.Windows.Forms.TextBox()
        Me.btnApplyCommSetting = New System.Windows.Forms.Button()
        Me.label7 = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label6 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.chkWiFi_ManualDNS = New System.Windows.Forms.CheckBox()
        Me.chkWiFi_DHCP = New System.Windows.Forms.CheckBox()
        Me.btnGetWiFiSetting = New System.Windows.Forms.Button()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.btnSetWiFiSetting = New System.Windows.Forms.Button()
        Me.txtWiFi_SecondaryDNSServer = New System.Windows.Forms.TextBox()
        Me.txtWiFi_DefaultGateway = New System.Windows.Forms.TextBox()
        Me.txtWiFi_PrimaryDNSServer = New System.Windows.Forms.TextBox()
        Me.txtWiFi_Subnet = New System.Windows.Forms.TextBox()
        Me.txtWiFi_IP = New System.Windows.Forms.TextBox()
        Me.txtWiFi_Key = New System.Windows.Forms.TextBox()
        Me.txtWiFi_SSID = New System.Windows.Forms.TextBox()
        Me.groupBox3.SuspendLayout()
        Me.groupBox4.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox3
        '
        Me.groupBox3.Controls.Add(Me.chkEther_ManualDNS)
        Me.groupBox3.Controls.Add(Me.chkEther_DHCP)
        Me.groupBox3.Controls.Add(Me.btnGetEthernetSetting)
        Me.groupBox3.Controls.Add(Me.label9)
        Me.groupBox3.Controls.Add(Me.label10)
        Me.groupBox3.Controls.Add(Me.label11)
        Me.groupBox3.Controls.Add(Me.label14)
        Me.groupBox3.Controls.Add(Me.label15)
        Me.groupBox3.Controls.Add(Me.btnSetEthernetSetting)
        Me.groupBox3.Controls.Add(Me.txtEther_SecondaryDNSServer)
        Me.groupBox3.Controls.Add(Me.txtEther_DefaultGateway)
        Me.groupBox3.Controls.Add(Me.txtEther_PrimaryDNSServer)
        Me.groupBox3.Controls.Add(Me.txtEther_Subnet)
        Me.groupBox3.Controls.Add(Me.txtEther_IP)
        Me.groupBox3.Location = New System.Drawing.Point(26, 16)
        Me.groupBox3.Margin = New System.Windows.Forms.Padding(2)
        Me.groupBox3.Name = "groupBox3"
        Me.groupBox3.Padding = New System.Windows.Forms.Padding(2)
        Me.groupBox3.Size = New System.Drawing.Size(373, 268)
        Me.groupBox3.TabIndex = 44
        Me.groupBox3.TabStop = False
        Me.groupBox3.Text = "Ethernet Setting"
        '
        'chkEther_ManualDNS
        '
        Me.chkEther_ManualDNS.AutoSize = True
        Me.chkEther_ManualDNS.Checked = True
        Me.chkEther_ManualDNS.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEther_ManualDNS.Location = New System.Drawing.Point(203, 137)
        Me.chkEther_ManualDNS.Margin = New System.Windows.Forms.Padding(2)
        Me.chkEther_ManualDNS.Name = "chkEther_ManualDNS"
        Me.chkEther_ManualDNS.Size = New System.Drawing.Size(103, 20)
        Me.chkEther_ManualDNS.TabIndex = 40
        Me.chkEther_ManualDNS.Text = "Manual DNS"
        Me.chkEther_ManualDNS.UseVisualStyleBackColor = True
        '
        'chkEther_DHCP
        '
        Me.chkEther_DHCP.AutoSize = True
        Me.chkEther_DHCP.Location = New System.Drawing.Point(203, 20)
        Me.chkEther_DHCP.Margin = New System.Windows.Forms.Padding(2)
        Me.chkEther_DHCP.Name = "chkEther_DHCP"
        Me.chkEther_DHCP.Size = New System.Drawing.Size(65, 20)
        Me.chkEther_DHCP.TabIndex = 40
        Me.chkEther_DHCP.Text = "DHCP"
        Me.chkEther_DHCP.UseVisualStyleBackColor = True
        '
        'btnGetEthernetSetting
        '
        Me.btnGetEthernetSetting.Location = New System.Drawing.Point(64, 231)
        Me.btnGetEthernetSetting.Name = "btnGetEthernetSetting"
        Me.btnGetEthernetSetting.Size = New System.Drawing.Size(81, 24)
        Me.btnGetEthernetSetting.TabIndex = 36
        Me.btnGetEthernetSetting.Text = "Get"
        Me.btnGetEthernetSetting.UseVisualStyleBackColor = True
        '
        'label9
        '
        Me.label9.AutoSize = True
        Me.label9.Location = New System.Drawing.Point(12, 197)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(152, 16)
        Me.label9.TabIndex = 34
        Me.label9.Text = "Secondary DNS Server:"
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(55, 110)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(109, 16)
        Me.label10.TabIndex = 34
        Me.label10.Text = "Default Gateway:"
        '
        'label11
        '
        Me.label11.AutoSize = True
        Me.label11.Location = New System.Drawing.Point(32, 166)
        Me.label11.Name = "label11"
        Me.label11.Size = New System.Drawing.Size(132, 16)
        Me.label11.TabIndex = 34
        Me.label11.Text = "Primary DNS Server:"
        '
        'label14
        '
        Me.label14.AutoSize = True
        Me.label14.Location = New System.Drawing.Point(75, 79)
        Me.label14.Name = "label14"
        Me.label14.Size = New System.Drawing.Size(89, 16)
        Me.label14.TabIndex = 34
        Me.label14.Text = "Subnet Mask:"
        '
        'label15
        '
        Me.label15.AutoSize = True
        Me.label15.Location = New System.Drawing.Point(141, 48)
        Me.label15.Name = "label15"
        Me.label15.Size = New System.Drawing.Size(23, 16)
        Me.label15.TabIndex = 34
        Me.label15.Text = "IP:"
        '
        'btnSetEthernetSetting
        '
        Me.btnSetEthernetSetting.Location = New System.Drawing.Point(176, 231)
        Me.btnSetEthernetSetting.Name = "btnSetEthernetSetting"
        Me.btnSetEthernetSetting.Size = New System.Drawing.Size(81, 24)
        Me.btnSetEthernetSetting.TabIndex = 37
        Me.btnSetEthernetSetting.Text = "Set"
        Me.btnSetEthernetSetting.UseVisualStyleBackColor = True
        '
        'txtEther_SecondaryDNSServer
        '
        Me.txtEther_SecondaryDNSServer.AcceptsReturn = True
        Me.txtEther_SecondaryDNSServer.BackColor = System.Drawing.SystemColors.Window
        Me.txtEther_SecondaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEther_SecondaryDNSServer.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEther_SecondaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEther_SecondaryDNSServer.Location = New System.Drawing.Point(203, 191)
        Me.txtEther_SecondaryDNSServer.MaxLength = 0
        Me.txtEther_SecondaryDNSServer.Name = "txtEther_SecondaryDNSServer"
        Me.txtEther_SecondaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEther_SecondaryDNSServer.Size = New System.Drawing.Size(152, 26)
        Me.txtEther_SecondaryDNSServer.TabIndex = 39
        '
        'txtEther_DefaultGateway
        '
        Me.txtEther_DefaultGateway.AcceptsReturn = True
        Me.txtEther_DefaultGateway.BackColor = System.Drawing.SystemColors.Window
        Me.txtEther_DefaultGateway.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEther_DefaultGateway.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEther_DefaultGateway.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEther_DefaultGateway.Location = New System.Drawing.Point(203, 104)
        Me.txtEther_DefaultGateway.MaxLength = 0
        Me.txtEther_DefaultGateway.Name = "txtEther_DefaultGateway"
        Me.txtEther_DefaultGateway.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEther_DefaultGateway.Size = New System.Drawing.Size(152, 26)
        Me.txtEther_DefaultGateway.TabIndex = 39
        Me.txtEther_DefaultGateway.Text = "192.168.1.1"
        '
        'txtEther_PrimaryDNSServer
        '
        Me.txtEther_PrimaryDNSServer.AcceptsReturn = True
        Me.txtEther_PrimaryDNSServer.BackColor = System.Drawing.SystemColors.Window
        Me.txtEther_PrimaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEther_PrimaryDNSServer.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEther_PrimaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEther_PrimaryDNSServer.Location = New System.Drawing.Point(203, 160)
        Me.txtEther_PrimaryDNSServer.MaxLength = 0
        Me.txtEther_PrimaryDNSServer.Name = "txtEther_PrimaryDNSServer"
        Me.txtEther_PrimaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEther_PrimaryDNSServer.Size = New System.Drawing.Size(152, 26)
        Me.txtEther_PrimaryDNSServer.TabIndex = 39
        Me.txtEther_PrimaryDNSServer.Text = "192.168.1.1"
        '
        'txtEther_Subnet
        '
        Me.txtEther_Subnet.AcceptsReturn = True
        Me.txtEther_Subnet.BackColor = System.Drawing.SystemColors.Window
        Me.txtEther_Subnet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEther_Subnet.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEther_Subnet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEther_Subnet.Location = New System.Drawing.Point(203, 73)
        Me.txtEther_Subnet.MaxLength = 0
        Me.txtEther_Subnet.Name = "txtEther_Subnet"
        Me.txtEther_Subnet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEther_Subnet.Size = New System.Drawing.Size(152, 26)
        Me.txtEther_Subnet.TabIndex = 39
        Me.txtEther_Subnet.Text = "255.255.255.0"
        '
        'txtEther_IP
        '
        Me.txtEther_IP.AcceptsReturn = True
        Me.txtEther_IP.BackColor = System.Drawing.SystemColors.Window
        Me.txtEther_IP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEther_IP.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEther_IP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEther_IP.Location = New System.Drawing.Point(203, 42)
        Me.txtEther_IP.MaxLength = 0
        Me.txtEther_IP.Name = "txtEther_IP"
        Me.txtEther_IP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEther_IP.Size = New System.Drawing.Size(152, 26)
        Me.txtEther_IP.TabIndex = 39
        Me.txtEther_IP.Text = "192.168.1.224"
        '
        'groupBox4
        '
        Me.groupBox4.Controls.Add(Me.label12)
        Me.groupBox4.Controls.Add(Me.label13)
        Me.groupBox4.Controls.Add(Me.label8)
        Me.groupBox4.Controls.Add(Me.btnGetCommSetting)
        Me.groupBox4.Controls.Add(Me.label17)
        Me.groupBox4.Controls.Add(Me.label16)
        Me.groupBox4.Controls.Add(Me.btnSetCommSetting)
        Me.groupBox4.Controls.Add(Me.txtP2P_Port)
        Me.groupBox4.Controls.Add(Me.txtP2P_Server)
        Me.groupBox4.Controls.Add(Me.txtDeviceID)
        Me.groupBox4.Controls.Add(Me.txtCommPwd)
        Me.groupBox4.Controls.Add(Me.txtTcpPort)
        Me.groupBox4.Location = New System.Drawing.Point(26, 289)
        Me.groupBox4.Margin = New System.Windows.Forms.Padding(2)
        Me.groupBox4.Name = "groupBox4"
        Me.groupBox4.Padding = New System.Windows.Forms.Padding(2)
        Me.groupBox4.Size = New System.Drawing.Size(373, 228)
        Me.groupBox4.TabIndex = 43
        Me.groupBox4.TabStop = False
        Me.groupBox4.Text = "Communication Setting"
        '
        'label12
        '
        Me.label12.AutoSize = True
        Me.label12.Location = New System.Drawing.Point(110, 25)
        Me.label12.Name = "label12"
        Me.label12.Size = New System.Drawing.Size(67, 16)
        Me.label12.TabIndex = 35
        Me.label12.Text = "DeviceID:"
        '
        'label13
        '
        Me.label13.AutoSize = True
        Me.label13.Location = New System.Drawing.Point(11, 57)
        Me.label13.Name = "label13"
        Me.label13.Size = New System.Drawing.Size(166, 16)
        Me.label13.TabIndex = 35
        Me.label13.Text = "Communication Password:"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(112, 89)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(65, 16)
        Me.label8.TabIndex = 35
        Me.label8.Text = "TCP Port:"
        '
        'btnGetCommSetting
        '
        Me.btnGetCommSetting.Location = New System.Drawing.Point(63, 184)
        Me.btnGetCommSetting.Name = "btnGetCommSetting"
        Me.btnGetCommSetting.Size = New System.Drawing.Size(81, 24)
        Me.btnGetCommSetting.TabIndex = 36
        Me.btnGetCommSetting.Text = "Get"
        Me.btnGetCommSetting.UseVisualStyleBackColor = True
        '
        'label17
        '
        Me.label17.AutoSize = True
        Me.label17.Location = New System.Drawing.Point(71, 150)
        Me.label17.Name = "label17"
        Me.label17.Size = New System.Drawing.Size(106, 16)
        Me.label17.TabIndex = 34
        Me.label17.Text = "P2P Server Port:"
        '
        'label16
        '
        Me.label16.AutoSize = True
        Me.label16.Location = New System.Drawing.Point(83, 119)
        Me.label16.Name = "label16"
        Me.label16.Size = New System.Drawing.Size(94, 16)
        Me.label16.TabIndex = 34
        Me.label16.Text = "P2P Server IP:"
        '
        'btnSetCommSetting
        '
        Me.btnSetCommSetting.Location = New System.Drawing.Point(174, 184)
        Me.btnSetCommSetting.Name = "btnSetCommSetting"
        Me.btnSetCommSetting.Size = New System.Drawing.Size(81, 24)
        Me.btnSetCommSetting.TabIndex = 37
        Me.btnSetCommSetting.Text = "Set"
        Me.btnSetCommSetting.UseVisualStyleBackColor = True
        '
        'txtP2P_Port
        '
        Me.txtP2P_Port.AcceptsReturn = True
        Me.txtP2P_Port.BackColor = System.Drawing.SystemColors.Window
        Me.txtP2P_Port.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtP2P_Port.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP2P_Port.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtP2P_Port.Location = New System.Drawing.Point(204, 144)
        Me.txtP2P_Port.MaxLength = 0
        Me.txtP2P_Port.Name = "txtP2P_Port"
        Me.txtP2P_Port.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtP2P_Port.Size = New System.Drawing.Size(152, 26)
        Me.txtP2P_Port.TabIndex = 39
        '
        'txtP2P_Server
        '
        Me.txtP2P_Server.AcceptsReturn = True
        Me.txtP2P_Server.BackColor = System.Drawing.SystemColors.Window
        Me.txtP2P_Server.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtP2P_Server.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP2P_Server.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtP2P_Server.Location = New System.Drawing.Point(204, 113)
        Me.txtP2P_Server.MaxLength = 0
        Me.txtP2P_Server.Name = "txtP2P_Server"
        Me.txtP2P_Server.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtP2P_Server.Size = New System.Drawing.Size(152, 26)
        Me.txtP2P_Server.TabIndex = 39
        '
        'txtDeviceID
        '
        Me.txtDeviceID.AcceptsReturn = True
        Me.txtDeviceID.BackColor = System.Drawing.SystemColors.Window
        Me.txtDeviceID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDeviceID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeviceID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDeviceID.Location = New System.Drawing.Point(204, 18)
        Me.txtDeviceID.MaxLength = 0
        Me.txtDeviceID.Name = "txtDeviceID"
        Me.txtDeviceID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDeviceID.Size = New System.Drawing.Size(152, 26)
        Me.txtDeviceID.TabIndex = 38
        Me.txtDeviceID.Text = "1"
        '
        'txtCommPwd
        '
        Me.txtCommPwd.AcceptsReturn = True
        Me.txtCommPwd.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommPwd.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommPwd.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommPwd.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommPwd.Location = New System.Drawing.Point(204, 50)
        Me.txtCommPwd.MaxLength = 0
        Me.txtCommPwd.Name = "txtCommPwd"
        Me.txtCommPwd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommPwd.Size = New System.Drawing.Size(152, 26)
        Me.txtCommPwd.TabIndex = 38
        Me.txtCommPwd.Text = "0"
        '
        'txtTcpPort
        '
        Me.txtTcpPort.AcceptsReturn = True
        Me.txtTcpPort.BackColor = System.Drawing.SystemColors.Window
        Me.txtTcpPort.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTcpPort.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTcpPort.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTcpPort.Location = New System.Drawing.Point(204, 82)
        Me.txtTcpPort.MaxLength = 0
        Me.txtTcpPort.Name = "txtTcpPort"
        Me.txtTcpPort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTcpPort.Size = New System.Drawing.Size(152, 26)
        Me.txtTcpPort.TabIndex = 38
        Me.txtTcpPort.Text = "5005"
        '
        'btnApplyCommSetting
        '
        Me.btnApplyCommSetting.Location = New System.Drawing.Point(478, 395)
        Me.btnApplyCommSetting.Name = "btnApplyCommSetting"
        Me.btnApplyCommSetting.Size = New System.Drawing.Size(192, 42)
        Me.btnApplyCommSetting.TabIndex = 42
        Me.btnApplyCommSetting.Text = "Apply Settings"
        Me.btnApplyCommSetting.UseVisualStyleBackColor = True
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(17, 255)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(152, 16)
        Me.label7.TabIndex = 34
        Me.label7.Text = "Secondary DNS Server:"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(60, 168)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(109, 16)
        Me.label5.TabIndex = 34
        Me.label5.Text = "Default Gateway:"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(37, 224)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(132, 16)
        Me.label6.TabIndex = 34
        Me.label6.Text = "Primary DNS Server:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(80, 137)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(89, 16)
        Me.label4.TabIndex = 34
        Me.label4.Text = "Subnet Mask:"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(127, 21)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(42, 16)
        Me.label1.TabIndex = 35
        Me.label1.Text = "SSID:"
        '
        'chkWiFi_ManualDNS
        '
        Me.chkWiFi_ManualDNS.AutoSize = True
        Me.chkWiFi_ManualDNS.Location = New System.Drawing.Point(190, 196)
        Me.chkWiFi_ManualDNS.Margin = New System.Windows.Forms.Padding(2)
        Me.chkWiFi_ManualDNS.Name = "chkWiFi_ManualDNS"
        Me.chkWiFi_ManualDNS.Size = New System.Drawing.Size(103, 20)
        Me.chkWiFi_ManualDNS.TabIndex = 40
        Me.chkWiFi_ManualDNS.Text = "Manual DNS"
        Me.chkWiFi_ManualDNS.UseVisualStyleBackColor = True
        '
        'chkWiFi_DHCP
        '
        Me.chkWiFi_DHCP.AutoSize = True
        Me.chkWiFi_DHCP.Checked = True
        Me.chkWiFi_DHCP.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWiFi_DHCP.Location = New System.Drawing.Point(190, 78)
        Me.chkWiFi_DHCP.Margin = New System.Windows.Forms.Padding(2)
        Me.chkWiFi_DHCP.Name = "chkWiFi_DHCP"
        Me.chkWiFi_DHCP.Size = New System.Drawing.Size(65, 20)
        Me.chkWiFi_DHCP.TabIndex = 40
        Me.chkWiFi_DHCP.Text = "DHCP"
        Me.chkWiFi_DHCP.UseVisualStyleBackColor = True
        '
        'btnGetWiFiSetting
        '
        Me.btnGetWiFiSetting.Location = New System.Drawing.Point(63, 289)
        Me.btnGetWiFiSetting.Name = "btnGetWiFiSetting"
        Me.btnGetWiFiSetting.Size = New System.Drawing.Size(81, 24)
        Me.btnGetWiFiSetting.TabIndex = 36
        Me.btnGetWiFiSetting.Text = "Get"
        Me.btnGetWiFiSetting.UseVisualStyleBackColor = True
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.chkWiFi_ManualDNS)
        Me.groupBox2.Controls.Add(Me.chkWiFi_DHCP)
        Me.groupBox2.Controls.Add(Me.label1)
        Me.groupBox2.Controls.Add(Me.btnGetWiFiSetting)
        Me.groupBox2.Controls.Add(Me.label7)
        Me.groupBox2.Controls.Add(Me.label5)
        Me.groupBox2.Controls.Add(Me.label6)
        Me.groupBox2.Controls.Add(Me.label4)
        Me.groupBox2.Controls.Add(Me.label3)
        Me.groupBox2.Controls.Add(Me.label2)
        Me.groupBox2.Controls.Add(Me.btnSetWiFiSetting)
        Me.groupBox2.Controls.Add(Me.txtWiFi_SecondaryDNSServer)
        Me.groupBox2.Controls.Add(Me.txtWiFi_DefaultGateway)
        Me.groupBox2.Controls.Add(Me.txtWiFi_PrimaryDNSServer)
        Me.groupBox2.Controls.Add(Me.txtWiFi_Subnet)
        Me.groupBox2.Controls.Add(Me.txtWiFi_IP)
        Me.groupBox2.Controls.Add(Me.txtWiFi_Key)
        Me.groupBox2.Controls.Add(Me.txtWiFi_SSID)
        Me.groupBox2.Location = New System.Drawing.Point(415, 21)
        Me.groupBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Padding = New System.Windows.Forms.Padding(2)
        Me.groupBox2.Size = New System.Drawing.Size(361, 325)
        Me.groupBox2.TabIndex = 45
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "WiFi Setting"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(146, 106)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(23, 16)
        Me.label3.TabIndex = 34
        Me.label3.Text = "IP:"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(135, 51)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(34, 16)
        Me.label2.TabIndex = 34
        Me.label2.Text = "Key:"
        '
        'btnSetWiFiSetting
        '
        Me.btnSetWiFiSetting.Location = New System.Drawing.Point(174, 289)
        Me.btnSetWiFiSetting.Name = "btnSetWiFiSetting"
        Me.btnSetWiFiSetting.Size = New System.Drawing.Size(81, 24)
        Me.btnSetWiFiSetting.TabIndex = 37
        Me.btnSetWiFiSetting.Text = "Set"
        Me.btnSetWiFiSetting.UseVisualStyleBackColor = True
        '
        'txtWiFi_SecondaryDNSServer
        '
        Me.txtWiFi_SecondaryDNSServer.AcceptsReturn = True
        Me.txtWiFi_SecondaryDNSServer.BackColor = System.Drawing.SystemColors.Window
        Me.txtWiFi_SecondaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWiFi_SecondaryDNSServer.Enabled = False
        Me.txtWiFi_SecondaryDNSServer.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWiFi_SecondaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWiFi_SecondaryDNSServer.Location = New System.Drawing.Point(190, 249)
        Me.txtWiFi_SecondaryDNSServer.MaxLength = 0
        Me.txtWiFi_SecondaryDNSServer.Name = "txtWiFi_SecondaryDNSServer"
        Me.txtWiFi_SecondaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWiFi_SecondaryDNSServer.Size = New System.Drawing.Size(152, 26)
        Me.txtWiFi_SecondaryDNSServer.TabIndex = 39
        '
        'txtWiFi_DefaultGateway
        '
        Me.txtWiFi_DefaultGateway.AcceptsReturn = True
        Me.txtWiFi_DefaultGateway.BackColor = System.Drawing.SystemColors.Window
        Me.txtWiFi_DefaultGateway.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWiFi_DefaultGateway.Enabled = False
        Me.txtWiFi_DefaultGateway.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWiFi_DefaultGateway.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWiFi_DefaultGateway.Location = New System.Drawing.Point(190, 162)
        Me.txtWiFi_DefaultGateway.MaxLength = 0
        Me.txtWiFi_DefaultGateway.Name = "txtWiFi_DefaultGateway"
        Me.txtWiFi_DefaultGateway.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWiFi_DefaultGateway.Size = New System.Drawing.Size(152, 26)
        Me.txtWiFi_DefaultGateway.TabIndex = 39
        '
        'txtWiFi_PrimaryDNSServer
        '
        Me.txtWiFi_PrimaryDNSServer.AcceptsReturn = True
        Me.txtWiFi_PrimaryDNSServer.BackColor = System.Drawing.SystemColors.Window
        Me.txtWiFi_PrimaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWiFi_PrimaryDNSServer.Enabled = False
        Me.txtWiFi_PrimaryDNSServer.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWiFi_PrimaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWiFi_PrimaryDNSServer.Location = New System.Drawing.Point(190, 219)
        Me.txtWiFi_PrimaryDNSServer.MaxLength = 0
        Me.txtWiFi_PrimaryDNSServer.Name = "txtWiFi_PrimaryDNSServer"
        Me.txtWiFi_PrimaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWiFi_PrimaryDNSServer.Size = New System.Drawing.Size(152, 26)
        Me.txtWiFi_PrimaryDNSServer.TabIndex = 39
        '
        'txtWiFi_Subnet
        '
        Me.txtWiFi_Subnet.AcceptsReturn = True
        Me.txtWiFi_Subnet.BackColor = System.Drawing.SystemColors.Window
        Me.txtWiFi_Subnet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWiFi_Subnet.Enabled = False
        Me.txtWiFi_Subnet.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWiFi_Subnet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWiFi_Subnet.Location = New System.Drawing.Point(190, 132)
        Me.txtWiFi_Subnet.MaxLength = 0
        Me.txtWiFi_Subnet.Name = "txtWiFi_Subnet"
        Me.txtWiFi_Subnet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWiFi_Subnet.Size = New System.Drawing.Size(152, 26)
        Me.txtWiFi_Subnet.TabIndex = 39
        '
        'txtWiFi_IP
        '
        Me.txtWiFi_IP.AcceptsReturn = True
        Me.txtWiFi_IP.BackColor = System.Drawing.SystemColors.Window
        Me.txtWiFi_IP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWiFi_IP.Enabled = False
        Me.txtWiFi_IP.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWiFi_IP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWiFi_IP.Location = New System.Drawing.Point(190, 101)
        Me.txtWiFi_IP.MaxLength = 0
        Me.txtWiFi_IP.Name = "txtWiFi_IP"
        Me.txtWiFi_IP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWiFi_IP.Size = New System.Drawing.Size(152, 26)
        Me.txtWiFi_IP.TabIndex = 39
        '
        'txtWiFi_Key
        '
        Me.txtWiFi_Key.AcceptsReturn = True
        Me.txtWiFi_Key.BackColor = System.Drawing.SystemColors.Window
        Me.txtWiFi_Key.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWiFi_Key.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWiFi_Key.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWiFi_Key.Location = New System.Drawing.Point(190, 46)
        Me.txtWiFi_Key.MaxLength = 0
        Me.txtWiFi_Key.Name = "txtWiFi_Key"
        Me.txtWiFi_Key.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWiFi_Key.Size = New System.Drawing.Size(152, 26)
        Me.txtWiFi_Key.TabIndex = 39
        '
        'txtWiFi_SSID
        '
        Me.txtWiFi_SSID.AcceptsReturn = True
        Me.txtWiFi_SSID.BackColor = System.Drawing.SystemColors.Window
        Me.txtWiFi_SSID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWiFi_SSID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWiFi_SSID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWiFi_SSID.Location = New System.Drawing.Point(190, 15)
        Me.txtWiFi_SSID.MaxLength = 0
        Me.txtWiFi_SSID.Name = "txtWiFi_SSID"
        Me.txtWiFi_SSID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWiFi_SSID.Size = New System.Drawing.Size(152, 26)
        Me.txtWiFi_SSID.TabIndex = 38
        '
        'frmNetworkSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 528)
        Me.Controls.Add(Me.groupBox3)
        Me.Controls.Add(Me.groupBox4)
        Me.Controls.Add(Me.btnApplyCommSetting)
        Me.Controls.Add(Me.groupBox2)
        Me.Name = "frmNetworkSetting"
        Me.Text = "frmNetworkSetting"
        Me.groupBox3.ResumeLayout(False)
        Me.groupBox3.PerformLayout()
        Me.groupBox4.ResumeLayout(False)
        Me.groupBox4.PerformLayout()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents groupBox3 As GroupBox
    Private WithEvents chkEther_ManualDNS As CheckBox
    Private WithEvents chkEther_DHCP As CheckBox
    Private WithEvents btnGetEthernetSetting As Button
    Private WithEvents label9 As Label
    Private WithEvents label10 As Label
    Private WithEvents label11 As Label
    Private WithEvents label14 As Label
    Private WithEvents label15 As Label
    Private WithEvents btnSetEthernetSetting As Button
    Public WithEvents txtEther_SecondaryDNSServer As TextBox
    Public WithEvents txtEther_DefaultGateway As TextBox
    Public WithEvents txtEther_PrimaryDNSServer As TextBox
    Public WithEvents txtEther_Subnet As TextBox
    Public WithEvents txtEther_IP As TextBox
    Private WithEvents groupBox4 As GroupBox
    Private WithEvents label12 As Label
    Private WithEvents label13 As Label
    Private WithEvents label8 As Label
    Private WithEvents btnGetCommSetting As Button
    Private WithEvents label17 As Label
    Private WithEvents label16 As Label
    Private WithEvents btnSetCommSetting As Button
    Public WithEvents txtP2P_Port As TextBox
    Public WithEvents txtP2P_Server As TextBox
    Public WithEvents txtDeviceID As TextBox
    Public WithEvents txtCommPwd As TextBox
    Public WithEvents txtTcpPort As TextBox
    Private WithEvents btnApplyCommSetting As Button
    Private WithEvents label7 As Label
    Private WithEvents label5 As Label
    Private WithEvents label6 As Label
    Private WithEvents label4 As Label
    Private WithEvents label1 As Label
    Private WithEvents chkWiFi_ManualDNS As CheckBox
    Private WithEvents chkWiFi_DHCP As CheckBox
    Private WithEvents btnGetWiFiSetting As Button
    Private WithEvents groupBox2 As GroupBox
    Private WithEvents label3 As Label
    Private WithEvents label2 As Label
    Private WithEvents btnSetWiFiSetting As Button
    Public WithEvents txtWiFi_SecondaryDNSServer As TextBox
    Public WithEvents txtWiFi_DefaultGateway As TextBox
    Public WithEvents txtWiFi_PrimaryDNSServer As TextBox
    Public WithEvents txtWiFi_Subnet As TextBox
    Public WithEvents txtWiFi_IP As TextBox
    Public WithEvents txtWiFi_Key As TextBox
    Public WithEvents txtWiFi_SSID As TextBox
End Class
