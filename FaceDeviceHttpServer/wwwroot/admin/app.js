
async function waitCommandJobs(jobIds,timeoutMs){
  const ids=(jobIds||[]).filter(Boolean);
  if(!ids.length)return {ok:true,message:'대기할 명령이 없습니다.'};
  const deadline=Date.now()+(timeoutMs||90000);
  let last='단말기 응답 대기 중...';
  while(Date.now()<deadline){
    let pending=0, failed=[], succeeded=[];
    for(const id of ids){
      try{
        const job=await apiGet('/admin/command-jobs/'+id);
        last=job.message||job.Message||last;
        const st=(job.status||job.Status||'Pending');
        if(st==='Pending')pending++;
        else if(st==='Failed')failed.push(last);
        else succeeded.push(last);
      }catch(e){pending++;}
    }
    if(pending===0){
      if(failed.length)return {ok:false,message:failed.join('\n')};
      return {ok:true,message:succeeded.join('\n')||'명령이 완료되었습니다.'};
    }
    await new Promise(r=>setTimeout(r,500));
  }
  return {ok:false,message:'단말기 응답 대기 시간이 초과되었습니다.\n'+last};
}

﻿
let devices=[],personnel=[],filteredPersonnel=[];
let _devRowNumbers={},_devRowCounter=0;
let _currentDeviceSN=null,_editingUserId=null,_photoBase64=null;
let _attPage=1,_attTotal=0;
const ATT_PAGE_SIZE=50;

document.addEventListener('DOMContentLoaded',()=>{
  document.querySelectorAll('.nav-tab').forEach(t=>t.addEventListener('click',()=>switchTab(t.dataset.tab)));
  setInterval(()=>{document.getElementById('headerTime').textContent=new Date().toLocaleString('ko-KR');},1000);
  setInterval(refreshSystemInfo,15000);
  refreshSystemInfo();
  setDefaultAttendanceDates();
});

function switchTab(n){
  document.querySelectorAll('.nav-tab').forEach(t=>t.classList.remove('active'));
  document.querySelectorAll('.tab-content').forEach(c=>c.classList.remove('active'));
  document.querySelector('[data-tab="'+n+'"]').classList.add('active');
  document.getElementById(n+'-tab').classList.add('active');
  if(n==='dashboard')refreshSystemInfo();
  else if(n==='devices')refreshDevices();
  else if(n==='personnel')refreshPersonnel();
  else if(n==='attendance')populateAttDeviceCombo();
}

function setStatus(m,t=''){
  const b=document.getElementById('statusBar');
  b.textContent=m; b.className='status-bar'+(t?' '+t:'');
  if(t==='ok')setTimeout(()=>{b.className='status-bar';b.textContent='준비';},4000);
}
function addLog(m){const b=document.getElementById('logBox');b.textContent+='['+new Date().toLocaleTimeString('ko-KR')+'] '+m+'\n';b.scrollTop=b.scrollHeight;}
function clearLog(){document.getElementById('logBox').textContent='';}

async function api(method,url,body,timeoutMs=15000){
  const ctrl=new AbortController();
  const tid=setTimeout(()=>ctrl.abort(),timeoutMs);
  try{
    const o={method,headers:{},signal:ctrl.signal};
    if(body!=null){o.headers['Content-Type']='application/json';o.body=JSON.stringify(body);}
    const r=await fetch(url,o);
    if(!r.ok)throw new Error('HTTP '+r.status);
    return r.json();
  }catch(e){
    if(e.name==='AbortError')throw new Error('타임아웃(응답 없음): '+url);
    throw e;
  }finally{
    clearTimeout(tid);
  }
}
const apiGet=u=>api('GET',u);
const apiPost=(u,b)=>api('POST',u,b??{});
const apiDel=u=>api('DELETE',u);

/* ── 대시보드 ── */
async function saveSettings(){
  const n=parseInt(document.getElementById('retentionMonths').value,10);
  const url=document.getElementById('serverUrlSelect')?.value||'';
  if(Number.isNaN(n)||n<0){setStatus('보관기간을 확인하세요','err');return;}
  try{
    const r=await apiPost('/admin/settings',{RecordRetentionMonths:n,ServerUrl:url});
    const m=r.RecordRetentionMonths??r.recordRetentionMonths??n;
    document.getElementById('retentionMonths').value=m;
    setStatus('설정 저장됨 (XML)','ok');
    addLog('설정 저장: URL='+(r.ServerUrl||url)+', 보관='+m+'개월');
    await refreshSystemInfo();
  }catch(e){setStatus('설정 저장 실패: '+e.message,'err');}
}
async function refreshSystemInfo(){
  try{
    const d=await apiGet('/admin/system-info');
    const st=await apiGet('/admin/settings');
    Object.assign(d,st);
    document.getElementById('statDevices').textContent=d.TotalDevices??'-';
    document.getElementById('statOnline').textContent=d.OnlineDevices??'-';
    document.getElementById('statPersonnel').textContent=d.TotalPeople??'-';
    document.getElementById('statRecords').textContent=d.TotalRecords??'-';
    const rm=document.getElementById('retentionMonths');
    if(rm && (d.RecordRetentionMonths??d.recordRetentionMonths)!=null) rm.value=d.RecordRetentionMonths??d.recordRetentionMonths;
    const sel=document.getElementById('serverUrlSelect');
    if(sel){
      const urls=d.LocalUrls||d.localUrls||[];
      const cur=d.ServerUrl||d.serverUrl||window.location.origin;
      sel.innerHTML=urls.map(u=>'<option value="'+u+'">'+u+'</option>').join('');
      if(cur && ![...sel.options].some(o=>o.value===cur)){
        const o=document.createElement('option');o.value=cur;o.textContent=cur;sel.appendChild(o);
      }
      sel.value=cur;
    }
    const sp=document.getElementById('infoSettingsPath');
    if(sp)sp.textContent='설정 파일: '+(d.SettingsPath||d.settingsPath||'');
    document.getElementById('infoDevices').textContent=d.TotalDevices??'-';
    document.getElementById('infoPersonnel').textContent=d.TotalPeople??'-';
    document.getElementById('infoRecords').textContent=d.TotalRecords??'-';
  }catch(e){}
}

