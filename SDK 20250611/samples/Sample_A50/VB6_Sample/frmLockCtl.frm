VERSION 5.00
Begin VB.Form frmMiscSettings 
   Caption         =   "Access Setting"
   ClientHeight    =   7920
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   7020
   LinkTopic       =   "Form1"
   ScaleHeight     =   7920
   ScaleWidth      =   7020
   StartUpPosition =   3  'Windows Default
   Begin VB.ComboBox cmbUseM1 
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
      ItemData        =   "frmLockCtl.frx":0000
      Left            =   3240
      List            =   "frmLockCtl.frx":000A
      TabIndex        =   27
      Text            =   "No"
      Top             =   7320
      Width           =   1455
   End
   Begin VB.CommandButton cmdWrite 
      Caption         =   "Write"
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
      Left            =   4920
      TabIndex        =   26
      Top             =   5520
      Width           =   1815
   End
   Begin VB.Frame Frame1 
      Caption         =   "Door Control"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2295
      Left            =   240
      TabIndex        =   18
      Top             =   1320
      Width           =   6135
      Begin VB.CommandButton cmdDoorOpen 
         Caption         =   "Door Open"
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
         Left            =   240
         TabIndex        =   25
         Top             =   360
         Width           =   1815
      End
      Begin VB.CommandButton cmdGetDoorStatus 
         Caption         =   "Get DoorStatus"
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
         Left            =   4080
         TabIndex        =   24
         Top             =   360
         Width           =   1815
      End
      Begin VB.CommandButton cmdUncondOpen 
         Caption         =   "Uncond Open"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Left            =   240
         TabIndex        =   23
         Top             =   960
         Width           =   1815
      End
      Begin VB.CommandButton cmdUncondClose 
         Caption         =   "Uncond Close"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Left            =   2160
         TabIndex        =   22
         Top             =   960
         Width           =   1815
      End
      Begin VB.CommandButton cmdAutoRecover 
         Caption         =   "Auto Recover"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   504
         Left            =   2160
         TabIndex        =   21
         Top             =   360
         Width           =   1815
      End
      Begin VB.CommandButton cmdRestart 
         Caption         =   "Reboot"
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
         Left            =   2160
         TabIndex        =   20
         Top             =   1560
         Width           =   1815
      End
      Begin VB.CommandButton cmdWarnCancel 
         Caption         =   "Warn Cancel"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Left            =   240
         TabIndex        =   19
         Top             =   1560
         Width           =   1815
      End
   End
   Begin VB.CommandButton cmdRead 
      Caption         =   "Read"
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
      Left            =   4920
      TabIndex        =   17
      Top             =   4920
      Width           =   1815
   End
   Begin VB.TextBox txtDualFpTimeout 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3240
      TabIndex        =   16
      Text            =   "0"
      Top             =   6840
      Width           =   1455
   End
   Begin VB.ComboBox cmbDualFpMode 
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
      ItemData        =   "frmLockCtl.frx":0017
      Left            =   3240
      List            =   "frmLockCtl.frx":0024
      TabIndex        =   15
      Text            =   "None"
      Top             =   6360
      Width           =   1455
   End
   Begin VB.TextBox txtDuressDelay 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3240
      TabIndex        =   14
      Text            =   "0"
      Top             =   5880
      Width           =   1455
   End
   Begin VB.ComboBox cmbOutMode 
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
      ItemData        =   "frmLockCtl.frx":0040
      Left            =   3240
      List            =   "frmLockCtl.frx":004D
      TabIndex        =   13
      Text            =   "Out+In"
      Top             =   5400
      Width           =   1455
   End
   Begin VB.TextBox txtIllegVerifTimes 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3240
      TabIndex        =   12
      Text            =   "0"
      Top             =   4920
      Width           =   1455
   End
   Begin VB.TextBox txtSynchOpenCount 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3240
      TabIndex        =   11
      Text            =   "0"
      Top             =   4440
      Width           =   1455
   End
   Begin VB.ComboBox cmbAccessMode 
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
      ItemData        =   "frmLockCtl.frx":0062
      Left            =   3240
      List            =   "frmLockCtl.frx":006F
      TabIndex        =   10
      Text            =   "Standalone"
      Top             =   3960
      Width           =   1455
   End
   Begin VB.TextBox txtDoorNumber 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3570
      TabIndex        =   1
      Text            =   "0"
      Top             =   840
      Width           =   1455
   End
   Begin VB.Label Label9 
      Alignment       =   1  'Right Justify
      Caption         =   "Use M1 Card:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1440
      TabIndex        =   28
      Top             =   7320
      Width           =   1575
   End
   Begin VB.Label Label8 
      Alignment       =   1  'Right Justify
      Caption         =   "Dual Fp Timeout:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1200
      TabIndex        =   9
      Top             =   6840
      Width           =   1815
   End
   Begin VB.Label Label7 
      Alignment       =   1  'Right Justify
      Caption         =   "Dual Fp Mode:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1440
      TabIndex        =   8
      Top             =   6360
      Width           =   1575
   End
   Begin VB.Label Label6 
      Alignment       =   1  'Right Justify
      Caption         =   "Duress Delay:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1440
      TabIndex        =   7
      Top             =   5880
      Width           =   1575
   End
   Begin VB.Label Label5 
      Alignment       =   1  'Right Justify
      Caption         =   "Alarm Output Mode:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   840
      TabIndex        =   6
      Top             =   5400
      Width           =   2175
   End
   Begin VB.Label Label4 
      Alignment       =   1  'Right Justify
      Caption         =   "Illegal Verification Times:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   240
      TabIndex        =   5
      Top             =   4920
      Width           =   2775
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      Caption         =   "Synch Open Count:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   960
      TabIndex        =   4
      Top             =   4440
      Width           =   2055
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "Access Mode:"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1560
      TabIndex        =   3
      Top             =   3960
      Width           =   1455
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Caption         =   "Door Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1800
      TabIndex        =   2
      Top             =   870
      Width           =   1695
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
      Left            =   288
      TabIndex        =   0
      Top             =   252
      Width           =   6012
   End
