mkdir .\x86\
copy /y ..\..\..\..\bin\x86\SBXPCJavaProxy.dll .\x86\
mkdir .\x64\
copy /y ..\..\..\..\bin\x64\SBXPCJavaProxy.dll .\x64\

copy /y ..\..\..\..\bin\SBXPCDLL64.dll .
copy /y ..\..\..\..\bin\SBXPCDLL.dll .
copy /y ..\..\..\..\bin\SBPCCOMM64.dll .
copy /y ..\..\..\..\bin\SBPCCOMM.dll .
copy /y ..\..\..\..\bin\GEN_FONT64.dll .
copy /y ..\..\..\..\bin\GEN_FONT.dll .

copy /y ..\..\..\..\JavaSBXPC\SBXPCSampleLIB\SBXPCSampleLIB.jar .\lib\

pause