/* ── 단말기 ── */
async function refreshDevices(){
  try{
    setStatus('단말기 목록 로딩 중...');
    devices=await apiGet('/admin/devices')??[];
    devices.forEach(d=>{if(!_devRowNumbers[d.SN])_devRowNumbers[d.SN]=++_devRowCounter;});
    devices.sort((a,b)=>(_devRowNumbers[a.SN]||99)-(_devRowNumbers[b.SN]||99));
    renderDevices();
    setStatus('단말기 '+devices.length+'개 로드됨','ok');
  }catch(e){setStatus('단말기 로드 실패: '+e.message,'err');addLog('ERROR: '+e.message);}
}
function renderDevices(){
  const tb=document.getElementById('devicesBody');
  if(!devices.length){tb.innerHTML='<tr><td colspan="8" class="empty-state">등록된 단말기가 없습니다.</td></tr>';return;}
  const cut=new Date(Date.now()-5*60*1000);
  tb.innerHTML=devices.map(d=>{
    const no=_devRowNumbers[d.SN]??'-';
    const on=d.LastKeepaliveAtUtc&&new Date(d.LastKeepaliveAtUtc)>=cut;
    const bg=on?'<span class="badge badge-success">온라인</span>':'<span class="badge badge-danger">오프라인</span>';
    return '<tr><td><input type="checkbox" class="chkDev" data-sn="'+esc(d.SN)+'">'+'</td>'
      +'<td style="text-align:center">'+no+'</td>'
      +'<td>'+esc(d.DeviceName??'')+'</td><td>'+esc(d.TagName??'')+'</td>'
      +'<td style="font-size:11px">'+esc(d.SN)+'</td><td>'+esc(d.IpAddress??'')+'</td>'
      +'<td>'+bg+'</td>'
      +'<td><button onclick="openDevSettings(\''+esc(d.SN)+'\')" style="font-size:11px;padding:3px 8px" class="btn-primary">설정</button></td>'
      +'</tr>';
  }).join('');
}
function toggleAllDev(c){document.querySelectorAll('.chkDev').forEach(x=>x.checked=c.checked);}
function getCheckedDevSNs(){return[...document.querySelectorAll('.chkDev:checked')].map(c=>c.dataset.sn);}

async function autoSearchDevices(){
  try{
    setStatus('브로드캐스트 검색 중...');addLog('단말기 자동 검색 시작...');
    const r=await apiPost('/api/Device/Search',{SearchType:'broadcast'});
    const f=r.content??r.Data??[];
    renderDiscovered(f);setStatus(f.length+'개 발견됨','ok');addLog('자동 검색 완료: '+f.length+'개');
  }catch(e){setStatus('검색 실패: '+e.message,'err');addLog('검색 실패: '+e.message);}
}
function showNetworkScanDialog(){document.getElementById('networkScanDialog').style.display='block';}
async function startNetworkScan(){
  const s=document.getElementById('scanSubnet').value.trim();
  if(!s){setStatus('서브넷을 입력하세요','err');return;}
  document.getElementById('networkScanDialog').style.display='none';
  setStatus(s+'.x 스캔 중...');addLog('네트워크 스캔 시작: '+s+'.1-254');
  let found=[];renderDiscovered(found);
  try{
    const resp=await fetch('/api/Device/SearchStream',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({SearchType:'scan',Subnet:s})});
    const reader=resp.body.getReader();const dec=new TextDecoder();let buf='';
    while(true){
      const{done,value}=await reader.read();if(done)break;
      buf+=dec.decode(value,{stream:true});
      const lines=buf.split('\n\n');buf=lines.pop()??'';
      for(const line of lines){
        if(!line.startsWith('data: '))continue;
        const sv=line.slice(6).trim();
        if(sv==='[DONE]'){setStatus('스캔 완료: '+found.length+'개','ok');addLog('스캔 완료: '+found.length+'개');continue;}
        try{found.push(JSON.parse(sv));renderDiscovered(found);}catch{}
      }
    }
  }catch(e){setStatus('스캔 실패: '+e.message,'err');}
}
function renderDiscovered(l){
  const c=document.getElementById('discoveredContainer');
  if(!l.length){c.innerHTML='<div class="empty-state">검색된 단말기가 없습니다.</div>';return;}
  c.innerHTML='<h4 style="margin-bottom:8px;font-size:13px">발견된 단말기 ('+l.length+'개)</h4>'
    +'<div class="table-wrap"><table>'
    +'<thead><tr><th>IP</th><th>SN</th><th>단말기명</th><th>모델</th><th>펌웨어</th><th>포트</th><th></th></tr></thead>'
    +'<tbody>'+l.map(d=>'<tr><td><b>'+d.IpAddress+'</b></td><td>'+(d.DeviceSN??'')+'</td><td>'+(d.DeviceName??'')+'</td>'
      +'<td>'+(d.Model??'')+'</td><td>'+(d.FirmwareVersion??'')+'</td><td>'+(d.HttpPort??80)+'</td>'
      +'<td><button class="btn-success" style="font-size:11px;padding:3px 8px" onclick="connectDevice(\''+d.IpAddress+'\',\''+(d.HttpPort??80)+'\')">연결</button></td></tr>').join('')
    +'</tbody></table></div>';
}
async function connectDevice(ip,port){
  try{
    setStatus(ip+':'+port+' 연결 중...');
    const p=await apiPost('/api/Device/ProbeDevice',{IpAddress:ip,HttpPort:parseInt(port)});
    if(p.Code!==0)throw new Error(p.Msg||'탐색 실패');
    const di=p.Data;
    const r=await apiPost('/api/Device/Connect',{DeviceSN:di.DeviceSN,IpAddress:ip,HttpPort:parseInt(port),DeviceName:di.DeviceName??'Face Device',Model:di.Model,FirmwareVersion:di.FirmwareVersion});
    if(r.Code!==0)throw new Error(r.Msg||'연결 실패');
    setStatus(di.DeviceSN+' 연결 완료','ok');addLog('단말기 연결: '+di.DeviceSN+' ('+ip+')');
    await refreshDevices();
  }catch(e){setStatus('연결 실패: '+e.message,'err');addLog('연결 실패: '+e.message);}
}
async function pullSelectedDevicePeople(){
  const sns=getCheckedDevSNs();if(!sns.length){setStatus('단말기를 선택하세요','err');return;}
  for(const sn of sns){try{await apiPost('/admin/devices/'+sn+'/pull-all-people',{});addLog('['+sn+'] 사용자 가져오기 명령 예약');}catch(e){addLog('['+sn+'] 실패: '+e.message);}}
  setStatus('사용자 가져오기 명령 예약 완료','ok');
}
async function distributeToDevices(){
  const u=getCheckedPersonUIDs();if(!u.length){setStatus('배포할 사용자를 사용자 탭에서 선택하세요','err');return;}
  if(!devices.length)await refreshDevices();showDistributeModal(u);
}
function showDistributeModal(u){
  if(!devices.length){setStatus('단말기 목록을 먼저 로드하세요','err');return;}
  const l=document.getElementById('distributeDevList');
  l.innerHTML=devices.map(d=>'<label style="display:flex;align-items:center;gap:8px;padding:6px 0;border-bottom:1px solid #f0f0f0">'
    +'<input type="checkbox" class="chkDistDev" value="'+esc(d.SN)+'" checked>'
    +'<span><b>'+esc(d.DeviceName??d.SN)+'</b> <span style="color:#999;font-size:11px">('+esc(d.SN)+')</span></span></label>').join('');
  l.dataset.uids=JSON.stringify(u);
  document.getElementById('distributeModal').classList.add('open');
}
async function executeDistribute(){
  const sns=[...document.querySelectorAll('.chkDistDev:checked')].map(c=>c.value);
  const u=JSON.parse(document.getElementById('distributeDevList').dataset.uids||'[]');
  if(!sns.length){setStatus('대상 단말기를 선택하세요','err');return;}
  closeDistributeModal();
  const prev=document.body.style.cursor;
  document.body.style.cursor='wait';
  try{
    const r=await apiPost('/admin/people/distribute-to-devices',{PersonIds:u,TargetSNs:sns});
    const items=(r&& (r.content||r.Content))||[];
    const jobIds=items.map(x=>x.JobId||x.jobId).filter(Boolean);
    setStatus('단말기 배포 결과 대기 중...','ok');
    const done=await waitCommandJobs(jobIds,90000);
    setStatus(done.message, done.ok?'ok':'err');
    addLog((done.ok?'배포 완료: ':'배포 실패: ')+done.message);
    alert(done.message);
  }
  catch(e){setStatus('배포 실패: '+e.message,'err');}
  finally{document.body.style.cursor=prev||'default';}
}
function closeDistributeModal(){document.getElementById('distributeModal').classList.remove('open');}
async function syncTimeSelected(){
  const sns=getCheckedDevSNs();if(!sns.length){setStatus('단말기를 선택하세요','err');return;}
  for(const sn of sns){try{await apiPost('/admin/devices/'+sn+'/remote-command',{SN:sn,CommandType:'synctime'});addLog('['+sn+'] 시간 동기화 명령 예약');}catch(e){addLog('['+sn+'] 실패: '+e.message);}}
  setStatus('시간 동기화 명령 예약 완료','ok');
}
async function removeSelectedDevices(){
  const sns=getCheckedDevSNs();if(!sns.length){setStatus('제거할 단말기를 선택하세요','err');return;}
  if(!confirm('선택한 '+sns.length+'개 단말기를 제거하시겠습니까?'))return;
  for(const sn of sns){try{await apiDel('/admin/devices/'+sn);addLog('['+sn+'] 단말기 제거');}catch(e){addLog('['+sn+'] 제거 실패: '+e.message);}}
  await refreshDevices();setStatus('제거 완료','ok');
}

