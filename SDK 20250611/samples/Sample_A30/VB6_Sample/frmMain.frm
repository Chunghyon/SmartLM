VERSION 5.00
Object = "{08B7A8C2-FA2E-445D-81F9-8254C7B3FD16}#1.0#0"; "SBXPC.ocx"
Begin VB.Form frmMain 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "z"
   ClientHeight    =   7680
   ClientLeft      =   4815
   ClientTop       =   3135
   ClientWidth     =   7890
   FillColor       =   &H008080FF&
   Icon            =   "frmMain.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7680
   ScaleWidth      =   7890
   StartUpPosition =   2  'CenterScreen
   Begin SBXPCLib.SBXPC SB100BPC1 
      Height          =   495
      Left            =   120
      TabIndex        =   34
      Top             =   240
      Visible         =   0   'False
      Width           =   1815
      _Version        =   65536
      _ExtentX        =   3201
      _ExtentY        =   873
      _StockProps     =   0
   End
   Begin VB.CommandButton cmdEvent 
      Caption         =   "Event Monitor"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   600
      TabIndex        =   33
      Top             =   6600
      Width           =   1695
   End
   Begin VB.Frame Frame4 
      Caption         =   "Connect"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1005
      Left            =   240
      TabIndex        =   19
      Top             =   1200
      Width           =   7380
      Begin VB.CommandButton cmdOpen 
         Caption         =   "Open"
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
         Left            =   4020
         TabIndex        =   23
         Top             =   360
         Width           =   1365
      End
      Begin VB.CommandButton cmdClose 
         Caption         =   "Close"
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
         Left            =   5670
         TabIndex        =   22
         Top             =   360
         Width           =   1365
      End
      Begin VB.ComboBox cmbMachineNumber 
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
         ItemData        =   "frmMain.frx":0442
         Left            =   2085
         List            =   "frmMain.frx":0461
         Style           =   2  'Dropdown List
         TabIndex        =   20
         Top             =   375
         Width           =   1335
      End
      Begin VB.Label lblMachineNumber 
         AutoSize        =   -1  'True
         Caption         =   "Machine Number :"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   210
         TabIndex        =   21
         Top             =   435
         Width           =   1695
      End
   End
   Begin VB.OptionButton optSerialDevice 
      Caption         =   "Serial Device"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   435
      TabIndex        =   15
      Top             =   2265
      Width           =   2085
   End
   Begin VB.OptionButton optNetworkDevice 
      Caption         =   "Network Device"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   4200
      TabIndex        =   14
      Top             =   2280
      Value           =   -1  'True
      Width           =   2205
   End
   Begin VB.Frame Frame3 
      Caption         =   "    "
      Height          =   1890
      Left            =   255
      TabIndex        =   11
      Top             =   2295
      Width           =   3660
      Begin VB.ComboBox cmbBaudrate 
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
         ItemData        =   "frmMain.frx":0480
         Left            =   1680
         List            =   "frmMain.frx":0493
         Style           =   2  'Dropdown List
         TabIndex        =   28
         Top             =   1080
         Width           =   1695
      End
      Begin VB.ComboBox cmbComPort 
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
         ItemData        =   "frmMain.frx":04BA
         Left            =   1680
         List            =   "frmMain.frx":04D3
         Style           =   2  'Dropdown List
         TabIndex        =   12
         Top             =   456
         Width           =   1695
      End
      Begin VB.Label lblBaudrate 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "Baudrate : "
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   360
         TabIndex        =   27
         Top             =   1080
         Width           =   990
      End
      Begin VB.Label lblComPort 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "ComPort : "
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   315
         TabIndex        =   13
         Top             =   465
         Width           =   1005
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "  "
      Height          =   1920
      Left            =   4080
      TabIndex        =   6
      Top             =   2280
      Width           =   3585
      Begin VB.TextBox txtPassword 
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
         Left            =   1725
         TabIndex        =   17
         Text            =   "0"
         Top             =   1320
         Width           =   1692
      End
      Begin VB.TextBox txtIPAddress 
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
         Left            =   1725
         TabIndex        =   8
         Text            =   "192.168.1.224"
         Top             =   360
         Width           =   1692
      End
      Begin VB.TextBox txtPortNo 
         Enabled         =   0   'False
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
         Left            =   1725
         TabIndex        =   7
         Text            =   "5005"
         Top             =   840
         Width           =   1692
      End
      Begin VB.Label lblPassword 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "Password :"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   360
         TabIndex        =   18
         Top             =   1320
         Width           =   1005
      End
      Begin VB.Label lblIPAddress 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "Ip Address :"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   360
         TabIndex        =   10
         Top             =   360
         Width           =   1125
      End
      Begin VB.Label lblPortNo 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "Port Number :"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   240
         TabIndex        =   9
         Top             =   840
         Width           =   1305
      End
   End
   Begin VB.Frame frmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMainfrmMain 
      Caption         =   "Management Group"
      ClipControls    =   0   'False
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2880
      Left            =   240
      TabIndex        =   1
      Top             =   4440
      Width           =   7440
      Begin VB.CommandButton btnLogTZ 
         Caption         =   "LogTZone"
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
         Left            =   5160
         TabIndex        =   32
         Top             =   2880
         Visible         =   0   'False
         Width           =   1875
      End
      Begin VB.CommandButton btnOpModeTZ 
         Caption         =   "ModeTZone"
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
         Left            =   2040
         TabIndex        =   31
         Top             =   1560
         Width           =   1605
      End
      Begin VB.CommandButton btnHoliday 
         Caption         =   "Holiday"
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
         Left            =   360
         TabIndex        =   30
         Top             =   1560
         Width           =   1605
      End
      Begin VB.CommandButton btnTZone 
         Caption         =   "AccessTZone"
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
         Left            =   3720
         TabIndex        =   29
         Top             =   1560
         Width           =   1605
      End
      Begin VB.CommandButton cmdLockCtl 
         Caption         =   "Lock Control"
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
         Left            =   2040
         TabIndex        =   26
         Top             =   960
         Width           =   1605
      End
      Begin VB.CommandButton cmdSystemInfo 
         Caption         =   "System Info"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   468
         Left            =   360
         TabIndex        =   25
         Top             =   960
         Width           =   1605
      End
      Begin VB.CommandButton cmdBellInfo 
         Caption         =   "Bell Time"
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
         Left            =   5400
         TabIndex        =   24
         Top             =   960
         Width           =   1605
      End
      Begin VB.CommandButton cmdLogData 
         Caption         =   "Log Data Management"
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
         Left            =   3960
         TabIndex        =   4
         Top             =   360
         Width           =   3045
      End
      Begin VB.CommandButton cmdEnrollData 
         Caption         =   "Enroll Data Management"
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
         Left            =   360
         TabIndex        =   3
         Top             =   324
         Width           =   3045
      End
      Begin VB.CommandButton cmdProuctCode 
         Caption         =   "Get SN"
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
         Left            =   3720
         TabIndex        =   2
         Top             =   960
         Width           =   1605
      End
      Begin VB.CommandButton cmdExit 
         Cancel          =   -1  'True
         Caption         =   "Exit"
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
         Left            =   5400
         TabIndex        =   5
         Top             =   1560
         Width           =   1605
      End
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "SBXPC"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   -1  'True
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   330
      Left            =   3390
      TabIndex        =   16
      Top             =   735
      Width           =   945
   End
   Begin VB.Label lbSubject 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "A30 VB6 Sample"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   20.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   465
      Left            =   2460
      TabIndex        =   0
      Top             =   240
      Width           =   2985
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Public gMachineNumber As Long
Dim mOpenFlag As Boolean

