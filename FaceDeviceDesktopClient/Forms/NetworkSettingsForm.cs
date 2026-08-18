using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace FaceDeviceDesktopClient.Forms;

/// <summary>
/// HTTPv2 프로토콜 - 네트워크 설정 탭
/// </summary>
public class NetworkSettingsForm : Form
{
    private readonly HttpClient _httpClient;
    private readonly string? _deviceSN;
    private readonly string? _deviceName;
    private TabControl tabControl = null!;

    // UDP Client Settings
    private CheckBox chkUseUDPClient = null!;
    private TextBox txtServerAddress = null!;
    private NumericUpDown numServerPort = null!;
    private NumericUpDown numKeepaliveTime = null!;

    // HTTP Client Settings
    private CheckBox chkUseHTTPClient = null!;
    private ComboBox cmbHTTPProtocolType = null!;
    private TextBox txtHTTPServerAddr = null!;
    private NumericUpDown numHTTPKeepaliveTime = null!;
    private CheckBox chkHTTPUseGZIP = null!;

    // MQTT Client Settings
    private CheckBox chkUseMQTTClient = null!;
    private CheckBox chkUseMQTTSSL = null!;
    private TextBox txtMQTTServerAddr = null!;
    private NumericUpDown numMQTTPort = null!;
    private TextBox txtMQTTLoginPassword = null!;
    private TextBox txtMQTTPublishTopic = null!;
    private TextBox txtMQTTSubscribeTopic = null!;
    private NumericUpDown numMQTTKeepaliveTime = null!;
    private CheckBox chkMQTTUseGZIP = null!;

    // WebSocket Client Settings
    private CheckBox chkUseWebsocketClient = null!;
    private ComboBox cmbWebsocketProtocolType = null!;
    private TextBox txtWebsocketServerAddr = null!;
    private CheckBox chkWebsocketUseGZIP = null!;
    private NumericUpDown numWebsocketKeepaliveTime = null!;

    // Yunzhu Platform
    private CheckBox chkUseYZW = null!;
    private TextBox txtYZWAddr = null!;

    private Button btnSave = null!;
    private Button btnLoad = null!;

    // 기본 생성자 (전역 메뉴용 - 첫 번째 단말기 선택)
    public NetworkSettingsForm(HttpClient httpClient) : this(httpClient, null, null)
    {
    }

    // 단말기 지정 생성자 (컨텍스트 메뉴용)
    public NetworkSettingsForm(HttpClient httpClient, string? deviceSN, string? deviceName)
    {
        _httpClient = httpClient;
        _deviceSN = deviceSN;
        _deviceName = deviceName;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        var title = string.IsNullOrWhiteSpace(_deviceName) 
            ? "네트워크 설정" 
            : $"네트워크 설정 - {_deviceName}";

        Text = title;
        Size = new Size(900, 700);
        StartPosition = FormStartPosition.CenterParent;

        tabControl = new TabControl
        {
            Dock = DockStyle.Fill
        };

        tabControl.TabPages.Add(CreateUDPClientTab());
        tabControl.TabPages.Add(CreateHTTPClientTab());
        tabControl.TabPages.Add(CreateMQTTClientTab());
        tabControl.TabPages.Add(CreateWebSocketClientTab());
        tabControl.TabPages.Add(CreateYunzhuTab());

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 60 };

        btnLoad = new Button
        {
            Text = "불러오기",
            Location = new Point(550, 15),
            Size = new Size(100, 35)
        };
        btnLoad.Click += BtnLoad_Click;

        btnSave = new Button
        {
            Text = "저장",
            Location = new Point(660, 15),
            Size = new Size(100, 35)
        };
        btnSave.Click += BtnSave_Click;

        var btnClose = new Button
        {
            Text = "닫기",
            Location = new Point(770, 15),
            Size = new Size(100, 35),
            DialogResult = DialogResult.Cancel
        };

        pnlButtons.Controls.AddRange(new Control[] { btnLoad, btnSave, btnClose });

        Controls.Add(tabControl);
        Controls.Add(pnlButtons);

