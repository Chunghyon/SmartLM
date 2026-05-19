unit Unit_SysInfo;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmSystemInfo = class(TForm)
    lblMessage: TStaticText;
    cmdGetDeviceTime: TButton;
    cmdPowerOn: TButton;
    cmdEnableDevice: TButton;
    cmdSetDeviceTime: TButton;
    PowerOffDevice: TButton;
    cmdExit: TButton;
    Label1: TLabel;
    Label2: TLabel;
    cmbSatus: TComboBox;
    Label3: TLabel;
    txtSetDevInfo: TEdit;
    cmdGetDeviceInfo: TButton;
    cmdSetDeviceInfo: TButton;
    cmdGetDeviceStaus: TButton;
    chkEnableDevice: TCheckBox;
    procedure cmdGetDeviceTimeClick(Sender: TObject);
    procedure cmdSetDeviceTimeClick(Sender: TObject);
    procedure cmdPowerOnClick(Sender: TObject);
    procedure PowerOffDeviceClick(Sender: TObject);
    procedure cmdEnableDeviceClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure cmdGetDeviceInfoClick(Sender: TObject);
    procedure cmdSetDeviceInfoClick(Sender: TObject);
    procedure cmdGetDeviceStausClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
    function GetWeekDay(anDay :Integer):string;
  public
    { Public declarations }
  end;

var
  frmSystemInfo: TfrmSystemInfo;

implementation

uses Unit_Main, Utils, SBXPCDLL_API;

{$R *.dfm}

function TfrmSystemInfo.GetWeekDay(anDay:Integer):string;
begin
    case anDay of
        1: GetWeekDay := 'Sunday';
        2: GetWeekDay := 'Monday';
        3: GetWeekDay := 'Tuesday';
        4: GetWeekDay := 'Wednesday';
        5: GetWeekDay := 'Thursday';
        6: GetWeekDay := 'Friday';
        7: GetWeekDay := 'Saturday';
        else GetWeekDay := 'Sunday';
    end;
end;

procedure TfrmSystemInfo.cmdGetDeviceTimeClick(Sender: TObject);
var
    vYear, vMonth, vDay, vHour, vMinute, vSecond, vDayOfWeek :Integer;
    vErrorCode      :Integer;
    vRet            :Boolean;
    strDataTime     :string;
begin
    lblMessage.Caption := 'Waiting...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _GetDeviceTime(gMachineNumber, vYear, vMonth, vDay, vHour, vMinute, vSecond, vDayOfWeek);
    if vRet then
    begin
        if vDayOfWeek = 0 then vDayOfWeek := 7;
        strDataTime := 'Date = ' + IntToStr(vYear) + '/' + IntToStr(vMonth) + '/' + IntToStr(vDay) + ' , ' + GetWeekDay(vDayOfWeek) + ' , Time = ' + Format('%.2d', [vHour]) + ':' + Format('%.2d', [vMinute]);
        lblMessage.Caption := strDataTime;
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmSystemInfo.cmdSetDeviceTimeClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _SetDeviceTime(gMachineNumber);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmSystemInfo.cmdPowerOnClick(Sender: TObject);
begin
    _PowerOnAllDevice(gMachineNumber);
end;

procedure TfrmSystemInfo.PowerOffDeviceClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    vRet := _PowerOffDevice(gMachineNumber);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmSystemInfo.cmdEnableDeviceClick(Sender: TObject);
var
    vRet        :Boolean;
    vErrorCode  :Integer;
    vFlag       :LongBool;
begin
    vErrorCode := 0;
    
    lblMessage.Caption := 'Waiting...';
    if chkEnableDevice.Checked then vFlag := True
    else vFlag := False;
    vRet := _EnableDevice(gMachineNumber, vFlag);
    if vRet then
    begin
        if vFlag then
        begin
            lblMessage.Caption := 'Enable Device Success!';
            cmdEnableDevice.Caption := 'DisableDevice';
        end
        else
        begin
            lblMessage.Caption := 'Disable Device Success!';
            cmdEnableDevice.Caption := 'EnableDevice';
        end;
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
        Exit;
    end;
    if chkEnableDevice.Checked then chkEnableDevice.Checked := false
    else  chkEnableDevice.Checked := true;
end;

procedure TfrmSystemInfo.cmdExitClick(Sender: TObject);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    Close;
end;

procedure TfrmSystemInfo.cmdGetDeviceInfoClick(Sender: TObject);
var
    vRet            :Boolean;
    vErrorCode      :Integer;
    vInfo           :Integer;
    vValue          :Integer;
