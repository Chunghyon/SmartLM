using System.Net.Http.Json;
using System.ComponentModel;
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
        SetupDiscoveredDevicesGrid();
        SetupPersonnelGrid();
        SetupAttendanceGrid();
        SetupDeviceContextMenu();
        LoadInitialData();
    }

    private void SetupDiscoveredDevicesGrid()
    {
        // Setup columns for discovered devices grid (IP and Serial Number only)
        dgvDiscoveredDevices.AutoGenerateColumns = false;
        dgvDiscoveredDevices.AllowUserToAddRows = false;
        dgvDiscoveredDevices.ReadOnly = true;
        dgvDiscoveredDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvDiscoveredDevices.Columns.Clear();

        dgvDiscoveredDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "IpAddress", 
            HeaderText = "IP 주소", 
            DataPropertyName = "IpAddress",
            Width = 150 
        });

        dgvDiscoveredDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "DeviceSN", 
            HeaderText = "시리얼넘버", 
            DataPropertyName = "DeviceSN",
            Width = 200 
        });

        dgvDiscoveredDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "DeviceName", 
            HeaderText = "디바이스명", 
            DataPropertyName = "DeviceName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
        });
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
            HeaderText = "순번", 
            DataPropertyName = "", 
            Width = 50,
            ReadOnly = true 
        });

        dgvDevices.Columns.Add(new DataGridViewCheckBoxColumn 
        { 
            Name = "Selected", 
            HeaderText = "선택", 
            Width = 70,
            ReadOnly = false 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "DeviceName", 
            HeaderText = "단말기명", 
            DataPropertyName = "DeviceName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "TagName", 
            HeaderText = "위치", 
            DataPropertyName = "TagName",
            Width = 120 
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn 
        { 
            Name = "SN", 
            HeaderText = "시리얼넘버", 
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
            Name = "Status", 
            HeaderText = "상태", 
            DataPropertyName = "Status",
            Width = 80 
        });

        // Add double-click event handler
        dgvDevices.CellDoubleClick += DgvDevices_CellDoubleClick;
        dgvDevices.RowPostPaint += DgvDevices_RowPostPaint;
    }

    private void SetupPersonnelGrid()
    {
        dgvPersonnel.AutoGenerateColumns = false;
        dgvPersonnel.AllowUserToAddRows = false;
        dgvPersonnel.ReadOnly = true;
        dgvPersonnel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvPersonnel.Columns.Clear();

        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "사용자번호",
            HeaderText = "사용자번호",
            DataPropertyName = "UserID",
            Width = 120
        });

        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "사용자명",
            HeaderText = "사용자명",
            DataPropertyName = "Name",
            Width = 150
        });

        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "사진등록",
            HeaderText = "사진등록",
            DataPropertyName = "PhotoUrl",
            Width = 100
        });

        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "패스워드",
            HeaderText = "패스워드",
            DataPropertyName = "Password",
            Width = 100
        });

        // Add a calculated column for "할당된 단말기수"
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "할당된단말기수",
            HeaderText = "할당된 단말기수",
            Width = 120
        });

        // Add cell formatting for photo and password columns
        dgvPersonnel.CellFormatting += (sender, e) =>
        {
            if (e.ColumnIndex == dgvPersonnel.Columns["사진등록"].Index && e.Value != null)
            {
                var photoValue = e.Value.ToString();
                if (!string.IsNullOrWhiteSpace(photoValue) && photoValue.Length > 50)
                {
                    e.Value = "사진 있음";
                    e.FormattingApplied = true;
                }
                else if (string.IsNullOrWhiteSpace(photoValue))
                {
                    e.Value = "사진 없음";
                    e.FormattingApplied = true;
                }
            }
            else if (e.ColumnIndex == dgvPersonnel.Columns["패스워드"].Index && e.Value != null)
            {
                var passwordValue = e.Value.ToString();
                if (!string.IsNullOrWhiteSpace(passwordValue))
                {
                    e.Value = new string('●', Math.Min(passwordValue.Length, 8));
                    e.FormattingApplied = true;
                }
            }
        };

        // Add double-click event to edit person
        dgvPersonnel.CellDoubleClick += (sender, e) =>
        {
            if (e.RowIndex >= 0)
            {
                btnEditPerson_Click(sender, EventArgs.Empty);
            }
        };
    }

    private void SetupAttendanceGrid()
    {
        dgvAttendance.AutoGenerateColumns = false;
        dgvAttendance.AllowUserToAddRows = false;
        dgvAttendance.ReadOnly = true;
        dgvAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvAttendance.Columns.Clear();

        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "시간",
            HeaderText = "시간",
            DataPropertyName = "Time",
            Width = 150
        });

        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "사용자번호",
            HeaderText = "사용자번호",
            DataPropertyName = "UserID",
            Width = 120
        });

        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "사용자명",
            HeaderText = "사용자명",
            DataPropertyName = "UserName",
            Width = 150
        });

        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "단말기명",
            HeaderText = "단말기명",
            DataPropertyName = "DeviceName",
            Width = 150
        });

        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "이벤트",
            HeaderText = "이벤트",
            DataPropertyName = "EventType",
            Width = 100
        });
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

        var menuSeparator1 = new ToolStripSeparator();

        // 설정 서브메뉴
        var menuSettings = new ToolStripMenuItem("설정");

        var menuNetworkSettings = new ToolStripMenuItem("네트워크 설정");
        menuNetworkSettings.Click += MenuDeviceNetworkSettings_Click;

        var menuAccessControlSettings = new ToolStripMenuItem("출입 제어 설정");
        menuAccessControlSettings.Click += MenuDeviceAccessControlSettings_Click;

        var menuAlarmSettings = new ToolStripMenuItem("알람 설정");
        menuAlarmSettings.Click += MenuDeviceAlarmSettings_Click;

        menuSettings.DropDownItems.AddRange(new ToolStripItem[] 
        { 
            menuNetworkSettings, 
            menuAccessControlSettings, 
            menuAlarmSettings 
        });

        var menuSeparator2 = new ToolStripSeparator();

        var menuDelete = new ToolStripMenuItem("Remove Device");
        menuDelete.Click += MenuDeleteDevice_Click;
        menuDelete.ForeColor = System.Drawing.Color.DarkRed;

        contextMenu.Items.AddRange(new ToolStripItem[] 
        { 
            menuRemoteControl, 
            menuRefresh, 
            menuSeparator1,
            menuSettings,
            menuSeparator2, 
            menuDelete 
        });
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

            // Update "할당된 단말기수" column for each row (placeholder - would need API support)
            foreach (DataGridViewRow row in dgvPersonnel.Rows)
            {
                row.Cells["할당된단말기수"].Value = "0"; // Placeholder
            }

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
                lblAttendanceCount.Text = $"전체: {result.Data.TotalCount}건";
            }
        }
        catch (Exception ex)
        {
            ShowError($"출입 조회 실패: {ex.Message}");
        }
    }

    private async void btnAutoSearch_Click(object sender, EventArgs e)
    {
        try
        {
            // 사용 가능한 네트워크 인터페이스 가져오기
            var interfacesResponse = await _httpClient.GetAsync("/api/Device/GetNetworkInterfaces");
            var interfacesResult = await interfacesResponse.Content.ReadFromJsonAsync<BrowserApiResponse<List<NetworkInterfaceInfo>>>();

            if (interfacesResult?.Code != 0 || interfacesResult.Data == null || interfacesResult.Data.Count == 0)
            {
                MessageBox.Show("사용 가능한 네트워크 인터페이스가 없습니다.", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var networkInterfaces = interfacesResult.Data;

            // Search 옵션 선택 다이얼로그
            using var searchDialog = new Form
            {
                Text = "단말기 검색 설정",
                Size = new Size(450, 330),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblInterface = new Label
            {
                Text = "네트워크 인터페이스:",
                Location = new Point(20, 20),
                Size = new Size(150, 20)
            };

            var cmbInterface = new ComboBox
            {
                Location = new Point(20, 45),
                Size = new Size(400, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            foreach (var iface in networkInterfaces)
            {
                cmbInterface.Items.Add($"{iface.LocalIp} (브로드캐스트: {iface.BroadcastIp})");
            }
            cmbInterface.SelectedIndex = 0;

            var rbBroadcast = new RadioButton
            {
                Text = "브로드캐스트 검색 (UDP Discovery)",
                Location = new Point(20, 90),
                Size = new Size(350, 25),
                Checked = true
            };

            var rbNetworkScan = new RadioButton
            {
                Text = "네트워크 스캔 (HTTP Probe)",
                Location = new Point(20, 120),
                Size = new Size(350, 25)
            };

            var lblPort = new Label
            {
                Text = "UDP 포트:",
                Location = new Point(20, 150),
                Size = new Size(100, 20)
            };

            var txtPort = new TextBox
            {
                Location = new Point(120, 147),
                Size = new Size(100, 25),
                Text = "8101",
                PlaceholderText = "8101"
            };

            var lblSubnet = new Label
            {
                Text = "서브넷:",
                Location = new Point(20, 180),
                Size = new Size(200, 20)
            };

            var txtSubnet = new TextBox
            {
                Location = new Point(20, 205),
                Size = new Size(200, 25),
                Text = "192.168.0",
                PlaceholderText = "예: 192.168.0"
            };

            var btnOk = new Button
            {
                Text = "검색 시작",
                DialogResult = DialogResult.OK,
                Location = new Point(290, 250),
                Size = new Size(120, 30)
            };

            var btnCancel = new Button
            {
                Text = "취소",
                DialogResult = DialogResult.Cancel,
                Location = new Point(170, 250),
                Size = new Size(100, 30)
            };

            searchDialog.Controls.AddRange(new Control[] { 
                lblInterface, cmbInterface,
                rbBroadcast, rbNetworkScan,
                lblPort, txtPort,
                lblSubnet, txtSubnet, 
                btnOk, btnCancel 
            });
            searchDialog.AcceptButton = btnOk;
            searchDialog.CancelButton = btnCancel;

            // 옵션 변경 시 subnet 입력 활성화/비활성화
            rbBroadcast.CheckedChanged += (s, args) =>
            {
                lblPort.Visible = rbBroadcast.Checked;
                txtPort.Visible = rbBroadcast.Checked;
                lblSubnet.Visible = !rbBroadcast.Checked;
                txtSubnet.Visible = !rbBroadcast.Checked;
            };

            lblSubnet.Visible = false;
            txtSubnet.Visible = false;

            if (searchDialog.ShowDialog() != DialogResult.OK)
                return;

            btnAutoSearch.Enabled = false;
            dgvDiscoveredDevices.DataSource = null;

            // 선택된 로컬 IP 추출
            var selectedIpText = cmbInterface.SelectedItem?.ToString() ?? "";
            var selectedLocalIp = selectedIpText.Split(' ')[0];

            // UDP 포트 파싱
            if (!int.TryParse(txtPort.Text, out var discoveryPort))
            {
                discoveryPort = 8101;
            }

            if (rbBroadcast.Checked)
            {
                // Broadcast Search - 일반 API 호출 (완료 후 결과 표시)
                lblStatus.Text = $"브로드캐스트 검색 중... (소스 IP: {selectedLocalIp}, 포트: {discoveryPort})";

                var devices = new List<DiscoveredDevice>();

                // 중지 버튼을 포함한 진행 대화상자
                var progressForm = new Form
                {
                    Text = "브로드캐스트 검색 중",
                    Size = new Size(400, 150),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                var lblProgress = new Label
                {
                    Text = "단말기 검색 중... 잠시만 기다려주세요.",
                    Location = new Point(20, 20),
                    Size = new Size(360, 25),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };

                var lblDeviceCount = new Label
                {
                    Text = "발견된 디바이스: 0개",
                    Location = new Point(20, 50),
                    Size = new Size(360, 25),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Font = new Font(lblProgress.Font.FontFamily, 10, FontStyle.Bold)
                };

                var btnStop = new Button
                {
                    Text = "중지",
                    Location = new Point(150, 80),
                    Size = new Size(100, 30)
                };

                progressForm.Controls.AddRange(new Control[] { lblProgress, lblDeviceCount, btnStop });

                var cts = new CancellationTokenSource();
                btnStop.Click += (s, args) =>
                {
                    cts.Cancel();
                    progressForm.Close();
                };
                progressForm.FormClosing += (s, args) =>
                {
                    if (!cts.IsCancellationRequested)
                        cts.Cancel();
                };

                // 비동기 검색 실행
                var searchTask = Task.Run(async () =>
                {
                    try
                    {
                        var request = new
                        {
                            SearchType = "broadcast",
                            LocalIpAddress = selectedLocalIp,
                            DiscoveryPort = discoveryPort
                        };

                        var response = await _httpClient.PostAsJsonAsync("/api/Device/Search", request, cts.Token);

                        // 응답 디버깅
                        var responseText = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"검색 응답 원본: {responseText}");

                        var result = JsonSerializer.Deserialize<BrowserApiResponse<List<DiscoveredDevice>>>(responseText);

                        System.Diagnostics.Debug.WriteLine($"역직렬화 결과: result={result?.Result}, Code={result?.Code}, Data count={result?.Data?.Count}");

                        if (result?.Result == true && result.Data != null && result.Data.Count > 0)
                        {
                            devices.AddRange(result.Data);
                            System.Diagnostics.Debug.WriteLine($"디바이스 추가됨: {devices.Count}개");

                            // UI 스레드에서 카운트 업데이트
                            if (lblDeviceCount.InvokeRequired)
                            {
                                lblDeviceCount.Invoke((Action)(() =>
                                {
                                    lblDeviceCount.Text = $"발견된 디바이스: {devices.Count}개";
                                }));
                            }
                            else
                            {
                                lblDeviceCount.Text = $"발견된 디바이스: {devices.Count}개";
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"조건 실패: Result={result?.Result}, Data null={result?.Data == null}, Data count={result?.Data?.Count}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // 사용자가 중지함
                    }
                    catch (Exception ex)
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke((Action)(() =>
                            {
                                MessageBox.Show($"검색 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                        else
                        {
                            MessageBox.Show($"검색 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }, cts.Token);

                // 대화상자 표시
                progressForm.Shown += async (s, args) =>
                {
                    await searchTask;
                    if (!cts.IsCancellationRequested)
                        progressForm.Close();
                };

                progressForm.ShowDialog();

                cts.Dispose();

                // 결과 표시
                if (devices.Count > 0)
                {
                    dgvDiscoveredDevices.DataSource = devices;
                    dgvDiscoveredDevices.AutoResizeColumns();
                    lblStatus.Text = $"발견: {devices.Count}개 단말기";
                }
                else
                {
                    dgvDiscoveredDevices.DataSource = null;
                    lblStatus.Text = "브로드캐스트 검색 결과 없음";
                    MessageBox.Show("단말기를 찾을 수 없습니다. 네트워크 스캔을 시도해보세요.", "검색 결과",
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
            MessageBox.Show("Please select a device to register", "Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvDiscoveredDevices.SelectedRows[0];
            var ip = row.Cells["IpAddress"].Value?.ToString();
            var deviceSN = row.Cells["DeviceSN"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(deviceSN))
            {
                ShowError("Invalid device information");
                return;
            }

            // 이미 등록된 디바이스인지 확인
            List<DeviceInfo>? existingDevices = null;
            try
            {
                existingDevices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
                System.Diagnostics.Debug.WriteLine($"클라이언트: 기존 디바이스 목록 조회 성공 ({existingDevices?.Count ?? 0}개)");
                if (existingDevices != null)
                {
                    foreach (var dev in existingDevices)
                    {
                        System.Diagnostics.Debug.WriteLine($"  - {dev.SN} at {dev.IpAddress}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"기존 디바이스 목록 조회 실패: {ex.Message}");
                // 실패해도 계속 진행 (중복 체크 건너뛰기)
            }

            if (existingDevices != null)
            {
                System.Diagnostics.Debug.WriteLine($"클라이언트: 중복 체크 - 등록 시도 중인 디바이스: {deviceSN} at {ip}");

                var duplicateBySN = existingDevices.FirstOrDefault(d => d.SN == deviceSN);
                var duplicateByIP = existingDevices.FirstOrDefault(d => d.IpAddress == ip);

                if (duplicateBySN != null)
                {
                    System.Diagnostics.Debug.WriteLine($"클라이언트: SN 중복 발견: {duplicateBySN.SN} at {duplicateBySN.IpAddress}");
                }
                if (duplicateByIP != null)
                {
                    System.Diagnostics.Debug.WriteLine($"클라이언트: IP 중복 발견: {duplicateByIP.SN} at {duplicateByIP.IpAddress}");
                }

                var alreadyRegistered = existingDevices.Any(d => 
                    d.SN == deviceSN || d.IpAddress == ip);

                if (alreadyRegistered)
                {
                    MessageBox.Show(
                        $"이 디바이스는 이미 등록되어 있습니다.\n\n" +
                        $"IP 주소: {ip}\n" +
                        $"디바이스 SN: {deviceSN}",
                        "중복 등록",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    lblStatus.Text = "이미 등록된 디바이스";
                    return;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"클라이언트: 중복 없음, 등록 진행");
                }
            }

            // 확인 대화상자
            var result = MessageBox.Show(
                $"디바이스를 등록하시겠습니까?\n\n" +
                $"IP 주소: {ip}\n" +
                $"디바이스 SN: {deviceSN}\n\n" +
                $"디바이스는 HTTPv2 프로토콜을 통해 이 서버(포트 80)와 통신합니다.",
                "디바이스 등록",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                lblStatus.Text = "등록 취소됨";
                return;
            }

            lblStatus.Text = $"디바이스 등록 중: {deviceSN}...";

            // 단순히 IP 주소만 저장
            var registerRequest = new
            {
                IpAddress = ip,
                DeviceSN = deviceSN
            };

            var registerResponse = await _httpClient.PostAsJsonAsync("/api/Device/Register", registerRequest);

            string responseContent = "";
            try
            {
                responseContent = await registerResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"등록 응답: {responseContent}");
            }
            catch (Exception ex)
            {
                ShowError($"응답 읽기 실패: {ex.Message}");
                return;
            }

            // JSON 파싱 시도
            BrowserApiResponse<string>? registerResult = null;
            try
            {
                registerResult = JsonSerializer.Deserialize<BrowserApiResponse<string>>(responseContent);
                System.Diagnostics.Debug.WriteLine($"등록 파싱 결과: Result={registerResult?.Result}, ErrCode={registerResult?.ErrCode}, Error={registerResult?.Error}");
            }
            catch (Exception ex)
            {
                ShowError($"등록 응답 JSON 파싱 실패:\n{ex.Message}\n\n응답 내용:\n{responseContent}");
                return;
            }

            if (registerResult == null)
            {
                ShowError($"등록 응답이 null입니다.\n\n응답 내용:\n{responseContent}");
                return;
            }

            if (registerResult.Result == true)
            {
                lblStatus.Text = $"디바이스 등록 완료: {deviceSN}";
                MessageBox.Show(
                    $"디바이스가 성공적으로 등록되었습니다.\n\n" +
                    $"IP: {ip}\n" +
                    $"SN: {deviceSN}\n\n" +
                    $"디바이스는 HTTPv2 프로토콜에 따라 자동으로 연결됩니다.", 
                    "등록 완료", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                await RefreshDevices();
                await RefreshSystemInfo();

                // Clear discovered devices after successful registration
                dgvDiscoveredDevices.DataSource = null;
            }
            else
            {
                ShowError($"등록 실패: {registerResult?.Error ?? "Unknown error"} (Code: {registerResult?.ErrCode})");
            }
        }
        catch (Exception ex)
        {
            ShowError($"등록 실패: {ex.Message}");
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
        using var form = new PersonForm(_httpClient);
        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Prepare PersonInfo with Base64 photo
                var person = form.Person;

                // Convert PhotoData to Base64 and store in Photo field for JSON transmission
                if (person.PhotoData != null && person.PhotoData.Length > 0)
                {
                    person.Photo = Convert.ToBase64String(person.PhotoData);
                }
                else
                {
                    person.Photo = null; // Clear Photo if no photo
                }

                var response = await _httpClient.PostAsJsonAsync("/api/People/New", person);
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

    private async void btnEditPerson_Click(object sender, EventArgs e)
    {
        if (dgvPersonnel.SelectedRows.Count == 0)
        {
            MessageBox.Show("수정할 사용자를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvPersonnel.SelectedRows[0];
            var userID = row.Cells["사용자번호"].Value?.ToString();

            // Fetch full person data from backend
            var getResponse = await _httpClient.PostAsJsonAsync("/api/People/GetDetail", new { UserID = userID });
            var getResult = await getResponse.Content.ReadFromJsonAsync<BrowserApiResponse<PersonInfo>>();

            if (getResult?.Code != 0 || getResult.Content == null)
            {
                ShowError($"사용자 정보 조회 실패: {getResult?.Msg}");
                return;
            }

            var person = getResult.Content;
            using var form = new PersonForm(_httpClient);

            // Set initial values including photo and password
            form.SetInitialValues(person.UserID, person.Name, person.Photo, person.Password);

            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Convert PhotoData to Base64 for transmission
                    if (form.Person.PhotoData != null && form.Person.PhotoData.Length > 0)
                    {
                        form.Person.Photo = Convert.ToBase64String(form.Person.PhotoData);
                    }
                    else
                    {
                        // Keep existing photo if not changed, or set to null if explicitly removed
                        if (string.IsNullOrWhiteSpace(form.Person.Photo))
                        {
                            form.Person.Photo = null;
                        }
                    }

                    // 서버에 수정된 정보 저장
                    var response = await _httpClient.PostAsJsonAsync("/api/People/Update", form.Person);
                    var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

                    if (result?.Code == 0)
                    {
                        lblStatus.Text = "사용자 정보가 수정되었습니다";
                        await RefreshPersonnel();
                        await RefreshSystemInfo();
                    }
                    else
                    {
                        ShowError($"사용자 수정 실패: {result?.Msg}");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"사용자 수정 중 오류 발생: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            ShowError($"사용자 수정 실패: {ex.Message}");
        }
    }

    private async void btnDeletePerson_Click(object sender, EventArgs e)
    {
        if (dgvPersonnel.SelectedRows.Count == 0)
        {
            MessageBox.Show("삭제할 사용자를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvPersonnel.SelectedRows[0];
            var userID = row.Cells["사용자번호"].Value?.ToString();
            var userName = row.Cells["사용자명"].Value?.ToString();

            var result = MessageBox.Show(
                $"{userName} (사용자번호: {userID})를 삭제하시겠습니까?",
                "사용자 삭제 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var deleteRequest = new { UserID = userID };
                var response = await _httpClient.PostAsJsonAsync("/api/People/Delete", deleteRequest);
                var apiResult = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

                if (apiResult?.Code == 0)
                {
                    lblStatus.Text = "사용자가 삭제되었습니다";
                    await RefreshPersonnel();
                    await RefreshSystemInfo();
                }
                else
                {
                    ShowError($"사용자 삭제 실패: {apiResult?.Msg}");
                }
            }
        }
        catch (Exception ex)
        {
            ShowError($"사용자 삭제 실패: {ex.Message}");
        }
    }

    private void btnRealTimeView_Click(object sender, EventArgs e)
    {
        try
        {
            var form = new Form
            {
                Text = "실시간 출입 보기",
                Size = new System.Drawing.Size(900, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            var checkedListBox = new CheckedListBox
            {
                Dock = DockStyle.Top,
                Height = 150
            };

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgv.Columns.Add("시간", "시간");
            dgv.Columns.Add("사용자번호", "사용자번호");
            dgv.Columns.Add("사용자명", "사용자명");
            dgv.Columns.Add("단말기명", "단말기명");
            dgv.Columns.Add("이벤트", "이벤트");

            foreach (DataGridViewRow row in dgvDevices.Rows)
            {
                var deviceName = row.Cells["DeviceName"].Value?.ToString();
                if (!string.IsNullOrEmpty(deviceName))
                {
                    checkedListBox.Items.Add(deviceName);
                }
            }

            var panel = new Panel { Dock = DockStyle.Fill };
            panel.Controls.Add(dgv);
            panel.Controls.Add(checkedListBox);

            form.Controls.Add(panel);
            form.ShowDialog();
        }
        catch (Exception ex)
        {
            ShowError($"실시간 보기 열기 실패: {ex.Message}");
        }
    }

    private void btnRefreshDevices_Click(object sender, EventArgs e) => _ = RefreshDevices();
    private void btnRefreshDepartments_Click(object sender, EventArgs e) => _ = RefreshDepartments();

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

    // Device Context Menu Event Handlers (단말기별 설정)
    private void MenuDeviceNetworkSettings_Click(object? sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("단말기를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var row = dgvDevices.SelectedRows[0];
        var sn = row.Cells["SN"].Value?.ToString();
        var deviceName = row.Cells["DeviceName"].Value?.ToString() ?? "Unknown";

        if (string.IsNullOrWhiteSpace(sn))
        {
            ShowError("Invalid device selection");
            return;
        }

        using var form = new NetworkSettingsForm(_httpClient, sn, deviceName);
        form.ShowDialog();
    }

    private void MenuDeviceAccessControlSettings_Click(object? sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("단말기를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var row = dgvDevices.SelectedRows[0];
        var sn = row.Cells["SN"].Value?.ToString();
        var deviceName = row.Cells["DeviceName"].Value?.ToString() ?? "Unknown";

        if (string.IsNullOrWhiteSpace(sn))
        {
            ShowError("Invalid device selection");
            return;
        }

        using var form = new AccessControlSettingsForm(_httpClient, sn, deviceName);
        form.ShowDialog();
    }

    private void MenuDeviceAlarmSettings_Click(object? sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("단말기를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var row = dgvDevices.SelectedRows[0];
        var sn = row.Cells["SN"].Value?.ToString();
        var deviceName = row.Cells["DeviceName"].Value?.ToString() ?? "Unknown";

        if (string.IsNullOrWhiteSpace(sn))
        {
            ShowError("Invalid device selection");
            return;
        }

        using var form = new AlarmSettingsForm(_httpClient, sn, deviceName);
        form.ShowDialog();
    }

    private void btnDeviceSettings_Click(object? sender, EventArgs e)
    {
        if (dgvDevices.SelectedRows.Count == 0)
        {
            MessageBox.Show("단말기를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var row = dgvDevices.SelectedRows[0];
        var sn = row.Cells["SN"].Value?.ToString();
        var deviceName = row.Cells["DeviceName"].Value?.ToString() ?? "Unknown";

        if (string.IsNullOrWhiteSpace(sn))
        {
            ShowError("Invalid device selection");
            return;
        }

        // 설정 메뉴를 표시
        var settingsMenu = new ContextMenuStrip();

        var networkItem = new ToolStripMenuItem("네트워크 설정");
        networkItem.Click += (s, args) => {
            using var form = new NetworkSettingsForm(_httpClient, sn, deviceName);
            form.ShowDialog();
        };

        var accessControlItem = new ToolStripMenuItem("출입 제어 설정");
        accessControlItem.Click += (s, args) => {
            using var form = new AccessControlSettingsForm(_httpClient, sn, deviceName);
            form.ShowDialog();
        };

        var alarmItem = new ToolStripMenuItem("알람 설정");
        alarmItem.Click += (s, args) => {
            using var form = new AlarmSettingsForm(_httpClient, sn, deviceName);
            form.ShowDialog();
        };

        settingsMenu.Items.AddRange(new ToolStripItem[] 
        {
            networkItem,
            accessControlItem,
            alarmItem
        });

        // 버튼 위치에서 메뉴 표시
        var btn = sender as Button;
        if (btn != null)
        {
            settingsMenu.Show(btn, new Point(0, btn.Height));
        }
    }
}