End
Attribute VB_Name = "frmMiscSettings"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mMachineNumber As Long

Private Sub cmdGetDoorStatus_Click()
    Dim vStatus As Long
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If

    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        vStatus = frmMain.SB100BPC1.XML_ParseLong(strXML, "DoorStatus")
        If vStatus = 1 Then
            lblMessage.Caption = "Uncond Door Open State!"
        ElseIf vStatus = 2 Then
            lblMessage.Caption = "Uncond Door Close State!"
        ElseIf vStatus = 3 Then
            lblMessage.Caption = "Door Open State!"
        ElseIf vStatus = 4 Then
            lblMessage.Caption = "Auto Recover State!"
        ElseIf vStatus = 5 Then
            lblMessage.Caption = "Door Close State!"
        ElseIf vStatus = 6 Then
            lblMessage.Caption = "Watching for Close!"
        ElseIf vStatus = 7 Then
            lblMessage.Caption = "Illegal open!"
        Else
            lblMessage.Caption = "User State !"
        End If
    Else
        frmMain.SB100BPC1.GetLastError (vErrorCode)
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdAutoRecover_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = CInt(txtDoorNumber)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 4

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "Auto Recover Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdDoorOpen_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim lDoorNumber As Long
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 3

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        lblMessage.Caption = "Door Open Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdRead_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim lDoorNumber As Long

    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "ReadAccessSetting"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If vRet = True Then
        cmbAccessMode.ListIndex = frmMain.SB100BPC1.XML_ParseLong(strXML, "AccessMode")
        txtIllegVerifTimes = frmMain.SB100BPC1.XML_ParseLong(strXML, "IllegVerifTimes")
        cmbOutMode.ListIndex = frmMain.SB100BPC1.XML_ParseLong(strXML, "AlarmOutMode")
        txtDuressDelay = frmMain.SB100BPC1.XML_ParseLong(strXML, "DuressDelay")
        txtSynchOpenCount = frmMain.SB100BPC1.XML_ParseLong(strXML, "SynchOpenCount")
        cmbDualFpMode.ListIndex = frmMain.SB100BPC1.XML_ParseLong(strXML, "DualFpMode")
        txtDualFpTimeout = frmMain.SB100BPC1.XML_ParseLong(strXML, "DualFpTimeout")
        cmbUseM1.ListIndex = frmMain.SB100BPC1.XML_ParseLong(strXML, "UseM1Card")
        lblMessage.Caption = "ReadAccessSetting Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    DoEvents
End Sub

Private Sub cmdRestart_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim lDoorNumber As Long

    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 5

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If vRet = True Then
        lblMessage.Caption = "Reboot Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    DoEvents
End Sub

Private Sub cmdUncondClose_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 2

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "Uncond Door Close Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdUncondOpen_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 1
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "Uncond Door Open Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdWarnCancel_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = 0
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 6
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        lblMessage.Caption = "Warning cancel Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdWrite_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim lDoorNumber As Long

    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "WriteAccessSetting"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "AccessMode", cmbAccessMode.ListIndex
    frmMain.SB100BPC1.XML_AddInt strXML, "IllegVerifTimes", CInt(txtIllegVerifTimes)
    frmMain.SB100BPC1.XML_AddInt strXML, "AlarmOutMode", cmbOutMode.ListIndex
    frmMain.SB100BPC1.XML_AddInt strXML, "DuressDelay", CInt(txtDuressDelay)
    frmMain.SB100BPC1.XML_AddInt strXML, "SynchOpenCount", CInt(txtSynchOpenCount)
    frmMain.SB100BPC1.XML_AddInt strXML, "DualFpMode", cmbDualFpMode.ListIndex
    frmMain.SB100BPC1.XML_AddInt strXML, "DualFpTimeout", CInt(txtDualFpTimeout)
    frmMain.SB100BPC1.XML_AddInt strXML, "UseM1Card", cmbUseM1.ListIndex

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If vRet = True Then
        lblMessage.Caption = "WriteAccessSetting Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    DoEvents
End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
    cmbAccessMode.ListIndex = 0
    cmbOutMode.ListIndex = 0
    cmbDualFpMode.ListIndex = 0
    cmbUseM1.ListIndex = 0
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub

