// Global state
let currentTab = 'software';
let devices = [];
let discoveredDevices = [];
let personnel = [];
let departments = [];
let records = [];

// Initialize
document.addEventListener('DOMContentLoaded', () => {
  setupTabSwitching();
  refreshSystemInfo();
});

// Tab switching
function setupTabSwitching() {
  document.querySelectorAll('.nav-tab').forEach(tab => {
    tab.addEventListener('click', () => {
      const tabName = tab.dataset.tab;
      switchTab(tabName);
    });
  });
}

function switchTab(tabName) {
  currentTab = tabName;
  document.querySelectorAll('.nav-tab').forEach(t => t.classList.remove('active'));
  document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
  document.querySelector(`[data-tab="${tabName}"]`).classList.add('active');
  document.getElementById(`${tabName}-tab`).classList.add('active');

  if (tabName === 'software') refreshSystemInfo();
  else if (tabName === 'device') refreshDevices();
  else if (tabName === 'corporate') refreshDepartments();
  else if (tabName === 'dooraccess') refreshPersonnel();
  else if (tabName === 'attendance') refreshAttendance();
  else if (tabName === 'record') refreshRecords();
}

// Status message
function showStatus(message, type = 'info') {
  const alert = document.getElementById('statusAlert');
  alert.className = `alert alert-${type}`;
  alert.textContent = message;
  alert.style.display = 'block';
  setTimeout(() => alert.style.display = 'none', 5000);
}

// API helpers
async function apiGet(url) {
  const response = await fetch(url);
  if (!response.ok) throw new Error(`HTTP ${response.status}`);
  return await response.json();
}

async function apiPost(url, data) {
  const response = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  });
  if (!response.ok) throw new Error(`HTTP ${response.status}`);
  return await response.json();
}

async function apiDelete(url) {
  const response = await fetch(url, { method: 'DELETE' });
  if (!response.ok) throw new Error(`HTTP ${response.status}`);
  return await response.json();
}

// ===== DEVICE TAB =====
async function autoSearchDevices() {
  try {
    showStatus('Searching for devices via broadcast...', 'info');
    const response = await apiPost('/api/Device/Search', { SearchType: 'broadcast' });

    if (response.result && response.content) {
      discoveredDevices = response.content;
      renderDiscoveredDevices();
      showStatus(`Found ${discoveredDevices.length} device(s)`, 'success');
    } else {
      showStatus('No devices found', 'error');
    }
  } catch (error) {
    showStatus('Search failed: ' + error.message, 'error');
  }
}

function showNetworkScanDialog() {
  document.getElementById('networkScanDialog').style.display = 'block';
}

function cancelNetworkScan() {
  document.getElementById('networkScanDialog').style.display = 'none';
}

async function startNetworkScan() {
  const subnet = document.getElementById('scanSubnet').value.trim();
  if (!subnet) {
    showStatus('Please enter subnet (e.g., 192.168.0)', 'error');
    return;
  }

  try {
    showStatus(`Scanning network ${subnet}.1-254...`, 'info');
    cancelNetworkScan();

    discoveredDevices = [];
    renderDiscoveredDevices();

    const response = await fetch('/api/Device/SearchStream', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ 
        SearchType: 'scan',
        Subnet: subnet
      })
    });

    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }

    const reader = response.body.getReader();
    const decoder = new TextDecoder();
    let buffer = '';

    while (true) {
      const { done, value } = await reader.read();

      if (done) {
        break;
      }

      buffer += decoder.decode(value, { stream: true });

      const lines = buffer.split('\n\n');
      buffer = lines.pop() || '';

      for (const line of lines) {
        if (line.startsWith('data: ')) {
          const jsonStr = line.substring(6).trim();

          if (jsonStr === '[DONE]') {
            showStatus(`Scan complete: Found ${discoveredDevices.length} device(s)`, 'success');
            continue;
          }

          try {
            const device = JSON.parse(jsonStr);
            discoveredDevices.push(device);
            renderDiscoveredDevices();
            showStatus(`Scanning... Found ${discoveredDevices.length} device(s) so far`, 'info');
          } catch (e) {
            console.error('Failed to parse device JSON:', jsonStr, e);
          }
        }
      }
    }

    if (discoveredDevices.length === 0) {
      showStatus('No devices found', 'error');
    }
  } catch (error) {
    showStatus('Scan failed: ' + error.message, 'error');
  }
}

