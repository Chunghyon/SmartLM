VERSION 5.00
Begin VB.Form frmEvent 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Event Monitor"
   ClientHeight    =   7815
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   12945
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7815
   ScaleWidth      =   12945
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClear 
      Caption         =   "Clear"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   6120
      TabIndex        =   15
      Top             =   1920
      Width           =   1815
   End
   Begin VB.CommandButton cmdEnd 
      Caption         =   "End Monitor"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3840
      TabIndex        =   14
      Top             =   1920
      Width           =   1815
   End
   Begin VB.CommandButton cmdStart 
      Caption         =   "Start Monitor"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1560
      TabIndex        =   13
      Top             =   1920
      Width           =   1815
   End
   Begin VB.ListBox lstEvents 
      Height          =   5130
      Left            =   120
      TabIndex        =   12
      Top             =   2520
      Width           =   12675
   End
   Begin VB.OptionButton optNetworkDevice 
      Caption         =   "Network Device"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   5160
      TabIndex        =   1
      Top             =   240
      Value           =   -1  'True
      Width           =   2205
   End
   Begin VB.OptionButton optSerialDevice 
      Caption         =   "Serial Device"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   720
      TabIndex        =   0
      Top             =   240
      Width           =   2085
   End
   Begin VB.Frame Frame2 
      Caption         =   "  "
      Height          =   1545
      Left            =   5040
      TabIndex        =   7
      Top             =   240
      Width           =   3660
      Begin VB.TextBox txtPortNo 
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
         Left            =   1725
         TabIndex        =   9
         Text            =   "5005"
         Top             =   960
         Width           =   1095
      End
      Begin VB.TextBox txtSrcIP 
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
         Left            =   1725
         TabIndex        =   8
         Text            =   "0.0.0.0"
         Top             =   456
         Width           =   1692
      End
      Begin VB.Label lblPortNo 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "Port Number :"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   240
         TabIndex        =   11
         Top             =   960
         Width           =   1305
      End
      Begin VB.Label lblIPAddress 
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "Source IP :"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   360
         TabIndex        =   10
         Top             =   480
         Width           =   1020
      End
   End
   Begin VB.Frame Frame3 
      Caption         =   "    "
      Height          =   1530
      Left            =   600
      TabIndex        =   2
      Top             =   240
      Width           =   3660
      Begin VB.ComboBox cmbComPort 
         BeginProperty DataFormat 
            Type            =   1
            Format          =   "0"
            HaveTrueFalseNull=   0
            FirstDayOfWeek  =   0
            FirstWeekOfYear =   0
            LCID            =   2052
            SubFormatType   =   1
         EndProperty
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
         ItemData        =   "frmEvent.frx":0000
         Left            =   1680
         List            =   "frmEvent.frx":001C
         Style           =   2  'Dropdown List
         TabIndex        =   4
         Top             =   456
         Width           =   1695
      End
      Begin VB.ComboBox cmbBaudrate 
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
         ItemData        =   "frmEvent.frx":0050
         Left            =   1680
         List            =   "frmEvent.frx":0063
         Style           =   2  'Dropdown List
         TabIndex        =   3
         Top             =   960
         Width           =   1695
      End
      Begin VB.Label lblComPort 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "ComPort : "
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   315
         TabIndex        =   6
         Top             =   465
         Width           =   1005
      End
      Begin VB.Label lblBaudrate 
         Alignment       =   1  'Right Justify
         AutoSize        =   -1  'True
         BackStyle       =   0  'Transparent
         Caption         =   "Baudrate : "
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   360
         TabIndex        =   5
         Top             =   960
         Width           =   990
      End
   End
End
Attribute VB_Name = "frmEvent"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub EnableControls(ByVal bComport As Boolean)
    cmbBaudrate.Enabled = bComport
    cmbComPort.Enabled = bComport
    txtSrcIP.Enabled = Not bComport
    txtPortNo.Enabled = Not bComport
End Sub

Private Sub cmdClear_Click()
    lstEvents.Clear
End Sub

Private Sub cmdEnd_Click()
    cmdStart.Enabled = True
    cmdEnd.Enabled = False
        
    frmMain.SB100BPC1.StopEventCapture
End Sub

