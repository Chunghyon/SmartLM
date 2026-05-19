// SBXPCCtrl.cpp : Implementation of the CSBXPCCtrl ActiveX Control class.

#include "stdafx.h"
#include "SBXPC.h"
#include "SBXPCCtrl.h"
#include "SBXPCPropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

/************************************************************************/
/*                                                                      */
/************************************************************************/
IMPLEMENT_DYNCREATE(CSBXPCCtrl, COleControl)



// Message map

BEGIN_MESSAGE_MAP(CSBXPCCtrl, COleControl)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_MESSAGE(WM_USER, FireOnReceiveEventXML_byWindowsMessage)
END_MESSAGE_MAP()



// Dispatch map

BEGIN_DISPATCH_MAP(CSBXPCCtrl, COleControl)
	DISP_PROPERTY_NOTIFY(CSBXPCCtrl, "CommPort", m_commPort, OnCommPortChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CSBXPCCtrl, "Baudrate", m_baudrate, OnBaudrateChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CSBXPCCtrl, "ReadMark", m_readMark, OnReadMarkChanged, VT_BOOL)

	DISP_FUNCTION(CSBXPCCtrl, "SetMachineType", SetMachineType, VT_BOOL, VTS_BSTR)
	DISP_FUNCTION(CSBXPCCtrl, "DotNET", DotNET, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CSBXPCCtrl, "GetEnrollData", GetEnrollData, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_PI4 VTS_PVARIANT VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "GetEnrollData1", GetEnrollData1, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "SetEnrollData", SetEnrollData, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_PVARIANT VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "SetEnrollData1", SetEnrollData1, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_PI4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "DeleteEnrollData", DeleteEnrollData, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "ReadSuperLogData", ReadSuperLogData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetSuperLogData", GetSuperLogData, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "ReadGeneralLogData", ReadGeneralLogData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetGeneralLogData", GetGeneralLogData, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "ReadAllSLogData", ReadAllSLogData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetAllSLogData", GetAllSLogData, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "ReadAllGLogData", ReadAllGLogData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetAllGLogData", GetAllGLogData, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "GetDeviceStatus", GetDeviceStatus, VT_BOOL, VTS_I4 VTS_I4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "GetDeviceInfo", GetDeviceInfo, VT_BOOL, VTS_I4 VTS_I4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "SetDeviceInfo", SetDeviceInfo, VT_BOOL, VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "EnableDevice", EnableDevice, VT_BOOL, VTS_I4 VTS_BOOL)
	DISP_FUNCTION(CSBXPCCtrl, "EnableUser", EnableUser, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	DISP_FUNCTION(CSBXPCCtrl, "GetDeviceTime", GetDeviceTime, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "SetDeviceTime", SetDeviceTime, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "PowerOnAllDevice", PowerOnAllDevice, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CSBXPCCtrl, "PowerOffDevice", PowerOffDevice, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "ModifyPrivilege", ModifyPrivilege, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "ReadAllUserID", ReadAllUserID, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetAllUserID", GetAllUserID, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "GetSerialNumber", GetSerialNumber, VT_BOOL, VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "GetBackupNumber", GetBackupNumber, VT_I4, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetProductCode", GetProductCode, VT_BOOL, VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "ClearKeeperData", ClearKeeperData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "EmptyEnrollData", EmptyEnrollData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "EmptyGeneralLogData", EmptyGeneralLogData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "EmptySuperLogData", EmptySuperLogData, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetUserName", GetUserName, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_PVARIANT)
	DISP_FUNCTION(CSBXPCCtrl, "GetUserName1", GetUserName1, VT_BOOL, VTS_I4 VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "SetUserName", SetUserName, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_PVARIANT)
	DISP_FUNCTION(CSBXPCCtrl, "SetUserName1", SetUserName1, VT_BOOL, VTS_I4 VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "GetCompanyName", GetCompanyName, VT_BOOL, VTS_I4 VTS_PVARIANT)
	DISP_FUNCTION(CSBXPCCtrl, "GetCompanyName1", GetCompanyName1, VT_BOOL, VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "SetCompanyName", SetCompanyName, VT_BOOL, VTS_I4 VTS_I4 VTS_PVARIANT)
	DISP_FUNCTION(CSBXPCCtrl, "SetCompanyName1", SetCompanyName1, VT_BOOL, VTS_I4 VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "GetDoorStatus", GetDoorStatus, VT_BOOL, VTS_I4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "SetDoorStatus", SetDoorStatus, VT_BOOL, VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetBellTime", GetBellTime, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "SetBellTime", SetBellTime, VT_BOOL, VTS_I4 VTS_I4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "ConnectSerial", ConnectSerial, VT_BOOL, VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "ConnectTcpip", ConnectTcpip, VT_BOOL, VTS_I4 VTS_PBSTR VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "Disconnect", Disconnect, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CSBXPCCtrl, "SetIPAddress", SetIPAddress, VT_BOOL, VTS_PBSTR VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "OpenCommPort", OpenCommPort, VT_BOOL, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "CloseCommPort", CloseCommPort, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CSBXPCCtrl, "GetLastError", GetLastError, VT_BOOL, VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "GeneralOperationXML", GeneralOperationXML, VT_BOOL, VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "GetDeviceLongInfo", GetDeviceLongInfo, VT_BOOL, VTS_I4 VTS_I4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "SetDeviceLongInfo", SetDeviceLongInfo, VT_BOOL, VTS_I4 VTS_I4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "ModifyDuressFP", ModifyDuressFP, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetMachineIP", GetMachineIP, VT_BOOL, VTS_BSTR VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "GetDepartName", GetDepartName, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "SetDepartName", SetDepartName, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_PBSTR)

	DISP_FUNCTION(CSBXPCCtrl, "StartEventCapture", StartEventCapture, VT_BOOL, VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "StopEventCapture", StopEventCapture, VT_EMPTY, VTS_NONE)

	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseInt", XML_ParseInt, VT_I4, VTS_PBSTR VTS_BSTR)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseLong", XML_ParseLong, VT_I4, VTS_PBSTR VTS_BSTR)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseBoolean", XML_ParseBoolean, VT_BOOL, VTS_PBSTR VTS_BSTR)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseString", XML_ParseString, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseBinaryByte", XML_ParseBinaryByte, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PUI1 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseBinaryWord", XML_ParseBinaryWord, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PI2 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseBinaryLong", XML_ParseBinaryLong, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PI4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseBinaryUnicode", XML_ParseBinaryUnicode, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PBSTR VTS_I4)

	DISP_FUNCTION(CSBXPCCtrl, "XML_AddInt", XML_AddInt, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddLong", XML_AddLong, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddBoolean", XML_AddBoolean, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_BOOL)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddString", XML_AddString, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddBinaryByte", XML_AddBinaryByte, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PUI1 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddBinaryWord", XML_AddBinaryWord, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PI2 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddBinaryLong", XML_AddBinaryLong, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PI4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddBinaryUnicode", XML_AddBinaryUnicode, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "XML_AddBinaryGlyph", XML_AddBinaryGlyph, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_I4 VTS_I4 VTS_BSTR)

	DISP_FUNCTION(CSBXPCCtrl, "ConnectP2p", ConnectP2p, VT_I4, VTS_PBSTR VTS_PBSTR VTS_I4 VTS_I4 VTS_PI4)
	DISP_FUNCTION(CSBXPCCtrl, "PrepareP2p", PrepareP2p, VT_I4, VTS_PBSTR VTS_PBSTR VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)

	DISP_FUNCTION(CSBXPCCtrl, "XML_AddBinaryNameGlyph", XML_AddBinaryNameGlyph, VT_BOOL, VTS_I4 VTS_PBSTR VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseMultiUnicode", XML_ParseMultiUnicode, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PBSTR VTS_I4)

	DISP_FUNCTION(CSBXPCCtrl, "SetDeviceTime1", SetDeviceTime1, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "Disconnect1", Disconnect1, VT_EMPTY, VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "DisconnectAll", DisconnectAll, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CSBXPCCtrl, "GeneralOperationXML1", GeneralOperationXML1, VT_BOOL, VTS_I4 VTS_PBSTR)
 	DISP_FUNCTION(CSBXPCCtrl, "UseInternalRedraw", UseInternalRedraw, VT_EMPTY, VTS_BOOL)

	DISP_FUNCTION(CSBXPCCtrl, "GetInternalFwVer", GetInternalFwVer, VT_I4, VTS_I4)

	DISP_FUNCTION(CSBXPCCtrl, "ReadGLogWithPos", ReadGLogWithPos, VT_BOOL, VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "ReadSLogWithPos", ReadSLogWithPos, VT_BOOL, VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CSBXPCCtrl, "GetDeviceUniqueID", GetDeviceUniqueID, VT_BOOL, VTS_I4 VTS_PVARIANT)
	DISP_FUNCTION(CSBXPCCtrl, "GetDeviceUniqueID1", GetDeviceUniqueID1, VT_BOOL, VTS_I4 VTS_PBSTR)
	DISP_FUNCTION(CSBXPCCtrl, "GetDeviceModel", GetDeviceModel, VT_BOOL, VTS_I4 VTS_PI4 VTS_PI4 VTS_PI4 VTS_PI4)

	DISP_FUNCTION(CSBXPCCtrl, "XML_ParseBinaryAnsi2Unicode", XML_ParseBinaryAnsi2Unicode, VT_BOOL, VTS_PBSTR VTS_BSTR VTS_PBSTR VTS_I4)

	DISP_FUNCTION_ID(CSBXPCCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)
END_DISPATCH_MAP()



// Event map

BEGIN_EVENT_MAP(CSBXPCCtrl, COleControl)
	EVENT_CUSTOM("OnReceiveEvent", FireOnReceiveEvent, VTS_I4  VTS_I4  VTS_I4)
	EVENT_CUSTOM("OnReceiveEventXML", FireOnReceiveEventXML, VTS_BSTR)
END_EVENT_MAP()



// Property pages

// TODO: Add more property pages as needed.  Remember to increase the count!
BEGIN_PROPPAGEIDS(CSBXPCCtrl, 1)
	PROPPAGEID(CSBXPCPropPage::guid)
END_PROPPAGEIDS(CSBXPCCtrl)



// Initialize class factory and guid

IMPLEMENT_OLECREATE_EX(CSBXPCCtrl, "SBXPC.SBXPCCtrl.1",
	0x2894e36d, 0x6941, 0x48e0, 0xab, 0xf9, 0xd, 0x38, 0x24, 0x18, 0x84, 0xfb)

// Type library ID and version

IMPLEMENT_OLETYPELIB(CSBXPCCtrl, _tlid, _wVerMajor, _wVerMinor)


// Interface IDs

const IID BASED_CODE IID_DSBXPC =
		{ 0x6C1D3CFC, 0x712A, 0x41B4, { 0xA5, 0x3F, 0xCC, 0x10, 0xF6, 0x41, 0x27, 0x91 } };
const IID BASED_CODE IID_DSBXPCEvents =
		{ 0x9FF3A121, 0xE3D, 0x4C35, { 0xA7, 0x76, 0xC9, 0xC7, 0x66, 0xFB, 0x14, 0xBE } };



// Control type information

static const DWORD BASED_CODE _dwSBXPCOleMisc =
	OLEMISC_ACTIVATEWHENVISIBLE |
	OLEMISC_SETCLIENTSITEFIRST |
	OLEMISC_INSIDEOUT |
	OLEMISC_CANTLINKINSIDE |
	OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(CSBXPCCtrl, IDS_SBXPC, _dwSBXPCOleMisc)

// CSBXPCCtrl::CSBXPCCtrlFactory::UpdateRegistry -
// Adds or removes system registry entries for CSBXPCCtrl

BOOL CSBXPCCtrl::CSBXPCCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	// TODO: Verify that your control follows apartment-model threading rules.
	// Refer to MFC TechNote 64 for more information.
	// If your control does not conform to the apartment-model rules, then
	// you must modify the code below, changing the 6th parameter from
	// afxRegApartmentThreading to 0.

	if (bRegister)
		return AfxOleRegisterControlClass(
			AfxGetInstanceHandle(),
			m_clsid,
			m_lpszProgID,
			IDS_SBXPC,
			IDB_SBXPC,
			afxRegApartmentThreading,
			_dwSBXPCOleMisc,
			_tlid,
			_wVerMajor,
			_wVerMinor);
	else
		return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}



