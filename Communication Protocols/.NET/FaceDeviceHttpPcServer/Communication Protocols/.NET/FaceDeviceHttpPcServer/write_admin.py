import os

d = r"D:\Documents\Smart_LM_China\Communication Protocols\.NET\FaceDeviceHttpPcServer\wwwroot\admin"
os.makedirs(d, exist_ok=True)

# ── index.html ───────────────────────────────────────────────────
html = """\
<!DOCTYPE html>
<html lang="ko">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>FaceDevice PC Client</title>
  <style>
    *{box-sizing:border-box;margin:0;padding:0}
    body{font-family:"Segoe UI","맑은 고딕",Malgun Gothic,sans-serif;background:#f0f2f5;color:#333;font-size:13px}
    .header{background:linear-gradient(135deg,#2c3e50 0%,#3498db 100%);color:white;padding:11px 20px;display:flex;align-items:center;gap:12px}
    .header h1{font-size:19px;font-weight:600}
    .header .subtitle{font-size:11px;opacity:.85}
    .nav-tabs{background:#2c3e50;display:flex;padding:0 12px}
    .nav-tab{padding:11px 20px;cursor:pointer;border:none;background:none;font-size:13px;font-weight:500;color:#adb5bd;border-bottom:3px solid transparent;transition:all .2s}
    .nav-tab:hover{color:white;background:rgba(255,255,255,.05)}
    .nav-tab.active{color:white;border-bottom-color:#3498db;background:rgba(255,255,255,.08)}
    .container{max-width:1600px;margin:0 auto;padding:14px}
    .tab-content{display:none}
    .tab-content.active{display:block}
    .panel{background:white;border-radius:6px;box-shadow:0 1px 3px rgba(0,0,0,.08);margin-bottom:12px;overflow:hidden}
    .panel-header{padding:9px 14px;background:#f8f9fa;border-bottom:1px solid #e1e4e8;font-weight:600;font-size:14px;display:flex;justify-content:space-between;align-items:center;gap:8px;flex-wrap:wrap}
    .panel-body{padding:12px 14px}
    .btn-group{display:flex;gap:5px;flex-wrap:wrap;align-items:center}
    button{padding:6px 13px;border:none;border-radius:4px;font-size:12px;font-weight:500;cursor:pointer;transition:all .15s}
    .btn-primary{background:#3498db;color:white}.btn-primary:hover{background:#2980b9}
    .btn-secondary{background:#6c757d;color:white}.btn-secondary:hover{background:#5a6268}
    .btn-success{background:#27ae60;color:white}.btn-success:hover{background:#219a52}
    .btn-danger{background:#e74c3c;color:white}.btn-danger:hover{background:#c0392b}
    .btn-warning{background:#f39c12;color:white}.btn-warning:hover{background:#d68910}
    .btn-info{background:#17a2b8;color:white}.btn-info:hover{background:#138496}
    button:disabled{opacity:.55;cursor:not-allowed}
    .form-grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(210px,1fr));gap:10px;margin-bottom:12px}
    .form-group{display:flex;flex-direction:column;gap:3px}
    .form-group label{font-size:11px;font-weight:600;color:#555}
    .form-group input,.form-group select{padding:6px 9px;border:1px solid #d1d5db;border-radius:4px;font-size:13px;font-family:inherit}
    .form-group input:focus,.form-group select:focus{outline:none;border-color:#3498db;box-shadow:0 0 0 2px rgba(52,152,219,.12)}
    .search-bar{display:flex;gap:7px;margin-bottom:10px;flex-wrap:wrap;align-items:center}
    .search-bar input{flex:1;min-width:200px;padding:6px 9px;border:1px solid #d1d5db;border-radius:4px;font-size:13px}
    .table-wrap{overflow-x:auto}
    table{width:100%;border-collapse:collapse;font-size:12px}
    thead{background:#f8f9fa}
    th{padding:8px 7px;text-align:left;font-weight:600;color:#495057;border-bottom:2px solid #dee2e6;white-space:nowrap}
    td{padding:7px 7px;border-bottom:1px solid #f0f0f0;vertical-align:middle}
    tbody tr:hover{background:#f5f8ff}
    .empty-state{text-align:center;padding:28px;color:#adb5bd;font-size:13px}
    .badge{display:inline-block;padding:2px 7px;border-radius:9px;font-size:11px;font-weight:600}
    .badge-success{background:#d4edda;color:#155724}
    .badge-danger{background:#f8d7da;color:#721c24}
    .badge-warning{background:#fff3cd;color:#856404}
    .badge-info{background:#d1ecf1;color:#0c5460}
    .badge-secondary{background:#e2e3e5;color:#383d41}
    .stat-cards{display:grid;grid-template-columns:repeat(auto-fit,minmax(150px,1fr));gap:10px;margin-bottom:12px}
    .stat-card{background:white;border-radius:6px;padding:14px;text-align:center;box-shadow:0 1px 3px rgba(0,0,0,.08);border-top:3px solid #3498db}
    .stat-card .val{font-size:26px;font-weight:700;color:#2c3e50}
    .stat-card .lbl{font-size:11px;color:#888;margin-top:3px}
    .log-box{background:#1a1a2e;color:#a8d8ea;font-family:Consolas,monospace;font-size:11px;padding:9px;border-radius:4px;height:180px;overflow-y:auto;white-space:pre-wrap;word-break:break-all}
    .modal-overlay{display:none;position:fixed;inset:0;background:rgba(0,0,0,.45);z-index:2000;justify-content:center;align-items:flex-start;padding-top:60px}
    .modal-overlay.open{display:flex}
    .modal{background:white;border-radius:8px;padding:20px;min-width:460px;max-width:96vw;max-height:85vh;overflow-y:auto;box-shadow:0 8px 32px rgba(0,0,0,.2)}
    .modal h2{font-size:16px;margin-bottom:14px;border-bottom:1px solid #eee;padding-bottom:9px}
    .photo-placeholder{width:72px;height:72px;background:#f0f0f0;border-radius:4px;border:1px dashed #ccc;display:flex;align-items:center;justify-content:center;font-size:22px;color:#ccc;flex-shrink:0}
    .photo-preview{width:72px;height:72px;object-fit:cover;border-radius:4px;border:1px solid #ddd;display:block;flex-shrink:0}
    .status-bar{position:fixed;bottom:0;left:0;right:0;background:#2c3e50;color:#adb5bd;font-size:11px;padding:4px 14px;z-index:1000;transition:background .3s}
    .status-bar.ok{background:#1a6b30;color:white}
    .status-bar.err{background:#9b1c24;color:white}
    input[type=checkbox]{width:14px;height:14px;cursor:pointer;accent-color:#3498db}
  </style>
</head>
<body>
<div class="header">
  <div>
    <h1>FaceDevice PC Client</h1>
    <div class="subtitle">얼굴인식 단말기 관리 시스템</div>
  </div>
  <div style="margin-left:auto;font-size:11px;color:rgba(255,255,255,0.7);" id="headerTime"></div>
</div>
<div class="nav-tabs">
  <button class="nav-tab active" data-tab="dashboard">대시보드</button>
  <button class="nav-tab" data-tab="devices">단말기</button>
  <button class="nav-tab" data-tab="personnel">사용자</button>
  <button class="nav-tab" data-tab="attendance">출입기록</button>
</div>
<div class="container">
  <!-- 대시보드 -->
  <div id="dashboard-tab" class="tab-content active">
    <div class="stat-cards">
      <div class="stat-card"><div class="val" id="statDevices">-</div><div class="lbl">등록 단말기</div></div>
      <div class="stat-card" style="border-top-color:#27ae60"><div class="val" id="statOnline">-</div><div class="lbl">온라인</div></div>
      <div class="stat-card" style="border-top-color:#9b59b6"><div class="val" id="statPersonnel">-</div><div class="lbl">등록 사용자</div></div>
      <div class="stat-card" style="border-top-color:#e74c3c"><div class="val" id="statRecords">-</div><div class="lbl">출입 기록</div></div>
    </div>
    <div class="panel">
      <div class="panel-header">
        <span>시스템 정보</span>
        <button class="btn-secondary" onclick="refreshSystemInfo()" style="font-size:11px;padding:4px 9px;">새로고침</button>
      </div>
      <div class="panel-body">
        <table>
          <tr><th style="width:160px;">서버 URL</th><td id="infoServerUrl">-</td></tr>
          <tr><th>등록 단말기</th><td id="infoDevices">-</td></tr>
          <tr><th>등록 사용자</th><td id="infoPersonnel">-</td></tr>
          <tr><th>출입 기록</th><td id="infoRecords">-</td></tr>
        </table>
      </div>
    </div>
    <div class="panel">
      <div class="panel-header">
        <span>시스템 로그</span>
        <button class="btn-secondary" onclick="clearLog()" style="font-size:11px;padding:4px 9px;">지우기</button>
      </div>
      <div class="panel-body"><div class="log-box" id="logBox"></div></div>
    </div>
  </div>
  <!-- 단말기 -->
  <div id="devices-tab" class="tab-content">
    <div class="panel">
      <div class="panel-header">
        <span>단말기 검색</span>
        <div class="btn-group">
          <button class="btn-success" onclick="autoSearchDevices()">자동 검색</button>
          <button class="btn-primary" onclick="showNetworkScanDialog()">네트워크 스캔</button>
        </div>
      </div>
      <div class="panel-body">
        <div id="networkScanDialog" style="display:none;margin-bottom:12px;padding:10px;background:#f8f9fa;border-radius:4px;">
          <div class="form-group" style="max-width:300px"><label>서브넷 (예: 10.100.100)</label><input type="text" id="scanSubnet" value="10.100.100"></div>
          <div class="btn-group" style="margin-top:8px;">
            <button class="btn-success" onclick="startNetworkScan()">스캔 시작</button>
            <button class="btn-secondary" onclick="document.getElementById('networkScanDialog').style.display='none'">취소</button>
          </div>
        </div>
        <div id="discoveredContainer"></div>
      </div>
    </div>
    <div class="panel">
      <div class="panel-header">
        <span>등록된 단말기</span>
        <div class="btn-group">
          <button class="btn-secondary" onclick="refreshDevices()">새로고침</button>
          <button class="btn-primary" onclick="pullSelectedDevicePeople()">사용자 가져오기</button>
          <button class="btn-success" onclick="distributeToDevices()">단말기로 배포</button>
          <button class="btn-warning" onclick="syncTimeSelected()">시간 동기화</button>
          <button class="btn-danger" onclick="removeSelectedDevices()">제거</button>
        </div>
      </div>
      <div class="panel-body">
        <div class="table-wrap"><table>
          <thead><tr>
            <th style="width:38px"><input type="checkbox" id="chkAllDev" onclick="toggleAllDev(this)"></th>
            <th style="width:42px">순번</th>
            <th>단말기명</th><th>위치</th><th>시리얼넘버</th><th>IP</th><th>상태</th><th>제어</th>
          </tr></thead>
          <tbody id="devicesBody"><tr><td colspan="8" class="empty-state">로딩 중...</td></tr></tbody>
        </table></div>
      </div>
    </div>
  </div>
  <!-- 사용자 -->
  <div id="personnel-tab" class="tab-content">
    <div class="panel">
      <div class="panel-header">
        <span>사용자 목록</span>
        <div class="btn-group">
          <button class="btn-success" onclick="showAddPersonModal()">추가</button>
          <button class="btn-primary" onclick="editSelectedPerson()">수정</button>
          <button class="btn-danger" onclick="deleteSelectedPersonnel()">제거</button>
          <button class="btn-secondary" onclick="refreshPersonnel()">새로고침</button>
          <button class="btn-info" onclick="reloadFromFiles()">파일 불러오기</button>
          <button class="btn-warning" onclick="distributePersonnel()">단말기로 배포</button>
        </div>
      </div>
      <div class="panel-body">
        <div class="search-bar"><input type="text" id="searchPerson" placeholder="이름, ID, 카드번호 검색..." oninput="filterPersonnel()"></div>
        <div class="table-wrap"><table>
          <thead><tr>
            <th style="width:38px"><input type="checkbox" id="chkAllPerson" onclick="toggleAllPerson(this)"></th>
            <th>동</th><th>호</th><th>멤버</th><th>사용자명</th>
            <th style="width:48px">카드</th><th style="width:58px">비밀번호</th>
            <th style="width:48px">지문</th><th style="width:52px">손바닥</th><th style="width:48px">얼굴</th>
          </tr></thead>
          <tbody id="personnelBody"><tr><td colspan="10" class="empty-state">로딩 중...</td></tr></tbody>
        </table></div>
      </div>
    </div>
  </div>
  <!-- 출입기록 -->
  <div id="attendance-tab" class="tab-content">
    <div class="panel">
      <div class="panel-header">출입기록 검색</div>
      <div class="panel-body">
        <div class="form-grid">
          <div class="form-group"><label>사용자 ID</label><input type="text" id="attUID" placeholder="사용자 ID"></div>
          <div class="form-group"><label>이름</label><input type="text" id="attName" placeholder="이름"></div>
          <div class="form-group"><label>단말기</label><select id="attDevice"><option value="">전체 단말기</option></select></div>
          <div class="form-group"><label>시작일시</label><input type="datetime-local" id="attStart"></div>
          <div class="form-group"><label>종료일시</label><input type="datetime-local" id="attEnd"></div>
        </div>
        <div class="btn-group">
          <button class="btn-success" onclick="searchAttendance()">검색</button>
          <button class="btn-secondary" onclick="clearAttSearch()">초기화</button>
          <button class="btn-warning" onclick="exportAttendance()">내보내기</button>
        </div>
      </div>
    </div>
    <div class="panel">
      <div class="panel-header" id="attHeader">출입 기록</div>
      <div class="panel-body">
        <div class="table-wrap"><table>
          <thead><tr><th>시간</th><th>사용자 ID</th><th>이름</th><th>단말기 SN</th><th>기록 타입</th><th>체온</th><th>사진</th></tr></thead>
          <tbody id="attBody"><tr><td colspan="7" class="empty-state">검색 조건을 입력하세요.</td></tr></tbody>
        </table></div>
        <div id="attPager" class="btn-group" style="margin-top:8px;"></div>
      </div>
    </div>
  </div>
</div>
<!-- 사용자 모달 -->
<div class="modal-overlay" id="personModal">
  <div class="modal">
    <h2 id="personModalTitle">사용자 추가</h2>
    <div class="form-grid">
      <div class="form-group"><label>사용자 ID *</label><input type="text" id="mUid" maxlength="32"></div>
      <div class="form-group"><label>이름 *</label><input type="text" id="mName" maxlength="64"></div>
      <div class="form-group"><label>동 (Department)</label><input type="text" id="mDong" maxlength="64"></div>
      <div class="form-group"><label>호 (Job)</label><input type="text" id="mHo" maxlength="64"></div>
      <div class="form-group"><label>멤버 (IdentityCard)</label><input type="text" id="mMember" maxlength="64"></div>
      <div class="form-group"><label>카드번호</label><input type="text" id="mCard" maxlength="64"></div>
      <div class="form-group"><label>비밀번호</label><input type="password" id="mPass" maxlength="8"></div>
      <div class="form-group"><label>출입 타입</label>
        <select id="mAccess"><option value="0">일반 사용자</option><option value="1">관리자</option><option value="2">블랙리스트</option></select>
      </div>
      <div class="form-group"><label>만료 일시</label><input type="datetime-local" id="mExpiry"></div>
    </div>
    <div style="margin-bottom:12px;">
      <label style="font-size:11px;font-weight:600;color:#555;display:block;margin-bottom:6px;">얼굴 사진</label>
      <div style="display:flex;align-items:center;gap:12px;">
        <div id="mPhotoWrap" class="photo-placeholder">?</div>
        <div>
          <input type="file" id="mPhotoFile" accept="image/*" style="display:none" onchange="previewPhoto(event)">
          <div class="btn-group" style="flex-direction:column;gap:4px;align-items:flex-start;">
            <button class="btn-secondary" onclick="document.getElementById('mPhotoFile').click()">사진 선택</button>
            <button class="btn-danger" onclick="clearPhoto()">사진 제거</button>
          </div>
          <div id="mPhotoStatus" style="font-size:11px;color:#888;margin-top:5px;"></div>
        </div>
      </div>
    </div>
    <div class="btn-group" style="justify-content:flex-end;">
      <button class="btn-success" onclick="savePerson()">저장</button>
      <button class="btn-secondary" onclick="closePersonModal()">취소</button>
    </div>
  </div>
</div>
<!-- 단말기 설정 모달 -->
<div class="modal-overlay" id="devSettingsModal">
  <div class="modal" style="min-width:520px;">
    <h2 id="devSettingsTitle">단말기 설정</h2>
    <div class="form-grid" style="grid-template-columns:1fr 1fr;">
      <div class="form-group"><label>단말기명</label><input type="text" id="dName"></div>
      <div class="form-group"><label>위치</label><input type="text" id="dTag"></div>
    </div>
    <div class="btn-group" style="margin-bottom:14px;">
      <button class="btn-primary" onclick="saveDeviceInfo()">정보 저장</button>
    </div>
    <hr style="border:none;border-top:1px solid #eee;margin-bottom:12px;">
    <div class="btn-group" style="flex-wrap:wrap;">
      <button class="btn-secondary" onclick="devCmd('restart')">재시작</button>
      <button class="btn-primary" onclick="devCmd('opendoor')">문 열기</button>
      <button class="btn-info" onclick="openDeviceUserList()">사용자 정보</button>
      <button class="btn-danger" onclick="devCmd('deleteAllPeople')">사용자 전체삭제</button>
      <button class="btn-danger" onclick="devCmd('clearRecords')">로그 삭제</button>
      <button class="btn-secondary" onclick="devCmd('repostRecord')">로그 가져오기</button>
      <button class="btn-info" onclick="devCmd('synctime')">시간 동기화</button>
    </div>
    <div class="log-box" id="deviceCmdLog" style="height:90px;margin-top:10px;"></div>
    <div class="btn-group" style="justify-content:flex-end;margin-top:8px;">
      <button class="btn-secondary" onclick="closeDevSettings()">닫기</button>
    </div>
  </div>
</div>
<!-- 단말기 사용자 목록 모달 -->
<div class="modal-overlay" id="devUsersModal">
  <div class="modal" style="min-width:640px;">
    <h2 id="devUsersTitle">단말기 사용자 정보</h2>
    <div class="table-wrap" style="max-height:380px;overflow-y:auto;"><table>
      <thead><tr><th>UserID</th><th>이름</th><th>카드</th><th>얼굴</th><th>손바닥</th><th>만료일</th></tr></thead>
      <tbody id="devUsersBody"><tr><td colspan="6" class="empty-state">로딩 중...</td></tr></tbody>
    </table></div>
    <div class="btn-group" style="justify-content:flex-end;margin-top:10px;">
      <button class="btn-secondary" onclick="closeDevUsers()">닫기</button>
    </div>
  </div>
</div>
<!-- 배포 모달 -->
<div class="modal-overlay" id="distributeModal">
  <div class="modal" style="min-width:380px;">
    <h2>배포 대상 단말기 선택</h2>
    <div id="distributeDevList" style="margin-bottom:14px;max-height:300px;overflow-y:auto;"></div>
    <div class="btn-group" style="justify-content:flex-end;">
      <button class="btn-success" onclick="executeDistribute()">배포</button>
      <button class="btn-secondary" onclick="closeDistributeModal()">취소</button>
    </div>
  </div>
</div>
<div class="status-bar" id="statusBar">준비</div>
<script src="app.js"></script>
</body>
</html>
"""

