Attribute VB_Name = "mdlPublic"
Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (Destination As Any, Source As Any, ByVal Length As Long)
Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
Public Declare Function GetSystemDefaultLangID Lib "kernel32" () As Integer
Public Const gstrNoDevice = "No Device"

Public Const FK_Company = 1
Public Const FK_328 = 101
Public Const FK_338 = 102
Public Const FK_528 = 201
Public Const FK_526 = 202

Public Const gCompPhotoSize = 8192

Public Const MAX_BELLCOUNT_DAY = 24
Type BellInfo
    mValid(MAX_BELLCOUNT_DAY - 1) As Byte
    mHour(MAX_BELLCOUNT_DAY - 1) As Byte
    mMinute(MAX_BELLCOUNT_DAY - 1) As Byte
End Type             '36byte

Function ErrorPrint(aErrorCode As Long) As String
   
   Select Case aErrorCode
        Case 0
            ErrorPrint = "SUCCESS"
        Case 1
            ErrorPrint = "ERR_COMPORT_ERROR"
        Case 2
            ErrorPrint = "ERR_WRITE_FAIL"
        Case 3
            ErrorPrint = "ERR_READ_FAIL"
        Case 4
            ErrorPrint = "ERR_INVALID_PARAM"
        Case 5
            ErrorPrint = "ERR_NON_CARRYOUT"
        Case 6
            ErrorPrint = "ERR_LOG_END"
        Case 7
            ErrorPrint = "ERR_MEMORY"
        Case 8
            ErrorPrint = "ERR_MULTIUSER"
    End Select
End Function
Function pubIPAddrToLong(txtIP As String) As Long
    Dim dwIP As Double
    Dim k As Long
    Dim szTmp As String
    k = InStr(1, txtIP, ".", vbTextCompare)
    If k = 0 Then
        pubIPAddrToLong = 0
        Exit Function
    End If
    szTmp = Left(txtIP, k - 1)
    txtIP = Mid(txtIP, k + 1)
    dwIP = Val(szTmp)
    If dwIP > 255 Or dwIP < 0 Then
        pubIPAddrToLong = 0
        Exit Function
    End If
    
    k = InStr(1, txtIP, ".", vbTextCompare)
    If k = 0 Then
        pubIPAddrToLong = 0
        Exit Function
    End If
    szTmp = Left(txtIP, k - 1)
    txtIP = Mid(txtIP, k + 1)
    
    If Val(szTmp) > 255 Or Val(sztemp) < 0 Then
        pubIPAddrToLong = 0
        Exit Function
    End If
    dwIP = dwIP * 256 + Val(szTmp)
    k = InStr(1, txtIP, ".", vbTextCompare)
    If k = 0 Then
        pubIPAddrToLong = 0
        Exit Function
    End If
    szTmp = Left(txtIP, k - 1)
    txtIP = Mid(txtIP, k + 1)
    If Val(szTmp) > 255 Or Val(txtIP) > 255 Or Val(szTmp) < 0 Or Val(txtIP) < 0 Then
        pubIPAddrToLong = 0
        Exit Function
    End If
    dwIP = dwIP * 256 + Val(szTmp)
    dwIP = dwIP * 256 + Val(txtIP)
    If dwIP > 2147483647 Then dwIP = dwIP - 4294967296#
    pubIPAddrToLong = dwIP
End Function

Function pubLongToIPAddr(vValue As Long) As String
    Dim txtIP As String
    Dim remain As Long
    
    remain = vValue Mod 256: If remain < 0 Then remain = 256 + remain
    txtIP = CStr(remain): vValue = (vValue - remain) \ 256
    remain = vValue Mod 256: If remain < 0 Then remain = 256 + remain
    txtIP = CStr(remain) & "." & txtIP: vValue = (vValue - remain) \ 256
    remain = vValue Mod 256: If remain < 0 Then remain = 256 + remain
    txtIP = CStr(remain) & "." & txtIP: vValue = (vValue - remain) \ 256
    remain = vValue Mod 256: If remain < 0 Then remain = 256 + remain
    txtIP = CStr(remain) & "." & txtIP
    pubLongToIPAddr = txtIP
End Function