// CSBXPCCtrl::CSBXPCCtrl - Constructor

CSBXPCCtrl::CSBXPCCtrl()
{
	InitializeIIDs(&IID_DSBXPC, &IID_DSBXPCEvents);

	m_strMachineType = _T("");
	m_dwMachineID = 0;

	m_ComMode = 1; //serial
	m_commPort = 1;
	m_baudrate = 115200;
	m_ipAddr = L"192.168.1.224";
	m_portNumber = 5005;
	m_dwPassword = 0;
	m_readMark = TRUE;
}



// CSBXPCCtrl::~CSBXPCCtrl - Destructor

CSBXPCCtrl::~CSBXPCCtrl()
{
}

// CSBXPCCtrl::OnDraw - Drawing function

void CSBXPCCtrl::OnDraw(
			CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
	if (!pdc)
		return;

	// TODO: Replace the following code with your own drawing code.
	pdc->FillRect(rcBounds, CBrush::FromHandle((HBRUSH)GetStockObject(WHITE_BRUSH)));
	//	pdc->Ellipse(rcBounds);
	CSize ts = pdc->GetTextExtent(_T("SBXPC"));
	pdc->TextOut((rcBounds.Width() - ts.cx) / 2,
		(rcBounds.Height() - ts.cy) / 2,
		_T("SBXPC"));
}



