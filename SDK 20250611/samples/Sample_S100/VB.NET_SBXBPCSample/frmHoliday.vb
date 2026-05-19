Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices

Public Class frmHoliday

    Const DBHOLIDAYS_MAX As Integer = 256
    Const HOLIDAY_BYTES As Integer = 3
    Const ALLBYTE_COUNT As Integer = DBHOLIDAYS_MAX * HOLIDAY_BYTES
    Dim holidayInfo(ALLBYTE_COUNT - 1) As UInt32


    Private Sub frmHoliday_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        HolidayInit()
        DrawHolidays()
    End Sub

    Private Sub HolidayInit()
        Dim i As Integer
        For i = 0 To DBHOLIDAYS_MAX - 1
            holidayInfo(i * HOLIDAY_BYTES) = 1
            holidayInfo(i * HOLIDAY_BYTES + 1) = 1
            holidayInfo(i * HOLIDAY_BYTES + 2) = 0
        Next
    End Sub

    Private Sub DrawHolidays()
        Dim i As Integer
        Dim itemString As String
        lstHoliday.Items.Clear()
        For i = 0 To DBHOLIDAYS_MAX - 1
            itemString = "[No.] " & String.Format("{0:D2}", i) & " "
            itemString &= "[Day/Month] "
            itemString &= String.Format("{0:D2}", holidayInfo(i * HOLIDAY_BYTES + 1)) & "/"
            itemString &= String.Format("{0:D2}", holidayInfo(i * HOLIDAY_BYTES)) & " "
            itemString &= "[Period] "
            itemString &= String.Format("{0:D2}", holidayInfo(i * HOLIDAY_BYTES + 2))

            lstHoliday.Items.Add(itemString)
        Next
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        If lstHoliday.SelectedIndex = -1 Then Exit Sub

        Dim index As Integer
        index = lstHoliday.SelectedIndex * HOLIDAY_BYTES

        holidayInfo(index) = dtHoliday.Value.Month
        holidayInfo(index + 1) = dtHoliday.Value.Day
        holidayInfo(index + 2) = Convert.ToInt32(txtDays.Text)

        DrawHolidays()
    End Sub

    Private Sub lstHoliday_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstHoliday.SelectedIndexChanged
        If lstHoliday.SelectedIndex = -1 Then Exit Sub

        Dim index As Integer
        index = lstHoliday.SelectedIndex * HOLIDAY_BYTES

        If (holidayInfo(index) > 0 And holidayInfo(index) <= 12 And _
                holidayInfo(index + 1) > 0 And holidayInfo(index + 1) <= 31) Then
            dtHoliday.Value = New DateTime(2000, holidayInfo(index), holidayInfo(index + 1))
        End If

        txtDays.Text = Convert.ToString(holidayInfo(index + 2))
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Close()
    End Sub

    Private Sub frmHoliday_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        frmMain.Visible = True
    End Sub

    Private Sub cmdRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRead.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer
        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 0) ' 0 : disable

        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        Dim gh As GCHandle = GCHandle.Alloc(holidayInfo, GCHandleType.Pinned)
        Dim AddrOfholidayInfo As IntPtr = gh.AddrOfPinnedObject()
        bRet = frmMain.SBXPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 6, AddrOfholidayInfo.ToInt32())

        If bRet Then
            lblMessage.Text = "Success!"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 1) ' 1 : enable

        DrawHolidays()
    End Sub

    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer
        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 0)

        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        Dim gh As GCHandle = GCHandle.Alloc(holidayInfo, GCHandleType.Pinned)
        Dim AddrOfholidayInfo As IntPtr = gh.AddrOfPinnedObject()
        bRet = frmMain.SBXPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 6, AddrOfholidayInfo.ToInt32())

        If bRet Then
            lblMessage.Text = "Success!"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 1) ' 1 : enable
    End Sub
End Class