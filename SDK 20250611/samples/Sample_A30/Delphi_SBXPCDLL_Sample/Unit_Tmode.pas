unit Unit_Tmode;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls;

type
  TfrmTMode = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    lstData: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    cmbTMode: TComboBox;
    dtStart: TDateTimePicker;
    dtEnd: TDateTimePicker;
    lblMessage: TStaticText;
    procedure DbTmodeInit();
    procedure DbTmodeDraw();
    procedure cmdUpdateClick(Sender: TObject);
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure FormCreate(Sender: TObject);
    procedure lstDataClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmTMode: TfrmTMode;

implementation

uses Unit_Main, Utils, SBXPCDLL_API;

{$R *.dfm}

const
    DB_TMODE_MAX = 10;
var
    DbTmodeArray : array[0 .. 10 * 5] of Integer;

procedure TfrmTMode.DbTmodeInit();
var
  i : Integer;
begin
  for i := 0 to DB_TMODE_MAX - 1 do
  begin
    DbTmodeArray[i * 5 + 1] := 0;
    DbTmodeArray[i * 5 + 2] := 0;
    DbTmodeArray[i * 5 + 3] := 23;
    DbTmodeArray[i * 5 + 4] := 59;
    DbTmodeArray[i * 5 + 0] := 0;
  end;
end;

procedure TfrmTMode.DbTmodeDraw();
var
  i : Integer;
  itemStr : String;
begin
  lstData.Items.Clear;
  for i := 0 to DB_TMODE_MAX - 1 do
  begin
     itemStr := '[No] ' + Format('%.2d', [i + 1]) + ' ';
     itemStr := itemStr + '[S] ' + Format('%.2d', [DbTmodeArray[i * 5 + 1]]);
     itemStr := itemStr + ':'    + Format('%.2d', [DbTmodeArray[i * 5 + 2]]) + ' ';
     itemStr := itemStr + '[E] ' + Format('%.2d', [DbTmodeArray[i * 5 + 3]]);
     itemStr := itemStr + ':'    + Format('%.2d', [DbTmodeArray[i * 5 + 4]]) + ' ';
     cmbTMode.ItemIndex := DbTmodeArray[i * 5];
     itemStr := itemStr + '[In/Out] ' + cmbTMode.Text;
     lstData.Items.Add(itemStr);
  end;
end;

procedure TfrmTMode.cmdUpdateClick(Sender: TObject);
var
    index                               : Integer;
    hour, minute, second, miliSecond    : WORD;
begin
    index := lstData.ItemIndex;
    if index = -1 then Exit;

    DecodeTime(dtStart.Time, hour, minute, second, miliSecond);
    DbTmodeArray[index * 5 + 1] := hour;
    DbTmodeArray[index * 5 + 2] := minute;
    DecodeTime(dtEnd.Time, hour, minute, second, miliSecond);
    DbTmodeArray[index * 5 + 3] := hour;
    DbTmodeArray[index * 5 + 4] := minute;
    DbTmodeArray[index * 5] := cmbTMode.ItemIndex;

    DbTmodeDraw();
end;

procedure TfrmTMode.cmdReadClick(Sender: TObject);
var
    vRet        : Boolean;
    vErrorCode  : Integer;
    vAddr       : Integer;
begin
    lblMessage.Caption := 'Waiting...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vAddr := Integer(addr(DbTmodeArray));
    vRet := _GetDeviceLongInfo(gMachineNumber, 4, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DbTmodeDraw();
      end
    else
      begin
          _GetLastError(gMachineNumber, vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmTMode.cmdWriteClick(Sender: TObject);
var
    vRet        : Boolean;
    vErrorCode  : Integer;
    vAddr       : Integer;
begin
    lblMessage.Caption := 'Waiting...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vAddr := Integer(addr(DbTmodeArray));
    vRet := _SetDeviceLongInfo(gMachineNumber, 4, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DbTmodeDraw();
      end
    else
      begin
          _GetLastError(gMachineNumber, vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    _EnableDevice(gMachineNumber, True);

end;


procedure TfrmTMode.cmdExitClick(Sender: TObject);
begin
  Close();
end;

procedure TfrmTMode.FormClose(Sender: TObject; var Action: TCloseAction);
begin
   TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmTMode.FormCreate(Sender: TObject);
begin
  DbTModeInit();
  DbTModeDraw();
end;

procedure TfrmTMode.lstDataClick(Sender: TObject);
var
  i : Integer;
begin
  i := lstData.ItemIndex;
  if (i < 0) or (i > DB_TMODE_MAX - 1) then Exit;

  dtStart.Time := EncodeTime(DbTmodeArray[i*5 + 1], DbTmodeArray[i*5 + 2], 0, 0);
  dtEnd.Time := EncodeTime(DbTmodeArray[i*5 + 3], DbTmodeArray[i*5 + 4], 0, 0);
  cmbTMode.ItemIndex := DbTModeArray[i*5 + 0];
end;

end.
