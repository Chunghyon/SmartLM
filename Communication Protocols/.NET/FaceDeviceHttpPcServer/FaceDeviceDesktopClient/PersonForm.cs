using FaceDeviceDesktopClient.Forms;
using System.ComponentModel;
using System.Net.Http.Json;

namespace FaceDeviceDesktopClient;

public partial class PersonForm : Form
{
    public PersonInfo Person { get; private set; }
    private readonly HttpClient? _httpClient;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsEditMode { get; set; }
    private string? _originalUserID;

    // ´Ü¸»±â IP ÁÖ¼Ò ¸ñ·Ï (LoadPhotoAsync¿¡¼­ »çÁø °¡Á®¿Ã ¶§ »ç¿ë)
    private List<string> _deviceIpAddresses = new();

    // Flag to indicate if person was already saved (via "ÀúÀå ¹× ¼±ÅÃÇÑ ´Ü¸»±â·Î Àü¼Û")
    public bool AlreadySaved { get; private set; }

    // ¦¡¦¡ UI ÄÁÆ®·Ñ ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private TextBox   txtDong   = null!;
    private TextBox   txtHo     = null!;
    private TextBox   txtMember = null!;
    private TextBox   txtName   = null!;

    private TextBox   txtCard     = null!;
    private TextBox   txtPassword = null!;

    // Áö¹® / Á¤¸Æ (ÀÐ±â Àü¿ë Ç¥½Ã)
    private Label lblFingerprintCount = null!;
    private Label lblPalmveinCount    = null!;

    // »çÁø
    private TextBox    txtPhotoUrl     = null!;
    private Button     btnBrowsePhoto  = null!;
    private PictureBox picPhotoPreview = null!;

    private CheckedListBox lstDevices         = null!;
    private Button         btnUploadToDevices = null!;
    private Button         btnOK              = null!;
    private Button         btnCancel          = null!;

    private List<DeviceInfo> _allDevices = new();

    // ¦¡¦¡ »ý¼ºÀÚ ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

    // ¦¡¦¡ SetInitialValues (¼­¹ö PersonInfo ÀüÃ¼ Àü´Þ) ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    public void SetInitialValues(PersonInfo person)
    {
        IsEditMode      = true;
        _originalUserID = person.UserID;

        // µ¿/È£/¸â¹ö
        if (long.TryParse(person.UserID, out long idNum))
        {
            txtDong.Text   = (idNum / 1_000_000L).ToString();
            txtHo.Text     = ((idNum / 100L) % 10_000L).ToString();
            txtMember.Text = (idNum % 100L).ToString();
        }
        else
        {
            txtDong.Text = person.UserID;
        }
        txtDong.ReadOnly    = true;
        txtHo.ReadOnly      = true;
        txtMember.ReadOnly  = true;
        txtDong.BackColor   = SystemColors.Control;
        txtHo.BackColor     = SystemColors.Control;
        txtMember.BackColor = SystemColors.Control;

        // ÀÌ¸§
        txtName.Text = person.Name ?? "";

        // Ä«µå
        var card = person.CardNum ?? "";
        txtCard.Text = (card == "0") ? "" : card;

        // ºñ¹Ð¹øÈ£
        txtPassword.Text = person.Password ?? "";

        // Áö¹® / Á¤¸Æ (µî·Ï ¼ö¸¸ Ç¥½Ã, ´Ü¸»±â¿¡¼­ Á÷Á¢ µî·ÏÇÏ¹Ç·Î PC ÆíÁý ºÒ°¡)
        int fpCount = person.Fingerprints?.Count ?? 0;
        int pvCount = person.Palmveins?.Count    ?? 0;
        lblFingerprintCount.Text      = fpCount > 0 ? $"µî·ÏµÊ ({fpCount}°³)" : "¹Ìµî·Ï";
        lblPalmveinCount.Text         = pvCount > 0 ? $"µî·ÏµÊ ({pvCount}°³)" : "¹Ìµî·Ï";
        lblFingerprintCount.ForeColor = fpCount > 0 ? Color.Green : Color.Gray;
        lblPalmveinCount.ForeColor    = pvCount > 0 ? Color.Green : Color.Gray;

        // Person °´Ã¼¿¡ Áö¹®/Á¤¸Æ µ¥ÀÌÅÍ º¸Á¸ (ÀúÀå ½Ã ±×´ë·Î Àü´Þ)
        Person.Fingerprints = person.Fingerprints ?? new();
        Person.Palmveins    = person.Palmveins    ?? new();

        // »çÁø ·Îµå (ºñµ¿±â - ´Ü¸»±â °æ·ÎÀÎ °æ¿ì ¼­¹ö ÇÁ·Ï½Ã¸¦ ÅëÇØ ´Ù¿î·Îµå)
        _ = LoadPhotoAsync(person.UserID, person.Photo);
    }

