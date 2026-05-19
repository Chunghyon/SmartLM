<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentPane.aspx.cs" Inherits="SmackBio.WebSocketSDK.Sample.Pages.DepartmentPane"
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
                 User Department
                 </td></tr>
             <tr>
                <td>1</td>
                <td>
                    <asp:Button ID="btnGetDepartment" runat="server" Text="GetDepartment" OnClick="btnGetDepartment_Click"/>
                </td>
                <td>
                    <asp:DropDownList ID="cmbDepartmentNo" runat="server" Width="150px" Height="25px"
                        DataSourceID="department_no_items">
                    </asp:DropDownList>
                    <asp:ObjectDataSource runat="server" ID="department_no_items" 
                        TypeName="SmackBio.WebSocketSDK.Sample.Pages.DepartmentPane" 
                        SelectMethod="GetDepartmentNoList" />
                </td>
            </tr>
            <tr>
                <td>2</td>
                <td>
                    <asp:Button ID="btnSetDepartment" runat="server" Text="SetDepartment" OnClick="btnSetDepartment_Click"/>
                </td>
                <td>
                    <asp:TextBox ID="txtDepartmentName" runat="server"></asp:TextBox>
                </td>
            </tr>
           </table>
        </td>
     </tr>
     <tr>
        <td>
            <table>
             <tr><td colspan="3">
                 Proxy Department
                 </td></tr>
             <tr>
                <td>3</td>
                <td>
                    <asp:Button ID="btnGetProxyDept" runat="server" Text="GetProxyDept" OnClick="btnGetProxyDept_Click"/>
                </td>
                <td>
                    <asp:DropDownList ID="cmbProxyDepartmentNo" runat="server" Width="150px" Height="25px"
                        DataSourceID="proxy_department_no_items">
                    </asp:DropDownList>
                    <asp:ObjectDataSource runat="server" ID="proxy_department_no_items" 
                        TypeName="SmackBio.WebSocketSDK.Sample.Pages.DepartmentPane" 
                        SelectMethod="GetProxyDepartmentNoList" />
                </td>
            </tr>
            <tr>
                <td>4</td>
                <td>
                    <asp:Button ID="btnSetProxyDept" runat="server" Text="SetProxyDept" OnClick="btnSetProxyDept_Click"/>
                </td>
                <td>
                    <asp:TextBox ID="txtProxyDepartmentName" runat="server"></asp:TextBox>
                </td>
            </tr>
           </table>
        </td>
    </tr>
    </table>
</asp:Content>