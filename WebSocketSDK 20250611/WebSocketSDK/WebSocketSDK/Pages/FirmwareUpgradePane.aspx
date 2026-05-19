<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FirmwareUpgradePane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.FirmwareUpgradePane"
    Async="true" AsyncTimeout="600" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:TextBox ID="session_id" runat="server" visible="false"/>
    <div>
        <asp:Label runat="server" ID="error_message" ForeColor="Red" />
    </div>

    <table>
    <tr><td colspan="3">
            <asp:Label ID="lblMessage" runat="server" width="100%" Font-Bold="True" Font-Size="Medium"></asp:Label>
    </td></tr>

    <tr>
        <td colspan="2">
            Get Firmware Version: 
            <asp:Button ID="btnGetFirmwareVersion" runat="server" Text="GetFirmwareVersion" OnClick="btnGetFirmwareVersion_Click" Width="190px" />
        </td>
    </tr>

    <tr>
        <td></td>
    </tr>



    <tr>
        <td colspan="2">
            Firmware Upgrade from Http(s). (ex) http://yqall02.baidupcs.com/file/58a7fdfffdf9... </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnFirmwareUpgradeHttp" runat="server" Text="Upgrade" OnClick="btnFirmwareUpgradeHttp_Click" Width="120px" />
            <asp:TextBox ID="txtUrl" runat="server" Width="400px"/>
        </td>
    </tr>
    </table>

</asp:Content>