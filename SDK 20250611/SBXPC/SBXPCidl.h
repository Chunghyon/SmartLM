

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Mon Jan 29 09:21:43 2018
 */
/* Compiler settings for SBXPC.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __SBXPCidl_h__
#define __SBXPCidl_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DSBXPC_FWD_DEFINED__
#define ___DSBXPC_FWD_DEFINED__
typedef interface _DSBXPC _DSBXPC;
#endif 	/* ___DSBXPC_FWD_DEFINED__ */


#ifndef ___DSBXPCEvents_FWD_DEFINED__
#define ___DSBXPCEvents_FWD_DEFINED__
typedef interface _DSBXPCEvents _DSBXPCEvents;
#endif 	/* ___DSBXPCEvents_FWD_DEFINED__ */


#ifndef __SBXPC_FWD_DEFINED__
#define __SBXPC_FWD_DEFINED__

#ifdef __cplusplus
typedef class SBXPC SBXPC;
#else
typedef struct SBXPC SBXPC;
#endif /* __cplusplus */

#endif 	/* __SBXPC_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 



#ifndef __SBXPCLib_LIBRARY_DEFINED__
#define __SBXPCLib_LIBRARY_DEFINED__

/* library SBXPCLib */
/* [control][helpstring][helpfile][version][uuid] */ 


DEFINE_GUID(LIBID_SBXPCLib,0x08B7A8C2,0xFA2E,0x445D,0x81,0xF9,0x82,0x54,0xC7,0xB3,0xFD,0x16);

#ifndef ___DSBXPC_DISPINTERFACE_DEFINED__
#define ___DSBXPC_DISPINTERFACE_DEFINED__

/* dispinterface _DSBXPC */
/* [helpstring][uuid] */ 


DEFINE_GUID(DIID__DSBXPC,0x6C1D3CFC,0x712A,0x41B4,0xA5,0x3F,0xCC,0x10,0xF6,0x41,0x27,0x91);

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("6C1D3CFC-712A-41B4-A53F-CC10F6412791")
    _DSBXPC : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DSBXPCVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DSBXPC * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DSBXPC * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DSBXPC * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DSBXPC * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DSBXPC * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DSBXPC * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DSBXPC * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DSBXPCVtbl;

    interface _DSBXPC
    {
        CONST_VTBL struct _DSBXPCVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DSBXPC_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DSBXPC_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DSBXPC_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DSBXPC_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DSBXPC_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DSBXPC_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DSBXPC_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DSBXPC_DISPINTERFACE_DEFINED__ */


#ifndef ___DSBXPCEvents_DISPINTERFACE_DEFINED__
#define ___DSBXPCEvents_DISPINTERFACE_DEFINED__

/* dispinterface _DSBXPCEvents */
/* [helpstring][uuid] */ 


DEFINE_GUID(DIID__DSBXPCEvents,0x9FF3A121,0x0E3D,0x4C35,0xA7,0x76,0xC9,0xC7,0x66,0xFB,0x14,0xBE);

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("9FF3A121-0E3D-4C35-A776-C9C766FB14BE")
    _DSBXPCEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DSBXPCEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DSBXPCEvents * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DSBXPCEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DSBXPCEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DSBXPCEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DSBXPCEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DSBXPCEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DSBXPCEvents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DSBXPCEventsVtbl;

    interface _DSBXPCEvents
    {
        CONST_VTBL struct _DSBXPCEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DSBXPCEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DSBXPCEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DSBXPCEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DSBXPCEvents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DSBXPCEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DSBXPCEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DSBXPCEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DSBXPCEvents_DISPINTERFACE_DEFINED__ */


DEFINE_GUID(CLSID_SBXPC,0x2894E36D,0x6941,0x48E0,0xAB,0xF9,0x0D,0x38,0x24,0x18,0x84,0xFB);

#ifdef __cplusplus

class DECLSPEC_UUID("2894E36D-6941-48E0-ABF9-0D38241884FB")
SBXPC;
#endif
#endif /* __SBXPCLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


