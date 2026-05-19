Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Runtime.InteropServices

Friend Class frmEnroll
    Inherits System.Windows.Forms.Form
    Const DATASIZE As Short = (1404 + 12) / 4 'SmackBio
    Const NAMESIZE As Short = 54
    Public gGetState As Boolean
    'Dim glngEnrollData As Object
    Dim gTemplngEnrollData(DATASIZE) As Integer
    Dim glngEnrollPData As Integer
    Dim gbytEnrollData(DATASIZE * 5) As Byte
    Dim mMachineNumber As Integer
    Dim mDeviceKind As Integer
    Dim mDeviceVer As Integer
    Dim mConvKind As Integer

    Private dsEnrolls As DataSet


    Private Sub chkEnable_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkEnable.CheckStateChanged
        If chkEnable.CheckState = 1 Then
            chkEnable.Text = "Enable User"
        Else
            chkEnable.Text = "Disable User"
        End If
    End Sub

    Private Sub cmdClearData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClearData.Click
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = frmMain.SBXPC1.ClearKeeperData(mMachineNumber)
        If vRet = True Then
            lblMessage.Text = "ClearKeeperData OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdDel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDel.Click
        '        If MsgBox("Are you sure to clear backup database?", MsgBoxStyle.YesNo, "Enroll Management") _
        '                  = MsgBoxResult.No Then
        '           Exit Sub
        '        End If

        EnrollData.DataModule.DeleteDB()

        lblMessage.Text = "Deleted Database"
        Label2.Text = "Total : " & 0

    End Sub

    Private Sub cmdDeleteCompany_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteCompany.Click
        Dim vEMachineNumber As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        Dim vName As String = ""

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vEMachineNumber = CInt(cmbEMachineNumber.Text)

        vRet = frmMain.SBXPC1.SetCompanyName1(mMachineNumber, 0, vName)
        If vRet = True Then
            lblMessage.Text = "Delete Company Name OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)

    End Sub

    Private Sub cmdDeleteEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteEnrollData.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vFingerNumber As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vEnrollNumber = Val(txtEnrollNumber.Text)
        vEMachineNumber = CInt(cmbEMachineNumber.Text)
        vFingerNumber = CInt(cmbBackupNumber.Text)

        vRet = frmMain.SBXPC1.DeleteEnrollData(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber)
        If vRet = True Then
            lblMessage.Text = "DeleteEnrollData OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdEmptyEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEmptyEnrollData.Click
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = frmMain.SBXPC1.EmptyEnrollData(mMachineNumber)
        If vRet = True Then
            lblMessage.Text = "Success!"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdEnableUser_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEnableUser.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vFingerNumber As Integer
        Dim vFlag As Boolean
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vEMachineNumber = cmbEMachineNumber.SelectedIndex + 1
        vEnrollNumber = Val(txtEnrollNumber.Text)
        vFingerNumber = CInt(cmbBackupNumber.Text)
        vFlag = chkEnable.CheckState

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = frmMain.SBXPC1.EnableUser(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vFlag)
        If vRet = True Then
            lblMessage.Text = "Success!"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdGetAllEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGetAllEnrollData.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vFingerNumber As Integer
        Dim vPrivilege As Integer
        Dim vEnable As Integer
        Dim vFlag As Boolean
        Dim vRet As Integer
        Dim vErrorCode As Integer
        Dim vStr As String = ""
        Dim i As Integer
        Dim vTitle As String

        Dim dbEnrollTble As DataTable
        Dim dbRow As DataRow
        Dim dsChange As DataSet

        Dim gh As GCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned)
        Dim AddrOfTemplngEnrollData As IntPtr = gh.AddrOfPinnedObject()

        lstEnrollData.Items.Clear()
        vTitle = Me.Text
        Label2.Text = ""
        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = frmMain.SBXPC1.ReadAllUserID(mMachineNumber)
        If vRet = True Then
            lblMessage.Text = "ReadAllUserID OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
            frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
            Exit Sub
        End If

        '---- Get Enroll data and save into database -------------
        Cursor = System.Windows.Forms.Cursors.WaitCursor
        vFlag = False

        dbEnrollTble = dsEnrolls.Tables(0)

        gGetState = True

        Do
            vRet = frmMain.SBXPC1.GetAllUserID(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vPrivilege, vEnable)
            If vRet <> True Then Exit Do
            vFlag = True
