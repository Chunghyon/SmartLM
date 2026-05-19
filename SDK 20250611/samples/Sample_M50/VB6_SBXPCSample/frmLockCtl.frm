VERSION 5.00
Begin VB.Form frmLockCtl 
   Caption         =   "Door Open Control"
   ClientHeight    =   2670
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   6570
   LinkTopic       =   "Form1"
   ScaleHeight     =   2670
   ScaleWidth      =   6570
   StartUpPosition =   3  'Windows Default
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
      Left            =   4440
      TabIndex        =   7
      Top             =   2028
      Width           =   1872
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
      Left            =   2340
      TabIndex        =   6
      Top             =   2052
      Width           =   1848
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
      Left            =   2328
      TabIndex        =   5
      Top             =   1464
      Width           =   1860
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
      Left            =   4440
      TabIndex        =   4
      Top             =   1452
      Width           =   1884
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
      Left            =   4452
      TabIndex        =   3
      Top             =   900
      Width           =   1884
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
      Left            =   300
      TabIndex        =   2
      Top             =   924
      Width           =   1815
   End
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
      Left            =   2328
      TabIndex        =   1
      Top             =   900
      Width           =   1872
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
Attribute VB_Name = "frmLockCtl"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mMachineNumber As Long

Private Sub cmdGetDoorStatus_Click()
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
        If vValue(0) = 1 Then
            lblMessage.Caption = "Uncond Door Open State!"
        ElseIf vValue(0) = 2 Then
            lblMessage.Caption = "Uncond Door Close State!"
        ElseIf vValue(0) = 3 Then
            lblMessage.Caption = "Door Open State!"
        ElseIf vValue(0) = 4 Then
            lblMessage.Caption = "Auto Recover State!"
        ElseIf vValue(0) = 5 Then
            lblMessage.Caption = "Door Close State!"
        ElseIf vValue(0) = 6 Then
            lblMessage.Caption = "Watching for Close!"
        ElseIf vValue(0) = 7 Then
            lblMessage.Caption = "Illegal open!"
        Else
            lblMessage.Caption = "User State !"
        End If
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    'txtSetDevInfo.Text = vValue(0)
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents

End Sub

Private Sub cmdAutoRecover_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDoorStatus(mMachineNumber, 4)
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
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDoorStatus(mMachineNumber, 3)
    If vRet = True Then
        lblMessage.Caption = "Door Open Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdRestart_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDoorStatus(mMachineNumber, 5)
    If vRet = True Then
        lblMessage.Caption = "Reboot Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdUncondClose_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDoorStatus(mMachineNumber, 2)
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
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDoorStatus(mMachineNumber, 1)
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
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.SetDoorStatus(mMachineNumber, 6)
    If vRet = True Then
        lblMessage.Caption = "Warning cancel Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
End Sub
Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub

