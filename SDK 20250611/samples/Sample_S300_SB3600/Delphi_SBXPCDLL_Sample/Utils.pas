unit Utils;

interface

uses SysUtils, SBXPCDLL_API;

const
    GSTR_NODEVICE     : string = 'No Device';
    TEMP_PHOTO_FILE   : String = 'C:\TempPhoto.jpg';
    COMPRESS_PHOTO_SIZE : Integer = 8192; // 8K jpeg image
    
function ErrorPrint(aErrorCode:Integer):string;
function pubIPAddrToLong(txtIP:string):Integer;
function pubLongToIPAddr(vValue:Integer):string;
function MakeXMLCommandHeader(strCmd : string) : WideString;

var gMachineNumber  :Integer;

implementation


function ErrorPrint(aErrorCode:Integer):string;
begin
    case aErrorcode of
        0: ErrorPrint := 'SUCCESS';
        1: ErrorPrint := 'ERR_COMPORT_ERROR';
        2: ErrorPrint := 'ERR_WRITE_FAIL';
        3: ErrorPrint := 'ERR_READ_FAIL';
        4: ErrorPrint := 'ERR_INVALID_PARAM';
        5: ErrorPrint := 'ERR_NON_CARRYOUT';
        6: ErrorPrint := 'ERR_LOG_END';
        7: ErrorPrint := 'ERR_MEMORY';
        8: ErrorPrint := 'ERR_MULTIUSER';
        else ErrorPrint := 'NO_ERROR';
    end;
end;

function pubIPAddrToLong(txtIP:string):Integer;
var
    dwIP    :int64;
    dwIPTmp :longint;
    k       :longint;
    code    :longint;
    szTmp   :string;
begin
    k := pos('.', txtIP);
    if k = 0 then pubIPAddrToLong := 0;
    szTmp := copy(txtIP, 0, k - 1);
    delete(txtIP, 1, k);
    val(szTmp, dwIP, code);

    k := pos('.', txtIP);
    if k = 0 then pubIPAddrToLong := 0;
    szTmp := copy(txtIP, 0, k - 1);
    delete(txtIP, 1, k);
    val(szTmp, dwIPTmp, code);
    dwIP := dwIP * 256 + dwIPTmp;

    k := pos('.', txtIP);
    if k = 0 then pubIPAddrToLong := 0;
    szTmp := copy(txtIP, 0, k - 1);
    delete(txtIP, 1, k);
    val(szTmp, dwIPTmp, code);
    dwIP := dwIP * 256 + dwIPTmp;
    val(txtIp, dwIPTmp, code);
    dwIP := dwIP * 256 + dwIPTmp;

    if dwIP > 2147483647 then dwIp := dwIP - 4294967296;
    pubIPAddrToLong := dwIP;
end;

function pubLongToIPAddr(vValue:Integer):string;
var
    txtIP       :string;
    remain      :Integer;
begin
    remain := vValue mod 256;
    if remain < 0 then remain := 256 + remain;
    txtIP := IntToStr(remain);
    vValue := (vValue - remain) div 256;

    remain := vValue mod 256;
    if remain < 0 then remain := 256 + remain;
    txtIP := IntToStr(remain) + '.' + txtIP;
    vValue := (vValue - remain) div 256;

    remain := vValue mod 256;
    if remain < 0 then remain := 256 + remain;
    txtIP := IntToStr(remain) + '.' + txtIP;
    vValue := (vValue - remain) div 256;

    remain := vValue mod 256;
    if remain < 0 then remain := 256 + remain;
    txtIP := IntToStr(remain) + '.' + txtIP;

    pubLongToIPAddr := txtIP;
end;

function MakeXMLCommandHeader(strCmd : string) : WideString;
var
    strXML : WideString;
begin
    strXML := '';
    _XML_AddString(strXML, 'REQUEST', PWideChar(WideString(strCmd)));
    _XML_AddString(strXML, 'MsgType', 'request');
    _XML_AddInt(strXML, 'MachineID', gMachineNumber);
    MakeXMLCommandHeader := strXML;
end;

end.