// CSBXPCCtrl::DoPropExchange - Persistence support

void CSBXPCCtrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	// TODO: Call PX_ functions for each persistent custom property.
}



// CSBXPCCtrl::OnResetState - Reset control to default state

void CSBXPCCtrl::OnResetState()
{
	COleControl::OnResetState();  // Resets defaults found in DoPropExchange

	// TODO: Reset any other control state here.
}



// CSBXPCCtrl::AboutBox - Display an "About" box to the user

void CSBXPCCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_SBXPC);
	dlgAbout.DoModal();
}



// CSBXPCCtrl message handlers

/************************************************************************/
/*                                                                      */
/************************************************************************/
void CSBXPCCtrl::OnCommPortChanged() 
{
	SetModifiedFlag();
	m_ComMode = 1; //serial
}

void CSBXPCCtrl::OnBaudrateChanged() 
{
	SetModifiedFlag();
	m_ComMode = 1; //serial
}

void CSBXPCCtrl::OnReadMarkChanged() 
{
	SetModifiedFlag();
}

BOOL CSBXPCCtrl::SetMachineType(LPCTSTR lpszMachineType)
{
	m_strMachineType = lpszMachineType;
	return TRUE;
}

void CSBXPCCtrl::DotNET() 
{
	_DotNET();
}

