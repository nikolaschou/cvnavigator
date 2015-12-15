<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Cvm.Web.Login" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/AdminPages/AdminMasterPage.master" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%=AdminContentMgr.instance.GetContent("Login.Intro") %>

    <div class="allActiveLinks loginctrl">
        <br />
        <br />
        <asp:Login runat="server" ID="LoginForm" OnLoggedIn="OnLoggedInHandler"></asp:Login><br/><br/>
        <a href="ForgotPassword.aspx"><%=AdminContentMgr.instance.GetContent("Login.GotoForgotPassword") %> </a> <br/>
        <asp:LinkButton ID="LBSignUp" runat="server" OnClick="LBSignUp_Click" Text="<%$Content:Login.Signup %>" />  <br/>

        <% if (ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull() != null)
           {%>
            <asp:LinkButton ID="LBCompanySignup" runat="server" OnClick="LBCompanySignUp_Click" Text="<%$Content:Login.CompanySignup %>" />
            <% } %>
    </div>
</asp:Content>
