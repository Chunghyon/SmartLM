using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace FaceDeviceDesktopClient.Forms;

/// <summary>
/// HTTPv2 프로토콜 - 출입 제어 설정 탭
/// </summary>
public class AccessControlSettingsForm : Form
{
    private readonly HttpClient _httpClient;
    private readonly string? _deviceSN;
    private readonly string? _deviceName;

    // Door Control Settings
    private CheckBox chkFreeOpen = null!;
    private NumericUpDown numDelayOpenDoorTime = null!;
    private NumericUpDown numOpenInterval = null!;
    private CheckBox chkOpenIntervalSaveRecord = null!;
    private CheckBox chkRelay = null!;
    private TextBox txtShortMessage = null!;
    private ComboBox cmbVerificationType = null!;

    // Permission Settings
    private CheckBox chkOverdueRemind = null!;
    private NumericUpDown numOverdueRemindDay = null!;

    // Timing Open Settings
    private CheckBox chkTimingOpen = null!;
    private ComboBox cmbTimingOpenMode = null!;
    private TextBox txtTimingOpenTimegroup = null!;

    // Timing Locked Settings
    private CheckBox chkTimingLocked = null!;
    private TextBox txtTimingLockedTimegroup = null!;

    // Visitor Settings
    private TextBox txtVisitorRootPassword = null!;
    private NumericUpDown numMultiPerson = null!;

    private Button btnSave = null!;
    private Button btnLoad = null!;

    // 기본 생성자 (전역 메뉴용 - 첫 번째 단말기 선택)
    public AccessControlSettingsForm(HttpClient httpClient) : this(httpClient, null, null)
    {
    }

    // 단말기 지정 생성자 (컨텍스트 메뉴용)
    public AccessControlSettingsForm(HttpClient httpClient, string? deviceSN, string? deviceName)
    {
        _httpClient = httpClient;
        _deviceSN = deviceSN;
        _deviceName = deviceName;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        var title = string.IsNullOrWhiteSpace(_deviceName) 
            ? "출입 제어 설정" 
            : $"출입 제어 설정 - {_deviceName}";

        Text = title;
        Size = new Size(800, 700);
        StartPosition = FormStartPosition.CenterParent;

        var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };

        int y = 10;

        // Door Control Section
        var grpDoorControl = new GroupBox { Text = "문 제어 설정", Location = new Point(20, y), Size = new Size(740, 280) };
        int gy = 25;

        var lblDelayOpen = new Label { Text = "지연 개방 시간(초):", Location = new Point(20, gy), Size = new Size(200, 25) };
        numDelayOpenDoorTime = new NumericUpDown { Location = new Point(230, gy), Size = new Size(150, 25), Minimum = 0, Maximum = 65535, Value = 0 };
        grpDoorControl.Controls.AddRange(new Control[] { lblDelayOpen, numDelayOpenDoorTime });
        gy += 35;

