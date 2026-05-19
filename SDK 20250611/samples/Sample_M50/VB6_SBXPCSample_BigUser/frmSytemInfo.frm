VERSION 5.00
Begin VB.Form frmSystemInfo 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Manage System Info"
   ClientHeight    =   4065
   ClientLeft      =   4995
   ClientTop       =   3105
   ClientWidth     =   7065
   Icon            =   "frmSytemInfo.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4065
   ScaleWidth      =   7065
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdSetDeviceTime 
      Caption         =   "SetDeviceTime"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   300
      TabIndex        =   14
      Top             =   1800
      Width           =   1875
   End
   Begin VB.CommandButton cmdGetDeviceTime 
      Caption         =   "GetDeviceTime"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   300
      TabIndex        =   13
      Top             =   1170
      Width           =   1875
   End
   Begin VB.CommandButton cmdGetDeviceInfo 
      Caption         =   "GetDeviceInfo"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   300
      TabIndex        =   12
      Top             =   3315
      Width           =   1875
   End
   Begin VB.CommandButton cmdExit 
      Caption         =   "Exit"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   4905
      TabIndex        =   11
      Top             =   1800
      Width           =   1875
   End
   Begin VB.CommandButton cmdPowerOn 
      Caption         =   "PowerOnDevice"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   2430
      TabIndex        =   10
      Top             =   1170
      Width           =   1875
   End
   Begin VB.CommandButton PowerOffDevice 
      Caption         =   "PowerOffDevice"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   2430
      TabIndex        =   9
      Top             =   1800
      Width           =   1875
   End
   Begin VB.CommandButton cmdSetDeviceInfo 
      Caption         =   "SetDeviceInfo"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   2550
      TabIndex        =   8
      Top             =   3315
      Width           =   1875
   End
   Begin VB.CommandButton cmdEnableDevice 
      Caption         =   "DisableDevice"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   4905
      TabIndex        =   7
      Top             =   1185
      Width           =   1875
   End
   Begin VB.ComboBox cmbSatus 
      BeginProperty DataFormat 
         Type            =   1
         Format          =   "0"
         HaveTrueFalseNull=   0
         FirstDayOfWeek  =   0
         FirstWeekOfYear =   0
         LCID            =   2052
         SubFormatType   =   1
      EndProperty
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   405
      ItemData        =   "frmSytemInfo.frx":0442
      Left            =   2220
      List            =   "frmSytemInfo.frx":048E
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   2640
      Width           =   1320
   End
   Begin VB.TextBox txtSetDevInfo 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   405
      Left            =   5040
      TabIndex        =   4
      Top             =   2620
      Width           =   1725
   End
   Begin VB.CommandButton cmdGetDeviceStaus 
      Caption         =   "GetDeviceStatus"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   480
      Left            =   4905
      TabIndex        =   2
      Top             =   3315
      Width           =   1875
   End
   Begin VB.CheckBox chkEnableDevice 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   345
      Left            =   4650
      TabIndex        =   1
      Top             =   1260
      Width           =   225
   End
   Begin VB.Label Label1 
      Caption         =   "Status Value:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   345
      Left            =   3765
      TabIndex        =   6
      Top             =   2670
      Width           =   1275
   End
   Begin VB.Label lblStatus 
      Caption         =   "Status Paramerter:  Info Paramerter:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   615
      Left            =   330
      TabIndex        =   3
      Top             =   2535
      Width           =   1740
   End
   Begin VB.Label lblMessage 
      Alignment       =   2  'Center
      BorderStyle     =   1  'Fixed Single
      Caption         =   "Message"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   345
      TabIndex        =   0
      Top             =   405
      Width           =   6450
   End
End
Attribute VB_Name = "frmSystemInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mMachineNumber As Long

Private Sub cmdEnableDevice_Click()
    Dim vFlag As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vFlag = chkEnableDevice.Value
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, vFlag)
    If vRet = True Then
        If vFlag = 1 Then
            lblMessage.Caption = "Enable Device Success!"
        Else
            lblMessage.Caption = "Disable Device Success!"
        End If
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        Exit Sub
    End If
    
    If chkEnableDevice.Value = 1 Then
        chkEnableDevice.Value = 0
    Else
        chkEnableDevice.Value = 1
    End If
    
    DoEvents
End Sub

