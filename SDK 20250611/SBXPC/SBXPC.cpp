// SBXPC.cpp : Implementation of CSBXPCApp and DLL registration.

#include "stdafx.h"
#include "SBXPC.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


CSBXPCApp theApp;

const GUID CDECL BASED_CODE _tlid =
		{ 0x8B7A8C2, 0xFA2E, 0x445D, { 0x81, 0xF9, 0x82, 0x54, 0xC7, 0xB3, 0xFD, 0x16 } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;

// CSBXPCApp::InitInstance - DLL initialization

BOOL CSBXPCApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{
		// TODO: Add your own module initialization code here.
	}

	return bInit;
}



// CSBXPCApp::ExitInstance - DLL termination

int CSBXPCApp::ExitInstance()
{
	// TODO: Add your own module termination code here.

	return COleControlModule::ExitInstance();
}



// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}



// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