        var lblFreeOpen = new Label { Text = "무인증 개방:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkFreeOpen = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "활성화" };
        grpDoorControl.Controls.AddRange(new Control[] { lblFreeOpen, chkFreeOpen });
        gy += 35;

        var lblOpenInterval = new Label { Text = "반복 인식 간격(ms):", Location = new Point(20, gy), Size = new Size(200, 25) };
        numOpenInterval = new NumericUpDown { Location = new Point(230, gy), Size = new Size(150, 25), Minimum = 0, Maximum = 65535, Value = 0 };
        var lblOpenIntervalNote = new Label { Text = "0=비활성화", Location = new Point(390, gy), Size = new Size(200, 25), ForeColor = Color.Gray };
        grpDoorControl.Controls.AddRange(new Control[] { lblOpenInterval, numOpenInterval, lblOpenIntervalNote });
        gy += 35;

        var lblSaveRecord = new Label { Text = "반복 간격 기록 저장:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkOpenIntervalSaveRecord = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "저장" };
        grpDoorControl.Controls.AddRange(new Control[] { lblSaveRecord, chkOpenIntervalSaveRecord });
        gy += 35;

        var lblRelay = new Label { Text = "릴레이 양안정 지원:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkRelay = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "지원" };
        grpDoorControl.Controls.AddRange(new Control[] { lblRelay, chkRelay });
        gy += 35;

        var lblShortMessage = new Label { Text = "합법 인증 후 메시지:", Location = new Point(20, gy), Size = new Size(200, 25) };
        txtShortMessage = new TextBox { Location = new Point(230, gy), Size = new Size(480, 25) };
        grpDoorControl.Controls.AddRange(new Control[] { lblShortMessage, txtShortMessage });
        gy += 35;

        var lblVerificationType = new Label { Text = "인증 방식:", Location = new Point(20, gy), Size = new Size(200, 25) };
        cmbVerificationType = new ComboBox { Location = new Point(230, gy), Size = new Size(480, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        cmbVerificationType.Items.AddRange(new object[]
        {
            "1. 표준 모드",
            "2. 얼굴/지문/손바닥/카드+비밀번호",
            "3. 카드+얼굴/지문/손바닥/비밀번호",
            "4. 다인 출석",
            "5. 인물-신분증 비교",
            "6. 카드+얼굴/지문/손바닥+비밀번호",
            "7. 카드+지문/손바닥+얼굴 인식",
            "8. 지문/손바닥+얼굴+비밀번호",
            "9. 지문+손바닥+얼굴",
            "10. 손바닥+얼굴",
            "11. 지문+얼굴 인식",
            "12. 손바닥만 사용",
            "13. 지문만 사용",
            "14. 카드만 사용",
            "15. 비밀번호만 사용",
            "16. 인물-신분증 비교 자동등록"
        });
        cmbVerificationType.SelectedIndex = 0;
        grpDoorControl.Controls.AddRange(new Control[] { lblVerificationType, cmbVerificationType });

        panel.Controls.Add(grpDoorControl);
        y += 290;

        // Permission Settings Section
        var grpPermission = new GroupBox { Text = "권한 설정", Location = new Point(20, y), Size = new Size(740, 100) };
        gy = 25;

        var lblOverdueRemind = new Label { Text = "권한 만료 알림:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkOverdueRemind = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "활성화" };
        grpPermission.Controls.AddRange(new Control[] { lblOverdueRemind, chkOverdueRemind });
        gy += 35;

        var lblOverdueDay = new Label { Text = "만료 알림 임계값(일):", Location = new Point(20, gy), Size = new Size(200, 25) };
        numOverdueRemindDay = new NumericUpDown { Location = new Point(230, gy), Size = new Size(150, 25), Minimum = 0, Maximum = 365, Value = 7 };
        grpPermission.Controls.AddRange(new Control[] { lblOverdueDay, numOverdueRemindDay });

        panel.Controls.Add(grpPermission);
        y += 110;

        // Timing Open Settings Section
        var grpTimingOpen = new GroupBox { Text = "정시 개방 설정", Location = new Point(20, y), Size = new Size(740, 140) };
        gy = 25;

        var lblTimingOpen = new Label { Text = "정시 개방 기능:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkTimingOpen = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "활성화" };
        grpTimingOpen.Controls.AddRange(new Control[] { lblTimingOpen, chkTimingOpen });
        gy += 35;

        var lblTimingOpenMode = new Label { Text = "자동 개방 모드:", Location = new Point(20, gy), Size = new Size(200, 25) };
        cmbTimingOpenMode = new ComboBox { Location = new Point(230, gy), Size = new Size(480, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        cmbTimingOpenMode.Items.AddRange(new object[]
        {
            "1. 합법 인증 후 지정 시간 내 개방",
            "2. 개방 권한 표시된 사용자만 인증 후 개방",
            "3. 자동 스위치 (시간 도달 시 자동 개폐)"
        });
        cmbTimingOpenMode.SelectedIndex = 0;
        grpTimingOpen.Controls.AddRange(new Control[] { lblTimingOpenMode, cmbTimingOpenMode });
        gy += 35;

        var lblTimingOpenTimegroup = new Label { Text = "시간대 (JSON):", Location = new Point(20, gy), Size = new Size(200, 25) };
        txtTimingOpenTimegroup = new TextBox { Location = new Point(230, gy), Size = new Size(480, 25), PlaceholderText = "{\"Week1\":\"09:00-18:00\"}" };
        grpTimingOpen.Controls.AddRange(new Control[] { lblTimingOpenTimegroup, txtTimingOpenTimegroup });

        panel.Controls.Add(grpTimingOpen);
        y += 150;

        // Timing Locked Settings Section
        var grpTimingLocked = new GroupBox { Text = "정시 잠금 설정", Location = new Point(20, y), Size = new Size(740, 100) };
        gy = 25;

        var lblTimingLocked = new Label { Text = "정시 잠금 기능:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkTimingLocked = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "활성화" };
        grpTimingLocked.Controls.AddRange(new Control[] { lblTimingLocked, chkTimingLocked });
        gy += 35;

        var lblTimingLockedTimegroup = new Label { Text = "시간대 (JSON):", Location = new Point(20, gy), Size = new Size(200, 25) };
        txtTimingLockedTimegroup = new TextBox { Location = new Point(230, gy), Size = new Size(480, 25), PlaceholderText = "{\"Week1\":\"20:00-06:00\"}" };
        grpTimingLocked.Controls.AddRange(new Control[] { lblTimingLockedTimegroup, txtTimingLockedTimegroup });

        panel.Controls.Add(grpTimingLocked);
        y += 110;

        // Visitor Settings Section
        var grpVisitor = new GroupBox { Text = "방문객 설정", Location = new Point(20, y), Size = new Size(740, 100) };
        gy = 25;

        var lblVisitorPassword = new Label { Text = "방문객 루트 비밀번호:", Location = new Point(20, gy), Size = new Size(200, 25) };
        txtVisitorRootPassword = new TextBox { Location = new Point(230, gy), Size = new Size(300, 25), UseSystemPasswordChar = true };
        grpVisitor.Controls.AddRange(new Control[] { lblVisitorPassword, txtVisitorRootPassword });
        gy += 35;

        var lblMultiPerson = new Label { Text = "다인 조합 개방 (인원):", Location = new Point(20, gy), Size = new Size(200, 25) };
        numMultiPerson = new NumericUpDown { Location = new Point(230, gy), Size = new Size(150, 25), Minimum = 1, Maximum = 50, Value = 1 };
        grpVisitor.Controls.AddRange(new Control[] { lblMultiPerson, numMultiPerson });

        panel.Controls.Add(grpVisitor);
        y += 110;

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 60 };

        btnLoad = new Button
        {
            Text = "불러오기",
            Location = new Point(450, 15),
            Size = new Size(100, 35)
        };
        btnLoad.Click += BtnLoad_Click;

        btnSave = new Button
        {
            Text = "저장",
            Location = new Point(560, 15),
            Size = new Size(100, 35)
        };
        btnSave.Click += BtnSave_Click;

        var btnClose = new Button
        {
            Text = "닫기",
            Location = new Point(670, 15),
            Size = new Size(100, 35),
            DialogResult = DialogResult.Cancel
        };

        pnlButtons.Controls.AddRange(new Control[] { btnLoad, btnSave, btnClose });

        Controls.Add(panel);
        Controls.Add(pnlButtons);

        AcceptButton = btnSave;
        CancelButton = btnClose;
    }

    private async void BtnLoad_Click(object? sender, EventArgs e)
    {
        try
        {
            string deviceSN;

            // 단말기가 지정된 경우 해당 단말기 사용, 아니면 첫 번째 단말기 사용
            if (!string.IsNullOrWhiteSpace(_deviceSN))
            {
                deviceSN = _deviceSN;
            }
            else
            {
                var response = await _httpClient.GetAsync("/admin/devices");
                if (!response.IsSuccessStatusCode) return;

                var devices = await response.Content.ReadFromJsonAsync<List<DeviceSummary>>();
                if (devices == null || devices.Count == 0)
                {
                    MessageBox.Show("등록된 단말기가 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                deviceSN = devices[0].SN;
            }

            var settingResponse = await _httpClient.GetAsync($"/admin/devices/{deviceSN}");
            if (!settingResponse.IsSuccessStatusCode) return;

            var deviceSnapshot = await settingResponse.Content.ReadFromJsonAsync<JsonObject>();
            var workSetting = deviceSnapshot?["LastUploadedWorkSetting"]?.AsObject() 
                           ?? deviceSnapshot?["DesiredWorkSetting"]?.AsObject();

            if (workSetting == null)
            {
                MessageBox.Show("작업 설정 데이터가 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Load door control settings
            numDelayOpenDoorTime.Value = workSetting["DelayOpenDoorTime"]?.GetValue<int>() ?? 0;
            chkFreeOpen.Checked = workSetting["FreeOpen"]?.GetValue<int>() == 1;
            numOpenInterval.Value = workSetting["OpenInterval"]?.GetValue<int>() ?? 0;
            chkOpenIntervalSaveRecord.Checked = workSetting["OpenInterval_SaveRecord"]?.GetValue<int>() == 1;
            chkRelay.Checked = workSetting["Relay"]?.GetValue<int>() == 1;
            txtShortMessage.Text = workSetting["ShortMessage"]?.GetValue<string>() ?? "";

            var verificationType = workSetting["VerificationType"]?.GetValue<int>() ?? 1;
            cmbVerificationType.SelectedIndex = Math.Max(0, Math.Min(verificationType - 1, 15));

            // Load permission settings
            chkOverdueRemind.Checked = workSetting["OverdueRemind"]?.GetValue<int>() == 1;
            numOverdueRemindDay.Value = workSetting["OverdueRemind_Day"]?.GetValue<int>() ?? 7;

            // Load timing open settings
            chkTimingOpen.Checked = workSetting["TimingOpen"]?.GetValue<int>() == 1;
            cmbTimingOpenMode.SelectedIndex = (workSetting["TimingOpen_mode"]?.GetValue<int>() ?? 1) - 1;
            txtTimingOpenTimegroup.Text = workSetting["TimingOpen_timegroup"]?.ToJsonString() ?? "";

            // Load timing locked settings
            chkTimingLocked.Checked = workSetting["TimingLocked"]?.GetValue<int>() == 1;
            txtTimingLockedTimegroup.Text = workSetting["TimingLocked_timegroup"]?.ToJsonString() ?? "";

            // Load visitor settings
            txtVisitorRootPassword.Text = workSetting["VisitorRootPassword"]?.GetValue<string>() ?? "";
            numMultiPerson.Value = workSetting["MultiPerson"]?.GetValue<int>() ?? 1;

            MessageBox.Show("설정을 불러왔습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"설정 불러오기 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            string deviceSN;

            // 단말기가 지정된 경우 해당 단말기 사용, 아니면 첫 번째 단말기 사용
            if (!string.IsNullOrWhiteSpace(_deviceSN))
            {
                deviceSN = _deviceSN;
            }
            else
            {
                var response = await _httpClient.GetAsync("/admin/devices");
                if (!response.IsSuccessStatusCode) return;

                var devices = await response.Content.ReadFromJsonAsync<List<DeviceSummary>>();
                if (devices == null || devices.Count == 0)
                {
                    MessageBox.Show("등록된 단말기가 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                deviceSN = devices[0].SN;
            }

            var workSetting = new JsonObject
            {
                ["DeviceSN"] = deviceSN,

                // Door control settings
                ["DelayOpenDoorTime"] = (int)numDelayOpenDoorTime.Value,
                ["FreeOpen"] = chkFreeOpen.Checked ? 1 : 0,
                ["OpenInterval"] = (int)numOpenInterval.Value,
                ["OpenInterval_SaveRecord"] = chkOpenIntervalSaveRecord.Checked ? 1 : 0,
                ["Relay"] = chkRelay.Checked ? 1 : 0,
                ["ShortMessage"] = txtShortMessage.Text,
                ["VerificationType"] = cmbVerificationType.SelectedIndex + 1,

                // Permission settings
                ["OverdueRemind"] = chkOverdueRemind.Checked ? 1 : 0,
                ["OverdueRemind_Day"] = (int)numOverdueRemindDay.Value,

                // Timing open settings
                ["TimingOpen"] = chkTimingOpen.Checked ? 1 : 0,
                ["TimingOpen_mode"] = cmbTimingOpenMode.SelectedIndex + 1,

                // Timing locked settings
                ["TimingLocked"] = chkTimingLocked.Checked ? 1 : 0,

                // Visitor settings
                ["VisitorRootPassword"] = txtVisitorRootPassword.Text,
                ["MultiPerson"] = (int)numMultiPerson.Value
            };

            // Parse timegroup JSON if provided
            if (!string.IsNullOrWhiteSpace(txtTimingOpenTimegroup.Text))
            {
                try
                {
                    workSetting["TimingOpen_timegroup"] = JsonNode.Parse(txtTimingOpenTimegroup.Text);
                }
                catch
                {
                    workSetting["TimingOpen_timegroup"] = new JsonObject();
                }
            }

            if (!string.IsNullOrWhiteSpace(txtTimingLockedTimegroup.Text))
            {
                try
                {
                    workSetting["TimingLocked_timegroup"] = JsonNode.Parse(txtTimingLockedTimegroup.Text);
                }
                catch
                {
                    workSetting["TimingLocked_timegroup"] = new JsonObject();
                }
            }

            var saveResponse = await _httpClient.PostAsJsonAsync($"/admin/devices/{deviceSN}/work-setting", workSetting);

            if (saveResponse.IsSuccessStatusCode)
            {
                await _httpClient.PostAsync($"/admin/devices/{deviceSN}/request-sync", null);

                MessageBox.Show(
                    "출입 제어 설정이 저장되었습니다.\n단말기가 다음 Keepalive 시 자동으로 동기화됩니다.",
                    "저장 완료",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("설정 저장에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"설정 저장 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
