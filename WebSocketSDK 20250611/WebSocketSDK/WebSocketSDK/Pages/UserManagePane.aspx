<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagePane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.UserManagePane" 
    Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 49px;
        }
    </style>
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
            <td><asp:Label runat="server" ID="TextMessage" ForeColor="Red" Font-Bold="True" Font-Size="Larger" /></td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr><td style="margin: 0px; padding: 0px"><asp:ObjectDataSource runat="server" ID="user_privilege_items" TypeName="SmackBio.WebSocketSDK.Sample.Pages.UserManagePane" SelectMethod="GetPrivilegeList" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetUserData" runat="server" Text="GetUserData" Width="150px" OnClick="btnGetUserData_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnSetUserData" runat="server" Text="SetUserData" Width="150px" OnClick="btnSetUserData_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnDeleteUser" runat="server" Text="DeleteUser" Width="150px" OnClick="btnDelete_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetUserPassword" runat="server" Text="GetUserPassword" Width="150px" OnClick="btnGetUserPassword_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetUserCardNo" runat="server" Text="GetUserCardNo" Width="150px" OnClick="btnGetUserCardNo_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetUserQR" runat="server" Text="GetUserQR" Width="150px" OnClick="btnGetUserQR_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetFingerData" runat="server" Text="GetFingerData" Width="150px" OnClick="btnGetFingerData_Click" /></td></tr>
                    <tr><td class="auto-style1" style="margin: 0px; padding: 0px"><asp:Button ID="btnSetFingerData" runat="server" Text="SetFingerData" Width="150px" OnClick="btnSetFingerData_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetFaceData" runat="server" Text="GetFaceData" Width="150px" OnClick="btnGetFaceData_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnSetFaceData" runat="server" Text="SetFaceData" Width="150px" OnClick="btnSetFaceData_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetUserPhoto" runat="server" Text="GetUserPhoto" Width="150px" OnClick="btnGetUserPhoto_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnSetUserPhoto" runat="server" Text="SetUserPhoto" Width="150px" OnClick="btnSetUserPhoto_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnEnrollFaceByPhoto" runat="server" Text="EnrollFaceByPhoto" Width="150px" OnClick="btnEnrollFaceByPhoto_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnTakeOffManager" runat="server" Text="TakeOffManager" Width="150px" OnClick="btnTakeOffManager_Click" /></td></tr>
                    <tr><td></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetFirstUserData" runat="server" Text="GetFirstUserData" Width="150px" OnClick="btnGetFirstUserData_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetNextUserData" runat="server" Text="GetNextUserData" Width="150px" OnClick="btnGetNextUserData_Click" /></td></tr>
                    <tr><td></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetAllUserData" runat="server" Text="GetAllUserData" Width="150px" OnClick="btnGetAllUserData_Click" /></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnSetAllUserData" runat="server" Text="SetAllUserData" Width="150px" OnClick="btnSetAllUserData_Click" /></td></tr>

                    <tr><td></td></tr>
                    <tr><td style="margin: 0px; padding: 0px"><asp:Button ID="btnGetAllUpdatedUserData" runat="server" Text="GetAllUpdatedUserData" Width="150px" OnClick="btnGetAllUpdatedUserData_Click" /></td></tr>
                </table>
            </td>
            <td>
                <table class="entity_table" style="padding: 0px; margin: 0px">
                    <tr class=""><td>UserID:</td><td><asp:TextBox ID="TextUserID" runat="server" /></td></tr>
                    <tr><td>Name:</td><td><asp:TextBox ID="TextName" runat="server" /></td></tr>
                    <tr><td>Depart:</td><td><asp:TextBox ID="TextDepart" runat="server" text="0"/></td></tr>
                    <tr><td>Privilege:</td><td>
                        <asp:DropDownList ID="comboUserPrivileges" runat="server" DataSourceID="user_privilege_items"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr><td>Enabled:</td><td><asp:CheckBox ID="ChkEnabled" runat="server" checked="true"/></td></tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>TimeSet:</td>
                                    <td>1</td>
                                    <td>2</td>
                                    <td>3</td>
                                    <td>4</td>
                                    <td>5</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td><asp:TextBox ID="TextTimeSet1" runat="server" Width="24px" text="0" /></td>
                                    <td><asp:TextBox ID="TextTimeSet2" runat="server" Width="24px" text="0" /></td>
                                    <td><asp:TextBox ID="TextTimeSet3" runat="server" Width="24px" text="0" /></td>
                                    <td><asp:TextBox ID="TextTimeSet4" runat="server" Width="24px" text="0" /></td>
                                    <td><asp:TextBox ID="TextTimeSet5" runat="server" Width="24px" text="0" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>Use Period:</td>
                        <td>
                            <asp:CheckBox ID="ChkUserPeriodUse" runat="server" checked="false"/>
                        </td></tr>
                    <tr>
                        <td></td>
                        <td>
                            <table>
                                <tr><td>Start Time:</td>
                                    <td><asp:TextBox ID="TextUserPeriodStart_y" runat="server" Width="40px" Text="" />-
                                        <asp:TextBox ID="TextUserPeriodStart_m" runat="server" Width="20px" Text="" />-
                                        <asp:TextBox ID="TextUserPeriodStart_d" runat="server" Width="20px" Text="" />
                                    </td></tr>
					            <tr><td>End Time:</td>
                                    <td><asp:TextBox ID="TextUserPeriodEnd_y" runat="server" Width="40px" Text="" />-
                                        <asp:TextBox ID="TextUserPeriodEnd_m" runat="server" Width="20px" Text="" />-
                                        <asp:TextBox ID="TextUserPeriodEnd_d" runat="server" Width="20px" Text="" />
                                    </td></tr>
                            </table>
                        </td>
                    </tr>
                                
                                
       
                    <tr><td>Card:</td><td><asp:TextBox ID="TextCard" runat="server" text="0" /></td></tr>
                    <tr><td>PWD:</td><td><asp:TextBox ID="TextPWD" runat="server" /></td></tr>
                    <tr><td>QR:</td><td><asp:TextBox ID="TextQR" runat="server" /></td></tr>
                    <tr>
                        <td colspan ="2">
                            <table>
                                <tr>
                                    <td></td>
                                    <td>0</td>
                                    <td>1</td>
                                    <td>2</td>
                                    <td>3</td>
                                    <td>4</td>
                                    <td>5</td>
                                    <td>6</td>
                                    <td>7</td>
                                    <td>8</td>
                                    <td>9</td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap">FP Enrolled:</td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled0" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled1" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled2" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled3" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled4" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled5" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled6" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled7" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled8" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpEnrolled9" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap">FP Duress:</td>
                                    <td><asp:CheckBox ID="ChkFpDuress0" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress1" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress2" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress3" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress4" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress5" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress6" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress7" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress8" runat="server" /></td>
                                    <td><asp:CheckBox ID="ChkFpDuress9" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap">Face Enrolled:</td>
                                    <td colspan="10"><asp:CheckBox ID="ChkFaceEnrolled" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnRemoteEnrollFace" runat="server" Text="RemoteEnrollFace" Width="150px" OnClick="btnRemoteEnrollFace_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnRemoteEnrollFP" runat="server" Text="RemoteEnrollFP" Width="150px" OnClick="btnRemoteEnrollFP_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnRemoteEnrollCard" runat="server" Text="RemoteEnrollCard" Width="150px" OnClick="btnRemoteEnrollCard_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnRemoteEnrollQR" runat="server" Text="RemoteEnrollQR" Width="150px" OnClick="btnRemoteEnrollQR_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnExitRemoteEnroll" runat="server" Text="Exit RemoteEnroll" Width="150px" OnClick="btnExitRemoteEnroll_Click" />
                        </td>
                    </tr>
                </table>
                
                <br />
                <br />
                <br />

                <table>
                    <tr><td nowrap="nowrap">FingerNo:</td><td><asp:TextBox ID="TextFingerNo" runat="server" /> (0-9)</td></tr>
                    <tr><td nowrap="nowrap">Duress:</td><td><asp:CheckBox ID="ChkDuress" runat="server" /></td></tr>
                    <tr><td nowrap="nowrap">Duplication Check:</td><td><asp:CheckBox ID="ChkDuplicationCheck" runat="server" /></td></tr>
                    <tr><td nowrap="nowrap">File Path:</td><td><asp:TextBox ID="TextFilePath" runat="server" >D:\UserPhoto.jpg</asp:TextBox>
						</td></tr>
                </table>
            </td>
        </tr>
    </table>



    <asp:ScriptManager ID="ScriptManager2" runat="server">
        </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="mvvProcess" runat="server" ActiveViewIndex="0">
                <asp:View ID="vLaunch" runat="server">
                <asp:Button ID="btnGetAllUserData2" runat="server"  Text="GetAllUserData(Timer)" onclick="btnGetAllUserData2_Click"/>
                </asp:View>
                <asp:View ID="vProgress" runat="server">
                <asp:Button ID="btnCancel" runat="server"  Text="Cancel" onclick="btnCancel_Click"/>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/progress.gif" width="50px" Height="50px"/>
                <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="100" ontick="Timer1_Tick"> </asp:Timer>
                </asp:View>
            </asp:MultiView>
            <asp:Label id="msg" runat="server" Text =""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>