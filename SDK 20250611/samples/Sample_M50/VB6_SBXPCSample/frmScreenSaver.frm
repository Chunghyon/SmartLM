VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmScreenSaver 
   Caption         =   "Screen Saver Settings"
   ClientHeight    =   9270
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   8070
   LinkTopic       =   "frmScreenSaver"
   ScaleHeight     =   9270
   ScaleWidth      =   8070
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdDebugOutFileBrowse 
      Caption         =   "..."
      Height          =   405
      Left            =   7080
      TabIndex        =   28
      Top             =   7560
      Width           =   375
   End
   Begin VB.CheckBox chkDebugOut 
      Caption         =   "DebugOut"
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
      Left            =   600
      TabIndex        =   27
      Top             =   7560
      Width           =   1455
   End
   Begin VB.TextBox txtDebugOutputFile 
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
      Left            =   2160
      MaxLength       =   256
      TabIndex        =   26
      Text            =   "C:\temp.bmp"
      Top             =   7560
      Width           =   4935
   End
   Begin VB.CommandButton cmdGetGlyphSize 
      Caption         =   "Get Glyph Size"
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
      Left            =   4800
      TabIndex        =   25
      Top             =   5040
      Width           =   2775
   End
   Begin VB.TextBox txtFontWeight 
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
      Left            =   2400
      MaxLength       =   256
      TabIndex        =   23
      Text            =   "700"
      Top             =   6720
      Width           =   1935
   End
   Begin VB.TextBox txtFontWidth 
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
      Left            =   2400
      MaxLength       =   256
      TabIndex        =   21
      Text            =   "11"
      Top             =   6240
      Width           =   1935
   End
   Begin VB.TextBox txtFontHeight 
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
      Left            =   2400
      MaxLength       =   256
      TabIndex        =   19
      Text            =   "20"
      Top             =   5760
      Width           =   1935
   End
   Begin VB.TextBox txtGlyphHeight 
      Enabled         =   0   'False
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
      Left            =   2400
      MaxLength       =   256
      TabIndex        =   17
      Top             =   5280
      Width           =   1935
   End
   Begin VB.TextBox txtGlyphWidth 
      Enabled         =   0   'False
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
      Left            =   2400
      MaxLength       =   256
      TabIndex        =   15
      Top             =   4800
      Width           =   1935
   End
   Begin VB.CheckBox chkStrikeOut 
      Caption         =   "StrikeOut"
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
      Left            =   5280
      TabIndex        =   14
      Top             =   6720
      Width           =   1335
   End
   Begin VB.CheckBox chkUnderline 
      Caption         =   "Underline"
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
      Left            =   5280
      TabIndex        =   13
      Top             =   6240
      Width           =   1335
   End
   Begin VB.CheckBox chkItalic 
      Caption         =   "Italic"
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
      Left            =   5280
      TabIndex        =   12
      Top             =   5760
      Width           =   1335
   End
   Begin VB.CommandButton cmdGetSleepMsg 
      Caption         =   "Get Sleep Message"
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
      Left            =   360
      TabIndex        =   6
      Top             =   8280
      Width           =   2775
   End
   Begin VB.CommandButton cmdSetSleepMsg 
      Caption         =   "Set Sleep Message"
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
      Left            =   3240
      TabIndex        =   7
      Top             =   8280
      Width           =   2775
   End
   Begin VB.TextBox txtSleepMsg 
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   975
      Left            =   2400
      MaxLength       =   256
      MultiLine       =   -1  'True
      TabIndex        =   3
      Top             =   3480
      Width           =   5175
   End
   Begin VB.TextBox txtCustomerName 
      Height          =   495
      Left            =   2400
      MaxLength       =   64
      TabIndex        =   2
      Top             =   1920
      Width           =   5175
   End
   Begin VB.TextBox txtCompanyName 
      Height          =   495
      Left            =   2400
      MaxLength       =   64
      TabIndex        =   1
      Top             =   1200
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
      Height          =   495
      Left            =   6240
      TabIndex        =   0
      Top             =   8280
      Width           =   1335
   End
   Begin VB.CommandButton cmdSetCustomerInfo 
      Caption         =   "Set Customer Info"
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
      Left            =   3840
      TabIndex        =   5
      Top             =   2640
      Width           =   2775
   End
   Begin VB.CommandButton cmdGetCustomerInfo 
      Caption         =   "Get Customer Info"
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
      Left            =   960
      TabIndex        =   4
      Top             =   2640
      Width           =   2775
   End
   Begin MSComDlg.CommonDialog OpenDlg 
      Left            =   7200
      Top             =   6120
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Label Label5 
      Caption         =   "Font Weight:"
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
      Left            =   360
      TabIndex        =   24
      Top             =   6720
      Width           =   1935
   End
   Begin VB.Label Label4 
      Caption         =   "Font Width:"
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
      Left            =   360
      TabIndex        =   22
      Top             =   6240
      Width           =   1935
   End
   Begin VB.Label Label3 
      Caption         =   "Font Height:"
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
      Left            =   360
      TabIndex        =   20
      Top             =   5760
      Width           =   1935
   End
   Begin VB.Label Label2 
      Caption         =   "Glyph Height:"
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
      Left            =   360
      TabIndex        =   18
      Top             =   5280
      Width           =   1935
   End
   Begin VB.Label Label1 
      Caption         =   "Glyph Width:"
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
      Left            =   360
      TabIndex        =   16
      Top             =   4800
      Width           =   1935
   End
   Begin VB.Label lblSleepMsg 
      Caption         =   "Sleep Message:"
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
      Left            =   360
      TabIndex        =   11
      Top             =   3480
      Width           =   1935
   End
   Begin VB.Label lblCustomerName 
      Caption         =   "Customer Name:"
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
      Left            =   360
      TabIndex        =   10
      Top             =   1920
      Width           =   1935
   End
   Begin VB.Label lblCompanyName 
      Caption         =   "Company Name:"
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
      Left            =   360
      TabIndex        =   9
      Top             =   1200
      Width           =   1935
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
      TabIndex        =   8
      Top             =   360
      Width           =   7425
   End
