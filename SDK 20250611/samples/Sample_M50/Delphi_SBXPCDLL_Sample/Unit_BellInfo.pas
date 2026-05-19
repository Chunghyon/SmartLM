unit Unit_BellInfo;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmBellInfo = class(TForm)
    lblMessage: TStaticText;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    _txtHour_0: TEdit;
    _txtHour_1: TEdit;
    _txtHour_2: TEdit;
    _txtHour_3: TEdit;
    _txtHour_4: TEdit;
    _txtHour_5: TEdit;
    _txtHour_6: TEdit;
    _txtHour_7: TEdit;
    _txtHour_8: TEdit;
    _txtHour_9: TEdit;
    _txtHour_10: TEdit;
    _txtHour_11: TEdit;
    _txtMinute_0: TEdit;
    _txtMinute_1: TEdit;
    _txtMinute_2: TEdit;
    _txtMinute_3: TEdit;
    _txtMinute_4: TEdit;
    _txtMinute_5: TEdit;
    _txtMinute_6: TEdit;
    _txtMinute_7: TEdit;
    _txtMinute_8: TEdit;
    _txtMinute_9: TEdit;
    _txtMinute_10: TEdit;
    _txtMinute_11: TEdit;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    Label8: TLabel;
    Label9: TLabel;
    Label10: TLabel;
    Label11: TLabel;
    Label12: TLabel;
    Label13: TLabel;
    Label14: TLabel;
    Label15: TLabel;
    Label16: TLabel;
    Label17: TLabel;
    Label18: TLabel;
    Label19: TLabel;
    Label20: TLabel;
    Label21: TLabel;
    Label22: TLabel;
    Label23: TLabel;
    Label24: TLabel;
    Label25: TLabel;
    Label26: TLabel;
    Label27: TLabel;
    Label28: TLabel;
    _chkValid_0: TCheckBox;
    _chkValid_1: TCheckBox;
    _chkValid_2: TCheckBox;
    _chkValid_3: TCheckBox;
    _chkValid_4: TCheckBox;
    _chkValid_5: TCheckBox;
    _chkValid_6: TCheckBox;
    _chkValid_7: TCheckBox;
    _chkValid_8: TCheckBox;
    _chkValid_9: TCheckBox;
    _chkValid_10: TCheckBox;
    _chkValid_11: TCheckBox;
    txtBellCount: TEdit;
    cmdRead: TButton;
    cmdWrite: TButton;
    cmdExit: TButton;
    Label29: TLabel;
    Label30: TLabel;
    Label31: TLabel;
    Label32: TLabel;
    Label33: TLabel;
    Label34: TLabel;
    Label35: TLabel;
    Label36: TLabel;
    Label37: TLabel;
    Label38: TLabel;
    Label39: TLabel;
    Label40: TLabel;
    Label41: TLabel;
    Label42: TLabel;
    Label43: TLabel;
    Label44: TLabel;
    Label45: TLabel;
    Label46: TLabel;
    Label47: TLabel;
    Label48: TLabel;
    Label49: TLabel;
    Label50: TLabel;
    Label51: TLabel;
    Label52: TLabel;
    Label53: TLabel;
    Label54: TLabel;
    Label55: TLabel;
    _txtHour_12: TEdit;
    _txtHour_13: TEdit;
    _txtHour_14: TEdit;
    _txtHour_15: TEdit;
    _txtHour_16: TEdit;
    _txtHour_17: TEdit;
    _txtHour_18: TEdit;
    _txtHour_19: TEdit;
    _txtHour_20: TEdit;
    _txtHour_21: TEdit;
    _txtHour_22: TEdit;
    _txtHour_23: TEdit;
    _txtMinute_12: TEdit;
    _txtMinute_13: TEdit;
    _txtMinute_14: TEdit;
    _txtMinute_15: TEdit;
    _txtMinute_16: TEdit;
    _txtMinute_17: TEdit;
    _txtMinute_18: TEdit;
    _txtMinute_19: TEdit;
    _txtMinute_20: TEdit;
    _txtMinute_21: TEdit;
    _txtMinute_22: TEdit;
    _txtMinute_23: TEdit;
    _chkValid_12: TCheckBox;
    _chkValid_13: TCheckBox;
    _chkValid_14: TCheckBox;
    _chkValid_15: TCheckBox;
    _chkValid_16: TCheckBox;
    _chkValid_17: TCheckBox;
    _chkValid_18: TCheckBox;
    _chkValid_19: TCheckBox;
    _chkValid_20: TCheckBox;
    _chkValid_21: TCheckBox;
    _chkValid_22: TCheckBox;
    _chkValid_23: TCheckBox;
    procedure cmdReadClick(Sender: TObject);
    procedure cmdWriteClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
    procedure BellInfoInit();
    procedure ShowValue();
    function GetValue():Boolean;
  public
    { Public declarations }
  end;

