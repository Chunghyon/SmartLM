namespace SBXPCSampleCSharp
{
    partial class frmHoliday
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
            this.lstHoliday = new System.Windows.Forms.ListBox();
            this.dtHoliday = new System.Windows.Forms.DateTimePicker();
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdWrite = new System.Windows.Forms.Button();
            this.cmdRead = new System.Windows.Forms.Button();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lstHoliday
            // 
            this.lstHoliday.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.lstHoliday.FormattingEnabled = true;
            this.lstHoliday.ItemHeight = 19;
            this.lstHoliday.Location = new System.Drawing.Point(13, 89);
            this.lstHoliday.Name = "lstHoliday";
            this.lstHoliday.Size = new System.Drawing.Size(501, 384);
            this.lstHoliday.TabIndex = 102;
            this.lstHoliday.SelectedIndexChanged += new System.EventHandler(this.lstHoliday_SelectedIndexChanged);
            // 
            // dtHoliday
            // 
            this.dtHoliday.CalendarFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.dtHoliday.CustomFormat = "MM.dd";
            this.dtHoliday.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.dtHoliday.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtHoliday.Location = new System.Drawing.Point(13, 48);
            this.dtHoliday.Name = "dtHoliday";
            this.dtHoliday.Size = new System.Drawing.Size(97, 26);
            this.dtHoliday.TabIndex = 101;
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
            this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdExit.Location = new System.Drawing.Point(537, 428);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdExit.Size = new System.Drawing.Size(104, 46);
            this.cmdExit.TabIndex = 98;
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
            this.cmdWrite.Location = new System.Drawing.Point(537, 364);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdWrite.Size = new System.Drawing.Size(104, 46);
            this.cmdWrite.TabIndex = 97;
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
            this.cmdRead.Location = new System.Drawing.Point(537, 299);
            this.cmdRead.Name = "cmdRead";
            this.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdRead.Size = new System.Drawing.Size(104, 46);
            this.cmdRead.TabIndex = 96;
            this.cmdRead.Text = "Read";
            this.cmdRead.UseVisualStyleBackColor = false;
            this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUpdate.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUpdate.Location = new System.Drawing.Point(537, 90);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUpdate.Size = new System.Drawing.Size(104, 46);
            this.cmdUpdate.TabIndex = 95;
            this.cmdUpdate.Text = "Update";
            this.cmdUpdate.UseVisualStyleBackColor = false;
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(13, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(632, 28);
            this.lblMessage.TabIndex = 93;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.label1.Location = new System.Drawing.Point(152, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 19);
            this.label1.TabIndex = 103;
            this.label1.Text = "Period:";
            // 
            // txtDays
            // 
            this.txtDays.AcceptsReturn = true;
            this.txtDays.BackColor = System.Drawing.SystemColors.Window;
            this.txtDays.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDays.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDays.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDays.Location = new System.Drawing.Point(210, 52);
            this.txtDays.MaxLength = 0;
            this.txtDays.Name = "txtDays";
            this.txtDays.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDays.Size = new System.Drawing.Size(113, 26);
            this.txtDays.TabIndex = 104;
            // 
            // frmHoliday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 485);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstHoliday);
            this.Controls.Add(this.dtHoliday);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdWrite);
            this.Controls.Add(this.cmdRead);
            this.Controls.Add(this.cmdUpdate);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmHoliday";
            this.Text = "frmHoliday";
            this.Load += new System.EventHandler(this.frmHoliday_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHoliday_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstHoliday;
        private System.Windows.Forms.DateTimePicker dtHoliday;
        public System.Windows.Forms.Button cmdExit;
        public System.Windows.Forms.Button cmdWrite;
        public System.Windows.Forms.Button cmdRead;
        public System.Windows.Forms.Button cmdUpdate;
        public System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtDays;

    }
}