EEE:

            If vFingerNumber >= 50 Then Continue Do

            vRet = frmMain.SBXPC1.GetEnrollData1(mMachineNumber, _
                                                   vEnrollNumber, _
                                                   vFingerNumber, _
                                                   vPrivilege, _
                                                   AddrOfTemplngEnrollData.ToInt32(), _
                                                   glngEnrollPData)

            If vRet <> True Then
                vFlag = False
                vStr = "GetEnrollData"
                frmMain.SBXPC1.GetLastError(vErrorCode)
                vRet = MsgBox(ErrorPrint(vErrorCode) & ": Continue ?", MsgBoxStyle.YesNoCancel, "GetEnrollData")
                If vRet = MsgBoxResult.Yes Then
                    GoTo EEE
                ElseIf vRet = MsgBoxResult.Cancel Then
                    Cursor = System.Windows.Forms.Cursors.Default
                    frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
                    gGetState = False
                    Exit Sub
                End If
            End If

            ' ----------------------
            For Each dbRow In dbEnrollTble.Rows
                If ((dbRow("EnrollNumber") = vEnrollNumber) And _
                        (dbRow("EMachineNumber") = vEMachineNumber) And _
                        (dbRow("FingerNumber") = vFingerNumber)) Then
                    lblMessage.Text = "Double ID"
                    GoTo FFF
                End If
            Next

            dbRow = dbEnrollTble.NewRow()
            dbRow("EMachineNumber") = vEMachineNumber
            dbRow("EnrollNumber") = vEnrollNumber
            dbRow("FingerNumber") = vFingerNumber
            dbRow("Privilige") = vPrivilege

            If vFingerNumber = 10 Then
                dbRow("Password1") = glngEnrollPData
            ElseIf vFingerNumber = 11 Then
                dbRow("Password1") = glngEnrollPData
            Else
                dbRow("Password1") = 0

                For i = 0 To DATASIZE - 1
                    gbytEnrollData(i * 5) = 1

                    If gTemplngEnrollData(i) < 0 Then
                        gbytEnrollData(i * 5) = 0
                        gTemplngEnrollData(i) = System.Math.Abs(gTemplngEnrollData(i))
                    End If

                    gbytEnrollData(i * 5 + 1) = (gTemplngEnrollData(i) \ 256 \ 256 \ 256)
                    gbytEnrollData(i * 5 + 2) = (gTemplngEnrollData(i) \ 256 \ 256) Mod 256
                    gbytEnrollData(i * 5 + 3) = (gTemplngEnrollData(i) \ 256) Mod 256
                    gbytEnrollData(i * 5 + 4) = gTemplngEnrollData(i) Mod 256
                Next

                'dbRow("FPdata") = gbytEnrollData        '<---------- Error

                Dim gbyt(DATASIZE * 5 - 1) As Byte
                For i = 0 To DATASIZE * 5 - 1
                    gbyt(i) = gbytEnrollData(i)
                Next
                dbRow("FPdata") = gbyt

            End If

            dbEnrollTble.Rows.Add(dbRow)
