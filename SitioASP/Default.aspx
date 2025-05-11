<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 429px;
        }
        .auto-style3 {
            height: 23px;
        }
        .auto-style4 {
            width: 85px;
        }
        .auto-style5 {
            width: 125px;
        }
        .auto-style6 {
            width: 127px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%; background-color: #0099CC;">
                <tr>
                    <td class="auto-style6">Nombre Art</td>
                    <td class="auto-style2" colspan="2">&nbsp;&nbsp;
                        <asp:TextBox ID="txtNombre" runat="server" Width="183px"></asp:TextBox>
&nbsp;&nbsp;
                        <asp:Button ID="btnFiltro" runat="server" Text="Filtro" Width="53px" OnClick="btnFiltro_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">Categorias</td>
                    <td colspan="3">&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlCategorias" runat="server" Height="16px" Width="188px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="auto-style3" style="text-align: right">
                        <asp:Label ID="lblError" runat="server" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvArtxVen" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="203px" Width="606px" OnSelectedIndexChanged="gvArtxVen_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvArtxVen_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="artCod" HeaderText="Codigo" />
                                <asp:BoundField DataField="artNom" HeaderText="Nombre" />
                                <asp:BoundField DataField="artFechVen" HeaderText="Fecha Vencimiento" />
                                <asp:CommandField SelectText="Desplegar" ShowSelectButton="True" />
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#E3EAEB" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        Codigo:&nbsp;&nbsp;
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style5">
                        Precio:</td>
                    <td>
                        <asp:TextBox ID="txtPrecio" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        Nombre:</td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtNombreArt" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style5">
                        Fecha de Ven:</td>
                    <td>
                        <asp:TextBox ID="txtFecha" runat="server" TextMode="DateTime"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        Tipo:</td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtTipo" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style5">
                        Cantidad de Ventas:</td>
                    <td>
                        <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        Tamaño:</td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtTamaño" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style5">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                    <td>
                        Categoria:</td>
                    <td>
                        <asp:TextBox ID="txtCategoria" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