function renderDiscoveredDevices() {
  const container = document.getElementById('discoveredDevicesContainer');

  if (!discoveredDevices.length) {
    container.innerHTML = '<div class="empty-state">No devices discovered. Try Auto Search or Network Scan.</div>';
    return;
  }

  const html = `
    <h3 style="margin:20px 0 10px 0;">Discovered Devices</h3>
    <table>
      <thead>
        <tr>
          <th>IP Address</th>
          <th>Device SN</th>
          <th>Device Name</th>
          <th>Model</th>
          <th>Firmware</th>
          <th>HTTP Port</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        ${discoveredDevices.map(d => `
          <tr>
            <td><strong>${d.IpAddress}</strong></td>
            <td>${d.DeviceSN}</td>
            <td>${d.DeviceName}</td>
            <td>${d.Model}</td>
            <td>${d.FirmwareVersion}</td>
            <td>${d.HttpPort}</td>
            <td>
              <button class="btn-success btn-sm" onclick="connectToDevice('${d.IpAddress}', ${d.HttpPort})">Connect</button>
            </td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  `;
  container.innerHTML = html;
}

async function connectToDevice(ip, port) {
  try {
    showStatus(`Connecting to device at ${ip}:${port}...`, 'info');

    // 서버의 프록시를 통해 디바이스 정보 확인
    const probeResponse = await apiPost('/api/Device/ProbeDevice', {
      IpAddress: ip,
      HttpPort: port
    });

    if (probeResponse.Code !== 0) {
      throw new Error(probeResponse.Msg || 'Failed to probe device');
    }

    const deviceInfo = probeResponse.Data;
    const deviceSN = deviceInfo.DeviceSN;
    const deviceName = deviceInfo.DeviceName || 'Face Device';
    const model = deviceInfo.Model || 'Unknown';
    const firmware = deviceInfo.FirmwareVersion || 'Unknown';

    // 서버에 디바이스 연결 정보 저장
    const connectResponse = await apiPost('/api/Device/Connect', {
      DeviceSN: deviceSN,
      IpAddress: ip,
      HttpPort: port,
      DeviceName: deviceName,
      Model: model,
      FirmwareVersion: firmware
    });

    if (connectResponse.Code === 0) {
      showStatus(`Successfully connected to ${deviceSN} at ${ip}:${port}`, 'success');
      // 연결된 디바이스 목록 새로고침
      await refreshDevices();
    } else {
      showStatus(`Connection failed: ${connectResponse.Msg}`, 'error');
    }
  } catch (error) {
    showStatus(`Connection failed: ${error.message}`, 'error');
  }
}

async function refreshDevices() {
  try {
    devices = await apiGet('/admin/devices');
    renderDevices();
    showStatus(`Loaded ${devices.length} connected device(s)`, 'success');
  } catch (error) {
    showStatus('Failed to load devices: ' + error.message, 'error');
  }
}

function renderDevices() {
  const container = document.getElementById('connectedDevicesContainer');
  if (!devices.length) {
    if (discoveredDevices.length === 0) {
      container.innerHTML = '<div class="empty-state">No devices connected yet. Use Auto Search to discover devices.</div>';
    } else {
      container.innerHTML = '';
    }
    return;
  }

  const html = `
    <h3 style="margin:20px 0 10px 0;">Connected Devices</h3>
    <table>
      <thead>
        <tr>
          <th>Device SN</th>
          <th>IP Address</th>
          <th>Device Name</th>
          <th>Model</th>
          <th>Last Keepalive</th>
          <th>Add Pending</th>
          <th>Delete Pending</th>
          <th>Records</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        ${devices.map(d => `
          <tr>
            <td><strong>${d.SN}</strong></td>
            <td>${d.IpAddress || '-'}:${d.HttpPort || 80}</td>
            <td>${d.DeviceName || '-'}</td>
            <td>${d.Model || '-'}</td>
            <td>${d.LastKeepaliveAtUtc ? new Date(d.LastKeepaliveAtUtc).toLocaleString() : '-'}</td>
            <td><span class="badge badge-info">${d.PendingAddPeopleCount || 0}</span></td>
            <td><span class="badge badge-warning">${d.PendingDeletePeopleCount || 0}</span></td>
            <td><span class="badge badge-success">${d.RecordCount || 0}</span></td>
            <td>
              <button class="btn-primary btn-sm" onclick="requestAddPeople('${d.SN}')">Sync People</button>
              <button class="btn-secondary btn-sm" onclick="requestDeletePeople('${d.SN}')">Sync Delete</button>
            </td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  `;
  container.innerHTML = html;
}

async function requestAddPeople(sn) {
  try {
    await apiPost(`/admin/devices/${encodeURIComponent(sn)}/request-add-people`, {});
    showStatus(`Add People request queued for ${sn}`, 'success');
    refreshDevices();
  } catch (error) {
    showStatus('Failed: ' + error.message, 'error');
  }
}

async function requestDeletePeople(sn) {
  try {
    await apiPost(`/admin/devices/${encodeURIComponent(sn)}/request-delete-people`, {});
    showStatus(`Delete People request queued for ${sn}`, 'success');
    refreshDevices();
  } catch (error) {
    showStatus('Failed: ' + error.message, 'error');
  }
}

// ===== CORPORATE TAB =====
async function refreshDepartments() {
  try {
    departments = await apiGet('/admin/departments');
    renderDepartments();
    updateDepartmentDropdown();
    showStatus(`Loaded ${departments.length} department(s)`, 'success');
  } catch (error) {
    showStatus('Failed to load departments: ' + error.message, 'error');
  }
}

function renderDepartments() {
  const container = document.getElementById('departmentsContainer');
  if (!departments.length) {
    container.innerHTML = '<div class="empty-state">No departments created yet.</div>';
    return;
  }

  const table = `
    <table>
      <thead>
        <tr>
          <th>Department ID</th>
          <th>Department Name</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        ${departments.map(d => `
          <tr>
            <td><strong>${d.DepartmentID}</strong></td>
            <td>${d.Name}</td>
            <td>
              <button class="btn-danger btn-sm" onclick="deleteDepartment('${d.DepartmentID}')">Delete</button>
            </td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  `;
  container.innerHTML = table;
}

function showAddDepartmentForm() {
  document.getElementById('addDepartmentForm').style.display = 'block';
  document.getElementById('deptId').value = '';
  document.getElementById('deptName').value = '';
}

function cancelAddDepartment() {
  document.getElementById('addDepartmentForm').style.display = 'none';
}

async function saveDepartment() {
  const deptId = document.getElementById('deptId').value.trim();
  const deptName = document.getElementById('deptName').value.trim();

  if (!deptId || !deptName) {
    showStatus('Please fill in all required fields', 'error');
    return;
  }

  try {
    await apiPost('/admin/departments', {
      DepartmentID: deptId,
      Name: deptName
    });
    showStatus('Department saved successfully', 'success');
    cancelAddDepartment();
    refreshDepartments();
  } catch (error) {
    showStatus('Failed to save department: ' + error.message, 'error');
  }
}

async function deleteDepartment(id) {
  if (!confirm(`Delete department ${id}?`)) return;

  try {
    await apiDelete(`/admin/departments/${encodeURIComponent(id)}`);
    showStatus('Department deleted successfully', 'success');
    refreshDepartments();
  } catch (error) {
    showStatus('Failed to delete department: ' + error.message, 'error');
  }
}

function updateDepartmentDropdown() {
  const select = document.getElementById('userDepartment');
  select.innerHTML = '<option value="">-- Select Department --</option>' +
    departments.map(d => `<option value="${d.Name}">${d.Name}</option>`).join('');
}

// ===== PERSONNEL TAB =====
async function refreshPersonnel() {
  try {
    personnel = await apiGet('/admin/people');
    renderPersonnel();
    showStatus(`Loaded ${personnel.length} personnel`, 'success');
  } catch (error) {
    showStatus('Failed to load personnel: ' + error.message, 'error');
  }
}

function renderPersonnel() {
  const container = document.getElementById('personnelContainer');
  const searchTerm = document.getElementById('searchPerson')?.value.toLowerCase() || '';

  const filtered = searchTerm ? personnel.filter(p => 
    p.UserID.toLowerCase().includes(searchTerm) ||
    p.Name.toLowerCase().includes(searchTerm) ||
    (p.CardNum && p.CardNum.toLowerCase().includes(searchTerm))
  ) : personnel;

  if (!filtered.length) {
    container.innerHTML = '<div class="empty-state">No personnel found.</div>';
    return;
  }

  const accessTypeLabel = (type) => {
    switch(type) {
      case 1: return '<span class="badge badge-warning">Admin</span>';
      case 2: return '<span class="badge badge-danger">Blacklist</span>';
      default: return '<span class="badge badge-success">Normal</span>';
    }
  };

  const table = `
    <table>
      <thead>
        <tr>
          <th>User ID</th>
          <th>Name</th>
          <th>Department</th>
          <th>Job</th>
          <th>Card Number</th>
          <th>Access Type</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        ${filtered.map(p => `
          <tr>
            <td><strong>${p.UserID}</strong></td>
            <td>${p.Name || '-'}</td>
            <td>${p.Department || '-'}</td>
            <td>${p.Job || '-'}</td>
            <td>${p.CardNum || '-'}</td>
            <td>${accessTypeLabel(p.AccessType)}</td>
            <td>
              <button class="btn-danger btn-sm" onclick="deletePerson('${p.UserID}')">Delete</button>
            </td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  `;
  container.innerHTML = table;
}

function searchPersonnel() {
  renderPersonnel();
}

function showAddPersonForm() {
  document.getElementById('addPersonForm').style.display = 'block';
  document.getElementById('userId').value = '';
  document.getElementById('userName').value = '';
  document.getElementById('userDepartment').value = '';
  document.getElementById('userJob').value = '';
  document.getElementById('userCardNum').value = '';
  document.getElementById('userPassword').value = '';
  document.getElementById('userAccessType').value = '0';

  if (departments.length === 0) {
    refreshDepartments();
  }
}

function cancelAddPerson() {
  document.getElementById('addPersonForm').style.display = 'none';
}

async function savePerson() {
  const userId = document.getElementById('userId').value.trim();
  const userName = document.getElementById('userName').value.trim();

  if (!userId || !userName) {
    showStatus('User ID and Name are required', 'error');
    return;
  }

  const personData = {
    UserID: userId,
    Name: userName,
    Department: document.getElementById('userDepartment').value,
    Job: document.getElementById('userJob').value.trim(),
    CardNum: document.getElementById('userCardNum').value.trim(),
    Password: document.getElementById('userPassword').value.trim(),
    AccessType: parseInt(document.getElementById('userAccessType').value)
  };

  try {
    await apiPost('/admin/people', personData);
    showStatus('Personnel saved successfully', 'success');
    cancelAddPerson();
    refreshPersonnel();
  } catch (error) {
    showStatus('Failed to save personnel: ' + error.message, 'error');
  }
}

async function deletePerson(userId) {
  if (!confirm(`Delete personnel ${userId}?`)) return;

  try {
    await apiDelete(`/admin/people/${encodeURIComponent(userId)}`);
    showStatus('Personnel deleted successfully', 'success');
    refreshPersonnel();
  } catch (error) {
    showStatus('Failed to delete personnel: ' + error.message, 'error');
  }
}

// ===== RECORD TAB =====
async function refreshRecords() {
  try {
    // Get all devices and extract records
    const devicesData = await apiGet('/admin/devices');
    records = [];
    for (const device of devicesData) {
      const details = await apiGet(`/admin/devices/${encodeURIComponent(device.SN)}`);
      if (details.Records) {
        records.push(...details.Records.map(r => ({...r, DeviceSN: device.SN})));
      }
    }
    renderRecords();
    showStatus(`Loaded ${records.length} record(s)`, 'success');
  } catch (error) {
    showStatus('Failed to load records: ' + error.message, 'error');
  }
}

function renderRecords() {
  const container = document.getElementById('recordsContainer');
  const searchTerm = document.getElementById('searchRecord')?.value.toLowerCase() || '';

  const filtered = searchTerm ? records.filter(r => {
    const detail = r.RecordDetail;
    if (!detail) return false;
    return (detail.UserID && detail.UserID.toLowerCase().includes(searchTerm)) ||
           (detail.Name && detail.Name.toLowerCase().includes(searchTerm));
  }) : records;

  if (!filtered.length) {
    container.innerHTML = '<div class="empty-state">No records found.</div>';
    return;
  }

  const table = `
    <table>
      <thead>
        <tr>
          <th>Device SN</th>
          <th>User ID</th>
          <th>Name</th>
          <th>Record Type</th>
          <th>Record Date</th>
          <th>Received At</th>
        </tr>
      </thead>
      <tbody>
        ${filtered.map(r => {
          const detail = r.RecordDetail || {};
          const recordDate = detail.RecordDate ? new Date(detail.RecordDate * 1000).toLocaleString() : '-';
          return `
            <tr>
              <td>${r.DeviceSN}</td>
              <td>${detail.UserID || '-'}</td>
              <td>${detail.Name || '-'}</td>
              <td><span class="badge badge-info">${detail.RecordType || '-'}</span></td>
              <td>${recordDate}</td>
              <td>${new Date(r.ReceivedAtUtc).toLocaleString()}</td>
            </tr>
          `;
        }).join('')}
      </tbody>
    </table>
  `;
  container.innerHTML = table;
}

function searchRecords() {
  renderRecords();
}

async function clearAllRecords() {
  if (!confirm('Are you sure you want to clear all records?')) return;

  showStatus('Clear all records feature not yet implemented', 'error');
}

// ===== SYSTEM TAB =====
async function refreshSystemInfo() {
  try {
    const devicesData = await apiGet('/admin/devices');
    const peopleData = await apiGet('/admin/people');
    const deptData = await apiGet('/admin/departments');

    document.getElementById('totalDevices').textContent = devicesData.length;
    document.getElementById('totalPersonnel').textContent = peopleData.length;
    document.getElementById('totalDepartments').textContent = deptData.length;

    let totalRecords = 0;
    for (const device of devicesData) {
      totalRecords += device.RecordCount || 0;
    }
    document.getElementById('totalRecords').textContent = totalRecords;

    showStatus('System information refreshed', 'success');
  } catch (error) {
    showStatus('Failed to load system info: ' + error.message, 'error');
  }
}

// ===== ATTENDANCE TAB =====
let attendanceRecords = [];

async function refreshAttendance() {
  // Refresh departments dropdown
  try {
    const deptData = await apiGet('/admin/departments');
    const select = document.getElementById('attendanceDepartment');
    select.innerHTML = '<option value="">All Departments</option>';
    deptData.forEach(dept => {
      const option = document.createElement('option');
      option.value = dept.DepartmentID;
      option.textContent = dept.DepartmentName;
      select.appendChild(option);
    });
  } catch (error) {
    console.error('Failed to load departments:', error);
  }

  // Load today's attendance by default
  const today = new Date();
  const startOfDay = new Date(today.getFullYear(), today.getMonth(), today.getDate());
  const endOfDay = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59);

  document.getElementById('attendanceStartTime').value = formatDateTimeLocal(startOfDay);
  document.getElementById('attendanceEndTime').value = formatDateTimeLocal(endOfDay);

  await searchAttendance();
}

async function searchAttendance() {
  try {
    const userID = document.getElementById('attendanceUserID').value.trim();
    const userName = document.getElementById('attendanceUserName').value.trim();
    const departmentID = document.getElementById('attendanceDepartment').value;
    const startTime = document.getElementById('attendanceStartTime').value;
    const endTime = document.getElementById('attendanceEndTime').value;

    const request = {
      UserID: userID || undefined,
      UserName: userName || undefined,
      DepartmentID: departmentID || undefined,
      StartTime: startTime ? new Date(startTime).toISOString() : undefined,
      EndTime: endTime ? new Date(endTime).toISOString() : undefined,
      PageIndex: 1,
      PageSize: 1000
    };

    const response = await apiPost('/api/Attendance/Search', request);
    if (response.Code === 0) {
      attendanceRecords = response.Data.DataList || [];
      renderAttendanceRecords();
      showStatus(`Found ${attendanceRecords.length} attendance record(s)`, 'success');
    } else {
      showStatus('Search failed: ' + response.Msg, 'error');
    }
  } catch (error) {
    showStatus('Failed to search attendance: ' + error.message, 'error');
  }
}

function renderAttendanceRecords() {
  const container = document.getElementById('attendanceContainer');

  if (attendanceRecords.length === 0) {
    container.innerHTML = '<div class="empty-state">No attendance records found</div>';
    return;
  }

  const html = `
    <table>
      <thead>
        <tr>
          <th>User ID</th>
          <th>Name</th>
          <th>Department</th>
          <th>Record Time</th>
          <th>Device SN</th>
          <th>Type</th>
          <th>Temperature</th>
        </tr>
      </thead>
      <tbody>
        ${attendanceRecords.map(record => `
          <tr>
            <td>${escapeHtml(record.UserID || '-')}</td>
            <td>${escapeHtml(record.UserName || '-')}</td>
            <td>${escapeHtml(record.DepartmentName || '-')}</td>
            <td>${escapeHtml(record.RecordTime || '-')}</td>
            <td>${escapeHtml(record.DeviceSN || '-')}</td>
            <td>${getRecordTypeLabel(record.RecordType)}</td>
            <td>${record.Temperature ? escapeHtml(record.Temperature) + '°C' : '-'}</td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  `;
  container.innerHTML = html;
}

async function getAttendanceStatistics() {
  try {
    const startTime = document.getElementById('attendanceStartTime').value;
    const endTime = document.getElementById('attendanceEndTime').value;

    const request = {
      StartTime: startTime ? new Date(startTime).toISOString() : undefined,
      EndTime: endTime ? new Date(endTime).toISOString() : undefined,
      PageIndex: 1,
      PageSize: 1
    };

    const response = await apiPost('/api/Attendance/Statistics', request);
    if (response.Code === 0) {
      const stats = response.Data;
      document.getElementById('statsTotalRecords').textContent = stats.TotalRecords || 0;
      document.getElementById('statsUniqueUsers').textContent = stats.UniqueUsers || 0;
      document.getElementById('statsUniqueDepts').textContent = stats.UniqueDepartments || 0;
      document.getElementById('attendanceStatsPanel').style.display = 'block';
      showStatus('Statistics loaded', 'success');
    } else {
      showStatus('Failed to load statistics: ' + response.Msg, 'error');
    }
  } catch (error) {
    showStatus('Failed to load statistics: ' + error.message, 'error');
  }
}

function clearAttendanceSearch() {
  document.getElementById('attendanceUserID').value = '';
  document.getElementById('attendanceUserName').value = '';
  document.getElementById('attendanceDepartment').value = '';

  const today = new Date();
  const startOfDay = new Date(today.getFullYear(), today.getMonth(), today.getDate());
  const endOfDay = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59);

  document.getElementById('attendanceStartTime').value = formatDateTimeLocal(startOfDay);
  document.getElementById('attendanceEndTime').value = formatDateTimeLocal(endOfDay);

  document.getElementById('attendanceStatsPanel').style.display = 'none';
  attendanceRecords = [];
  renderAttendanceRecords();
  showStatus('Search cleared', 'info');
}

function exportAttendance() {
  if (attendanceRecords.length === 0) {
    showStatus('No records to export', 'warning');
    return;
  }

  // Create CSV content
  const headers = ['User ID', 'Name', 'Department ID', 'Department Name', 'Record Time', 'Device SN', 'Type', 'Temperature'];
  const csvContent = [
    headers.join(','),
    ...attendanceRecords.map(record => [
      record.UserID || '',
      record.UserName || '',
      record.DepartmentID || '',
      record.DepartmentName || '',
      record.RecordTime || '',
      record.DeviceSN || '',
      getRecordTypeLabel(record.RecordType),
      record.Temperature || ''
    ].map(field => `"${field}"`).join(','))
  ].join('\n');

  // Download CSV
  const blob = new Blob(['\uFEFF' + csvContent], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement('a');
  const url = URL.createObjectURL(blob);
  link.setAttribute('href', url);
  link.setAttribute('download', `attendance_${formatDateForFilename(new Date())}.csv`);
  link.style.visibility = 'hidden';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);

  showStatus('Attendance data exported', 'success');
}

function getRecordTypeLabel(type) {
  const labels = {
    0: 'Face',
    1: 'Card',
    2: 'Password',
    3: 'Face+Card',
    4: 'Face+Password'
  };
  return labels[type] || 'Unknown';
}

function formatDateTimeLocal(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  return `${year}-${month}-${day}T${hours}:${minutes}`;
}

function formatDateForFilename(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  return `${year}${month}${day}_${hours}${minutes}`;
}
