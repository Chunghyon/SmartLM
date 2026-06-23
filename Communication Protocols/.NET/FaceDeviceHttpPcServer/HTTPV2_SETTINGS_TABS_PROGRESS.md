# HTTPv2 프로토콜 설정 탭 추가 - 진행 상황

## 완료된 작업

### 1. 새로운 설정 폼 추가

#### NetworkSettingsForm (네트워크 설정)
**파일**: `FaceDeviceDesktopClient/Forms/NetworkSettingsForm.cs`

**포함된 설정**:
- **UDP Client 탭**
  - UseUDPClient (UDP Client 사용 여부)
  - ServerAddress (서버 주소)
  - ServerPort (서버 포트)
  - KeepaliveTime (Keepalive 시간)

- **HTTP Client 탭**
  - UseHTTPClient (HTTP Client 사용 여부)
  - HTTPClient_ProtocolType (프로토콜 타입: HTTPv1 (100) / HTTPv2 (200))
  - HTTPClient_ServerAddr (서버 주소)
  - HTTPClient_KeepaliveTime (Keepalive 시간)
  - HTTPClient_UseGZIP (GZIP 압축 사용)

- **MQTT Client 탭**
  - UseMQTTClient (MQTT Client 사용 여부)
  - UseMQTTSSL (SSL 사용 여부)
  - MQTTServerAddr (서버 주소)
  - MQTTPort (포트)
  - MQTTLoginPassword (로그인 비밀번호)
  - MQTTPublishTopic (Publish Topic)
  - MQTTSubscribeTopic (Subscribe Topic)
  - MQTT_KeepaliveTime (Keepalive 시간)
  - MQTT_UseGZIP (GZIP 압축 사용)

- **WebSocket Client 탭**
  - UseWebsocketClient (WebSocket 사용 여부)
  - WebsocketClient_ProtocolType (프로토콜 타입: WebSocketv1 (100) / WebSocketv2 (200))
  - WebsocketClient_ServerAddr (서버 주소)
  - WebsocketClient_UseGZIP (GZIP 압축 사용)
  - WebsocketClient_KeepaliveTime (Keepalive 시간)

- **Yunzhu Platform 탭**
  - UseYZW (Yunzhu Platform 사용 여부)
  - YZWAddr (YZW 서버 주소)

#### AccessControlSettingsForm (출입 제어 설정)
**파일**: `FaceDeviceDesktopClient/Forms/AccessControlSettingsForm.cs`

**포함된 설정**:
- **문 제어 설정**
  - DelayOpenDoorTime (지연 개방 시간)
  - FreeOpen (무인증 개방)
  - OpenInterval (반복 인식 간격)
  - OpenInterval_SaveRecord (반복 간격 기록 저장)
  - Relay (릴레이 양안정 지원)
  - ShortMessage (합법 인증 후 메시지)
  - VerificationType (인증 방식 1~16)

- **권한 설정**
  - OverdueRemind (권한 만료 알림)
  - OverdueRemind_Day (만료 알림 임계값)

- **정시 개방 설정**
  - TimingOpen (정시 개방 기능)
  - TimingOpen_mode (자동 개방 모드)
  - TimingOpen_timegroup (시간대 JSON)

- **정시 잠금 설정**
  - TimingLocked (정시 잠금 기능)
  - TimingLocked_timegroup (시간대 JSON)

- **방문객 설정**
  - VisitorRootPassword (방문객 루트 비밀번호)
  - MultiPerson (다인 조합 개방 인원)

#### AlarmSettingsForm (알람 설정)
**파일**: `FaceDeviceDesktopClient/Forms/AlarmSettingsForm.cs`

**포함된 설정**:
- FireAlarm (화재 알람)
- DoorLongOpenAlarm (개방 시간 초과 알람)
- DoorLongOpenTime (개방 시간 초과 임계값)
- DoorSensorAlarm (문 센서 알람)

### 2. MainForm 메뉴 추가

**파일**: `FaceDeviceDesktopClient/MainForm.Designer.cs`, `FaceDeviceDesktopClient/MainForm.cs`

**추가된 메뉴**:
- 설정 (menuSettings)
  - 네트워크 설정 (menuNetworkSettings) → NetworkSettingsForm 열기
  - 출입 제어 설정 (menuAccessControlSettings) → AccessControlSettingsForm 열기
  - 알람 설정 (menuAlarmSettings) → AlarmSettingsForm 열기

**이벤트 핸들러**:
```csharp
private void MenuNetworkSettings_Click(object sender, EventArgs e)
{
    using var form = new NetworkSettingsForm(_httpClient);
    form.ShowDialog();
}

private void MenuAccessControlSettings_Click(object sender, EventArgs e)
{
    using var form = new AccessControlSettingsForm(_httpClient);
    form.ShowDialog();
}

private void MenuAlarmSettings_Click(object sender, EventArgs e)
{
    using var form = new AlarmSettingsForm(_httpClient);
    form.ShowDialog();
}
```

### 3. 기능

