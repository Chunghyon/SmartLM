unit Unit_LockCtrl;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmLockCtrl = class(TForm)
    lblMessage: TStaticText;
    cmdGetDoorStatus: TButton;
    cmdDoorOpen: TButton;
    cmdUncondOpen: TButton;
    cmdAutoRecover: TButton;
    cmdUncondClose: TButton;
    cmdRestart: TButton;
    cmdWarnCancel: TButton;
    GroupBox1: TGroupBox;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    btnReadDoorSetting: TButton;
    btnWriteDoorSetting: TButton;
    GroupBox2: TGroupBox;
    Label7: TLabel;
    txtUnlockGroup0: TEdit;
    txtUnlockGroup1: TEdit;
    Label8: TLabel;
    txtUnlockGroup2: TEdit;
    txtUnlockGroup3: TEdit;
    Label9: TLabel;
    Label10: TLabel;
    txtUnlockGroup4: TEdit;
    txtUnlockGroup5: TEdit;
    Label11: TLabel;
    txtUnlockGroup6: TEdit;
    txtUnlockGroup7: TEdit;
    Label12: TLabel;
    txtUnlockGroup8: TEdit;
    txtUnlockGroup9: TEdit;
    Label13: TLabel;
    txtUnlockGroup10: TEdit;
    txtUnlockGroup11: TEdit;
    Label14: TLabel;
    txtUnlockGroup12: TEdit;
    txtUnlockGroup13: TEdit;
    Label15: TLabel;
    txtUnlockGroup14: TEdit;
    txtUnlockGroup15: TEdit;
    Label16: TLabel;
    txtUnlockGroup16: TEdit;
    txtUnlockGroup17: TEdit;
    Label17: TLabel;
    txtUnlockGroup18: TEdit;
    txtUnlockGroup19: TEdit;
    Button3: TButton;
    Button4: TButton;
    cmbDoorSensorType: TComboBox;
    cmbAntipassUsage: TComboBox;
    cmbAntipassNo: TComboBox;
    cmbAntipassLocation: TComboBox;
    txtLockReleaseTime: TEdit;
    txtDoorOpenTimeout: TEdit;
    Label18: TLabel;
    txtDoorNumber: TEdit;
    procedure cmdGetDoorStatusClick(Sender: TObject);
    procedure cmdDoorOpenClick(Sender: TObject);
    procedure cmdAutoRecoverClick(Sender: TObject);
    procedure cmdUncondOpenClick(Sender: TObject);
    procedure cmdUncondCloseClick(Sender: TObject);
    procedure cmdRestartClick(Sender: TObject);
    procedure cmdWarnCancelClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure btnReadDoorSettingClick(Sender: TObject);
    procedure btnWriteDoorSettingClick(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button4Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmLockCtrl: TfrmLockCtrl;
  ctrlUnlockGroups : array[0..20] of TEdit;

implementation

uses Unit_Main, Utils, SBXPCLib_TLB;

{$R *.dfm}
var
  bpc         :TSBXPC;

procedure TfrmLockCtrl.cmdGetDoorStatusClick(Sender: TObject);
var
  vStatus : Integer;
  vErrorCode : Integer;
  vRet : Boolean;
  lDoorNumber : Integer;
  strXML : WideString;
begin
    lDoorNumber := StrToInt(txtDoorNumber.Text);
    vErrorCode := 0;

    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'GetDoorStatusMulti');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);

    vRet := bpc.GeneralOperationXML(strXML);
    if vRet then
    begin
        vStatus := bpc.XML_ParseLong(strXML, 'DoorStatus');
        case vStatus of
            1:lblMessage.Caption := 'Uncond Door Open State!';
            2:lblMessage.Caption := 'Uncond Door Close State!';
            3:lblMessage.Caption := 'Door Open State!';
            4:lblMessage.Caption := 'Auto Recover State!';
            5:lblMessage.Caption := 'Door Close State!';
            6:lblMessage.Caption := 'Watching for Close!';
            7:lblMessage.Caption := 'Illegal open!';
            else lblMessage.Caption := 'User State !';
        end;
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdDoorOpenClick(Sender: TObject);
var
    vRet        : Boolean;
    vErrorCode  : Integer;
    strXML      : WideString;
    lDoorNumber : Integer;
begin
    lDoorNumber := StrToInt(txtDoorNumber.Text);
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'SetDoorStatusMulti');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);
    bpc.XML_AddInt(strXML, 'DoorStatus', 3);

    vRet := bpc.GeneralOperationXML(strXML);
    
    if vRet then lblMessage.Caption := 'Door Open Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdAutoRecoverClick(Sender: TObject);
