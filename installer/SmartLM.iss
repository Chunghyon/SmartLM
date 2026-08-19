#define MyAppName "SmartLM"
#define MyAppVersion "1.0.0"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={autopf}\SmartLM
DefaultGroupName=SmartLM
OutputDir=output
OutputBaseFilename=SmartLM_Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin

[Files]
Source: "dist\FDHPS\*"; DestDir: "{app}\FDHPS"; Flags: ignoreversion recursesubdirs
Source: "dist\FDDC\*";  DestDir: "{app}\FDDC";  Flags: ignoreversion recursesubdirs
Source: "FaceDeviceSettings.xml"; DestDir: "{commonappdata}\SmartLM"; Flags: onlyifdoesntexist

[Icons]
Name: "{group}\Face Device HTTP Server"; Filename: "{app}\FDHPS\FaceDeviceHttpPcServer.exe"
Name: "{group}\Face Device Desktop Client"; Filename: "{app}\FDDC\FaceDeviceDesktopClient.exe"
Name: "{autodesktop}\FDHPS"; Filename: "{app}\FDHPS\FaceDeviceHttpPcServer.exe"
Name: "{autodesktop}\FDDC";  Filename: "{app}\FDDC\FaceDeviceDesktopClient.exe"

[Run]
Filename: "{app}\FDHPS\FaceDeviceHttpPcServer.exe"; Description: "서버 실행"; Flags: nowait postinstall skipifsilent
Filename: "{app}\FDDC\FaceDeviceDesktopClient.exe"; Description: "클라이언트 실행"; Flags: nowait postinstall skipifsilent