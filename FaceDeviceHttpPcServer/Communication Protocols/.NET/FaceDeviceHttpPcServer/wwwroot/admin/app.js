let devices = [];
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

// ─── 탭 전환 ────────────────────────────────────────────────────
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
  else if (name === 'devices')    refreshDevices();
  else if (name === 'personnel')  refreshPersonnel();
  else if (name === 'attendance') populateAttDeviceCombo();
}

// ─── 상태바 / 로그 ───────────────────────────────────────────────
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

// ─── API 헬퍼 ────────────────────────────────────────────────────
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
const apiGet  = url       => api('GET',    url);
const apiPost = (url, b)  => api('POST',   url, b ?? {});
const apiDel  = url       => api('DELETE', url);

// ─── 대시보드 ────────────────────────────────────────────────────
async function refreshSystemInfo() {
  try {
    const d = await apiGet('/admin/system-info');
    document.getElementById('statDevices').textContent   = d.TotalDevices   ?? '-';
    document.getElementById('statOnline').textContent    = d.OnlineDevices  ?? '-';
    document.getElementById('statPersonnel').textContent = d.TotalPeople    ?? '-';
    document.getElementById('statRecords').textContent   = d.TotalRecords   ?? '-';
    document.getElementById('infoServerUrl').textContent  = window.location.origin;
    document.getElementById('infoDevices').textContent    = d.TotalDevices  ?? '-';
    document.getElementById('infoPersonnel').textContent  = d.TotalPeople   ?? '-';
    document.getElementById('infoRecords').textContent    = d.TotalRecords  ?? '-';
  } catch(e) { /* silent */ }
}

// ─── 단말기 탭 ──────────────────────────────────────────────────
async function refreshDevices() {
  try {
    setStatus('단말기 목록 로딩 중...');
    devices = await apiGet('/admin/devices') ?? [];
    devices.forEach(d => { if (!_devRowNumbers[d.SN]) _devRowNumbers[d.SN] = ++_devRowCounter; });
    devices.sort((a, b) => (_devRowNumbers[a.SN] || 99) - (_devRowNumbers[b.SN] || 99));
    renderDevices();
    setStatus(`단말기 ${devices.length}개 로드됨`, 'ok');
  } catch(e) { setStatus('단말기 로드 실패: ' + e.message, 'err'); addLog('ERROR: ' + e.message); }
}

function renderDevices() {
  const tb = document.getElementById('devicesBody');
  if (!devices.length) { tb.innerHTML = '<tr><td colspan="8" class="empty-state">등록된 단말기가 없습니다.</td></tr>'; return; }
  tb.innerHTML = devices.map(d => {
    const no = _devRowNumbers[d.SN] ?? '-';
    const cutoff = new Date(Date.now() - 5 * 60 * 1000);
    const online = d.LastKeepaliveAtUtc && new Date(d.LastKeepaliveAtUtc) >= cutoff;
    const badge = online
      ? '<span class="badge badge-success">온라인</span>'
      : '<span class="badge badge-danger">오프라인</span>';
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

function toggleAllDev(chk) {
  document.querySelectorAll('.chkDev').forEach(c => c.checked = chk.checked);
}

function getCheckedDevSNs() {
  return [...document.querySelectorAll('.chkDev:checked')].map(c => c.dataset.sn);
}

// 자동 검색
async function autoSearchDevices() {
  try {
    setStatus('브로드캐스트 검색 중...');
    addLog('단말기 자동 검색 시작...');
    const res = await apiPost('/api/Device/Search', { SearchType: 'broadcast' });
    const found = res.content ?? res.Data ?? [];
    renderDiscovered(found);
    setStatus(`${found.length}개 발견됨`, 'ok');
    addLog(`자동 검색 완료: ${found.length}개`);
  } catch(e) { setStatus('검색 실패: ' + e.message, 'err'); addLog('검색 실패: ' + e.message); }
}

function showNetworkScanDialog() {
  document.getElementById('networkScanDialog').style.display = 'block';
}

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
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ SearchType: 'scan', Subnet: subnet })
    });
    const reader = response.body.getReader();
    const dec = new TextDecoder();
    let buf = '';
    while (true) {
      const { done, value } = await reader.read();
      if (done) break;
      buf += dec.decode(value, { stream: true });
      const lines = buf.split('\n\n');
      buf = lines.pop() ?? '';
      for (const line of lines) {
        if (!line.startsWith('data: ')) continue;
        const s = line.slice(6).trim();
        if (s === '[DONE]') { setStatus(`스캔 완료: ${found.length}개 발견`, 'ok'); addLog(`스캔 완료: ${found.length}개`); continue; }
        try { found.push(JSON.parse(s)); renderDiscovered(found); } catch {}
      }
    }
  } catch(e) { setStatus('스캔 실패: ' + e.message, 'err'); }
}

