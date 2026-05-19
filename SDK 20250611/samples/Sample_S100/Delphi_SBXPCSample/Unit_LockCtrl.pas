unit Unit_LockCtrl;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmLockCtrl = class(TForm)
    lblMessage: TStaticText;
    cmdGetDoorStatus: TButton;
    cmdDoorOpen: TButton;
    cmdUncondOpen: TButton;
    cmdAutoRecover: TButton;
    cmdUncondClose: TButton;
    cmdRestart: TButton;
    cmdWarnCancel: TButton;
    procedure FormShow(Sender: TObject);
    procedure cmdGetDoorStatusClick(Sender: TObject);
    procedure cmdDoorOpenClick(Sender: TObject);
    procedure cmdAutoRecoverClick(Sender: TObject);
    procedure cmdUncondOpenClick(Sender: TObject);
    procedure cmdUncondCloseClick(Sender: TObject);
    procedure cmdRestartClick(Sender: TObject);
    procedure cmdWarnCancelClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmLockCtrl: TfrmLockCtrl;

implementation

uses Unit_Main, Utils, SBXPCLib_TLB;

var bpc     :TSBXPC;
{$R *.dfm}


procedure TfrmLockCtrl.FormShow(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
end;

procedure TfrmLockCtrl.cmdGetDoorStatusClick(Sender: TObject);
var
    vRet        :Boolean;
    vValue      :Integer;
    vErrorCode  :Integer;
begin
    vValue := 0;
    vErrorCode := 0;

    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.GetDoorStatus(gMachineNumber, vValue);
    if vRet then
    begin
        case vValue of
            1:lblMessage.Caption := 'Uncond Door Open State!';
            2:lblMessage.Caption := 'Uncond Door Close State!';
            3:lblMessage.Caption := 'Door Open State!';
            4:lblMessage.Caption := 'Auto Recover State!';
            5:lblMessage.Caption := 'Door Close State!';
            6:lblMessage.Caption := 'Watching for Close!';
            7:lblMessage.Caption := 'Illegal open!';
            else lblMessage.Caption := 'User State !';
        end;
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdDoorOpenClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.SetDoorStatus(gMachineNumber, 3);
    if vRet then lblMessage.Caption := 'Door Open Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdAutoRecoverClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.SetDoorStatus(gMachineNumber, 4);
    if vRet then lblMessage.Caption := 'Auto Recover Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdUncondOpenClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.SetDoorStatus(gMachineNumber, 1);
    if vRet then lblMessage.Caption := 'Uncond Door Open Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdUncondCloseClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.SetDoorStatus(gMachineNumber, 2);
    if vRet then lblMessage.Caption := 'Uncond Door Close Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdRestartClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.SetDoorStatus(gMachineNumber, 5);
    if vRet then lblMessage.Caption := 'Reboot Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdWarnCancelClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.SetDoorStatus(gMachineNumber, 6);
    if vRet then lblMessage.Caption := 'Warning cancel Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

end.
