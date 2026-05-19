unit Unit_TMode;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls;

type
  TfrmTMode = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    lstTrMode: TListBox;
    cmdUpdate: TButton;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    dtStart: TDateTimePicker;
    dtEnd: TDateTimePicker;
    lblMessage: TStaticText;
    cmbDoorStatus: TComboBox;
    procedure TModeInit();
    procedure DrawTModeList();
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure cmdExitClick(Sender: TObject);
    procedure lstTModeClick(Sender: TObject);
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
  frmTMode: TfrmTMode;

implementation

uses Unit_Main, Utils, SBXPCLib_TLB;

{$R *.dfm}

const TMODE_COUNT : Integer = 10;

var
    bpc             :TSBXPC;
    mTModeInfoList :array[0..49] of Integer;

procedure TfrmTMode.FormShow(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
    TModeInit();
    DrawTModeList();
end;

procedure TfrmTMode.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmTMode.TModeInit();
var
  i : Integer;
begin
    for i := 0 to TMODE_COUNT - 1 do
    begin
        mTModeInfoList[i * 5 + 0] := 0;
        mTModeInfoList[i * 5 + 1] := 0;
        mTModeInfoList[i * 5 + 2] := 0;
        mTModeInfoList[i * 5 + 3] := 0;
        mTModeInfoList[i * 5 + 4] := 0;
    end;
end;

procedure TfrmTMode.DrawTModeList();
var
    i       : Integer;
    itemStr : String;
begin
    lstTrMode.Clear();
    for i := 0 to TMODE_COUNT - 1 do
    begin
        itemStr := '[No] ' + Format('%.2d', [i]) + ' ';
        itemStr := itemStr + '[S] ' + Format('%.2d', [mTModeInfoList[i * 5 + 1]]);
        itemStr := itemStr + ':'    + Format('%.2d', [mTModeInfoList[i * 5 + 2]]) + ' ';
        itemStr := itemStr + '[E] ' + Format('%.2d', [mTModeInfoList[i * 5 + 3]]);
        itemStr := itemStr + ':'    + Format('%.2d', [mTModeInfoList[i * 5 + 4]]) + ' ';
        cmbDoorStatus.ItemIndex := mTModeInfoList[i * 5];
        itemStr := itemStr + '[In/Out] ' + cmbDoorStatus.Text;
        lstTrMode.Items.Add(itemStr);
    end;
end;

procedure TfrmTMode.cmdExitClick(Sender: TObject);
begin
    Close();
end;

procedure TfrmTMode.lstTModeClick(Sender: TObject);
var
    index : Integer;
begin
    index := lstTrMode.ItemIndex;
    if index = -1 then Exit;

    dtStart.Time := EncodeTime(mTModeInfoList[index * 5 + 1], mTModeInfoList[index * 5 + 2], 0, 0);
    dtEnd.Time := EncodeTime(mTModeInfoList[index * 5 + 3], mTModeInfoList[index * 5 + 4], 0, 0);
    cmbDoorStatus.ItemIndex := mTModeInfoList[index * 5];
end;

procedure TfrmTMode.cmdUpdateClick(Sender: TObject);
var
    index                               : Integer;
    hour, minute, second, miliSecond    : WORD;
begin
    index := lstTrMode.ItemIndex;
    if index = -1 then Exit;

    DecodeTime(dtStart.Time, hour, minute, second, miliSecond);
    mTModeInfoList[index * 5 + 1] := hour;
    mTModeInfoList[index * 5 + 2] := minute;
    DecodeTime(dtEnd.Time, hour, minute, second, miliSecond);
    mTModeInfoList[index * 5 + 3] := hour;
    mTModeInfoList[index * 5 + 4] := minute;
    mTModeInfoList[index * 5] := cmbDoorStatus.ItemIndex;

    DrawTModeList();
end;

procedure TfrmTMode.cmdReadClick(Sender: TObject);
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
    vAddr := Integer(addr(mTModeInfoList));
    vRet := bpc.GetDeviceLongInfo(gMachineNumber, 4, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DrawTModeList();
      end
    else
      begin
          bpc.GetLastError(vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmTMode.cmdWriteClick(Sender: TObject);
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
    vAddr := Integer(addr(mTModeInfoList));
    vRet := bpc.SetDeviceLongInfo(gMachineNumber, 4, vAddr);
    if vRet then
      begin
          lblMessage.Caption := 'Success';
          DrawTModeList();
      end
    else
      begin
          bpc.GetLastError(vErrorCode);
          lblMessage.Caption := ErrorPrint(vErrorCode);
      end;
    bpc.EnableDevice(gMachineNumber, 1);

end;
end.
