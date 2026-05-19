unit Unit_Main;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, OleCtrls, SBXPCLib_TLB;

type
  TfrmMain = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    GroupBox1: TGroupBox;
    Label3: TLabel;
    cmbMachineNumber: TComboBox;
    cmdOpen: TButton;
    cmdClose: TButton;
    GroupBox2: TGroupBox;
    cmdEnrollData: TButton;
    cmdLogData: TButton;
    cmdSystemInfo: TButton;
    cmdLockCtl: TButton;
    cmdBellInfo: TButton;
    cmdProductCode: TButton;
    cmdExit: TButton;
    GroupBox3: TGroupBox;
    lblComPort: TLabel;
    lblBaudrate: TLabel;
    cmbComPort: TComboBox;
    cmbBaudrate: TComboBox;
    optSerialDevice: TRadioButton;
    GroupBox4: TGroupBox;
    optNetworkDevice: TRadioButton;
    lblIPAddress: TLabel;
    lblPortNo: TLabel;
    lblPassword: TLabel;
    txtIPAddress: TEdit;
    txtPortNo: TEdit;
    txtPassword: TEdit;
    optUSBDevice: TRadioButton;
    cmdHoliday: TButton;
    cmdModeTZone: TButton;
    cmdAccessTz: TButton;
    cmdEventMoniter: TButton;
    SBXPC1: TSBXPC;
    procedure cmdOpenClick(Sender: TObject);
    procedure cmdCloseClick(Sender: TObject);
    procedure optSerialDeviceClick(Sender: TObject);
    procedure optNetworkDeviceClick(Sender: TObject);
    procedure optUSBDeviceClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure cmdBellInfoClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure cmdProductCodeClick(Sender: TObject);
    procedure cmdLogDataClick(Sender: TObject);
    procedure cmdSystemInfoClick(Sender: TObject);
    procedure cmdLockCtlClick(Sender: TObject);
    procedure cmdEnrollDataClick(Sender: TObject);
    procedure cmdTrModeClick(Sender: TObject);
    procedure cmdAccessTzClick(Sender: TObject);
    procedure cmdEventMoniterClick(Sender: TObject);
    procedure EnableManagementGroup(bEnable: Boolean);
    procedure cmdHolidayClick(Sender: TObject);
    procedure cmdModeTZoneClick(Sender: TObject);
    procedure SBXPC1ReceiveEventXML(ASender: TObject;
      const lpszEventXML: WideString);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmMain: TfrmMain;

implementation

uses Unit_Enroll, Unit_SysInfo, Unit_Log, Unit_BellInfo, Unit_LockCtrl, Unit_Prtcode, Utils,
  Unit_TrMode, Unit_AccessTz,
  Unit_EventCapture, Unit_Holiday, Unit_Tmode;

var mOpenFlag       :Boolean;
{$R *.dfm}

procedure TfrmMain.cmdOpenClick(Sender: TObject);
var lpszIPAddr      :WideString;
var nError          :LongInt;
begin
    gMachineNumber := StrToInt(cmbMachineNumber.Text);
    if optNetworkDevice.Checked then
    begin
        lpszIPAddr := txtIPAddress.Text;
        if SBXPC1.ConnectTcpip(gMachineNumber, lpszIPAddr, StrToInt(txtPortNo.Text), StrToInt(txtPassword.Text)) then
        begin
            mOpenFlag := true;
            cmdOpen.Enabled := false;
            cmdClose.Enabled := true;
            EnableManagementGroup(true);
        end;
    end;
    if optSerialDevice.Checked then
    begin
        if SBXPC1.ConnectSerial(gMachineNumber, StrToInt(cmbComPort.Text), StrToInt(cmbBaudrate.Text)) then
        begin
            mOpenFlag := true;
            cmdOpen.Enabled := false;
            cmdClose.Enabled := true;
            EnableManagementGroup(true);
        end;
    end;
    if optUSBDevice.Checked then
    begin
        if SBXPC1.ConnectSerial(gMachineNumber, 0, 0) then
        begin
            mOpenFlag := true;
            cmdOpen.Enabled := false;
            cmdClose.Enabled := true;
            EnableManagementGroup(true);
        end;
    end;
