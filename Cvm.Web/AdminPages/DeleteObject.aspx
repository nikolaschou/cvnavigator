<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="DeleteObject.aspx.cs" Inherits="Cvm.Web.AdminPages.DeleteObject" Title="Untitled Page" %>

<%@ Register Src="CommonCtrl/DeleteObjectCtrl.ascx" TagName="DeleteObjectCtrl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:DeleteObjectCtrl ID="DeleteObjectCtrl1" runat="server" />
</asp:Content>
