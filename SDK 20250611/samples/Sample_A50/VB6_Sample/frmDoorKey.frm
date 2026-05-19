VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmDoorKey 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Door Key Control"
   ClientHeight    =   12015
   ClientLeft      =   45
   ClientTop       =   405
   ClientWidth     =   8910
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   12015
   ScaleWidth      =   8910
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdExit 
      Caption         =   "Exit"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   435
      Left            =   6870
      TabIndex        =   36
      Top             =   11460
      Width           =   1785
   End
   Begin VB.Frame Frame5 
      Caption         =   "6-Unlock Group"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3495
      Left            =   4920
      TabIndex        =   32
      Top             =   7800
      Width           =   3735
      Begin VB.ComboBox cmbUnlockGroup 
         Height          =   315
         ItemData        =   "frmDoorKey.frx":0000
         Left            =   1440
         List            =   "frmDoorKey.frx":0025
         Style           =   2  'Dropdown List
         TabIndex        =   40
         Top             =   840
         Visible         =   0   'False
         Width           =   855
      End
      Begin VB.CommandButton cmdWriteUnlockGroup 
         Caption         =   "Write"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2040
         TabIndex        =   35
         Top             =   3000
         Width           =   1095
      End
      Begin VB.CommandButton cmdReadUnlockGroup 
         Caption         =   "Read"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   600
         TabIndex        =   34
         Top             =   3000
         Width           =   1095
      End
      Begin MSFlexGridLib.MSFlexGrid gridUnlockGroupSet 
         Height          =   2505
         Left            =   240
         TabIndex        =   33
         Top             =   360
         Width           =   3255
         _ExtentX        =   5741
         _ExtentY        =   4419
         _Version        =   393216
         Rows            =   6
         Cols            =   4
      End
   End
   Begin VB.Frame Frame4 
      Caption         =   "5-User Access Group"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   4845
      Left            =   4920
      TabIndex        =   28
      Top             =   2850
      Width           =   3735
      Begin VB.ComboBox cmbGroup 
         Height          =   315
         ItemData        =   "frmDoorKey.frx":004B
         Left            =   240
         List            =   "frmDoorKey.frx":0070
         Style           =   2  'Dropdown List
         TabIndex        =   39
         Top             =   1440
         Width           =   840
      End
      Begin VB.CommandButton cmdWriteGroup 
         Caption         =   "Write"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2160
         TabIndex        =   31
         Top             =   4260
         Width           =   1095
      End
      Begin VB.CommandButton cmdReadGroup 
         Caption         =   "Read"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   600
         TabIndex        =   30
         Top             =   4260
         Width           =   1095
      End
      Begin MSFlexGridLib.MSFlexGrid gridUserGroupSet 
         Height          =   3645
         Left            =   240
         TabIndex        =   29
         Top             =   480
         Width           =   3255
         _ExtentX        =   5741
         _ExtentY        =   6429
         _Version        =   393216
         Rows            =   11
      End
   End
   Begin VB.Frame Frame3 
      Caption         =   "4-User Accept Mode"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1455
      Left            =   4920
      TabIndex        =   23
      Top             =   1200
      Width           =   3735
      Begin VB.CommandButton cmdWriteUserMode 
         Caption         =   "Write"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2160
         TabIndex        =   27
         Top             =   960
         Width           =   1095
      End
      Begin VB.CommandButton cmdReadUserMode 
         Caption         =   "Read"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   600
         TabIndex        =   26
         Top             =   960
         Width           =   1095
      End
      Begin VB.TextBox txtUserMode 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   1800
         TabIndex        =   24
         Top             =   480
         Width           =   1575
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         Caption         =   "UserMode :"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   225
         Left            =   120
         TabIndex        =   25
         Top             =   525
         Width           =   1575
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "1-General"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3975
      Left            =   240
      TabIndex        =   17
      Top             =   1200
      Width           =   4335
      Begin VB.ComboBox cmbLocation 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   420
         ItemData        =   "frmDoorKey.frx":0096
         Left            =   2520
         List            =   "frmDoorKey.frx":00A0
         TabIndex        =   47
         Text            =   "Master"
         Top             =   2800
         Width           =   1575
      End
      Begin VB.ComboBox cmbAntipassNo 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   420
         ItemData        =   "frmDoorKey.frx":00B3
         Left            =   2520
         List            =   "frmDoorKey.frx":00BD
         TabIndex        =   46
         Text            =   "Group 1"
         Top             =   2320
         Width           =   1575
      End
      Begin VB.ComboBox cmbUseAntipass 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   420
         ItemData        =   "frmDoorKey.frx":00D3
         Left            =   2535
         List            =   "frmDoorKey.frx":00DD
         TabIndex        =   44
         Text            =   "Use"
         Top             =   1840
         Width           =   1575
      End
      Begin VB.TextBox txtOpenTimeout 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   420
         Left            =   2520
         TabIndex        =   43
         Text            =   "10"
         Top             =   1360
         Width           =   1575
      End
      Begin VB.TextBox txtLockReleaseTime 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   420
         Left            =   2520
         TabIndex        =   42
         Text            =   "5"
         Top             =   880
         Width           =   1575
      End
      Begin VB.ComboBox cmbSensorType 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   420
         ItemData        =   "frmDoorKey.frx":00EE
         Left            =   2520
         List            =   "frmDoorKey.frx":00FB
         TabIndex        =   41
         Text            =   "N.O."
         Top             =   400
         Width           =   1575
      End
      Begin VB.CommandButton cmdGet 
         Caption         =   "Read"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   600
         TabIndex        =   22
         Top             =   3360
         Width           =   1215
      End
      Begin VB.CommandButton cmdSet 
         Caption         =   "Write"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2520
         TabIndex        =   21
         Top             =   3360
         Width           =   1215
      End
      Begin VB.Label Label11 
         Alignment       =   1  'Right Justify
         Caption         =   "Use Antipass:"
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
         Left            =   240
         TabIndex        =   45
         Top             =   1920
         Width           =   2175
      End
      Begin VB.Label Label10 
         Alignment       =   1  'Right Justify
         Caption         =   "Antipass Locale :"
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
         Left            =   225
         TabIndex        =   38
         Top             =   2880
         Width           =   2175
      End
      Begin VB.Label Label9 
         Alignment       =   1  'Right Justify
         Caption         =   "AntipassNo :"
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
         Left            =   225
         TabIndex        =   37
         Top             =   2400
         Width           =   2175
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Lock Release Time :"
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
         Left            =   225
         TabIndex        =   20
         Top             =   960
         Width           =   2175
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         Caption         =   "Door Open Timeout :"
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
         Left            =   225
         TabIndex        =   19
         Top             =   1440
         Width           =   2175
      End
      Begin VB.Label Label3 
         Alignment       =   1  'Right Justify
         Caption         =   "DoorSensor Type :"
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
         Left            =   225
         TabIndex        =   18
         Top             =   480
         Width           =   2175
      End
   End
   Begin VB.Frame Timezone 
      Caption         =   "2-Access Timezone"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2655
      Left            =   240
      TabIndex        =   8
      Top             =   5280
      Width           =   4335
      Begin VB.CommandButton cmdWriteTz 
         Caption         =   "Write"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2400
         TabIndex        =   16
         Top             =   2040
         Width           =   1215
      End
      Begin VB.CommandButton cmdReadTz 
         Caption         =   "Read"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   720
         TabIndex        =   15
         Top             =   2040
         Width           =   1215
      End
      Begin VB.TextBox txtTz1 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2040
         TabIndex        =   11
         Top             =   600
         Width           =   1575
      End
      Begin VB.TextBox txtTz2 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2040
         TabIndex        =   10
         Top             =   1080
         Width           =   1575
      End
      Begin VB.TextBox txtTz3 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2040
         TabIndex        =   9
         Top             =   1560
         Width           =   1575
      End
      Begin VB.Label Label4 
         Alignment       =   1  'Right Justify
         Caption         =   "Timezone1 :"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   345
         TabIndex        =   14
         Top             =   600
         Width           =   1575
      End
      Begin VB.Label Label5 
         Alignment       =   1  'Right Justify
         Caption         =   "Timezone2 :"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   345
         TabIndex        =   13
         Top             =   1080
         Width           =   1575
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Timezone3 :"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   345
         TabIndex        =   12
         Top             =   1560
         Width           =   1575
      End
   End
   Begin VB.TextBox txtDoorNumber 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4800
      TabIndex        =   7
      Text            =   "0"
      Top             =   720
      Width           =   1575
   End
   Begin VB.Frame Frame1 
      Caption         =   "3-Door Open Timezone"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3495
      Left            =   240
      TabIndex        =   1
      Top             =   8040
      Width           =   4335
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
         Height          =   405
         Index           =   0
         Left            =   2280
         TabIndex        =   3
         Top             =   3000
         Width           =   1755
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
         Height          =   405
         Index           =   0
         Left            =   360
         TabIndex        =   2
         Top             =   3000
         Width           =   1650
      End
      Begin MSComCtl2.DTPicker timePicker 
         Height          =   300
         Left            =   960
         TabIndex        =   4
         Top             =   1800
         Visible         =   0   'False
         Width           =   795
         _ExtentX        =   1402
         _ExtentY        =   529
         _Version        =   393216
         CustomFormat    =   "HH:mm"
         Format          =   102891523
         UpDown          =   -1  'True
         CurrentDate     =   39795
      End
      Begin MSFlexGridLib.MSFlexGrid gridDoorOpenTime 
         Height          =   2535
         Left            =   120
         TabIndex        =   5
         Top             =   360
         Width           =   4095
         _ExtentX        =   7223
         _ExtentY        =   4471
         _Version        =   393216
         Rows            =   9
         Cols            =   3
         AllowBigSelection=   0   'False
      End
   End
   Begin VB.Label Label8 
      Caption         =   "Door Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2760
      TabIndex        =   6
      Top             =   720
      Width           =   2055
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
      TabIndex        =   0
      Top             =   105
      Width           =   8625
   End
End
Attribute VB_Name = "frmDoorKey"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim bShowProcessing As Boolean
Option Explicit
Dim mMachineNumber As Long
Dim DbOpenTimeArray(4 * 8) As Byte
Dim DbUserGroupArray(1 * 10) As Byte
Dim DbUnlockGroupArray(3 * 5) As Byte
Private Sub DbDoorOpenTimeDraw()
    Dim r As Long, c As Long
    bShowProcessing = True
    timePicker.Visible = False
    gridDoorOpenTime.Enabled = False
    For r = 0 To 7
        gridDoorOpenTime.Row = r + 1
        For c = 0 To 1
            gridDoorOpenTime.Col = c + 1
            If c Mod 2 = 0 Then
                timePicker.Hour = DbOpenTimeArray(r * 4 + 0)
                timePicker.Minute = DbOpenTimeArray(r * 4 + 1)
            Else
                timePicker.Hour = DbOpenTimeArray(r * 4 + 2)
                timePicker.Minute = DbOpenTimeArray(r * 4 + 3)
            End If
            gridDoorOpenTime.Text = Format(timePicker.Value, "hh:mm")
        Next
    Next
    bShowProcessing = False
    gridDoorOpenTime.Enabled = True
End Sub

Private Sub DbUserGroupDraw()
    Dim r As Long, c As Long
    bShowProcessing = True
    cmbGroup.Visible = False
    gridUserGroupSet.Enabled = False
    For r = 0 To 9
        gridUserGroupSet.Row = r + 1
        gridUserGroupSet.Col = 1
        cmbGroup.ListIndex = DbUserGroupArray(r)
        gridUserGroupSet.Text = cmbGroup.Text
    Next
    bShowProcessing = False
    gridUserGroupSet.Enabled = True
