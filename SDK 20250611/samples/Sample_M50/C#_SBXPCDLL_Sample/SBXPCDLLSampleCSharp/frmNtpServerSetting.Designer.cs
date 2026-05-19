namespace SBXPCDLLSampleCSharp
{
    partial class frmNtpServerSetting
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
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnGetNtpServerSettings = new System.Windows.Forms.Button();
            this.btnSetNtpServerSettings = new System.Windows.Forms.Button();
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.txtTimezone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(42, 71);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 34;
            this.label13.Text = "Timezone:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(42, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "NTP Server Address:";
            // 
            // btnGetNtpServerSettings
            // 
            this.btnGetNtpServerSettings.Location = new System.Drawing.Point(82, 145);
            this.btnGetNtpServerSettings.Name = "btnGetNtpServerSettings";
            this.btnGetNtpServerSettings.Size = new System.Drawing.Size(81, 33);
            this.btnGetNtpServerSettings.TabIndex = 36;
            this.btnGetNtpServerSettings.Text = "Get";
            this.btnGetNtpServerSettings.UseVisualStyleBackColor = true;
            this.btnGetNtpServerSettings.Click += new System.EventHandler(this.btnGetDnsSettings_Click);
            // 
            // btnSetNtpServerSettings
            // 
            this.btnSetNtpServerSettings.Location = new System.Drawing.Point(189, 145);
            this.btnSetNtpServerSettings.Name = "btnSetNtpServerSettings";
            this.btnSetNtpServerSettings.Size = new System.Drawing.Size(81, 33);
            this.btnSetNtpServerSettings.TabIndex = 37;
            this.btnSetNtpServerSettings.Text = "Set";
            this.btnSetNtpServerSettings.UseVisualStyleBackColor = true;
            this.btnSetNtpServerSettings.Click += new System.EventHandler(this.btnSetDnsSettings_Click);
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.AcceptsReturn = true;
            this.txtServerAddress.BackColor = System.Drawing.SystemColors.Window;
            this.txtServerAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtServerAddress.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerAddress.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtServerAddress.Location = new System.Drawing.Point(173, 26);
            this.txtServerAddress.MaxLength = 0;
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtServerAddress.Size = new System.Drawing.Size(152, 26);
            this.txtServerAddress.TabIndex = 38;
            this.txtServerAddress.Text = "1.cn.pool.ntp.org";
            // 
            // txtTimezone
            // 
            this.txtTimezone.AcceptsReturn = true;
            this.txtTimezone.BackColor = System.Drawing.SystemColors.Window;
            this.txtTimezone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimezone.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimezone.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTimezone.Location = new System.Drawing.Point(173, 64);
            this.txtTimezone.MaxLength = 0;
            this.txtTimezone.Name = "txtTimezone";
            this.txtTimezone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTimezone.Size = new System.Drawing.Size(152, 26);
            this.txtTimezone.TabIndex = 39;
            this.txtTimezone.Text = "480";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Interval:";
            // 
            // txtInterval
            // 
            this.txtInterval.AcceptsReturn = true;
            this.txtInterval.BackColor = System.Drawing.SystemColors.Window;
            this.txtInterval.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtInterval.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInterval.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtInterval.Location = new System.Drawing.Point(173, 100);
            this.txtInterval.MaxLength = 0;
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtInterval.Size = new System.Drawing.Size(152, 26);
            this.txtInterval.TabIndex = 40;
            this.txtInterval.Text = "60";
            // 
            // frmNtpServerSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 193);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnGetNtpServerSettings);
            this.Controls.Add(this.btnSetNtpServerSettings);
            this.Controls.Add(this.txtServerAddress);
            this.Controls.Add(this.txtTimezone);
            this.Name = "frmNtpServerSetting";
            this.Text = "frmNtpServerSetting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNtpServerSetting_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnGetNtpServerSettings;
        private System.Windows.Forms.Button btnSetNtpServerSettings;
        public System.Windows.Forms.TextBox txtServerAddress;
        public System.Windows.Forms.TextBox txtTimezone;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtInterval;
    }
}