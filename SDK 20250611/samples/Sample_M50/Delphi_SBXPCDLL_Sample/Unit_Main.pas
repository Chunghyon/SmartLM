unit Unit_Main;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, OleCtrls;

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
    cmdDepartment: TButton;
    cmdScreenSaver: TButton;
    cmdTrMode: TButton;
    cmdDaigong: TButton;
    cmdAccessTz: TButton;
    cmdEventMoniter: TButton;
    Label4: TLabel;
    txtDeviceUuid: TEdit;
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
    procedure cmdDepartmentClick(Sender: TObject);
    procedure cmdDaigongClick(Sender: TObject);
    procedure cmdScreenSaverClick(Sender: TObject);
    procedure cmdEventMoniterClick(Sender: TObject);
    procedure EnableManagementGroup(bEnable: Boolean);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmMain: TfrmMain;

implementation

uses Unit_Enroll, Unit_SysInfo, Unit_Log, Unit_BellInfo, Unit_LockCtrl, Unit_Prtcode, Utils,
  Unit_TrMode, Unit_AccessTz, Unit_Department, Unit_Proxy, SBXPCDLL_API,
  Unit_ScreenSaver, Unit_EventCapture;

var mOpenFlag       :Boolean;
{$R *.dfm}

procedure TfrmMain.cmdOpenClick(Sender: TObject);
var lpszIPAddr      :WideString;
var lpszDeviceUUID  :WideString;
var nError          :LongInt;
begin
    gMachineNumber := StrToInt(cmbMachineNumber.Text);
    if optNetworkDevice.Checked then
    begin
        lpszIPAddr := txtIPAddress.Text;
        lpszDeviceUUID := txtDeviceUuid.Text;
        if (StrLen(PChar(string(lpszDeviceUUID))) >= 6) and (StrLen(PChar(string(lpszDeviceUUID))) <= 16) then
        begin
          gMachineNumber := _ConnectP2p(lpszDeviceUUID, lpszIPAddr, StrToInt(txtPortNo.Text), StrToInt(txtPassword.Text), nError);
          if gMachineNumber <> 0 then begin
            mOpenFlag := true;
            cmdOpen.Enabled := false;
            cmdClose.Enabled := true;
            EnableManagementGroup(true);

                        if (nError = 4) then
                            application.MessageBox( 'Relayed Connection!', 'SBXPC Sample')
                        else if (nError = 5) then
                            application.MessageBox('Direct Local Connection!', 'SBXPC Sample');
          end
          else begin
                        if (nError = 1) then
                            application.MessageBox('Cannot Connect To Server!', 'SBXPC Sample')
                        else if (nError = 2) then
                            application.MessageBox('Device Not Found!', 'SBXPC Sample')
                        else if (nError = 3) then
                            application.MessageBox('Password Mismatched!', 'SBXPC Sample')
                        else
                            application.MessageBox('Unknown Error!', 'SBXPC Sample');
          end;
        end
        else if _ConnectTcpip(gMachineNumber, lpszIPAddr, StrToInt(txtPortNo.Text), StrToInt(txtPassword.Text)) then
        begin
            mOpenFlag := true;
            cmdOpen.Enabled := false;
            cmdClose.Enabled := true;
            EnableManagementGroup(true);
        end;
    end;
    if optSerialDevice.Checked then
    begin
        if _ConnectSerial(gMachineNumber, StrToInt(cmbComPort.Text), StrToInt(cmbBaudrate.Text)) then
        begin
            mOpenFlag := true;
            cmdOpen.Enabled := false;
            cmdClose.Enabled := true;
            EnableManagementGroup(true);
        end;
    end;
    if optUSBDevice.Checked then
    begin
        if _ConnectSerial(gMachineNumber, 0, 0) then
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
        _Disconnect(gMachineNumber);
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
    if mOpenFlag then _Disconnect(gMachineNumber);
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

    _DotNET();
end;

procedure TfrmMain.EnableManagementGroup(bEnable : Boolean);
begin
    cmdEnrollData.Enabled := bEnable;
    cmdLogData.Enabled := bEnable;
    cmdSystemInfo.Enabled := bEnable;
    cmdProductCode.Enabled := bEnable;
    cmdBellInfo.Enabled := bEnable;
    cmdLockCtl.Enabled := bEnable;
    cmdTrMode.Enabled := bEnable;
    cmdAccessTz.Enabled := bEnable;
    cmdDepartment.Enabled := bEnable;
    cmdDaigong.Enabled := bEnable;
    cmdScreenSaver.Enabled := bEnable;
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

procedure TfrmMain.cmdDepartmentClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmDepartment.Show;
end;

procedure TfrmMain.cmdDaigongClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmProxy.Show;
end;

procedure TfrmMain.cmdScreenSaverClick(Sender: TObject);
begin
    frmMain.Visible := false;
    frmScreenSaver.Show;
end;

procedure TfrmMain.cmdEventMoniterClick(Sender: TObject);
begin
     frmMain.Visible := false;
     frmEventCapture.Show;
end;

end.
