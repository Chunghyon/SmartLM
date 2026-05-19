VERSION 5.00
Begin VB.Form frmBellInfo 
   Caption         =   "Setting Bell Time"
   ClientHeight    =   7725
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   10095
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
   ScaleHeight     =   7725
   ScaleWidth      =   10095
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   12
      Left            =   8280
      TabIndex        =   119
      Top             =   1560
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   12
      Left            =   9165
      TabIndex        =   118
      Top             =   1560
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   13
      Left            =   9165
      TabIndex        =   117
      Top             =   1980
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   13
      Left            =   8280
      TabIndex        =   116
      Top             =   1980
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   14
      Left            =   8280
      TabIndex        =   115
      Top             =   2400
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   14
      Left            =   9165
      TabIndex        =   114
      Top             =   2400
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   15
      Left            =   9165
      TabIndex        =   113
      Top             =   2805
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   15
      Left            =   8280
      TabIndex        =   112
      Top             =   2805
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   16
      Left            =   8280
      TabIndex        =   111
      Top             =   3225
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   16
      Left            =   9165
      TabIndex        =   110
      Top             =   3225
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   17
      Left            =   9165
      TabIndex        =   109
      Top             =   3645
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   17
      Left            =   8280
      TabIndex        =   108
      Top             =   3645
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   18
      Left            =   8280
      TabIndex        =   107
      Top             =   4065
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   18
      Left            =   9165
      TabIndex        =   106
      Top             =   4065
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   19
      Left            =   9165
      TabIndex        =   105
      Top             =   4485
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   19
      Left            =   8280
      TabIndex        =   104
      Top             =   4485
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   20
      Left            =   8280
      TabIndex        =   103
      Top             =   4950
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   20
      Left            =   9165
      TabIndex        =   102
      Top             =   4950
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   21
      Left            =   8280
      TabIndex        =   101
      Top             =   5430
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   21
      Left            =   9165
      TabIndex        =   100
      Top             =   5430
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   22
      Left            =   8280
      TabIndex        =   99
      Top             =   5910
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   22
      Left            =   9165
      TabIndex        =   98
      Top             =   5910
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   23
      Left            =   8280
      TabIndex        =   97
      Top             =   6390
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   23
      Left            =   9165
      TabIndex        =   96
      Top             =   6390
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   12
      Left            =   7815
      TabIndex        =   80
      Top             =   1635
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   13
      Left            =   7815
      TabIndex        =   79
      Top             =   2055
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   14
      Left            =   7815
      TabIndex        =   78
      Top             =   2475
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   15
      Left            =   7815
      TabIndex        =   77
      Top             =   2880
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   16
      Left            =   7815
      TabIndex        =   76
      Top             =   3300
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   17
      Left            =   7815
      TabIndex        =   75
      Top             =   3720
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   18
      Left            =   7815
      TabIndex        =   74
      Top             =   4140
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   19
      Left            =   7815
      TabIndex        =   73
      Top             =   4560
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   20
      Left            =   7815
      TabIndex        =   72
      Top             =   5025
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   21
      Left            =   7815
      TabIndex        =   71
      Top             =   5505
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   22
      Left            =   7815
      TabIndex        =   70
      Top             =   5985
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   23
      Left            =   7815
      TabIndex        =   69
      Top             =   6465
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   11
      Left            =   2160
      TabIndex        =   63
      Top             =   6405
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   11
      Left            =   3540
      TabIndex        =   62
      Top             =   6360
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   11
      Left            =   2655
      TabIndex        =   61
      Top             =   6360
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   10
      Left            =   2160
      TabIndex        =   59
      Top             =   5925
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   10
      Left            =   3540
      TabIndex        =   58
      Top             =   5880
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   10
      Left            =   2655
      TabIndex        =   57
      Top             =   5880
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   9
      Left            =   2160
      TabIndex        =   55
      Top             =   5445
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   9
      Left            =   3540
      TabIndex        =   54
      Top             =   5400
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   9
      Left            =   2655
      TabIndex        =   53
      Top             =   5400
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   8
      Left            =   2160
      TabIndex        =   51
      Top             =   4965
      Width           =   195
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   8
      Left            =   3540
      TabIndex        =   50
      Top             =   4920
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   8
      Left            =   2655
      TabIndex        =   49
      Top             =   4920
      Width           =   630
   End
   Begin VB.TextBox txtBellCount 
      Height          =   435
      Left            =   4710
      TabIndex        =   28
      Top             =   1500
      Width           =   585
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   7
      Left            =   2655
      TabIndex        =   27
      Top             =   4455
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   7
      Left            =   3540
      TabIndex        =   26
      Top             =   4455
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   6
      Left            =   3540
      TabIndex        =   25
      Top             =   4035
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   6
      Left            =   2655
      TabIndex        =   24
      Top             =   4035
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   5
      Left            =   2655
      TabIndex        =   23
      Top             =   3615
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   5
      Left            =   3540
      TabIndex        =   22
      Top             =   3615
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   4
      Left            =   3540
      TabIndex        =   21
      Top             =   3195
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   4
      Left            =   2655
      TabIndex        =   20
      Top             =   3195
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   3
      Left            =   2655
      TabIndex        =   19
      Top             =   2775
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   3
      Left            =   3540
      TabIndex        =   18
      Top             =   2775
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   2
      Left            =   3540
      TabIndex        =   17
      Top             =   2370
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   2
      Left            =   2655
      TabIndex        =   16
      Top             =   2370
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   1
      Left            =   2655
      TabIndex        =   15
      Top             =   1950
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   1
      Left            =   3540
      TabIndex        =   14
      Top             =   1950
      Width           =   630
   End
   Begin VB.TextBox txtMinute 
      Height          =   405
      Index           =   0
      Left            =   3540
      TabIndex        =   13
      Top             =   1530
      Width           =   630
   End
   Begin VB.TextBox txtHour 
      Height          =   405
      Index           =   0
      Left            =   2655
      TabIndex        =   12
      Top             =   1530
      Width           =   630
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   7
      Left            =   2160
      TabIndex        =   11
      Top             =   4500
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   6
      Left            =   2160
      TabIndex        =   10
      Top             =   4080
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   5
      Left            =   2160
      TabIndex        =   9
      Top             =   3660
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   4
      Left            =   2160
      TabIndex        =   8
      Top             =   3240
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   3
      Left            =   2160
      TabIndex        =   7
      Top             =   2820
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   2
      Left            =   2160
      TabIndex        =   6
      Top             =   2415
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   1
      Left            =   2160
      TabIndex        =   5
      Top             =   1995
      Width           =   195
   End
   Begin VB.CheckBox chkValid 
      Caption         =   "Time1"
      Height          =   285
      Index           =   0
      Left            =   2160
      TabIndex        =   4
      Top             =   1575
      Width           =   195
   End
   Begin VB.CommandButton cmdExit 
      Caption         =   "Exit"
      Height          =   510
      Left            =   5925
      TabIndex        =   2
      Top             =   7065
      Width           =   1485
   End
   Begin VB.CommandButton cmdWrite 
      Caption         =   "Write"
      Height          =   510
      Left            =   4050
      TabIndex        =   1
      Top             =   7065
      Width           =   1485
   End
   Begin VB.CommandButton cmdRead 
      Caption         =   "Read"
      Height          =   510
      Left            =   2160
      TabIndex        =   0
      Top             =   7065
      Width           =   1485
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   39
      Left            =   8985
      TabIndex        =   131
      Top             =   2025
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   38
      Left            =   8985
      TabIndex        =   130
      Top             =   2445
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   37
      Left            =   8985
      TabIndex        =   129
      Top             =   2850
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   36
      Left            =   8985
      TabIndex        =   128
      Top             =   3270
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   35
      Left            =   8985
      TabIndex        =   127
      Top             =   3690
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   34
      Left            =   8985
      TabIndex        =   126
      Top             =   4110
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   33
      Left            =   8985
      TabIndex        =   125
      Top             =   4530
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   32
      Left            =   8985
      TabIndex        =   124
      Top             =   1605
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   27
      Left            =   8985
      TabIndex        =   123
      Top             =   5070
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   26
      Left            =   8985
      TabIndex        =   122
      Top             =   5550
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   25
      Left            =   8985
      TabIndex        =   121
      Top             =   6030
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   24
      Left            =   8985
      TabIndex        =   120
      Top             =   6510
      Width           =   180
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 13:"
      Height          =   285
      Index           =   47
      Left            =   6525
      TabIndex        =   95
      Top             =   1635
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 14:"
      Height          =   285
      Index           =   46
      Left            =   6525
      TabIndex        =   94
      Top             =   2055
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 15:"
      Height          =   285
      Index           =   45
      Left            =   6525
      TabIndex        =   93
      Top             =   2475
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 16:"
      Height          =   285
      Index           =   44
      Left            =   6525
      TabIndex        =   92
      Top             =   2880
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 17:"
      Height          =   285
      Index           =   43
      Left            =   6525
      TabIndex        =   91
      Top             =   3300
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 18:"
      Height          =   285
      Index           =   42
      Left            =   6525
      TabIndex        =   90
      Top             =   3720
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 19:"
      Height          =   285
      Index           =   41
      Left            =   6525
      TabIndex        =   89
      Top             =   4140
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 20:"
      Height          =   285
      Index           =   40
      Left            =   6525
      TabIndex        =   88
      Top             =   4560
      Width           =   885
   End
   Begin VB.Label Label8 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Start Time"
      Height          =   285
      Left            =   8550
      TabIndex        =   87
      Top             =   1200
      Width           =   1065
   End
   Begin VB.Label Label7 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "UseFlag"
      Height          =   285
      Left            =   7515
      TabIndex        =   86
      Top             =   1200
      Width           =   825
   End
   Begin VB.Label Label6 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Bell Point"
      Height          =   285
      Left            =   6360
      TabIndex        =   85
      Top             =   1200
      Width           =   975
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 21:"
      Height          =   285
      Index           =   31
      Left            =   6525
      TabIndex        =   84
      Top             =   5025
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 22:"
      Height          =   285
      Index           =   30
      Left            =   6525
      TabIndex        =   83
      Top             =   5505
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 23:"
      Height          =   285
      Index           =   29
      Left            =   6525
      TabIndex        =   82
      Top             =   5985
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Point 24:"
      Height          =   285
      Index           =   28
      Left            =   6525
      TabIndex        =   81
      Top             =   6465
      Width           =   885
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   ":"
      Height          =   285
      Index           =   23
      Left            =   3360
      TabIndex        =   68
      Top             =   6480
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
      Top             =   6000
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
      Top             =   5520
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
      Top             =   5040
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
      Top             =   6405
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
      Top             =   5925
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
      Top             =   5445
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
      Top             =   4965
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
      Top             =   1575
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
      Top             =   4500
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
      Top             =   4080
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
      Top             =   3660
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
      Top             =   3240
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
      Top             =   2820
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
      Top             =   2415
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
      Top             =   1995
      Width           =   180
   End
   Begin VB.Label Label5 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Bell Point"
      Height          =   285
      Left            =   705
      TabIndex        =   40
      Top             =   1140
      Width           =   975
   End
   Begin VB.Label Label3 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "UseFlag"
      Height          =   285
      Left            =   1860
      TabIndex        =   39
      Top             =   1140
      Width           =   825
   End
   Begin VB.Label Label4 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Start Time"
      Height          =   285
      Left            =   2895
      TabIndex        =   38
      Top             =   1140
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
      Top             =   4500
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
      Top             =   4080
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
      Top             =   3660
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
      Top             =   3240
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
      Top             =   2820
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
      Top             =   2415
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
      Top             =   1995
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
      Top             =   1575
      Width           =   765
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Bell Count :"
      Height          =   285
      Left            =   4425
      TabIndex        =   29
      Top             =   1140
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
      Left            =   435
      TabIndex        =   3
      Top             =   375
      Width           =   9000
   End
End
Attribute VB_Name = "frmBellInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const DataLen = 3 * 24
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
