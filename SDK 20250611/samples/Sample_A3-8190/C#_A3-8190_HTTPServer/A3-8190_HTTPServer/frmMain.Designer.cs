namespace A3_8190_HTTPServer
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ─── Controls ────────────────────────────────────────────────────
            this.grpServer   = new System.Windows.Forms.GroupBox();
            this.lblPort     = new System.Windows.Forms.Label();
            this.txtPort     = new System.Windows.Forms.TextBox();
            this.btnStart    = new System.Windows.Forms.Button();
            this.btnStop     = new System.Windows.Forms.Button();
            this.lblStatus   = new System.Windows.Forms.Label();

            this.grpDevices  = new System.Windows.Forms.GroupBox();
            this.lvDevices   = new System.Windows.Forms.ListView();
            this.colSN       = new System.Windows.Forms.ColumnHeader();
            this.colTime     = new System.Windows.Forms.ColumnHeader();
            this.colRelay    = new System.Windows.Forms.ColumnHeader();
            this.colDoor     = new System.Windows.Forms.ColumnHeader();
            this.colLock     = new System.Windows.Forms.ColumnHeader();
            this.colAlarm    = new System.Windows.Forms.ColumnHeader();

            this.grpRemote   = new System.Windows.Forms.GroupBox();
            this.lblDevice   = new System.Windows.Forms.Label();
            this.cmbDevice   = new System.Windows.Forms.ComboBox();
            this.btnOpenDoor = new System.Windows.Forms.Button();
            this.btnKeepOpen = new System.Windows.Forms.Button();
            this.btnCloseDoor= new System.Windows.Forms.Button();
            this.btnRestart  = new System.Windows.Forms.Button();

            this.pnlButtons  = new System.Windows.Forms.Panel();
            this.btnPersonnel= new System.Windows.Forms.Button();
            this.btnRecords  = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();

            this.grpLog      = new System.Windows.Forms.GroupBox();
            this.rtbLog      = new System.Windows.Forms.RichTextBox();

            this.grpServer.SuspendLayout();
            this.grpDevices.SuspendLayout();
            this.grpRemote.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();

            // ─── grpServer ───────────────────────────────────────────────────
            this.grpServer.Text     = "서버 설정  (Server Settings)";
            this.grpServer.Location = new System.Drawing.Point(12, 12);
            this.grpServer.Size     = new System.Drawing.Size(660, 60);
            this.grpServer.Controls.Add(this.lblPort);
            this.grpServer.Controls.Add(this.txtPort);
            this.grpServer.Controls.Add(this.btnStart);
            this.grpServer.Controls.Add(this.btnStop);
            this.grpServer.Controls.Add(this.lblStatus);

            this.lblPort.Text      = "포트 (Port):";
            this.lblPort.Location  = new System.Drawing.Point(10, 24);
            this.lblPort.Size      = new System.Drawing.Size(80, 23);
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.txtPort.Text     = "8080";
            this.txtPort.Location = new System.Drawing.Point(95, 22);
            this.txtPort.Size     = new System.Drawing.Size(60, 23);

            this.btnStart.Text     = "서버 시작";
            this.btnStart.Location = new System.Drawing.Point(170, 20);
            this.btnStart.Size     = new System.Drawing.Size(90, 27);
            this.btnStart.BackColor= System.Drawing.Color.MediumSeaGreen;
            this.btnStart.ForeColor= System.Drawing.Color.White;
            this.btnStart.FlatStyle= System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Click   += new System.EventHandler(this.btnStart_Click);

            this.btnStop.Text      = "서버 중지";
            this.btnStop.Location  = new System.Drawing.Point(270, 20);
            this.btnStop.Size      = new System.Drawing.Size(90, 27);
            this.btnStop.BackColor = System.Drawing.Color.IndianRed;
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Enabled   = false;
            this.btnStop.Click    += new System.EventHandler(this.btnStop_Click);

            this.lblStatus.Text      = "● 중지됨";
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location  = new System.Drawing.Point(375, 24);
            this.lblStatus.Size      = new System.Drawing.Size(160, 23);
            this.lblStatus.Font      = new System.Drawing.Font("Malgun Gothic", 9.5F, System.Drawing.FontStyle.Bold);

            // ─── grpDevices ──────────────────────────────────────────────────
            this.grpDevices.Text     = "연결된 디바이스  (Connected Devices)";
            this.grpDevices.Location = new System.Drawing.Point(12, 82);
            this.grpDevices.Size     = new System.Drawing.Size(660, 130);
            this.grpDevices.Controls.Add(this.lvDevices);

            this.colSN.Text    = "디바이스 SN";    this.colSN.Width    = 160;
            this.colTime.Text  = "마지막 연결";    this.colTime.Width  = 80;
            this.colRelay.Text = "릴레이";         this.colRelay.Width = 75;
            this.colDoor.Text  = "도어 센서";      this.colDoor.Width  = 75;
            this.colLock.Text  = "잠금";           this.colLock.Width  = 60;
            this.colAlarm.Text = "알람";           this.colAlarm.Width = 160;

            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colSN, this.colTime, this.colRelay, this.colDoor, this.colLock, this.colAlarm });
            this.lvDevices.Location     = new System.Drawing.Point(8, 22);
            this.lvDevices.Size         = new System.Drawing.Size(644, 100);
            this.lvDevices.View         = System.Windows.Forms.View.Details;
            this.lvDevices.FullRowSelect= true;
            this.lvDevices.GridLines    = true;

            // ─── grpRemote ───────────────────────────────────────────────────
            this.grpRemote.Text     = "원격 제어  (Remote Control)";
            this.grpRemote.Location = new System.Drawing.Point(12, 222);
            this.grpRemote.Size     = new System.Drawing.Size(660, 62);
            this.grpRemote.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblDevice, this.cmbDevice,
                this.btnOpenDoor, this.btnKeepOpen, this.btnCloseDoor, this.btnRestart });

            this.lblDevice.Text      = "디바이스:";
            this.lblDevice.Location  = new System.Drawing.Point(10, 26);
            this.lblDevice.Size      = new System.Drawing.Size(65, 23);
            this.lblDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbDevice.Location       = new System.Drawing.Point(80, 23);
            this.cmbDevice.Size           = new System.Drawing.Size(165, 23);
            this.cmbDevice.DropDownStyle  = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.btnOpenDoor.Text      = "문 열기";
            this.btnOpenDoor.Location  = new System.Drawing.Point(258, 22);
            this.btnOpenDoor.Size      = new System.Drawing.Size(85, 28);
            this.btnOpenDoor.BackColor = System.Drawing.Color.LightGreen;
            this.btnOpenDoor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenDoor.Click    += new System.EventHandler(this.btnOpenDoor_Click);

            this.btnKeepOpen.Text      = "상시 개방";
            this.btnKeepOpen.Location  = new System.Drawing.Point(352, 22);
            this.btnKeepOpen.Size      = new System.Drawing.Size(85, 28);
            this.btnKeepOpen.BackColor = System.Drawing.Color.LightYellow;
            this.btnKeepOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeepOpen.Click    += new System.EventHandler(this.btnKeepOpen_Click);

            this.btnCloseDoor.Text      = "문 닫기";
            this.btnCloseDoor.Location  = new System.Drawing.Point(446, 22);
            this.btnCloseDoor.Size      = new System.Drawing.Size(85, 28);
            this.btnCloseDoor.BackColor = System.Drawing.Color.LightCoral;
            this.btnCloseDoor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseDoor.Click    += new System.EventHandler(this.btnCloseDoor_Click);

            this.btnRestart.Text      = "재시작";
            this.btnRestart.Location  = new System.Drawing.Point(540, 22);
            this.btnRestart.Size      = new System.Drawing.Size(80, 28);
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Click    += new System.EventHandler(this.btnRestart_Click);

            // ─── pnlButtons ──────────────────────────────────────────────────
            this.pnlButtons.Location = new System.Drawing.Point(12, 295);
            this.pnlButtons.Size     = new System.Drawing.Size(660, 40);
            this.pnlButtons.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnPersonnel, this.btnRecords, this.btnClearLog });

            this.btnPersonnel.Text      = "👤  인원 관리";
            this.btnPersonnel.Location  = new System.Drawing.Point(0, 5);
            this.btnPersonnel.Size      = new System.Drawing.Size(130, 30);
            this.btnPersonnel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPersonnel.Click    += new System.EventHandler(this.btnPersonnel_Click);

            this.btnRecords.Text      = "📋  출입 기록";
            this.btnRecords.Location  = new System.Drawing.Point(138, 5);
            this.btnRecords.Size      = new System.Drawing.Size(130, 30);
            this.btnRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecords.Click    += new System.EventHandler(this.btnRecords_Click);

            this.btnClearLog.Text      = "로그 지우기";
            this.btnClearLog.Location  = new System.Drawing.Point(276, 5);
            this.btnClearLog.Size      = new System.Drawing.Size(100, 30);
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.Click    += new System.EventHandler(this.btnClearLog_Click);

            // ─── grpLog ──────────────────────────────────────────────────────
            this.grpLog.Text     = "서버 로그  (Server Log)";
            this.grpLog.Location = new System.Drawing.Point(12, 344);
            this.grpLog.Size     = new System.Drawing.Size(660, 210);
            this.grpLog.Controls.Add(this.rtbLog);

            this.rtbLog.ReadOnly   = true;
            this.rtbLog.Location   = new System.Drawing.Point(8, 22);
            this.rtbLog.Size       = new System.Drawing.Size(644, 180);
            this.rtbLog.Font       = new System.Drawing.Font("Consolas", 8.5F);
            this.rtbLog.BackColor  = System.Drawing.Color.FromArgb(30, 30, 30);
            this.rtbLog.ForeColor  = System.Drawing.Color.LightGreen;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            // ─── Form ────────────────────────────────────────────────────────
            this.ClientSize      = new System.Drawing.Size(684, 568);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.grpServer, this.grpDevices, this.grpRemote, this.pnlButtons, this.grpLog });
            this.Text            = "A3-8190 HTTP 서버 예제  ─  SmartLM / BOWE";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Font            = new System.Drawing.Font("Malgun Gothic", 9F);
            this.FormClosing    += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);

            this.grpServer.ResumeLayout(false);
            this.grpDevices.ResumeLayout(false);
            this.grpRemote.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ─── Field Declarations ──────────────────────────────────────────────
        private System.Windows.Forms.GroupBox   grpServer;
        private System.Windows.Forms.Label      lblPort;
        private System.Windows.Forms.TextBox    txtPort;
        private System.Windows.Forms.Button     btnStart;
        private System.Windows.Forms.Button     btnStop;
        private System.Windows.Forms.Label      lblStatus;

        private System.Windows.Forms.GroupBox   grpDevices;
        private System.Windows.Forms.ListView   lvDevices;
        private System.Windows.Forms.ColumnHeader colSN;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colRelay;
        private System.Windows.Forms.ColumnHeader colDoor;
        private System.Windows.Forms.ColumnHeader colLock;
        private System.Windows.Forms.ColumnHeader colAlarm;

        private System.Windows.Forms.GroupBox   grpRemote;
        private System.Windows.Forms.Label      lblDevice;
        private System.Windows.Forms.ComboBox   cmbDevice;
        private System.Windows.Forms.Button     btnOpenDoor;
        private System.Windows.Forms.Button     btnKeepOpen;
        private System.Windows.Forms.Button     btnCloseDoor;
        private System.Windows.Forms.Button     btnRestart;

        private System.Windows.Forms.Panel      pnlButtons;
        private System.Windows.Forms.Button     btnPersonnel;
        private System.Windows.Forms.Button     btnRecords;
        private System.Windows.Forms.Button     btnClearLog;

        private System.Windows.Forms.GroupBox   grpLog;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}
