#pragma once

// SBXPC.h : main header file for SBXPC.DLL

#if !defined( __AFXCTL_H__ )
#error "include 'afxctl.h' before including this file"
#endif

#include "resource.h"       // main symbols
#include "dispids.h"

// CSBXPCApp : See SBXPC.cpp for implementation.

class CSBXPCApp : public COleControlModule
{
public:
	BOOL InitInstance();
	int ExitInstance();
};

extern const GUID CDECL _tlid;
extern const WORD _wVerMajor;
extern const WORD _wVerMinor;
