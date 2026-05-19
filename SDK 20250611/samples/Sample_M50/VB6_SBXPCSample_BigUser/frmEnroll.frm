VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmEnroll 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Manage Enroll Data"
   ClientHeight    =   6900
   ClientLeft      =   3075
   ClientTop       =   1530
   ClientWidth     =   15615
   Icon            =   "frmEnroll.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6900
   ScaleWidth      =   15615
   StartUpPosition =   2  'CenterScreen
   Begin VB.CheckBox chkDupCheck 
      Caption         =   "Fingerprint Duplication Check"
      Height          =   255
      Left            =   12120
      TabIndex        =   55
      Top             =   2520
      Value           =   1  'Checked
      Width           =   3015
   End
   Begin VB.CommandButton cmdDeleteUserPhoto 
      Caption         =   "Delete User Photo"
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
      Left            =   12120
      TabIndex        =   54
      Top             =   5280
      Width           =   3015
   End
   Begin VB.Frame Frame1 
      Caption         =   "User Photo Related"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2295
      Left            =   4440
      TabIndex        =   49
      Top             =   3840
      Width           =   3855
      Begin VB.CommandButton cmdUserPhotoBrowse 
         Caption         =   "..."
         Height          =   375
         Left            =   3120
         TabIndex        =   52
         Top             =   360
         Width           =   495
      End
      Begin VB.TextBox txtUserPhotoFileName 
         Height          =   375
         Left            =   120
         TabIndex        =   51
         Top             =   360
         Width           =   3015
      End
      Begin VB.TextBox txtUserPhotoFileSize 
         Enabled         =   0   'False
         Height          =   375
         Left            =   2640
         TabIndex        =   50
         Top             =   960
         Width           =   1095
      End
      Begin MSComDlg.CommonDialog OpenDlg 
         Left            =   2880
         Top             =   1680
         _ExtentX        =   847
         _ExtentY        =   847
         _Version        =   393216
      End
      Begin VB.Image imgUserPhoto 
         BorderStyle     =   1  'Fixed Single
         Height          =   1335
         Left            =   120
         Stretch         =   -1  'True
         Top             =   840
         Width           =   1815
      End
      Begin VB.Label Label11 
         Caption         =   "Size:"
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
         Left            =   2040
         TabIndex        =   53
         Top             =   960
         Width           =   495
      End
   End
   Begin VB.CommandButton cmdSetUserPhoto 
      Caption         =   "Set User Photo"
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
      Left            =   12120
      TabIndex        =   48
      Top             =   4800
      Width           =   3015
   End
   Begin VB.CommandButton cmdGetUserPhoto 
      Caption         =   "Get User Photo"
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
      Left            =   9120
      TabIndex        =   47
      Top             =   4800
      Width           =   3000
   End
   Begin VB.TextBox txtUserTZ5 
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
      TabIndex        =   45
      Text            =   "0"
      Top             =   5280
      Width           =   735
   End
   Begin VB.TextBox txtUserTZ4 
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
      TabIndex        =   43
      Text            =   "0"
      Top             =   4920
      Width           =   735
   End
   Begin VB.TextBox txtUserTZ3 
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
      TabIndex        =   41
      Text            =   "0"
      Top             =   4560
      Width           =   735
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
      Left            =   9120
      TabIndex        =   26
      ToolTipText     =   "Get All Enroll Data From Device And Save To DataBase"
      Top             =   2880
      Width           =   3000
   End
   Begin VB.TextBox txtUserDepart 
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
      Top             =   5760
      Width           =   735
   End
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
      TabIndex        =   37
      Text            =   "0"
      Top             =   4200
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
      TabIndex        =   36
      Text            =   "0"
      Top             =   3840
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
      Left            =   12120
      TabIndex        =   34
      Top             =   4320
      Width           =   3000
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
      TabIndex        =   33
      Text            =   "0"
      Top             =   3360
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
      TabIndex        =   31
      Top             =   1365
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
      Left            =   12120
      TabIndex        =   29
      Top             =   3360
      Width           =   3000
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
      Left            =   9120
      TabIndex        =   28
      Top             =   3360
      Width           =   3000
   End
   Begin VB.CommandButton cmdSendAllEnrollData 
      Caption         =   "Send All Enroll Data"
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
      Left            =   9120
      TabIndex        =   27
      Top             =   1560
      Visible         =   0   'False
      Width           =   3000
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
      Left            =   12120
      TabIndex        =   25
      ToolTipText     =   "Load All Enroll Data From DataBase And Set To Device"
      Top             =   2880
      Width           =   3000
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
      Left            =   9120
      TabIndex        =   24
      Top             =   4320
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
      Left            =   12120
      TabIndex        =   23
      Top             =   3840
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
      Left            =   12120
      TabIndex        =   22
      ToolTipText     =   "Load All Enroll Data From DataBase And Set To Device"
      Top             =   2040
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
      Left            =   9120
      TabIndex        =   21
      ToolTipText     =   "Get All Enroll Data From Device And Save To DataBase"
      Top             =   2040
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
      Left            =   9120
      TabIndex        =   20
      ToolTipText     =   "Get EnrollData From Device"
      Top             =   1080
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
      Left            =   12120
      TabIndex        =   19
      ToolTipText     =   "Clear EnrollData and LogDat Into Device"
      Top             =   5760
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
      Left            =   10680
      TabIndex        =   18
      Top             =   6360
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
      Left            =   9120
      TabIndex        =   17
      ToolTipText     =   "Get All Enrolled User Info From Device"
      Top             =   3840
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
      Left            =   12120
      TabIndex        =   16
      ToolTipText     =   "Delete Enroll Data Into Device"
      Top             =   1560
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
      Left            =   12120
      TabIndex        =   15
      ToolTipText     =   "Set EnrollData To Device"
      Top             =   1080
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
      Left            =   9120
      TabIndex        =   14
      Top             =   5760
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
      Left            =   5160
      MaxLength       =   25
      TabIndex        =   12
      Top             =   960
      Width           =   3135
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
      Left            =   4680
      TabIndex        =   11
      Top             =   1440
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
      Height          =   480
      Left            =   6735
      TabIndex        =   9
      ToolTipText     =   "Delete All Saved Data From DataBase"
      Top             =   6255
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
      Left            =   4620
      Options         =   0
      ReadOnly        =   0   'False
      RecordsetType   =   1  'Dynaset
      RecordSource    =   ""
      Top             =   6255
      Width           =   2115
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
      Height          =   405
      ItemData        =   "frmEnroll.frx":0456
      Left            =   2400
      List            =   "frmEnroll.frx":0463
      TabIndex        =   7
      Text            =   "cmbPrivilege"
      Top             =   2880
      Width           =   1215
   End
   Begin VB.ListBox lstEnrollData 
      Height          =   1620
      Left            =   4440
      TabIndex        =   4
      Top             =   2160
      Width           =   3900
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
      Top             =   825
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
      Height          =   405
      ItemData        =   "frmEnroll.frx":0470
      Left            =   2400
      List            =   "frmEnroll.frx":0495
      TabIndex        =   0
      Text            =   "cmbBackupNumber"
      Top             =   2400
      Width           =   1215
   End
   Begin VB.Label Label10 
      Caption         =   "UserTZ5 :"
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
      TabIndex        =   46
      Top             =   5280
      Width           =   1095
   End
   Begin VB.Label Label9 
      Caption         =   "UserTZ4 :"
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
      TabIndex        =   44
      Top             =   4920
      Width           =   1095
   End
   Begin VB.Label Label8 
      Caption         =   "UserTZ3 :"
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
      TabIndex        =   42
      Top             =   4560
      Width           =   1095
   End
   Begin VB.Label Label7 
      Caption         =   "User Depart"
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
      TabIndex        =   40
      Top             =   5760
      Width           =   1575
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
      TabIndex        =   38
      Top             =   4200
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
      TabIndex        =   35
      Top             =   3840
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
      TabIndex        =   32
      Top             =   3360
      Width           =   735
   End
   Begin VB.Label lblCardNum 
      Caption         =   "Card or Password Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   615
      Left            =   480
      TabIndex        =   30
      Top             =   1440
      Width           =   1695
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
      Left            =   4440
      TabIndex        =   13
      Top             =   1080
      Width           =   660
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
      Left            =   6360
      TabIndex        =   10
      Top             =   1800
      Width           =   630
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
      Top             =   2895
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
      Left            =   120
      TabIndex        =   6
      Top             =   240
      Width           =   15075
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
      Left            =   4545
      TabIndex        =   5
      Top             =   1800
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
      Height          =   285
      Left            =   480
      TabIndex        =   3
      Top             =   2400
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
      Top             =   885
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
Const DATASIZE_FACE = (27668) / 4 'SmackBio
Const NAMESIZE = 54
Public gGetState As Boolean
Dim glngEnrollData As Variant
Dim gTemplngEnrollData(DATASIZE_FACE) As Long
Dim glngEnrollPData As Long
Dim gbytEnrollData(DATASIZE_FACE * 5) As Byte
Dim mMachineNumber As Long
Dim mDeviceKind As Long
Dim mDeviceVer As Long
Dim mConvKind As Long
Dim glngUserName As Variant
Dim gTempEnrollName(NAMESIZE) As Long
Dim gTempPhotoFileName As String

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
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ClearKeeperData(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "ClearKeeperData OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdDel_Click()
    datEnroll.Database.Execute "delete * from tblEnroll"
    datEnroll.Refresh
End Sub

Private Sub cmdDeleteCompany_Click()
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim vName As String
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEMachineNumber = mMachineNumber
    
    vRet = frmMain.SB100BPC1.SetCompanyName1(mMachineNumber, _
                                           0, _
                                           vName)
    If vRet = True Then
        lblMessage.Caption = "Delete Company Name OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True

End Sub

Private Sub cmdDeleteEnrollData_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vEMachineNumber = mMachineNumber
    vFingerNumber = cmbBackupNumber.Text
    
    vRet = frmMain.SB100BPC1.DeleteEnrollData(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber)
    If vRet = True Then
        lblMessage.Caption = "DeleteEnrollData OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
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
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ModifyDuressFP(mMachineNumber, _
                                            vEnrollNumber, _
                                            vFingerNumber, _
                                            vDuressSetting)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True

End Sub

Private Sub cmdEmptyEnrollData_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
   
    vRet = frmMain.SB100BPC1.EmptyEnrollData(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
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
    
    vEMachineNumber = mMachineNumber
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vFingerNumber = cmbBackupNumber.Text
    vFlag = chkEnable.Value
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.EnableUser(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vFlag)
    If vRet = True Then
        lblMessage.Caption = "Success!"
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
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If

    vRet = frmMain.SB100BPC1.ReadAllUserID(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "ReadAllUserID OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        frmMain.SB100BPC1.EnableDevice mMachineNumber, True
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
NEXT2:
            vRet = frmMain.SB100BPC1.GetAllUserID(mMachineNumber, _
                                                 vEnrollNumber, _
                                                 vEMachineNumber, _
                                                 vFingerNumber, _
                                                 vPrivilege, _
                                                 vEnable)
            If vRet <> True Then Exit Do
            If vFingerNumber >= 50 Then GoTo NEXT2
            vFlag = True
EEE:
            If vFingerNumber = 10 Then vFingerNumber = 15
            vRet = frmMain.SB100BPC1.GetEnrollData(mMachineNumber, _
                                                  vEnrollNumber, _
                                                  vEMachineNumber, _
                                                  vFingerNumber, _
                                                  vPrivilege, _
                                                  glngEnrollData, _
                                                  glngEnrollPData)
           
            If vRet <> True Then
                vFlag = False
                vStr = "GetEnrollData"
                frmMain.SB100BPC1.GetLastError vErrorCode
                vRet = MsgBox(ErrorPrint(vErrorCode) & ": Continue ?", vbYesNoCancel, "GetEnrollData")
                If vRet = vbYes Then
                    GoTo EEE
                ElseIf vRet = vbCancel Then
                    MousePointer = vbDefault
                    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
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
                
                If vFingerNumber = 10 Then      'password
                    !Password = glngEnrollPData
                ElseIf vFingerNumber = 15 Then  'password
                    !Password = glngEnrollPData
                ElseIf vFingerNumber = 11 Then  'card data
                    !Password = glngEnrollPData
                ElseIf vFingerNumber = 17 Then 'face data
                    For i = 0 To DATASIZE_FACE - 1
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
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
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
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vFingerNumber = cmbBackupNumber.Text
    vEMachineNumber = mMachineNumber
    If vFingerNumber = 10 Then vFingerNumber = 15

    
    vRet = frmMain.SB100BPC1.GetEnrollData(mMachineNumber, _
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
            
            txtUserTZ1.Text = (CStr(glngEnrollPData Mod 64)): glngEnrollPData = glngEnrollPData \ 64
            txtUserTZ2.Text = (CStr(glngEnrollPData Mod 64)): glngEnrollPData = glngEnrollPData \ 64
            txtUserTZ3.Text = (CStr(glngEnrollPData Mod 64)): glngEnrollPData = glngEnrollPData \ 64
            txtUserTZ4.Text = (CStr(glngEnrollPData Mod 64)): glngEnrollPData = glngEnrollPData \ 64
            txtUserTZ5.Text = (CStr(glngEnrollPData Mod 64)): glngEnrollPData = glngEnrollPData \ 64
        ElseIf vFingerNumber = 16 Then
            txtUserDepart.Text = CStr(glngEnrollPData)
        ElseIf vFingerNumber = 17 Then
        For i = 0 To DATASIZE_FACE - 1
                lstEnrollData.AddItem (CStr(glngEnrollData(i)))
            Next
        Else
            For i = 0 To DATASIZE - 1
                lstEnrollData.AddItem (CStr(glngEnrollData(i)))
            Next
        End If
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
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
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ReadAllUserID(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "ReadAllUserID OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        frmMain.SB100BPC1.EnableDevice mMachineNumber, True
        Exit Sub
    End If
    
'------ Show all enroll information ----------
    vFlag = False
    i = 0
    lstEnrollData.AddItem ("No.    EnNo   EMNo   Fp   Priv  Enable Duress")
    Do
        vRet = frmMain.SB100BPC1.GetAllUserID(mMachineNumber, _
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
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetName_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i As Long
    Dim vName As String
    
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vEMachineNumber = mMachineNumber
    
    vRet = frmMain.SB100BPC1.GetUserName1(mMachineNumber, _
                                        vEnrollNumber, _
                                        vName)
    If vRet = True Then
        'FontForFK1.SetTextBitmap mDeviceKind, vName, glngUserName 'SmackBio
        txtName.Text = vName 'vName 'SmackBio
        lblMessage.Caption = "GetUserName OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetUserPhoto_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim vEnrollNumber As Long
    Dim i As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        GoTo 1:
    End If
    vEnrollNumber = Val(txtEnrollNumber.Text)
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetUserPhotoData"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddLong strXML, "UserID", vEnrollNumber
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = False Then
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        GoTo 2:
    End If
    lblMessage.Caption = "GetUserPhotoData OK"
    ReDim photoData(gCompPhotoSize) As Byte
    vRet = frmMain.SB100BPC1.XML_ParseBinaryByte(strXML, "PhotoData", photoData(0), gCompPhotoSize)
    If vRet = True Then
        Open gTempPhotoFileName For Binary Access Write Lock Read Write As #1
        For i = 0 To gCompPhotoSize - 1
            Put #1, , photoData(i)
        Next i
        Close #1
    Else
        lblMessage.Caption = "GetUserPhotoData - XML Parse Error!"
        GoTo 2:
    End If
1:
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, True)
    On Error GoTo 2:
    imgUserPhoto.Picture = LoadPicture(gTempPhotoFileName)
    txtUserPhotoFileName = gTempPhotoFileName
    Exit Sub
2:
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, True)
    imgUserPhoto.Picture = LoadPicture()
    txtUserPhotoFileName = ""
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
    vEMachineNumber = mMachineNumber
    vFingerNumber = cmbBackupNumber.Text
    vMachinePrivilege = cmbPrivilege.Text
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ModifyPrivilege(mMachineNumber, _
                                            vEnrollNumber, _
                                            vEMachineNumber, _
                                            vFingerNumber, _
                                            vMachinePrivilege)
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSendAllEnrollData_Click()
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
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
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
                If vFingerNumber = 17 Then
                    vStr = !FPdata
                    vByte = vStr
                    For i = 0 To DATASIZE_FACE - 1
                        glngEnrollData(i) = vByte(i * 5 + 1)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 2)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 3)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 4)
                        If vByte(i * 5) = 0 Then
                            glngEnrollData(i) = 0 - glngEnrollData(i)
                        End If
                    Next
                ElseIf vFingerNumber < 10 Then
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
                '- DATA Conv ------------------------------------------------------------
                'vConvResult = FkCnv4001.FkConvGeneral(vEnrollNumber, _
                '                           mMachineNumber, _
                '                           vFingerNumber, _
                '                           glngEnrollData, _
                '                           glngEnrollData, _
                '                           mConvKind)
                '
                'If vConvResult <> 0 Then
                '    vFlag = False
                '    vStr = "Conv Error"
                '    vRet = MsgBox(vStr & ": Continue ?", vbYesNoCancel, "ConvShToArm")
                '    If vRet = vbYes Then GoTo LLL
                '    If vRet = vbCancel Then GoTo EEE
                'End If
                '-------------------------------------------------------------
            
                vRet = frmMain.SB100BPC1.SetEnrollData(mMachineNumber, _
                                                      vEnrollNumber, _
                                                      vEMachineNumber, _
                                                      vFingerNumber + 20, _
                                                      vPrivilege, _
                                                      glngEnrollData, _
                                                      glngEnrollPData)
                If vRet <> True Then
                    vFlag = False
                    vStr = "SetEnrollData"
                    frmMain.SB100BPC1.GetLastError vErrorCode
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
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetAllEnrollData_Click()
    Dim vEnrollNumber As Long
    Dim vEMachineNumber As Long
    Dim vFingerNumber As Long
    Dim vFingerNumber2 As Long
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
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
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
                ElseIf vFingerNumber = 17 Then
                vStr = !FPdata
                    vByte = vStr
                    For i = 0 To DATASIZE_FACE - 1
                        glngEnrollData(i) = vByte(i * 5 + 1)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 2)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 3)
                        glngEnrollData(i) = glngEnrollData(i) * 256 + vByte(i * 5 + 4)
                        If vByte(i * 5) = 0 Then
                            glngEnrollData(i) = 0 - glngEnrollData(i)
                        End If
                    Next
                
                End If
                vFingerNumber2 = vFingerNumber
                If vFingerNumber2 < 10 And chkDupCheck.Value = 0 Then
                    vFingerNumber2 = vFingerNumber2 + 20
                End If
