using System.Net.Http.Json;
using System.Text.Json;
using FaceDeviceDesktopClient.Forms;

namespace FaceDeviceDesktopClient;

public partial class MainForm : Form
{
    private readonly HttpClient _httpClient;
    private readonly string _serverUrl = "http://localhost:8100";

    public MainForm()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri(_serverUrl) };
        SetupDeviceGrid();
        SetupDeviceContextMenu();
        LoadInitialData();
    }

    private void SetupDeviceGrid()
    {
        // Ensure AutoGenerateColumns is disabled for custom columns
        dgvDevices.AutoGenerateColumns = false;
        dgvDevices.AllowUserToAddRows = false;
        dgvDevices.ReadOnly = true;
        dgvDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // Add ACS-style columns
        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "No", 
            HeaderText = "No.", 
            DataPropertyName = "", 
            Width = 50,
            ReadOnly = true 
        });

        dgvDevices.Columns.Add(new DataGridViewCheckBoxColumn 
        { 
            Name = "Selected", 
            HeaderText = "Selected", 
            Width = 70,
            ReadOnly = false 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "DeviceName", 
            HeaderText = "Door", 
            DataPropertyName = "DeviceName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "TagName", 
            HeaderText = "Tag Name", 
            DataPropertyName = "TagName",
            Width = 120 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "Model", 
            HeaderText = "Model", 
            DataPropertyName = "Model",
            Width = 100 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "SN", 
            HeaderText = "SN", 
            DataPropertyName = "SN",
            Width = 150 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "IpAddress", 
            HeaderText = "IP", 
            DataPropertyName = "IpAddress",
            Width = 120 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "UnitNo", 
            HeaderText = "Unit No.", 
            DataPropertyName = "UnitNo",
            Width = 80 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "Status", 
            HeaderText = "Status", 
            DataPropertyName = "Status",
            Width = 80 
        });

        // Add double-click event handler
        dgvDevices.CellDoubleClick += DgvDevices_CellDoubleClick;
        dgvDevices.RowPostPaint += DgvDevices_RowPostPaint;
    }

    private void DgvDevices_RowPostPaint(object? sender, DataGridViewRowPostPaintEventArgs e)
    {
        // Display row number in "No." column
        var grid = sender as DataGridView;
        if (grid != null)
        {
            var rowIdx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.Columns["No"].Width, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, grid.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
    }

    private void DgvDevices_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        try
        {
            var row = dgvDevices.Rows[e.RowIndex];
            var device = row.DataBoundItem as DeviceInfo;

            if (device == null)
            {
                ShowError("Invalid device data");
                return;
            }

            // Open device detail form
            using var detailForm = new DeviceDetailForm(device, _httpClient);
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh device list after editing
                _ = RefreshDevices();
                _ = RefreshSystemInfo();
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to open device details: {ex.Message}");
        }
    }

    private void SetupDeviceContextMenu()
    {
        var contextMenu = new ContextMenuStrip();

        var menuRemoteControl = new ToolStripMenuItem("Remote Control");
        menuRemoteControl.Click += MenuRemoteControl_Click;

        var menuRefresh = new ToolStripMenuItem("Refresh Status");
        menuRefresh.Click += (s, e) => RefreshDevices();

        var menuSeparator = new ToolStripSeparator();

        var menuDelete = new ToolStripMenuItem("Remove Device");
        menuDelete.Click += MenuDeleteDevice_Click;
        menuDelete.ForeColor = System.Drawing.Color.DarkRed;

        contextMenu.Items.AddRange(new ToolStripItem[] { menuRemoteControl, menuRefresh, menuSeparator, menuDelete });
        dgvDevices.ContextMenuStrip = contextMenu;
    }

    private void MenuRemoteControl_Click(object? sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a device", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvDevices.SelectedRows[0];
            var sn = row.Cells["SN"].Value?.ToString();
            var name = row.Cells["DeviceName"].Value?.ToString() ?? "Unknown";

            if (string.IsNullOrWhiteSpace(sn))
            {
                ShowError("Invalid device selection");
                return;
            }

            using var remoteControlForm = new DeviceRemoteControlForm
            {
                DeviceSN = sn,
                DeviceName = name
            };
            remoteControlForm.ShowDialog();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to open remote control: {ex.Message}");
        }
    }

    private async void MenuDeleteDevice_Click(object? sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
            return;

        try
        {
            var row = dgvDevices.SelectedRows[0];
            var sn = row.Cells["SN"].Value?.ToString();
            var name = row.Cells["DeviceName"].Value?.ToString() ?? sn;

            if (string.IsNullOrWhiteSpace(sn))
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to remove device '{name}' ({sn})?\n\nThis will disconnect the device from the server.",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            var response = await _httpClient.DeleteAsync($"/admin/devices/{sn}");

            if (response.IsSuccessStatusCode)
            {
                lblStatus.Text = $"Device {sn} removed successfully";
                await RefreshDevices();
                await RefreshSystemInfo();
            }
            else
            {
                ShowError($"Failed to remove device: HTTP {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to remove device: {ex.Message}");
        }
    }

    private async void LoadInitialData()
    {
        try
        {
            await RefreshSystemInfo();
            await RefreshDevices();
            await RefreshDepartments();
            await RefreshPersonnel();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load initial data: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task RefreshSystemInfo()
    {
        try
        {
            var devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
            var people = await _httpClient.GetFromJsonAsync<List<PersonInfo>>("/admin/people");
            var departments = await _httpClient.GetFromJsonAsync<List<DepartmentInfo>>("/admin/departments");

            lblTotalDevices.Text = devices?.Count.ToString() ?? "0";
            lblTotalPersonnel.Text = people?.Count.ToString() ?? "0";
            lblTotalDepartments.Text = departments?.Count.ToString() ?? "0";

            int totalRecords = devices?.Sum(d => d.RecordCount) ?? 0;
            lblTotalRecords.Text = totalRecords.ToString();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to refresh system info: {ex.Message}");
        }
    }

    private async Task RefreshDevices()
    {
        try
        {
            lblStatus.Text = "Loading devices...";
            var devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");

            if (devices == null)
            {
                ShowError("Received null device list from server");
                lblStatus.Text = "Failed to load devices: null response";
                return;
            }

            // Update UI on the UI thread
            if (InvokeRequired)
            {
                Invoke(() => UpdateDeviceGrid(devices));
            }
            else
            {
                UpdateDeviceGrid(devices);
            }
        }
        catch (Exception ex)
        {
            var errorMsg = $"Failed to refresh devices: {ex.Message}\n\nStack trace: {ex.StackTrace}";
            ShowError(errorMsg);
            lblStatus.Text = $"Error loading devices: {ex.Message}";
        }
    }

    private void UpdateDeviceGrid(List<DeviceInfo> devices)
    {
        // Clear existing data (but keep columns)
        dgvDevices.DataSource = null;

        if (devices.Count > 0)
        {
            // Create a BindingList for better data binding
            var bindingList = new System.ComponentModel.BindingList<DeviceInfo>(devices);
            dgvDevices.DataSource = bindingList;
            lblStatus.Text = $"Loaded {devices.Count} device(s)";
        }
        else
        {
            lblStatus.Text = "No devices installed yet";
        }
    }

    private async Task RefreshDepartments()
    {
        try
        {
            var departments = await _httpClient.GetFromJsonAsync<List<DepartmentInfo>>("/admin/departments");

            dgvDepartments.DataSource = null;
            dgvDepartments.DataSource = departments;

            // Update combo boxes
            cmbDepartment.DataSource = null;
            cmbDepartment.DataSource = new List<DepartmentInfo>(departments ?? new());
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentID";
        }
        catch (Exception ex)
        {
            ShowError($"Failed to refresh departments: {ex.Message}");
        }
    }

    private async Task RefreshPersonnel()
    {
        try
        {
            var personnel = await _httpClient.GetFromJsonAsync<List<PersonInfo>>("/admin/people");

            dgvPersonnel.DataSource = null;
            dgvPersonnel.DataSource = personnel;
            dgvPersonnel.AutoResizeColumns();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to refresh personnel: {ex.Message}");
        }
    }

    private async Task RefreshAttendance()
    {
        try
        {
            var request = new AttendanceSearchRequest
            {
                UserID = txtAttendanceUserID.Text,
                UserName = txtAttendanceUserName.Text,
                DepartmentID = cmbAttendanceDepartment.SelectedValue?.ToString(),
                StartTime = dtpAttendanceStart.Checked ? dtpAttendanceStart.Value : null,
                EndTime = dtpAttendanceEnd.Checked ? dtpAttendanceEnd.Value : null,
                PageIndex = 1,
                PageSize = 1000
            };

            var response = await _httpClient.PostAsJsonAsync("/api/Attendance/Search", request);
            var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<AttendanceSearchResult>>();

            if (result?.Code == 0 && result.Data != null)
            {
                dgvAttendance.DataSource = null;
                dgvAttendance.DataSource = result.Data.DataList;
                dgvAttendance.AutoResizeColumns();
                lblAttendanceCount.Text = $"Total: {result.Data.TotalCount} records";
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to refresh attendance: {ex.Message}");
        }
    }

    private async void btnAutoSearch_Click(object sender, EventArgs e)
    {
        try
        {
            // Search 옵션 선택 다이얼로그
            using var searchDialog = new Form
            {
                Text = "Select Search Method",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var rbBroadcast = new RadioButton
            {
                Text = "Broadcast Search (UDP Discovery)",
                Location = new Point(20, 20),
                Size = new Size(350, 25),
                Checked = true
            };

            var rbNetworkScan = new RadioButton
            {
                Text = "Network Scan (HTTP Probe)",
                Location = new Point(20, 50),
                Size = new Size(350, 25)
            };

            var txtSubnet = new TextBox
            {
                Location = new Point(20, 80),
                Size = new Size(200, 25),
                Text = "192.168.0",
                PlaceholderText = "Enter subnet (e.g., 192.168.0)"
            };

            var lblSubnet = new Label
            {
                Text = "Subnet:",
                Location = new Point(20, 60),
                Size = new Size(200, 20)
            };

            var btnOk = new Button
            {
                Text = "Start Search",
                DialogResult = DialogResult.OK,
                Location = new Point(240, 110),
                Size = new Size(120, 30)
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(120, 110),
                Size = new Size(100, 30)
            };

            searchDialog.Controls.AddRange(new Control[] { rbBroadcast, rbNetworkScan, lblSubnet, txtSubnet, btnOk, btnCancel });
            searchDialog.AcceptButton = btnOk;
            searchDialog.CancelButton = btnCancel;

            // 옵션 변경 시 subnet 입력 활성화/비활성화
            rbBroadcast.CheckedChanged += (s, args) =>
            {
                lblSubnet.Visible = !rbBroadcast.Checked;
                txtSubnet.Visible = !rbBroadcast.Checked;
            };

            lblSubnet.Visible = false;
            txtSubnet.Visible = false;

            if (searchDialog.ShowDialog() != DialogResult.OK)
                return;

            btnAutoSearch.Enabled = false;
            dgvDiscoveredDevices.DataSource = null;

            if (rbBroadcast.Checked)
            {
                // Broadcast Search
                lblStatus.Text = "Broadcasting discovery request...";

                var response = await _httpClient.PostAsJsonAsync("/api/Device/Search", 
                    new { SearchType = "broadcast" });
                var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<List<DiscoveredDevice>>>();

                if (result?.Code == 0 && result.Data != null && result.Data.Count > 0)
                {
                    dgvDiscoveredDevices.DataSource = result.Data;
                    dgvDiscoveredDevices.AutoResizeColumns();
                    lblStatus.Text = $"Found {result.Data.Count} device(s)";
                }
                else
                {
                    lblStatus.Text = "No devices discovered via broadcast";
                    MessageBox.Show("No devices found. Try Network Scan instead.", "Search Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Network Scan
                var subnet = txtSubnet.Text.Trim();
                if (string.IsNullOrWhiteSpace(subnet))
                {
                    MessageBox.Show("Please enter a valid subnet (e.g., 192.168.0)", "Input Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblStatus.Text = $"Scanning network {subnet}.1-254...";

                var devices = new List<DiscoveredDevice>();
                var progressForm = new Form
                {
                    Text = "Network Scan Progress",
                    Size = new Size(500, 150),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    ControlBox = false
                };

                var lblProgress = new Label
                {
                    Text = "Scanning...",
                    Location = new Point(20, 20),
                    Size = new Size(450, 25)
                };

                var progressBar = new ProgressBar
                {
                    Location = new Point(20, 50),
                    Size = new Size(450, 25),
                    Maximum = 254
                };

                var lblDevices = new Label
                {
                    Text = "Devices found: 0",
                    Location = new Point(20, 80),
                    Size = new Size(450, 25)
                };

                progressForm.Controls.AddRange(new Control[] { lblProgress, progressBar, lblDevices });
                progressForm.Show();

                try
                {
                    // Server-Sent Events로 실시간 진행 상황 수신
                    using var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
                    httpClient.BaseAddress = new Uri(_serverUrl);

                    var request = new HttpRequestMessage(HttpMethod.Post, "/api/Device/SearchStream")
                    {
                        Content = JsonContent.Create(new { SearchType = "scan", Subnet = subnet })
                    };

                    using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    using var stream = await response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    int scannedCount = 0;
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        // 빈 줄 무시 (SSE 형식에서 \n\n으로 메시지 구분)
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        if (line.StartsWith("data: "))
                        {
                            var json = line.Substring(6);
                            if (json == "[DONE]")
                            {
                                break;
                            }

                            try
                            {
                                var device = JsonSerializer.Deserialize<DiscoveredDevice>(json);
                                if (device != null)
                                {
                                    devices.Add(device);
                                    lblDevices.Text = $"Devices found: {devices.Count}";
                                    Application.DoEvents();
                                }
                            }
                            catch (JsonException)
                            {
                                // JSON 파싱 실패 무시 (로그는 서버 쪽에서 처리)
                            }
                        }
                        else if (line.StartsWith("progress: "))
                        {
                            if (int.TryParse(line.Substring(10), out scannedCount))
                            {
                                progressBar.Value = Math.Min(scannedCount, 254);
                                lblProgress.Text = $"Scanning {subnet}.{scannedCount}/254...";
                                Application.DoEvents();
                            }
                        }
                    }

                    dgvDiscoveredDevices.DataSource = devices;
                    dgvDiscoveredDevices.AutoResizeColumns();
                    lblStatus.Text = $"Scan completed: {devices.Count} device(s) found";
                }
                finally
                {
                    progressForm.Close();
                }

                if (devices.Count == 0)
                {
                    MessageBox.Show("No devices found in the network.", "Search Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        catch (Exception ex)
        {
            ShowError($"Auto search failed: {ex.Message}");
        }
        finally
        {
            btnAutoSearch.Enabled = true;
        }
    }

    private async void btnConnectDevice_Click(object sender, EventArgs e)
    {
        if (dgvDiscoveredDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a device to install", "Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvDiscoveredDevices.SelectedRows[0];
            var ip = row.Cells["IpAddress"].Value?.ToString();
            var port = Convert.ToInt32(row.Cells["HttpPort"].Value ?? 80);
            var deviceSN = row.Cells["DeviceSN"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(deviceSN))
            {
                ShowError("Invalid device information");
                return;
            }

            lblStatus.Text = $"Probing device {deviceSN}...";

            // Probe device to get full details
            var probeResponse = await _httpClient.PostAsJsonAsync("/api/Device/ProbeDevice", 
                new { IpAddress = ip, HttpPort = port });
            var probeResult = await probeResponse.Content.ReadFromJsonAsync<BrowserApiResponse<DeviceProbeInfo>>();

            if (probeResult?.Code != 0 || probeResult.Data == null)
            {
                ShowError($"Failed to probe device: {probeResult?.Msg ?? "Unknown error"}");
                return;
            }

            // Show Install dialog
            using var installForm = new DeviceInstallForm
            {
                DeviceSN = probeResult.Data.DeviceSN ?? deviceSN,
                IpAddress = ip,
                HttpPort = port,
                Model = probeResult.Data.Model,
                FirmwareVersion = probeResult.Data.FirmwareVersion
            };

            if (installForm.ShowDialog() != DialogResult.OK)
            {
                lblStatus.Text = "Installation cancelled";
                return;
            }

            lblStatus.Text = $"Installing device {installForm.DeviceSN}...";

            // Install device with configuration
            var installRequest = new
            {
                DeviceSN = installForm.DeviceSN,
                IpAddress = installForm.IpAddress,
                HttpPort = installForm.HttpPort,
                DeviceName = installForm.DeviceName,
                TagName = installForm.TagName,
                MenuPassword = installForm.MenuPassword,
                Language = installForm.Language,
                Model = installForm.Model,
                FirmwareVersion = installForm.FirmwareVersion
            };

            var connectResponse = await _httpClient.PostAsJsonAsync("/api/Device/Connect", installRequest);
            var connectResult = await connectResponse.Content.ReadFromJsonAsync<BrowserApiResponse<string>>();

            if (connectResult?.Code == 0)
            {
                lblStatus.Text = $"Successfully installed device {installForm.DeviceSN}";
                MessageBox.Show($"Device {installForm.DeviceSN} has been successfully installed and registered.", 
                    "Installation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await RefreshDevices();
                await RefreshSystemInfo();

                // Clear discovered devices after successful install
                dgvDiscoveredDevices.DataSource = null;
            }
            else
            {
                ShowError($"Installation failed: {connectResult?.Msg ?? "Unknown error"}");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Installation failed: {ex.Message}");
        }
    }

    private async void btnAddDepartment_Click(object sender, EventArgs e)
    {
        using var form = new DepartmentForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/Department/New", form.Department);
                var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

                if (result?.Code == 0)
                {
                    lblStatus.Text = "Department added successfully";
                    await RefreshDepartments();
                }
                else
                {
                    ShowError($"Failed to add department: {result?.Msg}");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Failed to add department: {ex.Message}");
            }
        }
    }

    private async void btnAddPerson_Click(object sender, EventArgs e)
    {
        using var form = new PersonForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/People/New", form.Person);
                var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

                if (result?.Code == 0)
                {
                    lblStatus.Text = "Personnel added successfully";
                    await RefreshPersonnel();
                    await RefreshSystemInfo();
                }
                else
                {
                    ShowError($"Failed to add personnel: {result?.Msg}");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Failed to add personnel: {ex.Message}");
            }
        }
    }

    private async void btnSearchAttendance_Click(object sender, EventArgs e)
    {
        await RefreshAttendance();
    }

    private async void btnGetStatistics_Click(object sender, EventArgs e)
    {
        try
        {
            var request = new AttendanceSearchRequest
            {
                StartTime = dtpAttendanceStart.Checked ? dtpAttendanceStart.Value : null,
                EndTime = dtpAttendanceEnd.Checked ? dtpAttendanceEnd.Value : null,
                PageIndex = 1,
                PageSize = 1
            };

            var response = await _httpClient.PostAsJsonAsync("/api/Attendance/Statistics", request);
            var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<AttendanceStatistics>>();

            if (result?.Code == 0 && result.Data != null)
            {
                var stats = result.Data;
                MessageBox.Show(
                    $"Total Records: {stats.TotalRecords}\n" +
                    $"Unique Users: {stats.UniqueUsers}\n" +
                    $"Unique Departments: {stats.UniqueDepartments}",
                    "Attendance Statistics",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to get statistics: {ex.Message}");
        }
    }

    private void btnRefreshDevices_Click(object sender, EventArgs e) => _ = RefreshDevices();
    private void btnRefreshDepartments_Click(object sender, EventArgs e) => _ = RefreshDepartments();
    private void btnRefreshPersonnel_Click(object sender, EventArgs e) => _ = RefreshPersonnel();
    private void btnRefreshAttendance_Click(object sender, EventArgs e) => _ = RefreshAttendance();

    private void btnRemoteControl_Click(object sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a device from the list below", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvDevices.SelectedRows[0];
            var sn = row.Cells["SN"].Value?.ToString();
            var name = row.Cells["DeviceName"].Value?.ToString() ?? "Unknown";

            if (string.IsNullOrWhiteSpace(sn))
            {
                ShowError("Invalid device selection");
                return;
            }

            using var remoteControlForm = new DeviceRemoteControlForm
            {
                DeviceSN = sn,
                DeviceName = name
            };
            remoteControlForm.ShowDialog();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to open remote control: {ex.Message}");
        }
    }

    private async void btnRemoveDevice_Click(object sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a device to remove", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvDevices.SelectedRows[0];
            var sn = row.Cells["SN"].Value?.ToString();
            var name = row.Cells["DeviceName"].Value?.ToString() ?? sn;

            if (string.IsNullOrWhiteSpace(sn))
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to remove device '{name}' ({sn})?\n\nThis will disconnect the device from the server.",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            var response = await _httpClient.DeleteAsync($"/admin/devices/{sn}");

            if (response.IsSuccessStatusCode)
            {
                lblStatus.Text = $"Device {sn} removed successfully";
                await RefreshDevices();
                await RefreshSystemInfo();
            }
            else
            {
                ShowError($"Failed to remove device: HTTP {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to remove device: {ex.Message}");
        }
    }

    private void ShowError(string message)
    {
        lblStatus.Text = message;
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
