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

    private TextBox txtUserID = null!;
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
            txtUserID.Text = userId;
            txtUserID.ReadOnly = true;
            txtUserID.BackColor = SystemColors.Control;
        }
        if (!string.IsNullOrEmpty(name))
            txtName.Text = name;
        if (!string.IsNullOrEmpty(photoUrl))
            txtPhotoUrl.Text = photoUrl;
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

        // 2. 사용자번호 (Worker No.)
        mainPanel.Controls.Add(new Label
        {
            Text = "사용자번호:",
            Location = new Point(10, y),
            Width = labelWidth
        });
        txtUserID = new TextBox
        {
            Location = new Point(140, y - 3),
            Width = controlWidth,
            MaxLength = 32
        };
        mainPanel.Controls.Add(txtUserID);

        var lblHint = new Label
        {
            Text = "(10001~999999)",
            Location = new Point(500, y),
            Width = 120,
            ForeColor = Color.Gray
        };
        mainPanel.Controls.Add(lblHint);
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
            Size = new Size(590, 200)
        };

        lstDevices = new CheckedListBox
        {
            Location = new Point(10, 25),
            Size = new Size(560, 130),
            CheckOnClick = true
        };
        grpDevices.Controls.Add(lstDevices);

        btnUploadToDevices = new Button
        {
            Text = "선택한 단말기로 전송",
            Location = new Point(10, 160),
            Size = new Size(560, 30)
        };
        btnUploadToDevices.Click += BtnUploadToDevices_Click;
        grpDevices.Controls.Add(btnUploadToDevices);

        mainPanel.Controls.Add(grpDevices);
        y += 220;

        // 하단 버튼
        btnOK = new Button
        {
            Text = "확인",
            Location = new Point(370, y),
            Size = new Size(100, 35)
        };
        btnOK.Click += BtnOK_Click;
        mainPanel.Controls.Add(btnOK);

        btnCancel = new Button
        {
            Text = "취소",
            Location = new Point(480, y),
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
        var selectedIndices = lstDevices.CheckedIndices;
        if (selectedIndices.Count == 0)
        {
            MessageBox.Show("전송할 단말기를 선택해주세요", "안내",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // 사용자 정보 검증
        if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtUserID.Text))
        {
            MessageBox.Show("사용자명과 사용자번호를 입력해주세요", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnUploadToDevices.Enabled = false;
            btnUploadToDevices.Text = "전송 중...";

            // 전송할 사용자 데이터 준비
            var userData = new
            {
                UserID = txtUserID.Text.Trim(),
                Name = txtName.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                PhotoData = Person.PhotoData != null ? Convert.ToBase64String(Person.PhotoData) : null
            };

            var successCount = 0;
            var failCount = 0;

            // 선택된 각 단말기로 전송
            foreach (int index in selectedIndices)
            {
                if (index < _allDevices.Count)
                {
                    var device = _allDevices[index];
                    try
                    {
                        // 실제 단말기 API 호출 (placeholder - 실제 구현 필요)
                        // var response = await _httpClient.PostAsJsonAsync($"/api/Device/{device.SN}/UploadUser", userData);

                        // 시뮬레이션
                        await Task.Delay(200);
                        successCount++;
                    }
                    catch
                    {
                        failCount++;
                    }
                }
            }

            if (failCount == 0)
            {
                MessageBox.Show(
                    $"성공: {successCount}개 단말기로 사용자 정보를 전송했습니다" +
                    (Person.PhotoData != null ? "\n(얼굴 사진 포함)" : ""),
                    "전송 성공",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    $"전송 완료: 성공 {successCount}개, 실패 {failCount}개",
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

        if (string.IsNullOrWhiteSpace(txtUserID.Text))
        {
            MessageBox.Show("사용자번호를 입력해주세요", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUserID.Focus();
            return;
        }

        // Validate user ID range only for new users
        if (!IsEditMode)
        {
            if (int.TryParse(txtUserID.Text, out int userId))
            {
                if (userId < 10001 || userId > 999999)
                {
                    MessageBox.Show("사용자번호는 10001~999999 범위여야 합니다", "입력 오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUserID.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("사용자번호는 숫자여야 합니다", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserID.Focus();
                return;
            }
        }

        // Save data - use original UserID for updates
        Person.UserID = IsEditMode && !string.IsNullOrEmpty(_originalUserID) 
            ? _originalUserID 
            : txtUserID.Text.Trim();
        Person.Name = txtName.Text.Trim();
        Person.PhotoUrl = txtPhotoUrl.Text.Trim();
        Person.Password = txtPassword.Text.Trim();

        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}
