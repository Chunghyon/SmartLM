unit Unit_Device;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, Grids;

type
  TDevice = class(TThread)
  private
    cs: TRTLCriticalSection;
    status : Integer;
    deviceId: Integer;
    deviceIp : WideString;
    portNumber : Integer;
    password: Integer;
    downloadTime : Integer;

    procedure SetName;
    procedure InformStatus();

  public
    procedure Execute; override;

    constructor Create( id : Integer;
                        ip : WideString;
                        port : Integer;
                        password: Integer);
    destructor Destroy; override;

    procedure StartDownload();
    procedure StopDownload();
    procedure FinishThreadProc();

  end;

implementation

{ Important: Methods and properties of objects in visual components can only be
  used in a method called using Synchronize, for example,

      Synchronize(UpdateCaption);

  and UpdateCaption could look like,

    procedure TDevice.UpdateCaption;
    begin
      Form1.Caption := 'Updated in a thread';
    end; }

uses Unit_Main, SBXPCDLL_API;

{$IFDEF MSWINDOWS}
type
  TThreadNameInfo = record
    FType: LongWord;     // must be 0x1000
    FName: PChar;        // pointer to name (in user address space)
    FThreadID: LongWord; // thread ID (-1 indicates caller thread)
    FFlags: LongWord;    // reserved for future use, must be zero
  end;
{$ENDIF}

{ TDevice }

const
    STATUS_NONE = 0;
    STATUS_RELAY = 1;
    STATUS_DOWNLOAD = 2;
    
constructor TDevice.Create(id : Integer;
                          ip :WideString;
                          port : Integer;
                          password: Integer);
begin

  inherited Create(False);

  Self.deviceId := id;
  Self.deviceIp := ip;
  Self.portNumber := port;
  Self.password := password;

  InitializeCriticalSection(Self.cs);
  Self.downloadTime := 0;
  Self.status := STATUS_RELAY;
end;

destructor TDevice.Destroy;
begin
  DeleteCriticalSection(Self.cs);
end;

procedure TDevice.SetName;
{$IFDEF MSWINDOWS}
var
  ThreadNameInfo: TThreadNameInfo;
{$ENDIF}
begin
{$IFDEF MSWINDOWS}
  ThreadNameInfo.FType := $1000;
  ThreadNameInfo.FName := 'Device';
  ThreadNameInfo.FThreadID := $FFFFFFFF;
  ThreadNameInfo.FFlags := 0;

  try
    RaiseException( $406D1388, 0, sizeof(ThreadNameInfo) div sizeof(LongWord), @ThreadNameInfo );
  except
  end;
{$ENDIF}
end;

procedure TDevice.InformStatus;
var
  mainFrame : TfrmMain;
  statusText : String;
begin
  mainFrame := TfrmMain(application.FindComponent('frmMain'));
  statusText := 'Finished';
  if status = STATUS_RELAY then statusText := 'Relay...';
  if downloadTime <> 0 then statusText := statusText + ' download time=' + IntToStr(downloadTime); 
  if status = STATUS_DOWNLOAD then statusText := 'Downloadingn 60 fingers...';

  mainFrame.UpdateDeviceStatus(Self.deviceId, statusText);
end;

procedure TDevice.Execute;
var
  downloadStartTime  : Integer;
  i, j, addrOf, fileSize, fp_count : Integer;
  fingerData         : array of Byte;
  trigger            : Boolean;
  fingerDbFileHandle : Integer;
  fingerTemplate     : array[0..1416] of Byte;
  strXML             : WideString;
  checksum, i_checksum: LongWord;
