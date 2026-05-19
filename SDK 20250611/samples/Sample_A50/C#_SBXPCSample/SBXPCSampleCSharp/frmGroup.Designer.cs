namespace SBXPCSampleCSharp
{
    partial class frmGroup
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.txtGroupNumber = new System.Windows.Forms.TextBox();
            this.lblCardNum = new System.Windows.Forms.Label();
            this._lblEnrollNum_0 = new System.Windows.Forms.Label();
            this.cmdRead = new System.Windows.Forms.Button();
            this.cmdWrite = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(12, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(268, 26);
            this.lblMessage.TabIndex = 70;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtGroupName
            // 
            this.txtGroupName.AcceptsReturn = true;
            this.txtGroupName.BackColor = System.Drawing.SystemColors.Window;
            this.txtGroupName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGroupName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtGroupName.Location = new System.Drawing.Point(136, 79);
            this.txtGroupName.MaxLength = 0;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtGroupName.Size = new System.Drawing.Size(144, 26);
            this.txtGroupName.TabIndex = 75;
            // 
            // txtGroupNumber
            // 
            this.txtGroupNumber.AcceptsReturn = true;
            this.txtGroupNumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtGroupNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGroupNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupNumber.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtGroupNumber.Location = new System.Drawing.Point(136, 45);
            this.txtGroupNumber.MaxLength = 8;
            this.txtGroupNumber.Name = "txtGroupNumber";
            this.txtGroupNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtGroupNumber.Size = new System.Drawing.Size(144, 26);
            this.txtGroupNumber.TabIndex = 73;
            this.txtGroupNumber.Text = "1";
            // 
            // lblCardNum
            // 
            this.lblCardNum.BackColor = System.Drawing.SystemColors.Control;
            this.lblCardNum.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblCardNum.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNum.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCardNum.Location = new System.Drawing.Point(8, 83);
            this.lblCardNum.Name = "lblCardNum";
            this.lblCardNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCardNum.Size = new System.Drawing.Size(122, 36);
            this.lblCardNum.TabIndex = 74;
            this.lblCardNum.Text = "Group Name:";
            // 
            // _lblEnrollNum_0
            // 
            this._lblEnrollNum_0.AutoSize = true;
            this._lblEnrollNum_0.BackColor = System.Drawing.SystemColors.Control;
            this._lblEnrollNum_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblEnrollNum_0.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblEnrollNum_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblEnrollNum_0.Location = new System.Drawing.Point(8, 49);
            this._lblEnrollNum_0.Name = "_lblEnrollNum_0";
            this._lblEnrollNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblEnrollNum_0.Size = new System.Drawing.Size(105, 19);
            this._lblEnrollNum_0.TabIndex = 72;
            this._lblEnrollNum_0.Text = "Group Number:";
            // 
            // cmdRead
            // 
            this.cmdRead.BackColor = System.Drawing.SystemColors.Control;
            this.cmdRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRead.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdRead.Location = new System.Drawing.Point(13, 122);
            this.cmdRead.Name = "cmdRead";
            this.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdRead.Size = new System.Drawing.Size(83, 28);
            this.cmdRead.TabIndex = 76;
            this.cmdRead.Text = "Read";
            this.cmdRead.UseVisualStyleBackColor = false;
            this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
            // 
            // cmdWrite
            // 
            this.cmdWrite.BackColor = System.Drawing.SystemColors.Control;
            this.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdWrite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdWrite.Location = new System.Drawing.Point(102, 122);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdWrite.Size = new System.Drawing.Size(83, 28);
            this.cmdWrite.TabIndex = 77;
            this.cmdWrite.Text = "Write";
            this.cmdWrite.UseVisualStyleBackColor = false;
            this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
            this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdExit.Location = new System.Drawing.Point(197, 122);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdExit.Size = new System.Drawing.Size(83, 28);
            this.cmdExit.TabIndex = 78;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = false;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // frmGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 168);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdWrite);
            this.Controls.Add(this.cmdRead);
            this.Controls.Add(this.txtGroupName);
            this.Controls.Add(this.txtGroupNumber);
            this.Controls.Add(this.lblCardNum);
            this.Controls.Add(this._lblEnrollNum_0);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmGroup";
            this.Text = "frmGroup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGroup_FormClosing);
            this.Load += new System.EventHandler(this.frmGroup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblMessage;
        public System.Windows.Forms.TextBox txtGroupName;
        public System.Windows.Forms.TextBox txtGroupNumber;
        public System.Windows.Forms.Label lblCardNum;
        public System.Windows.Forms.Label _lblEnrollNum_0;
        public System.Windows.Forms.Button cmdRead;
        public System.Windows.Forms.Button cmdWrite;
        public System.Windows.Forms.Button cmdExit;
    }
}