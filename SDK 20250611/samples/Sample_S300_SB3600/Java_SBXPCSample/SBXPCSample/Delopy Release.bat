mkdir "..\..\Release\Java\x86\"
mkdir "..\..\Release\Java\x64\"

copy GEN_FONT.dll "..\..\Release\Java\GEN_FONT.dll"
copy SBPCCOMM.dll "..\..\Release\Java\SBPCCOMM.dll"
copy SBXPCDLL.dll "..\..\Release\Java\SBXPCDLL.dll"
copy "x86\SBXPCJavaProxy.dll" "..\..\Release\Java\x86\SBXPCJavaProxy.dll"

copy GEN_FONT64.dll "..\..\Release\Java\GEN_FONT64.dll"
copy SBPCCOMM64.dll "..\..\Release\Java\SBPCCOMM64.dll"
copy SBXPCDLL64.dll "..\..\Release\Java\SBXPCDLL64.dll"
copy "x64\SBXPCJavaProxy.dll" "..\..\Release\Java\x64\SBXPCJavaProxy.dll"

copy SBXPCSample.jar "..\..\Release\Java\SBXPCSample.jar"

pause