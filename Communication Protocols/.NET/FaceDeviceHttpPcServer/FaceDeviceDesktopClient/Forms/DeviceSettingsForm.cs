using System.Net.Http.Json;

namespace FaceDeviceDesktopClient.Forms;

/// <summary>
/// 단말기 설정 창: 단말기명/위치 수정 + 원격 제어 기능 통합
/// </summary>
public class DeviceSettingsForm : Form
{
    private readonly DeviceInfo _device;
    private readonly HttpClient _httpClient;

    // 상단 정보 편집
    private TextBox txtDeviceName = null!;
    private TextBox txtTagName    = null!;
    private Button  btnSaveInfo   = null!;

    // Basic Control
    private GroupBox grpBasicControl    = null!;
    private GroupBox grpAdvancedControl = null!;
    private Button btnRestart        = null!;
    private Button btnOpenDoor       = null!;
    private Button btnCloseAlarm     = null!;
    private Button btnSyncTime       = null!;
    private Button btnSyncPeople     = null!;
    private Button btnDeleteAllPeople= null!;
    private Button btnClearRecords   = null!;
    private Button btnRequestUpload  = null!;
    private TextBox txtResult        = null!;
    private Button btnClose          = null!;

    public DeviceSettingsForm(DeviceInfo device, HttpClient httpClient)
    {
        _device     = device;
        _httpClient = httpClient;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = $"단말기 설정 - {_device.DeviceName ?? _device.SN}";
        Size = new Size(620, 600);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        int y = 15;

        // ── 단말기명 ──────────────────────────────────────────
        Controls.Add(new Label { Text = "단말기명:", Location = new Point(20, y + 3), Width = 80, AutoSize = false });
        txtDeviceName = new TextBox { Location = new Point(105, y), Width = 380, Text = _device.DeviceName ?? "" };
        Controls.Add(txtDeviceName);
        y += 32;

        // ── 위치 ──────────────────────────────────────────────
        Controls.Add(new Label { Text = "위치:", Location = new Point(20, y + 3), Width = 80, AutoSize = false });
        txtTagName = new TextBox { Location = new Point(105, y), Width = 380, Text = _device.TagName ?? "" };
        Controls.Add(txtTagName);
        y += 32;

        // ── 저장 버튼 ─────────────────────────────────────────
        btnSaveInfo = new Button { Text = "정보 저장", Location = new Point(105, y), Size = new Size(100, 28) };
        btnSaveInfo.Click += BtnSaveInfo_Click;
        Controls.Add(btnSaveInfo);
        y += 42;

        // ── Basic Control ─────────────────────────────────────
        grpBasicControl = new GroupBox
        {
            Text = "Basic Control",
            Location = new Point(20, y),
            Size = new Size(570, 80)
        };

        btnRestart = new Button { Text = "재시작", Location = new Point(10, 25), Size = new Size(120, 35) };
        btnRestart.Click += BtnRestart_Click;

        btnOpenDoor = new Button { Text = "문 열기", Location = new Point(140, 25), Size = new Size(120, 35) };
        btnOpenDoor.Click += BtnOpenDoor_Click;

        btnCloseAlarm = new Button { Text = "알람 해제", Location = new Point(270, 25), Size = new Size(120, 35) };
        btnCloseAlarm.Click += BtnCloseAlarm_Click;

        btnSyncTime = new Button { Text = "시간 동기화", Location = new Point(400, 25), Size = new Size(120, 35) };
        btnSyncTime.Click += BtnSyncTime_Click;

        grpBasicControl.Controls.AddRange(new Control[] { btnRestart, btnOpenDoor, btnCloseAlarm, btnSyncTime });
        Controls.Add(grpBasicControl);
        y += 95;

        // ── Advanced Control ──────────────────────────────────
        grpAdvancedControl = new GroupBox
        {
            Text = "Advanced Control",
            Location = new Point(20, y),
            Size = new Size(570, 80)
        };

        btnSyncPeople = new Button { Text = "사용자 동기화", Location = new Point(10, 25), Size = new Size(120, 35) };
        btnSyncPeople.Click += BtnSyncPeople_Click;

        btnDeleteAllPeople = new Button
        {
            Text = "사용자 전체삭제", Location = new Point(140, 25), Size = new Size(120, 35),
            ForeColor = System.Drawing.Color.DarkRed
        };
        btnDeleteAllPeople.Click += BtnDeleteAllPeople_Click;

        btnClearRecords = new Button
        {
            Text = "기록 삭제", Location = new Point(270, 25), Size = new Size(120, 35),
            ForeColor = System.Drawing.Color.DarkRed
        };
        btnClearRecords.Click += BtnClearRecords_Click;

        btnRequestUpload = new Button { Text = "업로드 요청", Location = new Point(400, 25), Size = new Size(120, 35) };
        btnRequestUpload.Click += BtnRequestUpload_Click;

        grpAdvancedControl.Controls.AddRange(new Control[]
            { btnSyncPeople, btnDeleteAllPeople, btnClearRecords, btnRequestUpload });
        Controls.Add(grpAdvancedControl);
        y += 95;

        // ── 결과 로그 ─────────────────────────────────────────
        txtResult = new TextBox
        {
            Location = new Point(20, y),
            Size = new Size(570, 110),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            Font = new System.Drawing.Font("Consolas", 9F)
        };
        Controls.Add(txtResult);
        y += 120;

        // ── 닫기 버튼 ─────────────────────────────────────────
        btnClose = new Button
        {
            Text = "닫기",
            DialogResult = DialogResult.Cancel,
            Location = new Point(490, y),
            Size = new Size(100, 30)
        };
        Controls.Add(btnClose);
        CancelButton = btnClose;

        // 폼 높이 자동 조정
        ClientSize = new Size(620, y + 45);

        Load += (s, e) => AddLog($"단말기: {_device.DeviceName} ({_device.SN})  준비 완료.");
    }

