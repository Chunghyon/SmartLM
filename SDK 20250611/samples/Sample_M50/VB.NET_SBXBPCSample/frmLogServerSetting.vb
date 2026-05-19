Public Class frmLogServerSetting
    Private Sub BtnGetDnsSettings_Click(sender As Object, e As EventArgs) Handles btnGetDnsSettings.Click
        Dim strXML As String = ""
        Dim strValue As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetLogServerSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "ManagerPCDomainName", strValue)
            txtServerDomainName.Text = strValue

            textBgServerPort.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "ManagerPCPort"))

            Dim nMode As Integer = sbxpc.SBXPCDLL.XML_ParseLong(strXML, "EventSendMode")
            cmbLogServerMode.SelectedIndex = nMode

            MessageBox.Show("Get Log Server Settings OK!")
        Else
            MessageBox.Show("Get Log Server Settings Failed.")
        End If
    End Sub

    Private Sub BtnSetDnsSettings_Click(sender As Object, e As EventArgs) Handles btnSetDnsSettings.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetLogServerSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        sbxpc.SBXPCDLL.XML_AddString(strXML, "ManagerPCDomainName", txtServerDomainName.Text)
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "ManagerPCPort", Convert.ToInt32(textBgServerPort.Text))

        Dim nMode As Integer = cmbLogServerMode.SelectedIndex
        If (nMode < 0) Then
            nMode = 0
        End If

        sbxpc.SBXPCDLL.XML_AddLong(strXML, "EventSendMode", nMode)

        sbxpc.SBXPCDLL.XML_AddString(strXML, "ManagerPCDomainName", txtServerDomainName.Text)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            MessageBox.Show("Set Log Server Settings OK!")
        Else
            Dim str As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Result", str)

            MessageBox.Show("Set Log Server Settings Failed.\r\nResult:" + str)
        End If
    End Sub

    Private Sub FrmLogServerSetting_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        frmMain.Visible = True
    End Sub

    Private Sub FrmLogServerSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbLogServerMode.SelectedIndex = 1
    End Sub
End Class