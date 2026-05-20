[Setup]
AppName=Access Control System
AppVersion=9.30
DefaultDirName={autopf}\Access Control System
DefaultGroupName=Access Control System
OutputBaseFilename=AccessControlSetup_Korean
; 바탕화면 단축키 생성 여부를 사용자에게 묻는 체크박스
UsePreviousAppDir=yes
UsePreviousGroup=yes
AllowNoIcons=yes

[Files]
; 전체 설치 파일 (Program Files 원본)
Source: "C:\Program Files (x86)\Access Control System\*"; DestDir: "{app}"; Flags: recursesubdirs

; 한글 언어 파일 (Inno_Setup 폴더의 수정본으로 덮어쓰기)
Source: "D:\Documents\Smart_LM_China\Inno_Setup\Korean.XML"; DestDir: "{app}"; Flags: overwritereadonly
Source: "D:\Documents\Smart_LM_China\Inno_Setup\SoftWareInfo_Korean.XML"; DestDir: "{app}"; Flags: overwritereadonly
Source: "D:\Documents\Smart_LM_China\Inno_Setup\System.XML"; DestDir: "{app}"; Flags: overwritereadonly

[Tasks]
; 바탕화면 단축키 생성 여부를 사용자에게 묻는 옵션
Name: "desktopicon"; Description: "바탕화면에 단축 아이콘 만들기"; GroupDescription: "추가 아이콘:"; Flags: unchecked

[Icons]
; 시작 메뉴 프로그램 그룹
Name: "{group}\출입통제 시스템";         Filename: "{app}\Card3500.exe"
Name: "{group}\IC카드 편집기";            Filename: "{app}\ICCard_Editer.exe"
Name: "{group}\지문 리더기 드라이버";     Filename: "{app}\Fingerprint_Reader_Drive_2.1.2.exe"
Name: "{group}\USB 리더기 드라이버";      Filename: "{app}\USB Reader Drive.EXE"
Name: "{group}\프로그램 제거";            Filename: "{uninstallexe}"

; 바탕화면 단축키 (사용자가 선택한 경우에만 생성)
Name: "{autodesktop}\출입통제 시스템";    Filename: "{app}\Card3500.exe"; Tasks: desktopicon

[Run]
; Control 폴더 OCX 등록
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleButton.ocx"""; StatusMsg: "OCX 등록 중..."; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleCheckBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleComboBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleFrame.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleHSBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleHyperlink.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleListBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleOptionButton.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleProcessBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleSlider.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleSubtitling.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleTextBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleTooltip.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleTrackBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\AnoleVSBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\FlexCell.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\RMListView.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\vbalIml6.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\Vsflex7N.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Control\VsMenu64.ocx"""; Flags: runhidden
; dll 폴더 OCX 등록
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\dll\AnoleSlider.ocx"""; Flags: runhidden
; IPC 폴더 OCX 등록
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\IPC\IPCamConfig.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\IPC\IPCStream.ocx"""; Flags: runhidden
; Package 폴더 OCX 등록 (Microsoft 공용 컨트롤)
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\aResCtl.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\MSCOMCT2.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\MSCOMCTL.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\MSCOMM32.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\MSINET.Ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\msmask32.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\MSWINSCK.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\richtx32.Ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s ""{app}\Setup\Software\Package\TABCTL32.OCX"""; Flags: runhidden

[UninstallRun]
; 언인스톨 시 OCX 등록 해제
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleButton.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleCheckBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleComboBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleFrame.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleHSBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleHyperlink.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleListBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleOptionButton.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleProcessBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleSlider.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleSubtitling.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleTextBox.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleTooltip.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleTrackBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\AnoleVSBar.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\FlexCell.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\RMListView.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\vbalIml6.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\Vsflex7N.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Control\VsMenu64.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\dll\AnoleSlider.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\IPC\IPCamConfig.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\IPC\IPCStream.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\aResCtl.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\MSCOMCT2.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\MSCOMCTL.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\MSCOMM32.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\MSINET.Ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\msmask32.ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\MSWINSCK.OCX"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\richtx32.Ocx"""; Flags: runhidden
Filename: "{sys}\regsvr32.exe"; Parameters: "/s /u ""{app}\Setup\Software\Package\TABCTL32.OCX"""; Flags: runhidden