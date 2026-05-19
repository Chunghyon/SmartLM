VERSION 5.00
Begin VB.Form frmSerialNo 
   Caption         =   "Serial Number"
   ClientHeight    =   3735
   ClientLeft      =   5010
   ClientTop       =   3120
   ClientWidth     =   7965
   Icon            =   "frmPrtCode.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3735
   ScaleWidth      =   7965
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtProductCode 
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
      Left            =   2565
      MaxLength       =   32
      TabIndex        =   6
      Top             =   1785
      Width           =   4785
   End
   Begin VB.CommandButton cmdGetProductCode 
      Caption         =   "Get ProductCode"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   675
      Left            =   3600
      TabIndex        =   5
      Top             =   2665
      Width           =   1815
   End
   Begin VB.CommandButton cmdGetBackupNumber 
      Caption         =   "Get BackupNumber"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   675
      Left            =   2115
      TabIndex        =   4
      Top             =   2665
      Visible         =   0   'False
      Width           =   1815
   End
   Begin VB.TextBox txtBackupNo 
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
      Left            =   2565
      MaxLength       =   32
      TabIndex        =   3
      Top             =   1545
      Visible         =   0   'False
      Width           =   4785
   End
   Begin VB.TextBox txtSerialNo 
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
      Left            =   2565
      MaxLength       =   32
      TabIndex        =   2
      Top             =   945
      Width           =   4785
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
      Height          =   675
      Left            =   5835
      TabIndex        =   1
      Top             =   2665
      Width           =   1815
   End
   Begin VB.CommandButton cmdGetSerialNumber 
      Caption         =   "Get SerialNumber"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   675
      Left            =   1080
      TabIndex        =   0
      Top             =   2665
      Width           =   1815
   End
   Begin VB.Label lblBackuplNo 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Backup Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   510
      TabIndex        =   10
      Top             =   1605
      Visible         =   0   'False
      Width           =   1770
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Product Code :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   510
      TabIndex        =   9
      Top             =   1845
      Width           =   1485
   End
   Begin VB.Label lblSerialNo 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Serial Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   510
      TabIndex        =   8
      Top             =   1005
      Width           =   1590
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
      Left            =   270
      TabIndex        =   7
      Top             =   345
      Width           =   7425
   End
End
Attribute VB_Name = "frmSerialNo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mMachineNumber As Long

Private Sub cmdGetBackupNumber_Click()
    Dim vBackupNumber As Long
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Waiting..."
    txtBackupNo = ""
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vBackupNumber = frmMain.SB100BPC1.GetBackupNumber(mMachineNumber)
    If vBackupNumber <> 0 Then
        txtBackupNo = vBackupNumber
        lblMessage = "Success"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetProductCode_Click()
    Dim vProductCode As String
    Dim vRet As Long
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Waiting..."
    txtProductCode = ""
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.GetProductCode(mMachineNumber, vProductCode)
    If vRet = True Then
        txtProductCode = vProductCode
        lblMessage = "Success"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetSerialNumber_Click()
    Dim vSerialNumber As String
    Dim vRet As Long
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Waiting..."
    txtSerialNo = ""
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.GetSerialNumber(mMachineNumber, vSerialNumber)
    If vRet = True Then
        txtSerialNo = vSerialNumber
        lblMessage = "Success"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True
End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Unload Me
    frmMain.Visible = True
End Sub
