Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Runtime.InteropServices

Friend Class frmLog
    Inherits System.Windows.Forms.Form

    Const gMaxLow As Short = 30000
    Dim mMachineNumber As Integer
    Dim gGlogSearched As Boolean = False
    Dim prevGlogIndex As Integer = -1
    Public gstrLogItem As Object

    Const PhotoTag_UserPhotoTag As Integer = 1
    Const PhotoTag_GlogPhotoTag As Integer = 2

    Private Sub ShowGlogItem(ByVal vTMachineNumber As Integer, ByVal vSEnrollNumber As String, ByVal vSMachineNumber As Integer, ByVal vVerifyMode As Integer, ByVal vYear As Integer, ByVal vMonth As Integer, ByVal vDay As Integer, ByVal vHour As Integer, ByVal vMinute As Integer, ByVal vSecond As Integer, ByVal vGlogExt As Integer, ByVal vIndex As Integer, ByVal vMaxLogCnt As Integer, ByVal gridGlogData As AxMSFlexGridLib.AxMSFlexGrid)
        Dim vAttStatus As Integer, vAntipass As Integer
        Dim stAttStatus As String = "", stAntipass As String = ""
        Dim vDiv As Integer = 65536

        vAntipass = vVerifyMode / vDiv
        vVerifyMode = vVerifyMode Mod vDiv
        vAttStatus = vVerifyMode / 256
        vVerifyMode = vVerifyMode Mod 256

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
        ElseIf vAntipass = 3 Then
            stAntipass = "(AP_Out)"
        End If


        Dim body_temp_str As String = ""
        Const BodyTemperature100_Glog_Delta0 As Integer = 3000
        Dim body_temperature_10_delta As Integer = vGlogExt And &HFF   ' (body_temperature_10_delta: 8, mood: 2, reserved: 22)

        If (body_temperature_10_delta > 0) Then
            Dim body_temperature_100 As Integer = body_temperature_10_delta
            body_temperature_100 = body_temperature_100 * 10 + BodyTemperature100_Glog_Delta0
            body_temp_str = Int(body_temperature_100 / 100).ToString() & "." & Int((body_temperature_100 Mod 100) / 10).ToString() + "'C"
        End If
        With gridGlogData
            .Row = vIndex - vMaxLogCnt
            .Col = 0
            .Text = vIndex
            .Col = 1
            If vTMachineNumber = -1 Then
                .Text = "No Photo"
            Else
                .Text = CStr(vTMachineNumber)
            End If
            .Col = 2
            .Text = vSEnrollNumber
            .Col = 3
            .Text = body_temp_str
            .Col = 4
            .Text = CStr(vSMachineNumber)
            .Col = 5
            vVerifyMode = vVerifyMode Mod 256
            While (vVerifyMode > 50)
                vVerifyMode = vVerifyMode - 50
            End While
            If vVerifyMode = 1 Then
                .Text = "Fp"
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
            Else
                .Text = "--"
            End If

            If 1 <= vVerifyMode And vVerifyMode <= 7 Then
                .Text = .Text & stAttStatus
            ElseIf 30 <= vVerifyMode And vVerifyMode <= 34 Then
                .Text = .Text + stAttStatus
            End If

            gridGlogData.Text = gridGlogData.Text + stAntipass

            .Col = 6
            .Text = CStr(vYear) & "/" & VB6.Format(vMonth, "0#") & "/" & VB6.Format(vDay, "0#") & " " & VB6.Format(vHour, "0#") & ":" & VB6.Format(vMinute, "0#")
        End With
    End Sub

    Private Sub cmdAllGLogData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAllGLogData.Click
        gGlogSearched = True

        Dim vTMachineNumber As Integer
        Dim vSMachineNumber As Integer
        Dim vSEnrollNumber As Integer
        Dim strEnrollNumber As String
        Dim vVerifyMode As Integer
        Dim vYear As Integer
        Dim vMonth As Integer
        Dim vDay As Integer
        Dim vHour As Integer
        Dim vMinute As Integer
        Dim vSecond As Integer
        Dim vGlogExt As Integer
        Dim vErrorCode As Integer
        Dim vRet As Boolean
        Dim i As Integer
        Dim n As Integer
        Dim vMaxLogCnt As Integer

        vMaxLogCnt = gMaxLow

        lblMessage.Text = "Waiting..."
        LabelTotal.Text = "Total : "
        System.Windows.Forms.Application.DoEvents()

        gridSLogData.Height = VB6.TwipsToPixelsY(4800)
        gridSLogData.Redraw = False
        gridSLogData.Clear()
        gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData1.Height = 0
        gridSLogData1.Redraw = False
        gridSLogData1.Clear()
        gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData2.Height = 0
        gridSLogData2.Redraw = False
        gridSLogData2.Clear()

        gstrLogItem = New Object() {"", "Photo No", "EnrollNo", "BodyTemp", "EMachineNo", "VeriMode", "DateTime"}
        With gridSLogData
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .Text = gstrLogItem(i)
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With
        With gridSLogData1
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With
        With gridSLogData2
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With

        If Not (chkReadGlogExt.Checked) Then
            gridSLogData.set_ColWidth(3, 0)     ' col 3 : visible = false
        End If

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        vRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If (chkReadGlogExt.Checked) Then
            vRet = sbxpc.SBXPCDLL.ReadAllGLogData_Ext(mMachineNumber)
        Else
            vRet = sbxpc.SBXPCDLL.ReadAllGLogData(mMachineNumber)
        End If
        If vRet = False Then
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        Else
            If chkAndDelete.CheckState = 1 Then
                sbxpc.SBXPCDLL.EmptyGeneralLogData(mMachineNumber)
            End If
        End If

        If vRet = True Then
            lblMessage.Text = "Getting..."
            Cursor = System.Windows.Forms.Cursors.WaitCursor
            System.Windows.Forms.Application.DoEvents()
            gridSLogData.Redraw = False
            gridSLogData1.Redraw = False
            gridSLogData2.Redraw = False
            With gridSLogData
                i = 1
                Do
                    If (chkReadGlogExt.Checked) Then
                        vRet = sbxpc.SBXPCDLL.GetAllGLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                    Else
                        vRet = sbxpc.SBXPCDLL.GetAllGLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                    End If
                    sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        .AddItem(CStr(1))
                    End If

                    ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, 0, gridSLogData)

                    LabelTotal.Text = "Total : " & i
                    System.Windows.Forms.Application.DoEvents()
                    i = i + 1
                    If i > vMaxLogCnt Then Exit Do
                Loop
            End With

            If i > vMaxLogCnt Then
                gridSLogData.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Height) / 2)
                gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
                gridSLogData1.Height = gridSLogData.Height
                With gridSLogData1
                    Do
                        If (chkReadGlogExt.Checked) Then
                            vRet = sbxpc.SBXPCDLL.GetAllGLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                        Else
                            vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                        End If
                        sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                        If vRet = False Then Exit Do
                        If vRet = True And i <> 1 Then
                            If i - vMaxLogCnt > 1 Then .AddItem(CStr(1))
                        End If

                        ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, vMaxLogCnt, gridSLogData1)

                        LabelTotal.Text = "Total : " & i
                        System.Windows.Forms.Application.DoEvents()
                        i = i + 1
                        If i > vMaxLogCnt * 2 Then Exit Do
                    Loop
                End With
            End If
            vMaxLogCnt = vMaxLogCnt * 2
            If i > vMaxLogCnt Then
                gridSLogData.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Height) * 2 / 3)
                gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
                gridSLogData1.Height = gridSLogData.Height
                gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height) * 2)
                gridSLogData2.Height = gridSLogData.Height
                With gridSLogData2
                    Do
                        If (chkReadGlogExt.Checked) Then
                            vRet = sbxpc.SBXPCDLL.GetAllGLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                        Else
                            vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                        End If
                        sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                        If vRet = False Then Exit Do
                        If vRet = True And i <> 1 Then
                            If i - vMaxLogCnt > 1 Then .AddItem(CStr(1))
                        End If

                        ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, vMaxLogCnt, gridSLogData2)

                        LabelTotal.Text = "Total : " & i
                        System.Windows.Forms.Application.DoEvents()
                        i = i + 1
                    Loop
                End With
            End If
            gridSLogData.Redraw = True
            gridSLogData1.Redraw = True
            gridSLogData2.Redraw = True

            lblMessage.Text = "ReadAllGLogData OK"
        End If

        sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
        Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub cmdAllSLogData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAllSLogData.Click
        gGlogSearched = False

        Dim vTMachineNumber As Integer
        Dim vSMachineNumber As Integer
        Dim vSEnrollNumber As Integer
        Dim vGEnrollNumber As Integer
        Dim sgEnrollNumber As String
        Dim vGMachineNumber As Integer
        Dim vManipulation As Integer
        Dim vFingerNumber As Integer
        Dim vYear As Integer
        Dim vMonth As Integer
        Dim vDay As Integer
        Dim vHour As Integer
        Dim vMinute As Integer
        Dim vSecond As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim i As Integer
        Dim n As Integer

        gridSLogData.Height = VB6.TwipsToPixelsY(4800)
        gridSLogData.Redraw = False
        gridSLogData.Clear()
        gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData1.Height = 0
        gridSLogData1.Redraw = False
        gridSLogData1.Clear()
        gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData2.Height = 0
        gridSLogData2.Redraw = False
        gridSLogData2.Clear()

        lblMessage.Text = "Waiting..."
        LabelTotal.Text = "Total : "
        System.Windows.Forms.Application.DoEvents()
        gridSLogData.Redraw = False
        gridSLogData.Clear()
        gstrLogItem = New Object() {"", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime"}

        With gridSLogData
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 8
                .Col = i
                .Text = gstrLogItem(i)
                .set_ColWidth(i, 900)
                .set_ColAlignment(i, 3)
            Next i
            .set_ColWidth(6, 2000)
            .set_ColAlignment(6, 2)
            .set_ColWidth(7, 800)
            .Col = 8
            .Text = gstrLogItem(8)
            .set_ColWidth(8, 2000)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        vRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        vRet = sbxpc.SBXPCDLL.ReadAllSLogData(mMachineNumber)
        If vRet = False Then
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        Else
            If chkAndDelete.CheckState = 1 Then
                sbxpc.SBXPCDLL.EmptySuperLogData(mMachineNumber)
            End If
        End If

        If vRet = True Then
            lblMessage.Text = "Getting..."
            Cursor = System.Windows.Forms.Cursors.WaitCursor
            System.Windows.Forms.Application.DoEvents()
            With gridSLogData
                .Redraw = False
                i = 1
                Do
                    vRet = sbxpc.SBXPCDLL.GetAllSLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vGEnrollNumber, vGMachineNumber, vManipulation, vFingerNumber, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                    sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, sgEnrollNumber)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        .AddItem(CStr(1))
                    End If

                    .Row = i
                    .Col = 0
                    .Text = i
                    .Col = 1
                    .Text = CStr(vTMachineNumber)
                    .Col = 2
                    .Text = CStr(vSEnrollNumber)
                    .Col = 3
                    .Text = CStr(vSMachineNumber)
                    .Col = 4
                    .Text = sgEnrollNumber
                    .Col = 5
                    .Text = CStr(vGMachineNumber)
                    .Col = 6
                    Select Case vManipulation
                        Case 1
                        Case 2
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
                    End Select

                    .Col = 7
                    If vFingerNumber < 10 Then
                        .Text = CStr(vFingerNumber)
                    ElseIf vFingerNumber = 10 Then
                        .Text = "Password"
                    ElseIf vFingerNumber = 14 Then
                        .Text = "Face"
                    Else
                        .Text = "Card"
                    End If
                    .Col = 8
                    .Text = CStr(vYear) & "/" & VB6.Format(vMonth, "0#") & "/" & VB6.Format(vDay, "0#") & " " & VB6.Format(vHour, "0#") & ":" & VB6.Format(vMinute, "0#")

                    LabelTotal.Text = "Total : " & i
                    System.Windows.Forms.Application.DoEvents()
                    i = i + 1
                Loop
                .Redraw = True
            End With
            lblMessage.Text = "ReadAllSLogData OK"
        End If

        Cursor = System.Windows.Forms.Cursors.Default
        sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdEmptyGLog_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEmptyGLog.Click
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = sbxpc.SBXPCDLL.EmptyGeneralLogData(mMachineNumber)
        If vRet = True Then
            lblMessage.Text = "EmptyGeneralLogData OK"
        Else
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdEmptySLog_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEmptySLog.Click
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = sbxpc.SBXPCDLL.EmptySuperLogData(mMachineNumber)
        If vRet = True Then
            lblMessage.Text = "EmptySuperLogData OK"
        Else
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdGlogData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGlogData.Click
        gGlogSearched = True

        Dim vTMachineNumber As Integer
        Dim vSMachineNumber As Integer
        Dim vSEnrollNumber As Integer
        Dim strEnrollNumber As String
        '		Dim vInOutMode As Integer
        Dim vVerifyMode As Integer
        Dim vYear As Integer
        Dim vMonth As Integer
        Dim vDay As Integer
        Dim vHour As Integer
        Dim vMinute As Integer
        Dim vSecond As Integer
        Dim vGlogExt As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim i As Integer
        Dim n As Integer
        Dim vMaxLogCnt As Integer

        vMaxLogCnt = gMaxLow

        lblMessage.Text = "Waiting..."
        LabelTotal.Text = "Total : "
        System.Windows.Forms.Application.DoEvents()

        gridSLogData.Height = VB6.TwipsToPixelsY(4800)
        gridSLogData.Redraw = False
        gridSLogData.Clear()
        gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData1.Height = 0
        gridSLogData1.Redraw = False
        gridSLogData1.Clear()
        gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData2.Height = 0
        gridSLogData2.Redraw = False
        gridSLogData2.Clear()

        gstrLogItem = New Object() {"", "Photo No", "EnrollNo", "BodyTemp", "EMachineNo", "VeriMode", "DateTime"}
        With gridSLogData
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .Text = gstrLogItem(i)
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With
        With gridSLogData1
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With
        With gridSLogData2
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With

        If Not (chkReadGlogExt.Checked) Then
            gridSLogData.set_ColWidth(3, 0)     ' col 3 : visible = false
        End If

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        vRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If (chkReadGlogExt.Checked) Then
            vRet = sbxpc.SBXPCDLL.ReadGeneralLogData_Ext(mMachineNumber, chkReadMark.Checked)
        Else
            vRet = sbxpc.SBXPCDLL.ReadGeneralLogData(mMachineNumber, chkReadMark.Checked)
        End If

        If vRet = False Then
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        Else
            If chkAndDelete.CheckState = 1 Then
                sbxpc.SBXPCDLL.EmptyGeneralLogData(mMachineNumber)
            End If
        End If

        If vRet = True Then
            Cursor = System.Windows.Forms.Cursors.WaitCursor
            lblMessage.Text = "Getting ..."
            System.Windows.Forms.Application.DoEvents()
            gridSLogData.Redraw = False
            gridSLogData1.Redraw = False
            gridSLogData2.Redraw = False
            With gridSLogData
                i = 1

                Do
                    If (chkReadGlogExt.Checked) Then
                        vRet = sbxpc.SBXPCDLL.GetGeneralLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                    Else
                        vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                    End If
                    sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        .AddItem(CStr(1))
                    End If

                    ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, 0, gridSLogData)

                    LabelTotal.Text = "Total : " & i
                    System.Windows.Forms.Application.DoEvents()
                    i = i + 1
                    If i > vMaxLogCnt Then Exit Do
                Loop
            End With

            If i > vMaxLogCnt Then
                gridSLogData.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Height) / 2)
                gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
                gridSLogData1.Height = gridSLogData.Height
                With gridSLogData1
                    Do
                        If (chkReadGlogExt.Checked) Then
                            vRet = sbxpc.SBXPCDLL.GetAllGLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                        Else
                            vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                        End If
                        sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                        If vRet = False Then Exit Do
                        If vRet = True And i <> 1 Then
                            If i - vMaxLogCnt > 1 Then .AddItem(CStr(1))
                        End If

                        ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, 0, gridSLogData)

                        LabelTotal.Text = "Total : " & i
                        System.Windows.Forms.Application.DoEvents()
                        i = i + 1
                        If i > vMaxLogCnt * 2 Then Exit Do
                    Loop
                End With
            End If
            vMaxLogCnt = vMaxLogCnt * 2
            If i > vMaxLogCnt Then
                gridSLogData.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Height) * 2 / 3)
                gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
                gridSLogData1.Height = gridSLogData.Height
                gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height) * 2)
                gridSLogData2.Height = gridSLogData.Height
                With gridSLogData2
                    Do
                        If (chkReadGlogExt.Checked) Then
                            vRet = sbxpc.SBXPCDLL.GetAllGLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                        Else
                            vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                        End If
                        sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                        If vRet = False Then Exit Do
                        If vRet = True And i <> 1 Then
                            If i - vMaxLogCnt > 1 Then .AddItem(CStr(1))
                        End If

                        ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, 0, gridSLogData)

                        LabelTotal.Text = "Total : " & i
                        System.Windows.Forms.Application.DoEvents()
                        i = i + 1
                    Loop
                End With
            End If
            gridSLogData.Redraw = True
            gridSLogData1.Redraw = True
            gridSLogData2.Redraw = True

            lblMessage.Text = "ReadGeneralLogData OK"
        End If

        Cursor = System.Windows.Forms.Cursors.Default
        sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdSLogData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSLogData.Click
        gGlogSearched = False

        Dim vTMachineNumber As Integer
        Dim vSMachineNumber As Integer
        Dim vSEnrollNumber As Integer
        Dim sgEnrollNumber As String
        Dim vGEnrollNumber As Integer
        Dim vGMachineNumber As Integer
        Dim vManipulation As Integer
        Dim vFingerNumber As Integer
        Dim vYear As Integer
        Dim vMonth As Integer
        Dim vDay As Integer
        Dim vHour As Integer
        Dim vMinute As Integer
        Dim vSecond As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim i As Integer
        Dim n As Integer

        gridSLogData.Height = VB6.TwipsToPixelsY(4800)
        gridSLogData.Redraw = False
        gridSLogData.Clear()
        gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData1.Height = 0
        gridSLogData1.Redraw = False
        gridSLogData1.Clear()
        gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData2.Height = 0
        gridSLogData2.Redraw = False
        gridSLogData2.Clear()

        lblMessage.Text = "Waiting..."
        LabelTotal.Text = "Total : "
        System.Windows.Forms.Application.DoEvents()

        gridSLogData.Redraw = False
        gridSLogData.Clear()

        gstrLogItem = New Object() {"", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime"}
        With gridSLogData
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 8
                .Col = i
                .Text = gstrLogItem(i)
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 900)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColAlignment(6, 2)
            .set_ColWidth(7, 800)
            .Col = 8
            .Text = gstrLogItem(8)
            .set_ColWidth(8, 2000)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        vRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        vRet = sbxpc.SBXPCDLL.ReadSuperLogData(mMachineNumber, chkReadMark.Checked)
        If vRet = False Then
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        Else
            If chkAndDelete.CheckState = 1 Then
                sbxpc.SBXPCDLL.EmptySuperLogData(mMachineNumber)
            End If
        End If

        If vRet = True Then
            Cursor = System.Windows.Forms.Cursors.WaitCursor
            lblMessage.Text = "Getting ..."
            System.Windows.Forms.Application.DoEvents()
            With gridSLogData
                .Redraw = False
                i = 1
                Do
                    vRet = sbxpc.SBXPCDLL.GetSuperLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vGEnrollNumber, vGMachineNumber, vManipulation, vFingerNumber, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                    sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, sgEnrollNumber)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        .AddItem(CStr(1))
                    End If

                    .Row = i
                    .Col = 0
                    .Text = i
                    .Col = 1
                    .Text = CStr(vTMachineNumber)
                    .Col = 2
                    .Text = CStr(vSEnrollNumber)
                    .Col = 3
                    .Text = CStr(vSMachineNumber)
                    .Col = 4
                    .Text = sgEnrollNumber
                    .Col = 5
                    .Text = CStr(vGMachineNumber)
                    .Col = 6
                    Select Case vManipulation
                        Case 1
                        Case 2
                        Case 3
                            .Text = vManipulation & "--" & "Enroll User"
                        Case 4
                            .Text = vManipulation & "--" & "Enroll Manager"
                        Case 5
                            .Text = vManipulation & "--" & "Delete Fp Data"
                        Case 6
                            .Text = vManipulation & "--" & "Delete Password"
                        Case 7
                            .Text = vManipulation & "--" & "Delete All LogData"
                        Case 7
                            .Text = vManipulation & "--" & "Delete Card Data"
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
                    End Select
                    .Col = 7
                    If vFingerNumber < 10 Then
                        .Text = CStr(vFingerNumber)
                    ElseIf vFingerNumber = 10 Then
                        .Text = "Password"
                    ElseIf vFingerNumber = 14 Then
                        .Text = "Face"
                    Else
                        .Text = "Card"
                    End If
                    .Col = 8
                    .Text = CStr(vYear) & "/" & VB6.Format(vMonth, "0#") & "/" & VB6.Format(vDay, "0#") & " " & VB6.Format(vHour, "0#") & ":" & VB6.Format(vMinute, "0#")

                    LabelTotal.Text = "Total : " & i
                    System.Windows.Forms.Application.DoEvents()
                    i = i + 1
                Loop
                .Redraw = True
            End With
            lblMessage.Text = "ReadSuperLogData OK"
        End If

        Cursor = System.Windows.Forms.Cursors.Default
        sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub frmLog_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        mMachineNumber = frmMain.gMachineNumber
        chkReadMark.CheckState = System.Windows.Forms.CheckState.Checked
    End Sub

    Private Sub frmLog_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '	Me.Visible = False
        frmMain.Visible = True
        ClearPhoto()
    End Sub

    Private Sub cmdSetRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSetRange.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer
        Dim strXML As String

        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)

        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        strXML = MakeXMLCommandHeader("SetGLogSearchRange")
        sbxpc.SBXPCDLL.XML_AddBoolean(strXML, "UseSearchRange", chkUseSearchRange.Checked)

        If chkUseSearchRange.Checked Then
            sbxpc.SBXPCDLL.XML_AddLong(strXML, "StartYear", dtStart.Value.Year)
            sbxpc.SBXPCDLL.XML_AddLong(strXML, "StartMonth", dtStart.Value.Month)
            sbxpc.SBXPCDLL.XML_AddLong(strXML, "StartDate", dtStart.Value.Day)
            sbxpc.SBXPCDLL.XML_AddLong(strXML, "EndYear", dtEnd.Value.Year)
            sbxpc.SBXPCDLL.XML_AddLong(strXML, "EndMonth", dtEnd.Value.Month)
            sbxpc.SBXPCDLL.XML_AddLong(strXML, "EndDate", dtEnd.Value.Day)
        End If

        bRet = sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)

        If bRet Then
            lblMessage.Text = "SetGLogSearchRange OK"
        Else
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        bRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub ClearPhoto()
        On Error Resume Next
        picGLogPhoto.Image.Dispose()
        picGLogPhoto.Image = Nothing
        picGLogPhoto.ImageLocation = ""
    End Sub

    Private Sub gridSLogData_ClickEvent(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridSLogData.ClickEvent
        If Not chkShowGLogPhoto.Checked Then Exit Sub
        If Not gGlogSearched Then Exit Sub
        If prevGlogIndex = gridSLogData.Row Then Exit Sub
        prevGlogIndex = gridSLogData.Row
        ClearPhoto()
        If gridSLogData.get_TextMatrix(gridSLogData.Row, 1) = "No Photo" Then
            Exit Sub
        End If

        Dim bRet As Boolean
        Dim vErrorCode As Integer
        Dim strXML As String

        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)

        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        strXML = MakeXMLCommandHeader("GetGLogPhotoData")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "PhotoPos", Val(gridSLogData.get_TextMatrix(gridSLogData.Row, 1)))

        bRet = sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)

        If Not bRet Then
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
            GoTo _lExit
        End If

        lblMessage.Text = "GetGLogPhotoData OK"

        Dim photoData(gCompressPhotoSize - 1) As Byte
        Dim gh As GCHandle = GCHandle.Alloc(photoData, GCHandleType.Pinned)
        Dim AddrOfphotoData As IntPtr = gh.AddrOfPinnedObject()
        sbxpc.SBXPCDLL.XML_ParseBinaryLong(strXML, "PhotoData", AddrOfphotoData.ToInt32, gCompressPhotoSize)

        picGLogPhoto.Image = Image.FromStream(New MemoryStream(photoData))