FFF:
                '- DATA Conv ---------------------------------------------
                'vConvResult = FkCnv4001.FkConvGeneral(vEnrollNumber, _
                '                           mMachineNumber, _
                '                           vFingerNumber, _
                '                           glngEnrollData, _
                '                           glngEnrollData, _
                '                           mConvKind)
                '
                'If vConvResult <> 0 Then
                '    vFlag = False
                '    vStr = "Conv Error"
                '    vRet = MsgBox(vStr & ": Continue ?", vbYesNoCancel, "ConvShToArm")
                '    If vRet = vbYes Then GoTo LLL
                '    If vRet = vbCancel Then GoTo EEE
                'End If
                '-------------------------------------------------------------
                vRet = frmMain.SB100BPC1.SetEnrollData(mMachineNumber, _
                                                      vEnrollNumber, _
                                                      vEMachineNumber, _
                                                      vFingerNumber2, _
                                                      vPrivilege, _
                                                      glngEnrollData, _
                                                      glngEnrollPData)
                If vRet <> True Then
                    vFlag = False
                    vStr = "SetEnrollData"
                    frmMain.SB100BPC1.GetLastError vErrorCode
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
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetCompany_Click()
    Dim vEMachineNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim vName As String
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEMachineNumber = mMachineNumber
    
    vName = txtName.Text 'SmackBio
    'vRet = FontForFK1.GetTextBitmap(FK_Company, txtName.Text, glngUserName) 'SmackBio
    
    vRet = frmMain.SB100BPC1.SetCompanyName1(mMachineNumber, _
                                           1, _
                                           vName)
    If vRet = True Then
        lblMessage.Caption = "Set Company Name OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True

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
    Dim vUserTZ3 As Byte
    Dim vUserTZ4 As Byte
    Dim vUserTZ5 As Byte
    Dim vUserGroup As Byte
    Dim i As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vCardNumber = Val(txtCardNumber.Text)
    vFingerNumber = cmbBackupNumber.Text
    vPrivilege = cmbPrivilege.Text
    vEMachineNumber = mMachineNumber
    vUserTZ1 = Val(txtUserTZ1.Text)
    vUserTZ2 = Val(txtUserTZ2.Text)
    vUserTZ3 = Val(txtUserTZ3.Text)
    vUserTZ4 = Val(txtUserTZ4.Text)
    vUserTZ5 = Val(txtUserTZ5.Text)
    
        ' Card Number valid
    If vCardNumber <> 0 Then
        If vFingerNumber = 11 Then
            glngEnrollPData = vCardNumber
        End If
    End If
    
    If vFingerNumber = 14 Then
        glngEnrollPData = vUserTZ5
        glngEnrollPData = glngEnrollPData * 64 + vUserTZ4
        glngEnrollPData = glngEnrollPData * 64 + vUserTZ3
        glngEnrollPData = glngEnrollPData * 64 + vUserTZ2
        glngEnrollPData = glngEnrollPData * 64 + vUserTZ1
    End If
    If vFingerNumber = 10 Then vFingerNumber = 15
    If vFingerNumber = 15 Then
        i = Len(txtCardNumber.Text)
        If i > 6 Then i = 6
        glngEnrollPData = 0
        While i > 0
            glngEnrollPData = glngEnrollPData * 16 + Asc(Mid(txtCardNumber.Text, i, 1)) - Asc("0") + 1
            i = i - 1
        Wend
    End If
    
    If vFingerNumber = 16 Then
        glngEnrollPData = Val(txtUserDepart)
    End If
    '- DATA Conv ------------------------------------------------------------
    'vConvResult = FkCnv4001.FkConvGeneral(vEnrollNumber, _
    '                                    mMachineNumber, _
    '                                    vFingerNumber, _
    '                                    glngEnrollData, _
    '                                    glngEnrollData, _
    '                                    mConvKind)
    '
    'If vConvResult <> 0 Then
    '    lblMessage.Caption = "Conv Error"
    '    Exit Sub
    'End If
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If

    vRet = frmMain.SB100BPC1.SetEnrollData(mMachineNumber, _
                                          vEnrollNumber, _
                                          vEMachineNumber, _
                                          vFingerNumber, _
                                          vPrivilege, _
                                          glngEnrollData, _
                                          glngEnrollPData)
                            
    If vRet = True Then
        lblMessage.Caption = "SetEnrollData OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
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
    Dim vName As String
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vEnrollNumber = Val(txtEnrollNumber.Text)
    vEMachineNumber = mMachineNumber
    
    vName = txtName.Text 'SmackBio
    'vRet = FontForFK1.GetTextBitmap(mDeviceKind, txtName.Text, glngUserName) 'SmackBio
    vRet = frmMain.SB100BPC1.SetUserName1(mMachineNumber, _
                                        vEnrollNumber, _
                                        vName)
    If vRet = True Then
        lblMessage.Caption = "SetUserName OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSetUserPhoto_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim vEnrollNumber As Long
    Dim photoData() As Byte
    Dim i As Long
    Dim vUserPhotoFileSize As Long
    
    vUserPhotoFileSize = Val(txtUserPhotoFileSize)
    If vUserPhotoFileSize = 0 Then Exit Sub
    If vUserPhotoFileSize > gCompPhotoSize Then
        MsgBox "Image file is too big!"
        Exit Sub
    End If
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        GoTo 1:
    End If
    vEnrollNumber = Val(txtEnrollNumber.Text)
    
    ReDim photoData(vUserPhotoFileSize) As Byte
    Open txtUserPhotoFileName.Text For Binary Access Read Lock Read Write As #1
    Get #1, , photoData
    Close #1
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetUserPhotoData"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddLong strXML, "UserID", vEnrollNumber
    frmMain.SB100BPC1.XML_AddBinaryByte strXML, "PhotoData", photoData(0), vUserPhotoFileSize
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = False Then
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        GoTo 1:
    End If
    lblMessage.Caption = "SetUserPhotoData OK"
    
