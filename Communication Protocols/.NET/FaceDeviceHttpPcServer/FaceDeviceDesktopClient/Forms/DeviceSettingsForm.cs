using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace FaceDeviceDesktopClient.Forms;

public class DeviceSettingsForm : Form
{
    private readonly DeviceInfo _device;
    private readonly HttpClient _httpClient;

    // 단말기 정보
    private TextBox txtDeviceName = null!;
    private TextBox txtTagName    = null!;
    private Button  btnSaveInfo   = null!;

    // 제어 버튼
    private Button btnRestart         = null!;
    private Button btnOpenDoor        = null!;
    private Button btnDevicePeople    = null!;
    private Button btnDeleteAllPeople = null!;
    private Button btnClearRecords    = null!;
    private Button btnRequestUpload   = null!;
    private Button btnSyncTime        = null!;

    // 출입제어설정
    private NumericUpDown numReleaseTime         = null!;
    private CheckBox      chkFreeOpen            = null!;
    private ComboBox      cmbVerificationType    = null!;
    private TextBox       txtShortMessage        = null!;
    private TextBox       txtVisitorRootPassword = null!;
    private NumericUpDown numMultiPerson         = null!;

    // 로그
    private TextBox txtResult = null!;
    private Button  btnClose  = null!;

    private JsonObject? _fullWorkSetting;

    public DeviceSettingsForm(DeviceInfo device, HttpClient httpClient)
    {
        _device     = device;
        _httpClient = httpClient;
        BuildUI();
        _ = LoadAccessControlSettings();
    }

    private void BuildUI()
    {
        Text = $"단말기 설정 - {_device.DeviceName ?? _device.SN}";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        AutoScroll = true;

        const int lw = 160;
        const int tx = 175;
        const int cw = 370;
        const int fw = 590;
        int y = 15;

        // ── 단말기 정보 ───────────────────────────────────────
        Controls.Add(MkLabel("단말기명:", 20, y + 3, 80));
        txtDeviceName = new TextBox { Location = new Point(105, y), Width = 380, Text = _device.DeviceName ?? "" };
        Controls.Add(txtDeviceName);
        y += 32;

        Controls.Add(MkLabel("위치:", 20, y + 3, 80));
        txtTagName = new TextBox { Location = new Point(105, y), Width = 380, Text = _device.TagName ?? "" };
        Controls.Add(txtTagName);
        y += 32;

        btnSaveInfo = new Button { Text = "정보 저장", Location = new Point(105, y), Size = new Size(100, 28) };
        btnSaveInfo.Click += BtnSaveInfo_Click;
        Controls.Add(btnSaveInfo);
        y += 42;

        // ── 단말기 제어 (Basic + Advanced 통합) ──────────────
        var grpControl = new GroupBox
        {
            Text = "단말기 제어",
            Location = new Point(20, y),
            Size = new Size(fw, 120)
        };

        btnRestart         = MkBtn("재시작",          10,  25, BtnRestart_Click);
        btnOpenDoor        = MkBtn("문 열기",         140, 25, BtnOpenDoor_Click);
        btnDevicePeople    = MkBtn("사용자 정보",     270, 25, BtnDevicePeople_Click);
        btnDeleteAllPeople = MkBtn("사용자 전체삭제", 400, 25, BtnDeleteAllPeople_Click, System.Drawing.Color.DarkRed);
        btnClearRecords    = MkBtn("로그 삭제",        10, 68, BtnClearRecords_Click,    System.Drawing.Color.DarkRed);
        btnRequestUpload   = MkBtn("로그 가져오기",   140, 68, BtnRequestUpload_Click);
        btnSyncTime        = MkBtn("시간 동기화",     270, 68, BtnSyncTime_Click);

        grpControl.Controls.AddRange(new Control[]
        {
            btnRestart, btnOpenDoor, btnDevicePeople, btnDeleteAllPeople,
            btnClearRecords, btnRequestUpload, btnSyncTime
        });
        Controls.Add(grpControl);
        y += grpControl.Height + 10;

        // ── 출입제어설정 ──────────────────────────────────────
        var grpAccess = new GroupBox { Text = "출입제어설정", Location = new Point(20, y), Size = new Size(fw, 10) };
        int gy = 22;

        void AR(string label, Control ctrl)
        {
            grpAccess.Controls.Add(MkLabel(label, 10, gy + 3, lw));
            ctrl.Location = new Point(tx, gy);
            if (ctrl.Width < 10) ctrl.Width = cw;
            grpAccess.Controls.Add(ctrl);
            gy += 30;
        }

        numReleaseTime = new NumericUpDown { Minimum = 0, Maximum = 65535, Value = 3, Width = cw };
        AR("문 열림 유지시간 (s):", numReleaseTime);

        chkFreeOpen = new CheckBox { Text = "활성화", Width = cw };
        AR("무인증 개방:", chkFreeOpen);

        cmbVerificationType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = cw };
        cmbVerificationType.Items.AddRange(new object[]
        {
            "1. 표준 인증 (Standard)",
            "2. 얼굴/지문/손바닥/카드 + 비밀번호",
            "3. 카드 + 얼굴/지문/손바닥/비밀번호",
            "4. 다중 출석",
            "5. 신분증 비교",
            "6. 카드 + 얼굴/지문/손바닥 + 비밀번호",
            "7. 카드 + 지문/손바닥 + 얼굴",
            "8. 지문/손바닥 + 얼굴 + 비밀번호",
            "9. 지문 + 손바닥 + 얼굴",
            "10. 손바닥 + 얼굴",
            "11. 지문 + 얼굴",
            "12. 손바닥만",
            "13. 지문만",
            "14. 카드만",
            "15. 비밀번호만",
            "16. 신분증 비교 + 자동등록"
        });
        cmbVerificationType.SelectedIndex = 0;
        AR("인증 방식:", cmbVerificationType);

