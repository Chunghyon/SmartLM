unit Unit_Holiday;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls;

type
  TfrmHoliday = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    lstData: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    dtHoliday: TDateTimePicker;
    lblMessage: TStaticText;
    txtPeriod: TEdit;
    procedure FormCreate(Sender: TObject);
    procedure DbHolidayInit();
    procedure DbHolidayDraw();
    procedure cmdUpdateClick(Sender: TObject);
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure lstDataClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmHoliday: TfrmHoliday;

implementation

{$R *.dfm}

uses Unit_Main, Utils, SBXPCLib_TLB;

var
    DbHolidayArray : array[0..256*3] of Integer;
    bpc         :TSBXPC;
    
const
    DB_HOLIDAY_MAX = 256;

procedure TfrmHoliday.FormCreate(Sender: TObject);
var
    i : Integer;
begin
   bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
   DbHolidayInit();
   DbHolidayDraw();
end;

procedure TfrmHoliday.DbHolidayInit();
var
    i : Integer;
begin
    for i := 0 to DB_HOLIDAY_MAX - 1 do
    begin
        DbHolidayArray[i * 3 + 0] := 1;
        DbHolidayArray[i * 3 + 1] := 1;
        DbHolidayArray[i * 3 + 2] := 0;
    end;
end;

procedure TfrmHoliday.DbHolidayDraw();
var
    i : Integer;
    itemStr : String;
begin
    lstData.Items.Clear;
    for i := 0 to DB_HOLIDAY_MAX - 1 do
    begin
        itemStr := '[No.] ' + Format('%.3d', [i + 1]) + ' ';
        itemStr := itemStr + '[Day/Month] ' + Format('%02d', [DbHolidayArray[i*3 + 1]])
                     + '/' + Format('%02d', [DbHolidayArray[i*3 + 0]]) + ' ';
        itemStr := itemStr + '[Period] ' + IntToStr(DbHolidayArray[i*3 + 2]);
        lstData.Items.Add(itemStr);
    end;
end;

procedure TfrmHoliday.cmdUpdateClick(Sender: TObject);
var
    i : Integer;
    year, month, day : Word;
begin
    i := lstData.ItemIndex;
    if (i < 0) or (i > DB_HOLIDAY_MAX - 1) then Exit;

    DecodeDate(dtHoliday.Date, year, month, day);
    DbHolidayArray[i * 3 + 0] := month;
    DbHolidayArray[i * 3 + 1] := day;

    if (StrToInt(txtPeriod.Text) < 0) or (StrToInt(txtPeriod.Text) > 7) then
    begin
        application.MessageBox('Duration must be from 0 to 7days !', 'Holidays');
        Exit;
    end;

    DbHolidayArray[i * 3 + 2] := StrToInt(txtPeriod.Text);

    DbHolidayDraw();

    lstData.ItemIndex := i;
end;

procedure TfrmHoliday.cmdReadClick(Sender: TObject);
var
    bRet : Boolean;
    vErrorCode : Integer;
    addrOf : Integer;
begin
    lblMessage.Caption := 'Waiting...';
    bRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not bRet then
    begin
      lblMessage.Caption := GSTR_NODEVICE;
      Exit;
    end;

    addrOf := Integer(addr(DbHolidayArray));
    bRet := bpc.GetDeviceLongInfo(gMachineNumber, 6, addrOf);
    if bRet then
    begin
      DbHolidayDraw();
      lblMessage.Caption := 'Success!';
    end
    else
    begin
      bpc.GetLastError(vErrorCode);
      lblMessage.Caption := ErrorPrint(vErrorCode);
    end;

    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmHoliday.cmdWriteClick(Sender: TObject);
var
  bRet : Boolean;
  vErrorCode : Integer;
  addrOf : Integer;
begin
  lblMessage.Caption := 'Waiting...';

  bRet := bpc.EnableDevice(gMachineNumber, 0);

  addrOf := Integer(addr(DbHolidayArray));
  bRet := bpc.SetDeviceLongInfo(gMachineNumber, 6, addrOf);

  if Not bRet then
  begin
    DbHolidayDraw();
    lblMessage.Caption := 'Success!';
  end
  else
  begin
    bpc.GetLastError(vErrorCode);
    lblMessage.Caption := ErrorPrint(vErrorCode);
  end;

  bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmHoliday.cmdExitClick(Sender: TObject);
begin
  Close();
end;

procedure TfrmHoliday.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmHoliday.lstDataClick(Sender: TObject);
var
  i : Integer;
  year, month, day : Word;
begin
  i := lstData.ItemIndex;

  if (i < 0) or (i > DB_HOLIDAY_MAX - 1) then Exit;

  if (DBHolidayArray[i * 3 + 0] >= 1) and (DbHolidayArray[i*3 + 0] <= 12) then
  begin
    if (DBHolidayArray[i * 3 + 1] >= 1) and (DbHolidayArray[i*3 + 1] <= 31) then
    begin
      dtHoliday.Date := EncodeDate(2000, DbHolidayArray[i*3 + 0], DbHolidayArray[i*3 + 1]);
    end;
  end;

  txtPeriod.Text := IntToStr(DbHolidayArray[i*3 + 2]);
end;

end.
