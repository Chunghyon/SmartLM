unit Unit_EventCapture;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmEventCapture = class(TForm)
    GroupBox3: TGroupBox;
    lblComPort: TLabel;
    lblBaudrate: TLabel;
    cmbComPort: TComboBox;
    cmbBaudrate: TComboBox;
    optSerialDevice: TRadioButton;
    GroupBox4: TGroupBox;
    lblIPAddress: TLabel;
    lblPortNo: TLabel;
    optNetworkDevice: TRadioButton;
    txtIPAddress: TEdit;
    txtPortNo: TEdit;
    cmdStartEventCapture: TButton;
    cmdStopEventCapture: TButton;
    cmdClear: TButton;
    lstEvent: TListBox;
    procedure FormShow(Sender: TObject);
    procedure cmdClearClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure SwitchMode(network : boolean);
    procedure optSerialDeviceClick(Sender: TObject);
    procedure optNetworkDeviceClick(Sender: TObject);
    procedure cmdStartEventCaptureClick(Sender: TObject);
    procedure cmdStopEventCaptureClick(Sender: TObject);
    procedure OnReceiveEventXML(eventXML : WideString);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmEventCapture: TfrmEventCapture;

implementation

{$R *.dfm}

uses Unit_Main, Utils, SBXPCLib_TLB;

var
    bpc                 :TSBXPC;

