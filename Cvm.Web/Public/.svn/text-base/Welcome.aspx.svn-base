<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master"
    AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="Cvm.Web.Public.Welcome"  %>
<%@ OutputCache Duration="600"  VaryByParam="*"%>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Register Src="~/CommonCtrl/SkillMap1.ascx" TagPrefix="uc" TagName="SkillMap1" %>
    <%@ Register Src="~/CommonCtrl/SkillMap2.ascx" TagPrefix="uc" TagName="SkillMap" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table border="0" cellpadding="10" class="centeredTable">
        <tr>
            <td valign="top">
                <a href='<%=ContextObjectHelper.GetSearchAppForGlobal()%>' class="admLinkButton"><%=Utl.ContentHlp("Welcome.SearchApp") %></a><br />
                <%=Utl.Content("Welcome.SearchAppDescr") %>
            </td>
            <td valign="top">
                <ad:AdmHyperLink ID="AdmHyperLink4" Text="<%$Content:Welcome.Login %>" runat="server"
                    AutoBind="true" PageLink="<%#CvmPages.LoginPage %>" /><br />
                <%=Utl.Content("Welcome.ForExistingUsers") %>
            </td>
            <td valign="top">
                <ad:AdmHyperLink ID="AdmHyperLink2" Text="<%$Content:Welcome.SignUp %>" runat="server"
                    AutoBind="true" PageLink="<%#CvmPages.SignUpPage %>" /><br />
                <%=Utl.Content("Welcome.RegisterCV") %>
            </td>
            <td valign="top">
                <ad:AdmHyperLink ID="AdmHyperLink3" Text="<%$Content:Welcome.CreateNewSite %>" runat="server"
                    AutoBind="true" PageLink="<%#CvmPages.NewSitePage %>" /><br />
                <%=Utl.Content("Welcome.ForCompanies") %>
            </td>
        </tr>
        <tr><td colspan="6">
    <uc:SkillMap1 ID="SkillMap1" runat="server" DivWidth="700" NumberOfSkills="20" />
    <%=Utl.Content("Welcome.SkillSummary") %>
    <br /><br />
    <div class="roundedDiv" style="width:700px;">
    <uc:SkillMap runat="server" ID="MySkillMapGraph" MakeHrefLinks="true" />
    </div>
    <%=Utl.Content("Welcome.SkillGraphSummary") %>
    <ad:AdmHyperLink runat="server" AutoBind="true" PageLink="<%#CvmPages.SkillGraphLink(this.SkillMap1.Top1SkillName) %>" ContentId="Welcome.ClickToSeeMore" />
    
    </td></tr>
     
    </table>
</asp:Content>