        txtShortMessage = new TextBox { Width = cw };
        AR("인증 성공 메시지:", txtShortMessage);

        txtVisitorRootPassword = new TextBox { UseSystemPasswordChar = true, Width = cw };
        AR("방문자 루트 비밀번호:", txtVisitorRootPassword);

        numMultiPerson = new NumericUpDown { Minimum = 1, Maximum = 50, Value = 1, Width = cw };
        AR("다중 인증 인원 (명):", numMultiPerson);

        var btnLoadAccess = new Button { Text = "설정 불러오기", Location = new Point(tx, gy), Size = new Size(130, 28) };
        btnLoadAccess.Click += async (s, e) => await LoadAccessControlSettings();
        grpAccess.Controls.Add(btnLoadAccess);

        var btnSaveAccess = new Button { Text = "설정 저장", Location = new Point(tx + 140, gy), Size = new Size(100, 28) };
        btnSaveAccess.Click += BtnSaveAccess_Click;
        grpAccess.Controls.Add(btnSaveAccess);
        gy += 38;

        grpAccess.Height = gy + 10;
        Controls.Add(grpAccess);
        y += grpAccess.Height + 10;

        // ── 로그 창 ───────────────────────────────────────────
        txtResult = new TextBox
        {
            Location = new Point(20, y), Size = new Size(fw, 80),
            Multiline = true, ScrollBars = ScrollBars.Vertical,
            ReadOnly = true, Font = new System.Drawing.Font("Consolas", 9F)
        };
        Controls.Add(txtResult);
        y += 90;

