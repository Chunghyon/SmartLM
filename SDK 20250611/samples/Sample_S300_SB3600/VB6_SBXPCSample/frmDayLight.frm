VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmDayLight 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "DayLight"
   ClientHeight    =   3585
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6990
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3585
   ScaleWidth      =   6990
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtOffset 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2160
      TabIndex        =   11
      Text            =   "0"
      Top             =   2160
      Width           =   1935
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
      Height          =   645
      Left            =   3720
      TabIndex        =   9
      Top             =   2760
      Width           =   1410
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
      Height          =   645
      Left            =   2160
      TabIndex        =   8
      Top             =   2760
      Width           =   1410
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
      Height          =   645
      Left            =   5520
      TabIndex        =   10
      Top             =   2760
      Width           =   1410
   End
   Begin MSComCtl2.DTPicker dtpTime 
      Height          =   375
      Index           =   0
      Left            =   4200
      TabIndex        =   3
      Top             =   960
      Width           =   1935
      _ExtentX        =   3413
      _ExtentY        =   661
      _Version        =   393216
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Format          =   53215234
      CurrentDate     =   39090
   End
   Begin MSComCtl2.DTPicker dtpDate 
      Height          =   375
      Index           =   0
      Left            =   2160
      TabIndex        =   2
      Top             =   960
      Width           =   1935
      _ExtentX        =   3413
      _ExtentY        =   661
      _Version        =   393216
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      CustomFormat    =   "MM-dd"
      Format          =   53215235
      UpDown          =   -1  'True
      CurrentDate     =   39090
   End
   Begin MSComCtl2.DTPicker dtpTime 
      Height          =   375
      Index           =   1
      Left            =   4200
      TabIndex        =   6
      Top             =   1560
      Width           =   1935
      _ExtentX        =   3413
      _ExtentY        =   661
      _Version        =   393216
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Format          =   53215234
      CurrentDate     =   39090
   End
   Begin MSComCtl2.DTPicker dtpDate 
      Height          =   375
      Index           =   1
      Left            =   2160
      TabIndex        =   5
      Top             =   1560
      Width           =   1935
      _ExtentX        =   3413
      _ExtentY        =   661
      _Version        =   393216
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      CustomFormat    =   "MM-dd"
      Format          =   53215235
      UpDown          =   -1  'True
      CurrentDate     =   39090
   End
   Begin VB.Label lbls 
      Caption         =   "End of Time :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   2
      Left            =   600
      TabIndex        =   4
      Top             =   1560
      Width           =   1455
   End
   Begin VB.Label lbls 
      Caption         =   "Offset :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   3
      Left            =   1200
      TabIndex        =   7
      Top             =   2160
      Width           =   855
   End
   Begin VB.Label lbls 
      Caption         =   "Begin of Time :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   0
      Left            =   480
      TabIndex        =   1
      Top             =   960
      Width           =   1575
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
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6825
   End
End
Attribute VB_Name = "frmDayLight"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
   dtpDate(0).Year = Year(Date)
   dtpDate(1).Year = Year(Date)
End Sub
Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub
Private Sub cmdExit_Click()
    Unload Me
End Sub
Private Sub cmdRead_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long
    Dim lngTime(3) As Long

    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    bRet = frmMain.SB100BPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 1, lngTime(0))
    If bRet = True Then
        dtpDate(0).Month = lngTime(0)
        dtpDate(0).Day = lngTime(1)
        dtpTime(0).Hour = lngTime(2)
        dtpTime(0).Minute = lngTime(3)
        bRet = frmMain.SB100BPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 2, lngTime(0))
    End If
    
    If bRet = True Then
        dtpDate(1).Month = lngTime(0)
        dtpDate(1).Day = lngTime(1)
        dtpTime(1).Hour = lngTime(2)
        dtpTime(1).Minute = lngTime(3)
        bRet = frmMain.SB100BPC1.GetDeviceInfo(frmMain.gMachineNumber, 19, lngTime(0))
    End If
    
    If bRet = True Then
        txtOffset = lngTime(0)
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True
End Sub
Private Sub cmdWrite_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long
    Dim lngTime(3) As Long

    If Val(txtOffset) < 0 Or Val(txtOffset) > 3 Then
        MsgBox ("offset must be from 0 to 3 hours !")
        Exit Sub
    End If
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lngTime(0) = dtpDate(0).Month
    lngTime(1) = dtpDate(0).Day
    lngTime(2) = dtpTime(0).Hour
    lngTime(3) = dtpTime(0).Minute
    bRet = frmMain.SB100BPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 1, lngTime(0))
    
    If bRet = True Then
        lngTime(0) = dtpDate(1).Month
        lngTime(1) = dtpDate(1).Day
        lngTime(2) = dtpTime(1).Hour
        lngTime(3) = dtpTime(1).Minute
        bRet = frmMain.SB100BPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 2, lngTime(0))
    End If
    
    If bRet = True Then
        lngTime(0) = txtOffset
        bRet = frmMain.SB100BPC1.SetDeviceInfo(frmMain.gMachineNumber, 19, lngTime(0))
    End If
    
    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True
End Sub



