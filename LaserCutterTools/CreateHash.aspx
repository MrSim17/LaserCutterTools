<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateHash.aspx.cs" Inherits="LaserCutterTools.CreateHash" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>Width: </td><td><asp:TextBox ID="txtWidth" runat="server" Text="4" /></td>
        </tr><tr>
            <td>Height: </td><td><asp:TextBox ID="txtHeight" runat="server" Text="2" /></td>
        </tr><tr>
            <td>Line Spacing: </td><td><asp:TextBox ID="txtLineSpacing" runat="server" Text="0.05" /></td>
        </tr><tr>
            <td>Gap Width: </td><td><asp:TextBox ID="txtGapWidth" runat="server" Text="0.05" /></td>
        </tr><tr>
            <td>Hash Count: </td><td><asp:TextBox ID="txtHashCount" runat="server" Text="4" /></td>
        </tr><tr>
            <td>Download: </td><td><asp:CheckBox ID="chkForceDownload" runat="server" /></td>
        </tr><tr>
            <td colspan="2"><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Create Hash" /></td>
        </tr>
    </table>
</asp:Content>
