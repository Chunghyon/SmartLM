<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AutoAttendancePane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.AutoAttendancePane"
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
                 Auto Attendance Timing
                 </td></tr>
             <tr>
                <td>1</td>
                <td>
                    <asp:Button ID="btnGetAutoAttendance" runat="server" Text="GetAutoAttendance" OnClick="btnGetAutoAttendance_Click"/>
                </td>
                <td rowspan="2">
                </td>
            </tr>
            <tr>
                <td>2</td>
                <td>
                    <asp:Button ID="btnSetAutoAttendance" runat="server" Text="SetAutoAttendance" OnClick="btnSetAutoAttendance_Click"/>
                </td>
            </tr>
            <tr>
              <td></td>
              <td colspan="2" style="border: thin solid #999999">
                <asp:GridView ID="gridview_attendances" runat="server" DataSourceID="attendances_datasource"
                    AutoGenerateColumns="false" OnRowCommand="attendance_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="StartHour" HeaderText="Start" ShowHeader="true" />
                        <asp:BoundField DataField="StartMinute" HeaderText="(HH:mm)" />
                        <asp:BoundField DataField="EndHour" HeaderText="End" />
                        <asp:BoundField DataField="EndMinute" HeaderText="(HH:mm)" />
                        <asp:BoundField DataField="Status" HeaderText="(DutyOn, DutyOff, OvertimeOn, OvertimeOff, In, Out)" HeaderStyle-Width="100px" />

                        <asp:ButtonField Text="Edit" CommandName="Edit" />
                        <asp:ButtonField Text="Update" CommandName="Update" />
                        <asp:ButtonField Text="Cancel" CommandName="Cancel" />

                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="attendances_datasource" runat="server"
                     TypeName="SmackBio.WebSocketSDK.Sample.Pages.AutoAttendancePane" SelectMethod="GetAttendances" UpdateMethod="UpdateAttendance">
                </asp:ObjectDataSource>
               </td>
            </tr>
            </table>
        </td>
	  </tr>
    </table>
</asp:Content>