unit Unit_Department;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmDepartment = class(TForm)
    Label3: TLabel;
    lstDepartment: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    lblMessage: TStaticText;
    txtDepartment: TEdit;
    procedure FormShow(Sender: TObject);
    procedure DepartmentListInit();
    procedure DrawDepartmentList();
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure cmdExitClick(Sender: TObject);
    procedure lstDepartmentClick(Sender: TObject);
    procedure cmdUpdateClick(Sender: TObject);
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmDepartment: TfrmDepartment;

implementation

{$R *.dfm}

uses Unit_Main, Utils, SBXPCDLL_API;

const
    DEPT_COUNT : Integer = 20;
    
var
    mDepartmentList : array[0..19] of WideString;
    
procedure TfrmDepartment.FormShow(Sender: TObject);
begin
    DepartmentListInit();
    DrawDepartmentList();
end;

procedure TfrmDepartment.DepartmentListInit();
var
    i : Integer;
begin
    for i := 0 to DEPT_COUNT - 1 do
    begin
        mDepartmentList[i] := '';
    end;
end;

procedure TfrmDepartment.DrawDepartmentList();
var
    i       : Integer;
    itemStr : String;
begin
    lstDepartment.Clear();
    for i := 0 to DEPT_COUNT - 1 do
    begin
        itemStr := '[No] ' + Format('%.02d', [i]) + ' ';
        itemStr := itemStr + '[Name] ' + mDepartmentList[i];

        lstDepartment.Items.Add(itemStr);
    end;
end;

procedure TfrmDepartment.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmDepartment.cmdExitClick(Sender: TObject);
begin
    Close();
end;

procedure TfrmDepartment.lstDepartmentClick(Sender: TObject);
var
    index : Integer;
begin
    index := lstDepartment.ItemIndex;
    if index = -1 then Exit;

    txtDepartment.Text := mDepartmentList[index];
    txtDepartment.SetFocus();
end;

procedure TfrmDepartment.cmdUpdateClick(Sender: TObject);
var
    index : Integer;
begin
    index := lstDepartment.ItemIndex;
    if index = -1 then Exit;

    mDepartmentList[index] := txtDepartment.Text;

    DrawDepartmentList();
end;

procedure TfrmDepartment.cmdReadClick(Sender: TObject);
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

    for i := 0 to DEPT_COUNT - 1 do
    begin
        vRet := _GetDepartName(gMachineNumber, i, 0, vDepartName);
        if vRet then
          begin
              lblMessage.Caption := 'Success';
              pch := PWideChar(vDepartName);
              mDepartmentList[i] := pch;
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

procedure TfrmDepartment.cmdWriteClick(Sender: TObject);
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

    for i := 0 to DEPT_COUNT - 1 do
    begin
        vRet := _SetDepartName(gMachineNumber, i, 0, mDepartmentList[i]);
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