begin
    lblMessage.Caption := 'Working...';
    vInfo := cmbSatus.ItemIndex + 1;
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _GetDeviceInfo(gMachineNumber, vInfo, vValue);
    if vRet then
    begin
        case vInfo of
            1:lblMessage.Caption := '(1) = ManagerCount = ' + IntToStr(vValue);
            2:lblMessage.Caption := '(2) = Device ID = ' + IntToStr(vValue);
            3:lblMessage.Caption := '(3) = Language = ' + IntToStr(vValue);
            4:lblMessage.Caption := '(4) = PowerOffTime = ' + IntToStr(vValue);
            5:lblMessage.Caption := '(5) = Lock release time = ' + IntToStr(vValue);
            6:lblMessage.Caption := '(6) = GLogWarning = ' + IntToStr(vValue);
            7:lblMessage.Caption := '(7) = SLogWarning = ' + IntToStr(vValue);
            8:lblMessage.Caption := '(8) = ReVerifyTime = ' + IntToStr(vValue);
            9:lblMessage.Caption := '(9) = Baudrate = ' + IntToStr(vValue);
            10:lblMessage.Caption := '(10) = Parity check = ' + IntToStr(vValue);
            11:lblMessage.Caption := '(11) = Stop bit = ' + IntToStr(vValue);
            12:lblMessage.Caption := '(12) = Date Seperator = ' + IntToStr(vValue);
            13:lblMessage.Caption := '(13) = Identificatin mode = ' + IntToStr(vValue);
            14:lblMessage.Caption := '(14) = LockOperate = ' + IntToStr(vValue);
            15:lblMessage.Caption := '(15) = Door sensor type = ' + IntToStr(vValue);
            16:lblMessage.Caption := '(16) = Door open time limit = ' + IntToStr(vValue);
            17:lblMessage.Caption := '(17) = Anti-pass = ' + IntToStr(vValue);
            18:lblMessage.Caption := '(18) = Auto sleep time = ' + IntToStr(vValue);
            19:lblMessage.Caption := '(19) = Daylight offset = ' + IntToStr(vValue);
            20:lblMessage.Caption := '(20) = UDP Server = ' + pubLongToIPAddr(vValue);
            21:lblMessage.Caption := '(21) = DHCP Use = ' + IntToStr(vValue);
            22:lblMessage.Caption := '(22) = Main Lock Group = ' + IntToStr(vValue);
        end;
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmSystemInfo.cmdSetDeviceInfoClick(Sender: TObject);
var
    vRet            :Boolean;
    vErrorCode      :Integer;
    vInfo           :Integer;
    vValue          :Integer;
begin
    lblMessage.Caption := 'Waiting...';
    vInfo := cmbsatus.ItemIndex + 1;

    if vInfo <> 20 then vValue := StrToInt(txtSetDevInfo.Text)
    else vValue := pubIPAddrToLong(txtSetDevInfo.Text);
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _SetDeviceInfo(gMachineNumber, vInfo, vValue);
    if vRet then
    begin
        lblMessage.Caption := 'Success!';
        if vInfo = 2 then
        begin
            gMachineNumber := vValue;
            sleep(1000);
        end;
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmSystemInfo.cmdGetDeviceStausClick(Sender: TObject);
var
    vRet            :Boolean;
    vErrorCode      :Integer;
    vStatus         :Integer;
    vValue          :Integer;
begin
    lblMessage.Caption := 'Working...';
    vStatus := cmbSatus.ItemIndex + 1;
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _GetDeviceStatus(gMachineNumber, vStatus, vValue);
    if vRet then
    begin
        case vStatus of
            1:lblMessage.Caption := '(1) = Manager count = ' + IntToStr(vValue);
            2:lblMessage.Caption := '(2) = User count = ' + IntToStr(vValue);
            3:lblMessage.Caption := '(3) = Fp count = ' + IntToStr(vValue);
            4:lblMessage.Caption := '(4) = Password count = ' + IntToStr(vValue);
            5:lblMessage.Caption := '(5) = SLog count = ' + IntToStr(vValue);
            6:lblMessage.Caption := '(6) = GLog count = ' + IntToStr(vValue);
            7:lblMessage.Caption := '(7) = Card count = ' + IntToStr(vValue);
            8:lblMessage.Caption := '(8) = Alarm status = ' + IntToStr(vValue);
            9:lblMessage.Caption := '(9) = Face Count = ' + IntToStr(vValue);
            10:lblMessage.Caption := '(10) = SLog unread count = ' + IntToStr(vValue);
            11:lblMessage.Caption := '(11) = GLog unread count = ' + IntToStr(vValue);
        end;
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmSystemInfo.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

end.