1:
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, True)
    Exit Sub
End Sub
Private Sub cmdDeleteUserPhoto_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim vEnrollNumber As Long
    Dim i As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        GoTo 1:
    End If
    vEnrollNumber = Val(txtEnrollNumber.Text)
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetUserPhotoData"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddLong strXML, "UserID", vEnrollNumber
    ' Do not make "PhotoData" tag to delete user photo
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = False Then
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
        GoTo 1:
    End If
    lblMessage.Caption = "DeleteUserPhotoData OK"
    
1:
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, True)
    Exit Sub
End Sub

Private Sub cmdUserPhotoBrowse_Click()
    Dim nRet As Long
    Dim fs, f
    OpenDlg.ShowSave
    txtUserPhotoFileName = OpenDlg.FileName
    If txtUserPhotoFileName = "" Then GoTo 1:
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set f = fs.GetFile(txtUserPhotoFileName)
    If f = Empty Then
        txtUserPhotoFileSize = ""
        GoTo 1:
    End If
    nRet = f.Size
    txtUserPhotoFileSize = nRet
    On Error Resume Next
    imgUserPhoto.Picture = LoadPicture(txtUserPhotoFileName)
    Exit Sub
1:
    imgUserPhoto.Picture = LoadPicture()
End Sub


Private Sub Form_Load()
    gTempPhotoFileName = "C:\TempPhoto.jpg"
    Dim nLangId As Integer
    nLangId = GetSystemDefaultLangID()
    If nLangId = &H41E Then
        txtName.Font.Name = "Cordia New"
        txtName.Font.Charset = 222
    End If
    If nLangId = &H429 Then
        txtName.Font.Name = "Arial"
        txtName.Font.Charset = 178
    End If
    cmbBackupNumber.ListIndex = 0
    txtEnrollNumber.Text = 1
    txtCardNumber.Text = 0
    cmbPrivilege.Text = 0
    gGetState = False
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
    'frmMain.SB100BPC1.GetModel mMachineNumber, mDeviceKind, mDeviceVer
    mConvKind = 0
    'If mDeviceKind = FK_328 Then
    '    lblMessage.Caption = "FK328KM Device Ver " & Format(mDeviceVer)
    '    mConvKind = 1
    '    cmdSendAllEnrollData.Enabled = False
    '    cmdSetCompany.Enabled = False
    '    cmdDeleteCompany.Enabled = False
    'ElseIf mDeviceKind = FK_338 Then
    '    lblMessage.Caption = "FK338KM Device Ver " & Format(mDeviceVer)
    '    mConvKind = 4
    '    cmdSendAllEnrollData.Enabled = False
    '    cmdSetCompany.Enabled = False
    '    cmdDeleteCompany.Enabled = False
    'ElseIf mDeviceKind = FK_528 Then
    '    lblMessage.Caption = "FK528KM Device Ver " & Format(mDeviceVer)
    '    mConvKind = 2
    'ElseIf mDeviceKind = FK_526 Then
    '    lblMessage.Caption = "FK526EA Device Ver " & Format(mDeviceVer)
    '    mConvKind = 3
    'ElseIf mDeviceKind = 0 Then
    '    lblMessage.Caption = " Device Invalid "
    '    mConvKind = 0
    'End If
       
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
        ' = !EMachineNumber
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

Private Sub txtName_Change()
    ' SH3 Kind = 1
    ' FontForFK1.GetTextBitmap 1, txtName.Text, glngUserName
    ' Arm 44b0 Kind = 2
    ' FontForFK1.GetTextBitmap 2, txtName.Text, glngUserName 'SmackBio
End Sub
