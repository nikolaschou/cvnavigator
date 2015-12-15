<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Cvm.Web.Public.ForgotPassword" Title="Untitled Page" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="passwordRecoveryDiv">
<%=AdminContentMgr.instance.GetContent("ForgotPassword.Intro") %><br /><br />
<a href="Login.aspx"><%=AdminContentMgr.instance.GetContent("ForgotPassword.GotoLogin") %>
</a>
<br /><br />
<asp:PasswordRecovery runat="server" MailDefinition-IsBodyHtml="True" MailDefinition-BodyFileName='~/EmailTemplates/PasswordRecovery.txt' OnSendingMail="ForgotPassword_SendingMail" MailDefinition-Subject="Your password request from CVNavigator" />
</div>
<script>
    $('#passwordRecoveryDiv td').attr('align', 'left');
</script>
</asp:Content>