/* ── 단말기 설정 모달 ── */
function openDevSettings(sn){
  _currentDeviceSN=sn;const d=devices.find(x=>x.SN===sn)??{SN:sn};
  document.getElementById('devSettingsTitle').textContent='단말기 설정 - '+(d.DeviceName??sn);
  document.getElementById('dName').value=d.DeviceName??'';
  document.getElementById('dTag').value=d.TagName??'';
  document.getElementById('deviceCmdLog').textContent='';
  document.getElementById('devSettingsModal').classList.add('open');
}
function closeDevSettings(){document.getElementById('devSettingsModal').classList.remove('open');}
async function saveDeviceInfo(){
  const sn=_currentDeviceSN;
  try{await apiPost('/admin/devices/'+sn+'/update-info',{SN:sn,DeviceName:document.getElementById('dName').value,TagName:document.getElementById('dTag').value});appendDevLog('저장 완료');await refreshDevices();}
  catch(e){appendDevLog('저장 실패: '+e.message);}
}
async function devCmd(cmd){
  const sn=_currentDeviceSN;
  try{appendDevLog(cmd+' 명령 전송 중...');await apiPost('/admin/devices/'+sn+'/remote-command',{SN:sn,CommandType:cmd});appendDevLog(cmd+' 명령 예약 완료');addLog('['+sn+'] '+cmd);}
  catch(e){appendDevLog('실패: '+e.message);}
}
function appendDevLog(m){const b=document.getElementById('deviceCmdLog');b.textContent+='['+new Date().toLocaleTimeString('ko-KR')+'] '+m+'\n';b.scrollTop=b.scrollHeight;}
async function openDeviceUserList(){
  const sn=_currentDeviceSN;
  document.getElementById('devUsersTitle').textContent='단말기 사용자 정보 - '+sn;
  document.getElementById('devUsersModal').classList.add('open');
  document.getElementById('devUsersBody').innerHTML='<tr><td colspan="7" class="empty-state">단말기에 목록 요청 중...</td></tr>';
  try{
    await apiPost('/admin/devices/'+sn+'/query-owned-people',{});
    let arr=[];
    let last=-1, stable=0;
    for(let i=0;i<45;i++){
      await new Promise(r=>setTimeout(r,1000));
      const l=await apiGet('/admin/devices/'+sn+'/people');
      arr=Array.isArray(l)?l:(l.Data??[]);
      if(arr.length===last) stable++; else stable=0;
      last=arr.length;
      if(arr.length>0 && stable>=3) break;
    }
    window._devUsers=arr;
    if(!arr.length){
      document.getElementById('devUsersBody').innerHTML='<tr><td colspan="7" class="empty-state">단말기 응답 없음 (Keepalive 후 다시 열어보세요)</td></tr>';
      return;
    }
    document.getElementById('devUsersBody').innerHTML=arr.map(p=>{
      const pw=p.Password?('●'.repeat(Math.min(String(p.Password).length,8))):'';
      const fp=(p.Fingerprints&&p.Fingerprints.length)?p.Fingerprints.length:0;
      const pv=(p.Palmveins&&p.Palmveins.length)?p.Palmveins.length:0;
      const face=p.FaceFeature||p.Photo;
      return '<tr onclick="selectDevUser(\''+esc(p.UserID)+'\')" data-uid="'+esc(p.UserID)+'">'
        +'<td>'+esc(p.UserID)+'</td><td>'+esc(p.Name||'')+'</td>'
        +'<td>'+pw+'</td>'
        +'<td>'+(p.CardNum&&p.CardNum!=='0'?'O':'X')+'</td>'
        +'<td>'+(fp?'O':'X')+'</td>'
        +'<td>'+(pv?'O':'X')+'</td>'
        +'<td>'+(face?'O':'X')+'</td></tr>';
    }).join('');
  }catch(e){
    document.getElementById('devUsersBody').innerHTML='<tr><td colspan="7" class="empty-state">로드 실패: '+e.message+'</td></tr>';
  }
}
function selectDevUser(uid){
  window._selectedDevUserId=uid;
  document.querySelectorAll('#devUsersBody tr').forEach(tr=>{
    tr.style.background=tr.getAttribute('data-uid')===uid?'#e8f1ff':'';
  });
}
function devUserAdd(){
  window._devicePeopleSn=_currentDeviceSN;
  showAddPersonModal();
}
async function devUserEdit(){
  const uid=window._selectedDevUserId;
  if(!uid){alert('수정할 사용자를 선택하세요.');return;}
  window._devicePeopleSn=_currentDeviceSN;
  await openEditPerson(uid);
}
async function devUserDelete(){
  const uid=window._selectedDevUserId;
  if(!uid){alert('삭제할 사용자를 선택하세요.');return;}
  const p=(window._devUsers||[]).find(x=>x.UserID===uid);
  if(!confirm('사용자 ['+(p&&p.Name?p.Name:'')+' ('+uid+')]를 이 단말기에서 삭제하시겠습니까?\n\n서버 사용자 목록에는 영향을 주지 않습니다.'))return;
  const r=await fetch('/admin/devices/'+_currentDeviceSN+'/people/'+encodeURIComponent(uid),{method:'DELETE'});
  if(!r.ok){alert('삭제 실패: HTTP '+r.status);return;}
  await openDeviceUserList();
}
function closeDevUsers(){document.getElementById('devUsersModal').classList.remove('open');}

