using System.Drawing.Drawing2D;

namespace FaceDeviceDesktopClient.Forms;

public partial class FacePhotoEditorForm : Form
{
    private PictureBox pictureBox = null!;
    private TrackBar zoomTrackBar = null!;
    private Button btnMoveUp = null!;
    private Button btnMoveDown = null!;
    private Button btnMoveLeft = null!;
    private Button btnMoveRight = null!;
    private Button btnOK = null!;
    private Button btnCancel = null!;
    private Label lblInstructions = null!;

    private Image? _originalImage;
    private float _zoom = 1.0f;
    private Point _offset = Point.Empty;
    private const int CANVAS_SIZE = 400;
    private const int FACE_GUIDE_SIZE = 300;

    public byte[]? ProcessedImageData { get; private set; }

    public FacePhotoEditorForm(string imagePath)
    {
        try
        {
            _originalImage = Image.FromFile(imagePath);
            InitializeComponent();
            DrawPreview();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"이미지 로드 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.DialogResult = DialogResult.Cancel;
        }
    }

    private void InitializeComponent()
    {
        this.Text = "얼굴 사진 편집";
        this.Size = new Size(600, 800);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        var mainPanel = new Panel
        {
            Location = new Point(0, 0),
            Size = new Size(580, 760),
            AutoScroll = true,
            Padding = new Padding(20)
        };

        // 안내 레이블
        lblInstructions = new Label
        {
            Text = "얼굴이 가이드라인(파란색 원) 안에 위치하도록 조정해주세요",
            Location = new Point(20, 10),
            Size = new Size(540, 40),
            ForeColor = Color.Blue,
            Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };
        mainPanel.Controls.Add(lblInstructions);

        // 캔버스 (PictureBox)
        pictureBox = new PictureBox
        {
            Location = new Point(80, 60),
            Size = new Size(CANVAS_SIZE, CANVAS_SIZE),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White
        };
        pictureBox.Paint += PictureBox_Paint;
        mainPanel.Controls.Add(pictureBox);

        // 줌 컨트롤
        var lblZoom = new Label
        {
            Text = "확대/축소:",
            Location = new Point(20, CANVAS_SIZE + 80),
            Width = 100
        };
        mainPanel.Controls.Add(lblZoom);

        zoomTrackBar = new TrackBar
        {
            Location = new Point(120, CANVAS_SIZE + 75),
            Width = 300,
            Minimum = 50,
            Maximum = 300,
            Value = 100,
            TickFrequency = 25
        };
        zoomTrackBar.ValueChanged += (s, e) =>
        {
            _zoom = zoomTrackBar.Value / 100f;
            DrawPreview();
        };
        mainPanel.Controls.Add(zoomTrackBar);

        var lblZoomValue = new Label
        {
            Location = new Point(430, CANVAS_SIZE + 80),
            Width = 80,
            Text = "100%"
        };
        zoomTrackBar.ValueChanged += (s, e) =>
        {
            lblZoomValue.Text = $"{zoomTrackBar.Value}%";
        };
        mainPanel.Controls.Add(lblZoomValue);

        // 이동 컨트롤
        var lblMove = new Label
        {
            Text = "위치 조정:",
            Location = new Point(20, CANVAS_SIZE + 130),
            Width = 100
        };
        mainPanel.Controls.Add(lblMove);

        int btnY = CANVAS_SIZE + 125;
        int btnSize = 50;

        btnMoveUp = new Button
        {
            Text = "▲",
            Location = new Point(240, btnY),
            Size = new Size(btnSize, btnSize)
        };
        btnMoveUp.Click += (s, e) => { _offset.Y -= 10; DrawPreview(); };
        mainPanel.Controls.Add(btnMoveUp);

        btnMoveLeft = new Button
        {
            Text = "◀",
            Location = new Point(180, btnY + 60),
            Size = new Size(btnSize, btnSize)
        };
        btnMoveLeft.Click += (s, e) => { _offset.X -= 10; DrawPreview(); };
        mainPanel.Controls.Add(btnMoveLeft);

        btnMoveRight = new Button
        {
            Text = "▶",
            Location = new Point(300, btnY + 60),
            Size = new Size(btnSize, btnSize)
        };
        btnMoveRight.Click += (s, e) => { _offset.X += 10; DrawPreview(); };
        mainPanel.Controls.Add(btnMoveRight);

        btnMoveDown = new Button
        {
            Text = "▼",
            Location = new Point(240, btnY + 120),
            Size = new Size(btnSize, btnSize)
        };
        btnMoveDown.Click += (s, e) => { _offset.Y += 10; DrawPreview(); };
        mainPanel.Controls.Add(btnMoveDown);

        // 초기화 버튼
        var btnReset = new Button
        {
            Text = "초기화",
            Location = new Point(380, btnY + 60),
            Size = new Size(80, 50)
        };
        btnReset.Click += (s, e) =>
        {
            _zoom = 1.0f;
            _offset = Point.Empty;
            zoomTrackBar.Value = 100;
            DrawPreview();
        };
        mainPanel.Controls.Add(btnReset);

        // 하단 버튼
        int bottomY = CANVAS_SIZE + 300;

        btnOK = new Button
        {
            Text = "확인",
            Location = new Point(270, bottomY),
            Size = new Size(120, 40)
        };
        btnOK.Click += BtnOK_Click;
        mainPanel.Controls.Add(btnOK);

        btnCancel = new Button
        {
            Text = "취소",
            Location = new Point(400, bottomY),
            Size = new Size(120, 40),
            DialogResult = DialogResult.Cancel
        };
        mainPanel.Controls.Add(btnCancel);

        this.Controls.Add(mainPanel);
        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
    }

