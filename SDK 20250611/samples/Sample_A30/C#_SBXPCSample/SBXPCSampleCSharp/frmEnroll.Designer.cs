namespace SBXPCSampleCSharp
{
    partial class frmEnroll
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
			this.components = new System.ComponentModel.Container();
			this.txtCardNumber = new System.Windows.Forms.TextBox();
			this.cmdDeleteCompany = new System.Windows.Forms.Button();
			this.cmdSetCompany = new System.Windows.Forms.Button();
			this.cmdGetName = new System.Windows.Forms.Button();
			this.cmdSetName = new System.Windows.Forms.Button();
			this.cmdModifyPrivilege = new System.Windows.Forms.Button();
			this.cmdEnableUser = new System.Windows.Forms.Button();
			this.cmdSetAllEnrollData = new System.Windows.Forms.Button();
			this.cmdGetAllEnrollData = new System.Windows.Forms.Button();
			this.cmdGetEnrollData = new System.Windows.Forms.Button();
			this.cmdClearData = new System.Windows.Forms.Button();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.cmdGetEnrollInfo = new System.Windows.Forms.Button();
			this.cmdDeleteEnrollData = new System.Windows.Forms.Button();
			this.cmdSetEnrollData = new System.Windows.Forms.Button();
			this.cmdDel = new System.Windows.Forms.Button();
			this.btnSetUserPeriod = new System.Windows.Forms.Button();
			this.cmdExit = new System.Windows.Forms.Button();
			this.cmdEmptyEnrollData = new System.Windows.Forms.Button();
			this.txtName = new System.Windows.Forms.TextBox();
			this.chkEnable = new System.Windows.Forms.CheckBox();
			this.cmbEMachineNumber = new System.Windows.Forms.ComboBox();
			this.cmbPrivilege = new System.Windows.Forms.ComboBox();
			this.lstEnrollData = new System.Windows.Forms.ListBox();
			this.txtEnrollNumber = new System.Windows.Forms.TextBox();
			this.cmbBackupNumber = new System.Windows.Forms.ComboBox();
			this.lblCardNum = new System.Windows.Forms.Label();
			this.lbName = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.lblEMachineNumber = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.lblEnrollData = new System.Windows.Forms.Label();
			this.lblBackupNumber = new System.Windows.Forms.Label();
			this._lblEnrollNum_0 = new System.Windows.Forms.Label();
			this.cmdModifyDuressFP = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbDuressSetting = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtUserTz2 = new System.Windows.Forms.TextBox();
			this.txtUserTz1 = new System.Windows.Forms.TextBox();
			this.OpenFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.chkUsePeriod = new System.Windows.Forms.CheckBox();
			this.lblFrom = new System.Windows.Forms.Label();
			this.dtPeriodFrom = new System.Windows.Forms.DateTimePicker();
			this.lblTo = new System.Windows.Forms.Label();
			this.dtPeriodTo = new System.Windows.Forms.DateTimePicker();
			this.btnGetUserPeriod = new System.Windows.Forms.Button();
			this.txtUserGroup = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmdSetUserInfo = new System.Windows.Forms.Button();
			this.cmdGetUserInfo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtCardNumber
			// 
			this.txtCardNumber.AcceptsReturn = true;
			this.txtCardNumber.BackColor = System.Drawing.SystemColors.Window;
			this.txtCardNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtCardNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCardNumber.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtCardNumber.Location = new System.Drawing.Point(150, 93);
			this.txtCardNumber.MaxLength = 0;
			this.txtCardNumber.Name = "txtCardNumber";
			this.txtCardNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtCardNumber.Size = new System.Drawing.Size(81, 26);
			this.txtCardNumber.TabIndex = 71;
			// 
			// cmdDeleteCompany
			// 
			this.cmdDeleteCompany.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDeleteCompany.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDeleteCompany.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdDeleteCompany.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDeleteCompany.Location = new System.Drawing.Point(710, 184);
			this.cmdDeleteCompany.Name = "cmdDeleteCompany";
			this.cmdDeleteCompany.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDeleteCompany.Size = new System.Drawing.Size(201, 28);
			this.cmdDeleteCompany.TabIndex = 69;
			this.cmdDeleteCompany.Text = "Delete Company Name";
			this.cmdDeleteCompany.UseVisualStyleBackColor = false;
			this.cmdDeleteCompany.Click += new System.EventHandler(this.cmdDeleteCompany_Click);
			// 
			// cmdSetCompany
			// 
			this.cmdSetCompany.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSetCompany.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdSetCompany.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdSetCompany.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdSetCompany.Location = new System.Drawing.Point(503, 184);
			this.cmdSetCompany.Name = "cmdSetCompany";
			this.cmdSetCompany.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdSetCompany.Size = new System.Drawing.Size(201, 28);
			this.cmdSetCompany.TabIndex = 68;
			this.cmdSetCompany.Text = "Set Company Name";
			this.cmdSetCompany.UseVisualStyleBackColor = false;
			this.cmdSetCompany.Click += new System.EventHandler(this.cmdSetCompany_Click);
			// 
			// cmdGetName
			// 
			this.cmdGetName.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGetName.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGetName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGetName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGetName.Location = new System.Drawing.Point(502, 150);
			this.cmdGetName.Name = "cmdGetName";
			this.cmdGetName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGetName.Size = new System.Drawing.Size(201, 28);
			this.cmdGetName.TabIndex = 66;
			this.cmdGetName.Text = "Get Name Data";
			this.ToolTip1.SetToolTip(this.cmdGetName, "Get All Enroll Data From Device And Save To DataBase");
			this.cmdGetName.UseVisualStyleBackColor = false;
			this.cmdGetName.Click += new System.EventHandler(this.cmdGetName_Click);
			// 
			// cmdSetName
			// 
			this.cmdSetName.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSetName.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdSetName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdSetName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdSetName.Location = new System.Drawing.Point(709, 150);
			this.cmdSetName.Name = "cmdSetName";
			this.cmdSetName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdSetName.Size = new System.Drawing.Size(201, 28);
			this.cmdSetName.TabIndex = 65;
			this.cmdSetName.Text = "Set Name Data";
			this.ToolTip1.SetToolTip(this.cmdSetName, "Load All Enroll Data From DataBase And Set To Device");
			this.cmdSetName.UseVisualStyleBackColor = false;
			this.cmdSetName.Click += new System.EventHandler(this.cmdSetName_Click);
			// 
			// cmdModifyPrivilege
			// 
			this.cmdModifyPrivilege.BackColor = System.Drawing.SystemColors.Control;
			this.cmdModifyPrivilege.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdModifyPrivilege.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdModifyPrivilege.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdModifyPrivilege.Location = new System.Drawing.Point(504, 250);
			this.cmdModifyPrivilege.Name = "cmdModifyPrivilege";
			this.cmdModifyPrivilege.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdModifyPrivilege.Size = new System.Drawing.Size(200, 28);
			this.cmdModifyPrivilege.TabIndex = 64;
			this.cmdModifyPrivilege.Text = "ModifyPrivilege";
			this.cmdModifyPrivilege.UseVisualStyleBackColor = false;
			this.cmdModifyPrivilege.Click += new System.EventHandler(this.cmdModifyPrivilege_Click);
			// 
			// cmdEnableUser
			// 
			this.cmdEnableUser.BackColor = System.Drawing.SystemColors.Control;
			this.cmdEnableUser.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdEnableUser.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdEnableUser.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdEnableUser.Location = new System.Drawing.Point(711, 217);
			this.cmdEnableUser.Name = "cmdEnableUser";
			this.cmdEnableUser.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdEnableUser.Size = new System.Drawing.Size(200, 28);
			this.cmdEnableUser.TabIndex = 63;
			this.cmdEnableUser.Text = "EnableUser";
			this.cmdEnableUser.UseVisualStyleBackColor = false;
			this.cmdEnableUser.Click += new System.EventHandler(this.cmdEnableUser_Click);
			// 
			// cmdSetAllEnrollData
			// 
			this.cmdSetAllEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSetAllEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdSetAllEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdSetAllEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdSetAllEnrollData.Location = new System.Drawing.Point(709, 117);
			this.cmdSetAllEnrollData.Name = "cmdSetAllEnrollData";
			this.cmdSetAllEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdSetAllEnrollData.Size = new System.Drawing.Size(200, 28);
			this.cmdSetAllEnrollData.TabIndex = 62;
			this.cmdSetAllEnrollData.Text = "Set All Enroll Data";
			this.ToolTip1.SetToolTip(this.cmdSetAllEnrollData, "Load All Enroll Data From DataBase And Set To Device");
			this.cmdSetAllEnrollData.UseVisualStyleBackColor = false;
			this.cmdSetAllEnrollData.Click += new System.EventHandler(this.cmdSetAllEnrollData_Click);
			// 
			// cmdGetAllEnrollData
			// 
			this.cmdGetAllEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGetAllEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGetAllEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGetAllEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGetAllEnrollData.Location = new System.Drawing.Point(503, 117);
			this.cmdGetAllEnrollData.Name = "cmdGetAllEnrollData";
			this.cmdGetAllEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGetAllEnrollData.Size = new System.Drawing.Size(200, 28);
			this.cmdGetAllEnrollData.TabIndex = 61;
			this.cmdGetAllEnrollData.Text = "Get All Enroll Data";
			this.ToolTip1.SetToolTip(this.cmdGetAllEnrollData, "Get All Enroll Data From Device And Save To DataBase");
			this.cmdGetAllEnrollData.UseVisualStyleBackColor = false;
			this.cmdGetAllEnrollData.Click += new System.EventHandler(this.cmdGetAllEnrollData_Click);
			// 
			// cmdGetEnrollData
			// 
			this.cmdGetEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGetEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGetEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGetEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGetEnrollData.Location = new System.Drawing.Point(503, 54);
			this.cmdGetEnrollData.Name = "cmdGetEnrollData";
			this.cmdGetEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGetEnrollData.Size = new System.Drawing.Size(200, 28);
			this.cmdGetEnrollData.TabIndex = 60;
			this.cmdGetEnrollData.Text = "Get Enroll Data";
			this.ToolTip1.SetToolTip(this.cmdGetEnrollData, "Get EnrollData From Device");
			this.cmdGetEnrollData.UseVisualStyleBackColor = false;
			this.cmdGetEnrollData.Click += new System.EventHandler(this.cmdGetEnrollData_Click);
			// 
			// cmdClearData
			// 
			this.cmdClearData.BackColor = System.Drawing.SystemColors.Control;
			this.cmdClearData.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdClearData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdClearData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdClearData.Location = new System.Drawing.Point(710, 283);
			this.cmdClearData.Name = "cmdClearData";
			this.cmdClearData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdClearData.Size = new System.Drawing.Size(200, 28);
			this.cmdClearData.TabIndex = 59;
			this.cmdClearData.Text = "Clear All Data(E,GL,SL) ";
			this.ToolTip1.SetToolTip(this.cmdClearData, "Clear EnrollData and LogDat Into Device");
			this.cmdClearData.UseVisualStyleBackColor = false;
			this.cmdClearData.Click += new System.EventHandler(this.cmdClearData_Click);
			// 
			// cmdGetEnrollInfo
			// 
			this.cmdGetEnrollInfo.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGetEnrollInfo.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGetEnrollInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGetEnrollInfo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGetEnrollInfo.Location = new System.Drawing.Point(503, 217);
			this.cmdGetEnrollInfo.Name = "cmdGetEnrollInfo";
			this.cmdGetEnrollInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGetEnrollInfo.Size = new System.Drawing.Size(200, 28);
			this.cmdGetEnrollInfo.TabIndex = 57;
			this.cmdGetEnrollInfo.Text = "Get Enroll Info";
			this.ToolTip1.SetToolTip(this.cmdGetEnrollInfo, "Get All Enrolled User Info From Device");
			this.cmdGetEnrollInfo.UseVisualStyleBackColor = false;
			this.cmdGetEnrollInfo.Click += new System.EventHandler(this.cmdGetEnrollInfo_Click);
			// 
			// cmdDeleteEnrollData
			// 
			this.cmdDeleteEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDeleteEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDeleteEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdDeleteEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDeleteEnrollData.Location = new System.Drawing.Point(709, 86);
			this.cmdDeleteEnrollData.Name = "cmdDeleteEnrollData";
			this.cmdDeleteEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDeleteEnrollData.Size = new System.Drawing.Size(200, 28);
			this.cmdDeleteEnrollData.TabIndex = 56;
			this.cmdDeleteEnrollData.Text = "Delete Enroll Data";
			this.ToolTip1.SetToolTip(this.cmdDeleteEnrollData, "Delete Enroll Data Into Device");
			this.cmdDeleteEnrollData.UseVisualStyleBackColor = false;
			this.cmdDeleteEnrollData.Click += new System.EventHandler(this.cmdDeleteEnrollData_Click);
			// 
			// cmdSetEnrollData
			// 
			this.cmdSetEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSetEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdSetEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdSetEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdSetEnrollData.Location = new System.Drawing.Point(709, 54);
			this.cmdSetEnrollData.Name = "cmdSetEnrollData";
			this.cmdSetEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdSetEnrollData.Size = new System.Drawing.Size(200, 28);
			this.cmdSetEnrollData.TabIndex = 55;
			this.cmdSetEnrollData.Text = "Set Enroll Data";
			this.ToolTip1.SetToolTip(this.cmdSetEnrollData, "Set EnrollData To Device");
			this.cmdSetEnrollData.UseVisualStyleBackColor = false;
			this.cmdSetEnrollData.Click += new System.EventHandler(this.cmdSetEnrollData_Click);
			// 
			// cmdDel
			// 
			this.cmdDel.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDel.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdDel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDel.Location = new System.Drawing.Point(387, 275);
			this.cmdDel.Name = "cmdDel";
			this.cmdDel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDel.Size = new System.Drawing.Size(100, 28);
			this.cmdDel.TabIndex = 47;
			this.cmdDel.Text = "Delete DB";
			this.ToolTip1.SetToolTip(this.cmdDel, "Delete All Saved Data From DataBase");
			this.cmdDel.UseVisualStyleBackColor = false;
			this.cmdDel.Click += new System.EventHandler(this.cmdDel_Click);
			// 
			// btnSetUserPeriod
			// 
			this.btnSetUserPeriod.BackColor = System.Drawing.SystemColors.Control;
			this.btnSetUserPeriod.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnSetUserPeriod.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetUserPeriod.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnSetUserPeriod.Location = new System.Drawing.Point(710, 316);
			this.btnSetUserPeriod.Name = "btnSetUserPeriod";
			this.btnSetUserPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnSetUserPeriod.Size = new System.Drawing.Size(200, 28);
			this.btnSetUserPeriod.TabIndex = 92;
			this.btnSetUserPeriod.Text = "SetUserPeriod";
			this.ToolTip1.SetToolTip(this.btnSetUserPeriod, "Clear EnrollData and LogDat Into Device");
			this.btnSetUserPeriod.UseVisualStyleBackColor = false;
			this.btnSetUserPeriod.Click += new System.EventHandler(this.btnSetUserPeriod_Click);
			// 
			// cmdExit
			// 
			this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
			this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdExit.Location = new System.Drawing.Point(710, 401);
			this.cmdExit.Name = "cmdExit";
			this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdExit.Size = new System.Drawing.Size(200, 28);
			this.cmdExit.TabIndex = 58;
			this.cmdExit.Text = "Exit";
			this.cmdExit.UseVisualStyleBackColor = false;
			this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
			// 
			// cmdEmptyEnrollData
			// 
			this.cmdEmptyEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.cmdEmptyEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdEmptyEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdEmptyEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdEmptyEnrollData.Location = new System.Drawing.Point(504, 283);
			this.cmdEmptyEnrollData.Name = "cmdEmptyEnrollData";
			this.cmdEmptyEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdEmptyEnrollData.Size = new System.Drawing.Size(200, 28);
			this.cmdEmptyEnrollData.TabIndex = 54;
			this.cmdEmptyEnrollData.Text = "Empty Enroll Data";
			this.cmdEmptyEnrollData.UseVisualStyleBackColor = false;
			this.cmdEmptyEnrollData.Click += new System.EventHandler(this.cmdEmptyEnrollData_Click);
			// 
			// txtName
			// 
			this.txtName.AcceptsReturn = true;
			this.txtName.BackColor = System.Drawing.SystemColors.Window;
			this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtName.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtName.Location = new System.Drawing.Point(323, 61);
			this.txtName.MaxLength = 10;
			this.txtName.Name = "txtName";
			this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtName.Size = new System.Drawing.Size(154, 26);
			this.txtName.TabIndex = 52;
			// 
			// chkEnable
			// 
			this.chkEnable.BackColor = System.Drawing.SystemColors.Control;
			this.chkEnable.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkEnable.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkEnable.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkEnable.Location = new System.Drawing.Point(267, 96);
			this.chkEnable.Name = "chkEnable";
			this.chkEnable.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkEnable.Size = new System.Drawing.Size(143, 24);
			this.chkEnable.TabIndex = 49;
			this.chkEnable.Text = "Disable User";
			this.chkEnable.UseVisualStyleBackColor = false;
			// 
			// cmbEMachineNumber
			// 
			this.cmbEMachineNumber.BackColor = System.Drawing.SystemColors.Window;
			this.cmbEMachineNumber.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbEMachineNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbEMachineNumber.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbEMachineNumber.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
			this.cmbEMachineNumber.Location = new System.Drawing.Point(150, 160);
			this.cmbEMachineNumber.Name = "cmbEMachineNumber";
			this.cmbEMachineNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbEMachineNumber.Size = new System.Drawing.Size(81, 27);
			this.cmbEMachineNumber.TabIndex = 45;
			this.cmbEMachineNumber.Text = "1";
			// 
			// cmbPrivilege
			// 
			this.cmbPrivilege.BackColor = System.Drawing.SystemColors.Window;
			this.cmbPrivilege.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbPrivilege.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbPrivilege.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbPrivilege.Items.AddRange(new object[] {
            "0",
            "1"});
			this.cmbPrivilege.Location = new System.Drawing.Point(150, 219);
			this.cmbPrivilege.Name = "cmbPrivilege";
			this.cmbPrivilege.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbPrivilege.Size = new System.Drawing.Size(81, 27);
			this.cmbPrivilege.TabIndex = 43;
			this.cmbPrivilege.Text = "0";
			// 
			// lstEnrollData
			// 
			this.lstEnrollData.BackColor = System.Drawing.SystemColors.Window;
			this.lstEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.lstEnrollData.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lstEnrollData.HorizontalScrollbar = true;
			this.lstEnrollData.ItemHeight = 12;
			this.lstEnrollData.Location = new System.Drawing.Point(267, 146);
			this.lstEnrollData.Name = "lstEnrollData";
			this.lstEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lstEnrollData.Size = new System.Drawing.Size(220, 124);
			this.lstEnrollData.TabIndex = 40;
			// 
			// txtEnrollNumber
			// 
			this.txtEnrollNumber.AcceptsReturn = true;
			this.txtEnrollNumber.BackColor = System.Drawing.SystemColors.Window;
			this.txtEnrollNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtEnrollNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtEnrollNumber.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtEnrollNumber.Location = new System.Drawing.Point(150, 59);
			this.txtEnrollNumber.MaxLength = 8;
			this.txtEnrollNumber.Name = "txtEnrollNumber";
			this.txtEnrollNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtEnrollNumber.Size = new System.Drawing.Size(81, 26);
			this.txtEnrollNumber.TabIndex = 38;
			this.txtEnrollNumber.Text = "1";
			// 
			// cmbBackupNumber
			// 
			this.cmbBackupNumber.BackColor = System.Drawing.SystemColors.Window;
			this.cmbBackupNumber.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbBackupNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbBackupNumber.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbBackupNumber.Items.AddRange(new object[] {
            " 0",
            " 1",
            " 2",
            "10",
            "11",
            "12",
            "13",
            "14"});
			this.cmbBackupNumber.Location = new System.Drawing.Point(150, 189);
			this.cmbBackupNumber.Name = "cmbBackupNumber";
			this.cmbBackupNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbBackupNumber.Size = new System.Drawing.Size(81, 27);
			this.cmbBackupNumber.TabIndex = 36;
			this.cmbBackupNumber.Text = "0";
			// 
			// lblCardNum
			// 
			this.lblCardNum.BackColor = System.Drawing.SystemColors.Control;
			this.lblCardNum.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblCardNum.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCardNum.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCardNum.Location = new System.Drawing.Point(22, 97);
			this.lblCardNum.Name = "lblCardNum";
			this.lblCardNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblCardNum.Size = new System.Drawing.Size(122, 20);
			this.lblCardNum.TabIndex = 70;
			this.lblCardNum.Text = "Card Number :";
			// 
			// lbName
			// 
			this.lbName.AutoSize = true;
			this.lbName.BackColor = System.Drawing.SystemColors.Control;
			this.lbName.Cursor = System.Windows.Forms.Cursors.Default;
			this.lbName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbName.Location = new System.Drawing.Point(275, 66);
			this.lbName.Name = "lbName";
			this.lbName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lbName.Size = new System.Drawing.Size(53, 19);
			this.lbName.TabIndex = 53;
			this.lbName.Text = "Name :";
			// 
			// Label2
			// 
			this.Label2.AutoSize = true;
			this.Label2.BackColor = System.Drawing.SystemColors.Control;
			this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label2.Location = new System.Drawing.Point(388, 123);
			this.Label2.Name = "Label2";
			this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label2.Size = new System.Drawing.Size(50, 19);
			this.Label2.TabIndex = 48;
			this.Label2.Text = "Total : ";
			// 
			// lblEMachineNumber
			// 
			this.lblEMachineNumber.AutoSize = true;
			this.lblEMachineNumber.BackColor = System.Drawing.SystemColors.Control;
			this.lblEMachineNumber.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblEMachineNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblEMachineNumber.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblEMachineNumber.Location = new System.Drawing.Point(22, 160);
			this.lblEMachineNumber.Name = "lblEMachineNumber";
			this.lblEMachineNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblEMachineNumber.Size = new System.Drawing.Size(131, 19);
			this.lblEMachineNumber.TabIndex = 46;
			this.lblEMachineNumber.Text = "EMachine Number :";
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.BackColor = System.Drawing.SystemColors.Control;
			this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Label1.Location = new System.Drawing.Point(22, 220);
			this.Label1.Name = "Label1";
			this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Label1.Size = new System.Drawing.Size(67, 19);
			this.Label1.TabIndex = 44;
			this.Label1.Text = "Privilege :";
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
			this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMessage.Location = new System.Drawing.Point(14, 16);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblMessage.Size = new System.Drawing.Size(487, 27);
			this.lblMessage.TabIndex = 42;
			this.lblMessage.Text = "Message";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblEnrollData
			// 
			this.lblEnrollData.AutoSize = true;
			this.lblEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.lblEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblEnrollData.Location = new System.Drawing.Point(268, 122);
			this.lblEnrollData.Name = "lblEnrollData";
			this.lblEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblEnrollData.Size = new System.Drawing.Size(99, 19);
			this.lblEnrollData.TabIndex = 41;
			this.lblEnrollData.Text = "Enrolled Data :";
			// 
			// lblBackupNumber
			// 
			this.lblBackupNumber.AutoSize = true;
			this.lblBackupNumber.BackColor = System.Drawing.SystemColors.Control;
			this.lblBackupNumber.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblBackupNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBackupNumber.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblBackupNumber.Location = new System.Drawing.Point(22, 193);
			this.lblBackupNumber.Name = "lblBackupNumber";
			this.lblBackupNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblBackupNumber.Size = new System.Drawing.Size(117, 19);
			this.lblBackupNumber.TabIndex = 39;
			this.lblBackupNumber.Text = "Backup Number :";
			// 
			// _lblEnrollNum_0
			// 
			this._lblEnrollNum_0.AutoSize = true;
			this._lblEnrollNum_0.BackColor = System.Drawing.SystemColors.Control;
			this._lblEnrollNum_0.Cursor = System.Windows.Forms.Cursors.Default;
			this._lblEnrollNum_0.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblEnrollNum_0.ForeColor = System.Drawing.SystemColors.ControlText;
			this._lblEnrollNum_0.Location = new System.Drawing.Point(22, 63);
			this._lblEnrollNum_0.Name = "_lblEnrollNum_0";
			this._lblEnrollNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._lblEnrollNum_0.Size = new System.Drawing.Size(105, 19);
			this._lblEnrollNum_0.TabIndex = 37;
			this._lblEnrollNum_0.Text = "Enroll Number :";
			// 
			// cmdModifyDuressFP
			// 
			this.cmdModifyDuressFP.BackColor = System.Drawing.SystemColors.Control;
			this.cmdModifyDuressFP.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdModifyDuressFP.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdModifyDuressFP.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdModifyDuressFP.Location = new System.Drawing.Point(710, 249);
			this.cmdModifyDuressFP.Name = "cmdModifyDuressFP";
			this.cmdModifyDuressFP.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdModifyDuressFP.Size = new System.Drawing.Size(200, 28);
			this.cmdModifyDuressFP.TabIndex = 72;
			this.cmdModifyDuressFP.Text = "Modify Duress FP";
			this.cmdModifyDuressFP.UseVisualStyleBackColor = false;
			this.cmdModifyDuressFP.Click += new System.EventHandler(this.cmdModifyDuressFP_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.SystemColors.Control;
			this.label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(22, 254);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label4.Size = new System.Drawing.Size(54, 19);
			this.label4.TabIndex = 76;
			this.label4.Text = "Duress:";
			// 
			// cmbDuressSetting
			// 
			this.cmbDuressSetting.BackColor = System.Drawing.SystemColors.Window;
			this.cmbDuressSetting.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbDuressSetting.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbDuressSetting.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbDuressSetting.Items.AddRange(new object[] {
            "0",
            "1"});
			this.cmbDuressSetting.Location = new System.Drawing.Point(150, 249);
			this.cmbDuressSetting.Name = "cmbDuressSetting";
			this.cmbDuressSetting.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbDuressSetting.Size = new System.Drawing.Size(81, 27);
			this.cmbDuressSetting.TabIndex = 77;
			this.cmbDuressSetting.Text = "0";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.SystemColors.Control;
			this.label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label5.Location = new System.Drawing.Point(22, 294);
			this.label5.Name = "label5";
			this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label5.Size = new System.Drawing.Size(71, 19);
			this.label5.TabIndex = 78;
			this.label5.Text = "User TZ1:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.SystemColors.Control;
			this.label6.Cursor = System.Windows.Forms.Cursors.Default;
			this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label6.Location = new System.Drawing.Point(22, 322);
			this.label6.Name = "label6";
			this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label6.Size = new System.Drawing.Size(71, 19);
			this.label6.TabIndex = 79;
			this.label6.Text = "User TZ2:";
			// 
			// txtUserTz2
			// 
			this.txtUserTz2.AcceptsReturn = true;
			this.txtUserTz2.BackColor = System.Drawing.SystemColors.Window;
			this.txtUserTz2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUserTz2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUserTz2.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUserTz2.Location = new System.Drawing.Point(150, 318);
			this.txtUserTz2.MaxLength = 0;
			this.txtUserTz2.Name = "txtUserTz2";
			this.txtUserTz2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUserTz2.Size = new System.Drawing.Size(81, 26);
			this.txtUserTz2.TabIndex = 84;
			// 
			// txtUserTz1
			// 
			this.txtUserTz1.AcceptsReturn = true;
			this.txtUserTz1.BackColor = System.Drawing.SystemColors.Window;
			this.txtUserTz1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUserTz1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUserTz1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUserTz1.Location = new System.Drawing.Point(150, 291);
			this.txtUserTz1.MaxLength = 8;
			this.txtUserTz1.Name = "txtUserTz1";
			this.txtUserTz1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUserTz1.Size = new System.Drawing.Size(81, 26);
			this.txtUserTz1.TabIndex = 83;
			// 
			// OpenFileDlg
			// 
			this.OpenFileDlg.FileName = "openFileDialog1";
			// 
			// chkUsePeriod
			// 
			this.chkUsePeriod.BackColor = System.Drawing.SystemColors.Control;
			this.chkUsePeriod.Cursor = System.Windows.Forms.Cursors.Default;
			this.chkUsePeriod.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkUsePeriod.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkUsePeriod.Location = new System.Drawing.Point(267, 309);
			this.chkUsePeriod.Name = "chkUsePeriod";
			this.chkUsePeriod.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.chkUsePeriod.Size = new System.Drawing.Size(143, 24);
			this.chkUsePeriod.TabIndex = 86;
			this.chkUsePeriod.Text = "Use Period";
			this.chkUsePeriod.UseVisualStyleBackColor = false;
			this.chkUsePeriod.CheckedChanged += new System.EventHandler(this.chkUsePeriod_CheckedChanged);
			// 
			// lblFrom
			// 
			this.lblFrom.AutoSize = true;
			this.lblFrom.BackColor = System.Drawing.SystemColors.Control;
			this.lblFrom.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblFrom.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFrom.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblFrom.Location = new System.Drawing.Point(286, 338);
			this.lblFrom.Name = "lblFrom";
			this.lblFrom.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblFrom.Size = new System.Drawing.Size(45, 19);
			this.lblFrom.TabIndex = 87;
			this.lblFrom.Text = "From:";
			// 
			// dtPeriodFrom
			// 
			this.dtPeriodFrom.Enabled = false;
			this.dtPeriodFrom.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtPeriodFrom.Location = new System.Drawing.Point(345, 335);
			this.dtPeriodFrom.Name = "dtPeriodFrom";
			this.dtPeriodFrom.Size = new System.Drawing.Size(132, 26);
			this.dtPeriodFrom.TabIndex = 88;
			// 
			// lblTo
			// 
			this.lblTo.AutoSize = true;
			this.lblTo.BackColor = System.Drawing.SystemColors.Control;
			this.lblTo.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblTo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblTo.Location = new System.Drawing.Point(286, 365);
			this.lblTo.Name = "lblTo";
			this.lblTo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblTo.Size = new System.Drawing.Size(28, 19);
			this.lblTo.TabIndex = 89;
			this.lblTo.Text = "To:";
			// 
			// dtPeriodTo
			// 
			this.dtPeriodTo.Enabled = false;
			this.dtPeriodTo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtPeriodTo.Location = new System.Drawing.Point(345, 362);
			this.dtPeriodTo.Name = "dtPeriodTo";
			this.dtPeriodTo.Size = new System.Drawing.Size(132, 26);
			this.dtPeriodTo.TabIndex = 90;
			// 
			// btnGetUserPeriod
			// 
			this.btnGetUserPeriod.BackColor = System.Drawing.SystemColors.Control;
			this.btnGetUserPeriod.Cursor = System.Windows.Forms.Cursors.Default;
			this.btnGetUserPeriod.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGetUserPeriod.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnGetUserPeriod.Location = new System.Drawing.Point(504, 316);
			this.btnGetUserPeriod.Name = "btnGetUserPeriod";
			this.btnGetUserPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnGetUserPeriod.Size = new System.Drawing.Size(200, 28);
			this.btnGetUserPeriod.TabIndex = 91;
			this.btnGetUserPeriod.Text = "GetUserPeriod";
			this.btnGetUserPeriod.UseVisualStyleBackColor = false;
			this.btnGetUserPeriod.Click += new System.EventHandler(this.btnGetUserPeriod_Click);
			// 
			// txtUserGroup
			// 
			this.txtUserGroup.AcceptsReturn = true;
			this.txtUserGroup.BackColor = System.Drawing.SystemColors.Window;
			this.txtUserGroup.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUserGroup.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUserGroup.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUserGroup.Location = new System.Drawing.Point(150, 345);
			this.txtUserGroup.MaxLength = 0;
			this.txtUserGroup.Name = "txtUserGroup";
			this.txtUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUserGroup.Size = new System.Drawing.Size(81, 26);
			this.txtUserGroup.TabIndex = 94;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label3.Location = new System.Drawing.Point(22, 349);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label3.Size = new System.Drawing.Size(84, 19);
			this.label3.TabIndex = 93;
			this.label3.Text = "User Group:";
			// 
			// cmdSetUserInfo
			// 
			this.cmdSetUserInfo.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSetUserInfo.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdSetUserInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdSetUserInfo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdSetUserInfo.Location = new System.Drawing.Point(126, 380);
			this.cmdSetUserInfo.Name = "cmdSetUserInfo";
			this.cmdSetUserInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdSetUserInfo.Size = new System.Drawing.Size(105, 28);
			this.cmdSetUserInfo.TabIndex = 96;
			this.cmdSetUserInfo.Text = "SetUserInfo";
			this.ToolTip1.SetToolTip(this.cmdSetUserInfo, "Clear EnrollData and LogDat Into Device");
			this.cmdSetUserInfo.UseVisualStyleBackColor = false;
			this.cmdSetUserInfo.Click += new System.EventHandler(this.cmdSetUserInfo_Click);
			// 
			// cmdGetUserInfo
			// 
			this.cmdGetUserInfo.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGetUserInfo.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGetUserInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGetUserInfo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGetUserInfo.Location = new System.Drawing.Point(12, 380);
			this.cmdGetUserInfo.Name = "cmdGetUserInfo";
			this.cmdGetUserInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGetUserInfo.Size = new System.Drawing.Size(105, 28);
			this.cmdGetUserInfo.TabIndex = 95;
			this.cmdGetUserInfo.Text = "GetUserInfo";
			this.cmdGetUserInfo.UseVisualStyleBackColor = false;
			this.cmdGetUserInfo.Click += new System.EventHandler(this.cmdGetUserInfo_Click);
			// 
			// frmEnroll
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(923, 441);
			this.Controls.Add(this.cmdSetUserInfo);
			this.Controls.Add(this.cmdGetUserInfo);
			this.Controls.Add(this.txtUserGroup);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnSetUserPeriod);
			this.Controls.Add(this.btnGetUserPeriod);
			this.Controls.Add(this.dtPeriodTo);
			this.Controls.Add(this.lblTo);
			this.Controls.Add(this.dtPeriodFrom);
			this.Controls.Add(this.lblFrom);
			this.Controls.Add(this.chkUsePeriod);
			this.Controls.Add(this.txtUserTz2);
			this.Controls.Add(this.txtUserTz1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cmbDuressSetting);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cmdModifyDuressFP);
			this.Controls.Add(this.txtCardNumber);
			this.Controls.Add(this.cmdDeleteCompany);
			this.Controls.Add(this.cmdSetCompany);
			this.Controls.Add(this.cmdGetName);
			this.Controls.Add(this.cmdSetName);
			this.Controls.Add(this.cmdModifyPrivilege);
			this.Controls.Add(this.cmdEnableUser);
			this.Controls.Add(this.cmdSetAllEnrollData);
			this.Controls.Add(this.cmdGetAllEnrollData);
			this.Controls.Add(this.cmdGetEnrollData);
			this.Controls.Add(this.cmdClearData);
			this.Controls.Add(this.cmdExit);
			this.Controls.Add(this.cmdGetEnrollInfo);
			this.Controls.Add(this.cmdDeleteEnrollData);
			this.Controls.Add(this.cmdSetEnrollData);
			this.Controls.Add(this.cmdEmptyEnrollData);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.chkEnable);
			this.Controls.Add(this.cmdDel);
			this.Controls.Add(this.cmbEMachineNumber);
			this.Controls.Add(this.cmbPrivilege);
			this.Controls.Add(this.lstEnrollData);
			this.Controls.Add(this.txtEnrollNumber);
			this.Controls.Add(this.cmbBackupNumber);
			this.Controls.Add(this.lblCardNum);
			this.Controls.Add(this.lbName);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.lblEMachineNumber);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.lblEnrollData);
			this.Controls.Add(this.lblBackupNumber);
			this.Controls.Add(this._lblEnrollNum_0);
			this.Name = "frmEnroll";
			this.Text = "frmEnroll";
			this.Load += new System.EventHandler(this.frmEnroll_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEnroll_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtCardNumber;
        public System.Windows.Forms.Button cmdDeleteCompany;
        public System.Windows.Forms.Button cmdSetCompany;
        public System.Windows.Forms.Button cmdGetName;
        public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.Button cmdSetName;
        public System.Windows.Forms.Button cmdModifyPrivilege;
        public System.Windows.Forms.Button cmdEnableUser;
        public System.Windows.Forms.Button cmdSetAllEnrollData;
        public System.Windows.Forms.Button cmdGetAllEnrollData;
        public System.Windows.Forms.Button cmdGetEnrollData;
        public System.Windows.Forms.Button cmdClearData;
        public System.Windows.Forms.Button cmdGetEnrollInfo;
        public System.Windows.Forms.Button cmdDeleteEnrollData;
        public System.Windows.Forms.Button cmdSetEnrollData;
        public System.Windows.Forms.Button cmdDel;
        public System.Windows.Forms.Button cmdExit;
        public System.Windows.Forms.Button cmdEmptyEnrollData;
        public System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.CheckBox chkEnable;
        public System.Windows.Forms.ComboBox cmbEMachineNumber;
        public System.Windows.Forms.ComboBox cmbPrivilege;
        public System.Windows.Forms.ListBox lstEnrollData;
        public System.Windows.Forms.TextBox txtEnrollNumber;
        public System.Windows.Forms.ComboBox cmbBackupNumber;
        public System.Windows.Forms.Label lblCardNum;
        public System.Windows.Forms.Label lbName;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label lblEMachineNumber;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label lblMessage;
        public System.Windows.Forms.Label lblEnrollData;
        public System.Windows.Forms.Label lblBackupNumber;
        public System.Windows.Forms.Label _lblEnrollNum_0;
        public System.Windows.Forms.Button cmdModifyDuressFP;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox cmbDuressSetting;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtUserTz2;
        public System.Windows.Forms.TextBox txtUserTz1;
        private System.Windows.Forms.OpenFileDialog OpenFileDlg;
        public System.Windows.Forms.CheckBox chkUsePeriod;
        public System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtPeriodFrom;
        public System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtPeriodTo;
        public System.Windows.Forms.Button btnSetUserPeriod;
        public System.Windows.Forms.Button btnGetUserPeriod;
        public System.Windows.Forms.TextBox txtUserGroup;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button cmdSetUserInfo;
        public System.Windows.Forms.Button cmdGetUserInfo;
    }
}