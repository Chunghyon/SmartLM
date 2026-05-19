unit Unit_Enroll;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, DB, ExtCtrls, DBCtrls, DBTables, ADODB, jpeg;

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
    chkEnable: TCheckBox;
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
    Label7: TLabel;
    txtUserTz1: TEdit;
    Label8: TLabel;
    txtUserTz2: TEdit;
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
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmEnroll: TfrmEnroll;

implementation

uses Unit_Main, Utils, SBXPCLib_TLB;

const FPSIZE    :Integer = (1404 + 12) div 4;
const NAMESIZE  :Integer = 54;

var
    addrOf              :Integer;
    glngEnrollPData     :Integer;
    gTemplngEnrollData  :array[0..353{FPSIZE - 1}] of Integer;
    gbytEnrollData      :array[0..1769{FPSIZE * 5 - 1}] of Byte;

    bpc                 :TSBXPC;

{$R *.dfm}

procedure TfrmEnroll.FormShow(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
    
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);
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
    vRet := bpc.GetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
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
            txtUserTz1.Text := IntToStr(glngEnrollPData div 256);
            txtUserTz2.Text := IntToStr(glngEnrollPData mod 256);
        end
        else
            for i := 0 to FPSIZE - 1 do
                lstEnrollData.Items.Add(IntToStr(gTemplngEnrollData[i]));

    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
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
    vCardNumber := StrToInt(txtCardNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    if vFingerNumber = 10 then vFingerNumber := 15;
    vPrivilege := StrToInt(cmbPrivilege.Text);
    if (vCardNumber <> 0) and (vFingerNumber = 11) then
        glngEnrollPData := vCardNumber;
    if vFingerNumber = 15 then
    begin
        glngEnrollPData := 0;
        i := Length(txtCardNumber.Text);
        if i > 4 then i := 4;
        while i > 0 do
        begin
            glngEnrollPData := glngEnrollPData * 16 + StrToInt(txtCardNumber.Text[i]) + 1;
            i := i - 1;
        end;
    end;
    if vFingerNumber = 14 then
    begin
        glngEnrollPData := 0;
        glngEnrollPData := StrToInt(txtUserTz1.Text) * 256 + StrToInt(txtUserTz2.Text); 
    end;
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    addrOf := Integer(addr(gTemplngEnrollData));
    vRet := bpc.SetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
    if vRet then lblMessage.Caption := 'SetEnrollData OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vEMachineNumber := StrToInt(cmbEMachineNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    vRet := bpc.DeleteEnrollData(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber);
    if vRet then lblMessage.Caption := 'DeleteEnrollData OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vRet := bpc.GetUserName1(gMachineNumber, vEnrollNumber, vName);
    if vRet then
    begin
        pch := PWideChar(vName);
        txtName.Text := pch;
        lblMessage.Caption := 'GetUserName OK';
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmEnroll.cmdSetNameClick(Sender: TObject);
var
    vRet                :Boolean;
    vEnrollNumber       :Integer;
    vErrorCode          :Integer;
    vName               :WideString;

begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);

    vName := txtName.Text;
    vRet := bpc.SetUserName1(gMachineNumber, vEnrollNumber, vName);

    if vRet then
    begin
        lblMessage.Caption := 'SetUserName OK';
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmEnroll.cmdSetCompanyClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    vName               :WideString;
begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vName := txtName.Text;
    vRet := bpc.SetCompanyName1(gMachineNumber, 1, vName);
    if vRet then lblMessage.Caption := 'Set Company Name OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmEnroll.cmdDeleteCompanyClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    vName               :WideString;
begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.SetCompanyName1(gMachineNumber, 0, vName);
    if vRet then lblMessage.Caption := 'Delete Company Name OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.ReadAllUserID(gMachineNumber);
    if vRet then lblMessage.Caption := 'ReadAllUserID OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        bpc.EnableDevice(gMachineNumber, 1);
        lblMessage.Caption := ErrorPrint(vErrorCode);
        Exit;
    end;
