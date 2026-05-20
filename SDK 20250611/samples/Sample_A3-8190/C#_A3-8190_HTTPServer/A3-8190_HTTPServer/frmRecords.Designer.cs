namespace A3_8190_HTTPServer
{
    partial class frmRecords
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
            this.grpRecords  = new System.Windows.Forms.GroupBox();
            this.lvRecords   = new System.Windows.Forms.ListView();
            this.colRecID    = new System.Windows.Forms.ColumnHeader();
            this.colRSN      = new System.Windows.Forms.ColumnHeader();
            this.colRUID     = new System.Windows.Forms.ColumnHeader();
            this.colRName    = new System.Windows.Forms.ColumnHeader();
            this.colRDept    = new System.Windows.Forms.ColumnHeader();
            this.colRType    = new System.Windows.Forms.ColumnHeader();
            this.colREntry   = new System.Windows.Forms.ColumnHeader();
            this.colRTime    = new System.Windows.Forms.ColumnHeader();
            this.colRTemp    = new System.Windows.Forms.ColumnHeader();
            this.colRPhoto   = new System.Windows.Forms.ColumnHeader();

            this.pnlBottom   = new System.Windows.Forms.Panel();
            this.lblCount    = new System.Windows.Forms.Label();
            this.btnClear    = new System.Windows.Forms.Button();
            this.btnExport   = new System.Windows.Forms.Button();

            this.grpRecords.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();

            // ─── Column Headers ───────────────────────────────────────────────
            this.colRecID.Text   = "기록 ID";      this.colRecID.Width  = 80;
            this.colRSN.Text     = "장치 SN";      this.colRSN.Width    = 150;
            this.colRUID.Text    = "사용자 ID";    this.colRUID.Width   = 70;
            this.colRName.Text   = "이름";         this.colRName.Width  = 80;
            this.colRDept.Text   = "부서";         this.colRDept.Width  = 90;
            this.colRType.Text   = "인증 방법";    this.colRType.Width  = 100;
            this.colREntry.Text  = "입출";         this.colREntry.Width = 50;
            this.colRTime.Text   = "시간";         this.colRTime.Width  = 140;
            this.colRTemp.Text   = "체온";         this.colRTemp.Width  = 60;
            this.colRPhoto.Text  = "이미지";       this.colRPhoto.Width = 80;

            // ─── lvRecords ────────────────────────────────────────────────────
            this.lvRecords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colRecID, this.colRSN, this.colRUID, this.colRName, this.colRDept,
                this.colRType, this.colREntry, this.colRTime, this.colRTemp, this.colRPhoto });
            this.lvRecords.Location      = new System.Drawing.Point(8, 22);
            this.lvRecords.Size          = new System.Drawing.Size(964, 490);
            this.lvRecords.View          = System.Windows.Forms.View.Details;
            this.lvRecords.FullRowSelect = true;
            this.lvRecords.GridLines     = true;
            this.lvRecords.Font          = new System.Drawing.Font("Malgun Gothic", 8.5F);

            // ─── grpRecords ───────────────────────────────────────────────────
            this.grpRecords.Text     = "출입 기록   —   최신 기록이 상단에 표시됩니다";
            this.grpRecords.Location = new System.Drawing.Point(12, 12);
            this.grpRecords.Size     = new System.Drawing.Size(980, 522);
            this.grpRecords.Controls.Add(this.lvRecords);

            // ─── pnlBottom ────────────────────────────────────────────────────
            this.pnlBottom.Location = new System.Drawing.Point(12, 540);
            this.pnlBottom.Size     = new System.Drawing.Size(980, 36);
            this.pnlBottom.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblCount, this.btnClear, this.btnExport });

            this.lblCount.Text      = "총 0건";
            this.lblCount.Location  = new System.Drawing.Point(0, 8);
            this.lblCount.Size      = new System.Drawing.Size(100, 23);
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCount.Font      = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold);

            this.btnClear.Text      = "기록 지우기";
            this.btnClear.Location  = new System.Drawing.Point(750, 3);
            this.btnClear.Size      = new System.Drawing.Size(110, 30);
            this.btnClear.BackColor = System.Drawing.Color.Tomato;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Click    += new System.EventHandler(this.btnClear_Click);

            this.btnExport.Text      = "CSV 내보내기";
            this.btnExport.Location  = new System.Drawing.Point(868, 3);
            this.btnExport.Size      = new System.Drawing.Size(110, 30);
            this.btnExport.BackColor = System.Drawing.Color.SteelBlue;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Click    += new System.EventHandler(this.btnExport_Click);

            // ─── Form ────────────────────────────────────────────────────────
            this.ClientSize      = new System.Drawing.Size(1004, 586);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.grpRecords, this.pnlBottom });
            this.Text            = "출입 기록  ─  A3-8190";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize     = new System.Drawing.Size(800, 400);
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Font            = new System.Drawing.Font("Malgun Gothic", 9F);

            this.grpRecords.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ─── Field Declarations ──────────────────────────────────────────────
        private System.Windows.Forms.GroupBox     grpRecords;
        private System.Windows.Forms.ListView     lvRecords;
        private System.Windows.Forms.ColumnHeader colRecID;
        private System.Windows.Forms.ColumnHeader colRSN;
        private System.Windows.Forms.ColumnHeader colRUID;
        private System.Windows.Forms.ColumnHeader colRName;
        private System.Windows.Forms.ColumnHeader colRDept;
        private System.Windows.Forms.ColumnHeader colRType;
        private System.Windows.Forms.ColumnHeader colREntry;
        private System.Windows.Forms.ColumnHeader colRTime;
        private System.Windows.Forms.ColumnHeader colRTemp;
        private System.Windows.Forms.ColumnHeader colRPhoto;

        private System.Windows.Forms.Panel        pnlBottom;
        private System.Windows.Forms.Label        lblCount;
        private System.Windows.Forms.Button       btnClear;
        private System.Windows.Forms.Button       btnExport;
    }
}
