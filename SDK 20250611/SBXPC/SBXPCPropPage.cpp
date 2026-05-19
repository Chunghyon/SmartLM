// SBXPCPropPage.cpp : Implementation of the CSBXPCPropPage property page class.

#include "stdafx.h"
#include "SBXPC.h"
#include "SBXPCPropPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


IMPLEMENT_DYNCREATE(CSBXPCPropPage, COlePropertyPage)



// Message map

BEGIN_MESSAGE_MAP(CSBXPCPropPage, COlePropertyPage)
END_MESSAGE_MAP()



// Initialize class factory and guid

IMPLEMENT_OLECREATE_EX(CSBXPCPropPage, "SBXPC.SBXPCPropPage.1",
	0x6b09f5cb, 0x8fcf, 0x424a, 0x87, 0x5c, 0x92, 0, 0x1e, 0x5e, 0x97, 0xa0)



// CSBXPCPropPage::CSBXPCPropPageFactory::UpdateRegistry -
// Adds or removes system registry entries for CSBXPCPropPage

BOOL CSBXPCPropPage::CSBXPCPropPageFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_SBXPC_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}



// CSBXPCPropPage::CSBXPCPropPage - Constructor

CSBXPCPropPage::CSBXPCPropPage() :
	COlePropertyPage(IDD, IDS_SBXPC_PPG_CAPTION)
{
}



// CSBXPCPropPage::DoDataExchange - Moves data between page and properties

void CSBXPCPropPage::DoDataExchange(CDataExchange* pDX)
{
	DDP_PostProcessing(pDX);
}



// CSBXPCPropPage message handlers