Private Sub cmdGetDeviceInfo_Click()
    Dim vInfo As Long
    Dim vValue(0) As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim a As Integer, b As Integer, c As Integer, d As Integer
    lblMessage.Caption = "Working..."
    DoEvents
    
    vInfo = cmbSatus.ListIndex + 1
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.GetDeviceInfo(mMachineNumber, vInfo, vValue(0))
    If vRet = True Then
        Select Case vInfo
            Case 1:  lblMessage.Caption = "(1) = ManagerCount = " & vValue(0)
            Case 2:  lblMessage.Caption = "(2) = Device ID = " & vValue(0)
            Case 3:  lblMessage.Caption = "(3) = Language = " & vValue(0)
            Case 4:  lblMessage.Caption = "(4) = PowerOffTime = " & vValue(0)
            Case 5:  lblMessage.Caption = "(5) = Lock release time = " & vValue(0)
            Case 6:  lblMessage.Caption = "(6) = GLogWarning = " & vValue(0)
            Case 7:  lblMessage.Caption = "(7) = SLogWarning = " & vValue(0)
            Case 8:  lblMessage.Caption = "(8) = ReVerifyTime = " & vValue(0)
            Case 9:  lblMessage.Caption = "(9) = Baudrate = " & vValue(0)
            Case 10: lblMessage.Caption = "(10) = Parity check = " & vValue(0)
            Case 11: lblMessage.Caption = "(11) = Stop bit = " & vValue(0)
            Case 12: lblMessage.Caption = "(12) = Date Seperator = " & vValue(0)
            Case 13: lblMessage.Caption = "(13) = Identificatin mode = " & vValue(0)
            Case 14: lblMessage.Caption = "(14) = LockOperate = " & vValue(0)
            Case 15: lblMessage.Caption = "(15) = Door sensor type = " & vValue(0)
            Case 16: lblMessage.Caption = "(16) = Door open time limit = " & vValue(0)
            Case 17: lblMessage.Caption = "(17) = Anti-pass = " & vValue(0)
            Case 18: lblMessage.Caption = "(18) = Auto sleep time = " & vValue(0)
            Case 19: lblMessage.Caption = "(19) = Daylight offset = " & vValue(0)
            Case 20: lblMessage.Caption = "(20) = UDP Server = " & pubLongToIPAddr(vValue(0))
            Case 21: lblMessage.Caption = "(21) = DHCP Use = " & vValue(0)
            Case 22: lblMessage.Caption = "(22) = NOT SUPPORTED = "
            Case 23: lblMessage.Caption = "(23) = Manager PC IP Address = " & pubLongToIPAddr(vValue(0))
            Case 24: lblMessage.Caption = "(24) = Event Send Type = " & vValue(0)
        End Select
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetDeviceStaus_Click()
    Dim vStatus As Long
    Dim vValue(0) As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vStatus = cmbSatus.ListIndex + 1
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.GetDeviceStatus(mMachineNumber, vStatus, vValue(0))
    If vRet = True Then
        Select Case vStatus
            Case 1:  lblMessage.Caption = "(1) = Manager count = " & vValue(0)
            Case 2:  lblMessage.Caption = "(2) = User count = " & vValue(0)
            Case 3:  lblMessage.Caption = "(3) = Fp count = " & vValue(0)
            Case 4:  lblMessage.Caption = "(4) = Password count = " & vValue(0)
            Case 5:  lblMessage.Caption = "(5) = SLog count = " & vValue(0)
            Case 6:  lblMessage.Caption = "(6) = GLog count = " & vValue(0)
            Case 7:  lblMessage.Caption = "(7) = Card count = " & vValue(0)
            Case 8:  lblMessage.Caption = "(8) = Alarm status = " & vValue(0)
        End Select
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetDeviceTime_Click()
    Dim vYear As Long
    Dim vMonth As Long
    Dim vDay As Long
    Dim vHour As Long
    Dim vMinute As Long
    Dim vSecond As Long
    Dim vDayOfWeek As Long
    Dim strDataTime As String
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.GetDeviceTime(mMachineNumber, vYear, vMonth, vDay, vHour, vMinute, vSecond, vDayOfWeek)
    If vRet = True Then
        If vDayOfWeek = 0 Then vDayOfWeek = 7
        strDataTime = "Date = " & CStr(vYear) & "/" & CStr(vMonth) & "/" & CStr(vDay) & " , " & GetWeekDay(vDayOfWeek) & _
                        " , Time = " & CStr(vHour) & ":" & Format(CStr(vMinute), "0#")
        lblMessage.Caption = strDataTime
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdGetDoorStatus_Click()
    Dim vInfo As Long
    Dim vValue(0) As Long
    Dim vErrorCode As Long
    Dim vRet As Boolean
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.GetDoorStatus(mMachineNumber, vValue(0))
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    txtSetDevInfo.Text = vValue(0)
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents

End Sub

Private Sub cmdPowerOn_Click()
    frmMain.SB100BPC1.PowerOnAllDevice
End Sub

Private Sub cmdSetDeviceInfo_Click()
    Dim vInfo As Long
    Dim vValue As Long
    Dim vErrorCode As Long
    Dim vRet As Boolean
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vInfo = cmbSatus.ListIndex + 1
    If vInfo = 20 Then
        vValue = pubIPAddrToLong(txtSetDevInfo.Text)
    ElseIf vInfo = 23 Then  'Manager PC IP Address
        vValue = pubIPAddrToLong(txtSetDevInfo.Text)
        If vValue = 0 Then
            lblMessage.Caption = "Incorrect IP Address!"
            Exit Sub
        End If
    Else
        vValue = Val(txtSetDevInfo.Text)
    End If
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDeviceInfo(mMachineNumber, vInfo, vValue)
    If vRet = True Then
        lblMessage.Caption = "Success!"
        
        'SmackBio
        If vInfo = 2 Then
            mMachineNumber = vValue
            Sleep 1000
        End If
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdSetDeviceTime_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDeviceTime(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdSetDoorStatus_Click()
    Dim vInfo As Long
    Dim vValue As Long
    Dim vErrorCode As Long
    Dim vRet As Boolean
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vValue = Val(txtSetDevInfo.Text)
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDoorStatus(mMachineNumber, vValue)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub PowerOffDevice_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.PowerOffDevice(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        lblMessage.Caption = ErrorPrint(vErrorCode)
        frmMain.SB100BPC1.GetLastError vErrorCode
    End If
End Sub

Private Sub Form_Load()
    cmbSatus.ListIndex = 0
    mMachineNumber = frmMain.gMachineNumber
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub

Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True
End Sub

Private Function GetWeekDay(anDay As Long) As String
   Select Case anDay
        Case 1
            GetWeekDay = "Sunday"
        Case 2
            GetWeekDay = "Monday"
        Case 3
            GetWeekDay = "Tuesday"
        Case 4
            GetWeekDay = "Wednesday"
        Case 5
            GetWeekDay = "Thursday"
        Case 6
            GetWeekDay = "Friday"
        Case 7
            GetWeekDay = "Saturday"
    End Select
End Function

