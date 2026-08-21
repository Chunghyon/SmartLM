using FaceDeviceHttpPcServer.Services;
using FaceDeviceLogLevel = FaceDeviceHttpPcServer.Services.LogLevel;

namespace FaceDeviceHttpPcServer.Forms;

public sealed class MainForm : Form
{
    // ── Controls ──────────────────────────────────────────────────────────
    private readonly RichTextBox _logBox;
    private readonly StatusStrip _status;
    private readonly ToolStripStatusLabel _statusLabel;
    private readonly ToolStripStatusLabel _countLabel;
    private readonly ToolStrip _toolbar;

    // ── State ─────────────────────────────────────────────────────────────
    private int _totalRequests;
    private bool _paused;
    private const int MaxLines = 2000;
    private string _serverUrl = string.Empty;
    private readonly NotifyIcon _tray;
    private bool _exitRequested;
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Action? RequestShutdown { get; set; }
    private readonly List<string> _plainLog = new();


    public MainForm()
    {
        // ── Form ─────────────────────────────────────────────────────────
        Text = "FaceDevice HTTP Server - 실시간 로그";
        Size = new Size(1100, 720);
        MinimumSize = new Size(700, 400);
        StartPosition = FormStartPosition.CenterScreen;

        // ── Toolbar ──────────────────────────────────────────────────────
        _toolbar = new ToolStrip { Dock = DockStyle.Top };

        var btnClear = new ToolStripButton("지우기") { ToolTipText = "로그 창 지우기 (Ctrl+L)" };
        var btnPause = new ToolStripButton("일시정지") { ToolTipText = "로그 일시정지/재개", CheckOnClick = true };
        var btnCopy = new ToolStripButton("복사") { ToolTipText = "선택한 내용 복사 (Ctrl+C)" };
        var btnSave = new ToolStripButton("저장") { ToolTipText = "로그 파일로 저장" };
        var btnWebAdmin = new ToolStripButton("웹 관리자") { ToolTipText = "브라우저에서 관리자 인터페이스 열기" };

        var sepFilter = new ToolStripSeparator();
        var lblFilter = new ToolStripLabel("필터: ");
        var cbFilter = new ToolStripComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            AutoSize = false,
            Width = 130
        };
        cbFilter.Items.AddRange(["전체", "Request", "Info", "Warn", "Error"]);
        cbFilter.SelectedIndex = 0;

        _toolbar.Items.AddRange([
            btnClear, btnPause, btnCopy, btnSave, btnWebAdmin,
            sepFilter, lblFilter, cbFilter
        ]);

        btnClear.Click += (_, _) => ClearLog();
        btnPause.Click += (_, _) =>
        {
            _paused = btnPause.Checked;
            btnPause.Text = _paused ? "재개" : "일시정지";
        };
        btnCopy.Click += (_, _) => CopyLog();
        btnSave.Click += (_, _) => SaveLog();
        btnWebAdmin.Click += (_, _) => OpenWebAdmin();
        cbFilter.SelectedIndexChanged += (_, _) => ApplyFilter(cbFilter.SelectedItem?.ToString() ?? "전체");

