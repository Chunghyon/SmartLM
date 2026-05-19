<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeviceInfoPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.DeviceInfoPane"
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
      <td>
        <table>
            <tr><td>1</td>
                <td>
                    <asp:Button ID="btnGetTime" runat="server" Text="GetTime" OnClick="btnGetTime_Click" Width="120px" />
                </td>
            </tr>

            <tr><td>2</td>
                <td>
                    <asp:Button ID="btnSetTime" runat="server" Text="SetTime" OnClick="btnSetTime_Click" Width="120px" />
                </td>
            </tr>

            <tr><td>3</td>
                <td>
                    <asp:Button ID="btnEnableDevice" runat="server" Text="EnableDevice" OnClick="btnEnableDevice_Click" Width="120px" />
                    <asp:Button ID="btnDisableDevice" runat="server" Text="(DisableDevice)" OnClick="btnDisableDevice_Click" Width="120px" />
                </td>
            </tr>


             <tr>
                 <td colspan="2">Device Information </td>
             </tr>

             <tr><td>4</td>
                 <td>
                    <table><tr>
                        <td>
                            <asp:Button ID="btnDeviceInfoGet" runat="server" Text="GetDeviceInfo" OnClick="btnDeviceInfoGet_Click" Width="120px" />
                        </td>
                        <td>
                            <asp:DropDownList ID="comboDeviceInfo" runat="server" Width="150px" Height="25px"
                                DataSourceID="device_info_items">
                            </asp:DropDownList>
                            <asp:ObjectDataSource runat="server" ID="device_info_items" 
                                TypeName="SmackBio.WebSocketSDK.Sample.Pages.DeviceInfoPane" 
                                SelectMethod="GetDeviceInfoItemList" />
                       </td>
                    </tr></table>
             </tr>

             <tr><td>5</td>
                 <td>
                    <table><tr>
                        <td>
                            <asp:Button ID="btnDeviceInfoSet" runat="server" Text="SetDeviceInfo" OnClick="btnDeviceInfoSet_Click" Width="120px" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtSetDeviceInfoParam" runat="server" Width="150px"></asp:TextBox>
                        </td>
                    </tr></table>
             </tr>

             <tr>
                 <td colspan="2">Device Status</td>
             </tr>

             <tr><td>6</td>
                 <td>
                    <table><tr>
                        <td>
                            <asp:Button ID="btnDeviceStatusGet" runat="server" Text="GetDeviceStatus" OnClick="btnDeviceStatusGet_Click" Width="120px" />
                        </td>
                        <td>
                            <asp:DropDownList ID="comboDeviceStatus" runat="server" Width="150px" Height="25px"
                                 DataSourceID="device_status_items">
                            </asp:DropDownList>
                            <asp:ObjectDataSource runat="server" ID="device_status_items" 
                                TypeName="SmackBio.WebSocketSDK.Sample.Pages.DeviceInfoPane" 
                                SelectMethod="GetDeviceStatusItemList" />
                        </td>
                    </tr></table>
             </tr>
        </table>
      </td>
      <td></td>
      <td>
           <table>
               <tr><td colspan="2">Lock Control
                   </td></tr>
               <tr><td>7</td>
                   <td>
                       <asp:Button ID="btnLockControlStatus" runat="server" Text="LockControlStatus" OnClick="btnLockControlStatus_Click" Width="150px" />
                   </td>
               </tr>
               <tr><td>8</td>
                   <td>
                       <asp:Button ID="btnLockControl" runat="server" Text="LockControl" OnClick="btnLockControl_Click" Width="150px" />

                       <asp:DropDownList ID="comboLockControl" runat="server" Width="150px" Height="25px"
                                 DataSourceID="lock_control_items">
                            </asp:DropDownList>
                        <asp:ObjectDataSource runat="server" ID="lock_control_items" 
                            TypeName="SmackBio.WebSocketSDK.Sample.Pages.DeviceInfoPane" 
                            SelectMethod="GetLockControlItemList" />
                   </td>
               </tr>
               <tr>
                   <td>
                       &nbsp;
                   </td>
               </tr>
               <tr>
                   <td colspan="2">
                       <asp:Button ID="btnGetDeviceInfoAll" runat="server" Text="GetDeviceInfoAll" OnClick="btnDeviceInfoGetAll_Click" Width="150px" />
                       <asp:Button ID="btnGetDeviceStatusAll" runat="server" Text="GetDeviceStatusAll" OnClick="btnDeviceStatusGetAll_Click" Width="150px" />
                   </td>
               </tr>
           </table>
     </tr>
    </table>

    <span id="spanResult" runat="server"/>

</asp:Content>