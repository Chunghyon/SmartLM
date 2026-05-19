<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NTPServerPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.NTPServerPane"
    Async="true" %>

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
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtMessage" runat="server" width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnGet" runat="server" Text="GetNTPServer" OnClick="btnGet_Click" Width="120px" />
            </td>
             <td>
                <asp:Button ID="btnSet" runat="server" Text="SetNTPServer" OnClick="btnSet_Click" Width="120px" />
            </td>
       </tr>
        <tr>
            <td>
                Server Name:
            </td>
            <td>
                <asp:TextBox ID="txtNTPServer" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Timezone:
            </td>
            <td>
                UTC <asp:TextBox ID="txtTimezoneHour" runat="server" Width="32px"></asp:TextBox>
                : <asp:TextBox ID="txtTimezoneMinute" runat="server" Width="32px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Auto Sync Intervel:
            </td>
            <td>
                <asp:TextBox ID="txtAutoSyncInterval" runat="server" Width="150px"></asp:TextBox> minutes
            </td>
        </tr>
    </table>

</asp:Content>