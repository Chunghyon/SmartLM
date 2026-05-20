namespace A3_8190_HTTPServer
{
    partial class frmPersonnel
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
            this.grpList    = new System.Windows.Forms.GroupBox();
            this.lvPersonnel= new System.Windows.Forms.ListView();
            this.colUID     = new System.Windows.Forms.ColumnHeader();
            this.colName    = new System.Windows.Forms.ColumnHeader();
            this.colDeptH   = new System.Windows.Forms.ColumnHeader();
            this.colJobH    = new System.Windows.Forms.ColumnHeader();
            this.colCardH   = new System.Windows.Forms.ColumnHeader();
            this.colAccess  = new System.Windows.Forms.ColumnHeader();
            this.lblCount   = new System.Windows.Forms.Label();
            this.btnDelete  = new System.Windows.Forms.Button();
            this.btnClearAll= new System.Windows.Forms.Button();

            this.grpAdd     = new System.Windows.Forms.GroupBox();
            this.lblUID     = new System.Windows.Forms.Label();
            this.txtUserID  = new System.Windows.Forms.TextBox();
            this.lblNameL   = new System.Windows.Forms.Label();
            this.txtName    = new System.Windows.Forms.TextBox();
            this.lblDept    = new System.Windows.Forms.Label();
            this.txtDept    = new System.Windows.Forms.TextBox();
            this.lblJobL    = new System.Windows.Forms.Label();
            this.txtJob     = new System.Windows.Forms.TextBox();
            this.lblCard    = new System.Windows.Forms.Label();
            this.txtCard    = new System.Windows.Forms.TextBox();
            this.lblPwd     = new System.Windows.Forms.Label();
            this.txtPassword= new System.Windows.Forms.TextBox();
            this.grpAccess  = new System.Windows.Forms.GroupBox();
            this.rdoNormal  = new System.Windows.Forms.RadioButton();
            this.rdoAdmin   = new System.Windows.Forms.RadioButton();
            this.rdoBlacklist=new System.Windows.Forms.RadioButton();
            this.btnAdd     = new System.Windows.Forms.Button();

            this.grpPush    = new System.Windows.Forms.GroupBox();
            this.lblPushSN  = new System.Windows.Forms.Label();
            this.txtPushSN  = new System.Windows.Forms.TextBox();
            this.btnPushAdd = new System.Windows.Forms.Button();
            this.btnPushDelete=new System.Windows.Forms.Button();

            this.grpList.SuspendLayout();
            this.grpAdd.SuspendLayout();
            this.grpAccess.SuspendLayout();
            this.grpPush.SuspendLayout();
            this.SuspendLayout();

            // ─── grpList ─────────────────────────────────────────────────────
            this.grpList.Text     = "인원 목록";
            this.grpList.Location = new System.Drawing.Point(12, 12);
            this.grpList.Size     = new System.Drawing.Size(660, 185);
            this.grpList.Controls.Add(this.lvPersonnel);
            this.grpList.Controls.Add(this.lblCount);
            this.grpList.Controls.Add(this.btnDelete);
            this.grpList.Controls.Add(this.btnClearAll);

            this.colUID.Text    = "사용자 ID";       this.colUID.Width   = 80;
            this.colName.Text   = "이름";         this.colName.Width  = 90;
            this.colDeptH.Text  = "부서";         this.colDeptH.Width = 100;
            this.colJobH.Text   = "직위";         this.colJobH.Width  = 80;
            this.colCardH.Text  = "카드 번호";    this.colCardH.Width = 110;
            this.colAccess.Text = "권한";         this.colAccess.Width= 80;

            this.lvPersonnel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colUID, this.colName, this.colDeptH, this.colJobH, this.colCardH, this.colAccess });
            this.lvPersonnel.Location     = new System.Drawing.Point(8, 22);
            this.lvPersonnel.Size         = new System.Drawing.Size(644, 120);
            this.lvPersonnel.View         = System.Windows.Forms.View.Details;
            this.lvPersonnel.FullRowSelect= true;
            this.lvPersonnel.GridLines    = true;

            this.lblCount.Text     = "총 0명";
            this.lblCount.Location = new System.Drawing.Point(10, 150);
            this.lblCount.Size     = new System.Drawing.Size(80, 25);
            this.lblCount.TextAlign= System.Drawing.ContentAlignment.MiddleLeft;

            this.btnDelete.Text      = "선택 삭제";
            this.btnDelete.Location  = new System.Drawing.Point(474, 148);
            this.btnDelete.Size      = new System.Drawing.Size(85, 28);
            this.btnDelete.BackColor = System.Drawing.Color.Tomato;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Click    += new System.EventHandler(this.btnDelete_Click);

            this.btnClearAll.Text      = "전체 초기화";
            this.btnClearAll.Location  = new System.Drawing.Point(567, 148);
            this.btnClearAll.Size      = new System.Drawing.Size(85, 28);
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Click    += new System.EventHandler(this.btnClearAll_Click);

            // ─── grpAdd ──────────────────────────────────────────────────────
            this.grpAdd.Text     = "인원 추가";
            this.grpAdd.Location = new System.Drawing.Point(12, 206);
            this.grpAdd.Size     = new System.Drawing.Size(660, 145);
            this.grpAdd.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblUID, this.txtUserID, this.lblNameL, this.txtName,
                this.lblDept, this.txtDept, this.lblJobL, this.txtJob,
                this.lblCard, this.txtCard, this.lblPwd, this.txtPassword,
                this.grpAccess, this.btnAdd });

            // Row 1: UserID / Name / Dept
            this.lblUID.Text      = "사용자 ID *";
            this.lblUID.Location  = new System.Drawing.Point(10, 28); this.lblUID.Size = new System.Drawing.Size(65, 23);
            this.lblUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtUserID.Location = new System.Drawing.Point(80, 26); this.txtUserID.Size = new System.Drawing.Size(80, 23);

            this.lblNameL.Text    = "이름";
            this.lblNameL.Location= new System.Drawing.Point(170, 28); this.lblNameL.Size = new System.Drawing.Size(45, 23);
            this.lblNameL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtName.Location = new System.Drawing.Point(220, 26); this.txtName.Size = new System.Drawing.Size(100, 23);

            this.lblDept.Text     = "부서";
            this.lblDept.Location = new System.Drawing.Point(328, 28); this.lblDept.Size = new System.Drawing.Size(45, 23);
            this.lblDept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtDept.Location = new System.Drawing.Point(378, 26); this.txtDept.Size = new System.Drawing.Size(110, 23);

            // Row 2: Job / Card / Password
            this.lblJobL.Text     = "직위";
            this.lblJobL.Location = new System.Drawing.Point(10, 62); this.lblJobL.Size = new System.Drawing.Size(65, 23);
            this.lblJobL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtJob.Location  = new System.Drawing.Point(80, 60); this.txtJob.Size = new System.Drawing.Size(80, 23);

            this.lblCard.Text     = "카드 번호";
            this.lblCard.Location = new System.Drawing.Point(170, 62); this.lblCard.Size = new System.Drawing.Size(45, 23);
            this.lblCard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtCard.Location = new System.Drawing.Point(220, 60); this.txtCard.Size = new System.Drawing.Size(100, 23);

            this.lblPwd.Text      = "비밀번호";
            this.lblPwd.Location  = new System.Drawing.Point(328, 62); this.lblPwd.Size = new System.Drawing.Size(45, 23);
            this.lblPwd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtPassword.Location = new System.Drawing.Point(378, 60); this.txtPassword.Size = new System.Drawing.Size(80, 23);
            this.txtPassword.MaxLength = 8; this.txtPassword.PasswordChar = '*';

            // Row 3: AccessType + 추가 버튼
            this.grpAccess.Text     = "권한";
            this.grpAccess.Location = new System.Drawing.Point(10, 94);
            this.grpAccess.Size     = new System.Drawing.Size(250, 36);
            this.grpAccess.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.rdoNormal, this.rdoAdmin, this.rdoBlacklist });

            this.rdoNormal.Text     = "일반";     this.rdoNormal.Location   = new System.Drawing.Point(5, 12);
            this.rdoNormal.Size     = new System.Drawing.Size(60, 20); this.rdoNormal.Checked = true;
            this.rdoAdmin.Text      = "관리자";   this.rdoAdmin.Location    = new System.Drawing.Point(75, 12);
            this.rdoAdmin.Size      = new System.Drawing.Size(70, 20);
            this.rdoBlacklist.Text  = "블랙리스트"; this.rdoBlacklist.Location= new System.Drawing.Point(153, 12);
            this.rdoBlacklist.Size  = new System.Drawing.Size(90, 20);

            this.btnAdd.Text      = "목록에 추가";
            this.btnAdd.Location  = new System.Drawing.Point(540, 98);
            this.btnAdd.Size      = new System.Drawing.Size(110, 32);
            this.btnAdd.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Click    += new System.EventHandler(this.btnAdd_Click);

            // ─── grpPush ─────────────────────────────────────────────────────
            this.grpPush.Text     = "디바이스로 전송";
            this.grpPush.Location = new System.Drawing.Point(12, 360);
            this.grpPush.Size     = new System.Drawing.Size(660, 60);
            this.grpPush.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblPushSN, this.txtPushSN, this.btnPushAdd, this.btnPushDelete });

            this.lblPushSN.Text      = "디바이스 SN:";
            this.lblPushSN.Location  = new System.Drawing.Point(10, 24);
            this.lblPushSN.Size      = new System.Drawing.Size(85, 23);
            this.lblPushSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.txtPushSN.Location  = new System.Drawing.Point(100, 22);
            this.txtPushSN.Size      = new System.Drawing.Size(200, 23);
            this.txtPushSN.PlaceholderText = "예: FC-8200H12345678";

            this.btnPushAdd.Text      = "인원 추가 전송";
            this.btnPushAdd.Location  = new System.Drawing.Point(315, 20);
            this.btnPushAdd.Size      = new System.Drawing.Size(120, 30);
            this.btnPushAdd.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnPushAdd.ForeColor = System.Drawing.Color.White;
            this.btnPushAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushAdd.Click    += new System.EventHandler(this.btnPushAdd_Click);

            this.btnPushDelete.Text      = "인원 삭제 전송";
            this.btnPushDelete.Location  = new System.Drawing.Point(445, 20);
            this.btnPushDelete.Size      = new System.Drawing.Size(120, 30);
            this.btnPushDelete.BackColor = System.Drawing.Color.Tomato;
            this.btnPushDelete.ForeColor = System.Drawing.Color.White;
            this.btnPushDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushDelete.Click    += new System.EventHandler(this.btnPushDelete_Click);

            // ─── Form ────────────────────────────────────────────────────────
            this.ClientSize      = new System.Drawing.Size(684, 436);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.grpList, this.grpAdd, this.grpPush });
            this.Text            = "인원 관리  ─  A3-8190";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Font            = new System.Drawing.Font("Malgun Gothic", 9F);

            this.grpList.ResumeLayout(false);
            this.grpAdd.ResumeLayout(false);
            this.grpAccess.ResumeLayout(false);
            this.grpPush.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ─── Field Declarations ──────────────────────────────────────────────
        private System.Windows.Forms.GroupBox     grpList;
        private System.Windows.Forms.ListView     lvPersonnel;
        private System.Windows.Forms.ColumnHeader colUID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colDeptH;
        private System.Windows.Forms.ColumnHeader colJobH;
        private System.Windows.Forms.ColumnHeader colCardH;
        private System.Windows.Forms.ColumnHeader colAccess;
        private System.Windows.Forms.Label        lblCount;
        private System.Windows.Forms.Button       btnDelete;
        private System.Windows.Forms.Button       btnClearAll;

        private System.Windows.Forms.GroupBox     grpAdd;
        private System.Windows.Forms.Label        lblUID;
        private System.Windows.Forms.TextBox      txtUserID;
        private System.Windows.Forms.Label        lblNameL;
        private System.Windows.Forms.TextBox      txtName;
        private System.Windows.Forms.Label        lblDept;
        private System.Windows.Forms.TextBox      txtDept;
        private System.Windows.Forms.Label        lblJobL;
        private System.Windows.Forms.TextBox      txtJob;
        private System.Windows.Forms.Label        lblCard;
        private System.Windows.Forms.TextBox      txtCard;
        private System.Windows.Forms.Label        lblPwd;
        private System.Windows.Forms.TextBox      txtPassword;
        private System.Windows.Forms.GroupBox     grpAccess;
        private System.Windows.Forms.RadioButton  rdoNormal;
        private System.Windows.Forms.RadioButton  rdoAdmin;
        private System.Windows.Forms.RadioButton  rdoBlacklist;
        private System.Windows.Forms.Button       btnAdd;

        private System.Windows.Forms.GroupBox     grpPush;
        private System.Windows.Forms.Label        lblPushSN;
        private System.Windows.Forms.TextBox      txtPushSN;
        private System.Windows.Forms.Button       btnPushAdd;
        private System.Windows.Forms.Button       btnPushDelete;
    }
}
