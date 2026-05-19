unit Unit_Holiday;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls;

type
  TfrmHoliday = class(TForm)
    Label3: TLabel;
    lstHoliday: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    dtHoliday: TDateTimePicker;
    lblMessage: TStaticText;
    txtPeriod: TEdit;
    procedure FormCreate(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure HolidayInit();
    procedure DrawHolidaySetting();
    procedure FormShow(Sender: TObject);
    procedure lstHolidayClick(Sender: TObject);
    procedure cmdUpdateClick(Sender: TObject);
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmHoliday: TfrmHoliday;

implementation

uses Unit_Main, Utils, SBXPCLib_TLB;

{$R *.dfm}

const
    HOLIDAYS_MAX : Integer = 256;

var
    bpc             :TSBXPC;
    holidaysInfo    :array[0..767] of Integer;

procedure TfrmHoliday.FormCreate(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
end;

procedure TfrmHoliday.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmHoliday.HolidayInit();
var
    i  : Integer;
begin
    for i := 0 to  HOLIDAYS_MAX - 1 do
    begin
        holidaysInfo[i * 3 + 0] := 1;
        holidaysInfo[i * 3 + 1] := 1;
        holidaysInfo[i * 3 + 2] := 0;
    end;
end;

procedure TfrmHoliday.DrawHolidaySetting();
var
    i           : Integer;
    itemString  : String;
begin
    lstHoliday.Clear();

    for i := 0 to  HOLIDAYS_MAX - 1 do
    begin
        itemString := '[No.] ' + Format('%.02d', [i]) + ' ';
        itemString := itemString + '[Day/Month] ';
        itemString := itemString + Format('%.02d', [holidaysInfo[i * 3 + 1]]) + '/';
        itemString := itemString + Format('%.02d', [holidaysInfo[i * 3]]) + ' ';
        itemString := itemString + '[Period] ';
        itemString := itemString + Format('%.02d', [holidaysInfo[i * 3 + 2]]);

        lstHoliday.Items.Add(itemString);
    end;
end;    

procedure TfrmHoliday.FormShow(Sender: TObject);
begin
    HolidayInit();
    DrawHolidaySetting();
end;

procedure TfrmHoliday.lstHolidayClick(Sender: TObject);
var
    index   : Integer;
    year, month, day : WORD;
        
begin
    if lstHoliday.ItemIndex = -1 then Exit;
    index := lstHoliday.ItemIndex * 3;

    txtPeriod.Text := IntToStr(holidaysInfo[index + 2]);

    If ((holidaysInfo[index] > 0) and
        (holidaysInfo[index] <= 12) and
        (holidaysInfo[index + 1] > 0) and
        (holidaysInfo[index + 1] <= 31)
       ) Then
    begin
        year := 2000;
        month := holidaysInfo[index];
        day := holidaysInfo[index + 1];
        dtHoliday.Date := EncodeDate(year, month, day);
    end;
end;

procedure TfrmHoliday.cmdUpdateClick(Sender: TObject);
var
    index               : Integer;
    year, month, day    : WORD;
begin
    if lstHoliday.ItemIndex = -1 then Exit;
    index := lstHoliday.ItemIndex * 3;

    DecodeDate(dtHoliday.Date, year, month, day);
    holidaysInfo[index] := month;
    holidaysInfo[index + 1] := day;
    holidaysInfo[index + 2] := StrToInt(txtPeriod.Text);

    DrawHolidaySetting();
end;

procedure TfrmHoliday.cmdReadClick(Sender: TObject);
var
    vRet        : Boolean;
    vErrorCode  : Integer;
    vAddr       : Integer;
begin
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vAddr := Integer(addr(holidaysInfo));
    vRet := bpc.GetDeviceLongInfo(gMachineNumber, 6, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DrawHolidaySetting();
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
    vRet        : Boolean;
    vErrorCode  : Integer;
    vAddr       : Integer;
begin
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vAddr := Integer(addr(holidaysInfo));
    vRet := bpc.SetDeviceLongInfo(gMachineNumber, 6, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
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

end.