    private async Task LoadPhotoAsync(string userId, string? photo)
    {
        if (string.IsNullOrEmpty(photo)) return;

        // ´Ü¸»±â ³»ºÎ °æ·Î ÆÇº°: "/data/..." ÇüÅÂ (½½·¡½Ã·Î ½ÃÀÛÇÏ¸é¼­ È®ÀåÀÚ Æ÷ÇÔ)
        // JPEG Base64´Â "/9j/..." ·Î ½ÃÀÛÇÏ¹Ç·Î È®ÀåÀÚ Æ÷ÇÔ ¿©ºÎ·Î ±¸ºÐ
        bool isDevicePath = (photo.StartsWith("/") && System.IO.Path.HasExtension(photo))
                         || photo.Contains(":\\")
                         || photo.Contains(":/");

        if (!isDevicePath)
        {
            // Base64 »çÁø ¡æ ¹Ù·Î µðÄÚµù
            try
            {
                var bytes = Convert.FromBase64String(photo);
                Person.PhotoData = bytes;
                ShowPhotoPreview(bytes, "(µî·ÏµÈ »çÁø)");
            }
            catch { SetPhotoUrlText("(»çÁø Çü½Ä ¿À·ù)"); }
            return;
        }

        // ´Ü¸»±â ³»ºÎ °æ·Î(/data/...) ¡æ ´Ü¸»±â À¥ UI¸¦ ÅëÇØ »çÁø ´Ù¿î·Îµå ½Ãµµ
        SetPhotoUrlText("(»çÁø ºÒ·¯¿À´Â Áß...)");
        try
        {
            // µî·ÏµÈ ´Ü¸»±â IP°¡ ¾øÀ¸¸é ¼­¹ö¿¡¼­ °¡Á®¿È
            if (_deviceIpAddresses.Count == 0 && _httpClient != null)
            {
                try
                {
                    var devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
                    if (devices != null)
                        _deviceIpAddresses = devices
                            .Where(d => !string.IsNullOrWhiteSpace(d.IpAddress))
                            .Select(d => d.IpAddress!)
                            .ToList();
                }
                catch { }
            }

            if (_deviceIpAddresses.Count == 0)
            {
                SetPhotoUrlText("(´Ü¸»±â ¹Ì¿¬°á - Ã£¾Æº¸±â·Î »çÁø µî·Ï)");
                return;
            }

            var photoBytes = await DevicePhotoService.FetchUserPhotoAsync(_deviceIpAddresses, userId);
            if (photoBytes != null)
            {
                Person.PhotoData = photoBytes;
                ShowPhotoPreview(photoBytes, "(´Ü¸»±â¿¡¼­ ºÒ·¯¿Â »çÁø)");
            }
            else
            {
                SetPhotoUrlText("(´Ü¸»±â »çÁø ¾øÀ½ - Ã£¾Æº¸±â·Î µî·Ï)");
            }
        }
        catch (Exception ex)
        {
            SetPhotoUrlText($"(»çÁø ·Îµå ½ÇÆÐ: {ex.Message})");
        }
    }

    private void SetPhotoUrlText(string text)
    {
        if (InvokeRequired) { Invoke(() => SetPhotoUrlText(text)); return; }
        txtPhotoUrl.Text = text;
    }

    private void ShowPhotoPreview(byte[] bytes, string label)
    {
        if (InvokeRequired) { Invoke(() => ShowPhotoPreview(bytes, label)); return; }
        try
        {
            picPhotoPreview.Image?.Dispose();
            using var ms = new MemoryStream(bytes);
            picPhotoPreview.Image = new Bitmap(Image.FromStream(ms));
            txtPhotoUrl.Text = label;
        }
        catch
        {
            txtPhotoUrl.Text = "(»çÁø Ç¥½Ã ½ÇÆÐ)";
        }
    }

    // ÀÌÀü ½Ã±×´ÏÃ³ È£È¯¿ë ¿À¹ö·Îµå
    public void SetInitialValues(string? userId, string? name, string? photoUrl = null, string? password = null)
    {
        var p = new PersonInfo
        {
            UserID   = userId   ?? "",
            Name     = name     ?? "",
            Photo    = photoUrl ?? "",
            Password = password ?? ""
        };
        SetInitialValues(p);
    }

    // ¦¡¦¡ UI ±¸¼º ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private void InitializeComponent()
    {
        this.Text            = "»ç¿ëÀÚ Ãß°¡/¼öÁ¤";
        this.Size            = new Size(680, 700);
        this.StartPosition   = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox     = false;
        this.MinimizeBox     = false;

        var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

        int y          = 10;
        int labelWidth = 130;
        int ctrlX      = 150;
        int ctrlWidth  = 390;

        Label MkLabel(string text) => new Label
        {
            Text      = text,
            Location  = new Point(10, y + 2),
            Width     = labelWidth,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // ¦¡¦¡ »ç¿ëÀÚ¸í ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        mainPanel.Controls.Add(MkLabel("»ç¿ëÀÚ¸í:"));
        txtName = new TextBox { Location = new Point(ctrlX, y), Width = ctrlWidth, MaxLength = 64 };
        mainPanel.Controls.Add(txtName);
        y += 38;

        // ¦¡¦¡ µ¿ / È£ / ¸â¹ö ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        mainPanel.Controls.Add(new Label { Text = "µ¿:", Location = new Point(10, y + 2), Width = 35 });
        txtDong = new TextBox { Location = new Point(48, y), Width = 75, MaxLength = 10 };
        mainPanel.Controls.Add(txtDong);

        mainPanel.Controls.Add(new Label { Text = "È£:", Location = new Point(135, y + 2), Width = 28 });
        txtHo = new TextBox { Location = new Point(166, y), Width = 75, MaxLength = 10 };
        mainPanel.Controls.Add(txtHo);

        mainPanel.Controls.Add(new Label { Text = "¸â¹ö:", Location = new Point(253, y + 2), Width = 43 });
        txtMember = new TextBox { Location = new Point(299, y), Width = 60, MaxLength = 5 };
        mainPanel.Controls.Add(txtMember);
        y += 38;

        // ¦¡¦¡ Ä«µå¹øÈ£ ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        mainPanel.Controls.Add(MkLabel("Ä«µå¹øÈ£:"));
        txtCard = new TextBox { Location = new Point(ctrlX, y), Width = ctrlWidth, MaxLength = 32 };
        mainPanel.Controls.Add(txtCard);
        y += 38;

        // ¦¡¦¡ ºñ¹Ð¹øÈ£ ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        mainPanel.Controls.Add(MkLabel("ºñ¹Ð¹øÈ£:"));
        txtPassword = new TextBox
        {
            Location              = new Point(ctrlX, y),
            Width                 = ctrlWidth,
            MaxLength             = 16,
            UseSystemPasswordChar = true
        };
        mainPanel.Controls.Add(txtPassword);
        y += 38;

        // ¦¡¦¡ Áö¹® (ÀÐ±â Àü¿ë) ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        mainPanel.Controls.Add(MkLabel("Áö¹®:"));
        lblFingerprintCount = new Label { Location = new Point(ctrlX, y + 2), Width = 200, Text = "¹Ìµî·Ï", ForeColor = Color.Gray };
        mainPanel.Controls.Add(lblFingerprintCount);
        mainPanel.Controls.Add(new Label
        {
            Text      = "(´Ü¸»±â¿¡¼­ Á÷Á¢ µî·Ï)",
            Location  = new Point(ctrlX + 205, y + 2),
            Width     = 180,
            ForeColor = Color.Gray,
            Font      = new System.Drawing.Font(this.Font.FontFamily, 8f)
        });
        y += 30;

        // ¦¡¦¡ Á¤¸Æ (ÀÐ±â Àü¿ë) ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        mainPanel.Controls.Add(MkLabel("Á¤¸Æ:"));
        lblPalmveinCount = new Label { Location = new Point(ctrlX, y + 2), Width = 200, Text = "¹Ìµî·Ï", ForeColor = Color.Gray };
        mainPanel.Controls.Add(lblPalmveinCount);
        mainPanel.Controls.Add(new Label
        {
            Text      = "(´Ü¸»±â¿¡¼­ Á÷Á¢ µî·Ï)",
            Location  = new Point(ctrlX + 205, y + 2),
            Width     = 180,
            ForeColor = Color.Gray,
            Font      = new System.Drawing.Font(this.Font.FontFamily, 8f)
        });
        y += 38;

        // ¦¡¦¡ »çÁø ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        mainPanel.Controls.Add(MkLabel("»çÁø:"));
        txtPhotoUrl = new TextBox
        {
            Location  = new Point(ctrlX, y),
            Width     = 282,
            ReadOnly  = true,
            BackColor = SystemColors.Window
        };
        mainPanel.Controls.Add(txtPhotoUrl);

        btnBrowsePhoto = new Button
        {
            Text     = "Ã£¾Æº¸±â",
            Location = new Point(ctrlX + 290, y - 2),
            Size     = new Size(90, 28)
        };
        btnBrowsePhoto.Click += BtnBrowsePhoto_Click;
        mainPanel.Controls.Add(btnBrowsePhoto);
        y += 34;

        // »çÁø ¹Ì¸®º¸±â
        picPhotoPreview = new PictureBox
        {
            Location    = new Point(ctrlX, y),
            Size        = new Size(130, 130),
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode    = PictureBoxSizeMode.Zoom,
            BackColor   = Color.WhiteSmoke
        };
        mainPanel.Controls.Add(picPhotoPreview);
        y += 140;

        // ¦¡¦¡ ´Ü¸»±â ÇÒ´ç ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        var grpDevices = new GroupBox
        {
            Text     = "´Ü¸»±â ÇÒ´ç",
            Location = new Point(10, y),
            Size     = new Size(630, 145)
        };
        lstDevices = new CheckedListBox
        {
            Location     = new Point(10, 22),
            Size         = new Size(605, 110),
            CheckOnClick = true
        };
        grpDevices.Controls.Add(lstDevices);
        mainPanel.Controls.Add(grpDevices);
        y += 160;

        // ¦¡¦¡ ÇÏ´Ü ¹öÆ° ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        btnUploadToDevices = new Button
        {
            Text     = "ÀúÀå ¹× ¼±ÅÃÇÑ ´Ü¸»±â·Î Àü¼Û",
            Location = new Point(10, y),
            Size     = new Size(245, 35)
        };
        btnUploadToDevices.Click += BtnUploadToDevices_Click;
        mainPanel.Controls.Add(btnUploadToDevices);

        btnOK = new Button { Text = "ÀúÀå", Location = new Point(265, y), Size = new Size(100, 35) };
        btnOK.Click += BtnOK_Click;
        mainPanel.Controls.Add(btnOK);

        btnCancel = new Button
        {
            Text         = "Ãë¼Ò",
            Location     = new Point(375, y),
            Size         = new Size(100, 35),
            DialogResult = DialogResult.Cancel
        };
        mainPanel.Controls.Add(btnCancel);

        this.Controls.Add(mainPanel);
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
    }

    // ¦¡¦¡ ´Ü¸»±â ¸ñ·Ï ·Îµå ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private async Task LoadDevices()
    {
        if (_httpClient == null) return;
        try
        {
            var devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
            if (devices != null)
            {
                _allDevices = devices;
                foreach (var d in devices)
                    lstDevices.Items.Add($"{d.DeviceName} ({d.IpAddress})");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"´Ü¸»±â ¸ñ·Ï ·Îµå ½ÇÆÐ: {ex.Message}", "¿À·ù",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ¦¡¦¡ »çÁø ¼±ÅÃ ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private void BtnBrowsePhoto_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "ÀÌ¹ÌÁö ÆÄÀÏ|*.jpg;*.jpeg;*.png;*.bmp|¸ðµç ÆÄÀÏ|*.*",
            Title  = "¾ó±¼ »çÁø ¼±ÅÃ"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            // ÆÄÀÏÀ» Á÷Á¢ ÀÐ¾î BitmapÀ¸·Î º¯È¯ ÈÄ JPEG ¹ÙÀÌÆ®·Î ÀúÀå
            // (FacePhotoEditorForm ¾øÀÌ ¹Ù·Î µî·Ï ? ÆíÁýÀÌ ÇÊ¿äÇÏ¸é ÃßÈÄ ±¸Çö)
            byte[] imageBytes;
            using (var srcImg = Image.FromFile(dlg.FileName))
            using (var bmp = new Bitmap(srcImg))
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                imageBytes = ms.ToArray();
            }

            if (imageBytes.Length == 0)
            {
                MessageBox.Show("»çÁø ÆÄÀÏÀ» ÀÐÀ» ¼ö ¾ø½À´Ï´Ù.", "¿À·ù",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // PhotoData¿¡ ÀúÀå
            Person.PhotoData = imageBytes;
            txtPhotoUrl.Text  = dlg.FileName;

            // ¹Ì¸®º¸±â
            picPhotoPreview.Image?.Dispose();
            using var previewMs = new MemoryStream(imageBytes);
            picPhotoPreview.Image = new Bitmap(Image.FromStream(previewMs));

            MessageBox.Show("»çÁøÀÌ µî·ÏµÇ¾ú½À´Ï´Ù.", "¿Ï·á",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"»çÁø Ã³¸® ½ÇÆÐ: {ex.Message}", "¿À·ù",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ¦¡¦¡ ÀúÀå ¹× ´Ü¸»±â Àü¼Û ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private async void BtnUploadToDevices_Click(object? sender, EventArgs e)
    {
        if (_httpClient == null)
        {
            MessageBox.Show("HTTP Å¬¶óÀÌ¾ðÆ®°¡ ÃÊ±âÈ­µÇÁö ¾Ê¾Ò½À´Ï´Ù", "¿À·ù",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (lstDevices.CheckedIndices.Count == 0)
        {
            MessageBox.Show("Àü¼ÛÇÒ ´Ü¸»±â¸¦ ¼±ÅÃÇØÁÖ¼¼¿ä", "¾È³»",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("»ç¿ëÀÚ¸íÀ» ÀÔ·ÂÇØÁÖ¼¼¿ä", "ÀÔ·Â ¿À·ù",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!TryBuildUserID(out string builtUserID))
        {
            MessageBox.Show("µ¿/È£/¸â¹ö ¹øÈ£¸¦ ¿Ã¹Ù¸£°Ô ÀÔ·ÂÇØÁÖ¼¼¿ä.\n(µ¿¡¤È£¡¤¸â¹ö´Â ¸ðµÎ ¼ýÀÚ¿©¾ß ÇÕ´Ï´Ù)",
                "ÀÔ·Â ¿À·ù", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnUploadToDevices.Enabled = false;
            btnUploadToDevices.Text    = "Àü¼Û Áß...";

            var personInfo = BuildPersonInfo(builtUserID);

            bool userExists = false;
            try
            {
                var chk = await _httpClient.PostAsJsonAsync("/api/People/GetDetail", new { UserID = personInfo.UserID });
                var chkR = await chk.Content.ReadFromJsonAsync<BrowserApiResponse<PersonInfo>>();
                userExists = chkR?.Code == 0;
            }
            catch { }

            bool edit     = userExists || (IsEditMode && !string.IsNullOrEmpty(_originalUserID));
            string ep     = edit ? "/api/People/Update" : "/api/People/New";
            var addResp   = await _httpClient.PostAsJsonAsync(ep, personInfo);
            var addResult = await addResp.Content.ReadFromJsonAsync<BrowserApiResponse<object>>();

            if (addResult == null || addResult.Code != 0)
            {
                MessageBox.Show($"¼­¹ö ÀúÀå ½ÇÆÐ: {addResult?.Msg ?? "¾Ë ¼ö ¾ø´Â ¿À·ù"}",
                    "¿À·ù", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int successCount = 0, failCount = 0;
            var errors = new List<string>();

            foreach (int idx in lstDevices.CheckedIndices)
            {
                if (idx >= _allDevices.Count) continue;
                var dev = _allDevices[idx];
                try
                {
                    var r = await _httpClient.PostAsync($"/admin/devices/{dev.SN}/request-add-people", null);
                    if (r.IsSuccessStatusCode) successCount++;
                    else { failCount++; errors.Add($"{dev.DeviceName}: HTTP {r.StatusCode}"); }
                }
                catch (Exception ex) { failCount++; errors.Add($"{dev.DeviceName}: {ex.Message}"); }
            }

            if (failCount == 0)
            {
                MessageBox.Show(
                    $"¼º°ø: {successCount}°³ ´Ü¸»±â·Î »ç¿ëÀÚ Àü¼ÛÀ» ¿äÃ»Çß½À´Ï´Ù.\n" +
                    "´Ü¸»±â°¡ ´ÙÀ½ Keepalive ½ÅÈ£¸¦ º¸³¾ ¶§ »ç¿ëÀÚ Á¤º¸¸¦ ´Ù¿î·ÎµåÇÕ´Ï´Ù." +
                    (personInfo.Photo != null ? "\n(¾ó±¼ »çÁø Æ÷ÇÔ)" : ""),
                    "Àü¼Û ¼º°ø", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Person            = personInfo;
                AlreadySaved      = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string detail = errors.Count > 0 ? "\n\n¿À·ù:\n" + string.Join("\n", errors.Take(5)) : "";
                MessageBox.Show($"Àü¼Û ¿Ï·á: ¼º°ø {successCount}°³, ½ÇÆÐ {failCount}°³{detail}",
                    "Àü¼Û °á°ú", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Àü¼Û ½ÇÆÐ: {ex.Message}", "¿À·ù", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnUploadToDevices.Enabled = true;
            btnUploadToDevices.Text    = "ÀúÀå ¹× ¼±ÅÃÇÑ ´Ü¸»±â·Î Àü¼Û";
        }
    }

    // ¦¡¦¡ ÀúÀå ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private void BtnOK_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("»ç¿ëÀÚ¸íÀ» ÀÔ·ÂÇØÁÖ¼¼¿ä", "ÀÔ·Â ¿À·ù",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return;
        }

        if (!IsEditMode && !TryBuildUserID(out _))
        {
            MessageBox.Show("µ¿/È£/¸â¹ö ¹øÈ£¸¦ ¿Ã¹Ù¸£°Ô ÀÔ·ÂÇØÁÖ¼¼¿ä.\n(µ¿¡¤È£¡¤¸â¹ö´Â ¸ðµÎ ¼ýÀÚ¿©¾ß ÇÕ´Ï´Ù)",
                "ÀÔ·Â ¿À·ù", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDong.Focus();
            return;
        }

        TryBuildUserID(out string builtUserID);
        string finalUserID = (IsEditMode && !string.IsNullOrEmpty(_originalUserID)) ? _originalUserID! : builtUserID;

        Person = BuildPersonInfo(finalUserID);
        // PhotoUrl(= Photo alias)Àº µ¤¾î¾²Áö ¾ÊÀ½ ? BuildPersonInfo¿¡¼­ ÀÌ¹Ì Base64·Î ¼¼ÆÃµÊ

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    // ¦¡¦¡ °øÅë: PersonInfo ºôµå ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private PersonInfo BuildPersonInfo(string userId)
    {
        var p = new PersonInfo
        {
            UserID       = userId,
            Name         = txtName.Text.Trim(),
            CardNum      = string.IsNullOrWhiteSpace(txtCard.Text) ? "0" : txtCard.Text.Trim(),
            Password     = txtPassword.Text.Trim(),
            Fingerprints = Person.Fingerprints,
            Palmveins    = Person.Palmveins
        };

        if (Person.PhotoData != null && Person.PhotoData.Length > 0)
        {
            p.Photo = Convert.ToBase64String(Person.PhotoData);
            System.Diagnostics.Debug.WriteLine($"[BuildPersonInfo] PhotoData={Person.PhotoData.Length}bytes ¡æ Base64({p.Photo.Length}chars)");
        }
        else
        {
            p.Photo = Person.Photo;
            System.Diagnostics.Debug.WriteLine($"[BuildPersonInfo] PhotoData=null, Photo={p.Photo?.Substring(0, Math.Min(60, p.Photo?.Length ?? 0))}");
        }

        return p;
    }

    // ¦¡¦¡ UserID ºôµå ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    private bool TryBuildUserID(out string userID)
    {
        userID = string.Empty;
        if (!long.TryParse(txtDong.Text.Trim(),   out long dong)   ||
            !long.TryParse(txtHo.Text.Trim(),     out long ho)     ||
            !long.TryParse(txtMember.Text.Trim(), out long member))
            return false;
        userID = (dong * 1_000_000L + ho * 100L + member).ToString();
        return true;
    }
}
