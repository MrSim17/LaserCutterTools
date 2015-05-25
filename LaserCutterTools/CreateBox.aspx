<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateBox.aspx.cs" Inherits="LaserCutterTools.CreateBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>Dimension X: </td><td><asp:TextBox ID="txtDimensionX" runat="server" Text="1.5" /></td>
        </tr><tr>
            <td>Dimension Y: </td><td><asp:TextBox ID="txtDimensionY" runat="server" Text="1.5" /></td>
        </tr><tr>
            <td>Dimension X: </td><td><asp:TextBox ID="txtDimensionZ" runat="server" Text="1.5" /></td>
        </tr><tr>
            <td>Tabs X: </td><td><asp:TextBox ID="txtTabsX" runat="server" Text="2" /></td>
        </tr><tr>
            <td>Tabs Y: </td><td><asp:TextBox ID="txtTabsY" runat="server" Text="2" /></td>
        </tr><tr>
            <td>Tabs Z: </td><td><asp:TextBox ID="txtTabsZ" runat="server" Text="2" /></td>
        </tr><tr>
            <td>Material Thickness: </td><td><asp:TextBox ID="txtMaterialThickness" runat="server" Text="0.176" /></td>
        </tr><tr>
            <td>Tool Diameter: </td><td><asp:TextBox ID="txtToolDiameter" runat="server" Text="0.01" /></td>
        </tr><tr>
            <td>Rotate Parts: </td><td><asp:CheckBox ID="chkRotateParts" runat="server" /></td>
        </tr><tr>
            <td>Make Box Open: </td><td><asp:CheckBox ID="chkMakeBoxOpen" runat="server" /></td>
        </tr><tr>
            <td>Download: </td><td><asp:CheckBox ID="chkForceDownload" runat="server" /></td>
        </tr><tr>
            <td>Log Only: </td><td><asp:CheckBox ID="chkLogOnly" runat="server" /></td>
        </tr><tr>
            <td colspan="2"><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Create Hash" /></td>
        </tr>
    </table>
    <pre>
        <asp:Literal ID="litLog" runat="server" />
    </pre>
</asp:Content>