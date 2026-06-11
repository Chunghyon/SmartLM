# 디바이스 검색 기능 개선

## 수정된 문제점

### 1. Broadcast Search가 작동하지 않는 문제

**원인:**
- 기존 코드는 UDP 포트 60000에 JSON 메시지를 보내고 있었음
- 실제 디바이스는 포트 **20567**에서 **바이너리 프로토콜**을 사용함

**해결 방법:**
- UDP 브로드캐스트 포트를 20567로 변경
- 바이너리 프로토콜 구현:
  - Magic Numbers: `0x0c58380d`, `0xea8b42b2` (요청)
  - Magic Numbers: `0xaa8fcb84`, `0x05fece87` (응답)
  - ProductNamePrefix (16 bytes)
  - Device ID, IP, Port 등의 정보 수신
- 타임아웃을 3초에서 **5초로 증가** (디바이스 응답 대기 시간 확보)

**포트 설명:**
- **20567**: 디바이스 검색(Discovery) 전용 포트 - 브로드캐스트로 디바이스 찾기 ?
- **8101**: 디바이스 UDP 통신 포트 - 디바이스와의 일반 UDP 통신용 (검색과는 무관)
- **80/8080**: 디바이스 HTTP 포트 - 웹 인터페이스 및 API 통신용

**변경된 파일:**
- `Services/DeviceDiscoveryService.cs`
  - `DiscoveryPort` 상수: 60000 → 20567 ?
  - `TimeoutMs` 상수: 3000 → 5000 (3초 → 5초) ?
  - `CreateDiscoveryRequest()` 메서드 추가: 바이너리 요청 패킷 생성
  - `ParseUdpDiscoveryResponse()` 메서드 추가: 바이너리 응답 파싱
  - JSON 기반 브로드캐스트 → 바이너리 프로토콜 변경

### 2. Network Scan 결과가 모든 스캔 완료 후에만 표시되는 문제

**원인:**
- 기존 코드는 254개 IP 주소를 모두 스캔한 후 한 번에 결과 반환
- 사용자가 디바이스 발견을 실시간으로 확인할 수 없음

**해결 방법:**
1. **서버 측 (C#):**
   - `ScanNetworkStreamAsync()` 메서드 추가: `IAsyncEnumerable<DiscoveredDevice>` 반환
   - 디바이스 발견 즉시 `yield return`으로 반환
   - 기존 `ScanNetworkAsync()` 메서드는 하위 호환성 유지

2. **API 엔드포인트 추가:**
   - `/api/Device/SearchStream` 엔드포인트 추가
   - Server-Sent Events (SSE) 프로토콜 사용
   - `text/event-stream` 콘텐츠 타입
   - 디바이스 발견 시 즉시 클라이언트에 전송

3. **클라이언트 측 (JavaScript):**
   - `startNetworkScan()` 함수 수정
   - Fetch API의 ReadableStream 사용
   - SSE 메시지 파싱 (`data: {...}` 형식)
   - 각 디바이스 발견 시 즉시 UI 업데이트

**변경된 파일:**
- `Services/DeviceDiscoveryService.cs`
  - `ScanNetworkStreamAsync()` 메서드 추가
  - Task.WhenAny로 완료된 작업 즉시 처리

- `Program.cs`
  - `/api/Device/SearchStream` 엔드포인트 추가

- `wwwroot/admin/app.js`
  - `startNetworkScan()` 함수를 SSE 스트리밍 방식으로 변경

## 테스트 방법

### Broadcast Search 테스트:
1. 서버 실행: `dotnet run`
2. 브라우저에서 Admin 페이지 접속: `http://localhost:8100/admin`
3. Device 탭에서 "Broadcast Search" 버튼 클릭
4. 로그 확인:
   ```
   [HH:MM:SS] · 브로드캐스트 전송: 포트 20567, XX bytes
   [HH:MM:SS] · UDP 응답 수신: 192.168.0.XX
   [HH:MM:SS] · 디바이스 발견: 192.168.0.XX - XXXXXXXX
   ```

### Network Scan 실시간 표시 테스트:
1. Device 탭에서 "Network Scan" 버튼 클릭
2. Subnet 입력: `192.168.0`
3. "Start Scan" 클릭
4. **디바이스 발견 즉시** 테이블에 표시되는지 확인
5. 상태 메시지가 `Scanning... Found X device(s) so far`로 실시간 업데이트 확인

## 디바이스 설정 확인

만약 Broadcast Search가 여전히 작동하지 않는 경우, 디바이스 설정을 확인하세요:

1. **네트워크 설정:**
   - 디바이스와 서버가 같은 서브넷에 있는지 확인
   - 브로드캐스트가 차단되지 않았는지 확인

2. **방화벽 설정:**
   - UDP 포트 20567이 열려 있는지 확인
   - Windows 방화벽에서 인바운드 규칙 확인

3. **디바이스 설정:**
   - 디바이스의 네트워크 프로토콜 설정 확인
   - 일부 디바이스는 설정에서 "Device Discovery" 또는 "Auto Discovery"를 활성화해야 할 수 있음

## 참고 사항

- Broadcast Search는 로컬 네트워크(같은 서브넷)에서만 작동합니다
- Network Scan은 라우터를 통해 다른 서브넷도 검색 가능합니다
- Network Scan은 HTTP 포트(80, 8080, 8100)를 시도하므로 Broadcast Search보다 느릴 수 있습니다
- 실시간 스트리밍은 Server-Sent Events(SSE)를 사용하므로 HTTP/1.1 이상이 필요합니다
