
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Windows.Forms
Imports System.Threading

Partial Public Class frmDeviceSearch
    Inherits Form
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmDeviceSearch_Closed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.OpenForms("frmMain").Visible = True
        DirectCast(Application.OpenForms("frmMain"), frmMain).btnSearch.Enabled = True
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        txtResult.Text = ""
        txtResult.Text += "Prefix : " + txtProductName.Text + vbCr & vbLf
        txtResult.Text += "Device Searching ............" & vbCr & vbLf & vbCr & vbLf
        Application.DoEvents()
        Thread.Sleep(300)

        btnSearch.Enabled = False

        Dim duration As UInt32 = Convert.ToUInt32(txtDuration.Text)

        Dim req As New my_util.DeviceDiscoverRequest(txtProductName.Text)

        Dim sock As New UdpClient()
        Dim iep As New IPEndPoint(IPAddress.Parse("255.255.255.255"), CInt(my_util.DEVICE_DISCOVER_PORT))

        sock.Client.ReceiveTimeout = 1000

        Dim data As Byte() = req.toBytes()

        ' txtResult.Text += "------> Send Req Packet\r\n"; Application.DoEvents();
        sock.Send(data, data.Length, iep)

        '''//////////////////////////////////////////////////////////////////
        Dim from As New IPEndPoint(IPAddress.Any, 0)

        Dim data_recv As Byte() = New Byte(my_util.DeviceDiscoverResponse.getSize() - 1) {}

        Dim dwTime As Int32 = Environment.TickCount
        Dim count As Integer = 0

        Do
            Try
                data_recv = sock.Receive(from)
                Dim res As New my_util.DeviceDiscoverResponse(data_recv)

                If res.is_magic_valid() Then
                    FoundDevice(res)
                    count += 1
                Else
                    txtResult.Text += "xxx UnMatched xxx" & vbCr & vbLf
                End If

            Catch generatedExceptionName As Exception
            End Try
        Loop While Environment.TickCount - dwTime < duration

        sock.Close()

        txtResult.Text += vbCr & vbLf & "===== Search Finished. =====" & vbCr & vbLf
        txtResult.Text += String.Format("Found Devices : {0} " & vbCr & vbLf, count)

        btnSearch.Enabled = True
    End Sub

    Private Sub FoundDevice(res As my_util.DeviceDiscoverResponse)
        txtResult.Text += "=== Device Found ===" & vbCr & vbLf
        txtResult.Text += (Convert.ToString("   ProductName : ") & res.ProductName) + vbCr & vbLf
        txtResult.Text += "     Device ID : " + Convert.ToString(res.dwId) + vbCr & vbLf
        txtResult.Text += (Convert.ToString("    IP Address : ") & my_util.UInt32_to_IPstr(res.dwIp)) + vbCr & vbLf
        txtResult.Text += "          Port : " + Convert.ToString(res.wPort) + vbCr & vbLf
        txtResult.Text += (Convert.ToString("    SubnetMask : ") & my_util.UInt32_to_IPstr(res.dwSubnetMask)) + vbCr & vbLf
        txtResult.Text += (Convert.ToString("DefaultGateway : ") & my_util.UInt32_to_IPstr(res.dwDefaultGateway)) + vbCr & vbLf
        txtResult.Text += "      Use DHCP : " + (If(res.bUseDHCP = 0, "FALSE", "TRUE")) + vbCr & vbLf

        Application.DoEvents()
    End Sub
End Class

Public Class my_util
    Public Const DEVICE_DISCOVER_PORT As UInt32 = 20567

    Private Const DEVDISCOVER_REQUEST_MAGIC1 As UInt32 = &HC58380D
    Private Const DEVDISCOVER_REQUEST_MAGIC2 As UInt32 = &HEA8B42B2UI
    Private Const DEVDISCOVER_RESPONSE_MAGIC1 As UInt32 = &HAA8FCB84UI
    Private Const DEVDISCOVER_RESPONSE_MAGIC2 As UInt32 = &H5FECE87

    Private Const ProductNameLen_Max As Byte = 16

    '''/////////////////////////////////////////////////////////////////////////////////////////
    Public Class DeviceDiscoverRequest
        Public magic1 As UInt32
        Public magic2 As UInt32
        Public ProductNamePrefix As String
        ' 16bytes
        'public UInt32 reserved[2];
        Public Shared Function getSize() As Integer
            Return 4 + 4 + 16 + 4 * 2
        End Function

        Public Sub New(prefix As String)
            magic1 = DEVDISCOVER_REQUEST_MAGIC1
            magic2 = DEVDISCOVER_REQUEST_MAGIC2
            ProductNamePrefix = prefix
        End Sub

        Public Function toBytes() As Byte()
            Using buf As MemoryStream = New MemoryStream()
                Using w As BinaryWriter = New BinaryWriter(buf)
                    w.Write(magic1)
                    w.Write(magic2)
                    w.Write(my_util.string_to_utf8_nts(ProductNamePrefix, ProductNameLen_Max))
                    w.Write(0)
                    w.Write(0)
                    Return buf.ToArray()
                End Using
            End Using
        End Function
    End Class

    '''/////////////////////////////////////////////////////////////////////////////////////////
    Public Class DeviceDiscoverResponse
        Public magic1 As UInt32
        Public magic2 As UInt32
        Public ProductName As String
        ' 16bytes
        Public dwId As UInt32
        Public dwIp As UInt32
        Public dwSubnetMask As UInt32
        Public dwDefaultGateway As UInt32

        Public wPort As UInt16
        Public bUseDHCP As UInt16

        'public UInt32 reserved[32];

        Public Shared Function getSize() As Integer
            Return 4 + 4 + 16 + 4 * 5 + 4 * 32
        End Function

        Public Sub New(data As Byte())
            Using buf As MemoryStream = New MemoryStream(data)
                Using r As BinaryReader = New BinaryReader(buf)
                    magic1 = r.ReadUInt32()
                    magic2 = r.ReadUInt32()
                    ProductName = my_util.utf8_nts_to_string(r.ReadBytes(ProductNameLen_Max))

                    dwId = r.ReadUInt32()
                    dwIp = r.ReadUInt32()
                    dwSubnetMask = r.ReadUInt32()
                    dwDefaultGateway = r.ReadUInt32()
                    wPort = r.ReadUInt16()
                    bUseDHCP = r.ReadUInt16()
                End Using
            End Using
        End Sub

        Public Function is_magic_valid() As Boolean
            Return (magic1 = DEVDISCOVER_RESPONSE_MAGIC1) AndAlso (magic2 = DEVDISCOVER_RESPONSE_MAGIC2)
        End Function
    End Class

    '''/////////////////////////////////////////////////////////////////////////////////////////
    Public Shared Function utf8_nts_to_string(chars As Byte()) As String
        Dim len As Integer
        For len = 0 To chars.Length - 1
            If chars(len) = 0 Then
                Exit For
            End If
        Next

        Return Encoding.UTF8.GetString(chars, 0, len)
    End Function
    Public Shared Function string_to_utf8_nts(str As String, len As Integer) As Byte()
        Dim utf8 As Byte() = If((str IsNot Nothing), Encoding.UTF8.GetBytes(str), New Byte(-1) {})

        Dim result As Byte() = New Byte(len - 1) {}
        Dim effective As Integer = Math.Min(utf8.Length, len)
        For i As Integer = 0 To effective - 1
            result(i) = utf8(i)
        Next
        For i As Integer = effective To len - 1
            result(i) = 0
        Next

        Return result
    End Function

    Public Shared Function UInt32_to_IPstr(ip As UInt32) As String
        Return String.Format("{0}.{1}.{2}.{3}", (ip >> 24) And &HFF, (ip >> 16) And &HFF, (ip >> 8) And &HFF, (ip >> 0) And &HFF)
    End Function
End Class
