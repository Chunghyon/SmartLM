using System.Net.Http.Json;
using System.Text.Json;

namespace FaceDeviceDesktopClient;

public partial class MainForm : Form
{
    private readonly HttpClient _httpClient;
    private readonly string _serverUrl = "http://localhost:8100";

    public MainForm()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri(_serverUrl) };
        LoadInitialData();
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
            var devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");

            dgvDevices.DataSource = null;
            dgvDevices.DataSource = devices;
            dgvDevices.AutoResizeColumns();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to refresh devices: {ex.Message}");
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
            btnAutoSearch.Enabled = false;
            lblStatus.Text = "Searching for devices...";

            var response = await _httpClient.PostAsJsonAsync("/api/Device/Search", new { SearchMethod = "Broadcast" });
            var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<List<DiscoveredDevice>>>();

            if (result?.Code == 0 && result.Data != null)
            {
                dgvDiscoveredDevices.DataSource = null;
                dgvDiscoveredDevices.DataSource = result.Data;
                dgvDiscoveredDevices.AutoResizeColumns();
                lblStatus.Text = $"Found {result.Data.Count} device(s)";
            }
            else
            {
                lblStatus.Text = "Search completed with no results";
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
            MessageBox.Show("Please select a device to connect", "Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var row = dgvDiscoveredDevices.SelectedRows[0];
            var ip = row.Cells["IpAddress"].Value?.ToString();
            var port = Convert.ToInt32(row.Cells["HttpPort"].Value ?? 80);

            lblStatus.Text = $"Connecting to {ip}:{port}...";

            // Probe device
            var probeResponse = await _httpClient.PostAsJsonAsync("/api/Device/ProbeDevice", 
                new { IpAddress = ip, HttpPort = port });
            var probeResult = await probeResponse.Content.ReadFromJsonAsync<BrowserApiResponse<DeviceProbeInfo>>();

            if (probeResult?.Code != 0)
            {
                ShowError($"Failed to probe device: {probeResult?.Msg}");
                return;
            }

            // Connect device
            var connectRequest = new
            {
                DeviceSN = probeResult.Data?.DeviceSN,
                IpAddress = ip,
                HttpPort = port,
                DeviceName = probeResult.Data?.DeviceName,
                Model = probeResult.Data?.Model,
                FirmwareVersion = probeResult.Data?.FirmwareVersion
            };

            var connectResponse = await _httpClient.PostAsJsonAsync("/api/Device/Connect", connectRequest);
            var connectResult = await connectResponse.Content.ReadFromJsonAsync<BrowserApiResponse<string>>();

            if (connectResult?.Code == 0)
            {
                lblStatus.Text = $"Successfully connected to {probeResult.Data?.DeviceSN}";
                await RefreshDevices();
                await RefreshSystemInfo();
            }
            else
            {
                ShowError($"Connection failed: {connectResult?.Msg}");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Connection failed: {ex.Message}");
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

    private void ShowError(string message)
    {
        lblStatus.Text = message;
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
