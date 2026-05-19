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
            this.lblVerifyCount = new System.Windows.Forms.Label();
            this.chkUseVerifyCount = new System.Windows.Forms.CheckBox();
            this.txtVerifyCount = new System.Windows.Forms.TextBox();
            this.btnGetUserVerifyCount = new System.Windows.Forms.Button();
            this.btnSetUserVerifyCount = new System.Windows.Forms.Button();
            this.chkUserAttendOnly = new System.Windows.Forms.CheckBox();
            this.btnSetUserMessage = new System.Windows.Forms.Button();
            this.btnGetUserMessage = new System.Windows.Forms.Button();
            this.txtUserMessage = new System.Windows.Forms.TextBox();
            this.lblUserMessage = new System.Windows.Forms.Label();
            this.btnMessageColorPiker = new System.Windows.Forms.Button();
            this.txtMessageColor = new System.Windows.Forms.TextBox();
            this.lblMessageColor = new System.Windows.Forms.Label();
            this.btnGetMessageColor = new System.Windows.Forms.Button();
            this.btnSetMessageColor = new System.Windows.Forms.Button();
            this.lblMessageBkColor = new System.Windows.Forms.Label();
            this.txtMessageBkColor = new System.Windows.Forms.TextBox();
            this.btnMessageBkColorPiker = new System.Windows.Forms.Button();
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
            // lblVerifyCount
            // 
            this.lblVerifyCount.AutoSize = true;
            this.lblVerifyCount.BackColor = System.Drawing.SystemColors.Control;
            this.lblVerifyCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblVerifyCount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerifyCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVerifyCount.Location = new System.Drawing.Point(89, 155);
            this.lblVerifyCount.Name = "lblVerifyCount";
            this.lblVerifyCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblVerifyCount.Size = new System.Drawing.Size(139, 19);
            this.lblVerifyCount.TabIndex = 108;
            this.lblVerifyCount.Text = "VerifyCount (0~255):";
            // 
            // chkUseVerifyCount
            // 
            this.chkUseVerifyCount.BackColor = System.Drawing.SystemColors.Control;
            this.chkUseVerifyCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkUseVerifyCount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseVerifyCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkUseVerifyCount.Location = new System.Drawing.Point(70, 124);
            this.chkUseVerifyCount.Name = "chkUseVerifyCount";
            this.chkUseVerifyCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkUseVerifyCount.Size = new System.Drawing.Size(243, 26);
            this.chkUseVerifyCount.TabIndex = 107;
            this.chkUseVerifyCount.Text = "Use Verify Count (for K9)";
            this.chkUseVerifyCount.UseVisualStyleBackColor = false;
            // 
            // txtVerifyCount
            // 
            this.txtVerifyCount.AcceptsReturn = true;
            this.txtVerifyCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtVerifyCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVerifyCount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVerifyCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtVerifyCount.Location = new System.Drawing.Point(234, 152);
            this.txtVerifyCount.MaxLength = 8;
            this.txtVerifyCount.Name = "txtVerifyCount";
            this.txtVerifyCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtVerifyCount.Size = new System.Drawing.Size(119, 26);
            this.txtVerifyCount.TabIndex = 38;
            this.txtVerifyCount.Text = "1";
            // 
            // btnGetUserVerifyCount
            // 
            this.btnGetUserVerifyCount.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetUserVerifyCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetUserVerifyCount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetUserVerifyCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetUserVerifyCount.Location = new System.Drawing.Point(467, 144);
            this.btnGetUserVerifyCount.Name = "btnGetUserVerifyCount";
            this.btnGetUserVerifyCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGetUserVerifyCount.Size = new System.Drawing.Size(112, 30);
            this.btnGetUserVerifyCount.TabIndex = 100;
            this.btnGetUserVerifyCount.Text = "Get";
            this.btnGetUserVerifyCount.UseVisualStyleBackColor = false;
            this.btnGetUserVerifyCount.Click += new System.EventHandler(this.btnGetUserVerifyCount_Click);
            // 
            // btnSetUserVerifyCount
            // 
            this.btnSetUserVerifyCount.BackColor = System.Drawing.SystemColors.Control;
            this.btnSetUserVerifyCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSetUserVerifyCount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetUserVerifyCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSetUserVerifyCount.Location = new System.Drawing.Point(585, 144);
            this.btnSetUserVerifyCount.Name = "btnSetUserVerifyCount";
            this.btnSetUserVerifyCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSetUserVerifyCount.Size = new System.Drawing.Size(112, 30);
            this.btnSetUserVerifyCount.TabIndex = 100;
            this.btnSetUserVerifyCount.Text = "Set";
            this.btnSetUserVerifyCount.UseVisualStyleBackColor = false;
            this.btnSetUserVerifyCount.Click += new System.EventHandler(this.btnSetUserVerifyCount_Click);
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
            // btnSetUserMessage
            // 
            this.btnSetUserMessage.BackColor = System.Drawing.SystemColors.Control;
            this.btnSetUserMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSetUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetUserMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSetUserMessage.Location = new System.Drawing.Point(585, 274);
            this.btnSetUserMessage.Name = "btnSetUserMessage";
            this.btnSetUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSetUserMessage.Size = new System.Drawing.Size(112, 30);
            this.btnSetUserMessage.TabIndex = 111;
            this.btnSetUserMessage.Text = "Set";
            this.btnSetUserMessage.UseVisualStyleBackColor = false;
            this.btnSetUserMessage.Click += new System.EventHandler(this.btnSetUserMessage_Click);
            // 
            // btnGetUserMessage
            // 
            this.btnGetUserMessage.BackColor = System.Drawing.SystemColors.Control;
            this.btnGetUserMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnGetUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetUserMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetUserMessage.Location = new System.Drawing.Point(467, 274);
            this.btnGetUserMessage.Name = "btnGetUserMessage";
            this.btnGetUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGetUserMessage.Size = new System.Drawing.Size(112, 30);
            this.btnGetUserMessage.TabIndex = 112;
            this.btnGetUserMessage.Text = "Get";
            this.btnGetUserMessage.UseVisualStyleBackColor = false;
            this.btnGetUserMessage.Click += new System.EventHandler(this.btnGetUserMessage_Click);
            // 
            // txtUserMessage
            // 
            this.txtUserMessage.AcceptsReturn = true;
            this.txtUserMessage.BackColor = System.Drawing.SystemColors.Window;
            this.txtUserMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserMessage.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtUserMessage.Location = new System.Drawing.Point(70, 277);
            this.txtUserMessage.MaxLength = 100;
            this.txtUserMessage.Multiline = true;
            this.txtUserMessage.Name = "txtUserMessage";
            this.txtUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUserMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUserMessage.Size = new System.Drawing.Size(295, 70);
            this.txtUserMessage.TabIndex = 109;
            // 
            // lblUserMessage
            // 
            this.lblUserMessage.AutoSize = true;
            this.lblUserMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblUserMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblUserMessage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUserMessage.Location = new System.Drawing.Point(66, 255);
            this.lblUserMessage.Name = "lblUserMessage";
            this.lblUserMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUserMessage.Size = new System.Drawing.Size(135, 19);
            this.lblUserMessage.TabIndex = 110;
            this.lblUserMessage.Text = "Message : (for M91)";
            // 
            // btnMessageColorPiker
            // 
            this.btnMessageColorPiker.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMessageColorPiker.Location = new System.Drawing.Point(371, 364);
            this.btnMessageColorPiker.Name = "btnMessageColorPiker";
            this.btnMessageColorPiker.Size = new System.Drawing.Size(65, 26);
            this.btnMessageColorPiker.TabIndex = 117;
            this.btnMessageColorPiker.Text = "...";
            this.btnMessageColorPiker.UseVisualStyleBackColor = true;
            this.btnMessageColorPiker.Click += new System.EventHandler(this.btnMessageColorPiker_Click);
            // 
            // txtMessageColor
            // 
            this.txtMessageColor.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageColor.Location = new System.Drawing.Point(236, 364);
            this.txtMessageColor.Name = "txtMessageColor";
            this.txtMessageColor.Size = new System.Drawing.Size(129, 26);
            this.txtMessageColor.TabIndex = 116;
            // 
            // lblMessageColor
            // 
            this.lblMessageColor.AutoSize = true;
            this.lblMessageColor.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessageColor.Location = new System.Drawing.Point(23, 367);
            this.lblMessageColor.Name = "lblMessageColor";
            this.lblMessageColor.Size = new System.Drawing.Size(193, 19);
            this.lblMessageColor.TabIndex = 115;
            this.lblMessageColor.Text = "Message Color (RGB, HEX) :";
            // 
            // btnGetMessageColor
            // 
            this.btnGetMessageColor.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetMessageColor.Location = new System.Drawing.Point(467, 378);
            this.btnGetMessageColor.Name = "btnGetMessageColor";
            this.btnGetMessageColor.Size = new System.Drawing.Size(112, 30);
            this.btnGetMessageColor.TabIndex = 113;
            this.btnGetMessageColor.Text = "Get";
            this.btnGetMessageColor.UseVisualStyleBackColor = true;
            this.btnGetMessageColor.Click += new System.EventHandler(this.btnGetMessageColor_Click);
            // 
            // btnSetMessageColor
            // 
            this.btnSetMessageColor.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetMessageColor.Location = new System.Drawing.Point(585, 378);
            this.btnSetMessageColor.Name = "btnSetMessageColor";
            this.btnSetMessageColor.Size = new System.Drawing.Size(112, 30);
            this.btnSetMessageColor.TabIndex = 114;
            this.btnSetMessageColor.Text = "Set";
            this.btnSetMessageColor.UseVisualStyleBackColor = true;
            this.btnSetMessageColor.Click += new System.EventHandler(this.btnSetMessageColor_Click);
            // 
            // lblMessageBkColor
            // 
            this.lblMessageBkColor.AutoSize = true;
            this.lblMessageBkColor.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessageBkColor.Location = new System.Drawing.Point(23, 399);
            this.lblMessageBkColor.Name = "lblMessageBkColor";
            this.lblMessageBkColor.Size = new System.Drawing.Size(141, 19);
            this.lblMessageBkColor.TabIndex = 115;
            this.lblMessageBkColor.Text = "Message BackColor :";
            // 
            // txtMessageBkColor
            // 
            this.txtMessageBkColor.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageBkColor.Location = new System.Drawing.Point(236, 396);
            this.txtMessageBkColor.Name = "txtMessageBkColor";
            this.txtMessageBkColor.Size = new System.Drawing.Size(129, 26);
            this.txtMessageBkColor.TabIndex = 116;
            // 
            // btnMessageBkColorPiker
            // 
            this.btnMessageBkColorPiker.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMessageBkColorPiker.Location = new System.Drawing.Point(371, 396);
            this.btnMessageBkColorPiker.Name = "btnMessageBkColorPiker";
            this.btnMessageBkColorPiker.Size = new System.Drawing.Size(65, 26);
            this.btnMessageBkColorPiker.TabIndex = 117;
            this.btnMessageBkColorPiker.Text = "...";
            this.btnMessageBkColorPiker.UseVisualStyleBackColor = true;
            this.btnMessageBkColorPiker.Click += new System.EventHandler(this.btnMessageBkColorPiker_Click);
            // 
            // frmEnrollCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 454);
            this.Controls.Add(this.btnMessageBkColorPiker);
            this.Controls.Add(this.btnMessageColorPiker);
            this.Controls.Add(this.txtMessageBkColor);
            this.Controls.Add(this.lblMessageBkColor);
            this.Controls.Add(this.txtMessageColor);
            this.Controls.Add(this.lblMessageColor);
            this.Controls.Add(this.btnGetMessageColor);
            this.Controls.Add(this.btnSetMessageColor);
            this.Controls.Add(this.btnSetUserMessage);
            this.Controls.Add(this.btnGetUserMessage);
            this.Controls.Add(this.txtUserMessage);
            this.Controls.Add(this.lblUserMessage);
            this.Controls.Add(this.lblVerifyCount);
            this.Controls.Add(this.chkUserAttendOnly);
            this.Controls.Add(this.chkUseVerifyCount);
            this.Controls.Add(this.btnSetUserVerifyCount);
            this.Controls.Add(this.btnSetUserAttendOnly);
            this.Controls.Add(this.btnGetUserVerifyCount);
            this.Controls.Add(this.btnGetUserAttendOnly);
            this.Controls.Add(this.txtVerifyCount);
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
        public System.Windows.Forms.Label lblVerifyCount;
        public System.Windows.Forms.CheckBox chkUseVerifyCount;
        public System.Windows.Forms.TextBox txtVerifyCount;
        public System.Windows.Forms.Button btnGetUserVerifyCount;
        public System.Windows.Forms.Button btnSetUserVerifyCount;
        public System.Windows.Forms.CheckBox chkUserAttendOnly;
        public System.Windows.Forms.Button btnSetUserMessage;
        public System.Windows.Forms.Button btnGetUserMessage;
        public System.Windows.Forms.TextBox txtUserMessage;
        public System.Windows.Forms.Label lblUserMessage;
        private System.Windows.Forms.Button btnMessageColorPiker;
        private System.Windows.Forms.TextBox txtMessageColor;
        private System.Windows.Forms.Label lblMessageColor;
        private System.Windows.Forms.Button btnGetMessageColor;
        private System.Windows.Forms.Button btnSetMessageColor;
        private System.Windows.Forms.Label lblMessageBkColor;
        private System.Windows.Forms.TextBox txtMessageBkColor;
        private System.Windows.Forms.Button btnMessageBkColorPiker;
    }
}