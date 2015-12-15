<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ListFilterCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.ListFilterCtrl" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<%@ Import Namespace="Napp.Backend.BusinessObject" %>
<div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <h2>
                <%=this.GetContent("List") %></h2>
            <% if (this.GetListData().Count > 20)
               {%>
            <div>
                <asp:TextBox ToolTip="<%$Content:EditObject.FilterList %>" ID="FilterTextBox" runat="server" AutoPostBack="true" CausesValidation="false" OnTextChanged="OnChangeFilterText" />
            </div>
            <br />
            <%
               }%>
            <div class="listOfLinks">
                <ad:AdmHyperLink runat="server" ID="link1" SkinID="menuSkin" runat="server" AutoBind="true" PageLink="<%#PageNavigation.GetCurrentLink().IncludeExistingParms().ExcludeParm(QueryParmCvm.id).SetMode(PageMode.New) %>" Text="<%$Content:ListFilterCtrl.CreateNew %>" ToolTip="<%$Content:ListFilterCtrl.CreateNew.Help %>" />
                <asp:Repeater EnableViewState="false" runat="server" ID="rep">
                    <ItemTemplate>
                        <div class='<%#this.IsSelected((IBusinessObject)Container.DataItem)? "selected" : ""%>'>
                            <ad:AdmHyperLink runat="server" SkinID="menuSkin" ID="AdmHyperLink1" PageLink="<%#PageNavigation.GetCurrentLink().IncludeParm(QueryParmCvm.type).SetParm(QueryParmCvm.id,((IBusinessObject)Container.DataItem).Idfr.IdfrAsLong()).SetParm(QueryParmCvm.filter, this.ListFilter) %>" Text="<%#((IBusinessObject)(Container.DataItem)).ExtendedObjectTitle %>" ToolTip="<%#((IBusinessObject)(Container.DataItem)).ExtendedObjectTitle %>" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