var
    vRet        : Boolean;
    vErrorCode  : Integer;
    lDoorNumber : Integer;
    strXML      : WideString;
    
begin
    vErrorCode := 0;
    lDoorNumber := StrToInt(txtDoorNumber.Text);
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'SetDoorStatusMulti');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);
    bpc.XML_AddInt(strXML, 'DoorStatus', 4);
    
    vRet := bpc.GeneralOperationXML(strXML);
    if vRet then lblMessage.Caption := 'Auto Recover Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdUncondOpenClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
    strXML      : WideString;
    lDoorNumber : Integer;
begin
    vErrorCode := 0;
    lDoorNumber := StrToInt(txtDoorNumber.Text);
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'SetDoorStatusMulti');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);
    bpc.XML_AddInt(strXML, 'DoorStatus', 1);
    vRet := bpc.GeneralOperationXML(strXML);
    if vRet then lblMessage.Caption := 'Uncond Door Open Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdUncondCloseClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
    strXML      : WideString;
    lDoorNumber : Integer;
begin
    lDoorNumber := StrToInt(txtDoorNumber.Text);
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'SetDoorStatusMulti');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);
    bpc.XML_AddInt(strXML, 'DoorStatus', 2);
    vRet := bpc.GeneralOperationXML(strXML);
    if vRet then lblMessage.Caption := 'Uncond Door Close Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdRestartClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
    strXML      : WideString;
    lDoorNumber : Integer;
    
begin
    vErrorCode := 0;
    lDoorNumber := StrToInt(txtDoorNumber.Text);
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'SetDoorStatusMulti');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);
    bpc.XML_AddInt(strXML, 'DoorStatus', 5);
    vRet := bpc.GeneralOperationXML(strXML);
    if vRet then lblMessage.Caption := 'Reboot Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.cmdWarnCancelClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
    strXML      : WideString;
    lDoorNumber : Integer;
    
begin
    vErrorCode := 0;
    lDoorNumber := StrToInt(txtDoorNumber.Text);
    
    lblMessage.Caption := 'Waiting...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'SetDoorStatusMulti');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);
    bpc.XML_AddInt(strXML, 'DoorStatus', 6);
    
    vRet := bpc.GeneralOperationXML(strXML);
    if vRet then lblMessage.Caption := 'Warning cancel Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmLockCtrl.btnReadDoorSettingClick(Sender: TObject);
var
    bRet : Boolean;
    vErrorCode : Integer;
    lDoorNumber : Integer;
    lInfo : Integer;
    strXML : WideString;
    
begin
    lblMessage.Caption := 'Waiting...';

    bRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not bRet Then
    begin
      lblMessage.Caption := GSTR_NODEVICE;
      Exit;
    end;

    lDoorNumber := StrToInt(txtDoorNumber.Text);

    strXML := MakeXMLCommandHeader(bpc, 'GetDoorParam');
    bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);

    bRet := bpc.GeneralOperationXML(strXML);
    if bRet then
    begin
      cmbDoorSensorType.ItemIndex := bpc.XML_ParseInt(strXML, 'DoorSensorType');
      txtLockReleaseTime.Text := IntToStr(bpc.XML_ParseInt(strXML, 'LockReleaseTime'));
      txtDoorOpenTimeout.Text := IntToStr(bpc.XML_ParseInt(strXML, 'DoorOpenTimeout'));
      cmbAntipassUsage.ItemIndex := 1 - bpc.XML_ParseInt(strXML, 'DoorOpenTimeout');
      cmbAntipassNo.ItemIndex := bpc.XML_ParseInt(strXML, 'AntipassNo');
      cmbAntipassLocation.ItemIndex := bpc.XML_ParseInt(strXML, 'Location');
      lblMessage.Caption := 'Success!';
    end
    else
    begin
      bpc.GetLastError(vErrorCode);
      lblMessage.Caption := ErrorPrint(vErrorCode);
    end;

    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.btnWriteDoorSettingClick(Sender: TObject);
var
    bRet : Boolean;
    vErrorCode : Integer;
    lDoorNumber : Integer;
    lInfo : Integer;
    strXML : WideString;
