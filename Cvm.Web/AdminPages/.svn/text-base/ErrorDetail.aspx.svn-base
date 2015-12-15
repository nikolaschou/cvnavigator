<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="true" CodeBehind="ErrorDetail.aspx.cs" Inherits="Cvm.Web.AdminPages.ErrorDetail" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
<%=Utl.Content("ErrorDetail.SearchContent") %>
<asp:TextBox runat="server" ID="SearchContentTxtBox" OnTextChanged="OnChangeSearchContent" AutoPostBack="true"/>
<asp:Repeater runat="server" ID="Rep1">
<ItemTemplate>

<a style='<%# this.currentFile!=null && this.currentFile.Equals((String)Container.DataItem) ? "text-decoration:underline" : ""%>' href="<%#PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.fileName,(String)Container.DataItem).GetLinkAsHref() %>"><%#(String)Container.DataItem%></a>
<br />
</ItemTemplate>
</asp:Repeater>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
xxxxxxxxxx
<h2><asp:Literal runat="server" ID="FileNameLit" /></h2>
<%
    dynamic d = new {Name = "xxx", Age = 5};
    dynamic e = new System.Dynamic.ExpandoObject();
    e.Hans = "Hanspeter";
    e.Age = 9;
    %>
    <%=d.Name %>
    <%=e.Hans %>
<div style=" background-color:White;border:1px solid gray;">
<pre>
<asp:Literal runat="server" ID="TextLiteral" />
</pre>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightPane" runat="server">
</asp:Content>
