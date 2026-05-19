VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCT2.OCX"
Begin VB.Form frmLog 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Manage Log Data"
   ClientHeight    =   7230
   ClientLeft      =   4815
   ClientTop       =   3135
   ClientWidth     =   13215
   Icon            =   "frmLog.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7230
   ScaleWidth      =   13215
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame Frame1 
      Caption         =   "Glog Search Range"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2535
      Left            =   10680
      TabIndex        =   16
      Top             =   3600
      Width           =   2175
      Begin VB.CommandButton cmdGLogSearchRange 
         Caption         =   "Set Range"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   360
         TabIndex        =   22
         Top             =   2040
         Width           =   1455
      End
      Begin MSComCtl2.DTPicker dtGLogSearchEndTime 
         Height          =   375
         Left            =   720
         TabIndex        =   21
         Top             =   1440
         Width           =   1335
         _ExtentX        =   2355
         _ExtentY        =   661
         _Version        =   393216
         Format          =   16580609
         CurrentDate     =   40754
      End
      Begin MSComCtl2.DTPicker dtGLogSearchStartTime 
         Height          =   375
         Left            =   720
         TabIndex        =   18
         Top             =   840
         Width           =   1335
         _ExtentX        =   2355
         _ExtentY        =   661
         _Version        =   393216
         Format          =   16580609
         CurrentDate     =   40754
      End
      Begin VB.CheckBox chkUseSearchRange 
         Caption         =   "Use Search Range"
         Height          =   375
         Left            =   240
         TabIndex        =   17
         Top             =   360
         Width           =   1815
      End
      Begin VB.Label lblEndTime 
         Caption         =   "End:"
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
         Left            =   120
         TabIndex        =   20
         Top             =   1440
         Width           =   495
      End
      Begin VB.Label lblStartTime 
         Caption         =   "Start:"
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
         Left            =   120
         TabIndex        =   19
         Top             =   840
         Width           =   495
      End
   End
   Begin VB.CheckBox chkShowGLogPhoto 
      Caption         =   "Show Glog Photo"
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
      Left            =   10680
      TabIndex        =   15
      Top             =   1320
      Width           =   2175
   End
   Begin VB.CheckBox chkAndDelete 
      Caption         =   "and Delete "
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
      Left            =   7560
      TabIndex        =   14
      Top             =   1275
      Width           =   1365
   End
   Begin VB.CommandButton cmdEmptySLog 
      Caption         =   "Empty SLogData"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   645
      Left            =   3480
      TabIndex        =   11
      Top             =   6390
      Width           =   1410
   End
   Begin VB.CommandButton cmdEmptyGLog 
      Caption         =   "Empty GLogData"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   645
      Left            =   9120
      TabIndex        =   10
      Top             =   6390
      Width           =   1410
   End
   Begin VB.CheckBox chkReadMark 
      Caption         =   "ReadMark"
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
      Left            =   9165
      TabIndex        =   7
      Top             =   1275
      Width           =   1365
   End
   Begin VB.CommandButton cmdAllGLogData 
      Caption         =   "Read All GLogData"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   645
      Left            =   7560
      TabIndex        =   6
      Top             =   6390
      Width           =   1410
   End
   Begin VB.CommandButton cmdAllSLogData 
      Caption         =   "Read All SLogData"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   645
      Left            =   1920
      TabIndex        =   4
      Top             =   6390
      Width           =   1410
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
      Height          =   645
      Left            =   11280
      TabIndex        =   3
      Top             =   6390
      Width           =   1410
   End
   Begin VB.CommandButton cmdGlogData 
      Caption         =   "Read GLogData"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   645
      Left            =   6000
      TabIndex        =   2
      Top             =   6390
      Width           =   1410
   End
   Begin VB.CommandButton cmdSLogData 
      Caption         =   "Read SLogData"
      BeginProperty Font 
         Name            =   "Times New Roman"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   645
      Left            =   360
      TabIndex        =   1
      Top             =   6390
      Width           =   1410
   End
   Begin MSFlexGridLib.MSFlexGrid gridSLogData 
      Height          =   4800
      Left            =   360
      TabIndex        =   5
      Top             =   1560
      Width           =   10185
      _ExtentX        =   17965
      _ExtentY        =   8467
      _Version        =   393216
      Cols            =   9
      Redraw          =   -1  'True
      GridLines       =   2
      AllowUserResizing=   1
   End
   Begin MSFlexGridLib.MSFlexGrid gridSLogData1 
      Height          =   3200
      Left            =   360
      TabIndex        =   12
      Top             =   3160
      Width           =   10185
      _ExtentX        =   17965
      _ExtentY        =   5636
      _Version        =   393216
      Cols            =   9
      FixedRows       =   0
      Redraw          =   -1  'True
      GridLines       =   2
      AllowUserResizing=   1
   End
   Begin MSFlexGridLib.MSFlexGrid gridSLogData2 
      Height          =   1600
      Left            =   360
      TabIndex        =   13
      Top             =   4760
      Width           =   10185
      _ExtentX        =   17965
      _ExtentY        =   2831
      _Version        =   393216
      Cols            =   9
      FixedRows       =   0
      GridLines       =   2
      AllowUserResizing=   1
   End
   Begin VB.Image imgGlogPhoto 
      BorderStyle     =   1  'Fixed Single
      Height          =   1815
      Left            =   10680
      Stretch         =   -1  'True
      Top             =   1680
      Width           =   2175
   End
   Begin VB.Label lblEnrollData 
      AutoSize        =   -1  'True
      Caption         =   "Log Data :"
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
      Left            =   405
      TabIndex        =   9
      Top             =   1290
      Width           =   960
   End
   Begin VB.Label LabelTotal 
      AutoSize        =   -1  'True
      Caption         =   "Total :"
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
      Left            =   1920
      TabIndex        =   8
      Top             =   1290
      Width           =   570
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
      Left            =   360
      TabIndex        =   0
      Top             =   600
      Width           =   12465
   End
