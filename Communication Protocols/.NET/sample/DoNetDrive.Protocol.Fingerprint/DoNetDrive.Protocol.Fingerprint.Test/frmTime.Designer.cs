namespace DoNetDrive.Protocol.Fingerprint.Test
{
    partial class frmTime
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
            this.gpDeviceTime = new System.Windows.Forms.GroupBox();
            this.txtErrorTime = new System.Windows.Forms.TextBox();
            this.Lbl_DisparityTime = new System.Windows.Forms.Label();
            this.btnWriteBroadcastTime = new System.Windows.Forms.Button();
            this.btnWriteSystemTime = new System.Windows.Forms.Button();
            this.txtComputerTime = new System.Windows.Forms.TextBox();
            this.Lbl_ComputerTime = new System.Windows.Forms.Label();
            this.txtSystemTime = new System.Windows.Forms.TextBox();
            this.Lbl_DeviceTime = new System.Windows.Forms.Label();
            this.btnReadSystemTime = new System.Windows.Forms.Button();
            this.gpDiyTime = new System.Windows.Forms.GroupBox();
            this.CustomDateTime = new System.Windows.Forms.DateTimePicker();
            this.Lbl_DateTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnWriteCustomDateTime = new System.Windows.Forms.Button();
            this.gpDeviceTime.SuspendLayout();
            this.gpDiyTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpDeviceTime
            // 
            this.gpDeviceTime.Controls.Add(this.txtErrorTime);
            this.gpDeviceTime.Controls.Add(this.Lbl_DisparityTime);
            this.gpDeviceTime.Controls.Add(this.btnWriteBroadcastTime);
            this.gpDeviceTime.Controls.Add(this.btnWriteSystemTime);
            this.gpDeviceTime.Controls.Add(this.txtComputerTime);
            this.gpDeviceTime.Controls.Add(this.Lbl_ComputerTime);
            this.gpDeviceTime.Controls.Add(this.txtSystemTime);
            this.gpDeviceTime.Controls.Add(this.Lbl_DeviceTime);
            this.gpDeviceTime.Controls.Add(this.btnReadSystemTime);
            this.gpDeviceTime.Location = new System.Drawing.Point(12, 12);
            this.gpDeviceTime.Name = "gpDeviceTime";
            this.gpDeviceTime.Size = new System.Drawing.Size(607, 119);
            this.gpDeviceTime.TabIndex = 0;
            this.gpDeviceTime.TabStop = false;
            this.gpDeviceTime.Text = "设备时间";
            // 
            // txtErrorTime
            // 
            this.txtErrorTime.Location = new System.Drawing.Point(318, 92);
            this.txtErrorTime.MaxLength = 16;
            this.txtErrorTime.Name = "txtErrorTime";
            this.txtErrorTime.ReadOnly = true;
            this.txtErrorTime.Size = new System.Drawing.Size(259, 21);
            this.txtErrorTime.TabIndex = 25;
            // 
            // Lbl_DisparityTime
            // 
            this.Lbl_DisparityTime.AutoSize = true;
            this.Lbl_DisparityTime.Location = new System.Drawing.Point(316, 77);
            this.Lbl_DisparityTime.Name = "Lbl_DisparityTime";
            this.Lbl_DisparityTime.Size = new System.Drawing.Size(65, 12);
            this.Lbl_DisparityTime.TabIndex = 24;
            this.Lbl_DisparityTime.Text = "误差时间：";
            // 
            // btnWriteBroadcastTime
            // 
            this.btnWriteBroadcastTime.Location = new System.Drawing.Point(318, 45);
            this.btnWriteBroadcastTime.Name = "btnWriteBroadcastTime";
            this.btnWriteBroadcastTime.Size = new System.Drawing.Size(133, 23);
            this.btnWriteBroadcastTime.TabIndex = 23;
            this.btnWriteBroadcastTime.Text = "校准时间_广播";
            this.btnWriteBroadcastTime.UseVisualStyleBackColor = true;
            this.btnWriteBroadcastTime.Click += new System.EventHandler(this.BtnWriteBroadcastTime_Click);
            // 
            // btnWriteSystemTime
            // 
            this.btnWriteSystemTime.Location = new System.Drawing.Point(457, 17);
            this.btnWriteSystemTime.Name = "btnWriteSystemTime";
            this.btnWriteSystemTime.Size = new System.Drawing.Size(144, 23);
            this.btnWriteSystemTime.TabIndex = 22;
            this.btnWriteSystemTime.Text = "更新设备时间";
            this.btnWriteSystemTime.UseVisualStyleBackColor = true;
            this.btnWriteSystemTime.Click += new System.EventHandler(this.BtnWriteSystemTime_Click);
            // 
            // txtComputerTime
            // 
            this.txtComputerTime.Location = new System.Drawing.Point(77, 88);
            this.txtComputerTime.MaxLength = 16;
            this.txtComputerTime.Name = "txtComputerTime";
            this.txtComputerTime.ReadOnly = true;
            this.txtComputerTime.Size = new System.Drawing.Size(172, 21);
            this.txtComputerTime.TabIndex = 21;
            // 
            // Lbl_ComputerTime
            // 
            this.Lbl_ComputerTime.AutoSize = true;
            this.Lbl_ComputerTime.Location = new System.Drawing.Point(75, 73);
            this.Lbl_ComputerTime.Name = "Lbl_ComputerTime";
            this.Lbl_ComputerTime.Size = new System.Drawing.Size(77, 12);
            this.Lbl_ComputerTime.TabIndex = 20;
            this.Lbl_ComputerTime.Text = "计算机时间：";
            // 
            // txtSystemTime
            // 
            this.txtSystemTime.Location = new System.Drawing.Point(77, 37);
            this.txtSystemTime.MaxLength = 16;
            this.txtSystemTime.Name = "txtSystemTime";
            this.txtSystemTime.ReadOnly = true;
            this.txtSystemTime.Size = new System.Drawing.Size(172, 21);
            this.txtSystemTime.TabIndex = 19;
            // 
            // Lbl_DeviceTime
            // 
            this.Lbl_DeviceTime.AutoSize = true;
            this.Lbl_DeviceTime.Location = new System.Drawing.Point(77, 17);
            this.Lbl_DeviceTime.Name = "Lbl_DeviceTime";
            this.Lbl_DeviceTime.Size = new System.Drawing.Size(65, 12);
            this.Lbl_DeviceTime.TabIndex = 18;
            this.Lbl_DeviceTime.Text = "设备时间：";
            // 
            // btnReadSystemTime
            // 
            this.btnReadSystemTime.Location = new System.Drawing.Point(318, 16);
            this.btnReadSystemTime.Name = "btnReadSystemTime";
            this.btnReadSystemTime.Size = new System.Drawing.Size(133, 23);
            this.btnReadSystemTime.TabIndex = 17;
            this.btnReadSystemTime.Text = "读设备时间";
            this.btnReadSystemTime.UseVisualStyleBackColor = true;
            this.btnReadSystemTime.Click += new System.EventHandler(this.BtnReadSystemTime_Click);
            // 
            // gpDiyTime
            // 
            this.gpDiyTime.Controls.Add(this.CustomDateTime);
            this.gpDiyTime.Controls.Add(this.Lbl_DateTime);
            this.gpDiyTime.Controls.Add(this.label5);
            this.gpDiyTime.Controls.Add(this.btnWriteCustomDateTime);
            this.gpDiyTime.Location = new System.Drawing.Point(12, 137);
            this.gpDiyTime.Name = "gpDiyTime";
            this.gpDiyTime.Size = new System.Drawing.Size(607, 87);
            this.gpDiyTime.TabIndex = 97;
            this.gpDiyTime.TabStop = false;
            this.gpDiyTime.Text = "自定义时间";
            // 
            // CustomDateTime
            // 
            this.CustomDateTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.CustomDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CustomDateTime.Location = new System.Drawing.Point(77, 45);
            this.CustomDateTime.Name = "CustomDateTime";
            this.CustomDateTime.Size = new System.Drawing.Size(172, 21);
            this.CustomDateTime.TabIndex = 90;
            // 
            // Lbl_DateTime
            // 
            this.Lbl_DateTime.AutoSize = true;
            this.Lbl_DateTime.Location = new System.Drawing.Point(75, 25);
            this.Lbl_DateTime.Name = "Lbl_DateTime";
            this.Lbl_DateTime.Size = new System.Drawing.Size(65, 12);
            this.Lbl_DateTime.TabIndex = 89;
            this.Lbl_DateTime.Text = "时间日期：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 77;
            // 
            // btnWriteCustomDateTime
            // 
            this.btnWriteCustomDateTime.Location = new System.Drawing.Point(318, 43);
            this.btnWriteCustomDateTime.Name = "btnWriteCustomDateTime";
            this.btnWriteCustomDateTime.Size = new System.Drawing.Size(133, 23);
            this.btnWriteCustomDateTime.TabIndex = 86;
            this.btnWriteCustomDateTime.Text = "更新设备时间";
            this.btnWriteCustomDateTime.UseVisualStyleBackColor = true;
            this.btnWriteCustomDateTime.Click += new System.EventHandler(this.BtnWriteCustomDateTime_Click);
            // 
            // frmTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 227);
            this.Controls.Add(this.gpDiyTime);
            this.Controls.Add(this.gpDeviceTime);
            this.Name = "frmTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "时钟";
            this.Load += new System.EventHandler(this.frmTime_Load);
            this.gpDeviceTime.ResumeLayout(false);
            this.gpDeviceTime.PerformLayout();
            this.gpDiyTime.ResumeLayout(false);
            this.gpDiyTime.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpDeviceTime;
        private System.Windows.Forms.TextBox txtComputerTime;
        private System.Windows.Forms.Label Lbl_ComputerTime;
        private System.Windows.Forms.TextBox txtSystemTime;
        private System.Windows.Forms.Label Lbl_DeviceTime;
        private System.Windows.Forms.Button btnReadSystemTime;
        private System.Windows.Forms.Button btnWriteBroadcastTime;
        private System.Windows.Forms.Button btnWriteSystemTime;
        private System.Windows.Forms.TextBox txtErrorTime;
        private System.Windows.Forms.Label Lbl_DisparityTime;
        private System.Windows.Forms.GroupBox gpDiyTime;
        private System.Windows.Forms.Label Lbl_DateTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnWriteCustomDateTime;
        private System.Windows.Forms.DateTimePicker CustomDateTime;
    }
}