//------------- Show all enroll information -----------------------------------
    vFlag := false;
    i := 0;
    lstEnrollData.Items.Add('No.  EnNo   EMNo   Fp   Priv  Enable Duress');
    while true do
    begin
        vRet := bpc.GetAllUserID(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vPrivilege, vEnable);
        if Not vRet then Break;
        vFlag := true;
        lstEnrollData.Items.Add(Format('%.5d', [i]) + '    ' + Format('%.5d', [vEnrollNumber]) + '    ' + Format('%.5d', [vEMachineNumber]) + '    ' + Format('%.5d', [vFingerNumber]) + '    ' + IntToStr(vPrivilege) + '    ' + IntToStr(vEnable mod 256) + '     ' + IntToStr(vEnable div 256));
        Inc(i);
        lblTotal.Caption := 'Total : ' + IntToStr(i);
    end;
    if vFlag then lblMessage.Caption := 'GetAllUserID OK'
    else lblMessage.Caption := ErrorPrint(vErrorCode);
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmEnroll.cmdEnableUserClick(Sender: TObject);
var
    vEnrollNumber       :Integer;
    vEMachineNumber     :Integer;
    vFingerNumber       :Integer;
    vRet                :Boolean;
    vFlag               :Integer;
    vErrorCode          :Integer;
begin
    lblMessage.Caption := 'Working...';
    vEMachineNumber := cmbEMachineNumber.ItemIndex + 1;
    vEnrollNumber := StrToInt(txtEnrollNumber.Text);
    vFingerNumber := StrToInt(cmbBackupNumber.Text);
    if chkEnable.Checked then vFlag := 1
    else vFlag := 0;
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.EnableUser(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vFlag);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
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

    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.ModifyPrivilege(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vMachinePrivilege);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmEnroll.cmdEmptyEnrollDataClick(Sender: TObject);
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
    vRet := bpc.EmptyEnrollData(gMachineNumber);
    if vRet then lblMessage.Caption := 'Success!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmEnroll.cmdClearDataClick(Sender: TObject);
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
    vRet := bpc.ClearKeeperData(gMachineNumber);
    if vRet then lblMessage.Caption := 'ClearKeeperData OK!'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;
    vRet := bpc.ReadAllUserID(gMachineNumber);
    if vRet then lblMessage.Caption := 'ReadAllUserID OK'
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
        bpc.EnableDevice(gMachineNumber, 1);
        Exit;
    end;
    //--------- Get Enroll data and save into database ------------------
    frmEnroll.Cursor := crHourGlass;
    vFlag := false;
    while true do
    begin
        vRet := bpc.GetAllUserID(gMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vPrivilege, vEnable);
        if Not vRet then Break;
        vFlag := true;
EEE:
        addrOf := Integer(addr(gTemplngEnrollData));
        if vFingerNumber >= 50 then Continue;
        vRet := bpc.GetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
        if Not vRet then
        begin
            vFlag := false;
            vStr := 'GetEnrollData';
            bpc.GetLastError(vErrorCode);
            vMsgRet := application.MessageBox(PAnsiChar(ErrorPrint(vErrorCode) + ':Continue ?'), 'GetEnrollData', MB_YESNOCANCEL);
            if vMsgRet = IDYES then goto EEE
            else if vMsgRet = IDCANCEL then
            begin
                frmEnroll.Cursor := crDefault;
                bpc.EnableDevice(gMachineNumber, 1);
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
    bpc.EnableDevice(gMachineNumber, 1);
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
    vRet := bpc.EnableDevice(gMachineNumber, 0);
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
        vRet := bpc.SetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, vPrivilege, addrOf, glngEnrollPData);
        if Not vRet then
        begin
            bpc.GetLastError(vErrorCode);
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
    bpc.EnableDevice(gMachineNumber, 1);
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

end.
