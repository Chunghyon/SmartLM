Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices

Friend Class frmBellInfo
	Inherits System.Windows.Forms.Form

	Const MAX_BELLCOUNT As Integer = 42
	Dim BellInfo() As Integer
	Private Sub frmBellInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		ReDim BellInfo(MAX_BELLCOUNT * 4 - 1)
		dtStart.ShowUpDown = True

		Dim i As Integer

		For i = 0 To MAX_BELLCOUNT - 1
			BellInfo(i * 4) = 0
			BellInfo(i * 4 + 1) = 0
			BellInfo(i * 4 + 2) = 0
			BellInfo(i * 4 + 3) = 0
		Next
		DrawBellInfo()
	End Sub
	Private Sub DrawBellInfo()
		Dim itemString As String = ""
		lstTimeZone.Items.Clear()
		Dim valid, startHour, startMinute, weekDay As Integer

		Dim i As Integer

		For i = 0 To MAX_BELLCOUNT - 1
			valid = BellInfo(i * 4)
			startHour = BellInfo(i * 4 + 1)
			startMinute = BellInfo(i * 4 + 2)
			weekDay = BellInfo(i * 4 + 3)

			itemString = "[" + String.Format("{0:D2} ] ", i)
			itemString += "[VALID] " + String.Format("{0:D1} ", valid)
			itemString += "[TIME] " + String.Format("{0:D2}:{1:D2} ", startHour, startMinute)
			itemString += "[WEEKDAY] " + String.Format("{0:D1} ", weekDay)
			lstTimeZone.Items.Add(itemString)
		Next
	End Sub
	Private Sub cmdRead_Click(sender As Object, e As EventArgs) Handles cmdRead.Click
		Dim vRet As Boolean = True
		Dim vErrorCode As Integer = 0

		lblMessage.Text = "Waiting..."
		Application.DoEvents()

		If (Not sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 0)) Then ' 0  :  false
			lblMessage.Text = gstrNoDevice
			Exit Sub
		End If

		Dim strXML As String = ""
		sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "GetBellTime42")
		sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
		sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

		vRet = sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)

		If (vRet) Then
			txtBellCount.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "BellRingTimes"))
			txtBellPeriod.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(strXML, "BellPeriod"))

			Dim i As Integer
			For i = 0 To MAX_BELLCOUNT - 1
				BellInfo(i * 4) = sbxpc.SBXPCDLL.XML_ParseLong(strXML, String.Format("BellValid_{0:D2}", i))
				BellInfo(i * 4 + 1) = sbxpc.SBXPCDLL.XML_ParseLong(strXML, String.Format("BellHour_{0:D2}", i))
				BellInfo(i * 4 + 2) = sbxpc.SBXPCDLL.XML_ParseLong(strXML, String.Format("BellMin_{0:D2}", i))
				BellInfo(i * 4 + 3) = sbxpc.SBXPCDLL.XML_ParseLong(strXML, String.Format("BellDay_{0:D2}", i))
			Next
			DrawBellInfo()

			lblMessage.Text = "Success!"
		Else
			sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
			lblMessage.Text = ErrorPrint(vErrorCode)
		End If
		sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 1)  ' 1 : true
	End Sub

	Private Sub cmdWrite_Click(sender As Object, e As EventArgs) Handles cmdWrite.Click
		Dim vRet As Boolean
		Dim vErrorCode As Integer = 0

		lblMessage.Text = "Waiting..."
		Application.DoEvents()

		If (Not sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 0)) Then ' 0 : false
			lblMessage.Text = gstrNoDevice
			Exit Sub
		End If

		Dim strXML As String = ""
		sbxpc.SBXPCDLL.XML_AddString(strXML, "REQUEST", "SetBellTime42")
		sbxpc.SBXPCDLL.XML_AddString(strXML, "MSGTYPE", "request")
		sbxpc.SBXPCDLL.XML_AddLong(strXML, "MachineID", frmMain.gMachineNumber)

		sbxpc.SBXPCDLL.XML_AddLong(strXML, "BellRingTimes", Convert.ToInt32(txtBellCount.Text))
		sbxpc.SBXPCDLL.XML_AddLong(strXML, "BellPeriod", Convert.ToInt32(txtBellPeriod.Text))
		Dim i As Integer
		For i = 0 To MAX_BELLCOUNT - 1
			sbxpc.SBXPCDLL.XML_AddLong(strXML, String.Format("BellValid_{0:D2}", i), BellInfo(i * 4))
			sbxpc.SBXPCDLL.XML_AddLong(strXML, String.Format("BellHour_{0:D2}", i), BellInfo(i * 4 + 1))
			sbxpc.SBXPCDLL.XML_AddLong(strXML, String.Format("BellMin_{0:D2}", i), BellInfo(i * 4 + 2))
			sbxpc.SBXPCDLL.XML_AddLong(strXML, String.Format("BellDay_{0:D2}", i), BellInfo(i * 4 + 3))
		Next

		vRet = sbxpc.SBXPCDLL.GeneralOperationXML(frmMain.gMachineNumber, strXML)

		If (vRet) Then
			lblMessage.Text = "Success!"
		Else
			sbxpc.SBXPCDLL.GetLastError(frmMain.gMachineNumber, vErrorCode)
			lblMessage.Text = ErrorPrint(vErrorCode)
		End If

		sbxpc.SBXPCDLL.EnableDevice(frmMain.gMachineNumber, 1) ' 1 : true
	End Sub

	Private Sub cmdExit_Click(sender As Object, e As EventArgs) Handles cmdExit.Click
		Close()
	End Sub
	Private Sub lstTimeZone_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstTimeZone.SelectedIndexChanged
		If (lstTimeZone.SelectedIndex = -1) Then
			Exit Sub
		End If

		Dim Index As Integer = lstTimeZone.SelectedIndex

		chkUsed.Checked = (BellInfo(Index * 4) <> 0)
		dtStart.Value = New DateTime(2000, 1, 1,         ' Don't care year/month/date
										BellInfo(Index * 4 + 1),
										BellInfo(Index * 4 + 2),
										0
									)
		cmbWeekday.SelectedIndex = BellInfo(Index * 4 + 3)
	End Sub

	Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
		If (lstTimeZone.SelectedIndex = -1) Then
			Exit Sub
		End If
		Dim Index As Integer = lstTimeZone.SelectedIndex

		BellInfo(Index * 4) = IIf(chkUsed.Checked, 1, 0)
		BellInfo(Index * 4 + 1) = dtStart.Value.Hour
		BellInfo(Index * 4 + 2) = dtStart.Value.Minute
		BellInfo(Index * 4 + 3) = cmbWeekday.SelectedIndex
		DrawBellInfo()
	End Sub

	Private Sub frmBellInfo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
		frmMain.Visible = True
	End Sub
End Class