using System.Net.Http.Json;
using System.Text.Json;

namespace FaceDeviceDesktopClient.Forms;

/// <summary>
/// 단말기에 등록된 사용자 목록을 보여주고 추가/수정/삭제하는 창.
/// 여기서의 변경은 해당 단말기에만 영향을 주며 서버 사용자 목록과는 완전히 독립적입니다.
/// </summary>
public class DeviceUserListForm : Form
{
    private readonly DeviceInfo _device;
    private readonly HttpClient _httpClient;

    private DataGridView _dgv = null!;
    private Label _lblStatus  = null!;
    private List<PersonInfo> _people = new();

    public DeviceUserListForm(DeviceInfo device, HttpClient httpClient)
    {
        _device     = device;
        _httpClient = httpClient;
        BuildUI();
        _ = SyncAndLoadPeople();  // 창 열 때 단말기에서 최신 목록 동기화
    }

    private void BuildUI()
    {
        Text = $"사용자 정보 - {_device.DeviceName ?? _device.SN}  (단말기 전용)";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(900, 580);
        MinimumSize = new Size(750, 450);
        FormBorderStyle = FormBorderStyle.Sizable;

        // ── 버튼 패널 ────────────────────────────────────────
        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 42, Padding = new Padding(8, 6, 8, 0) };

        var btnAdd    = new Button { Text = "추가",   Size = new Size(90, 28), Location = new Point(8,  6) };
        var btnEdit   = new Button { Text = "수정",   Size = new Size(90, 28), Location = new Point(106, 6) };
        var btnDelete = new Button { Text = "삭제",   Size = new Size(90, 28), Location = new Point(204, 6),
                                     ForeColor = System.Drawing.Color.DarkRed };
        var btnRefresh = new Button { Text = "새로고침", Size = new Size(90, 28), Location = new Point(302, 6) };
        var btnClose   = new Button { Text = "닫기",   Size = new Size(90, 28), Location = new Point(400, 6) };
        btnClose.Click += (s, e) => this.Close();

        _lblStatus = new Label
        {
            AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
            Location = new Point(502, 9), Size = new Size(360, 22),
            ForeColor = System.Drawing.Color.DimGray
        };

        var infoLabel = new Label
        {
            Text = "? 이 창의 변경사항은 이 단말기에만 적용됩니다. 서버 사용자 목록에는 영향을 주지 않습니다.",
            AutoSize = false, Dock = DockStyle.Bottom, Height = 22,
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            ForeColor = System.Drawing.Color.DarkOrange,
            Font = new System.Drawing.Font(Font.FontFamily, 8.5f)
        };

        btnAdd.Click    += BtnAdd_Click;
        btnEdit.Click   += BtnEdit_Click;
        btnDelete.Click += BtnDelete_Click;
        btnRefresh.Click += async (s, e) => await SyncAndLoadPeople();