End
Attribute VB_Name = "frmScreenSaver"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mMachineNumber As Long
Const SLEEEP_MSG_LEN = 128 * 2
Const COMPANY_NAME_LEN = 64 * 2    'Company Name can be 64 characters of UNICODE
Const CUSTOMER_NAME_LEN = 64 * 2   'Customer Name can be 64 characters of UNICODE

Private Sub cmdDebugOutFileBrowse_Click()
    OpenDlg.ShowSave
    txtDebugOutputFile.Text = OpenDlg.FileName
End Sub

Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True
End Sub

Private Sub cmdGetCustomerInfo_Click()
    Dim strXML As String
    Dim strCustomerName As String
    Dim strCompanyName As String
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetCustomerInfo"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        vRet = frmMain.SB100BPC1.XML_ParseBinaryUnicode(strXML, "CompanyName", strCompanyName, COMPANY_NAME_LEN)
        If vRet = False Then
            lblMessage.Caption = "GetCustomerInfo(Company Name) - XML Parse Error!"
            GoTo 1
        End If
        vRet = frmMain.SB100BPC1.XML_ParseBinaryUnicode(strXML, "CustomerName", strCustomerName, CUSTOMER_NAME_LEN)
        If vRet = False Then
            lblMessage.Caption = "GetCustomerInfo(Customer Name) - XML Parse Error!"
            GoTo 1
        End If
        
        txtCompanyName.Text = strCompanyName
        txtCustomerName.Text = strCustomerName
        lblMessage.Caption = "GetCustomerInfo OK"
    End If
    
1:
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetGlyphSize_Click()
    Dim strXML As String
    Dim vRet As Boolean
    Dim vErrorCode As Long
    strXML = ""
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetSleepMsgGlyphSize"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        lblMessage.Caption = "GetSleepMsgGlyphSize OK"
        txtGlyphWidth.Text = frmMain.SB100BPC1.XML_ParseInt(strXML, "Width")
        txtGlyphHeight.Text = frmMain.SB100BPC1.XML_ParseInt(strXML, "Height")
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetCustomerInfo_Click()
    Dim strXML As String
    Dim strCompanyName As String
    Dim strCustomerName As String
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    strCustomerName = txtCustomerName.Text
    strCompanyName = txtCompanyName.Text
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetCustomerInfo"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddBinaryUnicode strXML, "CompanyName", strCompanyName
    frmMain.SB100BPC1.XML_AddBinaryUnicode strXML, "CustomerName", strCustomerName
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "SetCustomerInfo OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetSleepMsg_Click()
    Dim strXML As String
    Dim strSleepMessage As String
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetSleepMessage"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        vRet = frmMain.SB100BPC1.XML_ParseBinaryUnicode(strXML, "SleepMessage", strSleepMessage, SLEEEP_MSG_LEN)
        If vRet = True Then
            txtSleepMsg.Text = strSleepMessage
            lblMessage.Caption = "GetSleepMessage OK"
        Else
            lblMessage.Caption = "GetSleepMessage - XML Parse Error!"
        End If
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetSleepMsg_Click()
    Dim strXML As String, strFontXML As String
    Dim strSleepMessage As String
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim vGlyphWidth As Long, vGlyphHeight As Long
    Dim vFontHeight As Long, vFontWidth As Long, vFontWeight As Long
    Dim vDebugOutputFile As String
    strFontXML = ""
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    strSleepMessage = txtSleepMsg.Text
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetSleepMsgGlyphSize"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        lblMessage.Caption = "GetSleepMsgGlyphSize OK"
        vGlyphWidth = frmMain.SB100BPC1.XML_ParseInt(strXML, "Width")
        vGlyphHeight = frmMain.SB100BPC1.XML_ParseInt(strXML, "Height")
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        GoTo 1:
    End If
    
    vFontHeight = Val(txtFontHeight.Text)
    vFontWidth = Val(txtFontWidth.Text)
    vFontWeight = Val(txtFontWeight.Text)
    frmMain.SB100BPC1.XML_AddString strFontXML, "FaceName", "Arial"
    frmMain.SB100BPC1.XML_AddInt strFontXML, "Height", vFontHeight
    frmMain.SB100BPC1.XML_AddInt strFontXML, "Width", vFontWidth
    frmMain.SB100BPC1.XML_AddInt strFontXML, "Weight", vFontWeight
    frmMain.SB100BPC1.XML_AddBoolean strFontXML, "Italic", chkItalic.Value
    frmMain.SB100BPC1.XML_AddBoolean strFontXML, "Underline", chkUnderline.Value
    frmMain.SB100BPC1.XML_AddBoolean strFontXML, "StrikeOut", chkStrikeOut.Value
    vDebugOutputFile = txtDebugOutputFile.Text
    If chkDebugOut.Value = 1 And vDebugOutputFile <> "" Then
        frmMain.SB100BPC1.XML_AddString strFontXML, "DebugOut", vDebugOutputFile
    End If
    strXML = ""
    strSleepMessage = txtSleepMsg.Text
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetSleepMessage"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    
    frmMain.SB100BPC1.XML_AddBinaryGlyph strXML, strSleepMessage, vGlyphWidth, vGlyphHeight, strFontXML
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        lblMessage.Caption = "SetSleepMessage OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        GoTo 1:
    End If
1:
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Unload Me
    frmMain.Visible = True
End Sub