End Sub

Private Sub DbUnlockGroupDraw()
    Dim r As Long, c As Long
    bShowProcessing = True
    cmbUnlockGroup.Visible = False
    gridUserGroupSet.Enabled = False
    For r = 0 To 4
        For c = 0 To 2
        gridUnlockGroupSet.Row = r + 1
        gridUnlockGroupSet.Col = c + 1
        cmbUnlockGroup.ListIndex = DbUnlockGroupArray(r * 3 + c)
        gridUnlockGroupSet.Text = cmbUnlockGroup.Text
        Next
    Next
    bShowProcessing = False
    gridUserGroupSet.Enabled = True
End Sub

Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True
End Sub

Private Sub cmdGet_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 1
    
    'make xml
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", lInfo
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        cmbSensorType.ListIndex = frmMain.SB100BPC1.XML_ParseInt(strXML, "DoorSensorType")
        txtLockReleaseTime = frmMain.SB100BPC1.XML_ParseInt(strXML, "LockReleaseTime")
        txtOpenTimeout = frmMain.SB100BPC1.XML_ParseInt(strXML, "DoorOpenTimeout")
        cmbUseAntipass.ListIndex = 1 - frmMain.SB100BPC1.XML_ParseInt(strXML, "UseAntipass")
        cmbAntipassNo.ListIndex = frmMain.SB100BPC1.XML_ParseInt(strXML, "AntipassNo")
        cmbLocation.ListIndex = frmMain.SB100BPC1.XML_ParseInt(strXML, "Locale")
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdRead_Click(Index As Integer)
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 3
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "reqeust"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", lInfo
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If bRet = True Then
        bRet = frmMain.SB100BPC1.XML_ParseBinaryByte(strXML, "DoorOpenTimeBinary", DbOpenTimeArray(0), 4 * 8)
        If bRet = True Then
            DbDoorOpenTimeDraw
            lblMessage.Caption = "GetDoorParam(3-DoorOpenTime) = Success!"
        Else
            lblMessage.Caption = "XML Parse Error!"
        End If
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdReadGroup_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 5
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", lInfo
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        frmMain.SB100BPC1.XML_ParseBinaryByte strXML, "GroupBinary", DbUserGroupArray(0), 10
        DbUserGroupDraw

        lblMessage.Caption = "GetDoorParam(5-UserGroup) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdReadTz_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim DbDataArray(3) As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 2
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", lInfo
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        txtTz1 = frmMain.SB100BPC1.XML_ParseInt(strXML, "Timezone1")
        txtTz2 = frmMain.SB100BPC1.XML_ParseInt(strXML, "Timezone2")
        txtTz3 = frmMain.SB100BPC1.XML_ParseInt(strXML, "Timezone3")
        lblMessage.Caption = "GetDoorParam(2-Timezone) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdReadUnlockGroup_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 6
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", lInfo
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        frmMain.SB100BPC1.XML_ParseBinaryByte strXML, "UnlockGroupBinary", DbUnlockGroupArray(0), 3 * 5
        DbUnlockGroupDraw
                
        lblMessage.Caption = "GetDoorParam(6-UnlockGroup) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdReadUserMode_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim DbDataArray(1) As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 4
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", lInfo
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        txtUserMode = frmMain.SB100BPC1.XML_ParseInt(strXML, "UserMode")
        lblMessage.Caption = "GetDoorParam(4-UserMode) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdSet_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim DbDataArray(5) As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 1
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", lInfo
    
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorSensorType", Val(cmbSensorType.ListIndex)
    frmMain.SB100BPC1.XML_AddInt strXML, "LockReleaseTime", Val(txtLockReleaseTime)
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorOpenTimeout", Val(txtOpenTimeout)
    frmMain.SB100BPC1.XML_AddInt strXML, "UseAntipass", Val(1 - cmbUseAntipass.ListIndex)
    frmMain.SB100BPC1.XML_AddInt strXML, "AntipassNo", Val(cmbAntipassNo.ListIndex)
    frmMain.SB100BPC1.XML_AddInt strXML, "Locale", Val(cmbLocation.ListIndex)

    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub


