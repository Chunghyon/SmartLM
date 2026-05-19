namespace SBXPCDLLSampleCSharp
{
    partial class frmLockCtrl
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
			this.cmdWarnCancel = new System.Windows.Forms.Button();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.cmdRestart = new System.Windows.Forms.Button();
			this.cmdAutoRecover = new System.Windows.Forms.Button();
			this.cmdUncondClose = new System.Windows.Forms.Button();
			this.cmdUncondOpen = new System.Windows.Forms.Button();
			this.cmdGetDoorStatus = new System.Windows.Forms.Button();
			this.cmdDoorOpen = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.txtDoorNumber = new System.Windows.Forms.TextBox();
			this._lblEnrollNum_0 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmdSet = new System.Windows.Forms.Button();
			this.cmdGet = new System.Windows.Forms.Button();
			this.cmbLocation = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cmbAntipassNo = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbUseAntipass = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtOpenTimeout = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtLockReleaseTime = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbSensorType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtUnlockGroup1 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup2 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup4 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup3 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtUnlockGroup6 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup5 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtUnlockGroup8 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup7 = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtUnlockGroup10 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup9 = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.txtUnlockGroup20 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup19 = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.txtUnlockGroup18 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup17 = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.txtUnlockGroup16 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup15 = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.txtUnlockGroup14 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup13 = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.txtUnlockGroup12 = new System.Windows.Forms.TextBox();
			this.txtUnlockGroup11 = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.cmdWriteUnlockGroup = new System.Windows.Forms.Button();
			this.cmdReadUnlockGroup = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdWarnCancel
			// 
			this.cmdWarnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.cmdWarnCancel.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdWarnCancel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdWarnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdWarnCancel.Location = new System.Drawing.Point(281, 283);
			this.cmdWarnCancel.Name = "cmdWarnCancel";
			this.cmdWarnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdWarnCancel.Size = new System.Drawing.Size(125, 30);
			this.cmdWarnCancel.TabIndex = 15;
			this.cmdWarnCancel.Text = "Warn Cancel";
			this.cmdWarnCancel.UseVisualStyleBackColor = false;
			this.cmdWarnCancel.Click += new System.EventHandler(this.cmdWarnCancel_Click);
			// 
			// cmdRestart
			// 
			this.cmdRestart.BackColor = System.Drawing.SystemColors.Control;
			this.cmdRestart.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdRestart.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdRestart.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdRestart.Location = new System.Drawing.Point(281, 319);
			this.cmdRestart.Name = "cmdRestart";
			this.cmdRestart.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdRestart.Size = new System.Drawing.Size(124, 30);
			this.cmdRestart.TabIndex = 14;
			this.cmdRestart.Text = "Reboot";
			this.cmdRestart.UseVisualStyleBackColor = false;
			this.cmdRestart.Click += new System.EventHandler(this.cmdRestart_Click);
			// 
			// cmdAutoRecover
			// 
			this.cmdAutoRecover.BackColor = System.Drawing.SystemColors.Control;
			this.cmdAutoRecover.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdAutoRecover.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdAutoRecover.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdAutoRecover.Location = new System.Drawing.Point(281, 174);
			this.cmdAutoRecover.Name = "cmdAutoRecover";
			this.cmdAutoRecover.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdAutoRecover.Size = new System.Drawing.Size(124, 31);
			this.cmdAutoRecover.TabIndex = 13;
			this.cmdAutoRecover.Text = "Auto Recover";
			this.cmdAutoRecover.UseVisualStyleBackColor = false;
			this.cmdAutoRecover.Click += new System.EventHandler(this.cmdAutoRecover_Click);
			// 
			// cmdUncondClose
			// 
			this.cmdUncondClose.BackColor = System.Drawing.SystemColors.Control;
			this.cmdUncondClose.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdUncondClose.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdUncondClose.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdUncondClose.Location = new System.Drawing.Point(281, 247);
			this.cmdUncondClose.Name = "cmdUncondClose";
			this.cmdUncondClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdUncondClose.Size = new System.Drawing.Size(126, 30);
			this.cmdUncondClose.TabIndex = 12;
			this.cmdUncondClose.Text = "Uncond Close";
			this.cmdUncondClose.UseVisualStyleBackColor = false;
			this.cmdUncondClose.Click += new System.EventHandler(this.cmdUncondClose_Click);
			// 
			// cmdUncondOpen
			// 
			this.cmdUncondOpen.BackColor = System.Drawing.SystemColors.Control;
			this.cmdUncondOpen.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdUncondOpen.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdUncondOpen.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdUncondOpen.Location = new System.Drawing.Point(281, 211);
			this.cmdUncondOpen.Name = "cmdUncondOpen";
			this.cmdUncondOpen.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdUncondOpen.Size = new System.Drawing.Size(126, 30);
			this.cmdUncondOpen.TabIndex = 11;
			this.cmdUncondOpen.Text = "Uncond Open";
			this.cmdUncondOpen.UseVisualStyleBackColor = false;
			this.cmdUncondOpen.Click += new System.EventHandler(this.cmdUncondOpen_Click);
			// 
			// cmdGetDoorStatus
			// 
			this.cmdGetDoorStatus.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGetDoorStatus.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGetDoorStatus.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGetDoorStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGetDoorStatus.Location = new System.Drawing.Point(283, 102);
			this.cmdGetDoorStatus.Name = "cmdGetDoorStatus";
			this.cmdGetDoorStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGetDoorStatus.Size = new System.Drawing.Size(122, 30);
			this.cmdGetDoorStatus.TabIndex = 10;
			this.cmdGetDoorStatus.Text = "Get DoorStatus";
			this.cmdGetDoorStatus.UseVisualStyleBackColor = false;
			this.cmdGetDoorStatus.Click += new System.EventHandler(this.cmdGetDoorStatus_Click);
			// 
			// cmdDoorOpen
			// 
			this.cmdDoorOpen.BackColor = System.Drawing.SystemColors.Control;
			this.cmdDoorOpen.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdDoorOpen.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdDoorOpen.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdDoorOpen.Location = new System.Drawing.Point(282, 138);
			this.cmdDoorOpen.Name = "cmdDoorOpen";
			this.cmdDoorOpen.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdDoorOpen.Size = new System.Drawing.Size(125, 30);
			this.cmdDoorOpen.TabIndex = 9;
			this.cmdDoorOpen.Text = "Door Open";
			this.cmdDoorOpen.UseVisualStyleBackColor = false;
			this.cmdDoorOpen.Click += new System.EventHandler(this.cmdDoorOpen_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
			this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMessage.Location = new System.Drawing.Point(7, 13);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblMessage.Size = new System.Drawing.Size(401, 26);
			this.lblMessage.TabIndex = 8;
			this.lblMessage.Text = "Message";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtDoorNumber
			// 
			this.txtDoorNumber.AcceptsReturn = true;
			this.txtDoorNumber.BackColor = System.Drawing.SystemColors.Window;
			this.txtDoorNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtDoorNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDoorNumber.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtDoorNumber.Location = new System.Drawing.Point(233, 47);
			this.txtDoorNumber.MaxLength = 8;
			this.txtDoorNumber.Name = "txtDoorNumber";
			this.txtDoorNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtDoorNumber.Size = new System.Drawing.Size(81, 26);
			this.txtDoorNumber.TabIndex = 40;
			this.txtDoorNumber.Text = "0";
			// 
			// _lblEnrollNum_0
			// 
			this._lblEnrollNum_0.AutoSize = true;
			this._lblEnrollNum_0.BackColor = System.Drawing.SystemColors.Control;
			this._lblEnrollNum_0.Cursor = System.Windows.Forms.Cursors.Default;
			this._lblEnrollNum_0.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblEnrollNum_0.ForeColor = System.Drawing.SystemColors.ControlText;
			this._lblEnrollNum_0.Location = new System.Drawing.Point(105, 51);
			this._lblEnrollNum_0.Name = "_lblEnrollNum_0";
			this._lblEnrollNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._lblEnrollNum_0.Size = new System.Drawing.Size(102, 19);
			this._lblEnrollNum_0.TabIndex = 39;
			this._lblEnrollNum_0.Text = "Door Number :";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cmdSet);
			this.groupBox1.Controls.Add(this.cmdGet);
			this.groupBox1.Controls.Add(this.cmbLocation);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.cmbAntipassNo);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.cmbUseAntipass);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtOpenTimeout);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtLockReleaseTime);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cmbSensorType);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(15, 91);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(248, 274);
			this.groupBox1.TabIndex = 41;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Door Setting";
			// 
			// cmdSet
			// 
			this.cmdSet.BackColor = System.Drawing.SystemColors.Control;
			this.cmdSet.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdSet.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdSet.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdSet.Location = new System.Drawing.Point(135, 233);
			this.cmdSet.Name = "cmdSet";
			this.cmdSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdSet.Size = new System.Drawing.Size(91, 30);
			this.cmdSet.TabIndex = 58;
			this.cmdSet.Text = "Write";
			this.cmdSet.UseVisualStyleBackColor = false;
			this.cmdSet.Click += new System.EventHandler(this.cmdSet_Click);
			// 
			// cmdGet
			// 
			this.cmdGet.BackColor = System.Drawing.SystemColors.Control;
			this.cmdGet.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdGet.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdGet.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdGet.Location = new System.Drawing.Point(21, 233);
			this.cmdGet.Name = "cmdGet";
			this.cmdGet.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdGet.Size = new System.Drawing.Size(93, 30);
			this.cmdGet.TabIndex = 57;
			this.cmdGet.Text = "Read";
			this.cmdGet.UseVisualStyleBackColor = false;
			this.cmdGet.Click += new System.EventHandler(this.cmdGet_Click);
			// 
			// cmbLocation
			// 
			this.cmbLocation.BackColor = System.Drawing.SystemColors.Window;
			this.cmbLocation.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLocation.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbLocation.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbLocation.Items.AddRange(new object[] {
            "Master",
            "Slave"});
			this.cmbLocation.Location = new System.Drawing.Point(138, 200);
			this.cmbLocation.Name = "cmbLocation";
			this.cmbLocation.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbLocation.Size = new System.Drawing.Size(81, 27);
			this.cmbLocation.TabIndex = 56;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.SystemColors.Control;
			this.label6.Cursor = System.Windows.Forms.Cursors.Default;
			this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label6.Location = new System.Drawing.Point(15, 203);
			this.label6.Name = "label6";
			this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label6.Size = new System.Drawing.Size(118, 19);
			this.label6.TabIndex = 55;
			this.label6.Text = "Antipass Location";
			// 
			// cmbAntipassNo
			// 
			this.cmbAntipassNo.BackColor = System.Drawing.SystemColors.Window;
			this.cmbAntipassNo.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbAntipassNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAntipassNo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbAntipassNo.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbAntipassNo.Items.AddRange(new object[] {
            "Group 1",
            "Group 2"});
			this.cmbAntipassNo.Location = new System.Drawing.Point(138, 167);
			this.cmbAntipassNo.Name = "cmbAntipassNo";
			this.cmbAntipassNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbAntipassNo.Size = new System.Drawing.Size(81, 27);
			this.cmbAntipassNo.TabIndex = 54;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.SystemColors.Control;
			this.label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label5.Location = new System.Drawing.Point(48, 170);
			this.label5.Name = "label5";
			this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label5.Size = new System.Drawing.Size(85, 19);
			this.label5.TabIndex = 53;
			this.label5.Text = "Antipass No";
			// 
			// cmbUseAntipass
			// 
			this.cmbUseAntipass.BackColor = System.Drawing.SystemColors.Window;
			this.cmbUseAntipass.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbUseAntipass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbUseAntipass.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbUseAntipass.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbUseAntipass.Items.AddRange(new object[] {
            "Use",
            "No Use"});
			this.cmbUseAntipass.Location = new System.Drawing.Point(138, 133);
			this.cmbUseAntipass.Name = "cmbUseAntipass";
			this.cmbUseAntipass.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbUseAntipass.Size = new System.Drawing.Size(81, 27);
			this.cmbUseAntipass.TabIndex = 52;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.SystemColors.Control;
			this.label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(45, 136);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label4.Size = new System.Drawing.Size(88, 19);
			this.label4.TabIndex = 51;
			this.label4.Text = "Use Antipass";
			// 
			// txtOpenTimeout
			// 
			this.txtOpenTimeout.AcceptsReturn = true;
			this.txtOpenTimeout.BackColor = System.Drawing.SystemColors.Window;
			this.txtOpenTimeout.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtOpenTimeout.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtOpenTimeout.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtOpenTimeout.Location = new System.Drawing.Point(138, 101);
			this.txtOpenTimeout.MaxLength = 8;
			this.txtOpenTimeout.Name = "txtOpenTimeout";
			this.txtOpenTimeout.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtOpenTimeout.Size = new System.Drawing.Size(81, 26);
			this.txtOpenTimeout.TabIndex = 50;
			this.txtOpenTimeout.Text = "10";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label3.Location = new System.Drawing.Point(2, 104);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label3.Size = new System.Drawing.Size(131, 19);
			this.label3.TabIndex = 49;
			this.label3.Text = "Door Open Timeout";
			// 
			// txtLockReleaseTime
			// 
			this.txtLockReleaseTime.AcceptsReturn = true;
			this.txtLockReleaseTime.BackColor = System.Drawing.SystemColors.Window;
			this.txtLockReleaseTime.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtLockReleaseTime.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLockReleaseTime.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtLockReleaseTime.Location = new System.Drawing.Point(138, 67);
			this.txtLockReleaseTime.MaxLength = 8;
			this.txtLockReleaseTime.Name = "txtLockReleaseTime";
			this.txtLockReleaseTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtLockReleaseTime.Size = new System.Drawing.Size(81, 26);
			this.txtLockReleaseTime.TabIndex = 48;
			this.txtLockReleaseTime.Text = "5";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.SystemColors.Control;
			this.label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label2.Location = new System.Drawing.Point(8, 70);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label2.Size = new System.Drawing.Size(125, 19);
			this.label2.TabIndex = 47;
			this.label2.Text = "Lock Release Time";
			// 
			// cmbSensorType
			// 
			this.cmbSensorType.BackColor = System.Drawing.SystemColors.Window;
			this.cmbSensorType.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmbSensorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSensorType.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbSensorType.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbSensorType.Items.AddRange(new object[] {
            "No",
            "N.O.",
            "N.C."});
			this.cmbSensorType.Location = new System.Drawing.Point(138, 33);
			this.cmbSensorType.Name = "cmbSensorType";
			this.cmbSensorType.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmbSensorType.Size = new System.Drawing.Size(81, 27);
			this.cmbSensorType.TabIndex = 46;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label1.Location = new System.Drawing.Point(12, 36);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label1.Size = new System.Drawing.Size(121, 19);
			this.label1.TabIndex = 40;
			this.label1.Text = "Door Sensor Type";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cmdWriteUnlockGroup);
			this.groupBox2.Controls.Add(this.cmdReadUnlockGroup);
			this.groupBox2.Controls.Add(this.txtUnlockGroup20);
			this.groupBox2.Controls.Add(this.txtUnlockGroup19);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.txtUnlockGroup18);
			this.groupBox2.Controls.Add(this.txtUnlockGroup17);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.txtUnlockGroup16);
			this.groupBox2.Controls.Add(this.txtUnlockGroup15);
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.txtUnlockGroup14);
			this.groupBox2.Controls.Add(this.txtUnlockGroup13);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.txtUnlockGroup12);
			this.groupBox2.Controls.Add(this.txtUnlockGroup11);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.txtUnlockGroup10);
			this.groupBox2.Controls.Add(this.txtUnlockGroup9);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.txtUnlockGroup8);
			this.groupBox2.Controls.Add(this.txtUnlockGroup7);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.txtUnlockGroup6);
			this.groupBox2.Controls.Add(this.txtUnlockGroup5);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.txtUnlockGroup4);
			this.groupBox2.Controls.Add(this.txtUnlockGroup3);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtUnlockGroup2);
			this.groupBox2.Controls.Add(this.txtUnlockGroup1);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold);
			this.groupBox2.Location = new System.Drawing.Point(16, 374);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(388, 228);
			this.groupBox2.TabIndex = 42;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Unlock Setting";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.BackColor = System.Drawing.SystemColors.Control;
			this.label7.Cursor = System.Windows.Forms.Cursors.Default;
			this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label7.Location = new System.Drawing.Point(16, 35);
			this.label7.Name = "label7";
			this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label7.Size = new System.Drawing.Size(107, 19);
			this.label7.TabIndex = 41;
			this.label7.Text = "Unlock Group1:";
			// 
			// txtUnlockGroup1
			// 
			this.txtUnlockGroup1.AcceptsReturn = true;
			this.txtUnlockGroup1.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup1.Location = new System.Drawing.Point(122, 34);
			this.txtUnlockGroup1.MaxLength = 8;
			this.txtUnlockGroup1.Name = "txtUnlockGroup1";
			this.txtUnlockGroup1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup1.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup1.TabIndex = 49;
			// 
			// txtUnlockGroup2
			// 
			this.txtUnlockGroup2.AcceptsReturn = true;
			this.txtUnlockGroup2.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup2.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup2.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup2.Location = new System.Drawing.Point(156, 34);
			this.txtUnlockGroup2.MaxLength = 8;
			this.txtUnlockGroup2.Name = "txtUnlockGroup2";
			this.txtUnlockGroup2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup2.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup2.TabIndex = 50;
			// 
			// txtUnlockGroup4
			// 
			this.txtUnlockGroup4.AcceptsReturn = true;
			this.txtUnlockGroup4.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup4.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup4.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup4.Location = new System.Drawing.Point(156, 64);
			this.txtUnlockGroup4.MaxLength = 8;
			this.txtUnlockGroup4.Name = "txtUnlockGroup4";
			this.txtUnlockGroup4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup4.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup4.TabIndex = 53;
			// 
			// txtUnlockGroup3
			// 
			this.txtUnlockGroup3.AcceptsReturn = true;
			this.txtUnlockGroup3.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup3.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup3.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup3.Location = new System.Drawing.Point(122, 64);
			this.txtUnlockGroup3.MaxLength = 8;
			this.txtUnlockGroup3.Name = "txtUnlockGroup3";
			this.txtUnlockGroup3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup3.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup3.TabIndex = 52;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.BackColor = System.Drawing.SystemColors.Control;
			this.label8.Cursor = System.Windows.Forms.Cursors.Default;
			this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label8.Location = new System.Drawing.Point(16, 65);
			this.label8.Name = "label8";
			this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label8.Size = new System.Drawing.Size(107, 19);
			this.label8.TabIndex = 51;
			this.label8.Text = "Unlock Group2:";
			// 
			// txtUnlockGroup6
			// 
			this.txtUnlockGroup6.AcceptsReturn = true;
			this.txtUnlockGroup6.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup6.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup6.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup6.Location = new System.Drawing.Point(156, 94);
			this.txtUnlockGroup6.MaxLength = 8;
			this.txtUnlockGroup6.Name = "txtUnlockGroup6";
			this.txtUnlockGroup6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup6.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup6.TabIndex = 56;
			// 
			// txtUnlockGroup5
			// 
			this.txtUnlockGroup5.AcceptsReturn = true;
			this.txtUnlockGroup5.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup5.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup5.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup5.Location = new System.Drawing.Point(122, 94);
			this.txtUnlockGroup5.MaxLength = 8;
			this.txtUnlockGroup5.Name = "txtUnlockGroup5";
			this.txtUnlockGroup5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup5.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup5.TabIndex = 55;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.BackColor = System.Drawing.SystemColors.Control;
			this.label9.Cursor = System.Windows.Forms.Cursors.Default;
			this.label9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label9.Location = new System.Drawing.Point(16, 95);
			this.label9.Name = "label9";
			this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label9.Size = new System.Drawing.Size(107, 19);
			this.label9.TabIndex = 54;
			this.label9.Text = "Unlock Group3:";
			// 
			// txtUnlockGroup8
			// 
			this.txtUnlockGroup8.AcceptsReturn = true;
			this.txtUnlockGroup8.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup8.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup8.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup8.Location = new System.Drawing.Point(156, 124);
			this.txtUnlockGroup8.MaxLength = 8;
			this.txtUnlockGroup8.Name = "txtUnlockGroup8";
			this.txtUnlockGroup8.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup8.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup8.TabIndex = 59;
			// 
			// txtUnlockGroup7
			// 
			this.txtUnlockGroup7.AcceptsReturn = true;
			this.txtUnlockGroup7.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup7.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup7.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup7.Location = new System.Drawing.Point(122, 124);
			this.txtUnlockGroup7.MaxLength = 8;
			this.txtUnlockGroup7.Name = "txtUnlockGroup7";
			this.txtUnlockGroup7.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup7.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup7.TabIndex = 58;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.BackColor = System.Drawing.SystemColors.Control;
			this.label10.Cursor = System.Windows.Forms.Cursors.Default;
			this.label10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label10.Location = new System.Drawing.Point(16, 125);
			this.label10.Name = "label10";
			this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label10.Size = new System.Drawing.Size(107, 19);
			this.label10.TabIndex = 57;
			this.label10.Text = "Unlock Group4:";
			// 
			// txtUnlockGroup10
			// 
			this.txtUnlockGroup10.AcceptsReturn = true;
			this.txtUnlockGroup10.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup10.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup10.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup10.Location = new System.Drawing.Point(156, 154);
			this.txtUnlockGroup10.MaxLength = 8;
			this.txtUnlockGroup10.Name = "txtUnlockGroup10";
			this.txtUnlockGroup10.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup10.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup10.TabIndex = 62;
			// 
			// txtUnlockGroup9
			// 
			this.txtUnlockGroup9.AcceptsReturn = true;
			this.txtUnlockGroup9.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup9.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup9.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup9.Location = new System.Drawing.Point(122, 154);
			this.txtUnlockGroup9.MaxLength = 8;
			this.txtUnlockGroup9.Name = "txtUnlockGroup9";
			this.txtUnlockGroup9.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup9.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup9.TabIndex = 61;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.BackColor = System.Drawing.SystemColors.Control;
			this.label11.Cursor = System.Windows.Forms.Cursors.Default;
			this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label11.Location = new System.Drawing.Point(16, 155);
			this.label11.Name = "label11";
			this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label11.Size = new System.Drawing.Size(107, 19);
			this.label11.TabIndex = 60;
			this.label11.Text = "Unlock Group5:";
			// 
			// txtUnlockGroup20
			// 
			this.txtUnlockGroup20.AcceptsReturn = true;
			this.txtUnlockGroup20.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup20.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup20.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup20.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup20.Location = new System.Drawing.Point(342, 154);
			this.txtUnlockGroup20.MaxLength = 8;
			this.txtUnlockGroup20.Name = "txtUnlockGroup20";
			this.txtUnlockGroup20.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup20.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup20.TabIndex = 77;
			// 
			// txtUnlockGroup19
			// 
			this.txtUnlockGroup19.AcceptsReturn = true;
			this.txtUnlockGroup19.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup19.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup19.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup19.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup19.Location = new System.Drawing.Point(308, 154);
			this.txtUnlockGroup19.MaxLength = 8;
			this.txtUnlockGroup19.Name = "txtUnlockGroup19";
			this.txtUnlockGroup19.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup19.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup19.TabIndex = 76;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.BackColor = System.Drawing.SystemColors.Control;
			this.label12.Cursor = System.Windows.Forms.Cursors.Default;
			this.label12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label12.Location = new System.Drawing.Point(197, 155);
			this.label12.Name = "label12";
			this.label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label12.Size = new System.Drawing.Size(115, 19);
			this.label12.TabIndex = 75;
			this.label12.Text = "Unlock Group10:";
			// 
			// txtUnlockGroup18
			// 
			this.txtUnlockGroup18.AcceptsReturn = true;
			this.txtUnlockGroup18.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup18.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup18.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup18.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup18.Location = new System.Drawing.Point(342, 124);
			this.txtUnlockGroup18.MaxLength = 8;
			this.txtUnlockGroup18.Name = "txtUnlockGroup18";
			this.txtUnlockGroup18.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup18.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup18.TabIndex = 74;
			// 
			// txtUnlockGroup17
			// 
			this.txtUnlockGroup17.AcceptsReturn = true;
			this.txtUnlockGroup17.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup17.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup17.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup17.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup17.Location = new System.Drawing.Point(308, 124);
			this.txtUnlockGroup17.MaxLength = 8;
			this.txtUnlockGroup17.Name = "txtUnlockGroup17";
			this.txtUnlockGroup17.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup17.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup17.TabIndex = 73;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.BackColor = System.Drawing.SystemColors.Control;
			this.label13.Cursor = System.Windows.Forms.Cursors.Default;
			this.label13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label13.Location = new System.Drawing.Point(202, 125);
			this.label13.Name = "label13";
			this.label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label13.Size = new System.Drawing.Size(107, 19);
			this.label13.TabIndex = 72;
			this.label13.Text = "Unlock Group9:";
			// 
			// txtUnlockGroup16
			// 
			this.txtUnlockGroup16.AcceptsReturn = true;
			this.txtUnlockGroup16.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup16.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup16.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup16.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup16.Location = new System.Drawing.Point(342, 94);
			this.txtUnlockGroup16.MaxLength = 8;
			this.txtUnlockGroup16.Name = "txtUnlockGroup16";
			this.txtUnlockGroup16.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup16.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup16.TabIndex = 71;
			// 
			// txtUnlockGroup15
			// 
			this.txtUnlockGroup15.AcceptsReturn = true;
			this.txtUnlockGroup15.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup15.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup15.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup15.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup15.Location = new System.Drawing.Point(308, 94);
			this.txtUnlockGroup15.MaxLength = 8;
			this.txtUnlockGroup15.Name = "txtUnlockGroup15";
			this.txtUnlockGroup15.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup15.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup15.TabIndex = 70;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.BackColor = System.Drawing.SystemColors.Control;
			this.label14.Cursor = System.Windows.Forms.Cursors.Default;
			this.label14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label14.Location = new System.Drawing.Point(202, 95);
			this.label14.Name = "label14";
			this.label14.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label14.Size = new System.Drawing.Size(107, 19);
			this.label14.TabIndex = 69;
			this.label14.Text = "Unlock Group8:";
			// 
			// txtUnlockGroup14
			// 
			this.txtUnlockGroup14.AcceptsReturn = true;
			this.txtUnlockGroup14.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup14.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup14.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup14.Location = new System.Drawing.Point(342, 64);
			this.txtUnlockGroup14.MaxLength = 8;
			this.txtUnlockGroup14.Name = "txtUnlockGroup14";
			this.txtUnlockGroup14.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup14.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup14.TabIndex = 68;
			// 
			// txtUnlockGroup13
			// 
			this.txtUnlockGroup13.AcceptsReturn = true;
			this.txtUnlockGroup13.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup13.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup13.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup13.Location = new System.Drawing.Point(308, 64);
			this.txtUnlockGroup13.MaxLength = 8;
			this.txtUnlockGroup13.Name = "txtUnlockGroup13";
			this.txtUnlockGroup13.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup13.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup13.TabIndex = 67;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.BackColor = System.Drawing.SystemColors.Control;
			this.label15.Cursor = System.Windows.Forms.Cursors.Default;
			this.label15.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label15.Location = new System.Drawing.Point(202, 65);
			this.label15.Name = "label15";
			this.label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label15.Size = new System.Drawing.Size(107, 19);
			this.label15.TabIndex = 66;
			this.label15.Text = "Unlock Group7:";
			// 
			// txtUnlockGroup12
			// 
			this.txtUnlockGroup12.AcceptsReturn = true;
			this.txtUnlockGroup12.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup12.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup12.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup12.Location = new System.Drawing.Point(342, 34);
			this.txtUnlockGroup12.MaxLength = 8;
			this.txtUnlockGroup12.Name = "txtUnlockGroup12";
			this.txtUnlockGroup12.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup12.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup12.TabIndex = 65;
			// 
			// txtUnlockGroup11
			// 
			this.txtUnlockGroup11.AcceptsReturn = true;
			this.txtUnlockGroup11.BackColor = System.Drawing.SystemColors.Window;
			this.txtUnlockGroup11.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtUnlockGroup11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUnlockGroup11.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnlockGroup11.Location = new System.Drawing.Point(308, 34);
			this.txtUnlockGroup11.MaxLength = 8;
			this.txtUnlockGroup11.Name = "txtUnlockGroup11";
			this.txtUnlockGroup11.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtUnlockGroup11.Size = new System.Drawing.Size(31, 26);
			this.txtUnlockGroup11.TabIndex = 64;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.BackColor = System.Drawing.SystemColors.Control;
			this.label16.Cursor = System.Windows.Forms.Cursors.Default;
			this.label16.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label16.Location = new System.Drawing.Point(202, 35);
			this.label16.Name = "label16";
			this.label16.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label16.Size = new System.Drawing.Size(107, 19);
			this.label16.TabIndex = 63;
			this.label16.Text = "Unlock Group6:";
			// 
			// cmdWriteUnlockGroup
			// 
			this.cmdWriteUnlockGroup.BackColor = System.Drawing.SystemColors.Control;
			this.cmdWriteUnlockGroup.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdWriteUnlockGroup.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdWriteUnlockGroup.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdWriteUnlockGroup.Location = new System.Drawing.Point(207, 186);
			this.cmdWriteUnlockGroup.Name = "cmdWriteUnlockGroup";
			this.cmdWriteUnlockGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdWriteUnlockGroup.Size = new System.Drawing.Size(91, 30);
			this.cmdWriteUnlockGroup.TabIndex = 79;
			this.cmdWriteUnlockGroup.Text = "Write";
			this.cmdWriteUnlockGroup.UseVisualStyleBackColor = false;
			this.cmdWriteUnlockGroup.Click += new System.EventHandler(this.cmdWriteUnlockGroup_Click);
			// 
			// cmdReadUnlockGroup
			// 
			this.cmdReadUnlockGroup.BackColor = System.Drawing.SystemColors.Control;
			this.cmdReadUnlockGroup.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdReadUnlockGroup.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdReadUnlockGroup.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdReadUnlockGroup.Location = new System.Drawing.Point(93, 186);
			this.cmdReadUnlockGroup.Name = "cmdReadUnlockGroup";
			this.cmdReadUnlockGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdReadUnlockGroup.Size = new System.Drawing.Size(93, 30);
			this.cmdReadUnlockGroup.TabIndex = 78;
			this.cmdReadUnlockGroup.Text = "Read";
			this.cmdReadUnlockGroup.UseVisualStyleBackColor = false;
			this.cmdReadUnlockGroup.Click += new System.EventHandler(this.cmdReadUnlockGroup_Click);
			// 
			// frmLockCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(418, 614);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtDoorNumber);
			this.Controls.Add(this._lblEnrollNum_0);
			this.Controls.Add(this.cmdWarnCancel);
			this.Controls.Add(this.cmdRestart);
			this.Controls.Add(this.cmdAutoRecover);
			this.Controls.Add(this.cmdUncondClose);
			this.Controls.Add(this.cmdUncondOpen);
			this.Controls.Add(this.cmdGetDoorStatus);
			this.Controls.Add(this.cmdDoorOpen);
			this.Controls.Add(this.lblMessage);
			this.MaximizeBox = false;
			this.Name = "frmLockCtrl";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmLockCrl";
			this.Load += new System.EventHandler(this.frmLockCrl_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLockCrl_FormClosed);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button cmdWarnCancel;
        public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.Button cmdRestart;
        public System.Windows.Forms.Button cmdAutoRecover;
        public System.Windows.Forms.Button cmdUncondClose;
        public System.Windows.Forms.Button cmdUncondOpen;
        public System.Windows.Forms.Button cmdGetDoorStatus;
        public System.Windows.Forms.Button cmdDoorOpen;
        public System.Windows.Forms.Label lblMessage;
        public System.Windows.Forms.TextBox txtDoorNumber;
        public System.Windows.Forms.Label _lblEnrollNum_0;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button cmdSet;
        public System.Windows.Forms.Button cmdGet;
        public System.Windows.Forms.ComboBox cmbLocation;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.ComboBox cmbAntipassNo;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox cmbUseAntipass;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtOpenTimeout;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtLockReleaseTime;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cmbSensorType;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.TextBox txtUnlockGroup2;
		public System.Windows.Forms.TextBox txtUnlockGroup1;
		public System.Windows.Forms.Label label7;
		public System.Windows.Forms.TextBox txtUnlockGroup10;
		public System.Windows.Forms.TextBox txtUnlockGroup9;
		public System.Windows.Forms.Label label11;
		public System.Windows.Forms.TextBox txtUnlockGroup8;
		public System.Windows.Forms.TextBox txtUnlockGroup7;
		public System.Windows.Forms.Label label10;
		public System.Windows.Forms.TextBox txtUnlockGroup6;
		public System.Windows.Forms.TextBox txtUnlockGroup5;
		public System.Windows.Forms.Label label9;
		public System.Windows.Forms.TextBox txtUnlockGroup4;
		public System.Windows.Forms.TextBox txtUnlockGroup3;
		public System.Windows.Forms.Label label8;
		public System.Windows.Forms.TextBox txtUnlockGroup20;
		public System.Windows.Forms.TextBox txtUnlockGroup19;
		public System.Windows.Forms.Label label12;
		public System.Windows.Forms.TextBox txtUnlockGroup18;
		public System.Windows.Forms.TextBox txtUnlockGroup17;
		public System.Windows.Forms.Label label13;
		public System.Windows.Forms.TextBox txtUnlockGroup16;
		public System.Windows.Forms.TextBox txtUnlockGroup15;
		public System.Windows.Forms.Label label14;
		public System.Windows.Forms.TextBox txtUnlockGroup14;
		public System.Windows.Forms.TextBox txtUnlockGroup13;
		public System.Windows.Forms.Label label15;
		public System.Windows.Forms.TextBox txtUnlockGroup12;
		public System.Windows.Forms.TextBox txtUnlockGroup11;
		public System.Windows.Forms.Label label16;
		public System.Windows.Forms.Button cmdWriteUnlockGroup;
		public System.Windows.Forms.Button cmdReadUnlockGroup;
    }
}