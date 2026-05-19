VERSION 5.00
Begin VB.Form frmLockCtl 
   Caption         =   "Door Open Control"
   ClientHeight    =   9315
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   8025
   LinkTopic       =   "Form1"
   ScaleHeight     =   9315
   ScaleWidth      =   8025
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame Frame1 
      Caption         =   "Unlock Setting"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3615
      Left            =   240
      TabIndex        =   25
      Top             =   5520
      Width           =   7575
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   19
         Left            =   6720
         TabIndex        =   57
         Top             =   2400
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   18
         Left            =   6120
         TabIndex        =   55
         Top             =   2400
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   17
         Left            =   6720
         TabIndex        =   54
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   16
         Left            =   6120
         TabIndex        =   52
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   15
         Left            =   6720
         TabIndex        =   51
         Top             =   1440
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   14
         Left            =   6120
         TabIndex        =   49
         Top             =   1440
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   13
         Left            =   6720
         TabIndex        =   48
         Top             =   960
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   12
         Left            =   6120
         TabIndex        =   46
         Top             =   960
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   11
         Left            =   6720
         TabIndex        =   45
         Top             =   480
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   10
         Left            =   6120
         TabIndex        =   43
         Top             =   480
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   9
         Left            =   2880
         TabIndex        =   42
         Top             =   2400
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   8
         Left            =   2280
         TabIndex        =   40
         Top             =   2400
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   7
         Left            =   2880
         TabIndex        =   39
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   6
         Left            =   2280
         TabIndex        =   37
         Top             =   1920
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   5
         Left            =   2880
         TabIndex        =   36
         Top             =   1440
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   4
         Left            =   2280
         TabIndex        =   34
         Top             =   1440
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   3
         Left            =   2880
         TabIndex        =   33
         Top             =   960
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   2
         Left            =   2280
         TabIndex        =   31
         Top             =   960
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   1
         Left            =   2880
         TabIndex        =   30
         Top             =   480
         Width           =   495
      End
      Begin VB.TextBox txtUnlockGroup 
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
         Index           =   0
         Left            =   2280
         TabIndex        =   28
         Top             =   480
         Width           =   495
      End
      Begin VB.CommandButton cmdReadUnlockGroup 
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
         Height          =   375
         Left            =   2160
         TabIndex        =   27
         Top             =   3000
         Width           =   1215
      End
      Begin VB.CommandButton cmdWriteUnlockGroup 
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
         Height          =   375
         Left            =   4080
         TabIndex        =   26
         Top             =   3000
         Width           =   1215
      End
      Begin VB.Label Label6 
         Caption         =   "Unlock Group 10:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   9
         Left            =   4080
         TabIndex        =   56
         Top             =   2430
         Width           =   1935
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 9:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   8
         Left            =   4080
         TabIndex        =   53
         Top             =   1950
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 8:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   7
         Left            =   4080
         TabIndex        =   50
         Top             =   1470
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 7:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   6
         Left            =   4080
         TabIndex        =   47
         Top             =   990
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 6:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   5
         Left            =   4080
         TabIndex        =   44
         Top             =   510
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 5:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   4
         Left            =   240
         TabIndex        =   41
         Top             =   2430
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 4:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   3
         Left            =   240
         TabIndex        =   38
         Top             =   1950
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 3:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   2
         Left            =   240
         TabIndex        =   35
         Top             =   1470
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 2:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   1
         Left            =   240
         TabIndex        =   32
         Top             =   990
         Width           =   1695
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         Caption         =   "Unlock Group 1:"
         BeginProperty Font 
            Name            =   "Times New Roman"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   315
         Index           =   0
         Left            =   240
         TabIndex        =   29
         Top             =   510
         Width           =   1695
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "Door Setting"
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
      TabIndex        =   10
      Top             =   1320
      Width           =   4335
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
         ItemData        =   "frmLockCtl.frx":0000
         Left            =   2520
         List            =   "frmLockCtl.frx":000D
         TabIndex        =   24
         Text            =   "N.O."
         Top             =   450
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
         ItemData        =   "frmLockCtl.frx":0021
         Left            =   2520
         List            =   "frmLockCtl.frx":002B
         TabIndex        =   23
         Text            =   "Group 1"
         Top             =   2400
         Width           =   1575
      End
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
         ItemData        =   "frmLockCtl.frx":0041
         Left            =   2520
         List            =   "frmLockCtl.frx":004B
         TabIndex        =   22
         Text            =   "Master"
         Top             =   2880
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
         ItemData        =   "frmLockCtl.frx":005E
         Left            =   2520
         List            =   "frmLockCtl.frx":0068
         TabIndex        =   21
         Text            =   "Use"
         Top             =   1920
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
         Height          =   375
         Left            =   2520
         TabIndex        =   14
         Text            =   "10"
         Top             =   1440
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
         Height          =   375
         Left            =   2520
         TabIndex        =   13
         Text            =   "5"
         Top             =   960
         Width           =   1575
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
         TabIndex        =   12
         Top             =   3480
         Width           =   1215
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
         TabIndex        =   11
         Top             =   3480
         Width           =   1215
      End
      Begin VB.Label Label5 
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
         Left            =   225
         TabIndex        =   20
         Top             =   1920
         Width           =   2175
      End
      Begin VB.Label Label4 
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
         TabIndex        =   19
         Top             =   480
         Width           =   2175
      End
      Begin VB.Label Label3 
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
         TabIndex        =   18
         Top             =   1440
         Width           =   2175
      End
      Begin VB.Label Label2 
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
         TabIndex        =   17
         Top             =   960
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
         TabIndex        =   16
         Top             =   2450
         Width           =   2175
      End
      Begin VB.Label Label10 
         Alignment       =   1  'Right Justify
         Caption         =   "Antipass Location :"
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
         TabIndex        =   15
         Top             =   2880
         Width           =   2175
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
      Left            =   3570
      TabIndex        =   8
      Text            =   "0"
      Top             =   840
      Width           =   1455
   End
   Begin VB.CommandButton cmdWarnCancel 
      Caption         =   "Warn Cancel"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   492
      Left            =   4800
      TabIndex        =   7
      Top             =   3950
      Width           =   1815
   End
   Begin VB.CommandButton cmdRestart 
      Caption         =   "Reboot"
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
      Left            =   4800
      TabIndex        =   6
      Top             =   4440
      Width           =   1815
   End
   Begin VB.CommandButton cmdAutoRecover 
      Caption         =   "Auto Recover"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   504
      Left            =   4800
      TabIndex        =   5
      Top             =   2430
      Width           =   1815
   End
   Begin VB.CommandButton cmdUncondClose 
      Caption         =   "Uncond Close"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   492
      Left            =   4800
      TabIndex        =   4
      Top             =   3430
      Width           =   1815
   End
   Begin VB.CommandButton cmdUncondOpen 
      Caption         =   "Uncond Open"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   492
      Left            =   4800
      TabIndex        =   3
      Top             =   2945
      Width           =   1815
   End
   Begin VB.CommandButton cmdGetDoorStatus 
      Caption         =   "Get DoorStatus"
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
      TabIndex        =   2
      Top             =   1440
      Width           =   1815
   End
   Begin VB.CommandButton cmdDoorOpen 
      Caption         =   "Door Open"
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
      TabIndex        =   1
      Top             =   1930
      Width           =   1815
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Caption         =   "Door Number :"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1800
      TabIndex        =   9
      Top             =   870
      Width           =   1695
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
      Left            =   288
      TabIndex        =   0
      Top             =   252
      Width           =   6012
   End
