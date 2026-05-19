VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmTmode 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Tmode"
   ClientHeight    =   6840
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   10440
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6840
   ScaleWidth      =   10440
   StartUpPosition =   2  'CenterScreen
   Begin VB.ComboBox cmbDoorMode 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   405
      ItemData        =   "frmTmode.frx":0000
      Left            =   3000
      List            =   "frmTmode.frx":0013
      Style           =   2  'Dropdown List
      TabIndex        =   6
      Top             =   1080
      Width           =   3255
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
      Left            =   8880
      TabIndex        =   10
      Top             =   5280
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
      Left            =   8880
      TabIndex        =   9
      Top             =   4440
      Width           =   1410
   End
   Begin VB.CommandButton cmdUpdate 
      Caption         =   "Update"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   615
      Left            =   8880
      TabIndex        =   8
      Top             =   1560
      Width           =   1410
   End
   Begin VB.ListBox lstTmode 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5010
      Left            =   120
      TabIndex        =   7
      Top             =   1560
      Width           =   8535
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
      Left            =   8880
      TabIndex        =   11
      Top             =   6120
      Width           =   1410
   End
   Begin MSComCtl2.DTPicker dtpStart 
      Height          =   375
      Left            =   840
      TabIndex        =   2
      Top             =   600
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
      Format          =   21102594
      CurrentDate     =   39090
   End
   Begin MSComCtl2.DTPicker dtpEnd 
      Height          =   375
      Left            =   840
      TabIndex        =   4
      Top             =   1080
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
      Format          =   21102594
      CurrentDate     =   39090
   End
   Begin VB.Label lblDoorMode 
      Caption         =   "Door Mode :"
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
      Left            =   3000
      TabIndex        =   5
      Top             =   600
      Width           =   1575
   End
   Begin VB.Label lblStart 
      Caption         =   "Start :"
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
      Left            =   120
      TabIndex        =   1
      Top             =   600
      Width           =   735
   End
   Begin VB.Label lblEnd 
      Caption         =   "End :"
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
      Left            =   120
      TabIndex        =   3
      Top             =   1080
      Width           =   735
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
      Width           =   10185
   End
End
Attribute VB_Name = "frmTmode"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const DB_TMODE_MAX = 10
Dim DbTmodeArray(DB_TMODE_MAX * 5 - 1) As Long
Private Sub DbTmodeInit()
    Dim i As Long
    
    For i = 0 To DB_TMODE_MAX - 1
        DbTmodeArray(i * 5 + 1) = 0
        DbTmodeArray(i * 5 + 2) = 0
        DbTmodeArray(i * 5 + 3) = 23
        DbTmodeArray(i * 5 + 4) = 59
        DbTmodeArray(i * 5 + 0) = 0
    Next
    
    cmbDoorMode.ListIndex = 0
End Sub
Private Sub DbTmodeDraw()
    Dim i As Long
    
    lstTmode.Clear
    For i = 0 To DB_TMODE_MAX - 1
        lstTmode.AddItem _
            "[No.] " & Format(i + 1, "0#") & " " & _
            "[S] " & _
            Format(DbTmodeArray(i * 5 + 1), "0#") & ":" & _
            Format(DbTmodeArray(i * 5 + 2), "0#") & " " & _
            "[E] " & _
            Format(DbTmodeArray(i * 5 + 3), "0#") & ":" & _
            Format(DbTmodeArray(i * 5 + 4), "0#") & " " & _
            "[M] [" & _
            cmbDoorMode.List(DbTmodeArray(i * 5 + 0)) & "] "
    Next
End Sub
Private Sub Form_Load()
    DbTmodeInit
    DbTmodeDraw
End Sub
Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub
Private Sub cmdExit_Click()
    Unload Me
End Sub
Private Sub lstTmode_Click()
    Dim i As Long
    
    i = lstTmode.ListIndex
    If i < 0 Or i > DB_TMODE_MAX - 1 Then Exit Sub
    
    dtpStart.Hour = DbTmodeArray(i * 5 + 1)
    dtpStart.Minute = DbTmodeArray(i * 5 + 2)
    dtpEnd.Hour = DbTmodeArray(i * 5 + 3)
    dtpEnd.Minute = DbTmodeArray(i * 5 + 4)
    cmbDoorMode.ListIndex = DbTmodeArray(i * 5 + 0)
End Sub
Private Sub cmdUpdate_Click()
    Dim i As Long
    
    i = lstTmode.ListIndex
    If i < 0 Or i > DB_TMODE_MAX - 1 Then Exit Sub
    
    DbTmodeArray(i * 5 + 1) = dtpStart.Hour
    DbTmodeArray(i * 5 + 2) = dtpStart.Minute
    DbTmodeArray(i * 5 + 3) = dtpEnd.Hour
    DbTmodeArray(i * 5 + 4) = dtpEnd.Minute
    DbTmodeArray(i * 5 + 0) = cmbDoorMode.ListIndex
    
    DbTmodeDraw
    
    lstTmode.ListIndex = i
End Sub
Private Sub cmdRead_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    bRet = frmMain.SB100BPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 4, DbTmodeArray(0))
    
    If bRet = True Then
        DbTmodeDraw
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

    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    bRet = frmMain.SB100BPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 4, DbTmodeArray(0))
    
    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True
End Sub