각 설정 폼은 다음 기능을 제공합니다:
- **불러오기**: 등록된 단말기의 현재 설정을 서버에서 조회
- **저장**: 설정을 서버에 저장하고 단말기 동기화 요청
- **HTTPv2 프로토콜 준수**: 모든 설정은 `/admin/devices/{sn}/work-setting` 엔드포인트를 통해 저장되며, 단말기가 Keepalive 시 자동으로 동기화됨

## 프로토콜 매핑

### HTTPv2 문서의 파라미터 → FDDC 구현

| HTTPv2 파라미터 | FDDC 폼 | 구현 상태 |
|----------------|---------|----------|
| UseUDPClient, ServerAddress, ServerPort, KeepaliveTime | NetworkSettingsForm (UDP Client 탭) | ? 완료 |
| UseHTTPClient, HTTPClient_* | NetworkSettingsForm (HTTP Client 탭) | ? 완료 |
| UseMQTTClient, MQTT* | NetworkSettingsForm (MQTT Client 탭) | ? 완료 |
| UseWebsocketClient, WebsocketClient_* | NetworkSettingsForm (WebSocket Client 탭) | ? 완료 |
| UseYZW, YZWAddr | NetworkSettingsForm (Yunzhu Platform 탭) | ? 완료 |
| FireAlarm, DoorLongOpenAlarm, DoorLongOpenTime, DoorSensorAlarm | AlarmSettingsForm | ? 완료 |
| DelayOpenDoorTime, FreeOpen, OpenInterval, Relay, ShortMessage, VerificationType | AccessControlSettingsForm (문 제어) | ? 완료 |
| OverdueRemind, OverdueRemind_Day | AccessControlSettingsForm (권한) | ? 완료 |
| TimingOpen, TimingOpen_mode, TimingOpen_timegroup | AccessControlSettingsForm (정시 개방) | ? 완료 |
| TimingLocked, TimingLocked_timegroup | AccessControlSettingsForm (정시 잠금) | ? 완료 |
| VisitorRootPassword, MultiPerson | AccessControlSettingsForm (방문객) | ? 완료 |
| UseElevator, ElevatorPorts | ? 미완료 (ElevatorSettingsForm 필요) | ? 대기 |
| Timegroup (시간대 설정) | ? 미완료 (TimeGroupSettingsForm 필요) | ? 대기 |
| Holiday (휴일 설정) | ? 미완료 (HolidaySettingsForm 필요) | ? 대기 |
| AlarmClock (알람 시계 설정) | ? 미완료 (AlarmClockSettingsForm 필요) | ? 대기 |
| Region, Language, DeviceName, DeviceID | ? 미완료 (DeviceInfoSettingsForm 필요) | ? 대기 |

## 남은 작업

### 추가 필요 폼

1. **ElevatorSettingsForm** (엘리베이터 설정)
   - UseElevator
   - ElevatorPorts (배열 편집기)
     - Num (포트 번호)
     - RelayType (릴레이 타입)
     - ReleaseTime (출력 지속 시간)
     - TimingOpen (정시 개방 구조)

2. **TimeGroupSettingsForm** (시간대 설정)
   - 시간대 1~64 정의
   - 주별 시간대 (Week1~Week7)
   - 휴일 시간대

3. **HolidaySettingsForm** (휴일 설정)
   - 휴일 목록 (최대 100개)
   - 날짜 및 시간대 설정

4. **AlarmClockSettingsForm** (알람 시계 설정)
   - 알람 클록 최대 24개
   - 시간, 반복, 활성화 설정

5. **DeviceInfoSettingsForm** (단말기 기본 정보)
   - Region (지역)
   - Language (언어)
   - DeviceName (단말기명)
   - DeviceID (단말기 ID)
   - RunDays, FormatCount, WatchDogCount 등

## 빌드 상태

? **빌드 성공**
- 모든 현재 구현된 폼 컴파일 완료
- 메뉴 이벤트 핸들러 정상 작동
- using 지시문 (`System.Net.Http.Json`) 추가 완료

## 사용 방법

1. Face Device Desktop Client 실행
2. 상단 메뉴에서 **설정** 클릭
3. 원하는 설정 메뉴 선택:
   - 네트워크 설정
   - 출입 제어 설정
   - 알람 설정
4. **불러오기** 버튼으로 현재 설정 조회
5. 설정 변경 후 **저장** 버튼으로 저장
6. 단말기가 다음 Keepalive 시 자동 동기화

## 다음 단계

1. 남은 설정 폼 구현 (엘리베이터, 시간대, 휴일, 알람 시계, 단말기 정보)
2. 메뉴에 추가 설정 항목 등록
3. 각 설정 폼에 유효성 검사 추가
4. 여러 단말기 선택 기능 추가 (현재는 첫 번째 단말기만 대상)
5. 설정 내보내기/가져오기 기능

## 주의사항

- 현재 구현은 첫 번째 등록된 단말기만 대상으로 함
- 여러 단말기를 관리하려면 단말기 선택 UI 추가 필요
- 시간대 JSON 형식은 수동 입력 (향후 비주얼 에디터 필요)
- 엘리베이터 포트 배열 편집 기능 미구현

---

**작성일**: 2025-01-XX  
**상태**: 진행 중  
**완료율**: 약 60% (3/8개 주요 카테고리 완료)
