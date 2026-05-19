namespace SBXPCDLLSampleCSharp
{
    partial class frmNetworkSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkWiFi_ManualDNS = new System.Windows.Forms.CheckBox();
            this.chkWiFi_DHCP = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetWiFiSetting = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSetWiFiSetting = new System.Windows.Forms.Button();
            this.txtWiFi_SecondaryDNSServer = new System.Windows.Forms.TextBox();
            this.txtWiFi_DefaultGateway = new System.Windows.Forms.TextBox();
            this.txtWiFi_PrimaryDNSServer = new System.Windows.Forms.TextBox();
            this.txtWiFi_Subnet = new System.Windows.Forms.TextBox();
            this.txtWiFi_IP = new System.Windows.Forms.TextBox();
            this.txtWiFi_Key = new System.Windows.Forms.TextBox();
            this.txtWiFi_SSID = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkEther_ManualDNS = new System.Windows.Forms.CheckBox();
            this.chkEther_DHCP = new System.Windows.Forms.CheckBox();
            this.btnGetEthernetSetting = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnSetEthernetSetting = new System.Windows.Forms.Button();
            this.txtEther_SecondaryDNSServer = new System.Windows.Forms.TextBox();
            this.txtEther_DefaultGateway = new System.Windows.Forms.TextBox();
            this.txtEther_PrimaryDNSServer = new System.Windows.Forms.TextBox();
            this.txtEther_Subnet = new System.Windows.Forms.TextBox();
            this.txtEther_IP = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnGetCommSetting = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnSetCommSetting = new System.Windows.Forms.Button();
            this.txtP2P_Port = new System.Windows.Forms.TextBox();
            this.txtP2P_Server = new System.Windows.Forms.TextBox();
            this.txtDeviceID = new System.Windows.Forms.TextBox();
            this.txtCommPwd = new System.Windows.Forms.TextBox();
            this.txtTcpPort = new System.Windows.Forms.TextBox();
            this.btnApplyCommSetting = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkWiFi_ManualDNS);
            this.groupBox2.Controls.Add(this.chkWiFi_DHCP);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnGetWiFiSetting);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnSetWiFiSetting);
            this.groupBox2.Controls.Add(this.txtWiFi_SecondaryDNSServer);
            this.groupBox2.Controls.Add(this.txtWiFi_DefaultGateway);
            this.groupBox2.Controls.Add(this.txtWiFi_PrimaryDNSServer);
            this.groupBox2.Controls.Add(this.txtWiFi_Subnet);
            this.groupBox2.Controls.Add(this.txtWiFi_IP);
            this.groupBox2.Controls.Add(this.txtWiFi_Key);
            this.groupBox2.Controls.Add(this.txtWiFi_SSID);
            this.groupBox2.Location = new System.Drawing.Point(337, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(314, 325);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WiFi Setting";
            // 
            // chkWiFi_ManualDNS
            // 
            this.chkWiFi_ManualDNS.AutoSize = true;
            this.chkWiFi_ManualDNS.Location = new System.Drawing.Point(145, 196);
            this.chkWiFi_ManualDNS.Margin = new System.Windows.Forms.Padding(2);
            this.chkWiFi_ManualDNS.Name = "chkWiFi_ManualDNS";
            this.chkWiFi_ManualDNS.Size = new System.Drawing.Size(87, 17);
            this.chkWiFi_ManualDNS.TabIndex = 40;
            this.chkWiFi_ManualDNS.Text = "Manual DNS";
            this.chkWiFi_ManualDNS.UseVisualStyleBackColor = true;
            this.chkWiFi_ManualDNS.CheckedChanged += new System.EventHandler(this.chkWiFi_ManualDNS_CheckedChanged);
            // 
            // chkWiFi_DHCP
            // 
            this.chkWiFi_DHCP.AutoSize = true;
            this.chkWiFi_DHCP.Checked = true;
            this.chkWiFi_DHCP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWiFi_DHCP.Location = new System.Drawing.Point(145, 78);
            this.chkWiFi_DHCP.Margin = new System.Windows.Forms.Padding(2);
            this.chkWiFi_DHCP.Name = "chkWiFi_DHCP";
            this.chkWiFi_DHCP.Size = new System.Drawing.Size(56, 17);
            this.chkWiFi_DHCP.TabIndex = 40;
            this.chkWiFi_DHCP.Text = "DHCP";
            this.chkWiFi_DHCP.UseVisualStyleBackColor = true;
            this.chkWiFi_DHCP.CheckedChanged += new System.EventHandler(this.chkWiFi_DHCP_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "SSID:";
            // 
            // btnGetWiFiSetting
            // 
            this.btnGetWiFiSetting.Location = new System.Drawing.Point(63, 289);
            this.btnGetWiFiSetting.Name = "btnGetWiFiSetting";
            this.btnGetWiFiSetting.Size = new System.Drawing.Size(81, 24);
            this.btnGetWiFiSetting.TabIndex = 36;
            this.btnGetWiFiSetting.Text = "Get";
            this.btnGetWiFiSetting.UseVisualStyleBackColor = true;
            this.btnGetWiFiSetting.Click += new System.EventHandler(this.btnGetWiFiSetting_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 255);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Secondary DNS Server:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Default Gateway:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Primary DNS Server:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Subnet Mask:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Key:";
            // 
            // btnSetWiFiSetting
            // 
            this.btnSetWiFiSetting.Location = new System.Drawing.Point(174, 289);
            this.btnSetWiFiSetting.Name = "btnSetWiFiSetting";
            this.btnSetWiFiSetting.Size = new System.Drawing.Size(81, 24);
            this.btnSetWiFiSetting.TabIndex = 37;
            this.btnSetWiFiSetting.Text = "Set";
            this.btnSetWiFiSetting.UseVisualStyleBackColor = true;
            this.btnSetWiFiSetting.Click += new System.EventHandler(this.btnSetWiFiSetting_Click);
            // 
            // txtWiFi_SecondaryDNSServer
            // 
            this.txtWiFi_SecondaryDNSServer.AcceptsReturn = true;
            this.txtWiFi_SecondaryDNSServer.BackColor = System.Drawing.SystemColors.Window;
            this.txtWiFi_SecondaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWiFi_SecondaryDNSServer.Enabled = false;
            this.txtWiFi_SecondaryDNSServer.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWiFi_SecondaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWiFi_SecondaryDNSServer.Location = new System.Drawing.Point(145, 249);
            this.txtWiFi_SecondaryDNSServer.MaxLength = 0;
            this.txtWiFi_SecondaryDNSServer.Name = "txtWiFi_SecondaryDNSServer";
            this.txtWiFi_SecondaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWiFi_SecondaryDNSServer.Size = new System.Drawing.Size(152, 26);
            this.txtWiFi_SecondaryDNSServer.TabIndex = 39;
            // 
            // txtWiFi_DefaultGateway
            // 
            this.txtWiFi_DefaultGateway.AcceptsReturn = true;
            this.txtWiFi_DefaultGateway.BackColor = System.Drawing.SystemColors.Window;
            this.txtWiFi_DefaultGateway.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWiFi_DefaultGateway.Enabled = false;
            this.txtWiFi_DefaultGateway.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWiFi_DefaultGateway.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWiFi_DefaultGateway.Location = new System.Drawing.Point(145, 162);
            this.txtWiFi_DefaultGateway.MaxLength = 0;
            this.txtWiFi_DefaultGateway.Name = "txtWiFi_DefaultGateway";
            this.txtWiFi_DefaultGateway.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWiFi_DefaultGateway.Size = new System.Drawing.Size(152, 26);
            this.txtWiFi_DefaultGateway.TabIndex = 39;
            // 
            // txtWiFi_PrimaryDNSServer
            // 
            this.txtWiFi_PrimaryDNSServer.AcceptsReturn = true;
            this.txtWiFi_PrimaryDNSServer.BackColor = System.Drawing.SystemColors.Window;
            this.txtWiFi_PrimaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWiFi_PrimaryDNSServer.Enabled = false;
            this.txtWiFi_PrimaryDNSServer.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWiFi_PrimaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWiFi_PrimaryDNSServer.Location = new System.Drawing.Point(145, 219);
            this.txtWiFi_PrimaryDNSServer.MaxLength = 0;
            this.txtWiFi_PrimaryDNSServer.Name = "txtWiFi_PrimaryDNSServer";
            this.txtWiFi_PrimaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWiFi_PrimaryDNSServer.Size = new System.Drawing.Size(152, 26);
            this.txtWiFi_PrimaryDNSServer.TabIndex = 39;
            // 
            // txtWiFi_Subnet
            // 
            this.txtWiFi_Subnet.AcceptsReturn = true;
            this.txtWiFi_Subnet.BackColor = System.Drawing.SystemColors.Window;
            this.txtWiFi_Subnet.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWiFi_Subnet.Enabled = false;
            this.txtWiFi_Subnet.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWiFi_Subnet.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWiFi_Subnet.Location = new System.Drawing.Point(145, 132);
            this.txtWiFi_Subnet.MaxLength = 0;
            this.txtWiFi_Subnet.Name = "txtWiFi_Subnet";
            this.txtWiFi_Subnet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWiFi_Subnet.Size = new System.Drawing.Size(152, 26);
            this.txtWiFi_Subnet.TabIndex = 39;
            // 
            // txtWiFi_IP
            // 
            this.txtWiFi_IP.AcceptsReturn = true;
            this.txtWiFi_IP.BackColor = System.Drawing.SystemColors.Window;
            this.txtWiFi_IP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWiFi_IP.Enabled = false;
            this.txtWiFi_IP.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWiFi_IP.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWiFi_IP.Location = new System.Drawing.Point(145, 101);
            this.txtWiFi_IP.MaxLength = 0;
            this.txtWiFi_IP.Name = "txtWiFi_IP";
            this.txtWiFi_IP.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWiFi_IP.Size = new System.Drawing.Size(152, 26);
            this.txtWiFi_IP.TabIndex = 39;
            // 
            // txtWiFi_Key
            // 
            this.txtWiFi_Key.AcceptsReturn = true;
            this.txtWiFi_Key.BackColor = System.Drawing.SystemColors.Window;
            this.txtWiFi_Key.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWiFi_Key.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWiFi_Key.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWiFi_Key.Location = new System.Drawing.Point(145, 46);
            this.txtWiFi_Key.MaxLength = 0;
            this.txtWiFi_Key.Name = "txtWiFi_Key";
            this.txtWiFi_Key.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWiFi_Key.Size = new System.Drawing.Size(152, 26);
            this.txtWiFi_Key.TabIndex = 39;
            // 
            // txtWiFi_SSID
            // 
            this.txtWiFi_SSID.AcceptsReturn = true;
            this.txtWiFi_SSID.BackColor = System.Drawing.SystemColors.Window;
            this.txtWiFi_SSID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWiFi_SSID.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWiFi_SSID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtWiFi_SSID.Location = new System.Drawing.Point(145, 15);
            this.txtWiFi_SSID.MaxLength = 0;
            this.txtWiFi_SSID.Name = "txtWiFi_SSID";
            this.txtWiFi_SSID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWiFi_SSID.Size = new System.Drawing.Size(152, 26);
            this.txtWiFi_SSID.TabIndex = 38;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkEther_ManualDNS);
            this.groupBox3.Controls.Add(this.chkEther_DHCP);
            this.groupBox3.Controls.Add(this.btnGetEthernetSetting);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.btnSetEthernetSetting);
            this.groupBox3.Controls.Add(this.txtEther_SecondaryDNSServer);
            this.groupBox3.Controls.Add(this.txtEther_DefaultGateway);
            this.groupBox3.Controls.Add(this.txtEther_PrimaryDNSServer);
            this.groupBox3.Controls.Add(this.txtEther_Subnet);
            this.groupBox3.Controls.Add(this.txtEther_IP);
            this.groupBox3.Location = new System.Drawing.Point(9, 10);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(314, 268);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ethernet Setting";
            // 
            // chkEther_ManualDNS
            // 
            this.chkEther_ManualDNS.AutoSize = true;
            this.chkEther_ManualDNS.Checked = true;
            this.chkEther_ManualDNS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEther_ManualDNS.Location = new System.Drawing.Point(146, 137);
            this.chkEther_ManualDNS.Margin = new System.Windows.Forms.Padding(2);
            this.chkEther_ManualDNS.Name = "chkEther_ManualDNS";
            this.chkEther_ManualDNS.Size = new System.Drawing.Size(87, 17);
            this.chkEther_ManualDNS.TabIndex = 40;
            this.chkEther_ManualDNS.Text = "Manual DNS";
            this.chkEther_ManualDNS.UseVisualStyleBackColor = true;
            this.chkEther_ManualDNS.CheckedChanged += new System.EventHandler(this.chkEther_ManualDNS_CheckedChanged);
            // 
            // chkEther_DHCP
            // 
            this.chkEther_DHCP.AutoSize = true;
            this.chkEther_DHCP.Location = new System.Drawing.Point(146, 20);
            this.chkEther_DHCP.Margin = new System.Windows.Forms.Padding(2);
            this.chkEther_DHCP.Name = "chkEther_DHCP";
            this.chkEther_DHCP.Size = new System.Drawing.Size(56, 17);
            this.chkEther_DHCP.TabIndex = 40;
            this.chkEther_DHCP.Text = "DHCP";
            this.chkEther_DHCP.UseVisualStyleBackColor = true;
            this.chkEther_DHCP.CheckedChanged += new System.EventHandler(this.chkEther_DHCP_CheckedChanged);
            // 
            // btnGetEthernetSetting
            // 
            this.btnGetEthernetSetting.Location = new System.Drawing.Point(64, 231);
            this.btnGetEthernetSetting.Name = "btnGetEthernetSetting";
            this.btnGetEthernetSetting.Size = new System.Drawing.Size(81, 24);
            this.btnGetEthernetSetting.TabIndex = 36;
            this.btnGetEthernetSetting.Text = "Get";
            this.btnGetEthernetSetting.UseVisualStyleBackColor = true;
            this.btnGetEthernetSetting.Click += new System.EventHandler(this.btnGetEthernetSetting_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Secondary DNS Server:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(53, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "Default Gateway:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(39, 166);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Primary DNS Server:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(70, 79);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "Subnet Mask:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(122, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(20, 13);
            this.label15.TabIndex = 34;
            this.label15.Text = "IP:";
            // 
            // btnSetEthernetSetting
            // 
            this.btnSetEthernetSetting.Location = new System.Drawing.Point(176, 231);
            this.btnSetEthernetSetting.Name = "btnSetEthernetSetting";
            this.btnSetEthernetSetting.Size = new System.Drawing.Size(81, 24);
            this.btnSetEthernetSetting.TabIndex = 37;
            this.btnSetEthernetSetting.Text = "Set";
            this.btnSetEthernetSetting.UseVisualStyleBackColor = true;
            this.btnSetEthernetSetting.Click += new System.EventHandler(this.btnSetEthernetSetting_Click);
            // 
            // txtEther_SecondaryDNSServer
            // 
            this.txtEther_SecondaryDNSServer.AcceptsReturn = true;
            this.txtEther_SecondaryDNSServer.BackColor = System.Drawing.SystemColors.Window;
            this.txtEther_SecondaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEther_SecondaryDNSServer.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEther_SecondaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEther_SecondaryDNSServer.Location = new System.Drawing.Point(146, 191);
            this.txtEther_SecondaryDNSServer.MaxLength = 0;
            this.txtEther_SecondaryDNSServer.Name = "txtEther_SecondaryDNSServer";
            this.txtEther_SecondaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEther_SecondaryDNSServer.Size = new System.Drawing.Size(152, 26);
            this.txtEther_SecondaryDNSServer.TabIndex = 39;
            // 
            // txtEther_DefaultGateway
            // 
            this.txtEther_DefaultGateway.AcceptsReturn = true;
            this.txtEther_DefaultGateway.BackColor = System.Drawing.SystemColors.Window;
            this.txtEther_DefaultGateway.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEther_DefaultGateway.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEther_DefaultGateway.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEther_DefaultGateway.Location = new System.Drawing.Point(146, 104);
            this.txtEther_DefaultGateway.MaxLength = 0;
            this.txtEther_DefaultGateway.Name = "txtEther_DefaultGateway";
            this.txtEther_DefaultGateway.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEther_DefaultGateway.Size = new System.Drawing.Size(152, 26);
            this.txtEther_DefaultGateway.TabIndex = 39;
            this.txtEther_DefaultGateway.Text = "192.168.1.1";
            // 
            // txtEther_PrimaryDNSServer
            // 
            this.txtEther_PrimaryDNSServer.AcceptsReturn = true;
            this.txtEther_PrimaryDNSServer.BackColor = System.Drawing.SystemColors.Window;
            this.txtEther_PrimaryDNSServer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEther_PrimaryDNSServer.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEther_PrimaryDNSServer.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEther_PrimaryDNSServer.Location = new System.Drawing.Point(146, 160);
            this.txtEther_PrimaryDNSServer.MaxLength = 0;
            this.txtEther_PrimaryDNSServer.Name = "txtEther_PrimaryDNSServer";
            this.txtEther_PrimaryDNSServer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEther_PrimaryDNSServer.Size = new System.Drawing.Size(152, 26);
            this.txtEther_PrimaryDNSServer.TabIndex = 39;
            this.txtEther_PrimaryDNSServer.Text = "192.168.1.1";
            // 
            // txtEther_Subnet
            // 
            this.txtEther_Subnet.AcceptsReturn = true;
            this.txtEther_Subnet.BackColor = System.Drawing.SystemColors.Window;
            this.txtEther_Subnet.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEther_Subnet.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEther_Subnet.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEther_Subnet.Location = new System.Drawing.Point(146, 73);
            this.txtEther_Subnet.MaxLength = 0;
            this.txtEther_Subnet.Name = "txtEther_Subnet";
            this.txtEther_Subnet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEther_Subnet.Size = new System.Drawing.Size(152, 26);
            this.txtEther_Subnet.TabIndex = 39;
            this.txtEther_Subnet.Text = "255.255.255.0";
            // 
            // txtEther_IP
            // 
            this.txtEther_IP.AcceptsReturn = true;
            this.txtEther_IP.BackColor = System.Drawing.SystemColors.Window;
            this.txtEther_IP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEther_IP.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEther_IP.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEther_IP.Location = new System.Drawing.Point(146, 42);
            this.txtEther_IP.MaxLength = 0;
            this.txtEther_IP.Name = "txtEther_IP";
            this.txtEther_IP.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEther_IP.Size = new System.Drawing.Size(152, 26);
            this.txtEther_IP.TabIndex = 39;
            this.txtEther_IP.Text = "192.168.1.224";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.btnGetCommSetting);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.btnSetCommSetting);
            this.groupBox4.Controls.Add(this.txtP2P_Port);
            this.groupBox4.Controls.Add(this.txtP2P_Server);
            this.groupBox4.Controls.Add(this.txtDeviceID);
            this.groupBox4.Controls.Add(this.txtCommPwd);
            this.groupBox4.Controls.Add(this.txtTcpPort);
            this.groupBox4.Location = new System.Drawing.Point(9, 283);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(314, 215);
            this.groupBox4.TabIndex = 40;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Communication Setting";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(88, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "DeviceID:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 57);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(131, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "Communication Password:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(88, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "TCP Port:";
            // 
            // btnGetCommSetting
            // 
            this.btnGetCommSetting.Location = new System.Drawing.Point(63, 177);
            this.btnGetCommSetting.Name = "btnGetCommSetting";
            this.btnGetCommSetting.Size = new System.Drawing.Size(81, 24);
            this.btnGetCommSetting.TabIndex = 36;
            this.btnGetCommSetting.Text = "Get";
            this.btnGetCommSetting.UseVisualStyleBackColor = true;
            this.btnGetCommSetting.Click += new System.EventHandler(this.btnGetCommSetting_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(54, 150);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(86, 13);
            this.label17.TabIndex = 34;
            this.label17.Text = "P2P Server Port:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(64, 119);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 13);
            this.label16.TabIndex = 34;
            this.label16.Text = "P2P Server IP:";
            // 
            // btnSetCommSetting
            // 
            this.btnSetCommSetting.Location = new System.Drawing.Point(174, 177);
            this.btnSetCommSetting.Name = "btnSetCommSetting";
            this.btnSetCommSetting.Size = new System.Drawing.Size(81, 24);
            this.btnSetCommSetting.TabIndex = 37;
            this.btnSetCommSetting.Text = "Set";
            this.btnSetCommSetting.UseVisualStyleBackColor = true;
            this.btnSetCommSetting.Click += new System.EventHandler(this.btnSetCommSetting_Click);
            // 
            // txtP2P_Port
            // 
            this.txtP2P_Port.AcceptsReturn = true;
            this.txtP2P_Port.BackColor = System.Drawing.SystemColors.Window;
            this.txtP2P_Port.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtP2P_Port.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtP2P_Port.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtP2P_Port.Location = new System.Drawing.Point(145, 144);
            this.txtP2P_Port.MaxLength = 0;
            this.txtP2P_Port.Name = "txtP2P_Port";
            this.txtP2P_Port.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtP2P_Port.Size = new System.Drawing.Size(152, 26);
            this.txtP2P_Port.TabIndex = 39;
            // 
            // txtP2P_Server
            // 
            this.txtP2P_Server.AcceptsReturn = true;
            this.txtP2P_Server.BackColor = System.Drawing.SystemColors.Window;
            this.txtP2P_Server.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtP2P_Server.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtP2P_Server.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtP2P_Server.Location = new System.Drawing.Point(145, 113);
            this.txtP2P_Server.MaxLength = 0;
            this.txtP2P_Server.Name = "txtP2P_Server";
            this.txtP2P_Server.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtP2P_Server.Size = new System.Drawing.Size(152, 26);
            this.txtP2P_Server.TabIndex = 39;
            // 
            // txtDeviceID
            // 
            this.txtDeviceID.AcceptsReturn = true;
            this.txtDeviceID.BackColor = System.Drawing.SystemColors.Window;
            this.txtDeviceID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDeviceID.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeviceID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDeviceID.Location = new System.Drawing.Point(145, 18);
            this.txtDeviceID.MaxLength = 0;
            this.txtDeviceID.Name = "txtDeviceID";
            this.txtDeviceID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDeviceID.Size = new System.Drawing.Size(152, 26);
            this.txtDeviceID.TabIndex = 38;
            this.txtDeviceID.Text = "1";
            // 
            // txtCommPwd
            // 
            this.txtCommPwd.AcceptsReturn = true;
            this.txtCommPwd.BackColor = System.Drawing.SystemColors.Window;
            this.txtCommPwd.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCommPwd.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommPwd.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCommPwd.Location = new System.Drawing.Point(145, 50);
            this.txtCommPwd.MaxLength = 0;
            this.txtCommPwd.Name = "txtCommPwd";
            this.txtCommPwd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCommPwd.Size = new System.Drawing.Size(152, 26);
            this.txtCommPwd.TabIndex = 38;
            this.txtCommPwd.Text = "0";
            // 
            // txtTcpPort
            // 
            this.txtTcpPort.AcceptsReturn = true;
            this.txtTcpPort.BackColor = System.Drawing.SystemColors.Window;
            this.txtTcpPort.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTcpPort.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTcpPort.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTcpPort.Location = new System.Drawing.Point(145, 82);
            this.txtTcpPort.MaxLength = 0;
            this.txtTcpPort.Name = "txtTcpPort";
            this.txtTcpPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTcpPort.Size = new System.Drawing.Size(152, 26);
            this.txtTcpPort.TabIndex = 38;
            this.txtTcpPort.Text = "5005";
            // 
            // btnApplyCommSetting
            // 
            this.btnApplyCommSetting.Location = new System.Drawing.Point(400, 389);
            this.btnApplyCommSetting.Name = "btnApplyCommSetting";
            this.btnApplyCommSetting.Size = new System.Drawing.Size(192, 24);
            this.btnApplyCommSetting.TabIndex = 37;
            this.btnApplyCommSetting.Text = "Apply Settings";
            this.btnApplyCommSetting.UseVisualStyleBackColor = true;
            this.btnApplyCommSetting.Click += new System.EventHandler(this.btnApplyCommSetting_Click);
            // 
            // frmNetworkSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 509);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnApplyCommSetting);
            this.Name = "frmNetworkSetting";
            this.Text = "frmNetworkSetting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNetworkSetting_FormClosed);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkWiFi_DHCP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetWiFiSetting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSetWiFiSetting;
        public System.Windows.Forms.TextBox txtWiFi_Key;
        public System.Windows.Forms.TextBox txtWiFi_SSID;
        private System.Windows.Forms.CheckBox chkWiFi_ManualDNS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtWiFi_SecondaryDNSServer;
        public System.Windows.Forms.TextBox txtWiFi_DefaultGateway;
        public System.Windows.Forms.TextBox txtWiFi_PrimaryDNSServer;
        public System.Windows.Forms.TextBox txtWiFi_Subnet;
        public System.Windows.Forms.TextBox txtWiFi_IP;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkEther_ManualDNS;
        private System.Windows.Forms.CheckBox chkEther_DHCP;
        private System.Windows.Forms.Button btnGetEthernetSetting;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnSetEthernetSetting;
        public System.Windows.Forms.TextBox txtEther_SecondaryDNSServer;
        public System.Windows.Forms.TextBox txtEther_DefaultGateway;
        public System.Windows.Forms.TextBox txtEther_PrimaryDNSServer;
        public System.Windows.Forms.TextBox txtEther_Subnet;
        public System.Windows.Forms.TextBox txtEther_IP;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnGetCommSetting;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnSetCommSetting;
        public System.Windows.Forms.TextBox txtP2P_Port;
        public System.Windows.Forms.TextBox txtP2P_Server;
        public System.Windows.Forms.TextBox txtTcpPort;
        private System.Windows.Forms.Button btnApplyCommSetting;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.TextBox txtDeviceID;
        public System.Windows.Forms.TextBox txtCommPwd;
    }
}