<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PageList.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.PageList" 
    Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <table>
        <tr>
            <td>Session ID :</td>
            <td>
                <asp:TextBox ID="session_id" runat="server" />
                <asp:TextBox ID="device_uid" runat="server" Visible="false"/>
            </td>
        </tr>
    </table>

    <asp:Panel ID="Panel1" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnUserManage" runat="server" Text="User Manage" OnClick="btnUserManage_Click" Width="200px" /> <br />
                    <asp:Button ID="btnDepartment" runat="server" Text="Department" OnClick="btnDepartment_Click" Width="200px" /> <br />
                    <asp:Button ID="btnAutoAttendance" runat="server" Text="AutoAttendance" OnClick="btnAutoAttendance_Click" Width="200px" /> <br />
                    <asp:Button ID="btnAccessTimeZone" runat="server" Text="AccessTimeZone" OnClick="btnAccessTimeZone_Click" Width="200px" /> <br />
                    <asp:Button ID="btnBellTime" runat="server" Text="BellTime" OnClick="btnBellTime_Click" Width="200px" /> <br />
                    <asp:Button ID="btnDeviceInfo" runat="server" Text="DeviceInfo" OnClick="btnDeviceInfo_Click" Width="200px" /> <br />
                    <asp:Button ID="btnNetworkSetting" runat="server" Text="NetworkSetting" OnClick="btnNetworkSetting_Click" Width="200px" /> <br />
                    <asp:Button ID="btnWiFiSetting" runat="server" Text="WiFiSetting" OnClick="btnWiFiSetting_Click" Width="200px" /> <br />
                    <asp:Button ID="btnDataEmpty" runat="server" Text="Empty Log, Enroll Data" OnClick="btnDataEmpty_Click" Width="200px" /> <br />
                    <asp:Button ID="btnTimeLogRead" runat="server" Text="TimeLog" OnClick="btnTimeLogRead_Click" Width="200px" /> <br />
                </td>
                <td>
					<br />
                    <asp:Button ID="btnNTPServer" runat="server" Text="NTPServer" OnClick="btnNTPServer_Click" Width="200px" />  <br />
                    <asp:Button ID="btnServerURL" runat="server" Text="Server Settings" OnClick="btnServerURL_Click" Width="200px" />  <br />
					<br />
                    <br />
                    <asp:Button ID="btnFirmwareUpgrade" runat="server" Text="FirmwareUpgrade" OnClick="btnFirmwareUpgrade_Click" Width="200px" />  <br />
                </td>
            </tr>
        </table>
        
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <asp:Button ID="btnViewOnlineDevices" runat="server" Text="Back to ViewOnlineDevices" OnClick="btnViewOnlineDevices_Click" />
    </asp:Panel>

</asp:Content>