/* ── 사용자 ── */
async function refreshPersonnel(){
  try{
    setStatus('사용자 목록 로딩 중...');
    personnel=await apiGet('/admin/people')??[];
    filteredPersonnel=[...personnel];
    renderPersonnel();setStatus('사용자 '+personnel.length+'명 로드됨','ok');
  }catch(e){setStatus('사용자 로드 실패: '+e.message,'err');}
}
function parseDongHoMember(uid){
  const n=parseInt(uid,10);
  if(isNaN(n)||String(uid).length<3)return{dong:uid,ho:'',member:''};
  return{dong:String(Math.floor(n/1000000)),ho:String(Math.floor((n/100)%10000)),member:String(n%100)};
}
function filterPersonnel(){
  const q=document.getElementById('searchPerson').value.trim().toLowerCase();
  const fd=(document.getElementById('filterDong')?.value||'').trim();
  const fh=(document.getElementById('filterHo')?.value||'').trim();
  const fm=(document.getElementById('filterMember')?.value||'').trim();
  filteredPersonnel=personnel.filter(p=>{
    if(q&&!(p.Name||'').toLowerCase().includes(q)&&!(p.UserID||'').toLowerCase().includes(q)&&!(p.CardNum||'').toLowerCase().includes(q))return false;
    if(fd||fh||fm){
      const{dong,ho,member}=parseDongHoMember(p.UserID);
      if(fd&&dong!==fd)return false;
      if(fh&&ho!==fh)return false;
      if(fm&&member!==fm)return false;
    }
    return true;
  });
  renderPersonnel();
}
function renderPersonnel(){
  const tb=document.getElementById('personnelBody');
  if(!filteredPersonnel.length){tb.innerHTML='<tr><td colspan="10" class="empty-state">사용자가 없습니다.</td></tr>';return;}
  tb.innerHTML=filteredPersonnel.map(p=>{
    const{dong,ho,member}=parseDongHoMember(p.UserID);
    const card=(p.CardNum&&p.CardNum!=='0')?'O':'-';
    const pass=p.Password?'****':'-';
    const fp=(p.Fingerprints&&p.Fingerprints.length>0)?'O':'-';
    const palm=(p.Palmveins&&p.Palmveins.length>0)?'O':'-';
    const face=p.Photo?'O':'-';
    return '<tr data-uid="'+esc(p.UserID)+'"><td><input type="checkbox" class="chkPerson" data-uid="'+esc(p.UserID)+'">'+'</td>'
      +'<td>'+esc(dong)+'</td><td>'+esc(ho)+'</td><td>'+esc(member)+'</td>'
      +'<td><b>'+esc(p.Name)+'</b> <span style="color:#999;font-size:11px">('+esc(p.UserID)+')</span></td>'
      +'<td style="text-align:center">'+card+'</td><td style="text-align:center">'+pass+'</td>'
      +'<td style="text-align:center">'+fp+'</td><td style="text-align:center">'+palm+'</td>'
      +'<td style="text-align:center">'+face+'</td></tr>';
  }).join('');
  document.querySelectorAll('#personnelBody tr[data-uid]').forEach(r=>r.addEventListener('dblclick',()=>openEditPerson(r.dataset.uid)));
}
function toggleAllPerson(c){document.querySelectorAll('.chkPerson').forEach(x=>x.checked=c.checked);}
function getCheckedPersonUIDs(){return[...document.querySelectorAll('.chkPerson:checked')].map(c=>c.dataset.uid);}