with open(os.path.join(d, "index.html"), "w", encoding="utf-8") as f:
    f.write(html)
print("index.html:", os.path.getsize(os.path.join(d, "index.html")), "bytes")

# ── app.js ───────────────────────────────────────────────────────
js = r"""
let devices = [];
let personnel = [];
let filteredPersonnel = [];
let _devRowNumbers = {};
let _devRowCounter = 0;
let _currentDeviceSN = null;
let _editingUserId = null;
let _photoBase64 = null;
let _attPage = 1;
let _attTotal = 0;
const ATT_PAGE_SIZE = 50;

document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('.nav-tab').forEach(tab => {
    tab.addEventListener('click', () => switchTab(tab.dataset.tab));
  });
  setInterval(() => {
    document.getElementById('headerTime').textContent = new Date().toLocaleString('ko-KR');
  }, 1000);
  setInterval(refreshSystemInfo, 15000);
  refreshSystemInfo();
  setDefaultAttendanceDates();
});

function switchTab(name) {
  document.querySelectorAll('.nav-tab').forEach(t => t.classList.remove('active'));
  document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
  document.querySelector(`[data-tab="${name}"]`).classList.add('active');
  document.getElementById(`${name}-tab`).classList.add('active');
  if (name === 'dashboard') refreshSystemInfo();
  else if (name === 'devices') refreshDevices();
  else if (name === 'personnel') refreshPersonnel();
  else if (name === 'attendance') populateAttDeviceCombo();
}

function setStatus(msg, type = '') {
  const bar = document.getElementById('statusBar');
  bar.textContent = msg;
  bar.className = 'status-bar' + (type ? ' ' + type : '');
  if (type === 'ok') setTimeout(() => { bar.className = 'status-bar'; bar.textContent = '준비'; }, 4000);
}
function addLog(msg) {
  const box = document.getElementById('logBox');
  box.textContent += `[${new Date().toLocaleTimeString('ko-KR')}] ${msg}\n`;
  box.scrollTop = box.scrollHeight;
}
function clearLog() { document.getElementById('logBox').textContent = ''; }

async function api(method, url, body) {
  const opts = { method, headers: {} };
  if (body !== undefined && body !== null) {
    opts.headers['Content-Type'] = 'application/json';
    opts.body = JSON.stringify(body);
  }
  const r = await fetch(url, opts);
  if (!r.ok) throw new Error(`HTTP ${r.status}`);
  return r.json();
}
const apiGet = url => api('GET', url);
const apiPost = (url, b) => api('POST', url, b ?? {});
const apiDel = url => api('DELETE', url);

async function refreshSystemInfo() {
  try {
    const d = await apiGet('/admin/system-info');
    document.getElementById('statDevices').textContent = d.TotalDevices ?? '-';
    document.getElementById('statOnline').textContent = d.OnlineDevices ?? '-';
    document.getElementById('statPersonnel').textContent = d.TotalPeople ?? '-';
    document.getElementById('statRecords').textContent = d.TotalRecords ?? '-';
    document.getElementById('infoServerUrl').textContent = window.location.origin;
    document.getElementById('infoDevices').textContent = d.TotalDevices ?? '-';
    document.getElementById('infoPersonnel').textContent = d.TotalPeople ?? '-';
    document.getElementById('infoRecords').textContent = d.TotalRecords ?? '-';
  } catch (e) { /* silent */ }
}

async function refreshDevices() {
  try {
    setStatus('단말기 목록 로딩 중...');
    devices = await apiGet('/admin/devices') ?? [];
    devices.forEach(d => { if (!_devRowNumbers[d.SN]) _devRowNumbers[d.SN] = ++_devRowCounter; });
    devices.sort((a, b) => (_devRowNumbers[a.SN] || 99) - (_devRowNumbers[b.SN] || 99));
    renderDevices();
    setStatus(`단말기 ${devices.length}개 로드됨`, 'ok');
  } catch (e) { setStatus('단말기 로드 실패: ' + e.message, 'err'); addLog('ERROR: ' + e.message); }
}

function renderDevices() {
  const tb = document.getElementById('devicesBody');
  if (!devices.length) { tb.innerHTML = '<tr><td colspan="8" class="empty-state">등록된 단말기가 없습니다.</td></tr>'; return; }
  const cutoff = new Date(Date.now() - 5 * 60 * 1000);
  tb.innerHTML = devices.map(d => {
    const no = _devRowNumbers[d.SN] ?? '-';
    const online = d.LastKeepaliveAtUtc && new Date(d.LastKeepaliveAtUtc) >= cutoff;
    const badge = online ? '<span class="badge badge-success">온라인</span>' : '<span class="badge badge-danger">오프라인</span>';
    return `<tr>
      <td><input type="checkbox" class="chkDev" data-sn="${esc(d.SN)}"></td>
      <td style="text-align:center;">${no}</td>
      <td>${esc(d.DeviceName ?? '')}</td>
      <td>${esc(d.TagName ?? '')}</td>
      <td style="font-size:11px;">${esc(d.SN)}</td>
      <td>${esc(d.IpAddress ?? '')}</td>
      <td>${badge}</td>
      <td><button onclick="openDevSettings('${esc(d.SN)}')" style="font-size:11px;padding:3px 8px;" class="btn-primary">설정</button></td>
    </tr>`;
  }).join('');
}

function toggleAllDev(chk) { document.querySelectorAll('.chkDev').forEach(c => c.checked = chk.checked); }
function getCheckedDevSNs() { return [...document.querySelectorAll('.chkDev:checked')].map(c => c.dataset.sn); }

async function autoSearchDevices() {
  try {
    setStatus('브로드캐스트 검색 중...');
    addLog('단말기 자동 검색 시작...');
    const res = await apiPost('/api/Device/Search', { SearchType: 'broadcast' });
    const found = res.content ?? res.Data ?? [];
    renderDiscovered(found);
    setStatus(`${found.length}개 발견됨`, 'ok');
    addLog(`자동 검색 완료: ${found.length}개`);
  } catch (e) { setStatus('검색 실패: ' + e.message, 'err'); addLog('검색 실패: ' + e.message); }
}

function showNetworkScanDialog() { document.getElementById('networkScanDialog').style.display = 'block'; }

async function startNetworkScan() {
  const subnet = document.getElementById('scanSubnet').value.trim();
  if (!subnet) { setStatus('서브넷을 입력하세요', 'err'); return; }
  document.getElementById('networkScanDialog').style.display = 'none';
  setStatus(`${subnet}.x 스캔 중...`);
  addLog(`네트워크 스캔 시작: ${subnet}.1-254`);
  let found = [];
  renderDiscovered(found);
  try {
    const response = await fetch('/api/Device/SearchStream', {
      method: 'POST', headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ SearchType: 'scan', Subnet: subnet })
    });
    const reader = response.body.getReader();
    const dec = new TextDecoder();
    let buf = '';
    while (true) {
      const { done, value } = await reader.read();
      if (done) break;
      buf += dec.decode(value, { stream: true });
      const lines = buf.split('\n\n'); buf = lines.pop() ?? '';
      for (const line of lines) {
        if (!line.startsWith('data: ')) continue;
        const s = line.slice(6).trim();
        if (s === '[DONE]') { setStatus(`스캔 완료: ${found.length}개`, 'ok'); addLog(`스캔 완료: ${found.length}개`); continue; }
        try { found.push(JSON.parse(s)); renderDiscovered(found); } catch {}
      }
    }
  } catch (e) { setStatus('스캔 실패: ' + e.message, 'err'); }
}

function renderDiscovered(list) {
  const c = document.getElementById('discoveredContainer');
  if (!list.length) { c.innerHTML = '<div class="empty-state">검색된 단말기가 없습니다.</div>'; return; }
  c.innerHTML = `<h4 style="margin-bottom:8px;font-size:13px;">발견된 단말기 (${list.length}개)</h4>
  <div class="table-wrap"><table>
    <thead><tr><th>IP</th><th>SN</th><th>단말기명</th><th>모델</th><th>펌웨어</th><th>포트</th><th></th></tr></thead>
    <tbody>${list.map(d => `<tr>
      <td><b>${d.IpAddress}</b></td><td>${d.DeviceSN ?? ''}</td><td>${d.DeviceName ?? ''}</td>
      <td>${d.Model ?? ''}</td><td>${d.FirmwareVersion ?? ''}</td><td>${d.HttpPort ?? 80}</td>
      <td><button class="btn-success" style="font-size:11px;padding:3px 8px;" onclick="connectDevice('${d.IpAddress}',${d.HttpPort ?? 80})">연결</button></td>
    </tr>`).join('')}</tbody>
  </table></div>`;
}

async function connectDevice(ip, port) {
  try {
    setStatus(`${ip}:${port} 연결 중...`);
    const probe = await apiPost('/api/Device/ProbeDevice', { IpAddress: ip, HttpPort: port });
    if (probe.Code !== 0) throw new Error(probe.Msg || '탐색 실패');
    const di = probe.Data;
    const reg = await apiPost('/api/Device/Connect', {
      DeviceSN: di.DeviceSN, IpAddress: ip, HttpPort: port,
      DeviceName: di.DeviceName ?? 'Face Device', Model: di.Model, FirmwareVersion: di.FirmwareVersion
    });
    if (reg.Code !== 0) throw new Error(reg.Msg || '연결 실패');
    setStatus(`${di.DeviceSN} 연결 완료`, 'ok');
    addLog(`단말기 연결: ${di.DeviceSN} (${ip})`);
    await refreshDevices();
  } catch (e) { setStatus('연결 실패: ' + e.message, 'err'); addLog('연결 실패: ' + e.message); }
}

async function pullSelectedDevicePeople() {
  const sns = getCheckedDevSNs();
  if (!sns.length) { setStatus('단말기를 선택하세요', 'err'); return; }
  for (const sn of sns) {
    try { await apiPost(`/admin/devices/${sn}/pull-all-people`, {}); addLog(`[${sn}] 사용자 가져오기 명령 예약`); }
    catch (e) { addLog(`[${sn}] 실패: ${e.message}`); }
  }
  setStatus('사용자 가져오기 명령 예약 완료', 'ok');
}

async function distributeToDevices() {
  const uids = getCheckedPersonUIDs();
  if (!uids.length) { setStatus('배포할 사용자를 사용자 탭에서 선택하세요', 'err'); return; }
  if (!devices.length) await refreshDevices();
  showDistributeModal(uids);
}

function showDistributeModal(uids) {
  if (!devices.length) { setStatus('단말기 목록을 먼저 로드하세요', 'err'); return; }
  const list = document.getElementById('distributeDevList');
  list.innerHTML = devices.map(d => `
    <label style="display:flex;align-items:center;gap:8px;padding:6px 0;border-bottom:1px solid #f0f0f0;">
      <input type="checkbox" class="chkDistDev" value="${esc(d.SN)}" checked>
      <span><b>${esc(d.DeviceName ?? d.SN)}</b> <span style="color:#999;font-size:11px;">(${esc(d.SN)})</span></span>
    </label>`).join('');
  list.dataset.uids = JSON.stringify(uids);
  document.getElementById('distributeModal').classList.add('open');
}

async function executeDistribute() {
  const sns = [...document.querySelectorAll('.chkDistDev:checked')].map(c => c.value);
  const uids = JSON.parse(document.getElementById('distributeDevList').dataset.uids || '[]');
  if (!sns.length) { setStatus('대상 단말기를 선택하세요', 'err'); return; }
  closeDistributeModal();
  try {
    await apiPost('/admin/people/distribute-to-devices', { PersonIds: uids, TargetSNs: sns });
    setStatus('배포 완료', 'ok');
    addLog(`배포: ${uids.length}명 -> ${sns.length}개 단말기`);
  } catch (e) { setStatus('배포 실패: ' + e.message, 'err'); }
}
function closeDistributeModal() { document.getElementById('distributeModal').classList.remove('open'); }

async function syncTimeSelected() {
  const sns = getCheckedDevSNs();
  if (!sns.length) { setStatus('단말기를 선택하세요', 'err'); return; }
  for (const sn of sns) {
    try { await apiPost(`/admin/devices/${sn}/remote-command`, { SN: sn, CommandType: 'synctime' }); addLog(`[${sn}] 시간 동기화 명령 예약`); }
    catch (e) { addLog(`[${sn}] 실패: ${e.message}`); }
  }
  setStatus('시간 동기화 명령 예약 완료', 'ok');
}

async function removeSelectedDevices() {
  const sns = getCheckedDevSNs();
  if (!sns.length) { setStatus('제거할 단말기를 선택하세요', 'err'); return; }
  if (!confirm(`선택한 ${sns.length}개 단말기를 제거하시겠습니까?`)) return;
  for (const sn of sns) {
    try { await apiDel(`/admin/devices/${sn}`); addLog(`[${sn}] 단말기 제거`); }
    catch (e) { addLog(`[${sn}] 제거 실패: ${e.message}`); }
  }
  await refreshDevices();
  setStatus('제거 완료', 'ok');
}

function openDevSettings(sn) {
  _currentDeviceSN = sn;
  const d = devices.find(x => x.SN === sn) ?? { SN: sn };
  document.getElementById('devSettingsTitle').textContent = `단말기 설정 - ${d.DeviceName ?? sn}`;
  document.getElementById('dName').value = d.DeviceName ?? '';
  document.getElementById('dTag').value = d.TagName ?? '';
  document.getElementById('deviceCmdLog').textContent = '';
  document.getElementById('devSettingsModal').classList.add('open');
}
function closeDevSettings() { document.getElementById('devSettingsModal').classList.remove('open'); }

async function saveDeviceInfo() {
  const sn = _currentDeviceSN;
  try {
    await apiPost(`/admin/devices/${sn}/update-info`, { SN: sn, DeviceName: document.getElementById('dName').value, TagName: document.getElementById('dTag').value });
    appendDevLog('저장 완료');
    await refreshDevices();
  } catch (e) { appendDevLog('저장 실패: ' + e.message); }
}

async function devCmd(cmd) {
  const sn = _currentDeviceSN;
  try {
    appendDevLog(`${cmd} 명령 전송 중...`);
    await apiPost(`/admin/devices/${sn}/remote-command`, { SN: sn, CommandType: cmd });
    appendDevLog(`${cmd} 명령 예약 완료`);
    addLog(`[${sn}] ${cmd}`);
  } catch (e) { appendDevLog(`실패: ${e.message}`); }
}
function appendDevLog(msg) {
  const box = document.getElementById('deviceCmdLog');
  box.textContent += `[${new Date().toLocaleTimeString('ko-KR')}] ${msg}\n`;
  box.scrollTop = box.scrollHeight;
}

async function openDeviceUserList() {
  const sn = _currentDeviceSN;
  document.getElementById('devUsersTitle').textContent = `단말기 사용자 정보 - ${sn}`;
  document.getElementById('devUsersModal').classList.add('open');
  document.getElementById('devUsersBody').innerHTML = '<tr><td colspan="6" class="empty-state">로딩 중...</td></tr>';
  try {
    const list = await apiGet(`/admin/devices/${sn}/people`);
    const arr = Array.isArray(list) ? list : (list.Data ?? []);
    if (!arr.length) { document.getElementById('devUsersBody').innerHTML = '<tr><td colspan="6" class="empty-state">사용자 없음</td></tr>'; return; }
    document.getElementById('devUsersBody').innerHTML = arr.map(p => {
      const exp = p.ExpirationDate > 0 ? new Date(p.ExpirationDate * 1000).toLocaleDateString('ko-KR') : '무제한';
      return `<tr><td>${esc(p.UserID)}</td><td>${esc(p.Name)}</td>
        <td>${p.CardNum && p.CardNum !== '0' ? 'O' : '-'}</td>
        <td>${p.Photo ? 'O' : '-'}</td>
        <td>${(p.Palmveins?.length > 0) ? 'O' : '-'}</td>
        <td>${exp}</td></tr>`;
    }).join('');
  } catch (e) { document.getElementById('devUsersBody').innerHTML = `<tr><td colspan="6" class="empty-state">로드 실패: ${e.message}</td></tr>`; }
}
function closeDevUsers() { document.getElementById('devUsersModal').classList.remove('open'); }

async function refreshPersonnel() {
  try {
    setStatus('사용자 목록 로딩 중...');
    personnel = await apiGet('/admin/people') ?? [];
    filteredPersonnel = [...personnel];
    renderPersonnel();
    setStatus(`사용자 ${personnel.length}명 로드됨`, 'ok');
  } catch (e) { setStatus('사용자 로드 실패: ' + e.message, 'err'); }
}

function filterPersonnel() {
  const q = document.getElementById('searchPerson').value.trim().toLowerCase();
  filteredPersonnel = q
    ? personnel.filter(p => (p.Name || '').toLowerCase().includes(q) || (p.UserID || '').toLowerCase().includes(q) || (p.CardNum || '').toLowerCase().includes(q))
    : [...personnel];
  renderPersonnel();
}

function renderPersonnel() {
  const tb = document.getElementById('personnelBody');
  if (!filteredPersonnel.length) { tb.innerHTML = '<tr><td colspan="10" class="empty-state">사용자가 없습니다.</td></tr>'; return; }
  tb.innerHTML = filteredPersonnel.map(p => {
    const card = (p.CardNum && p.CardNum !== '0') ? 'O' : '-';
    const pass = p.Password ? '****' : '-';
    const fp = (p.Fingerprints?.length > 0) ? 'O' : '-';
    const palm = (p.Palmveins?.length > 0) ? 'O' : '-';
    const face = p.Photo ? 'O' : '-';
    return `<tr data-uid="${esc(p.UserID)}">
      <td><input type="checkbox" class="chkPerson" data-uid="${esc(p.UserID)}"></td>
      <td>${esc(p.Department ?? '')}</td><td>${esc(p.Job ?? '')}</td><td>${esc(p.IdentityCard ?? '')}</td>
      <td><b>${esc(p.Name)}</b> <span style="color:#999;font-size:11px;">(${esc(p.UserID)})</span></td>
      <td style="text-align:center;">${card}</td><td style="text-align:center;">${pass}</td>
      <td style="text-align:center;">${fp}</td><td style="text-align:center;">${palm}</td>
      <td style="text-align:center;">${face}</td>
    </tr>`;
  }).join('');
  document.querySelectorAll('#personnelBody tr[data-uid]').forEach(row => {
    row.addEventListener('dblclick', () => openEditPerson(row.dataset.uid));
  });
}

function toggleAllPerson(chk) { document.querySelectorAll('.chkPerson').forEach(c => c.checked = chk.checked); }
function getCheckedPersonUIDs() { return [...document.querySelectorAll('.chkPerson:checked')].map(c => c.dataset.uid); }

async function reloadFromFiles() {
  try {
    const res = await apiPost('/admin/people/reload-from-files', {});
    setStatus('파일에서 불러오기 완료', 'ok');
    addLog('파일 불러오기: ' + (res.Message ?? JSON.stringify(res)));
    await refreshPersonnel();
  } catch (e) { setStatus('불러오기 실패: ' + e.message, 'err'); }
}

async function distributePersonnel() {
  const uids = getCheckedPersonUIDs();
  if (!uids.length) { setStatus('배포할 사용자를 선택하세요', 'err'); return; }
  if (!devices.length) await refreshDevices();
  showDistributeModal(uids);
}

function showAddPersonModal() {
  _editingUserId = null; _photoBase64 = null;
  clearPersonModal();
  document.getElementById('personModalTitle').textContent = '사용자 추가';
  document.getElementById('mUid').disabled = false;
  document.getElementById('personModal').classList.add('open');
}

async function openEditPerson(uid) {
  if (!uid) {
    const uids = getCheckedPersonUIDs();
    if (uids.length !== 1) { setStatus('수정할 사용자를 1명 선택하세요', 'err'); return; }
    uid = uids[0];
  }
  _editingUserId = uid; _photoBase64 = null;
  clearPersonModal();
  document.getElementById('personModalTitle').textContent = '사용자 수정';
  document.getElementById('mUid').disabled = true;
  try {
    const res = await apiPost('/api/People/GetDetail', { UserID: uid });
    const p = res.Data ?? res;
    document.getElementById('mUid').value = p.UserID ?? '';
    document.getElementById('mName').value = p.Name ?? '';
    document.getElementById('mDong').value = p.Department ?? '';
    document.getElementById('mHo').value = p.Job ?? '';
    document.getElementById('mMember').value = p.IdentityCard ?? '';
    document.getElementById('mCard').value = (p.CardNum && p.CardNum !== '0') ? p.CardNum : '';
    document.getElementById('mPass').value = p.Password ?? '';
    document.getElementById('mAccess').value = p.AccessType ?? 0;
    if (p.ExpirationDate > 0) {
      document.getElementById('mExpiry').value = new Date(p.ExpirationDate * 1000).toISOString().slice(0, 16);
    }
    if (p.Photo) {
      _photoBase64 = p.Photo;
      const wrap = document.getElementById('mPhotoWrap');
      const img = document.createElement('img');
      img.id = 'mPhotoWrap'; img.className = 'photo-preview';
      img.src = 'data:image/jpeg;base64,' + p.Photo;
      wrap.replaceWith(img);
      document.getElementById('mPhotoStatus').textContent = '기존 사진 있음';
    }
  } catch (e) { setStatus('사용자 정보 로드 실패: ' + e.message, 'err'); return; }
  document.getElementById('personModal').classList.add('open');
}

function editSelectedPerson() { openEditPerson(null); }

function clearPersonModal() {
  ['mUid','mName','mDong','mHo','mMember','mCard','mPass','mExpiry'].forEach(id => { document.getElementById(id).value = ''; });
  document.getElementById('mAccess').value = '0';
  document.getElementById('mPhotoStatus').textContent = '';
  document.getElementById('mPhotoFile').value = '';
  _photoBase64 = null;
  const wrap = document.getElementById('mPhotoWrap');
  if (wrap) {
    const ph = document.createElement('div');
    ph.id = 'mPhotoWrap'; ph.className = 'photo-placeholder'; ph.textContent = '?';
    wrap.replaceWith(ph);
  }
}
function closePersonModal() { document.getElementById('personModal').classList.remove('open'); }

function previewPhoto(event) {
  const file = event.target.files[0];
  if (!file) return;
  const reader = new FileReader();
  reader.onload = e => {
    const data = e.target.result;
    _photoBase64 = data.split(',')[1];
    const wrap = document.getElementById('mPhotoWrap');
    const img = document.createElement('img');
    img.id = 'mPhotoWrap'; img.className = 'photo-preview'; img.src = data;
    wrap.replaceWith(img);
    document.getElementById('mPhotoStatus').textContent = `${file.name} (${(file.size / 1024).toFixed(1)}KB)`;
  };
  reader.readAsDataURL(file);
}

function clearPhoto() {
  _photoBase64 = null;
  document.getElementById('mPhotoFile').value = '';
  document.getElementById('mPhotoStatus').textContent = '';
  const wrap = document.getElementById('mPhotoWrap');
  if (wrap) {
    const ph = document.createElement('div');
    ph.id = 'mPhotoWrap'; ph.className = 'photo-placeholder'; ph.textContent = '?';
    wrap.replaceWith(ph);
  }
}

async function savePerson() {
  const uid = document.getElementById('mUid').value.trim();
  const name = document.getElementById('mName').value.trim();
  if (!uid || !name) { setStatus('사용자 ID와 이름은 필수입니다', 'err'); return; }
  let expiry = 0;
  const expiryVal = document.getElementById('mExpiry').value;
  if (expiryVal) expiry = Math.floor(new Date(expiryVal).getTime() / 1000);
  const person = {
    UserID: uid, Name: name,
    Department: document.getElementById('mDong').value.trim(),
    Job: document.getElementById('mHo').value.trim(),
    IdentityCard: document.getElementById('mMember').value.trim(),
    CardNum: document.getElementById('mCard').value.trim() || '0',
    Password: document.getElementById('mPass').value.trim(),
    AccessType: parseInt(document.getElementById('mAccess').value) || 0,
    ExpirationDate: expiry, OpenTimes: 65535, Timegroup: 1,
    Photo: _photoBase64 ?? ''
  };
  try {
    if (_editingUserId) {
      const res = await apiPost('/api/People/Update', person);
      if (res.Code !== 0 && res.Success !== 1) throw new Error(res.Msg ?? '수정 실패');
      setStatus('사용자 수정 완료', 'ok'); addLog(`사용자 수정: ${uid}`);
    } else {
      const res = await apiPost('/api/People/New', person);
      if (res.Code !== 0 && res.Success !== 1) throw new Error(res.Msg ?? '추가 실패');
      setStatus('사용자 추가 완료', 'ok'); addLog(`사용자 추가: ${uid}`);
    }
    closePersonModal();
    await refreshPersonnel();
  } catch (e) { setStatus('저장 실패: ' + e.message, 'err'); addLog('저장 실패: ' + e.message); }
}

async function deleteSelectedPersonnel() {
  const uids = getCheckedPersonUIDs();
  if (!uids.length) { setStatus('삭제할 사용자를 선택하세요', 'err'); return; }
  const names = uids.map(uid => personnel.find(x => x.UserID === uid)?.Name ?? uid);
  if (!confirm(`${names.join(', ')} (${uids.length}명)을 삭제하시겠습니까?`)) return;
  let ok = 0, fail = 0;
  for (const uid of uids) {
    try {
      const res = await apiPost('/api/People/Delete', { UserID: uid });
      if (res.Code === 0 || res.Success === 1) ok++; else fail++;
    } catch { fail++; }
  }
  setStatus(`삭제: ${ok}명 성공${fail > 0 ? `, ${fail}명 실패` : ''}`, ok > 0 ? 'ok' : 'err');
  addLog(`삭제: 성공=${ok}, 실패=${fail}`);
  await refreshPersonnel();
}

function setDefaultAttendanceDates() {
  const now = new Date();
  const start = new Date(now); start.setHours(0, 0, 0, 0);
  const pad = n => String(n).padStart(2, '0');
  const fmt = d => `${d.getFullYear()}-${pad(d.getMonth()+1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
  document.getElementById('attEnd').value = fmt(now);
  document.getElementById('attStart').value = fmt(start);
}

async function populateAttDeviceCombo() {
  const sel = document.getElementById('attDevice');
  const prev = sel.value;
  sel.innerHTML = '<option value="">전체 단말기</option>';
  try {
    const devs = await apiGet('/admin/devices') ?? [];
    devs.forEach(d => {
      const opt = document.createElement('option');
      opt.value = d.SN; opt.textContent = `${d.DeviceName ?? d.SN} (${d.SN})`;
      sel.appendChild(opt);
    });
    if (prev) sel.value = prev;
  } catch {}
}

async function searchAttendance(page = 1) {
  _attPage = page;
  const req = {
    PageIndex: page, PageSize: ATT_PAGE_SIZE,
    UserID: document.getElementById('attUID').value.trim() || undefined,
    UserName: document.getElementById('attName').value.trim() || undefined,
    DeviceSN: document.getElementById('attDevice').value || undefined,
    StartTime: document.getElementById('attStart').value ? new Date(document.getElementById('attStart').value).toISOString() : undefined,
    EndTime: document.getElementById('attEnd').value ? new Date(document.getElementById('attEnd').value).toISOString() : undefined,
  };
  Object.keys(req).forEach(k => req[k] === undefined && delete req[k]);
  try {
    setStatus('출입기록 검색 중...');
    const res = await apiPost('/api/Attendance/Search', req);
    const records = res.Data?.Records ?? res.Records ?? [];
    _attTotal = res.Data?.TotalCount ?? res.TotalCount ?? records.length;
    document.getElementById('attHeader').textContent = `출입 기록 (총 ${_attTotal}건)`;
    renderAttendance(records);
    renderAttPager();
    setStatus(`${_attTotal}건 검색됨`, 'ok');
  } catch (e) { setStatus('검색 실패: ' + e.message, 'err'); }
}

function renderAttendance(records) {
  const tb = document.getElementById('attBody');
  if (!records.length) { tb.innerHTML = '<tr><td colspan="7" class="empty-state">기록 없음</td></tr>'; return; }
  tb.innerHTML = records.map(r => {
    const photoCell = r.PhotoUrl ? `<img src="${r.PhotoUrl}" style="height:36px;border-radius:3px;" onerror="this.style.display='none'">` : '-';
    const typeLabel = r.RecordType === 1 ? '<span class="badge badge-success">인식</span>'
                    : r.RecordType === 2 ? '<span class="badge badge-info">도어센서</span>'
                    : '<span class="badge badge-secondary">시스템</span>';
    return `<tr>
      <td>${esc(r.RecordTime ?? '')}</td><td>${esc(r.UserID ?? '')}</td><td>${esc(r.UserName ?? '')}</td>
      <td style="font-size:11px;">${esc(r.DeviceSN ?? '')}</td><td>${typeLabel}</td>
      <td>${r.Temperature ? r.Temperature + '℃' : '-'}</td><td>${photoCell}</td>
    </tr>`;
  }).join('');
}

function renderAttPager() {
  const totalPages = Math.ceil(_attTotal / ATT_PAGE_SIZE);
  const pager = document.getElementById('attPager');
  if (totalPages <= 1) { pager.innerHTML = ''; return; }
  let html = '';
  if (_attPage > 1) html += `<button class="btn-secondary" onclick="searchAttendance(${_attPage - 1})">이전</button>`;
  html += `<span style="font-size:12px;color:#666;">${_attPage} / ${totalPages} 페이지</span>`;
  if (_attPage < totalPages) html += `<button class="btn-secondary" onclick="searchAttendance(${_attPage + 1})">다음</button>`;
  pager.innerHTML = html;
}

function clearAttSearch() {
  document.getElementById('attUID').value = '';
  document.getElementById('attName').value = '';
  document.getElementById('attDevice').value = '';
  setDefaultAttendanceDates();
  document.getElementById('attBody').innerHTML = '<tr><td colspan="7" class="empty-state">검색 조건을 입력하세요.</td></tr>';
  document.getElementById('attHeader').textContent = '출입 기록';
  document.getElementById('attPager').innerHTML = '';
}

async function exportAttendance() {
  try {
    const req = {
      PageIndex: 1, PageSize: 9999,
      UserID: document.getElementById('attUID').value.trim() || undefined,
      UserName: document.getElementById('attName').value.trim() || undefined,
      DeviceSN: document.getElementById('attDevice').value || undefined,
      StartTime: document.getElementById('attStart').value ? new Date(document.getElementById('attStart').value).toISOString() : undefined,
      EndTime: document.getElementById('attEnd').value ? new Date(document.getElementById('attEnd').value).toISOString() : undefined,
    };
    Object.keys(req).forEach(k => req[k] === undefined && delete req[k]);
    const res = await apiPost('/api/Attendance/Search', req);
    const records = res.Data?.Records ?? res.Records ?? [];
    const header = ['시간', '사용자ID', '이름', '단말기SN', '기록타입', '체온'];
    const rows = records.map(r => [r.RecordTime, r.UserID, r.UserName, r.DeviceSN, r.RecordType, r.Temperature ?? ''].join('\t'));
    const text = [header.join('\t'), ...rows].join('\n');
    const blob = new Blob(['\uFEFF' + text], { type: 'text/tab-separated-values;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url; a.download = `attendance_${new Date().toISOString().slice(0, 10)}.tsv`;
    a.click(); URL.revokeObjectURL(url);
    setStatus(`${records.length}건 내보내기 완료`, 'ok');
  } catch (e) { setStatus('내보내기 실패: ' + e.message, 'err'); }
}

function esc(s) {
  return String(s ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}
"""

with open(os.path.join(d, "app.js"), "w", encoding="utf-8") as f:
    f.write(js)
print("app.js:", os.path.getsize(os.path.join(d, "app.js")), "bytes")
print("Done.")
