Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices

Public Class frmAccessTz
    Public Const DB_ACCESS_TIMEZONE_COUNT As Integer = 50
    Public Const DB_TIMESECTION_COUNT As Integer = 8
    Private Const DB_ALLCOUNT As Integer = DB_ACCESS_TIMEZONE_COUNT * DB_TIMESECTION_COUNT
    Dim timeZoneInfo(DB_ALLCOUNT * 4 - 1) As UInt32

    Private Sub frmAccessTz_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        timeZoneInit()
        DrawTimezoneInfo()
        dtStart.ShowUpDown = True
        dtEnd.ShowUpDown = True
    End Sub

    Private Sub timeZoneInit()
        Dim i As Integer, j As Integer, index As Integer
        For i = 0 To DB_ACCESS_TIMEZONE_COUNT - 1
            For j = 0 To DB_TIMESECTION_COUNT - 1
                index = i * DB_TIMESECTION_COUNT + j
                timeZoneInfo(index * 4 + 0) = 0
                timeZoneInfo(index * 4 + 1) = 0
                timeZoneInfo(index * 4 + 2) = 23
                timeZoneInfo(index * 4 + 3) = 59
            Next j
        Next i
    End Sub

    Private Sub DrawTimezoneInfo()
        Dim startHour As Integer, startMinute As Integer
        Dim endHour As Integer, endMinute As Integer
        Dim index As Integer
        Dim i As Integer, j As Integer
        Dim itemString As String

        lstTimeZone.Items.Clear()

        For i = 0 To DB_ACCESS_TIMEZONE_COUNT - 1
            For j = 0 To DB_TIMESECTION_COUNT - 1
                index = (i * DB_TIMESECTION_COUNT + j) * 4
                startHour = timeZoneInfo(index)
                startMinute = timeZoneInfo(index + 1)
                endHour = timeZoneInfo(index + 2)
                endMinute = timeZoneInfo(index + 3)
                itemString = "[Tz.]" & String.Format("{0:D2}-{1:D1} ", i, j)
                itemString += "[S]" & String.Format("{0:D2}:{1:D2} ", startHour, startMinute)
                itemString += "[E]" & String.Format("{0:D2}:{1:D2}", endHour, endMinute) + " "

                lstTimeZone.Items.Add(itemString)
            Next j
        Next i
    End Sub

    Private Sub lstTimeZone_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstTimeZone.SelectedIndexChanged
        If lstTimeZone.SelectedIndex = -1 Then
            Return
        End If

        Dim index As Integer = lstTimeZone.SelectedIndex
        dtStart.Value = New DateTime(2000, 1, 1, timeZoneInfo(index * 4), timeZoneInfo(index * 4 + 1), 0)
        dtEnd.Value = New DateTime(2000, 1, 1, timeZoneInfo(index * 4 + 2), timeZoneInfo(index * 4 + 3), 0)
    End Sub


    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        If lstTimeZone.SelectedIndex = -1 Then
            Return
        End If

        Dim index As Integer = lstTimeZone.SelectedIndex
        timeZoneInfo(index * 4 + 0) = dtStart.Value.Hour
        timeZoneInfo(index * 4 + 1) = dtStart.Value.Minute
        timeZoneInfo(index * 4 + 2) = dtEnd.Value.Hour
        timeZoneInfo(index * 4 + 3) = dtEnd.Value.Minute
        DrawTimezoneInfo()
    End Sub

    Private Sub frmAccessTz_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        frmMain.Visible = True
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Close()
    End Sub

    Private Sub cmdRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRead.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, False)
        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        Dim gh As GCHandle = GCHandle.Alloc(timeZoneInfo, GCHandleType.Pinned)
        Dim AddrOftimZoneInfo As IntPtr = gh.AddrOfPinnedObject()
        bRet = frmMain.SBXPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 3, AddrOftimZoneInfo.ToInt32())

        If bRet Then
            lblMessage.Text = "Success"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, True)
        DrawTimezoneInfo()
    End Sub

    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, False)
        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        Dim gh As GCHandle = GCHandle.Alloc(timeZoneInfo, GCHandleType.Pinned)
        Dim AddrOftimZoneInfo As IntPtr = gh.AddrOfPinnedObject()
        bRet = frmMain.SBXPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 3, AddrOftimZoneInfo.ToInt32())

        If bRet Then
            lblMessage.Text = "Success"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, True)
    End Sub
End Class