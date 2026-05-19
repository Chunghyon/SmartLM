
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataEmptyPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.LogPane"
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
    <tr><td colspan="3">
            <asp:TextBox ID="txtMessage" runat="server" width="100%"></asp:TextBox>
    </td></tr>
    
    <tr>
        <td>1</td>
        <td>
          <asp:Button ID="btnEmptyTimeLog" runat="server" Text="EmptyTimeLog" OnClick="btnEmptyTimeLog_Click" Width="200px" />
        </td>
    </tr>
    <tr>
        <td>2</td>
        <td>
          <asp:Button ID="btnEmptyManageLog" runat="server" Text="EmptyManageLog" OnClick="btnEmptyManageLog_Click" Width="200px" />
        </td>
    </tr>
    <tr>
        <td>3</td>
        <td>
          <asp:Button ID="btnEmptyAllData" runat="server" Text="EmptyAllData" OnClick="btnEmptyAllData_Click" Width="200px" />
        </td>
    </tr>
    <tr>
        <td>4</td>
        <td>
          <asp:Button ID="btnEmptyUserEnrollmentData" runat="server" Text="EmptyUserEnrollmentData" OnClick="btnEmptyUserEnrollmentData_Click" Width="200px" />
        </td>
    </tr>
    </table>
</asp:Content>