_lExit:
        bRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, True)
    End Sub

    Private Function parseLogPos(ByVal ctrl As TextBox, ByRef pos As Integer) As Boolean
        pos = -1
        Try
            pos = Integer.Parse(ctrl.Text)
        Catch
            pos = -1
        End Try
        If (pos < 0) Then
            MessageBox.Show("Position must be equal to or greater than zero!", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ctrl.Focus()
            ctrl.SelectAll()
            parseLogPos = False
            Exit Function
        End If
        parseLogPos = True
    End Function
    Private Sub CmdGetSLogPosInfo_Click(sender As Object, e As EventArgs) Handles cmdGetSLogPosInfo.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetSLogPosInfo")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            Dim log_count As Integer = sbxpc.SBXPCDLL.XML_ParseInt(strXML, "LogCount")
            Dim start_pos As Integer = sbxpc.SBXPCDLL.XML_ParseInt(strXML, "StartPos")
            Dim max_count As Integer = sbxpc.SBXPCDLL.XML_ParseInt(strXML, "MaxCount")

            txtSLogStartPos.Text = start_pos.ToString()
            txtSLogEndPos.Text = log_count.ToString()
            lblMessage.Text = "LogCount=" & log_count.ToString() &
                    ", StartPos=" & start_pos.ToString() &
                    ", MaxCount=" & max_count.ToString() &
                    ", ValidPos: " & start_pos.ToString() & " ~ " & ((start_pos + log_count) Mod (max_count + 1)).ToString()
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdReadSLogWithPos_Click(sender As Object, e As EventArgs) Handles cmdReadSLogWithPos.Click
        gGlogSearched = False

        Dim start_pos, end_pos As Integer
        If ((Not parseLogPos(txtSLogStartPos, start_pos)) Or
            (Not parseLogPos(txtSLogEndPos, end_pos))) Then
            Exit Sub
        End If

        Dim vTMachineNumber As Integer
        Dim vSMachineNumber As Integer
        Dim vSEnrollNumber As Integer
        Dim vGEnrollNumber As Integer
        Dim vGMachineNumber As Integer
        Dim vManipulation As Integer
        Dim vFingerNumber As Integer
        Dim vYear As Integer
        Dim vMonth As Integer
        Dim vDay As Integer
        Dim vHour As Integer
        Dim vMinute As Integer
        Dim vSecond As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim i As Integer
        Dim n As Integer

        gridSLogData.Height = VB6.TwipsToPixelsY(4800)
        gridSLogData.Redraw = False
        gridSLogData.Clear()
        gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData1.Height = 0
        gridSLogData1.Redraw = False
        gridSLogData1.Clear()
        gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData2.Height = 0
        gridSLogData2.Redraw = False
        gridSLogData2.Clear()

        lblMessage.Text = "Waiting..."
        LabelTotal.Text = "Total : "
        Application.DoEvents()

        gridSLogData.Redraw = False
        gridSLogData.Clear()

        gstrLogItem = New Object() {"", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime"}
        With gridSLogData
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 8
                .Col = i
                .Text = gstrLogItem(i)
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 900)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColAlignment(6, 2)
            .set_ColWidth(7, 800)
            .Col = 8
            .Text = gstrLogItem(8)
            .set_ColWidth(8, 2000)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        vRet = sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 0)   ' 0 : false
        If (Not vRet) Then
            lblMessage.Text = gstrNoDevice
            Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If (Not sbxpc.SBXPCDLL.ReadSLogWithPos(frmMain.gMachineNumber, start_pos, end_pos)) Then
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
            sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 1)  ' 1 : true
            Exit Sub
        End If

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        lblMessage.Text = "Getting ..."
        Application.DoEvents()

        With gridSLogData
            .Redraw = False
            i = 1
            Do
                vRet = sbxpc.SBXPCDLL.GetSuperLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vGEnrollNumber, vGMachineNumber, vManipulation, vFingerNumber, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                If vRet = False Then Exit Do
                If vRet = True And i <> 1 Then
                    .AddItem(CStr(1))
                End If

                .Row = i
                .Col = 0
                .Text = i
                .Col = 1
                .Text = CStr(vTMachineNumber)
                .Col = 2
                .Text = CStr(vSEnrollNumber)
                .Col = 3
                .Text = CStr(vSMachineNumber)
                .Col = 4
                .Text = CStr(vGEnrollNumber)
                .Col = 5
                .Text = CStr(vGMachineNumber)
                .Col = 6
                Select Case vManipulation
                    Case 1
                    Case 2
                    Case 3
                        .Text = vManipulation & "--" & "Enroll User"
                    Case 4
                        .Text = vManipulation & "--" & "Enroll Manager"
                    Case 5
                        .Text = vManipulation & "--" & "Delete Fp Data"
                    Case 6
                        .Text = vManipulation & "--" & "Delete Password"
                    Case 7
                        .Text = vManipulation & "--" & "Delete All LogData"
                    Case 7
                        .Text = vManipulation & "--" & "Delete Card Data"
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
                End Select
                .Col = 7
                If vFingerNumber < 10 Then
                    .Text = CStr(vFingerNumber)
                ElseIf vFingerNumber = 10 Then
                    .Text = "Password"
                ElseIf vFingerNumber = 14 Then
                    .Text = "Face"
                Else
                    .Text = "Card"
                End If
                .Col = 8
                .Text = CStr(vYear) & "/" & VB6.Format(vMonth, "0#") & "/" & VB6.Format(vDay, "0#") & " " & VB6.Format(vHour, "0#") & ":" & VB6.Format(vMinute, "0#")

                LabelTotal.Text = "Total : " & i
                System.Windows.Forms.Application.DoEvents()
                i = i + 1
            Loop
            .Redraw = True
        End With
        lblMessage.Text = "ReadSLogWithPos OK"

        Cursor = System.Windows.Forms.Cursors.Default
        sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 1)  ' 1 : true
    End Sub

    Private Sub CmdDeleteSLogWithPos_Click(sender As Object, e As EventArgs) Handles cmdDeleteSLogWithPos.Click
        Dim endpos As Integer

        If (Not parseLogPos(txtSLogEndPos, endpos)) Then
            Exit Sub
        End If

        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "DeleteSLogWithPos")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "EndPos", endpos)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            lblMessage.Text = "Success"
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdGetGLogPosInfo_Click(sender As Object, e As EventArgs) Handles cmdGetGLogPosInfo.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetGLogPosInfo")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            Dim log_count As Integer = sbxpc.SBXPCDLL.XML_ParseInt(strXML, "LogCount")
            Dim start_pos As Integer = sbxpc.SBXPCDLL.XML_ParseInt(strXML, "StartPos")
            Dim max_count As Integer = sbxpc.SBXPCDLL.XML_ParseInt(strXML, "MaxCount")

            txtGLogStartPos.Text = start_pos.ToString()
            txtGLogEndPos.Text = log_count.ToString()
            lblMessage.Text = "LogCount=" & log_count.ToString() &
                    ", StartPos=" & start_pos.ToString() &
                    ", MaxCount=" & max_count.ToString() &
                    ", ValidPos: " & start_pos.ToString() & " ~ " & ((start_pos + log_count) Mod (max_count + 1)).ToString()
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdReadGLogWithPos_Click(sender As Object, e As EventArgs) Handles cmdReadGLogWithPos.Click
        gGlogSearched = True

        Dim start_pos, end_pos As Integer
        If ((Not parseLogPos(txtGLogStartPos, start_pos)) Or
            (Not parseLogPos(txtGLogEndPos, end_pos))) Then
            Exit Sub
        End If

        gGlogSearched = True

        Dim vTMachineNumber As Integer
        Dim vSMachineNumber As Integer
        Dim vSEnrollNumber As Integer
        Dim strEnrollNumber As String
        '		Dim vInOutMode As Integer
        Dim vVerifyMode As Integer
        Dim vYear As Integer
        Dim vMonth As Integer
        Dim vDay As Integer
        Dim vHour As Integer
        Dim vMinute As Integer
        Dim vSecond As Integer
        Dim vGlogExt As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim i As Integer
        Dim n As Integer
        Dim vMaxLogCnt As Integer

        vMaxLogCnt = gMaxLow

        lblMessage.Text = "Waiting..."
        LabelTotal.Text = "Total : "
        System.Windows.Forms.Application.DoEvents()

        gridSLogData.Height = VB6.TwipsToPixelsY(4800)
        gridSLogData.Redraw = False
        gridSLogData.Clear()
        gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData1.Height = 0
        gridSLogData1.Redraw = False
        gridSLogData1.Clear()
        gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
        gridSLogData2.Height = 0
        gridSLogData2.Redraw = False
        gridSLogData2.Clear()

        gstrLogItem = New Object() {"", "Photo No", "EnrollNo", "BodyTemp", "EMachineNo", "VeriMode", "DateTime"}
        With gridSLogData
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .Text = gstrLogItem(i)
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With
        With gridSLogData1
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With
        With gridSLogData2
            .Row = 0
            .set_ColWidth(0, 600)
            For i = 1 To 6
                .Col = i
                .set_ColAlignment(i, 3)
                .set_ColWidth(i, 1200)
            Next i
            .Col = 6
            .set_ColWidth(6, 2000)
            .set_ColWidth(7, 700)
            .set_ColWidth(8, 700)
            n = .Rows
            If n > 2 Then
                Do
                    If n = 2 Then Exit Do
                    .RemoveItem((n))
                    n = n - 1
                Loop
            End If
            .Redraw = True
        End With

        If Not (chkReadGlogExt.Checked) Then
            gridSLogData.set_ColWidth(3, 0)     ' col 3 : visible = false
        End If

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        vRet = sbxpc.SBXPCDLL.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If


        Dim ret As Boolean
        If (chkReadGlogExt.Checked) Then
            ret = sbxpc.SBXPCDLL.ReadGLogWithPos_Ext(frmMain.gMachineNumber, start_pos, end_pos)
        Else
            ret = sbxpc.SBXPCDLL.ReadGLogWithPos(frmMain.gMachineNumber, start_pos, end_pos)
        End If

        If (Not ret) Then
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
            sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 1)  ' 1 : true
            Exit Sub
        End If

        Cursor = System.Windows.Forms.Cursors.WaitCursor
        lblMessage.Text = "Getting ..."
        Application.DoEvents()

        gridSLogData.Redraw = False
        gridSLogData1.Redraw = False
        gridSLogData2.Redraw = False
        With gridSLogData
            i = 1

            Do
                If (chkReadGlogExt.Checked) Then
                    vRet = sbxpc.SBXPCDLL.GetGeneralLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                Else
                    vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                End If
                sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                If vRet = False Then Exit Do
                If vRet = True And i <> 1 Then
                    .AddItem(CStr(1))
                End If

                ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, 0, gridSLogData)

                LabelTotal.Text = "Total : " & i
                System.Windows.Forms.Application.DoEvents()
                i = i + 1
                If i > vMaxLogCnt Then Exit Do
            Loop
        End With

        If i > vMaxLogCnt Then
            gridSLogData.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Height) / 2)
            gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
            gridSLogData1.Height = gridSLogData.Height
            With gridSLogData1
                Do
                    If (chkReadGlogExt.Checked) Then
                        vRet = sbxpc.SBXPCDLL.GetAllGLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                    Else
                        vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                    End If
                    sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        If i - vMaxLogCnt > 1 Then .AddItem(CStr(1))
                    End If

                    ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, 0, gridSLogData)

                    LabelTotal.Text = "Total : " & i
                    System.Windows.Forms.Application.DoEvents()
                    i = i + 1
                    If i > vMaxLogCnt * 2 Then Exit Do
                Loop
            End With
        End If
        vMaxLogCnt = vMaxLogCnt * 2
        If i > vMaxLogCnt Then
            gridSLogData.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Height) * 2 / 3)
            gridSLogData1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height))
            gridSLogData1.Height = gridSLogData.Height
            gridSLogData2.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(gridSLogData.Top) + VB6.PixelsToTwipsY(gridSLogData.Height) * 2)
            gridSLogData2.Height = gridSLogData.Height
            With gridSLogData2
                Do
                    If (chkReadGlogExt.Checked) Then
                        vRet = sbxpc.SBXPCDLL.GetAllGLogData_Ext(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt)
                    Else
                        vRet = sbxpc.SBXPCDLL.GetGeneralLogData(mMachineNumber, vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond)
                    End If
                    sbxpc.SBXPCDLL.GetLastBigUserId_AsString1(mMachineNumber, strEnrollNumber)
                    If vRet = False Then Exit Do
                    If vRet = True And i <> 1 Then
                        If i - vMaxLogCnt > 1 Then .AddItem(CStr(1))
                    End If

                    ShowGlogItem(vTMachineNumber, strEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond, vGlogExt, i, 0, gridSLogData)

                    LabelTotal.Text = "Total : " & i
                    System.Windows.Forms.Application.DoEvents()
                    i = i + 1
                Loop
            End With
        End If
        gridSLogData.Redraw = True
        gridSLogData1.Redraw = True
        gridSLogData2.Redraw = True

        lblMessage.Text = "ReadGLogWithPos OK"

        Cursor = System.Windows.Forms.Cursors.Default
        sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 1)      ' 1 : true
    End Sub

    Private Sub CmdDeleteGLogWithPos_Click(sender As Object, e As EventArgs) Handles cmdDeleteGLogWithPos.Click
        Dim end_pos As Integer = 0
        If (Not parseLogPos(txtGLogEndPos, end_pos)) Then
            Exit Sub
        End If

        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "DeleteGLogWithPos")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "EndPos", end_pos)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            lblMessage.Text = "Success"
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdDeleteGLogPhoto_Click(sender As Object, e As EventArgs) Handles cmdDeleteGLogPhoto.Click
        If Not gGlogSearched Then
            lblMessage.Text = "Please select log to delete photo."
            Exit Sub
        End If
        If gridSLogData.get_TextMatrix(gridSLogData.Row, 1) = "No Photo" Then
            lblMessage.Text = "No Photo Id"
            Exit Sub
        End If

        Dim photoPos As Integer = Val(gridSLogData.get_TextMatrix(gridSLogData.Row, 1))

        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "DeleteGLogPhotoData")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "PhotoPos", photoPos)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            Dim strResultCode As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "ResultCode", strResultCode)

            If (strResultCode = "Success") Then
                lblMessage.Text = "Delete GLog Photo Success. (photoPos=" + photoPos.ToString() + ")"
            ElseIf (strResultCode = "No Photo") Then
                lblMessage.Text = "No Photo."
            ElseIf (strResultCode = "InvalidParam") Then
                lblMessage.Text = "Invalid Parameter."
            Else
                lblMessage.Text = "Unknown Error."
            End If
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdClearPhoto_Glog_Click(sender As Object, e As EventArgs) Handles cmdClearPhoto_Glog.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "ClearPhoto")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "PhotoType", Convert.ToInt32(PhotoTag_GlogPhotoTag))

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            Dim strResultCode As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "ResultCode", strResultCode)

            If (strResultCode = "Success") Then
                lblMessage.Text = "Clear All GLog Photo Success."
            Else
                lblMessage.Text = "Unknown Error."
            End If
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdClearPhoto_User_Click(sender As Object, e As EventArgs) Handles cmdClearPhoto_User.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "ClearPhoto")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "PhotoType", Convert.ToInt32(PhotoTag_UserPhotoTag))

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            Dim strResultCode As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "ResultCode", strResultCode)

            If (strResultCode = "Success") Then
                lblMessage.Text = "Clear All Enroll Photo Success."
            Else
                lblMessage.Text = "Unknown Error."
            End If
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdClearPhoto_All_Click(sender As Object, e As EventArgs) Handles cmdClearPhoto_All.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "ClearPhoto")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        ' sbxpc.SBXPCDLL.XML_AddInt(strXML, "PhotoType", -1)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            Dim strResultCode As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "ResultCode", strResultCode)

            If (strResultCode = "Success") Then
                lblMessage.Text = "Clear All Photo Success."
            Else
                lblMessage.Text = "Unknown Error."
            End If
        Else
            Dim vErrorCode As Integer = 0
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub
End Class