FFF:

            lblMessage.Text = VB6.Format(vEMachineNumber, "00#") & "-" & VB6.Format(vEnrollNumber, "0000#") & "-" & vFingerNumber
            Me.Text = VB6.Format(vEnrollNumber, "0000#")
            txtEnrollNumber.Text = CStr(vEnrollNumber)
            cmbBackupNumber.Text = CStr(vFingerNumber)
            cmbEMachineNumber.Text = CStr(vEMachineNumber)
            cmbPrivilege.Text = CStr(vPrivilege)
            System.Windows.Forms.Application.DoEvents()
        Loop

        Label2.Text = "Total : " & dsEnrolls.Tables.Item("tblEnroll").Rows.Count
        dsChange = dsEnrolls.GetChanges
        EnrollData.DataModule.SaveEnrolls(dsEnrolls)

        gh.Free()

        gGetState = False

        vTitle = Me.Text
        Cursor = System.Windows.Forms.Cursors.Default

        If vFlag = True Then
            lblMessage.Text = "GetAllUserID OK"
        Else
            lblMessage.Text = vStr & ":" & ErrorPrint(vErrorCode)
        End If

        System.Windows.Forms.Application.DoEvents()
        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdGetEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGetEnrollData.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vFingerNumber As Integer
        Dim vPrivilege As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim i As Integer

        lstEnrollData.Items.Clear()
        Label2.Text = ""
        lstEnrollData.Items.Clear()
        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vEnrollNumber = Val(txtEnrollNumber.Text)
        vFingerNumber = CInt(cmbBackupNumber.Text)
        vEMachineNumber = CInt(cmbEMachineNumber.Text)
        If vFingerNumber = 10 Then vFingerNumber = 15

        Dim gh As GCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned)
        Dim AddrOfTemplngEnrollData As IntPtr = gh.AddrOfPinnedObject()

        vRet = frmMain.SBXPC1.GetEnrollData1(mMachineNumber, _
                                                vEnrollNumber, _
                                                vFingerNumber, _
                                                vPrivilege, _
                                                AddrOfTemplngEnrollData.ToInt32(), _
                                                glngEnrollPData)

        gh.Free()


        If vRet = True Then
            cmbPrivilege.SelectedIndex = vPrivilege
            lblMessage.Text = "GetEnrollData OK"
            If vFingerNumber = 11 Then
                txtCardNumber.Text = Convert.ToString(glngEnrollPData, 16).ToUpper()
            ElseIf vFingerNumber = 14 Then
                txtUserTz1.Text = glngEnrollPData \ 256
                txtUserTz2.Text = glngEnrollPData Mod 256
            ElseIf vFingerNumber = 15 Then
                txtCardNumber.Text = ""
                While glngEnrollPData > 0
                    i = glngEnrollPData Mod 16 - 1
                    txtCardNumber.Text = txtCardNumber.Text & Convert.ToString(i)
                    glngEnrollPData = glngEnrollPData \ 16
                End While
            Else
                For i = 0 To DATASIZE - 1
                    lstEnrollData.Items.Add((CStr(gTemplngEnrollData(i))))
                Next
            End If
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

            frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdGetEnrollInfo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGetEnrollInfo.Click
        Dim vEMachineNumber As Integer
        Dim vEnrollNumber As Integer
        Dim vFingerNumber As Integer
        Dim vPrivilege As Integer
        Dim vEnable As Integer
        Dim vRet As Integer
        Dim vFlag As Boolean
        Dim vErrorCode As Integer
        Dim i As Integer

        lblEnrollData.Text = "User IDs"
        lstEnrollData.Items.Clear()
        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = frmMain.SBXPC1.ReadAllUserID(mMachineNumber)
        If vRet = True Then
            lblMessage.Text = "ReadAllUserID OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
            frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
            Exit Sub
        End If

        '------ Show all enroll information ----------
        vFlag = False
        i = 0
        lstEnrollData.Items.Add(("No.     EnNo    EMNo    Fp      Priv  Enable"))
        Do
            vRet = frmMain.SBXPC1.GetAllUserID(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vPrivilege, vEnable)
            If vRet <> True Then Exit Do
            vFlag = True
            lstEnrollData.Items.Add((VB6.Format(i, "00#") & "    " & VB6.Format(vEnrollNumber, "0000#") & "     " & VB6.Format(vEMachineNumber, "00#") & "       " & VB6.Format(vFingerNumber, "0#") & "       " & CStr(vPrivilege) & "        " & CStr(vEnable Mod 256)))

            i = i + 1
            Label2.Text = "Total : " & i
        Loop

        If vFlag = True Then
            lblMessage.Text = "GetAllUserID OK"
        Else
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdGetName_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGetName.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim vName As String

        vName = "            "

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vEnrollNumber = Val(txtEnrollNumber.Text)
        vEMachineNumber = CInt(cmbEMachineNumber.Text)

        vRet = frmMain.SBXPC1.GetUserName1(mMachineNumber, _
                                             vEnrollNumber, _
                                             vName)

        If vRet = True Then
            txtName.Text = vName    'vName 'SmackBio
            lblMessage.Text = "GetUserName OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdModifyPrivilege_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdModifyPrivilege.Click
        Dim vEnrollNumber As Integer
        Dim vFingerNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vMachinePrivilege As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vEnrollNumber = Val(txtEnrollNumber.Text)
        vEMachineNumber = cmbEMachineNumber.SelectedIndex + 1
        vFingerNumber = CInt(cmbBackupNumber.Text)
        vMachinePrivilege = cmbPrivilege.SelectedIndex

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vRet = frmMain.SBXPC1.ModifyPrivilege(mMachineNumber, vEnrollNumber, vEMachineNumber, vFingerNumber, vMachinePrivilege)
        If vRet = True Then
            lblMessage.Text = "Success!"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdSendAllEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSendAllEnrollData.Click

    End Sub

    Private Sub cmdSetAllEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSetAllEnrollData.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vFingerNumber As Integer
        Dim vPrivilege As Integer
        Dim vFlag As Boolean
        Dim vRet As Integer
        Dim vErrorCode As Integer
        Dim vStr As String
        Dim vByte() As Byte
        Dim i As Integer
        Dim vTitle As String

        Dim dbEnrollTble As DataTable
        Dim dbRow As DataRow

        Dim gh As GCHandle

        Dim num As Integer


        lstEnrollData.Items.Clear()
        vTitle = Me.Text
        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vFlag = False
        gGetState = True
        Cursor = System.Windows.Forms.Cursors.WaitCursor

        dbEnrollTble = dsEnrolls.Tables(0)
        num = 0

        If dbEnrollTble.Rows.Count = 0 Then GoTo EEE

        For Each dbRow In dbEnrollTble.Rows

            vEMachineNumber = dbRow("EMachineNumber")
            vEnrollNumber = dbRow("EnrollNumber")
            vFingerNumber = dbRow("FingerNumber")
            vPrivilege = dbRow("Privilige")
            glngEnrollPData = dbRow("Password1")

            num = num + 1

            If vFingerNumber < 10 Then
                vByte = dbRow("FPData")

                For i = 0 To DATASIZE - 1
                    gTemplngEnrollData(i) = vByte(i * 5 + 1)
                    gTemplngEnrollData(i) = gTemplngEnrollData(i) * 256 + vByte(i * 5 + 2)
                    gTemplngEnrollData(i) = gTemplngEnrollData(i) * 256 + vByte(i * 5 + 3)
                    gTemplngEnrollData(i) = gTemplngEnrollData(i) * 256 + vByte(i * 5 + 4)
                    If vByte(i * 5) = 0 Then
                        gTemplngEnrollData(i) = 0 - gTemplngEnrollData(i)
                    End If
                Next
            End If
