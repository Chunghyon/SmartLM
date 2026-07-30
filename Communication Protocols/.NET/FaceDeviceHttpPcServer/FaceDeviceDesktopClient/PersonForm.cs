using System.Net.Http.Json;
using FaceDeviceDesktopClient.Forms;
using System.ComponentModel;

namespace FaceDeviceDesktopClient;

public partial class PersonForm : Form
{
    public PersonInfo Person { get; private set; }
    private readonly HttpClient? _httpClient;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsEditMode { get; set; }
    private string? _originalUserID;

    // Flag to indicate if person was already saved (via "저장 및 선택한 단말기로 전송")
    public bool AlreadySaved { get; private set; }

    private TextBox txtDong = null!;
    private TextBox txtHo = null!;
    private TextBox txtMember = null!;
    private TextBox txtName = null!;
    private TextBox txtPhotoUrl = null!;
    private Button btnBrowsePhoto = null!;
    private PictureBox picPhotoPreview = null!;
    private TextBox txtPassword = null!;
    private CheckedListBox lstDevices = null!;
    private Button btnUploadToDevices = null!;
    private Button btnOK = null!;
    private Button btnCancel = null!;

    private List<DeviceInfo> _allDevices = new();

    public PersonForm()
    {
        InitializeComponent();
        Person = new PersonInfo();
    }

    public PersonForm(HttpClient httpClient) : this()
    {
        _httpClient = httpClient;
        _ = LoadDevices();
    }

    public void SetInitialValues(string? userId, string? name, string? photoUrl = null, string? password = null)
    {
        IsEditMode = true;
        _originalUserID = userId;

        if (!string.IsNullOrEmpty(userId))
        {
            if (long.TryParse(userId, out long idNum))
            {
                txtDong.Text = (idNum / 1_000_000).ToString();
                txtHo.Text = ((idNum / 100) % 10_000).ToString();
                txtMember.Text = (idNum % 100).ToString();
            }
            else
            {
                // 숫자가 아닌 경우 동 필드에 원본값 표시
                txtDong.Text = userId;
            }
            txtDong.ReadOnly = true;
            txtHo.ReadOnly = true;
            txtMember.ReadOnly = true;
            txtDong.BackColor = SystemColors.Control;
            txtHo.BackColor = SystemColors.Control;
            txtMember.BackColor = SystemColors.Control;
        }
        if (!string.IsNullOrEmpty(name))
            txtName.Text = name;
        if (!string.IsNullOrEmpty(photoUrl))
        {
            // Check if photoUrl is a device file path (e.g., /data/attend_data/photo/frame_...)
            if (photoUrl.StartsWith("/") || photoUrl.Contains("/") || photoUrl.Contains("\\"))
            {
                // This is a device file path - show indicator but don't try to display
                txtPhotoUrl.Text = "(단말기에 저장된 사진)";
                // Optionally: In the future, we could implement downloading the photo from the device
            }
            // Try to decode as Base64
            else
            {
                try
                {
                    Person.PhotoData = Convert.FromBase64String(photoUrl);
                    txtPhotoUrl.Text = "(등록된 사진)";

                    // Show preview
                    using (var ms = new MemoryStream(Person.PhotoData))
                    {
                        picPhotoPreview.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    // If decoding fails, it might be a regular string path or invalid data
                    txtPhotoUrl.Text = photoUrl;
                }
            }
        }
        if (!string.IsNullOrEmpty(password))
            txtPassword.Text = password;
    }

    private void InitializeComponent()
    {
        this.Text = "사용자 추가/수정";
        this.Size = new Size(650, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20)
        };

        int y = 10;
        int labelWidth = 120;
        int controlWidth = 350;

        // 1. 사용자명
        mainPanel.Controls.Add(new Label
        {
            Text = "사용자명:",
            Location = new Point(10, y),
            Width = labelWidth
        });
        txtName = new TextBox
        {
            Location = new Point(140, y - 3),
            Width = controlWidth,
            MaxLength = 64
        };
        mainPanel.Controls.Add(txtName);
        y += 40;

        // 2. 동 / 호 / 멤버
        mainPanel.Controls.Add(new Label
        {
            Text = "동:",
            Location = new Point(10, y),
            Width = 40
        });
        txtDong = new TextBox
        {
            Location = new Point(55, y - 3),
            Width = 80,
            MaxLength = 10
        };
        mainPanel.Controls.Add(txtDong);

        mainPanel.Controls.Add(new Label
        {
            Text = "호:",
            Location = new Point(145, y),
            Width = 30
        });
        txtHo = new TextBox
        {
            Location = new Point(180, y - 3),
            Width = 80,
            MaxLength = 10
        };
        mainPanel.Controls.Add(txtHo);

        mainPanel.Controls.Add(new Label
        {
            Text = "멤버:",
            Location = new Point(270, y),
            Width = 45
        });
        txtMember = new TextBox
        {
            Location = new Point(320, y - 3),
            Width = 60,
            MaxLength = 5
        };
        mainPanel.Controls.Add(txtMember);
        y += 40;

        // 3. 사진등록 (경로 표시)
        mainPanel.Controls.Add(new Label
        {
            Text = "사진등록:",
            Location = new Point(10, y),
            Width = labelWidth
        });
        txtPhotoUrl = new TextBox
        {
            Location = new Point(140, y - 3),
            Width = 250,
            ReadOnly = true,
            BackColor = SystemColors.Window
        };
        mainPanel.Controls.Add(txtPhotoUrl);

        btnBrowsePhoto = new Button
        {
            Text = "찾아보기",
            Location = new Point(400, y - 5),
            Size = new Size(90, 28)
        };
        btnBrowsePhoto.Click += BtnBrowsePhoto_Click;
        mainPanel.Controls.Add(btnBrowsePhoto);
        y += 40;

        // 사진 미리보기
        picPhotoPreview = new PictureBox
        {
            Location = new Point(140, y),
            Size = new Size(100, 100),
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.WhiteSmoke
        };
        mainPanel.Controls.Add(picPhotoPreview);
        y += 110;

        // 4. 패스워드
        mainPanel.Controls.Add(new Label
        {
            Text = "패스워드:",
            Location = new Point(10, y),
            Width = labelWidth
        });
        txtPassword = new TextBox
        {
            Location = new Point(140, y - 3),
            Width = controlWidth,
            MaxLength = 16,
            UseSystemPasswordChar = true
        };
        mainPanel.Controls.Add(txtPassword);
        y += 40;

        // 5. 단말기 할당
        var grpDevices = new GroupBox
        {
            Text = "단말기 할당",
            Location = new Point(10, y),
            Size = new Size(590, 170)
        };

        lstDevices = new CheckedListBox
        {
            Location = new Point(10, 25),
            Size = new Size(560, 130),
            CheckOnClick = true
        };
        grpDevices.Controls.Add(lstDevices);

        mainPanel.Controls.Add(grpDevices);
        y += 190;

        // 하단 버튼
        btnUploadToDevices = new Button
        {
            Text = "저장 및 선택한 단말기로 전송",
            Location = new Point(10, y),
            Size = new Size(240, 35)
        };
        btnUploadToDevices.Click += BtnUploadToDevices_Click;
        mainPanel.Controls.Add(btnUploadToDevices);

        btnOK = new Button
        {
            Text = "저장",
            Location = new Point(260, y),
            Size = new Size(100, 35)
        };
        btnOK.Click += BtnOK_Click;
        mainPanel.Controls.Add(btnOK);

        btnCancel = new Button
        {
            Text = "취소",
            Location = new Point(370, y),
            Size = new Size(100, 35),
            DialogResult = DialogResult.Cancel
        };
        mainPanel.Controls.Add(btnCancel);

        this.Controls.Add(mainPanel);
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
    }

