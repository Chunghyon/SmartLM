VERSION 5.00
Begin VB.Form frmEnroll 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Manage Enroll Data"
   ClientHeight    =   9495
   ClientLeft      =   3075
   ClientTop       =   1530
   ClientWidth     =   7350
   Icon            =   "frmEnroll.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   9495
   ScaleWidth      =   7350
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtUserTZ2 
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
      Left            =   2400
      TabIndex        =   40
      Text            =   "0"
      Top             =   4560
      Width           =   735
   End
   Begin VB.TextBox txtUserTZ1 
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
      Left            =   2400
      TabIndex        =   39
      Text            =   "0"
      Top             =   4080
      Width           =   735
   End
   Begin VB.CommandButton cmdDuress 
      Caption         =   "ModifyDuressFP"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   37
      Top             =   7440
      Width           =   3015
   End
   Begin VB.ComboBox cmbDuress 
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
      ItemData        =   "frmEnroll.frx":0442
      Left            =   2400
      List            =   "frmEnroll.frx":044C
      TabIndex        =   36
      Text            =   "0"
      Top             =   3600
      Width           =   1215
   End
   Begin VB.TextBox txtCardNumber 
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
      TabIndex        =   34
      Top             =   1608
      Width           =   1215
   End
   Begin VB.CommandButton cmdDeleteCompany 
      Caption         =   "Delete Company Name"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   32
      Top             =   5400
      Width           =   3012
   End
   Begin VB.CommandButton cmdSetCompany 
      Caption         =   "Set Company Name"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   31
      Top             =   4920
      Width           =   3012
   End
   Begin VB.CommandButton cmdGetName 
      Caption         =   "Get Name Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   30
      ToolTipText     =   "Get All Enroll Data From Device And Save To DataBase"
      Top             =   3960
      Width           =   3012
   End
   Begin VB.CommandButton cmdSetName 
      Caption         =   "Set Name Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   29
      ToolTipText     =   "Load All Enroll Data From DataBase And Set To Device"
      Top             =   4440
      Width           =   3012
   End
   Begin VB.CommandButton cmdModifyPrivilege 
      Caption         =   "ModifyPrivilege"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   28
      Top             =   6960
      Width           =   3000
   End
   Begin VB.CommandButton cmdEnableUser 
      Caption         =   "EnableUser"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   27
      Top             =   6480
      Width           =   3000
   End
   Begin VB.CommandButton cmdSetAllEnrollData 
      Caption         =   "Set All Enroll Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   26
      ToolTipText     =   "Load All Enroll Data From DataBase And Set To Device"
      Top             =   2880
      Width           =   3000
   End
   Begin VB.CommandButton cmdGetAllEnrollData 
      Caption         =   "Get All Enroll Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   25
      ToolTipText     =   "Get All Enroll Data From Device And Save To DataBase"
      Top             =   2400
      Width           =   3000
   End
   Begin VB.CommandButton cmdGetEnrollData 
      Caption         =   "Get Enroll Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4008
      TabIndex        =   24
      ToolTipText     =   "Get EnrollData From Device"
      Top             =   960
      Width           =   3000
   End
   Begin VB.CommandButton cmdClearData 
      Caption         =   "Clear All Data(E,GL,SL) "
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   23
      ToolTipText     =   "Clear EnrollData and LogDat Into Device"
      Top             =   8400
      Width           =   3000
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
      Height          =   450
      Left            =   4000
      TabIndex        =   22
      Top             =   9000
      Width           =   3000
   End
   Begin VB.CommandButton cmdGetEnrollInfo 
      Caption         =   "Get Enroll Info"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   21
      ToolTipText     =   "Get All Enrolled User Info From Device"
      Top             =   6000
      Width           =   3000
   End
   Begin VB.CommandButton cmdDeleteEnrollData 
      Caption         =   "Delete Enroll Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   20
      ToolTipText     =   "Delete Enroll Data Into Device"
      Top             =   1920
      Width           =   3000
   End
   Begin VB.CommandButton cmdSetEnrollData 
      Caption         =   "Set Enroll Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   19
      ToolTipText     =   "Set EnrollData To Device"
      Top             =   1440
      Width           =   3000
   End
   Begin VB.CommandButton cmdEmptyEnrollData 
      Caption         =   "Empty Enroll Data"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Left            =   4000
      TabIndex        =   18
      Top             =   7920
      Width           =   3000
   End
   Begin VB.TextBox txtName 
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
      Left            =   1200
      MaxLength       =   8
      TabIndex        =   16
      Top             =   5040
      Width           =   2310
   End
   Begin VB.ComboBox cmbConvType 
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
      ItemData        =   "frmEnroll.frx":0456
      Left            =   2160
      List            =   "frmEnroll.frx":0466
      TabIndex        =   15
      Text            =   "None"
      Top             =   5880
      Width           =   1455
   End
   Begin VB.CheckBox chkEnable 
      Caption         =   "Disable User"
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
      Left            =   465
      TabIndex        =   13
      Top             =   5520
      Width           =   1680
   End
   Begin VB.CommandButton cmdDel 
      Caption         =   "Delete DB"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   564
      Left            =   2415
      TabIndex        =   11
      ToolTipText     =   "Delete All Saved Data From DataBase"
      Top             =   8835
      Width           =   1245
   End
   Begin VB.Data datEnroll 
      Caption         =   "0/0"
      Connect         =   "Access"
      DatabaseName    =   ""
      DefaultCursorType=   0  'DefaultCursor
      DefaultType     =   2  'UseODBC
      Exclusive       =   0   'False
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   300
      Options         =   0
      ReadOnly        =   0   'False
      RecordsetType   =   1  'Dynaset
      RecordSource    =   ""
      Top             =   8895
      Width           =   2115
   End
   Begin VB.ComboBox cmbEMachineNumber 
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
      Height          =   360
      ItemData        =   "frmEnroll.frx":0481
      Left            =   2400
      List            =   "frmEnroll.frx":04A0
      TabIndex        =   9
      Text            =   "cmbEMachineNumber"
      Top             =   2160
      Width           =   1215
   End
   Begin VB.ComboBox cmbPrivilege 
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
      Height          =   360
      ItemData        =   "frmEnroll.frx":04BF
      Left            =   2400
      List            =   "frmEnroll.frx":04C9
      TabIndex        =   7
      Text            =   "cmbPrivilege"
      Top             =   3120
      Width           =   1215
   End
   Begin VB.ListBox lstEnrollData 
      Height          =   2010
      Left            =   120
      TabIndex        =   4
      Top             =   6720
      Width           =   3780
   End
   Begin VB.TextBox txtEnrollNumber 
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
      MaxLength       =   8
      TabIndex        =   2
      Top             =   1065
      Width           =   1215
   End
   Begin VB.ComboBox cmbBackupNumber 
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
      Height          =   360
      ItemData        =   "frmEnroll.frx":04D5
      Left            =   2400
      List            =   "frmEnroll.frx":04EB
      TabIndex        =   0
      Text            =   "cmbBackupNumber"
      Top             =   2640
      Width           =   1215
   End
   Begin VB.Label Label6 
      Caption         =   "UserTZ2 :"
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
      Left            =   480
      TabIndex        =   41
      Top             =   4560
      Width           =   1095
   End
   Begin VB.Label Label5 
      Caption         =   "UserTZ1 :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   480
      TabIndex        =   38
      Top             =   4080
      Width           =   1095
   End
   Begin VB.Label Label4 
      AutoSize        =   -1  'True
      Caption         =   "Duress :"
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
      Left            =   480
      TabIndex        =   35
      Top             =   3600
      Width           =   735
   End
   Begin VB.Label lblCardNum 
      Caption         =   "Card Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Left            =   480
      TabIndex        =   33
      Top             =   1680
      Width           =   1692
   End
   Begin VB.Label lbName 
      AutoSize        =   -1  'True
      Caption         =   "Name :"
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
      Left            =   480
      TabIndex        =   17
      Top             =   5115
      Width           =   660
   End
   Begin VB.Label Label3 
      Caption         =   "ConvType"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   480
      TabIndex        =   14
      Top             =   5880
      Width           =   975
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      Caption         =   "Total : "
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
      Left            =   2025
      TabIndex        =   12
      Top             =   6360
      Width           =   630
   End
   Begin VB.Label lblEMachineNumber 
      AutoSize        =   -1  'True
      Caption         =   "EMachine Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   288
      Left            =   468
      TabIndex        =   10
      Top             =   2160
      Width           =   1836
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      Caption         =   "Privilege :"
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
      Left            =   465
      TabIndex        =   8
      Top             =   3135
      Width           =   870
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
      Height          =   435
      Left            =   360
      TabIndex        =   6
      Top             =   360
      Width           =   6675
   End
   Begin VB.Label lblEnrollData 
      AutoSize        =   -1  'True
      Caption         =   "Enrolled Data :"
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
      Left            =   225
      TabIndex        =   5
      Top             =   6360
      Width           =   1350
   End
   Begin VB.Label lblBackupNumber 
      AutoSize        =   -1  'True
      Caption         =   "Backup Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   288
      Left            =   468
      TabIndex        =   3
      Top             =   2700
      Width           =   1620
   End
   Begin VB.Label lblEnrollNum 
      AutoSize        =   -1  'True
      Caption         =   "Enroll Number :"
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
      Index           =   0
      Left            =   465
      TabIndex        =   1
      Top             =   1125
      Width           =   1440
   End
