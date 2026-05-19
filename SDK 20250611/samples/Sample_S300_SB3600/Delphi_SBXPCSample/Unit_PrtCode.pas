unit Unit_PrtCode;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmPrtCode = class(TForm)
    lblMessage: TStaticText;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    txtSerialNo: TEdit;
    txtBackupNo: TEdit;
    txtProductCode: TEdit;
    cmdGetSerialNumber: TButton;
    cmdGetBackupNumber: TButton;
    cmdGetProductCode: TButton;
    cmdExit: TButton;
    procedure cmdGetSerialNumberClick(Sender: TObject);
    procedure cmdGetBackupNumberClick(Sender: TObject);
    procedure cmdGetProductCodeClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmPrtCode: TfrmPrtCode;

implementation

uses Unit_Main, SBXPCLib_TLB, Utils;

var bpc     :TSBXPC;
{$R *.dfm}

procedure TfrmPrtCode.cmdGetSerialNumberClick(Sender: TObject);
var
    vRet            :Boolean;
    vErrorCode      :Integer;
    vSerialNumber   :WideString;
begin
    vErrorCode := 0;
    vSerialNumber := '';

    txtSerialNo.Text := '';
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.GetSerialNumber(gMachineNumber, vSerialNumber);
    if vRet then
    begin
        txtSerialNo.Text := vSerialNumber;
        lblMessage.Caption := 'Success!';
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmPrtCode.cmdGetBackupNumberClick(Sender: TObject);
var
    vRet            :Boolean;
    vErrorCode      :Integer;
    vBackupNumber   :Integer;
begin
    vErrorCode := 0;

    txtBackupNo.Text := '';
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vBackupNumber := bpc.GetBackupNumber(gMachineNumber);
    if vBackupNumber <> 0 then
    begin
        txtBackupNo.Text := IntToStr(vBackupNumber);
        lblMessage.Caption := 'Success!';
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmPrtCode.cmdGetProductCodeClick(Sender: TObject);
var
    vRet            :Boolean;
    vErrorCode      :Integer;
    vProductCode    :WideString;
begin
    vErrorCode := 0;
    vProductCode := '';

    txtProductCode.Text := '';
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.GetProductCode(gMachineNumber, vProductCode);
    if vRet then
    begin
        txtProductCode.Text := vProductCode;
        lblMessage.Caption := 'Success!';
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmPrtCode.cmdExitClick(Sender: TObject);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    Close;
end;

procedure TfrmPrtCode.FormShow(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
end;

procedure TfrmPrtCode.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

end.
