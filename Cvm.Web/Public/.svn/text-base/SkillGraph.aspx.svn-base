<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="SkillGraph.aspx.cs" Inherits="Cvm.Web.Public.SkillGraph" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Register Src="~/CommonCtrl/SkillMap2.ascx" TagPrefix="uc" TagName="SkillMap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ad:AdmHyperLink ID="AdmHyperLink1" runat="server" AutoBind="true" PageLink="<%#CvmPages.WelcomePage %>" ContentId="SkillGraph.GotoWelcomePage" />
    
<uc:SkillMap runat="server" ID="MySkillMap" />

<%=Utl.ContentBread("SkillMap.EnterSkillName") %>
<asp:TextBox runat=server ID="SearchStringBx" OnTextChanged="OnSearchTextChange" AutoPostBack="true"/><br />

</asp:Content>