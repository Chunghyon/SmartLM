<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VideoStreamingPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.VideoStremingSettingPane" 
    Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 49px;
        }
        .auto-style2 {
            height: 58px;
        }
        .auto-style3 {
            width: 446px;
        }
        .auto-style4 {
            height: 49px;
            width: 446px;
        }
    </style>
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
                        <td class="auto-style3"><asp:TextBox ID="session_id" runat="server" visible="false"/></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtMessage" runat="server" width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="auto-style1">1</td>
                        <td class="auto-style4">
                            <asp:Button ID="btnGetRtspSetting" runat="server" Text="GetRtspSetting" OnClick="btnGetRtspSetting_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">2</td>
                        <td class="auto-style3">
                            <asp:Button ID="btnSetEthernetSetting" runat="server" Text="SetRtspSetting" OnClick="btnSetRtspSetting_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="auto-style3">
                            <table>
                                <tr>
                                    <td class="auto-style2">
                                        Rtsp Enable: </td>
                                    <td class="auto-style2">
                                        <asp:CheckBox ID="chkRtspEnable" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Rtsp Resolution:
                                    </td>
                                    <td>
                    <asp:DropDownList ID="cmbRtspResolution" runat="server" Width="150px" Height="25px"
                        DataSourceID="rtsp_resolution_items">
                    </asp:DropDownList>
                    <asp:ObjectDataSource runat="server" ID="rtsp_resolution_items" 
                        TypeName="SmackBio.WebSocketSDK.Sample.Pages.VideoStremingSettingPane" 
                        SelectMethod="GetRtspResolutionList" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Rtsp bitrate (Mbps):</td>
                                    <td>
                    <asp:DropDownList ID="cmbRtspBitrate" runat="server" Width="150px" Height="25px"
                        DataSourceID="rtsp_bitrate_items">
                    </asp:DropDownList>
                    <asp:ObjectDataSource runat="server" ID="rtsp_bitrate_items" 
                        TypeName="SmackBio.WebSocketSDK.Sample.Pages.VideoStremingSettingPane" 
                        SelectMethod="GetRtspBitrateList" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top" class="auto-style1">1</td>
                        <td class="auto-style4">
                            <asp:Button ID="btnGetCenterScreenMsg" runat="server" Text="GetCenterScreenMsg" OnClick="btnGetCenterScreenMsg_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">2</td>
                        <td class="auto-style3">
                            <asp:Button ID="btnSetCenterScreenMsg" runat="server" Text="SetCenterScreenMsg" OnClick="btnSetCenterScreenMsg_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="auto-style3">
                            <table>
                                <tr>
                                    <td class="auto-style2">
                                        Verify Disable: </td>
                                    <td class="auto-style2">
                                        <asp:CheckBox ID="chkVerifyDisable" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Center Screen Message:
                                    </td>
                                    <td>
                            <asp:TextBox ID="txtCenterScreenMsg" runat="server" width="300%" Height="200px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Text Color (RGB, HEX) :</td>
                                    <td>
                            <asp:TextBox ID="txtTextColor" runat="server" width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Text Border Color (RGB, HEX) :</td>
                                    <td>
                            <asp:TextBox ID="txtTextBorderColor" runat="server" width="100%"></asp:TextBox>
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