End
Attribute VB_Name = "frmLog"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Const gMaxRow = 30000
Const gDiv = 65536
Dim gTempPhotoFileName As String
Dim mMachineNumber As Long
Dim prevSelectLogIndex As Long
Dim GlogSearched As Boolean
Public gstrLogItem As Variant

Private Sub chkReadMark_Click()
    frmMain.SB100BPC1.ReadMark = chkReadMark
End Sub

Private Sub DrawOneGeneralLog(mMachineNumber As Long, _
                                vPhotoNumber As Long, _
                                vSEnrollNumber As Long, _
                                vSMachineNumber As Long, _
                                vVerifyMode As Long, _
                                vYear As Long, _
                                vMonth As Long, _
                                vDay As Long, _
                                vHour As Long, _
                                vMinute As Long, _
                                vSecond As Long, _
                                vMaxLogCnt As Long, _
                                vIndex As Long, _
                                ByRef gridCtrl As MSFlexGrid)
                                
    Dim vAttStatus As Long, vAntipass As Long
    Dim stAttStatus As String, stAntipass As String
    Dim vDaigong As Long
    With gridCtrl
        vAntipass = vVerifyMode \ gDiv
        vDaigong = vAntipass \ 4
        vAntipass = vAntipass Mod 4
        vVerifyMode = vVerifyMode Mod gDiv
        vAttStatus = vVerifyMode \ 256
        vVerifyMode = vVerifyMode Mod 256
        stAttStatus = ""
        stAntipass = ""
        If vAttStatus = 0 Then
            stAttStatus = "_DutyOn"
        ElseIf vAttStatus = 1 Then
            stAttStatus = "_DutyOff"
        ElseIf vAttStatus = 2 Then
            stAttStatus = "_OverOn"
        ElseIf vAttStatus = 3 Then
            stAttStatus = "_OverOff"
        ElseIf vAttStatus = 4 Then
            stAttStatus = "_GoIn"
        ElseIf vAttStatus = 5 Then
            stAttStatus = "_GoOut"
        End If
        If vAntipass = 1 Then
            stAntipass = "(AP_In)"
        ElseIf vAntipass = 2 Then
            stAntipass = "(AP_Out)"
        End If
        .Row = vIndex - vMaxLogCnt
        .Col = 0
        .Text = vIndex
        .Col = 1
        If vPhotoNumber = -1 Then
            .Text = "No Photo"
        Else
            .Text = vPhotoNumber
        End If
        .Col = 2
        .Text = vSEnrollNumber
        .Col = 3
        .Text = vSMachineNumber
        .Col = 4
        If vVerifyMode = 0 Then
            .Text = "FP+ID"
        ElseIf vVerifyMode = 1 Then
            .Text = "FP"
        ElseIf vVerifyMode = 2 Then
            .Text = "Password"
        ElseIf vVerifyMode = 3 Then
            .Text = "Card"
        ElseIf vVerifyMode = 4 Then
            .Text = "FP+Card"
        ElseIf vVerifyMode = 5 Then
            .Text = "FP+Pwd"
        ElseIf vVerifyMode = 6 Then
            .Text = "Card+Pwd"
        ElseIf vVerifyMode = 7 Then
            .Text = "FP+Card+Pwd"
        ElseIf vVerifyMode = 10 Then
            .Text = "Hand Lock"
        ElseIf vVerifyMode = 11 Then
            .Text = "Prog Lock"
        ElseIf vVerifyMode = 12 Then
            .Text = "Prog Open"
        ElseIf vVerifyMode = 13 Then
            .Text = "Prog Close"
        ElseIf vVerifyMode = 14 Then
            .Text = "Auto Recover"
        ElseIf vVerifyMode = 20 Then
            .Text = "Lock Over"
        ElseIf vVerifyMode = 21 Then
            .Text = "Illegal Open"
        ElseIf vVerifyMode = 22 Then
            .Text = "Duress alarm"
        ElseIf vVerifyMode = 23 Then
            .Text = "Tamper detect"
        ElseIf vVerifyMode = 30 Then
            .Text = "FACE"
        ElseIf vVerifyMode = 31 Then
            .Text = "FACE+CARD"
        ElseIf vVerifyMode = 32 Then
            .Text = "FACE+PWD"
        ElseIf vVerifyMode = 33 Then
            .Text = "FACE+CARD+PWD"
        ElseIf vVerifyMode = 34 Then
            .Text = "FACE+FP"
        ElseIf vVerifyMode = 51 Then
            .Text = "FP"
        ElseIf vVerifyMode = 52 Then
            .Text = "Password"
        ElseIf vVerifyMode = 53 Then
            .Text = "Password"
        ElseIf vVerifyMode = 101 Then
            .Text = "FP"
        ElseIf vVerifyMode = 102 Then
            .Text = "Password"
        ElseIf vVerifyMode = 103 Then
            .Text = "Password"
        ElseIf vVerifyMode = 151 Then
            .Text = "FP"
        ElseIf vVerifyMode = 152 Then
            .Text = "Password"
        ElseIf vVerifyMode = 153 Then
            .Text = "Password"
        Else
            .Text = "--"
        End If
        
        If 0 <= vVerifyMode And vVerifyMode <= 7 Then
            .Text = .Text & stAttStatus
        ElseIf 30 <= vVerifyMode And vVerifyMode <= 33 Then
            .Text = .Text & stAttStatus
        ElseIf 51 <= vVerifyMode And vVerifyMode <= 53 Then
            .Text = .Text & stAttStatus
        ElseIf 101 <= vVerifyMode And vVerifyMode <= 103 Then
            .Text = .Text & stAttStatus
        ElseIf 151 <= vVerifyMode And vVerifyMode <= 153 Then
            .Text = .Text & stAttStatus
        End If
        .Text = .Text & stAntipass
        .Col = 5
        .Text = CStr(vYear) & "/" & Format(vMonth, "0#") & "/" & Format(vDay, "0#") & _
                " " & Format(vHour, "0#") & ":" & Format(vMinute, "0#")
        .Col = 6
        If vDaigong = 0 Then
            .Text = ""
        Else
            .Text = Format(vDaigong, "#")
        End If
    End With