function renderDiscovered(list) {
  const c = document.getElementById('discoveredContainer');
  if (!list.length) { c.innerHTML = '<div class="empty-state">검색된 단말기가 없습니다. 자동 검색 또는 네트워크 스캔을 사용하세요.</div>'; return; }
  c.innerHTML = `<h4 style="margin-bottom:8px;font-size:13px;">발견된 단말기 (${list.length}개)</h4>
  <div class="table-wrap"><table>
    <thead><tr><th>IP</th><th>SN</th><th>단말기명</th><th>모델</th><th>펌웨어</th><th>포트</th><th></th></tr></thead>
    <tbody>${list.map(d => `<tr>
      <td><b>${d.IpAddress}</b></td><td>${d.DeviceSN??''}</td><td>${d.DeviceName??''}</td>
      <td>${d.Model??''}</td><td>${d.FirmwareVersion??''}</td><td>${d.HttpPort??80}</td>
      <td><button class="btn-success" style="font-size:11px;padding:3px 8px;" onclick="connectDevice('${d.IpAddress}',${d.HttpPort??80})">연결</button></td>
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
      DeviceName: di.DeviceName ?? 'Face Device',
      Model: di.Model, FirmwareVersion: di.FirmwareVersion
    });
    if (reg.Code !== 0) throw new Error(reg.Msg || '연결 실패');
    setStatus(`${di.DeviceSN} 연결 완료`, 'ok');
    addLog(`단말기 연결: ${di.DeviceSN} (${ip})`);
    await refreshDevices();
  } catch(e) { setStatus('연결 실패: ' + e.message, 'err'); addLog('연결 실패: ' + e.message); }
}

async function pullSelectedDevicePeople() {
  const sns = getCheckedDevSNs();
  if (!sns.length) { setStatus('단말기를 선택하세요', 'err'); return; }
  for (const sn of sns) {
    try {
      await apiPost(`/admin/devices/${sn}/pull-all-people`, {});
      addLog(`[${sn}] 사용자 가져오기 명령 예약`);
    } catch(e) { addLog(`[${sn}] 사용자 가져오기 실패: ${e.message}`); }
  }
  setStatus('사용자 가져오기 명령 예약 완료', 'ok');
}

async function distributeToDevices() {
  const selectedUIDs = getCheckedPersonUIDs();
  if (!selectedUIDs.length) { setStatus('배포할 사용자를 사용자 탭에서 먼저 선택하세요', 'err'); return; }
  showDistributeModal(selectedUIDs);
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
    addLog(`배포: ${uids.length}명 → ${sns.length}개 단말기`);
  } catch(e) { setStatus('배포 실패: ' + e.message, 'err'); }
}

function closeDistributeModal() { document.getElementById('distributeModal').classList.remove('open'); }

async function syncTimeSelected() {
  const sns = getCheckedDevSNs();
  if (!sns.length) { setStatus('단말기를 선택하세요', 'err'); return; }
  for (const sn of sns) {
    try {
      await apiPost(`/admin/devices/${sn}/remote-command`, { SN: sn, CommandType: 'synctime' });
      addLog(`[${sn}] 시간 동기화 명령 예약`);
    } catch(e) { addLog(`[${sn}] 시간 동기화 실패: ${e.message}`); }
  }
  setStatus('시간 동기화 명령 예약 완료', 'ok');
}

async function removeSelectedDevices() {
  const sns = getCheckedDevSNs();
  if (!sns.length) { setStatus('제거할 단말기를 선택하세요', 'err'); return; }
  if (!confirm(`선택한 ${sns.length}개 단말기를 제거하시겠습니까?`)) return;
  for (const sn of sns) {
    try { await apiDel(`/admin/devices/${sn}`); addLog(`[${sn}] 단말기 제거`); }
    catch(e) { addLog(`[${sn}] 제거 실패: ${e.message}`); }
  }
  await refreshDevices();
  setStatus('제거 완료', 'ok');
}

