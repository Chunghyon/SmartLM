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

    public MainForm()
    {
        // ── Form ─────────────────────────────────────────────────────────
        Text = "FaceDevice HTTP Server ? 실시간 로그";
        Size = new Size(1100, 720);
        MinimumSize = new Size(700, 400);
        StartPosition = FormStartPosition.CenterScreen;

        // ── Toolbar ──────────────────────────────────────────────────────
        _toolbar = new ToolStrip { Dock = DockStyle.Top };

        var btnClear = new ToolStripButton("?? 지우기") { ToolTipText = "로그 창 지우기 (Ctrl+L)" };
        var btnPause = new ToolStripButton("? 일시정지") { ToolTipText = "로그 일시정지/재개", CheckOnClick = true };
        var btnCopy = new ToolStripButton("?? 복사") { ToolTipText = "선택한 내용 복사 (Ctrl+C)" };
        var btnSave = new ToolStripButton("?? 저장") { ToolTipText = "로그 파일로 저장" };
        var btnWebAdmin = new ToolStripButton("?? 웹 관리자") { ToolTipText = "브라우저에서 관리자 인터페이스 열기" };

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
        btnPause.Click += (_, _) => { _paused = btnPause.Checked; btnPause.Text = _paused ? "▶ 재개" : "? 일시정지"; };
        btnCopy.Click += (_, _) =>
        {
            if (_logBox?.SelectionLength <= 0) return;
            var selectedText = _logBox?.SelectedText;
            if (!string.IsNullOrEmpty(selectedText))
                Clipboard.SetText(selectedText);
        };
        btnSave.Click += (_, _) => SaveLog();
        btnWebAdmin.Click += (_, _) => OpenWebAdmin();
        cbFilter.SelectedIndexChanged += (_, _) => ApplyFilter(cbFilter.SelectedItem?.ToString() ?? "전체");

        // ── Log RichTextBox ───────────────────────────────────────────────
        _logBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
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
    }

    // ── LogHub callback (may arrive on any thread) ────────────────────────

    private string _currentFilter = "전체";

    private void OnEntryAdded(LogEntry entry)
    {
        if (_paused) return;

        if (_InvokeRequired())
        {
            BeginInvoke(() => OnEntryAdded(entry));
            return;
        }

        if (!PassesFilter(entry, _currentFilter)) return;

        AppendEntry(entry);
        Interlocked.Increment(ref _totalRequests);
        _countLabel.Text = $"요청: {_totalRequests:N0}";
        _statusLabel.Text = $"마지막: {entry.Timestamp:HH:mm:ss}  {entry.Message}";
    }

    private bool _InvokeRequired() => InvokeRequired;

    // ── Append a single entry ─────────────────────────────────────────────

    private void AppendEntry(LogEntry entry)
    {
        _logBox.SuspendLayout();

        // 최대 줄 수 유지
        TrimLines();

        var (color, prefix) = entry.Level switch
        {
            FaceDeviceLogLevel.Request => (Color.FromArgb(100, 200, 255), "?"),
            FaceDeviceLogLevel.Warn => (Color.FromArgb(255, 220, 80), "?"),
            FaceDeviceLogLevel.Error => (Color.FromArgb(255, 90, 90), "?"),
            _ => (Color.FromArgb(160, 255, 160), "·")
        };

        // 타임스탬프
        AppendColored($"[{entry.Timestamp:HH:mm:ss.fff}] ", Color.FromArgb(120, 120, 120));
        AppendColored($"{prefix} ", color);
        AppendColored(entry.Message + "\n", color);

        // 상세 본문 (들여쓰기)
        if (!string.IsNullOrWhiteSpace(entry.Detail))
        {
            foreach (var line in entry.Detail.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                AppendColored("    " + line.TrimEnd() + "\n", Color.FromArgb(200, 200, 200));
            }
        }

        _logBox.ResumeLayout();
        _logBox.ScrollToCaret();
    }

    private void AppendColored(string text, Color color)
    {
        _logBox.SelectionStart = _logBox.TextLength;
        _logBox.SelectionLength = 0;
        _logBox.SelectionColor = color;
        _logBox.AppendText(text);
    }

    private void TrimLines()
    {
        // 줄 수가 MaxLines를 초과하면 앞쪽 절반 제거
        if (_logBox.Lines.Length <= MaxLines) return;
        var keep = MaxLines / 2;
        var start = _logBox.GetFirstCharIndexFromLine(_logBox.Lines.Length - keep);
        _logBox.Select(0, start);
        _logBox.SelectedText = string.Empty;
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
        LogHub.Instance.Info("로그 창 초기화");
    }

    private void SaveLog()
    {
        using var dlg = new SaveFileDialog
        {
            Title = "로그 저장",
            Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일|*.*",
            FileName = $"FaceDevice_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        File.WriteAllText(dlg.FileName, _logBox.Text);
        LogHub.Instance.Info($"로그 저장 완료: {dlg.FileName}");
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