Private Sub cmdWrite_Click(Index As Integer)
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 3
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", 3
    frmMain.SB100BPC1.XML_AddBinaryByte strXML, "DoorOpenTimeBinary", DbOpenTimeArray(0), 4 * 8
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        DbDoorOpenTimeDraw
                
        lblMessage.Caption = "SetDoorParam(3-DoorOpenTime) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdWriteGroup_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 5
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", 5
    frmMain.SB100BPC1.XML_AddBinaryByte strXML, "GroupBinary", DbUserGroupArray(0), 10
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        DbUserGroupDraw
                
        lblMessage.Caption = "SetDoorParam(5-UserGroup) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdWriteTz_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 2
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", 2
    frmMain.SB100BPC1.XML_AddInt strXML, "Timezone1", Val(txtTz1.Text)
    frmMain.SB100BPC1.XML_AddInt strXML, "Timezone2", Val(txtTz2.Text)
    frmMain.SB100BPC1.XML_AddInt strXML, "Timezone3", Val(txtTz3.Text)
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        
        lblMessage.Caption = "SetDoorParam(2-Timezone) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdWriteUnlockGroup_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 6
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", 6
    frmMain.SB100BPC1.XML_AddBinaryByte strXML, "UnlockGroupBinary", DbUnlockGroupArray(0), 3 * 5
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        DbUnlockGroupDraw
                
        lblMessage.Caption = "SetDoorParam(6-UnlockGroup) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdWriteUserMode_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lDoorNumber As Long
    Dim lInfo As Long
    Dim strXML As String
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    lDoorNumber = Val(txtDoorNumber.Text)
    lInfo = 4
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "SubType", 4
    frmMain.SB100BPC1.XML_AddInt strXML, "UserMode", Val(txtUserMode.Text)
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        
        lblMessage.Caption = "SetDoorParam(4-UserMode) = Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True

End Sub


Private Sub Form_Load()
    Dim OpenTimeColText As Variant
    Dim UserGroupColText As Variant
    Dim UnlockGroupColText As Variant
    Dim i As Long
    
    mMachineNumber = frmMain.gMachineNumber
    
    bShowProcessing = False
    OpenTimeColText = Array("No.", "Start", "End")
    UserGroupColText = Array("No.", "Group")
    UnlockGroupColText = Array("No.", "Group1", "Group2", "Group3")
    
    cmbSensorType.ListIndex = 1
    cmbUseAntipass.ListIndex = 0
    cmbLocation.ListIndex = 0
    cmbAntipassNo.ListIndex = 0
    
'======================================================================
    With gridDoorOpenTime
        .Clear
        .Row = 0
        For i = 0 To 2
            .Col = i
            .Text = OpenTimeColText(i)
            .ColWidth(i) = 810
            .ColAlignment(i) = flexAlignCenterCenter
        Next
        .ColWidth(0) = 500
        .RowHeight(0) = 300
        For i = 1 To 8
            .Row = i
            .Col = 0
            .Text = i
            .RowHeight(i) = timePicker.Height
        Next
    End With
    DbDoorOpenTimeDraw
'======================================================================

    With gridUserGroupSet
    .Clear
    .Row = 0
    For i = 0 To 1
        .Col = i
        .Text = UserGroupColText(i)
        .ColWidth(i) = 1000
        .ColAlignment(i) = flexAlignCenterCenter
    Next
    .ColWidth(0) = 500
    .RowHeight(0) = 300
    For i = 1 To 10
        .Row = i
        .Col = 0
        .Text = i
        .RowHeight(i) = cmbGroup.Height
    Next
    End With
    DbUserGroupDraw
'======================================================================
    With gridUnlockGroupSet
    .Clear
    .Row = 0
    For i = 0 To 3
        .Col = i
        .Text = UnlockGroupColText(i)
        .ColWidth(i) = 810
        .ColAlignment(i) = flexAlignCenterCenter
    Next
    .ColWidth(0) = 500
    .RowHeight(0) = 300
    For i = 1 To 5
        .Row = i
        .Col = 0
        .Text = i
        .RowHeight(i) = cmbUnlockGroup.Height
    Next
    End With
    DbUnlockGroupDraw
    mMachineNumber = frmMain.gMachineNumber
End Sub

Private Sub Form_Unload(Cancel As Integer)
    gridDoorOpenTime_LeaveCell
    gridUserGroupSet_LeaveCell
    gridUnlockGroupSet_LeaveCell
    Unload Me
    frmMain.Visible = True
End Sub



Private Sub gridDoorOpenTime_Click()
    gridDoorOpenTime_EnterCell
End Sub

Private Sub gridDoorOpenTime_EnterCell()
    Dim c As Integer, r As Integer
    If Me.Visible And Not bShowProcessing Then
        If Not timePicker.Visible Then timePicker.Visible = True
        r = gridDoorOpenTime.Row - 1
        c = gridDoorOpenTime.Col - 1
        timePicker.Left = gridDoorOpenTime.Left + gridDoorOpenTime.CellLeft
        timePicker.Top = gridDoorOpenTime.Top + gridDoorOpenTime.CellTop
        If c Mod 2 = 0 Then
            timePicker.Hour = DbOpenTimeArray(r * 4 + 0)
            timePicker.Minute = DbOpenTimeArray(r * 4 + 1)
        Else
            timePicker.Hour = DbOpenTimeArray(r * 4 + 2)
            timePicker.Minute = DbOpenTimeArray(r * 4 + 3)
        End If
        timePicker.Second = 0
    End If

