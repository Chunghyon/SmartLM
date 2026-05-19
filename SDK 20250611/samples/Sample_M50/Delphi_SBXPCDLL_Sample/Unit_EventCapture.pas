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
  private
    { Private declarations }
  public
    { Public declarations }
  end;

  procedure OnReceiveEventXML(var context: Pointer; eventXML: PChar); stdcall;

var
  frmEventCapture: TfrmEventCapture;
  EventListBox : TListBox;

implementation

{$R *.dfm}

uses Unit_Main, Utils, SBXPCDLL_API;

procedure TfrmEventCapture.FormShow(Sender: TObject);
begin
    cmbComPort.ItemIndex := 0;
    cmbBaudrate.ItemIndex := 4;
    SwitchMode(true);
    cmdStartEventCapture.Enabled := true;
    cmdStopEventCapture.Enabled := false;
    EventListBox := lstEvent;
end;

procedure TfrmEventCapture.cmdClearClick(Sender: TObject);
begin
    lstEvent.Clear();
end;

procedure TfrmEventCapture.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    _StopEventCapture();
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
var
  eventProcessFunc : CB_FIRE_EVENT;
  
begin
    eventProcessFunc := OnReceiveEventXML;
    if optNetworkDevice.Checked then
    begin
        _StartEventCapture(0, pubIPAddrToLong(txtIPAddress.Text), StrToInt(txtPortNo.Text), eventProcessFunc, Nil);
    end
    else
    begin
        _StartEventCapture(1, cmbComPort.ItemIndex + 1, StrToInt(cmbBaudrate.Text), eventProcessFunc, Nil);
    end;
    cmdStartEventCapture.Enabled := false;
    cmdStopEventCapture.Enabled := true;
    optNetworkDevice.Enabled := false;
    optSerialDevice.Enabled := false;
end;

procedure TfrmEventCapture.cmdStopEventCaptureClick(Sender: TObject);
begin
    _StopEventCapture();
    cmdStartEventCapture.Enabled := true;
    cmdStopEventCapture.Enabled := false;
    optNetworkDevice.Enabled := true;
    optSerialDevice.Enabled := true;
end;

procedure OnReceiveEventXML(var context: Pointer; eventXML: PChar) stdcall;
var
    year, month, day, hour, minute, second, weekday : Integer;
    strXML                                          : WideString;
    eventItemString                                 : String;
    strMachineType, strEventType                    : WideString;
    machinId, managerId, userId, result             : Integer;
    str1, str2, str3, str4                          : WideString;

begin
    strXML := WideString(eventXML);

    year := _XML_ParseInt(strXML, 'Year');
    month := _XML_ParseInt(strXML, 'Month');
    day := _XML_ParseInt(strXML, 'Day');
    hour := _XML_ParseInt(strXML, 'Hour');
    minute := _XML_ParseInt(strXML, 'Minute');
    second := _XML_ParseInt(strXML, 'Second');
    weekday := _XML_ParseInt(strXML, 'Weekday');

    machinId := _XML_ParseInt(strXML, 'MachineID');
    _XML_ParseString(strXML, 'MachineType', strMachineType);
    _XML_ParseString(strXML, 'EventType', strEventType);

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
        managerId := _XML_ParseInt(strXML, 'ManagerID');
        userId := _XML_ParseInt(strXML, 'UserID');
        _XML_ParseString(strXML, 'Action', str1);
        result := _XML_ParseLong(strXML, 'Result');
        eventItemString := eventItemString + 'Manager ID = ' + Format('%.05d, ', [managerId]);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'Action = ' + str1 + ', ';
        eventItemString := eventItemString + 'Result = ' + IntToStr(result);
    end
    else if WideCompareText('Time Log', strEventType) = 0 then
    begin
        userId := _XML_ParseInt(strXML, 'UserID');
        _XML_ParseString(strXML, 'AttendanceStatus', str1);
        _XML_ParseString(strXML, 'VerificationMode', str2);
        _XML_ParseString(strXML, 'AntipassStatus', str3);
        _XML_ParseString(strXML, 'Photo', str4);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'AttendanceStatus = ' + str1 + ', ';
        eventItemString := eventItemString + 'VerificationMode = ' + str2 + ', ';
        eventItemString := eventItemString + 'AntipassStatus = ' + str3 + ', ';
        eventItemString := eventItemString + 'Photo = ' + str4;
    end
    else if WideCompareText('Verification Success', strEventType) = 0 then
    begin
        userId := _XML_ParseInt(strXML, 'UserID');
        _XML_ParseString(strXML, 'VerificationMode', str1);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'VerificationMode = ' + str1;
    end
    else if WideCompareText('Verification Failure', strEventType) = 0 then
    begin
        userId := _XML_ParseInt(strXML, 'UserID');
        _XML_ParseString(strXML, 'VerificationMode', str1);
        _XML_ParseString(strXML, 'ReasonOfFailure', str2);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'VerificationMode = ' + str1 + ', ';
        eventItemString := eventItemString + 'ReasonOfFailure = ' + str2;
    end
    else if WideCompareText('Alarm On', strEventType) = 0 then
    begin
        userId := _XML_ParseInt(strXML, 'UserID');
        _XML_ParseString(strXML, 'AlarmType', str1);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d, ', [userId]);
        eventItemString := eventItemString + 'AlarmType = ' + str1;
    end
    else if WideCompareText('Alarm Off', strEventType) = 0 then
    begin
        userId := _XML_ParseInt(strXML, 'UserID');
        _XML_ParseString(strXML, 'AlarmType', str1);
        _XML_ParseString(strXML, 'AlarmOffMethod', str2);
        eventItemString := eventItemString + 'User ID = ' + Format('%.05d', [userId]);
        eventItemString := eventItemString + 'AlarmType = ' + str1 + ', ';
        eventItemString := eventItemString + 'AlarmOffMethod = ' + str2;
    end
    else if WideCompareText('DoorBell', strEventType) = 0 then
    begin
        _XML_ParseString(strXML, 'InputType', str1);
        eventItemString := eventItemString + 'Input Type = ' + str1;
    end;

    EventListBox.Items.Add(eventItemString); 
end;

end.
