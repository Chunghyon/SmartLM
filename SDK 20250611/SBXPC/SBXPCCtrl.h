#pragma once

#include "../bin/SBXPCDLL_API.h"

// SBXPCCtrl.h : Declaration of the CSBXPCCtrl ActiveX Control class.


// CSBXPCCtrl : See SBXPCCtrl.cpp for implementation.

class CSBXPCCtrl : public COleControl
{
	DECLARE_DYNCREATE(CSBXPCCtrl)

// Constructor
public:
	CSBXPCCtrl();

// Overrides
public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();

// Implementation
protected:
	~CSBXPCCtrl();

	DECLARE_OLECREATE_EX(CSBXPCCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CSBXPCCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CSBXPCCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CSBXPCCtrl)		// Type name and misc status

// Message maps
	DECLARE_MESSAGE_MAP()
	
	afx_msg void OnCommPortChanged();
	afx_msg void OnBaudrateChanged();
	afx_msg void OnReadMarkChanged();

	//////////////////////////////////////////////////////////////////////////
	// API functions
	afx_msg BOOL SetMachineType(LPCTSTR lpszMachineType);
	afx_msg void DotNET();
	afx_msg BOOL GetEnrollData(long dwMachineNumber,long dwEnrollNumber,long dwEMachineNumber,long dwBackupNumber,long FAR*  dwMachinePrivilege,VARIANT FAR* dwEnrollData,long FAR* dwPassWord);
	afx_msg BOOL GetEnrollData1(long dwMachineNumber, long dwEnrollNumber, long dwBackupNumber, long FAR* dwMachinePrivilege, long FAR* dwEnrollData, long FAR* dwPassWord);
	afx_msg BOOL SetEnrollData(long dwMachineNumber,long dwEnrollNumber,long dwEMachineNumber,long dwBackupNumber,long dwMachinePrivilege,VARIANT FAR* dwEnrollData,long dwPassWord);
	afx_msg BOOL SetEnrollData1(long dwMachineNumber, long dwEnrollNumber, long dwBackupNumber, long dwMachinePrivilege, long FAR* dwEnrollData, long dwPassWord);
	afx_msg BOOL DeleteEnrollData(long dwMachineNumber,long dwEnrollNumber,long dwEMachineNumber,long dwBackupNumber);
	afx_msg BOOL ReadSuperLogData(long dwMachineNumber);
	afx_msg BOOL GetSuperLogData(long dwMachineNumber,long* dwTMachineNumber,long* dwSEnrollNumber,long* dwSMachineNumber,long* dwGEnrollNumber,long* dwGMachineNumber,long* dwManipulation,long* dwBackupNumber,long* dwYear,long* dwMonth,long* dwDay,long* dwHour,long* dwMinute, long* dwSecond);
	afx_msg BOOL ReadGeneralLogData(long dwMachineNumber);
	afx_msg BOOL GetGeneralLogData(long dwMachineNumber,long* dwTMachineNumber,long* dwEnrollNumber,long* dwEMachineNumber,long* dwVerifyMode,long* dwYear,long* dwMonth,long* dwDay,long* dwHour,long* dwMinute, long* dwSecond);
	afx_msg BOOL ReadAllSLogData(long dwMachineNumber);
	afx_msg BOOL GetAllSLogData(long dwMachineNumber,long* dwTMachineNumber,long* dwSEnrollNumber,long* dwSMachineNumber,long* dwGEnrollNumber,long* dwGMachineNumber,long* dwManipulation,long* dwBackupNumber,long* dwYear,long* dwMonth,long* dwDay,long* dwHour,long* dwMinute, long* dwSecond);
	afx_msg BOOL ReadAllGLogData(long dwMachineNumber);
	afx_msg BOOL GetAllGLogData(long dwMachineNumber,long* dwTMachineNumber,long* dwEnrollNumber,long* dwEMachineNumber,long* dwVerifyMode,long* dwYear,long* dwMonth,long* dwDay,long* dwHour,long* dwMinute, long* dwSecond);
	afx_msg BOOL GetDeviceStatus(long dwMachineNumber, long dwStatus, long* dwValue);
	afx_msg BOOL GetDeviceInfo(long dwMachineNumber, long dwInfo, long* dwValue);
	afx_msg BOOL SetDeviceInfo(long dwMachineNumber, long dwInfo, long dwValue);
	afx_msg BOOL EnableDevice(long dwMachineNumber, BOOL bFlag);
	afx_msg BOOL EnableUser(long dwMachineNumber,long dwEnrollNumber,long dwEMachineNumber,long dwBackupNumber,BOOL bFlag);
	afx_msg BOOL GetDeviceTime(long dwMachineNumber,long* dwYear,long* dwMonth,long* dwDay,long* dwHour,long* dwMinute,long* dwSecond,long* dwDayOfWeek);
	afx_msg BOOL SetDeviceTime(long dwMachineNumber);
	afx_msg BOOL SetDeviceTime1(long dwMachineNumber,long dwYear,long dwMonth,long dwDay,long dwHour,long dwMinute,long dwSecond);
	afx_msg BOOL PowerOnAllDevice();
	afx_msg BOOL PowerOffDevice(long dwMachineNumber);
	afx_msg BOOL ModifyPrivilege(long dwMachineNumber, long dwEnrollNumber, long dwEMachineNumber, long dwBackupNumber, long dwMachinePrivilege);
	afx_msg BOOL ReadAllUserID(long dwMachineNumber);
	afx_msg BOOL GetAllUserID(long dwMachineNumber,long* dwEnrollNumber,long* dwEMachineNumber,long* dwBackupNumber,long* dwMachinePrivilege,long* dwEnable);
	afx_msg BOOL GetSerialNumber(long dwMachineNumber, BSTR FAR* lpszSerialNumber);
	afx_msg long GetBackupNumber(long dwMachineNumber);
	afx_msg BOOL GetProductCode(long dwMachineNumber, BSTR FAR* lpszProductCode);
	afx_msg BOOL ClearKeeperData(long dwMachineNumber);
	afx_msg BOOL EmptyEnrollData(long dwMachineNumber);
	afx_msg BOOL EmptyGeneralLogData(long dwMachineNumber);
	afx_msg BOOL EmptySuperLogData(long dwMachineNumber);
	afx_msg BOOL GetUserName(long DeviceKind,long dwMachineNumber,long dwEnrollNumber,long dwEMachineNumber,VARIANT FAR* lpszUserName);
	afx_msg BOOL GetUserName1(long dwMachineNumber, long dwEnrollNumber, BSTR FAR* lpszUserName);
	afx_msg BOOL SetUserName(long DeviceKind,long dwMachineNumber,long dwEnrollNumber,long dwEMachineNumber,VARIANT FAR* lpszUserName);
	afx_msg BOOL SetUserName1(long dwMachineNumber, long dwEnrollNumber, BSTR FAR* lpszUserName);
	afx_msg BOOL GetCompanyName(long dwMachineNumber, VARIANT FAR* dwCompanyName);
	afx_msg BOOL GetCompanyName1(long dwMachineNumber, BSTR FAR* dwCompanyName);
	afx_msg BOOL SetCompanyName(long dwMachineNumber, long vKind, VARIANT FAR* CompanyName);
	afx_msg BOOL SetCompanyName1(long dwMachineNumber, long vKind, BSTR FAR* dwCompanyName);
	afx_msg BOOL GetDoorStatus(long dwMachineNumber, long* dwValue);
	afx_msg BOOL SetDoorStatus(long dwMachineNumber, long dwValue);
	afx_msg BOOL GetBellTime(long dwMachineNumber, long* dwValue, long* dwBellInfo);
	afx_msg BOOL SetBellTime(long dwMachineNumber, long dwValue, long* dwBellInfo);
	afx_msg BOOL ConnectSerial(long dwMachineNumber, long dwCommPort, long dwBaudRate);
	afx_msg BOOL ConnectTcpip(long dwMachineNumber, BSTR FAR* lpszIPAddress, long dwPortNumber, long dwPassWord);
	afx_msg long ConnectP2p(BSTR FAR* lpszMachineID, BSTR FAR* lpszServerIPAddress, long dwServerPortNumber, long dwPassWord, long* pnError);
	afx_msg long PrepareP2p(BSTR FAR* lpszMachineID, BSTR FAR* lpszServerIPAddress, long dwServerPortNumber, long* dwYearStart,long* dwMonthStart,long* dwDayStart,long* dwYearEnd,long* dwMonthEnd,long* dwDayEnd, long* pnError);
	afx_msg void Disconnect();
	afx_msg void Disconnect1(long dwMachineNumber);
	afx_msg void DisconnectAll();
	afx_msg BOOL SetIPAddress(BSTR FAR* lpszIPAddress, long dwPortNumber, long dwPassWord);
	afx_msg BOOL OpenCommPort(long dwMachineNumber);
	afx_msg void CloseCommPort();
	afx_msg BOOL GetLastError(long* dwErrorCode);
	afx_msg BOOL GeneralOperationXML( BSTR FAR* lpszReqNResXML );
	afx_msg BOOL GeneralOperationXML1( long dwMachineNumber, BSTR FAR* lpszReqNResXML );
	afx_msg BOOL GetDeviceLongInfo(long dwMachineNumber, long dwInfo, long FAR* dwValue);
	afx_msg BOOL SetDeviceLongInfo(long dwMachineNumber, long dwInfo, long FAR* dwValue);
	afx_msg BOOL ModifyDuressFP(long dwMachineNumber, long dwEnrollNumber, long dwBackupNumber, long dwDuressSetting);
	afx_msg BOOL GetMachineIP( LPCTSTR lpszProduct, long dwMachineNumber, BSTR FAR* lpszIPBuf );
	afx_msg BOOL GetDepartName(long dwMachineNumber, long dwDepartNumber, long dwDepartOrDaigong, BSTR FAR* lpszDepartName);
	afx_msg BOOL SetDepartName(long dwMachineNumber, long dwDepartNumber, long dwDepartOrDaigong, BSTR FAR* lpszDepartName);
	afx_msg long GetInternalFwVer(long dwMachineNumber);
	afx_msg BOOL ReadGLogWithPos(long dwMachineNumber, long dwStartPos, long dwEndPos);
	afx_msg BOOL ReadSLogWithPos(long dwMachineNumber, long dwStartPos, long dwEndPos);
	afx_msg BOOL GetDeviceUniqueID(long dwMachineNumber, VARIANT FAR* dwDeviceUniqueID);
	afx_msg BOOL GetDeviceUniqueID1(long dwMachineNumber, BSTR FAR* dwDeviceUniqueID);
	afx_msg BOOL GetDeviceModel(long dwMachineNumber, long* dwIsBigUserId, long* dwCompanyType, long* dwMachineType, long* dwMachineVersion);

