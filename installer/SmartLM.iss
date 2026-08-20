#define MyAppVersion "1.0.0"
#define MyAppName "SmartLM"
#define MyAppPublisher "Illitek Co., Ltd."

[Setup]
AppPublisher={#MyAppPublisher}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={autopf}\SmartLM
DefaultGroupName=SmartLM
OutputDir=output
OutputBaseFilename=SmartLM_Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=none
MinVersion=10.0
DisableProgramGroupPage=yes

[Dirs]
Name: "{userdocs}\SmartLM"
Name: "{userdocs}\SmartLM\App_Data"

[Files]
Source: "dist\FDHS\*"; DestDir: "{app}\FDHS"; Flags: ignoreversion recursesubdirs
Source: "dist\FDDC\*";  DestDir: "{app}\FDDC";  Flags: ignoreversion recursesubdirs
Source: "FaceDeviceSettings.xml"; DestDir: "{userdocs}\SmartLM"; Flags: onlyifdoesntexist
Source: "appsettings.installed.json"; DestDir: "{app}\FDHS"; DestName: "appsettings.json"; Flags: ignoreversion

[Icons]
Name: "{group}\Face Device HTTP Server"; Filename: "{app}\FDHS\FaceDeviceHttpServer.exe"; WorkingDir: "{app}\FDHS"
Name: "{group}\Face Device Desktop Client"; Filename: "{app}\FDDC\FaceDeviceDesktopClient.exe"; WorkingDir: "{app}\FDDC"
Name: "{autodesktop}\FDHS"; Filename: "{app}\FDHS\FaceDeviceHttpServer.exe"; WorkingDir: "{app}\FDHS"
Name: "{autodesktop}\FDDC"; Filename: "{app}\FDDC\FaceDeviceDesktopClient.exe"; WorkingDir: "{app}\FDDC"

[Run]
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""SmartLM FDHS HTTP"" dir=in action=allow protocol=TCP localport=80"; Flags: runhidden
Filename: "{app}\FDHS\FaceDeviceHttpServer.exe"; Description: "FDHS 실행"; WorkingDir: "{app}\FDHS"; Flags: nowait postinstall skipifsilent
Filename: "{app}\FDDC\FaceDeviceDesktopClient.exe"; Description: "FDDC 실행"; WorkingDir: "{app}\FDDC"; Flags: nowait postinstall skipifsilent unchecked