begin
  lblMessage.Caption := 'Waiting...';

  bRet := bpc.EnableDevice(gMachineNumber, 0);
  if Not bRet then
  begin
    lblMessage.Caption := GSTR_NODEVICE;
    Exit;
  end;

  lDoorNumber := StrToInt(txtDoorNumber.Text);
  strXML := MakeXMLCommandHeader(bpc, 'SetDoorParam');
  bpc.XML_AddInt(strXML, 'DoorNo', lDoorNumber);

  bpc.XML_AddInt(strXML, 'DoorSensorType', cmbDoorSensorType.ItemIndex);
  bpc.XML_AddInt(strXML, 'LockReleaseTime', StrToInt(txtLockReleaseTime.Text));
  bpc.XML_AddInt(strXML, 'DoorOpenTimeout', StrToInt(txtDoorOpenTimeout.Text));
  bpc.XML_AddInt(strXML, 'UseAntipass', 1 - cmbAntipassUsage.ItemIndex);
  bpc.XML_AddInt(strXML, 'AntipassNo', cmbAntipassNo.ItemIndex);
  bpc.XML_AddInt(strXML, 'Location', cmbAntipassLocation.ItemIndex);

  bRet := bpc.GeneralOperationXML(strXML);
  if bRet then
  begin
    lblMessage.Caption := 'Success!';
  end
  else
  begin
    bpc.GetLastError(vErrorCode);
    lblMessage.Caption := ErrorPrint(vErrorCode);
  end;

  bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmLockCtrl.Button3Click(Sender: TObject);
var
    bRet : Boolean;
    vErrorCode : Integer;
    lInfo : Integer;
    strXML : WideString;
    i : Integer;
    unlockgroups : array[0..20] of byte;

begin
    lblMessage.Caption := 'Waiting...';

    bRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not bRet then
    begin
      lblMessage.Caption := GSTR_NODEVICE;
      Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'GetUnlockgroup');

    bRet := bpc.GeneralOperationXML(strXML);
    if bRet then
    begin
      lInfo := Integer(addr(unlockgroups));
      bpc.XML_ParseBinaryLong(strXML, 'UnlockGroupBinary', lInfo, 2 * 10);
      for i:= 0 to 19 do
      begin
        ctrlUnlockGroups[i].Text := IntToStr(unlockgroups[i]);
      end;
      lblMessage.Caption := 'GetUnlockgroup Success!';
    end
    else
    begin
      bpc.GetLastError(vErrorCode);
      lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);    
end;

procedure TfrmLockCtrl.FormCreate(Sender: TObject);
begin
  ctrlUnlockGroups[0] := txtUnlockGroup0;
  ctrlUnlockGroups[1] := txtUnlockGroup1;
  ctrlUnlockGroups[2] := txtUnlockGroup2;
  ctrlUnlockGroups[3] := txtUnlockGroup3;
  ctrlUnlockGroups[4] := txtUnlockGroup4;
  ctrlUnlockGroups[5] := txtUnlockGroup5;
  ctrlUnlockGroups[6] := txtUnlockGroup6;
  ctrlUnlockGroups[7] := txtUnlockGroup7;
  ctrlUnlockGroups[8] := txtUnlockGroup8;
  ctrlUnlockGroups[9] := txtUnlockGroup9;
  ctrlUnlockGroups[10] := txtUnlockGroup10;
  ctrlUnlockGroups[11] := txtUnlockGroup11;
  ctrlUnlockGroups[12] := txtUnlockGroup12;
  ctrlUnlockGroups[13] := txtUnlockGroup13;
  ctrlUnlockGroups[14] := txtUnlockGroup14;
  ctrlUnlockGroups[15] := txtUnlockGroup15;
  ctrlUnlockGroups[16] := txtUnlockGroup16;
  ctrlUnlockGroups[17] := txtUnlockGroup17;
  ctrlUnlockGroups[18] := txtUnlockGroup18;
  ctrlUnlockGroups[19] := txtUnlockGroup19;
  bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
end;

procedure TfrmLockCtrl.Button4Click(Sender: TObject);
var
    bRet : Boolean;
    vErrorCode : Integer;
    strXML : WideString;
    i : Integer;
    lInfo : Integer;
    unlockgroups : array[0..20] of byte;

begin
  lblMessage.Caption := 'Waiting...';

  bRet := bpc.EnableDevice(gMachineNumber, 0);
  if Not bRet then
  begin
      lblMessage.Caption := GSTR_NODEVICE;
      Exit;
  end;

  for i := 0 to 19 do
  begin
    unlockgroups[i] := StrToInt(ctrlUnlockGroups[i].Text);
  end;

  lInfo := Integer(addr(unlockgroups));
  strXML := MakeXMLCommandHeader(bpc, 'SetUnlockgroup');
  bpc.XML_AddBinaryLong(strXML, 'UnlockGroupBinary', lInfo, 2 * 10);

  bRet := bpc.GeneralOperationXML(strXML);
  if bRet then
  begin
    lblMessage.Caption := 'SetUnlockgroup Success!';
  end
  else
  begin
    bpc.GetLastError(vErrorCode);
    lblMessage.Caption := ErrorPrint(vErrorCode);
  end;

  bpc.EnableDevice(gMachineNumber, 1);
end;

end.
