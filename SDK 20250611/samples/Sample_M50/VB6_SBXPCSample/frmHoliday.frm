VERSION 5.00
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmHoliday 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Holiday"
   ClientHeight    =   6315
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7515
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6315
   ScaleWidth      =   7515
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtDays 
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
      Left            =   2880
      TabIndex        =   8
      Text            =   "0"
      Top             =   720
      Width           =   1335
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
      Left            =   6000
      TabIndex        =   5
      Top             =   4680
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
      Left            =   6000
      TabIndex        =   4
      Top             =   3840
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
      Height          =   375
      Left            =   4680
      TabIndex        =   2
      Top             =   720
      Width           =   2655
   End
   Begin VB.ListBox lstHoliday 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   4785
      Left            =   120
      TabIndex        =   3
      Top             =   1200
      Width           =   5775
   End
   Begin MSComCtl2.DTPicker dtpHoliday 
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   720
      Width           =   1695
      _ExtentX        =   2990
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
      Format          =   21168129
      CurrentDate     =   39090
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
      Left            =   6000
      TabIndex        =   6
      Top             =   5520
      Width           =   1410
   End
   Begin VB.Label LabelDays 
      Caption         =   "Period"
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
      Left            =   2040
      TabIndex        =   7
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
      Width           =   7305
   End
End
Attribute VB_Name = "frmHoliday"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const DB_HOLIDAY_MAX = 256
Dim DbHolidayArray(DB_HOLIDAY_MAX * 3 - 1) As Long
Private Sub DbHolidayInit()
    Dim i As Long
    
    For i = 0 To DB_HOLIDAY_MAX - 1
        DbHolidayArray(i * 3 + 0) = 1
        DbHolidayArray(i * 3 + 1) = 1
        DbHolidayArray(i * 3 + 2) = 0
    Next
End Sub
Private Sub DbHolidayDraw()
    Dim i As Long
    
    lstHoliday.Clear
    For i = 0 To DB_HOLIDAY_MAX - 1
        lstHoliday.AddItem _
            "[No.] " & Format(i + 1, "00#") & " " & _
            "[Day/Month] " & _
            Format(DbHolidayArray(i * 3 + 1), "0#") & "/" & _
            Format(DbHolidayArray(i * 3 + 0), "0#") & " [Period] " & _
            CStr(DbHolidayArray(i * 3 + 2))
    Next
End Sub
Private Sub Form_Load()
    DbHolidayInit
    DbHolidayDraw
End Sub
Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub
Private Sub cmdExit_Click()
    Unload Me
End Sub
Private Sub lstHoliday_Click()
    Dim i As Long
    
    i = lstHoliday.ListIndex
    If i < 0 Or i > DB_HOLIDAY_MAX - 1 Then Exit Sub
    
    If DbHolidayArray(i * 3 + 0) >= 1 And DbHolidayArray(i * 3 + 0) <= 12 Then
        dtpHoliday.Month = DbHolidayArray(i * 3 + 0)
    End If
    
    If DbHolidayArray(i * 3 + 1) >= 1 And DbHolidayArray(i * 3 + 1) <= 31 Then
        dtpHoliday.Day = DbHolidayArray(i * 3 + 1)
    End If
    
    txtDays = DbHolidayArray(i * 3 + 2)
    
End Sub
Private Sub cmdUpdate_Click()
    Dim i As Long
    
    i = lstHoliday.ListIndex
    If i < 0 Or i > DB_HOLIDAY_MAX - 1 Then Exit Sub
    
    DbHolidayArray(i * 3 + 0) = dtpHoliday.Month
    DbHolidayArray(i * 3 + 1) = dtpHoliday.Day
    
    If Val(txtDays) < 0 Or Val(txtDays) > 7 Then
        MsgBox ("Duration must be from 0 to 7days !")
        Exit Sub
    End If
    DbHolidayArray(i * 3 + 2) = Val(txtDays)
    
    DbHolidayDraw
    
    lstHoliday.ListIndex = i
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
    
    bRet = frmMain.SB100BPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 6, DbHolidayArray(0))
    
    If bRet = True Then
        DbHolidayDraw
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
    
    bRet = frmMain.SB100BPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 6, DbHolidayArray(0))
        
    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True
End Sub

