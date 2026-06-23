using System.Net.Http.Json;
using System.Text.Json;

namespace FaceDeviceDesktopClient.Forms;

public partial class DeviceDetailForm : Form
{
    private readonly DeviceInfo _device;
    private readonly HttpClient _httpClient;

    // UI Controls
    private TextBox txtDeviceName = null!;
    private TextBox txtTagName = null!;
    private CheckedListBox lstAssignedUsers = null!;
    private Button btnAddUser = null!;
    private Button btnRemoveUser = null!;
    private Button btnUpload = null!;
    private Button btnDownload = null!;
    private Button btnInitialize = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;

    private List<PersonInfo> _allUsers = new();
    private List<PersonInfo> _assignedUsers = new();

    public DeviceDetailForm(DeviceInfo device, HttpClient httpClient)
    {
        _device = device;
        _httpClient = httpClient;

        InitializeComponent();
        _ = LoadData();
    }

    private void InitializeComponent()
    {
        this.Text = $"단말기 수정 - {_device.DeviceName}";
        this.Size = new Size(700, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            Padding = new Padding(20)
        };
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

        // 1. 단말기명
        var pnlDeviceName = new Panel { Dock = DockStyle.Fill };
        pnlDeviceName.Controls.Add(new Label 
        { 
            Text = "단말기명:", 
            Location = new Point(0, 10), 
            Width = 100 
        });
        txtDeviceName = new TextBox 
        { 
            Location = new Point(110, 8), 
            Width = 500,
            Text = _device.DeviceName 
        };
        pnlDeviceName.Controls.Add(txtDeviceName);
        mainPanel.Controls.Add(pnlDeviceName, 0, 0);

        // 2. 위치
        var pnlLocation = new Panel { Dock = DockStyle.Fill };
        pnlLocation.Controls.Add(new Label 
        { 
            Text = "위치:", 
            Location = new Point(0, 10), 
            Width = 100 
        });
        txtTagName = new TextBox 
        { 
            Location = new Point(110, 8), 
            Width = 500,
            Text = _device.TagName 
        };
        pnlLocation.Controls.Add(txtTagName);
        mainPanel.Controls.Add(pnlLocation, 0, 1);

        // 3. 출입자 할당
        var grpUsers = new GroupBox 
        { 
            Text = "출입자 할당", 
            Dock = DockStyle.Fill 
        };

        lstAssignedUsers = new CheckedListBox
        {
            Location = new Point(10, 25),
            Size = new Size(540, 250),
            CheckOnClick = true
        };
        grpUsers.Controls.Add(lstAssignedUsers);

        btnAddUser = new Button
        {
            Text = "추가",
            Location = new Point(560, 25),
            Size = new Size(90, 30)
        };
        btnAddUser.Click += BtnAddUser_Click;
        grpUsers.Controls.Add(btnAddUser);

        btnRemoveUser = new Button
        {
            Text = "제거",
            Location = new Point(560, 65),
            Size = new Size(90, 30),
            ForeColor = Color.DarkRed
        };
        btnRemoveUser.Click += BtnRemoveUser_Click;
        grpUsers.Controls.Add(btnRemoveUser);

        mainPanel.Controls.Add(grpUsers, 0, 2);

        // 4. 업로드
        var pnlUpload = new Panel { Dock = DockStyle.Fill };
        btnUpload = new Button
        {
            Text = "업로드 (할당된 사용자를 단말기로 전송)",
            Location = new Point(10, 10),
            Size = new Size(640, 35)
        };
        btnUpload.Click += BtnUpload_Click;
        pnlUpload.Controls.Add(btnUpload);
        mainPanel.Controls.Add(pnlUpload, 0, 3);

        // 5. 다운로드
        var pnlDownload = new Panel { Dock = DockStyle.Fill };
        btnDownload = new Button
        {
            Text = "다운로드 (단말기로부터 사용자 정보 수신)",
            Location = new Point(10, 5),
            Size = new Size(640, 35)
        };
        btnDownload.Click += BtnDownload_Click;
        pnlDownload.Controls.Add(btnDownload);
        mainPanel.Controls.Add(pnlDownload, 0, 4);

        // 6. 하단 버튼
        var pnlButtons = new Panel { Dock = DockStyle.Fill };

        btnInitialize = new Button
        {
            Text = "초기화",
            Location = new Point(10, 15),
            Size = new Size(100, 35),
            ForeColor = Color.DarkRed
        };
        btnInitialize.Click += BtnInitialize_Click;
        pnlButtons.Controls.Add(btnInitialize);

        btnSave = new Button
        {
            Text = "저장",
            Location = new Point(430, 15),
            Size = new Size(100, 35)
        };
        btnSave.Click += BtnSave_Click;
        pnlButtons.Controls.Add(btnSave);

        btnCancel = new Button
        {
            Text = "취소",
            Location = new Point(540, 15),
            Size = new Size(100, 35),
            DialogResult = DialogResult.Cancel
        };
        pnlButtons.Controls.Add(btnCancel);

        mainPanel.Controls.Add(pnlButtons, 0, 5);

        this.Controls.Add(mainPanel);
        this.AcceptButton = btnSave;
        this.CancelButton = btnCancel;
    }