/************************************************************************/
/*                                                                      */
/************************************************************************/
BOOL CSBXPCCtrl::GetEnrollData(long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber, long dwBackupNumber,
							   long FAR* dwMachinePrivilege, VARIANT FAR* dwEnrollData, long FAR* dwPassWord) 
{
	return _GetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber,
							dwMachinePrivilege, dwEnrollData, dwPassWord);
}

BOOL CSBXPCCtrl::GetEnrollData1(long dwMachineNumber, long dwEnrollNumber, long dwBackupNumber, long FAR* dwMachinePrivilege,
								long FAR* dwEnrollData, long FAR* dwPassWord)
{
	return _GetEnrollData1(dwMachineNumber, dwEnrollNumber, dwBackupNumber, dwMachinePrivilege,
							dwEnrollData, dwPassWord);
}

BOOL CSBXPCCtrl::SetEnrollData(long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber, long dwBackupNumber,
							   long dwMachinePrivilege, VARIANT FAR* dwEnrollData, long dwPassWord) 
{
	return _SetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber,
							dwMachinePrivilege, dwEnrollData, dwPassWord);
}

BOOL CSBXPCCtrl::SetEnrollData1(long dwMachineNumber, long dwEnrollNumber, long dwBackupNumber, long dwMachinePrivilege,
								long FAR* dwEnrollData, long dwPassWord) 
{
	return _SetEnrollData1(dwMachineNumber, dwEnrollNumber, dwBackupNumber, dwMachinePrivilege,
							dwEnrollData, dwPassWord);
}

BOOL CSBXPCCtrl::DeleteEnrollData(long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber, long dwBackupNumber) 
{
	return _DeleteEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber);
}

BOOL CSBXPCCtrl::ReadSuperLogData(long dwMachineNumber) 
{
	return _ReadSuperLogData(dwMachineNumber, m_readMark);
}

