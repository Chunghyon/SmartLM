VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmTzone 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Tzone"
   ClientHeight    =   6540
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   10440
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6540
   ScaleWidth      =   10440
   StartUpPosition =   2  'CenterScreen
   Begin VB.ComboBox cboVerifyMode 
      Height          =   315
      ItemData        =   "frmTzone.frx":0000
      Left            =   6240
      List            =   "frmTzone.frx":0013
      Style           =   2  'Dropdown List
      TabIndex        =   11
      Top             =   810
      Width           =   1815
   End
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
      Height          =   4560
      Left            =   120
      TabIndex        =   9
      Top             =   1230
      Width           =   10155
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
      Height          =   345
      Left            =   9330
      TabIndex        =   5
      Top             =   810
      Width           =   960
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
      Height          =   435
      Left            =   4620
      TabIndex        =   6
      Top             =   5940
      Width           =   1230
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
      Height          =   435
      Left            =   6150
      TabIndex        =   7
      Top             =   5940
      Width           =   1230
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
      Height          =   435
      Left            =   9030
      TabIndex        =   8
      Top             =   5940
      Width           =   1230
   End
   Begin MSComCtl2.DTPicker dtpStart 
      Height          =   375
      Left            =   930
      TabIndex        =   2
      Top             =   780
      Width           =   1335
      _ExtentX        =   2355
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
      Format          =   93978626
      CurrentDate     =   39090
   End
   Begin MSComCtl2.DTPicker dtpEnd 
      Height          =   375
      Left            =   3000
      TabIndex        =   4
      Top             =   780
      Width           =   1305
      _ExtentX        =   2302
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
      Format          =   93978626
      CurrentDate     =   39090
   End
   Begin VB.Label Label1 
      Caption         =   "Verify Mode :"
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
      Left            =   4740
      TabIndex        =   10
      Top             =   840
      Width           =   1425
   End
   Begin VB.Label lblEnd 
      Alignment       =   1  'Right Justify
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
      Left            =   2400
      TabIndex        =   3
      Top             =   840
      Width           =   555
   End
   Begin VB.Label lblStart 
      Alignment       =   1  'Right Justify
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
      Left            =   150
      TabIndex        =   1
      Top             =   840
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
Const ELEM_CNT = 5
Const VERIFY_TZONE_CNT = 50
Dim DbTzoneArray(DB_TZONE_MAX * 5 - 1) As Byte
Private Sub DbTzoneInit()
    Dim i As Long
    
    For i = 0 To DB_TZONE_MAX - 1
        DbTzoneArray(i * ELEM_CNT + 0) = 0
        DbTzoneArray(i * ELEM_CNT + 1) = 0
        DbTzoneArray(i * ELEM_CNT + 2) = 23
        DbTzoneArray(i * ELEM_CNT + 3) = 59
        DbTzoneArray(i * ELEM_CNT + 4) = 0
    Next
    
End Sub
Private Sub DbTzoneDraw()
    Dim i As Long
    Dim VERIFY_MODE_CONST
    
    VERIFY_MODE_CONST = Array("FP|CD|PWD", _
                                "CD&FP", _
                                "FP&PWD", _
                                "RF&PWD", _
                              "FP&CD&PWD")
    lstTzone.Clear
    For i = 0 To DB_TZONE_MAX - 1
        lstTzone.AddItem _
            "[Tz.] " & Format(i \ 8 + 1, "00#") & " - " & CStr(i Mod 8 + 1) & " " & _
            "[S] " & _
            Format(DbTzoneArray(i * ELEM_CNT + 0), "0#") & ":" & _
            Format(DbTzoneArray(i * ELEM_CNT + 1), "0#") & " " & _
            "[E] " & _
            Format(DbTzoneArray(i * ELEM_CNT + 2), "0#") & ":" & _
            Format(DbTzoneArray(i * ELEM_CNT + 3), "0#") & _
            "  [VM] " & _
            VERIFY_MODE_CONST(DbTzoneArray(i * ELEM_CNT + 4))
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
    
    dtpStart.Hour = DbTzoneArray(i * ELEM_CNT + 0)
    dtpStart.Minute = DbTzoneArray(i * ELEM_CNT + 1)
    dtpEnd.Hour = DbTzoneArray(i * ELEM_CNT + 2)
    dtpEnd.Minute = DbTzoneArray(i * ELEM_CNT + 3)
    cboVerifyMode.ListIndex = DbTzoneArray(i * ELEM_CNT + 4)
    
End Sub
Private Sub cmdUpdate_Click()
    Dim i As Long
    
    i = lstTzone.ListIndex
    If i < 0 Or i > DB_TZONE_MAX - 1 Then Exit Sub
    
    DbTzoneArray(i * ELEM_CNT + 0) = dtpStart.Hour
    DbTzoneArray(i * ELEM_CNT + 1) = dtpStart.Minute
    DbTzoneArray(i * ELEM_CNT + 2) = dtpEnd.Hour
    DbTzoneArray(i * ELEM_CNT + 3) = dtpEnd.Minute
    
    If i < VERIFY_TZONE_CNT * 8 Then
        DbTzoneArray(i * ELEM_CNT + 4) = cboVerifyMode.ListIndex
    Else
        DbTzoneArray(i * ELEM_CNT + 4) = 0  'can not set
    End If
    DbTzoneDraw
    
    lstTzone.ListIndex = i
End Sub
Private Sub cmdRead_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long
    Dim strXML As String

    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetTimezone"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    frmMain.SB100BPC1.XML_ParseBinaryByte strXML, "TimezoneBinary", DbTzoneArray(0), DB_TZONE_MAX * 5
    If bRet = True Then
        DbTzoneDraw
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
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetTimezone"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddBinaryByte strXML, "TimezoneBinary", DbTzoneArray(0), DB_TZONE_MAX * 5
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
'    bRet = frmMain.SB100BPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 3, DbTzoneArray(0))
        
    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True
End Sub