End Sub

Private Sub gridDoorOpenTime_LeaveCell()
    Dim c As Integer, r As Integer
    If Me.Visible And Not bShowProcessing Then
        r = gridDoorOpenTime.Row - 1
        c = gridDoorOpenTime.Col - 1
        If r < 0 Or c < 0 Then Exit Sub
        If c Mod 2 = 0 Then
            DbOpenTimeArray(r * 4 + 0) = timePicker.Hour
            DbOpenTimeArray(r * 4 + 1) = timePicker.Minute
        Else
            DbOpenTimeArray(r * 4 + 2) = timePicker.Hour
            DbOpenTimeArray(r * 4 + 3) = timePicker.Minute
        End If
        gridDoorOpenTime.Text = Format(timePicker.Value, "hh:mm")
    End If
End Sub

Private Sub gridDoorOpenTime_Scroll()
    gridDoorOpenTime_LeaveCell
End Sub

Private Sub gridUnlockGroupSet_Click()
    gridUnlockGroupSet_EnterCell

End Sub

Private Sub gridUnlockGroupSet_EnterCell()
    Dim c As Integer, r As Integer
    If Me.Visible And Not bShowProcessing Then
        r = gridUnlockGroupSet.Row - 1
        c = gridUnlockGroupSet.Col - 1
        If Not cmbUnlockGroup.Visible Then cmbUnlockGroup.Visible = True
'        cmbVM.Visible = False
        cmbUnlockGroup.Left = gridUnlockGroupSet.Left + gridUnlockGroupSet.CellLeft
        cmbUnlockGroup.Top = gridUnlockGroupSet.Top + gridUnlockGroupSet.CellTop
        If DbUnlockGroupArray(r * 3 + c) = 255 Then
            cmbUnlockGroup.ListIndex = 11
        Else
            cmbUnlockGroup.ListIndex = DbUnlockGroupArray(r * 3 + c)
        End If
    End If
End Sub

Private Sub gridUnlockGroupSet_LeaveCell()
    Dim c As Integer, r As Integer
    If Me.Visible And Not bShowProcessing Then
        r = gridUnlockGroupSet.Row - 1
        c = gridUnlockGroupSet.Col - 1
        If r < 0 Or c < 0 Then Exit Sub
        If cmbUnlockGroup.ListIndex = 11 Then
            DbUnlockGroupArray(r * 3 + c) = 255
        Else
            DbUnlockGroupArray(r * 3 + c) = cmbUnlockGroup.ListIndex
        End If
        gridUnlockGroupSet.Text = cmbUnlockGroup.Text
    End If

End Sub

Private Sub gridUnlockGroupSet_Scroll()
    gridUnlockGroupSet_LeaveCell

End Sub

Private Sub gridUserGroupSet_Click()
    gridUserGroupSet_EnterCell
End Sub

Private Sub gridUserGroupSet_Scroll()
    gridUserGroupSet_LeaveCell

End Sub
Private Sub gridUserGroupSet_EnterCell()
    Dim c As Integer, r As Integer
    If Me.Visible And Not bShowProcessing Then
        r = gridUserGroupSet.Row - 1
        c = 1
        If Not cmbGroup.Visible Then cmbGroup.Visible = True
        cmbGroup.Left = gridUserGroupSet.Left + gridUserGroupSet.CellLeft
        cmbGroup.Top = gridUserGroupSet.Top + gridUserGroupSet.CellTop
        cmbGroup.ListIndex = DbUserGroupArray(r)
    End If
End Sub

Private Sub gridUserGroupSet_LeaveCell()
    Dim c As Integer, r As Integer
    If Me.Visible And Not bShowProcessing Then
        r = gridUserGroupSet.Row - 1
        c = 1
        If r < 0 Or c < 0 Then Exit Sub
            DbUserGroupArray(r) = cmbGroup.ListIndex
        gridUserGroupSet.Text = cmbGroup.Text
    End If

End Sub