var
  frmBellInfo: TfrmBellInfo;

implementation

uses Unit_Main, Utils, SBXPCDLL_API;

const   MAX_BELLPOINT_COUNT:Integer = 24;

var
    mBellCount      :Integer;
    addrOf          :Integer;
    mlngBellInfo    :array[0..71] of Byte;
    chkValid        :array[0..23] of TCheckBox;
    txtHour         :array[0..23] of TEdit;
    txtMinute       :array[0..23] of TEdit;
    
{$R *.dfm}

procedure TfrmBellInfo.BellInfoInit();
var i       :Integer;
begin
    txtHour[0] := _txtHour_0;
    txtHour[1] := _txtHour_1;
    txtHour[2] := _txtHour_2;
    txtHour[3] := _txtHour_3;
    txtHour[4] := _txtHour_4;
    txtHour[5] := _txtHour_5;
    txtHour[6] := _txtHour_6;
    txtHour[7] := _txtHour_7;
    txtHour[8] := _txtHour_8;
    txtHour[9] := _txtHour_9;
    txtHour[10] := _txtHour_10;
    txtHour[11] := _txtHour_11;
    txtHour[12] := _txtHour_12;
    txtHour[13] := _txtHour_13;
    txtHour[14] := _txtHour_14;
    txtHour[15] := _txtHour_15;
    txtHour[16] := _txtHour_16;
    txtHour[17] := _txtHour_17;
    txtHour[18] := _txtHour_18;
    txtHour[19] := _txtHour_19;
    txtHour[20] := _txtHour_20;
    txtHour[21] := _txtHour_21;
    txtHour[22] := _txtHour_22;
    txtHour[23] := _txtHour_23;

    txtMinute[0] := _txtMinute_0;
    txtMinute[1] := _txtMinute_1;
    txtMinute[2] := _txtMinute_2;
    txtMinute[3] := _txtMinute_3;
    txtMinute[4] := _txtMinute_4;
    txtMinute[5] := _txtMinute_5;
    txtMinute[6] := _txtMinute_6;
    txtMinute[7] := _txtMinute_7;
    txtMinute[8] := _txtMinute_8;
    txtMinute[9] := _txtMinute_9;
    txtMinute[10] := _txtMinute_10;
    txtMinute[11] := _txtMinute_11;
    txtMinute[12] := _txtMinute_12;
    txtMinute[13] := _txtMinute_13;
    txtMinute[14] := _txtMinute_14;
    txtMinute[15] := _txtMinute_15;
    txtMinute[16] := _txtMinute_16;
    txtMinute[17] := _txtMinute_17;
    txtMinute[18] := _txtMinute_18;
    txtMinute[19] := _txtMinute_19;
    txtMinute[20] := _txtMinute_20;
    txtMinute[21] := _txtMinute_21;
    txtMinute[22] := _txtMinute_22;
    txtMinute[23] := _txtMinute_23;

    chkValid[0] := _chkValid_0;
    chkValid[1] := _chkValid_1;
    chkValid[2] := _chkValid_2;
    chkValid[3] := _chkValid_3;
    chkValid[4] := _chkValid_4;
    chkValid[5] := _chkValid_5;
    chkValid[6] := _chkValid_6;
    chkValid[7] := _chkValid_7;
    chkValid[8] := _chkValid_8;
    chkValid[9] := _chkValid_9;
    chkValid[10] := _chkValid_10;
    chkValid[11] := _chkValid_11;
    chkValid[12] := _chkValid_12;
    chkValid[13] := _chkValid_13;
    chkValid[14] := _chkValid_14;
    chkValid[15] := _chkValid_15;
    chkValid[16] := _chkValid_16;
    chkValid[17] := _chkValid_17;
    chkValid[18] := _chkValid_18;
    chkValid[19] := _chkValid_19;
    chkValid[20] := _chkValid_20;
    chkValid[21] := _chkValid_21;
    chkValid[22] := _chkValid_22;
    chkValid[23] := _chkValid_23;

    for i := 0 to MAX_BELLPOINT_COUNT * 3 - 1 do
        mlngBellInfo[i] := 0;
    ShowValue;
