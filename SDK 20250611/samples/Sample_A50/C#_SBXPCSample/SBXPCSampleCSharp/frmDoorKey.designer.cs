namespace SBXPCSampleCSharp
{
    partial class frmDoorKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDoorKey));
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdDoorOpenTZWrite = new System.Windows.Forms.Button();
            this.cmdDoorOpenTZRead = new System.Windows.Forms.Button();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.gridDoorOpenTime = new AxMSFlexGridLib.AxMSFlexGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdUnlockGroupWrite = new System.Windows.Forms.Button();
            this.cmdUnlockGroupRead = new System.Windows.Forms.Button();
            this.cmbUnlockGroup = new System.Windows.Forms.ComboBox();
            this.gridUnlockGroupSet = new AxMSFlexGridLib.AxMSFlexGrid();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmdUserAccessGroupWrite = new System.Windows.Forms.Button();
            this.cmdUserAccessGroupRead = new System.Windows.Forms.Button();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.gridUserGroupSet = new AxMSFlexGridLib.AxMSFlexGrid();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmdSet = new System.Windows.Forms.Button();
            this.cmdGet = new System.Windows.Forms.Button();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbAntipassNo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbUseAntipass = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtOpenTimeout = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLockReleaseTime = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbSensorType = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDoorNumber = new System.Windows.Forms.TextBox();
            this._lblEnrollNum_0 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cmdAccessTimezoneWrite = new System.Windows.Forms.Button();
            this.cmdAccessTimezoneRead = new System.Windows.Forms.Button();
            this.txtTimezone3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTimezone2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTimezone1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cmdUserAcceptModeWrite = new System.Windows.Forms.Button();
            this.cmdUserAcceptModeRead = new System.Windows.Forms.Button();
            this.txtUserMode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDoorOpenTime)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUnlockGroupSet)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUserGroupSet)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(12, 18);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(589, 26);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdDoorOpenTZWrite);
            this.groupBox1.Controls.Add(this.cmdDoorOpenTZRead);
            this.groupBox1.Controls.Add(this.timePicker);
            this.groupBox1.Controls.Add(this.gridDoorOpenTime);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(12, 547);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 226);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "3. Door Open Timezone";
            // 
            // cmdDoorOpenTZWrite
            // 
            this.cmdDoorOpenTZWrite.BackColor = System.Drawing.SystemColors.Control;
            this.cmdDoorOpenTZWrite.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdDoorOpenTZWrite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDoorOpenTZWrite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdDoorOpenTZWrite.Location = new System.Drawing.Point(151, 193);
            this.cmdDoorOpenTZWrite.Name = "cmdDoorOpenTZWrite";
            this.cmdDoorOpenTZWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdDoorOpenTZWrite.Size = new System.Drawing.Size(85, 25);
            this.cmdDoorOpenTZWrite.TabIndex = 79;
            this.cmdDoorOpenTZWrite.Text = "Write";
            this.cmdDoorOpenTZWrite.UseVisualStyleBackColor = false;
            this.cmdDoorOpenTZWrite.Click += new System.EventHandler(this.cmdDoorOpenTZWrite_Click);
            // 
            // cmdDoorOpenTZRead
            // 
            this.cmdDoorOpenTZRead.BackColor = System.Drawing.SystemColors.Control;
            this.cmdDoorOpenTZRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdDoorOpenTZRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDoorOpenTZRead.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdDoorOpenTZRead.Location = new System.Drawing.Point(51, 193);
            this.cmdDoorOpenTZRead.Name = "cmdDoorOpenTZRead";
            this.cmdDoorOpenTZRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdDoorOpenTZRead.Size = new System.Drawing.Size(85, 25);
            this.cmdDoorOpenTZRead.TabIndex = 78;
            this.cmdDoorOpenTZRead.Text = "Read";
            this.cmdDoorOpenTZRead.UseVisualStyleBackColor = false;
            this.cmdDoorOpenTZRead.Click += new System.EventHandler(this.cmdDoorOpenTZRead_Click);
            // 
            // timePicker
            // 
            this.timePicker.CustomFormat = "HH:mm";
            this.timePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePicker.Location = new System.Drawing.Point(138, 63);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(67, 22);
            this.timePicker.TabIndex = 9;
            this.timePicker.Value = new System.DateTime(2010, 12, 21, 0, 0, 0, 0);
            this.timePicker.Visible = false;
            // 
            // gridDoorOpenTime
            // 
            this.gridDoorOpenTime.Location = new System.Drawing.Point(6, 38);
            this.gridDoorOpenTime.Name = "gridDoorOpenTime";
            this.gridDoorOpenTime.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gridDoorOpenTime.OcxState")));
            this.gridDoorOpenTime.Size = new System.Drawing.Size(283, 139);
            this.gridDoorOpenTime.TabIndex = 0;
            this.gridDoorOpenTime.ClickEvent += new System.EventHandler(this.gridDoorOpenTime_ClickEvent);
            this.gridDoorOpenTime.EnterCell += new System.EventHandler(this.gridDoorOpenTime_EnterCell);
            this.gridDoorOpenTime.LeaveCell += new System.EventHandler(this.gridDoorOpenTime_LeaveCell);
            this.gridDoorOpenTime.Scroll += new System.EventHandler(this.gridDoorOpenTime_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdUnlockGroupWrite);
            this.groupBox2.Controls.Add(this.cmdUnlockGroupRead);
            this.groupBox2.Controls.Add(this.cmbUnlockGroup);
            this.groupBox2.Controls.Add(this.gridUnlockGroupSet);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(326, 523);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(275, 250);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "6. Unlock Group";
            // 
            // cmdUnlockGroupWrite
            // 
            this.cmdUnlockGroupWrite.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUnlockGroupWrite.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUnlockGroupWrite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUnlockGroupWrite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUnlockGroupWrite.Location = new System.Drawing.Point(151, 217);
            this.cmdUnlockGroupWrite.Name = "cmdUnlockGroupWrite";
            this.cmdUnlockGroupWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUnlockGroupWrite.Size = new System.Drawing.Size(85, 25);
            this.cmdUnlockGroupWrite.TabIndex = 81;
            this.cmdUnlockGroupWrite.Text = "Write";
            this.cmdUnlockGroupWrite.UseVisualStyleBackColor = false;
            this.cmdUnlockGroupWrite.Click += new System.EventHandler(this.cmdUnlockGroupWrite_Click);
            // 
            // cmdUnlockGroupRead
            // 
            this.cmdUnlockGroupRead.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUnlockGroupRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUnlockGroupRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUnlockGroupRead.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUnlockGroupRead.Location = new System.Drawing.Point(49, 217);
            this.cmdUnlockGroupRead.Name = "cmdUnlockGroupRead";
            this.cmdUnlockGroupRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUnlockGroupRead.Size = new System.Drawing.Size(85, 25);
            this.cmdUnlockGroupRead.TabIndex = 80;
            this.cmdUnlockGroupRead.Text = "Read";
            this.cmdUnlockGroupRead.UseVisualStyleBackColor = false;
            this.cmdUnlockGroupRead.Click += new System.EventHandler(this.cmdUnlockGroupRead_Click);
            // 
            // cmbUnlockGroup
            // 
            this.cmbUnlockGroup.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUnlockGroup.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbUnlockGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnlockGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUnlockGroup.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbUnlockGroup.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "255"});
            this.cmbUnlockGroup.Location = new System.Drawing.Point(130, 94);
            this.cmbUnlockGroup.Name = "cmbUnlockGroup";
            this.cmbUnlockGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbUnlockGroup.Size = new System.Drawing.Size(63, 23);
            this.cmbUnlockGroup.TabIndex = 13;
            this.cmbUnlockGroup.Visible = false;
            // 
            // gridUnlockGroupSet
            // 
            this.gridUnlockGroupSet.Location = new System.Drawing.Point(21, 34);
            this.gridUnlockGroupSet.Name = "gridUnlockGroupSet";
            this.gridUnlockGroupSet.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gridUnlockGroupSet.OcxState")));
            this.gridUnlockGroupSet.Size = new System.Drawing.Size(245, 173);
            this.gridUnlockGroupSet.TabIndex = 0;
            this.gridUnlockGroupSet.ClickEvent += new System.EventHandler(this.gridUnlockGroupSet_ClickEvent);
            this.gridUnlockGroupSet.EnterCell += new System.EventHandler(this.gridUnlockGroupSet_EnterCell);
            this.gridUnlockGroupSet.LeaveCell += new System.EventHandler(this.gridUnlockGroupSet_LeaveCell);
            this.gridUnlockGroupSet.Scroll += new System.EventHandler(this.gridUnlockGroupSet_Scroll);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmdUserAccessGroupWrite);
            this.groupBox3.Controls.Add(this.cmdUserAccessGroupRead);
            this.groupBox3.Controls.Add(this.cmbGroup);
            this.groupBox3.Controls.Add(this.gridUserGroupSet);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(326, 229);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(275, 288);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "5. User Access Group";
            // 
            // cmdUserAccessGroupWrite
            // 
            this.cmdUserAccessGroupWrite.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUserAccessGroupWrite.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUserAccessGroupWrite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUserAccessGroupWrite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUserAccessGroupWrite.Location = new System.Drawing.Point(151, 253);
            this.cmdUserAccessGroupWrite.Name = "cmdUserAccessGroupWrite";
            this.cmdUserAccessGroupWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUserAccessGroupWrite.Size = new System.Drawing.Size(85, 25);
            this.cmdUserAccessGroupWrite.TabIndex = 79;
            this.cmdUserAccessGroupWrite.Text = "Write";
            this.cmdUserAccessGroupWrite.UseVisualStyleBackColor = false;
            this.cmdUserAccessGroupWrite.Click += new System.EventHandler(this.cmdUserAccessGroupWrite_Click);
            // 
            // cmdUserAccessGroupRead
            // 
            this.cmdUserAccessGroupRead.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUserAccessGroupRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUserAccessGroupRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUserAccessGroupRead.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUserAccessGroupRead.Location = new System.Drawing.Point(49, 253);
            this.cmdUserAccessGroupRead.Name = "cmdUserAccessGroupRead";
            this.cmdUserAccessGroupRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUserAccessGroupRead.Size = new System.Drawing.Size(85, 25);
            this.cmdUserAccessGroupRead.TabIndex = 78;
            this.cmdUserAccessGroupRead.Text = "Read";
            this.cmdUserAccessGroupRead.UseVisualStyleBackColor = false;
            this.cmdUserAccessGroupRead.Click += new System.EventHandler(this.cmdUserAccessGroupRead_Click);
            // 
            // cmbGroup
            // 
            this.cmbGroup.BackColor = System.Drawing.SystemColors.Window;
            this.cmbGroup.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGroup.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbGroup.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbGroup.Location = new System.Drawing.Point(113, 81);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbGroup.Size = new System.Drawing.Size(57, 23);
            this.cmbGroup.TabIndex = 21;
            this.cmbGroup.Visible = false;
            // 
            // gridUserGroupSet
            // 
            this.gridUserGroupSet.Location = new System.Drawing.Point(21, 26);
            this.gridUserGroupSet.Name = "gridUserGroupSet";
            this.gridUserGroupSet.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gridUserGroupSet.OcxState")));
            this.gridUserGroupSet.Size = new System.Drawing.Size(230, 221);
            this.gridUserGroupSet.TabIndex = 0;
            this.gridUserGroupSet.ClickEvent += new System.EventHandler(this.gridUserGroupSet_ClickEvent);
            this.gridUserGroupSet.EnterCell += new System.EventHandler(this.gridUserGroupSet_EnterCell);
            this.gridUserGroupSet.LeaveCell += new System.EventHandler(this.gridUserGroupSet_LeaveCell);
            this.gridUserGroupSet.Scroll += new System.EventHandler(this.gridUserGroupSet_Scroll);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cmdSet);
            this.groupBox5.Controls.Add(this.cmdGet);
            this.groupBox5.Controls.Add(this.cmbLocation);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.cmbAntipassNo);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.cmbUseAntipass);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.txtOpenTimeout);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.txtLockReleaseTime);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.cmbSensorType);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.Color.Black;
            this.groupBox5.Location = new System.Drawing.Point(12, 104);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(299, 261);
            this.groupBox5.TabIndex = 47;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "1. General";
            // 
            // cmdSet
            // 
            this.cmdSet.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSet.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSet.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSet.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSet.Location = new System.Drawing.Point(170, 223);
            this.cmdSet.Name = "cmdSet";
            this.cmdSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSet.Size = new System.Drawing.Size(91, 30);
            this.cmdSet.TabIndex = 72;
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
            this.cmdGet.Location = new System.Drawing.Point(56, 223);
            this.cmdGet.Name = "cmdGet";
            this.cmdGet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGet.Size = new System.Drawing.Size(93, 30);
            this.cmdGet.TabIndex = 71;
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
            this.cmbLocation.Location = new System.Drawing.Point(173, 190);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbLocation.Size = new System.Drawing.Size(81, 27);
            this.cmbLocation.TabIndex = 70;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(50, 193);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(118, 19);
            this.label6.TabIndex = 69;
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
            this.cmbAntipassNo.Location = new System.Drawing.Point(173, 157);
            this.cmbAntipassNo.Name = "cmbAntipassNo";
            this.cmbAntipassNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbAntipassNo.Size = new System.Drawing.Size(81, 27);
            this.cmbAntipassNo.TabIndex = 68;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(83, 160);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(85, 19);
            this.label7.TabIndex = 67;
            this.label7.Text = "Antipass No";
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
            this.cmbUseAntipass.Location = new System.Drawing.Point(173, 123);
            this.cmbUseAntipass.Name = "cmbUseAntipass";
            this.cmbUseAntipass.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbUseAntipass.Size = new System.Drawing.Size(81, 27);
            this.cmbUseAntipass.TabIndex = 66;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.SystemColors.Control;
            this.label12.Cursor = System.Windows.Forms.Cursors.Default;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(80, 126);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label12.Size = new System.Drawing.Size(88, 19);
            this.label12.TabIndex = 65;
            this.label12.Text = "Use Antipass";
            // 
            // txtOpenTimeout
            // 
            this.txtOpenTimeout.AcceptsReturn = true;
            this.txtOpenTimeout.BackColor = System.Drawing.SystemColors.Window;
            this.txtOpenTimeout.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOpenTimeout.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOpenTimeout.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOpenTimeout.Location = new System.Drawing.Point(173, 91);
            this.txtOpenTimeout.MaxLength = 8;
            this.txtOpenTimeout.Name = "txtOpenTimeout";
            this.txtOpenTimeout.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtOpenTimeout.Size = new System.Drawing.Size(81, 26);
            this.txtOpenTimeout.TabIndex = 64;
            this.txtOpenTimeout.Text = "10";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.SystemColors.Control;
            this.label13.Cursor = System.Windows.Forms.Cursors.Default;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(37, 94);
            this.label13.Name = "label13";
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label13.Size = new System.Drawing.Size(131, 19);
            this.label13.TabIndex = 63;
            this.label13.Text = "Door Open Timeout";
            // 
            // txtLockReleaseTime
            // 
            this.txtLockReleaseTime.AcceptsReturn = true;
            this.txtLockReleaseTime.BackColor = System.Drawing.SystemColors.Window;
            this.txtLockReleaseTime.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLockReleaseTime.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLockReleaseTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLockReleaseTime.Location = new System.Drawing.Point(173, 57);
            this.txtLockReleaseTime.MaxLength = 8;
            this.txtLockReleaseTime.Name = "txtLockReleaseTime";
            this.txtLockReleaseTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLockReleaseTime.Size = new System.Drawing.Size(81, 26);
            this.txtLockReleaseTime.TabIndex = 62;
            this.txtLockReleaseTime.Text = "5";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.Control;
            this.label14.Cursor = System.Windows.Forms.Cursors.Default;
            this.label14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(43, 60);
            this.label14.Name = "label14";
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label14.Size = new System.Drawing.Size(125, 19);
            this.label14.TabIndex = 61;
            this.label14.Text = "Lock Release Time";
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
            this.cmbSensorType.Location = new System.Drawing.Point(173, 23);
            this.cmbSensorType.Name = "cmbSensorType";
            this.cmbSensorType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbSensorType.Size = new System.Drawing.Size(81, 27);
            this.cmbSensorType.TabIndex = 60;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.SystemColors.Control;
            this.label15.Cursor = System.Windows.Forms.Cursors.Default;
            this.label15.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(47, 26);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label15.Size = new System.Drawing.Size(121, 19);
            this.label15.TabIndex = 59;
            this.label15.Text = "Door Sensor Type";
            // 
            // txtDoorNumber
            // 
            this.txtDoorNumber.AcceptsReturn = true;
            this.txtDoorNumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtDoorNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDoorNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDoorNumber.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDoorNumber.Location = new System.Drawing.Point(291, 69);
            this.txtDoorNumber.MaxLength = 8;
            this.txtDoorNumber.Name = "txtDoorNumber";
            this.txtDoorNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDoorNumber.Size = new System.Drawing.Size(135, 26);
            this.txtDoorNumber.TabIndex = 49;
            this.txtDoorNumber.Text = "0";
            // 
            // _lblEnrollNum_0
            // 
            this._lblEnrollNum_0.AutoSize = true;
            this._lblEnrollNum_0.BackColor = System.Drawing.SystemColors.Control;
            this._lblEnrollNum_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblEnrollNum_0.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblEnrollNum_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblEnrollNum_0.Location = new System.Drawing.Point(217, 73);
            this._lblEnrollNum_0.Name = "_lblEnrollNum_0";
            this._lblEnrollNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblEnrollNum_0.Size = new System.Drawing.Size(68, 19);
            this._lblEnrollNum_0.TabIndex = 48;
            this._lblEnrollNum_0.Text = "Door No:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cmdAccessTimezoneWrite);
            this.groupBox6.Controls.Add(this.cmdAccessTimezoneRead);
            this.groupBox6.Controls.Add(this.txtTimezone3);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.txtTimezone2);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.txtTimezone1);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.Color.Black;
            this.groupBox6.Location = new System.Drawing.Point(12, 371);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(299, 170);
            this.groupBox6.TabIndex = 79;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "2. Access Timezone";
            // 
            // cmdAccessTimezoneWrite
            // 
            this.cmdAccessTimezoneWrite.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAccessTimezoneWrite.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdAccessTimezoneWrite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAccessTimezoneWrite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdAccessTimezoneWrite.Location = new System.Drawing.Point(151, 131);
            this.cmdAccessTimezoneWrite.Name = "cmdAccessTimezoneWrite";
            this.cmdAccessTimezoneWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdAccessTimezoneWrite.Size = new System.Drawing.Size(85, 25);
            this.cmdAccessTimezoneWrite.TabIndex = 77;
            this.cmdAccessTimezoneWrite.Text = "Write";
            this.cmdAccessTimezoneWrite.UseVisualStyleBackColor = false;
            this.cmdAccessTimezoneWrite.Click += new System.EventHandler(this.cmdAccessTimezoneWrite_Click);
            // 
            // cmdAccessTimezoneRead
            // 
            this.cmdAccessTimezoneRead.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAccessTimezoneRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdAccessTimezoneRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAccessTimezoneRead.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdAccessTimezoneRead.Location = new System.Drawing.Point(49, 131);
            this.cmdAccessTimezoneRead.Name = "cmdAccessTimezoneRead";
            this.cmdAccessTimezoneRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdAccessTimezoneRead.Size = new System.Drawing.Size(85, 25);
            this.cmdAccessTimezoneRead.TabIndex = 76;
            this.cmdAccessTimezoneRead.Text = "Read";
            this.cmdAccessTimezoneRead.UseVisualStyleBackColor = false;
            this.cmdAccessTimezoneRead.Click += new System.EventHandler(this.cmdAccessTimezoneRead_Click);
            // 
            // txtTimezone3
            // 
            this.txtTimezone3.AcceptsReturn = true;
            this.txtTimezone3.BackColor = System.Drawing.SystemColors.Window;
            this.txtTimezone3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimezone3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimezone3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTimezone3.Location = new System.Drawing.Point(188, 93);
            this.txtTimezone3.MaxLength = 8;
            this.txtTimezone3.Name = "txtTimezone3";
            this.txtTimezone3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTimezone3.Size = new System.Drawing.Size(85, 26);
            this.txtTimezone3.TabIndex = 52;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(26, 96);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(144, 18);
            this.label8.TabIndex = 51;
            this.label8.Text = "Timezone 3:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTimezone2
            // 
            this.txtTimezone2.AcceptsReturn = true;
            this.txtTimezone2.BackColor = System.Drawing.SystemColors.Window;
            this.txtTimezone2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimezone2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimezone2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTimezone2.Location = new System.Drawing.Point(188, 65);
            this.txtTimezone2.MaxLength = 8;
            this.txtTimezone2.Name = "txtTimezone2";
            this.txtTimezone2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTimezone2.Size = new System.Drawing.Size(85, 26);
            this.txtTimezone2.TabIndex = 50;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(26, 67);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(144, 18);
            this.label9.TabIndex = 49;
            this.label9.Text = "Timezone 2:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTimezone1
            // 
            this.txtTimezone1.AcceptsReturn = true;
            this.txtTimezone1.BackColor = System.Drawing.SystemColors.Window;
            this.txtTimezone1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimezone1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimezone1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTimezone1.Location = new System.Drawing.Point(188, 36);
            this.txtTimezone1.MaxLength = 8;
            this.txtTimezone1.Name = "txtTimezone1";
            this.txtTimezone1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTimezone1.Size = new System.Drawing.Size(85, 26);
            this.txtTimezone1.TabIndex = 48;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.Control;
            this.label10.Cursor = System.Windows.Forms.Cursors.Default;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(26, 39);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(144, 18);
            this.label10.TabIndex = 47;
            this.label10.Text = "Timezone1:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cmdUserAcceptModeWrite);
            this.groupBox7.Controls.Add(this.cmdUserAcceptModeRead);
            this.groupBox7.Controls.Add(this.txtUserMode);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.ForeColor = System.Drawing.Color.Black;
            this.groupBox7.Location = new System.Drawing.Point(326, 104);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(275, 116);
            this.groupBox7.TabIndex = 81;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "4. User Accept Mode";
            // 
            // cmdUserAcceptModeWrite
            // 
            this.cmdUserAcceptModeWrite.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUserAcceptModeWrite.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUserAcceptModeWrite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUserAcceptModeWrite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUserAcceptModeWrite.Location = new System.Drawing.Point(151, 77);
            this.cmdUserAcceptModeWrite.Name = "cmdUserAcceptModeWrite";
            this.cmdUserAcceptModeWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUserAcceptModeWrite.Size = new System.Drawing.Size(85, 25);
            this.cmdUserAcceptModeWrite.TabIndex = 77;
            this.cmdUserAcceptModeWrite.Text = "Write";
            this.cmdUserAcceptModeWrite.UseVisualStyleBackColor = false;
            this.cmdUserAcceptModeWrite.Click += new System.EventHandler(this.cmdUserAcceptModeWrite_Click);
            // 
            // cmdUserAcceptModeRead
            // 
            this.cmdUserAcceptModeRead.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUserAcceptModeRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUserAcceptModeRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUserAcceptModeRead.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUserAcceptModeRead.Location = new System.Drawing.Point(49, 77);
            this.cmdUserAcceptModeRead.Name = "cmdUserAcceptModeRead";
            this.cmdUserAcceptModeRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUserAcceptModeRead.Size = new System.Drawing.Size(85, 25);
            this.cmdUserAcceptModeRead.TabIndex = 76;
            this.cmdUserAcceptModeRead.Text = "Read";
            this.cmdUserAcceptModeRead.UseVisualStyleBackColor = false;
            this.cmdUserAcceptModeRead.Click += new System.EventHandler(this.cmdUserAcceptModeRead_Click);
            // 
            // txtUserMode
            // 
            this.txtUserMode.AcceptsReturn = true;
            this.txtUserMode.BackColor = System.Drawing.SystemColors.Window;
            this.txtUserMode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUserMode.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserMode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtUserMode.Location = new System.Drawing.Point(140, 36);
            this.txtUserMode.MaxLength = 8;
            this.txtUserMode.Name = "txtUserMode";
            this.txtUserMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUserMode.Size = new System.Drawing.Size(85, 26);
            this.txtUserMode.TabIndex = 48;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.Control;
            this.label11.Cursor = System.Windows.Forms.Cursors.Default;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(26, 39);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(108, 18);
            this.label11.TabIndex = 47;
            this.label11.Text = "User Mode:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmDoorKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 785);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.txtDoorNumber);
            this.Controls.Add(this._lblEnrollNum_0);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmDoorKey";
            this.Text = "frmDoorKey";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDoorKey_FormClosed);
            this.Load += new System.EventHandler(this.frmDoorKey_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDoorOpenTime)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUnlockGroupSet)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUserGroupSet)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private AxMSFlexGridLib.AxMSFlexGrid gridDoorOpenTime;
        private AxMSFlexGridLib.AxMSFlexGrid gridUnlockGroupSet;
        private AxMSFlexGridLib.AxMSFlexGrid gridUserGroupSet;
        public System.Windows.Forms.ComboBox cmbUnlockGroup;
        private System.Windows.Forms.DateTimePicker timePicker;
        public System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.TextBox txtDoorNumber;
        public System.Windows.Forms.Label _lblEnrollNum_0;
        private System.Windows.Forms.GroupBox groupBox6;
        public System.Windows.Forms.Button cmdAccessTimezoneWrite;
        public System.Windows.Forms.Button cmdAccessTimezoneRead;
        public System.Windows.Forms.TextBox txtTimezone3;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox txtTimezone2;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox txtTimezone1;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.Button cmdDoorOpenTZWrite;
        public System.Windows.Forms.Button cmdDoorOpenTZRead;
        private System.Windows.Forms.GroupBox groupBox7;
        public System.Windows.Forms.Button cmdUserAcceptModeWrite;
        public System.Windows.Forms.Button cmdUserAcceptModeRead;
        public System.Windows.Forms.TextBox txtUserMode;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.Button cmdUserAccessGroupWrite;
        public System.Windows.Forms.Button cmdUserAccessGroupRead;
        public System.Windows.Forms.Button cmdUnlockGroupWrite;
        public System.Windows.Forms.Button cmdUnlockGroupRead;
        public System.Windows.Forms.Button cmdSet;
        public System.Windows.Forms.Button cmdGet;
        public System.Windows.Forms.ComboBox cmbLocation;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.ComboBox cmbAntipassNo;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox cmbUseAntipass;
        public System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox txtOpenTimeout;
        public System.Windows.Forms.Label label13;
        public System.Windows.Forms.TextBox txtLockReleaseTime;
        public System.Windows.Forms.Label label14;
        public System.Windows.Forms.ComboBox cmbSensorType;
        public System.Windows.Forms.Label label15;
    }
}