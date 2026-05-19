namespace SBXPCSampleCSharp
{
    partial class frmBellInfo
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
			this.txtBellPeriod = new System.Windows.Forms.TextBox();
			this.txtBellCount = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.chkUsed = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbWeekday = new System.Windows.Forms.ComboBox();
			this.lstTimeZone = new System.Windows.Forms.ListBox();
			this.dtStart = new System.Windows.Forms.DateTimePicker();
			this.cmdUpdate = new System.Windows.Forms.Button();
			this.lblEnrollData = new System.Windows.Forms.Label();
			this.cmdExit = new System.Windows.Forms.Button();
			this.cmdWrite = new System.Windows.Forms.Button();
			this.cmdRead = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtBellPeriod
			// 
			this.txtBellPeriod.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBellPeriod.Location = new System.Drawing.Point(485, 59);
			this.txtBellPeriod.Margin = new System.Windows.Forms.Padding(4);
			this.txtBellPeriod.Name = "txtBellPeriod";
			this.txtBellPeriod.Size = new System.Drawing.Size(132, 30);
			this.txtBellPeriod.TabIndex = 104;
			this.txtBellPeriod.Text = "1";
			// 
			// txtBellCount
			// 
			this.txtBellCount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBellCount.Location = new System.Drawing.Point(166, 61);
			this.txtBellCount.Margin = new System.Windows.Forms.Padding(4);
			this.txtBellCount.Name = "txtBellCount";
			this.txtBellCount.Size = new System.Drawing.Size(132, 30);
			this.txtBellCount.TabIndex = 105;
			this.txtBellCount.Text = "0";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label3.Location = new System.Drawing.Point(357, 62);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label3.Size = new System.Drawing.Size(108, 22);
			this.label3.TabIndex = 100;
			this.label3.Text = "Bell Period:";
			// 
			// chkUsed
			// 
			this.chkUsed.AutoSize = true;
			this.chkUsed.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkUsed.Location = new System.Drawing.Point(43, 132);
			this.chkUsed.Margin = new System.Windows.Forms.Padding(4);
			this.chkUsed.Name = "chkUsed";
			this.chkUsed.Size = new System.Drawing.Size(72, 26);
			this.chkUsed.TabIndex = 103;
			this.chkUsed.Text = "Used";
			this.chkUsed.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.Cursor = System.Windows.Forms.Cursors.Default;
			this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label1.Location = new System.Drawing.Point(38, 63);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label1.Size = new System.Drawing.Size(101, 22);
			this.label1.TabIndex = 101;
			this.label1.Text = "Bell Count:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.SystemColors.Control;
			this.label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label2.Location = new System.Drawing.Point(494, 103);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label2.Size = new System.Drawing.Size(87, 22);
			this.label2.TabIndex = 102;
			this.label2.Text = "Weekday:";
			// 
			// cmbWeekday
			// 
			this.cmbWeekday.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbWeekday.Font = new System.Drawing.Font("Times New Roman", 12F);
			this.cmbWeekday.FormattingEnabled = true;
			this.cmbWeekday.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Everyday"});
			this.cmbWeekday.Location = new System.Drawing.Point(499, 132);
			this.cmbWeekday.Margin = new System.Windows.Forms.Padding(4);
			this.cmbWeekday.Name = "cmbWeekday";
			this.cmbWeekday.Size = new System.Drawing.Size(204, 30);
			this.cmbWeekday.TabIndex = 99;
			// 
			// lstTimeZone
			// 
			this.lstTimeZone.Font = new System.Drawing.Font("Times New Roman", 12F);
			this.lstTimeZone.FormattingEnabled = true;
			this.lstTimeZone.ItemHeight = 22;
			this.lstTimeZone.Location = new System.Drawing.Point(43, 173);
			this.lstTimeZone.Margin = new System.Windows.Forms.Padding(4);
			this.lstTimeZone.Name = "lstTimeZone";
			this.lstTimeZone.Size = new System.Drawing.Size(667, 466);
			this.lstTimeZone.TabIndex = 98;
			this.lstTimeZone.SelectedIndexChanged += new System.EventHandler(this.lstTimeZone_SelectedIndexChanged);
			// 
			// dtStart
			// 
			this.dtStart.CalendarFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
			this.dtStart.CustomFormat = "hh:mm:ss";
			this.dtStart.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
			this.dtStart.Font = new System.Drawing.Font("Times New Roman", 12F);
			this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtStart.Location = new System.Drawing.Point(231, 133);
			this.dtStart.Margin = new System.Windows.Forms.Padding(4);
			this.dtStart.Name = "dtStart";
			this.dtStart.Size = new System.Drawing.Size(157, 30);
			this.dtStart.TabIndex = 97;
			this.dtStart.Value = new System.DateTime(2011, 10, 12, 10, 44, 0, 0);
			// 
			// cmdUpdate
			// 
			this.cmdUpdate.BackColor = System.Drawing.SystemColors.Control;
			this.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdUpdate.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdUpdate.Location = new System.Drawing.Point(742, 174);
			this.cmdUpdate.Margin = new System.Windows.Forms.Padding(4);
			this.cmdUpdate.Name = "cmdUpdate";
			this.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdUpdate.Size = new System.Drawing.Size(139, 57);
			this.cmdUpdate.TabIndex = 96;
			this.cmdUpdate.Text = "Update";
			this.cmdUpdate.UseVisualStyleBackColor = false;
			this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
			// 
			// lblEnrollData
			// 
			this.lblEnrollData.AutoSize = true;
			this.lblEnrollData.BackColor = System.Drawing.SystemColors.Control;
			this.lblEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblEnrollData.Location = new System.Drawing.Point(226, 103);
			this.lblEnrollData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblEnrollData.Name = "lblEnrollData";
			this.lblEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblEnrollData.Size = new System.Drawing.Size(56, 22);
			this.lblEnrollData.TabIndex = 95;
			this.lblEnrollData.Text = "Time:";
			// 
			// cmdExit
			// 
			this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
			this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdExit.Location = new System.Drawing.Point(742, 600);
			this.cmdExit.Margin = new System.Windows.Forms.Padding(4);
			this.cmdExit.Name = "cmdExit";
			this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdExit.Size = new System.Drawing.Size(132, 42);
			this.cmdExit.TabIndex = 93;
			this.cmdExit.Text = "Exit";
			this.cmdExit.UseVisualStyleBackColor = false;
			this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
			// 
			// cmdWrite
			// 
			this.cmdWrite.BackColor = System.Drawing.SystemColors.Control;
			this.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdWrite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdWrite.Location = new System.Drawing.Point(742, 551);
			this.cmdWrite.Margin = new System.Windows.Forms.Padding(4);
			this.cmdWrite.Name = "cmdWrite";
			this.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdWrite.Size = new System.Drawing.Size(132, 42);
			this.cmdWrite.TabIndex = 92;
			this.cmdWrite.Text = "Write";
			this.cmdWrite.UseVisualStyleBackColor = false;
			this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
			// 
			// cmdRead
			// 
			this.cmdRead.BackColor = System.Drawing.SystemColors.Control;
			this.cmdRead.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdRead.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdRead.Location = new System.Drawing.Point(742, 501);
			this.cmdRead.Margin = new System.Windows.Forms.Padding(4);
			this.cmdRead.Name = "cmdRead";
			this.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdRead.Size = new System.Drawing.Size(132, 42);
			this.cmdRead.TabIndex = 91;
			this.cmdRead.Text = "Read";
			this.cmdRead.UseVisualStyleBackColor = false;
			this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
			this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblMessage.Location = new System.Drawing.Point(17, 18);
			this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblMessage.Size = new System.Drawing.Size(872, 34);
			this.lblMessage.TabIndex = 94;
			this.lblMessage.Text = "Message";
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// frmBellInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(907, 653);
			this.Controls.Add(this.txtBellPeriod);
			this.Controls.Add(this.txtBellCount);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.chkUsed);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbWeekday);
			this.Controls.Add(this.lstTimeZone);
			this.Controls.Add(this.dtStart);
			this.Controls.Add(this.cmdUpdate);
			this.Controls.Add(this.lblEnrollData);
			this.Controls.Add(this.cmdExit);
			this.Controls.Add(this.cmdWrite);
			this.Controls.Add(this.cmdRead);
			this.Controls.Add(this.lblMessage);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "frmBellInfo";
			this.Text = "frmBellInfo";
			this.Load += new System.EventHandler(this.frmBellInfo_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.TextBox txtBellPeriod;
		private System.Windows.Forms.TextBox txtBellCount;
		public System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkUsed;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbWeekday;
		private System.Windows.Forms.ListBox lstTimeZone;
		private System.Windows.Forms.DateTimePicker dtStart;
		public System.Windows.Forms.Button cmdUpdate;
		public System.Windows.Forms.Label lblEnrollData;
		public System.Windows.Forms.Button cmdExit;
		public System.Windows.Forms.Button cmdWrite;
		public System.Windows.Forms.Button cmdRead;
		public System.Windows.Forms.Label lblMessage;
	}
}