        btnClose = new Button { Text = "닫기", DialogResult = DialogResult.Cancel, Location = new Point(fw - 80, y), Size = new Size(100, 30) };
        Controls.Add(btnClose);
        CancelButton = btnClose;
        ClientSize = new Size(fw + 40, y + 45);
    }

    private static Label MkLabel(string text, int x, int y, int w) =>
        new Label { Text = text, Location = new Point(x, y), Width = w, AutoSize = false };

    private static Button MkBtn(string text, int x, int y, EventHandler h, System.Drawing.Color? fg = null)
    {
        var b = new Button { Text = text, Location = new Point(x, y), Size = new Size(120, 35) };
        if (fg.HasValue) b.ForeColor = fg.Value;
        b.Click += h;
        return b;
    }

    private async void BtnSaveInfo_Click(object? sender, EventArgs e)
    {
        try
        {
            var resp = await _httpClient.PostAsJsonAsync($"/admin/devices/{_device.SN}/update-info",
                new { SN = _device.SN, DeviceName = txtDeviceName.Text.Trim(), TagName = txtTagName.Text.Trim() });
            AddLog(resp.IsSuccessStatusCode
                ? $"[{DateTime.Now:HH:mm:ss}] ? 단말기 정보 저장 완료."
                : $"[{DateTime.Now:HH:mm:ss}] ? 정보 저장 실패: HTTP {resp.StatusCode}");
            if (resp.IsSuccessStatusCode) DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { AddLog($"[{DateTime.Now:HH:mm:ss}] ? 오류: {ex.Message}"); }
    }

    private async void BtnRestart_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("단말기를 재시작하시겠습니까?", "확인",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        await ExecCmd("재시작", "restart");
    }

    private async void BtnOpenDoor_Click(object? sender, EventArgs e) =>
        await ExecCmd("문 열기", "opendoor");

    private void BtnDevicePeople_Click(object? sender, EventArgs e)
    {
        using var form = new DeviceUserListForm(_device, _httpClient);
        form.ShowDialog(this);
    }

    private async void BtnDeleteAllPeople_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show(
            "단말기의 사용자를 전체 삭제하시겠습니까?\n\n이 작업은 단말기의 사용자 정보만 삭제하며\n서버에는 영향을 주지 않습니다.",
            "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        await ExecCmd("사용자 전체삭제", "deleteAllPeople");
    }

    private async void BtnClearRecords_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("단말기의 로그를 전체 삭제하시겠습니까?", "경고",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        await ExecCmd("로그 삭제", "clearRecords");
    }

    private async void BtnRequestUpload_Click(object? sender, EventArgs e) =>
        await ExecCmd("로그 가져오기", "repostRecord");

    private async void BtnSyncTime_Click(object? sender, EventArgs e) =>
        await ExecCmd("시간 동기화", "synctime");

    private async Task LoadAccessControlSettings()
    {
        try
        {
            var resp = await _httpClient.GetAsync($"/admin/devices/{_device.SN}/work-setting");
            if (!resp.IsSuccessStatusCode)
            {
                AddLog($"[{DateTime.Now:HH:mm:ss}] 설정 불러오기 실패: HTTP {resp.StatusCode}");
                return;
            }
            var ws = await resp.Content.ReadFromJsonAsync<JsonObject>();
            if (ws == null) { AddLog($"[{DateTime.Now:HH:mm:ss}] 저장된 출입제어설정이 없습니다."); return; }

            _fullWorkSetting = (JsonObject)ws.DeepClone();

            int Val(string key, int def)
            {
                var node = ws[key];
                if (node is null) return def;
                var raw = node.ToJsonString().Trim('"');
                return int.TryParse(raw, out int v) ? v : def;
            }
            string Str(string key) => ws[key]?.ToJsonString().Trim('"') ?? "";

            numReleaseTime.Value              = Math.Min(Val("ReleaseTime", 3), 65535);
            chkFreeOpen.Checked               = Val("FreeOpen", 0) == 1;
            cmbVerificationType.SelectedIndex = Math.Max(0, Math.Min(Val("VerificationType", 1) - 1, 15));
            txtShortMessage.Text              = Str("ShortMessage");
            txtVisitorRootPassword.Text       = Str("VisitorRootPassword");
            numMultiPerson.Value              = Math.Min(Math.Max(Val("MultiPerson", 1), 1), 50);

            AddLog($"[{DateTime.Now:HH:mm:ss}] ? 출입제어설정을 불러왔습니다.");
        }
        catch (Exception ex) { AddLog($"[{DateTime.Now:HH:mm:ss}] ? 설정 불러오기 오류: {ex.Message}"); }
    }

    private async void BtnSaveAccess_Click(object? sender, EventArgs e)
    {
        try
        {
            JsonObject ws = _fullWorkSetting != null
                ? (JsonObject)_fullWorkSetting.DeepClone()
                : new JsonObject();

            ws["DeviceSN"]            = _device.SN;
            ws["ReleaseTime"]         = (int)numReleaseTime.Value;
            ws["FreeOpen"]            = chkFreeOpen.Checked ? 1 : 0;
            ws["VerificationType"]    = cmbVerificationType.SelectedIndex + 1;
            ws["ShortMessage"]        = txtShortMessage.Text;
            ws["VisitorRootPassword"] = txtVisitorRootPassword.Text;
            ws["MultiPerson"]         = (int)numMultiPerson.Value;

            var resp = await _httpClient.PutAsJsonAsync($"/admin/devices/{_device.SN}/work-setting", ws);
            if (resp.IsSuccessStatusCode)
            {
                await _httpClient.PostAsync($"/admin/devices/{_device.SN}/request-sync", null);
                AddLog($"[{DateTime.Now:HH:mm:ss}] ? 출입제어설정 저장 및 동기화 요청 완료.");
            }
            else AddLog($"[{DateTime.Now:HH:mm:ss}] ? 저장 실패: HTTP {resp.StatusCode}");
        }
        catch (Exception ex) { AddLog($"[{DateTime.Now:HH:mm:ss}] ? 오류: {ex.Message}"); }
    }

    private async Task ExecCmd(string label, string commandType)
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
        catch (Exception ex) { AddLog($"[{DateTime.Now:HH:mm:ss}] ? 오류: {ex.Message}"); }
    }

    private void AddLog(string message)
    {
        if (InvokeRequired) { Invoke(new Action<string>(AddLog), message); return; }
        txtResult.AppendText(message + Environment.NewLine);
        txtResult.SelectionStart = txtResult.Text.Length;
        txtResult.ScrollToCaret();
    }
}