Private Sub cmdStart_Click()
    Dim bResult As Boolean
    
    If optNetworkDevice = True Then
        bResult = frmMain.SB100BPC1.StartEventCapture(0, pubIPAddrToLong(txtSrcIP), Val(txtPortNo))
    Else
        bResult = frmMain.SB100BPC1.StartEventCapture(1, cmbComPort.ListIndex + 1, CLng(cmbBaudrate.Text))
    End If
    
    If bResult = False Then
        MsgBox "Cannot start Event capture!"
        Exit Sub
    End If
    cmdStart.Enabled = False
    cmdEnd.Enabled = True
End Sub

Private Sub Form_Load()
    cmbComPort.ListIndex = 0
    cmbBaudrate.Text = "115200"
    cmdEnd.Enabled = False
    lstEvents.Clear
End Sub

Private Sub optNetworkDevice_Click()
    If optNetworkDevice = True Then
        EnableControls False
    Else
        EnableControls True
    End If
End Sub

Private Sub optSerialDevice_Click()
    If optSerialDevice = True Then
        EnableControls True
    Else
        EnableControls False
    End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
    frmMain.SB100BPC1.StopEventCapture
    Me.Visible = False
    frmMain.Visible = True
End Sub

Public Sub OnEventXML(strEventXML As String)
    On Error GoTo err_parse
    Dim y, m, d, h, mm, s, w
    Dim strParseXML As String
    Dim bParseResult As Boolean
    Dim strMachineType As String
    Dim nMachineID As Integer
    Dim strEventType As String
    
    Dim strEvent As String
    Dim nManagerID As Long, nUserID As Long, nResult As Long
    Dim str1 As String, str2 As String, str3 As String, str4 As String, str5 As String
    
    strParseXML = strEventXML
    y = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "Year")
    m = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "Month")
    d = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "Day")
    h = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "Hour")
    mm = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "Minute")
    s = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "Second")
    w = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "Weekday")

    bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "MachineType", strMachineType)
    nMachineID = frmMain.SB100BPC1.XML_ParseInt(strParseXML, "MachineID")
    bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "EventType", strEventType)
    strEvent = y & "-" & m & "-" & d & " " & h & ":" & mm & ":" & s & " [" & strMachineType & ":" & nMachineID & "] " & strEventType & ", "
    Select Case strEventType
        Case "Management Log":
            nManagerID = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "ManagerID")
            nUserID = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "UserID")
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "Action", str1)
            nResult = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "Result")
            strEvent = strEvent & "Manager ID=" & nManagerID & ", User ID=" & nUserID & ", Action=" & str1 & ", Result=" & nResult
        Case "Time Log":
            nUserID = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "UserID")
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "AttendanceStatus", str1)
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "VerificationMode", str2)
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "AntipassStatus", str3)
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "Photo", str4)
            strEvent = strEvent & "User ID=" & nUserID & ", AttendanceStatus=" & str1 & ", VerificationMode=" & str2 & ", AntipassStatus=" & str3 & ", Photo=" & str4
        Case "Verification Success":
            nUserID = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "UserID")
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "VerificationMode", str1)
            strEvent = strEvent & "User ID=" & nUserID & ", VerificationMode=" & str1
        Case "Verification Failure":
            nUserID = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "UserID")
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "VerificationMode", str1)
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "ReasonOfFailure", str2)
            strEvent = strEvent & "User ID=" & nUserID & ", VerificationMode=" & str1 & ", ReasonOfFailure=" & str2
        Case "Alarm On":
            nUserID = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "UserID")
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "AlarmType", str1)
            strEvent = strEvent & "User ID=" & nUserID & ", AlarmType=" & str1
        Case "Alarm Off":
            nUserID = frmMain.SB100BPC1.XML_ParseLong(strParseXML, "UserID")
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "AlarmType", str1)
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "AlarmOffMethod", str2)
            strEvent = strEvent & "User ID=" & nUserID & ", AlarmType=" & str1 & ", AlarmOffMethod=" & str2
        Case "DoorBell":
            bParseResult = frmMain.SB100BPC1.XML_ParseString(strParseXML, "InputType", str1)
            strEvent = strEvent & "InputType=" & str1
    End Select

    lstEvents.AddItem strEvent
    
    Exit Sub
err_parse:
'    Debug.Print "event XML parse error!, ", strEvent
'    Debug.Print strParseXML
End Sub
