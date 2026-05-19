<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccessTimeZonePane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.AccessTimeZonePane"
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
    <tr><td colspan="2">
            <asp:TextBox ID="txtMessage" runat="server" width="100%"></asp:TextBox>
    </td></tr>
    <tr>
        <td>
            <table>
             <tr><td colspan="3">
                 Access Time Zone
                 </td></tr>
             <tr>
                <td>1</td>
                <td>
                    <asp:Button ID="btnGetAccessTimeZone" runat="server" Text="GetAccessTimeZone" OnClick="btnGetAccessTimeZone_Click"/>
                </td>
                <td rowspan="2">
                    <asp:DropDownList ID="cmbTimezoneNo" runat="server" Width="150px" Height="25px"
                        DataSourceID="timezone_items">
                    </asp:DropDownList>
                    <asp:ObjectDataSource runat="server" ID="timezone_items" 
                        TypeName="SmackBio.WebSocketSDK.Sample.Pages.AccessTimeZonePane" 
                        SelectMethod="GetTimezoneList" />
                </td>
            </tr>
            <tr>
                <td>2</td>
                <td>
                    <asp:Button ID="btnSetAccessTimeZone" runat="server" Text="SetAccessTimeZone" OnClick="btnSetAccessTimeZone_Click"/>
                </td>
            </tr>
            <tr>
              <td></td>
              <td colspan="2" style="border: thin solid #999999">
                <asp:GridView ID="gridview_timesections" runat="server" DataSourceID="timesections_datasource"
                    AutoGenerateColumns="false" OnRowCommand="timezone_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="StartHour" HeaderText="Start(HH:mm)" ShowHeader="true" />
                        <asp:BoundField DataField="StartMinute" HeaderText="" />
                        <asp:BoundField DataField="EndHour" HeaderText="End(HH:mm)" />
                        <asp:BoundField DataField="EndMinute" HeaderText="" />

                        <asp:ButtonField Text="Edit" CommandName="Edit" />
                        <asp:ButtonField Text="Update" CommandName="Update" />
                        <asp:ButtonField Text="Cancel" CommandName="Cancel" />

                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="timesections_datasource" runat="server"
                     TypeName="SmackBio.WebSocketSDK.Sample.Pages.AccessTimeZonePane" SelectMethod="GetTimeSections" UpdateMethod="UpdateTimesection">
                </asp:ObjectDataSource>
               </td>
            </tr>
            </table>
        </td>
	  </tr>
    </table>
</asp:Content>