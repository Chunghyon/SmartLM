unit Unit_Proxy;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmProxy = class(TForm)
    Label3: TLabel;
    lstProxy: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    lblMessage: TStaticText;
    txtProxy: TEdit;
    procedure FormShow(Sender: TObject);
    procedure DepartmentListInit();
    procedure DrawDepartmentList();
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure cmdExitClick(Sender: TObject);
    procedure lstProxyClick(Sender: TObject);
    procedure cmdUpdateClick(Sender: TObject);
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmProxy: TfrmProxy;

implementation

{$R *.dfm}

uses Unit_Main, Utils, SBXPCDLL_API;

const
    PROXY_COUNT : Integer = 20;
    
var
    mProxyList  : array[0..19] of WideString;
    
procedure TfrmProxy.FormShow(Sender: TObject);
begin
    DepartmentListInit();
    DrawDepartmentList();
end;

procedure TfrmProxy.DepartmentListInit();
var
    i : Integer;
begin
    for i := 0 to PROXY_COUNT - 1 do
    begin
        mProxyList[i] := '';
    end;
end;

procedure TfrmProxy.DrawDepartmentList();
var
    i       : Integer;
    itemStr : String;
begin
    lstProxy.Clear();
    for i := 0 to PROXY_COUNT - 1 do
    begin
        itemStr := '[No] ' + Format('%.02d', [i]) + ' ';
        itemStr := itemStr + '[Name] ' + mProxyList[i];

        lstProxy.Items.Add(itemStr);
    end;
end;

procedure TfrmProxy.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmProxy.cmdExitClick(Sender: TObject);
begin
    Close();
end;

procedure TfrmProxy.lstProxyClick(Sender: TObject);
var
    index : Integer;
begin
    index := lstProxy.ItemIndex;
    if index = -1 then Exit;

    txtProxy.Text := mProxyList[index];
    txtProxy.SetFocus();
end;

procedure TfrmProxy.cmdUpdateClick(Sender: TObject);
var
    index : Integer;
begin
    index := lstProxy.ItemIndex;
    if index = -1 then Exit;

    mProxyList[index] := txtProxy.Text;

    DrawDepartmentList();
end;

procedure TfrmProxy.cmdReadClick(Sender: TObject);
var
    vRet        : Boolean;
    vErrorCode  : Integer;
    i           : Integer;
    vDepartName : WideString;
    pch         : PWideChar;
begin
    lblMessage.Caption := 'Waiting...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    for i := 0 to PROXY_COUNT - 1 do
    begin
        vRet := _GetDepartName(gMachineNumber, i + 1, 1, vDepartName);
        if vRet then
          begin
              lblMessage.Caption := 'Success';
              pch := PWideChar(vDepartName);
              mProxyList[i] := pch;
          end
        else
          begin
              _GetLastError(gMachineNumber, vErrorCode);
              lblMessage.Caption := ErrorPrint(vErrorCode);
          end;
    end;
    DrawDepartmentList();
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmProxy.cmdWriteClick(Sender: TObject);
var
    vRet        : Boolean;
    vErrorCode  : Integer;
    i           : Integer;
begin
    lblMessage.Caption := 'Waiting...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    for i := 0 to PROXY_COUNT - 1 do
    begin
        vRet := _SetDepartName(gMachineNumber, i + 1, 1, mProxyList[i]);
        if vRet then
          begin
              lblMessage.Caption := 'Success';
          end
        else
          begin
              _GetLastError(gMachineNumber, vErrorCode);
              lblMessage.Caption := ErrorPrint(vErrorCode);
          end;
    end;
    _EnableDevice(gMachineNumber, True);
end;

end.

