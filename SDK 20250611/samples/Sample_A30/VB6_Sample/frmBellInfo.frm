VERSION 5.00
Begin VB.Form frmBellInfo 
   Caption         =   "Setting Bell Time"
   ClientHeight    =   8340
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   8625
   BeginProperty Font 
      Name            =   "Times New Roman"
      Size            =   12
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmBellInfo.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   8340
   ScaleWidth      =   8625
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   23
      Left            =   6510
      TabIndex        =   129
      Top             =   6765
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   23
      Left            =   7395
      TabIndex        =   128
      Top             =   6765
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   23
      Left            =   6015
      TabIndex        =   127
      Top             =   6765
      Width           =   195
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   22
      Left            =   6510
      TabIndex        =   124
      Top             =   6285
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   22
      Left            =   7395
      TabIndex        =   123
      Top             =   6285
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   22
      Left            =   6015
      TabIndex        =   122
      Top             =   6285
      Width           =   195
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   21
      Left            =   6510
      TabIndex        =   119
      Top             =   5805
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   21
      Left            =   7395
      TabIndex        =   118
      Top             =   5805
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   21
      Left            =   6015
      TabIndex        =   117
      Top             =   5805
      Width           =   195
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   20
      Left            =   6510
      TabIndex        =   114
      Top             =   5325
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   20
      Left            =   7395
      TabIndex        =   113
      Top             =   5325
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   20
      Left            =   6015
      TabIndex        =   112
      Top             =   5325
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   19
      Left            =   6015
      TabIndex        =   109
      Top             =   4860
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   19
      Left            =   7395
      TabIndex        =   108
      Top             =   4860
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   19
      Left            =   6510
      TabIndex        =   107
      Top             =   4860
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   18
      Left            =   6015
      TabIndex        =   103
      Top             =   4440
      Width           =   195
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   18
      Left            =   6510
      TabIndex        =   102
      Top             =   4440
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   18
      Left            =   7395
      TabIndex        =   101
      Top             =   4440
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   17
      Left            =   6015
      TabIndex        =   99
      Top             =   4020
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   17
      Left            =   7395
      TabIndex        =   98
      Top             =   4020
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   17
      Left            =   6510
      TabIndex        =   97
      Top             =   4020
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   16
      Left            =   6015
      TabIndex        =   83
      Top             =   1935
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   15
      Left            =   6015
      TabIndex        =   82
      Top             =   2355
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   14
      Left            =   6015
      TabIndex        =   81
      Top             =   2775
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   13
      Left            =   6015
      TabIndex        =   80
      Top             =   3180
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   12
      Left            =   6015
      TabIndex        =   79
      Top             =   3600
      Width           =   195
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   12
      Left            =   6510
      TabIndex        =   78
      Top             =   1935
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   12
      Left            =   7395
      TabIndex        =   77
      Top             =   1935
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   13
      Left            =   7395
      TabIndex        =   76
      Top             =   2355
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   13
      Left            =   6510
      TabIndex        =   75
      Top             =   2355
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   14
      Left            =   6510
      TabIndex        =   74
      Top             =   2775
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   14
      Left            =   7395
      TabIndex        =   73
      Top             =   2775
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   15
      Left            =   7395
      TabIndex        =   72
      Top             =   3180
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   15
      Left            =   6510
      TabIndex        =   71
      Top             =   3180
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   16
      Left            =   6510
      TabIndex        =   70
      Top             =   3600
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   16
      Left            =   7395
      TabIndex        =   69
      Top             =   3600
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   11
      Left            =   2160
      TabIndex        =   63
      Top             =   6765
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   11
      Left            =   3540
      TabIndex        =   62
      Top             =   6720
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   11
      Left            =   2655
      TabIndex        =   61
      Top             =   6720
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   10
      Left            =   2160
      TabIndex        =   59
      Top             =   6285
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   10
      Left            =   3540
      TabIndex        =   58
      Top             =   6240
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   10
      Left            =   2655
      TabIndex        =   57
      Top             =   6240
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   9
      Left            =   2160
      TabIndex        =   55
      Top             =   5805
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   9
      Left            =   3540
      TabIndex        =   54
      Top             =   5760
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   9
      Left            =   2655
      TabIndex        =   53
      Top             =   5760
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   8
      Left            =   2160
      TabIndex        =   51
      Top             =   5325
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   8
      Left            =   3540
      TabIndex        =   50
      Top             =   5280
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   8
      Left            =   2655
      TabIndex        =   49
      Top             =   5280
      Width           =   630
   End
   Begin VB.TextBox txtBellCount 
      Height          =   435
      Left            =   2040
      TabIndex        =   28
      Top             =   1000
      Width           =   585
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   7
      Left            =   2655
      TabIndex        =   27
      Top             =   4815
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   7
      Left            =   3540
      TabIndex        =   26
      Top             =   4815
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   6
      Left            =   3540
      TabIndex        =   25
      Top             =   4395
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   6
      Left            =   2655
      TabIndex        =   24
      Top             =   4395
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   5
      Left            =   2655
      TabIndex        =   23
      Top             =   3975
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   5
      Left            =   3540
      TabIndex        =   22
      Top             =   3975
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   4
      Left            =   3540
      TabIndex        =   21
      Top             =   3555
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   4
      Left            =   2655
      TabIndex        =   20
      Top             =   3555
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   3
      Left            =   2655
      TabIndex        =   19
      Top             =   3135
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   3
      Left            =   3540
      TabIndex        =   18
      Top             =   3135
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   2
      Left            =   3540
      TabIndex        =   17
      Top             =   2730
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   2
      Left            =   2655
      TabIndex        =   16
      Top             =   2730
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   1
      Left            =   2655
      TabIndex        =   15
      Top             =   2310
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   1
      Left            =   3540
      TabIndex        =   14
      Top             =   2310
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   0
      Left            =   3540
      TabIndex        =   13
      Top             =   1890
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   0
      Left            =   2655
      TabIndex        =   12
      Top             =   1890
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   7
      Left            =   2160
      TabIndex        =   11
      Top             =   4860
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   6
      Left            =   2160
      TabIndex        =   10
      Top             =   4440
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   5
      Left            =   2160
      TabIndex        =   9
      Top             =   4020
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   4
      Left            =   2160
      TabIndex        =   8
      Top             =   3600
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   3
      Left            =   2160
      TabIndex        =   7
      Top             =   3180
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   2
      Left            =   2160
      TabIndex        =   6
      Top             =   2775
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   1
      Left            =   2160
      TabIndex        =   5
      Top             =   2355
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   0
      Left            =   2160
      TabIndex        =   4
      Top             =   1935
      Width           =   195
   End
   Begin VB.CommandButton cmdExit 
      Caption         =   "Exit"
      Height          =   510
      Left            =   5565
      TabIndex        =   2
      Top             =   7545
      Width           =   1485
   End
   Begin VB.CommandButton cmdWrite 
      Caption         =   "Write"
      Height          =   510
      Left            =   3690
      TabIndex        =   1
      Top             =   7545
      Width           =   1485
   End
   Begin VB.CommandButton cmdRead 
      Caption         =   "Read"
      Height          =   510
      Left            =   1800
      TabIndex        =   0
      Top             =   7545
      Width           =   1485
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 24:"
      Height          =   285
      Index           =   47
      Left            =   4725
      TabIndex        =   131
      Top             =   6765
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   46
      Left            =   7170
      TabIndex        =   130
      Top             =   6840
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 23:"
      Height          =   285
      Index           =   45
      Left            =   4725
      TabIndex        =   126
      Top             =   6285
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   44
      Left            =   7170
      TabIndex        =   125
      Top             =   6360
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 22:"
      Height          =   285
      Index           =   43
      Left            =   4725
      TabIndex        =   121
      Top             =   5805
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   42
      Left            =   7170
      TabIndex        =   120
      Top             =   5880
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 21:"
      Height          =   285
      Index           =   41
      Left            =   4725
      TabIndex        =   116
      Top             =   5325
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   40
      Left            =   7170
      TabIndex        =   115
      Top             =   5400
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 20:"
      Height          =   285
      Index           =   39
      Left            =   4725
      TabIndex        =   111
      Top             =   4860
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   38
      Left            =   7170
      TabIndex        =   110
      Top             =   4965
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   37
      Left            =   7200
      TabIndex        =   106
      Top             =   4080
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 19:"
      Height          =   285
      Index           =   36
      Left            =   4725
      TabIndex        =   105
      Top             =   4440
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   35
      Left            =   7170
      TabIndex        =   104
      Top             =   4485
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 18:"
      Height          =   285
      Index           =   34
      Left            =   4725
      TabIndex        =   100
      Top             =   4020
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 13:"
      Height          =   285
      Index           =   33
      Left            =   4725
      TabIndex        =   96
      Top             =   1935
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 14:"
      Height          =   285
      Index           =   32
      Left            =   4725
      TabIndex        =   95
      Top             =   2355
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 15:"
      Height          =   285
      Index           =   31
      Left            =   4725
      TabIndex        =   94
      Top             =   2775
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 16:"
      Height          =   285
      Index           =   30
      Left            =   4725
      TabIndex        =   93
      Top             =   3180
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 17:"
      Height          =   285
      Index           =   29
      Left            =   4725
      TabIndex        =   92
      Top             =   3600
      Width           =   885
   End
   Begin VB.Label Label8 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Start Time"
      Height          =   285
      Left            =   6750
      TabIndex        =   91
      Top             =   1560
      Width           =   1065
   End
   Begin VB.Label Label7 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "UseFlag"
      Height          =   285
      Left            =   5715
      TabIndex        =   90
      Top             =   1560
      Width           =   825
   End
   Begin VB.Label Label6 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Bell Point"
      Height          =   285
      Left            =   4560
      TabIndex        =   89
      Top             =   1560
      Width           =   975
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   28
      Left            =   7215
      TabIndex        =   88
      Top             =   2415
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   27
      Left            =   7215
      TabIndex        =   87
      Top             =   2835
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   26
      Left            =   7200
      TabIndex        =   86
      Top             =   3240
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   25
      Left            =   7215
      TabIndex        =   85
      Top             =   3660
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   24
      Left            =   7215
      TabIndex        =   84
      Top             =   1995
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   23
      Left            =   3360
      TabIndex        =   68
      Top             =   6720
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   22
      Left            =   3360
      TabIndex        =   67
      Top             =   6240
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   21
      Left            =   3360
      TabIndex        =   66
      Top             =   5760
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   20
      Left            =   3360
      TabIndex        =   65
      Top             =   5280
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 12:"
      Height          =   285
      Index           =   19
      Left            =   870
      TabIndex        =   64
      Top             =   6765
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 11:"
      Height          =   285
      Index           =   18
      Left            =   870
      TabIndex        =   60
      Top             =   6285
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 10:"
      Height          =   285
      Index           =   17
      Left            =   870
      TabIndex        =   56
      Top             =   5805
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 9:"
      Height          =   285
      Index           =   16
      Left            =   870
      TabIndex        =   52
      Top             =   5325
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   15
      Left            =   3360
      TabIndex        =   48
      Top             =   1935
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   14
      Left            =   3360
      TabIndex        =   47
      Top             =   4860
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   13
      Left            =   3360
      TabIndex        =   46
      Top             =   4440
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   12
      Left            =   3360
      TabIndex        =   45
      Top             =   4020
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   11
      Left            =   3360
      TabIndex        =   44
      Top             =   3600
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   10
      Left            =   3360
      TabIndex        =   43
      Top             =   3180
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   9
      Left            =   3360
      TabIndex        =   42
      Top             =   2775
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   8
      Left            =   3360
      TabIndex        =   41
      Top             =   2355
      Width           =   180
   End
   Begin VB.Label Label5 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Bell Point"
      Height          =   285
      Left            =   705
      TabIndex        =   40
      Top             =   1560
      Width           =   975
   End
   Begin VB.Label Label3 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "UseFlag"
      Height          =   285
      Left            =   1860
      TabIndex        =   39
      Top             =   1560
      Width           =   825
   End
   Begin VB.Label Label4 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Start Time"
      Height          =   285
      Left            =   2895
      TabIndex        =   38
      Top             =   1560
      Width           =   1065
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 8:"
      Height          =   285
      Index           =   7
      Left            =   870
      TabIndex        =   37
      Top             =   4860
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 7:"
      Height          =   285
      Index           =   6
      Left            =   870
      TabIndex        =   36
      Top             =   4440
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 6:"
      Height          =   285
      Index           =   5
      Left            =   870
      TabIndex        =   35
      Top             =   4020
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 5:"
      Height          =   285
      Index           =   4
      Left            =   870
      TabIndex        =   34
      Top             =   3600
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 4:"
      Height          =   285
      Index           =   3
      Left            =   870
      TabIndex        =   33
      Top             =   3180
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 3:"
      Height          =   285
      Index           =   2
      Left            =   870
      TabIndex        =   32
      Top             =   2775
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 2:"
      Height          =   285
      Index           =   1
      Left            =   870
      TabIndex        =   31
      Top             =   2355
      Width           =   765
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 1:"
      Height          =   285
      Index           =   0
      Left            =   870
      TabIndex        =   30
      Top             =   1935
      Width           =   765
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Bell Count :"
      Height          =   285
      Left            =   720
      TabIndex        =   29
      Top             =   1080
      Width           =   1200
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
      Left            =   1440
      TabIndex        =   3
      Top             =   375
      Width           =   5760
   End
