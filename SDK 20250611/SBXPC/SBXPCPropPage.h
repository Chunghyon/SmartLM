#pragma once

// SBXPCPropPage.h : Declaration of the CSBXPCPropPage property page class.


// CSBXPCPropPage : See SBXPCPropPage.cpp for implementation.

class CSBXPCPropPage : public COlePropertyPage
{
	DECLARE_DYNCREATE(CSBXPCPropPage)
	DECLARE_OLECREATE_EX(CSBXPCPropPage)

// Constructor
public:
	CSBXPCPropPage();

// Dialog Data
	enum { IDD = IDD_PROPPAGE_SBXPC };

// Implementation
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Message maps
protected:
	DECLARE_MESSAGE_MAP()
};

