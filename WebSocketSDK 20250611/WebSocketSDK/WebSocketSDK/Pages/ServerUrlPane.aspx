<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServerUrlPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.ServerUrlPane"
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
    <asp:TextBox ID="txtMessage" runat="server" width="550px"></asp:TextBox>

    <table>
        <tr>
          <td>
            <table><tr>
                <td>
                    <asp:Button ID="btnWebServerUrlGet" runat="server" Text="Get WebServerUrl" OnClick="btnWebServerUrlGet_Click" Width="180px" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnWebServerUrlSet" runat="server" Text="Set WebServerUrl" OnClick="btnWebServerUrlSet_Click" Width="180px" />
                </td>
                <td>
                    <asp:TextBox ID="txtWebServerUrl" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            </table>
          </td>
        </tr>
    </table>
</asp:Content>
