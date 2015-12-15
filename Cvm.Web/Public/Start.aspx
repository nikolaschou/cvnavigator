<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage0.master" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="Cvm.Web.Public.Start" Title="Untitled Page" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Holder1" runat="server">
<div style="vertical-align:middle;text-align:center;width:100%;height:600px;">
<a href="NewSite.aspx">
    <img style="border:0px solid white" src="../images/master/kompas-med-logo.png" alt="Start" /><br />
    <span style="font-size:48px"><%=AdminContentMgr.instance.GetContent("Start.StartHere") %></span>
</a>
</div>
</asp:Content>
