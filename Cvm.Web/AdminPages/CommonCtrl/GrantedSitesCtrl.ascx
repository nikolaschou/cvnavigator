<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GrantedSitesCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.GrantedSitesCtrl" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<div>
<asp:DropDownList DataTextField="StandardObjectTitle" DataValueField="SysCode" runat="server" ID="GrantedSitesDropDown" AutoPostBack="true" OnTextChanged="OnChangeGrantSiteTB"/>
<br /><br />

<asp:Repeater runat="server" ID="Rep1">
<ItemTemplate>
<%#((SysResource)Container.DataItem).SysOwnerName %> (<%#((SysResource)Container.DataItem).RelatedSysRoot.SysCode %>) <asp:LinkButton runat="server" CommandArgument="<%#((SysResource)Container.DataItem).SysId %>" OnClick="OnClickRemoveRelation" Text="<%$Content:GratedSitesCtrl.Remove %>"/>
<br />
</ItemTemplate>
</asp:Repeater>
</div>