        // ── Log RichTextBox ───────────────────────────────────────────────
        _logBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            HideSelection = true,
            BackColor = Color.FromArgb(18, 18, 18),
            ForeColor = Color.LightGray,
            Font = new Font("Consolas", 9.5f),
            BorderStyle = BorderStyle.None,
            ScrollBars = RichTextBoxScrollBars.Vertical,
            WordWrap = false
        };
        _logBox.KeyDown += (_, e) =>
        {
            if (e.Control && e.KeyCode == Keys.L) { ClearLog(); e.Handled = true; }
        };

        // ── Status bar ────────────────────────────────────────────────────
        _status = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel("서버 시작 중…") { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
        _countLabel = new ToolStripStatusLabel("요청: 0") { BorderSides = ToolStripStatusLabelBorderSides.Left };
        _status.Items.AddRange([_statusLabel, _countLabel]);

        // ── Layout ───────────────────────────────────────────────────────
        Controls.AddRange([_logBox, _toolbar, _status]);

        // ── Subscribe to LogHub ──────────────────────────────────────────
        LogHub.Instance.EntryAdded += OnEntryAdded;

        // ── Keyboard shortcuts ────────────────────────────────────────────
        KeyPreview = true;
        KeyDown += (_, e) =>
        {
            if (e.Control && e.KeyCode == Keys.L) ClearLog();
        };

        FormClosed += (_, _) => LogHub.Instance.EntryAdded -= OnEntryAdded;

        var trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("로그 창 열기", null, (_, _) => ShowFromTray());
        trayMenu.Items.Add("종료", null, (_, _) => ExitFromTray());
        _tray = new NotifyIcon
        {
            Text = "SmartLM FDHS",
            Icon = SystemIcons.Application,
            Visible = true,
            ContextMenuStrip = trayMenu
        };
        _tray.DoubleClick += (_, _) => ShowFromTray();

        ShowInTaskbar = false;
        FormClosing += OnFormClosing;
    }

    private void OnFormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_exitRequested || e.CloseReason != CloseReason.UserClosing)
            return;
        e.Cancel = true;
        Hide();
        ShowInTaskbar = false;
        if (_tray != null) _tray.Visible = true;
    }

    private void ShowFromTray()
    {
        Show();
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = true;
        RebuildLogBoxFromBuffer();
        Activate();
    }

    private void ExitFromTray()
    {
        _exitRequested = true;
        try
        {
            if (_tray != null)
            {
                _tray.Visible = false;
                _tray.Dispose();
            }
        }
        catch { }
        RequestShutdown?.Invoke();
    }


    // ── LogHub callback (may arrive on any thread) ────────────────────────

    private string _currentFilter = "전체";

    private void OnEntryAdded(LogEntry entry)
    {
        if (_paused) return;
        if (!PassesFilter(entry, _currentFilter)) return;

        // 창이 없거나 숨김이면 RichTextBox를 건드리지 않는다.
        // 숨은 창에서 Select/Trim 하면 나중에 로그마다 시스템 비프가 난다.
        if (!IsHandleCreated || !Visible)
        {
            AddPlain(entry);
            Interlocked.Increment(ref _totalRequests);
            return;
        }

        if (InvokeRequired)
        {
            BeginInvoke(() => OnEntryAdded(entry));
            return;
        }

        AppendEntry(entry);
        Interlocked.Increment(ref _totalRequests);
        _countLabel.Text = $"요청: {_totalRequests:N0}";
        _statusLabel.Text = $"마지막: {entry.Timestamp:HH:mm:ss}  {entry.Message}";
    }

    // ── Append a single entry ─────────────────────────────────────────────

    private static string FormatPlain(LogEntry entry, string prefix="·")
    {
        var msg = entry.Message ?? "";
        var detail = entry.Detail;
        if (!string.IsNullOrEmpty(detail) && detail.Length > 400)
            detail = detail[..400] + " …";
        var plain = $"[{entry.Timestamp:HH:mm:ss.fff}] {prefix} {msg}";
        if (!string.IsNullOrWhiteSpace(detail))
            plain += Environment.NewLine + "    " + detail.Replace("\n", " ").Trim();
        return plain;
    }

    private void AddPlain(LogEntry entry)
    {
        var prefix = entry.Level switch
        {
            FaceDeviceLogLevel.Request => ">",
            FaceDeviceLogLevel.Warn => "!",
            FaceDeviceLogLevel.Error => "X",
            _ => "·"
        };
        _plainLog.Add(FormatPlain(entry, prefix));
        if (_plainLog.Count > MaxLines)
            _plainLog.RemoveRange(0, _plainLog.Count - MaxLines / 2);
    }

    private void RebuildLogBoxFromBuffer()
    {
        try
        {
            _logBox.Clear();
            foreach (var line in _plainLog.TakeLast(500))
                _logBox.AppendText(line + Environment.NewLine);
            _logBox.SelectionStart = _logBox.TextLength;
            _logBox.ScrollToCaret();
        }
        catch { }
    }

    private void AppendEntry(LogEntry entry)
    {
        AddPlain(entry);
        if (!Visible) return;
        try
        {
            if (_plainLog.Count >= MaxLines)
            {
                RebuildLogBoxFromBuffer();
                return;
            }

            var (color, prefix) = entry.Level switch
            {
                FaceDeviceLogLevel.Request => (Color.FromArgb(100, 200, 255), ">"),
                FaceDeviceLogLevel.Warn => (Color.FromArgb(255, 220, 80), "!"),
                FaceDeviceLogLevel.Error => (Color.FromArgb(255, 90, 90), "X"),
                _ => (Color.FromArgb(160, 255, 160), "·")
            };

            AppendColored($"[{entry.Timestamp:HH:mm:ss.fff}] ", Color.FromArgb(120, 120, 120));
            AppendColored($"{prefix} ", color);
            AppendColored(entry.Message + "\n", color);
            if (!string.IsNullOrWhiteSpace(entry.Detail))
            {
                var d = entry.Detail.Length > 400 ? entry.Detail[..400] + " …" : entry.Detail;
                AppendColored("    " + d.Replace("\n", " ").Trim() + "\n", Color.FromArgb(200, 200, 200));
            }
            _logBox.SelectionStart = _logBox.TextLength;
            _logBox.SelectionLength = 0;
            _logBox.ScrollToCaret();
        }
        catch { }
    }

    private void AppendColored(string text, Color color)
    {
        _logBox.SelectionStart = _logBox.TextLength;
        _logBox.SelectionLength = 0;
        _logBox.SelectionColor = color;
        _logBox.AppendText(text);
    }

    private void CopyLog()
    {
        try
        {
            if (_logBox.SelectionLength > 0)
                _logBox.Copy();
            else if (_logBox.TextLength > 0)
            {
                _logBox.SelectAll();
                _logBox.Copy();
                _logBox.SelectionLength = 0;
                _logBox.SelectionStart = _logBox.TextLength;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("클립보드 복사 실패: " + ex.Message, "복사", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ── Filter ────────────────────────────────────────────────────────────

    private static bool PassesFilter(LogEntry entry, string filter) => filter switch
    {
        "Request" => entry.Level == FaceDeviceLogLevel.Request,
        "Info" => entry.Level == FaceDeviceLogLevel.Info,
        "Warn" => entry.Level == FaceDeviceLogLevel.Warn,
        "Error" => entry.Level == FaceDeviceLogLevel.Error,
        _ => true
    };

    private void ApplyFilter(string filter)
    {
        _currentFilter = filter;
        // 필터 변경 시 창 초기화 (재표시는 새 요청부터 적용)
        ClearLog();
        LogHub.Instance.Info($"필터 변경: {filter}");
    }

    // ── Clear / Save ──────────────────────────────────────────────────────

    private void ClearLog()
    {
        _logBox.Clear();
        _plainLog.Clear();
        LogHub.Instance.Info("로그 창 초기화");
    }

    private static string GetLogFolder()
    {
        var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        if (string.IsNullOrWhiteSpace(docs))
            docs = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dir = Path.Combine(docs, "SmartLM_Data", "FDHS_Log");
        Directory.CreateDirectory(dir);
        return dir;
    }

    private void SaveLog()
    {
        var dir = GetLogFolder();
        var path = Path.Combine(dir, $"FaceDevice_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
        try
        {
            var text = _plainLog.Count > 0
                ? string.Join(Environment.NewLine, _plainLog)
                : (_logBox.Text ?? string.Empty);
            File.WriteAllText(path, text);
            _statusLabel.Text = "로그 저장 완료: " + path;
        }
        catch (Exception ex)
        {
            MessageBox.Show("저장 실패: " + ex.Message, "저장", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Public: server status ─────────────────────────────────────────────

    public void SetServerUrl(string url)
    {
        if (InvokeRequired) { Invoke(() => SetServerUrl(url)); return; }
        _serverUrl = url;
        _statusLabel.Text = $"서버 실행 중: {url}";
        LogHub.Instance.Info($"서버 시작됨 → {url}");
    }

    private void OpenWebAdmin()
    {
        if (string.IsNullOrWhiteSpace(_serverUrl))
        {
            MessageBox.Show("서버 URL이 아직 설정되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var adminUrl = _serverUrl.TrimEnd('/') + "/admin/";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = adminUrl,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
            LogHub.Instance.Info($"웹 관리자 열기: {adminUrl}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"브라우저를 열 수 없습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LogHub.Instance.Error($"웹 관리자 열기 실패", ex.Message);
        }
    }
}