end;

procedure TfrmMain.cmdCloseClick(Sender: TObject);
begin
    if mOpenFlag then
    begin
        SBXPC1.Disconnect();
        mOpenFlag := false;
        cmdOpen.Enabled := true;
        cmdClose.Enabled := false;
        EnableManagementGroup(false);
    end;
end;

procedure TfrmMain.optSerialDeviceClick(Sender: TObject);
var lpszIPAddr      :WideString;
begin
    optSerialDevice.Checked := true;
    optNetworkDevice.Checked := false;
    optUSBDevice.Checked := false;
    if optSerialDevice.Checked then
    begin
        lblComPort.Enabled := true;
        cmbComPort.Enabled := true;
        lblBaudrate.Enabled := true;
        cmbBaudrate.Enabled := true;
        lblIPAddress.Enabled := false;
        txtIPAddress.Enabled := false;
        lblPortNo.Enabled := false;
        txtPortNo.Enabled := false;
        lblPassword.Enabled := false;
        txtPassword.Enabled := false;
    end
    else if optNetworkDevice.Checked then
    begin
        lblComPort.Enabled := false;
        cmbComPort.Enabled := false;
        lblBaudrate.Enabled := false;
        cmbBaudrate.Enabled := false;
        lblIPAddress.Enabled := true;
        txtIPAddress.Enabled := true;
        lblPortNo.Enabled := true;
        txtPortNo.Enabled := true;
        lblPassword.Enabled := true;
        txtPassword.Enabled := true;
        lpszIPAddr := txtIPAddress.Text;
    end
    else
    begin
        lblComPort.Enabled := false;
        cmbComPort.Enabled := false;
        lblBaudrate.Enabled := false;
        cmbBaudrate.Enabled := false;
        lblIPAddress.Enabled := false;
        txtIPAddress.Enabled := false;
        lblPortNo.Enabled := false;
        txtPortNo.Enabled := false;
        lblPassword.Enabled := false;
        txtPassword.Enabled := false;
    end;
end;

procedure TfrmMain.optNetworkDeviceClick(Sender: TObject);
var lpszIPAddr      :WideString;
begin
    optSerialDevice.Checked := false;
    optNetworkDevice.Checked := true;
    optUSBDevice.Checked := false;
    if optSerialDevice.Checked then
    begin
        lblComPort.Enabled := true;
        cmbComPort.Enabled := true;
        lblBaudrate.Enabled := true;
        cmbBaudrate.Enabled := true;
        lblIPAddress.Enabled := false;
        txtIPAddress.Enabled := false;
        lblPortNo.Enabled := false;
        txtPortNo.Enabled := false;
        lblPassword.Enabled := false;
        txtPassword.Enabled := false;
    end
    else if optNetworkDevice.Checked then
    begin
        lblComPort.Enabled := false;
        cmbComPort.Enabled := false;
        lblBaudrate.Enabled := false;
        cmbBaudrate.Enabled := false;
        lblIPAddress.Enabled := true;
        txtIPAddress.Enabled := true;
        lblPortNo.Enabled := true;
        txtPortNo.Enabled := true;
        lblPassword.Enabled := true;
        txtPassword.Enabled := true;
        lpszIPAddr := txtIPAddress.Text;
    end
    else
    begin
        lblComPort.Enabled := false;
        cmbComPort.Enabled := false;
        lblBaudrate.Enabled := false;
        cmbBaudrate.Enabled := false;
        lblIPAddress.Enabled := false;
        txtIPAddress.Enabled := false;
        lblPortNo.Enabled := false;
        txtPortNo.Enabled := false;
        lblPassword.Enabled := false;
        txtPassword.Enabled := false;
    end;
end;