End
Attribute VB_Name = "frmLockCtl"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim mMachineNumber As Long
Dim DbUnlockGroupArray(2 * 10) As Byte

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
    
    'make xml
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        cmbSensorType.ListIndex = frmMain.SB100BPC1.XML_ParseInt(strXML, "DoorSensorType")
        txtLockReleaseTime = frmMain.SB100BPC1.XML_ParseInt(strXML, "LockReleaseTime")
        txtOpenTimeout = frmMain.SB100BPC1.XML_ParseInt(strXML, "DoorOpenTimeout")
        cmbUseAntipass.ListIndex = 1 - frmMain.SB100BPC1.XML_ParseInt(strXML, "UseAntipass")
        cmbAntipassNo.ListIndex = frmMain.SB100BPC1.XML_ParseInt(strXML, "AntipassNo")
        cmbLocation.ListIndex = frmMain.SB100BPC1.XML_ParseInt(strXML, "Location")
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGetDoorStatus_Click()
    Dim vStatus As Long
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If

    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        vStatus = frmMain.SB100BPC1.XML_ParseLong(strXML, "DoorStatus")
        If vStatus = 1 Then
            lblMessage.Caption = "Uncond Door Open State!"
        ElseIf vStatus = 2 Then
            lblMessage.Caption = "Uncond Door Close State!"
        ElseIf vStatus = 3 Then
            lblMessage.Caption = "Door Open State!"
        ElseIf vStatus = 4 Then
            lblMessage.Caption = "Auto Recover State!"
        ElseIf vStatus = 5 Then
            lblMessage.Caption = "Door Close State!"
        ElseIf vStatus = 6 Then
            lblMessage.Caption = "Watching for Close!"
        ElseIf vStatus = 7 Then
            lblMessage.Caption = "Illegal open!"
        Else
            lblMessage.Caption = "User State !"
        End If
    Else
        frmMain.SB100BPC1.GetLastError (vErrorCode)
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdAutoRecover_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = CInt(txtDoorNumber)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 4

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "Auto Recover Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdDoorOpen_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim lDoorNumber As Long
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 3

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        lblMessage.Caption = "Door Open Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdReadUnlockGroup_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim lInfo As Long
    Dim strXML As String
    Dim i As Integer
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetUnlockgroup"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        frmMain.SB100BPC1.XML_ParseBinaryByte strXML, "UnlockGroupBinary", DbUnlockGroupArray(0), 2 * 10
        For i = 0 To 19
            txtUnlockGroup(i) = CStr(DbUnlockGroupArray(i))
        Next
        lblMessage.Caption = "GetUnlockgroup Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdRestart_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim strXML As String
    Dim lDoorNumber As Long

    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 5

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If vRet = True Then
        lblMessage.Caption = "Reboot Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    DoEvents
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
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorParam"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorSensorType", Val(cmbSensorType.ListIndex)
    frmMain.SB100BPC1.XML_AddInt strXML, "LockReleaseTime", Val(txtLockReleaseTime)
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorOpenTimeout", Val(txtOpenTimeout)
    frmMain.SB100BPC1.XML_AddInt strXML, "UseAntipass", Val(1 - cmbUseAntipass.ListIndex)
    frmMain.SB100BPC1.XML_AddInt strXML, "AntipassNo", Val(cmbAntipassNo.ListIndex)
    frmMain.SB100BPC1.XML_AddInt strXML, "Location", Val(cmbLocation.ListIndex)

    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdUncondClose_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 2

    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "Uncond Door Close Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdUncondOpen_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = Val(txtDoorNumber.Text)
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 1
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    If vRet = True Then
        lblMessage.Caption = "Uncond Door Open Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdWarnCancel_Click()
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim lDoorNumber As Long
    Dim strXML As String
    
    lDoorNumber = 0
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetDoorStatusMulti"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorNo", lDoorNumber
    frmMain.SB100BPC1.XML_AddInt strXML, "DoorStatus", 6
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = True Then
        lblMessage.Caption = "Warning cancel Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    DoEvents
End Sub

Private Sub cmdWriteUnlockGroup_Click()
    Dim bRet As Boolean
    Dim vErrorCode As Long

    Dim strXML As String
    Dim i As Integer
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    For i = 0 To 19
        DbUnlockGroupArray(i) = CInt(txtUnlockGroup(i))
    Next
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetUnlockgroup"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", frmMain.gMachineNumber
    frmMain.SB100BPC1.XML_AddBinaryByte strXML, "UnlockGroupBinary", DbUnlockGroupArray(0), 2 * 10
    
    bRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)

    If bRet = True Then
        lblMessage.Caption = "SetUnlockgroup Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
    cmbSensorType.ListIndex = 1
    cmbUseAntipass.ListIndex = 0
    cmbLocation.ListIndex = 0
    cmbAntipassNo.ListIndex = 0
End Sub
Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub
