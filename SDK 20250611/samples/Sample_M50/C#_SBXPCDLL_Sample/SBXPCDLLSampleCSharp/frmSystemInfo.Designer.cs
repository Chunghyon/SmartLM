namespace SBXPCDLLSampleCSharp
{
    partial class frmSystemInfo
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
            this.cmdSetDeviceTime = new System.Windows.Forms.Button();
            this.cmdGetDeviceTime = new System.Windows.Forms.Button();
            this.cmdGetDeviceInfo = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdPowerOn = new System.Windows.Forms.Button();
            this.PowerOffDevice = new System.Windows.Forms.Button();
            this.cmdSetDeviceInfo = new System.Windows.Forms.Button();
            this.cmdEnableDevice = new System.Windows.Forms.Button();
            this.cmbSatus = new System.Windows.Forms.ComboBox();
            this.txtSetDevInfo = new System.Windows.Forms.TextBox();
            this.cmdGetDeviceStaus = new System.Windows.Forms.Button();
            this.chkEnableDevice = new System.Windows.Forms.CheckBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAdminPwd = new System.Windows.Forms.TextBox();
            this.txtPictureFolderDir = new System.Windows.Forms.TextBox();
            this.cmdPictureFolderBrowse = new System.Windows.Forms.Button();
            this.cmdGetGPSCoordinate = new System.Windows.Forms.Button();
            this.cmdUploadPictureByAdmin = new System.Windows.Forms.Button();
            this.cmdUploadPicture = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtIconNo = new System.Windows.Forms.TextBox();
            this.cmdSet = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIconStatus = new System.Windows.Forms.TextBox();
            this.txtIconFile = new System.Windows.Forms.TextBox();
            this.cmdIconBrowse = new System.Windows.Forms.Button();
            this.OpenFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdSetDeviceTime
            // 
            this.cmdSetDeviceTime.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSetDeviceTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSetDeviceTime.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSetDeviceTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSetDeviceTime.Location = new System.Drawing.Point(10, 120);
            this.cmdSetDeviceTime.Name = "cmdSetDeviceTime";
            this.cmdSetDeviceTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSetDeviceTime.Size = new System.Drawing.Size(125, 32);
            this.cmdSetDeviceTime.TabIndex = 29;
            this.cmdSetDeviceTime.Text = "SetDeviceTime";
            this.cmdSetDeviceTime.UseVisualStyleBackColor = false;
            this.cmdSetDeviceTime.Click += new System.EventHandler(this.cmdSetDeviceTime_Click);
            // 
            // cmdGetDeviceTime
            // 
            this.cmdGetDeviceTime.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetDeviceTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetDeviceTime.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetDeviceTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetDeviceTime.Location = new System.Drawing.Point(10, 78);
            this.cmdGetDeviceTime.Name = "cmdGetDeviceTime";
            this.cmdGetDeviceTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetDeviceTime.Size = new System.Drawing.Size(125, 32);
            this.cmdGetDeviceTime.TabIndex = 28;
            this.cmdGetDeviceTime.Text = "GetDeviceTime";
            this.cmdGetDeviceTime.UseVisualStyleBackColor = false;
            this.cmdGetDeviceTime.Click += new System.EventHandler(this.cmdGetDeviceTime_Click);
            // 
            // cmdGetDeviceInfo
            // 
            this.cmdGetDeviceInfo.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetDeviceInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetDeviceInfo.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetDeviceInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetDeviceInfo.Location = new System.Drawing.Point(10, 221);
            this.cmdGetDeviceInfo.Name = "cmdGetDeviceInfo";
            this.cmdGetDeviceInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetDeviceInfo.Size = new System.Drawing.Size(125, 32);
            this.cmdGetDeviceInfo.TabIndex = 27;
            this.cmdGetDeviceInfo.Text = "GetDeviceInfo";
            this.cmdGetDeviceInfo.UseVisualStyleBackColor = false;
            this.cmdGetDeviceInfo.Click += new System.EventHandler(this.cmdGetDeviceInfo_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
            this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdExit.Location = new System.Drawing.Point(301, 120);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdExit.Size = new System.Drawing.Size(125, 32);
            this.cmdExit.TabIndex = 26;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = false;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdPowerOn
            // 
            this.cmdPowerOn.BackColor = System.Drawing.SystemColors.Control;
            this.cmdPowerOn.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdPowerOn.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPowerOn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdPowerOn.Location = new System.Drawing.Point(152, 78);
            this.cmdPowerOn.Name = "cmdPowerOn";
            this.cmdPowerOn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdPowerOn.Size = new System.Drawing.Size(125, 32);
            this.cmdPowerOn.TabIndex = 25;
            this.cmdPowerOn.Text = "PowerOnDevice";
            this.cmdPowerOn.UseVisualStyleBackColor = false;
            this.cmdPowerOn.Click += new System.EventHandler(this.cmdPowerOn_Click);
            // 
            // PowerOffDevice
            // 
            this.PowerOffDevice.BackColor = System.Drawing.SystemColors.Control;
            this.PowerOffDevice.Cursor = System.Windows.Forms.Cursors.Default;
            this.PowerOffDevice.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PowerOffDevice.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PowerOffDevice.Location = new System.Drawing.Point(152, 120);
            this.PowerOffDevice.Name = "PowerOffDevice";
            this.PowerOffDevice.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PowerOffDevice.Size = new System.Drawing.Size(125, 32);
            this.PowerOffDevice.TabIndex = 24;
            this.PowerOffDevice.Text = "PowerOffDevice";
            this.PowerOffDevice.UseVisualStyleBackColor = false;
            this.PowerOffDevice.Click += new System.EventHandler(this.PowerOffDevice_Click);
            // 
            // cmdSetDeviceInfo
            // 
            this.cmdSetDeviceInfo.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSetDeviceInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSetDeviceInfo.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSetDeviceInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSetDeviceInfo.Location = new System.Drawing.Point(152, 221);
            this.cmdSetDeviceInfo.Name = "cmdSetDeviceInfo";
            this.cmdSetDeviceInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSetDeviceInfo.Size = new System.Drawing.Size(125, 32);
            this.cmdSetDeviceInfo.TabIndex = 23;
            this.cmdSetDeviceInfo.Text = "SetDeviceInfo";
            this.cmdSetDeviceInfo.UseVisualStyleBackColor = false;
            this.cmdSetDeviceInfo.Click += new System.EventHandler(this.cmdSetDeviceInfo_Click);
            // 
            // cmdEnableDevice
            // 
            this.cmdEnableDevice.BackColor = System.Drawing.SystemColors.Control;
            this.cmdEnableDevice.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdEnableDevice.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEnableDevice.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdEnableDevice.Location = new System.Drawing.Point(301, 79);
            this.cmdEnableDevice.Name = "cmdEnableDevice";
            this.cmdEnableDevice.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdEnableDevice.Size = new System.Drawing.Size(125, 32);
            this.cmdEnableDevice.TabIndex = 22;
            this.cmdEnableDevice.Text = "DisableDevice";
            this.cmdEnableDevice.UseVisualStyleBackColor = false;
            this.cmdEnableDevice.Click += new System.EventHandler(this.cmdEnableDevice_Click);
            // 
            // cmbSatus
            // 
            this.cmbSatus.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbSatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSatus.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbSatus.Items.AddRange(new object[] {
            " 1",
            " 2",
            " 3",
            " 4",
            " 5",
            " 6",
            " 7",
            " 8",
            " 9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48"});
            this.cmbSatus.Location = new System.Drawing.Point(146, 176);
            this.cmbSatus.Name = "cmbSatus";
            this.cmbSatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbSatus.Size = new System.Drawing.Size(88, 27);
            this.cmbSatus.TabIndex = 20;
            // 
            // txtSetDevInfo
            // 
            this.txtSetDevInfo.AcceptsReturn = true;
            this.txtSetDevInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtSetDevInfo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSetDevInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSetDevInfo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSetDevInfo.Location = new System.Drawing.Point(358, 173);
            this.txtSetDevInfo.MaxLength = 0;
            this.txtSetDevInfo.Name = "txtSetDevInfo";
            this.txtSetDevInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtSetDevInfo.Size = new System.Drawing.Size(59, 26);
            this.txtSetDevInfo.TabIndex = 19;
            // 
            // cmdGetDeviceStaus
            // 
            this.cmdGetDeviceStaus.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetDeviceStaus.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetDeviceStaus.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetDeviceStaus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetDeviceStaus.Location = new System.Drawing.Point(284, 221);
            this.cmdGetDeviceStaus.Name = "cmdGetDeviceStaus";
            this.cmdGetDeviceStaus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetDeviceStaus.Size = new System.Drawing.Size(142, 32);
            this.cmdGetDeviceStaus.TabIndex = 17;
            this.cmdGetDeviceStaus.Text = "GetDeviceStatus";
            this.cmdGetDeviceStaus.UseVisualStyleBackColor = false;
            this.cmdGetDeviceStaus.Click += new System.EventHandler(this.cmdGetDeviceStaus_Click);
            // 
            // chkEnableDevice
            // 
            this.chkEnableDevice.BackColor = System.Drawing.SystemColors.Control;
            this.chkEnableDevice.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkEnableDevice.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableDevice.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkEnableDevice.Location = new System.Drawing.Point(284, 84);
            this.chkEnableDevice.Name = "chkEnableDevice";
            this.chkEnableDevice.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkEnableDevice.Size = new System.Drawing.Size(15, 23);
            this.chkEnableDevice.TabIndex = 16;
            this.chkEnableDevice.UseVisualStyleBackColor = false;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(260, 178);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(97, 23);
            this.Label1.TabIndex = 21;
            this.Label1.Text = "Status Value:";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStatus.Location = new System.Drawing.Point(18, 169);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStatus.Size = new System.Drawing.Size(137, 41);
            this.lblStatus.TabIndex = 18;
            this.lblStatus.Text = "Status Paramerter:  Info Paramerter:";
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(13, 27);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.ReadOnly = true;
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(415, 29);
            this.lblMessage.TabIndex = 15;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtAdminPwd);
            this.groupBox1.Controls.Add(this.txtPictureFolderDir);
            this.groupBox1.Controls.Add(this.cmdPictureFolderBrowse);
            this.groupBox1.Controls.Add(this.cmdGetGPSCoordinate);
            this.groupBox1.Controls.Add(this.cmdUploadPictureByAdmin);
            this.groupBox1.Controls.Add(this.cmdUploadPicture);
            this.groupBox1.Location = new System.Drawing.Point(13, 267);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 157);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "M91";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "AdminPassword";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Please select folder for jpg files to upload.";
            // 
            // txtAdminPwd
            // 
            this.txtAdminPwd.Location = new System.Drawing.Point(111, 78);
            this.txtAdminPwd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAdminPwd.Name = "txtAdminPwd";
            this.txtAdminPwd.Size = new System.Drawing.Size(106, 20);
            this.txtAdminPwd.TabIndex = 3;
            // 
            // txtPictureFolderDir
            // 
            this.txtPictureFolderDir.Location = new System.Drawing.Point(21, 39);
            this.txtPictureFolderDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPictureFolderDir.Name = "txtPictureFolderDir";
            this.txtPictureFolderDir.Size = new System.Drawing.Size(216, 20);
            this.txtPictureFolderDir.TabIndex = 3;
            // 
            // cmdPictureFolderBrowse
            // 
            this.cmdPictureFolderBrowse.Location = new System.Drawing.Point(237, 37);
            this.cmdPictureFolderBrowse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmdPictureFolderBrowse.Name = "cmdPictureFolderBrowse";
            this.cmdPictureFolderBrowse.Size = new System.Drawing.Size(27, 25);
            this.cmdPictureFolderBrowse.TabIndex = 2;
            this.cmdPictureFolderBrowse.Text = "...";
            this.cmdPictureFolderBrowse.UseVisualStyleBackColor = true;
            this.cmdPictureFolderBrowse.Click += new System.EventHandler(this.cmdPictureFolderBrowse_Click);
            // 
            // cmdGetGPSCoordinate
            // 
            this.cmdGetGPSCoordinate.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetGPSCoordinate.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetGPSCoordinate.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetGPSCoordinate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetGPSCoordinate.Location = new System.Drawing.Point(223, 119);
            this.cmdGetGPSCoordinate.Name = "cmdGetGPSCoordinate";
            this.cmdGetGPSCoordinate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetGPSCoordinate.Size = new System.Drawing.Size(190, 32);
            this.cmdGetGPSCoordinate.TabIndex = 27;
            this.cmdGetGPSCoordinate.Text = "Get GPS Coordinate";
            this.cmdGetGPSCoordinate.UseVisualStyleBackColor = false;
            this.cmdGetGPSCoordinate.Click += new System.EventHandler(this.CmdGetGPSCoordinate_Click);
            // 
            // cmdUploadPictureByAdmin
            // 
            this.cmdUploadPictureByAdmin.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUploadPictureByAdmin.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUploadPictureByAdmin.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUploadPictureByAdmin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUploadPictureByAdmin.Location = new System.Drawing.Point(223, 73);
            this.cmdUploadPictureByAdmin.Name = "cmdUploadPictureByAdmin";
            this.cmdUploadPictureByAdmin.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUploadPictureByAdmin.Size = new System.Drawing.Size(190, 32);
            this.cmdUploadPictureByAdmin.TabIndex = 27;
            this.cmdUploadPictureByAdmin.Text = "UploadPicture(Admin)";
            this.cmdUploadPictureByAdmin.UseVisualStyleBackColor = false;
            this.cmdUploadPictureByAdmin.Click += new System.EventHandler(this.cmdUploadPictureByAdmin_Click);
            // 
            // cmdUploadPicture
            // 
            this.cmdUploadPicture.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUploadPicture.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUploadPicture.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUploadPicture.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUploadPicture.Location = new System.Drawing.Point(288, 33);
            this.cmdUploadPicture.Name = "cmdUploadPicture";
            this.cmdUploadPicture.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUploadPicture.Size = new System.Drawing.Size(125, 32);
            this.cmdUploadPicture.TabIndex = 27;
            this.cmdUploadPicture.Text = "UploadPicture";
            this.cmdUploadPicture.UseVisualStyleBackColor = false;
            this.cmdUploadPicture.Click += new System.EventHandler(this.cmdUploadPicture_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkDelete);
            this.groupBox2.Controls.Add(this.picIcon);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtIconNo);
            this.groupBox2.Controls.Add(this.cmdSet);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtIconStatus);
            this.groupBox2.Location = new System.Drawing.Point(13, 440);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(425, 200);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "M91 German TR Icon Setting";
            // 
            // chkDelete
            // 
            this.chkDelete.BackColor = System.Drawing.SystemColors.Control;
            this.chkDelete.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkDelete.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDelete.Location = new System.Drawing.Point(24, 58);
            this.chkDelete.Name = "chkDelete";
            this.chkDelete.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkDelete.Size = new System.Drawing.Size(112, 19);
            this.chkDelete.TabIndex = 50;
            this.chkDelete.Text = "Delete";
            this.chkDelete.UseVisualStyleBackColor = false;
            // 
            // picIcon
            // 
            this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picIcon.Location = new System.Drawing.Point(262, 63);
            this.picIcon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(114, 80);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIcon.TabIndex = 46;
            this.picIcon.TabStop = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(20, 83);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(101, 23);
            this.label6.TabIndex = 43;
            this.label6.Text = "Icon No(1~8):";
            // 
            // txtIconNo
            // 
            this.txtIconNo.AcceptsReturn = true;
            this.txtIconNo.BackColor = System.Drawing.SystemColors.Window;
            this.txtIconNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIconNo.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txtIconNo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtIconNo.Location = new System.Drawing.Point(139, 81);
            this.txtIconNo.MaxLength = 32;
            this.txtIconNo.Name = "txtIconNo";
            this.txtIconNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtIconNo.Size = new System.Drawing.Size(84, 23);
            this.txtIconNo.TabIndex = 41;
            this.txtIconNo.Text = "1";
            // 
            // cmdSet
            // 
            this.cmdSet.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSet.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSet.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSet.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSet.Location = new System.Drawing.Point(291, 151);
            this.cmdSet.Name = "cmdSet";
            this.cmdSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSet.Size = new System.Drawing.Size(125, 32);
            this.cmdSet.TabIndex = 44;
            this.cmdSet.Text = "Set";
            this.cmdSet.UseVisualStyleBackColor = false;
            this.cmdSet.Click += new System.EventHandler(this.cmdSet_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(21, 179);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(146, 16);
            this.label9.TabIndex = 42;
            this.label9.Text = "2: Selected(No scale)";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(21, 160);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(158, 23);
            this.label8.TabIndex = 42;
            this.label8.Text = "1: Pressed(scaled to 166*125)";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(21, 139);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(149, 23);
            this.label7.TabIndex = 42;
            this.label7.Text = "0: Normal(scaled to 166*125)";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(20, 110);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(116, 23);
            this.label5.TabIndex = 42;
            this.label5.Text = "Status(0~2):";
            // 
            // txtIconStatus
            // 
            this.txtIconStatus.AcceptsReturn = true;
            this.txtIconStatus.BackColor = System.Drawing.SystemColors.Window;
            this.txtIconStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIconStatus.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txtIconStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtIconStatus.Location = new System.Drawing.Point(138, 108);
            this.txtIconStatus.MaxLength = 32;
            this.txtIconStatus.Name = "txtIconStatus";
            this.txtIconStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtIconStatus.Size = new System.Drawing.Size(85, 23);
            this.txtIconStatus.TabIndex = 40;
            this.txtIconStatus.Text = "0";
            // 
            // txtIconFile
            // 
            this.txtIconFile.Location = new System.Drawing.Point(36, 473);
            this.txtIconFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtIconFile.Name = "txtIconFile";
            this.txtIconFile.Size = new System.Drawing.Size(354, 20);
            this.txtIconFile.TabIndex = 48;
            // 
            // cmdIconBrowse
            // 
            this.cmdIconBrowse.Location = new System.Drawing.Point(396, 470);
            this.cmdIconBrowse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmdIconBrowse.Name = "cmdIconBrowse";
            this.cmdIconBrowse.Size = new System.Drawing.Size(27, 25);
            this.cmdIconBrowse.TabIndex = 47;
            this.cmdIconBrowse.Text = "...";
            this.cmdIconBrowse.UseVisualStyleBackColor = true;
            this.cmdIconBrowse.Click += new System.EventHandler(this.cmdIconBrowse_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 453);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Please select .png file to upload.";
            // 
            // frmSystemInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 652);
            this.Controls.Add(this.txtIconFile);
            this.Controls.Add(this.cmdIconBrowse);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdSetDeviceTime);
            this.Controls.Add(this.cmdGetDeviceTime);
            this.Controls.Add(this.cmdGetDeviceInfo);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdPowerOn);
            this.Controls.Add(this.PowerOffDevice);
            this.Controls.Add(this.cmdSetDeviceInfo);
            this.Controls.Add(this.cmdEnableDevice);
            this.Controls.Add(this.cmbSatus);
            this.Controls.Add(this.txtSetDevInfo);
            this.Controls.Add(this.cmdGetDeviceStaus);
            this.Controls.Add(this.chkEnableDevice);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmSystemInfo";
            this.Text = "frmSystemInfo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSystemInfo_FormClosed);
            this.Load += new System.EventHandler(this.frmSystemInfo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button cmdSetDeviceTime;
        public System.Windows.Forms.Button cmdGetDeviceTime;
        public System.Windows.Forms.Button cmdGetDeviceInfo;
        public System.Windows.Forms.Button cmdExit;
        public System.Windows.Forms.Button cmdPowerOn;
        public System.Windows.Forms.Button PowerOffDevice;
        public System.Windows.Forms.Button cmdSetDeviceInfo;
        public System.Windows.Forms.Button cmdEnableDevice;
        public System.Windows.Forms.ComboBox cmbSatus;
        public System.Windows.Forms.TextBox txtSetDevInfo;
        public System.Windows.Forms.Button cmdGetDeviceStaus;
        public System.Windows.Forms.CheckBox chkEnableDevice;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.TextBox lblMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPictureFolderDir;
        private System.Windows.Forms.Button cmdPictureFolderBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.Button cmdUploadPicture;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button cmdGetGPSCoordinate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAdminPwd;
        public System.Windows.Forms.Button cmdUploadPictureByAdmin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox picIcon;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtIconNo;
        public System.Windows.Forms.Button cmdSet;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtIconStatus;
        private System.Windows.Forms.TextBox txtIconFile;
        private System.Windows.Forms.Button cmdIconBrowse;
        private System.Windows.Forms.OpenFileDialog OpenFileDlg;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox chkDelete;
    }
}