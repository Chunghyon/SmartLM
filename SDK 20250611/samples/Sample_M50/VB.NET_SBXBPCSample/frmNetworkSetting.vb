Public Class frmNetworkSetting
    Private Sub BtnGetEthernetSetting_Click(sender As Object, e As EventArgs) Handles btnGetEthernetSetting.Click
        Dim strXML As String = ""
        Dim strValue As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetEthernetSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            chkEther_DHCP.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(strXML, "DHCP") <> 0)
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "IP", strValue)
            txtEther_IP.Text = strValue
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Subnet", strValue)
            txtEther_Subnet.Text = strValue
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "DefaultGateway", strValue)
            txtEther_DefaultGateway.Text = strValue
            chkEther_ManualDNS.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(strXML, "ManualDNS") <> 0)
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "PrimaryDNSServer", strValue)
            txtEther_PrimaryDNSServer.Text = strValue
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "SecondaryDNSServer", strValue)
            txtEther_SecondaryDNSServer.Text = strValue

            ChkEther_DHCP_CheckedChanged(sender, e)
            ChkEther_ManualDNS_CheckedChanged(sender, e)

            MessageBox.Show("Get Ethernet Setting OK!")
        Else
            MessageBox.Show("Get Ethernet Setting Failed.")
        End If
    End Sub

    Private Sub BtnSetEthernetSetting_Click(sender As Object, e As EventArgs) Handles btnSetEthernetSetting.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetEthernetSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        sbxpc.SBXPCDLL.XML_AddInt(strXML, "DHCP", Convert.ToInt32(chkEther_DHCP.Checked))
        If (Not chkEther_DHCP.Checked) Then
            sbxpc.SBXPCDLL.XML_AddString(strXML, "IP", txtEther_IP.Text)
            sbxpc.SBXPCDLL.XML_AddString(strXML, "Subnet", txtEther_Subnet.Text)
            sbxpc.SBXPCDLL.XML_AddString(strXML, "DefaultGateway", txtEther_DefaultGateway.Text)
        End If
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "ManualDNS", Convert.ToInt32(chkEther_ManualDNS.Checked))
        If (chkEther_ManualDNS.Checked) Then
            sbxpc.SBXPCDLL.XML_AddString(strXML, "PrimaryDNSServer", txtEther_PrimaryDNSServer.Text)
            sbxpc.SBXPCDLL.XML_AddString(strXML, "SecondaryDNSServer", txtEther_SecondaryDNSServer.Text)
        End If

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            MessageBox.Show("Set Ethernet Setting OK!")
        Else
            Dim str As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Result", str)

            MessageBox.Show("Set Ethernet Setting Failed.\r\nResult:" & str)
        End If
    End Sub

    Private Sub BtnGetCommSetting_Click(sender As Object, e As EventArgs) Handles btnGetCommSetting.Click
        Dim strXML As String = ""
        Dim strValue As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetCommSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            txtDeviceID.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "DeviceID"))
            txtCommPwd.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "CommPwd"))
            txtTcpPort.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "TcpPort"))
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "P2PSvr", strValue)
            txtP2P_Server.Text = strValue
            txtP2P_Port.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "P2PPort"))

            MessageBox.Show("Get Communication Setting OK!")
        Else
            MessageBox.Show("Get Communication Setting Failed.")
        End If

    End Sub

    Private Sub BtnSetCommSetting_Click(sender As Object, e As EventArgs) Handles btnSetCommSetting.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetCommSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        sbxpc.SBXPCDLL.XML_AddLong(strXML, "DeviceID", Convert.ToInt32(txtDeviceID.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "CommPwd", Convert.ToInt32(txtCommPwd.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "TcpPort", Convert.ToInt32(txtTcpPort.Text))
        sbxpc.SBXPCDLL.XML_AddString(strXML, "P2PSvr", txtP2P_Server.Text)
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "P2PPort", Convert.ToInt32(txtP2P_Port.Text))

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            MessageBox.Show("Set Communication Setting OK!")
        Else
            Dim str As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Result", str)

            MessageBox.Show("Set Communication Setting Failed.\r\nResult:" & str)
        End If
    End Sub

    Private Sub BtnGetWiFiSetting_Click(sender As Object, e As EventArgs) Handles btnGetWiFiSetting.Click
        Dim strXML As String = ""
        Dim strValue As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetWiFiSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "SSID", strValue)
            txtWiFi_SSID.Text = strValue
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Key", strValue)
            txtWiFi_Key.Text = strValue
            chkWiFi_DHCP.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(strXML, "DHCP") <> 0)
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "IP", strValue)
            txtWiFi_IP.Text = strValue
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Subnet", strValue)
            txtWiFi_Subnet.Text = strValue
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "DefaultGateway", strValue)
            txtWiFi_DefaultGateway.Text = strValue
            chkWiFi_ManualDNS.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(strXML, "ManualDNS") <> 0)
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "PrimaryDNSServer", strValue)
            txtWiFi_PrimaryDNSServer.Text = strValue
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "SecondaryDNSServer", strValue)
            txtWiFi_SecondaryDNSServer.Text = strValue

            ChkWiFi_DHCP_CheckedChanged(sender, e)
            ChkWiFi_ManualDNS_CheckedChanged(sender, e)

            MessageBox.Show("Get WiFi Setting OK!")
        Else
            MessageBox.Show("Get WiFi Setting Failed.")
        End If
    End Sub

    Private Sub BtnSetWiFiSetting_Click(sender As Object, e As EventArgs) Handles btnSetWiFiSetting.Click
        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetWiFiSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        sbxpc.SBXPCDLL.XML_AddString(strXML, "SSID", txtWiFi_SSID.Text)
        sbxpc.SBXPCDLL.XML_AddString(strXML, "Key", txtWiFi_Key.Text)
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "DHCP", Convert.ToInt32(chkWiFi_DHCP.Checked))
        If (Not chkWiFi_DHCP.Checked) Then
            sbxpc.SBXPCDLL.XML_AddString(strXML, "IP", txtWiFi_IP.Text)
            sbxpc.SBXPCDLL.XML_AddString(strXML, "Subnet", txtWiFi_Subnet.Text)
            sbxpc.SBXPCDLL.XML_AddString(strXML, "DefaultGateway", txtWiFi_DefaultGateway.Text)
        End If
        sbxpc.SBXPCDLL.XML_AddInt(strXML, "ManualDNS", Convert.ToInt32(chkWiFi_ManualDNS.Checked))
        If (chkWiFi_ManualDNS.Checked) Then
            sbxpc.SBXPCDLL.XML_AddString(strXML, "PrimaryDNSServer", txtWiFi_PrimaryDNSServer.Text)
            sbxpc.SBXPCDLL.XML_AddString(strXML, "SecondaryDNSServer", txtWiFi_SecondaryDNSServer.Text)
        End If

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            MessageBox.Show("Set WiFi Setting OK!")
        Else
            Dim str As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Result", str)

            MessageBox.Show("Set WiFi Setting Failed.\r\nResult:" & str)
        End If
    End Sub


    Private Sub FrmNetworkSetting_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        frmMain.Visible = True
    End Sub

    Private Sub ChkEther_DHCP_CheckedChanged(sender As Object, e As EventArgs) Handles chkEther_DHCP.CheckedChanged
        txtEther_IP.Enabled = Not chkEther_DHCP.Checked
        txtEther_Subnet.Enabled = Not chkEther_DHCP.Checked
        txtEther_DefaultGateway.Enabled = Not chkEther_DHCP.Checked
    End Sub

    Private Sub ChkEther_ManualDNS_CheckedChanged(sender As Object, e As EventArgs) Handles chkEther_ManualDNS.CheckedChanged
        txtEther_PrimaryDNSServer.Enabled = chkEther_ManualDNS.Checked
        txtEther_SecondaryDNSServer.Enabled = chkEther_ManualDNS.Checked
    End Sub

    Private Sub ChkWiFi_DHCP_CheckedChanged(sender As Object, e As EventArgs) Handles chkWiFi_DHCP.CheckedChanged
        txtWiFi_IP.Enabled = Not chkWiFi_DHCP.Checked
        txtWiFi_Subnet.Enabled = Not chkWiFi_DHCP.Checked
        txtWiFi_DefaultGateway.Enabled = Not chkWiFi_DHCP.Checked
    End Sub

    Private Sub ChkWiFi_ManualDNS_CheckedChanged(sender As Object, e As EventArgs) Handles chkWiFi_ManualDNS.CheckedChanged
        txtWiFi_PrimaryDNSServer.Enabled = chkWiFi_ManualDNS.Checked
        txtWiFi_SecondaryDNSServer.Enabled = chkWiFi_ManualDNS.Checked
    End Sub

    Private Sub BtnApplyCommSetting_Click(sender As Object, e As EventArgs) Handles btnApplyCommSetting.Click
        Dim strXML As String = ""

        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "ApplyCommSetting")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        sbxpc.SBXPCDLL.XML_AddInt(strXML, "Apply", 1)

        If (sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)) Then
            MessageBox.Show("Apply Settings OK!")
        Else
            Dim str As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Result", str)

            MessageBox.Show("Apply Settings Failed.\r\nResult:" & str)
        End If
    End Sub

End Class