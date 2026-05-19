Public Class frmNtpServerSetting
    Private Sub BtnGetNtpServerSettings_Click(sender As Object, e As EventArgs) Handles btnGetNtpServerSettings.Click

        Dim strXML As String = ""
        Dim strValue As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetDeviceInfoExt")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddString(strXML, "ParamName", "NTPServer")

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Value1", strValue)
            txtServerAddress.Text = strValue

            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Value2", strValue)
            txtTimezone.Text = strValue

            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Value3", strValue)
            txtInterval.Text = strValue

            MessageBox.Show("Get NTP Server Settings OK!")
        Else
            MessageBox.Show("Get NTP Server Settings Failed.")
        End If

    End Sub

    Private Sub BtnSetNtpServerSettings_Click(sender As Object, e As EventArgs) Handles btnSetNtpServerSettings.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetDeviceInfoExt")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddString(strXML, "ParamName", "NTPServer")

        sbxpc.SBXPCDLL.XML_AddString(strXML, "Value1", txtServerAddress.Text)
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "Value2", Convert.ToInt32(txtTimezone.Text))
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "Value3", Convert.ToInt32(txtInterval.Text))

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            MessageBox.Show("Set NTP Server Settings OK!")
        Else
            Dim str As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Result", str)

            MessageBox.Show("Set NTP Server Settings Failed.\r\nResult:" & str)
        End If
    End Sub

    Private Sub FrmNtpServerSetting_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        frmMain.Visible = True
    End Sub
End Class