function personFileName(uid){
  return String(uid||'user').replace(/[\\/:*?"<>|]/g,'_')+'.json';
}
function canUseDirectoryPicker(){
  return window.isSecureContext===true && typeof window.showDirectoryPicker==='function';
}
function ensurePeopleFileInputs(){
  if(document.getElementById('peopleDirInput'))return;
  const dir=document.createElement('input');
  dir.type='file'; dir.id='peopleDirInput'; dir.style.display='none';
  dir.setAttribute('webkitdirectory',''); dir.setAttribute('directory',''); dir.multiple=true;
  dir.addEventListener('change',()=>importPeopleFromFileList(dir.files));
  const files=document.createElement('input');
  files.type='file'; files.id='peopleFileInput'; files.style.display='none';
  files.multiple=true; files.accept='.json,application/json';
  files.addEventListener('change',()=>importPeopleFromFileList(files.files));
  document.body.appendChild(dir);
  document.body.appendChild(files);
}
function downloadTextFile(name,text){
  const blob=new Blob([text],{type:'application/json;charset=utf-8'});
  const url=URL.createObjectURL(blob);
  const a=document.createElement('a');
  a.href=url; a.download=name; a.style.display='none';
  document.body.appendChild(a); a.click();
  setTimeout(()=>{URL.revokeObjectURL(url);a.remove();},1000);
}
async function upsertPersonFromJson(p){
  const uid=p.UserID||p.userId||p.user_id;
  if(!uid)throw new Error('UserID 없음');
  p.UserID=String(uid);
  const r=await fetch('/admin/people',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(p)});
  if(r.status===409){
    const u=await fetch('/api/People/Update',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(p)});
    if(!u.ok)throw new Error('수정 실패 HTTP '+u.status);
    return 'updated';
  }
  if(!r.ok)throw new Error('추가 실패 HTTP '+r.status);
  return 'added';
}
async function importPeopleFromFileList(fileList){
  const files=[...fileList||[]].filter(f=>f.name.toLowerCase().endsWith('.json'));
  if(!files.length){setStatus('선택한 항목에 JSON 파일이 없습니다','err');return;}
  let added=0, updated=0, errors=0;
  setStatus('로컬 파일에서 불러오는 중...','ok');
  for(const file of files){
    try{
      const parsed=JSON.parse(await file.text());
      const list=Array.isArray(parsed)?parsed:[parsed];
      for(const p of list){
        const kind=await upsertPersonFromJson(p);
        if(kind==='updated')updated++; else added++;
      }
    }catch(e){
      errors++; addLog('불러오기 실패 '+file.name+': '+e.message);
    }
  }
  setStatus('파일 불러오기: 추가 '+added+', 수정 '+updated+', 오류 '+errors,'ok');
  addLog('로컬 파일 불러오기: added='+added+', updated='+updated+', errors='+errors);
  await refreshPersonnel();
}
async function saveToFiles(){
  const u=getCheckedPersonUIDs();
  if(!u.length){setStatus('저장할 사용자를 선택하세요','err');return;}
  let saved=0, errors=0;
  setStatus('사용자 JSON 저장 중...','ok');
  if(canUseDirectoryPicker()){
    try{
      const dir=await window.showDirectoryPicker({mode:'readwrite'});
      for(const uid of u){
        try{
          const r=await fetch('/admin/people/'+encodeURIComponent(uid)+'/export');
          if(!r.ok)throw new Error('HTTP '+r.status);
          const json=await r.text();
          const handle=await dir.getFileHandle(personFileName(uid),{create:true});
          const w=await handle.createWritable();
          await w.write(json); await w.close();
          saved++;
        }catch(e){errors++; addLog('저장 실패 '+uid+': '+e.message);}
      }
      setStatus('파일 저장: '+saved+'명 → 선택한 폴더'+(errors? ', 오류 '+errors+'건':''), errors?'err':'ok');
      return;
    }catch(e){
      if(e&&e.name==='AbortError')return;
      addLog('폴더 선택 불가, 다운로드로 전환: '+e.message);
    }
  }
  for(const uid of u){
    try{
      const r=await fetch('/admin/people/'+encodeURIComponent(uid)+'/export');
      if(!r.ok)throw new Error('HTTP '+r.status);
      downloadTextFile(personFileName(uid), await r.text());
      saved++;
      await new Promise(x=>setTimeout(x,150));
    }catch(e){errors++; addLog('저장 실패 '+uid+': '+e.message);}
  }
  setStatus('파일 저장: '+saved+'명을 브라우저 다운로드 폴더에 저장'+(errors? ', 오류 '+errors+'건':''), errors?'err':'ok');
  addLog('로컬 저장: saved='+saved+', errors='+errors);
}
async function reloadFromFiles(){
  if(canUseDirectoryPicker()){
    try{
      const dir=await window.showDirectoryPicker({mode:'read'});
      const files=[];
      for await (const [name, handle] of dir.entries()){
        if(handle.kind==='file' && name.toLowerCase().endsWith('.json'))
          files.push(await handle.getFile());
      }
      await importPeopleFromFileList(files);
      return;
    }catch(e){
      if(e&&e.name==='AbortError')return;
      addLog('폴더 선택 불가, 파일 선택으로 전환: '+e.message);
    }
  }
  ensurePeopleFileInputs();
  const useDir=confirm('JSON이 들어 있는 폴더를 선택할까요?\n\n취소를 누르면 JSON 파일만 여러 개 선택합니다.');
  if(useDir) document.getElementById('peopleDirInput').click();
  else document.getElementById('peopleFileInput').click();
}

async function distributePersonnel(){
  const u=getCheckedPersonUIDs();if(!u.length){setStatus('배포할 사용자를 선택하세요','err');return;}
  if(!devices.length)await refreshDevices();showDistributeModal(u);
}
function showAddPersonModal(){
  _editingUserId=null;_photoBase64=null;clearPersonModal();
  document.getElementById('personModalTitle').textContent='사용자 추가';
  document.getElementById('mUid').value='';
  document.getElementById('personModal').classList.add('open');
}
async function openEditPerson(uid){
  if(!uid){const u=getCheckedPersonUIDs();if(u.length!==1){setStatus('수정할 사용자를 1명 선택하세요','err');return;}uid=u[0];}
  _editingUserId=uid;_photoBase64=null;_photoCleared=false;clearPersonModal();
  document.getElementById('personModalTitle').textContent='사용자 수정';
  document.getElementById('mUid').disabled=true;
  try{
    const r=await apiPost('/api/People/GetDetail',{UserID:uid});
    if(!r.result)throw new Error(r.error??'사용자 조회 실패');
    const p=r.content??r.Data??r;
    // UserID → 동/호/멤버# 분리표시
    const n=parseInt(p.UserID,10);
    if(!isNaN(n)&&p.UserID.length>=3){
      document.getElementById('mDong').value=String(Math.floor(n/1000000));
      document.getElementById('mHo').value=String(Math.floor((n/100)%10000));
      document.getElementById('mMember').value=String(n%100);
    }else{
      document.getElementById('mDong').value=p.UserID??'';
    }
    document.getElementById('mUid').value=p.UserID??'';
    document.getElementById('mDong').readOnly=true;
    document.getElementById('mHo').readOnly=true;
    document.getElementById('mMember').readOnly=true;
    document.getElementById('mName').value=p.Name??'';
    document.getElementById('mCard').value=(p.CardNum&&p.CardNum!=='0')?p.CardNum:'';
    document.getElementById('mPass').value=p.Password??'';
    _editFingerprints=p.Fingerprints||[];
    _editPalmveins=p.Palmveins||[];
    updateBiometricStatus();
    const unlimited=!p.ExpirationDate||p.ExpirationDate>=4102412399;
    document.getElementById('mExpiryUnlimited').checked=unlimited;
    document.getElementById('mExpiry').disabled=unlimited;
    if(!unlimited)document.getElementById('mExpiry').value=new Date(p.ExpirationDate*1000).toISOString().slice(0,16);

    if(p.Photo){_photoBase64=p.Photo;const w=document.getElementById('mPhotoWrap');const img=document.createElement('img');img.id='mPhotoWrap';img.className='photo-preview';img.src='data:image/jpeg;base64,'+p.Photo;w.replaceWith(img);document.getElementById('mPhotoStatus').textContent='기존 사진 있음';}
  }catch(e){setStatus('사용자 정보 로드 실패: '+e.message,'err');return;}
  document.getElementById('personModal').classList.add('open');
}
function editSelectedPerson(){openEditPerson(null);}
function clearPersonModal(){
  ['mUid','mName','mDong','mHo','mMember','mCard','mPass','mExpiry'].forEach(id=>{const el=document.getElementById(id);if(el){el.value='';el.readOnly=false;}});
  document.getElementById('mExpiryUnlimited').checked=true;
  document.getElementById('mExpiry').disabled=true;
  _editFingerprints=[];_editPalmveins=[];
  updateBiometricStatus();
  document.getElementById('mPhotoStatus').textContent='';document.getElementById('mPhotoFile').value='';_photoBase64=null;
  const w=document.getElementById('mPhotoWrap');if(w){const ph=document.createElement('div');ph.id='mPhotoWrap';ph.className='photo-placeholder';ph.textContent='?';w.replaceWith(ph);}
  updateUidPreview();
}
function closePersonModal(){document.getElementById('personModal').classList.remove('open');window._devicePeopleSn=null;}
function updateUidPreview(){
  const d=parseInt(document.getElementById('mDong')?.value||'0')||0;
  const h=parseInt(document.getElementById('mHo')?.value||'0')||0;
  const m=parseInt(document.getElementById('mMember')?.value||'0')||0;
  const el=document.getElementById('mUid');
  if(el&&!el.readOnly){el.value=(d||h||m)?String(d*1000000+h*100+m):'';}
}

