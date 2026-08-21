namespace FaceDeviceDesktopClient
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDashboard = new System.Windows.Forms.TabPage();
            this.tabDevices = new System.Windows.Forms.TabPage();
            this.tabDepartments = new System.Windows.Forms.TabPage();
            this.tabPersonnel = new System.Windows.Forms.TabPage();
            this.tabAttendance = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();

            // Dashboard controls
            this.grpSystemInfo = new System.Windows.Forms.GroupBox();
            this.lblRetention = new System.Windows.Forms.Label();
            this.numRetentionMonths = new System.Windows.Forms.NumericUpDown();
            this.lblRetentionHint = new System.Windows.Forms.Label();
            this.btnSaveRetention = new System.Windows.Forms.Button();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.cmbServerUrl = new System.Windows.Forms.ComboBox();
            this.lblTotalDevices = new System.Windows.Forms.Label();
            this.lblTotalPersonnel = new System.Windows.Forms.Label();
            this.lblTotalDepartments = new System.Windows.Forms.Label();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();

            // Device controls
            this.grpDeviceSearch = new System.Windows.Forms.GroupBox();
            this.btnAutoSearch = new System.Windows.Forms.Button();
            this.btnRefreshDevices = new System.Windows.Forms.Button();
            this.dgvDiscoveredDevices = new System.Windows.Forms.DataGridView();
            this.btnConnectDevice = new System.Windows.Forms.Button();
            this.grpDeviceManagement = new System.Windows.Forms.GroupBox();
            this.btnRemoteControl = new System.Windows.Forms.Button();
            this.btnRemoveDevice = new System.Windows.Forms.Button();
            this.btnPullPeople = new System.Windows.Forms.Button();
            this.dgvDevices = new System.Windows.Forms.DataGridView();

            // Department controls
            this.btnAddDepartment = new System.Windows.Forms.Button();
            this.btnRefreshDepartments = new System.Windows.Forms.Button();
            this.dgvDepartments = new System.Windows.Forms.DataGridView();

            // Personnel controls
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.btnEditPerson = new System.Windows.Forms.Button();
            this.btnDeletePerson = new System.Windows.Forms.Button();
            this.btnRefreshPersonnel = new System.Windows.Forms.Button();
            this.btnDistributePeople = new System.Windows.Forms.Button();
            this.btnReloadFromFiles  = new System.Windows.Forms.Button();
            this.btnSaveToFiles = new System.Windows.Forms.Button();
            this.lblFilterDong = new System.Windows.Forms.Label();
            this.txtFilterDong = new System.Windows.Forms.TextBox();
            this.lblFilterHo   = new System.Windows.Forms.Label();
            this.txtFilterHo   = new System.Windows.Forms.TextBox();
            this.btnSelectByFilter = new System.Windows.Forms.Button();
            this.dgvPersonnel = new System.Windows.Forms.DataGridView();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();

            // Attendance controls
            this.grpAttendanceSearch = new System.Windows.Forms.GroupBox();
            this.txtAttDong    = new System.Windows.Forms.TextBox();
            this.lblAttHo      = new System.Windows.Forms.Label();
            this.txtAttHo      = new System.Windows.Forms.TextBox();
            this.lblAttMember  = new System.Windows.Forms.Label();
            this.txtAttMember  = new System.Windows.Forms.TextBox();
            this.txtAttendanceUserName = new System.Windows.Forms.TextBox();
            this.lblAttDevice  = new System.Windows.Forms.Label();
            this.cmbAttDevice  = new System.Windows.Forms.ComboBox();
            this.dtpAttendanceStart = new System.Windows.Forms.DateTimePicker();
            this.dtpAttendanceEnd = new System.Windows.Forms.DateTimePicker();
            this.btnSearchAttendance = new System.Windows.Forms.Button();
            this.btnRealTimeView = new System.Windows.Forms.Button();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.lblAttendanceCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();

            this.tabControl.SuspendLayout();
            this.tabDashboard.SuspendLayout();
            this.tabDevices.SuspendLayout();
            this.tabDepartments.SuspendLayout();
            this.tabPersonnel.SuspendLayout();
            this.tabAttendance.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.grpSystemInfo.SuspendLayout();
            this.grpDeviceSearch.SuspendLayout();
            this.grpAttendanceSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscoveredDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonnel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).BeginInit();
            this.SuspendLayout();

            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDashboard);
            this.tabControl.Controls.Add(this.tabDevices);
            this.tabControl.Controls.Add(this.tabPersonnel);
            this.tabControl.Controls.Add(this.tabAttendance);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1200, 600);
            this.tabControl.TabIndex = 0;

            // 
            // tabDashboard
            // 
            this.tabDashboard.Controls.Add(this.grpSystemInfo);
            this.tabDashboard.Location = new System.Drawing.Point(4, 24);
            this.tabDashboard.Name = "tabDashboard";
            this.tabDashboard.Padding = new System.Windows.Forms.Padding(10);
            this.tabDashboard.Size = new System.Drawing.Size(1192, 572);
            this.tabDashboard.TabIndex = 0;
            this.tabDashboard.Text = "시스템요약";
            this.tabDashboard.UseVisualStyleBackColor = true;

            // 
            // grpSystemInfo
            // 
            this.grpSystemInfo.Controls.Add(this.label1);
            this.grpSystemInfo.Controls.Add(this.lblTotalDevices);
            this.grpSystemInfo.Controls.Add(this.label2);
            this.grpSystemInfo.Controls.Add(this.lblTotalPersonnel);
            this.grpSystemInfo.Controls.Add(this.label4);
            this.grpSystemInfo.Controls.Add(this.lblTotalRecords);
            this.grpSystemInfo.Controls.Add(this.lblRetention);
            this.grpSystemInfo.Controls.Add(this.numRetentionMonths);
            this.grpSystemInfo.Controls.Add(this.lblRetentionHint);
            this.grpSystemInfo.Controls.Add(this.btnSaveRetention);
            this.grpSystemInfo.Controls.Add(this.lblServerUrl);
            this.grpSystemInfo.Controls.Add(this.cmbServerUrl);
            this.grpSystemInfo.Location = new System.Drawing.Point(20, 20);
            this.grpSystemInfo.Name = "grpSystemInfo";
            this.grpSystemInfo.Size = new System.Drawing.Size(620, 310);
            this.grpSystemInfo.TabIndex = 0;
            this.grpSystemInfo.TabStop = false;
            this.grpSystemInfo.Text = "";

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "단말기 수:";

            this.lblTotalDevices.AutoSize = true;
            this.lblTotalDevices.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalDevices.Location = new System.Drawing.Point(150, 38);
            this.lblTotalDevices.Name = "lblTotalDevices";
            this.lblTotalDevices.Size = new System.Drawing.Size(19, 21);
            this.lblTotalDevices.TabIndex = 1;
            this.lblTotalDevices.Text = "0";

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "사용자 수:";

            this.lblTotalPersonnel.AutoSize = true;
            this.lblTotalPersonnel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalPersonnel.Location = new System.Drawing.Point(150, 78);
            this.lblTotalPersonnel.Name = "lblTotalPersonnel";
            this.lblTotalPersonnel.Size = new System.Drawing.Size(19, 21);
            this.lblTotalPersonnel.TabIndex = 3;
            this.lblTotalPersonnel.Text = "0";

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "부서 수:";

            this.lblTotalDepartments.AutoSize = true;
            this.lblTotalDepartments.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalDepartments.Location = new System.Drawing.Point(150, 118);
            this.lblTotalDepartments.Name = "lblTotalDepartments";
            this.lblTotalDepartments.Size = new System.Drawing.Size(19, 21);
            this.lblTotalDepartments.TabIndex = 5;
            this.lblTotalDepartments.Text = "0";

            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "출입기록 수:";

            this.lblTotalRecords.AutoSize = true;
            this.lblTotalRecords.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalRecords.Location = new System.Drawing.Point(150, 118);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(19, 21);
            this.lblTotalRecords.TabIndex = 7;
            this.lblTotalRecords.Text = "0";

            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(20, 162);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Text = "기록 보관기간:";

            this.numRetentionMonths.Location = new System.Drawing.Point(150, 160);
            this.numRetentionMonths.Name = "numRetentionMonths";
            this.numRetentionMonths.Minimum = 0;
            this.numRetentionMonths.Maximum = 120;
            this.numRetentionMonths.Value = 12;
            this.numRetentionMonths.Size = new System.Drawing.Size(70, 23);

            this.lblRetentionHint.AutoSize = true;
            this.lblRetentionHint.Location = new System.Drawing.Point(228, 163);
            this.lblRetentionHint.Name = "lblRetentionHint";
            this.lblRetentionHint.Text = "개월 (0 = 삭제 안 함)";

            this.btnSaveRetention.Location = new System.Drawing.Point(150, 250);
            this.btnSaveRetention.Name = "btnSaveRetention";
            this.btnSaveRetention.Size = new System.Drawing.Size(120, 32);
            this.btnSaveRetention.Text = "저장";
            this.btnSaveRetention.UseVisualStyleBackColor = true;
            this.btnSaveRetention.Click += new System.EventHandler(this.btnSaveRetention_Click);

            this.lblServerUrl.AutoSize = true;
            this.lblServerUrl.Location = new System.Drawing.Point(20, 210);
            this.lblServerUrl.Name = "lblServerUrl";
            this.lblServerUrl.Text = "서버 URL:";

            this.cmbServerUrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbServerUrl.Location = new System.Drawing.Point(150, 206);
            this.cmbServerUrl.Name = "cmbServerUrl";
            this.cmbServerUrl.Size = new System.Drawing.Size(300, 23);

            // 
            // tabDevices
            // 
            this.tabDevices.Controls.Add(this.dgvDevices);
            this.tabDevices.Controls.Add(this.grpDeviceManagement);
            this.tabDevices.Controls.Add(this.grpDeviceSearch);
            this.tabDevices.Location = new System.Drawing.Point(4, 24);
            this.tabDevices.Name = "tabDevices";
            this.tabDevices.Padding = new System.Windows.Forms.Padding(10);
            this.tabDevices.Size = new System.Drawing.Size(1192, 572);
            this.tabDevices.TabIndex = 1;
            this.tabDevices.Text = "단말기";
            this.tabDevices.UseVisualStyleBackColor = true;

            // 
            // grpDeviceSearch
            // 
            this.grpDeviceSearch.Controls.Add(this.btnAutoSearch);
            this.grpDeviceSearch.Controls.Add(this.dgvDiscoveredDevices);
            this.grpDeviceSearch.Controls.Add(this.btnConnectDevice);
            this.grpDeviceSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDeviceSearch.Location = new System.Drawing.Point(10, 10);
            this.grpDeviceSearch.Name = "grpDeviceSearch";
            this.grpDeviceSearch.Size = new System.Drawing.Size(1172, 250);
            this.grpDeviceSearch.TabIndex = 0;
            this.grpDeviceSearch.TabStop = false;
            this.grpDeviceSearch.Text = "추가";

            this.btnAutoSearch.Location = new System.Drawing.Point(10, 25);
            this.btnAutoSearch.Name = "btnAutoSearch";
            this.btnAutoSearch.Size = new System.Drawing.Size(120, 30);
            this.btnAutoSearch.TabIndex = 0;
            this.btnAutoSearch.Text = "검색";
            this.btnAutoSearch.UseVisualStyleBackColor = true;
            this.btnAutoSearch.Click += new System.EventHandler(this.btnAutoSearch_Click);

            this.btnRefreshDevices.Location = new System.Drawing.Point(140, 25);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshDevices.TabIndex = 1;
            this.btnRefreshDevices.Text = "새로고침";
            this.btnRefreshDevices.UseVisualStyleBackColor = true;
            this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);

            this.btnConnectDevice.Location = new System.Drawing.Point(140, 25);
            this.btnConnectDevice.Name = "btnConnectDevice";
            this.btnConnectDevice.Size = new System.Drawing.Size(120, 30);
            this.btnConnectDevice.TabIndex = 1;
            this.btnConnectDevice.Text = "등록";
            this.btnConnectDevice.UseVisualStyleBackColor = true;
            this.btnConnectDevice.Click += new System.EventHandler(this.btnConnectDevice_Click);

            this.dgvDiscoveredDevices.AllowUserToAddRows = false;
            this.dgvDiscoveredDevices.AllowUserToDeleteRows = false;
            this.dgvDiscoveredDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscoveredDevices.AutoGenerateColumns = true;
            this.dgvDiscoveredDevices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDiscoveredDevices.Location = new System.Drawing.Point(10, 65);
            this.dgvDiscoveredDevices.MultiSelect = false;
            this.dgvDiscoveredDevices.Name = "dgvDiscoveredDevices";
            this.dgvDiscoveredDevices.ReadOnly = true;
            this.dgvDiscoveredDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDiscoveredDevices.Size = new System.Drawing.Size(1152, 175);
            this.dgvDiscoveredDevices.TabIndex = 3;

            this.dgvDevices.AllowUserToAddRows = false;
            this.dgvDevices.AllowUserToDeleteRows = false;
            this.dgvDevices.AutoGenerateColumns = true;
            this.dgvDevices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevices.Location = new System.Drawing.Point(10, 335);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.ReadOnly = true;
            this.dgvDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevices.Size = new System.Drawing.Size(1172, 227);
            this.dgvDevices.TabIndex = 1;

            // 
            // grpDeviceManagement
            // 
            this.grpDeviceManagement.Controls.Add(this.btnRemoteControl);
            this.grpDeviceManagement.Controls.Add(this.btnRemoveDevice);
            this.grpDeviceManagement.Controls.Add(this.btnPullPeople);
            this.grpDeviceManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDeviceManagement.Location = new System.Drawing.Point(10, 260);
            this.grpDeviceManagement.Name = "grpDeviceManagement";
            this.grpDeviceManagement.Size = new System.Drawing.Size(1172, 75);
            this.grpDeviceManagement.TabIndex = 2;
            this.grpDeviceManagement.TabStop = false;
            this.grpDeviceManagement.Text = "관리";

            this.btnRemoteControl.Location = new System.Drawing.Point(10, 25);
            this.btnRemoteControl.Name = "btnRemoteControl";
            this.btnRemoteControl.Size = new System.Drawing.Size(150, 35);
            this.btnRemoteControl.TabIndex = 0;
            this.btnRemoteControl.Text = "설정";
            this.btnRemoteControl.UseVisualStyleBackColor = true;
            this.btnRemoteControl.Click += new System.EventHandler(this.btnRemoteControl_Click);

            this.btnRemoveDevice.Location = new System.Drawing.Point(170, 25);
            this.btnRemoveDevice.Name = "btnRemoveDevice";
            this.btnRemoveDevice.Size = new System.Drawing.Size(150, 35);
            this.btnRemoveDevice.TabIndex = 1;
            this.btnRemoveDevice.Text = "제거";
            this.btnRemoveDevice.ForeColor = System.Drawing.Color.DarkRed;
            this.btnRemoveDevice.UseVisualStyleBackColor = true;
            this.btnRemoveDevice.Click += new System.EventHandler(this.btnRemoveDevice_Click);

            this.btnPullPeople.Location = new System.Drawing.Point(330, 25);
            this.btnPullPeople.Name = "btnPullPeople";
            this.btnPullPeople.Size = new System.Drawing.Size(170, 35);
            this.btnPullPeople.TabIndex = 2;
            this.btnPullPeople.Text = "사용자 가져오기";
            this.btnPullPeople.UseVisualStyleBackColor = true;
            this.btnPullPeople.Click += new System.EventHandler(this.btnPullPeople_Click);

            // 
            // tabDepartments
            // 
            this.tabDepartments.Controls.Add(this.btnAddDepartment);
            this.tabDepartments.Controls.Add(this.btnRefreshDepartments);
            this.tabDepartments.Controls.Add(this.dgvDepartments);
            this.tabDepartments.Location = new System.Drawing.Point(4, 24);
            this.tabDepartments.Name = "tabDepartments";
            this.tabDepartments.Padding = new System.Windows.Forms.Padding(10);
            this.tabDepartments.Size = new System.Drawing.Size(1192, 572);
            this.tabDepartments.TabIndex = 2;
            this.tabDepartments.Text = "부서";
            this.tabDepartments.UseVisualStyleBackColor = true;

            this.btnAddDepartment.Location = new System.Drawing.Point(10, 10);
            this.btnAddDepartment.Name = "btnAddDepartment";
            this.btnAddDepartment.Size = new System.Drawing.Size(120, 30);
            this.btnAddDepartment.TabIndex = 0;
            this.btnAddDepartment.Text = "Add Department";
            this.btnAddDepartment.UseVisualStyleBackColor = true;
            this.btnAddDepartment.Click += new System.EventHandler(this.btnAddDepartment_Click);

            this.btnRefreshDepartments.Location = new System.Drawing.Point(140, 10);
            this.btnRefreshDepartments.Name = "btnRefreshDepartments";
            this.btnRefreshDepartments.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshDepartments.TabIndex = 1;
            this.btnRefreshDepartments.Text = "Refresh";
            this.btnRefreshDepartments.UseVisualStyleBackColor = true;
            this.btnRefreshDepartments.Click += new System.EventHandler(this.btnRefreshDepartments_Click);

            this.dgvDepartments.AllowUserToAddRows = false;
            this.dgvDepartments.AllowUserToDeleteRows = false;
            this.dgvDepartments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDepartments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDepartments.Location = new System.Drawing.Point(10, 50);
            this.dgvDepartments.Name = "dgvDepartments";
            this.dgvDepartments.ReadOnly = true;
            this.dgvDepartments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDepartments.Size = new System.Drawing.Size(1172, 512);
            this.dgvDepartments.TabIndex = 2;

            // 
            // tabPersonnel
            // 
            this.tabPersonnel.Controls.Add(this.btnAddPerson);
            this.tabPersonnel.Controls.Add(this.btnEditPerson);
            this.tabPersonnel.Controls.Add(this.btnDeletePerson);
            this.tabPersonnel.Controls.Add(this.btnRefreshPersonnel);
            this.tabPersonnel.Controls.Add(this.btnDistributePeople);
            this.tabPersonnel.Controls.Add(this.btnReloadFromFiles);
            this.tabPersonnel.Controls.Add(this.btnSaveToFiles);
            this.tabPersonnel.Controls.Add(this.lblFilterDong);
            this.tabPersonnel.Controls.Add(this.txtFilterDong);
            this.tabPersonnel.Controls.Add(this.lblFilterHo);
            this.tabPersonnel.Controls.Add(this.txtFilterHo);
            this.tabPersonnel.Controls.Add(this.btnSelectByFilter);
            this.tabPersonnel.Controls.Add(this.dgvPersonnel);
            this.tabPersonnel.Location = new System.Drawing.Point(4, 24);
            this.tabPersonnel.Name = "tabPersonnel";
            this.tabPersonnel.Padding = new System.Windows.Forms.Padding(10);
            this.tabPersonnel.Size = new System.Drawing.Size(1192, 572);
            this.tabPersonnel.TabIndex = 3;
            this.tabPersonnel.Text = "사용자";
            this.tabPersonnel.UseVisualStyleBackColor = true;

            this.btnAddPerson.Location = new System.Drawing.Point(10, 10);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(100, 30);
            this.btnAddPerson.TabIndex = 0;
            this.btnAddPerson.Text = "추가";
            this.btnAddPerson.UseVisualStyleBackColor = true;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);

            this.btnEditPerson.Location = new System.Drawing.Point(120, 10);
            this.btnEditPerson.Name = "btnEditPerson";
            this.btnEditPerson.Size = new System.Drawing.Size(100, 30);
            this.btnEditPerson.TabIndex = 1;
            this.btnEditPerson.Text = "수정";
            this.btnEditPerson.UseVisualStyleBackColor = true;
            this.btnEditPerson.Click += new System.EventHandler(this.btnEditPerson_Click);

            this.btnDeletePerson.Location = new System.Drawing.Point(230, 10);
            this.btnDeletePerson.Name = "btnDeletePerson";
            this.btnDeletePerson.Size = new System.Drawing.Size(100, 30);
            this.btnDeletePerson.TabIndex = 2;
            this.btnDeletePerson.Text = "제거";
            this.btnDeletePerson.ForeColor = System.Drawing.Color.DarkRed;
            this.btnDeletePerson.UseVisualStyleBackColor = true;
            this.btnDeletePerson.Click += new System.EventHandler(this.btnDeletePerson_Click);

            this.btnRefreshPersonnel.Location = new System.Drawing.Point(340, 10);
            this.btnRefreshPersonnel.Name = "btnRefreshPersonnel";
            this.btnRefreshPersonnel.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshPersonnel.TabIndex = 3;
            this.btnRefreshPersonnel.Text = "새로고침";
            this.btnRefreshPersonnel.UseVisualStyleBackColor = true;
            this.btnRefreshPersonnel.Click += new System.EventHandler(this.btnRefreshPersonnel_Click);

            this.btnDistributePeople.Location = new System.Drawing.Point(460, 10);
            this.btnDistributePeople.Name = "btnDistributePeople";
            this.btnDistributePeople.Size = new System.Drawing.Size(150, 30);
            this.btnDistributePeople.TabIndex = 4;
            this.btnDistributePeople.Text = "단말기로 배포";
            this.btnDistributePeople.UseVisualStyleBackColor = true;
            this.btnDistributePeople.Click += new System.EventHandler(this.btnDistributePeople_Click);

            this.btnReloadFromFiles.Location = new System.Drawing.Point(10, 46);
            this.btnReloadFromFiles.Name = "btnReloadFromFiles";
            this.btnReloadFromFiles.Size = new System.Drawing.Size(160, 30);
            this.btnReloadFromFiles.TabIndex = 5;
            this.btnReloadFromFiles.Text = "파일에서 불러오기";
            this.btnReloadFromFiles.UseVisualStyleBackColor = true;
            this.btnReloadFromFiles.Click += new System.EventHandler(this.btnReloadFromFiles_Click);

            this.btnSaveToFiles.Location = new System.Drawing.Point(180, 46);
            this.btnSaveToFiles.Name = "btnSaveToFiles";
            this.btnSaveToFiles.Size = new System.Drawing.Size(130, 30);
            this.btnSaveToFiles.TabIndex = 6;
            this.btnSaveToFiles.Text = "파일로 저장";
            this.btnSaveToFiles.UseVisualStyleBackColor = true;
            this.btnSaveToFiles.Click += new System.EventHandler(this.btnSaveToFiles_Click);

            this.lblFilterDong.AutoSize = true;
            this.lblFilterDong.Location = new System.Drawing.Point(330, 53);
            this.lblFilterDong.Name = "lblFilterDong";
            this.lblFilterDong.Text = "동";

            this.txtFilterDong.Location = new System.Drawing.Point(355, 48);
            this.txtFilterDong.Name = "txtFilterDong";
            this.txtFilterDong.Size = new System.Drawing.Size(60, 23);
            this.txtFilterDong.TabIndex = 6;

            this.lblFilterHo.AutoSize = true;
            this.lblFilterHo.Location = new System.Drawing.Point(425, 53);
            this.lblFilterHo.Name = "lblFilterHo";
            this.lblFilterHo.Text = "호";
            this.lblFilterHo.Enabled = false;

            this.txtFilterHo.Location = new System.Drawing.Point(450, 48);
            this.txtFilterHo.Name = "txtFilterHo";
            this.txtFilterHo.Size = new System.Drawing.Size(60, 23);
            this.txtFilterHo.TabIndex = 7;
            this.txtFilterHo.Enabled = false;

            this.btnSelectByFilter.Location = new System.Drawing.Point(520, 46);
            this.btnSelectByFilter.Name = "btnSelectByFilter";
            this.btnSelectByFilter.Size = new System.Drawing.Size(75, 30);
            this.btnSelectByFilter.TabIndex = 8;
            this.btnSelectByFilter.Text = "선택";
            this.btnSelectByFilter.UseVisualStyleBackColor = true;
            this.btnSelectByFilter.Click += new System.EventHandler(this.btnSelectByFilter_Click);

            this.dgvPersonnel.AllowUserToDeleteRows = false;
            this.dgvPersonnel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPersonnel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPersonnel.Location = new System.Drawing.Point(10, 86);
            this.dgvPersonnel.Name = "dgvPersonnel";
            this.dgvPersonnel.ReadOnly = true;
            this.dgvPersonnel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersonnel.Size = new System.Drawing.Size(1172, 476);
            this.dgvPersonnel.TabIndex = 2;

            // 
            // tabAttendance
            // 
            this.tabAttendance.Controls.Add(this.grpAttendanceSearch);
            this.tabAttendance.Controls.Add(this.dgvAttendance);
            this.tabAttendance.Controls.Add(this.lblAttendanceCount);
            this.tabAttendance.Location = new System.Drawing.Point(4, 24);
            this.tabAttendance.Name = "tabAttendance";
            this.tabAttendance.Padding = new System.Windows.Forms.Padding(10);
            this.tabAttendance.Size = new System.Drawing.Size(1192, 572);
            this.tabAttendance.TabIndex = 4;
            this.tabAttendance.Text = "출입기록";
            this.tabAttendance.UseVisualStyleBackColor = true;

            // 
            // grpAttendanceSearch
            // 
            this.grpAttendanceSearch.Controls.Add(this.label5);
            this.grpAttendanceSearch.Controls.Add(this.txtAttDong);
            this.grpAttendanceSearch.Controls.Add(this.lblAttHo);
            this.grpAttendanceSearch.Controls.Add(this.txtAttHo);
            this.grpAttendanceSearch.Controls.Add(this.lblAttMember);
            this.grpAttendanceSearch.Controls.Add(this.txtAttMember);
            this.grpAttendanceSearch.Controls.Add(this.label6);
            this.grpAttendanceSearch.Controls.Add(this.txtAttendanceUserName);
            this.grpAttendanceSearch.Controls.Add(this.lblAttDevice);
            this.grpAttendanceSearch.Controls.Add(this.cmbAttDevice);
            this.grpAttendanceSearch.Controls.Add(this.label8);
            this.grpAttendanceSearch.Controls.Add(this.dtpAttendanceStart);
            this.grpAttendanceSearch.Controls.Add(this.label9);
            this.grpAttendanceSearch.Controls.Add(this.dtpAttendanceEnd);
            this.grpAttendanceSearch.Controls.Add(this.btnSearchAttendance);
            this.grpAttendanceSearch.Controls.Add(this.btnRealTimeView);
            this.grpAttendanceSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpAttendanceSearch.Location = new System.Drawing.Point(10, 10);
            this.grpAttendanceSearch.Name = "grpAttendanceSearch";
            this.grpAttendanceSearch.Size = new System.Drawing.Size(1172, 120);
            this.grpAttendanceSearch.TabIndex = 0;
            this.grpAttendanceSearch.TabStop = false;
            this.grpAttendanceSearch.Text = "출입기록";

            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 26);
            this.label5.Name = "label5";
            this.label5.TabIndex = 0;
            this.label5.Text = "동:";

            this.txtAttDong.Location = new System.Drawing.Point(38, 22);
            this.txtAttDong.Name = "txtAttDong";
            this.txtAttDong.Size = new System.Drawing.Size(60, 23);
            this.txtAttDong.TabIndex = 1;
            this.txtAttDong.TextChanged += new System.EventHandler(this.txtAttDong_TextChanged);

            this.lblAttHo.AutoSize = true;
            this.lblAttHo.Location = new System.Drawing.Point(108, 26);
            this.lblAttHo.Name = "lblAttHo";
            this.lblAttHo.Text = "호:";

            this.txtAttHo.Location = new System.Drawing.Point(130, 22);
            this.txtAttHo.Name = "txtAttHo";
            this.txtAttHo.Size = new System.Drawing.Size(60, 23);
            this.txtAttHo.TabIndex = 2;
            this.txtAttHo.Enabled = false;
            this.txtAttHo.TextChanged += new System.EventHandler(this.txtAttHo_TextChanged);

            this.lblAttMember.AutoSize = true;
            this.lblAttMember.Location = new System.Drawing.Point(200, 26);
            this.lblAttMember.Name = "lblAttMember";
            this.lblAttMember.Text = "멤버#:";

            this.txtAttMember.Location = new System.Drawing.Point(250, 22);
            this.txtAttMember.Name = "txtAttMember";
            this.txtAttMember.Size = new System.Drawing.Size(60, 23);
            this.txtAttMember.TabIndex = 3;
            this.txtAttMember.Enabled = false;

            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(324, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "사용자명:";

            this.txtAttendanceUserName.Location = new System.Drawing.Point(386, 22);
            this.txtAttendanceUserName.Name = "txtAttendanceUserName";
            this.txtAttendanceUserName.Size = new System.Drawing.Size(140, 23);
            this.txtAttendanceUserName.TabIndex = 5;

            this.lblAttDevice.AutoSize = true;
            this.lblAttDevice.Location = new System.Drawing.Point(540, 26);
            this.lblAttDevice.Name = "lblAttDevice";
            this.lblAttDevice.TabIndex = 6;
            this.lblAttDevice.Text = "단말기:";

            this.cmbAttDevice.Location = new System.Drawing.Point(590, 22);
            this.cmbAttDevice.Name = "cmbAttDevice";
            this.cmbAttDevice.Size = new System.Drawing.Size(200, 23);
            this.cmbAttDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttDevice.TabIndex = 7;

            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 15);
            this.label8.TabIndex = 6;
            this.label8.Text = "시작일시:";

            this.dtpAttendanceStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttendanceStart.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpAttendanceStart.Location = new System.Drawing.Point(72, 57);
            this.dtpAttendanceStart.Name = "dtpAttendanceStart";
            this.dtpAttendanceStart.ShowCheckBox = true;
            this.dtpAttendanceStart.ShowUpDown = true;
            this.dtpAttendanceStart.Checked = false;
            this.dtpAttendanceStart.Size = new System.Drawing.Size(200, 23);
            this.dtpAttendanceStart.TabIndex = 7;

            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(288, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "종료일시:";

            this.dtpAttendanceEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttendanceEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpAttendanceEnd.Location = new System.Drawing.Point(352, 57);
            this.dtpAttendanceEnd.Name = "dtpAttendanceEnd";
            this.dtpAttendanceEnd.ShowCheckBox = true;
            this.dtpAttendanceEnd.ShowUpDown = true;
            this.dtpAttendanceEnd.Checked = false;
            this.dtpAttendanceEnd.Size = new System.Drawing.Size(200, 23);
            this.dtpAttendanceEnd.TabIndex = 9;

            this.btnSearchAttendance.Location = new System.Drawing.Point(570, 54);
            this.btnSearchAttendance.Name = "btnSearchAttendance";
            this.btnSearchAttendance.Size = new System.Drawing.Size(100, 30);
            this.btnSearchAttendance.TabIndex = 10;
            this.btnSearchAttendance.Text = "조회";
            this.btnSearchAttendance.UseVisualStyleBackColor = true;
            this.btnSearchAttendance.Click += new System.EventHandler(this.btnSearchAttendance_Click);

            this.btnRealTimeView.Location = new System.Drawing.Point(680, 54);
            this.btnRealTimeView.Name = "btnRealTimeView";
            this.btnRealTimeView.Size = new System.Drawing.Size(120, 30);
            this.btnRealTimeView.TabIndex = 11;
            this.btnRealTimeView.Text = "실시간보기";
            this.btnRealTimeView.UseVisualStyleBackColor = true;
            this.btnRealTimeView.Click += new System.EventHandler(this.btnRealTimeView_Click);

            this.lblAttendanceCount.AutoSize = true;
            this.lblAttendanceCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblAttendanceCount.Location = new System.Drawing.Point(10, 547);
            this.lblAttendanceCount.Name = "lblAttendanceCount";
            this.lblAttendanceCount.Padding = new System.Windows.Forms.Padding(5);
            this.lblAttendanceCount.Size = new System.Drawing.Size(54, 25);
            this.lblAttendanceCount.TabIndex = 2;
            this.lblAttendanceCount.Text = "Total: 0";

            this.dgvAttendance.AllowUserToAddRows = false;
            this.dgvAttendance.AllowUserToDeleteRows = false;
            this.dgvAttendance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAttendance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAttendance.Location = new System.Drawing.Point(10, 136);
            this.dgvAttendance.Name = "dgvAttendance";
            this.dgvAttendance.ReadOnly = true;
            this.dgvAttendance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttendance.Size = new System.Drawing.Size(1172, 401);
            this.dgvAttendance.TabIndex = 1;

            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 600);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";

            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 17);
            this.lblStatus.Text = "Ready";

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 622);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Face Device Desktop Client";

            this.tabControl.ResumeLayout(false);
            this.tabDashboard.ResumeLayout(false);
            this.tabDevices.ResumeLayout(false);
            this.tabDepartments.ResumeLayout(false);
            this.tabPersonnel.ResumeLayout(false);
            this.tabAttendance.ResumeLayout(false);
            this.tabAttendance.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.grpSystemInfo.ResumeLayout(false);
            this.grpSystemInfo.PerformLayout();
            this.grpDeviceSearch.ResumeLayout(false);
            this.grpAttendanceSearch.ResumeLayout(false);
            this.grpAttendanceSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscoveredDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDepartments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonnel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDashboard;
        private System.Windows.Forms.TabPage tabDevices;
        private System.Windows.Forms.TabPage tabDepartments;
        private System.Windows.Forms.TabPage tabPersonnel;
        private System.Windows.Forms.TabPage tabAttendance;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.GroupBox grpSystemInfo;
        private System.Windows.Forms.Label lblRetention;
        private System.Windows.Forms.NumericUpDown numRetentionMonths;
        private System.Windows.Forms.Label lblRetentionHint;
        private System.Windows.Forms.Button btnSaveRetention;
        private System.Windows.Forms.Label lblServerUrl;
        private System.Windows.Forms.ComboBox cmbServerUrl;
        private System.Windows.Forms.Label lblTotalDevices;
        private System.Windows.Forms.Label lblTotalPersonnel;
        private System.Windows.Forms.Label lblTotalDepartments;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpDeviceSearch;
        private System.Windows.Forms.Button btnAutoSearch;
        private System.Windows.Forms.Button btnRefreshDevices;
        private System.Windows.Forms.Button btnConnectDevice;
        private System.Windows.Forms.DataGridView dgvDiscoveredDevices;
        private System.Windows.Forms.GroupBox grpDeviceManagement;
        private System.Windows.Forms.Button btnRemoteControl;
        private System.Windows.Forms.Button btnRemoveDevice;
        private System.Windows.Forms.Button btnPullPeople;
        private System.Windows.Forms.DataGridView dgvDevices;
        private System.Windows.Forms.Button btnAddDepartment;
        private System.Windows.Forms.Button btnRefreshDepartments;
        private System.Windows.Forms.DataGridView dgvDepartments;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.Button btnEditPerson;
        private System.Windows.Forms.Button btnDeletePerson;
        private System.Windows.Forms.Button btnRefreshPersonnel;
        private System.Windows.Forms.Button btnDistributePeople;
        private System.Windows.Forms.Button btnReloadFromFiles;
        private System.Windows.Forms.Button btnSaveToFiles;
        private System.Windows.Forms.Label lblFilterDong;
        private System.Windows.Forms.TextBox txtFilterDong;
        private System.Windows.Forms.Label lblFilterHo;
        private System.Windows.Forms.TextBox txtFilterHo;
        private System.Windows.Forms.Button btnSelectByFilter;
        private System.Windows.Forms.DataGridView dgvPersonnel;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.GroupBox grpAttendanceSearch;
        private System.Windows.Forms.Label lblAttHo;
        private System.Windows.Forms.Label lblAttMember;
        private System.Windows.Forms.Label lblAttDevice;
        private System.Windows.Forms.TextBox txtAttDong;
        private System.Windows.Forms.TextBox txtAttHo;
        private System.Windows.Forms.TextBox txtAttMember;
        private System.Windows.Forms.TextBox txtAttendanceUserName;
        private System.Windows.Forms.ComboBox cmbAttDevice;
        private System.Windows.Forms.DateTimePicker dtpAttendanceStart;
        private System.Windows.Forms.DateTimePicker dtpAttendanceEnd;
        private System.Windows.Forms.Button btnSearchAttendance;
        private System.Windows.Forms.Button btnRealTimeView;
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.Label lblAttendanceCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}
