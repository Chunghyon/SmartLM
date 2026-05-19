unit Unit_Log;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids, ExtCtrls, ComCtrls, jpeg;

type
  TfrmLog = class(TForm)
    lblMessage: TStaticText;
    gridSLogData: TStringGrid;
    Label1: TLabel;
    LabelTotal: TLabel;
    chkAndDelete: TCheckBox;
    chkReadMark: TCheckBox;
    cmdSLogData: TButton;
    cmdAllSLogData: TButton;
    cmdEmptySLog: TButton;
    cmdGlogData: TButton;
    cmdAllGLogData: TButton;
    cmdEmptyGLog: TButton;
    cmdExit: TButton;
    procedure FormShow(Sender: TObject);
    procedure chkReadMarkClick(Sender: TObject);
    procedure cmdSLogDataClick(Sender: TObject);
    procedure cmdAllSLogDataClick(Sender: TObject);
    procedure cmdEmptySLogClick(Sender: TObject);
    procedure cmdGlogDataClick(Sender: TObject);
    procedure cmdAllGLogDataClick(Sender: TObject);
    procedure cmdEmptyGLogClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmLog: TfrmLog;

implementation

uses Unit_Main, Utils, SBXPCLib_TLB;

var
    bpc             : TSBXPC;

{$R *.dfm}

