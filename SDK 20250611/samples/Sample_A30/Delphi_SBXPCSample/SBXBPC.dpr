program SBXPC;

uses
  Forms,
  Unit_Main in 'Unit_Main.pas' {frmMain},
  Unit_BellInfo in 'Unit_BellInfo.pas' {frmBellInfo},
  Unit_LockCtrl in 'Unit_LockCtrl.pas' {frmLockCtrl},
  Unit_Log in 'Unit_Log.pas' {frmLog},
  Unit_PrtCode in 'Unit_PrtCode.pas' {frmPrtCode},
  Unit_SysInfo in 'Unit_SysInfo.pas' {frmSystemInfo},
  Unit_Enroll in 'Unit_Enroll.pas' {frmEnroll},
  Utils in 'Utils.pas',
  Unit_TrMode in 'Unit_TrMode.pas' {frmTrMode},
  Unit_AccessTz in 'Unit_AccessTz.pas' {frmAccessTz},
  Unit_EventCapture in 'Unit_EventCapture.pas' {frmEventCapture},
  Unit_Holiday in 'Unit_Holiday.pas' {frmHoliday},
  Unit_Tmode in 'Unit_Tmode.pas' {frmTMode};

{$R *.res}

begin
  Application.Initialize;
  Application.Title := 'Smackbio OCX Sample';
  Application.CreateForm(TfrmMain, frmMain);
  Application.CreateForm(TfrmBellInfo, frmBellInfo);
  Application.CreateForm(TfrmLockCtrl, frmLockCtrl);
  Application.CreateForm(TfrmLog, frmLog);
  Application.CreateForm(TfrmPrtCode, frmPrtCode);
  Application.CreateForm(TfrmSystemInfo, frmSystemInfo);
  Application.CreateForm(TfrmEnroll, frmEnroll);
  Application.CreateForm(TfrmTrMode, frmTrMode);
  Application.CreateForm(TfrmAccessTz, frmAccessTz);
  Application.CreateForm(TfrmEventCapture, frmEventCapture);
  Application.CreateForm(TfrmHoliday, frmHoliday);
  Application.CreateForm(TfrmTMode, frmTMode);
  Application.Run;
end.
