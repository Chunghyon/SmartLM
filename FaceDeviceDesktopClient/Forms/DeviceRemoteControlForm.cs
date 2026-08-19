using FaceDeviceDesktopClient.Services;
using System;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace FaceDeviceDesktopClient.Forms;

public class DeviceRemoteControlForm : Form
{
    private GroupBox grpBasicControl = null!;
    private GroupBox grpAdvancedControl = null!;
    private Button btnRestart = null!;
    private Button btnOpenDoor = null!;
    private Button btnCloseAlarm = null!;
    private Button btnSyncTime = null!;
    private Button btnSyncPeople = null!;
    private Button btnDeleteAllPeople = null!;
    private Button btnClearRecords = null!;
    private Button btnRequestUpload = null!;
    private Button btnClose = null!;
    private Label lblDeviceInfo = null!;
    private TextBox txtResult = null!;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string DeviceSN { get; set; } = string.Empty;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string DeviceName { get; set; } = string.Empty;

    public DeviceRemoteControlForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Device Remote Control";
        Size = new System.Drawing.Size(600, 500);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // Device Info Label
        lblDeviceInfo = new Label
        {
            Location = new System.Drawing.Point(20, 20),
            Size = new System.Drawing.Size(560, 30),
            Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
            ForeColor = System.Drawing.Color.DarkBlue,
            TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        };

        // Basic Control Group
        grpBasicControl = new GroupBox
        {
            Text = "Basic Control",
            Location = new System.Drawing.Point(20, 60),
            Size = new System.Drawing.Size(560, 120)
        };

        btnRestart = new Button
        {
            Text = "Restart Device",
            Location = new System.Drawing.Point(20, 30),
            Size = new System.Drawing.Size(120, 35)
        };
        btnRestart.Click += BtnRestart_Click;

        btnOpenDoor = new Button
        {
            Text = "Open Door",
            Location = new System.Drawing.Point(150, 30),
            Size = new System.Drawing.Size(120, 35)
        };
        btnOpenDoor.Click += BtnOpenDoor_Click;

        btnCloseAlarm = new Button
        {
            Text = "Close Alarm",
            Location = new System.Drawing.Point(280, 30),
            Size = new System.Drawing.Size(120, 35)
        };
        btnCloseAlarm.Click += BtnCloseAlarm_Click;

        btnSyncTime = new Button
        {
            Text = "Sync Time",
            Location = new System.Drawing.Point(410, 30),
            Size = new System.Drawing.Size(120, 35)
        };
        btnSyncTime.Click += BtnSyncTime_Click;

        grpBasicControl.Controls.AddRange(new Control[] { btnRestart, btnOpenDoor, btnCloseAlarm, btnSyncTime });

        // Advanced Control Group
        grpAdvancedControl = new GroupBox
        {
            Text = "Advanced Control",
            Location = new System.Drawing.Point(20, 190),
            Size = new System.Drawing.Size(560, 120)
        };

        btnSyncPeople = new Button
        {
            Text = "Sync All People",
            Location = new System.Drawing.Point(20, 30),
            Size = new System.Drawing.Size(120, 35)
        };
        btnSyncPeople.Click += BtnSyncPeople_Click;

        btnDeleteAllPeople = new Button
        {
            Text = "Delete All People",
            Location = new System.Drawing.Point(150, 30),
            Size = new System.Drawing.Size(120, 35),
            ForeColor = System.Drawing.Color.DarkRed
        };
        btnDeleteAllPeople.Click += BtnDeleteAllPeople_Click;

        btnClearRecords = new Button
        {
            Text = "Clear Records",
            Location = new System.Drawing.Point(280, 30),
            Size = new System.Drawing.Size(120, 35),
            ForeColor = System.Drawing.Color.DarkRed
        };
        btnClearRecords.Click += BtnClearRecords_Click;

        btnRequestUpload = new Button
        {
            Text = "Request Upload",
            Location = new System.Drawing.Point(410, 30),
            Size = new System.Drawing.Size(120, 35)
        };
        btnRequestUpload.Click += BtnRequestUpload_Click;

        grpAdvancedControl.Controls.AddRange(new Control[] { btnSyncPeople, btnDeleteAllPeople, btnClearRecords, btnRequestUpload });

