# Korean translation script
$file = "FaceDeviceDesktopClient\MainForm.cs"
$content = [System.IO.File]::ReadAllText($file, [System.Text.Encoding]::UTF8)

# Search dialog
$content = $content -replace '"Select Search Method"', '"검색 방법 선택"'
$content = $content -replace '"Broadcast Search \(UDP Discovery\)"', '"브로드캐스트 검색 (UDP)"'
$content = $content -replace '"Network Scan \(HTTP Probe\)"', '"네트워크 스캔 (HTTP)"'
$content = $content -replace '"Subnet:"', '"서브넷:"'
$content = $content -replace '"Start Search"', '"검색 시작"'
$content = $content -replace '"Network Scan Progress"', '"네트워크 스캔 진행 중"'
$content = $content -replace '"Scanning..."', '"스캔 중..."'

# Status messages
$content = $content -replace '"Loading devices..."', '"단말기 로딩 중..."'
$content = $content -replace '"No devices installed yet"', '"설치된 단말기가 없습니다"'

# Comment out department references
$content = $content -replace 'lblTotalDepartments\.Text', '// lblTotalDepartments.Text'
$content = $content -replace 'await RefreshDepartments\(\);', '// await RefreshDepartments();'

[System.IO.File]::WriteAllText($file, $content, [System.Text.Encoding]::UTF8)
Write-Host "MainForm.cs updated successfully"
