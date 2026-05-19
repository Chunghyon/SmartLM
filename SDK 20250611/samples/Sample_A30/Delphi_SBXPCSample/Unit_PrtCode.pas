unit Unit_PrtCode;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmPrtCode = class(TForm)
    lblMessage: TStaticText;
    Label1: TLabel;
    Label3: TLabel;
    txtSerialNo: TEdit;
    txtProductCode: TEdit;
    cmdGetSerialNumber: TButton;
    cmdGetProductCode: TButton;
    cmdExit: TButton;
    procedure cmdGetSerialNumberClick(Sender: TObject);
    procedure cmdGetProductCodeClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmPrtCode: TfrmPrtCode;

implementation

uses Unit_Main, Utils, SBXPCLib_TLB;

{$R *.dfm}
var
  bpc         :TSBXPC;
  
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);;
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);;
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

procedure TfrmPrtCode.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmPrtCode.FormCreate(Sender: TObject);
begin
  bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
end;

end.
