unit Unit_ScreenSaver;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmScreenSaver = class(TForm)
    lblMessage: TStaticText;
    txtCustomerName: TEdit;
    Label3: TLabel;
    Label1: TLabel;
    txtCompanyName: TEdit;
    cmdGetCustomerInfo: TButton;
    cmdSetCustomerInfo: TButton;
    Label5: TLabel;
    Label2: TLabel;
    Label4: TLabel;
    txtGlyphHeight: TEdit;
    txtGlyphWidth: TEdit;
    Label6: TLabel;
    Label7: TLabel;
    txtFontWidth: TEdit;
    txtFontHeight: TEdit;
    Label8: TLabel;
    txtFontWeight: TEdit;
    cmdGetSleepMessage: TButton;
    cmdSetSleepMessage: TButton;
    cmdExit: TButton;
    chkItalic: TCheckBox;
    chkUnderline: TCheckBox;
    chkStrikeOut: TCheckBox;
    chkDebugOut: TCheckBox;
    txtDebugOutFile: TEdit;
    cmdGetGlyphSize: TButton;
    cmdDebugOutFileBrowse: TButton;
    txtSleepMessage: TEdit;
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure FormShow(Sender: TObject);
    procedure cmdExitClick(Sender: TObject);
    procedure cmdGetCustomerInfoClick(Sender: TObject);
    procedure cmdSetCustomerInfoClick(Sender: TObject);
    procedure cmdGetGlyphSizeClick(Sender: TObject);
    procedure cmdGetSleepMessageClick(Sender: TObject);
    procedure cmdSetSleepMessageClick(Sender: TObject);
    procedure cmdDebugOutFileBrowseClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmScreenSaver: TfrmScreenSaver;

implementation

{$R *.dfm}

uses Unit_Main, Utils, SBXPCLib_TLB;

const
    CUSTOMER_NAME_LEN   : Integer = 64 * 2; // Customer Name can be 64 characters of Unicode
    COMPANY_NAME_LEN    : Integer = 64 * 2; // Company Name can be 64 characters of Unicode
    SLEEP_MSG_LEN       : Integer = 128 * 2; // SleepMessage Name can be 128 characters of Unicode
var
    bpc     :TSBXPC;

procedure TfrmScreenSaver.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
    TfrmMain(application.FindComponent('frmMain')).Visible := true;
end;

procedure TfrmScreenSaver.FormShow(Sender: TObject);
begin
    bpc := TfrmMain(application.FindComponent('frmMain')).SBXPC1;
end;

procedure TfrmScreenSaver.cmdExitClick(Sender: TObject);
begin
    Close();
end;

procedure TfrmScreenSaver.cmdGetCustomerInfoClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    strXML              :WideString;
    customerName        :WideString;
    companyName         :WideString;
    pch                 :PWideChar;
begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'GetCustomerInfo');

    vRet := bpc.GeneralOperationXML(strXML);

    if vRet then
    begin
        lblMessage.Caption := 'GetCustomerInfo OK';
        bpc.XML_ParseBinaryUnicode(strXML, 'CustomerName', customerName, CUSTOMER_NAME_LEN);
        bpc.XML_ParseBinaryUnicode(strXML, 'CompanyName', companyName, COMPANY_NAME_LEN);
        pch := PWideChar(customerName);
        txtCustomerName.Text := pch;
        pch := PWideChar(companyName);
        txtCompanyName.Text := pch;
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmScreenSaver.cmdSetCustomerInfoClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    strXML              :WideString;
    customerName        :WideString;
    companyName         :WideString;
begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    customerName := txtCustomerName.Text;
    companyName := txtCompanyName.Text;
    strXML := MakeXMLCommandHeader(bpc, 'SetCustomerInfo');
    bpc.XML_AddBinaryUnicode(strXML, 'CustomerName', customerName);
    bpc.XML_AddBinaryUnicode(strXML, 'CompanyName', companyName);

    vRet := bpc.GeneralOperationXML(strXML);

    if vRet then
    begin
        lblMessage.Caption := 'SetCustomerInfo OK';
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmScreenSaver.cmdGetGlyphSizeClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    strXML              :WideString;
begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'GetSleepMsgGlyphSize');

    vRet := bpc.GeneralOperationXML(strXML);

    if vRet then
    begin
        lblMessage.Caption := 'GetSleepMsgGlyphSize OK';
        txtGlyphWidth.Text := IntToStr(bpc.XML_ParseInt(strXML, 'Width'));
        txtGlyphHeight.Text := IntToStr(bpc.XML_ParseInt(strXML, 'Height'));
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmScreenSaver.cmdGetSleepMessageClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    strXML              :WideString;
    sleepMessage        :WideString;
begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'GetSleepMessage');

    vRet := bpc.GeneralOperationXML(strXML);

    if vRet then
    begin
        lblMessage.Caption := 'GetSleepMessage OK';
        bpc.XML_ParseBinaryUnicode(strXML, 'SleepMessage', sleepMessage, SLEEP_MSG_LEN);
        txtSleepMessage.Text := sleepMessage;
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;

    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmScreenSaver.cmdSetSleepMessageClick(Sender: TObject);
var
    vRet                :Boolean;
    vErrorCode          :Integer;
    strXML              :WideString;
    strFontXML          :WideString;
    vGlyphWidth, vGlyphHeight : Integer;

label _lexit;
begin
    lblMessage.Caption := 'Working...';
    vRet := bpc.EnableDevice(gMachineNumber, 0);
    if Not vRet then
    begin
        lblMessage.Caption := GSTR_NODEVICE;
        Exit;
    end;

    strXML := MakeXMLCommandHeader(bpc, 'GetSleepMsgGlyphSize');

    vRet := bpc.GeneralOperationXML(strXML);

    if vRet then
    begin
        lblMessage.Caption := 'GetSleepMsgGlyphSize OK';
        vGlyphWidth := bpc.XML_ParseInt(strXML, 'Width');
        vGlyphHeight := bpc.XML_ParseInt(strXML, 'Height');
        txtGlyphWidth.Text := IntToStr(vGlyphWidth);
        txtGlyphHeight.Text := IntToStr(vGlyphHeight);
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
        goto _lexit;
    end;

    bpc.XML_AddString(strFontXML, 'FaceName', 'Arial');
    bpc.XML_AddInt(strFontXML, 'Height', StrToInt(txtFontHeight.Text));
    bpc.XML_AddInt(strFontXML, 'Width', StrToInt(txtFontWidth.Text));
    bpc.XML_AddInt(strFontXML, 'Weight', StrToInt(txtFontWeight.Text));
    bpc.XML_AddBoolean(strFontXML, 'Italic', chkItalic.Checked);
    bpc.XML_AddBoolean(strFontXML, 'Underline', chkUnderline.Checked);
    bpc.XML_AddBoolean(strFontXML, 'StrikeOut', chkStrikeOut.Checked);

    if chkDebugOut.Checked then
    begin
        bpc.XML_AddString(strFontXML, 'DebugOut', txtDebugOutFile.Text);
    end;

    strXML := MakeXMLCommandHeader(bpc, 'SetSleepMessage');
    bpc.XML_AddBinaryGlyph(strXML, txtSleepMessage.Text, vGlyphWidth, vGlyphHeight, strFontXML);

    vRet := bpc.GeneralOperationXML(strXML);

    if vRet then
    begin
        lblMessage.Caption := 'SetSleepMessage OK';
    end
    else
    begin
        bpc.GetLastError(vErrorCode);
        lblMessage.Caption := ErrorPrint(vErrorCode);
    end;

_lexit:
    bpc.EnableDevice(gMachineNumber, 1);
end;

procedure TfrmScreenSaver.cmdDebugOutFileBrowseClick(Sender: TObject);
var openDlg : TSaveDialog;
begin
    openDlg := TSaveDialog.Create(NIL);
    if openDlg.Execute() then
    begin
        txtDebugOutFile.Text := openDlg.FileName;
    end;
end;

end.