begin
  SetName;
  _DotNet();
  
  if _ConnectTcpip(Self.deviceId, Self.deviceIp, Self.portNumber, Self.password) then
  begin
    Synchronize(InformStatus);

    while true do
    begin
      if Self.status = 0 then break;

      if (status = STATUS_DOWNLOAD) then
      begin
        downloadStartTime := GetTickCount();
        i := 0;
        fingerDbFileHandle := FileOpen('fp.db', fmOpenRead);
        if fingerDbFileHandle > 0 then
        begin
            fileSize := FileSeek(fingerDbFileHandle, 0, 2);
            fp_count := fileSize div 1404;
            if fp_count > 60 then fp_count := 60;
            fileSize := fp_count * 1404;

            if (fileSize = 0) then
            begin
              Application.MessageBox('Failed to read fp.db!', 'FP DB');
              break;
            end;

            FileSeek(fingerDbFileHandle, 0, 0);
            SetLength(fingerData, fileSize);
            FileRead(fingerDbFileHandle, Pointer(fingerData)^, fileSize);
            FileClose(fingerDbFileHandle);
        end;

        _EnableDevice(Self.deviceId, False);
        Synchronize(InformStatus);
        fingerTemplate[0] := Byte('S');
        fingerTemplate[1] := Byte('m');
        fingerTemplate[2] := Byte('a');
        fingerTemplate[3] := Byte('c');
        fingerTemplate[4] := Byte('k');
        fingerTemplate[5] := Byte('B');
        fingerTemplate[6] := Byte('i');
        fingerTemplate[7] := Byte('o');
        while i < fp_count do
        begin
          if status <> STATUS_DOWNLOAD then break;
          checksum := 0;
          i_checksum := 0;
          for j := 0 to 1403 do
          begin
            fingerTemplate[j + 8] := fingerData[i * 1404 + j];
            i_checksum := i_checksum + fingerTemplate[j + 8] shl ((j mod 4) * 8);
            if (j + 1) mod 4 = 0 then
            begin
              checksum := checksum + i_checksum;
              i_checksum := 0;
            end;
          end;

          fingerTemplate[1412] := checksum mod 256; checksum := checksum div 256;
          fingerTemplate[1413] := checksum mod 256; checksum := checksum div 256;
          fingerTemplate[1414] := checksum mod 256; checksum := checksum div 256;
          fingerTemplate[1415] := checksum mod 256; checksum := checksum div 256;
          
          addrOf := Integer(addr(fingerTemplate));
          _SetEnrollData1(Self.deviceId, i + 1, 0, 0, addrOf, 0);
          i := i + 1;
        end;
        _EnableDevice(Self.deviceId, True);
        EnterCriticalSection(Self.cs);
        status := STATUS_RELAY;
        LeaveCriticalSection(Self.cs);
        downloadtime := GetTickCount() - downloadStartTime;
        Synchronize(InformStatus);
      end;

      if trigger then
      begin
        trigger := False;
        _EnableDevice(Self.deviceId, False);
        strXML := '';
        _XML_AddString(strXML, 'REQUEST', 'SetDoorStatusMulti');
        _XML_AddString(strXML, 'MsgType', 'request');
        _XML_AddInt(strXML, 'MachineID', Self.deviceId);
        _XML_AddInt(strXML, 'DoorNo', 0);
        _XML_AddInt(strXML, 'DoorStatus', 1);
        _GeneralOperationXML(Self.deviceId, strXML);
      end
      else
      begin
        trigger := True;
        _EnableDevice(Self.deviceId, True);
        strXML := '';
        _XML_AddString(strXML, 'REQUEST', 'SetDoorStatusMulti');
        _XML_AddString(strXML, 'MsgType', 'request');
        _XML_AddInt(strXML, 'MachineID', Self.deviceId);
        _XML_AddInt(strXML, 'DoorNo', 0);
        _XML_AddInt(strXML, 'DoorStatus', 2);
        _GeneralOperationXML(Self.deviceId, strXML);
      end;
      Sleep(1000);
    end;
  end;

  _EnableDevice(Self.deviceId, True);
  _Disconnect(Self.deviceId);
  EnterCriticalSection(Self.cs);
  Self.status := 0;
  LeaveCriticalSection(Self.cs);
end;

procedure TDevice.StartDownload();
begin
  EnterCriticalSection(Self.cs);
  Self.status := STATUS_DOWNLOAD;
  LeaveCriticalSection(Self.cs);
end;

procedure TDevice.StopDownload();
begin
  EnterCriticalSection(Self.cs);
  Self.status:= STATUS_RELAY;
  LeaveCriticalSection(Self.cs);
end;

procedure TDevice.FinishThreadProc();
begin
  EnterCriticalSection(Self.cs);
  Self.status:= STATUS_NONE;
  LeaveCriticalSection(Self.cs);
end;

end.