        pnlTop.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnRefresh, btnClose, _lblStatus });

        // ── 그리드 ────────────────────────────────────────────
        _dgv = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoGenerateColumns = false,
            RowHeadersWidth = 30
        };

        _dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColUserID",   HeaderText = "사용자ID",  Width = 110 });
        _dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName",     HeaderText = "이름",      Width = 130 });
        _dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPassword", HeaderText = "비밀번호",  Width = 70  });
        _dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCard",     HeaderText = "카드",      Width = 55  });
        _dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColFP",       HeaderText = "지문",      Width = 55  });
        _dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPV",       HeaderText = "손바닥",    Width = 60  });
        _dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColFace",     HeaderText = "얼굴",      Width = 55  });

        _dgv.CellFormatting += (s, e) =>
        {
            if (e.RowIndex < 0) return;
            var colName = _dgv.Columns[e.ColumnIndex].Name;
            if (colName == "ColPassword")
            {
                var pw = e.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(pw))
                {
                    e.Value = new string('\u25cf', Math.Min(pw.Length, 8));
                    e.FormattingApplied = true;
                }
            }
            else if (colName == "ColCard")
            {
                var v = e.Value?.ToString();
                e.Value = string.IsNullOrWhiteSpace(v) || v == "0" ? "X" : "O";
                e.FormattingApplied = true;
            }
            else if (colName is "ColFP" or "ColPV" or "ColFace")
            {
                e.Value = e.Value?.ToString() == "0" || string.IsNullOrWhiteSpace(e.Value?.ToString()) ? "X" : "O";
                e.FormattingApplied = true;
            }
        };

        _dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEdit_Click(s, e); };

        Controls.Add(_dgv);
        Controls.Add(pnlTop);
        Controls.Add(infoLabel);
    }

    private async Task SyncAndLoadPeople()
    {
        try
        {
            SetStatus("단말기에 목록 요청 중...");

            // 단말기에 QueryPeople(PushAllPeople) 명령 전송
            var cmdResp = await _httpClient.PostAsync(
                $"/admin/devices/{_device.SN}/query-owned-people", null);

            if (!cmdResp.IsSuccessStatusCode)
            {
                // 명령 전송 실패 시 서버 캐시에서 로드
                SetStatus("단말기 명령 실패 - 캐시에서 로드");
                await LoadPeople();
                return;
            }

            // 단말기가 PushPeople(PushType=4)로 응답할 때까지 폴링 (최대 15초)
            SetStatus("단말기 응답 대기 중...");
            int lastCount = -1;
            int stable = 0;
            for (int i = 0; i < 45; i++)
            {
                await Task.Delay(1000);
                await LoadPeople();

                if (_people.Count == lastCount)
                    stable++;
                else
                    stable = 0;
                lastCount = _people.Count;

                // 1명 이상 수신된 뒤 2초 동안 건수가 같으면 완료
                if (_people.Count > 0 && i >= 2 && stable >= 2)
                {
                    SetStatus($"동기화 완료: 총 {_people.Count}명");
                    return;
                }
            }
            SetStatus($"동기화 완료: 총 {_people.Count}명");
        }
        catch (Exception ex)
        {
            SetStatus($"동기화 오류: {ex.Message}");
            await LoadPeople();
        }
    }

    private async Task LoadPeople()
    {
        try
        {
            SetStatus("불러오는 중...");
            var resp = await _httpClient.GetAsync($"/admin/devices/{_device.SN}/people");
            if (!resp.IsSuccessStatusCode)
            {
                SetStatus($"오류: HTTP {resp.StatusCode}");
                return;
            }
            _people = await resp.Content.ReadFromJsonAsync<List<PersonInfo>>()
                      ?? new List<PersonInfo>();
            PopulateGrid();
            SetStatus($"총 {_people.Count}명 (단말기 등록 사용자)");
        }
        catch (Exception ex)
        {
            SetStatus($"오류: {ex.Message}");
        }
    }

    private void PopulateGrid()
    {
        _dgv.Rows.Clear();
        foreach (var p in _people.OrderBy(x => x.UserID, StringComparer.OrdinalIgnoreCase))
        {
            int rowIdx = _dgv.Rows.Add(
                p.UserID ?? string.Empty,
                p.Name ?? string.Empty,
                p.Password ?? string.Empty,
                p.CardNum ?? string.Empty,
                (p.Fingerprints?.Count ?? 0).ToString(),
                (p.Palmveins?.Count    ?? 0).ToString(),
                string.IsNullOrWhiteSpace(p.FaceFeature) ? "0" : "1"
            );
            _dgv.Rows[rowIdx].Tag = p;
        }
    }

    private async void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var form = new PersonForm(_httpClient);
        if (form.ShowDialog(this) != DialogResult.OK) return;

        var person = form.Person;
        if (string.IsNullOrWhiteSpace(person.UserID))
        {
            MessageBox.Show("UserID가 비어 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            var resp = await _httpClient.PostAsJsonAsync($"/admin/devices/{_device.SN}/people", person);
            if (resp.IsSuccessStatusCode)
            {
                SetStatus($"사용자 {person.UserID} 추가 완료 → 단말기 전송 예약");
                await LoadPeople();
            }
            else
            {
                MessageBox.Show($"추가 실패: HTTP {resp.StatusCode}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (_dgv.SelectedRows.Count == 0) { MessageBox.Show("수정할 사용자를 선택하세요.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        var selectedPerson = _dgv.SelectedRows[0].Tag as PersonInfo;
        if (selectedPerson is null) return;

        using var form = new PersonForm(_httpClient);
        form.SetInitialValues(selectedPerson);

        if (form.ShowDialog(this) != DialogResult.OK) return;

        var person = form.Person;
        try
        {
            var resp = await _httpClient.PostAsJsonAsync($"/admin/devices/{_device.SN}/people", person);
            if (resp.IsSuccessStatusCode)
            {
                SetStatus($"사용자 {person.UserID} 수정 완료 → 단말기 전송 예약");
                await LoadPeople();
            }
            else
            {
                MessageBox.Show($"수정 실패: HTTP {resp.StatusCode}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_dgv.SelectedRows.Count == 0) { MessageBox.Show("삭제할 사용자를 선택하세요.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        var selectedPerson = _dgv.SelectedRows[0].Tag as PersonInfo;
        if (selectedPerson is null) return;

        var confirm = MessageBox.Show(
            $"사용자 [{selectedPerson.Name} ({selectedPerson.UserID})]를 이 단말기에서 삭제하시겠습니까?\n\n서버 사용자 목록에는 영향을 주지 않습니다.",
            "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        try
        {
            var resp = await _httpClient.DeleteAsync($"/admin/devices/{_device.SN}/people/{selectedPerson.UserID}");
            if (resp.IsSuccessStatusCode)
            {
                SetStatus($"사용자 {selectedPerson.UserID} 삭제 명령 전송 완료");
                await LoadPeople();
            }
            else
            {
                MessageBox.Show($"삭제 실패: HTTP {resp.StatusCode}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetStatus(string msg)
    {
        if (InvokeRequired) { Invoke(new Action<string>(SetStatus), msg); return; }
        _lblStatus.Text = msg;
    }
}
