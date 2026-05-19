namespace SBXPCSampleCSharp
{
    partial class frmScreenSaver
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
            this.lblEnrollData = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.cmdGetCustomerInfo = new System.Windows.Forms.Button();
            this.cmdSetCustomerInfo = new System.Windows.Forms.Button();
            this.txtSleepMessage = new System.Windows.Forms.TextBox();
            this.txtGlyphWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGlyphHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFontHeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFontWidth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFontWeight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmdGetGlyphSize = new System.Windows.Forms.Button();
            this.chkItalic = new System.Windows.Forms.CheckBox();
            this.chkUnderline = new System.Windows.Forms.CheckBox();
            this.chkStrikeOut = new System.Windows.Forms.CheckBox();
            this.cmdSetSleepMsg = new System.Windows.Forms.Button();
            this.cmdGetSleepMsg = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.chkDebugOut = new System.Windows.Forms.CheckBox();
            this.txtDebugOutFile = new System.Windows.Forms.TextBox();
            this.cmdDebugOutFileBrowse = new System.Windows.Forms.Button();
            this.OpenSaveDlg = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(574, 28);
            this.lblMessage.TabIndex = 28;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblEnrollData
            // 
            this.lblEnrollData.AutoSize = true;
            this.lblEnrollData.BackColor = System.Drawing.SystemColors.Control;
            this.lblEnrollData.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblEnrollData.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnrollData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEnrollData.Location = new System.Drawing.Point(48, 67);
            this.lblEnrollData.Name = "lblEnrollData";
            this.lblEnrollData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblEnrollData.Size = new System.Drawing.Size(112, 19);
            this.lblEnrollData.TabIndex = 36;
            this.lblEnrollData.Text = "Company Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(26, 209);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(104, 19);
            this.label1.TabIndex = 37;
            this.label1.Text = "Sleep Message:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(48, 109);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(112, 19);
            this.label2.TabIndex = 38;
            this.label2.Text = "Customer Name:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.AcceptsReturn = true;
            this.txtCompanyName.BackColor = System.Drawing.SystemColors.Window;
            this.txtCompanyName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCompanyName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompanyName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCompanyName.Location = new System.Drawing.Point(197, 66);
            this.txtCompanyName.MaxLength = 64;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCompanyName.Size = new System.Drawing.Size(373, 26);
            this.txtCompanyName.TabIndex = 39;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.AcceptsReturn = true;
            this.txtCustomerName.BackColor = System.Drawing.SystemColors.Window;
            this.txtCustomerName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCustomerName.Location = new System.Drawing.Point(197, 105);
            this.txtCustomerName.MaxLength = 64;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCustomerName.Size = new System.Drawing.Size(373, 26);
            this.txtCustomerName.TabIndex = 40;
            // 
            // cmdGetCustomerInfo
            // 
            this.cmdGetCustomerInfo.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetCustomerInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetCustomerInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetCustomerInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetCustomerInfo.Location = new System.Drawing.Point(92, 157);
            this.cmdGetCustomerInfo.Name = "cmdGetCustomerInfo";
            this.cmdGetCustomerInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetCustomerInfo.Size = new System.Drawing.Size(200, 30);
            this.cmdGetCustomerInfo.TabIndex = 55;
            this.cmdGetCustomerInfo.Text = "Get Customer Info";
            this.cmdGetCustomerInfo.UseVisualStyleBackColor = false;
            this.cmdGetCustomerInfo.Click += new System.EventHandler(this.cmdGetCustomerInfo_Click);
            // 
            // cmdSetCustomerInfo
            // 
            this.cmdSetCustomerInfo.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSetCustomerInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSetCustomerInfo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSetCustomerInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSetCustomerInfo.Location = new System.Drawing.Point(336, 157);
            this.cmdSetCustomerInfo.Name = "cmdSetCustomerInfo";
            this.cmdSetCustomerInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSetCustomerInfo.Size = new System.Drawing.Size(200, 30);
            this.cmdSetCustomerInfo.TabIndex = 56;
            this.cmdSetCustomerInfo.Text = "Set Customer Info";
            this.cmdSetCustomerInfo.UseVisualStyleBackColor = false;
            this.cmdSetCustomerInfo.Click += new System.EventHandler(this.cmdSetCustomerInfo_Click);
            // 
            // txtSleepMessage
            // 
            this.txtSleepMessage.Location = new System.Drawing.Point(140, 209);
            this.txtSleepMessage.MaxLength = 128;
            this.txtSleepMessage.Multiline = true;
            this.txtSleepMessage.Name = "txtSleepMessage";
            this.txtSleepMessage.Size = new System.Drawing.Size(429, 83);
            this.txtSleepMessage.TabIndex = 57;
            // 
            // txtGlyphWidth
            // 
            this.txtGlyphWidth.AcceptsReturn = true;
            this.txtGlyphWidth.BackColor = System.Drawing.SystemColors.Window;
            this.txtGlyphWidth.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGlyphWidth.Enabled = false;
            this.txtGlyphWidth.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlyphWidth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtGlyphWidth.Location = new System.Drawing.Point(144, 317);
            this.txtGlyphWidth.MaxLength = 8;
            this.txtGlyphWidth.Name = "txtGlyphWidth";
            this.txtGlyphWidth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtGlyphWidth.Size = new System.Drawing.Size(133, 26);
            this.txtGlyphWidth.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(26, 320);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(89, 19);
            this.label3.TabIndex = 58;
            this.label3.Text = "Glyph Width:";
            // 
            // txtGlyphHeight
            // 
            this.txtGlyphHeight.AcceptsReturn = true;
            this.txtGlyphHeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtGlyphHeight.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGlyphHeight.Enabled = false;
            this.txtGlyphHeight.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlyphHeight.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtGlyphHeight.Location = new System.Drawing.Point(144, 348);
            this.txtGlyphHeight.MaxLength = 8;
            this.txtGlyphHeight.Name = "txtGlyphHeight";
            this.txtGlyphHeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtGlyphHeight.Size = new System.Drawing.Size(133, 26);
            this.txtGlyphHeight.TabIndex = 61;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(26, 351);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(91, 19);
            this.label4.TabIndex = 60;
            this.label4.Text = "Glyph Height:";
            // 
            // txtFontHeight
            // 
            this.txtFontHeight.AcceptsReturn = true;
            this.txtFontHeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtFontHeight.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFontHeight.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFontHeight.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFontHeight.Location = new System.Drawing.Point(144, 381);
            this.txtFontHeight.MaxLength = 8;
            this.txtFontHeight.Name = "txtFontHeight";
            this.txtFontHeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFontHeight.Size = new System.Drawing.Size(133, 26);
            this.txtFontHeight.TabIndex = 63;
            this.txtFontHeight.Text = "20";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(26, 384);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(83, 19);
            this.label5.TabIndex = 62;
            this.label5.Text = "Font Height:";
            // 
            // txtFontWidth
            // 
            this.txtFontWidth.AcceptsReturn = true;
            this.txtFontWidth.BackColor = System.Drawing.SystemColors.Window;
            this.txtFontWidth.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFontWidth.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFontWidth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFontWidth.Location = new System.Drawing.Point(144, 413);
            this.txtFontWidth.MaxLength = 8;
            this.txtFontWidth.Name = "txtFontWidth";
            this.txtFontWidth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFontWidth.Size = new System.Drawing.Size(133, 26);
            this.txtFontWidth.TabIndex = 65;
            this.txtFontWidth.Text = "11";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(26, 416);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(81, 19);
            this.label6.TabIndex = 64;
            this.label6.Text = "Font Width:";
            // 
            // txtFontWeight
            // 
            this.txtFontWeight.AcceptsReturn = true;
            this.txtFontWeight.BackColor = System.Drawing.SystemColors.Window;
            this.txtFontWeight.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFontWeight.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFontWeight.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFontWeight.Location = new System.Drawing.Point(144, 446);
            this.txtFontWeight.MaxLength = 8;
            this.txtFontWeight.Name = "txtFontWeight";
            this.txtFontWeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFontWeight.Size = new System.Drawing.Size(133, 26);
            this.txtFontWeight.TabIndex = 67;
            this.txtFontWeight.Text = "700";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(26, 449);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(87, 19);
            this.label7.TabIndex = 66;
            this.label7.Text = "Font Weight:";
            // 
            // cmdGetGlyphSize
            // 
            this.cmdGetGlyphSize.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetGlyphSize.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetGlyphSize.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetGlyphSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetGlyphSize.Location = new System.Drawing.Point(351, 329);
            this.cmdGetGlyphSize.Name = "cmdGetGlyphSize";
            this.cmdGetGlyphSize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetGlyphSize.Size = new System.Drawing.Size(200, 30);
            this.cmdGetGlyphSize.TabIndex = 68;
            this.cmdGetGlyphSize.Text = "Get Glyph Size";
            this.cmdGetGlyphSize.UseVisualStyleBackColor = false;
            this.cmdGetGlyphSize.Click += new System.EventHandler(this.cmdGetGlyphSize_Click);
            // 
            // chkItalic
            // 
            this.chkItalic.AutoSize = true;
            this.chkItalic.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkItalic.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.chkItalic.Location = new System.Drawing.Point(380, 376);
            this.chkItalic.Name = "chkItalic";
            this.chkItalic.Size = new System.Drawing.Size(63, 24);
            this.chkItalic.TabIndex = 69;
            this.chkItalic.Text = "Italic";
            this.chkItalic.UseVisualStyleBackColor = true;
            // 
            // chkUnderline
            // 
            this.chkUnderline.AutoSize = true;
            this.chkUnderline.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkUnderline.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.chkUnderline.Location = new System.Drawing.Point(380, 408);
            this.chkUnderline.Name = "chkUnderline";
            this.chkUnderline.Size = new System.Drawing.Size(92, 24);
            this.chkUnderline.TabIndex = 70;
            this.chkUnderline.Text = "Underline";
            this.chkUnderline.UseVisualStyleBackColor = true;
            // 
            // chkStrikeOut
            // 
            this.chkStrikeOut.AutoSize = true;
            this.chkStrikeOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkStrikeOut.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.chkStrikeOut.Location = new System.Drawing.Point(380, 441);
            this.chkStrikeOut.Name = "chkStrikeOut";
            this.chkStrikeOut.Size = new System.Drawing.Size(93, 24);
            this.chkStrikeOut.TabIndex = 71;
            this.chkStrikeOut.Text = "StrikeOut";
            this.chkStrikeOut.UseVisualStyleBackColor = true;
            // 
            // cmdSetSleepMsg
            // 
            this.cmdSetSleepMsg.BackColor = System.Drawing.SystemColors.Control;
            this.cmdSetSleepMsg.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdSetSleepMsg.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSetSleepMsg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdSetSleepMsg.Location = new System.Drawing.Point(243, 547);
            this.cmdSetSleepMsg.Name = "cmdSetSleepMsg";
            this.cmdSetSleepMsg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdSetSleepMsg.Size = new System.Drawing.Size(200, 30);
            this.cmdSetSleepMsg.TabIndex = 73;
            this.cmdSetSleepMsg.Text = "Set Sleep Message";
            this.cmdSetSleepMsg.UseVisualStyleBackColor = false;
            this.cmdSetSleepMsg.Click += new System.EventHandler(this.cmdSetSleepMsg_Click);
            // 
            // cmdGetSleepMsg
            // 
            this.cmdGetSleepMsg.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetSleepMsg.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetSleepMsg.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetSleepMsg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetSleepMsg.Location = new System.Drawing.Point(37, 547);
            this.cmdGetSleepMsg.Name = "cmdGetSleepMsg";
            this.cmdGetSleepMsg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetSleepMsg.Size = new System.Drawing.Size(200, 30);
            this.cmdGetSleepMsg.TabIndex = 72;
            this.cmdGetSleepMsg.Text = "Get Sleep Message";
            this.cmdGetSleepMsg.UseVisualStyleBackColor = false;
            this.cmdGetSleepMsg.Click += new System.EventHandler(this.cmdGetSleepMsg_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.SystemColors.Control;
            this.cmdExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdExit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdExit.Location = new System.Drawing.Point(458, 547);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdExit.Size = new System.Drawing.Size(113, 30);
            this.cmdExit.TabIndex = 74;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = false;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // chkDebugOut
            // 
            this.chkDebugOut.AutoSize = true;
            this.chkDebugOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkDebugOut.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.chkDebugOut.Location = new System.Drawing.Point(52, 497);
            this.chkDebugOut.Name = "chkDebugOut";
            this.chkDebugOut.Size = new System.Drawing.Size(97, 24);
            this.chkDebugOut.TabIndex = 75;
            this.chkDebugOut.Text = "DebugOut";
            this.chkDebugOut.UseVisualStyleBackColor = true;
            // 
            // txtDebugOutFile
            // 
            this.txtDebugOutFile.AcceptsReturn = true;
            this.txtDebugOutFile.BackColor = System.Drawing.SystemColors.Window;
            this.txtDebugOutFile.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDebugOutFile.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDebugOutFile.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDebugOutFile.Location = new System.Drawing.Point(159, 497);
            this.txtDebugOutFile.MaxLength = 8;
            this.txtDebugOutFile.Name = "txtDebugOutFile";
            this.txtDebugOutFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDebugOutFile.Size = new System.Drawing.Size(355, 26);
            this.txtDebugOutFile.TabIndex = 76;
            this.txtDebugOutFile.Text = "C:\\temp.bmp";
            // 
            // cmdDebugOutFileBrowse
            // 
            this.cmdDebugOutFileBrowse.BackColor = System.Drawing.SystemColors.Control;
            this.cmdDebugOutFileBrowse.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdDebugOutFileBrowse.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDebugOutFileBrowse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdDebugOutFileBrowse.Location = new System.Drawing.Point(514, 496);
            this.cmdDebugOutFileBrowse.Name = "cmdDebugOutFileBrowse";
            this.cmdDebugOutFileBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdDebugOutFileBrowse.Size = new System.Drawing.Size(30, 27);
            this.cmdDebugOutFileBrowse.TabIndex = 77;
            this.cmdDebugOutFileBrowse.Text = "...";
            this.cmdDebugOutFileBrowse.UseVisualStyleBackColor = false;
            this.cmdDebugOutFileBrowse.Click += new System.EventHandler(this.cmdDebugOutFileBrowse_Click);
            // 
            // frmScreenSaver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 589);
            this.Controls.Add(this.cmdDebugOutFileBrowse);
            this.Controls.Add(this.txtDebugOutFile);
            this.Controls.Add(this.chkDebugOut);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdSetSleepMsg);
            this.Controls.Add(this.cmdGetSleepMsg);
            this.Controls.Add(this.chkStrikeOut);
            this.Controls.Add(this.chkUnderline);
            this.Controls.Add(this.chkItalic);
            this.Controls.Add(this.cmdGetGlyphSize);
            this.Controls.Add(this.txtFontWeight);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtFontWidth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtFontHeight);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtGlyphHeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtGlyphWidth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSleepMessage);
            this.Controls.Add(this.cmdSetCustomerInfo);
            this.Controls.Add(this.cmdGetCustomerInfo);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.txtCompanyName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblEnrollData);
            this.Controls.Add(this.lblMessage);
            this.Name = "frmScreenSaver";
            this.Text = "frmScreenSaver";
            this.Load += new System.EventHandler(this.frmScreenSaver_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScreenSaver_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblMessage;
        public System.Windows.Forms.Label lblEnrollData;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtCompanyName;
        public System.Windows.Forms.TextBox txtCustomerName;
        public System.Windows.Forms.Button cmdGetCustomerInfo;
        public System.Windows.Forms.Button cmdSetCustomerInfo;
        private System.Windows.Forms.TextBox txtSleepMessage;
        public System.Windows.Forms.TextBox txtGlyphWidth;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtGlyphHeight;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtFontHeight;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtFontWidth;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtFontWeight;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Button cmdGetGlyphSize;
        private System.Windows.Forms.CheckBox chkItalic;
        private System.Windows.Forms.CheckBox chkUnderline;
        private System.Windows.Forms.CheckBox chkStrikeOut;
        public System.Windows.Forms.Button cmdSetSleepMsg;
        public System.Windows.Forms.Button cmdGetSleepMsg;
        public System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.CheckBox chkDebugOut;
        public System.Windows.Forms.TextBox txtDebugOutFile;
        public System.Windows.Forms.Button cmdDebugOutFileBrowse;
        private System.Windows.Forms.SaveFileDialog OpenSaveDlg;
    }
}