End
Attribute VB_Name = "frmEnroll"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Const DATASIZE = (1404 + 12) / 4 'SmackBio
Const NAMESIZE = 54
Public gGetState As Boolean
Dim glngEnrollData As Variant
Dim gTemplngEnrollData(DATASIZE) As Long
Dim glngEnrollPData As Long
Dim gbytEnrollData(DATASIZE * 5) As Byte
Dim mMachineNumber As Long
Dim mDeviceKind As Long
Dim glngUserName As Variant
Dim gTempEnrollName(NAMESIZE) As Long

Private Sub chkEnable_Click()
    If chkEnable.Value = 1 Then
        chkEnable.Caption = "Enable User"
    Else
        chkEnable.Caption = "Disable User"
    End If
End Sub

Private Sub cmdClearData_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.Fk528KM1.ClearKeeperData(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "ClearKeeperData OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdDel_Click()
    datEnroll.Database.Execute "delete * from tblEnroll"
    datEnroll.Refresh
End Sub

Private Sub cmdDeleteCompany_Click()
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEMachineNumber = cmbEMachineNumber.Text
    
    vRet = frmMain.Fk528KM1.SetCompanyName(mMachineNumber, _
                                           0, _
                                           glngUserName)
    If vRet = True Then
        lblMessage.Caption = "Delete Company Name OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True

End Sub

Private Sub cmdDeleteEnrollData_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vEMachineNumber = cmbEMachineNumber.Text
    vFingerNumber = cmbBackupNumber.Text
    
    vRet = frmMain.Fk528KM1.DeleteEnrollData(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber)
    If vRet = True Then
        lblMessage.Caption = "DeleteEnrollData OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdDuress_Click()
    Dim vEnrollNumber As Long
    Dim vFingerNumber As Long
    Dim vDuressSetting As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vFingerNumber = cmbBackupNumber.Text
    vDuressSetting = cmbDuress.Text
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.Fk528KM1.ModifyDuressFP(mMachineNumber, _
                                            vEnrollNumber, _
                                            vFingerNumber, _
                                            vDuressSetting)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True

End Sub

Private Sub cmdEmptyEnrollData_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
   
    vRet = frmMain.Fk528KM1.EmptyEnrollData(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdEnableUser_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vFlag As Boolean
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vEMachineNumber = cmbEMachineNumber.ListIndex + 1
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vFingerNumber = cmbBackupNumber.Text
    vFlag = chkEnable.Value
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.Fk528KM1.EnableUser(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vFlag)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True
End Sub

Private Sub cmdGetAllEnrollData_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vPrivilege As Long
    Dim vEnable As Long
    Dim vFlag As Boolean
    Dim vRet As Long
    Dim vErrorCode As Long
    Dim vStr As String
    Dim i As Long
    Dim vTitle As String
    
    lstEnrollData.Clear
    vTitle = frmEnroll.Caption
    Label2.Caption = ""
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If

    vRet = frmMain.Fk528KM1.ReadAllUserID(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "ReadAllUserID OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        frmMain.Fk528KM1.EnableDevice mMachineNumber, True
        Exit Sub
    End If
    
'---- Get Enroll data and save into database -------------
    MousePointer = vbHourglass
    vFlag = False
    With datEnroll
        gGetState = True
        .RecordSource = "select * from " & "tblEnroll"
        .Refresh
        Do
            vRet = frmMain.Fk528KM1.GetAllUserID(mMachineNumber, _
                                                 vEnrollNumber, _
                                                 vEMachineNumber, _
                                                 vFingerNumber, _
                                                 vPrivilege, _
                                                 vEnable)
            If vRet <> True Then Exit Do
            vFlag = True
EEE:
            If vFingerNumber = 10 Then vFingerNumber = 15
            vRet = frmMain.Fk528KM1.GetEnrollData(mMachineNumber, _
                                                  vEnrollNumber, _
                                                  vEMachineNumber, _
                                                  vFingerNumber, _
                                                  vPrivilege, _
                                                  glngEnrollData, _
                                                  glngEnrollPData)
           
            If vRet <> True Then
                vFlag = False
                vStr = "GetEnrollData"
                frmMain.Fk528KM1.GetLastError vErrorCode
                vRet = MsgBox(ErrorPrint(vErrorCode) & ": Continue ?", vbYesNoCancel, "GetEnrollData")
                If vRet = vbYes Then
                    GoTo EEE
                ElseIf vRet = vbCancel Then
                    MousePointer = vbDefault
                    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
                    gGetState = False
                    Exit Sub
                End If
            End If
            
            With .Recordset
                .FindFirst "[EnrollNumber]=" & CStr(vEnrollNumber)
                If Not .NoMatch Then
                    .FindFirst "[EMachineNumber]=" & CStr(vEMachineNumber)
                    If Not .NoMatch Then
                        .FindFirst "[FingerNumber]=" & CStr(vFingerNumber)
                        If Not .NoMatch Then
                            lblMessage.Caption = "Double ID"
                            GoTo FFF
                        End If
                    End If
                End If
                
                .AddNew
                !EMachineNumber = vEMachineNumber
                !EnrollNumber = vEnrollNumber
                !FingerNumber = vFingerNumber
                !Privilige = vPrivilege
                
                If vFingerNumber = 10 Then
                    !Password = glngEnrollPData
                ElseIf vFingerNumber = 15 Then
                    !Password = glngEnrollPData
                ElseIf vFingerNumber = 11 Then
                    !Password = glngEnrollPData
                Else
                    For i = 0 To DATASIZE - 1
                        gbytEnrollData(i * 5) = 1
                        If glngEnrollData(i) < 0 Then
                            gbytEnrollData(i * 5) = 0
                            glngEnrollData(i) = Abs(glngEnrollData(i))
                        End If
                        gbytEnrollData(i * 5 + 1) = (glngEnrollData(i) \ 256 \ 256 \ 256)
                        gbytEnrollData(i * 5 + 2) = (glngEnrollData(i) \ 256 \ 256) Mod 256
                        gbytEnrollData(i * 5 + 3) = (glngEnrollData(i) \ 256) Mod 256
                        gbytEnrollData(i * 5 + 4) = glngEnrollData(i) Mod 256
                    Next
                    !FPdata = gbytEnrollData
                End If
                .Update
FFF:
            End With
            
            lblMessage.Caption = Format(vEMachineNumber, "00#") & "-" & Format(vEnrollNumber, "0000#") & "-" & vFingerNumber
            frmEnroll.Caption = Format(vEnrollNumber, "0000#")
            txtEnrollNumber.Text = vEnrollNumber
            cmbBackupNumber.Text = vFingerNumber
            cmbEMachineNumber.Text = vEMachineNumber
            cmbPrivilege.Text = vPrivilege
            DoEvents
        Loop
        gGetState = False
        If .Recordset.RecordCount > 1 Then .Recordset.MoveLast
    End With
    vTitle = frmEnroll.Caption
    MousePointer = vbDefault
    
    If vFlag = True Then
        lblMessage.Caption = "GetAllUserID OK"
    Else
        lblMessage.Caption = vStr & ":" & ErrorPrint(vErrorCode)
    End If
    
    DoEvents
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetEnrollData_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vPrivilege As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i As Long
    
    lstEnrollData.Clear
    Label2.Caption = ""
    lstEnrollData.Clear
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vFingerNumber = cmbBackupNumber.Text
    vEMachineNumber = cmbEMachineNumber.Text
    If vFingerNumber = 10 Then vFingerNumber = 15

    
    vRet = frmMain.Fk528KM1.GetEnrollData(mMachineNumber, _
                                          vEnrollNumber, _
                                          vEMachineNumber, _
                                          vFingerNumber, _
                                          vPrivilege, _
                                          glngEnrollData(0), _
                                          glngEnrollPData)
   If vRet = True Then
        cmbPrivilege.ListIndex = vPrivilege
        lblMessage.Caption = "GetEnrollData OK"
        If vFingerNumber = 15 Then
            txtCardNumber.Text = ""
            While glngEnrollPData > 0
                i = glngEnrollPData Mod 16 - 1 + Asc("0")
                txtCardNumber.Text = txtCardNumber.Text + Chr(i)
                glngEnrollPData = glngEnrollPData \ 16
            Wend
        ElseIf vFingerNumber = 11 Then
            txtCardNumber.Text = (CStr(glngEnrollPData))
            lstEnrollData.AddItem (CStr(glngEnrollPData))
        ElseIf vFingerNumber = 14 Then
            txtUserTZ1.Text = (CStr(glngEnrollPData \ 256))
            txtUserTZ2.Text = (CStr(glngEnrollPData Mod 256))
        Else
            For i = 0 To DATASIZE - 1
                lstEnrollData.AddItem (CStr(glngEnrollData(i)))
            Next
        End If
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetEnrollInfo_Click()
    Dim vEMachineNumber As Long
    Dim vEnrollNumber As Long
    Dim vFingerNumber As Long
    Dim vPrivilege As Long
    Dim vEnable As Long
    Dim vRet As Long
    Dim vFlag As Boolean
    Dim vErrorCode As Long
    Dim i As Long
    
    lblEnrollData = "User IDs"
    lstEnrollData.Clear
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.Fk528KM1.ReadAllUserID(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "ReadAllUserID OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        frmMain.Fk528KM1.EnableDevice mMachineNumber, True
        Exit Sub
    End If
    
'------ Show all enroll information ----------
    vFlag = False
    i = 0
    lstEnrollData.AddItem ("No.    EnNo   EMNo   Fp   Priv  Enable Duress")
    Do
        vRet = frmMain.Fk528KM1.GetAllUserID(mMachineNumber, _
                                             vEnrollNumber, _
                                             vEMachineNumber, _
                                             vFingerNumber, _
                                             vPrivilege, _
                                             vEnable)
        If vRet <> True Then Exit Do
        vFlag = True
        lstEnrollData.AddItem (Format(i, "00#") & "   " & _
                               Format(vEnrollNumber, "0000#") & "    " & _
                               Format(vEMachineNumber, "00#") & "      " & _
                               Format(vFingerNumber, "0#") & "    " & _
                               CStr(vPrivilege) & "        " & _
                               CStr(vEnable Mod 256) & "        " & _
                               CStr(vEnable \ 256))

        i = i + 1
        Label2.Caption = "Total : " & i
    Loop
    
    If vFlag = True Then
        lblMessage.Caption = "GetAllUserID OK"
    Else
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetName_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i As Long
    Dim vName As String
    
    Dim temp As String
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vEMachineNumber = cmbEMachineNumber.Text
    
    vRet = frmMain.Fk528KM1.GetUserName(mDeviceKind, _
                                        mMachineNumber, _
                                        vEnrollNumber, _
                                        vEMachineNumber, _
                                        glngUserName)
    If vRet = True Then
        'FontForFK1.SetTextBitmap mDeviceKind, vName, glngUserName 'SmackBio
        txtName.Text = glngUserName 'vName 'SmackBio
        lblMessage.Caption = "GetUserName OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdModifyPrivilege_Click()
    Dim vEnrollNumber As Long
    Dim vFingerNumber As Long
    Dim vEMachineNumber As Long
    Dim vMachinePrivilege As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vEMachineNumber = cmbEMachineNumber.ListIndex + 1
    vFingerNumber = cmbBackupNumber.Text
    vMachinePrivilege = cmbPrivilege.Text
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.Fk528KM1.ModifyPrivilege(mMachineNumber, _
                                            vEnrollNumber, _
                                            vEMachineNumber, _
                                            vFingerNumber, _
                                            vMachinePrivilege)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetAllEnrollData_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vPrivilege As Long
    Dim vEnable As Long
    Dim vFlag As Boolean
    Dim vRet As Long
    Dim vErrorCode As Long
    Dim vStr As String
    Dim vByte() As Byte
    Dim i As Long
    Dim vTitle As String
    Dim vConvResult As Long
    
    lstEnrollData.Clear
    vTitle = frmEnroll.Caption
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vFlag = False
    gGetState = True
    MousePointer = vbHourglass
    With datEnroll
        .RecordSource = "select * from " & "tblEnroll"
        .Refresh
         
         With .Recordset
             If .RecordCount = 0 Then GoTo EEE
            .MoveLast
            .MoveFirst
            Do While .EOF = False
                vEMachineNumber = !EMachineNumber
                vEnrollNumber = !EnrollNumber
                vFingerNumber = !FingerNumber
                vPrivilege = !Privilige
                glngEnrollPData = !Password
                If vFingerNumber < 10 Then
                    vStr = !FPdata
                    vByte = vStr
                    For i = 0 To DATASIZE - 1
                        glngEnrollData(i) = vByte(i * 5 + 1)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 2)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 3)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 4)
                        If vByte(i * 5) = 0 Then
                            glngEnrollData(i) = 0 - glngEnrollData(i)
                        End If
                    Next
                End If
FFF:
                vRet = frmMain.Fk528KM1.SetEnrollData(mMachineNumber, _
                                                      vEnrollNumber, _
                                                      vEMachineNumber, _
                                                      vFingerNumber, _
                                                      vPrivilege, _
                                                      glngEnrollData, _
                                                      glngEnrollPData)
                If vRet <> True Then
                    vFlag = False
                    vStr = "SetEnrollData"
                    frmMain.Fk528KM1.GetLastError vErrorCode
                    vRet = MsgBox(ErrorPrint(vErrorCode) & ": Continue ?", vbYesNoCancel, "SetEnrollData")
                    If vRet = vbYes Then GoTo FFF
                    If vRet = vbCancel Then GoTo EEE
                End If

LLL:
                lblMessage.Caption = "EMachine = " & Format(vEMachineNumber, "00#") & ", ID = " & Format(vEnrollNumber, "000#") & ", FpNo = " & vFingerNumber _
                                    & ", Count = " & (.AbsolutePosition + 1)
                
                frmEnroll.Caption = (.AbsolutePosition + 1)
                DoEvents
                .MoveNext
            Loop
        End With
EEE:
    End With
    vTitle = frmEnroll.Caption
    MousePointer = vbDefault
    gGetState = False
    
    lblMessage.Caption = "SetAllUserData OK"
    DoEvents
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetCompany_Click()
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEMachineNumber = cmbEMachineNumber.Text
    
    glngUserName = txtName.Text 'SmackBio

    vRet = frmMain.Fk528KM1.SetCompanyName(mMachineNumber, _
                                           1, _
                                           glngUserName)
    If vRet = True Then
        lblMessage.Caption = "Set Company Name OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True

End Sub

Private Sub cmdSetEnrollData_Click()
    Dim vEnrollNumber As Long
    Dim vCardNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vPrivilege As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim vConvResult As Long
    Dim vUserTZ1 As Byte
    Dim vUserTZ2 As Byte
    Dim i As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vCardNumber = Val(txtCardNumber.Text)
    vFingerNumber = cmbBackupNumber.Text
    vPrivilege = cmbPrivilege.Text
    vEMachineNumber = cmbEMachineNumber.Text
    vUserTZ1 = Val(txtUserTZ1.Text)
    vUserTZ2 = Val(txtUserTZ2.Text)
    
    ' Card Number valid
    If vCardNumber <> 0 Then
        If vFingerNumber = 11 Then
            glngEnrollPData = vCardNumber
        End If
    End If
    
    If vFingerNumber = 14 Then
        glngEnrollPData = vUserTZ1 * 256 + vUserTZ2
    End If
    
    If vFingerNumber = 10 Then
        vFingerNumber = 15
        i = Len(txtCardNumber.Text)
        If i > 4 Then i = 4
        glngEnrollPData = 0
        While i > 0
            glngEnrollPData = glngEnrollPData * 16 + Asc(Mid(txtCardNumber.Text, i, 1)) - Asc("0") + 1
            i = i - 1
        Wend
    End If
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If

    vRet = frmMain.Fk528KM1.SetEnrollData(mMachineNumber, _
                                          vEnrollNumber, _
                                          vEMachineNumber, _
                                          vFingerNumber, _
                                          vPrivilege, _
                                          glngEnrollData, _
                                          glngEnrollPData)
                            
    If vRet = True Then
        lblMessage.Caption = "SetEnrollData OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetName_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    Dim vBirthYear As Long
    Dim vBirthMonth As Long
    Dim vBirthDate As Long
    Dim vBirthFont As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.Fk528KM1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vEMachineNumber = cmbEMachineNumber.Text
    
    glngUserName = txtName.Text 'SmackBio
    vRet = frmMain.Fk528KM1.SetUserName(mDeviceKind, _
                                        mMachineNumber, _
                                        vEnrollNumber, _
                                        vEMachineNumber, _
                                        glngUserName)
    If vRet = True Then
        lblMessage.Caption = "SetUserName OK"
    Else
        frmMain.Fk528KM1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.Fk528KM1.EnableDevice mMachineNumber, True
End Sub

Private Sub datEnroll_Reposition()
    If gGetState = True Then Exit Sub
    With datEnroll.Recordset
        datEnroll.Caption = (.AbsolutePosition + 1) & "/" & .RecordCount
        If .RecordCount > 1 Then CurRecView
    End With
End Sub

Private Sub Form_Load()
Dim nLangId As Integer
    nLangId = GetSystemDefaultLangID()
    If nLangId = &H41E Then
        txtName.Font.Name = "Cordia New"
        txtName.Font.Charset = 222
    End If
    cmbBackupNumber.ListIndex = 0
    cmbEMachineNumber.ListIndex = 0
    txtEnrollNumber.Text = 1
    txtCardNumber.Text = 0
    cmbPrivilege.Text = 0
    gGetState = False
    cmbConvType.ListIndex = 0
    cmbDuress.ListIndex = 0
    
    If VarType(glngEnrollData) = vbEmpty Then
        glngEnrollData = gTemplngEnrollData
    End If
    If VarType(glngUserName) = vbEmpty Then
        glngUserName = gTempEnrollName
    End If
   
    With datEnroll
        .DatabaseName = App.Path & "\datEnrollDat.mdb"
        .RecordSource = "select * from tblEnroll"
        .Refresh
        If .Recordset.RecordCount > 0 Then
            .Recordset.MoveLast
            .Recordset.MoveFirst
        End If
    End With
    mMachineNumber = frmMain.gMachineNumber
    mDeviceKind = 0
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub

Private Function CurRecView()
    Dim vStr As String
    Dim vByte() As Byte
    Dim i As Long
    
    With datEnroll.Recordset
        If .RecordCount = 0 Then Exit Function
        If .AbsolutePosition = -1 Then Exit Function
        If !EnrollNumber <= 0 Then Exit Function
        txtEnrollNumber = !EnrollNumber
        cmbBackupNumber = !FingerNumber
        cmbEMachineNumber = !EMachineNumber
        lstEnrollData.Clear
        If !FingerNumber = 10 Then
            lstEnrollData.AddItem !Password
        End If
        If !FingerNumber < 10 Then
            vStr = !FPdata
            vByte = vStr
            For i = 0 To DATASIZE - 1
                glngEnrollData(i) = vByte(i * 5 + 1)
                glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 2)
                glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 3)
                glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 4)
                If vByte(i * 5) = 0 Then
                    glngEnrollData(i) = 0 - glngEnrollData(i)
                End If
                lstEnrollData.AddItem (CStr(glngEnrollData(i)))
            Next
        End If
    End With
End Function
