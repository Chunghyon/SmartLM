<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewRegisterDevices.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.ViewRegisterDevices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Button Text="Refresh" runat="server" OnClick="Refresh_Click" />
    </div>
    <asp:GridView ID="register_devices" runat="server" DataSourceID="register_devices_datasource"
        AutoGenerateColumns="false" OnRowCommand="register_devices_RowCommand" >
        <Columns>
            <asp:BoundField DataField="device_name" HeaderText="Model " />
            <asp:BoundField DataField="device_uid" HeaderText="DeviceUID " />
            <asp:BoundField DataField="session_id" HeaderText="SessionID " />
            <asp:ButtonField Text="Register" CommandName="Register" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="register_devices_datasource" runat="server"
         TypeName="SmackBio.WebSocketSDK.Sample.DeviceLoginManager" SelectMethod="GetRegisterDevices">
    </asp:ObjectDataSource>
</asp:Content>