const PE_VIEW=400, PE_OUT=640;
let _peImg=null,_peScale=1,_peX=0,_peY=0,_peDrag=false,_peLast={x:0,y:0};

function openPhotoEditorFromFile(ev){
  const f=ev.target.files&&ev.target.files[0];
  if(!f)return;
  const img=new Image();
  img.onload=()=>{
    _peImg=img;
    _peScale=Math.min(1.5, Math.max(0.10, Math.max(PE_VIEW/img.width, PE_VIEW/img.height)));
    _peX=(PE_VIEW-img.width*_peScale)/2;
    _peY=(PE_VIEW-img.height*_peScale)/2;
    const z=document.getElementById('peZoom');
    if(z)z.value=String(Math.round(_peScale*100));
    document.getElementById('photoEditorModal').classList.add('open');
    bindPeCanvas();
    drawPhotoEditor();
  };
  img.src=URL.createObjectURL(f);
}
function bindPeCanvas(){
  const c=document.getElementById('peCanvas');
  if(!c||c._peBound)return;
  c._peBound=true;
  c.addEventListener('mousedown',e=>{_peDrag=true;_peLast={x:e.offsetX,y:e.offsetY};c.style.cursor='grabbing';});
  window.addEventListener('mouseup',()=>{_peDrag=false;const c2=document.getElementById('peCanvas');if(c2)c2.style.cursor='grab';});
  c.addEventListener('mousemove',e=>{
    if(!_peDrag||!_peImg)return;
    _peX+=e.offsetX-_peLast.x; _peY+=e.offsetY-_peLast.y;
    _peLast={x:e.offsetX,y:e.offsetY};
    drawPhotoEditor();
  });
  c.addEventListener('wheel',e=>{
    e.preventDefault();
    const n=_peScale*(e.deltaY<0?1.08:0.92);
    onPeZoom(n*100);
    const z=document.getElementById('peZoom'); if(z)z.value=String(Math.round(n*100));
  },{passive:false});
}
function onPeZoom(v){
  const ns=Math.min(1.5, Math.max(0.10, Number(v)/100));
  if(!_peImg)return;
  const cx=PE_VIEW/2, cy=PE_VIEW/2;
  const ix=(cx-_peX)/_peScale, iy=(cy-_peY)/_peScale;
  _peScale=ns;
  _peX=cx-ix*_peScale; _peY=cy-iy*_peScale;
  drawPhotoEditor();
}
function drawPhotoEditor(){
  const c=document.getElementById('peCanvas'); if(!c)return;
  const ctx=c.getContext('2d');
  ctx.fillStyle='#1a1a1a'; ctx.fillRect(0,0,PE_VIEW,PE_VIEW);
  if(_peImg) ctx.drawImage(_peImg,_peX,_peY,_peImg.width*_peScale,_peImg.height*_peScale);
  ctx.strokeStyle='rgba(80,220,120,.85)'; ctx.lineWidth=2;
  ctx.beginPath(); ctx.ellipse(200,190,95,125,0,0,Math.PI*2); ctx.stroke();
}
function closePhotoEditor(){
  document.getElementById('photoEditorModal').classList.remove('open');
}
function renderPeOutput(){
  const out=document.createElement('canvas');
  out.width=out.height=PE_OUT;
  const ctx=out.getContext('2d');
  ctx.fillStyle='#ffffff'; ctx.fillRect(0,0,PE_OUT,PE_OUT);
  const k=PE_OUT/PE_VIEW;
  if(_peImg) ctx.drawImage(_peImg,_peX*k,_peY*k,_peImg.width*_peScale*k,_peImg.height*_peScale*k);
  return out;
}
async function applyPhotoEditor(){
  if(!_peImg){closePhotoEditor();return;}
  const out=renderPeOutput();
  const msg=document.getElementById('peFaceMsg');
  if(msg)msg.textContent='얼굴 검사 중...';
  try{
    const data=out.toDataURL('image/jpeg',0.85);
    const r=await fetch('/admin/people/validate-photo',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({Photo:data})});
    const txt=await r.text();
    if(!txt){if(msg)msg.textContent='얼굴 검사 API가 없습니다. FDHS를 ServerFaceValidate 적용 후 다시 빌드하세요.';return;}
    let j; try{j=JSON.parse(txt);}catch(e){if(msg)msg.textContent='얼굴 검사 응답이 JSON이 아닙니다. FDHS를 다시 빌드하세요.';return;}
    if(!j.ok){if(msg)msg.textContent=j.message||'얼굴을 인식할 수 없습니다.';return;}
  }catch(e){
    if(msg)msg.textContent='얼굴 검사 실패: '+(e.message||e);
    return;
  }
  const data=out.toDataURL('image/jpeg',0.85);
  _photoBase64=data.split(',')[1];
  _photoCleared=false;
  const w=document.getElementById('mPhotoWrap');
  const img=document.createElement('img');
  img.id='mPhotoWrap'; img.className='photo-preview'; img.src=data;
  if(w)w.replaceWith(img);
  const st=document.getElementById('mPhotoStatus');
  if(st)st.textContent='편집된 사진 ('+Math.round(_photoBase64.length*0.75/1024)+'KB)';
  closePhotoEditor();
}

function togglePassVisible(){
  const el=document.getElementById('mPass');
  const show=document.getElementById('mPassShow')?.checked;
  if(el)el.type=show?'text':'password';
}
function previewPhoto(ev){
  const f=ev.target.files[0];if(!f)return;
  const r=new FileReader();r.onload=e=>{const data=e.target.result;_photoBase64=data.split(',')[1];const w=document.getElementById('mPhotoWrap');const img=document.createElement('img');img.id='mPhotoWrap';img.className='photo-preview';img.src=data;w.replaceWith(img);document.getElementById('mPhotoStatus').textContent=f.name+' ('+(f.size/1024).toFixed(1)+'KB)';};r.readAsDataURL(f);
}
function clearPhoto(){_photoBase64=null;_photoCleared=true;document.getElementById('mPhotoFile').value='';document.getElementById('mPhotoStatus').textContent='';const w=document.getElementById('mPhotoWrap');if(w){const ph=document.createElement('div');ph.id='mPhotoWrap';ph.className='photo-placeholder';ph.textContent='?';w.replaceWith(ph);}}

