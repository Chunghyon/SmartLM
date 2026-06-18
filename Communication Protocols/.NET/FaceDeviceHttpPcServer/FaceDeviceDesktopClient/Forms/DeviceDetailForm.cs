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

            var deviceUrl = $"http://{_device.IpAddress}:{_device.HttpPort}";

            using (var deviceClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
            {
                deviceClient.BaseAddress = new Uri(deviceUrl);

                int successCount = 0;
                int failCount = 0;

                foreach (var user in _assignedUsers)
                {
                    try
                    {
                        // 단말기에 사용자 추가
                        var userData = new
                        {
                            id = user.UserID,
                            name = user.Name,
                            password = user.Password ?? "",
                            cardNum = user.CardNum ?? "",
                            faceData = user.PhotoData != null ? Convert.ToBase64String(user.PhotoData) : null
                        };

                        var response = await deviceClient.PostAsJsonAsync("/personnel/new", userData);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var jsonDoc = JsonDocument.Parse(content);
                            var root = jsonDoc.RootElement;

                            if (root.TryGetProperty("result", out var result) && result.GetBoolean())
                            {
                                successCount++;
                            }
                            else
                            {
                                failCount++;
                            }
                        }
                        else
                        {
                            failCount++;
                        }
                    }
                    catch
                    {
                        failCount++;
                    }
                }

                if (failCount == 0)
                {
                    MessageBox.Show(
                        $"성공: {successCount}명의 사용자 정보를 단말기로 전송했습니다",
                        "업로드 성공",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"업로드 완료\n성공: {successCount}명\n실패: {failCount}명",
                        "업로드 결과",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }
        catch (TaskCanceledException)
        {
            MessageBox.Show("단말기 응답 시간 초과", "시간 초과",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // 단말기로부터 사용자 정보 다운로드
            var deviceUrl = $"http://{_device.IpAddress}:{_device.HttpPort}";

            using (var deviceClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
            {
                deviceClient.BaseAddress = new Uri(deviceUrl);

                // Try different possible endpoints
                string[] endpoints = new[] 
                { 
                    "/personnel/listRecord",
                    "/cgi-bin/js/personnel/listRecord",
                    "/person/findList",
                    "/cgi-bin/person/findList"
                };

                HttpResponseMessage? response = null;
                string? successEndpoint = null;

                foreach (var endpoint in endpoints)
                {
                    try
                    {
                        response = await deviceClient.PostAsync(endpoint, null);
                        if (response.IsSuccessStatusCode)
                        {
                            successEndpoint = endpoint;
                            break;
                        }
                    }
                    catch
                    {
                        // Try next endpoint
                        continue;
                    }
                }

                if (response?.IsSuccessStatusCode == true && successEndpoint != null)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    // JSON 파싱하여 사용자 정보 추출
                    var jsonDoc = JsonDocument.Parse(content);
                    var root = jsonDoc.RootElement;

                    if (root.TryGetProperty("result", out var result) && result.GetBoolean())
                    {
                        if (root.TryGetProperty("content", out var contentObj) &&
                            contentObj.TryGetProperty("record", out var records))
                        {
                            var downloadedUsers = new List<PersonInfo>();

                            foreach (var record in records.EnumerateArray())
                            {
                                var person = new PersonInfo
                                {
                                    UserID = record.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
                                    Name = record.TryGetProperty("name", out var name) ? name.GetString() ?? "" : "",
                                    Password = record.TryGetProperty("password", out var pwd) ? pwd.GetString() : null,
                                    CardNum = record.TryGetProperty("cardNum", out var card) ? card.GetString() : null
                                };

                                downloadedUsers.Add(person);
                            }

                            // 서버에 다운로드된 사용자 정보 저장
                            foreach (var person in downloadedUsers)
                            {
                                try
                                {
                                    await _httpClient.PostAsJsonAsync("/api/People/New", person);
                                }
                                catch
                                {
                                    // 이미 존재하는 사용자일 수 있으므로 업데이트 시도
                                    await _httpClient.PostAsJsonAsync("/api/People/Update", person);
                                }
                            }

                            MessageBox.Show(
                                $"성공: 단말기로부터 {downloadedUsers.Count}명의 사용자 정보를 수신했습니다\n" +
                                $"사용된 엔드포인트: {successEndpoint}",
                                "다운로드 성공",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("단말기에 등록된 사용자가 없습니다", "안내",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        var errMsg = root.TryGetProperty("error", out var error) ? error.GetString() : "알 수 없는 오류";
                        MessageBox.Show($"다운로드 실패: {errMsg}", "오류",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var statusCode = response?.StatusCode.ToString() ?? "NotFound";
                    MessageBox.Show(
                        $"단말기 통신 실패 (HTTP {statusCode})\n" +
                        $"단말기 주소: {deviceUrl}\n\n" +
                        $"참고: 이 단말기는 웹 브라우저에서 로그인이 필요한 장치입니다.\n" +
                        $"현재 프로그램은 단말기의 HTTP API를 직접 사용할 수 없습니다.\n\n" +
                        $"해결 방법:\n" +
                        $"1. 단말기의 HTTP API가 활성화되어 있는지 확인\n" +
                        $"2. 단말기 설정에서 인증 없이 API 접근 허용 설정 확인\n" +
                        $"3. 단말기 펌웨어 버전 및 API 문서 확인",
                        "통신 오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        catch (TaskCanceledException)
        {
            MessageBox.Show(
                "단말기 응답 시간 초과\n" +
                $"단말기 ({_device.IpAddress}:{_device.HttpPort})가 응답하지 않습니다",
                "시간 초과",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                $"단말기 연결 실패\n" +
                $"주소: {_device.IpAddress}:{_device.HttpPort}\n" +
                $"오류: {ex.Message}\n\n" +
                $"네트워크 연결 및 단말기 상태를 확인해주세요",
                "연결 오류",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"다운로드 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            var deviceUrl = $"http://{_device.IpAddress}:{_device.HttpPort}";

            using (var deviceClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
            {
                deviceClient.BaseAddress = new Uri(deviceUrl);

                // 단말기 초기화 명령 전송
                var response = await deviceClient.PostAsync("/personnel/deleteAll", null);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JsonDocument.Parse(content);
                    var root = jsonDoc.RootElement;

                    if (root.TryGetProperty("result", out var resultProp) && resultProp.GetBoolean())
                    {
                        _assignedUsers.Clear();
                        RefreshUserList();

                        MessageBox.Show("단말기가 초기화되었습니다", "초기화 완료",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        var errMsg = root.TryGetProperty("error", out var error) ? error.GetString() : "알 수 없는 오류";
                        MessageBox.Show($"초기화 실패: {errMsg}", "오류",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"단말기 통신 실패 (HTTP {response.StatusCode})", "통신 오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (TaskCanceledException)
        {
            MessageBox.Show("단말기 응답 시간 초과", "시간 초과",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"초기화 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
