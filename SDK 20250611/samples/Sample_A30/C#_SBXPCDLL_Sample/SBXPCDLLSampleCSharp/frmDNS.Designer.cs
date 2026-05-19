namespace SBXPCDLLSampleCSharp
{
    partial class frmDNS
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnGetDnsSettings = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSetDnsSettings = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtServerDomainName = new System.Windows.Forms.TextBox();
            this.txtDnsServer1 = new System.Windows.Forms.TextBox();
            this.chkDnsObtainAuto = new System.Windows.Forms.CheckBox();
            this.txtDnsServer0 = new System.Windows.Forms.TextBox();
            this.textBgServerPort = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnGetDnsSettings);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.btnSetDnsSettings);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtServerDomainName);
            this.groupBox2.Controls.Add(this.txtDnsServer1);
            this.groupBox2.Controls.Add(this.chkDnsObtainAuto);
            this.groupBox2.Controls.Add(this.txtDnsServer0);
            this.groupBox2.Controls.Add(this.textBgServerPort);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 280);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DNS Settings";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 186);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Server port:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 148);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Server domain name(or IP):";
            // 
            // btnGetDnsSettings
            // 
            this.btnGetDnsSettings.Location = new System.Drawing.Point(80, 228);
            this.btnGetDnsSettings.Name = "btnGetDnsSettings";
            this.btnGetDnsSettings.Size = new System.Drawing.Size(81, 33);
            this.btnGetDnsSettings.TabIndex = 32;
            this.btnGetDnsSettings.Text = "Get";
            this.btnGetDnsSettings.UseVisualStyleBackColor = true;
            this.btnGetDnsSettings.Click += new System.EventHandler(this.btnGetDnsSettings_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Alternate DNS server:";
            // 
            // btnSetDnsSettings
            // 
            this.btnSetDnsSettings.Location = new System.Drawing.Point(187, 228);
            this.btnSetDnsSettings.Name = "btnSetDnsSettings";
            this.btnSetDnsSettings.Size = new System.Drawing.Size(81, 33);
            this.btnSetDnsSettings.TabIndex = 32;
            this.btnSetDnsSettings.Text = "Set";
            this.btnSetDnsSettings.UseVisualStyleBackColor = true;
            this.btnSetDnsSettings.Click += new System.EventHandler(this.btnSetDnsSettings_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Preferred DNS server:";
            // 
            // txtServerDomainName
            // 
            this.txtServerDomainName.AcceptsReturn = true;
            this.txtServerDomainName.BackColor = System.Drawing.SystemColors.Window;
            this.txtServerDomainName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtServerDomainName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerDomainName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtServerDomainName.Location = new System.Drawing.Point(162, 141);
            this.txtServerDomainName.MaxLength = 0;
            this.txtServerDomainName.Name = "txtServerDomainName";
            this.txtServerDomainName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtServerDomainName.Size = new System.Drawing.Size(152, 26);
            this.txtServerDomainName.TabIndex = 33;
            this.txtServerDomainName.Text = "logserver.test.domain";
            // 
            // txtDnsServer1
            // 
            this.txtDnsServer1.AcceptsReturn = true;
            this.txtDnsServer1.BackColor = System.Drawing.SystemColors.Window;
            this.txtDnsServer1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDnsServer1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDnsServer1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDnsServer1.Location = new System.Drawing.Point(162, 99);
            this.txtDnsServer1.MaxLength = 0;
            this.txtDnsServer1.Name = "txtDnsServer1";
            this.txtDnsServer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDnsServer1.Size = new System.Drawing.Size(152, 26);
            this.txtDnsServer1.TabIndex = 33;
            this.txtDnsServer1.Text = "0.0.0.0";
            // 
            // chkDnsObtainAuto
            // 
            this.chkDnsObtainAuto.AutoSize = true;
            this.chkDnsObtainAuto.Location = new System.Drawing.Point(21, 31);
            this.chkDnsObtainAuto.Name = "chkDnsObtainAuto";
            this.chkDnsObtainAuto.Size = new System.Drawing.Size(219, 17);
            this.chkDnsObtainAuto.TabIndex = 0;
            this.chkDnsObtainAuto.Text = "Obtain DNS server address automatically";
            this.chkDnsObtainAuto.UseVisualStyleBackColor = true;
            // 
            // txtDnsServer0
            // 
            this.txtDnsServer0.AcceptsReturn = true;
            this.txtDnsServer0.BackColor = System.Drawing.SystemColors.Window;
            this.txtDnsServer0.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDnsServer0.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDnsServer0.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDnsServer0.Location = new System.Drawing.Point(162, 60);
            this.txtDnsServer0.MaxLength = 0;
            this.txtDnsServer0.Name = "txtDnsServer0";
            this.txtDnsServer0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDnsServer0.Size = new System.Drawing.Size(152, 26);
            this.txtDnsServer0.TabIndex = 33;
            this.txtDnsServer0.Text = "0.0.0.0";
            // 
            // textBgServerPort
            // 
            this.textBgServerPort.AcceptsReturn = true;
            this.textBgServerPort.BackColor = System.Drawing.SystemColors.Window;
            this.textBgServerPort.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBgServerPort.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBgServerPort.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBgServerPort.Location = new System.Drawing.Point(162, 179);
            this.textBgServerPort.MaxLength = 0;
            this.textBgServerPort.Name = "textBgServerPort";
            this.textBgServerPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBgServerPort.Size = new System.Drawing.Size(152, 26);
            this.textBgServerPort.TabIndex = 33;
            this.textBgServerPort.Text = "5005";
            // 
            // frmDNS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 306);
            this.Controls.Add(this.groupBox2);
            this.Name = "frmDNS";
            this.Text = "frmDNS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDNS_FormClosed);
            this.Load += new System.EventHandler(this.frmDNS_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnGetDnsSettings;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSetDnsSettings;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox txtServerDomainName;
        public System.Windows.Forms.TextBox txtDnsServer1;
        private System.Windows.Forms.CheckBox chkDnsObtainAuto;
        public System.Windows.Forms.TextBox txtDnsServer0;
        public System.Windows.Forms.TextBox textBgServerPort;
    }
}