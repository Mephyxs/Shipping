<%@ Page Title="Titles" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="shippinghscodes.aspx.cs" Inherits="admin_shippinghscodes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp1" runat="Server">
    <h1>HS CODES </h1>
    <asp:DataGrid ID="dg1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        DataKeyField="hid" OnCancelCommand="dg1_CancelCommand" OnDeleteCommand="dg1_DeleteCommand"
        OnEditCommand="dg1_EditCommand" OnItemCommand="dg1_ItemCommand" OnPageIndexChanged="dg1_PageIndexChanged"
        OnSortCommand="dg1_SortCommand" OnUpdateCommand="dg1_UpdateCommand" CssClass="datagrid" CellPadding="2" PageSize="20" OnItemCreated="dg1_ItemCreated" >
        <Columns>
            <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" UpdateText="Update"></asp:EditCommandColumn>
            <asp:ButtonColumn CommandName="Delete" Text="Delete"></asp:ButtonColumn>
            <asp:TemplateColumn HeaderText="HS CODES" SortExpression="hscode">
                <EditItemTemplate>
                    <asp:TextBox ID="txthscode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.hscode") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.hscode") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="DESCRIPTION" SortExpression="description">
                <EditItemTemplate>
                    <asp:TextBox ID="txtdescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.description") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.description") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <FooterStyle CssClass="footer" />
        <HeaderStyle CssClass="header" />
        <ItemStyle CssClass="item" />
        <AlternatingItemStyle CssClass="alternatingitem" />
        <SelectedItemStyle CssClass="selecteditem" />
        <EditItemStyle CssClass="edititem" />
        <PagerStyle CssClass="pager" Mode="NumericPages" HorizontalAlign="Right" />
    </asp:DataGrid>
    
    <div id="addHolder">
        <asp:LinkButton CssClass="AddNew" ID="AddNew" runat="server" OnClick="AddNew_Click">Add New</asp:LinkButton><br />
        <asp:Label ID="message" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
    </div>
</asp:Content>