BOOL CSBXPCCtrl::GetSuperLogData(long dwMachineNumber, long* dwTMachineNumber, long* dwSEnrollNumber, long* dwSMachineNumber,
								 long* dwGEnrollNumber, long* dwGMachineNumber, long* dwManipulation, long* dwBackupNumber,
								 long* dwYear, long* dwMonth, long* dwDay, long* dwHour,
								 long* dwMinute, long* dwSecond)
{
	return _GetSuperLogData(dwMachineNumber, dwTMachineNumber, dwSEnrollNumber, dwSMachineNumber,
		dwGEnrollNumber, dwGMachineNumber, dwManipulation, dwBackupNumber,
		dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
}

BOOL CSBXPCCtrl::ReadGeneralLogData(long dwMachineNumber) 
{
	return _ReadGeneralLogData(dwMachineNumber, m_readMark);
}

BOOL CSBXPCCtrl::GetGeneralLogData(long dwMachineNumber, long* dwTMachineNumber, long* dwEnrollNumber, long* dwEMachineNumber,
								   long* dwVerifyMode, long* dwYear, long* dwMonth, long* dwDay,
								   long* dwHour, long* dwMinute, long* dwSecond)
{
	return _GetGeneralLogData(dwMachineNumber, dwTMachineNumber, dwEnrollNumber, dwEMachineNumber,
		dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
}

BOOL CSBXPCCtrl::ReadAllSLogData(long dwMachineNumber) 
{
	return _ReadAllSLogData(dwMachineNumber);
}

BOOL CSBXPCCtrl::GetAllSLogData(long dwMachineNumber, long* dwTMachineNumber, long* dwSEnrollNumber, long* dwSMachineNumber,
								long* dwGEnrollNumber, long* dwGMachineNumber, long* dwManipulation, long* dwBackupNumber,
								long* dwYear, long* dwMonth, long* dwDay, long* dwHour,
								long* dwMinute, long* dwSecond)
{
	return _GetAllSLogData(dwMachineNumber, dwTMachineNumber, dwSEnrollNumber, dwSMachineNumber,
		dwGEnrollNumber, dwGMachineNumber, dwManipulation, dwBackupNumber,
		dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
}

BOOL CSBXPCCtrl::ReadAllGLogData(long dwMachineNumber) 
{
	return _ReadAllGLogData(dwMachineNumber);
}

BOOL CSBXPCCtrl::GetAllGLogData(long dwMachineNumber, long* dwTMachineNumber, long* dwEnrollNumber, long* dwEMachineNumber,
								long* dwVerifyMode, long* dwYear, long* dwMonth, long* dwDay,
								long* dwHour, long* dwMinute, long* dwSecond)
{
	return _GetAllGLogData(dwMachineNumber, dwTMachineNumber, dwEnrollNumber, dwEMachineNumber,
		dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
}

BOOL CSBXPCCtrl::GetDeviceStatus(long dwMachineNumber, long dwStatus, long* dwValue) 
{
	return _GetDeviceStatus(dwMachineNumber, dwStatus, dwValue);
}

BOOL CSBXPCCtrl::GetDeviceInfo(long dwMachineNumber, long dwInfo, long* dwValue) 
{
	return _GetDeviceInfo(dwMachineNumber, dwInfo, dwValue);
}

BOOL CSBXPCCtrl::SetDeviceInfo(long dwMachineNumber, long dwInfo, long dwValue) 
{
	return _SetDeviceInfo(dwMachineNumber, dwInfo, dwValue);
}

BOOL CSBXPCCtrl::EnableDevice(long dwMachineNumber, BOOL bFlag) 
{
	return _EnableDevice(dwMachineNumber, bFlag);
}

BOOL CSBXPCCtrl::EnableUser(long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber, long dwBackupNumber, BOOL bFlag) 
{
	return _EnableUser(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, bFlag);
}

BOOL CSBXPCCtrl::GetDeviceTime(long dwMachineNumber, long* dwYear, long* dwMonth, long* dwDay,
							   long* dwHour, long* dwMinute, long* dwSecond, long* dwDayOfWeek)
{
	return _GetDeviceTime(dwMachineNumber, dwYear, dwMonth, 
		dwDay, dwHour, dwMinute, dwSecond, dwDayOfWeek);
}

BOOL CSBXPCCtrl::SetDeviceTime(long dwMachineNumber) 
{
	return _SetDeviceTime(dwMachineNumber);
}

BOOL CSBXPCCtrl::SetDeviceTime1(long dwMachineNumber,long dwYear,long dwMonth,long dwDay,long dwHour,long dwMinute,long dwSecond) 
{
	return _SetDeviceTime1(dwMachineNumber, dwYear, dwMonth, 
		dwDay, dwHour, dwMinute, dwSecond);
}

BOOL CSBXPCCtrl::PowerOnAllDevice() 
{
	return _PowerOnAllDevice(m_dwMachineID);
}

BOOL CSBXPCCtrl::PowerOffDevice(long dwMachineNumber) 
{
	return _PowerOffDevice(dwMachineNumber);
}

BOOL CSBXPCCtrl::ModifyPrivilege(long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber, 
								 long dwBackupNumber, long dwMachinePrivilege) 
{
	return _ModifyPrivilege(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege);
}

BOOL CSBXPCCtrl::ReadAllUserID(long dwMachineNumber) 
{
	return _ReadAllUserID(dwMachineNumber);
}

BOOL CSBXPCCtrl::GetAllUserID(long dwMachineNumber, long* dwEnrollNumber, long* dwEMachineNumber, long* dwBackupNumber,
							  long* dwMachinePrivilege, long* dwEnable) 
{
	return _GetAllUserID(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber,
		dwMachinePrivilege, dwEnable);
}

BOOL CSBXPCCtrl::GetSerialNumber(long dwMachineNumber, BSTR FAR* lpszSerialNumber) 
{
	return _GetSerialNumber(dwMachineNumber, lpszSerialNumber);
}

long CSBXPCCtrl::GetBackupNumber(long dwMachineNumber) 
{
	return _GetBackupNumber(dwMachineNumber);
}

BOOL CSBXPCCtrl::GetProductCode(long dwMachineNumber, BSTR FAR* lpszProductCode) 
{
	return _GetProductCode(dwMachineNumber, lpszProductCode);
}

BOOL CSBXPCCtrl::ClearKeeperData(long dwMachineNumber) 
{
	return _ClearKeeperData(dwMachineNumber);
}

BOOL CSBXPCCtrl::EmptyEnrollData(long dwMachineNumber) 
{
	return _EmptyEnrollData(dwMachineNumber);
}

BOOL CSBXPCCtrl::EmptyGeneralLogData(long dwMachineNumber) 
{
	return _EmptyGeneralLogData(dwMachineNumber);
}

BOOL CSBXPCCtrl::EmptySuperLogData(long dwMachineNumber) 
{
	return _EmptySuperLogData(dwMachineNumber);
}

BOOL CSBXPCCtrl::GetUserName(long DeviceKind, long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber,
							 VARIANT FAR* lpszUserName) 
{
	return _GetUserName(DeviceKind, dwMachineNumber, dwEnrollNumber, dwEMachineNumber, lpszUserName);
}

BOOL CSBXPCCtrl::GetUserName1(long dwMachineNumber, long dwEnrollNumber, BSTR FAR* lpszUserName) 
{
	return _GetUserName1(dwMachineNumber, dwEnrollNumber, lpszUserName);
}

BOOL CSBXPCCtrl::SetUserName(long DeviceKind, long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber, VARIANT FAR* lpszUserName) 
{
	return _SetUserName(DeviceKind, dwMachineNumber, dwEnrollNumber, dwEMachineNumber, lpszUserName);
}

BOOL CSBXPCCtrl::SetUserName1(long dwMachineNumber, long dwEnrollNumber, BSTR FAR* lpszUserName) 
{
	return _SetUserName1(dwMachineNumber, dwEnrollNumber, lpszUserName);
}

BOOL CSBXPCCtrl::GetCompanyName(long dwMachineNumber, VARIANT FAR* dwCompanyName) 
{
	return _GetCompanyName(dwMachineNumber, dwCompanyName);
}

BOOL CSBXPCCtrl::GetCompanyName1(long dwMachineNumber, BSTR FAR* dwCompanyName) 
{
	return _GetCompanyName1(dwMachineNumber, dwCompanyName);
}

BOOL CSBXPCCtrl::SetCompanyName(long dwMachineNumber, long vKind, VARIANT FAR* dwCompanyName) 
{
	return _SetCompanyName(dwMachineNumber, vKind, dwCompanyName);
}

BOOL CSBXPCCtrl::SetCompanyName1(long dwMachineNumber, long vKind, BSTR FAR* dwCompanyName) 
{
	return _SetCompanyName1(dwMachineNumber, vKind, dwCompanyName);
}

BOOL CSBXPCCtrl::GetDoorStatus(long dwMachineNumber, long* dwValue) 
{
	return _GetDoorStatus(dwMachineNumber, dwValue);
}

BOOL CSBXPCCtrl::SetDoorStatus(long dwMachineNumber, long dwValue) 
{
	return _SetDoorStatus(dwMachineNumber, dwValue);
}

BOOL CSBXPCCtrl::GetBellTime(long dwMachineNumber, long* dwValue, long* dwBellInfo) 
{
	return _GetBellTime(dwMachineNumber, dwValue, dwBellInfo);
}

BOOL CSBXPCCtrl::SetBellTime(long dwMachineNumber, long dwValue, long* dwBellInfo) 
{
	return _SetBellTime(dwMachineNumber, dwValue, dwBellInfo);
}

BOOL CSBXPCCtrl::ConnectSerial(long dwMachineNumber, long dwCommPort, long dwBaudRate)
{
	m_dwMachineID = dwMachineNumber;
	if(_ConnectSerial(dwMachineNumber, dwCommPort, dwBaudRate))
	{
		_SetMachineType(dwMachineNumber, m_strMachineType);
		return TRUE;
	}
	return FALSE;
}

BOOL CSBXPCCtrl::ConnectTcpip(long dwMachineNumber, BSTR FAR* lpszIPAddress, long dwPortNumber, long dwPassWord)
{
	m_dwMachineID = dwMachineNumber;
	if(_ConnectTcpip(dwMachineNumber, lpszIPAddress, dwPortNumber, dwPassWord))
	{
		_SetMachineType(dwMachineNumber, m_strMachineType);
		return TRUE;
	}
	return FALSE;
}

long CSBXPCCtrl::ConnectP2p(BSTR FAR* lpszMachineID, BSTR FAR* lpszServerIPAddress, long dwServerPortNumber, long dwPassWord, long* pnError)
{
	m_dwMachineID = _ConnectP2p(lpszMachineID, lpszServerIPAddress, dwServerPortNumber, dwPassWord, pnError);
	return m_dwMachineID;
}

long CSBXPCCtrl::PrepareP2p(BSTR FAR* lpszMachineID, BSTR FAR* lpszServerIPAddress, long dwServerPortNumber, long* dwYearStart,long* dwMonthStart,long* dwDayStart,long* dwYearEnd,long* dwMonthEnd,long* dwDayEnd, long* pnError)
{
	return _PrepareP2p(lpszMachineID, lpszServerIPAddress, dwServerPortNumber, dwYearStart,dwMonthStart,dwDayStart,dwYearEnd,dwMonthEnd,dwDayEnd, pnError);
}

void CSBXPCCtrl::Disconnect()
{
	_Disconnect(m_dwMachineID);
	m_dwMachineID = 0;
}

void CSBXPCCtrl::Disconnect1(long dwMachineNumber)
{
	_Disconnect(dwMachineNumber);
}

void CSBXPCCtrl::DisconnectAll()
{
	_DisconnectAll();
}

BOOL CSBXPCCtrl::SetIPAddress(BSTR FAR* lpszIPAddress, long dwPortNumber, long dwPassWord) 
{
	m_ComMode = 0;

	m_ipAddr = *lpszIPAddress;
	m_portNumber = dwPortNumber;
	m_dwPassword = dwPassWord;

	return TRUE;
}

BOOL CSBXPCCtrl::OpenCommPort(long dwMachineNumber) 
{
	BOOL bRet;

	m_dwMachineID = dwMachineNumber;
	if (m_ComMode == 0)
	{
		BSTR ipAddr = (BSTR)(LPCWSTR)m_ipAddr;
		bRet = _ConnectTcpip(dwMachineNumber, &ipAddr, m_portNumber, m_dwPassword);
	}
	else
		bRet = _ConnectSerial(dwMachineNumber, m_commPort, m_baudrate);

	if (bRet)
		_SetMachineType(dwMachineNumber, m_strMachineType);

	return bRet;
}

void CSBXPCCtrl::CloseCommPort() 
{
	_Disconnect(m_dwMachineID);
	m_dwMachineID = 0;
}

BOOL CSBXPCCtrl::GetLastError(long* dwErrorCode) 
{
	return _GetLastError(m_dwMachineID, dwErrorCode);
}

BOOL CSBXPCCtrl::GeneralOperationXML(BSTR FAR* lpszReqNResXML) 
{
	return _GeneralOperationXML(m_dwMachineID, lpszReqNResXML);
}

BOOL CSBXPCCtrl::GeneralOperationXML1(long dwMachineNumber, BSTR FAR* lpszReqNResXML) 
{
	return _GeneralOperationXML(dwMachineNumber, lpszReqNResXML);
}

BOOL CSBXPCCtrl::GetDeviceLongInfo(long dwMachineNumber, long dwInfo, long* dwValue) 
{
	return _GetDeviceLongInfo(dwMachineNumber, dwInfo, dwValue);
}

BOOL CSBXPCCtrl::SetDeviceLongInfo(long dwMachineNumber, long dwInfo, long* dwValue) 
{
	return _SetDeviceLongInfo(dwMachineNumber, dwInfo, dwValue);
}

BOOL CSBXPCCtrl::ModifyDuressFP(long dwMachineNumber, long dwEnrollNumber, long dwBackupNumber, long dwDuressSetting) 
{
	return _ModifyDuressFP(dwMachineNumber, dwEnrollNumber, dwBackupNumber, dwDuressSetting);
}

BOOL CSBXPCCtrl::GetMachineIP( LPCTSTR lpszProduct, long dwMachineNumber, BSTR FAR* lpszIPBuf)
{
	return _GetMachineIP(lpszProduct, dwMachineNumber, lpszIPBuf );
}

BOOL CSBXPCCtrl::GetDepartName(long dwMachineNumber, long dwDepartNumber, long dwDepartOrDaigong, BSTR FAR* lpszDepartName)
{
	return _GetDepartName(dwMachineNumber, dwDepartNumber, dwDepartOrDaigong, lpszDepartName);
}

BOOL CSBXPCCtrl::SetDepartName(long dwMachineNumber, long dwDepartNumber, long dwDepartOrDaigong, BSTR FAR* lpszDepartName) 
{
	return _SetDepartName(dwMachineNumber, dwDepartNumber, dwDepartOrDaigong, lpszDepartName);
}

long CSBXPCCtrl::GetInternalFwVer(long dwMachineNumber)
{
	return _GetInternalFwVer(dwMachineNumber);
}

BOOL CSBXPCCtrl::ReadGLogWithPos(long dwMachineNumber, long dwStartPos, long dwEndPos)
{
	return _ReadGLogWithPos(dwMachineNumber, dwStartPos, dwEndPos);
}

BOOL CSBXPCCtrl::ReadSLogWithPos(long dwMachineNumber, long dwStartPos, long dwEndPos)
{
	return _ReadSLogWithPos(dwMachineNumber, dwStartPos, dwEndPos);
}

BOOL CSBXPCCtrl::GetDeviceUniqueID(long dwMachineNumber, VARIANT FAR* dwDeviceUniqueID)
{
	return _GetDeviceUniqueID(dwMachineNumber, dwDeviceUniqueID);
}

BOOL CSBXPCCtrl::GetDeviceUniqueID1(long dwMachineNumber, BSTR FAR* dwDeviceUniqueID)
{
	return _GetDeviceUniqueID1(dwMachineNumber, dwDeviceUniqueID);
}

BOOL CSBXPCCtrl::GetDeviceModel(long dwMachineNumber, long* dwIsBigUserId, long* dwCompanyType, long* dwMachineType, long* dwMachineVersion)
{
	return _GetDeviceModel(dwMachineNumber, dwIsBigUserId, dwCompanyType, dwMachineType, dwMachineVersion);
}

/************************************************************************/
/*                                                                      */
/************************************************************************/
BOOL CSBXPCCtrl::StartEventCapture(long dwCommType, long dwParam1, long dwParam2 ) 
{
	return _StartEventCapture(dwCommType, dwParam1, dwParam2, &CSBXPCCtrl::Fire_Event, this);
}

void CSBXPCCtrl::StopEventCapture() 
{
	_StopEventCapture();
}

//////////////////////////////////////////////////////////////////////////
//XML Helper interface implementations
long CSBXPCCtrl::XML_ParseInt(BSTR FAR* lpszXML, LPCTSTR lpszTagname)
{
	return _XML_ParseInt(lpszXML, lpszTagname);
}
long CSBXPCCtrl::XML_ParseLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname)
{
	return _XML_ParseLong(lpszXML, lpszTagname);
}
BOOL CSBXPCCtrl::XML_ParseBoolean(BSTR FAR* lpszXML, LPCTSTR lpszTagname)
{
	return _XML_ParseBoolean(lpszXML, lpszTagname);
}
BOOL CSBXPCCtrl::XML_ParseString(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* lpszValue)
{
	return _XML_ParseString(lpszXML, lpszTagname, lpszValue);
}
BOOL CSBXPCCtrl::XML_ParseBinaryByte(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BYTE FAR* pData, long dwLen)
{
	return _XML_ParseBinaryByte(lpszXML, lpszTagname, pData, dwLen);
}
BOOL CSBXPCCtrl::XML_ParseBinaryWord(BSTR FAR* lpszXML, LPCTSTR lpszTagname, WORD FAR* pData, long dwLen)
{
	return _XML_ParseBinaryWord(lpszXML, lpszTagname, pData, dwLen);
}
BOOL CSBXPCCtrl::XML_ParseBinaryLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname, long FAR* pData, long dwLen)
{
	return _XML_ParseBinaryLong(lpszXML, lpszTagname, pData, dwLen);
}
BOOL CSBXPCCtrl::XML_ParseBinaryUnicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* pData, long dwLen)
{
	return _XML_ParseBinaryUnicode(lpszXML, lpszTagname, pData, dwLen);
}
BOOL CSBXPCCtrl::XML_ParseMultiUnicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* pData, long dwLen)
{
	return _XML_ParseMultiUnicode(lpszXML, lpszTagname, pData, dwLen);
}
BOOL CSBXPCCtrl::XML_ParseBinaryAnsi2Unicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* pData, long dwLen)
{
	return _XML_ParseBinaryAnsi2Unicode(lpszXML, lpszTagname, pData, dwLen);
}