// ─── 단말기 설정 모달 ────────────────────────────────────────────
function openDevSettings(sn) {
  _currentDeviceSN = sn;
  const d = devices.find(x => x.SN === sn) ?? { SN: sn };
  document.getElementById('devSettingsTitle').textContent = `단말기 설정 - ${d.DeviceName ?? sn}`;
  document.getElementById('dName').value = d.DeviceName ?? '';
  document.getElementById('dTag').value  = d.TagName    ?? '';
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
  } catch(e) { appendDevLog('저장 실패: ' + e.message); }
}

async function devCmd(cmd) {
  const sn = _currentDeviceSN;
  try {
    appendDevLog(`${cmd} 명령 전송 중...`);
    await apiPost(`/admin/devices/${sn}/remote-command`, { SN: sn, CommandType: cmd });
    appendDevLog(`${cmd} 명령 예약 완료`);
    addLog(`[${sn}] ${cmd} 명령 예약`);
  } catch(e) { appendDevLog(`실패: ${e.message}`); }
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
      return `<tr>
        <td>${esc(p.UserID)}</td><td>${esc(p.Name)}</td>
        <td>${p.CardNum && p.CardNum !== '0' ? 'O' : '-'}</td>
        <td>${p.Photo ? 'O' : '-'}</td>
        <td>${(p.Palmveins?.length > 0) ? 'O' : '-'}</td>
        <td>${exp}</td>
      </tr>`;
    }).join('');
  } catch(e) { document.getElementById('devUsersBody').innerHTML = `<tr><td colspan="6" class="empty-state">로드 실패: ${e.message}</td></tr>`; }
}

function closeDevUsers() { document.getElementById('devUsersModal').classList.remove('open'); }

// ─── 사용자 탭 ──────────────────────────────────────────────────
async function refreshPersonnel() {
  try {
    setStatus('사용자 목록 로딩 중...');
    personnel = await apiGet('/admin/people') ?? [];
    filteredPersonnel = [...personnel];
    renderPersonnel();
    setStatus(`사용자 ${personnel.length}명 로드됨`, 'ok');
  } catch(e) { setStatus('사용자 로드 실패: ' + e.message, 'err'); }
}

function filterPersonnel() {
  const q = document.getElementById('searchPerson').value.trim().toLowerCase();
  filteredPersonnel = q
    ? personnel.filter(p => (p.Name||'').toLowerCase().includes(q) || (p.UserID||'').toLowerCase().includes(q) || (p.CardNum||'').toLowerCase().includes(q))
    : [...personnel];
  renderPersonnel();
}

function renderPersonnel() {
  const tb = document.getElementById('personnelBody');
  if (!filteredPersonnel.length) { tb.innerHTML = '<tr><td colspan="10" class="empty-state">사용자가 없습니다.</td></tr>'; return; }
  tb.innerHTML = filteredPersonnel.map(p => {
    const dong   = p.Department ?? '';
    const ho     = p.Job ?? '';
    const member = p.IdentityCard ?? '';
    const card   = (p.CardNum && p.CardNum !== '0') ? 'O' : '-';
    const pass   = p.Password ? '●●●●' : '?';
    const fp     = (p.Fingerprints?.length > 0) ? 'O' : '-';
    const palm   = (p.Palmveins?.length > 0) ? 'O' : '-';
    const face   = p.Photo ? 'O' : '-';
    return `<tr data-uid="${esc(p.UserID)}">
      <td><input type="checkbox" class="chkPerson" data-uid="${esc(p.UserID)}"></td>
      <td>${esc(dong)}</td><td>${esc(ho)}</td><td>${esc(member)}</td>
      <td><b>${esc(p.Name)}</b> <span style="color:#999;font-size:11px;">(${esc(p.UserID)})</span></td>
      <td style="text-align:center;">${card}</td>
      <td style="text-align:center;">${pass}</td>
      <td style="text-align:center;">${fp}</td>
      <td style="text-align:center;">${palm}</td>
      <td style="text-align:center;">${face}</td>
    </tr>`;
  }).join('');
  document.querySelectorAll('#personnelBody tr[data-uid]').forEach(row => {
    row.addEventListener('dblclick', () => openEditPerson(row.dataset.uid));
  });
}

