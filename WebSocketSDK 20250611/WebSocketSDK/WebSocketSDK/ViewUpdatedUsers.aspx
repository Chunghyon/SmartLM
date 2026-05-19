<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewUpdatedUsers.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.ViewUserSyncPending" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Button Text="Refresh" runat="server" OnClick="Refresh_Click" />
        <asp:Button Text="Clear" runat="server" OnClick="Clear_Click" />
    </div>
    <asp:GridView ID="updated_users" runat="server" DataSourceID="updated_users_datasource" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="device_uid" HeaderText="DeviceUID" />
            <asp:BoundField DataField="action" HeaderText="Action"/>
            <asp:BoundField DataField="user_id" HeaderText="UserId"/>  
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="updated_users_datasource" runat="server"
        TypeName="SmackBio.WebSocketSDK.Sample.DeviceUpdatedUserQueue" SelectMethod="GetQueue"></asp:ObjectDataSource>
</asp:Content>