End Sub



Private Sub cmdAllGLogData_Click()
    GlogSearched = True
    
    Dim vPhotoNumber As Long
    Dim vSMachineNumber As Long
    Dim vSEnrollNumber As Long
    Dim vVerifyMode As Long
    Dim vYear As Long
    Dim vMonth As Long
    Dim vDay As Long
    Dim vHour As Long
    Dim vMinute As Long
    Dim vSecond As Long
    Dim vErrorCode As Long
    Dim vRet As Boolean
    Dim i As Long, n As Long
    Dim vMaxLogCnt  As Long
    
    vMaxLogCnt = gMaxRow
    
    lblMessage.Caption = "Waiting..."
    LabelTotal.Caption = "Total : "
    DoEvents
    
    gridSLogData.Height = 4800
    gridSLogData.Redraw = False
    gridSLogData.Clear
    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData1.Height = 0
    gridSLogData1.Redraw = False
    gridSLogData1.Clear
    gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData2.Height = 0
    gridSLogData2.Redraw = False
    gridSLogData2.Clear
    
    gstrLogItem = Array("", "PhotoNo", "EnrollNo", "EMachineNo", "VeriMode", "DateTime", "Daigong")
    With gridSLogData
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 6
            .Col = i
            .Text = gstrLogItem(i)
            .ColAlignment(i) = 3
            .ColWidth(i) = 1200
        Next i
        .ColWidth(4) = 2000
        .ColWidth(5) = 2000
        .ColWidth(6) = 1500
        .ColWidth(7) = 700
        .ColWidth(8) = 700
       n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    With gridSLogData1
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 6
            .Col = i
            .ColAlignment(i) = 3
            .ColWidth(i) = 1200
        Next i
        .ColWidth(4) = 2000
        .Col = 5
        .ColWidth(5) = 2000
        .ColWidth(6) = 1500
        .ColWidth(7) = 700
        .ColWidth(8) = 700
        n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    With gridSLogData2
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 6
            .Col = i
            .ColAlignment(i) = 3
            .ColWidth(i) = 1200
        Next i
        .ColWidth(4) = 2000
        .Col = 5
        .ColWidth(5) = 2000
        .ColWidth(6) = 1500
        .ColWidth(7) = 700
        .ColWidth(8) = 700
        n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    
    MousePointer = vbHourglass
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        MousePointer = vbDefault
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ReadAllGLogData(mMachineNumber)
    If vRet = False Then
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    Else
        If chkAndDelete.Value = 1 Then
            frmMain.SB100BPC1.EmptyGeneralLogData (mMachineNumber)
        End If
    End If
    
    If vRet = True Then
        lblMessage.Caption = "Getting..."
        MousePointer = vbHourglass
        DoEvents
        gridSLogData.Redraw = False
        gridSLogData1.Redraw = False
        gridSLogData2.Redraw = False
        With gridSLogData
            i = 1
            Do

                vRet = frmMain.SB100BPC1.GetAllGLogData(mMachineNumber, _
                                                 vPhotoNumber, _
                                                 vSEnrollNumber, _
                                                 vSMachineNumber, _
                                                 vVerifyMode, _
                                                 vYear, _
                                                 vMonth, _
                                                 vDay, _
                                                 vHour, _
                                                 vMinute, _
                                                 vSecond)
                If vRet = False Then Exit Do
                If vRet = True And i <> 1 Then
                    .AddItem (1)
                End If
                
                DrawOneGeneralLog mMachineNumber, _
                                    vPhotoNumber, _
                                    vSEnrollNumber, _
                                    vSMachineNumber, _
                                    vVerifyMode, _
                                    vYear, _
                                    vMonth, _
                                    vDay, _
                                    vHour, _
                                    vMinute, _
                                    vSecond, _
                                    0, _
                                    i, _
                                    gridSLogData
                                    
                LabelTotal.Caption = "Total : " & i
                DoEvents
                i = i + 1
                If i > vMaxLogCnt Then Exit Do
            Loop
        End With
        
        If i > vMaxLogCnt Then
            gridSLogData.Height = gridSLogData.Height / 2
            gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
            gridSLogData1.Height = gridSLogData.Height
            With gridSLogData1
                Do
                    vRet = frmMain.SB100BPC1.GetGeneralLogData(mMachineNumber, _
                                                        vPhotoNumber, _
                                                        vSEnrollNumber, _
                                                        vSMachineNumber, _
                                                        vVerifyMode, _
                                                        vYear, _
                                                        vMonth, _
                                                        vDay, _
                                                        vHour, _
                                                        vMinute, _
                                                        vSecond)
                If vRet = False Then Exit Do
                If vRet = True And i <> 1 Then
                    If i - vMaxLogCnt > 1 Then .AddItem (1)
                End If
               
                DrawOneGeneralLog mMachineNumber, _
                                    vPhotoNumber, _
                                    vSEnrollNumber, _
                                    vSMachineNumber, _
                                    vVerifyMode, _
                                    vYear, _
                                    vMonth, _
                                    vDay, _
                                    vHour, _
                                    vMinute, _
                                    vSecond, _
                                    vMaxLogCnt, _
                                    i, _
                                    gridSLogData1
                                    
                LabelTotal.Caption = "Total : " & i
                DoEvents
                i = i + 1
                If i > vMaxLogCnt * 2 Then Exit Do
                Loop
            End With
        End If
        vMaxLogCnt = vMaxLogCnt * 2
        If i > vMaxLogCnt Then
            gridSLogData.Height = gridSLogData.Height * 2 / 3
            gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
            gridSLogData1.Height = gridSLogData.Height
            gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height * 2
            gridSLogData2.Height = gridSLogData.Height
            With gridSLogData2
                Do
                    vRet = frmMain.SB100BPC1.GetGeneralLogData(mMachineNumber, _
                                                        vPhotoNumber, _
                                                        vSEnrollNumber, _
                                                        vSMachineNumber, _
                                                        vVerifyMode, _
                                                        vYear, _
                                                        vMonth, _
                                                        vDay, _
                                                        vHour, _
                                                        vMinute, _
                                                        vSecond)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        If i - vMaxLogCnt > 1 Then .AddItem (1)
                    End If
               
                    DrawOneGeneralLog mMachineNumber, _
                                        vPhotoNumber, _
                                        vSEnrollNumber, _
                                        vSMachineNumber, _
                                        vVerifyMode, _
                                        vYear, _
                                        vMonth, _
                                        vDay, _
                                        vHour, _
                                        vMinute, _
                                        vSecond, _
                                        vMaxLogCnt * 2, _
                                        i, _
                                        gridSLogData2
                                        
                    LabelTotal.Caption = "Total : " & i
                    DoEvents
                    i = i + 1
                    If i > gMaxRow * 3 Then Exit Do
             Loop
            End With
        End If
        gridSLogData.Redraw = True
        gridSLogData1.Redraw = True
        gridSLogData2.Redraw = True
        
        lblMessage.Caption = "ReadAllGLogData OK"
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
    MousePointer = vbDefault