function toggleAllPerson(chk) {
  document.querySelectorAll('.chkPerson').forEach(c => c.checked = chk.checked);
}

function getCheckedPersonUIDs() {
  return [...document.querySelectorAll('.chkPerson:checked')].map(c => c.dataset.uid);
}

async function reloadFromFiles() {
  try {
    const res = await apiPost('/admin/people/reload-from-files', {});
    setStatus('파일에서 불러오기 완료', 'ok');
    addLog('파일 불러오기 결과: ' + (res.Message ?? JSON.stringify(res)));
    await refreshPersonnel();
  } catch(e) { setStatus('불러오기 실패: ' + e.message, 'err'); }
}

async function distributePersonnel() {
  const uids = getCheckedPersonUIDs();
  if (!uids.length) { setStatus('배포할 사용자를 선택하세요', 'err'); return; }
  if (!devices.length) await refreshDevices();
  showDistributeModal(uids);
}

// 사용자 추가 모달
function showAddPersonModal() {
  _editingUserId = null;
  _photoBase64 = null;
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
  _editingUserId = uid;
  _photoBase64 = null;
  clearPersonModal();
  document.getElementById('personModalTitle').textContent = '사용자 수정';
  document.getElementById('mUid').disabled = true;
  try {
    const res = await apiPost('/api/People/GetDetail', { UserID: uid });
    const p = res.Data ?? res;
    document.getElementById('mUid').value    = p.UserID ?? '';
    document.getElementById('mName').value   = p.Name ?? '';
    document.getElementById('mDong').value   = p.Department ?? '';
    document.getElementById('mHo').value     = p.Job ?? '';
    document.getElementById('mMember').value = p.IdentityCard ?? '';
    document.getElementById('mCard').value   = (p.CardNum && p.CardNum !== '0') ? p.CardNum : '';
    document.getElementById('mPass').value   = p.Password ?? '';
    document.getElementById('mAccess').value = p.AccessType ?? 0;
    if (p.ExpirationDate > 0) {
      const dt = new Date(p.ExpirationDate * 1000);
      document.getElementById('mExpiry').value = dt.toISOString().slice(0, 16);
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
  } catch(e) { setStatus('사용자 정보 로드 실패: ' + e.message, 'err'); return; }
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
    ph.id = 'mPhotoWrap'; ph.className = 'photo-placeholder'; ph.textContent = '\uD83D\uDE36';
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
    ph.id = 'mPhotoWrap'; ph.className = 'photo-placeholder'; ph.textContent = '\uD83D\uDE36';
    wrap.replaceWith(ph);
  }
}

async function savePerson() {
  const uid  = document.getElementById('mUid').value.trim();
  const name = document.getElementById('mName').value.trim();
  if (!uid || !name) { setStatus('사용자 ID와 이름은 필수입니다', 'err'); return; }

  let expiry = 0;
  const expiryVal = document.getElementById('mExpiry').value;
  if (expiryVal) expiry = Math.floor(new Date(expiryVal).getTime() / 1000);

  const person = {
    UserID: uid, Name: name,
    Department: document.getElementById('mDong').value.trim(),
    Job:         document.getElementById('mHo').value.trim(),
    IdentityCard: document.getElementById('mMember').value.trim(),
    CardNum:     document.getElementById('mCard').value.trim() || '0',
    Password:    document.getElementById('mPass').value.trim(),
    AccessType:  parseInt(document.getElementById('mAccess').value) || 0,
    ExpirationDate: expiry,
    OpenTimes: 65535, Timegroup: 1,
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
  } catch(e) { setStatus('저장 실패: ' + e.message, 'err'); addLog('저장 실패: ' + e.message); }
}

