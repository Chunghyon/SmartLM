VERSION 5.00
Begin VB.Form frmUnlockGroup 
   Caption         =   "Unlock Group"
   ClientHeight    =   3840
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6060
   LinkTopic       =   "Form1"
   ScaleHeight     =   3840
   ScaleWidth      =   6060
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkUse 
      Caption         =   "Check1"
      Height          =   255
      Index           =   4
      Left            =   3000
      TabIndex        =   15
      Top             =   1080
      Width           =   255
   End
   Begin VB.CheckBox chkUse 
      Caption         =   "Check1"
      Height          =   255
      Index           =   3
      Left            =   2280
      TabIndex        =   14
      Top             =   1080
      Width           =   255
   End
   Begin VB.CheckBox chkUse 
      Caption         =   "Check1"
      Height          =   255
      Index           =   2
      Left            =   1560
      TabIndex        =   13
      Top             =   1080
      Width           =   255
   End
   Begin VB.CheckBox chkUse 
      Caption         =   "Check1"
      Height          =   255
      Index           =   1
      Left            =   840
      TabIndex        =   12
      Top             =   1080
      Width           =   255
   End
   Begin VB.CheckBox chkUse 
      Caption         =   "Check1"
      Height          =   255
      Index           =   0
      Left            =   120
      TabIndex        =   11
      Top             =   1080
      Width           =   255
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
      Left            =   4560
      TabIndex        =   5
      Top             =   3120
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
      Left            =   4560
      TabIndex        =   4
      Top             =   1440
      Width           =   1410
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
      Left            =   4560
      TabIndex        =   3
      Top             =   2280
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
      Left            =   4560
      TabIndex        =   2
      Top             =   720
      Width           =   1410
   End
   Begin VB.ListBox lstUnlockGroup 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2310
      ItemData        =   "frmUnlockGroup.frx":0000
      Left            =   120
      List            =   "frmUnlockGroup.frx":0002
      TabIndex        =   1
      Top             =   1440
      Width           =   4335
   End
   Begin VB.Label Label4 
      Caption         =   "G5"
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
      TabIndex        =   10
      Top             =   720
      Width           =   375
   End
   Begin VB.Label Label3 
      Caption         =   "G4"
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
      Left            =   2280
      TabIndex        =   9
      Top             =   720
      Width           =   375
   End
   Begin VB.Label Label2 
      Caption         =   "G3"
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
      Left            =   1560
      TabIndex        =   8
      Top             =   720
      Width           =   375
   End
   Begin VB.Label Label1 
      Caption         =   "G2"
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
      Left            =   840
      TabIndex        =   7
      Top             =   720
      Width           =   375
   End
   Begin VB.Label lblStart 
      Caption         =   "G1"
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
      TabIndex        =   6
      Top             =   720
      Width           =   375
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
      Width           =   5865
   End
End
Attribute VB_Name = "frmUnlockGroup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Const DB_UNLOCKGRPUP_MAX = 10 * 5
Dim DbUnlockGroupArray(DB_UNLOCKGRPUP_MAX * 5 - 1) As Long

Private Sub DbUnlockGroupInit()
    Dim i As Long
    
    For i = 0 To DB_UNLOCKGRPUP_MAX - 1
        DbUnlockGroupArray(i * 5 + 0) = 0
        DbUnlockGroupArray(i * 5 + 1) = 0
        DbUnlockGroupArray(i * 5 + 2) = 0
        DbUnlockGroupArray(i * 5 + 3) = 0
        DbUnlockGroupArray(i * 5 + 4) = 0
    Next
    
End Sub
Private Sub DbUnlockGroupDraw()
    Dim i As Long
    
    lstUnlockGroup.Clear
    For i = 0 To DB_UNLOCKGRPUP_MAX / 5 - 1
        lstUnlockGroup.AddItem _
            "[UnlockGroup.] " & Format(i + 1, "##:") & " - " & _
            CStr(DbUnlockGroupArray(i * 5 + 0)) & ":" & _
            CStr(DbUnlockGroupArray(i * 5 + 1)) & ":" & _
            CStr(DbUnlockGroupArray(i * 5 + 2)) & ":" & _
            CStr(DbUnlockGroupArray(i * 5 + 3)) & ":" & _
            CStr(DbUnlockGroupArray(i * 5 + 4))
    Next
End Sub
Private Sub cmdExit_Click()
    Unload Me
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
    
    bRet = frmMain.SB100BPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 7, DbUnlockGroupArray(0))
    
    If bRet = True Then
        DbUnlockGroupDraw
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True
End Sub

Private Sub cmdUpdate_Click()
    Dim i As Long, j As Long
    
    i = lstUnlockGroup.ListIndex
    If i < 0 Or i > DB_UNLOCKGRPUP_MAX - 1 Then Exit Sub
    
    For j = 0 To 4
        If chkUse(j).Value = 1 Then
            DbUnlockGroupArray(i * 5 + j) = 1
        Else
            DbUnlockGroupArray(i * 5 + j) = 0
        End If
    Next
    
    DbUnlockGroupDraw
    
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
    
    bRet = frmMain.SB100BPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 7, DbUnlockGroupArray(0))
    
    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True

End Sub

Private Sub Form_Load()
    DbUnlockGroupInit
    DbUnlockGroupDraw
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub

Private Sub lstUnlockGroup_Click()
    Dim i As Long, j As Long
    
    i = lstUnlockGroup.ListIndex
    If i < 0 Or i > DB_UNLOCKGRPUP_MAX - 1 Then Exit Sub
    
    For j = 0 To 4
        If DbUnlockGroupArray(i * 5 + j) = 1 Then
            chkUse(j).Value = 1
        Else
            chkUse(j).Value = 0
        End If
    Next
End Sub