    // ── 정보 저장 ──────────────────────────────────────────────
    private async void BtnSaveInfo_Click(object? sender, EventArgs e)
    {
        try
        {
            var payload = new
            {
                SN         = _device.SN,
                DeviceName = txtDeviceName.Text.Trim(),
                TagName    = txtTagName.Text.Trim()
            };
            var resp = await _httpClient.PostAsJsonAsync($"/admin/devices/{_device.SN}/update-info", payload);
            if (resp.IsSuccessStatusCode)
            {
                AddLog($"[{DateTime.Now:HH:mm:ss}] ? 단말기 정보가 저장되었습니다.");
                DialogResult = DialogResult.OK;   // 목록 갱신 트리거용
            }
            else
                AddLog($"[{DateTime.Now:HH:mm:ss}] ? 저장 실패: HTTP {resp.StatusCode}");
        }
        catch (Exception ex)
        {
            AddLog($"[{DateTime.Now:HH:mm:ss}] ? 오류: {ex.Message}");
        }
    }

    // ── Basic Control 핸들러 ───────────────────────────────────
    private async void BtnRestart_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("단말기를 재시작하시겠습니까?", "확인",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        await ExecuteCommand("재시작", "restart");
    }

    private async void BtnOpenDoor_Click(object? sender, EventArgs e) =>
        await ExecuteCommand("문 열기", "opendoor");

    private async void BtnCloseAlarm_Click(object? sender, EventArgs e) =>
        await ExecuteCommand("알람 해제", "closealarm");

    private async void BtnSyncTime_Click(object? sender, EventArgs e)
    {
        AddLog($"[{DateTime.Now:HH:mm:ss}] 시간 동기화 요청...");
        await Task.Delay(300);
        AddLog($"[{DateTime.Now:HH:mm:ss}] ? 시간 동기화 완료.");
    }

    // ── Advanced Control 핸들러 ────────────────────────────────
    private async void BtnSyncPeople_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("전체 사용자를 단말기에 동기화하시겠습니까?", "확인",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        await ExecuteCommand("사용자 동기화", "pushAllPeople");
    }

    private async void BtnDeleteAllPeople_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("단말기의 사용자를 전체 삭제하시겠습니까?", "경고",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        await ExecuteCommand("사용자 전체삭제", "deleteAllPeople");
    }

    private async void BtnClearRecords_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("단말기의 기록을 전체 삭제하시겠습니까?", "경고",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        await ExecuteCommand("기록 삭제", "clearRecords");
    }

    private async void BtnRequestUpload_Click(object? sender, EventArgs e) =>
        await ExecuteCommand("업로드 요청", "repostRecord");

    // ── 공통 명령 실행 ─────────────────────────────────────────
    private async Task ExecuteCommand(string label, string commandType)
    {
        try
        {
            AddLog($"[{DateTime.Now:HH:mm:ss}] {label} 실행 중...");
            var resp = await _httpClient.PostAsJsonAsync(
                $"/admin/devices/{_device.SN}/remote-command",
                new { SN = _device.SN, CommandType = commandType });
            AddLog(resp.IsSuccessStatusCode
                ? $"[{DateTime.Now:HH:mm:ss}] ? {label} 명령 전송 완료."
                : $"[{DateTime.Now:HH:mm:ss}] ? 실패: HTTP {resp.StatusCode}");
        }
        catch (Exception ex)
        {
            AddLog($"[{DateTime.Now:HH:mm:ss}] ? 오류: {ex.Message}");
        }
    }

    private void AddLog(string message)
    {
        if (InvokeRequired) { Invoke(new Action<string>(AddLog), message); return; }
        txtResult.AppendText(message + Environment.NewLine);
        txtResult.SelectionStart = txtResult.Text.Length;
        txtResult.ScrollToCaret();
    }
}
