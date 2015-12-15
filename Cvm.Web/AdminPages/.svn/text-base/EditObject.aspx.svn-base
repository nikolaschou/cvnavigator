<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="true"
    CodeBehind="EditObject.aspx.cs" Inherits="Nshop.Web.AdminPages.CommonPages.EditObject"
    Title="Untitled Page" %>

<%@ Import Namespace="Cvm.Backend.Business.Users" %>

<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Backend.Business.Common" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<%@ Import Namespace="Napp.Backend.BusinessObject" %>
<%@ Register TagPrefix="Ex" Namespace="Napp.Web.AdmControl" Assembly="Napp.Web.AdmControl" %>
<%@ Register Src="~/CommonCtrl/EditObjectCtrl.ascx" TagName="EditObjectCtrl" TagPrefix="uc2" %>
<%@ Register Src="~/AdminPages/CommonCtrl/DeleteObjectCtrl2.ascx" TagName="DeleteObjectCtrl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
    <Ex:AdmHyperLink ID="CreateLink" runat="server"  />
    <br />
    <br />
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <h2>
                <%=this.GetContent("List") %></h2>
            <br />
            <asp:TextBox ToolTip="<%$Content:EditObject.FilterList %>" ID="FilterTextBox" runat="server"
                AutoPostBack="true" CausesValidation="false" OnTextChanged="OnChangeFilterText" />
            <br />
            <asp:Repeater EnableViewState="false" runat="server" ID="rep">
                <ItemTemplate>
                    <nobr>
                        <Ex:AdmHyperLink runat="server" SkinID="menuSkin" PageLink="<%#PageNavigation.GetCurrentLink().IncludeParm(QueryParmCvm.type).SetParm(QueryParmCvm.id,((IBusinessObject)Container.DataItem).Idfr.IdfrAsLong()).SetParm(QueryParmCvm.filter,this.ListFilter) %>" Text="<%#((IBusinessObject)(Container.DataItem)).ExtendedObjectTitle %>" ToolTip="<%#((IBusinessObject)(Container.DataItem)).ExtendedObjectTitle %>" />
                    </nobr>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
    <uc2:EditObjectCtrl runat="server" ID="EditObjCtrl" />
    <uc2:DeleteObjectCtrl runat="server" ID="DeleteObjCtrl" />
</asp:Content>
<asp:Content ContentPlaceHolderID="RightPane" runat="server">
    <ad:AuxLinkButton Visible="true" ID="DeleteLinkBtn" runat="server" Text="<%$Content:EditObject.DeleteObject %>" OnClick="OnClickDeleteObject" />
</asp:Content>
