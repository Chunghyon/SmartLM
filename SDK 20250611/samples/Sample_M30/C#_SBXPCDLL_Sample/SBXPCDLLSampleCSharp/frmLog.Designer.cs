namespace SBXPCDLLSampleCSharp
{
	partial class frmLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmdExit = new System.Windows.Forms.Button();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblEnrollData = new System.Windows.Forms.Label();
            this.chkAndDelete = new System.Windows.Forms.CheckBox();
            this.chkReadMark = new System.Windows.Forms.CheckBox();
            this.LabelTotal = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdGlogData = new System.Windows.Forms.Button();
            this.cmdSLogData = new System.Windows.Forms.Button();
            this.cmdEmptySLog = new System.Windows.Forms.Button();
            this.cmdEmptyGLog = new System.Windows.Forms.Button();
            this.cmdAllGLogData = new System.Windows.Forms.Button();
            this.cmdAllSLogData = new System.Windows.Forms.Button();
            this.grdSlog = new System.Windows.Forms.DataGridView();
            this.tmachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.senroll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genroll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gmachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manipulation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finger = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdGlog = new System.Windows.Forms.DataGridView();
            this.photo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enroll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.verify_mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.glogtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSlog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGlog)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
            this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdExit.Location = new System.Drawing.Point(696, 488);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdExit.Size = new System.Drawing.Size(87, 43);
            this.cmdExit.TabIndex = 30;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // lblEnrollData
            // 
            this.lblEnrollData.AutoSize = true;
            this.lblEnrollData.BackColor = System.Drawing.SystemColors.Control;
            this.lblEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEnrollData.Location = new System.Drawing.Point(27, 75);
            this.lblEnrollData.Name = "lblEnrollData";
            this.lblEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblEnrollData.Size = new System.Drawing.Size(73, 19);
            this.lblEnrollData.TabIndex = 35;
            this.lblEnrollData.Text = "Log Data :";
            // 
            // chkAndDelete
            // 
            this.chkAndDelete.BackColor = System.Drawing.SystemColors.Control;
            this.chkAndDelete.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkAndDelete.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAndDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAndDelete.Location = new System.Drawing.Point(457, 74);
            this.chkAndDelete.Name = "chkAndDelete";
            this.chkAndDelete.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkAndDelete.Size = new System.Drawing.Size(138, 19);
            this.chkAndDelete.TabIndex = 38;
            this.chkAndDelete.Text = "and Delete ";
            this.chkAndDelete.UseVisualStyleBackColor = true;
            // 
            // chkReadMark
            // 
            this.chkReadMark.BackColor = System.Drawing.SystemColors.Control;
            this.chkReadMark.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkReadMark.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkReadMark.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkReadMark.Location = new System.Drawing.Point(601, 74);
            this.chkReadMark.Name = "chkReadMark";
            this.chkReadMark.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkReadMark.Size = new System.Drawing.Size(101, 19);
            this.chkReadMark.TabIndex = 33;
            this.chkReadMark.Text = "ReadMark";
            this.chkReadMark.UseVisualStyleBackColor = true;
            // 
            // LabelTotal
            // 
            this.LabelTotal.AutoSize = true;
            this.LabelTotal.BackColor = System.Drawing.SystemColors.Control;
            this.LabelTotal.Cursor = System.Windows.Forms.Cursors.Default;
            this.LabelTotal.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTotal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabelTotal.Location = new System.Drawing.Point(128, 75);
            this.LabelTotal.Name = "LabelTotal";
            this.LabelTotal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelTotal.Size = new System.Drawing.Size(46, 19);
            this.LabelTotal.TabIndex = 34;
            this.LabelTotal.Text = "Total :";
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(24, 29);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(759, 28);
            this.lblMessage.TabIndex = 27;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdGlogData);
            this.groupBox2.Controls.Add(this.cmdSLogData);
            this.groupBox2.Controls.Add(this.cmdEmptySLog);
            this.groupBox2.Controls.Add(this.cmdEmptyGLog);
            this.groupBox2.Controls.Add(this.cmdAllGLogData);
            this.groupBox2.Controls.Add(this.cmdAllSLogData);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.groupBox2.Location = new System.Drawing.Point(24, 463);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(652, 80);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Operating in a Traditional Way";
            // 
            // cmdGlogData
            // 
            this.cmdGlogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGlogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGlogData.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGlogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGlogData.Location = new System.Drawing.Point(343, 25);
            this.cmdGlogData.Name = "cmdGlogData";
            this.cmdGlogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGlogData.Size = new System.Drawing.Size(94, 43);
            this.cmdGlogData.TabIndex = 39;
            this.cmdGlogData.Text = "Read GLogData";
            this.cmdGlogData.UseVisualStyleBackColor = true;
            this.cmdGlogData.Click += new System.EventHandler(this.cmdGlogData_Click);
            // 
            // cmdSLogData
            // 
            this.cmdSLogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSLogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSLogData.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSLogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSLogData.Location = new System.Drawing.Point(19, 25);
            this.cmdSLogData.Name = "cmdSLogData";
            this.cmdSLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSLogData.Size = new System.Drawing.Size(94, 43);
            this.cmdSLogData.TabIndex = 38;
            this.cmdSLogData.Text = "Read SLogData";
            this.cmdSLogData.UseVisualStyleBackColor = true;
            this.cmdSLogData.Click += new System.EventHandler(this.cmdSLogData_Click);
            // 
            // cmdEmptySLog
            // 
            this.cmdEmptySLog.BackColor = System.Drawing.SystemColors.Control;
            this.cmdEmptySLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdEmptySLog.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEmptySLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdEmptySLog.Location = new System.Drawing.Point(219, 25);
            this.cmdEmptySLog.Name = "cmdEmptySLog";
            this.cmdEmptySLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdEmptySLog.Size = new System.Drawing.Size(94, 43);
            this.cmdEmptySLog.TabIndex = 43;
            this.cmdEmptySLog.Text = "Empty SLogData";
            this.cmdEmptySLog.UseVisualStyleBackColor = true;
            this.cmdEmptySLog.Click += new System.EventHandler(this.cmdEmptySLog_Click);
            // 
            // cmdEmptyGLog
            // 
            this.cmdEmptyGLog.BackColor = System.Drawing.SystemColors.Control;
            this.cmdEmptyGLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdEmptyGLog.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEmptyGLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdEmptyGLog.Location = new System.Drawing.Point(543, 25);
            this.cmdEmptyGLog.Name = "cmdEmptyGLog";
            this.cmdEmptyGLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdEmptyGLog.Size = new System.Drawing.Size(94, 43);
            this.cmdEmptyGLog.TabIndex = 42;
            this.cmdEmptyGLog.Text = "Empty GLogData";
            this.cmdEmptyGLog.UseVisualStyleBackColor = true;
            this.cmdEmptyGLog.Click += new System.EventHandler(this.cmdEmptyGLog_Click);
            // 
            // cmdAllGLogData
            // 
            this.cmdAllGLogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAllGLogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdAllGLogData.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAllGLogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdAllGLogData.Location = new System.Drawing.Point(443, 25);
            this.cmdAllGLogData.Name = "cmdAllGLogData";
            this.cmdAllGLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdAllGLogData.Size = new System.Drawing.Size(94, 43);
            this.cmdAllGLogData.TabIndex = 41;
            this.cmdAllGLogData.Text = "Read All GLogData";
            this.cmdAllGLogData.UseVisualStyleBackColor = true;
            this.cmdAllGLogData.Click += new System.EventHandler(this.cmdAllGLogData_Click);
            // 
            // cmdAllSLogData
            // 
            this.cmdAllSLogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAllSLogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdAllSLogData.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAllSLogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdAllSLogData.Location = new System.Drawing.Point(119, 25);
            this.cmdAllSLogData.Name = "cmdAllSLogData";
            this.cmdAllSLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdAllSLogData.Size = new System.Drawing.Size(94, 43);
            this.cmdAllSLogData.TabIndex = 40;
            this.cmdAllSLogData.Text = "Read All SLogData";
            this.cmdAllSLogData.UseVisualStyleBackColor = true;
            this.cmdAllSLogData.Click += new System.EventHandler(this.cmdAllSLogData_Click);
            // 
            // grdSlog
            // 
            this.grdSlog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSlog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tmachine,
            this.senroll,
            this.smachine,
            this.genroll,
            this.gmachine,
            this.manipulation,
            this.finger,
            this.logtime});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdSlog.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdSlog.Location = new System.Drawing.Point(24, 97);
            this.grdSlog.Name = "grdSlog";
            this.grdSlog.Size = new System.Drawing.Size(686, 353);
            this.grdSlog.TabIndex = 60;
            // 
            // tmachine
            // 
            this.tmachine.DataPropertyName = "tmachine";
            this.tmachine.HeaderText = "TMachineNo";
            this.tmachine.Name = "tmachine";
            this.tmachine.Width = 70;
            // 
            // senroll
            // 
            this.senroll.DataPropertyName = "senroll";
            this.senroll.HeaderText = "SEnrollNo";
            this.senroll.Name = "senroll";
            this.senroll.Width = 70;
            // 
            // smachine
            // 
            this.smachine.DataPropertyName = "smachine";
            this.smachine.HeaderText = "SMachineNo";
            this.smachine.Name = "smachine";
            this.smachine.Width = 70;
            // 
            // genroll
            // 
            this.genroll.DataPropertyName = "genroll";
            this.genroll.HeaderText = "GEnrollNo";
            this.genroll.Name = "genroll";
            this.genroll.Width = 70;
            // 
            // gmachine
            // 
            this.gmachine.DataPropertyName = "gmachine";
            this.gmachine.HeaderText = "GMachineNo";
            this.gmachine.Name = "gmachine";
            this.gmachine.Width = 70;
            // 
            // manipulation
            // 
            this.manipulation.DataPropertyName = "manipulation";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.manipulation.DefaultCellStyle = dataGridViewCellStyle1;
            this.manipulation.HeaderText = "Manipulation";
            this.manipulation.Name = "manipulation";
            this.manipulation.Width = 150;
            // 
            // finger
            // 
            this.finger.DataPropertyName = "finger";
            this.finger.HeaderText = "FP No";
            this.finger.Name = "finger";
            this.finger.Width = 60;
            // 
            // logtime
            // 
            this.logtime.DataPropertyName = "logtime";
            this.logtime.HeaderText = "Date & Time";
            this.logtime.Name = "logtime";
            this.logtime.Width = 120;
            // 
            // grdGlog
            // 
            this.grdGlog.AllowUserToAddRows = false;
            this.grdGlog.AllowUserToDeleteRows = false;
            this.grdGlog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdGlog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.photo,
            this.enroll,
            this.machine,
            this.verify_mode,
            this.glogtime});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdGlog.DefaultCellStyle = dataGridViewCellStyle4;
            this.grdGlog.Location = new System.Drawing.Point(24, 97);
            this.grdGlog.Name = "grdGlog";
            this.grdGlog.ReadOnly = true;
            this.grdGlog.Size = new System.Drawing.Size(759, 353);
            this.grdGlog.TabIndex = 61;
            // 
            // photo
            // 
            this.photo.DataPropertyName = "photo";
            this.photo.HeaderText = "PhotoNo";
            this.photo.Name = "photo";
            this.photo.ReadOnly = true;
            this.photo.Width = 80;
            // 
            // enroll
            // 
            this.enroll.DataPropertyName = "enroll";
            this.enroll.HeaderText = "EnrollNo";
            this.enroll.Name = "enroll";
            this.enroll.ReadOnly = true;
            this.enroll.Width = 80;
            // 
            // machine
            // 
            this.machine.DataPropertyName = "machine";
            this.machine.HeaderText = "MachineNo";
            this.machine.Name = "machine";
            this.machine.ReadOnly = true;
            this.machine.Width = 80;
            // 
            // verify_mode
            // 
            this.verify_mode.DataPropertyName = "verify_mode";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.verify_mode.DefaultCellStyle = dataGridViewCellStyle3;
            this.verify_mode.HeaderText = "VerifyMode";
            this.verify_mode.Name = "verify_mode";
            this.verify_mode.ReadOnly = true;
            this.verify_mode.Width = 180;
            // 
            // glogtime
            // 
            this.glogtime.DataPropertyName = "logtime";
            this.glogtime.HeaderText = "Date & Time";
            this.glogtime.Name = "glogtime";
            this.glogtime.ReadOnly = true;
            this.glogtime.Width = 120;
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 555);
            this.Controls.Add(this.grdGlog);
            this.Controls.Add(this.grdSlog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.lblEnrollData);
            this.Controls.Add(this.chkAndDelete);
            this.Controls.Add(this.chkReadMark);
            this.Controls.Add(this.LabelTotal);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmLog";
            this.Text = "frmLog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLog_FormClosing);
            this.Load += new System.EventHandler(this.frmLog_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSlog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGlog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button cmdExit;
		public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.Label lblEnrollData;
		public System.Windows.Forms.CheckBox chkAndDelete;
		public System.Windows.Forms.CheckBox chkReadMark;
        public System.Windows.Forms.Label LabelTotal;
        public System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.Button cmdGlogData;
		public System.Windows.Forms.Button cmdSLogData;
		public System.Windows.Forms.Button cmdEmptySLog;
		public System.Windows.Forms.Button cmdEmptyGLog;
		public System.Windows.Forms.Button cmdAllGLogData;
        public System.Windows.Forms.Button cmdAllSLogData;
		private System.Windows.Forms.DataGridView grdSlog;
		private System.Windows.Forms.DataGridView grdGlog;
		private System.Windows.Forms.DataGridViewTextBoxColumn tmachine;
		private System.Windows.Forms.DataGridViewTextBoxColumn senroll;
		private System.Windows.Forms.DataGridViewTextBoxColumn smachine;
		private System.Windows.Forms.DataGridViewTextBoxColumn genroll;
		private System.Windows.Forms.DataGridViewTextBoxColumn gmachine;
		private System.Windows.Forms.DataGridViewTextBoxColumn manipulation;
		private System.Windows.Forms.DataGridViewTextBoxColumn finger;
		private System.Windows.Forms.DataGridViewTextBoxColumn logtime;
		private System.Windows.Forms.DataGridViewTextBoxColumn photo;
		private System.Windows.Forms.DataGridViewTextBoxColumn enroll;
		private System.Windows.Forms.DataGridViewTextBoxColumn machine;
		private System.Windows.Forms.DataGridViewTextBoxColumn verify_mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn glogtime;
    }
}