end;

procedure TfrmBellInfo.ShowValue();
var i       :Integer;
begin
    for i := 0 to MAX_BELLPOINT_COUNT - 1 do
    begin
        if mlngBellInfo[i] > 1 then mlngBellInfo[i] := 0;
        if mlngBellInfo[i] = 1 then chkValid[i].Checked := true
        else chkValid[i].Checked := false;
        txtHour[i].Text := Format('%.2d', [mlngBellInfo[i + MAX_BELLPOINT_COUNT]]);
        txtMinute[i].Text := Format('%.2d', [mlngBellInfo[i + MAX_BELLPOINT_COUNT * 2]]);
    end;
    txtBellCount.Text := IntToStr(mBellCount);
end;

function TfrmBellInfo.GetValue():Boolean;
var i       :Integer;
begin
    for i := 0 to MAX_BELLPOINT_COUNT - 1 do
    begin
        if chkValid[i].Checked then mlngBellInfo[i] := 1
        else mlngBellInfo[i] := 0;
        if txtHour[i].Text = '' then GetValue := false;
        if Not (StrToInt(txtHour[i].Text) in [0..23]) then GetValue := false;
        mlngBellInfo[i + MAX_BELLPOINT_COUNT] := StrToInt(txtHour[i].Text);
        if txtMinute[i].Text = '' then GetValue := false;
        if Not (StrToInt(txtMinute[i].Text) in [0..59]) then GetValue := false;
        mlngBellInfo[i + MAX_BELLPOINT_COUNT * 2] := StrToInt(txtMinute[i].Text);
    end;
    if txtBellCount.Text = '' then GetValue := false;
    mBellCount := StrToInt(txtBellCount.Text);
    GetValue := true;
end;

procedure TfrmBellInfo.cmdReadClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    lblMessage.Caption := 'Waiting...';

    if (Not _EnableDevice(gMachineNumber, False)) then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    addrOf := Integer(addr(mlngBellInfo));
    vRet := _GetBellTime(gMachineNumber, mBellCount, addrOf);
    if vRet then
    begin
        ShowValue;
        lblMessage.Caption := 'Success';
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);;
end;

procedure TfrmBellInfo.cmdWriteClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    lblMessage.Caption := 'Waiting...';
    if Not _EnableDevice(gMachineNumber, False) then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    if Not GetValue then
        application.MessageBox('Invalid Value.', 'Bell Info')
    else
    begin
        addrOf := Integer(addr(mlngBellInfo));
        vRet := _SetBellTime(gMachineNumber, mBellCount, addrOf);
        if vRet then
        begin
            ShowValue;
            lblMessage.Caption := 'Success!';
        end
        else
        begin
            _GetLastError(gMachineNumber, vErrorCode);
            lblMessage.Caption := ErrorPrint(vErrorCode);
        end;
    end;
    _EnableDevice(gMachineNumber, True);;
end;

procedure TfrmBellInfo.FormShow(Sender: TObject);
begin
    BellInfoInit;
end;

procedure TfrmBellInfo.cmdExitClick(Sender: TObject);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    Close;
end;

procedure TfrmBellInfo.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

end.