function updateBiometricStatus(){
  const fp=(_editFingerprints&&_editFingerprints.length)||0;
  const pv=(_editPalmveins&&_editPalmveins.length)||0;
  const fs=document.getElementById('mFpStatus');
  const ps=document.getElementById('mPalmStatus');
  if(fs)fs.textContent=fp?fp+'개 등록':'없음';
  if(ps)ps.textContent=pv?pv+'개 등록':'없음';
}
function clearFingerprints(){
  _editFingerprints=[];
  updateBiometricStatus();
}
function clearPalmveins(){
  _editPalmveins=[];
  updateBiometricStatus();
}
function onExpiryUnlimitedChange(){
  const u=document.getElementById('mExpiryUnlimited').checked;
  const el=document.getElementById('mExpiry');
  el.disabled=u;
  if(u)el.value='';
}
async function savePerson(){
  const name=document.getElementById('mName').value.trim();
  const dongStr=document.getElementById('mDong').value.trim();
  const hoStr=document.getElementById('mHo').value.trim();
  const memberStr=document.getElementById('mMember').value.trim();
  const dN=parseInt(dongStr),hN=parseInt(hoStr),mN=parseInt(memberStr);
  if(!name){setStatus('이름은 필수입니다','err');return;}
  if(isNaN(dN)||isNaN(hN)||isNaN(mN)||!dongStr||!hoStr||!memberStr){setStatus('동/호/멤버# 동시 입력 필수 (모두 숫자)','err');return;}
  let uid;
  if(_editingUserId){uid=_editingUserId;}else{uid=String(dN*1000000+hN*100+mN);}
  let exp=4102412399;
  if(!document.getElementById('mExpiryUnlimited').checked){
    const ev=document.getElementById('mExpiry').value;
    if(ev)exp=Math.floor(new Date(ev).getTime()/1000);
  }
  const p={UserID:uid,Name:name,Department:dongStr,Job:hoStr,IdentityCard:memberStr,CardNum:document.getElementById('mCard').value.trim()||'0',Password:document.getElementById('mPass').value.trim(),AccessType:0,ExpirationDate:exp,OpenTimes:65535,Timegroup:1,Photo:_photoCleared?'':(_photoBase64??undefined),PhotoLen:_photoCleared?-1:undefined,Fingerprints:_editFingerprints||[],Palmveins:_editPalmveins||[]};
  try{
    if(window._devicePeopleSn){
      await apiPost('/admin/devices/'+window._devicePeopleSn+'/people',p);
      setStatus('단말기 사용자 저장 예약','ok');
      window._devicePeopleSn=null;
      closePersonModal();
      await openDeviceUserList();
      return;
    }
    if(_editingUserId){const r=await apiPost('/api/People/Update',p);if(!r.result)throw new Error(r.error??'수정 실패');setStatus('사용자 수정 완료','ok');addLog('사용자 수정: '+uid);}
    else{const r=await apiPost('/api/People/New',p);if(!r.result)throw new Error(r.error??'추가 실패');setStatus('사용자 추가 완료','ok');addLog('사용자 추가: '+uid);}
    closePersonModal();await refreshPersonnel();
  }catch(e){setStatus('저장 실패: '+e.message,'err');addLog('저장 실패: '+e.message);}
}
async function deleteSelectedPersonnel(){
  const u=getCheckedPersonUIDs();if(!u.length){setStatus('삭제할 사용자를 선택하세요','err');return;}
  const names=u.map(id=>personnel.find(x=>x.UserID===id)?.Name??id);
  if(!confirm(names.join(', ')+' ('+u.length+'명)을 삭제하시겠습니까?'))return;
  let ok=0,fail=0;
  for(const id of u){try{const r=await apiPost('/api/People/Delete',{UserID:id});if(r.result===true)ok++;else fail++;}catch{fail++;}}
  setStatus('삭제: '+ok+'명 성공'+(fail>0?', '+fail+'명 실패':''),ok>0?'ok':'err');addLog('삭제: 성공='+ok+', 실패='+fail);
  await refreshPersonnel();
}

