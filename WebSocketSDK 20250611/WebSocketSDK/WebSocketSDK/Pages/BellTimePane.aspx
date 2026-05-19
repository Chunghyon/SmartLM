<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BellTimePane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.BellTimePane"
    Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
                 Bell Timing
                 </td></tr>
             <tr>
                <td>1</td>
                <td>
                    <asp:Button ID="btnGetBellTime" runat="server" Text="GetBellTime" OnClick="btnGetBellTime_Click"/>
                </td>
                <td rowspan="2">
                </td>
            </tr>
            <tr>
                <td>2</td>
                <td>
                    <asp:Button ID="btnSetBellTime" runat="server" Text="SetBellTime" OnClick="btnSetBellTime_Click"/>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    Ring Times : 
                    <asp:TextBox ID="txtBellRingTimes" runat="server"/>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    BellCount : 
                    <asp:TextBox ID="txtBellCount" runat="server"/>
                </td>
            </tr>
            <tr>
              <td></td>
              <td colspan="2" style="border: thin solid #999999">
                <asp:GridView ID="gridview_bells" runat="server" DataSourceID="bells_datasource"
                    AutoGenerateColumns="false" OnRowCommand="bell_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="valid" HeaderText="valid" ShowHeader="true" />
                        <asp:BoundField DataField="type" HeaderText="type" />
                        <asp:BoundField DataField="hour" HeaderText="hour" />
                        <asp:BoundField DataField="minute" HeaderText="minute" />

                        <asp:ButtonField Text="Edit" CommandName="Edit" />
                        <asp:ButtonField Text="Update" CommandName="Update" />
                        <asp:ButtonField Text="Cancel" CommandName="Cancel" />

                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="bells_datasource" runat="server"
                     TypeName="SmackBio.WebSocketSDK.Sample.Pages.BellTimePane" SelectMethod="GetBells" UpdateMethod="UpdateBell">
                </asp:ObjectDataSource>
               </td>
            </tr>
            </table>
        </td>
	  </tr>
    </table>
</asp:Content>