procedure TfrmEventCapture.FormShow(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
    cmbComPort.ItemIndex := 0;
    cmbBaudrate.ItemIndex := 4;
    SwitchMode(true);
    cmdStartEventCapture.Enabled := true;
    cmdStopEventCapture.Enabled := false;
end;

procedure TfrmEventCapture.cmdClearClick(Sender: TObject);
begin
    lstEvent.Clear();
end;

procedure TfrmEventCapture.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    bpc.StopEventCapture();
end;

procedure TfrmEventCapture.SwitchMode(network : boolean);
begin
    cmbComPort.Enabled := not network;
    cmbBaudrate.Enabled := not network;
    txtIPAddress.Enabled := network;
    txtPortNo.Enabled := network;
end;

procedure TfrmEventCapture.optSerialDeviceClick(Sender: TObject);
begin
    if optSerialDevice.Checked then optNetworkDevice.Checked := false;
    SwitchMode(optNetworkDevice.Checked);
end;

procedure TfrmEventCapture.optNetworkDeviceClick(Sender: TObject);
begin
    if optNetworkDevice.Checked then optSerialDevice.Checked := false;
    SwitchMode(optNetworkDevice.Checked);
end;

procedure TfrmEventCapture.cmdStartEventCaptureClick(Sender: TObject);
begin
    if optNetworkDevice.Checked then
    begin
        bpc.StartEventCapture(0, pubIPAddrToLong(txtIPAddress.Text), StrToInt(txtPortNo.Text));
    end
    else
    begin
        bpc.StartEventCapture(1, cmbComPort.ItemIndex + 1, StrToInt(cmbBaudrate.Text));
    end;
    cmdStartEventCapture.Enabled := false;
    cmdStopEventCapture.Enabled := true;
    optNetworkDevice.Enabled := false;
    optSerialDevice.Enabled := false;
end;

procedure TfrmEventCapture.cmdStopEventCaptureClick(Sender: TObject);
begin
    bpc.StopEventCapture();
    cmdStartEventCapture.Enabled := true;
    cmdStopEventCapture.Enabled := false;
    optNetworkDevice.Enabled := true;
    optSerialDevice.Enabled := true;
end;

procedure TfrmEventCapture.OnReceiveEventXML(eventXML : WideString);
var
    year, month, day, hour, minute, second, weekday : Integer;
    strXML                                          : WideString;
    eventItemString                                 : String;
    strMachineType, strEventType                    : WideString;
    machinId, managerId, userId, result             : Integer;
    str1, str2, str3, str4                          : WideString;

begin
    strXML := eventXML;
    year := bpc.XML_ParseInt(strXML, 'Year');
    month := bpc.XML_ParseInt(strXML, 'Month');
    day := bpc.XML_ParseInt(strXML, 'Day');
    hour := bpc.XML_ParseInt(strXML, 'Hour');
    minute := bpc.XML_ParseInt(strXML, 'Minute');
    second := bpc.XML_ParseInt(strXML, 'Second');
    weekday := bpc.XML_ParseInt(strXML, 'Weekday');

    machinId := bpc.XML_ParseInt(strXML, 'MachineID');
    bpc.XML_ParseString(strXML, 'MachineType', strMachineType);
    bpc.XML_ParseString(strXML, 'EventType', strEventType);

    eventItemString := Format('%.02d-', [year]);
    eventItemString := eventItemString + Format('%.02d-', [month]);
    eventItemString := eventItemString + Format('%.02d ', [day]);
    eventItemString := eventItemString + Format('%.02d:', [hour]);
    eventItemString := eventItemString + Format('%.02d:', [minute]);
    eventItemString := eventItemString + Format('%.02d ', [second]);

    eventItemString := eventItemString + '[' + strMachineType + ':';
    eventItemString := eventItemString + IntToStr(machinId) + '] ';
    eventItemString := eventItemString + strEventType + ', ';

    if WideCompareText('Management Log', strEventType) = 0 then
    begin
        managerId := bpc.XML_ParseInt(strXML, 'ManagerID');
        userId := bpc.XML_ParseInt(strXML, 'UserID');
        bpc.XML_ParseString(strXML, 'Action', str1);
        result := bpc.XML_ParseLong(strXML, 'Result');
        eventItemString := eventItemString + 'Manager ID = ' + Format('%.05d, ', [managerId]);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'Action = ' + str1 + ', ';
        eventItemString := eventItemString + 'Result = ' + IntToStr(result);
    end
    else if WideCompareText('Time Log', strEventType) = 0 then
    begin
        userId := bpc.XML_ParseInt(strXML, 'UserID');
        bpc.XML_ParseString(strXML, 'AttendanceStatus', str1);
        bpc.XML_ParseString(strXML, 'VerificationMode', str2);
        bpc.XML_ParseString(strXML, 'AntipassStatus', str3);
        bpc.XML_ParseString(strXML, 'Photo', str4);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'AttendanceStatus = ' + str1 + ', ';
        eventItemString := eventItemString + 'VerificationMode = ' + str2 + ', ';
        eventItemString := eventItemString + 'AntipassStatus = ' + str3 + ', ';
        eventItemString := eventItemString + 'Photo = ' + str4;
    end
    else if WideCompareText('Verification Success', strEventType) = 0 then
    begin
        userId := bpc.XML_ParseInt(strXML, 'UserID');
        bpc.XML_ParseString(strXML, 'VerificationMode', str1);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'VerificationMode = ' + str1;
    end
    else if WideCompareText('Verification Failure', strEventType) = 0 then
    begin
        userId := bpc.XML_ParseInt(strXML, 'UserID');
        bpc.XML_ParseString(strXML, 'VerificationMode', str1);
        bpc.XML_ParseString(strXML, 'ReasonOfFailure', str2);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'VerificationMode = ' + str1 + ', ';
        eventItemString := eventItemString + 'ReasonOfFailure = ' + str2;
    end
    else if WideCompareText('Alarm On', strEventType) = 0 then
    begin
        userId := bpc.XML_ParseInt(strXML, 'UserID');
        bpc.XML_ParseString(strXML, 'AlarmType', str1);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'AlarmType = ' + str1;
    end
    else if WideCompareText('Alarm Off', strEventType) = 0 then
    begin
        userId := bpc.XML_ParseInt(strXML, 'UserID');
        bpc.XML_ParseString(strXML, 'AlarmType', str1);
        bpc.XML_ParseString(strXML, 'AlarmOffMethod', str2);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d', [userId]);
        eventItemString := eventItemString + 'AlarmType = ' + str1 + ', ';
        eventItemString := eventItemString + 'AlarmOffMethod = ' + str2;
    end
    else if WideCompareText('DoorBell', strEventType) = 0 then
    begin
        bpc.XML_ParseString(strXML, 'InputType', str1);
        eventItemString := eventItemString + 'Input Type = ' + str1;
    end;

    lstEvent.Items.Add(eventItemString);
end;

end.
