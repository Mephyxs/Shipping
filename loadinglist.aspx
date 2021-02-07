<%@ Page Title="Loading List" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="loadinglist.aspx.cs" Inherits="admin_loadinglist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../styles/datalist.css" rel="stylesheet" type="text/css" />
    <link href="../styles/bootstrap.min.css" rel="stylesheet" type="text/css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cp1" runat="Server">
    <div class="container">
        <main>
    <h1>Loading List</h1>
    <table class="siteTable">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Select Country" CssClass="siteLabel"></asp:Label></td>
            <td>

                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="custom-select form-control-sm" DataValueField="col1" DataTextField="col2" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList></td>
            <td>
                <img alt="" style="width: 20px; height: 1px; border: 0px" src="../images/spacer.gif" /></td>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Select Port" CssClass="siteLabel"></asp:Label></td>
            <td>

                <asp:DropDownList ID="ddlPort" runat="server" CssClass="custom-select form-control-sm" DataValueField="col1" DataTextField="col2" AutoPostBack="True" OnSelectedIndexChanged="ddlPort_SelectedIndexChanged"></asp:DropDownList></td>
            <td>
                <img alt="" style="width: 20px; height: 1px; border: 0px" src="../images/spacer.gif" /></td>
            <td></td>
        </tr>
         <tr>
            <td>
                 </td>
            <td>

                <asp:CheckBox ID="chkAll" runat="server" Text=" Show All" AutoPostBack="true" Checked="true" OnCheckedChanged="chkAll_CheckedChanged" /></td>
            <td>
                <img alt="" style="width: 20px; height: 1px; border: 0px" src="../images/spacer.gif" /></td>
            <td></td>
        </tr>
    </table>
    
     <div class="col-md-12 mx-auto" runat="server" id="tblSearch">
   
       

    <div class="card">
        
        <asp:DataList ID="dl1" CellSpacing="1" runat="server" DataKeyField="containerid" HorizontalAlign="Left" RepeatDirection="Horizontal" OnItemCommand="DataList1_ItemCommand" CssClass="datalist" GridLines="Both">
            <ItemTemplate>

                <div class="list-group">
                     <a href="#" class="list-group-item list-group-item-action">
                       
                            <asp:LinkButton ID="SelectButton" CssClass="btn btn-success" Text="Select container" CommandName="Select" runat="server" />
                     </a>
                   
                    <a href="#" class="list-group-item list-group-item-action">
                        <div class="d-flex w-100 justify-content-between">
                            
                            <small class="text-muted"><asp:Label ID="Label7" runat="server" Text='<%# Eval("DAYSS") %>' /> days ago</small>
                        </div>
                        <div class="shadow-lg mb-5 alert-warning rounded">
                            <dl class="row">
                                <dt class="col-sm-5">VOLUME</dt>
                                <dd class="col-sm-7">
                                 <asp:Label ID="containeridLabel" runat="server" Text='<%# Eval("usedVolume","{0:###0.0}") %>' /> / <asp:Label ID="Label2" runat="server" Text='<%# Eval("maxvol","{0:###0.0}") %>' /></dd>

                                <dt class="col-sm-5">WEIGHT</dt>
                                <dd class="col-sm-7"> <asp:Label ID="wtlabel" runat="server" Text='<%# Eval("usedWeight","{0:###0.0}") %>' />
                                </dd>
                            </dl>
                        </div>
                          <div class="shadow-lg mb-5 alert-primary rounded">
                            <dl class="row">
                                <dt class="col-sm-5">CONTAINER</dt>
                                <dd class="col-sm-7">
                                 <asp:Label ID="containerLabel" runat="server" Text='<%# Eval("container") %>' />

                                </dd>

                                <dt class="col-sm-5">Port</dt>
                                <dd class="col-sm-7"> <asp:Label ID="Label4" runat="server" Text='<%# Eval("port") %>' /> , <asp:Label ID="Label5" runat="server" Text='<%# Eval("country") %>' />
                                </dd>
                            </dl>
                        </div>
                         <div class="shadow-lg mb-5 alert-info rounded">
                            <dl class="row">
                                <dt class="col-sm-5">CLOSING DATE</dt>
                                <dd class="col-sm-7">
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("closingDate") %>' />

                                </dd>

                                <dt class="col-sm-5">DEP. DATE</dt>
                                <dd class="col-sm-7"> <asp:Label ID="deoLabel" runat="server" Text='<%# Eval("departureDate") %>' />
                                </dd>
                                <dt class="col-sm-5">Arr. DATE</dt>
                                <dd class="col-sm-7"> <asp:Label ID="arrlab" runat="server" Text='<%# Eval("arrivalDate") %>' />
                                </dd>
                            </dl>
                        </div>
                       
                    </a>
                    
                        <div class="btn-group" role="group" aria-label="Basic checkbox toggle button group">
                            <div class="btn-group" role="group" aria-label="Basic outlined example">
                                 <asp:LinkButton runat="server" CssClass="btn btn-outline-primary" CommandName="CREATEPDF" Text="Create & Download"></asp:LinkButton>
                                <a href='loadinglistpdf.aspx?c=<%# Eval("containerid") %>' target="_blank" class="btn btn-outline-primary">Loading List</a>
                                <a href='../office/manifestpdf.aspx?f=1&id=<%# Eval("containerid") %>' target="_blank" class="btn btn-outline-primary">Manifest</a>
                                <a href='mailto:<%# Eval("ShippingAgentEmail") %>&subject=Manifest Attached&body=Please see attached manifest from WI Freight' class="btn btn-outline-primary">Email</a>
                               <%--<asp:LinkButton CssClass="btn btn-outline-primary" ID="btnloadedcontainer" runat="server" Text="Loaded Items" OnClick="btnloadedcontainer_Click"></asp:LinkButton>--%>
                                
                            </div>
                        </div>
                   
                <br />
             
                    </div>
            </ItemTemplate>
            <SelectedItemStyle CssClass="selecteditem" />
            <ItemStyle CssClass="item" />
            <AlternatingItemStyle CssClass="alternatingitem" />
        </asp:DataList>

    </div>
  </div>

        
         
    <div class="col-md-9 mx-auto" runat="server" id="Div1">
    <div class="card">
        <div class="btn-group" role="group" aria-label="Basic outlined example">
            <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAdd" runat="server" Text="Load Container" OnClick="btnAdd_Click"></asp:LinkButton>
            <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnRemove" runat="server" Text=" Remove from Container" OnClick="btnRemove_Click"></asp:LinkButton>
            <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnAddContainer" runat="server" Text="Add Container" OnClick="btnAddContainer_Click"></asp:LinkButton>
            <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnSelectAll" runat="server" Text="Select All" OnClick="btnSelectAll_Click"></asp:LinkButton>
            <asp:LinkButton CssClass="btn btn-outline-primary" ID="btnDeSelectAll" runat="server" Text="Deselect All" OnClick="btndeSelectAll_Click"></asp:LinkButton>
            <asp:LinkButton CssClass="btn btn-danger" ID="btnDelContainer" runat="server" Text="Delete Container" OnClick="btnDelContainer_Click"></asp:LinkButton>

        </div>
    </div>
        </div>
   
    <asp:DataGrid ID="dg1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        DataKeyField="shippingorderid"
        OnItemCommand="dg1_ItemCommand" OnPageIndexChanged="dg1_PageIndexChanged"
        OnSortCommand="dg1_SortCommand" CssClass="datagrid" CellPadding="2" PageSize="5">
        <Columns>
            <asp:TemplateColumn HeaderText="Select">
                <ItemTemplate>
                    <asp:CheckBox ID="cbSelected" runat="server" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="txtShipOrder" HeaderText="Order No" SortExpression="shippingorderid"></asp:BoundColumn>
            <asp:BoundColumn DataField="orderdate" HeaderText="Order Date" SortExpression="orderdate" DataFormatString="{0:dd MMM yyyy}"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Consignor" SortExpression="consignor">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.consignor") %>' NavigateUrl='<%# "~/office/shippingorder.aspx?id=" + DataBinder.Eval(Container, "DataItem.shippingorderid")  %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Consignee" SortExpression="consignee">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.consignee") %>' NavigateUrl='<%# "~/office/shippingorder.aspx?id=" + DataBinder.Eval(Container, "DataItem.shippingorderid")  %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="ordervolume" HeaderText="Volume" SortExpression="ordervolume"></asp:BoundColumn>
            <asp:BoundColumn DataField="orderweight" HeaderText="Weight" SortExpression="orderweight"></asp:BoundColumn>
            <asp:BoundColumn DataField="container" HeaderText="Container" SortExpression="container"></asp:BoundColumn>
            <asp:BoundColumn DataField="DestinationCountry" HeaderText="Destination" SortExpression="DestinationCountry"></asp:BoundColumn>
            <asp:BoundColumn DataField="port" HeaderText="Port" SortExpression="port"></asp:BoundColumn>
            <asp:BoundColumn DataField="delmethod" HeaderText="Method" SortExpression="delmethod"></asp:BoundColumn>
            <asp:BoundColumn DataField="ReqDate" HeaderText="Req Arr Date" SortExpression="requesteddeldate"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="BOL">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text="Bill Of Loading" NavigateUrl='<%# "~/office/billofladingpdf.aspx?id=" + DataBinder.Eval(Container, "DataItem.shippingorderid")  %>' Target="_blank"></asp:HyperLink>
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
    <div id="addNewHolder">
        <asp:Label ID="message" ForeColor="red" Font-Bold="true" runat="server"></asp:Label>
    </div>
            </main>
            </div>
   
</asp:Content>