BOOL CSBXPCCtrl::XML_AddInt(BSTR FAR* lpszXML, LPCTSTR lpszTagname, int nValue)
{
	return _XML_AddInt(lpszXML, lpszTagname, nValue);
}
BOOL CSBXPCCtrl::XML_AddLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname, long dwValue)
{
	return _XML_AddLong(lpszXML, lpszTagname, dwValue);
}
BOOL CSBXPCCtrl::XML_AddBoolean(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BOOL bValue)
{
	return _XML_AddBoolean(lpszXML, lpszTagname, bValue);
}
BOOL CSBXPCCtrl::XML_AddString(BSTR FAR* lpszXML, LPCTSTR lpszTagname, LPCTSTR lpszValue)
{
	return _XML_AddString(lpszXML, lpszTagname, lpszValue);
}
BOOL CSBXPCCtrl::XML_AddBinaryByte(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BYTE FAR* dwData, long dwLen)
{
	return _XML_AddBinaryByte(lpszXML, lpszTagname, dwData, dwLen);
}
BOOL CSBXPCCtrl::XML_AddBinaryWord(BSTR FAR* lpszXML, LPCTSTR lpszTagname, WORD FAR * dwData, long dwLen)
{
	return _XML_AddBinaryWord(lpszXML, lpszTagname, dwData, dwLen);
}
BOOL CSBXPCCtrl::XML_AddBinaryLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname, long FAR* dwData, long dwLen)
{
	return _XML_AddBinaryLong(lpszXML, lpszTagname, dwData, dwLen);
}
BOOL CSBXPCCtrl::XML_AddBinaryUnicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* lpszData)
{
	return _XML_AddBinaryUnicode(lpszXML, lpszTagname, lpszData);
}
BOOL CSBXPCCtrl::XML_AddBinaryGlyph(BSTR FAR* lpszXML, LPCTSTR lpszMessage, long dwWidth, long dwHeight, LPCTSTR lpszFontDescriptor)
{
	return _XML_AddBinaryGlyph(lpszXML, lpszMessage, dwWidth, dwHeight, lpszFontDescriptor);
}
BOOL CSBXPCCtrl::XML_AddBinaryNameGlyph(long dwMachineNumber, BSTR FAR* lpszXML, BSTR FAR* lpszName)
{
	return _XML_AddBinaryNameGlyph(dwMachineNumber, lpszXML, lpszName);
}

BOOL WINAPI DefaultTranseiveCallback(long dwCurrent, long dwTotal)
{
	BOOL ret;
	MSG msg;
	while ( (ret = PeekMessage(&msg,NULL,0,0,PM_REMOVE)) ) 
	{
		if (ret != -1)
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return TRUE;
}

void CSBXPCCtrl::UseInternalRedraw(BOOL redraw)
{
	if (redraw)
		_SetTranseiveCallback(DefaultTranseiveCallback);
	else
		_SetTranseiveCallback(NULL);
}