async function deleteSelectedPersonnel() {
  const uids = getCheckedPersonUIDs();
  if (!uids.length) { setStatus('삭제할 사용자를 선택하세요', 'err'); return; }
  const names = uids.map(uid => { const p = personnel.find(x => x.UserID === uid); return p?.Name ?? uid; });
  if (!confirm(`${names.join(', ')} (${uids.length}명)을 삭제하시겠습니까?`)) return;
  let ok = 0, fail = 0;
  for (const uid of uids) {
    try {
      const res = await apiPost('/api/People/Delete', { UserID: uid });
      if (res.Code === 0 || res.Success === 1) ok++; else fail++;
    } catch { fail++; }
  }
  setStatus(`삭제 완료: ${ok}명 성공${fail > 0 ? `, ${fail}명 실패` : ''}`, ok > 0 ? 'ok' : 'err');
  addLog(`삭제: 성공=${ok}, 실패=${fail}`);
  await refreshPersonnel();
}

// ─── 출입기록 탭 ────────────────────────────────────────────────
function setDefaultAttendanceDates() {
  const now = new Date();
  const start = new Date(now);
  start.setHours(0, 0, 0, 0);
  document.getElementById('attEnd').value   = toLocalISOString(now);
  document.getElementById('attStart').value = toLocalISOString(start);
}

function toLocalISOString(d) {
  const pad = n => String(n).padStart(2, '0');
  return `${d.getFullYear()}-${pad(d.getMonth()+1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
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
    UserID:    document.getElementById('attUID').value.trim()  || undefined,
    UserName:  document.getElementById('attName').value.trim() || undefined,
    DeviceSN:  document.getElementById('attDevice').value      || undefined,
    StartTime: document.getElementById('attStart').value ? new Date(document.getElementById('attStart').value).toISOString() : undefined,
    EndTime:   document.getElementById('attEnd').value   ? new Date(document.getElementById('attEnd').value).toISOString()   : undefined,
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
  } catch(e) { setStatus('검색 실패: ' + e.message, 'err'); }
}

function renderAttendance(records) {
  const tb = document.getElementById('attBody');
  if (!records.length) { tb.innerHTML = '<tr><td colspan="7" class="empty-state">기록 없음</td></tr>'; return; }
  tb.innerHTML = records.map(r => {
    const photoCell = r.PhotoUrl
      ? `<img src="${r.PhotoUrl}" style="height:36px;border-radius:3px;" onerror="this.style.display='none'">`
      : '?';
    const typeLabel = r.RecordType === 1 ? '<span class="badge badge-success">인식</span>'
                    : r.RecordType === 2 ? '<span class="badge badge-info">도어센서</span>'
                    : '<span class="badge badge-secondary">시스템</span>';
    return `<tr>
      <td>${esc(r.RecordTime ?? '')}</td>
      <td>${esc(r.UserID ?? '')}</td>
      <td>${esc(r.UserName ?? '')}</td>
      <td style="font-size:11px;">${esc(r.DeviceSN ?? '')}</td>
      <td>${typeLabel}</td>
      <td>${r.Temperature ? r.Temperature + '℃' : '?'}</td>
      <td>${photoCell}</td>
    </tr>`;
  }).join('');
}

function renderAttPager() {
  const totalPages = Math.ceil(_attTotal / ATT_PAGE_SIZE);
  const pager = document.getElementById('attPager');
  if (totalPages <= 1) { pager.innerHTML = ''; return; }
  let html = '';
  if (_attPage > 1) html += `<button class="btn-secondary" onclick="searchAttendance(${_attPage-1})">◀ 이전</button>`;
  html += `<span style="font-size:12px;color:#666;">${_attPage} / ${totalPages} 페이지</span>`;
  if (_attPage < totalPages) html += `<button class="btn-secondary" onclick="searchAttendance(${_attPage+1})">다음 ▶</button>`;
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
      UserID:    document.getElementById('attUID').value.trim()  || undefined,
      UserName:  document.getElementById('attName').value.trim() || undefined,
      DeviceSN:  document.getElementById('attDevice').value      || undefined,
      StartTime: document.getElementById('attStart').value ? new Date(document.getElementById('attStart').value).toISOString() : undefined,
      EndTime:   document.getElementById('attEnd').value   ? new Date(document.getElementById('attEnd').value).toISOString()   : undefined,
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
  } catch(e) { setStatus('내보내기 실패: ' + e.message, 'err'); }
}

// ─── 유틸 ───────────────────────────────────────────────────────
function esc(s) {
  return String(s ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}