Private Sub btnLockGroup_Click()
    Me.Visible = False
    frmUnlockGroup.Visible = True
End Sub

Private Sub cmbMachineNumber_Click()
    gMachineNumber = cmbMachineNumber.ListIndex + 1
End Sub
Private Sub btnDaylight_Click()
    Me.Visible = False
    frmDayLight.Visible = True
End Sub

Private Sub btnHoliday_Click()
    Me.Visible = False
    frmHoliday.Visible = True
End Sub

Private Sub btnLogTZ_Click()
    Me.Visible = False
    frmTzLog.Visible = True
End Sub

Private Sub btnOpModeTZ_Click()
    Me.Visible = False
    frmTmode.Visible = True
End Sub

Private Sub btnTZone_Click()
    Me.Visible = False
    frmTzone.Visible = True
End Sub

Private Sub cmdBellInfo_Click()
    Me.Visible = False
    frmBellInfo.Visible = True
End Sub

Private Sub cmdClose_Click()
    If mOpenFlag = True Then
        SB100BPC1.Disconnect
        mOpenFlag = False
        cmdOpen.Enabled = True
        cmdClose.Enabled = False
        cmdEnrollData.Enabled = False
        cmdLogData.Enabled = False
        cmdSystemInfo.Enabled = False
        cmdProuctCode.Enabled = False
        cmdBellInfo.Enabled = False
        cmdLockCtl.Enabled = False
        btnOpModeTZ.Enabled = False
        btnTZone.Enabled = False
        btnLogTZ.Enabled = False
        btnHoliday.Enabled = False
     End If
