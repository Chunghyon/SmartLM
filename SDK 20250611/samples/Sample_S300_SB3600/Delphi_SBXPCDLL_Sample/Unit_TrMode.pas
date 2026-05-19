unit Unit_TrMode;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls;
  
type
  TfrmTrMode = class(TForm)
    lstTrMode: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    cmbTrStatus: TComboBox;
    dtStart: TDateTimePicker;
    dtEnd: TDateTimePicker;
    lblMessage: TStaticText;
    procedure TrModeInit();
    procedure DrawTrModeList();
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure cmdExitClick(Sender: TObject);
    procedure lstTrModeClick(Sender: TObject);
    procedure cmdUpdateClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmTrMode: TfrmTrMode;

implementation

uses Unit_Main, Utils, SBXPCDLL_API;

{$R *.dfm}

const TRMODE_COUNT : Integer = 10;

var
    mTrModeInfoList :array[0..49] of Integer;

procedure TfrmTrMode.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmTrMode.TrModeInit();
var
  i : Integer;
begin
    for i := 0 to TRMODE_COUNT - 1 do
    begin
        mTrModeInfoList[i * 5 + 0] := 0;
        mTrModeInfoList[i * 5 + 1] := 0;
        mTrModeInfoList[i * 5 + 2] := 0;
        mTrModeInfoList[i * 5 + 3] := 0;
        mTrModeInfoList[i * 5 + 4] := 0;
    end;
end;

procedure TfrmTrMode.DrawTrModeList();
var
    i       : Integer;
    itemStr : String;
begin
    lstTrMode.Clear();
    for i := 0 to TRMODE_COUNT-1 do
    begin
        itemStr := '[No] ' + Format('%.2d', [i]) + ' ';
        itemStr := itemStr + '[S] ' + Format('%.2d', [mTrModeInfoList[i * 5 + 1]]);
        itemStr := itemStr + ':'    + Format('%.2d', [mTrModeInfoList[i * 5 + 2]]) + ' ';
        itemStr := itemStr + '[E] ' + Format('%.2d', [mTrModeInfoList[i * 5 + 3]]);
        itemStr := itemStr + ':'    + Format('%.2d', [mTrModeInfoList[i * 5 + 4]]) + ' ';
        cmbTrStatus.ItemIndex := mTrModeInfoList[i * 5];
        itemStr := itemStr + '[In/Out] ' + cmbTrStatus.Text;
        lstTrMode.Items.Add(itemStr);
    end;
end;

procedure TfrmTrMode.cmdExitClick(Sender: TObject);
begin
    Close();
end;

procedure TfrmTrMode.lstTrModeClick(Sender: TObject);
var
    index : Integer;
begin
    index := lstTrMode.ItemIndex;
    if index = -1 then Exit;

    dtStart.Time := EncodeTime(mTrModeInfoList[index * 5 + 1], mTrModeInfoList[index * 5 + 2], 0, 0);
    dtEnd.Time := EncodeTime(mTrModeInfoList[index * 5 + 3], mTrModeInfoList[index * 5 + 4], 0, 0);
    cmbTrStatus.ItemIndex := mTrModeInfoList[index * 5];
end;

procedure TfrmTrMode.cmdUpdateClick(Sender: TObject);
var
    index                               : Integer;
    hour, minute, second, miliSecond    : WORD;
begin
    index := lstTrMode.ItemIndex;
    if index = -1 then Exit;

    DecodeTime(dtStart.Time, hour, minute, second, miliSecond);
    mTrModeInfoList[index * 5 + 1] := hour;
    mTrModeInfoList[index * 5 + 2] := minute;
    DecodeTime(dtEnd.Time, hour, minute, second, miliSecond);
    mTrModeInfoList[index * 5 + 3] := hour;
    mTrModeInfoList[index * 5 + 4] := minute;
    mTrModeInfoList[index * 5] := cmbTrStatus.ItemIndex;

    DrawTrModeList();
end;

procedure TfrmTrMode.FormShow(Sender: TObject);
begin
    TrModeInit();
    DrawTrModeList();
end;

procedure TfrmTrMode.cmdReadClick(Sender: TObject);
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
    vAddr := Integer(addr(mTrModeInfoList));
    vRet := _GetDeviceLongInfo(gMachineNumber, 5, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DrawTrModeList();
      end
    else
      begin
          _GetLastError(gMachineNumber, vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmTrMode.cmdWriteClick(Sender: TObject);
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
    vAddr := Integer(addr(mTrModeInfoList));
    vRet := _SetDeviceLongInfo(gMachineNumber, 5, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DrawTrModeList();
      end
    else
      begin
          _GetLastError(gMachineNumber, vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    _EnableDevice(gMachineNumber, True);

end;

end.
