<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="ClientWelcome.aspx.cs" Inherits="Cvm.Web.AdminPages.ClientWelcome" Title="Untitled Page" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ad:AdmPanel runat="server" AutoBind="true" Visible="<%#this.CanListResources() %>">
<div class="niceLink">
<img src="../images/master/cv-icon-medium.PNG" alt="CVNavigator" />
<ad:AdmHyperLink runat="server" Text="<%$Content:ClientWelcome.ListCv %>" AutoBind="true" PageLink="<%#CvmPages.ReportCvPage %>"/>
</div>
</ad:AdmPanel>
<div class="niceLink">
<img src="../images/master/cv-icon-medium.PNG" alt="CVNavigator" />
    <ad:AdmHyperLink ID="AdmHyperLink1" Text="<%$Content:ClientWelcome.Assignments %>" runat="server" AutoBind="true" PageLink="<%#CvmPages.Assignments %>"/>
</div>

</asp:Content>