End Sub

Private Sub cmdAllSLogData_Click()
    GlogSearched = False
    
    Dim vTMachineNumber As Long
    Dim vSMachineNumber As Long
    Dim vSEnrollNumber As Long
    Dim vGEnrollNumber As Long
    Dim vGMachineNumber As Long
    Dim vManipulation As Long
    Dim vFingerNumber As Long
    Dim vYear As Long
    Dim vMonth As Long
    Dim vDay As Long
    Dim vHour As Long
    Dim vMinute As Long
    Dim vSecond As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i, n As Long
    
    gridSLogData.Height = 4800
    gridSLogData.Redraw = False
    gridSLogData.Clear
    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData1.Height = 0
    gridSLogData1.Redraw = False
    gridSLogData1.Clear
    gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData2.Height = 0
    gridSLogData2.Redraw = False
    gridSLogData2.Clear
    
    lblMessage.Caption = "Waiting..."
    LabelTotal.Caption = "Total : "
    DoEvents
    gridSLogData.Redraw = False
    gridSLogData.Clear
    gstrLogItem = Array("", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime")
    
    With gridSLogData
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 8
            .Col = i
            .Text = gstrLogItem(i)
            .ColWidth(i) = 900
            .ColAlignment(i) = 3
        Next i
        .ColWidth(6) = 2000
        .ColAlignment(6) = 2
        .ColWidth(7) = 800
        .Col = 8
        .Text = gstrLogItem(8)
        .ColWidth(8) = 2000
        n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    
    MousePointer = vbHourglass
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        MousePointer = vbDefault
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ReadAllSLogData(mMachineNumber)
    If vRet = False Then
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    Else
        If chkAndDelete.Value = 1 Then
            frmMain.SB100BPC1.EmptySuperLogData (mMachineNumber)
        End If
    End If
    
    If vRet = True Then
        lblMessage.Caption = "Getting..."
        MousePointer = vbHourglass
        DoEvents
        With gridSLogData
            .Redraw = False
            i = 1
            Do
                vRet = frmMain.SB100BPC1.GetAllSLogData(mMachineNumber, _
                                                        vTMachineNumber, _
                                                        vSEnrollNumber, _
                                                        vSMachineNumber, _
                                                        vGEnrollNumber, _
                                                        vGMachineNumber, _
                                                        vManipulation, _
                                                        vFingerNumber, _
                                                        vYear, _
                                                        vMonth, _
                                                        vDay, _
                                                        vHour, _
                                                        vMinute, _
                                                        vSecond)
                If vRet = False Then Exit Do
                If vRet = True And i <> 1 Then
                    .AddItem (1)
                End If

                .Row = i
                .Col = 0
                .Text = i
                .Col = 1
                .Text = vTMachineNumber
                .Col = 2
                .Text = vSEnrollNumber
                .Col = 3
                .Text = vSMachineNumber
                .Col = 4
                .Text = vGEnrollNumber
                .Col = 5
                .Text = vGMachineNumber
                .Col = 6
                Select Case vManipulation
                    Case 3
                        .Text = vManipulation & "--" & "Enroll User"
                    Case 4
                        .Text = vManipulation & "--" & "Enroll Manager"
                    Case 5
                        .Text = vManipulation & "--" & "Delete Fp Data"
                    Case 6
                        .Text = vManipulation & "--" & "Delete Password"
                    Case 7
                        .Text = vManipulation & "--" & "Delete Card Data"
                    Case 8
                        .Text = vManipulation & "--" & "Delete All LogData"
                    Case 9
                        .Text = vManipulation & "--" & "Modify System Info"
                    Case 10
                        .Text = vManipulation & "--" & "Modify System Time"
                    Case 11
                        .Text = vManipulation & "--" & "Modify Log Setting"
                    Case 12
                        .Text = vManipulation & "--" & "Modify Comm Setting"
                    Case 13
                        .Text = vManipulation & "--" & "Modify Timezone Setting"
                    Case 14
                        .Text = vManipulation & "--" & "Delete Face"
                    Case Else
                        .Text = "--Unknown--"
                End Select
                
                .Col = 7
                If vFingerNumber < 10 Then
                    .Text = vFingerNumber
                ElseIf vFingerNumber = 10 Then
                    .Text = "Password"
                ElseIf vFingerNumber = 14 Then
                    .Text = "Face"
                Else
                    .Text = "Card"
                End If
                .Col = 8
                .Text = CStr(vYear) & "/" & Format(vMonth, "0#") & "/" & Format(vDay, "0#") & _
                        " " & Format(vHour, "0#") & ":" & Format(vMinute, "0#")
                
                LabelTotal.Caption = "Total : " & i
                DoEvents
                i = i + 1
            Loop
            .Redraw = True
        End With
        lblMessage.Caption = "ReadAllSLogData OK"
    End If
    
    MousePointer = vbDefault
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdEmptyGLog_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.EmptyGeneralLogData(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "EmptyGeneralLogData OK"
    Else
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    End If
    
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdEmptySLog_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.EmptySuperLogData(mMachineNumber)
    If vRet = True Then
        lblMessage.Caption = "EmptySuperLogData OK"
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

Private Sub cmdGlogData_Click()
    GlogSearched = True
    
    Dim vPhotoNumber As Long
    Dim vSMachineNumber As Long
    Dim vSEnrollNumber As Long
    Dim vInOutMode As Long
    Dim vVerifyMode As Long
    Dim vYear As Long
    Dim vMonth As Long
    Dim vDay As Long
    Dim vHour As Long
    Dim vMinute As Long
    Dim vSecond As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i As Long, n As Long
    Dim vMaxLogCnt As Long
    
    vMaxLogCnt = gMaxRow
    
    lblMessage.Caption = "Waiting..."
    LabelTotal.Caption = "Total : "
    DoEvents
    
    gridSLogData.Height = 4800
    gridSLogData.Redraw = False
    gridSLogData.Clear
    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData1.Height = 0
    gridSLogData1.Redraw = False
    gridSLogData1.Clear
    gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData2.Height = 0
    gridSLogData2.Redraw = False
    gridSLogData2.Clear
  
    gstrLogItem = Array("", "PhotoNo", "EnrollNo", "EMachineNo", "VeriMode", "DateTime", "DaiGong")
    With gridSLogData
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 6
            .Col = i
            .Text = gstrLogItem(i)
            .ColAlignment(i) = 3
            .ColWidth(i) = 1200
        Next i
        .ColWidth(4) = 2000
        .Col = 5
        .ColWidth(5) = 2000
        .ColWidth(6) = 1500
        .ColWidth(7) = 700
        .ColWidth(8) = 700
        n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    With gridSLogData1
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 6
            .Col = i
            .ColAlignment(i) = 3
            .ColWidth(i) = 1200
        Next i
        .ColWidth(4) = 2000
        .Col = 5
        .ColWidth(5) = 2000
        .ColWidth(6) = 1500
        .ColWidth(7) = 700
        .ColWidth(8) = 700
        n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    With gridSLogData2
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 6
            .Col = i
            .ColAlignment(i) = 3
            .ColWidth(i) = 1200
        Next i
        .ColWidth(4) = 2000
        .Col = 5
        .ColWidth(5) = 2000
        .ColWidth(6) = 1500
        .ColWidth(7) = 700
        .ColWidth(8) = 700
        n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    
    MousePointer = vbHourglass
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        MousePointer = vbDefault
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ReadGeneralLogData(mMachineNumber)
    If vRet = False Then
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    Else
        If chkAndDelete.Value = 1 Then
            frmMain.SB100BPC1.EmptyGeneralLogData (mMachineNumber)
        End If
    End If
    
    If vRet = True Then
        MousePointer = vbHourglass
        lblMessage.Caption = "Getting ..."
        DoEvents
        gridSLogData.Redraw = False
        gridSLogData1.Redraw = False
        gridSLogData2.Redraw = False
        With gridSLogData
            i = 1
            Do
                vRet = frmMain.SB100BPC1.GetGeneralLogData(mMachineNumber, _
                                                    vPhotoNumber, _
                                                    vSEnrollNumber, _
                                                    vSMachineNumber, _
                                                    vVerifyMode, _
                                                    vYear, _
                                                    vMonth, _
                                                    vDay, _
                                                    vHour, _
                                                    vMinute, _
                                                    vSecond)
                If vRet = False Then Exit Do
                If vRet = True And i <> 1 Then
                    .AddItem (1)
                End If
                
                DrawOneGeneralLog mMachineNumber, _
                                  vPhotoNumber, _
                                  vSEnrollNumber, _
                                  vSMachineNumber, _
                                  vVerifyMode, _
                                  vYear, _
                                  vMonth, _
                                  vDay, _
                                  vHour, _
                                  vMinute, _
                                  vSecond, _
                                  0, _
                                  i, _
                                  gridSLogData
                
                LabelTotal.Caption = "Total : " & i
                DoEvents
                i = i + 1
                If i > vMaxLogCnt Then Exit Do
            Loop
        End With
        
        If i > vMaxLogCnt Then
            gridSLogData.Height = gridSLogData.Height / 2
            gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
            gridSLogData1.Height = gridSLogData.Height
            With gridSLogData1
                Do
                    vRet = frmMain.SB100BPC1.GetGeneralLogData(mMachineNumber, _
                                                        vPhotoNumber, _
                                                        vSEnrollNumber, _
                                                        vSMachineNumber, _
                                                        vVerifyMode, _
                                                        vYear, _
                                                        vMonth, _
                                                        vDay, _
                                                        vHour, _
                                                        vMinute, _
                                                        vSecond)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        If i - vMaxLogCnt > 1 Then .AddItem (1)
                    End If
                
                    DrawOneGeneralLog mMachineNumber, _
                                        vPhotoNumber, _
                                        vSEnrollNumber, _
                                        vSMachineNumber, _
                                        vVerifyMode, _
                                        vYear, _
                                        vMonth, _
                                        vDay, _
                                        vHour, _
                                        vMinute, _
                                        vSecond, _
                                        vMaxLogCnt, _
                                        i, _
                                        gridSLogData1
                    
                    LabelTotal.Caption = "Total : " & i
                    DoEvents
                    i = i + 1
                    If i > vMaxLogCnt * 2 Then Exit Do
                Loop
            End With
        End If
'        vMaxLogCnt = vMaxLogCnt * 2
'        If i > vMaxLogCnt Then
'            gridSLogData.Height = gridSLogData.Height * 2 / 3
'            gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
'            gridSLogData1.Height = gridSLogData.Height
'            gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height * 2
'            gridSLogData2.Height = gridSLogData.Height
'            With gridSLogData2
'                Do
'                    vRet = frmMain.SB100BPC1.GetGeneralLogData(mMachineNumber, _
'                                                        vTMachineNumber, _
'                                                        vSEnrollNumber, _
'                                                        vSMachineNumber, _
'                                                        vVerifyMode, _
'                                                        vYear, _
'                                                        vMonth, _
'                                                        vDay, _
'                                                        vHour, _
'                                                        vMinute, _
'                                                        vSecond)
'                    If vRet = False Then Exit Do
'                    If vRet = True And i <> 1 Then
'                        If i - vMaxLogCnt > 1 Then .AddItem (1)
'                    End If
'
'                    DrawOneGeneralLog mMachineNumber, _
'                                        vTMachineNumber, _
'                                        vSEnrollNumber, _
'                                        vSMachineNumber, _
'                                        vVerifyMode, _
'                                        vYear, _
'                                        vMonth, _
'                                        vDay, _
'                                        vHour, _
'                                        vMinute, _
'                                        vSecond, _
'                                        vMaxLogCnt, _
'                                        i, _
'                                        gridSLogData2
'
'                    LabelTotal.Caption = "Total : " & i
'                    DoEvents
'                    i = i + 1
'                    If i > gMaxRow * 3 Then Exit Do
'                Loop
'            End With
'        End If
        gridSLogData.Redraw = True
        gridSLogData1.Redraw = True
        gridSLogData2.Redraw = True
        
        lblMessage.Caption = "ReadGeneralLogData OK"
    End If
    
    MousePointer = vbDefault
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub cmdGLogSearchRange_Click()
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim strXML As String
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        GoTo 1:
    End If
    
    frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "SetGLogSearchRange"
    frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
    frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
    frmMain.SB100BPC1.XML_AddBoolean strXML, "UseSearchRange", chkUseSearchRange.Value
    
    If chkUseSearchRange.Value = 1 Then
        frmMain.SB100BPC1.XML_AddLong strXML, "StartYear", dtGLogSearchStartTime.Year
        frmMain.SB100BPC1.XML_AddLong strXML, "StartMonth", dtGLogSearchStartTime.Month
        frmMain.SB100BPC1.XML_AddLong strXML, "StartDate", dtGLogSearchStartTime.Day
        frmMain.SB100BPC1.XML_AddLong strXML, "EndYear", dtGLogSearchEndTime.Year
        frmMain.SB100BPC1.XML_AddLong strXML, "EndMonth", dtGLogSearchEndTime.Month
        frmMain.SB100BPC1.XML_AddLong strXML, "EndDate", dtGLogSearchEndTime.Day
    End If
    
    vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
    
    If vRet = False Then
            frmMain.SB100BPC1.GetLastError vErrorCode
            lblMessage.Caption = ErrorPrint(vErrorCode)
        GoTo 1:
    End If
        
    lblMessage.Caption = "SetGLogSearchRange OK"
1:
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, True)
End Sub

Private Sub cmdSLogData_Click()
    GlogSearched = False
    
    Dim vTMachineNumber As Long
    Dim vSMachineNumber As Long
    Dim vSEnrollNumber As Long
    Dim vGEnrollNumber As Long
    Dim vGMachineNumber As Long
    Dim vManipulation As Long
    Dim vFingerNumber As Long
    Dim vYear As Long
    Dim vMonth As Long
    Dim vDay As Long
    Dim vHour As Long
    Dim vMinute As Long
    Dim vSecond As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i, n As Long
    
    gridSLogData.Height = 4800
    gridSLogData.Redraw = False
    gridSLogData.Clear
    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData1.Height = 0
    gridSLogData1.Redraw = False
    gridSLogData1.Clear
    gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height
    gridSLogData2.Height = 0
    gridSLogData2.Redraw = False
    gridSLogData2.Clear
    
    lblMessage.Caption = "Waiting..."
    LabelTotal.Caption = "Total : "
    DoEvents
    
    gridSLogData.Redraw = False
    gridSLogData.Clear
    
    gstrLogItem = Array("", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime")
    With gridSLogData
        .Row = 0
        .ColWidth(0) = 600
        For i = 1 To 8
            .Col = i
            .Text = gstrLogItem(i)
            .ColAlignment(i) = 3
            .ColWidth(i) = 900
        Next i
        .Col = 6
        .ColWidth(6) = 2000
        .ColAlignment(6) = 2
        .ColWidth(7) = 800
        .Col = 8
        .Text = gstrLogItem(8)
        .ColWidth(8) = 2000
        n = .Rows
        If n > 2 Then
            Do
                If n = 2 Then Exit Do
                .RemoveItem (n)
                n = n - 1
            Loop
        End If
        .Redraw = True
    End With
    
    MousePointer = vbHourglass
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        MousePointer = vbDefault
        Exit Sub
    End If
    
    vRet = frmMain.SB100BPC1.ReadSuperLogData(mMachineNumber)
    If vRet = False Then
        frmMain.SB100BPC1.GetLastError vErrorCode
        lblMessage.Caption = ErrorPrint(vErrorCode)
    Else
        If chkAndDelete.Value = 1 Then
            frmMain.SB100BPC1.EmptySuperLogData (mMachineNumber)
        End If
    End If
    
    If vRet = True Then
        MousePointer = vbHourglass
        lblMessage.Caption = "Getting ..."
        DoEvents
        With gridSLogData
            .Redraw = False
            i = 1
            Do
                vRet = frmMain.SB100BPC1.GetSuperLogData(mMachineNumber, _
                                                        vTMachineNumber, _
                                                        vSEnrollNumber, _
                                                        vSMachineNumber, _
                                                        vGEnrollNumber, _
                                                        vGMachineNumber, _
                                                        vManipulation, _
                                                        vFingerNumber, _
                                                        vYear, _
                                                        vMonth, _
                                                        vDay, _
                                                        vHour, _
                                                        vMinute, _
                                                        vSecond)
                If vRet = False Then Exit Do
                If vRet = True And i <> 1 Then
                    .AddItem (1)
                End If

                .Row = i
                .Col = 0
                .Text = i
                .Col = 1
                .Text = vTMachineNumber
                .Col = 2
                .Text = vSEnrollNumber
                .Col = 3
                .Text = vSMachineNumber
                .Col = 4
                .Text = vGEnrollNumber
                .Col = 5
                .Text = vGMachineNumber
                .Col = 6
                Select Case vManipulation
                    Case 3
                        .Text = vManipulation & "--" & "Enroll User"
                    Case 4
                        .Text = vManipulation & "--" & "Enroll Manager"
                    Case 5
                        .Text = vManipulation & "--" & "Delete Fp Data"
                    Case 6
                        .Text = vManipulation & "--" & "Delete Password"
                    Case 7
                        .Text = vManipulation & "--" & "Delete Card Data"
                    Case 8
                        .Text = vManipulation & "--" & "Delete All LogData"
                    Case 9
                        .Text = vManipulation & "--" & "Modify System Info"
                    Case 10
                        .Text = vManipulation & "--" & "Modify System Time"
                    Case 11
                        .Text = vManipulation & "--" & "Modify Log Setting"
                    Case 12
                        .Text = vManipulation & "--" & "Modify Comm Setting"
                    Case 13
                        .Text = vManipulation & "--" & "Modify Timezone Setting"
                    Case 14
                        .Text = vManipulation & "--" & "Delete Face"
                    Case Else
                        .Text = "--Unknown--"
                End Select
                .Col = 7
                If vFingerNumber < 10 Then
                    .Text = vFingerNumber
                ElseIf vFingerNumber = 10 Then
                    .Text = "Password"
                ElseIf vFingerNumber = 14 Then
                    .Text = "Face"
                Else
                    .Text = "Card"
                End If
                .Col = 8
                .Text = CStr(vYear) & "/" & Format(vMonth, "0#") & "/" & Format(vDay, "0#") & _
                        " " & Format(vHour, "0#") & ":" & Format(vMinute, "0#")
                
                LabelTotal.Caption = "Total : " & i
                DoEvents
                i = i + 1
            Loop
            .Redraw = True
        End With
        lblMessage.Caption = "ReadSuperLogData OK"
    End If
    
    MousePointer = vbDefault
    frmMain.SB100BPC1.EnableDevice mMachineNumber, True
End Sub

Private Sub Form_Load()
    mMachineNumber = frmMain.gMachineNumber
    GlogSearched = False
    chkReadMark.Value = 1
    prevSelectLogIndex = -1
    gTempPhotoFileName = "C:\TempPhoto.jpg"
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Me.Visible = False
    frmMain.Visible = True
End Sub

Private Sub gridSLogData_Click()
    If GlogSearched = False Then Exit Sub
    If chkShowGLogPhoto.Value = False Then Exit Sub
    If gridSLogData.Row = prevSelectLogIndex Then Exit Sub
    prevSelectLogIndex = gridSLogData.Row
    If gridSLogData.TextMatrix(gridSLogData.Row, 1) = "No Photo" Then GoTo 2:
    Dim strXML As String
    Dim photoData() As Byte
    Dim photoNumber As Long
    Dim vRet As Boolean
    Dim vErrorCode As Long
    Dim i As Long
    
    lblMessage.Caption = "Working..."
    DoEvents
    
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, False)
    If vRet = False Then
        lblMessage.Caption = gstrNoDevice
        GoTo 1:
    End If
    With gridSLogData
        photoNumber = Val(.TextMatrix(.Row, 1))
        
        frmMain.SB100BPC1.XML_AddString strXML, "REQUEST", "GetGLogPhotoData"
        frmMain.SB100BPC1.XML_AddString strXML, "MSGTYPE", "request"
        frmMain.SB100BPC1.XML_AddInt strXML, "MachineID", mMachineNumber
        frmMain.SB100BPC1.XML_AddLong strXML, "PhotoPos", photoNumber
        
        vRet = frmMain.SB100BPC1.GeneralOperationXML(strXML)
        
        If vRet = False Then
            frmMain.SB100BPC1.GetLastError vErrorCode
            lblMessage.Caption = ErrorPrint(vErrorCode)
            GoTo 1:
        End If
        lblMessage.Caption = "GetGLogPhotoData OK"
        ReDim photoData(gCompPhotoSize) As Byte
        vRet = frmMain.SB100BPC1.XML_ParseBinaryByte(strXML, "PhotoData", photoData(0), gCompPhotoSize)
        
        If vRet = True Then
            Open gTempPhotoFileName For Binary Access Write Lock Read Write As #1
            For i = 0 To gCompPhotoSize - 1
                Put #1, , photoData(i)
            Next i
            Close #1
        Else
            lblMessage.Caption = "GetGLogPhotoData - XML Parse Error!"
            GoTo 2:
        End If
    End With
1:
    vRet = frmMain.SB100BPC1.EnableDevice(mMachineNumber, True)
    On Error GoTo 2:
    imgGlogPhoto.Picture = LoadPicture(gTempPhotoFileName)
    Exit Sub
2:
    imgGlogPhoto.Picture = LoadPicture()
End Sub