FFF:
            gh = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned)
            Dim AddrOfTemplngEnrollData As IntPtr = gh.AddrOfPinnedObject()

            vRet = frmMain.SBXPC1.SetEnrollData1(mMachineNumber, _
                                                    vEnrollNumber, _
                                                    vFingerNumber, _
                                                    vPrivilege, _
                                                    AddrOfTemplngEnrollData.ToInt32(), _
                                                    glngEnrollPData)

            If vRet <> True Then
                vFlag = False
                vStr = "SetEnrollData"
                frmMain.SBXPC1.GetLastError(vErrorCode)
                vRet = MsgBox(ErrorPrint(vErrorCode) & ": Continue ?", MsgBoxStyle.YesNoCancel, "SetEnrollData")
                If vRet = MsgBoxResult.Yes Then GoTo FFF
                If vRet = MsgBoxResult.Cancel Then GoTo EEE
            End If

LLL:
            lblMessage.Text = "EMachine = " & VB6.Format(vEMachineNumber, "00#") & ", ID = " & VB6.Format(vEnrollNumber, "000#") & ", FpNo = " & vFingerNumber & ", Count = " & num

            Me.Text = CStr(num)
            System.Windows.Forms.Application.DoEvents()
        Next

EEE:
        vTitle = Me.Text
        Cursor = System.Windows.Forms.Cursors.Default
        gGetState = False

        lblMessage.Text = "SetAllUserData OK"
        System.Windows.Forms.Application.DoEvents()

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdSetCompany_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSetCompany.Click
        Dim vEMachineNumber As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer
        Dim vName As String

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vEMachineNumber = CInt(cmbEMachineNumber.Text)

        vName = txtName.Text 'SmackBio

        vRet = frmMain.SBXPC1.SetCompanyName1(mMachineNumber, 1, vName)
        If vRet = True Then
            lblMessage.Text = "Set Company Name OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)

    End Sub

    Private Sub cmdSetEnrollData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSetEnrollData.Click
        Dim vEnrollNumber As Integer
        Dim vCardNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vFingerNumber As Integer
        Dim vPrivilege As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vEnrollNumber = Val(txtEnrollNumber.Text)
        vCardNumber = Convert.ToInt32(txtCardNumber.Text, 16)
        vFingerNumber = CInt(cmbBackupNumber.Text)
        vPrivilege = CInt(cmbPrivilege.Text)
        vEMachineNumber = CInt(cmbEMachineNumber.Text)

        If vFingerNumber = 10 Then vFingerNumber = 15
        ' Card Number valid
        If vFingerNumber = 11 Then
            If vCardNumber <> 0 Then
                glngEnrollPData = vCardNumber
            End If
        ElseIf vFingerNumber = 14 Then
            glngEnrollPData = Convert.ToInt32(txtUserTz1.Text) * 256 + Convert.ToInt32(txtUserTz2.Text)
        ElseIf vFingerNumber = 15 Then
            Dim i As Integer = txtCardNumber.Text.Length
            If i > 4 Then i = 4
            glngEnrollPData = 0
            While i > 0
                glngEnrollPData = glngEnrollPData * 16 + Convert.ToInt16(Mid(txtCardNumber.Text, i, 1)) + 1
                i = i - 1
            End While
        End If



        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        Dim gh As GCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned)
        Dim AddrOfTemplngEnrollData As IntPtr = gh.AddrOfPinnedObject()

        vRet = frmMain.SBXPC1.SetEnrollData1(mMachineNumber, _
                                               vEnrollNumber, _
                                               vFingerNumber, _
                                               vPrivilege, _
                                               AddrOfTemplngEnrollData.ToInt32(), _
                                               glngEnrollPData)
        gh.Free()

        If vRet = True Then
            lblMessage.Text = "SetEnrollData OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub cmdSetName_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSetName.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        Dim vName As String

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vEnrollNumber = Val(txtEnrollNumber.Text)
        vEMachineNumber = CInt(cmbEMachineNumber.Text)

        'UPGRADE_WARNING: Couldn't resolve default property of object glngUserName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vName = txtName.Text 'SmackBio
        'vRet = FontForFK1.GetTextBitmap(mDeviceKind, txtName.Text, glngUserName) 'SmackBio
        vRet = frmMain.SBXPC1.SetUserName1(mMachineNumber, vEnrollNumber, vName)
        If vRet = True Then
            lblMessage.Text = "SetUserName OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

    Private Sub frmEnroll_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        frmMain.Visible = True
    End Sub

    Private Sub frmEnroll_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim nLangId As Short
        nLangId = GetSystemDefaultLangID()
        If nLangId = &H41E Then
            txtName.Font = VB6.FontChangeName(txtName.Font, "Cordia New")
            txtName.Font = VB6.FontChangeGdiCharSet(txtName.Font, 222)
        End If
        cmbBackupNumber.SelectedIndex = 0
        cmbEMachineNumber.SelectedIndex = 0
        txtEnrollNumber.Text = CStr(1)
        txtCardNumber.Text = CStr(0)
        cmbPrivilege.Text = CStr(0)
        gGetState = False


        mMachineNumber = frmMain.gMachineNumber
        mConvKind = 0

        Dim sPath As String = Application.ExecutablePath
        sPath = System.IO.Path.GetDirectoryName(sPath)

        If sPath.EndsWith("\bin") Then
            sPath = sPath.Substring(0, Len(sPath) - 4)
        End If

        EnrollData.DataModule = New EnrollData(sPath)
        dsEnrolls = EnrollData.DataModule.GetEnrollDatas()

        Label2.Text = "Total : " & dsEnrolls.Tables(0).Rows.Count
    End Sub

    Private Sub cmdModifyDuressFP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModifyDuressFP.Click
        Dim vEnrollNumber As Integer
        Dim vEMachineNumber As Integer
        Dim vFingerNumber As Integer
        Dim vRet As Boolean
        Dim vErrorCode As Integer

        Dim vDuressSetting As Integer

        lblMessage.Text = "Working..."
        System.Windows.Forms.Application.DoEvents()

        vRet = frmMain.SBXPC1.EnableDevice(mMachineNumber, False)
        If vRet = False Then
            lblMessage.Text = gstrNoDevice
            Exit Sub
        End If

        vEnrollNumber = Val(txtEnrollNumber.Text)
        vEMachineNumber = CInt(cmbEMachineNumber.Text)
        vFingerNumber = Val(cmbBackupNumber.Text)
        vDuressSetting = cmbDuressSetting.SelectedIndex 'SmackBio
        vRet = frmMain.SBXPC1.ModifyDuressFP(mMachineNumber, vEnrollNumber, vFingerNumber, vDuressSetting)

        If vRet = True Then
            lblMessage.Text = "Modify Duress FP OK"
        Else
            frmMain.SBXPC1.GetLastError(vErrorCode)
            lblMessage.Text = ErrorPrint(vErrorCode)
        End If

        frmMain.SBXPC1.EnableDevice(mMachineNumber, True)
    End Sub

End Class