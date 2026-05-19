<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WiFiSettingPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.WiFiSettingPane" 
    Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label runat="server" ID="error_message" ForeColor="Red" />
    </div>
    <table><tr>
        <td>
            <table>
                <tbody>
                    <tr>
                        <td></td>
                        <td><asp:TextBox ID="session_id" runat="server" visible="false"/></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtMessage" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">1</td>
                        <td>
                            <asp:Button ID="btnGetWiFiSetting" runat="server" Text="GetWiFiSetting" OnClick="btnGetWiFiSetting_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">2</td>
                        <td>
                            <asp:Button ID="btnSetWiFiSetting" runat="server" Text="SetWiFi" OnClick="btnSetWiFiSetting_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        Use Wifi:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkUseWifi" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SSID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSSID" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Key:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKey" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Use DHCP:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkUseDHCP" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        IP:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIP" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Subnet Mask:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubnetMask" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Gateway:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGateway" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Port:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPort" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        (Received from DHCP) <br /> (readonly)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        IP:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIP_from_dhcp" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Subnet Mask:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubnetMask_from_dhcp" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Gateway:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGateway_from_dhcp" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr></table>
</asp:Content>
