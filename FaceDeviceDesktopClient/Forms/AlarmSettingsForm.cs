using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace FaceDeviceDesktopClient.Forms;

/// <summary>
/// HTTPv2 프로토콜 - 알람 설정 탭
/// </summary>
public class AlarmSettingsForm : Form
{
    private readonly HttpClient _httpClient;
    private readonly string? _deviceSN;
    private readonly string? _deviceName;

    // Alarm Settings
    private CheckBox chkFireAlarm = null!;
    private CheckBox chkDoorLongOpenAlarm = null!;
    private NumericUpDown numDoorLongOpenTime = null!;
    private CheckBox chkDoorSensorAlarm = null!;

    private Button btnSave = null!;
    private Button btnLoad = null!;

    // 기본 생성자 (전역 메뉴용 - 첫 번째 단말기 선택)
    public AlarmSettingsForm(HttpClient httpClient) : this(httpClient, null, null)
    {
    }

    // 단말기 지정 생성자 (컨텍스트 메뉴용)
    public AlarmSettingsForm(HttpClient httpClient, string? deviceSN, string? deviceName)
    {
        _httpClient = httpClient;
        _deviceSN = deviceSN;
        _deviceName = deviceName;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        var title = string.IsNullOrWhiteSpace(_deviceName) 
            ? "알람 설정" 
            : $"알람 설정 - {_deviceName}";

        Text = title;
        Size = new Size(600, 400);
        StartPosition = FormStartPosition.CenterParent;

        var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };

        int y = 10;

        var grpAlarm = new GroupBox { Text = "알람 설정", Location = new Point(20, y), Size = new Size(540, 200) };
        int gy = 25;

        var lblFireAlarm = new Label { Text = "화재 알람:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkFireAlarm = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "활성화" };
        grpAlarm.Controls.AddRange(new Control[] { lblFireAlarm, chkFireAlarm });
        gy += 35;

        var lblDoorLongOpenAlarm = new Label { Text = "개방 시간 초과 알람:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkDoorLongOpenAlarm = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "활성화" };
        grpAlarm.Controls.AddRange(new Control[] { lblDoorLongOpenAlarm, chkDoorLongOpenAlarm });
        gy += 35;

        var lblDoorLongOpenTime = new Label { Text = "개방 시간 초과 임계값(초):", Location = new Point(20, gy), Size = new Size(200, 25) };
        numDoorLongOpenTime = new NumericUpDown { Location = new Point(230, gy), Size = new Size(150, 25), Minimum = 1, Maximum = 65535, Value = 30 };
        var lblTimeNote = new Label { Text = "이 시간 초과 시 알람 발생", Location = new Point(390, gy), Size = new Size(140, 25), ForeColor = Color.Gray };
        grpAlarm.Controls.AddRange(new Control[] { lblDoorLongOpenTime, numDoorLongOpenTime, lblTimeNote });
        gy += 35;

        var lblDoorSensorAlarm = new Label { Text = "문 센서 알람:", Location = new Point(20, gy), Size = new Size(200, 25) };
        chkDoorSensorAlarm = new CheckBox { Location = new Point(230, gy), Size = new Size(200, 25), Text = "활성화" };
        grpAlarm.Controls.AddRange(new Control[] { lblDoorSensorAlarm, chkDoorSensorAlarm });
        gy += 35;

        panel.Controls.Add(grpAlarm);
        y += 210;

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 60 };

        btnLoad = new Button
        {
            Text = "불러오기",
            Location = new Point(250, 15),
            Size = new Size(100, 35)
        };
        btnLoad.Click += BtnLoad_Click;

        btnSave = new Button
        {
            Text = "저장",
            Location = new Point(360, 15),
            Size = new Size(100, 35)
        };
        btnSave.Click += BtnSave_Click;

        var btnClose = new Button
        {
            Text = "닫기",
            Location = new Point(470, 15),
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

            chkFireAlarm.Checked = workSetting["FireAlarm"]?.GetValue<int>() == 1;
            chkDoorLongOpenAlarm.Checked = workSetting["DoorLongOpenAlarm"]?.GetValue<int>() == 1;
            numDoorLongOpenTime.Value = workSetting["DoorLongOpenTime"]?.GetValue<int>() ?? 30;
            chkDoorSensorAlarm.Checked = workSetting["DoorSensorAlarm"]?.GetValue<int>() == 1;

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
                ["FireAlarm"] = chkFireAlarm.Checked ? 1 : 0,
                ["DoorLongOpenAlarm"] = chkDoorLongOpenAlarm.Checked ? 1 : 0,
                ["DoorLongOpenTime"] = (int)numDoorLongOpenTime.Value,
                ["DoorSensorAlarm"] = chkDoorSensorAlarm.Checked ? 1 : 0
            };

            var saveResponse = await _httpClient.PostAsJsonAsync($"/admin/devices/{deviceSN}/work-setting", workSetting);

            if (saveResponse.IsSuccessStatusCode)
            {
                await _httpClient.PostAsync($"/admin/devices/{deviceSN}/request-sync", null);

                MessageBox.Show(
                    "알람 설정이 저장되었습니다.\n단말기가 다음 Keepalive 시 자동으로 동기화됩니다.",
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
