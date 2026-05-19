<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewOnlineDevices.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.ViewOnlineDevices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Button Text="Refresh" runat="server" OnClick="Refresh_Click" />
    </div>
    <asp:GridView ID="online_devices" runat="server" DataSourceID="online_devices_datasource"
        AutoGenerateColumns="false" OnRowCommand="online_devices_RowCommand" >
        <Columns>
            <asp:BoundField DataField="device_uid" HeaderText="DeviceSN " />
            <asp:BoundField DataField="session_id" HeaderText="SessionID " />
            <asp:ButtonField Text="Open" CommandName="Open" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="online_devices_datasource" runat="server"
         TypeName="SmackBio.WebSocketSDK.Sample.DeviceLoginManager" SelectMethod="GetOnlineDevices">
    </asp:ObjectDataSource>
    <asp:Timer ID="Timer1" runat="server" Interval="3000" OnTick="Refresh_Click">
    </asp:Timer>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
</asp:Content>
