<%@ Page Language="C#" MasterPageFile="~/AdminPages/Popup.Master" AutoEventWireup="true" CodeBehind="EditActiveCms.aspx.cs" Inherits="Nshop.Web.AdminPages.Content.EditActiveCms" Title="Untitled Page" ValidateRequest="false" %>
<%@ Register Assembly="Napp.Web.ActiveCms" Namespace="Napp.Web.ActiveCms" TagPrefix="Cms"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<Cms:EditContentControl runat="server" />
</asp:Content>
