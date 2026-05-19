<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestCommand.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.TestCommand" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label runat="server" ID="error_message" ForeColor="Red" />
    </div>
    <table><tr>
        <td>
            <table>
                <tbody>
                    <tr>
                        <td>Session ID :</td>
                        <td><asp:TextBox ID="session_id" runat="server" /></td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">Request XML :</td>
                        <td><asp:TextBox ID="request_xml" runat="server" TextMode="MultiLine" Height="150" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:right">
                            <asp:Button runat="server" Text="Execute" OnClick="Execute_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">Response XML :</td>
                        <td><asp:TextBox ID="response_xml" runat="server" TextMode="MultiLine" Height="150" ReadOnly="true" /></td>
                    </tr>
                </tbody>
            </table>
        </td>
        <td>
            <asp:Button ID="btnGetTime" runat="server" Text="GetTime" OnClick="btnGetTime_Click" />
        </td>
    </tr></table>
</asp:Content>
