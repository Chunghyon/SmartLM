namespace SBXPCSampleCSharp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLog));
            this.cmdExit = new System.Windows.Forms.Button();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdGlogData = new System.Windows.Forms.Button();
            this.cmdSLogData = new System.Windows.Forms.Button();
            this.lblEnrollData = new System.Windows.Forms.Label();
            this.chkAndDelete = new System.Windows.Forms.CheckBox();
            this.cmdEmptySLog = new System.Windows.Forms.Button();
            this.cmdEmptyGLog = new System.Windows.Forms.Button();
            this.chkReadMark = new System.Windows.Forms.CheckBox();
            this.cmdAllGLogData = new System.Windows.Forms.Button();
            this.cmdAllSLogData = new System.Windows.Forms.Button();
            this.LabelTotal = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.gridSLogData2 = new AxMSFlexGridLib.AxMSFlexGrid();
            this.gridSLogData1 = new AxMSFlexGridLib.AxMSFlexGrid();
            this.gridSLogData = new AxMSFlexGridLib.AxMSFlexGrid();
            this.chkShowGlogPhoto = new System.Windows.Forms.CheckBox();
            this.picGlogPhoto = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetRange = new System.Windows.Forms.Button();
            this.dtGlogSearchEnd = new System.Windows.Forms.DateTimePicker();
            this.dtGlogSearchStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSearchRangeUse = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridSLogData2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSLogData1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSLogData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGlogPhoto)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
            this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdExit.Location = new System.Drawing.Point(608, 415);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdExit.Size = new System.Drawing.Size(94, 43);
            this.cmdExit.TabIndex = 30;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdGlogData
            // 
            this.cmdGlogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGlogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGlogData.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGlogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGlogData.Location = new System.Drawing.Point(317, 415);
            this.cmdGlogData.Name = "cmdGlogData";
            this.cmdGlogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGlogData.Size = new System.Drawing.Size(94, 43);
            this.cmdGlogData.TabIndex = 29;
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
            this.cmdSLogData.Location = new System.Drawing.Point(24, 415);
            this.cmdSLogData.Name = "cmdSLogData";
            this.cmdSLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSLogData.Size = new System.Drawing.Size(94, 43);
            this.cmdSLogData.TabIndex = 28;
            this.cmdSLogData.Text = "Read SLogData";
            this.cmdSLogData.UseVisualStyleBackColor = true;
            this.cmdSLogData.Click += new System.EventHandler(this.cmdSLogData_Click);
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
            // cmdEmptySLog
            // 
            this.cmdEmptySLog.BackColor = System.Drawing.SystemColors.Control;
            this.cmdEmptySLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdEmptySLog.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEmptySLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdEmptySLog.Location = new System.Drawing.Point(220, 415);
            this.cmdEmptySLog.Name = "cmdEmptySLog";
            this.cmdEmptySLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdEmptySLog.Size = new System.Drawing.Size(94, 43);
            this.cmdEmptySLog.TabIndex = 37;
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
            this.cmdEmptyGLog.Location = new System.Drawing.Point(513, 415);
            this.cmdEmptyGLog.Name = "cmdEmptyGLog";
            this.cmdEmptyGLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdEmptyGLog.Size = new System.Drawing.Size(94, 43);
            this.cmdEmptyGLog.TabIndex = 36;
            this.cmdEmptyGLog.Text = "Empty GLogData";
            this.cmdEmptyGLog.UseVisualStyleBackColor = true;
            this.cmdEmptyGLog.Click += new System.EventHandler(this.cmdEmptyGLog_Click);
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
            this.chkReadMark.CheckedChanged += new System.EventHandler(this.chkReadMark_CheckedChanged);
            // 
            // cmdAllGLogData
            // 
            this.cmdAllGLogData.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAllGLogData.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdAllGLogData.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAllGLogData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdAllGLogData.Location = new System.Drawing.Point(415, 415);
            this.cmdAllGLogData.Name = "cmdAllGLogData";
            this.cmdAllGLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdAllGLogData.Size = new System.Drawing.Size(94, 43);
            this.cmdAllGLogData.TabIndex = 32;
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
            this.cmdAllSLogData.Location = new System.Drawing.Point(122, 415);
            this.cmdAllSLogData.Name = "cmdAllSLogData";
            this.cmdAllSLogData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdAllSLogData.Size = new System.Drawing.Size(94, 43);
            this.cmdAllSLogData.TabIndex = 31;
            this.cmdAllSLogData.Text = "Read All SLogData";
            this.cmdAllSLogData.UseVisualStyleBackColor = true;
            this.cmdAllSLogData.Click += new System.EventHandler(this.cmdAllSLogData_Click);
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
            this.LabelTotal.Size = new System.Drawing.Size(47, 19);
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
            this.lblMessage.Size = new System.Drawing.Size(871, 28);
            this.lblMessage.TabIndex = 27;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // gridSLogData2
            // 
            this.gridSLogData2.Location = new System.Drawing.Point(24, 329);
            this.gridSLogData2.Name = "gridSLogData2";
            this.gridSLogData2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gridSLogData2.OcxState")));
            this.gridSLogData2.Size = new System.Drawing.Size(678, 66);
            this.gridSLogData2.TabIndex = 39;
            // 
            // gridSLogData1
            // 
            this.gridSLogData1.Location = new System.Drawing.Point(24, 263);
            this.gridSLogData1.Name = "gridSLogData1";
            this.gridSLogData1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gridSLogData1.OcxState")));
            this.gridSLogData1.Size = new System.Drawing.Size(678, 132);
            this.gridSLogData1.TabIndex = 40;
            // 
            // gridSLogData
            // 
            this.gridSLogData.Location = new System.Drawing.Point(24, 97);
            this.gridSLogData.Name = "gridSLogData";
            this.gridSLogData.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("gridSLogData.OcxState")));
            this.gridSLogData.Size = new System.Drawing.Size(679, 298);
            this.gridSLogData.TabIndex = 41;
            this.gridSLogData.ClickEvent += new System.EventHandler(this.gridSLogData_ClickEvent);
            // 
            // chkShowGlogPhoto
            // 
            this.chkShowGlogPhoto.BackColor = System.Drawing.SystemColors.Control;
            this.chkShowGlogPhoto.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkShowGlogPhoto.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowGlogPhoto.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkShowGlogPhoto.Location = new System.Drawing.Point(727, 74);
            this.chkShowGlogPhoto.Name = "chkShowGlogPhoto";
            this.chkShowGlogPhoto.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkShowGlogPhoto.Size = new System.Drawing.Size(151, 25);
            this.chkShowGlogPhoto.TabIndex = 42;
            this.chkShowGlogPhoto.Text = "Show Glog Photo";
            this.chkShowGlogPhoto.UseVisualStyleBackColor = true;
            // 
            // picGlogPhoto
            // 
            this.picGlogPhoto.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picGlogPhoto.Location = new System.Drawing.Point(719, 105);
            this.picGlogPhoto.Name = "picGlogPhoto";
            this.picGlogPhoto.Size = new System.Drawing.Size(164, 129);
            this.picGlogPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGlogPhoto.TabIndex = 43;
            this.picGlogPhoto.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSetRange);
            this.groupBox1.Controls.Add(this.dtGlogSearchEnd);
            this.groupBox1.Controls.Add(this.dtGlogSearchStart);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkSearchRangeUse);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.groupBox1.Location = new System.Drawing.Point(717, 240);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 182);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glog Search Range";
            // 
            // btnSetRange
            // 
            this.btnSetRange.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.btnSetRange.Location = new System.Drawing.Point(33, 138);
            this.btnSetRange.Name = "btnSetRange";
            this.btnSetRange.Size = new System.Drawing.Size(116, 27);
            this.btnSetRange.TabIndex = 5;
            this.btnSetRange.Text = "Set Range";
            this.btnSetRange.UseVisualStyleBackColor = true;
            this.btnSetRange.Click += new System.EventHandler(this.btnSetRange_Click);
            // 
            // dtGlogSearchEnd
            // 
            this.dtGlogSearchEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtGlogSearchEnd.Location = new System.Drawing.Point(60, 98);
            this.dtGlogSearchEnd.Name = "dtGlogSearchEnd";
            this.dtGlogSearchEnd.Size = new System.Drawing.Size(118, 26);
            this.dtGlogSearchEnd.TabIndex = 4;
            // 
            // dtGlogSearchStart
            // 
            this.dtGlogSearchStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtGlogSearchStart.Location = new System.Drawing.Point(60, 64);
            this.dtGlogSearchStart.Name = "dtGlogSearchStart";
            this.dtGlogSearchStart.Size = new System.Drawing.Size(118, 26);
            this.dtGlogSearchStart.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "End:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start:";
            // 
            // chkSearchRangeUse
            // 
            this.chkSearchRangeUse.AutoSize = true;
            this.chkSearchRangeUse.Location = new System.Drawing.Point(21, 25);
            this.chkSearchRangeUse.Name = "chkSearchRangeUse";
            this.chkSearchRangeUse.Size = new System.Drawing.Size(140, 23);
            this.chkSearchRangeUse.TabIndex = 0;
            this.chkSearchRangeUse.Text = "Use Search Range";
            this.chkSearchRangeUse.UseVisualStyleBackColor = true;
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 468);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picGlogPhoto);
            this.Controls.Add(this.chkShowGlogPhoto);
            this.Controls.Add(this.gridSLogData);
            this.Controls.Add(this.gridSLogData1);
            this.Controls.Add(this.gridSLogData2);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdGlogData);
            this.Controls.Add(this.cmdSLogData);
            this.Controls.Add(this.lblEnrollData);
            this.Controls.Add(this.chkAndDelete);
            this.Controls.Add(this.cmdEmptySLog);
            this.Controls.Add(this.cmdEmptyGLog);
            this.Controls.Add(this.chkReadMark);
            this.Controls.Add(this.cmdAllGLogData);
            this.Controls.Add(this.cmdAllSLogData);
            this.Controls.Add(this.LabelTotal);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmLog";
            this.Text = "frmLog";
            this.Load += new System.EventHandler(this.frmLog_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridSLogData2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSLogData1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSLogData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGlogPhoto)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button cmdExit;
        public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.Button cmdGlogData;
        public System.Windows.Forms.Button cmdSLogData;
        public System.Windows.Forms.Label lblEnrollData;
        public System.Windows.Forms.CheckBox chkAndDelete;
        public System.Windows.Forms.Button cmdEmptySLog;
        public System.Windows.Forms.Button cmdEmptyGLog;
        public System.Windows.Forms.CheckBox chkReadMark;
        public System.Windows.Forms.Button cmdAllGLogData;
        public System.Windows.Forms.Button cmdAllSLogData;
        public System.Windows.Forms.Label LabelTotal;
        public System.Windows.Forms.Label lblMessage;
        private AxMSFlexGridLib.AxMSFlexGrid gridSLogData2;
        private AxMSFlexGridLib.AxMSFlexGrid gridSLogData1;
        private AxMSFlexGridLib.AxMSFlexGrid gridSLogData;
        public System.Windows.Forms.CheckBox chkShowGlogPhoto;
        private System.Windows.Forms.PictureBox picGlogPhoto;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSearchRangeUse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtGlogSearchEnd;
        private System.Windows.Forms.DateTimePicker dtGlogSearchStart;
        private System.Windows.Forms.Button btnSetRange;
    }
}