procedure TfrmMain.optUSBDeviceClick(Sender: TObject);
var lpszIPAddr      :WideString;
begin
    optSerialDevice.Checked := false;
    optNetworkDevice.Checked := false;
    optUSBDevice.Checked := true;
    if optSerialDevice.Checked then
    begin
        lblComPort.Enabled := true;
        cmbComPort.Enabled := true;
        lblBaudrate.Enabled := true;
        cmbBaudrate.Enabled := true;
        lblIPAddress.Enabled := false;
        txtIPAddress.Enabled := false;
        lblPortNo.Enabled := false;
        txtPortNo.Enabled := false;
        lblPassword.Enabled := false;
        txtPassword.Enabled := false;
    end
    else if optNetworkDevice.Checked then
    begin
        lblComPort.Enabled := false;
        cmbComPort.Enabled := false;
        lblBaudrate.Enabled := false;
        cmbBaudrate.Enabled := false;
        lblIPAddress.Enabled := true;
        txtIPAddress.Enabled := true;
        lblPortNo.Enabled := true;
        txtPortNo.Enabled := true;
        lblPassword.Enabled := true;
        txtPassword.Enabled := true;
        lpszIPAddr := txtIPAddress.Text;
    end
    else
    begin
        lblComPort.Enabled := false;
        cmbComPort.Enabled := false;
        lblBaudrate.Enabled := false;
        cmbBaudrate.Enabled := false;
        lblIPAddress.Enabled := false;
        txtIPAddress.Enabled := false;
        lblPortNo.Enabled := false;
        txtPortNo.Enabled := false;
        lblPassword.Enabled := false;
        txtPassword.Enabled := false;
    end;
end;

procedure TfrmMain.cmdExitClick(Sender: TObject);
begin
    if mOpenFlag then SBXPC1.Disconnect();
    Close;
end;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
    optSerialDevice.Checked := true;
    lblComPort.Enabled := true;
    cmbComPort.Enabled := true;
    lblBaudrate.Enabled := true;
    cmbBaudrate.Enabled := true;

    optNetworkDevice.Checked := false;
    lblIPAddress.Enabled := false;
    txtIPAddress.Enabled := false;
    lblPortNo.Enabled := false;
    txtPortNo.Enabled := false;
    lblPassword.Enabled := false;
    txtPassword.Enabled := false;

    cmdOpen.Enabled := true;
    cmdClose.Enabled := false;
    EnableManagementGroup(false);

    mOpenFlag := false;
    cmbMachineNumber.Text := IntToStr(1);
    cmbComPort.Text := IntToStr(1);
    cmbBaudrate.Text := '115200';

    SBXPC1.DotNET();
end;

procedure TfrmMain.EnableManagementGroup(bEnable : Boolean);
begin
    cmdEnrollData.Enabled := bEnable;
    cmdLogData.Enabled := bEnable;
    cmdSystemInfo.Enabled := bEnable;
    cmdProductCode.Enabled := bEnable;
    cmdBellInfo.Enabled := bEnable;
    cmdLockCtl.Enabled := bEnable;
    cmdHoliday.Enabled := bEnable;
    cmdAccessTz.Enabled := bEnable;
    cmdModeTZone.Enabled := bEnable;
end;

procedure TfrmMain.cmdBellInfoClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmBellInfo.Show;
end;

procedure TfrmMain.cmdProductCodeClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmPrtCode.Show;
end;

procedure TfrmMain.cmdLogDataClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmLog.Show;
end;

procedure TfrmMain.cmdSystemInfoClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmSystemInfo.Show;
end;

procedure TfrmMain.cmdLockCtlClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmLockCtrl.Show;
end;

procedure TfrmMain.cmdEnrollDataClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmEnroll.Show;
end;

procedure TfrmMain.cmdTrModeClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmTrMode.Show;
end;

procedure TfrmMain.cmdAccessTzClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmAccessTz.Show;
end;

procedure TfrmMain.cmdEventMoniterClick(Sender: TObject);
begin
     frmMain.Visible := false;
     frmEventCapture.Show;
end;

procedure TfrmMain.cmdHolidayClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmHoliday.Show;
end;

procedure TfrmMain.cmdModeTZoneClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmTMode.Show;
end;

procedure TfrmMain.SBXPC1ReceiveEventXML(ASender: TObject;
  const lpszEventXML: WideString);
begin
  frmEventCapture.OnReceiveEventXML(lpszEventXML);
end;

end.
