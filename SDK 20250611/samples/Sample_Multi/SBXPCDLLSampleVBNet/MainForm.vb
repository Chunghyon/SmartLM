' SBXPC Multi-Thread Safe C# sample
' © 2004-2012 Beijing Smackbio Technology Co., Ltd.  All rights reserved.
' This sample shows how to use SBXPC's Multi-Thread Safe function 

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Imports System.Threading

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security.Permissions

Imports System.Diagnostics
Imports System.IO
Imports Microsoft.Win32

Imports System.Drawing.Imaging

Imports System.Text.RegularExpressions

Partial Public Class MainForm
    Inherits Form
    Public Sub MyTrace(format As String, ParamArray args As Object())
        BeginInvoke(m_delegateAddLog, String.Format(format, args))
    End Sub

    Public Sub OCXThreadProc(param As Object)
        Dim th As Thread = Thread.CurrentThread
        Dim p As OCXThreadParam = param

        p.mainForm.MyTrace("{0}: Thread started", th.Name)

        Dim ppsIpAddr As IntPtr = Marshal.AllocHGlobal(IntPtr.Size)
        Dim psIpAddr As IntPtr = Marshal.StringToBSTR(p.sIpAddr)
        Marshal.WriteIntPtr(ppsIpAddr, psIpAddr)

        While True
            Try
                p.mainForm.MyTrace("{0}: Connecting...", th.Name)
                Dim ret As Integer = SBXPCDLL._ConnectTcpip(p.nDevID, ppsIpAddr, p.nPortNum, p.nPassword)
                p.mainForm.MyTrace("{0}: Connecting result = {1}", Me.Name, ret)

                If ret <> 0 Then
                    Dim dwYear As Integer = 0
                    Dim dwMonth As Integer = 0
                    Dim dwDay As Integer = 0
                    Dim dwHour As Integer = 0
                    Dim dwMinute As Integer = 0
                    Dim dwSecond As Integer = 0
                    Dim dwDayOfWeek As Integer = 0

                    ret = SBXPCDLL._GetDeviceTime(p.nDevID, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, dwDayOfWeek)
                    If ret <> 0 Then
                        p.mainForm.MyTrace("{0}: The Device's Time = {1}/{2}/{3} {4}:{5}:{6}", Me.Name, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond)
                    End If
                    Thread.Sleep(1000)
                End If

                SBXPCDLL._Disconnect(p.nDevID)
                p.mainForm.MyTrace("{0}: Disconnected", th.Name)

                Thread.Sleep(10)
            Catch ex As ThreadAbortException
                SBXPCDLL._Disconnect(p.nDevID)

                p.mainForm.MyTrace("{0}: Thread stopped", th.Name)
                Marshal.FreeHGlobal(ppsIpAddr)
                Marshal.FreeBSTR(psIpAddr)
            End Try
        End While
    End Sub

    Dim m_Working As Boolean
    Dim OCXThread1 As Thread, OCXThread2 As Thread
    Delegate Sub delegateAddLog(sLogLine As String)
    Dim m_delegateAddLog As delegateAddLog
    Sub AddLog(sLogLine As String)
        rtbLog.Text += (sLogLine + Chr(13) + Chr(10))
        rtbLog.Select(rtbLog.Text.Length, 0)
    End Sub

    Public Sub New()
        InitializeComponent()
        m_Working = False
        m_delegateAddLog = New delegateAddLog(AddressOf AddLog)
    End Sub

    Private Sub btnStartMulti_Click(sender As Object, e As EventArgs) Handles btnStartMulti.Click
        If (Not m_Working) Then
            btnStartMulti.Text = "Stop"
            m_Working = True

            OCXThread1 = New Thread(New ParameterizedThreadStart(AddressOf OCXThreadProc))
            OCXThread1.Name = "OCXThread1"
            OCXThread1.Start(New OCXThreadParam(Me, 2, "192.168.1.37", 5005, 0))

            OCXThread2 = New Thread(New ParameterizedThreadStart(AddressOf OCXThreadProc))
            OCXThread2.Name = "OCXThread2"
            OCXThread2.Start(New OCXThreadParam(Me, 1, "192.168.1.16", 5005, 0))
        Else
            OCXThread1.Abort()
            OCXThread1.Join()
            OCXThread2.Abort()
            OCXThread2.Join()

            btnStartMulti.Text = "Start"
            m_Working = False
        End If
    End Sub


    Public Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If m_Working Then
            e.Cancel = True
        End If
    End Sub
    Public Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SBXPCDLL._DotNET()
    End Sub
End Class

Public Class OCXThreadParam
    Public mainForm As MainForm
    Public nDevID As Integer
    Public sIpAddr As String
    Public nPortNum As Integer
    Public nPassword As Integer

    Public Sub New(_mainForm As MainForm, _nDevID As Integer, _sIpAddr As String, _nPortNum As Integer, _nPassword As Integer)
        mainForm = _mainForm
        nDevID = _nDevID
        sIpAddr = _sIpAddr
        nPortNum = _nPortNum
        nPassword = _nPassword
    End Sub
End Class

Public Class SBXPCDLL
    <DllImport("SBXPCDLL.dll", CallingConvention:=CallingConvention.Winapi)> _
    Public Shared Sub _DotNET()
    End Sub

    <DllImport("SBXPCDLL.dll", CallingConvention:=CallingConvention.Winapi)> _
    Public Shared Function _ConnectTcpip(dwMachineNumber As Integer, lpszIPAddress As IntPtr, dwPortNumber As Integer, dwPassWord As Integer) As Integer
    End Function

    <DllImport("SBXPCDLL.dll", CallingConvention:=CallingConvention.Winapi)> _
    Public Shared Sub _Disconnect(dwMachineNumber As Integer)
    End Sub

    <DllImport("SBXPCDLL.dll", CallingConvention:=CallingConvention.Winapi)> _
    Public Shared Function _GetDeviceTime(dwMachineNumber As Integer, ByRef dwYear As Integer, ByRef dwMonth As Integer, ByRef dwDay As Integer, ByRef dwHour As Integer, ByRef dwMinute As Integer, ByRef dwSecond As Integer, ByRef dwDayOfWeek As Integer) As Integer
    End Function
End Class
