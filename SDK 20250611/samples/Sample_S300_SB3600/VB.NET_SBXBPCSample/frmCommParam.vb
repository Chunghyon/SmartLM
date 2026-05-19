
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Partial Public Class frmCommParam
    Inherits Form
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmCommParam_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtMachineID.Text = Convert.ToString(DirectCast(Application.OpenForms("frmMain"), frmMain).cmbMachineNumber.SelectedIndex + 1)
        txtIP.Text = DirectCast(Application.OpenForms("frmMain"), frmMain).txtIPAddress.Text
        txtPort.Text = DirectCast(Application.OpenForms("frmMain"), frmMain).txtPortNo.Text

        Dim event_types As [String]() = {"NO", "TCP/IP", "RS485"}
        For Each t As [String] In event_types
            cmbEventOutType.Items.Add(t)
        Next
        cmbEventOutType.SelectedIndex = 0
    End Sub

    Private Sub frmCommParam_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.OpenForms("frmMain").Visible = True
    End Sub

    Private Sub btnGet_Click(sender As Object, e As EventArgs) Handles btnGet.Click
        Dim strXML As String = Nothing
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetCommParam")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        If sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML) Then
            txtMachineID.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "MachineID"))
            chkUseDHCP.Checked = If((sbxpc.SBXPCDLL.XML_ParseLong(strXML, "UseDHCP") = 1), True, False)
            txtIP.Text = IP_to_String(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "IP"))
            txtPort.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "Port"))
            txtSubnetMask.Text = IP_to_String(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "SubnetMask"))
            txtGateway.Text = IP_to_String(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "Gateway"))
            cmbEventOutType.SelectedIndex = sbxpc.SBXPCDLL.XML_ParseLong(strXML, "EventOutType")
            txtServerIP.Text = IP_to_String(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "ServerIP"))
            txtServerPort.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "ServerPort"))

            MessageBox.Show("Get Communication Parameters OK")
        Else
            MessageBox.Show("Get Communication Parameters Failed.")
        End If
    End Sub
    Private Sub btnSet_Click(sender As Object, e As EventArgs) Handles btnSet.Click
        If IP_to_String(String_to_IP(txtIP.Text)) <> txtIP.Text Then
            MessageBox.Show("Invalid IP.")
            Return
        End If
        If IP_to_String(String_to_IP(txtSubnetMask.Text)) <> txtSubnetMask.Text Then
            MessageBox.Show("Invalid SubnetMask.")
            Return
        End If
        If IP_to_String(String_to_IP(txtGateway.Text)) <> txtGateway.Text Then
            MessageBox.Show("Invalid Gateway.")
            Return
        End If
        If IP_to_String(String_to_IP(txtServerIP.Text)) <> txtServerIP.Text Then
            MessageBox.Show("Invalid ServerIP.")
            Return
        End If

        Dim strXML As String = Nothing
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetCommParam")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID_New", Convert.ToInt32(txtMachineID.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "UseDHCP", If(chkUseDHCP.Checked, 1, 0))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "IP", String_to_IP(txtIP.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "Port", Convert.ToInt32(txtPort.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "SubnetMask", String_to_IP(txtSubnetMask.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "Gateway", String_to_IP(txtGateway.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "EventOutType", cmbEventOutType.SelectedIndex)
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "ServerIP", String_to_IP(txtServerIP.Text))
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "ServerPort", Convert.ToInt32(txtServerPort.Text))

        If sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML) Then
            '                 string str = "";
            '                 sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Result", out str);

            MessageBox.Show("Set Communication Parameters OK")
        Else
            Dim str As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Result", str)

            MessageBox.Show(Convert.ToString("Set Communication Parameters Failed." & vbCr & vbLf & "Result:") & str)
        End If
    End Sub

    Private Function String_to_IP(IP_str As String) As Integer
        Dim t As String() = IP_str.Split("."c)

        If t.Length <> 4 Then
            Return 0
        End If

        Dim ip As Integer = Convert.ToInt32(t(0)) << 24 Or Convert.ToInt32(t(1)) << 16 Or Convert.ToInt32(t(2)) << 8 Or Convert.ToInt32(t(3)) << 0
        Return ip
    End Function

    Private Function IP_to_String(ip As Integer) As String
        Return String.Format("{0}.{1}.{2}.{3}", (ip >> 24) And &HFF, (ip >> 16) And &HFF, (ip >> 8) And &HFF, ip And &HFF)
    End Function
End Class
