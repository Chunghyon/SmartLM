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
            this.dgvDevices = new System.Windows.Forms.DataGridView();

            // Department controls
            this.btnAddDepartment = new System.Windows.Forms.Button();
            this.btnRefreshDepartments = new System.Windows.Forms.Button();
            this.dgvDepartments = new System.Windows.Forms.DataGridView();

            // Personnel controls
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.btnRefreshPersonnel = new System.Windows.Forms.Button();
            this.dgvPersonnel = new System.Windows.Forms.DataGridView();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();

            // Attendance controls
            this.grpAttendanceSearch = new System.Windows.Forms.GroupBox();
            this.txtAttendanceUserID = new System.Windows.Forms.TextBox();
            this.txtAttendanceUserName = new System.Windows.Forms.TextBox();
            this.cmbAttendanceDepartment = new System.Windows.Forms.ComboBox();
            this.dtpAttendanceStart = new System.Windows.Forms.DateTimePicker();
            this.dtpAttendanceEnd = new System.Windows.Forms.DateTimePicker();
            this.btnSearchAttendance = new System.Windows.Forms.Button();
            this.btnGetStatistics = new System.Windows.Forms.Button();
            this.btnRefreshAttendance = new System.Windows.Forms.Button();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.lblAttendanceCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
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
            this.tabControl.Controls.Add(this.tabDepartments);
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
            this.tabDashboard.Text = "Dashboard";
            this.tabDashboard.UseVisualStyleBackColor = true;

            // 
            // grpSystemInfo
            // 
            this.grpSystemInfo.Controls.Add(this.label1);
            this.grpSystemInfo.Controls.Add(this.lblTotalDevices);
            this.grpSystemInfo.Controls.Add(this.label2);
            this.grpSystemInfo.Controls.Add(this.lblTotalPersonnel);
            this.grpSystemInfo.Controls.Add(this.label3);
            this.grpSystemInfo.Controls.Add(this.lblTotalDepartments);
            this.grpSystemInfo.Controls.Add(this.label4);
            this.grpSystemInfo.Controls.Add(this.lblTotalRecords);
            this.grpSystemInfo.Location = new System.Drawing.Point(20, 20);
            this.grpSystemInfo.Name = "grpSystemInfo";
            this.grpSystemInfo.Size = new System.Drawing.Size(500, 200);
            this.grpSystemInfo.TabIndex = 0;
            this.grpSystemInfo.TabStop = false;
            this.grpSystemInfo.Text = "System Information";

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Devices:";

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
            this.label2.Text = "Total Personnel:";

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
            this.label3.Text = "Total Departments:";

            this.lblTotalDepartments.AutoSize = true;
            this.lblTotalDepartments.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalDepartments.Location = new System.Drawing.Point(150, 118);
            this.lblTotalDepartments.Name = "lblTotalDepartments";
            this.lblTotalDepartments.Size = new System.Drawing.Size(19, 21);
            this.lblTotalDepartments.TabIndex = 5;
            this.lblTotalDepartments.Text = "0";

            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Total Records:";

            this.lblTotalRecords.AutoSize = true;
            this.lblTotalRecords.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalRecords.Location = new System.Drawing.Point(150, 158);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(19, 21);
            this.lblTotalRecords.TabIndex = 7;
            this.lblTotalRecords.Text = "0";

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
            this.tabDevices.Text = "Device Management";
            this.tabDevices.UseVisualStyleBackColor = true;

            // 
            // grpDeviceSearch
            // 
            this.grpDeviceSearch.Controls.Add(this.btnAutoSearch);
            this.grpDeviceSearch.Controls.Add(this.btnRefreshDevices);
            this.grpDeviceSearch.Controls.Add(this.dgvDiscoveredDevices);
            this.grpDeviceSearch.Controls.Add(this.btnConnectDevice);
            this.grpDeviceSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDeviceSearch.Location = new System.Drawing.Point(10, 10);
            this.grpDeviceSearch.Name = "grpDeviceSearch";
            this.grpDeviceSearch.Size = new System.Drawing.Size(1172, 250);
            this.grpDeviceSearch.TabIndex = 0;
            this.grpDeviceSearch.TabStop = false;
            this.grpDeviceSearch.Text = "Device Discovery";

            this.btnAutoSearch.Location = new System.Drawing.Point(10, 25);
            this.btnAutoSearch.Name = "btnAutoSearch";
            this.btnAutoSearch.Size = new System.Drawing.Size(120, 30);
            this.btnAutoSearch.TabIndex = 0;
            this.btnAutoSearch.Text = "Auto Search";
            this.btnAutoSearch.UseVisualStyleBackColor = true;
            this.btnAutoSearch.Click += new System.EventHandler(this.btnAutoSearch_Click);

            this.btnRefreshDevices.Location = new System.Drawing.Point(140, 25);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshDevices.TabIndex = 1;
            this.btnRefreshDevices.Text = "Refresh";
            this.btnRefreshDevices.UseVisualStyleBackColor = true;
            this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);

            this.btnConnectDevice.Location = new System.Drawing.Point(270, 25);
            this.btnConnectDevice.Name = "btnConnectDevice";
            this.btnConnectDevice.Size = new System.Drawing.Size(120, 30);
            this.btnConnectDevice.TabIndex = 2;
            this.btnConnectDevice.Text = "Install Device";
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
            this.grpDeviceManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDeviceManagement.Location = new System.Drawing.Point(10, 260);
            this.grpDeviceManagement.Name = "grpDeviceManagement";
            this.grpDeviceManagement.Size = new System.Drawing.Size(1172, 75);
            this.grpDeviceManagement.TabIndex = 2;
            this.grpDeviceManagement.TabStop = false;
            this.grpDeviceManagement.Text = "Installed Devices - Management";

            this.btnRemoteControl.Location = new System.Drawing.Point(10, 25);
            this.btnRemoteControl.Name = "btnRemoteControl";
            this.btnRemoteControl.Size = new System.Drawing.Size(150, 35);
            this.btnRemoteControl.TabIndex = 0;
            this.btnRemoteControl.Text = "Remote Control";
            this.btnRemoteControl.UseVisualStyleBackColor = true;
            this.btnRemoteControl.Click += new System.EventHandler(this.btnRemoteControl_Click);

            this.btnRemoveDevice.Location = new System.Drawing.Point(170, 25);
            this.btnRemoveDevice.Name = "btnRemoveDevice";
            this.btnRemoveDevice.Size = new System.Drawing.Size(150, 35);
            this.btnRemoveDevice.TabIndex = 1;
            this.btnRemoveDevice.Text = "Remove Device";
            this.btnRemoveDevice.ForeColor = System.Drawing.Color.DarkRed;
            this.btnRemoveDevice.UseVisualStyleBackColor = true;
            this.btnRemoveDevice.Click += new System.EventHandler(this.btnRemoveDevice_Click);

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
            this.tabDepartments.Text = "Departments";
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
            this.tabPersonnel.Controls.Add(this.btnRefreshPersonnel);
            this.tabPersonnel.Controls.Add(this.dgvPersonnel);
            this.tabPersonnel.Location = new System.Drawing.Point(4, 24);
            this.tabPersonnel.Name = "tabPersonnel";
            this.tabPersonnel.Padding = new System.Windows.Forms.Padding(10);
            this.tabPersonnel.Size = new System.Drawing.Size(1192, 572);
            this.tabPersonnel.TabIndex = 3;
            this.tabPersonnel.Text = "Personnel";
            this.tabPersonnel.UseVisualStyleBackColor = true;

            this.btnAddPerson.Location = new System.Drawing.Point(10, 10);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(120, 30);
            this.btnAddPerson.TabIndex = 0;
            this.btnAddPerson.Text = "Add Personnel";
            this.btnAddPerson.UseVisualStyleBackColor = true;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);

            this.btnRefreshPersonnel.Location = new System.Drawing.Point(140, 10);
            this.btnRefreshPersonnel.Name = "btnRefreshPersonnel";
            this.btnRefreshPersonnel.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshPersonnel.TabIndex = 1;
            this.btnRefreshPersonnel.Text = "Refresh";
            this.btnRefreshPersonnel.UseVisualStyleBackColor = true;
            this.btnRefreshPersonnel.Click += new System.EventHandler(this.btnRefreshPersonnel_Click);

            this.dgvPersonnel.AllowUserToAddRows = false;
            this.dgvPersonnel.AllowUserToDeleteRows = false;
            this.dgvPersonnel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPersonnel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPersonnel.Location = new System.Drawing.Point(10, 50);
            this.dgvPersonnel.Name = "dgvPersonnel";
            this.dgvPersonnel.ReadOnly = true;
            this.dgvPersonnel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersonnel.Size = new System.Drawing.Size(1172, 512);
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
            this.tabAttendance.Text = "Attendance";
            this.tabAttendance.UseVisualStyleBackColor = true;

            // 
            // grpAttendanceSearch
            // 
            this.grpAttendanceSearch.Controls.Add(this.label5);
            this.grpAttendanceSearch.Controls.Add(this.txtAttendanceUserID);
            this.grpAttendanceSearch.Controls.Add(this.label6);
            this.grpAttendanceSearch.Controls.Add(this.txtAttendanceUserName);
            this.grpAttendanceSearch.Controls.Add(this.label7);
            this.grpAttendanceSearch.Controls.Add(this.cmbAttendanceDepartment);
            this.grpAttendanceSearch.Controls.Add(this.label8);
            this.grpAttendanceSearch.Controls.Add(this.dtpAttendanceStart);
            this.grpAttendanceSearch.Controls.Add(this.label9);
            this.grpAttendanceSearch.Controls.Add(this.dtpAttendanceEnd);
            this.grpAttendanceSearch.Controls.Add(this.btnSearchAttendance);
            this.grpAttendanceSearch.Controls.Add(this.btnGetStatistics);
            this.grpAttendanceSearch.Controls.Add(this.btnRefreshAttendance);
            this.grpAttendanceSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpAttendanceSearch.Location = new System.Drawing.Point(10, 10);
            this.grpAttendanceSearch.Name = "grpAttendanceSearch";
            this.grpAttendanceSearch.Size = new System.Drawing.Size(1172, 120);
            this.grpAttendanceSearch.TabIndex = 0;
            this.grpAttendanceSearch.TabStop = false;
            this.grpAttendanceSearch.Text = "Attendance Search";

            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "User ID:";

            this.txtAttendanceUserID.Location = new System.Drawing.Point(100, 22);
            this.txtAttendanceUserID.Name = "txtAttendanceUserID";
            this.txtAttendanceUserID.Size = new System.Drawing.Size(150, 23);
            this.txtAttendanceUserID.TabIndex = 1;

            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(270, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Name:";

            this.txtAttendanceUserName.Location = new System.Drawing.Point(330, 22);
            this.txtAttendanceUserName.Name = "txtAttendanceUserName";
            this.txtAttendanceUserName.Size = new System.Drawing.Size(150, 23);
            this.txtAttendanceUserName.TabIndex = 3;

            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(500, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Department:";

            this.cmbAttendanceDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttendanceDepartment.FormattingEnabled = true;
            this.cmbAttendanceDepartment.Location = new System.Drawing.Point(590, 22);
            this.cmbAttendanceDepartment.Name = "cmbAttendanceDepartment";
            this.cmbAttendanceDepartment.Size = new System.Drawing.Size(200, 23);
            this.cmbAttendanceDepartment.TabIndex = 5;

            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 15);
            this.label8.TabIndex = 6;
            this.label8.Text = "Start Time:";

            this.dtpAttendanceStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttendanceStart.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpAttendanceStart.Location = new System.Drawing.Point(100, 57);
            this.dtpAttendanceStart.Name = "dtpAttendanceStart";
            this.dtpAttendanceStart.ShowCheckBox = true;
            this.dtpAttendanceStart.Size = new System.Drawing.Size(200, 23);
            this.dtpAttendanceStart.TabIndex = 7;

            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(320, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "End Time:";

            this.dtpAttendanceEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttendanceEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpAttendanceEnd.Location = new System.Drawing.Point(400, 57);
            this.dtpAttendanceEnd.Name = "dtpAttendanceEnd";
            this.dtpAttendanceEnd.ShowCheckBox = true;
            this.dtpAttendanceEnd.Size = new System.Drawing.Size(200, 23);
            this.dtpAttendanceEnd.TabIndex = 9;

            this.btnSearchAttendance.Location = new System.Drawing.Point(620, 55);
            this.btnSearchAttendance.Name = "btnSearchAttendance";
            this.btnSearchAttendance.Size = new System.Drawing.Size(100, 30);
            this.btnSearchAttendance.TabIndex = 10;
            this.btnSearchAttendance.Text = "Search";
            this.btnSearchAttendance.UseVisualStyleBackColor = true;
            this.btnSearchAttendance.Click += new System.EventHandler(this.btnSearchAttendance_Click);

            this.btnGetStatistics.Location = new System.Drawing.Point(730, 55);
            this.btnGetStatistics.Name = "btnGetStatistics";
            this.btnGetStatistics.Size = new System.Drawing.Size(100, 30);
            this.btnGetStatistics.TabIndex = 11;
            this.btnGetStatistics.Text = "Statistics";
            this.btnGetStatistics.UseVisualStyleBackColor = true;
            this.btnGetStatistics.Click += new System.EventHandler(this.btnGetStatistics_Click);

            this.btnRefreshAttendance.Location = new System.Drawing.Point(840, 55);
            this.btnRefreshAttendance.Name = "btnRefreshAttendance";
            this.btnRefreshAttendance.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshAttendance.TabIndex = 12;
            this.btnRefreshAttendance.Text = "Refresh";
            this.btnRefreshAttendance.UseVisualStyleBackColor = true;
            this.btnRefreshAttendance.Click += new System.EventHandler(this.btnRefreshAttendance_Click);

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
        private System.Windows.Forms.DataGridView dgvDevices;
        private System.Windows.Forms.Button btnAddDepartment;
        private System.Windows.Forms.Button btnRefreshDepartments;
        private System.Windows.Forms.DataGridView dgvDepartments;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.Button btnRefreshPersonnel;
        private System.Windows.Forms.DataGridView dgvPersonnel;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.GroupBox grpAttendanceSearch;
        private System.Windows.Forms.TextBox txtAttendanceUserID;
        private System.Windows.Forms.TextBox txtAttendanceUserName;
        private System.Windows.Forms.ComboBox cmbAttendanceDepartment;
        private System.Windows.Forms.DateTimePicker dtpAttendanceStart;
        private System.Windows.Forms.DateTimePicker dtpAttendanceEnd;
        private System.Windows.Forms.Button btnSearchAttendance;
        private System.Windows.Forms.Button btnGetStatistics;
        private System.Windows.Forms.Button btnRefreshAttendance;
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.Label lblAttendanceCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}
