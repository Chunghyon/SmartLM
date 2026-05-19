<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManageCustomPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.UserManageCustomPane" 
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
                        <td>AttendOnly:</td>
                        <td>
                            <asp:CheckBox ID="ChkAttendOnly" runat="server" Checked="true" /></td>
                    </tr>
                </table>
            </td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnGetUserAttendOnly" runat="server" Text="Get" Width="100px" OnClick="btnGetUserAttendOnly_Click" /></td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnSetUserAttendOnly" runat="server" Text="Set" Width="100px" OnClick="btnSetUserAttendOnly_Click" /></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <table>
                    <tr>
                        <td>User Message: (for M91)</td>
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
            <td colspan="2">
                <table>
                    <tr>
                        <td>Message Color (RGB, HEX) :</td>
                        <td>
                            <asp:TextBox ID="txtMessageColor" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Message Back Color (RGB, HEX) :</td>
                        <td>
                            <asp:TextBox ID="txtMessageBkColor" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnGetMessageColor" runat="server" Text="Get" Width="100px" OnClick="btnGetMessageColor_Click" /></td>
            <td style="margin: 0px; padding: 0px">
                <asp:Button ID="btnSetMessageColor" runat="server" Text="Set" Width="100px" OnClick="btnSetMessageColor_Click" /></td>
        </tr>
    </table>
</asp:Content>