End Sub

Private Sub cmdEnrollData_Click()
    Me.Visible = False
    frmEnroll.Visible = True
End Sub

Private Sub cmdEvent_Click()
    Me.Visible = False
    frmEvent.Visible = True
End Sub

Private Sub cmdExit_Click()
    Unload Me
End Sub

Private Sub cmdLockCtl_Click()
    Me.Visible = False
    frmLockCtl.Visible = True
End Sub

Private Sub cmdLogData_Click()
    Me.Visible = False
    frmLog.Visible = True
End Sub

Public Sub cmdOpen_Click()
    Dim lpszIPAddress As String
    Dim vRet As Boolean
    
    If optNetworkDevice = True Then
        lpszIPAddress = txtIPAddress
        If SB100BPC1.ConnectTcpip(gMachineNumber, lpszIPAddress, CLng(txtPortNo.Text), CLng(txtPassword.Text)) = True Then
            mOpenFlag = True
            cmdOpen.Enabled = False
            cmdClose.Enabled = True
            cmdEnrollData.Enabled = True
            cmdLogData.Enabled = True
            cmdSystemInfo.Enabled = True
            cmdProuctCode.Enabled = True
            cmdBellInfo.Enabled = True
            cmdLockCtl.Enabled = True
            btnOpModeTZ.Enabled = True
            btnTZone.Enabled = True
            btnLogTZ.Enabled = True
            btnHoliday.Enabled = True
        End If
    End If
    If optSerialDevice = True Then
        If SB100BPC1.ConnectSerial(gMachineNumber, cmbComPort.ListIndex, CLng(cmbBaudrate.Text)) = True Then
            mOpenFlag = True
            cmdOpen.Enabled = False
            cmdClose.Enabled = True
            cmdEnrollData.Enabled = True
            cmdLogData.Enabled = True
            cmdSystemInfo.Enabled = True
            cmdProuctCode.Enabled = True
            cmdBellInfo.Enabled = True
            cmdLockCtl.Enabled = True
            btnOpModeTZ.Enabled = True
            btnTZone.Enabled = True
            btnLogTZ.Enabled = True
            btnHoliday.Enabled = True
        End If
    End If
End Sub

Private Sub cmdOpenDlg_Click()
    Dim vstrFileName As String
    
End Sub

Private Sub cmdProuctCode_Click()
    Me.Visible = False
    frmSerialNo.Visible = True
End Sub

Private Sub cmdSystemInfo_Click()
    Me.Visible = False
    frmSystemInfo.Visible = True
End Sub

Private Sub Form_Load()
    Dim lpszIPAddress As String

    'Set Initial Value
    optSerialDevice.Value = True
    lblComPort.Enabled = True
    cmbComPort.Enabled = True
    lblBaudrate.Enabled = True
        cmbBaudrate.Enabled = True
    
    optNetworkDevice.Value = False
    lblIPAddress.Enabled = False
    txtIPAddress.Enabled = False
    lblPortNo.Enabled = False
    'txtPortNo.Enabled = False
    lblPassword.Enabled = False
    txtPassword.Enabled = False
    
    cmdOpen.Enabled = True
    cmdClose.Enabled = False
    cmdEnrollData.Enabled = False
    cmdLogData.Enabled = False
    cmdSystemInfo.Enabled = False
    cmdProuctCode.Enabled = False
    cmdBellInfo.Enabled = False
    cmdLockCtl.Enabled = False
    btnOpModeTZ.Enabled = False
    btnTZone.Enabled = False
    btnLogTZ.Enabled = False
    btnHoliday.Enabled = False
    
    mOpenFlag = False
    cmbMachineNumber = 1
    'cmbComPort = 1
    cmbComPort.ListIndex = 0
        cmbBaudrate.Text = "115200"
End Sub

Private Sub Form_Unload(Cancel As Integer)
    If mOpenFlag = True Then
        SB100BPC1.Disconnect
        mOpenFlag = False
     End If
