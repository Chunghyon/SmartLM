<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDeviceEvents.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.ViewDeviceEvents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Button Text="Refresh" runat="server" OnClick="Refresh_Click" />
        <asp:Button Text="Clear" runat="server" OnClick="Clear_Click" />
    </div>
    <asp:GridView ID="device_events" runat="server" DataSourceID="device_events_datasource" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="device_uid" HeaderText="DeviceUID" />
            <asp:BoundField DataField="comprehensive_string" HeaderText="Content"/>
            <asp:BoundField DataField="content_string" HeaderText="Content(xml)" visible="false"/>  
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="device_events_datasource" runat="server"
        TypeName="SmackBio.WebSocketSDK.Sample.DeviceEventQueue" SelectMethod="GetQueuedEvents"></asp:ObjectDataSource>
    <asp:timer ID="Timer1" runat="server" Interval="3000" OnTick="Timer_Watch" Enabled="true"></asp:timer>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
</asp:Content>
