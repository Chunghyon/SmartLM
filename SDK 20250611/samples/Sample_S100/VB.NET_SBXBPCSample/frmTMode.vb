Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices

Public Class frmTMode
    Const DB_TMODE_COUNT As Integer = 10
    Const DB_TMODE_BYTE_COUNT As Integer = 5
    Dim tModeInfo(DB_TMODE_COUNT * DB_TMODE_BYTE_COUNT - 1) As UInt32

    Private Sub TModeInfoInit()
        Dim i As Integer
        For i = 0 To DB_TMODE_COUNT - 1
            tModeInfo(i * DB_TMODE_BYTE_COUNT + 0) = 0
            tModeInfo(i * DB_TMODE_BYTE_COUNT + 1) = 0
            tModeInfo(i * DB_TMODE_BYTE_COUNT + 2) = 0
            tModeInfo(i * DB_TMODE_BYTE_COUNT + 3) = 0
            tModeInfo(i * DB_TMODE_BYTE_COUNT + 4) = 0
        Next
    End Sub

    Private Sub DrawTModeInfo()
        Dim itemString As String
        Dim i As Integer
        lstTMode.Items.Clear()
        For i = 0 To DB_TMODE_COUNT - 1
            itemString = "[No.]" & String.Format("{0:D2}", i) & " "
            itemString &= "[S]"
            itemString &= String.Format("{0:D2}", tModeInfo(i * DB_TMODE_BYTE_COUNT + 1)) & ":"
            itemString &= String.Format("{0:D2}", tModeInfo(i * DB_TMODE_BYTE_COUNT + 2)) & " "
            itemString &= "[E]"
            itemString &= String.Format("{0:D2}", tModeInfo(i * DB_TMODE_BYTE_COUNT + 3)) & ":"
            itemString &= String.Format("{0:D2}", tModeInfo(i * DB_TMODE_BYTE_COUNT + 4)) & " "
            itemString &= cmbDoorMode.Items(tModeInfo(i * DB_TMODE_BYTE_COUNT))
            lstTMode.Items.Add(itemString)
        Next
    End Sub

    Private Sub frmTMode_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TModeInfoInit()
        DrawTModeInfo()
        dtStart.ShowUpDown = True
        dtEnd.ShowUpDown = True
    End Sub

    Private Sub lstTMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstTMode.SelectedIndexChanged
        Dim index As Integer, startHour As Integer, startMinute As Integer
        Dim endHour As Integer, endMinute As Integer
        Dim tMode As Integer
        If lstTMode.SelectedIndex = -1 Then Exit Sub
        index = lstTMode.SelectedIndex * DB_TMODE_BYTE_COUNT
        startHour = tModeInfo(index + 1)
        startMinute = tModeInfo(index + 2)
        endHour = tModeInfo(index + 3)
        endMinute = tModeInfo(index + 4)
        tMode = tModeInfo(index)

        dtStart.Value = New DateTime(2000, 1, 1, startHour, startMinute, 0)
        dtEnd.Value = New DateTime(2000, 1, 1, endHour, endMinute, 0)
        cmbDoorMode.SelectedIndex = tMode
    End Sub

    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 0) ' 0 : disable

        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        Dim gh As GCHandle = GCHandle.Alloc(tModeInfo, GCHandleType.Pinned)
        Dim AddrOftModeInfo As IntPtr = gh.AddrOfPinnedObject()

        bRet = frmMain.SBXPC1.SetDeviceLongInfo(frmMain.gMachineNumber, 4, AddrOftModeInfo.ToInt32())

        If bRet Then
            lblMessage.Text = "Success!"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 1) ' 1 : enable

        DrawTModeInfo()
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Dim index As Integer
        If lstTMode.SelectedIndex = -1 Then Exit Sub
        index = lstTMode.SelectedIndex * DB_TMODE_BYTE_COUNT
        tModeInfo(index + 1) = dtStart.Value.Hour
        tModeInfo(index + 2) = dtStart.Value.Minute
        tModeInfo(index + 3) = dtEnd.Value.Hour
        tModeInfo(index + 4) = dtEnd.Value.Minute
        tModeInfo(index) = cmbDoorMode.SelectedIndex

        DrawTModeInfo()
    End Sub

    Private Sub frmTMode_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        frmMain.Visible = True
    End Sub

    Private Sub cmdRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRead.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer
        vErrorCode = 0

        lblMessage.Text = "Working..."
        Application.DoEvents()

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 0) ' 0 : disable

        If Not bRet Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        Dim gh As GCHandle = GCHandle.Alloc(tModeInfo, GCHandleType.Pinned)
        Dim AddrOftModeInfo As IntPtr = gh.AddrOfPinnedObject()

        bRet = frmMain.SBXPC1.GetDeviceLongInfo(frmMain.gMachineNumber, 4, AddrOftModeInfo.ToInt32())

        If bRet Then
            lblMessage.Text = "Success!"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        bRet = frmMain.SBXPC1.EnableDevice(frmMain.gMachineNumber, 1) ' 1 : enable
        DrawTModeInfo()
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Close()
    End Sub
End Class