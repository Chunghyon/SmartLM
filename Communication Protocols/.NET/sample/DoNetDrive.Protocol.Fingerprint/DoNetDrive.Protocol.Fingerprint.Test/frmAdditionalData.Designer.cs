namespace DoNetDrive.Protocol.Fingerprint.Test
{
    partial class frmAdditionalData
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
            this.gpUpload = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.butUploadFolder = new System.Windows.Forms.Button();
            this.butUploadImage = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSaveBase64ToBin = new System.Windows.Forms.Button();
            this.txtCodeData = new System.Windows.Forms.TextBox();
            this.Lbl_CodeData = new System.Windows.Forms.Label();
            this.btnUploadCode = new System.Windows.Forms.Button();
            this.btnDeleteCode = new System.Windows.Forms.Button();
            this.btnCompute = new System.Windows.Forms.Button();
            this.cmbUploadSerialNumber = new System.Windows.Forms.ComboBox();
            this.Lbl_UploadSerialNumber = new System.Windows.Forms.Label();
            this.cmbUploadType = new System.Windows.Forms.ComboBox();
            this.Lbl_UploadType = new System.Windows.Forms.Label();
            this.txtUploadUserCode = new System.Windows.Forms.TextBox();
            this.Lbl_UploadUserCode = new System.Windows.Forms.Label();
            this.btnGetPerson = new System.Windows.Forms.Button();
            this.gpDownload = new System.Windows.Forms.GroupBox();
            this.chkByBlock = new System.Windows.Forms.CheckBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.cmbDownloadSerialNumber = new System.Windows.Forms.ComboBox();
            this.Lbl_DownloadSerialNumber = new System.Windows.Forms.Label();
            this.txtDownloadUserCode = new System.Windows.Forms.TextBox();
            this.cmbDownloadType = new System.Windows.Forms.ComboBox();
            this.Lbl_DownloadUserCode = new System.Windows.Forms.Label();
            this.Lbl_DownloadType = new System.Windows.Forms.Label();
            this.gpUpdateSoftware = new System.Windows.Forms.GroupBox();
            this.chkFaceSoftware = new System.Windows.Forms.CheckBox();
            this.butUpdateSoftware = new System.Windows.Forms.Button();
            this.cmbEquptType = new System.Windows.Forms.ComboBox();
            this.Lbl_EquptType = new System.Windows.Forms.Label();
            this.chkSkipTimeoutPacket = new System.Windows.Forms.CheckBox();
            this.gpUpload.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gpDownload.SuspendLayout();
            this.gpUpdateSoftware.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpUpload
            // 
            this.gpUpload.Controls.Add(this.tabControl1);
            this.gpUpload.Controls.Add(this.cmbUploadSerialNumber);
            this.gpUpload.Controls.Add(this.Lbl_UploadSerialNumber);
            this.gpUpload.Controls.Add(this.cmbUploadType);
            this.gpUpload.Controls.Add(this.Lbl_UploadType);
            this.gpUpload.Controls.Add(this.txtUploadUserCode);
            this.gpUpload.Controls.Add(this.Lbl_UploadUserCode);
            this.gpUpload.Location = new System.Drawing.Point(13, 13);
            this.gpUpload.Name = "gpUpload";
            this.gpUpload.Size = new System.Drawing.Size(603, 281);
            this.gpUpload.TabIndex = 0;
            this.gpUpload.TabStop = false;
            this.gpUpload.Text = "上传";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(6, 58);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(587, 223);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.butUploadFolder);
            this.tabPage2.Controls.Add(this.butUploadImage);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(579, 197);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "照片";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // butUploadFolder
            // 
            this.butUploadFolder.Location = new System.Drawing.Point(6, 104);
            this.butUploadFolder.Name = "butUploadFolder";
            this.butUploadFolder.Size = new System.Drawing.Size(222, 23);
            this.butUploadFolder.TabIndex = 19;
            this.butUploadFolder.Text = "上传文件夹";
            this.butUploadFolder.UseVisualStyleBackColor = true;
            this.butUploadFolder.Click += new System.EventHandler(this.butUploadFolder_Click);
            // 
            // butUploadImage
            // 
            this.butUploadImage.Location = new System.Drawing.Point(354, 156);
            this.butUploadImage.Name = "butUploadImage";
            this.butUploadImage.Size = new System.Drawing.Size(222, 23);
            this.butUploadImage.TabIndex = 18;
            this.butUploadImage.Text = "上传照片";
            this.butUploadImage.UseVisualStyleBackColor = true;
            this.butUploadImage.Click += new System.EventHandler(this.ButUploadImage_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSaveBase64ToBin);
            this.tabPage1.Controls.Add(this.txtCodeData);
            this.tabPage1.Controls.Add(this.Lbl_CodeData);
            this.tabPage1.Controls.Add(this.btnUploadCode);
            this.tabPage1.Controls.Add(this.btnDeleteCode);
            this.tabPage1.Controls.Add(this.btnCompute);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(579, 197);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "特征码";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSaveBase64ToBin
            // 
            this.btnSaveBase64ToBin.Location = new System.Drawing.Point(15, 172);
            this.btnSaveBase64ToBin.Name = "btnSaveBase64ToBin";
            this.btnSaveBase64ToBin.Size = new System.Drawing.Size(142, 23);
            this.btnSaveBase64ToBin.TabIndex = 17;
            this.btnSaveBase64ToBin.Text = "保存到bin文件";
            this.btnSaveBase64ToBin.UseVisualStyleBackColor = true;
            this.btnSaveBase64ToBin.Click += new System.EventHandler(this.btnSaveBase64ToBin_Click);
            // 
            // txtCodeData
            // 
            this.txtCodeData.Location = new System.Drawing.Point(16, 31);
            this.txtCodeData.MaxLength = 0;
            this.txtCodeData.Multiline = true;
            this.txtCodeData.Name = "txtCodeData";
            this.txtCodeData.Size = new System.Drawing.Size(555, 106);
            this.txtCodeData.TabIndex = 16;
            // 
            // Lbl_CodeData
            // 
            this.Lbl_CodeData.AutoSize = true;
            this.Lbl_CodeData.Location = new System.Drawing.Point(33, 15);
            this.Lbl_CodeData.Name = "Lbl_CodeData";
            this.Lbl_CodeData.Size = new System.Drawing.Size(125, 12);
            this.Lbl_CodeData.TabIndex = 12;
            this.Lbl_CodeData.Text = "特征码数据(base64)：";
            // 
            // btnUploadCode
            // 
            this.btnUploadCode.Location = new System.Drawing.Point(429, 143);
            this.btnUploadCode.Name = "btnUploadCode";
            this.btnUploadCode.Size = new System.Drawing.Size(142, 23);
            this.btnUploadCode.TabIndex = 15;
            this.btnUploadCode.Text = "上传特征码";
            this.btnUploadCode.UseVisualStyleBackColor = true;
            this.btnUploadCode.Click += new System.EventHandler(this.BtnUploadCode_Click);
            // 
            // btnDeleteCode
            // 
            this.btnDeleteCode.Location = new System.Drawing.Point(16, 143);
            this.btnDeleteCode.Name = "btnDeleteCode";
            this.btnDeleteCode.Size = new System.Drawing.Size(142, 23);
            this.btnDeleteCode.TabIndex = 13;
            this.btnDeleteCode.Text = "删除特征码";
            this.btnDeleteCode.UseVisualStyleBackColor = true;
            this.btnDeleteCode.Click += new System.EventHandler(this.BtnDeleteCode_Click);
            // 
            // btnCompute
            // 
            this.btnCompute.Location = new System.Drawing.Point(281, 143);
            this.btnCompute.Name = "btnCompute";
            this.btnCompute.Size = new System.Drawing.Size(142, 23);
            this.btnCompute.TabIndex = 14;
            this.btnCompute.Text = "计算CRC32";
            this.btnCompute.UseVisualStyleBackColor = true;
            this.btnCompute.Click += new System.EventHandler(this.BtnCompute_Click);
            // 
            // cmbUploadSerialNumber
            // 
            this.cmbUploadSerialNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUploadSerialNumber.FormattingEnabled = true;
            this.cmbUploadSerialNumber.Location = new System.Drawing.Point(488, 32);
            this.cmbUploadSerialNumber.Name = "cmbUploadSerialNumber";
            this.cmbUploadSerialNumber.Size = new System.Drawing.Size(105, 20);
            this.cmbUploadSerialNumber.TabIndex = 5;
            // 
            // Lbl_UploadSerialNumber
            // 
            this.Lbl_UploadSerialNumber.AutoSize = true;
            this.Lbl_UploadSerialNumber.Location = new System.Drawing.Point(486, 16);
            this.Lbl_UploadSerialNumber.Name = "Lbl_UploadSerialNumber";
            this.Lbl_UploadSerialNumber.Size = new System.Drawing.Size(41, 12);
            this.Lbl_UploadSerialNumber.TabIndex = 4;
            this.Lbl_UploadSerialNumber.Text = "序号：";
            // 
            // cmbUploadType
            // 
            this.cmbUploadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUploadType.FormattingEnabled = true;
            this.cmbUploadType.Location = new System.Drawing.Point(253, 32);
            this.cmbUploadType.Name = "cmbUploadType";
            this.cmbUploadType.Size = new System.Drawing.Size(149, 20);
            this.cmbUploadType.TabIndex = 3;
            this.cmbUploadType.SelectedIndexChanged += new System.EventHandler(this.CmbUploadType_SelectedIndexChanged);
            // 
            // Lbl_UploadType
            // 
            this.Lbl_UploadType.AutoSize = true;
            this.Lbl_UploadType.Location = new System.Drawing.Point(251, 17);
            this.Lbl_UploadType.Name = "Lbl_UploadType";
            this.Lbl_UploadType.Size = new System.Drawing.Size(41, 12);
            this.Lbl_UploadType.TabIndex = 2;
            this.Lbl_UploadType.Text = "类型：";
            // 
            // txtUploadUserCode
            // 
            this.txtUploadUserCode.Location = new System.Drawing.Point(26, 31);
            this.txtUploadUserCode.Name = "txtUploadUserCode";
            this.txtUploadUserCode.Size = new System.Drawing.Size(141, 21);
            this.txtUploadUserCode.TabIndex = 1;
            // 
            // Lbl_UploadUserCode
            // 
            this.Lbl_UploadUserCode.AutoSize = true;
            this.Lbl_UploadUserCode.Location = new System.Drawing.Point(24, 16);
            this.Lbl_UploadUserCode.Name = "Lbl_UploadUserCode";
            this.Lbl_UploadUserCode.Size = new System.Drawing.Size(53, 12);
            this.Lbl_UploadUserCode.TabIndex = 0;
            this.Lbl_UploadUserCode.Text = "用户号：";
            // 
            // btnGetPerson
            // 
            this.btnGetPerson.Location = new System.Drawing.Point(26, 65);
            this.btnGetPerson.Name = "btnGetPerson";
            this.btnGetPerson.Size = new System.Drawing.Size(142, 23);
            this.btnGetPerson.TabIndex = 11;
            this.btnGetPerson.Text = "获取人员数据库详情";
            this.btnGetPerson.UseVisualStyleBackColor = true;
            this.btnGetPerson.Click += new System.EventHandler(this.BtnGetPerson_Click);
            // 
            // gpDownload
            // 
            this.gpDownload.Controls.Add(this.chkByBlock);
            this.gpDownload.Controls.Add(this.btnDownload);
            this.gpDownload.Controls.Add(this.btnGetPerson);
            this.gpDownload.Controls.Add(this.cmbDownloadSerialNumber);
            this.gpDownload.Controls.Add(this.Lbl_DownloadSerialNumber);
            this.gpDownload.Controls.Add(this.txtDownloadUserCode);
            this.gpDownload.Controls.Add(this.cmbDownloadType);
            this.gpDownload.Controls.Add(this.Lbl_DownloadUserCode);
            this.gpDownload.Controls.Add(this.Lbl_DownloadType);
            this.gpDownload.Location = new System.Drawing.Point(12, 300);
            this.gpDownload.Name = "gpDownload";
            this.gpDownload.Size = new System.Drawing.Size(603, 97);
            this.gpDownload.TabIndex = 1;
            this.gpDownload.TabStop = false;
            this.gpDownload.Text = "下载";
            // 
            // chkByBlock
            // 
            this.chkByBlock.AutoSize = true;
            this.chkByBlock.Location = new System.Drawing.Point(254, 72);
            this.chkByBlock.Name = "chkByBlock";
            this.chkByBlock.Size = new System.Drawing.Size(84, 16);
            this.chkByBlock.TabIndex = 12;
            this.chkByBlock.Text = "分块读文件";
            this.chkByBlock.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(489, 68);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(105, 23);
            this.btnDownload.TabIndex = 11;
            this.btnDownload.Text = "下载文件";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // cmbDownloadSerialNumber
            // 
            this.cmbDownloadSerialNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDownloadSerialNumber.FormattingEnabled = true;
            this.cmbDownloadSerialNumber.Location = new System.Drawing.Point(489, 39);
            this.cmbDownloadSerialNumber.Name = "cmbDownloadSerialNumber";
            this.cmbDownloadSerialNumber.Size = new System.Drawing.Size(105, 20);
            this.cmbDownloadSerialNumber.TabIndex = 10;
            // 
            // Lbl_DownloadSerialNumber
            // 
            this.Lbl_DownloadSerialNumber.AutoSize = true;
            this.Lbl_DownloadSerialNumber.Location = new System.Drawing.Point(487, 24);
            this.Lbl_DownloadSerialNumber.Name = "Lbl_DownloadSerialNumber";
            this.Lbl_DownloadSerialNumber.Size = new System.Drawing.Size(41, 12);
            this.Lbl_DownloadSerialNumber.TabIndex = 9;
            this.Lbl_DownloadSerialNumber.Text = "序号：";
            // 
            // txtDownloadUserCode
            // 
            this.txtDownloadUserCode.Location = new System.Drawing.Point(26, 39);
            this.txtDownloadUserCode.Name = "txtDownloadUserCode";
            this.txtDownloadUserCode.Size = new System.Drawing.Size(142, 21);
            this.txtDownloadUserCode.TabIndex = 8;
            // 
            // cmbDownloadType
            // 
            this.cmbDownloadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDownloadType.FormattingEnabled = true;
            this.cmbDownloadType.Location = new System.Drawing.Point(254, 39);
            this.cmbDownloadType.Name = "cmbDownloadType";
            this.cmbDownloadType.Size = new System.Drawing.Size(149, 20);
            this.cmbDownloadType.TabIndex = 8;
            this.cmbDownloadType.SelectedIndexChanged += new System.EventHandler(this.CmbDownloadType_SelectedIndexChanged);
            // 
            // Lbl_DownloadUserCode
            // 
            this.Lbl_DownloadUserCode.AutoSize = true;
            this.Lbl_DownloadUserCode.Location = new System.Drawing.Point(24, 24);
            this.Lbl_DownloadUserCode.Name = "Lbl_DownloadUserCode";
            this.Lbl_DownloadUserCode.Size = new System.Drawing.Size(53, 12);
            this.Lbl_DownloadUserCode.TabIndex = 7;
            this.Lbl_DownloadUserCode.Text = "用户号：";
            // 
            // Lbl_DownloadType
            // 
            this.Lbl_DownloadType.AutoSize = true;
            this.Lbl_DownloadType.Location = new System.Drawing.Point(252, 24);
            this.Lbl_DownloadType.Name = "Lbl_DownloadType";
            this.Lbl_DownloadType.Size = new System.Drawing.Size(41, 12);
            this.Lbl_DownloadType.TabIndex = 7;
            this.Lbl_DownloadType.Text = "类型：";
            this.Lbl_DownloadType.Click += new System.EventHandler(this.Lbl_DownloadType_Click);
            // 
            // gpUpdateSoftware
            // 
            this.gpUpdateSoftware.Controls.Add(this.chkSkipTimeoutPacket);
            this.gpUpdateSoftware.Controls.Add(this.chkFaceSoftware);
            this.gpUpdateSoftware.Controls.Add(this.butUpdateSoftware);
            this.gpUpdateSoftware.Controls.Add(this.cmbEquptType);
            this.gpUpdateSoftware.Controls.Add(this.Lbl_EquptType);
            this.gpUpdateSoftware.Location = new System.Drawing.Point(13, 403);
            this.gpUpdateSoftware.Name = "gpUpdateSoftware";
            this.gpUpdateSoftware.Size = new System.Drawing.Size(604, 58);
            this.gpUpdateSoftware.TabIndex = 20;
            this.gpUpdateSoftware.TabStop = false;
            this.gpUpdateSoftware.Text = "上传固件";
            // 
            // chkFaceSoftware
            // 
            this.chkFaceSoftware.AutoSize = true;
            this.chkFaceSoftware.Checked = true;
            this.chkFaceSoftware.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFaceSoftware.Location = new System.Drawing.Point(318, 13);
            this.chkFaceSoftware.Name = "chkFaceSoftware";
            this.chkFaceSoftware.Size = new System.Drawing.Size(60, 16);
            this.chkFaceSoftware.TabIndex = 13;
            this.chkFaceSoftware.Text = "人脸机";
            this.chkFaceSoftware.UseVisualStyleBackColor = true;
            // 
            // butUpdateSoftware
            // 
            this.butUpdateSoftware.Location = new System.Drawing.Point(488, 29);
            this.butUpdateSoftware.Name = "butUpdateSoftware";
            this.butUpdateSoftware.Size = new System.Drawing.Size(105, 23);
            this.butUpdateSoftware.TabIndex = 11;
            this.butUpdateSoftware.Text = "上传固件";
            this.butUpdateSoftware.UseVisualStyleBackColor = true;
            this.butUpdateSoftware.Click += new System.EventHandler(this.ButUploadSoftware_Click);
            // 
            // cmbEquptType
            // 
            this.cmbEquptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEquptType.FormattingEnabled = true;
            this.cmbEquptType.Location = new System.Drawing.Point(27, 32);
            this.cmbEquptType.Name = "cmbEquptType";
            this.cmbEquptType.Size = new System.Drawing.Size(375, 20);
            this.cmbEquptType.TabIndex = 8;
            // 
            // Lbl_EquptType
            // 
            this.Lbl_EquptType.AutoSize = true;
            this.Lbl_EquptType.Location = new System.Drawing.Point(26, 17);
            this.Lbl_EquptType.Name = "Lbl_EquptType";
            this.Lbl_EquptType.Size = new System.Drawing.Size(65, 12);
            this.Lbl_EquptType.TabIndex = 7;
            this.Lbl_EquptType.Text = "设备类型：";
            // 
            // chkSkipTimeoutPacket
            // 
            this.chkSkipTimeoutPacket.AutoSize = true;
            this.chkSkipTimeoutPacket.Checked = true;
            this.chkSkipTimeoutPacket.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSkipTimeoutPacket.Location = new System.Drawing.Point(402, 13);
            this.chkSkipTimeoutPacket.Name = "chkSkipTimeoutPacket";
            this.chkSkipTimeoutPacket.Size = new System.Drawing.Size(72, 16);
            this.chkSkipTimeoutPacket.TabIndex = 14;
            this.chkSkipTimeoutPacket.Text = "忽略超时";
            this.chkSkipTimeoutPacket.UseVisualStyleBackColor = true;
            // 
            // frmAdditionalData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 469);
            this.Controls.Add(this.gpUpdateSoftware);
            this.Controls.Add(this.gpUpload);
            this.Controls.Add(this.gpDownload);
            this.Name = "frmAdditionalData";
            this.Text = "人员附加数据";
            this.Load += new System.EventHandler(this.frmAdditionalData_Load);
            this.gpUpload.ResumeLayout(false);
            this.gpUpload.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.gpDownload.ResumeLayout(false);
            this.gpDownload.PerformLayout();
            this.gpUpdateSoftware.ResumeLayout(false);
            this.gpUpdateSoftware.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpUpload;
        private System.Windows.Forms.ComboBox cmbUploadSerialNumber;
        private System.Windows.Forms.Label Lbl_UploadSerialNumber;
        private System.Windows.Forms.ComboBox cmbUploadType;
        private System.Windows.Forms.Label Lbl_UploadType;
        private System.Windows.Forms.TextBox txtUploadUserCode;
        private System.Windows.Forms.Label Lbl_UploadUserCode;
        private System.Windows.Forms.GroupBox gpDownload;
        private System.Windows.Forms.TextBox txtDownloadUserCode;
        private System.Windows.Forms.Label Lbl_DownloadUserCode;
        private System.Windows.Forms.ComboBox cmbDownloadSerialNumber;
        private System.Windows.Forms.Label Lbl_DownloadSerialNumber;
        private System.Windows.Forms.ComboBox cmbDownloadType;
        private System.Windows.Forms.Label Lbl_DownloadType;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnGetPerson;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtCodeData;
        private System.Windows.Forms.Label Lbl_CodeData;
        private System.Windows.Forms.Button btnUploadCode;
        private System.Windows.Forms.Button btnDeleteCode;
        private System.Windows.Forms.Button btnCompute;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button butUploadImage;
        private System.Windows.Forms.CheckBox chkByBlock;
        private System.Windows.Forms.GroupBox gpUpdateSoftware;
        private System.Windows.Forms.Button butUpdateSoftware;
        private System.Windows.Forms.ComboBox cmbEquptType;
        private System.Windows.Forms.Label Lbl_EquptType;
        private System.Windows.Forms.Button butUploadFolder;
        private System.Windows.Forms.CheckBox chkFaceSoftware;
        private System.Windows.Forms.Button btnSaveBase64ToBin;
        private System.Windows.Forms.CheckBox chkSkipTimeoutPacket;
    }
}