    private async Task LoadData()
    {
        try
        {
            // Load all users
            var usersResponse = await _httpClient.GetFromJsonAsync<List<PersonInfo>>("/admin/people");
            if (usersResponse != null)
            {
                _allUsers = usersResponse;
            }

            // Load assigned users for this device (placeholder - would need API support)
            // For now, show all users and let admin assign them
            RefreshUserList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"데이터 로드 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RefreshUserList()
    {
        lstAssignedUsers.Items.Clear();
        foreach (var user in _assignedUsers)
        {
            lstAssignedUsers.Items.Add($"{user.UserID} - {user.Name}", true);
        }
    }

    private void BtnAddUser_Click(object? sender, EventArgs e)
    {
        using var selectForm = new Form
        {
            Text = "사용자 선택",
            Size = new Size(500, 400),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var lstUsers = new CheckedListBox
        {
            Dock = DockStyle.Fill,
            CheckOnClick = true
        };

        var assignedUserIds = _assignedUsers.Select(u => u.UserID).ToHashSet();
        foreach (var user in _allUsers)
        {
            if (!assignedUserIds.Contains(user.UserID))
            {
                lstUsers.Items.Add($"{user.UserID} - {user.Name}");
            }
        }

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 50 };
        var btnOK = new Button
        {
            Text = "확인",
            Location = new Point(280, 10),
            Size = new Size(90, 30),
            DialogResult = DialogResult.OK
        };
        var btnCancelSelect = new Button
        {
            Text = "취소",
            Location = new Point(380, 10),
            Size = new Size(90, 30),
            DialogResult = DialogResult.Cancel
        };
        pnlButtons.Controls.Add(btnOK);
        pnlButtons.Controls.Add(btnCancelSelect);

        selectForm.Controls.Add(lstUsers);
        selectForm.Controls.Add(pnlButtons);

        if (selectForm.ShowDialog() == DialogResult.OK)
        {
            foreach (var item in lstUsers.CheckedItems)
            {
                var text = item.ToString();
                var userId = text?.Split('-')[0].Trim();
                var user = _allUsers.FirstOrDefault(u => u.UserID == userId);
                if (user != null && !_assignedUsers.Any(u => u.UserID == userId))
                {
                    _assignedUsers.Add(user);
                }
            }
            RefreshUserList();
        }
    }

    private void BtnRemoveUser_Click(object? sender, EventArgs e)
    {
        var selectedIndices = lstAssignedUsers.CheckedIndices;
        if (selectedIndices.Count == 0)
        {
            MessageBox.Show("제거할 사용자를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"{selectedIndices.Count}명의 사용자를 제거하시겠습니까?",
            "확인",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            var toRemove = new List<PersonInfo>();
            foreach (int index in selectedIndices)
            {
                toRemove.Add(_assignedUsers[index]);
            }
            foreach (var user in toRemove)
            {
                _assignedUsers.Remove(user);
            }
            RefreshUserList();
        }
    }

    private async void BtnUpload_Click(object? sender, EventArgs e)
    {
        if (_assignedUsers.Count == 0)
        {
            MessageBox.Show("업로드할 사용자가 없습니다", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            btnUpload.Enabled = false;
            btnUpload.Text = "업로드 중...";

            // HTTPv2 프로토콜: 서버에 사용자 정보 저장 → Keepalive를 통해 장치가 다운로드
            int successCount = 0;
            int failCount = 0;

            foreach (var user in _assignedUsers)
            {
                try
                {
                    // 서버 DB에 사용자 추가/업데이트
                    var response = await _httpClient.PostAsJsonAsync("/api/People/New", user);
                    var result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

                    if (result?.Code == 0)
                    {
                        successCount++;
                    }
                    else
                    {
                        // 이미 존재하면 업데이트 시도
                        response = await _httpClient.PostAsJsonAsync("/api/People/Update", user);
                        result = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();
                        if (result?.Code == 0)
                            successCount++;
                        else
                            failCount++;
                    }
                }
                catch
                {
                    failCount++;
                }
            }

            if (successCount > 0)
            {
                // 서버에 장치 동기화 요청 (HTTPv2: Keepalive 응답에서 AddPeople 플래그 설정)
                try
                {
                    var syncResponse = await _httpClient.PostAsync(
                        $"/admin/devices/{_device.SN}/remote-command",
                        JsonContent.Create(new { CommandType = "pushallpeople" }));

                    if (syncResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show(
                            $"서버에 {successCount}명의 사용자 정보를 저장했습니다.\n" +
                            $"단말기가 다음 Keepalive 시 자동으로 동기화됩니다.\n\n" +
                            $"성공: {successCount}명\n실패: {failCount}명",
                            "업로드 완료",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"사용자 정보는 저장되었으나 동기화 요청 실패\n성공: {successCount}명\n실패: {failCount}명",
                            "부분 성공",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"사용자 정보는 저장되었으나 동기화 요청 실패: {ex.Message}\n성공: {successCount}명",
                        "부분 성공",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            else if (failCount > 0)
            {
                MessageBox.Show(
                    $"모든 사용자 저장 실패\n실패: {failCount}명",
                    "업로드 실패",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"업로드 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnUpload.Enabled = true;
            btnUpload.Text = "업로드 (할당된 사용자를 단말기로 전송)";
        }
    }

    private async void BtnDownload_Click(object? sender, EventArgs e)
    {
        try
        {
            btnDownload.Enabled = false;
            btnDownload.Text = "다운로드 중...";

            // HTTPv2 프로토콜: 서버 DB에서 사용자 목록 조회
            // (장치는 Keepalive를 통해 서버로 사용자를 업로드하므로 서버 DB가 최신 상태)

            var response = await _httpClient.GetAsync("/admin/people");

            if (response.IsSuccessStatusCode)
            {
                var allUsers = await response.Content.ReadFromJsonAsync<List<PersonInfo>>();

                if (allUsers != null && allUsers.Count > 0)
                {
                    MessageBox.Show(
                        $"서버로부터 {allUsers.Count}명의 사용자 정보를 조회했습니다.\n\n" +
                        $"참고: HTTPv2 프로토콜에서는 장치가 Keepalive를 통해\n" +
                        $"서버로 데이터를 전송합니다. 서버 DB가 최신 상태입니다.",
                        "다운로드 성공",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // 필요시 현재 장치에 할당된 사용자만 필터링
                    // _assignedUsers = allUsers.Where(...).ToList();
                }
                else
                {
                    MessageBox.Show(
                        "서버에 등록된 사용자가 없습니다.",
                        "안내",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(
                    $"서버 통신 실패 (HTTP {response.StatusCode})",
                    "통신 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"다운로드 실패: {ex.Message}\n\n" +
                $"HTTPv2 프로토콜에서는 장치가 서버로 데이터를 전송하므로\n" +
                $"서버 DB를 조회하는 방식입니다.",
                "오류",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnDownload.Enabled = true;
            btnDownload.Text = "다운로드 (단말기로부터 사용자 정보 수신)";
        }
    }

    private async void BtnInitialize_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "경고: 이 단말기의 모든 사용자 정보가 삭제됩니다.\n계속하시겠습니까?",
            "단말기 초기화 확인",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
            return;

        try
        {
            btnInitialize.Enabled = false;
            btnInitialize.Text = "초기화 중...";

            // HTTPv2 프로토콜: 서버에 초기화 명령 전송 → Keepalive를 통해 장치가 삭제 수행
            var response = await _httpClient.PostAsync(
                $"/admin/devices/{_device.SN}/remote-command",
                JsonContent.Create(new { CommandType = "deleteallpeople" }));

            if (response.IsSuccessStatusCode)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

                if (apiResult?.Code == 0)
                {
                    _assignedUsers.Clear();
                    RefreshUserList();

                    MessageBox.Show(
                        "서버에서 모든 사용자 정보를 삭제했습니다.\n" +
                        "단말기가 다음 Keepalive 시 자동으로 동기화됩니다.\n\n" +
                        $"메시지: {apiResult.Content}",
                        "초기화 완료",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"초기화 실패: {apiResult?.Content ?? "알 수 없는 오류"}",
                        "오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    $"서버 통신 실패 (HTTP {response.StatusCode})",
                    "통신 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"초기화 실패: {ex.Message}\n\n" +
                $"HTTPv2 프로토콜에서는 서버를 통해 단말기를 초기화합니다.",
                "오류",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnInitialize.Enabled = true;
            btnInitialize.Text = "초기화";
        }
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            btnSave.Enabled = false;
            btnSave.Text = "저장 중...";

            // Prepare update payload
            var updatePayload = new
            {
                DeviceSN = _device.SN,
                IpAddress = _device.IpAddress,
                HttpPort = _device.HttpPort,
                DeviceName = txtDeviceName.Text.Trim(),
                TagName = txtTagName.Text.Trim(),
                Model = _device.Model,
                FirmwareVersion = _device.FirmwareVersion,
                UnitNo = _device.UnitNo
            };

            // Send update request
            var response = await _httpClient.PostAsJsonAsync("/api/Device/Connect", updatePayload);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("단말기 정보가 저장되었습니다!", "성공", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("단말기 정보 저장에 실패했습니다", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"저장 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            btnSave.Text = "저장";
        }
    }
}
