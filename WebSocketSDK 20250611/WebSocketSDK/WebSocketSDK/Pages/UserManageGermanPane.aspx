<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManageGermanPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.UserManageGermanPane" 
    Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 49px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td colspan ="2">
                Session ID:  
                <asp:Label ID="session_id" runat="server"></asp:Label> <br />

                Device UID:
                <asp:Label ID="device_uid" runat="server"></asp:Label>
            </td>
            <td colspan="2"><asp:Label runat="server" ID="TextMessage" ForeColor="Red" Font-Bold="True" Font-Size="Larger" /></td>
        </tr>
        <tr>
             <td>UserID:</td><td><asp:TextBox ID="TextUserID" runat="server" /></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <table>
                    <tr>
                        <td>Message:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUserMessage" runat="server" TextMode="MultiLine" Width="350px" Height="80px" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnGetUserMessage" runat="server" Text="Get" Width="100px" OnClick="btnGetUserMessage_Click" /></td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnSetUserMessage" runat="server" Text="Set" Width="100px" OnClick="btnSetUserMessage_Click" /></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <table>
                    <tr>
                        <td>Balance Time:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUserBalanceHour" runat="server" Width="50px" Text="00"/> : <asp:TextBox ID="txtUserBalanceMinute"  Width="50px" runat="server" Text="00"/> 
                        </td>
                    </tr>
                </table>
            </td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnGetUserBalanceTime" runat="server" Text="Get" Width="100px" OnClick="btnGetUserBalanceTime_Click" /></td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnSetUserBalanceTime" runat="server" Text="Set" Width="100px" OnClick="btnSetUserBalanceTime_Click" /></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <table>
                    <tr>
                        <td>Holidays:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUserHolidays" runat="server" Text="0.0"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnGetUserHolidays" runat="server" Text="Get" Width="100px" OnClick="btnGetUserHolidays_Click" /></td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnSetUserHolidays" runat="server" Text="Set" Width="100px" OnClick="btnSetUserHolidays_Click" /></td>
        </tr>
    </table>
</asp:Content>