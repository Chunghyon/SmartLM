Public Class frmTrString
    Private Sub CmdRead_Click(sender As Object, e As EventArgs) Handles cmdRead.Click
        Dim bRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        Application.DoEvents()

        If (txtTrNo.Text.Length = 0) Then
            lblMessage.Text = "Please input TrNo"
            Exit Sub
        End If

        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetTrString")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "TrNo", Convert.ToInt32(txtTrNo.Text))

        bRet = sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)

        If (bRet) Then
            lblMessage.Text = "Tr[" & txtTrNo.Text & "]: "

            Dim base64_name As String = Nothing
            If (Not sbxpc.SBXPCDLL.XML_ParseString(strXML, "TrName", base64_name)) Then
                Dim error_info As String = Nothing
                sbxpc.SBXPCDLL.XML_ParseString(strXML, "Error", error_info)
                lblMessage.Text = "Error: " + error_info

            Else
                If (base64_name <> Nothing) Then
                    Try
                        Dim name_binary() As Byte = Convert.FromBase64String(base64_name)
                        Dim Index As Integer = 0
                        For i As Integer = 0 To (name_binary.Length - 1) - 1 Step 2
                            If (name_binary(i) = 0 And name_binary(i + 1) = 0) Then
                                Index = i
                                Exit For
                            End If
                        Next

                        txtTrString.Text = System.Text.Encoding.Unicode.GetString(name_binary, 0, Index)
                        lblMessage.Text += txtTrString.Text
                    Catch
                    End Try
                End If
            End If

        Else
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub CmdWrite_Click(sender As Object, e As EventArgs) Handles cmdWrite.Click
        lblMessage.Text = "Working..."
        Application.DoEvents()

        If (txtTrNo.Text.Length = 0) Then
            lblMessage.Text = "Please input TrNo"
            Exit Sub
        End If

        Dim strXML As String = ""
        sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetTrString")
        sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)
        sbxpc.SBXPCDLL.XML_AddLong(strXML, "TrNo", Convert.ToInt32(txtTrNo.Text))
        Dim name_binary() As Byte = System.Text.Encoding.Unicode.GetBytes(txtTrString.Text)
        sbxpc.SBXPCDLL.XML_AddString(strXML, "TrName", Convert.ToBase64String(name_binary))
        Dim bRet As Boolean = sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)

        If (bRet) Then
            Dim error_info As String = ""
            sbxpc.SBXPCDLL.XML_ParseString(strXML, "Error", error_info)
            If (error_info = "Success") Then
                lblMessage.Text = "Set OK. Tr[" & txtTrNo.Text & "]: " & txtTrString.Text
            Else
                lblMessage.Text = "Error: " & error_info
            End If
        Else
            Dim vErrorCode As Integer
            sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If
    End Sub

    Private Sub FrmTrString_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        frmMain.Visible = True
    End Sub
End Class