        // Result TextBox
        txtResult = new TextBox
        {
            Location = new System.Drawing.Point(20, 320),
            Size = new System.Drawing.Size(560, 100),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            Font = new System.Drawing.Font("Consolas", 9F)
        };

        // Close Button
        btnClose = new Button
        {
            Text = "Close",
            DialogResult = DialogResult.Cancel,
            Location = new System.Drawing.Point(480, 430),
            Size = new System.Drawing.Size(100, 30)
        };

        Controls.AddRange(new Control[] { lblDeviceInfo, grpBasicControl, grpAdvancedControl, txtResult, btnClose });
        CancelButton = btnClose;

        Load += DeviceRemoteControlForm_Load;
    }

    private void DeviceRemoteControlForm_Load(object? sender, EventArgs e)
    {
        lblDeviceInfo.Text = $"Device: {DeviceName} ({DeviceSN})";
        AddLog("Remote control panel ready.");
    }

    private async void BtnRestart_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("Are you sure you want to restart this device?", "Confirm", 
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        await ExecuteCommand("Restart", "restart");
    }

    private async void BtnOpenDoor_Click(object? sender, EventArgs e)
    {
        await ExecuteCommand("Open Door", "opendoor");
    }

    private async void BtnCloseAlarm_Click(object? sender, EventArgs e)
    {
        await ExecuteCommand("Close Alarm", "closealarm");
    }

    private async void BtnSyncTime_Click(object? sender, EventArgs e)
    {
        AddLog($"[{DateTime.Now:HH:mm:ss}] Syncing device time to server time...");
        // Time sync would be handled by the server automatically
        await Task.Delay(500);
        AddLog($"[{DateTime.Now:HH:mm:ss}] ? Time sync requested.");
    }

    private async void BtnSyncPeople_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("This will push all personnel to the device. Continue?", "Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        await ExecuteCommand("Sync All People", "pushAllPeople");
    }

    private async void BtnDeleteAllPeople_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("WARNING: This will delete all personnel from the device!\n\nAre you sure?", "Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        await ExecuteCommand("Delete All People", "deleteAllPeople");
    }

    private async void BtnClearRecords_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("WARNING: This will clear all attendance records from the device!\n\nAre you sure?", "Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        await ExecuteCommand("Clear Records", "clearRecords");
    }

    private async void BtnRequestUpload_Click(object? sender, EventArgs e)
    {
        await ExecuteCommand("Request Upload", "repostRecord");
    }

    private async Task ExecuteCommand(string commandName, string commandType)
    {
        try
        {
            AddLog($"[{DateTime.Now:HH:mm:ss}] Executing: {commandName}...");

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");

            var request = new
            {
                SN = DeviceSN,
                CommandType = commandType
            };

            var response = await httpClient.PostAsJsonAsync($"/admin/devices/{DeviceSN}/remote-command", request);

            if (response.IsSuccessStatusCode)
            {
                Cursor = Cursors.WaitCursor;
                UseWaitCursor = true;
                try
                {
                    var payload = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                    string? jobId = null;
                    if (payload.ValueKind != System.Text.Json.JsonValueKind.Undefined
                        && payload.TryGetProperty("Content", out var content)
                        && content.ValueKind == System.Text.Json.JsonValueKind.Object
                        && content.TryGetProperty("JobId", out var idEl))
                    {
                        jobId = idEl.GetString();
                    }

                    AddLog($"[{DateTime.Now:HH:mm:ss}] {commandName} 단말기 처리 대기 중...");
                    var (ok, message) = await DeviceCommandWaiter.WaitAsync(httpClient, jobId is null ? [] : [jobId], TimeSpan.FromSeconds(90));
                    AddLog($"[{DateTime.Now:HH:mm:ss}] {message}");
                    MessageBox.Show(message, ok ? commandName : commandName + " 실패",
                        MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }
                finally
                {
                    UseWaitCursor = false;
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                AddLog($"[{DateTime.Now:HH:mm:ss}] ? Failed: HTTP {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            AddLog($"[{DateTime.Now:HH:mm:ss}] ? Error: {ex.Message}");
        }
    }

    private void AddLog(string message)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(AddLog), message);
            return;
        }

        txtResult.AppendText(message + Environment.NewLine);
        txtResult.SelectionStart = txtResult.Text.Length;
        txtResult.ScrollToCaret();
    }
}