	afx_msg BOOL StartEventCapture( long dwCommType, long dwParam1, long dwParam2 );
	afx_msg void StopEventCapture();

	//xml helper interfaces
	afx_msg long XML_ParseInt(BSTR FAR* lpszXML, LPCTSTR lpszTagname);
	afx_msg long XML_ParseLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname);
	afx_msg BOOL XML_ParseBoolean(BSTR FAR* lpszXML, LPCTSTR lpszTagname);
	afx_msg BOOL XML_ParseString(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* lpszValue);
	afx_msg BOOL XML_ParseBinaryByte(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BYTE FAR* pData, long dwLen);
	afx_msg BOOL XML_ParseBinaryWord(BSTR FAR* lpszXML, LPCTSTR lpszTagname, WORD FAR* pData, long dwLen);
	afx_msg BOOL XML_ParseBinaryLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname, long FAR* pData, long dwLen);
	afx_msg BOOL XML_ParseBinaryUnicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* pData, long dwLen);
	afx_msg BOOL XML_ParseMultiUnicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* pData, long dwLen);
	afx_msg BOOL XML_ParseBinaryAnsi2Unicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* pData, long dwLen);

	afx_msg BOOL XML_AddInt(BSTR FAR* lpszXML, LPCTSTR lpszTagname, int nValue);
	afx_msg BOOL XML_AddLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname, long dwValue);
	afx_msg BOOL XML_AddBoolean(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BOOL bValue);
	afx_msg BOOL XML_AddString(BSTR FAR* lpszXML, LPCTSTR lpszTagname, LPCTSTR lpszValue);
	afx_msg BOOL XML_AddBinaryByte(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BYTE FAR* dwData, long dwLen);
	afx_msg BOOL XML_AddBinaryWord(BSTR FAR* lpszXML, LPCTSTR lpszTagname, WORD FAR* dwData, long dwLen);
	afx_msg BOOL XML_AddBinaryLong(BSTR FAR* lpszXML, LPCTSTR lpszTagname, long FAR* dwData, long dwLen);
	afx_msg BOOL XML_AddBinaryUnicode(BSTR FAR* lpszXML, LPCTSTR lpszTagname, BSTR FAR* lpszData);
	afx_msg BOOL XML_AddBinaryGlyph(BSTR FAR* lpszXML, LPCTSTR lpszMessage, long width, long height, LPCTSTR lpszFontDescriptor);
	afx_msg BOOL XML_AddBinaryNameGlyph(long dwMachineNumber, BSTR FAR* lpszXML, BSTR FAR* lpszName);

	afx_msg void UseInternalRedraw(BOOL redraw);

// Dispatch maps
	DECLARE_DISPATCH_MAP()

	afx_msg void AboutBox();

// Event maps
	void FireOnReceiveEventXML( BSTR lpszEventXML )
	{
		FireEvent(EVENTID_ONRECEIVEEVENTXML, EVENT_PARAM(VTS_BSTR),
			lpszEventXML);
	}
	LRESULT FireOnReceiveEventXML_byWindowsMessage(WPARAM wParam, LPARAM lParam)
	{
		if (wParam == 0)
		{
			CString str;
			str = (char*)lParam;
			FireOnReceiveEventXML(str.AllocSysString());
		}
		return 0;
	}
	DECLARE_EVENT_MAP()

// Dispatch and event IDs
public:

// Private members
public:
	CString m_strMachineType;
	DWORD	m_dwMachineID;

	int		m_ComMode;
	long	m_commPort;
	long	m_baudrate;
	CStringW	m_ipAddr;
	int		m_portNumber;
	DWORD	m_dwPassword;
	BOOL	m_readMark;

public:
	static void WINAPI Fire_Event(void* context, char* xml)
	{
		reinterpret_cast<CSBXPCCtrl*>(context)->SendMessage(WM_USER, 0, (LPARAM)xml);
	}
};
