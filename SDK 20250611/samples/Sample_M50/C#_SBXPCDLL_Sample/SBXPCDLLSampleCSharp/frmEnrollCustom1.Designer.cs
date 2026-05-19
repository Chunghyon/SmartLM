namespace SBXPCDLLSampleCSharp
{
    partial class frmEnrollCustom1
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
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtEnrollNumber = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.TextBox();
            this.lblEnrollNumber = new System.Windows.Forms.Label();
            this.OpenFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.lblUserMessage = new System.Windows.Forms.Label();
            this.txtUserMessage = new System.Windows.Forms.TextBox();
            this.lblHolidays = new System.Windows.Forms.Label();
            this.txtHolidays = new System.Windows.Forms.TextBox();
            this.lblBalanceTime = new System.Windows.Forms.Label();
            this.btnGetUserMessage = new System.Windows.Forms.Button();
            this.dtBalanceTime = new System.Windows.Forms.DateTimePicker();
            this.btnSetUserMessage = new System.Windows.Forms.Button();
            this.btnGetUserBalanceTime = new System.Windows.Forms.Button();
            this.btnSetUserBalanceTime = new System.Windows.Forms.Button();
            this.btnGetUserHolidays = new System.Windows.Forms.Button();
            this.btnSetUserHolidays = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtEnrollNumber
            // 
            this.txtEnrollNumber.AcceptsReturn = true;
            this.txtEnrollNumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtEnrollNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEnrollNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEnrollNumber.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEnrollNumber.Location = new System.Drawing.Point(150, 64);
            this.txtEnrollNumber.MaxLength = 8;
            this.txtEnrollNumber.Name = "txtEnrollNumber";
            this.txtEnrollNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEnrollNumber.Size = new System.Drawing.Size(119, 26);
            this.txtEnrollNumber.TabIndex = 38;
            this.txtEnrollNumber.Text = "1";
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(14, 17);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.ReadOnly = true;
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(431, 29);
            this.lblMessage.TabIndex = 42;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEnrollNumber
            // 
            this.lblEnrollNumber.AutoSize = true;
            this.lblEnrollNumber.BackColor = System.Drawing.SystemColors.Control;
            this.lblEnrollNumber.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblEnrollNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnrollNumber.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEnrollNumber.Location = new System.Drawing.Point(22, 68);
            this.lblEnrollNumber.Name = "lblEnrollNumber";
            this.lblEnrollNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblEnrollNumber.Size = new System.Drawing.Size(105, 19);
            this.lblEnrollNumber.TabIndex = 37;
            this.lblEnrollNumber.Text = "Enroll Number :";
            // 
            // OpenFileDlg
            // 
            this.OpenFileDlg.FileName = "openFileDialog1";
            // 
            // lblUserMessage
            // 
            this.lblUserMessage.AutoSize = true;
            this.lblUserMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblUserMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUserMessage.Location = new System.Drawing.Point(57, 103);
            this.lblUserMessage.Name = "lblUserMessage";
            this.lblUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUserMessage.Size = new System.Drawing.Size(70, 19);
            this.lblUserMessage.TabIndex = 53;
            this.lblUserMessage.Text = "Message :";
            // 
            // txtUserMessage
            // 
            this.txtUserMessage.AcceptsReturn = true;
            this.txtUserMessage.BackColor = System.Drawing.SystemColors.Window;
            this.txtUserMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserMessage.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtUserMessage.Location = new System.Drawing.Point(150, 103);
            this.txtUserMessage.MaxLength = 100;
            this.txtUserMessage.Multiline = true;
            this.txtUserMessage.Name = "txtUserMessage";
            this.txtUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUserMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUserMessage.Size = new System.Drawing.Size(295, 70);
            this.txtUserMessage.TabIndex = 52;
            // 
            // lblHolidays
            // 
            this.lblHolidays.AutoSize = true;
            this.lblHolidays.BackColor = System.Drawing.SystemColors.Control;
            this.lblHolidays.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblHolidays.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHolidays.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblHolidays.Location = new System.Drawing.Point(62, 215);
            this.lblHolidays.Name = "lblHolidays";
            this.lblHolidays.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblHolidays.Size = new System.Drawing.Size(65, 19);
            this.lblHolidays.TabIndex = 88;
            this.lblHolidays.Text = "Holidays:";
            // 
            // txtHolidays
            // 
            this.txtHolidays.AcceptsReturn = true;
            this.txtHolidays.BackColor = System.Drawing.SystemColors.Window;
            this.txtHolidays.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtHolidays.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHolidays.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtHolidays.Location = new System.Drawing.Point(150, 215);
            this.txtHolidays.MaxLength = 0;
            this.txtHolidays.Name = "txtHolidays";
            this.txtHolidays.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtHolidays.Size = new System.Drawing.Size(119, 26);
            this.txtHolidays.TabIndex = 89;
            // 
            // lblBalanceTime
            // 
            this.lblBalanceTime.AutoSize = true;
            this.lblBalanceTime.BackColor = System.Drawing.SystemColors.Control;
            this.lblBalanceTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblBalanceTime.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalanceTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBalanceTime.Location = new System.Drawing.Point(34, 183);
            this.lblBalanceTime.Name = "lblBalanceTime";
            this.lblBalanceTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBalanceTime.Size = new System.Drawing.Size(93, 19);
            this.lblBalanceTime.TabIndex = 96;
            this.lblBalanceTime.Text = "Balance Time:";
            // 
            // btnGetUserMessage
            // 
            this.btnGetUserMessage.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetUserMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetUserMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetUserMessage.Location = new System.Drawing.Point(467, 103);
            this.btnGetUserMessage.Name = "btnGetUserMessage";
            this.btnGetUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGetUserMessage.Size = new System.Drawing.Size(112, 30);
            this.btnGetUserMessage.TabIndex = 100;
            this.btnGetUserMessage.Text = "Get";
            this.btnGetUserMessage.UseVisualStyleBackColor = false;
            this.btnGetUserMessage.Click += new System.EventHandler(this.btnGetUserMessage_Click);
            // 
            // dtBalanceTime
            // 
            this.dtBalanceTime.CalendarFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.dtBalanceTime.CustomFormat = "HH:mm";
            this.dtBalanceTime.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.dtBalanceTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtBalanceTime.Location = new System.Drawing.Point(150, 183);
            this.dtBalanceTime.Name = "dtBalanceTime";
            this.dtBalanceTime.Size = new System.Drawing.Size(119, 26);
            this.dtBalanceTime.TabIndex = 106;
            this.dtBalanceTime.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            // 
            // btnSetUserMessage
            // 
            this.btnSetUserMessage.BackColor = System.Drawing.SystemColors.Control;
            this.btnSetUserMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSetUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetUserMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSetUserMessage.Location = new System.Drawing.Point(585, 103);
            this.btnSetUserMessage.Name = "btnSetUserMessage";
            this.btnSetUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSetUserMessage.Size = new System.Drawing.Size(112, 30);
            this.btnSetUserMessage.TabIndex = 100;
            this.btnSetUserMessage.Text = "Set";
            this.btnSetUserMessage.UseVisualStyleBackColor = false;
            this.btnSetUserMessage.Click += new System.EventHandler(this.btnSetUserMessage_Click);
            // 
            // btnGetUserBalanceTime
            // 
            this.btnGetUserBalanceTime.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetUserBalanceTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetUserBalanceTime.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetUserBalanceTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetUserBalanceTime.Location = new System.Drawing.Point(467, 173);
            this.btnGetUserBalanceTime.Name = "btnGetUserBalanceTime";
            this.btnGetUserBalanceTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGetUserBalanceTime.Size = new System.Drawing.Size(112, 30);
            this.btnGetUserBalanceTime.TabIndex = 100;
            this.btnGetUserBalanceTime.Text = "Get";
            this.btnGetUserBalanceTime.UseVisualStyleBackColor = false;
            this.btnGetUserBalanceTime.Click += new System.EventHandler(this.btnGetUserBalanceTime_Click);
            // 
            // btnSetUserBalanceTime
            // 
            this.btnSetUserBalanceTime.BackColor = System.Drawing.SystemColors.Control;
            this.btnSetUserBalanceTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSetUserBalanceTime.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetUserBalanceTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSetUserBalanceTime.Location = new System.Drawing.Point(585, 173);
            this.btnSetUserBalanceTime.Name = "btnSetUserBalanceTime";
            this.btnSetUserBalanceTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSetUserBalanceTime.Size = new System.Drawing.Size(112, 30);
            this.btnSetUserBalanceTime.TabIndex = 100;
            this.btnSetUserBalanceTime.Text = "Set";
            this.btnSetUserBalanceTime.UseVisualStyleBackColor = false;
            this.btnSetUserBalanceTime.Click += new System.EventHandler(this.btnSetUserBalanceTime_Click);
            // 
            // btnGetUserHolidays
            // 
            this.btnGetUserHolidays.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetUserHolidays.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetUserHolidays.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetUserHolidays.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetUserHolidays.Location = new System.Drawing.Point(467, 211);
            this.btnGetUserHolidays.Name = "btnGetUserHolidays";
            this.btnGetUserHolidays.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGetUserHolidays.Size = new System.Drawing.Size(112, 30);
            this.btnGetUserHolidays.TabIndex = 100;
            this.btnGetUserHolidays.Text = "Get";
            this.btnGetUserHolidays.UseVisualStyleBackColor = false;
            this.btnGetUserHolidays.Click += new System.EventHandler(this.btnGetUserHolidays_Click);
            // 
            // btnSetUserHolidays
            // 
            this.btnSetUserHolidays.BackColor = System.Drawing.SystemColors.Control;
            this.btnSetUserHolidays.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSetUserHolidays.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetUserHolidays.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSetUserHolidays.Location = new System.Drawing.Point(585, 211);
            this.btnSetUserHolidays.Name = "btnSetUserHolidays";
            this.btnSetUserHolidays.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSetUserHolidays.Size = new System.Drawing.Size(112, 30);
            this.btnSetUserHolidays.TabIndex = 100;
            this.btnSetUserHolidays.Text = "Set";
            this.btnSetUserHolidays.UseVisualStyleBackColor = false;
            this.btnSetUserHolidays.Click += new System.EventHandler(this.btnSetUserHolidays_Click);
            // 
            // frmEnrollCustom1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 281);
            this.Controls.Add(this.dtBalanceTime);
            this.Controls.Add(this.btnSetUserMessage);
            this.Controls.Add(this.btnSetUserHolidays);
            this.Controls.Add(this.btnSetUserBalanceTime);
            this.Controls.Add(this.btnGetUserHolidays);
            this.Controls.Add(this.btnGetUserBalanceTime);
            this.Controls.Add(this.btnGetUserMessage);
            this.Controls.Add(this.lblBalanceTime);
            this.Controls.Add(this.txtHolidays);
            this.Controls.Add(this.lblHolidays);
            this.Controls.Add(this.txtUserMessage);
            this.Controls.Add(this.txtEnrollNumber);
            this.Controls.Add(this.lblUserMessage);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblEnrollNumber);
            this.Name = "frmEnrollCustom1";
            this.Text = "frmEnrollCustom1 (for German Custom FW)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEnrollCustom1_FormClosed);
            this.Load += new System.EventHandler(this.frmEnrollCustom1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.TextBox txtEnrollNumber;
        public System.Windows.Forms.TextBox lblMessage;
        public System.Windows.Forms.Label lblEnrollNumber;
        private System.Windows.Forms.OpenFileDialog OpenFileDlg;
		public System.Windows.Forms.Label lblUserMessage;
		public System.Windows.Forms.TextBox txtUserMessage;
		public System.Windows.Forms.Label lblHolidays;
		public System.Windows.Forms.TextBox txtHolidays;
		public System.Windows.Forms.Label lblBalanceTime;
		public System.Windows.Forms.Button btnGetUserMessage;
		private System.Windows.Forms.DateTimePicker dtBalanceTime;
		public System.Windows.Forms.Button btnSetUserMessage;
		public System.Windows.Forms.Button btnGetUserBalanceTime;
		public System.Windows.Forms.Button btnSetUserBalanceTime;
		public System.Windows.Forms.Button btnGetUserHolidays;
		public System.Windows.Forms.Button btnSetUserHolidays;
    }
}