/* ── 출입기록 ── */
function setDefaultAttendanceDates(){
  const n=new Date();const s=new Date(n);s.setHours(0,0,0,0);
  const pad=x=>String(x).padStart(2,'0');
  const fmt=d=>d.getFullYear()+'-'+pad(d.getMonth()+1)+'-'+pad(d.getDate())+'T'+pad(d.getHours())+':'+pad(d.getMinutes());
  document.getElementById('attStart').value=fmt(s);
  document.getElementById('attEnd').value=fmt(n);
  const bind=(chkId,inpId)=>{
    const c=document.getElementById(chkId),i=document.getElementById(inpId);
    if(!c||c.dataset.bound)return;
    c.dataset.bound='1';
    c.addEventListener('change',()=>{i.disabled=!c.checked;});
    i.disabled=!c.checked;
  };
  bind('attStartOn','attStart');
  bind('attEndOn','attEnd');
}
async function populateAttDeviceCombo(){
  const sel=document.getElementById('attDevice');const prev=sel.value;
  sel.innerHTML='<option value="">전체 단말기</option>';
  try{const devs=await apiGet('/admin/devices')??[];devs.forEach(d=>{const o=document.createElement('option');o.value=d.SN;o.textContent=(d.DeviceName??d.SN)+' ('+d.SN+')';sel.appendChild(o);});if(prev)sel.value=prev;}catch{}
}
async function searchAttendance(page=1){
  _attPage=page;
  const req={PageIndex:page,PageSize:ATT_PAGE_SIZE};
  const dong=(document.getElementById('attDong')?.value||'').trim();
  const ho=(document.getElementById('attHo')?.value||'').trim();
  const member=(document.getElementById('attMember')?.value||'').trim();
  const name=(document.getElementById('attName')?.value||'').trim();
  const sn=document.getElementById('attDevice').value;
  const st=document.getElementById('attStart').value;
  const et=document.getElementById('attEnd').value;
  if(dong||ho||member){
    const dN=parseInt(dong)||0,hN=parseInt(ho)||0,mN=parseInt(member)||0;
    if(dong&&ho&&member)req.UserID=String(dN*1000000+hN*100+mN);
    else if(dong&&ho){req.UserIDMin=dN*1000000+hN*100;req.UserIDMax=dN*1000000+hN*100+100;}
    else if(dong){req.UserIDMin=dN*1000000;req.UserIDMax=(dN+1)*1000000;}
  }
  if(name)req.UserName=name;
  if(sn)req.DeviceSN=sn;
  if(document.getElementById('attStartOn')?.checked && st)req.StartTime=new Date(st).toISOString();
  if(document.getElementById('attEndOn')?.checked && et)req.EndTime=new Date(et).toISOString();
  try{
    setStatus('출입기록 검색 중...');
    const r=await api('POST','/api/Attendance/Search',req,20000);
    const data=r.content??r.Data??{};
    const recs=data.DataList??data.Records??[];
    _attTotal=data.TotalCount??recs.length;
    document.getElementById('attHeader').textContent='출입 기록 (총 '+_attTotal+'건)';
    renderAttendance(recs);renderAttPager();setStatus(_attTotal+'건 검색됨','ok');
  }catch(e){setStatus('검색 실패: '+e.message,'err');}
}
function renderAttendance(recs){
  const tb=document.getElementById('attBody');
  if(!recs.length){tb.innerHTML='<tr><td colspan="7" class="empty-state">기록 없음</td></tr>';return;}
  tb.innerHTML=recs.map(r=>{
    const tl='<span class="badge '+rtBadge(r.RecordType)+'">'+rtLabel(r.RecordType)+'</span>';
    const{dong,ho,member}=parseDongHoMember(r.UserID??'');
    return '<tr>'
      +'<td>'+esc(r.RecordTime??'')+'</td>'
      +'<td>'+esc(dong)+'</td>'
      +'<td>'+esc(ho)+'</td>'
      +'<td>'+esc(member)+'</td>'
      +'<td>'+esc(r.UserName??'')+'</td>'
      +'<td style="font-size:11px">'+esc(r.DeviceSN??'')+'</td>'
      +'<td>'+tl+'</td>'
      +'</tr>';
  }).join('');
}
function renderAttPager(){
  const tp=Math.ceil(_attTotal/ATT_PAGE_SIZE);const pg=document.getElementById('attPager');
  if(tp<=1){pg.innerHTML='';return;}
  let h='';
  if(_attPage>1)h+='<button class="btn-secondary" onclick="searchAttendance('+(_attPage-1)+')">이전</button>';
  h+='<span style="font-size:12px;color:#666">'+_attPage+' / '+tp+' 페이지</span>';
  if(_attPage<tp)h+='<button class="btn-secondary" onclick="searchAttendance('+(_attPage+1)+')">다음</button>';
  pg.innerHTML=h;
}
function clearAttSearch(){
  ['attDong','attHo','attMember','attName'].forEach(id=>{const el=document.getElementById(id);if(el)el.value='';});
  document.getElementById('attDevice').value='';
  const so=document.getElementById('attStartOn'),eo=document.getElementById('attEndOn');
  if(so){so.checked=false;document.getElementById('attStart').disabled=true;}
  if(eo){eo.checked=false;document.getElementById('attEnd').disabled=true;}
  setDefaultAttendanceDates();
  document.getElementById('attBody').innerHTML='<tr><td colspan="7" class="empty-state">검색 조건을 입력하세요.</td></tr>';
  document.getElementById('attHeader').textContent='출입 기록';document.getElementById('attPager').innerHTML='';
}
async function exportAttendance(){
  try{
    const req={PageIndex:1,PageSize:9999};
    const dong=document.getElementById('attDong').value.trim();
    const ho=document.getElementById('attHo').value.trim();
    const member=document.getElementById('attMember').value.trim();
    const name=document.getElementById('attName').value.trim();
    const sn=document.getElementById('attDevice').value;const st=document.getElementById('attStart').value;const et=document.getElementById('attEnd').value;
    if(dong||ho||member){
      const dongN=parseInt(dong)||0;const hoN=parseInt(ho)||0;const memberN=parseInt(member)||0;
      if(dong&&!ho&&!member){req.UserIDMin=dongN*1000000;req.UserIDMax=(dongN+1)*1000000;}
      else if(dong&&ho&&!member){req.UserIDMin=dongN*1000000+hoN*100;req.UserIDMax=dongN*1000000+hoN*100+100;}
      else if(dong&&ho&&member){req.UserID=String(dongN*1000000+hoN*100+memberN);}
    }
    if(name)req.UserName=name;if(sn)req.DeviceSN=sn;
    if(document.getElementById('attStartOn')?.checked&&st)req.StartTime=new Date(st).toISOString();if(document.getElementById('attEndOn')?.checked&&et)req.EndTime=new Date(et).toISOString();
    const r=await api('POST','/api/Attendance/Search',req,30000);
    const data2=r.content??r.Data??{};
    const recs=data2.DataList??data2.Records??[];
    const hdr=['시간','동','호','멤버','사용자ID','이름','단말기SN','기록타입'];
    const rows=recs.map(x=>{
      const{dong,ho,member}=parseDongHoMember(x.UserID??'');
      return[x.RecordTime,dong,ho,member,x.UserID,x.UserName,x.DeviceSN,x.RecordType].join('\t');
    });
    const text=[hdr.join('\t'),...rows].join('\n');
    const blob=new Blob(['\uFEFF'+text],{type:'text/tab-separated-values;charset=utf-8'});
    const url=URL.createObjectURL(blob);const a=document.createElement('a');
    a.href=url;a.download='attendance_'+new Date().toISOString().slice(0,10)+'.tsv';a.click();URL.revokeObjectURL(url);
    setStatus(recs.length+'건 내보내기 완료','ok');
  }catch(e){setStatus('내보내기 실패: '+e.message,'err');}
}

/* ── 유틸 ── */
function esc(s){return String(s??'').replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');}
function rtLabel(t){const m={1:'카드',2:'얼굴',3:'지문',4:'카드+얼굴',5:'얼굴+얼굴',6:'카드+얼굴',7:'카드+비밀번호',8:'얼굴+비밀번호',9:'지문+비밀번호',10:'비밀번호',11:'카드+지문+비밀번호',12:'카드+얼굴+비밀번호',13:'지문+얼굴+비밀번호',14:'카드+지문+얼굴',15:'중복입력',16:'유효기간만료',17:'시간대만료',18:'구역진입불가',19:'이미등록',20:'강제개방',21:'입력횟수초과',22:'위협감지차단',23:'분실신고카드',24:'게스트카드',25:'원격문열기',26:'카드원격문열기',27:'얼굴원격문열기',28:'컨트롤러원격열기',29:'유효기간임박',30:'체온이상차단',31:'방문자비밀번호',32:'QR코드개방',33:'메뉴관리자추가',34:'메뉴관리자조회',35:'메뉴관리자삭제',36:'손바닥정맥',37:'카드+손바닥+얼굴',38:'손바닥+비밀번호',39:'카드+손바닥',40:'얼굴+손바닥',41:'카드+손바닥+비밀번호',42:'손바닥+얼굴+비밀번호',43:'지문+손바닥+얼굴',44:'원격문열기출입',45:'손바닥원격열기',46:'얼굴원격열기',47:'차단차단',48:'이동카드',49:'이동QR'};return m[t]??'(타입'+t+')';}
function rtBadge(t){if(t>=1&&t<=14||t===36||t>=37&&t<=43)return'badge-success';if(t===15||t===16||t===17||t===18||t===22||t===30||t===47)return'badge-danger';if(t===20||t===25||t===26||t===27||t===28||t===44||t===45||t===46)return'badge-info';return'badge-secondary';}
