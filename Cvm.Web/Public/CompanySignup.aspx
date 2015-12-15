<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="CompanySignup.aspx.cs" Inherits="Cvm.Web.Public.CompanySignup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="MainPanel" Visible="true">
        <ad:ContainerCtrl ID="CCUserData" ContentId="Company.UserData" runat="server">
            <table border="0" cellpadding="5" class="autoform">
                <auto:AutoForm1 ID="UserObjCtrl" OmitProperties="LastModifiedTs" runat="server" SkipHeader="true" AutoBind="true" ObjectSourceInstance="<%#this.user %>" IncludeOnlyProperties="Email;FirstName;LastName;Mobile;Phone"/>
                <tr>
                    <td><%=Utl.ContentHlp("Company.Password") %></td>
                    <td><ad:AdmTextBox runat="server" ID="ATBPassword" /></td>
                </tr>  
                <tr>
                    <td><%=Utl.ContentHlp("Company.ReEnterPassword") %></td>
                    <td><ad:AdmTextBox runat="server" ID="ATBReEnterPassword" /></td>
                </tr>  
            </table>
        </ad:ContainerCtrl>
        <ad:ContainerCtrl ID="CCCompanyData" ContentId="Company.CompanyData" runat="server">
            <table border="0" cellpadding="5" class="autoform">
                <tr>
                    <td>
                        <auto:AutoForm1 ID="CompanyInfoCtrl" OmitProperties="LastModifiedTs" runat="server" SkipHeader="true" ObjectSourceInstance="<%#this.company %>" />
                    </td>
                </tr>
            </table>
        </ad:ContainerCtrl>
        <div class="buttons">
            <ad:AdmButton ID="ABCreateCompany" runat="server" CssClass="saveBtn" ContentId="Company.CreateCompany"      OnClick="ABCreateCompany_Click" />
        </div>
    </asp:Panel>
</asp:Content>
