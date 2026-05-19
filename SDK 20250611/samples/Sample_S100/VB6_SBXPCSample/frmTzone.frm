VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmTzone 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Tzone"
   ClientHeight    =   7080
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   10440
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7080
   ScaleWidth      =   10440
   StartUpPosition =   2  'CenterScreen
   Begin VB.ListBox lstTzone 
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
      TabIndex        =   9
      Top             =   1680
      Width           =   8535
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
      TabIndex        =   5
      Top             =   1680
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
      TabIndex        =   6
      Top             =   4560
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
      Left            =   8880
      TabIndex        =   7
      Top             =   5400
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
      Left            =   8880
      TabIndex        =   8
      Top             =   6240
      Width           =   1410
   End
   Begin MSComCtl2.DTPicker dtpStart 
      Height          =   375
      Left            =   840
      TabIndex        =   2
      Top             =   720
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
      Format          =   21889026
      CurrentDate     =   39090
   End
   Begin MSComCtl2.DTPicker dtpEnd 
      Height          =   375
      Left            =   840
      TabIndex        =   4
      Top             =   1200
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
      Format          =   21889026
      CurrentDate     =   39090
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
      Top             =   1200
      Width           =   735
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
      Top             =   720
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
Attribute VB_Name = "frmTzone"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const DB_TZONE_MAX = 50 * 8
Dim DbTzoneArray(DB_TZONE_MAX * 4 - 1) As Long
Private Sub DbTzoneInit()
    Dim i As Long
    
    For i = 0 To DB_TZONE_MAX - 1
        DbTzoneArray(i * 4 + 0) = 0
        DbTzoneArray(i * 4 + 1) = 0
        DbTzoneArray(i * 4 + 2) = 23
        DbTzoneArray(i * 4 + 3) = 59
    Next
    
End Sub
Private Sub DbTzoneDraw()
    Dim i As Long
    
    lstTzone.Clear
    For i = 0 To DB_TZONE_MAX - 1
        lstTzone.AddItem _
            "[Tz.] " & Format(i \ 8 + 1, "00#") & " - " & CStr(i Mod 8 + 1) & " " & _
            "[S] " & _
            Format(DbTzoneArray(i * 4 + 0), "0#") & ":" & _
            Format(DbTzoneArray(i * 4 + 1), "0#") & " " & _
            "[E] " & _
            Format(DbTzoneArray(i * 4 + 2), "0#") & ":" & _
            Format(DbTzoneArray(i * 4 + 3), "0#")
    Next
End Sub
Private Sub Form_Load()
    DbTzoneInit
    DbTzoneDraw
End Sub
Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub
Private Sub cmdExit_Click()
    Unload Me
End Sub
Private Sub lstTzone_Click()
    Dim i As Long
    
    i = lstTzone.ListIndex
    If i < 0 Or i > DB_TZONE_MAX - 1 Then Exit Sub
    
    dtpStart.Hour = DbTzoneArray(i * 4 + 0)
    dtpStart.Minute = DbTzoneArray(i * 4 + 1)
    dtpEnd.Hour = DbTzoneArray(i * 4 + 2)
    dtpEnd.Minute = DbTzoneArray(i * 4 + 3)
End Sub
Private Sub cmdUpdate_Click()
    Dim i As Long
    
    i = lstTzone.ListIndex
    If i < 0 Or i > DB_TZONE_MAX - 1 Then Exit Sub
    
    DbTzoneArray(i * 4 + 0) = dtpStart.Hour
    DbTzoneArray(i * 4 + 1) = dtpStart.Minute
    DbTzoneArray(i * 4 + 2) = dtpEnd.Hour
    DbTzoneArray(i * 4 + 3) = dtpEnd.Minute
    
    DbTzoneDraw
    
    lstTzone.ListIndex = i
End Sub
Private Sub cmdRead_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.Fk528KM1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    bRet = frmMain.Fk528KM1.GetDeviceLongInfo(frmMain.gMachineNumber, 3, DbTzoneArray(0))
        
    If bRet = True Then
        DbTzoneDraw
        lblMessage.Caption = "Success!"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice frmMain.gMachineNumber, True
End Sub
Private Sub cmdWrite_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.Fk528KM1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    bRet = frmMain.Fk528KM1.SetDeviceLongInfo(frmMain.gMachineNumber, 3, DbTzoneArray(0))
        
    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice frmMain.gMachineNumber, True
End Sub