        AcceptButton = btnSave;
        CancelButton = btnClose;
    }

    private TabPage CreateUDPClientTab()
    {
        var tab = new TabPage("UDP Client");
        var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };

        int y = 10;

        // Use UDP Client
        var lblUseUDP = new Label { Text = "UDP Client 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkUseUDPClient = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblUseUDP, chkUseUDPClient });
        y += 35;

        // Server Address
        var lblServerAddr = new Label { Text = "서버 주소:", Location = new Point(20, y), Size = new Size(150, 25) };
        txtServerAddress = new TextBox { Location = new Point(180, y), Size = new Size(300, 25) };
        panel.Controls.AddRange(new Control[] { lblServerAddr, txtServerAddress });
        y += 35;

        // Server Port
        var lblServerPort = new Label { Text = "서버 포트:", Location = new Point(20, y), Size = new Size(150, 25) };
        numServerPort = new NumericUpDown { Location = new Point(180, y), Size = new Size(150, 25), Minimum = 1, Maximum = 65535, Value = 9003 };
        panel.Controls.AddRange(new Control[] { lblServerPort, numServerPort });
        y += 35;

        // Keepalive Time
        var lblKeepalive = new Label { Text = "Keepalive 시간(초):", Location = new Point(20, y), Size = new Size(150, 25) };
        numKeepaliveTime = new NumericUpDown { Location = new Point(180, y), Size = new Size(150, 25), Minimum = 1, Maximum = 3600, Value = 30 };
        panel.Controls.AddRange(new Control[] { lblKeepalive, numKeepaliveTime });
        y += 35;

        tab.Controls.Add(panel);
        return tab;
    }

    private TabPage CreateHTTPClientTab()
    {
        var tab = new TabPage("HTTP Client");
        var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };

        int y = 10;

        // Use HTTP Client
        var lblUseHTTP = new Label { Text = "HTTP Client 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkUseHTTPClient = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblUseHTTP, chkUseHTTPClient });
        y += 35;

        // Protocol Type
        var lblProtocolType = new Label { Text = "프로토콜 타입:", Location = new Point(20, y), Size = new Size(150, 25) };
        cmbHTTPProtocolType = new ComboBox { Location = new Point(180, y), Size = new Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        cmbHTTPProtocolType.Items.AddRange(new object[] { "HTTPv1 (100)", "HTTPv2 (200)" });
        cmbHTTPProtocolType.SelectedIndex = 1;
        panel.Controls.AddRange(new Control[] { lblProtocolType, cmbHTTPProtocolType });
        y += 35;

        // Server Address
        var lblHTTPAddr = new Label { Text = "서버 주소:", Location = new Point(20, y), Size = new Size(150, 25) };
        txtHTTPServerAddr = new TextBox { Location = new Point(180, y), Size = new Size(400, 25), Text = "http://192.168.1.100" };
        panel.Controls.AddRange(new Control[] { lblHTTPAddr, txtHTTPServerAddr });
        y += 35;

        // Keepalive Time
        var lblHTTPKeepalive = new Label { Text = "Keepalive 시간(초):", Location = new Point(20, y), Size = new Size(150, 25) };
        numHTTPKeepaliveTime = new NumericUpDown { Location = new Point(180, y), Size = new Size(150, 25), Minimum = 1, Maximum = 3600, Value = 30 };
        panel.Controls.AddRange(new Control[] { lblHTTPKeepalive, numHTTPKeepaliveTime });
        y += 35;

        // Use GZIP
        var lblHTTPGZIP = new Label { Text = "GZIP 압축 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkHTTPUseGZIP = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblHTTPGZIP, chkHTTPUseGZIP });
        y += 35;

        tab.Controls.Add(panel);
        return tab;
    }

    private TabPage CreateMQTTClientTab()
    {
        var tab = new TabPage("MQTT Client");
        var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };

        int y = 10;

        // Use MQTT Client
        var lblUseMQTT = new Label { Text = "MQTT Client 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkUseMQTTClient = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblUseMQTT, chkUseMQTTClient });
        y += 35;

        // Use SSL
        var lblMQTTSSL = new Label { Text = "SSL 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkUseMQTTSSL = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblMQTTSSL, chkUseMQTTSSL });
        y += 35;

        // Server Address
        var lblMQTTAddr = new Label { Text = "서버 주소:", Location = new Point(20, y), Size = new Size(150, 25) };
        txtMQTTServerAddr = new TextBox { Location = new Point(180, y), Size = new Size(300, 25), Text = "192.168.1.100" };
        panel.Controls.AddRange(new Control[] { lblMQTTAddr, txtMQTTServerAddr });
        y += 35;

        // Port
        var lblMQTTPort = new Label { Text = "포트:", Location = new Point(20, y), Size = new Size(150, 25) };
        numMQTTPort = new NumericUpDown { Location = new Point(180, y), Size = new Size(150, 25), Minimum = 0, Maximum = 65535, Value = 1883 };
        panel.Controls.AddRange(new Control[] { lblMQTTPort, numMQTTPort });
        y += 35;

        // Login Password
        var lblPassword = new Label { Text = "로그인 비밀번호:", Location = new Point(20, y), Size = new Size(150, 25) };
        txtMQTTLoginPassword = new TextBox { Location = new Point(180, y), Size = new Size(300, 25), UseSystemPasswordChar = true };
        panel.Controls.AddRange(new Control[] { lblPassword, txtMQTTLoginPassword });
        y += 35;

        // Publish Topic
        var lblPublishTopic = new Label { Text = "Publish Topic:", Location = new Point(20, y), Size = new Size(150, 25) };
        txtMQTTPublishTopic = new TextBox { Location = new Point(180, y), Size = new Size(400, 25) };
        panel.Controls.AddRange(new Control[] { lblPublishTopic, txtMQTTPublishTopic });
        y += 35;

        // Subscribe Topic
        var lblSubscribeTopic = new Label { Text = "Subscribe Topic:", Location = new Point(20, y), Size = new Size(150, 25) };
        txtMQTTSubscribeTopic = new TextBox { Location = new Point(180, y), Size = new Size(400, 25) };
        panel.Controls.AddRange(new Control[] { lblSubscribeTopic, txtMQTTSubscribeTopic });
        y += 35;

        // Keepalive Time
        var lblMQTTKeepalive = new Label { Text = "Keepalive 시간(초):", Location = new Point(20, y), Size = new Size(150, 25) };
        numMQTTKeepaliveTime = new NumericUpDown { Location = new Point(180, y), Size = new Size(150, 25), Minimum = 1, Maximum = 3600, Value = 30 };
        panel.Controls.AddRange(new Control[] { lblMQTTKeepalive, numMQTTKeepaliveTime });
        y += 35;

        // Use GZIP
        var lblMQTTGZIP = new Label { Text = "GZIP 압축 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkMQTTUseGZIP = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblMQTTGZIP, chkMQTTUseGZIP });
        y += 35;

        tab.Controls.Add(panel);
        return tab;
    }

    private TabPage CreateWebSocketClientTab()
    {
        var tab = new TabPage("WebSocket Client");
        var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };

        int y = 10;

        // Use WebSocket Client
        var lblUseWS = new Label { Text = "WebSocket 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkUseWebsocketClient = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblUseWS, chkUseWebsocketClient });
        y += 35;

        // Protocol Type
        var lblWSProtocolType = new Label { Text = "프로토콜 타입:", Location = new Point(20, y), Size = new Size(150, 25) };
        cmbWebsocketProtocolType = new ComboBox { Location = new Point(180, y), Size = new Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        cmbWebsocketProtocolType.Items.AddRange(new object[] { "WebSocketv1 (100)", "WebSocketv2 (200)" });
        cmbWebsocketProtocolType.SelectedIndex = 1;
        panel.Controls.AddRange(new Control[] { lblWSProtocolType, cmbWebsocketProtocolType });
        y += 35;

        // Server Address
        var lblWSAddr = new Label { Text = "서버 주소:", Location = new Point(20, y), Size = new Size(150, 25) };
        txtWebsocketServerAddr = new TextBox { Location = new Point(180, y), Size = new Size(400, 25), Text = "ws://192.168.1.100/ws" };
        panel.Controls.AddRange(new Control[] { lblWSAddr, txtWebsocketServerAddr });
        y += 35;

        // Use GZIP
        var lblWSGZIP = new Label { Text = "GZIP 압축 사용:", Location = new Point(20, y), Size = new Size(150, 25) };
        chkWebsocketUseGZIP = new CheckBox { Location = new Point(180, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblWSGZIP, chkWebsocketUseGZIP });
        y += 35;

        // Keepalive Time
        var lblWSKeepalive = new Label { Text = "Keepalive 시간(초):", Location = new Point(20, y), Size = new Size(150, 25) };
        numWebsocketKeepaliveTime = new NumericUpDown { Location = new Point(180, y), Size = new Size(150, 25), Minimum = 1, Maximum = 3600, Value = 30 };
        panel.Controls.AddRange(new Control[] { lblWSKeepalive, numWebsocketKeepaliveTime });
        y += 35;

        tab.Controls.Add(panel);
        return tab;
    }

    private TabPage CreateYunzhuTab()
    {
        var tab = new TabPage("Yunzhu Platform");
        var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20) };

        int y = 10;

        // Use Yunzhu
        var lblUseYZW = new Label { Text = "Yunzhu Platform 사용:", Location = new Point(20, y), Size = new Size(180, 25) };
        chkUseYZW = new CheckBox { Location = new Point(210, y), Size = new Size(200, 25) };
        panel.Controls.AddRange(new Control[] { lblUseYZW, chkUseYZW });
        y += 35;

        // YZW Address
        var lblYZWAddr = new Label { Text = "YZW 서버 주소:", Location = new Point(20, y), Size = new Size(180, 25) };
        txtYZWAddr = new TextBox { Location = new Point(210, y), Size = new Size(400, 25), Text = "http://192.168.1.10" };
        panel.Controls.AddRange(new Control[] { lblYZWAddr, txtYZWAddr });
        y += 35;

        tab.Controls.Add(panel);
        return tab;
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
                // Get first device
                var response = await _httpClient.GetAsync("/admin/devices");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("단말기 목록을 가져올 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var devices = await response.Content.ReadFromJsonAsync<List<DeviceSummary>>();
                if (devices == null || devices.Count == 0)
                {
                    MessageBox.Show("등록된 단말기가 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                deviceSN = devices[0].SN;
            }

            // Get device work setting
            var settingResponse = await _httpClient.GetAsync($"/admin/devices/{deviceSN}");
            if (!settingResponse.IsSuccessStatusCode)
            {
                MessageBox.Show("단말기 설정을 가져올 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var deviceSnapshot = await settingResponse.Content.ReadFromJsonAsync<JsonObject>();
            if (deviceSnapshot == null)
            {
                MessageBox.Show("단말기 설정 데이터가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var workSetting = deviceSnapshot["LastUploadedWorkSetting"]?.AsObject() 
                           ?? deviceSnapshot["DesiredWorkSetting"]?.AsObject();

            if (workSetting == null)
            {
                MessageBox.Show("작업 설정 데이터가 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Load UDP settings
            chkUseUDPClient.Checked = workSetting["UseUDPClient"]?.GetValue<int>() == 1;
            txtServerAddress.Text = workSetting["ServerAddress"]?.GetValue<string>() ?? "";
            numServerPort.Value = workSetting["ServerPort"]?.GetValue<int>() ?? 9003;
            numKeepaliveTime.Value = workSetting["KeepaliveTime"]?.GetValue<int>() ?? 30;

            // Load HTTP settings
            chkUseHTTPClient.Checked = workSetting["UseHTTPClient"]?.GetValue<int>() == 1;
            var httpProtocolType = workSetting["HTTPClient_ProtocolType"]?.GetValue<int>() ?? 200;
            cmbHTTPProtocolType.SelectedIndex = httpProtocolType == 100 ? 0 : 1;
            txtHTTPServerAddr.Text = workSetting["HTTPClient_ServerAddr"]?.GetValue<string>() ?? "";
            numHTTPKeepaliveTime.Value = workSetting["HTTPClient_KeepaliveTime"]?.GetValue<int>() ?? 30;
            chkHTTPUseGZIP.Checked = workSetting["HTTPClient_UseGZIP"]?.GetValue<int>() == 1;

            // Load MQTT settings
            chkUseMQTTClient.Checked = workSetting["UseMQTTClient"]?.GetValue<int>() == 1;
            chkUseMQTTSSL.Checked = workSetting["UseMQTTSSL"]?.GetValue<int>() == 1;
            txtMQTTServerAddr.Text = workSetting["MQTTServerAddr"]?.GetValue<string>() ?? "";
            numMQTTPort.Value = workSetting["MQTTPort"]?.GetValue<int>() ?? 1883;
            txtMQTTLoginPassword.Text = workSetting["MQTTLoginPassword"]?.GetValue<string>() ?? "";
            txtMQTTPublishTopic.Text = workSetting["MQTTPublishTopic"]?.GetValue<string>() ?? "";
            txtMQTTSubscribeTopic.Text = workSetting["MQTTSubscribeTopic"]?.GetValue<string>() ?? "";
            numMQTTKeepaliveTime.Value = workSetting["MQTT_KeepaliveTime"]?.GetValue<int>() ?? 30;
            chkMQTTUseGZIP.Checked = workSetting["MQTT_UseGZIP"]?.GetValue<int>() == 1;

            // Load WebSocket settings
            chkUseWebsocketClient.Checked = workSetting["UseWebsocketClient"]?.GetValue<int>() == 1;
            var wsProtocolType = workSetting["WebsocketClient_ProtocolType"]?.GetValue<int>() ?? 200;
            cmbWebsocketProtocolType.SelectedIndex = wsProtocolType == 100 ? 0 : 1;
            txtWebsocketServerAddr.Text = workSetting["WebsocketClient_ServerAddr"]?.GetValue<string>() ?? "";
            chkWebsocketUseGZIP.Checked = workSetting["WebsocketClient_UseGZIP"]?.GetValue<int>() == 1;
            numWebsocketKeepaliveTime.Value = workSetting["WebsocketClient_KeepaliveTime"]?.GetValue<int>() ?? 30;

            // Load Yunzhu settings
            chkUseYZW.Checked = workSetting["UseYZW"]?.GetValue<int>() == 1;
            txtYZWAddr.Text = workSetting["YZWAddr"]?.GetValue<string>() ?? "";

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
                // Get first device
                var response = await _httpClient.GetAsync("/admin/devices");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("단말기 목록을 가져올 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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

                // UDP settings
                ["UseUDPClient"] = chkUseUDPClient.Checked ? 1 : 0,
                ["ServerAddress"] = txtServerAddress.Text,
                ["ServerPort"] = (int)numServerPort.Value,
                ["KeepaliveTime"] = (int)numKeepaliveTime.Value,

                // HTTP settings
                ["UseHTTPClient"] = chkUseHTTPClient.Checked ? 1 : 0,
                ["HTTPClient_ProtocolType"] = cmbHTTPProtocolType.SelectedIndex == 0 ? 100 : 200,
                ["HTTPClient_ServerAddr"] = txtHTTPServerAddr.Text,
                ["HTTPClient_KeepaliveTime"] = (int)numHTTPKeepaliveTime.Value,
                ["HTTPClient_UseGZIP"] = chkHTTPUseGZIP.Checked ? 1 : 0,

                // MQTT settings
                ["UseMQTTClient"] = chkUseMQTTClient.Checked ? 1 : 0,
                ["UseMQTTSSL"] = chkUseMQTTSSL.Checked ? 1 : 0,
                ["MQTTServerAddr"] = txtMQTTServerAddr.Text,
                ["MQTTPort"] = (int)numMQTTPort.Value,
                ["MQTTLoginPassword"] = txtMQTTLoginPassword.Text,
                ["MQTTPublishTopic"] = txtMQTTPublishTopic.Text,
                ["MQTTSubscribeTopic"] = txtMQTTSubscribeTopic.Text,
                ["MQTT_KeepaliveTime"] = (int)numMQTTKeepaliveTime.Value,
                ["MQTT_UseGZIP"] = chkMQTTUseGZIP.Checked ? 1 : 0,

                // WebSocket settings
                ["UseWebsocketClient"] = chkUseWebsocketClient.Checked ? 1 : 0,
                ["WebsocketClient_ProtocolType"] = cmbWebsocketProtocolType.SelectedIndex == 0 ? 100 : 200,
                ["WebsocketClient_ServerAddr"] = txtWebsocketServerAddr.Text,
                ["WebsocketClient_UseGZIP"] = chkWebsocketUseGZIP.Checked ? 1 : 0,
                ["WebsocketClient_KeepaliveTime"] = (int)numWebsocketKeepaliveTime.Value,

                // Yunzhu settings
                ["UseYZW"] = chkUseYZW.Checked ? 1 : 0,
                ["YZWAddr"] = txtYZWAddr.Text
            };

            var saveResponse = await _httpClient.PostAsJsonAsync($"/admin/devices/{deviceSN}/work-setting", workSetting);

            if (saveResponse.IsSuccessStatusCode)
            {
                // Request sync to device
                await _httpClient.PostAsync($"/admin/devices/{deviceSN}/request-sync", null);

                MessageBox.Show(
                    "네트워크 설정이 저장되었습니다.\n단말기가 다음 Keepalive 시 자동으로 동기화됩니다.",
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

public class DeviceSummary
{
    public string SN { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public int HttpPort { get; set; }
    public string? DeviceName { get; set; }
    public string? TagName { get; set; }
}
