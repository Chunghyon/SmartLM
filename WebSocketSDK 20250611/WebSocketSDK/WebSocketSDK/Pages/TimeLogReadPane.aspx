<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeLogReadPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.TimeLogReadPane" 
    Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
        </tr>
        <tr>
            <td colspan ="2"><asp:Label runat="server" ID="TextMessage" ForeColor="Red" Font-Bold="True" Font-Size="Larger" /></td>
        </tr>
        <tr>
            <td>
				<table>
                    <tr>
                        <td style="margin: 0px; padding: 0px">
                            <asp:Button ID="btnReadTimeLog" runat="server" Text="ReadTimeLog" Width="150px" OnClick="btnReadTimeLog_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
					        <table>
					            <tr><td>UserID:</td>
                                    <td><asp:TextBox ID="TextUserID" runat="server" />
                                    </td></tr>
					            <tr><td>Start Time:</td>
                                    <td><asp:TextBox ID="TextStartTime_y" runat="server" Width="40px" Text="2016" />-
                                        <asp:TextBox ID="TextStartTime_m" runat="server" Width="20px" Text="03" />-
                                        <asp:TextBox ID="TextStartTime_d" runat="server" Width="20px" Text="10" /> &nbsp;&nbsp;
                                        <asp:TextBox ID="TextStartTime_hh" runat="server" Width="20px" Text="00" />:
                                        <asp:TextBox ID="TextStartTime_mm" runat="server" Width="20px" Text="00" />
                                    </td></tr>
					            <tr><td>End Time:</td>
                                    <td><asp:TextBox ID="TextEndTime_y" runat="server" Width="40px" Text="" />-
                                        <asp:TextBox ID="TextEndTime_m" runat="server" Width="20px" Text="" />-
                                        <asp:TextBox ID="TextEndTime_d" runat="server" Width="20px" Text="" /> &nbsp;&nbsp;
                                        <asp:TextBox ID="TextEndTime_hh" runat="server" Width="20px" Text="" />:
                                        <asp:TextBox ID="TextEndTime_mm" runat="server" Width="20px" Text="" />
                                    </td></tr>
                            </table>
                        </td>
                    </tr>
				</table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnGetTimeLogPosInfo" runat="server" Text="GetTimeLogPosInfo" Width="150px" OnClick="btnGetTimeLogPosInfo_Click" />
                        </td>
                        <td>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDeleteTimelogWithPos" runat="server" Text="DeleteTimelog" Width="150px" OnClick="btnDeleteTimelogWithPos_Click" />
                        </td>
                        <td>
                            EndPos:
                            <asp:TextBox ID="txtEndPos" runat="server" Width="109px"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>


    <asp:Label runat="server" ID="txtSearchCondition" Font-Bold="True"/>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="no" HeaderText="No"/>
            <asp:BoundField DataField="comprehensive_string" HeaderText="Content"/>
            <asp:BoundField DataField="content_string" HeaderText="Content(xml)" visible="false"/>  
        </Columns>
    </asp:GridView>



    
    <asp:ScriptManager ID="ScriptManager2" runat="server">
        </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="mvvProcess" runat="server" ActiveViewIndex="0">
                <asp:View ID="vLaunch" runat="server">
                <asp:Button ID="btnReadTimeLog_timer" runat="server"  Text="ReadTimeLog(Timer)" onclick="btnReadTimeLogTimer_Click"/>
                </asp:View>
                <asp:View ID="vProgress" runat="server">
                <asp:Button ID="btnCancel" runat="server"  Text="Cancel" onclick="btnCancel_Click"/>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/progress.gif" width="50px" Height="50px"/>
                <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="100" ontick="Timer1_Tick"> </asp:Timer>
                </asp:View>
            </asp:MultiView>
            <asp:Label id="msg" runat="server" Text =""></asp:Label>

            <br /><asp:Label runat="server" ID="txtSearchCondition2" Font-Bold="True"/>
            <br /><asp:Label runat="server" ID="comprehensive_string2"/>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>