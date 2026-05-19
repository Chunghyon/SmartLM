unit Unit_Main;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids;

type
  TfrmMain = class(TForm)
    Memo1: TMemo;
    GroupBox1: TGroupBox;
    btnStartDownload: TButton;
    btnStopDownload: TButton;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    btnAddDevice: TButton;
    btnRemoveDevice: TButton;
    txtId: TEdit;
    txtIp: TEdit;
    txtPort: TEdit;
    txtPassword: TEdit;
    gridDevices: TStringGrid;
    procedure btnAddDeviceClick(Sender: TObject);
    procedure InitDeviceTable();
    procedure FormCreate(Sender: TObject);
    procedure btnRemoveDeviceClick(Sender: TObject);
    procedure btnStartDownloadClick(Sender: TObject);
    procedure btnStopDownloadClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
  public
    procedure UpdateDeviceStatus(id : Integer; status : String);

  end;

var
  frmMain: TfrmMain;

implementation

{$R *.dfm}

uses Unit_Device, SBXPCDLL_API;

const
  MAX_DEVICE = 20;
var
  onlineDevices: array[0..19] of TDevice;
  deviceCount : Integer;

procedure TfrmMain.InitDeviceTable;
var
  i : Integer;
begin
    gridDevices.RowCount := 2;
    gridDevices.ColCount := 9;
    gridDevices.Rows[1].Clear;
    gridDevices.Cells[1, 0] := 'Device ID';
    gridDevices.ColWidths[1] := 60;
    gridDevices.Cells[2, 0] := 'IP';
    gridDevices.ColWidths[2] := 120;
    gridDevices.Cells[3, 0] := 'Port';
    gridDevices.ColWidths[3] := 60;
    gridDevices.Cells[4, 0] := 'Password';
    gridDevices.ColWidths[4] := 60;
    gridDevices.Cells[5, 0] := 'Status';
    gridDevices.ColWidths[5] := 200;
end;

procedure TFrmMain.UpdateDeviceStatus(id : Integer; status : String);
var
  i : Integer;
  deviceId : Integer;
  found : Boolean;
begin
  found := False;
  for i := 1 to gridDevices.RowCount - 1 do
  begin
    deviceId := StrToInt(gridDevices.Cells[1, i]);
    if deviceId = id then
    begin
      found := True;
      break;
    end;
  end;

  if found then
  begin
    gridDevices.Cells[5, i] := status;
  end;
end;

procedure TfrmMain.btnAddDeviceClick(Sender: TObject);
var
  id, port, password : Integer;
  ip : WideString;
begin
  if deviceCount >= MAX_DEVICE then
  begin
    {This maximum is for just sample program, don't need to worry about it}
    application.MessageBox('No more devices (Max=20)', '');
    Exit;
  end;

  id := StrToInt(txtId.Text);
  ip := txtIp.Text;
  port := StrToInt(txtPort.Text);
  password := StrToInt(txtPassword.Text);

  gridDevices.RowCount := deviceCount + 2;
  gridDevices.Cells[1, deviceCount + 1] := txtId.Text;
  gridDevices.Cells[2, deviceCount + 1] := txtIp.Text;
  gridDevices.Cells[3, deviceCount + 1] := txtPort.Text;
  gridDevices.Cells[4, deviceCount + 1] := txtPassword.Text;
  gridDevices.Cells[5, deviceCount + 1] := '';

  txtId.Text := IntToStr(id + 1); 
  onlineDevices[deviceCount] := TDevice.Create(id, ip, port, password);
  deviceCount := deviceCount + 1;
end;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
  InitDeviceTable;
  _DotNet();
end;

procedure TfrmMain.btnRemoveDeviceClick(Sender: TObject);
var
  i : Integer;
begin
  for i := 0 to deviceCount - 1 do
  begin
    onlineDevices[i].FinishThreadProc();
    onlineDevices[i].Terminate();
    onlineDevices[i] := nil;
  end;

  deviceCount := 0;
  InitDeviceTable;
end;

procedure TfrmMain.btnStartDownloadClick(Sender: TObject);
var
  i : Integer;
begin
  for i := 0 to deviceCount - 1 do
  begin
    onlineDevices[i].StartDownload();
  end;
end;

procedure TfrmMain.btnStopDownloadClick(Sender: TObject);
var
  i : Integer;
begin
  for i := 0 to deviceCount - 1 do
  begin
    onlineDevices[i].StopDownload();
  end;
end;

procedure TfrmMain.FormClose(Sender: TObject; var Action: TCloseAction);
var
  i : Integer;
begin
  for i := 0 to deviceCount - 1 do
  begin
    onlineDevices[i].FinishThreadProc();
    onlineDevices[i].Terminate();
    onlineDevices[i] := nil;
  end;
end;

end.
