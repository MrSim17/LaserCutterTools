<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="LaserCutterTools._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table>
        <tr>
            <td>Width: </td><td><asp:TextBox ID="txtWidth" runat="server" /></td>
        </tr><tr>
            <td>Height: </td><td><asp:TextBox ID="txtHeight" runat="server" /></td>
        </tr><tr>
            <td>Line Spacing: </td><td><asp:TextBox ID="txtLineSpacing" runat="server" /></td>
        </tr><tr>
            <td>Gap Width: </td><td><asp:TextBox ID="txtGapWidth" runat="server" /></td>
        </tr><tr>
            <td>Hash Count: </td><td><asp:TextBox ID="txtHashCount" runat="server" /></td>
        </tr><tr>
            <td colspan="2"><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" /></td>
        </tr>
    </table>
</asp:Content>
