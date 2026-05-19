program Multithreading;

uses
  Forms,
  Unit_Main in 'Unit_Main.pas' {frmMain},
  Unit_Device in 'Unit_Device.pas',
  SBXPCDLL_API in 'SBXPCDLL_API.PAS';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrmMain, frmMain);
  Application.Run;
end.