procedure TfrmLog.FormShow(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
    chkReadMark.Checked := true;
end;

procedure TfrmLog.chkReadMarkClick(Sender: TObject);
begin
    bpc.ReadMark := chkReadMark.Checked;
end;

procedure TfrmLog.cmdSLogDataClick(Sender: TObject);
const gstrLogItem:array[0..8] of string = ('', 'TMNo', 'SEnlNo', 'SMNo', 'GEnlNo', 'GMNo', 'Manipulation', 'FpNo', 'DateTime' );
var
    vRet            :Boolean;
    vTMachineNumber :Integer;
    vSMachineNumber :Integer;
    vSEnrollNumber  :Integer;
    vGEnrollNumber  :Integer;
    vGMachineNumber :Integer;
    vManipulation   :Integer;
    vFingerNumber   :Integer;
    vYear           :Integer;
    vMonth          :Integer;
    vDay            :Integer;
    vHour           :Integer;
    vMinute         :Integer;
    vSecond         :Integer;
    vErrorCode      :Integer;
    i               :Integer;
begin
    lblMessage.Caption := 'Waiting...';
    LabelTotal.Caption := 'Total : ';
    gridSLogData.RowCount := 2;
    gridSLogData.ColCount := 9;
    gridSLogData.Rows[1].Clear;
    gridSLogData.ColWidths[0] := 40;
    for i := 1 to 8 do
    begin
        gridSLogData.Cells[i, 0] := gstrLogItem[i];
        gridSLogData.ColWidths[i] := 60;
    end;
    gridSLogData.ColWidths[6] := 120;
    gridSLogData.ColWidths[7] := 50;
    gridSLogData.Cells[8, 0] := gstrLogItem[8];
    gridSLogData.ColWidths[8] := 100;

    frmLog.Cursor := crHourGlass;
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblmessage.Caption := GSTR_NODEVICE;
        frmLog.Cursor := crDefault;
        Exit;
    end;
    vRet := bpc.ReadSuperLogData(gMachineNumber);
    if Not vRet then
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end
    else
        if chkAndDelete.Checked then bpc.EmptySuperLogData(gMachineNumber);
    if vRet then
    begin
        frmLog.Cursor := crHourGlass;
        lblMessage.Caption := 'Getting...';
        i := 1;
        while  true do
        begin
            vRet := bpc.GetSuperLogData(gMachineNumber,vTMachineNumber, vSEnrollNumber, vSMachineNumber, vGEnrollNumber, vGMachineNumber, vManipulation, vFingerNumber, vYear, vMonth, vDay, vHour, vMinute, vSecond);
            if Not vRet then Break;
            if vRet and (i <> 1) then gridSLogData.RowCount := gridSLogData.RowCount + 1;
            gridSLogData.Cells[0, i] := IntToStr(i);
            gridSLogData.Cells[1, i] := IntToStr(vTMachineNumber);
            gridSLogData.Cells[2, i] := IntToStr(vSEnrollNumber);
            gridSLogData.Cells[3, i] := IntToStr(vSMachineNumber);
            gridSLogData.Cells[4, i] := IntToStr(vGEnrollNumber);
            gridSLogData.Cells[5, i] := IntToStr(vGMachineNumber);
            case vManipulation of
                1..3: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Enroll user';
                4:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Enroll Manager';
                5:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete Fp Data';
                6:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete Password';
                7:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete Card Data';
                8:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete All LogData';
                9:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify System Info';
                10: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify System Time';
                11: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify Log Setting';
                12: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify Comm Setting';
                13: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify Timezone Setting';
            end;
            if vFingerNumber < 10 then gridSLogData.Cells[7, i] := IntToStr(vFingerNumber)
            else if vFingerNumber = 10 then  gridSLogData.Cells[7, i] := 'Password'
            else gridSLogData.Cells[7, i] := 'Card';
            gridSLogData.Cells[8, i] := IntToStr(vYear) + '/' + Format('%.2d', [vMonth]) + '/' + Format('%.2d', [vDay]) + ' ' + Format('%.2d', [vHour]) + ':' + Format('%.2d', [vMinute]);
            LabelTotal.Caption := 'Total : ' + IntToStr(i);
            Inc(i);
        end;
        lblMessage.Caption := 'ReadSuperLogData OK';
    end;
    frmLog.Cursor := crDefault;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLog.cmdAllSLogDataClick(Sender: TObject);
const gstrLogItem:array[0..8] of string = ('', 'TMNo', 'SEnlNo', 'SMNo', 'GEnlNo', 'GMNo', 'Manipulation', 'FpNo', 'DateTime' );
var
    vRet            :Boolean;
    vTMachineNumber :Integer;
    vSMachineNumber :Integer;
    vSEnrollNumber  :Integer;
    vGEnrollNumber  :Integer;
    vGMachineNumber :Integer;
    vManipulation   :Integer;
    vFingerNumber   :Integer;
    vYear           :Integer;
    vMonth          :Integer;
    vDay            :Integer;
    vHour           :Integer;
    vMinute         :Integer;
    vSecond         :Integer;
    vErrorCode      :Integer;
    i               :Integer;
begin
    lblMessage.Caption := 'Waiting...';
    LabelTotal.Caption := 'Total : ';
    gridSLogData.RowCount := 2;
    gridSLogData.ColCount := 9;
    gridSLogData.Rows[1].Clear;
    gridSLogData.ColWidths[0] := 40;
    for i := 1 to 8 do
    begin
        gridSLogData.Cells[i, 0] := gstrLogItem[i];
        gridSLogData.ColWidths[i] := 60;
    end;
    gridSLogData.ColWidths[6] := 120;
    gridSLogData.ColWidths[7] := 50;
    gridSLogData.Cells[8, 0] := gstrLogItem[8];
    gridSLogData.ColWidths[8] := 100;

    frmLog.Cursor := crHourGlass;
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblmessage.Caption := GSTR_NODEVICE;
        frmLog.Cursor := crDefault;
        Exit;
    end;
    vRet := bpc.ReadAllSLogData(gMachineNumber);
    if Not vRet then
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end
    else
        if chkAndDelete.Checked then bpc.EmptySuperLogData(gMachineNumber);
    if vRet then
    begin
        frmLog.Cursor := crHourGlass;
        lblMessage.Caption := 'Getting...';
        i := 1;
        while  true do
        begin
            vRet := bpc.GetAllSLogData(gMachineNumber,vTMachineNumber, vSEnrollNumber, vSMachineNumber, vGEnrollNumber, vGMachineNumber, vManipulation, vFingerNumber, vYear, vMonth, vDay, vHour, vMinute, vSecond);
            if Not vRet then Break;
            if vRet and (i <> 1) then gridSLogData.RowCount := gridSLogData.RowCount + 1;
            gridSLogData.Cells[0, i] := IntToStr(i);
            gridSLogData.Cells[1, i] := IntToStr(vTMachineNumber);
            gridSLogData.Cells[2, i] := IntToStr(vSEnrollNumber);
            gridSLogData.Cells[3, i] := IntToStr(vSMachineNumber);
            gridSLogData.Cells[4, i] := IntToStr(vGEnrollNumber);
            gridSLogData.Cells[5, i] := IntToStr(vGMachineNumber);
            case vManipulation of
                1..3: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Enroll user';
                4:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Enroll Manager';
                5:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete Fp Data';
                6:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete Password';
                7:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete Card Data';
                8:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Delete All LogData';
                9:  gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify System Info';
                10: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify System Time';
                11: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify Log Setting';
                12: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify Comm Setting';
                13: gridSLogData.Cells[6, i] := IntToStr(vManipulation) + '--' + 'Modify Timezone Setting';
            end;
            if vFingerNumber < 10 then gridSLogData.Cells[7, i] := IntToStr(vFingerNumber)
            else if vFingerNumber = 10 then  gridSLogData.Cells[7, i] := 'Password'
            else gridSLogData.Cells[7, i] := 'Card';
            gridSLogData.Cells[8, i] := IntToStr(vYear) + '/' + Format('%.2d', [vMonth]) + '/' + Format('%.2d', [vDay]) + ' ' + Format('%.2d', [vHour]) + ':' + Format('%.2d', [vMinute]);
            LabelTotal.Caption := 'Total : ' + IntToStr(i);
            Inc(i);
        end;
        lblMessage.Caption := 'ReadSuperLogData OK';
    end;
    frmLog.Cursor := crDefault;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLog.cmdEmptySLogClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    gridSLogData.RowCount := 2;
//    gridSLogData.ColCount := 9;
    gridSLogData.Rows[1].Clear;
    LabelTotal.Caption := 'Total : ';
    vRet := bpc.EmptySuperLogData(gMachineNumber);
    if vRet then lblMessage.Caption := 'EmptySuperLogData OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLog.cmdGlogDataClick(Sender: TObject);
const gstrLogItem:array[0..5] of string = ( '', 'PhotoNo', 'EnrollNo', 'EMachineNo', 'VeriMode', 'DateTime');
var
    vRet            :Boolean;
    vTMachineNumber :Integer;
    vSMachineNumber :Integer;
    vSEnrollNumber  :Integer;
    vVerifyMode     :Integer;
    vYear           :Integer;
    vMonth          :Integer;
    vDay            :Integer;
    vHour           :Integer;
    vMinute         :Integer;
    vSecond         :Integer;
    vErrorCode      :Integer;
    i               :Integer;
    vAttStatus      :Integer;
    vAntipass       :Integer;
    vDiv            :Integer;
    stAttStatus     :string;
    stAntipass      :string;
begin
    vDiv := 65536;
    lblMessage.Caption := 'Waiting...';
    LabelTotal.Caption := 'Total : ';
    gridSLogData.RowCount := 2;
    gridSLogData.ColCount := 6;
    gridSLogData.Rows[1].Clear;
    gridSLogData.ColWidths[0] := 40;
    for i := 1 to 5 do
    begin
        gridSLogData.Cells[i, 0] := gstrLogItem[i];
        gridSLogData.ColWidths[i] := 70;
    end;
    gridSLogData.ColWidths[4] := 120;
    gridSLogData.ColWidths[5] := 120;

    frmLog.Cursor := crHourGlass;
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        frmLog.Cursor := crDefault;
        Exit;
    end;
    vRet := bpc.ReadGeneralLogData(gMachineNumber);
    if Not vRet then
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end
    else
        if chkAndDelete.Checked then bpc.EmptyGeneralLogData(gMachineNumber);
    if vRet then
    begin
        frmLog.Cursor := crHourGlass;
        lblMessage.Caption := 'Getting...';
        i := 1;
        while true do
        begin
            vRet := bpc.GetGeneralLogData(gMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond);
            if Not vRet then Break;
            if vRet and (i <> 1) then gridSLogData.RowCount := gridSLogData.RowCount + 1;
            vAntipass := vVerifyMode div vDiv;
            vVerifyMode := vVerifyMode mod vDiv;
            vAttStatus := vVerifyMode div 256;
            vVerifyMode := vVerifyMode mod 256;
            stAttStatus := '';
            stAntipass := '';
            case vAttStatus of
                0: stAttStatus := '_DutyOn';
                1: stAttStatus := '_DutyOff';
                2: stAttStatus := '_OverOn';
                3: stAttStatus := '_OverOff';
                4: stAttStatus := '_GoIn';
                5: stAttStatus := '_GoOut';
            end;
            if vAntipass = 1 then stAntipass := '(AP_In)'
            else if vAntipass = 3 then stAntipass := '(AP_Out)';
            gridSLogData.Cells[0, i] := IntToStr(i);
            gridSLogData.Cells[1, i] := IntToStr(vTMachineNumber);
            gridSLogData.Cells[2, i] := IntToStr(vSEnrollNumber);
            gridSLogData.Cells[3, i] := IntToStr(vSMachineNumber);
            if vVerifyMode = 1 then
                gridSLogData.Cells[4, i] := 'Fp'
            else if vVerifyMode = 2 then
                gridSLogData.Cells[4, i] := 'Password'
            else if vVerifyMode = 3 then
                gridSLogData.Cells[4, i] := 'Card'
            else if vVerifyMode = 4 then
                gridSLogData.Cells[4, i] := 'FP+Card'
            else if vVerifyMode = 5 then
                gridSLogData.Cells[4, i] := 'FP+Pwd'
            else if vVerifyMode = 6 then
                gridSLogData.Cells[4, i] := 'Card+Pwd'
            else if vVerifyMode = 7 then
                gridSLogData.Cells[4, i] := 'FP+Card+Pwd'
            else if vVerifyMode = 10 then
                gridSLogData.Cells[4, i] := 'Hand Lock'
            else if vVerifyMode = 11 then
                gridSLogData.Cells[4, i] := 'Prog Lock'
            else if vVerifyMode = 12 then
                gridSLogData.Cells[4, i] := 'Prog Open'
            else if vVerifyMode = 13 then
                gridSLogData.Cells[4, i] := 'Prog Close'
            else if vVerifyMode = 14 then
                gridSLogData.Cells[4, i] := 'Auto Recover'
            else if vVerifyMode = 20 then
                gridSLogData.Cells[4, i] := 'Lock Over'
            else if vVerifyMode = 21 then
                gridSLogData.Cells[4, i] := 'Illegal Open'
            else if vVerifyMode = 22 then
                gridSLogData.Cells[4, i] := 'Duress alarm'
            else if vVerifyMode = 23 then
                gridSLogData.Cells[4, i] := 'Tamper detect'
            else
                gridSLogData.Cells[4, i] := '--';

            if (1 <= vVerifyMode) and (vVerifyMode <= 7) then
                gridSLogData.Cells[4, i] := gridSLogData.Cells[4, i] + stAttStatus;

            gridSLogData.Cells[4, i] := gridSLogData.Cells[4, i] + stAntipass;
            gridSLogData.Cells[5, i] := IntToStr(vYear) + '/' + Format('%.2d', [vMonth]) + '/' + Format('%.2d', [vDay]) + ' ' + Format('%.2d', [vHour]) + ':' + Format('%.2d', [vMinute]);
            LabelTotal.Caption := 'Total : ' + IntToStr(i);
            Inc(i);
        end;
        lblMessage.Caption := 'ReadGeneralLogData OK';
    end;
    frmLog.Cursor := crDefault;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLog.cmdAllGLogDataClick(Sender: TObject);
const gstrLogItem:array[0..5] of string = ( '', 'PhotoNo', 'EnrollNo', 'EMachineNo', 'VeriMode', 'DateTime');
var
    vRet            :Boolean;
    vTMachineNumber :Integer;
    vSMachineNumber :Integer;
    vSEnrollNumber  :Integer;
    vVerifyMode     :Integer;
    vYear           :Integer;
    vMonth          :Integer;
    vDay            :Integer;
    vHour           :Integer;
    vMinute         :Integer;
    vSecond         :Integer;
    vErrorCode      :Integer;
    i               :Integer;
    vAttStatus      :Integer;
    vAntipass       :Integer;
    vDiv            :Integer;
    stAttStatus     :string;
    stAntipass      :string;
begin
    vDiv := 65536;
    lblMessage.Caption := 'Waiting...';
    LabelTotal.Caption := 'Total : ';
    gridSLogData.RowCount := 2;
    gridSLogData.ColCount := 6;
    gridSLogData.Rows[1].Clear;
    gridSLogData.ColWidths[0] := 40;
    for i := 1 to 5 do
    begin
        gridSLogData.Cells[i, 0] := gstrLogItem[i];
        gridSLogData.ColWidths[i] := 70;
    end;
    gridSLogData.ColWidths[4] := 120;
    gridSLogData.ColWidths[5] := 120;
   
    frmLog.Cursor := crHourGlass;
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        frmLog.Cursor := crDefault;
        Exit;
    end;
    vRet := bpc.ReadAllGLogData(gMachineNumber);
    if Not vRet then
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end
    else
        if chkAndDelete.Checked then bpc.EmptyGeneralLogData(gMachineNumber);
    if vRet then
    begin
        frmLog.Cursor := crHourGlass;
        lblMessage.Caption := 'Getting...';
        i := 1;
        while true do
        begin
            vRet := bpc.GetAllGLogData(gMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond);
            if Not vRet then Break;
            if vRet and (i <> 1) then gridSLogData.RowCount := gridSLogData.RowCount + 1;
            vAntipass := vVerifyMode div vDiv;
            vVerifyMode := vVerifyMode mod vDiv;
            vAttStatus := vVerifyMode div 256;
            vVerifyMode := vVerifyMode mod 256;
            stAttStatus := '';
            stAntipass := '';
            case vAttStatus of
                0: stAttStatus := '_DutyOn';
                1: stAttStatus := '_DutyOff';
                2: stAttStatus := '_OverOn';
                3: stAttStatus := '_OverOff';
                4: stAttStatus := '_GoIn';
                5: stAttStatus := '_GoOut';
            end;
            if vAntipass = 1 then stAntipass := '(AP_In)'
            else if vAntipass = 3 then stAntipass := '(AP_Out)';
            gridSLogData.Cells[0, i] := IntToStr(i);
            gridSLogData.Cells[1, i] := IntToStr(vTMachineNumber);
            gridSLogData.Cells[2, i] := IntToStr(vSEnrollNumber);
            gridSLogData.Cells[3, i] := IntToStr(vSMachineNumber);
            if vVerifyMode = 1 then
                gridSLogData.Cells[4, i] := 'Fp'
            else if vVerifyMode = 2 then
                gridSLogData.Cells[4, i] := 'Password'
            else if vVerifyMode = 3 then
                gridSLogData.Cells[4, i] := 'Card'
            else if vVerifyMode = 4 then
                gridSLogData.Cells[4, i] := 'FP+Card'
            else if vVerifyMode = 5 then
                gridSLogData.Cells[4, i] := 'FP+Pwd'
            else if vVerifyMode = 6 then
                gridSLogData.Cells[4, i] := 'Card+Pwd'
            else if vVerifyMode = 7 then
                gridSLogData.Cells[4, i] := 'FP+Card+Pwd'
            else if vVerifyMode = 10 then
                gridSLogData.Cells[4, i] := 'Hand Lock'
            else if vVerifyMode = 11 then
                gridSLogData.Cells[4, i] := 'Prog Lock'
            else if vVerifyMode = 12 then
                gridSLogData.Cells[4, i] := 'Prog Open'
            else if vVerifyMode = 13 then
                gridSLogData.Cells[4, i] := 'Prog Close'
            else if vVerifyMode = 14 then
                gridSLogData.Cells[4, i] := 'Auto Recover'
            else if vVerifyMode = 20 then
                gridSLogData.Cells[4, i] := 'Lock Over'
            else if vVerifyMode = 21 then
                gridSLogData.Cells[4, i] := 'Illegal Open'
            else if vVerifyMode = 22 then
                gridSLogData.Cells[4, i] := 'Duress alarm'
            else if vVerifyMode = 23 then
                gridSLogData.Cells[4, i] := 'Tamper detect'
            else
                gridSLogData.Cells[4, i] := '--';

            if (1 <= vVerifyMode) and (vVerifyMode <= 7) then
                gridSLogData.Cells[4, i] := gridSLogData.Cells[4, i] + stAttStatus;

            gridSLogData.Cells[4, i] := gridSLogData.Cells[4, i] + stAntipass;
            gridSLogData.Cells[5, i] := IntToStr(vYear) + '/' + Format('%.2d', [vMonth]) + '/' + Format('%.2d', [vDay]) + ' ' + Format('%.2d', [vHour]) + ':' + Format('%.2d', [vMinute]);
            LabelTotal.Caption := 'Total : ' + IntToStr(i);
            Inc(i);
        end;
        lblMessage.Caption := 'ReadGeneralLogData OK';
    end;
    frmLog.Cursor := crDefault;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLog.cmdEmptyGLogClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;

    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    gridSLogData.RowCount := 2;
//    gridSLogData.ColCount := 6;
    gridSLogData.Rows[1].Clear;
    LabelTotal.Caption := 'Total : ';
    vRet := bpc.EmptyGeneralLogData(gMachineNumber);
    if vRet then lblMessage.Caption := 'EmptyGeneralLogData OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLog.cmdExitClick(Sender: TObject);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    Close;
end;

procedure TfrmLog.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

end.
