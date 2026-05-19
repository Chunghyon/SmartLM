Option Strict Off
Option Explicit On
Friend Class frmMain
	Inherits System.Windows.Forms.Form
	Public gMachineNumber As Integer
	Dim mOpenFlag As Boolean
	
	'UPGRADE_WARNING: Event cmbMachineNumber.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub cmbMachineNumber_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbMachineNumber.SelectedIndexChanged
		gMachineNumber = cmbMachineNumber.SelectedIndex + 1
	End Sub
	
	Private Sub cmdBellInfo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBellInfo.Click
		Me.Visible = False
		frmBellInfo.Visible = True
	End Sub
	
	Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click
		If mOpenFlag = True Then
            SBXPC1.Disconnect()
            mOpenFlag = False
            cmdOpen.Enabled = True
            cmdClose.Enabled = False
            EnableManagementGroup(False)
        End If
    End Sub

    Private Sub cmdEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEnrollData.Click
        Me.Visible = False
        frmEnroll.Visible = True
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdLockCtl_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdLockCtl.Click
        Me.Visible = False
        frmLockCtl.Visible = True
    End Sub

    Private Sub cmdLogData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdLogData.Click
        Me.Visible = False
        frmLog.Visible = True
    End Sub

    Private Sub cmdOpen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOpen.Click
        Dim lpszIPAddress As String

        If optNetworkDevice.Checked = True Then
            lpszIPAddress = txtIPAddress.Text
            If SBXPC1.ConnectTcpip(gMachineNumber, lpszIPAddress, CInt(txtPortNo.Text), CInt(txtPassword.Text)) = True Then
                mOpenFlag = True
                cmdOpen.Enabled = False
                cmdClose.Enabled = True
                EnableManagementGroup(True)
            End If
        End If
        If optSerialDevice.Checked = True Then
            If SBXPC1.ConnectSerial(gMachineNumber, cmbComPort.SelectedIndex + 1, CInt(cmbBaudrate.Text)) = True Then
                mOpenFlag = True
                cmdOpen.Enabled = False
                cmdClose.Enabled = True
                EnableManagementGroup(True)
            End If
        End If
        If optUSBDevice.Checked = True Then
            If SBXPC1.ConnectSerial(gMachineNumber, 0, 0) = True Then
                mOpenFlag = True
                cmdOpen.Enabled = False
                cmdClose.Enabled = True
                EnableManagementGroup(True)
            End If
        End If
    End Sub

    Private Sub cmdProductCode_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdProductCode.Click
        Me.Visible = False
        frmSerialNo.Visible = True
    End Sub

    Private Sub cmdSystemInfo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSystemInfo.Click
        Me.Visible = False
        frmSystemInfo.Visible = True
    End Sub

    Private Sub frmMain_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        lblComPort.Enabled = False
        cmbComPort.Enabled = False
        lblBaudrate.Enabled = False
        cmbBaudrate.Enabled = False

        optNetworkDevice.Checked = True
        lblIPAddress.Enabled = True
        txtIPAddress.Enabled = True
        lblPortNo.Enabled = True
        txtPortNo.Enabled = True
        lblPassword.Enabled = True
        txtPassword.Enabled = True

        cmdOpen.Enabled = True
        cmdClose.Enabled = False
        EnableManagementGroup(False)
        mOpenFlag = False
        cmbMachineNumber.Text = CStr(1)
        cmbComPort.Text = CStr(1)
        cmbBaudrate.Text = "115200"

        SBXPC1.DotNET()
    End Sub

    Private Sub EnableManagementGroup(ByVal bEnable As Boolean)
        cmdEnrollData.Enabled = bEnable
        cmdLogData.Enabled = bEnable
        cmdSystemInfo.Enabled = bEnable
        cmdProductCode.Enabled = bEnable
        cmdBellInfo.Enabled = bEnable
        cmdLockCtl.Enabled = bEnable
        cmdTrMode.Enabled = bEnable
        cmdAccessTz.Enabled = bEnable
        cmdModeTZone.Enabled = bEnable
        cmdHoliday.Enabled = bEnable
    End Sub

    Private Sub frmMain_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If mOpenFlag = True Then
            SBXPC1.Disconnect()
            mOpenFlag = False
        End If
    End Sub

    'UPGRADE_WARNING: Event optNetworkDevice.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optNetworkDevice_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optNetworkDevice.CheckedChanged
        If eventSender.Checked Then
            Dim lpszIPAddress As String
            optSerialDevice.Checked = False
            optUSBDevice.Checked = False

            If optNetworkDevice.Checked = True Then
                lblComPort.Enabled = False
                cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
                lblIPAddress.Enabled = True
                txtIPAddress.Enabled = True
                lblPortNo.Enabled = True
                txtPortNo.Enabled = True
                lblPassword.Enabled = True
                txtPassword.Enabled = True
                lpszIPAddress = txtIPAddress.Text
            ElseIf optSerialDevice.Checked = True Then
                lblComPort.Enabled = True
                cmbComPort.Enabled = True
                lblBaudrate.Enabled = True
                cmbBaudrate.Enabled = True
                lblIPAddress.Enabled = False
                txtIPAddress.Enabled = False
                lblPortNo.Enabled = False
                txtPortNo.Enabled = False
                lblPassword.Enabled = False
                txtPassword.Enabled = False
            Else
                lblComPort.Enabled = False
                cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
                lblIPAddress.Enabled = False
                txtIPAddress.Enabled = False
                lblPortNo.Enabled = False
                txtPortNo.Enabled = False
                lblPassword.Enabled = False
                txtPassword.Enabled = False
            End If
        End If
    End Sub

    'UPGRADE_WARNING: Event optSerialDevice.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optSerialDevice_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSerialDevice.CheckedChanged
        If eventSender.Checked Then
            Dim lpszIPAddress As String
            optNetworkDevice.Checked = False
            optUSBDevice.Checked = False

            If optSerialDevice.Checked = True Then
                lblComPort.Enabled = True
                cmbComPort.Enabled = True
                lblBaudrate.Enabled = True
                cmbBaudrate.Enabled = True
                lblIPAddress.Enabled = False
                txtIPAddress.Enabled = False
                lblPortNo.Enabled = False
                txtPortNo.Enabled = False
                lblPassword.Enabled = False
                txtPassword.Enabled = False
            ElseIf optNetworkDevice.Checked = True Then
                lblComPort.Enabled = False
                cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
                lblIPAddress.Enabled = True
                txtIPAddress.Enabled = True
                lblPortNo.Enabled = True
                txtPortNo.Enabled = True
                lblPassword.Enabled = True
                txtPassword.Enabled = True
                lpszIPAddress = txtIPAddress.Text
            Else
                lblComPort.Enabled = False
                cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
                lblIPAddress.Enabled = False
                txtIPAddress.Enabled = False
                lblPortNo.Enabled = False
                txtPortNo.Enabled = False
                lblPassword.Enabled = False
                txtPassword.Enabled = False
            End If
        End If
    End Sub

    Private Sub optUSBDevice_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optUSBDevice.CheckedChanged
        If eventSender.Checked Then
            Dim lpszIPAddress As String
            optSerialDevice.Checked = False
            optNetworkDevice.Checked = False

            If optSerialDevice.Checked = True Then
                lblComPort.Enabled = True
                cmbComPort.Enabled = True
                lblBaudrate.Enabled = True
                cmbBaudrate.Enabled = True
                lblIPAddress.Enabled = False
                txtIPAddress.Enabled = False
                lblPortNo.Enabled = False
                txtPortNo.Enabled = False
                lblPassword.Enabled = False
                txtPassword.Enabled = False
            ElseIf optNetworkDevice.Checked = True Then
                lblComPort.Enabled = False
                cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
                lblIPAddress.Enabled = True
                txtIPAddress.Enabled = True
                lblPortNo.Enabled = True
                txtPortNo.Enabled = True
                lblPassword.Enabled = True
                txtPassword.Enabled = True
                lpszIPAddress = txtIPAddress.Text
            Else
                lblComPort.Enabled = False
                cmbComPort.Enabled = False
                lblBaudrate.Enabled = False
                cmbBaudrate.Enabled = False
                lblIPAddress.Enabled = False
                txtIPAddress.Enabled = False
                lblPortNo.Enabled = False
                txtPortNo.Enabled = False
                lblPassword.Enabled = False
                txtPassword.Enabled = False
            End If
        End If
    End Sub

    'UPGRADE_WARNING: Event txtIPAddress.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtIPAddress_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtIPAddress.TextChanged
        Dim lpszIPAddress As String

        If txtIPAddress.Text = "" Then Exit Sub
        If txtPortNo.Text = "" Then Exit Sub
        lpszIPAddress = txtIPAddress.Text
    End Sub

    'UPGRADE_WARNING: Event txtPortNo.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtPortNo_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtPortNo.TextChanged
        Dim lpszIPAddress As String

        If txtIPAddress.Text = "" Then Exit Sub
        If txtPortNo.Text = "" Then Exit Sub
        lpszIPAddress = txtIPAddress.Text
    End Sub

    Private Sub cmdTrMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTrMode.Click
        frmTrMode.Visible = True
        Me.Visible = False
    End Sub

    Private Sub cmdAccessTz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAccessTz.Click
        frmAccessTz.Visible = True
        Me.Visible = False
    End Sub

    Private Sub cmdModeTZone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModeTZone.Click
        frmTMode.Visible = True
        Me.Visible = False
    End Sub

    Private Sub cmdHoliday_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHoliday.Click
        frmHoliday.Visible = True
        Me.Visible = False
    End Sub
End Class