    private async Task LoadDevices()
    {
        if (_httpClient == null) return;

        try
        {
            var devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
            if (devices != null)
            {
                _allDevices = devices;
                foreach (var device in devices)
                {
                    lstDevices.Items.Add($"{device.DeviceName} ({device.IpAddress})");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"단말기 목록 로드 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnBrowsePhoto_Click(object? sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog
        {
            Filter = "이미지 파일|*.jpg;*.jpeg;*.png;*.bmp|모든 파일|*.*",
            Title = "얼굴 사진 선택"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // 얼굴 사진 편집기 열기
                using var editorForm = new FacePhotoEditorForm(openFileDialog.FileName);
                if (editorForm.ShowDialog() == DialogResult.OK)
                {
                    if (editorForm.ProcessedImageData != null)
                    {
                        // 편집된 사진 데이터 저장
                        Person.PhotoData = editorForm.ProcessedImageData;

                        // 파일 경로는 표시용으로만 사용
                        txtPhotoUrl.Text = openFileDialog.FileName;

                        // 미리보기 업데이트
                        using (var ms = new MemoryStream(editorForm.ProcessedImageData))
                        {
                            picPhotoPreview.Image = Image.FromStream(ms);
                        }

                        MessageBox.Show("얼굴 사진이 등록되었습니다", "완료",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사진 처리 실패: {ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async void BtnUploadToDevices_Click(object? sender, EventArgs e)
    {
        if (_httpClient == null)
        {
            MessageBox.Show("HTTP 클라이언트가 초기화되지 않았습니다", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var selectedIndices = lstDevices.CheckedIndices;
        if (selectedIndices.Count == 0)
        {
            MessageBox.Show("전송할 단말기를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // 사용자 정보 검증
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("사용자명과 사용자번호를 입력해주세요", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!TryBuildUserID(out string builtUserID))
        {
            MessageBox.Show("동/호/멤버 번호를 올바르게 입력해주세요.\n(동·호·멤버는 모두 숫자여야 합니다)", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnUploadToDevices.Enabled = false;
            btnUploadToDevices.Text = "전송 중...";

            // 1단계: 서버에 사용자 추가 (또는 업데이트)
            var personInfo = new PersonInfo
            {
                UserID = builtUserID,
                Name = txtName.Text.Trim(),
                Password = txtPassword.Text.Trim()
            };

            // 사진 데이터를 Base64로 변환하여 Photo 필드에 저장
            if (Person.PhotoData != null && Person.PhotoData.Length > 0)
            {
                personInfo.Photo = Convert.ToBase64String(Person.PhotoData);
            }

            // 서버에서 사용자 존재 여부 확인
            bool userExists = false;
            try
            {
                var checkResponse = await _httpClient.PostAsJsonAsync("/api/People/GetDetail", new { UserID = personInfo.UserID });
                var checkResult = await checkResponse.Content.ReadFromJsonAsync<BrowserApiResponse<PersonInfo>>();
                userExists = checkResult?.Code == 0;
            }
            catch { }

            // 존재하면 Update, 없으면 New
            bool isEditMode = userExists || (IsEditMode && !string.IsNullOrEmpty(_originalUserID));
            string apiEndpoint = isEditMode ? "/api/People/Update" : "/api/People/New";

            // 서버에 사용자 추가/업데이트
            var addResponse = await _httpClient.PostAsJsonAsync(apiEndpoint, personInfo);
            var addResult = await addResponse.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

            if (addResult == null || addResult.Code != 0)
            {
                MessageBox.Show(
                    $"서버에 사용자 {(isEditMode ? "업데이트" : "추가")} 실패: {addResult?.Msg ?? "알 수 없는 오류"}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // 2단계: 선택된 각 단말기로 전송 요청
            var successCount = 0;
            var failCount = 0;
            var errorMessages = new List<string>();

            foreach (int index in selectedIndices)
            {
                if (index < _allDevices.Count)
                {
                    var device = _allDevices[index];
                    try
                    {
                        // 단말기에 사용자 추가 요청
                        var requestUrl = $"/admin/devices/{device.SN}/request-add-people";
                        var deviceResponse = await _httpClient.PostAsync(requestUrl, null);

                        if (deviceResponse.IsSuccessStatusCode)
                        {
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                            errorMessages.Add($"{device.DeviceName}: HTTP {deviceResponse.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        errorMessages.Add($"{device.DeviceName}: {ex.Message}");
                    }
                }
            }

            // 3단계: 결과 표시
            if (failCount == 0)
            {
                MessageBox.Show(
                    $"성공: {successCount}개 단말기로 사용자 전송을 요청했습니다.\n\n" +
                    $"단말기가 다음 Keepalive 신호를 보낼 때 사용자 정보를 다운로드합니다." +
                    (Person.PhotoData != null ? "\n(얼굴 사진 포함)" : "") +
                    "\n\n서버 콘솔에서 전송 과정을 확인할 수 있습니다.",
                    "전송 성공",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Update Person object for parent form
                Person.UserID = personInfo.UserID;
                Person.Name = personInfo.Name;
                Person.Password = personInfo.Password;

                AlreadySaved = true; // Person was saved by this button
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                var errorDetail = errorMessages.Count > 0
                    ? "\n\n오류 상세:\n" + string.Join("\n", errorMessages.Take(5))
                    : "";

                MessageBox.Show(
                    $"전송 완료: 성공 {successCount}개, 실패 {failCount}개{errorDetail}",
                    "전송 결과",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"전송 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnUploadToDevices.Enabled = true;
            btnUploadToDevices.Text = "선택한 단말기로 전송";
        }
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("사용자명을 입력해주세요", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return;
        }

        if (!IsEditMode && !TryBuildUserID(out _))
        {
            MessageBox.Show("동/호/멤버 번호를 올바르게 입력해주세요.\n(동·호·멤버는 모두 숫자여야 합니다)", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDong.Focus();
            return;
        }

        TryBuildUserID(out string builtUserID);

        // Save data - use original UserID for updates
        Person.UserID = IsEditMode && !string.IsNullOrEmpty(_originalUserID)
            ? _originalUserID
            : builtUserID;
        Person.Name = txtName.Text.Trim();
        Person.PhotoUrl = txtPhotoUrl.Text.Trim();
        Person.Password = txtPassword.Text.Trim();

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    /// <summary>동/호/멤버 입력값을 UserID 문자열로 변환합니다.</summary>
    private bool TryBuildUserID(out string userID)
    {
        userID = string.Empty;
        if (!long.TryParse(txtDong.Text.Trim(), out long dong) ||
            !long.TryParse(txtHo.Text.Trim(), out long ho) ||
            !long.TryParse(txtMember.Text.Trim(), out long member))
            return false;
        long id = dong * 1_000_000L + ho * 100L + member;
        userID = id.ToString();
        return true;
    }
}
