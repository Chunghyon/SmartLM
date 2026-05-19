namespace SBXPCSampleCSharp
{
    partial class frmMiscSettings
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
            this.cmdWarnCancel = new System.Windows.Forms.Button();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdRestart = new System.Windows.Forms.Button();
            this.cmdAutoRecover = new System.Windows.Forms.Button();
            this.cmdUncondClose = new System.Windows.Forms.Button();
            this.cmdUncondOpen = new System.Windows.Forms.Button();
            this.cmdGetDoorStatus = new System.Windows.Forms.Button();
            this.cmdDoorOpen = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtDoorNumber = new System.Windows.Forms.TextBox();
            this._lblEnrollNum_0 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbAccessMode = new System.Windows.Forms.ComboBox();
            this.txtSynchOpenCount = new System.Windows.Forms.TextBox();
            this.txtIllegVerifTimes = new System.Windows.Forms.TextBox();
            this.cmbAlarmOutMode = new System.Windows.Forms.ComboBox();
            this.txtDuressDelay = new System.Windows.Forms.TextBox();
            this.cmbDualFpMode = new System.Windows.Forms.ComboBox();
            this.txtDualFpTimeout = new System.Windows.Forms.TextBox();
            this.cmdRead = new System.Windows.Forms.Button();
            this.cmdWrite = new System.Windows.Forms.Button();
            this.cmbUseM1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdWarnCancel
            // 
            this.cmdWarnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.cmdWarnCancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdWarnCancel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdWarnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdWarnCancel.Location = new System.Drawing.Point(16, 102);
            this.cmdWarnCancel.Name = "cmdWarnCancel";
            this.cmdWarnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdWarnCancel.Size = new System.Drawing.Size(125, 30);
            this.cmdWarnCancel.TabIndex = 15;
            this.cmdWarnCancel.Text = "Warn Cancel";
            this.cmdWarnCancel.UseVisualStyleBackColor = false;
            this.cmdWarnCancel.Click += new System.EventHandler(this.cmdWarnCancel_Click);
            // 
            // cmdRestart
            // 
            this.cmdRestart.BackColor = System.Drawing.SystemColors.Control;
            this.cmdRestart.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdRestart.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRestart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdRestart.Location = new System.Drawing.Point(147, 102);
            this.cmdRestart.Name = "cmdRestart";
            this.cmdRestart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdRestart.Size = new System.Drawing.Size(124, 30);
            this.cmdRestart.TabIndex = 14;
            this.cmdRestart.Text = "Reboot";
            this.cmdRestart.UseVisualStyleBackColor = false;
            this.cmdRestart.Click += new System.EventHandler(this.cmdRestart_Click);
            // 
            // cmdAutoRecover
            // 
            this.cmdAutoRecover.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAutoRecover.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdAutoRecover.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAutoRecover.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdAutoRecover.Location = new System.Drawing.Point(146, 20);
            this.cmdAutoRecover.Name = "cmdAutoRecover";
            this.cmdAutoRecover.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdAutoRecover.Size = new System.Drawing.Size(124, 31);
            this.cmdAutoRecover.TabIndex = 13;
            this.cmdAutoRecover.Text = "Auto Recover";
            this.cmdAutoRecover.UseVisualStyleBackColor = false;
            this.cmdAutoRecover.Click += new System.EventHandler(this.cmdAutoRecover_Click);
            // 
            // cmdUncondClose
            // 
            this.cmdUncondClose.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUncondClose.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUncondClose.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUncondClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUncondClose.Location = new System.Drawing.Point(147, 57);
            this.cmdUncondClose.Name = "cmdUncondClose";
            this.cmdUncondClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUncondClose.Size = new System.Drawing.Size(126, 30);
            this.cmdUncondClose.TabIndex = 12;
            this.cmdUncondClose.Text = "Uncond Close";
            this.cmdUncondClose.UseVisualStyleBackColor = false;
            this.cmdUncondClose.Click += new System.EventHandler(this.cmdUncondClose_Click);
            // 
            // cmdUncondOpen
            // 
            this.cmdUncondOpen.BackColor = System.Drawing.SystemColors.Control;
            this.cmdUncondOpen.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdUncondOpen.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdUncondOpen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdUncondOpen.Location = new System.Drawing.Point(15, 57);
            this.cmdUncondOpen.Name = "cmdUncondOpen";
            this.cmdUncondOpen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdUncondOpen.Size = new System.Drawing.Size(126, 30);
            this.cmdUncondOpen.TabIndex = 11;
            this.cmdUncondOpen.Text = "Uncond Open";
            this.cmdUncondOpen.UseVisualStyleBackColor = false;
            this.cmdUncondOpen.Click += new System.EventHandler(this.cmdUncondOpen_Click);
            // 
            // cmdGetDoorStatus
            // 
            this.cmdGetDoorStatus.BackColor = System.Drawing.SystemColors.Control;
            this.cmdGetDoorStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdGetDoorStatus.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGetDoorStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdGetDoorStatus.Location = new System.Drawing.Point(276, 21);
            this.cmdGetDoorStatus.Name = "cmdGetDoorStatus";
            this.cmdGetDoorStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdGetDoorStatus.Size = new System.Drawing.Size(122, 30);
            this.cmdGetDoorStatus.TabIndex = 10;
            this.cmdGetDoorStatus.Text = "Get DoorStatus";
            this.cmdGetDoorStatus.UseVisualStyleBackColor = false;
            this.cmdGetDoorStatus.Click += new System.EventHandler(this.cmdGetDoorStatus_Click);
            // 
            // cmdDoorOpen
            // 
            this.cmdDoorOpen.BackColor = System.Drawing.SystemColors.Control;
            this.cmdDoorOpen.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdDoorOpen.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDoorOpen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdDoorOpen.Location = new System.Drawing.Point(15, 20);
            this.cmdDoorOpen.Name = "cmdDoorOpen";
            this.cmdDoorOpen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdDoorOpen.Size = new System.Drawing.Size(125, 30);
            this.cmdDoorOpen.TabIndex = 9;
            this.cmdDoorOpen.Text = "Door Open";
            this.cmdDoorOpen.UseVisualStyleBackColor = false;
            this.cmdDoorOpen.Click += new System.EventHandler(this.cmdDoorOpen_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(7, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(401, 26);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDoorNumber
            // 
            this.txtDoorNumber.AcceptsReturn = true;
            this.txtDoorNumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtDoorNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDoorNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDoorNumber.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDoorNumber.Location = new System.Drawing.Point(233, 47);
            this.txtDoorNumber.MaxLength = 8;
            this.txtDoorNumber.Name = "txtDoorNumber";
            this.txtDoorNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDoorNumber.Size = new System.Drawing.Size(81, 26);
            this.txtDoorNumber.TabIndex = 40;
            this.txtDoorNumber.Text = "0";
            // 
            // _lblEnrollNum_0
            // 
            this._lblEnrollNum_0.AutoSize = true;
            this._lblEnrollNum_0.BackColor = System.Drawing.SystemColors.Control;
            this._lblEnrollNum_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblEnrollNum_0.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblEnrollNum_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblEnrollNum_0.Location = new System.Drawing.Point(105, 51);
            this._lblEnrollNum_0.Name = "_lblEnrollNum_0";
            this._lblEnrollNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblEnrollNum_0.Size = new System.Drawing.Size(105, 19);
            this._lblEnrollNum_0.TabIndex = 39;
            this._lblEnrollNum_0.Text = "Enroll Number :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdGetDoorStatus);
            this.groupBox1.Controls.Add(this.cmdDoorOpen);
            this.groupBox1.Controls.Add(this.cmdAutoRecover);
            this.groupBox1.Controls.Add(this.cmdRestart);
            this.groupBox1.Controls.Add(this.cmdWarnCancel);
            this.groupBox1.Controls.Add(this.cmdUncondOpen);
            this.groupBox1.Controls.Add(this.cmdUncondClose);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(21, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 146);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Door Control";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(93, 252);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(109, 19);
            this.label1.TabIndex = 42;
            this.label1.Text = "Access Mode :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(60, 282);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(142, 19);
            this.label2.TabIndex = 43;
            this.label2.Text = "Synch Open Count :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(16, 312);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(186, 19);
            this.label3.TabIndex = 44;
            this.label3.Text = "Illegal Verification Times :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(49, 342);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(153, 19);
            this.label4.TabIndex = 45;
            this.label4.Text = "Alarm Output Mode :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(92, 372);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(110, 19);
            this.label5.TabIndex = 46;
            this.label5.Text = "Duress Delay :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(87, 402);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(115, 19);
            this.label6.TabIndex = 47;
            this.label6.Text = "Dual Fp Mode :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(72, 432);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(130, 19);
            this.label7.TabIndex = 48;
            this.label7.Text = "Dual Fp Timeout :";
            // 
            // cmbAccessMode
            // 
            this.cmbAccessMode.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAccessMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbAccessMode.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAccessMode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbAccessMode.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.cmbAccessMode.Items.AddRange(new object[] {
            "Standalone",
            "Single-Door",
            "Multi-Door"});
            this.cmbAccessMode.Location = new System.Drawing.Point(210, 247);
            this.cmbAccessMode.Name = "cmbAccessMode";
            this.cmbAccessMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbAccessMode.Size = new System.Drawing.Size(104, 27);
            this.cmbAccessMode.TabIndex = 49;
            this.cmbAccessMode.Text = "Standalone";
            // 
            // txtSynchOpenCount
            // 
            this.txtSynchOpenCount.AcceptsReturn = true;
            this.txtSynchOpenCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtSynchOpenCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSynchOpenCount.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSynchOpenCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSynchOpenCount.Location = new System.Drawing.Point(210, 277);
            this.txtSynchOpenCount.MaxLength = 8;
            this.txtSynchOpenCount.Name = "txtSynchOpenCount";
            this.txtSynchOpenCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtSynchOpenCount.Size = new System.Drawing.Size(104, 26);
            this.txtSynchOpenCount.TabIndex = 50;
            this.txtSynchOpenCount.Text = "0";
            // 
            // txtIllegVerifTimes
            // 
            this.txtIllegVerifTimes.AcceptsReturn = true;
            this.txtIllegVerifTimes.BackColor = System.Drawing.SystemColors.Window;
            this.txtIllegVerifTimes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIllegVerifTimes.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIllegVerifTimes.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtIllegVerifTimes.Location = new System.Drawing.Point(210, 307);
            this.txtIllegVerifTimes.MaxLength = 8;
            this.txtIllegVerifTimes.Name = "txtIllegVerifTimes";
            this.txtIllegVerifTimes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtIllegVerifTimes.Size = new System.Drawing.Size(104, 26);
            this.txtIllegVerifTimes.TabIndex = 51;
            this.txtIllegVerifTimes.Text = "0";
            // 
            // cmbAlarmOutMode
            // 
            this.cmbAlarmOutMode.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAlarmOutMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbAlarmOutMode.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAlarmOutMode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbAlarmOutMode.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.cmbAlarmOutMode.Items.AddRange(new object[] {
            "Out+In",
            "In",
            "Out"});
            this.cmbAlarmOutMode.Location = new System.Drawing.Point(210, 337);
            this.cmbAlarmOutMode.Name = "cmbAlarmOutMode";
            this.cmbAlarmOutMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbAlarmOutMode.Size = new System.Drawing.Size(104, 27);
            this.cmbAlarmOutMode.TabIndex = 52;
            this.cmbAlarmOutMode.Text = "Out+In";
            // 
            // txtDuressDelay
            // 
            this.txtDuressDelay.AcceptsReturn = true;
            this.txtDuressDelay.BackColor = System.Drawing.SystemColors.Window;
            this.txtDuressDelay.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDuressDelay.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDuressDelay.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDuressDelay.Location = new System.Drawing.Point(210, 367);
            this.txtDuressDelay.MaxLength = 8;
            this.txtDuressDelay.Name = "txtDuressDelay";
            this.txtDuressDelay.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDuressDelay.Size = new System.Drawing.Size(104, 26);
            this.txtDuressDelay.TabIndex = 53;
            this.txtDuressDelay.Text = "0";
            // 
            // cmbDualFpMode
            // 
            this.cmbDualFpMode.BackColor = System.Drawing.SystemColors.Window;
            this.cmbDualFpMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbDualFpMode.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDualFpMode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbDualFpMode.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.cmbDualFpMode.Items.AddRange(new object[] {
            "None",
            "FP1+FP1",
            "FP1+FP2"});
            this.cmbDualFpMode.Location = new System.Drawing.Point(210, 397);
            this.cmbDualFpMode.Name = "cmbDualFpMode";
            this.cmbDualFpMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbDualFpMode.Size = new System.Drawing.Size(104, 27);
            this.cmbDualFpMode.TabIndex = 54;
            this.cmbDualFpMode.Text = "None";
            // 
            // txtDualFpTimeout
            // 
            this.txtDualFpTimeout.AcceptsReturn = true;
            this.txtDualFpTimeout.BackColor = System.Drawing.SystemColors.Window;
            this.txtDualFpTimeout.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDualFpTimeout.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDualFpTimeout.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDualFpTimeout.Location = new System.Drawing.Point(210, 427);
            this.txtDualFpTimeout.MaxLength = 8;
            this.txtDualFpTimeout.Name = "txtDualFpTimeout";
            this.txtDualFpTimeout.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDualFpTimeout.Size = new System.Drawing.Size(104, 26);
            this.txtDualFpTimeout.TabIndex = 55;
            this.txtDualFpTimeout.Text = "30";
            // 
            // cmdRead
            // 
            this.cmdRead.BackColor = System.Drawing.SystemColors.Control;
            this.cmdRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdRead.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRead.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdRead.Location = new System.Drawing.Point(334, 312);
            this.cmdRead.Name = "cmdRead";
            this.cmdRead.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdRead.Size = new System.Drawing.Size(85, 25);
            this.cmdRead.TabIndex = 77;
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
            this.cmdWrite.Location = new System.Drawing.Point(334, 343);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdWrite.Size = new System.Drawing.Size(85, 25);
            this.cmdWrite.TabIndex = 78;
            this.cmdWrite.Text = "Write";
            this.cmdWrite.UseVisualStyleBackColor = false;
            this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
            // 
            // cmbUseM1
            // 
            this.cmbUseM1.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUseM1.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbUseM1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUseM1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbUseM1.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.cmbUseM1.Items.AddRange(new object[] {
            "No",
            "Yes"});
            this.cmbUseM1.Location = new System.Drawing.Point(210, 459);
            this.cmbUseM1.Name = "cmbUseM1";
            this.cmbUseM1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbUseM1.Size = new System.Drawing.Size(104, 27);
            this.cmbUseM1.TabIndex = 80;
            this.cmbUseM1.Text = "No";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(87, 464);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(109, 19);
            this.label8.TabIndex = 79;
            this.label8.Text = "Use M1 Card :";
            // 
            // frmMiscSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 499);
            this.Controls.Add(this.cmbUseM1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmdWrite);
            this.Controls.Add(this.cmdRead);
            this.Controls.Add(this.txtDualFpTimeout);
            this.Controls.Add(this.cmbDualFpMode);
            this.Controls.Add(this.txtDuressDelay);
            this.Controls.Add(this.cmbAlarmOutMode);
            this.Controls.Add(this.txtIllegVerifTimes);
            this.Controls.Add(this.txtSynchOpenCount);
            this.Controls.Add(this.cmbAccessMode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtDoorNumber);
            this.Controls.Add(this._lblEnrollNum_0);
            this.Controls.Add(this.lblMessage);
            this.MaximizeBox = false;
            this.Name = "frmMiscSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMiscSettings";
            this.Load += new System.EventHandler(this.frmLockCrl_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLockCrl_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button cmdWarnCancel;
        public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.Button cmdRestart;
        public System.Windows.Forms.Button cmdAutoRecover;
        public System.Windows.Forms.Button cmdUncondClose;
        public System.Windows.Forms.Button cmdUncondOpen;
        public System.Windows.Forms.Button cmdGetDoorStatus;
        public System.Windows.Forms.Button cmdDoorOpen;
        public System.Windows.Forms.Label lblMessage;
        public System.Windows.Forms.TextBox txtDoorNumber;
        public System.Windows.Forms.Label _lblEnrollNum_0;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox cmbAccessMode;
        public System.Windows.Forms.TextBox txtSynchOpenCount;
        public System.Windows.Forms.TextBox txtIllegVerifTimes;
        public System.Windows.Forms.ComboBox cmbAlarmOutMode;
        public System.Windows.Forms.TextBox txtDuressDelay;
        public System.Windows.Forms.ComboBox cmbDualFpMode;
        public System.Windows.Forms.TextBox txtDualFpTimeout;
        public System.Windows.Forms.Button cmdRead;
        public System.Windows.Forms.Button cmdWrite;
        public System.Windows.Forms.ComboBox cmbUseM1;
        public System.Windows.Forms.Label label8;
    }
}