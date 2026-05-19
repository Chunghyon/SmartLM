VERSION 5.00
Begin VB.Form frmGroup 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Group Name Management"
   ClientHeight    =   2985
   ClientLeft      =   45
   ClientTop       =   405
   ClientWidth     =   5160
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2985
   ScaleWidth      =   5160
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdExit 
      Caption         =   "Exit"
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
      Left            =   3480
      TabIndex        =   7
      Top             =   2280
      Width           =   1335
   End
   Begin VB.CommandButton cmdSet 
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
      Left            =   1740
      TabIndex        =   5
      Top             =   2280
      Width           =   1335
   End
   Begin VB.CommandButton cmdGet 
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
      Left            =   360
      TabIndex        =   4
      Top             =   2280
      Width           =   1335
   End
   Begin VB.TextBox txtGroupName 
      Height          =   375
      Left            =   1740
      TabIndex        =   3
      Top             =   1560
      Width           =   2985
   End
   Begin VB.TextBox txtGroupNumber 
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
      Left            =   1740
      TabIndex        =   1
      Top             =   990
      Width           =   2985
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
      Left            =   180
      TabIndex        =   6
      Top             =   210
      Width           =   4785
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "Name :"
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
      Index           =   0
      Left            =   450
      TabIndex        =   2
      Top             =   1590
      Width           =   1215
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Caption         =   "Group No :"
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
      Left            =   450
      TabIndex        =   0
      Top             =   1020
      Width           =   1215
   End
End
Attribute VB_Name = "frmGroup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mMachineNumber As Long
Dim glngGroupName As Variant
Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True
End Sub

Private Sub cmdGet_Click()
    Dim vGroupNumber As Long
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i As Long
    Dim vName As String
    
    Dim temp As String
    Dim strXML As String
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vGroupNumber = Val(txtGroupNumber.Text)
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetGroupName"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "GroupNo", vGroupNumber
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        vRet = frmMain.SB100BPC1.XML_ParseMultiUnicode(strXML, "GroupName", vName, 5 * 2)
        If vRet = True Then
            txtGroupName.Text = vName
            lblMessage.Caption = "GetGroupName OK"
        Else
            lblMessage.Caption = "GetGroupName - XML Parse Error!"
        End If
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub


Private Sub cmdSet_Click()
    Dim vGroupNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim strXML As String
    Dim vName As String
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vGroupNumber = Val(txtGroupNumber.Text)
    vName = txtGroupName.Text
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetGroupName"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "GroupNo", vGroupNumber
    frmMain.SB100BPC1.XML_AddBinaryNameGlyph mMachineNumber, strXML, vName
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "SetGroupName OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True

End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
    txtGroupNumber.Text = 1
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Unload Me
    frmMain.Visible = True
End Sub

