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

Public Const MAX_BELLCOUNT_DAY = 12
Type BellInfo
    mValid(MAX_BELLCOUNT_DAY - 1) As Byte
    mHour(MAX_BELLCOUNT_DAY - 1) As Byte
    mMinute(MAX_BELLCOUNT_DAY - 1) As Byte
End Type             '24byte

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
