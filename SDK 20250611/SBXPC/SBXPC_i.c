

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


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


#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, LIBID_SBXPCLib,0x08B7A8C2,0xFA2E,0x445D,0x81,0xF9,0x82,0x54,0xC7,0xB3,0xFD,0x16);


MIDL_DEFINE_GUID(IID, DIID__DSBXPC,0x6C1D3CFC,0x712A,0x41B4,0xA5,0x3F,0xCC,0x10,0xF6,0x41,0x27,0x91);


MIDL_DEFINE_GUID(IID, DIID__DSBXPCEvents,0x9FF3A121,0x0E3D,0x4C35,0xA7,0x76,0xC9,0xC7,0x66,0xFB,0x14,0xBE);


MIDL_DEFINE_GUID(CLSID, CLSID_SBXPC,0x2894E36D,0x6941,0x48E0,0xAB,0xF9,0x0D,0x38,0x24,0x18,0x84,0xFB);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



