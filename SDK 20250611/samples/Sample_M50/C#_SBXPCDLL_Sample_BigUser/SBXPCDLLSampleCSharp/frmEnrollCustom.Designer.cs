namespace SBXPCDLLSampleCSharp
{
    partial class frmEnrollCustom
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
            this.btnGetUserAttendOnly = new System.Windows.Forms.Button();
            this.btnSetUserAttendOnly = new System.Windows.Forms.Button();
            this.chkUserAttendOnly = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            this.txtEnrollNumber.Size = new System.Drawing.Size(295, 26);
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
            // btnGetUserAttendOnly
            // 
            this.btnGetUserAttendOnly.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetUserAttendOnly.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetUserAttendOnly.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetUserAttendOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetUserAttendOnly.Location = new System.Drawing.Point(467, 211);
            this.btnGetUserAttendOnly.Name = "btnGetUserAttendOnly";
            this.btnGetUserAttendOnly.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGetUserAttendOnly.Size = new System.Drawing.Size(112, 30);
            this.btnGetUserAttendOnly.TabIndex = 100;
            this.btnGetUserAttendOnly.Text = "Get";
            this.btnGetUserAttendOnly.UseVisualStyleBackColor = false;
            this.btnGetUserAttendOnly.Click += new System.EventHandler(this.btnGetUserAttendOnly_Click);
            // 
            // btnSetUserAttendOnly
            // 
            this.btnSetUserAttendOnly.BackColor = System.Drawing.SystemColors.Control;
            this.btnSetUserAttendOnly.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSetUserAttendOnly.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetUserAttendOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSetUserAttendOnly.Location = new System.Drawing.Point(585, 211);
            this.btnSetUserAttendOnly.Name = "btnSetUserAttendOnly";
            this.btnSetUserAttendOnly.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSetUserAttendOnly.Size = new System.Drawing.Size(112, 30);
            this.btnSetUserAttendOnly.TabIndex = 100;
            this.btnSetUserAttendOnly.Text = "Set";
            this.btnSetUserAttendOnly.UseVisualStyleBackColor = false;
            this.btnSetUserAttendOnly.Click += new System.EventHandler(this.btnSetUserAttendOnly_Click);
            // 
            // chkUserAttendOnly
            // 
            this.chkUserAttendOnly.BackColor = System.Drawing.SystemColors.Control;
            this.chkUserAttendOnly.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkUserAttendOnly.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUserAttendOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkUserAttendOnly.Location = new System.Drawing.Point(70, 211);
            this.chkUserAttendOnly.Name = "chkUserAttendOnly";
            this.chkUserAttendOnly.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkUserAttendOnly.Size = new System.Drawing.Size(243, 26);
            this.chkUserAttendOnly.TabIndex = 107;
            this.chkUserAttendOnly.Text = "User Attend Only (for RS910)";
            this.chkUserAttendOnly.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Location = new System.Drawing.Point(150, 96);
            this.textBox1.MaxLength = 24;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox1.Size = new System.Drawing.Size(278, 69);
            this.textBox1.TabIndex = 108;
            this.textBox1.Text = "Enroll Number: (base 36 integer)\r\ndigit and english alphabet mixed.\r\nmiddle non-a" +
    "lphanumeric symbol not allowed.\r\nfront 0 ignored.\r\n";
            // 
            // frmEnrollCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 380);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkUserAttendOnly);
            this.Controls.Add(this.btnSetUserAttendOnly);
            this.Controls.Add(this.btnGetUserAttendOnly);
            this.Controls.Add(this.txtEnrollNumber);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblEnrollNumber);
            this.Name = "frmEnrollCustom";
            this.Text = "frmEnrollCustom";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEnrollCustom_FormClosed);
            this.Load += new System.EventHandler(this.frmEnrollCustom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.TextBox txtEnrollNumber;
        public System.Windows.Forms.TextBox lblMessage;
        public System.Windows.Forms.Label lblEnrollNumber;
        private System.Windows.Forms.OpenFileDialog OpenFileDlg;
		public System.Windows.Forms.Button btnGetUserAttendOnly;
		public System.Windows.Forms.Button btnSetUserAttendOnly;
        public System.Windows.Forms.CheckBox chkUserAttendOnly;
        public System.Windows.Forms.TextBox textBox1;
    }
}