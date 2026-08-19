using FaceDeviceDesktopClient.Services;
using System.Net.Http.Json;
using System.ComponentModel;
using System.Text.Json;
using FaceDeviceDesktopClient.Forms;

namespace FaceDeviceDesktopClient;

public partial class MainForm : Form
{
    private readonly HttpClient _httpClient;
    private string _serverUrl = "http://localhost:8100";

    // Personnel grid sorting
    private List<PersonInfo> _personnelList = new();
    private List<PersonInfo> _sortedPersonnelList = new();
    private string _personnelSortColumn = string.Empty;
    private ListSortDirection _personnelSortDirection = ListSortDirection.Ascending;

    // 단말기 고정 순번: SN → 순번 (한 번 부여된 번호는 변경되지 않음)
    private readonly Dictionary<string, int> _deviceRowNumbers =
        new(StringComparer.OrdinalIgnoreCase);
    private int _deviceRowCounter = 0;

    public MainForm()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri(_serverUrl) };
        SetupDeviceGrid();
        SetupDiscoveredDevicesGrid();
        SetupPersonnelGrid();
        SetupAttendanceGrid();
        SetupDeviceContextMenu();
        tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
        this.Activated += (_, _) =>
        {
            if (tabControl.SelectedTab == tabDashboard)
                _ = RefreshSystemInfo();
        };
        LoadInitialData();
    }

    private void tabControl_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (tabControl.SelectedTab == tabDashboard)
            _ = RefreshSystemInfo();
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
        dgvDevices.AutoGenerateColumns = false;
        dgvDevices.AllowUserToAddRows = false;
        dgvDevices.ReadOnly = false;
        dgvDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvDevices.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "Selected",
            HeaderText = "선택",
            Width = 55,
            ReadOnly = false
        });

        dgvDevices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "No",
            HeaderText = "순번",
            DataPropertyName = "",
            Width = 50,
            ReadOnly = true
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
            Width = 80,
            ReadOnly = true
        });

        // 체크박스 클릭 처리: CellContentClick + CommitEdit로 즉시 반영
        dgvDevices.CellContentClick += (s, e) =>
        {
            if (e.RowIndex < 0) return;
            if (dgvDevices.Columns["Selected"] is not { } selectedCol || e.ColumnIndex != selectedCol.Index) return;
            dgvDevices.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };

        // 상태 컬럼 색상 표시 + 최신 상태 재계산
        dgvDevices.CellFormatting += (s, e) =>
        {
            if (e.RowIndex < 0) return;
            if (dgvDevices.Columns["Status"] is { } statusCol && e.ColumnIndex == statusCol.Index)
            {
                // 바인딩된 DeviceInfo에서 실시간 재계산
                if (dgvDevices.Rows[e.RowIndex].DataBoundItem is DeviceInfo di)
                    e.Value = di.Status;
                var val = e.Value?.ToString();
                if (val == "정상")
                {
                    e.CellStyle.ForeColor = System.Drawing.Color.Green;
                    e.CellStyle.Font = new System.Drawing.Font(dgvDevices.Font, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = System.Drawing.Color.Red;
                    e.CellStyle.Font = new System.Drawing.Font(dgvDevices.Font, System.Drawing.FontStyle.Bold);
                }
                e.FormattingApplied = true;
            }
        };

        dgvDevices.CellDoubleClick += DgvDevices_CellDoubleClick;
        dgvDevices.RowPostPaint += DgvDevices_RowPostPaint;
    }

    private void SetupPersonnelGrid()
    {
        dgvPersonnel.AutoGenerateColumns = false;
        dgvPersonnel.AllowUserToAddRows = false;
        dgvPersonnel.ReadOnly = false;
        dgvPersonnel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPersonnel.ColumnHeaderMouseClick += DgvPersonnel_ColumnHeaderMouseClick;

        dgvPersonnel.Columns.Clear();

        dgvPersonnel.Columns.Add(new DataGridViewCheckBoxColumn { Name = "ColSelect",      HeaderText = "선택",         Width = 55,  SortMode = DataGridViewColumnSortMode.NotSortable, ReadOnly = false });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDong",        HeaderText = "동",           Width = 70,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColHo",          HeaderText = "호",           Width = 70,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMember",      HeaderText = "멤버#",         Width = 60,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName",        HeaderText = "사용자명",     Width = 150, SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCard",        HeaderText = "카드",         Width = 70,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPassword",    HeaderText = "비밀번호",     Width = 80,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColFingerprint", HeaderText = "지문",         Width = 55,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPalmvein",    HeaderText = "손바닥",       Width = 55,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });
        dgvPersonnel.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPhoto",       HeaderText = "얼굴",         Width = 55,  SortMode = DataGridViewColumnSortMode.Programmatic, ReadOnly = true });

        dgvPersonnel.CellFormatting += (sender, e) =>
        {
            if (dgvPersonnel.Columns["ColPhoto"] is { } photoCol && e.ColumnIndex == photoCol.Index && e.Value != null)
            {
                var pv = e.Value.ToString();
                e.Value = string.IsNullOrWhiteSpace(pv) ? "X" : "O";
                e.FormattingApplied = true;
            }
            else if (dgvPersonnel.Columns["ColPassword"] is { } pwCol && e.ColumnIndex == pwCol.Index && e.Value != null)
            {
                var pw = e.Value.ToString();
                if (!string.IsNullOrWhiteSpace(pw))
                {
                    e.Value = new string('●', Math.Min(pw.Length, 8));
                    e.FormattingApplied = true;
                }
            }
            else if (dgvPersonnel.Columns["ColCard"] is { } cardCol && e.ColumnIndex == cardCol.Index && e.Value != null)
            {
                var cv = e.Value.ToString();
                e.Value = (string.IsNullOrWhiteSpace(cv) || cv == "0") ? "X" : "O";
                e.FormattingApplied = true;
            }
            else if (dgvPersonnel.Columns["ColFingerprint"] is { } fpCol && e.ColumnIndex == fpCol.Index && e.Value != null)
            {
                e.Value = e.Value.ToString() == "0" ? "X" : "O";
                e.FormattingApplied = true;
            }
            else if (dgvPersonnel.Columns["ColPalmvein"] is { } pvCol && e.ColumnIndex == pvCol.Index && e.Value != null)
            {
                e.Value = e.Value.ToString() == "0" ? "X" : "O";
                e.FormattingApplied = true;
            }
        };

        dgvPersonnel.CellDoubleClick += (sender, e) =>
        {
            if (e.RowIndex >= 0)
                btnEditPerson_Click(sender!, EventArgs.Empty);
        };

        // 동 입력 시 호 활성화/비활성화
        txtFilterDong.TextChanged += (s, e) =>
        {
            bool hasDong = !string.IsNullOrWhiteSpace(txtFilterDong.Text);
            txtFilterHo.Enabled = hasDong;
            lblFilterHo.Enabled = hasDong;
            if (!hasDong)
                txtFilterHo.Text = "";
            btnSelectByFilter.Text = "선택";
        };

        txtFilterHo.TextChanged += (s, e) =>
        {
            btnSelectByFilter.Text = "선택";
        };
    }

    private void SetupAttendanceGrid()
    {
        dgvAttendance.AutoGenerateColumns = false;
        dgvAttendance.AllowUserToAddRows = false;
        dgvAttendance.ReadOnly = true;
        dgvAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvAttendance.Columns.Clear();

        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn { Name = "AttTime",   HeaderText = "시간",     DataPropertyName = "RecordTime",  Width = 160 });
        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn { Name = "AttDong",   HeaderText = "동",     DataPropertyName = "UserID",      Width = 65  });
        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn { Name = "AttHo",     HeaderText = "호",       DataPropertyName = "UserID",      Width = 65  });
        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn { Name = "AttMember", HeaderText = "멤버#",   DataPropertyName = "UserID",      Width = 55  });
        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn { Name = "AttName",   HeaderText = "사용자명", DataPropertyName = "UserName",    Width = 130 });
        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn { Name = "AttDevice", HeaderText = "단말기",   DataPropertyName = "DeviceSN",    Width = 130 });
        dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn { Name = "AttEvent",  HeaderText = "이벤트",    DataPropertyName = "RecordType",  Width = 100 });

        dgvAttendance.CellFormatting += DgvAttendance_CellFormatting;

        // Mutual exclusion: only one date range endpoint active at a time
        dtpAttendanceStart.ValueChanged += (s, e) => { if (dtpAttendanceStart.Checked) dtpAttendanceEnd.Checked = false; };
        dtpAttendanceEnd.ValueChanged   += (s, e) => { if (dtpAttendanceEnd.Checked)   dtpAttendanceStart.Checked = false; };
    }

    private void DgvAttendance_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.Value == null) return;
        int dongIdx   = dgvAttendance.Columns["AttDong"]!.Index;
        int hoIdx     = dgvAttendance.Columns["AttHo"]!.Index;
        int memberIdx = dgvAttendance.Columns["AttMember"]!.Index;
        int eventIdx  = dgvAttendance.Columns["AttEvent"]!.Index;
        if ((e.ColumnIndex == dongIdx || e.ColumnIndex == hoIdx || e.ColumnIndex == memberIdx)
            && long.TryParse(e.Value.ToString(), out long id))
        {
            e.Value = e.ColumnIndex == dongIdx   ? (id / 1_000_000L).ToString()
                    : e.ColumnIndex == hoIdx     ? ((id / 100L) % 10_000L).ToString()
                    :                              (id % 100L).ToString();
            e.FormattingApplied = true;
        }
        else if (e.ColumnIndex == eventIdx)
        {
            if (int.TryParse(e.Value.ToString(), out int recType))
            {
                e.Value = RecordTypeToLabel(recType);
                e.FormattingApplied = true;
            }
        }
    }

    private void DgvDevices_RowPostPaint(object? sender, DataGridViewRowPostPaintEventArgs e)
    {
        // Display fixed row number in "No." column (based on SN, not row index)
        var grid = sender as DataGridView;
        if (grid == null) return;

        // 해당 행의 SN으로 고정 순번 조회
        string? sn = null;
        if (e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            sn = grid.Rows[e.RowIndex].Cells["SN"].Value?.ToString();
        string rowLabel = (sn != null && _deviceRowNumbers.TryGetValue(sn, out var fixedNo))
            ? fixedNo.ToString()
            : (e.RowIndex + 1).ToString();

        // Calculate the X offset of the "No" column
        int xOffset = grid.RowHeadersWidth;
        int noColIndex = grid.Columns["No"]!.Index;
        for (int i = 0; i < noColIndex; i++)
            if (grid.Columns[i].Visible)
                xOffset += grid.Columns[i].Width;

        var centerFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        var noBounds = new Rectangle(xOffset, e.RowBounds.Top, grid.Columns["No"]!.Width, e.RowBounds.Height);
        e.Graphics.DrawString(rowLabel, grid.Font, SystemBrushes.ControlText, noBounds, centerFormat);
    }

    private void DgvDevices_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        // 선택 체크박스 열 더블클릭은 무시
        if (dgvDevices.Columns["Selected"] is { } selCol && e.ColumnIndex == selCol.Index) return;

        try
        {
            var device = dgvDevices.Rows[e.RowIndex].DataBoundItem as DeviceInfo;
            if (device == null) { ShowError("Invalid device data"); return; }

            using var form = new DeviceSettingsForm(device, _httpClient);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = RefreshDevices();
                _ = RefreshSystemInfo();
            }
        }
        catch (Exception ex)
        {
            ShowError($"단말기 설정 열기 실패: {ex.Message}");
        }
    }

    private void SetupDeviceContextMenu()
    {
        var contextMenu = new ContextMenuStrip();

        var menuRemoteControl = new ToolStripMenuItem("수정");
        menuRemoteControl.Click += MenuRemoteControl_Click;

        var menuRefresh = new ToolStripMenuItem("새로 고침");
        menuRefresh.Click += async (s, e) => await RefreshDevices();

        var menuSeparator = new ToolStripSeparator();

        var menuDelete = new ToolStripMenuItem("제거");
        menuDelete.Click += MenuDeleteDevice_Click;
        menuDelete.ForeColor = System.Drawing.Color.DarkRed;

        contextMenu.Items.AddRange(new ToolStripItem[] { menuRemoteControl, menuRefresh, menuSeparator, menuDelete });
        dgvDevices.ContextMenuStrip = contextMenu;
    }

    private void MenuRemoteControl_Click(object? sender, EventArgs e)
    {
        var row = dgvDevices.SelectedRows.Count > 0 ? dgvDevices.SelectedRows[0] : null;
        if (row == null) { MessageBox.Show("단말기를 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        try
        {
            var device = row.DataBoundItem as DeviceInfo;
            if (device == null) { ShowError("Invalid device selection"); return; }
            using var form = new DeviceSettingsForm(device, _httpClient);
            if (form.ShowDialog() == DialogResult.OK) { _ = RefreshDevices(); _ = RefreshSystemInfo(); }
        }
        catch (Exception ex) { ShowError($"단말기 설정 열기 실패: {ex.Message}"); }
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

            try
            {
                var settings = await _httpClient.GetFromJsonAsync<System.Text.Json.JsonElement>("/admin/settings");
                if (settings.ValueKind == System.Text.Json.JsonValueKind.Object)
                {
                    int months = 12;
                    if (settings.TryGetProperty("RecordRetentionMonths", out var p))
                        months = p.GetInt32();
                    else if (settings.TryGetProperty("recordRetentionMonths", out var p2))
                        months = p2.GetInt32();
                    if (months < 0) months = 0;
                    if (months > 120) months = 120;
                    numRetentionMonths.Value = months;

                    var urls = new List<string>();
                    if (settings.TryGetProperty("LocalUrls", out var lu) || settings.TryGetProperty("localUrls", out lu))
                    {
                        foreach (var item in lu.EnumerateArray())
                        {
                            var u = item.GetString();
                            if (!string.IsNullOrWhiteSpace(u)) urls.Add(u);
                        }
                    }
                    string? cur = null;
                    if (settings.TryGetProperty("ServerUrl", out var su)) cur = su.GetString();
                    else if (settings.TryGetProperty("serverUrl", out var su2)) cur = su2.GetString();

                    cmbServerUrl.Items.Clear();
                    foreach (var u in urls.Distinct())
                        cmbServerUrl.Items.Add(u);
                    if (!string.IsNullOrWhiteSpace(cur) && !cmbServerUrl.Items.Contains(cur))
                        cmbServerUrl.Items.Add(cur);
                    if (!string.IsNullOrWhiteSpace(cur))
                        cmbServerUrl.Text = cur;
                }
            }
            catch { }
        }
        catch (Exception ex)
        {
            ShowError($"Failed to refresh system info: {ex.Message}");
        }
    }

    private async void btnSaveRetention_Click(object? sender, EventArgs e)
    {
        try
        {
            int months = (int)numRetentionMonths.Value;
            var url = (cmbServerUrl.Text ?? "").Trim();
            var resp = await _httpClient.PostAsJsonAsync("/admin/settings", new
            {
                RecordRetentionMonths = months,
                ServerUrl = url
            });
            if (!resp.IsSuccessStatusCode)
            {
                ShowError($"설정 저장 실패: HTTP {resp.StatusCode}");
                return;
            }
            if (!string.IsNullOrWhiteSpace(url))
                _serverUrl = url.TrimEnd('/');
            lblStatus.Text = "설정이 XML에 저장되었습니다.";
            MessageBox.Show(
                $"서버 URL: {(string.IsNullOrWhiteSpace(url) ? "(유지)" : url)}\n" +
                (months == 0 ? "출입기록 자동 삭제 안 함" : $"출입기록 {months}개월 이후 자동 삭제"),
                "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError($"보관기간 저장 실패: {ex.Message}");
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

    private System.Windows.Forms.Timer? _deviceStatusTimer;

    private void UpdateDeviceGrid(List<DeviceInfo> devices)
    {
        // 현재 체크된 SN 목록 보존
        var checkedSNs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (DataGridViewRow row in dgvDevices.Rows)
            if (row.Cells["Selected"].Value is true)
            {
                var sn = row.Cells["SN"].Value?.ToString();
                if (!string.IsNullOrEmpty(sn)) checkedSNs.Add(sn);
            }

        // 현재 스크롤 위치와 선택 SN 보존
        int firstVisibleRow = dgvDevices.FirstDisplayedScrollingRowIndex;
        string? selectedSN = null;
        if (dgvDevices.SelectedRows.Count > 0)
            selectedSN = dgvDevices.SelectedRows[0].Cells["SN"].Value?.ToString();

        // Clear existing data (but keep columns)
        dgvDevices.DataSource = null;

        if (devices.Count > 0)
        {
            // 새로 나타난 SN에 순번 부여 (기존 SN은 유지)
            foreach (var d in devices)
                if (!_deviceRowNumbers.ContainsKey(d.SN))
                    _deviceRowNumbers[d.SN] = ++_deviceRowCounter;

            // 순번 기준으로 정렬하여 표시 순서를 고정
            devices.Sort((a, b) =>
            {
                int na = _deviceRowNumbers.TryGetValue(a.SN, out var va) ? va : int.MaxValue;
                int nb = _deviceRowNumbers.TryGetValue(b.SN, out var vb) ? vb : int.MaxValue;
                return na.CompareTo(nb);
            });

            var bindingList = new System.ComponentModel.BindingList<DeviceInfo>(devices);
            dgvDevices.DataSource = bindingList;
            lblStatus.Text = $"Loaded {devices.Count} device(s)";

            // 체크박스 상태 복원 (미선택 행도 명시적으로 false 지정하여 null 방지)
            foreach (DataGridViewRow row in dgvDevices.Rows)
            {
                var sn = row.Cells["SN"].Value?.ToString();
                row.Cells["Selected"].Value = !string.IsNullOrEmpty(sn) && checkedSNs.Contains(sn);
            }

            // 선택 행 복원 (자동 선택된 행 먼저 해제 후 복원)
            dgvDevices.ClearSelection();
            if (selectedSN != null)
            {
                foreach (DataGridViewRow row in dgvDevices.Rows)
                    if (string.Equals(row.Cells["SN"].Value?.ToString(), selectedSN, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        break;
                    }
            }

            // 스크롤 위치 복원
            if (firstVisibleRow >= 0 && firstVisibleRow < dgvDevices.RowCount)
                dgvDevices.FirstDisplayedScrollingRowIndex = firstVisibleRow;

            // 상태 컬럼 주기 갱신 타이머 (20초마다 Refresh)
            _deviceStatusTimer?.Stop();
            _deviceStatusTimer?.Dispose();
            _deviceStatusTimer = new System.Windows.Forms.Timer { Interval = 20_000 };
            _deviceStatusTimer.Tick += async (s, e) => await RefreshDevices();
            _deviceStatusTimer.Start();
        }
        else
        {
            _deviceStatusTimer?.Stop();
            lblStatus.Text = "No devices installed yet";
        }

        // Refresh device filter combo in event search
        var prevSel = cmbAttDevice.SelectedItem?.ToString();
        cmbAttDevice.Items.Clear();
        cmbAttDevice.Items.Add("전체 단말기");
        foreach (var d in devices)
        {
            var label = string.IsNullOrWhiteSpace(d.DeviceName) ? d.SN : d.DeviceName;
            cmbAttDevice.Items.Add(label);
        }
        cmbAttDevice.SelectedIndex = Math.Max(0, cmbAttDevice.Items.IndexOf(prevSel ?? "전체 단말기"));
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
            _personnelList = personnel ?? new List<PersonInfo>();
            ApplyPersonnelSort();

            Dictionary<string, int>? assignments = null;
            try { assignments = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>("/admin/people/device-assignments"); }
            catch { }

            PopulatePersonnelComputedCells(assignments);
        }
        catch (Exception ex)
        {
            ShowError($"Failed to refresh personnel: {ex.Message}");
        }
    }

    private void PopulatePersonnelComputedCells(Dictionary<string, int>? assignments)
    {
        dgvPersonnel.Rows.Clear();
        foreach (var person in _sortedPersonnelList)
        {
            long idNum = 0;
            bool parsed = long.TryParse(person.UserID, out idNum);
            string dong   = parsed ? (idNum / 1_000_000L).ToString() : person.UserID;
            string ho     = parsed ? ((idNum / 100L) % 10_000L).ToString() : "";
            string member = parsed ? (idNum % 100L).ToString() : "";
            int rowIdx = dgvPersonnel.Rows.Add(
                false,
                dong, ho, member,
                person.Name ?? "",
                person.CardNum ?? "",
                person.Password ?? "",
                person.Fingerprints?.Count.ToString() ?? "0",
                person.Palmveins?.Count.ToString() ?? "0",
                person.Photo ?? "");
            dgvPersonnel.Rows[rowIdx].Tag = person;
        }
    }

    private void ApplyPersonnelSort()
    {
        IEnumerable<PersonInfo> sorted = _personnelList;
        if (!string.IsNullOrEmpty(_personnelSortColumn))
        {
            bool asc = _personnelSortDirection == System.ComponentModel.ListSortDirection.Ascending;
            sorted = _personnelSortColumn switch
            {
                "ColDong" => asc
                    ? _personnelList.OrderBy(p => long.TryParse(p.UserID, out long v) ? v / 1_000_000L : long.MaxValue)
                    : _personnelList.OrderByDescending(p => long.TryParse(p.UserID, out long v) ? v / 1_000_000L : long.MinValue),
                "ColHo" => asc
                    ? _personnelList.OrderBy(p => long.TryParse(p.UserID, out long v) ? (v / 100L) % 10_000L : long.MaxValue)
                    : _personnelList.OrderByDescending(p => long.TryParse(p.UserID, out long v) ? (v / 100L) % 10_000L : long.MinValue),
                "ColMember" => asc
                    ? _personnelList.OrderBy(p => long.TryParse(p.UserID, out long v) ? v % 100L : long.MaxValue)
                    : _personnelList.OrderByDescending(p => long.TryParse(p.UserID, out long v) ? v % 100L : long.MinValue),
                "ColName" => asc
                    ? _personnelList.OrderBy(p => p.Name)
                    : _personnelList.OrderByDescending(p => p.Name),
                _ => sorted
            };
        }
        _sortedPersonnelList = sorted.ToList();
    }

    private void DgvPersonnel_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        var col = dgvPersonnel.Columns[e.ColumnIndex];
        if (col.Name == _personnelSortColumn)
            _personnelSortDirection = _personnelSortDirection == System.ComponentModel.ListSortDirection.Ascending
                ? System.ComponentModel.ListSortDirection.Descending
                : System.ComponentModel.ListSortDirection.Ascending;
        else
        {
            _personnelSortColumn = col.Name;
            _personnelSortDirection = System.ComponentModel.ListSortDirection.Ascending;
        }
        foreach (DataGridViewColumn c in dgvPersonnel.Columns)
            c.HeaderCell.SortGlyphDirection = SortOrder.None;
        col.HeaderCell.SortGlyphDirection = _personnelSortDirection == System.ComponentModel.ListSortDirection.Ascending
            ? SortOrder.Ascending : SortOrder.Descending;
        ApplyPersonnelSort();
        PopulatePersonnelComputedCells(null);
    }

    private async Task RefreshAttendance()
    {
        try
        {
            // Build UserID search from dong/ho/member using range logic
            var (userIDExact, userIDMin, userIDMax) = BuildAttUserIDFilter(
                txtAttDong.Text.Trim(), txtAttHo.Text.Trim(), txtAttMember.Text.Trim());

            // Resolve selected device SN
            string? deviceSnFilter = null;
            if (cmbAttDevice.SelectedIndex > 0)
            {
                var selectedLabel = cmbAttDevice.SelectedItem?.ToString() ?? "";
                foreach (DataGridViewRow row in dgvDevices.Rows)
                {
                    var dname = row.Cells["DeviceName"].Value?.ToString() ?? "";
                    var sn    = row.Cells["SN"].Value?.ToString() ?? "";
                    var label = string.IsNullOrWhiteSpace(dname) ? sn : dname;
                    if (label == selectedLabel) { deviceSnFilter = sn; break; }
                }
            }

            var request = new AttendanceSearchRequest
            {
                UserID    = userIDExact,
                UserIDMin = userIDMin,
                UserIDMax = userIDMax,
                UserName  = txtAttendanceUserName.Text,
                DeviceSN  = deviceSnFilter,
                StartTime = dtpAttendanceStart.Checked ? dtpAttendanceStart.Value : null,
                EndTime   = dtpAttendanceEnd.Checked ? dtpAttendanceEnd.Value : null,
                PageIndex = 1,
                PageSize  = 1000
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
            if (form.AlreadySaved)
            {
                // Person was already saved via "저장 및 선택한 단말기로 전송"
                lblStatus.Text = "Personnel added and device transfer requested";
                // Refresh to update UI - assignment counts will update after device keepalive
                await RefreshPersonnel();
                await RefreshSystemInfo();
            }
            else
            {
                // User clicked "저장" button - need to save now
                try
                {
                    var person = form.Person;

                    // PersonForm.BuildPersonInfo()에서 PhotoData → Photo(Base64) 변환이 이미 완료됨
                    // PhotoData가 있는데 Photo가 아직 변환 안 됐을 경우만 보정
                    if (person.PhotoData != null && person.PhotoData.Length > 0 &&
                        string.IsNullOrWhiteSpace(person.Photo))
                    {
                        person.Photo = Convert.ToBase64String(person.PhotoData);
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
            var userID = (row.Tag as PersonInfo)?.UserID;

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

            // Set initial values including photo, password, card, fingerprints, palmveins
            form.SetInitialValues(person);

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.AlreadySaved)
                {
                    // Person was already saved via "저장 및 선택한 단말기로 전송"
                    lblStatus.Text = "Personnel updated and device transfer requested";
                    // Refresh to update UI - assignment counts will update after device keepalive
                    await RefreshPersonnel();
                    await RefreshSystemInfo();
                }
                else
                {
                    // User clicked "저장" button - need to save now
                    try
                    {
                        // Photo가 단말기 내부 경로("/data/user_pic/xxx.jpg" 형태)인 경우만 null 처리
                        // Base64 JPEG는 "/9j/"로 시작하므로 확장자 포함 여부로 단말기 경로 판별
                        if (!string.IsNullOrWhiteSpace(form.Person.Photo))
                        {
                            bool looksLikePath = form.Person.Photo.Contains(":\\") ||
                                                 form.Person.Photo.Contains(":/") ||
                                                 (form.Person.Photo.StartsWith("/") &&
                                                  System.IO.Path.HasExtension(form.Person.Photo));
                            if (looksLikePath)
                                form.Person.Photo = null;
                        }

                        System.Diagnostics.Debug.WriteLine($"[Update] Photo length={form.Person.Photo?.Length ?? 0}");
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
        }
        catch (Exception ex)
        {
            ShowError($"사용자 수정 실패: {ex.Message}");
        }
    }

    private async void btnDeletePerson_Click(object sender, EventArgs e)
    {
        // ColSelect 체크된 행 우선, 없으면 SelectedRows 전체 사용
        var targetRows = dgvPersonnel.Rows.Cast<DataGridViewRow>()
            .Where(r => r.Cells["ColSelect"].Value is true)
            .ToList();
        if (targetRows.Count == 0)
            targetRows = dgvPersonnel.SelectedRows.Cast<DataGridViewRow>().ToList();

        if (targetRows.Count == 0)
        {
            MessageBox.Show("삭제할 사용자를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var names = string.Join(", ", targetRows
                .Select(r => (r.Tag as PersonInfo)?.Name)
                .Where(n => n != null));
            var result = MessageBox.Show(
                $"{names} ({targetRows.Count}명)를 삭제하시겠습니까?",
                "사용자 삭제 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            int successCount = 0;
            var errors = new List<string>();
            foreach (var row in targetRows)
            {
                var boundPerson = row.Tag as PersonInfo;
                var userID = boundPerson?.UserID;
                if (string.IsNullOrEmpty(userID)) continue;
                try
                {
                    var deleteRequest = new { UserID = userID };
                    var response = await _httpClient.PostAsJsonAsync("/api/People/Delete", deleteRequest);
                    var apiResult = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();
                    if (apiResult?.Code == 0) successCount++;
                    else errors.Add($"{boundPerson?.Name}: {apiResult?.Msg}");
                }
                catch (Exception ex)
                {
                    errors.Add($"{boundPerson?.Name}: {ex.Message}");
                }
            }

            lblStatus.Text = $"{successCount}명 삭제 완료";
            if (errors.Count > 0)
                ShowError($"일부 삭제 실패:\n{string.Join("\n", errors)}");
            await RefreshPersonnel();
            await RefreshSystemInfo();
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
                Text = "실시간 이벤트 보기",
                Size = new System.Drawing.Size(1000, 650),
                StartPosition = FormStartPosition.CenterParent
            };

            var filterPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(5) };
            var lblFilter = new Label { Text = "단말기:", AutoSize = true, Location = new Point(5, 12) };
            var cmbDevice = new ComboBox
            {
                Location = new Point(60, 8), Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDevice.Items.Add("전체 단말기");
            foreach (var item in cmbAttDevice.Items)
                if (item.ToString() != "전체 단말기")
                    cmbDevice.Items.Add(item);
            cmbDevice.SelectedIndex = 0;

            var lblCount = new Label { AutoSize = true, Location = new Point(275, 12) };
            filterPanel.Controls.Add(lblFilter);
            filterPanel.Controls.Add(cmbDevice);
            filterPanel.Controls.Add(lblCount);

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RT_Time",   HeaderText = "시간",     Width = 160 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RT_Dong",   HeaderText = "동",       Width = 65  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RT_Ho",     HeaderText = "호",       Width = 65  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RT_Member", HeaderText = "멤버#",     Width = 55  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RT_Name",   HeaderText = "사용자명", Width = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RT_Device", HeaderText = "단말기",   Width = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "RT_Event",  HeaderText = "이벤트",   Width = 100 });

            async Task LoadRecords()
            {
                try
                {
                    var devSnFilter = (string?)null;
                    if (cmbDevice.SelectedIndex > 0)
                    {
                        var selLabel = cmbDevice.SelectedItem?.ToString() ?? "";
                        foreach (DataGridViewRow row in dgvDevices.Rows)
                        {
                            var dname = row.Cells["DeviceName"].Value?.ToString() ?? "";
                            var sn    = row.Cells["SN"].Value?.ToString() ?? "";
                            var label = string.IsNullOrWhiteSpace(dname) ? sn : dname;
                            if (label == selLabel) { devSnFilter = sn; break; }
                        }
                    }
                    var req = new AttendanceSearchRequest { PageIndex = 1, PageSize = 500, DeviceSN = devSnFilter };
                    var resp = await _httpClient.PostAsJsonAsync("/api/Attendance/Search", req);
                    var result = await resp.Content.ReadFromJsonAsync<BrowserApiResponse<AttendanceSearchResult>>();
                    if (result?.Code == 0 && result.Data != null && !form.IsDisposed)
                    {
                        form.Invoke(() =>
                        {
                            dgv.Rows.Clear();
                            foreach (var rec in result.Data.DataList.OrderByDescending(r => r.RecordTime))
                            {
                                long.TryParse(rec.UserID, out long uid);
                                int rt = rec.RecordType;
                                string evtLabel = RecordTypeToLabel(rt);
                                dgv.Rows.Add(
                                    rec.RecordTime,
                                    uid == 0 ? rec.UserID : (uid / 1_000_000L).ToString(),
                                    uid == 0 ? "" : ((uid / 100L) % 10_000L).ToString(),
                                    uid == 0 ? "" : (uid % 100L).ToString(),
                                    rec.UserName,
                                    rec.DeviceSN,
                                    evtLabel
                                );
                            }
                            lblCount.Text = $"총 {result.Data.TotalCount}건";
                        });
                    }
                }
                catch { /* 무시 - 타이머 반복 호출 */ }
            }

            var timer = new System.Windows.Forms.Timer { Interval = 3000 };
            timer.Tick += async (s2, ev) => await LoadRecords();
            cmbDevice.SelectedIndexChanged += async (s2, ev) => await LoadRecords();
            form.Shown += async (s2, ev) => { await LoadRecords(); timer.Start(); };
            form.FormClosed += (s2, ev) => timer.Dispose();

            form.Controls.Add(dgv);
            form.Controls.Add(filterPanel);
            form.ShowDialog();
        }
        catch (Exception ex)
        {
            ShowError($"실시간 보기 열기 실패: {ex.Message}");
        }
    }

    private void PopulateRealTimeGrid(DataGridView dgv, string deviceFilter)
    {
        dgv.Rows.Clear();
        // Pull directly from current attendance grid rows
        foreach (DataGridViewRow row in dgvAttendance.Rows)
        {
            if (row.DataBoundItem is not AttendanceRecord rec) continue;
            var devDisplay = rec.DeviceSN;
            if (deviceFilter != "전체 단말기" && devDisplay != deviceFilter) continue;
            long uid = 0;
            long.TryParse(rec.UserID, out uid);
            dgv.Rows.Add(
                rec.RecordTime,
                uid == 0 ? rec.UserID : (uid / 1_000_000L).ToString(),
                uid == 0 ? "" : ((uid / 100L) % 10_000L).ToString(),
                uid == 0 ? "" : (uid % 100L).ToString(),
                rec.UserName,
                rec.DeviceSN,
                rec.RecordType
            );
        }
    }

    private void btnRefreshDevices_Click(object sender, EventArgs e) => _ = RefreshDevices();
    private void btnRefreshDepartments_Click(object sender, EventArgs e) => _ = RefreshDepartments();

    // ── 단말기에서 사용자 가져오기 ──────────────────────────────────────────────────
    private async void btnPullPeople_Click(object sender, EventArgs e)
    {
        // 체크된 단말기 목록 수집
        var checkedRows = dgvDevices.Rows.Cast<DataGridViewRow>()
            .Where(r => r.Cells["Selected"].Value is true)
            .ToList();

        if (checkedRows.Count == 0 && dgvDevices.SelectedRows.Count > 0)
            checkedRows = dgvDevices.SelectedRows.Cast<DataGridViewRow>().ToList();

        if (checkedRows.Count == 0)
        {
            MessageBox.Show("사용자를 가져올 단말기를 선택(체크)하세요.", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var names = checkedRows
            .Select(r => r.Cells["DeviceName"].Value?.ToString() ?? r.Cells["SN"].Value?.ToString() ?? "")
            .Where(n => !string.IsNullOrEmpty(n));

        var confirm = MessageBox.Show(
            $"다음 단말기 {checkedRows.Count}개에 저장된 모든 사용자 데이터를\nPC 서버로 가져오겠습니까?\n\n{string.Join("\n", names)}\n\n단말기가 다음 폴링 시에 사용자 데이터를 전송합니다.",
            "사용자 가져오기 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        try
        {
            int ok = 0, fail = 0;
            foreach (var row in checkedRows)
            {
                var sn = row.Cells["SN"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(sn)) continue;
                var resp = await _httpClient.PostAsJsonAsync($"/admin/devices/{sn}/pull-all-people", new { });
                if (resp.IsSuccessStatusCode) ok++; else fail++;
            }
            lblStatus.Text = $"사용자 가져오기 명령 전송: {ok}개 성공, {fail}개 실패";
            MessageBox.Show(
                $"{ok}개 단말기에 사용자 가져오기 명령을 전송했습니다.\n단말기가 다음 폴링 시 사용자 데이터를 서버로 전송합니다.\n\n잠시 후 사용자 탭을 새로고침하세요.",
                "전송 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError($"사용자 가져오기 실패: {ex.Message}");
        }
    }

    // ── 서버 사용자를 단말기로 배포 ──────────────────────────────────────────────
    private async void btnDistributePeople_Click(object sender, EventArgs e)
    {
        // 접속된 단말기 목록 표시 후 선택
        List<DeviceInfo>? devices;
        try
        {
            devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
        }
        catch (Exception ex)
        {
            ShowError($"단말기 목록 조회 실패: {ex.Message}");
            return;
        }

        if (devices == null || devices.Count == 0)
        {
            MessageBox.Show("등록된 단말기가 없습니다.", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // 단말기 선택 다이얼로그
        using var dlg = new Form
        {
            Text = "배포 대상 단말기 선택",
            Size = new Size(500, 400),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false, MinimizeBox = false
        };

        var lbl = new Label
        {
            Text = "사용자를 전송할 단말기를 선택하세요 (복수 선택 가능):",
            Location = new Point(15, 15), Size = new Size(460, 20)
        };

        var clb = new CheckedListBox
        {
            Location = new Point(15, 45), Size = new Size(460, 270),
            CheckOnClick = true
        };
        foreach (var d in devices)
        {
            var label = string.IsNullOrWhiteSpace(d.DeviceName) ? d.SN : $"{d.DeviceName} ({d.SN})";
            clb.Items.Add(new KeyValuePair<string, string>(d.SN, label), false);
        }
        clb.DisplayMember = "Value";

        var btnAll = new Button { Text = "전체 선택", Location = new Point(15, 325), Size = new Size(100, 28) };
        btnAll.Click += (s, _) => { for (int i = 0; i < clb.Items.Count; i++) clb.SetItemChecked(i, true); };

        var btnOk = new Button
        {
            Text = "배포 시작", DialogResult = DialogResult.OK,
            Location = new Point(275, 325), Size = new Size(90, 28)
        };
        var btnCancel = new Button
        {
            Text = "취소", DialogResult = DialogResult.Cancel,
            Location = new Point(375, 325), Size = new Size(90, 28)
        };

        dlg.Controls.AddRange(new Control[] { lbl, clb, btnAll, btnOk, btnCancel });
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnCancel;

        if (dlg.ShowDialog() != DialogResult.OK) return;

        var selectedSNs = clb.CheckedItems.Cast<KeyValuePair<string, string>>()
            .Select(kv => kv.Key).ToList();

        if (selectedSNs.Count == 0)
        {
            MessageBox.Show("배포할 단말기를 하나 이상 선택하세요.", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // 그리드에서 선택(체크)된 사용자 ID 수집
        var selectedUserIds = dgvPersonnel.Rows
            .Cast<DataGridViewRow>()
            .Where(r => r.Cells["ColSelect"].Value is true)
            .Select(r => (r.Tag as PersonInfo)?.UserID)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToList();

        if (selectedUserIds.Count == 0)
        {
            MessageBox.Show("배포할 사용자를 하나 이상 선택하세요.", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var resp = await _httpClient.PostAsJsonAsync(
                "/admin/people/distribute-to-devices",
                new { TargetSNs = selectedSNs, PersonIds = selectedUserIds });

            if (resp.IsSuccessStatusCode)
            {
                UseWaitCursor = true;
                Cursor = Cursors.WaitCursor;
                try
                {
                    var payload = await resp.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                    var jobIds = new List<string>();
                    if (payload.ValueKind != System.Text.Json.JsonValueKind.Undefined
                        && payload.TryGetProperty("Content", out var content)
                        && content.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        foreach (var item in content.EnumerateArray())
                        {
                            if (item.TryGetProperty("JobId", out var idEl))
                            {
                                var id = idEl.GetString();
                                if (!string.IsNullOrWhiteSpace(id))
                                    jobIds.Add(id);
                            }
                        }
                    }

                    lblStatus.Text = "단말기 배포 결과 대기 중...";
                    var (ok, message) = await DeviceCommandWaiter.WaitAsync(_httpClient, jobIds, TimeSpan.FromSeconds(90));
                    lblStatus.Text = message;
                    MessageBox.Show(message, ok ? "배포 완료" : "배포 실패",
                        MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    await RefreshDevices();
                }
                finally
                {
                    UseWaitCursor = false;
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                ShowError($"배포 명령 전송 실패: HTTP {resp.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            ShowError($"배포 실패: {ex.Message}");
        }
    }

    private void btnRemoteControl_Click(object sender, EventArgs e)
    {
        var row = dgvDevices.SelectedRows.Count > 0 ? dgvDevices.SelectedRows[0] : null;
        if (row == null)
        {
            MessageBox.Show("단말기를 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        try
        {
            var device = row.DataBoundItem as DeviceInfo;
            if (device == null) { ShowError("Invalid device selection"); return; }
            using var form = new DeviceSettingsForm(device, _httpClient);
            if (form.ShowDialog() == DialogResult.OK) { _ = RefreshDevices(); _ = RefreshSystemInfo(); }
        }
        catch (Exception ex) { ShowError($"단말기 설정 열기 실패: {ex.Message}"); }
    }

    private async void btnRemoveDevice_Click(object sender, EventArgs e)
    {
        // 체크박스로 선택된 행 수집
        var checkedRows = dgvDevices.Rows.Cast<DataGridViewRow>()
            .Where(r => r.Cells["Selected"].Value is true)
            .ToList();

        // 체크된 항목이 없으면 그리드 선택 행 사용
        if (checkedRows.Count == 0)
        {
            if (dgvDevices.SelectedRows.Count == 0)
            {
                MessageBox.Show("제거할 단말기를 선택(체크)하거나 행을 클릭하세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            checkedRows = dgvDevices.SelectedRows.Cast<DataGridViewRow>().ToList();
        }

        var names = checkedRows
            .Select(r => r.Cells["DeviceName"].Value?.ToString() ?? r.Cells["SN"].Value?.ToString() ?? "")
            .Where(n => !string.IsNullOrEmpty(n));

        var confirm = MessageBox.Show(
            $"다음 단말기 {checkedRows.Count}개를 제거하시겠습니까?\n\n{string.Join("\n", names)}",
            "단말기 제거 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        try
        {
            int ok = 0, fail = 0;
            foreach (var row in checkedRows)
            {
                var sn = row.Cells["SN"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(sn)) continue;
                var resp = await _httpClient.DeleteAsync($"/admin/devices/{sn}");
                if (resp.IsSuccessStatusCode) ok++; else fail++;
            }
            lblStatus.Text = $"제거 완료: {ok}개 성공, {fail}개 실패";
            await RefreshDevices();
            await RefreshSystemInfo();
        }
        catch (Exception ex)
        {
            ShowError($"단말기 제거 실패: {ex.Message}");
        }
    }

    private void ShowError(string message)
    {
        lblStatus.Text = message;
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    // Device Context Menu Event Handlers
    private async void btnRefreshPersonnel_Click(object sender, EventArgs e)
    {
        await RefreshPersonnel();
    }

    private void btnSelectByFilter_Click(object sender, EventArgs e)
    {
        string dong = txtFilterDong.Text.Trim();
        string ho   = txtFilterHo.Text.Trim();

        if (string.IsNullOrEmpty(dong))
        {
            MessageBox.Show("동을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int colDong   = dgvPersonnel.Columns["ColDong"]?.Index ?? -1;
        int colHo     = dgvPersonnel.Columns["ColHo"]?.Index ?? -1;
        int colSelect = dgvPersonnel.Columns["ColSelect"]?.Index ?? -1;
        if (colDong < 0) return;

        bool useHo = !string.IsNullOrEmpty(ho) && colHo >= 0;

        // 해당 필터에 매칭되는 행들 수집
        var matched = new List<DataGridViewRow>();
        foreach (DataGridViewRow row in dgvPersonnel.Rows)
        {
            if (row.IsNewRow) continue;
            var cellDong = row.Cells[colDong].Value?.ToString()?.Trim() ?? "";
            if (cellDong != dong) continue;
            if (useHo)
            {
                var cellHo = colHo >= 0 ? row.Cells[colHo].Value?.ToString()?.Trim() ?? "" : "";
                if (cellHo != ho) continue;
            }
            matched.Add(row);
        }

        if (matched.Count == 0) return;

        // 이미 모두 선택된 상태이면 → 해제, 아니면 → 선택 (토글)
        bool allSelected = matched.All(r => r.Selected);
        bool deselect = allSelected;

        dgvPersonnel.ClearSelection();
        foreach (var row in matched)
        {
            bool check = !deselect;
            if (colSelect >= 0 && dgvPersonnel.Columns["ColSelect"] is DataGridViewCheckBoxColumn)
                row.Cells[colSelect].Value = check;
            row.Selected = check;
        }

        btnSelectByFilter.Text = deselect ? "선택" : "해제";
    }


    private async void btnSaveToFiles_Click(object sender, EventArgs e)
    {
        var selectedUserIds = dgvPersonnel.Rows
            .Cast<DataGridViewRow>()
            .Where(r => r.Cells["ColSelect"].Value is true)
            .Select(r => (r.Tag as PersonInfo)?.UserID)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id!)
            .ToList();

        if (selectedUserIds.Count == 0)
        {
            MessageBox.Show("저장할 사용자를 하나 이상 선택하세요.", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"선택한 {selectedUserIds.Count}명을 App_Data/people 폴더에 JSON으로 저장합니다.\n이미 있는 파일은 덮어씁니다.\n\n계속하시겠습니까?",
            "파일로 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        try
        {
            var resp = await _httpClient.PostAsJsonAsync("/admin/people/save-to-files", new { UserIds = selectedUserIds });
            if (!resp.IsSuccessStatusCode)
            {
                ShowError($"파일 저장 실패: HTTP {resp.StatusCode}");
                return;
            }
            var result = await resp.Content.ReadFromJsonAsync<System.Text.Json.Nodes.JsonObject>();
            int saved   = result?["saved"]?.GetValue<int>()   ?? 0;
            int skipped = result?["skipped"]?.GetValue<int>() ?? 0;
            int errors  = result?["errors"]?.GetValue<int>()  ?? 0;
            lblStatus.Text = $"파일 저장: {saved}명 저장, {skipped}건 건너뜀, {errors}건 오류";
            MessageBox.Show($"저장 {saved}명, 건너뜀 {skipped}건, 오류 {errors}건", "파일로 저장",
                MessageBoxButtons.OK, errors > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError($"파일 저장 실패: {ex.Message}");
        }
    }

    private async void btnReloadFromFiles_Click(object sender, EventArgs e)
    {
        var confirm = MessageBox.Show(
            "서버의 people 폴더에 저장된 JSON 파일들을 읽어 사용자 목록을 갱신합니다.\n" +
            "현재 메모리에 없는 사용자가 추가되고, 기존 사용자는 파일 내용으로 덮어씁니다.\n\n계속하시겠습니까?",
            "파일에서 불러오기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        try
        {
            var resp = await _httpClient.PostAsync("/admin/people/reload-from-files", null);
            if (!resp.IsSuccessStatusCode)
            {
                ShowError($"파일 불러오기 실패: HTTP {resp.StatusCode}");
                return;
            }
            var result = await resp.Content.ReadFromJsonAsync<System.Text.Json.Nodes.JsonObject>();
            int loaded  = result?["loaded"]?.GetValue<int>()  ?? 0;
            int skipped = result?["skipped"]?.GetValue<int>() ?? 0;
            int errors  = result?["errors"]?.GetValue<int>()  ?? 0;

            await RefreshPersonnel();
            lblStatus.Text = $"파일 불러오기 완료: {loaded}명 로드, {skipped}건 건너뜀, {errors}건 오류";
            MessageBox.Show(
                $"people 폴더에서 불러오기 완료\n\n로드: {loaded}명\n건너뜀: {skipped}건\n오류: {errors}건",
                "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError($"파일 불러오기 중 오류: {ex.Message}");
        }
    }

    private static (string? exact, long? min, long? max) BuildAttUserIDFilter(string dong, string ho, string member)
    {
        long dv = 0, hv = 0, mv = 0;
        bool hasDong   = !string.IsNullOrEmpty(dong)   && long.TryParse(dong,   out dv);
        bool hasHo     = !string.IsNullOrEmpty(ho)     && long.TryParse(ho,     out hv);
        bool hasMember = !string.IsNullOrEmpty(member) && long.TryParse(member, out mv);

        if (!hasDong && !hasHo && !hasMember)
            return (null, null, null);

        if (hasDong && hasHo && hasMember)
        {
            // Exact match
            long uid = dv * 1_000_000L + hv * 100L + mv;
            return (uid.ToString(), null, null);
        }
        if (hasDong && hasHo)
        {
            // All members of this 호
            long min = dv * 1_000_000L + hv * 100L;
            long max = min + 100L;
            return (null, min, max);
        }
        // dong only
        {
            long min = dv * 1_000_000L;
            long max = (dv + 1) * 1_000_000L;
            return (null, min, max);
        }
    }
    private void txtAttDong_TextChanged(object sender, EventArgs e)
    {
        bool hasDong = !string.IsNullOrWhiteSpace(txtAttDong.Text);
        txtAttHo.Enabled = hasDong;
        if (!hasDong) { txtAttHo.Text = ""; txtAttMember.Text = ""; txtAttMember.Enabled = false; }
    }

    private void txtAttHo_TextChanged(object sender, EventArgs e)
    {
        bool hasHo = !string.IsNullOrWhiteSpace(txtAttHo.Text);
        txtAttMember.Enabled = hasHo;
        if (!hasHo) { txtAttMember.Text = ""; }
    }

    private static string RecordTypeToLabel(int rt) => rt switch
    {
        1  => "카드",
        2  => "지문",
        3  => "얼굴인식",
        4  => "카드+지문",
        5  => "얼굴+지문",
        6  => "카드+얼굴",
        7  => "카드+비밀번호",
        8  => "얼굴+비밀번호",
        9  => "지문+비밀번호",
        10 => "비밀번호",
        11 => "카드+지문+비밀번호",
        12 => "카드+얼굴+비밀번호",
        13 => "지문+얼굴+비밀번호",
        14 => "카드+지문+얼굴",
        15 => "중복인증",
        16 => "유효기간만료",
        17 => "시간대만료",
        18 => "휴일출입불가",
        19 => "미등록사용자",
        20 => "잠금감지",
        21 => "인증횟수초과",
        22 => "잠금중인증거부",
        23 => "분실신고카드",
        24 => "블랙리스트카드",
        25 => "무인증개방",
        26 => "카드인증금지",
        27 => "지문인증금지",
        28 => "컨트롤러만료",
        29 => "유효기간임박",
        30 => "체온이상거부",
        31 => "방문자비밀번호",
        32 => "QR코드개방",
        33 => "메뉴사용자추가",
        34 => "메뉴사용자수정",
        35 => "메뉴사용자삭제",
        36 => "손바닥정맥",
        37 => "카드+손바닥정맥+얼굴",
        38 => "손바닥정맥+비밀번호",
        39 => "카드+손바닥정맥",
        40 => "얼굴+손바닥정맥",
        41 => "카드+손바닥정맥+비밀번호",
        42 => "손바닥정맥+얼굴+비밀번호",
        43 => "지문+손바닥정맥+얼굴",
        44 => "복합인증대기",
        45 => "복합인증실패",
        46 => "복합인증성공",
        47 => "신분증비교",
        48 => "미등록카드",
        49 => "미등록QR",
        _  => $"(유형{rt})"
    };

    private static string GetLocationLabel(string? userID)
    {
        if (long.TryParse(userID, out long id))
            return $"{id / 1_000_000L}동 {(id / 100L) % 10_000L}호 멤버{id % 100L}";
        return userID ?? "";
    }

}