End Sub

Private Sub optNetworkDevice_Click()
    Dim lpszIPAddress As String
    
    If optNetworkDevice = True Then
        lblComPort.Enabled = False
        cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
        lblIPAddress.Enabled = True
        txtIPAddress.Enabled = True
        lblPortNo.Enabled = True
        'txtPortNo.Enabled = True
        lblPassword.Enabled = True
        txtPassword.Enabled = True
        lpszIPAddress = txtIPAddress
    Else
        lblComPort.Enabled = True
        cmbComPort.Enabled = True
                lblBaudrate.Enabled = True
                cmbBaudrate.Enabled = True
        lblIPAddress.Enabled = False
        txtIPAddress.Enabled = False
        lblPortNo.Enabled = False
        'txtPortNo.Enabled = False
        lblPassword.Enabled = False
        txtPassword.Enabled = False
    End If
End Sub

Private Sub optSerialDevice_Click()
    Dim lpszIPAddress As String
    
    If optSerialDevice = True Then
        lblComPort.Enabled = True
        cmbComPort.Enabled = True
                lblBaudrate.Enabled = True
                cmbBaudrate.Enabled = True
        lblIPAddress.Enabled = False
        txtIPAddress.Enabled = False
        lblPortNo.Enabled = False
        'txtPortNo.Enabled = False
        lblPassword.Enabled = False
        txtPassword.Enabled = False
    Else
        lblComPort.Enabled = False
        cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
        lblIPAddress.Enabled = True
        txtIPAddress.Enabled = True
        lblPortNo.Enabled = True
        'txtPortNo.Enabled = True
        lblPassword.Enabled = True
        txtPassword.Enabled = True
        lpszIPAddress = txtIPAddress
    End If
End Sub

Private Sub SB100BPC1_OnReceiveEvent(ByVal M_No As Long, ByVal evType As Long, ByVal evData As Long)
    Dim sz As String
    Dim szType As String
    
    szType = "UNKNOWN"
    Select Case evType
        Case 0
            sz = "M_No = " & M_No & ", Type = SLOG, MgrID = " & evData
        Case 1
            sz = "M_No = " & M_No & ", Type = GLOG, UserID = " & evData
        Case 2
            sz = "M_No = " & M_No & ", Type = PRESS_FP"
        Case 3
            sz = "M_No = " & M_No & ", Type = VERIFY_OK, ID = " & evData
        Case 4
            sz = "M_No = " & M_No & ", Type = VERIFY_FAIL, Mode = " & evData
        Case 5
            sz = "M_No = " & M_No & ", Type = AlarmOn, Type = " & evData Mod 65536 & ", ID = " & Int(evData / 65536)
        Case 6
            sz = "M_No = " & M_No & ", Type = AlarmOff, OffMode = " & evData Mod 65536 & ", ID = " & Int(evData / 65536)
        Case 7
            sz = "M_No = " & M_No & ", Type = SLOG, MgrID = " & evData
        Case 8
            sz = "M_No = " & M_No & ", Type = GLOG, UserID = " & evData
        Case 9
            sz = "M_No = " & M_No & ", Type = Power " & evData
        Case 10
            sz = "M_No = " & M_No & ", Type = Lock " & evData
        Case 11
            sz = "M_No = " & M_No & ", Type = Door " & evData
    End Select
    
    If frmEvent.Visible = True Then
        frmEvent.lstEvents.AddItem sz
    End If
    'sz = "M_No  = " & M_No & ", Type = " & szType & ", Data(ID) = " & evData
    'lstEvent.AddItem sz
    'MessageBox Me.hwnd, sz, "sample", MB_OK
    'txtMsg = sz
End Sub

Private Sub SB100BPC1_OnReceiveEventXML(ByVal lpszEventXML As String)
    frmEvent.OnEventXML lpszEventXML
End Sub

Private Sub txtIPAddress_Change()
    Dim lpszIPAddress As String
    
    If txtIPAddress = "" Then Exit Sub
    If txtPortNo = "" Then Exit Sub
    lpszIPAddress = txtIPAddress
End Sub

Private Sub txtPortNo_Change()
    Dim lpszIPAddress As String
    
    If txtIPAddress = "" Then Exit Sub
    If txtPortNo = "" Then Exit Sub
    lpszIPAddress = txtIPAddress
End Sub
