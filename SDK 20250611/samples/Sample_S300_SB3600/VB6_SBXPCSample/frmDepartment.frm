VERSION 5.00
Begin VB.Form frmDepartment 
   Caption         =   "Depart Management"
   ClientHeight    =   8325
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   10650
   LinkTopic       =   "Form1"
   ScaleHeight     =   8325
   ScaleWidth      =   10650
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtDepart 
      Height          =   375
      Left            =   1920
      MaxLength       =   12
      TabIndex        =   6
      Top             =   840
      Width           =   5175
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
      Left            =   9000
      TabIndex        =   4
      Top             =   7440
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
      Left            =   9000
      TabIndex        =   3
      Top             =   6600
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
      Left            =   9000
      TabIndex        =   2
      Top             =   5760
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
      Left            =   9000
      TabIndex        =   1
      Top             =   840
      Width           =   1410
   End
   Begin VB.ListBox lstDepart 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   6810
      Left            =   240
      TabIndex        =   0
      Top             =   1320
      Width           =   8535
   End
   Begin VB.Label Label1 
      Caption         =   "Department Name"
      Height          =   255
      Left            =   360
      TabIndex        =   7
      Top             =   840
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
      Left            =   240
      TabIndex        =   5
      Top             =   120
      Width           =   10185
   End
End
Attribute VB_Name = "frmDepartment"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const DEPARTMENT_COUNT = 20
Dim DbDepart(DEPARTMENT_COUNT - 1) As String

Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True

End Sub

Private Sub cmdRead_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long
    Dim i As Integer
    Dim str As String
    Dim bSuccess As Boolean
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    bSuccess = True
    For i = 0 To DEPARTMENT_COUNT - 1
        bRet = frmMain.SB100BPC1.GetDepartName(frmMain.gMachineNumber, i, 0, str)
            
        If bRet = True Then
            DbDepart(i) = str
        Else
            bSuccess = False
            DbDepart(i) = ""
'            Exit For
        End If
    Next
    
    DbDepartDraw
    If bSuccess = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If

    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True
End Sub

Private Sub cmdUpdate_Click()
    Dim i As Long
    
    i = lstDepart.ListIndex
    If i < 0 Or i > DEPARTMENT_COUNT - 1 Then Exit Sub
    
    DbDepart(i) = txtDepart
    
    DbDepartDraw
    
    lstDepart.ListIndex = i
End Sub

Private Sub cmdWrite_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long
    Dim i As Integer
    Dim str As String
    Dim bSuccess As Boolean
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(frmMain.gMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    bSuccess = True
    For i = 0 To DEPARTMENT_COUNT - 1
        bRet = frmMain.SB100BPC1.SetDepartName(frmMain.gMachineNumber, i, 0, DbDepart(i))
            
        If bRet = False Then
            bSuccess = False
            Exit For
        End If
    Next
    
    If bSuccess = True Then
        DbDepartDraw
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If

    frmMain.SB100BPC1.EnableDevice frmMain.gMachineNumber, True

End Sub

Private Sub DbDepartDraw()
    Dim i As Long
    
    lstDepart.Clear
    For i = 0 To DEPARTMENT_COUNT - 1
        lstDepart.AddItem _
            "[No.] " & Format(i, "0#") & "  " & _
            "[Name] " & _
            DbDepart(i)
    Next
End Sub
Private Sub DbDepartInit()
    Dim i As Long
    
    For i = 0 To DEPARTMENT_COUNT - 1
        DbDepart(i) = ""
    Next
    
End Sub


Private Sub Form_Load()
    DbDepartInit
    DbDepartDraw
   
End Sub

Private Sub lstDepart_Click()
    Dim i As Long
    
    i = lstDepart.ListIndex
    If i < 0 Or i > DEPARTMENT_COUNT - 1 Then Exit Sub
    
    txtDepart = DbDepart(i)
End Sub