End
Attribute VB_Name = "frmBellInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const DataLen = 72
Dim mlngBellInfo(DataLen / 4 - 1) As Long
Dim mBellCount As Long
Dim mBellInfo As BellInfo
Dim mMachineNumber As Long

Private Sub cmdExit_Click()
    Unload Me
    frmMain.Visible = True
End Sub

Private Sub cmdRead_Click()
Dim vRet As Boolean
    Dim vErrorCode As Long

    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.GetBellTime(mMachineNumber, mBellCount, mlngBellInfo(0))
    If vRet = True Then
        CopyMemory mBellInfo, mlngBellInfo(0), DataLen
        ShowValue
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If

    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdWrite_Click()
Dim vRet As Boolean
Dim vErrorCode As Long
    
    lblMessage.Caption = "Waiting..."
    DoEvents
    
    If frmMain.SB100BPC1.EnableDevice(mMachineNumber, False) = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    GetValue
    CopyMemory mlngBellInfo(0), mBellInfo, DataLen
    
    vRet = frmMain.SB100BPC1.SetBellTime(mMachineNumber, mBellCount, mlngBellInfo(0))
    If vRet = True Then
        lblMessage.Caption = "Success!"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If

    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Unload Me
    frmMain.Visible = True
End Sub

Private Sub ShowValue()
Dim i As Long

    For i = 0 To MAX_BELLCOUNT_DAY - 1
        txtHour(i).Text = mBellInfo.mHour(i)
        txtMinute(i).Text = mBellInfo.mMinute(i)
        If mBellInfo.mValid(i) > 1 Then mBellInfo.mValid(i) = 0
        chkValid(i).Value = mBellInfo.mValid(i)
    Next i
    txtBellCount.Text = mBellCount
End Sub

Private Sub GetValue()
Dim i As Long

    For i = 0 To MAX_BELLCOUNT_DAY - 1
        mBellInfo.mHour(i) = Val(txtHour(i).Text)
        mBellInfo.mMinute(i) = Val(txtMinute(i).Text)
        mBellInfo.mValid(i) = chkValid(i).Value
    Next i
    mBellCount = Val(txtBellCount.Text)
End Sub