    private void PictureBox_Paint(object? sender, PaintEventArgs e)
    {
        if (_originalImage == null) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // 중앙 위치 계산
        int centerX = CANVAS_SIZE / 2;
        int centerY = CANVAS_SIZE / 2;

        // 이미지 크기 계산
        float scaledWidth = _originalImage.Width * _zoom;
        float scaledHeight = _originalImage.Height * _zoom;

        // 이미지 그리기 위치
        float x = centerX - (scaledWidth / 2) + _offset.X;
        float y = centerY - (scaledHeight / 2) + _offset.Y;

        // 이미지 그리기
        g.DrawImage(_originalImage,
            new RectangleF(x, y, scaledWidth, scaledHeight));

        // 얼굴 가이드라인 (원) 그리기
        int guideX = (CANVAS_SIZE - FACE_GUIDE_SIZE) / 2;
        int guideY = (CANVAS_SIZE - FACE_GUIDE_SIZE) / 2;

        using (var pen = new Pen(Color.Blue, 3))
        {
            pen.DashStyle = DashStyle.Dash;
            g.DrawEllipse(pen, guideX, guideY, FACE_GUIDE_SIZE, FACE_GUIDE_SIZE);
        }

        // 중심선 그리기
        using (var pen = new Pen(Color.LightBlue, 1))
        {
            pen.DashStyle = DashStyle.Dot;
            // 수직선
            g.DrawLine(pen, centerX, guideY, centerX, guideY + FACE_GUIDE_SIZE);
            // 수평선
            g.DrawLine(pen, guideX, centerY, guideX + FACE_GUIDE_SIZE, centerY);
        }
    }

    private void DrawPreview()
    {
        pictureBox.Invalidate();
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        try
        {
            btnOK.Enabled = false;
            btnOK.Text = "처리 중...";

            // 최종 이미지 생성 (300x300 고정 크기)
            var finalImage = new Bitmap(FACE_GUIDE_SIZE, FACE_GUIDE_SIZE);

            using (var g = Graphics.FromImage(finalImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.White);

                if (_originalImage != null)
                {
                    // 가이드라인 중심에 맞춰 이미지 그리기
                    int centerX = FACE_GUIDE_SIZE / 2;
                    int centerY = FACE_GUIDE_SIZE / 2;

                    float scaledWidth = _originalImage.Width * _zoom;
                    float scaledHeight = _originalImage.Height * _zoom;

                    // 캔버스 좌표를 최종 이미지 좌표로 변환
                    float canvasCenterX = CANVAS_SIZE / 2;
                    float canvasCenterY = CANVAS_SIZE / 2;
                    float guideOffsetX = (CANVAS_SIZE - FACE_GUIDE_SIZE) / 2;
                    float guideOffsetY = (CANVAS_SIZE - FACE_GUIDE_SIZE) / 2;

                    float x = centerX - (scaledWidth / 2) + _offset.X - guideOffsetX;
                    float y = centerY - (scaledHeight / 2) + _offset.Y - guideOffsetY;

                    g.DrawImage(_originalImage,
                        new RectangleF(x, y, scaledWidth, scaledHeight));
                }
            }

            // 이미지를 바이트 배열로 변환
            using (var ms = new MemoryStream())
            {
                finalImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ProcessedImageData = ms.ToArray();
            }

            finalImage.Dispose();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"이미지 처리 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnOK.Enabled = true;
            btnOK.Text = "확인";
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _originalImage?.Dispose();
        }
        base.Dispose(disposing);
    }
}
