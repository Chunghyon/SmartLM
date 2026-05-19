namespace SBXPCSampleCSharp
{
    partial class frmVideoStreaming
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkRtspEnable = new System.Windows.Forms.CheckBox();
            this.cmbRtspBitrateMbps = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRtspResolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetRTSPSettings = new System.Windows.Forms.Button();
            this.btnSetRTSPSettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkVerifyDisable = new System.Windows.Forms.CheckBox();
            this.btnTextBorderColorPiker = new System.Windows.Forms.Button();
            this.btnTextColorPiker = new System.Windows.Forms.Button();
            this.txtTextBorderColor = new System.Windows.Forms.TextBox();
            this.txtTextColor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGetCenterScreenMsg = new System.Windows.Forms.Button();
            this.btnSetCenterScreenMsg = new System.Windows.Forms.Button();
            this.txtCenterScreenMsg = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkRtspEnable);
            this.groupBox1.Controls.Add(this.cmbRtspBitrateMbps);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbRtspResolution);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnGetRTSPSettings);
            this.groupBox1.Controls.Add(this.btnSetRTSPSettings);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 154);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Video Streaming (RTSP)";
            // 
            // chkRtspEnable
            // 
            this.chkRtspEnable.AutoSize = true;
            this.chkRtspEnable.Location = new System.Drawing.Point(169, 20);
            this.chkRtspEnable.Name = "chkRtspEnable";
            this.chkRtspEnable.Size = new System.Drawing.Size(90, 17);
            this.chkRtspEnable.TabIndex = 50;
            this.chkRtspEnable.Text = "RTSP enable";
            this.chkRtspEnable.UseVisualStyleBackColor = true;
            // 
            // cmbRtspBitrateMbps
            // 
            this.cmbRtspBitrateMbps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRtspBitrateMbps.FormattingEnabled = true;
            this.cmbRtspBitrateMbps.Items.AddRange(new object[] {
            "5 (default)",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.cmbRtspBitrateMbps.Location = new System.Drawing.Point(169, 69);
            this.cmbRtspBitrateMbps.Name = "cmbRtspBitrateMbps";
            this.cmbRtspBitrateMbps.Size = new System.Drawing.Size(152, 21);
            this.cmbRtspBitrateMbps.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "RTSP bitrate (Mbps) :";
            // 
            // cmbRtspResolution
            // 
            this.cmbRtspResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRtspResolution.FormattingEnabled = true;
            this.cmbRtspResolution.Items.AddRange(new object[] {
            "1920x1080",
            "1280x720",
            "960x540",
            "640x360"});
            this.cmbRtspResolution.Location = new System.Drawing.Point(169, 42);
            this.cmbRtspResolution.Name = "cmbRtspResolution";
            this.cmbRtspResolution.Size = new System.Drawing.Size(152, 21);
            this.cmbRtspResolution.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "RTSP resolution :";
            // 
            // btnGetRTSPSettings
            // 
            this.btnGetRTSPSettings.Location = new System.Drawing.Point(78, 104);
            this.btnGetRTSPSettings.Name = "btnGetRTSPSettings";
            this.btnGetRTSPSettings.Size = new System.Drawing.Size(81, 33);
            this.btnGetRTSPSettings.TabIndex = 47;
            this.btnGetRTSPSettings.Text = "Get";
            this.btnGetRTSPSettings.UseVisualStyleBackColor = true;
            this.btnGetRTSPSettings.Click += new System.EventHandler(this.btnGetRTSPSettings_Click);
            // 
            // btnSetRTSPSettings
            // 
            this.btnSetRTSPSettings.Location = new System.Drawing.Point(185, 104);
            this.btnSetRTSPSettings.Name = "btnSetRTSPSettings";
            this.btnSetRTSPSettings.Size = new System.Drawing.Size(81, 33);
            this.btnSetRTSPSettings.TabIndex = 48;
            this.btnSetRTSPSettings.Text = "Set";
            this.btnSetRTSPSettings.UseVisualStyleBackColor = true;
            this.btnSetRTSPSettings.Click += new System.EventHandler(this.btnSetRTSPSettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkVerifyDisable);
            this.groupBox2.Controls.Add(this.btnTextBorderColorPiker);
            this.groupBox2.Controls.Add(this.btnTextColorPiker);
            this.groupBox2.Controls.Add(this.txtTextBorderColor);
            this.groupBox2.Controls.Add(this.txtTextColor);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnGetCenterScreenMsg);
            this.groupBox2.Controls.Add(this.btnSetCenterScreenMsg);
            this.groupBox2.Controls.Add(this.txtCenterScreenMsg);
            this.groupBox2.Location = new System.Drawing.Point(12, 172);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 284);
            this.groupBox2.TabIndex = 48;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Center Screen Message";
            // 
            // chkVerifyDisable
            // 
            this.chkVerifyDisable.AutoSize = true;
            this.chkVerifyDisable.Location = new System.Drawing.Point(169, 151);
            this.chkVerifyDisable.Name = "chkVerifyDisable";
            this.chkVerifyDisable.Size = new System.Drawing.Size(90, 17);
            this.chkVerifyDisable.TabIndex = 53;
            this.chkVerifyDisable.Text = "Verify Disable";
            this.chkVerifyDisable.UseVisualStyleBackColor = true;
            // 
            // btnTextBorderColorPiker
            // 
            this.btnTextBorderColorPiker.Location = new System.Drawing.Point(282, 202);
            this.btnTextBorderColorPiker.Name = "btnTextBorderColorPiker";
            this.btnTextBorderColorPiker.Size = new System.Drawing.Size(65, 20);
            this.btnTextBorderColorPiker.TabIndex = 52;
            this.btnTextBorderColorPiker.Text = "...";
            this.btnTextBorderColorPiker.UseVisualStyleBackColor = true;
            this.btnTextBorderColorPiker.Click += new System.EventHandler(this.btnTextBorderColorPiker_Click);
            // 
            // btnTextColorPiker
            // 
            this.btnTextColorPiker.Location = new System.Drawing.Point(282, 174);
            this.btnTextColorPiker.Name = "btnTextColorPiker";
            this.btnTextColorPiker.Size = new System.Drawing.Size(65, 20);
            this.btnTextColorPiker.TabIndex = 52;
            this.btnTextColorPiker.Text = "...";
            this.btnTextColorPiker.UseVisualStyleBackColor = true;
            this.btnTextColorPiker.Click += new System.EventHandler(this.btnTextColorPiker_Click);
            // 
            // txtTextBorderColor
            // 
            this.txtTextBorderColor.Location = new System.Drawing.Point(169, 203);
            this.txtTextBorderColor.Name = "txtTextBorderColor";
            this.txtTextBorderColor.Size = new System.Drawing.Size(97, 20);
            this.txtTextBorderColor.TabIndex = 51;
            // 
            // txtTextColor
            // 
            this.txtTextColor.Location = new System.Drawing.Point(169, 174);
            this.txtTextColor.Name = "txtTextColor";
            this.txtTextColor.Size = new System.Drawing.Size(97, 20);
            this.txtTextColor.TabIndex = 51;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 13);
            this.label3.TabIndex = 50;
            this.label3.Text = "Text Border Color  (RGB, HEX) :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Text Color (RGB, HEX) :";
            // 
            // btnGetCenterScreenMsg
            // 
            this.btnGetCenterScreenMsg.Location = new System.Drawing.Point(78, 237);
            this.btnGetCenterScreenMsg.Name = "btnGetCenterScreenMsg";
            this.btnGetCenterScreenMsg.Size = new System.Drawing.Size(81, 33);
            this.btnGetCenterScreenMsg.TabIndex = 48;
            this.btnGetCenterScreenMsg.Text = "Get";
            this.btnGetCenterScreenMsg.UseVisualStyleBackColor = true;
            this.btnGetCenterScreenMsg.Click += new System.EventHandler(this.btnGetCenterScreenMsg_Click);
            // 
            // btnSetCenterScreenMsg
            // 
            this.btnSetCenterScreenMsg.Location = new System.Drawing.Point(185, 237);
            this.btnSetCenterScreenMsg.Name = "btnSetCenterScreenMsg";
            this.btnSetCenterScreenMsg.Size = new System.Drawing.Size(81, 33);
            this.btnSetCenterScreenMsg.TabIndex = 49;
            this.btnSetCenterScreenMsg.Text = "Set";
            this.btnSetCenterScreenMsg.UseVisualStyleBackColor = true;
            this.btnSetCenterScreenMsg.Click += new System.EventHandler(this.btnSetCenterScreenMsg_Click);
            // 
            // txtCenterScreenMsg
            // 
            this.txtCenterScreenMsg.Location = new System.Drawing.Point(6, 19);
            this.txtCenterScreenMsg.Multiline = true;
            this.txtCenterScreenMsg.Name = "txtCenterScreenMsg";
            this.txtCenterScreenMsg.Size = new System.Drawing.Size(341, 112);
            this.txtCenterScreenMsg.TabIndex = 47;
            // 
            // frmVideoStreaming
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 464);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmVideoStreaming";
            this.Text = "frmVideoStreaming";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmVideoStreaming_FormClosed);
            this.Load += new System.EventHandler(this.frmVideoStreaming_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkRtspEnable;
        private System.Windows.Forms.ComboBox cmbRtspResolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetRTSPSettings;
        private System.Windows.Forms.Button btnSetRTSPSettings;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnTextBorderColorPiker;
        private System.Windows.Forms.Button btnTextColorPiker;
        private System.Windows.Forms.TextBox txtTextBorderColor;
        private System.Windows.Forms.TextBox txtTextColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGetCenterScreenMsg;
        private System.Windows.Forms.Button btnSetCenterScreenMsg;
        private System.Windows.Forms.TextBox txtCenterScreenMsg;
        private System.Windows.Forms.CheckBox chkVerifyDisable;
        private System.Windows.Forms.ComboBox cmbRtspBitrateMbps;
        private System.Windows.Forms.Label label4;
    }
}