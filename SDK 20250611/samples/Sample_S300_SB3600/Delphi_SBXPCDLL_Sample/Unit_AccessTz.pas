unit Unit_AccessTz;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls;

type
  TfrmAccessTz = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    lstAccessTz: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    cmbVerifyMode: TComboBox;
    dtStart: TDateTimePicker;
    dtEnd: TDateTimePicker;
    lblMessage: TStaticText;
    procedure FormShow(Sender: TObject);
    procedure AccessTzListInit();
    procedure DrawAccessTzList();
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure cmdExitClick(Sender: TObject);
    procedure lstAccessTzClick(Sender: TObject);
    procedure cmdUpdateClick(Sender: TObject);
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmAccessTz: TfrmAccessTz;

implementation

{$R *.dfm}

uses Unit_Main, Utils, SBXPCDLL_API;

const
    TIMEZONE_COUNT      : Integer = 50;
    VMTIMEZONE_COUNT    : Integer = 10;
    TIMESECTION_COUNT   : Integer = 8;
    DEFAULT_VM          : Integer = 5;
var
    mTimeZoneInfoList   : array[0..1999] of Integer;

procedure TfrmAccessTz.FormShow(Sender: TObject);
begin
    AccessTzListInit();
    DrawAccessTzList();
end;

procedure TfrmAccessTz.AccessTzListInit();
var
    i, j, index : Integer;
begin
    for i := 0 to TIMEZONE_COUNT - 1 do
    begin
        for j := 0 to TIMESECTION_COUNT - 1 do
        begin
            index := i * TIMESECTION_COUNT + j;
            mTimeZoneInfoList[index * 5 + 0] := 0;
            mTimeZoneInfoList[index * 5 + 1] := 0;
            mTimeZoneInfoList[index * 5 + 2] := 23;
            mTimeZoneInfoList[index * 5 + 3] := 59;
            
            mTimeZoneInfoList[i * 5 + 4] := DEFAULT_VM;
        end;
    end;
end;

procedure TfrmAccessTz.DrawAccessTzList();
var
    i, j, index : Integer;
    itemStr     : String;
begin
    lstAccessTz.Clear();
    for i := 0 to TIMEZONE_COUNT - 1 do
    begin
        for j := 0 to TIMESECTION_COUNT - 1 do
        begin
            index := i * TIMESECTION_COUNT + j;

            itemStr := '[No] ' + Format('%.2d', [i]) + '-' + Format('%.3d', [j]) + ' ';
            itemStr := itemStr + '[S]'  + Format('%.2d', [mTimeZoneInfoList[index * 5 + 0]]);
            itemStr := itemStr + ':'    + Format('%.2d', [mTimeZoneInfoList[index * 5 + 1]]) + ' ';
            itemStr := itemStr + '[E]'  + Format('%.2d', [mTimeZoneInfoList[index * 5 + 2]]);
            itemStr := itemStr + ':'    + Format('%.2d', [mTimeZoneInfoList[index * 5 + 3]]) + ' ';

            if i < VMTIMEZONE_COUNT then
            begin
                cmbVerifyMode.ItemIndex := mTimeZoneInfoList[index * 5 + 4];
            end
            else
            begin
                cmbVerifyMode.ItemIndex := DEFAULT_VM;
            end;
            itemStr := itemStr + '[VM] ' + cmbVerifyMode.Text;

            lstAccessTz.Items.Add(itemStr);
        end;
    end;
end;

procedure TfrmAccessTz.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmAccessTz.cmdExitClick(Sender: TObject);
begin
    Close();
end;

procedure TfrmAccessTz.lstAccessTzClick(Sender: TObject);
var
    index : Integer;
begin
    index := lstAccessTz.ItemIndex;
    if index = -1 then Exit;

    dtStart.Time := EncodeTime(mTimeZoneInfoList[index * 5], mTimeZoneInfoList[index * 5 + 1], 0, 0);
    dtEnd.Time := EncodeTime(mTimeZoneInfoList[index * 5 + 2], mTimeZoneInfoList[index * 5 + 3], 0, 0);

    if index < VMTIMEZONE_COUNT * TIMESECTION_COUNT then
    begin
        cmbVerifyMode.ItemIndex := mTimeZoneInfoList[index * 5 + 4];
    end
    else
    begin
        cmbVerifyMode.ItemIndex := DEFAULT_VM;
    end;

end;

procedure TfrmAccessTz.cmdUpdateClick(Sender: TObject);
var
    hour, minute, second, miliSecond    : WORD;
    index                               : Integer;
begin
    index := lstAccessTz.ItemIndex;
    if index = -1 then Exit;

    DecodeTime(dtStart.Time, hour, minute, second, miliSecond);
    mTimeZoneInfoList[index * 5 + 0] := hour;
    mTimeZoneInfoList[index * 5 + 1] := minute;
    DecodeTime(dtEnd.Time, hour, minute, second, miliSecond);
    mTimeZoneInfoList[index * 5 + 2] := hour;
    mTimeZoneInfoList[index * 5 + 3] := minute;

    if index < VMTIMEZONE_COUNT * TIMESECTION_COUNT then
    begin
        mTimeZoneInfoList[index * 5 + 4] := cmbVerifyMode.ItemIndex;
    end
    else
    begin
        mTimeZoneInfoList[index * 5 + 4] := DEFAULT_VM;
    end;
    
    DrawAccessTzList();
end;

procedure TfrmAccessTz.cmdReadClick(Sender: TObject);
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
    vAddr := Integer(addr(mTimeZoneInfoList));
    vRet := _GetDeviceLongInfo(gMachineNumber, 3, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DrawAccessTzList();
      end
    else
      begin
          _GetLastError(gMachineNumber, vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmAccessTz.cmdWriteClick(Sender: TObject);
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
    vAddr := Integer(addr(mTimeZoneInfoList));
    vRet := _SetDeviceLongInfo(gMachineNumber, 3, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
      end
    else
      begin
          _GetLastError(gMachineNumber, vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    _EnableDevice(gMachineNumber, True);
end;

end.
