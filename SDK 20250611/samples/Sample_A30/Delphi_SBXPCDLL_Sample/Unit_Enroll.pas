unit Unit_Enroll;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, DB, ExtCtrls, DBCtrls, DBTables, ADODB, jpeg, ComCtrls;

type
  TfrmEnroll = class(TForm)
    lblMessage: TStaticText;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    txtEnrollNumber: TEdit;
    txtCardNumber: TEdit;
    cmbEMachineNumber: TComboBox;
    cmbBackupNumber: TComboBox;
    cmbPrivilege: TComboBox;
    txtName: TEdit;
    chkDisable: TCheckBox;
    lblEnrollData: TLabel;
    lblTotal: TLabel;
    lstEnrollData: TListBox;
    cmdDel: TButton;
    cmdGetEnrollData: TButton;
    cmdSetEnrollData: TButton;
    cmdDeleteEnrollData: TButton;
    cmdGetAllEnrollData: TButton;
    cmdSetAllEnrollData: TButton;
    cmdGetName: TButton;
    cmdSetName: TButton;
    cmdSetCompany: TButton;
    cmdDeleteCompany: TButton;
    cmdGetEnrollInfo: TButton;
    cmdEnableUser: TButton;
    cmdModifyPrivilege: TButton;
    cmdEmptyEnrollData: TButton;
    cmdClearData: TButton;
    cmdExit: TButton;
    con: TADOConnection;
    tblEnroll: TADOTable;
    ds: TDataSource;
    cmdModifyDuress: TButton;
    Label7: TLabel;
    cmbDuress: TComboBox;
    Label8: TLabel;
    txtUserTz1: TEdit;
    Label9: TLabel;
    txtUserTz2: TEdit;
    Label10: TLabel;
    txtUserGroup: TEdit;
    chkUsePeriod: TCheckBox;
    Label11: TLabel;
    Label12: TLabel;
    Label14: TLabel;
    ComboBox1: TComboBox;
    dtValidFrom: TDateTimePicker;
    dtValidUntil: TDateTimePicker;
    cmdGetUserInfo: TButton;
    cmdSetUserInfo: TButton;
    cmdGetUserPeriod: TButton;
    cmdSetUserPeriod: TButton;
    procedure FormShow(Sender: TObject);
    procedure cmdGetEnrollDataClick(Sender: TObject);
    procedure cmdSetEnrollDataClick(Sender: TObject);
    procedure cmdDeleteEnrollDataClick(Sender: TObject);
    procedure cmdGetNameClick(Sender: TObject);
    procedure cmdSetNameClick(Sender: TObject);
    procedure cmdSetCompanyClick(Sender: TObject);
    procedure cmdDeleteCompanyClick(Sender: TObject);
    procedure cmdGetEnrollInfoClick(Sender: TObject);
    procedure cmdEnableUserClick(Sender: TObject);
    procedure cmdModifyPrivilegeClick(Sender: TObject);
    procedure cmdEmptyEnrollDataClick(Sender: TObject);
    procedure cmdClearDataClick(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure cmdGetAllEnrollDataClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure cmdSetAllEnrollDataClick(Sender: TObject);
    procedure cmdDelClick(Sender: TObject);
    procedure cmdGetUserInfoClick(Sender: TObject);
    procedure cmdSetUserInfoClick(Sender: TObject);
    procedure cmdGetUserPeriodClick(Sender: TObject);
    procedure chkUsePeriodClick(Sender: TObject);
    procedure cmdSetUserPeriodClick(Sender: TObject);
    procedure cmdModifyDuressClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmEnroll: TfrmEnroll;

implementation

uses Unit_Main, SBXPCDLL_API, Utils;

const FPSIZE    :Integer = (1404 + 12) div 4;
const NAMESIZE  :Integer = 54;

var
    addrOf              :Integer;
    glngEnrollPData     :Integer;
    gTemplngEnrollData  :array[0..353{FPSIZE - 1}] of Integer;
    gbytEnrollData      :array[0..1769{FPSIZE * 5 - 1}] of Byte;

{$R *.dfm}

procedure TfrmEnroll.FormShow(Sender: TObject);
begin
    tblEnroll.TableName := 'tblEnroll';
    ds.DataSet := tblEnroll;
    ds.DataSet.Open;
end;

procedure TfrmEnroll.cmdGetEnrollDataClick(Sender: TObject);
var
    vRet            :Boolean;
    vEnrollNumber   :Integer;
    vFingerNumber   :Integer;
    vPrivilege      :Integer;
    vErrorCode      :Integer;
    i               :Integer;
begin
    lstEnrollData.Items.Clear;
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    lblEnrollData.Caption := 'Enrolled Data';
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    if vFingerNumber = 10 then vFingerNumber := 15;

    addrOf := Integer(addr(gTemplngEnrollData));
    glngEnrollPData := 0;
    vRet := _GetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
    if vRet then
    begin
        cmbPrivilege.ItemIndex := vPrivilege;
        lblMessage.Caption := 'GetEnrollData OK';
        if vFingerNumber = 15 then
        begin
            txtCardNumber.Text := '';
            while glngEnrollPData > 0 do
            begin
                i := glngEnrollPData mod 16 - 1;
                txtCardNumber.Text := txtCardNumber.Text + IntToStr(i);
                glngEnrollPData := glngEnrollPData div 16;
            end;
        end
        else if vFingerNumber = 11 then
        begin
            txtCardNumber.Text := Uppercase(Format('%x', [glngEnrollPData]));
            lstEnrollData.Items.Add(Uppercase(Format('%x', [glngEnrollPData])));
        end
        else if vFingerNumber = 14 then
        begin
          txtUserTZ1.Text := (IntToStr(glngEnrollPData Mod 64)); glngEnrollPData := glngEnrollPData div 64;
          txtUserTZ2.Text := (IntToStr(glngEnrollPData Mod 64)); glngEnrollPData := glngEnrollPData div 64;
        end
        else
            for i := 0 to FPSIZE - 1 do
                lstEnrollData.Items.Add(IntToStr(gTemplngEnrollData[i]));

    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdSetEnrollDataClick(Sender: TObject);
var
    vRet            : Boolean;
    vEnrollNumber   : Integer;
    vCardNumber     : Integer;
    vFingerNumber   : Integer;
    vPrivilege      : Integer;
    vErrorCode      : Integer;
    i               : Integer;
begin
    lblMessage.Caption := 'Working...';
    if txtEnrollNumber.Text = '' then txtEnrollNumber.Text := '0';
    if txtCardNumber.Text = '' then txtCardNumber.Text := '0';
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    if vFingerNumber = 10 then vFingerNumber := 15;
    vPrivilege := StrToInt(cmbPrivilege.Text);
    if (vCardNumber <> 0) and (vFingerNumber = 11) then
        vCardNumber := StrToInt('0x' + txtCardNumber.Text);
        glngEnrollPData := vCardNumber;
    if vFingerNumber = 15 then
    begin
        glngEnrollPData := 0;
        i := Length(txtCardNumber.Text);
        if i > 6 then i := 6;
        while i > 0 do
        begin
            glngEnrollPData := glngEnrollPData * 16 + StrToInt(txtCardNumber.Text[i]) + 1;
            i := i - 1;
        end;
    end
    else if vFingerNumber = 14 then
    begin
      glngEnrollPData := StrToInt(txtUserTz1.Text) * 256 + StrToInt(txtUserTz2.Text);
    end;

    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    addrOf := Integer(addr(gTemplngEnrollData));
    vRet := _SetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
    if vRet then lblMessage.Caption := 'SetEnrollData OK'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdDeleteEnrollDataClick(Sender: TObject);
var
    vRet            :Boolean;
    vEnrollNumber   :Integer;
    vEMachineNumber :Integer;
    vFingerNumber   :Integer;
    vErrorCode      :Integer;
begin
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vEMachineNumber := StrToInt(cmbEMachineNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    vRet := _DeleteEnrollData(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber);
    if vRet then lblMessage.Caption := 'DeleteEnrollData OK'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdGetNameClick(Sender: TObject);
var
    vRet                :Boolean;
    vEnrollNumber       :Integer;
    vErrorCode          :Integer;
    vName               :WideString;
    pch                 :PWideChar;

begin
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vRet := _GetUserName1(gMachineNumber, vEnrollNumber, vName);
    if vRet then
    begin
        pch := PWideChar(vName);
        txtName.Text := pch;
        lblMessage.Caption := 'GetUserName OK';
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdSetNameClick(Sender: TObject);
var
    vRet                :Boolean;
    vEnrollNumber       :Integer;
    vErrorCode          :Integer;
    vName               :WideString;

begin
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);

    vName := txtName.Text;
    vRet := _SetUserName1(gMachineNumber, vEnrollNumber, vName);

    if vRet then
    begin
        lblMessage.Caption := 'SetUserName OK';
    end
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdSetCompanyClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    vName               :WideString;
begin
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vName := txtName.Text;
    vRet := _SetCompanyName1(gMachineNumber, 1, vName);
    if vRet then lblMessage.Caption := 'Set Company Name OK'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdDeleteCompanyClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    vName               :WideString;
begin
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _SetCompanyName1(gMachineNumber, 0, vName);
    if vRet then lblMessage.Caption := 'Delete Company Name OK'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdGetEnrollInfoClick(Sender: TObject);
var
    vRet, vFlag         :Boolean;
    vEMachineNumber     :Integer;
    vEnrollNumber       :Integer;
    vFingerNumber       :Integer;
    vPrivilege          :Integer;
    vEnable             :Integer;
    vErrorCode          :Integer;
    i                  :Integer;
begin
    lblEnrollData.Caption := 'User IDs';
    lstEnrollData.Items.Clear;
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _ReadAllUserID(gMachineNumber);
    if vRet then lblMessage.Caption := 'ReadAllUserID OK'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        _EnableDevice(gMachineNumber, True);
        lblMessage.Caption := ErrorPrint(vErrorCode);
        Exit;
    end;
//------------- Show all enroll information -----------------------------------
    vFlag := false;
    i := 0;
    lstEnrollData.Items.Add('No.  EnNo   EMNo   Fp   Priv  Enable Duress');
    while true do
    begin
        vRet := _GetAllUserID(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vPrivilege, vEnable);
        if Not vRet then Break;
        vFlag := true;
        lstEnrollData.Items.Add(Format('%.5d', [i]) + '    ' + Format('%.5d', [vEnrollNumber]) + '    ' + Format('%.5d', [vEMachineNumber]) + '    ' + Format('%.5d', [vFingerNumber]) + '    ' + IntToStr(vPrivilege) + '    ' + IntToStr(vEnable mod 256) + '     ' + IntToStr(vEnable div 256));
        Inc(i);
        lblTotal.Caption := 'Total : ' + IntToStr(i);
    end;
    if vFlag then lblMessage.Caption := 'GetAllUserID OK'
    else lblMessage.Caption := ErrorPrint(vErrorCode);
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdEnableUserClick(Sender: TObject);
var
    vEnrollNumber       :Integer;
    vEMachineNumber     :Integer;
    vFingerNumber       :Integer;
    vRet                :Boolean;
    vFlag               :LongBool;
    vErrorCode          :Integer;
begin
    lblMessage.Caption := 'Working...';
    vEMachineNumber := cmbEMachineNumber.ItemIndex + 1;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    if not chkDisable.Checked then vFlag := True
    else vFlag := False;
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _EnableUser(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vFlag);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdModifyPrivilegeClick(Sender: TObject);
var
    vEnrollNumber       :Integer;
    vEMachineNumber     :Integer;
    vFingerNumber       :Integer;
    vRet                :Boolean;
    vMachinePrivilege   :Integer;
    vErrorCode          :Integer;
begin
    lblMessage.Caption := 'Working...';
    vEMachineNumber := cmbEMachineNumber.ItemIndex + 1;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    vMachinePrivilege := StrToInt(cmbPrivilege.Text);

    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _ModifyPrivilege(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vMachinePrivilege);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdEmptyEnrollDataClick(Sender: TObject);
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
    vRet := _EmptyEnrollData(gMachineNumber);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdClearDataClick(Sender: TObject);
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
    vRet := _ClearKeeperData(gMachineNumber);
    if vRet then lblMessage.Caption := 'ClearKeeperData OK!'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdExitClick(Sender: TObject);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    ds.DataSet.Close;
    Close;
end;

procedure TfrmEnroll.FormClose(Sender: TObject; var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
    ds.DataSet.Close;   
end;

procedure TfrmEnroll.cmdGetAllEnrollDataClick(Sender: TObject);
var
    vFlag               :Boolean;
    vRet                :Boolean;
    vEnrollNumber       :Integer;
    vEMachineNumber     :Integer;
    vFingerNumber       :Integer;
    vPrivilege          :Integer;
    vEnable             :Integer;
    vMsgRet             :Integer;
    vErrorCode          :Integer;
    i                   :Integer;
    vStr, vTitle        :string;
    bExist              :Boolean;
    searchOpt           :TLocateOptions;
    bStream             :TBlobStream;

label EEE, re, FFF;
begin
    lstEnrollData.Items.Clear;
    vTitle := frmEnroll.Caption;
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := _ReadAllUserID(gMachineNumber);
    if vRet then lblMessage.Caption := 'ReadAllUserID OK'
    else
    begin
        _GetLastError(gMachineNumber, vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
        _EnableDevice(gMachineNumber, True);
        Exit;
    end;
    //--------- Get Enroll data and save into database ------------------
    frmEnroll.Cursor := crHourGlass;
    vFlag := false;
    while true do
    begin
        vRet := _GetAllUserID(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vPrivilege, vEnable);
        if Not vRet then Break;
        vFlag := true;
EEE:
        addrOf := Integer(addr(gTemplngEnrollData));
        if vFingerNumber >= 50 then Continue;
        vRet := _GetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
        if Not vRet then
        begin
            vFlag := false;
            vStr := 'GetEnrollData';
            _GetLastError(gMachineNumber, vErrorCode);
            vMsgRet := application.MessageBox(PAnsiChar(ErrorPrint(vErrorCode) + ':Continue ?'), 'GetEnrollData', MB_YESNOCANCEL);
            if vMsgRet = IDYES then goto EEE
            else if vMsgRet = IDCANCEL then
            begin
                frmEnroll.Cursor := crDefault;
                _EnableDevice(gMachineNumber, True);
                Exit;
            end;
        end;
        bExist := false;
        if ds.DataSet.RecordCount > 0 then
        begin
            ds.DataSet.First;
            searchOpt := [loPartialKey];
            bExist := ds.DataSet.Locate('EnrollNumber;EMachineNumber;FingerNumber', VarArrayOf([vEnrollNumber,vEMachineNumber,vFingerNumber]), searchOpt);
        end;
        if Not bExist then ds.DataSet.Append
        else ds.DataSet.Edit;
        ds.DataSet.FieldByName('EMachineNumber').AsInteger := vEMachineNumber;
        ds.DataSet.FieldByName('EnrollNumber').AsInteger := vEnrollNumber;
        ds.DataSet.FieldByName('FingerNumber').AsInteger := vFingerNumber;
        ds.DataSet.FieldByName('Privilige').AsInteger := vPrivilege;
        if vFingerNumber in [10, 11, 15] then
            ds.DataSet.FieldByName('Password1').AsInteger := glngEnrollPData
        else
        begin
            ds.DataSet.FieldByName('Password1').AsInteger := 0;
            for i := 0 to FPSIZE - 1 do
            begin
                gbytEnrollData[i * 5] := 1;
                if gTemplngEnrollData[i] < 0 then
                begin
                    gbytEnrollData[i * 5] := 0;
                    gTemplngEnrollData[i] := abs(gTemplngEnrollData[i]);
                end;
                gbytEnrollData[i * 5 + 1] := (gTemplngEnrollData[i] div 256 div 256 div 256);
                gbytEnrollData[i * 5 + 2] := ((gTemplngEnrollData[i] div 256 div 256) mod 256);
                gbytEnrollData[i * 5 + 3] := ((gTemplngEnrollData[i] div 256) mod 256);
                gbytEnrollData[i * 5 + 4] := (gTemplngEnrollData[i] mod 256);
            end;
            bStream := TBlobStream(ds.DataSet.CreateBlobStream(ds.DataSet.FieldByName('FPdata'),bmWrite));
            bStream.WriteBuffer(gbytEnrollData, Length(gbytEnrollData));
            bStream.Free;
        end;
        ds.DataSet.Post;
FFF:
        lblMessage.Caption := Format('%.3d',[vEMachineNumber]) + '-' + Format('%.5d',[vEnrollNumber]) + '-' + IntToStr(vFingerNumber);
        frmEnroll.Caption := Format('%.5d', [vEnrollNumber]);
        txtEnrollNumber.Text := IntToStr(vEnrollNumber);
        cmbBackupNumber.Text := IntToStr(vFingerNumber);
        cmbEMachineNumber.Text := IntToStr(vEMachineNumber);
        cmbPrivilege.Text := IntToStr(vPrivilege);
    end;
    lblTotal.Caption := 'Total : ' + IntToStr(ds.DataSet.RecordCount);
    vTitle := frmEnroll.Caption;
    frmEnroll.Cursor := crDefault;
    
    if vFlag = true then lblMessage.Caption := 'GetAllUserID OK'
    else lblMessage.Caption := vStr + ':' + ErrorPrint(vErrorCode);
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.FormCreate(Sender: TObject);
begin
    con.ConnectionString := 'Provider=Microsoft.Jet.OLEDB.4.0;' + 'Data Source=' + GetCurrentDir + '\datEnrollDat.mdb;Persist Security Info=False';
    tblEnroll.Connection := con;
end;

procedure TfrmEnroll.cmdSetAllEnrollDataClick(Sender: TObject);
var
    vRet                :Boolean;
    vEnrollNumber       :Integer;
    vEMachineNumber     :Integer;
    vFingerNumber       :Integer;
    vPrivilege          :Integer;
    vMsgRet             :Integer;
    vErrorCode          :Integer;
    i, num              :Integer;
    vTitle              :string;
    bStream             :TStream;

label EEE, re, FFF;
begin
    lstEnrollData.Items.Clear;
    vTitle := frmEnroll.Caption;
    lblMessage.Caption := 'Working...';
    vRet := _EnableDevice(gMachineNumber, False);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    frmEnroll.Cursor := crHourGlass;
    if ds.DataSet.RecordCount = 0 then goto EEE;
    ds.DataSet.First;
    num := 0;
    while Not ds.DataSet.Eof do
    begin
        ds.DataSet.Edit;
        vEMachineNumber := ds.DataSet.FieldValues['EMachineNumber'];
        vEnrollNumber := ds.DataSet.FieldValues['EnrollNumber'];
        vFingerNumber := ds.DataSet.FieldValues['FingerNumber'];
        vPrivilege := ds.DataSet.FieldValues['Privilige'];
        glngEnrollPData := ds.DataSet.FieldValues['Password1'];

        num := num + 1;
        if vFingerNumber < 10 then
        begin
            bStream := ds.DataSet.CreateBlobStream(ds.DataSet.FieldByName('FPdata'), bmRead);
            bStream.ReadBuffer(gbytEnrollData, Length(gbytEnrollData));
            bStream.Free;
            for i := 0 to FPSIZE - 1 do
            begin
                gTemplngEnrollData[i] := gbytEnrollData[i * 5 + 1];
                gTemplngEnrollData[i] := gTemplngEnrollData[i] * 256 + gbytEnrollData[i * 5 + 2];
                gTemplngEnrollData[i] := gTemplngEnrollData[i] * 256 + gbytEnrollData[i * 5 + 3];
                gTemplngEnrollData[i] := gTemplngEnrollData[i] * 256 + gbytEnrollData[i * 5 + 4];
                if gbytEnrollData[i * 5] = 0 then
                    gTemplngEnrollData[i] := 0 - gTemplngEnrollData[i];
            end;
        end;
FFF:
        addrOf := Integer(addr(gTemplngEnrollData));
        vRet := _SetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
        if Not vRet then
        begin
            _GetLastError(gMachineNumber, vErrorCode);
            vMsgRet := application.MessageBox(PAnsiChar(ErrorPrint(vErrorCode) + ': Continue? '), 'SetEnrollData', MB_YESNOCANCEL);
            if vMsgRet = IDYES then goto FFF;
            if vMsgRet = IDNO then goto EEE;
        end;
        lblMessage.Caption := 'EMachine = ' + IntToStr(vEMachineNumber) + ', ID = ' + Format('%.5d', [vEnrollNumber]) + ', FpNo = ' + IntToStr(vFingerNumber) + ', Count = ' + IntToStr(num);
        frmEnroll.Caption := IntToStr(num);
        ds.DataSet.Next;
    end;
EEE:
    vTitle := frmEnroll.Caption;
    frmEnroll.Cursor := crDefault;
    lblMessage.Caption := 'SetAllUserData OK';
    _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdDelClick(Sender: TObject);
begin
    ds.DataSet.First;
    while ds.DataSet.RecordCount > 0 do
    begin
        ds.DataSet.Delete;
        ds.DataSet.Next;
    end;

    lblTotal.Caption := 'Total : 0';
    lblMessage.Caption := 'Deleted PC Database';
end;


procedure TfrmEnroll.cmdGetUserInfoClick(Sender: TObject);
var
  strXML : WideString;
  vEnrollNumber : Integer;
  vRet : Boolean;
  vErrorCode : Integer;
begin
  lblMessage.Caption := 'Working...';

  vEnrollNumber := StrToInt(txtEnrollNumber.Text);

  strXML := MakeXMLCommandHeader('GetUserInfo');
  _XML_AddInt(strXML, 'UserID', vEnrollNumber);

  vRet := _EnableDevice(gMachineNumber, False);
  if Not vRet then
  begin
    lblMessage.Caption := GSTR_NODEVICE;
    Exit;
  end;

  vRet := _GeneralOperationXML(gMachineNumber, strXML);

  if vRet then
  begin
    lblMessage.Caption := 'Get UserInfo Success!';
    txtUserTz1.Text := IntToStr(_XML_ParseInt(strXML, 'Timezone1'));
    txtUserTz2.Text := IntToStr(_XML_ParseInt(strXML, 'Timezone2'));
  end
  else
  begin
    _GetLastError(gMachineNumber, vErrorCode);
    lblMessage.Caption := ErrorPrint(vErrorCode);
  end;
end;

procedure TfrmEnroll.cmdSetUserInfoClick(Sender: TObject);
var
  vEnrollNumber : Integer;
  vTimezone1, vTimezone2, vTimezone3 : Integer;
  vGroupNo : Integer;
  vRet : Boolean;
  vErrorCode : Integer;
  strXML : WideString;
begin
  lblMessage.Caption := 'Working...';

  vEnrollNumber := StrToInt(txtEnrollNumber.Text);
  vTimezone1 := StrToInt(txtUserTZ1.Text);
  vTimezone2 := StrToInt(txtUserTZ2.Text);
  vGroupNo := StrToInt(txtUserGroup.Text);

  strXML := MakeXMLCommandHeader('SetUserInfo');
  _XML_AddInt(strXML, 'UserID', vEnrollNumber);
  _XML_AddInt(strXML, 'Timezone1', vTimezone1);
  _XML_AddInt(strXML, 'Timezone2', vTimezone2);
  _XML_AddInt(strXML, 'GroupNo', vGroupNo);

  vRet := _EnableDevice(gMachineNumber, False);
  if Not vRet then
  begin
    lblMessage.Caption := GSTR_NODEVICE;
    Exit;
  end;
  
  vRet := _GeneralOperationXML(gMachineNumber, strXML);
  if vRet then
  begin
      lblMessage.Caption := 'Set UserInfo Success!';
  end
  else
  begin
      _GetLastError(gMachineNumber, vErrorCode);
      lblMessage.Caption := ErrorPrint(vErrorCode);
  end;

  _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdGetUserPeriodClick(Sender: TObject);
var
  bRet : Boolean;
  vErrorCode : Integer;
  lUserID : Integer;
  lStartDate : Integer;
  lEndDate : Integer;
  yy, mm, dd : Integer;
  strXML : WideString;
begin
  lblMessage.Caption := 'Waiting...';

  bRet := _EnableDevice(gMachineNumber, False);
  if (Not bRet) then
  begin
    lblMessage.Caption := GSTR_NODEVICE;
    Exit;
  end;

  lUserID := StrToInt(txtEnrollNumber.Text);

  strXML := MakeXMLCommandHeader('GetUserPeriod');
  _XML_AddInt(strXML, 'UserID', lUserID);

  bRet := _GeneralOperationXML(gMachineNumber, strXML);
  if bRet then
  begin
    chkUsePeriod.Checked := (_XML_ParseInt(strXML, 'Used') = 1);
    lStartDate := _XML_ParseInt(strXML, 'Start');
    lEndDate := _XML_ParseInt(strXML, 'End');
    if (chkUsePeriod.Checked) then
    begin
      yy := lStartDate div 256 div 256;
      mm := (lStartDate - yy * 256 * 256) div 256;
      dd := lStartDate mod 256;
      yy := yy + 2000;
      dtValidFrom.Date := EncodeDate(yy, mm, dd);

      yy := lEndDate div 256 div 256;
      mm := (lEndDate - yy * 256 * 256) div 256;
      dd := lEndDate mod 256;
      yy := yy + 2000;
      dtValidUntil.Date := EncodeDate(yy, mm, dd);
    end
    else
    begin
      dtValidFrom.Enabled := False;
      dtValidUntil.Enabled := False;
    end;

    lblMessage.Caption := 'Success!';
  end
  else
  begin
    _GetLastError(gMachineNumber, vErrorCode);
    lblMessage.Caption := ErrorPrint(vErrorCode);
  end;

  _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.chkUsePeriodClick(Sender: TObject);
begin
  dtValidFrom.Enabled := chkUsePeriod.Checked;
  dtValidUntil.Enabled := chkUsePeriod.Checked;
end;

procedure TfrmEnroll.cmdSetUserPeriodClick(Sender: TObject);
var
  bRet : Boolean;
  vErrorCode : Integer;
  lUserID : Integer;
  lStartPeriod, lEndPeriod : Integer;
  lInfo : Integer;
  strXML : WideString;
  year, month, day : Word;
begin

  lblMessage.Caption := 'Waiting...';

  bRet := _EnableDevice(gMachineNumber, False);
  if (Not bRet) then
  begin
    lblMessage.Caption := GSTR_NODEVICE;
    Exit;
  end;

  lUserID := StrToInt(txtEnrollNumber.Text);
  strXML := MakeXMLCommandHeader('SetUserPeriod');
  _XML_AddInt(strXML, 'UserID', lUserID);
  if (chkUsePeriod.Checked) then
  begin
    _XML_AddInt(strXML, 'Used', 1);
  end
  else
  begin
    _XML_AddInt(strXML, 'Used', 0);
  end;

  lStartPeriod := 1 * 256 + 1;
  lEndPeriod := 1 * 256 + 1;
  if (chkUsePeriod.Checked) then
  begin
    DecodeDate(dtValidFrom.Date, year, month, day);
    lStartPeriod := (year - 2000) * 256 * 256 + month * 256 + day;
    DecodeDate(dtValidUntil.Date, year, month, day);
    lEndPeriod := (year - 2000) * 256 * 256 + month * 256 + day;
  end;

  _XML_AddInt(strXML, 'Start', lStartPeriod);
  _XML_AddInt(strXML, 'End', lEndPeriod);

  bRet := _GeneralOperationXML(gMachineNumber, strXML);
  if bRet then
  begin
    lblMessage.Caption := 'Success!';
  end
  else
  begin
    _GetLastError(gMachineNumber, vErrorCode);
    lblMessage.Caption := ErrorPrint(vErrorCode);
  end;

  _EnableDevice(gMachineNumber, True);
end;

procedure TfrmEnroll.cmdModifyDuressClick(Sender: TObject);
var
    vEnrollNumber : Integer;
    vFingerNumber : Integer;
    vDuressSetting : Integer;
    vRet : Boolean;
    vErrorCode : Integer;
begin
    lblMessage.Caption := 'Working...';

    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    vDuressSetting := StrToInt(cmbDuress.Text);

    vRet := _EnableDevice(gMachineNumber, False);
    if (Not vRet) Then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    vRet := _ModifyDuressFP(gMachineNumber, vEnrollNumber, vFingerNumber, vDuressSetting);
    if vRet then
    begin
      lblMessage.Caption := 'Success!';
    end
    else
    begin
      _GetLastError(gMachineNumber, vErrorCode);
      lblMessage.Caption := ErrorPrint(vErrorCode);
    end;

    _EnableDevice(gMachineNumber, True);
end;

end.
