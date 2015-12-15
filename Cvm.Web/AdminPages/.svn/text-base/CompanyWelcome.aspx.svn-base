<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="true" CodeBehind="CompanyWelcome.aspx.cs" Inherits="Cvm.Web.Public.CompanyWelcome" title="Test"%>

<%@ Register TagPrefix="my" TagName="ListFilterCtrl" Src="~/AdminPages/CommonCtrl/ListFilterCtrl.ascx" %>

<asp:Content ID="CompanyLeftPane" ContentPlaceHolderID="LeftPane" runat="server">
    <ad:ContainerCtrl ID="ContainerCtrl1" runat="server" ContentId="<%$Content:CompanyWelcome.JobList %>">
        <my:ListFilterCtrl runat="server" ObjectType="JobAnnouncement" TableName="companyId" TableValue="<%=TableLookupValue %>" ID="ListFilterCtrl1" />
    </ad:ContainerCtrl>
    <ad:ContainerCtrl ID="ContainerCtrl4" runat="server" ContentId="<%$Content:CompanyWelcome.User/Company %>">
        <asp:LinkButton ID="LBNewUser"    runat="server" OnClick="LB_Click" Text="<%$Content:CompanyWelcome.InviteNewUser %>"    />  <br/>
        <asp:LinkButton ID="LBDeleteUser" runat="server" OnClick="LB_Click" Text="<%$Content:CompanyWelcome.DeleteUser %>"       />  <br/>
        <asp:LinkButton ID="LBEdit"       runat="server" OnClick="LB_Click" Text="<%$Content:CompanyWelcome.EditCompany/User %>" />  <br/>
    </ad:ContainerCtrl>
</asp:Content>
<asp:Content ID="CompanyMiddlePane" ContentPlaceHolderID="MiddlePane" runat="server">
     <ad:ContainerCtrl ID="CCJobDescription" runat="server" ContentId="<%$Content:CompanyWelcome.JobDescription %>" Visible="false">
        <auto:AutoForm1 SkipHeader="false" runat="server" ID="JobForm"/>
        <div class="buttons">
            <ad:AdmButton ID="ADBSaveCompany" CssClass="saveBtn" runat="server" CausesValidation="true" OnClick="ABCreateJobAnnouncement" Text="<%$Content:CompanyWelcome.Save %>" />
            <ad:AdmButton ID="ABSaveAndPreview" CssClass="saveBtn" runat="server" CausesValidation="true" OnClick="ABSaveAndPreview_Click" OnClientClick="aspnetForm.target ='_blank';" Text="<%$Content:CompanyWelcome.SaveAndPreview %>" />
        </div>
    </ad:ContainerCtrl>

    <ad:ContainerCtrl ID="CCNewUserData" ContentId="Company.UserData" runat="server">
        <table border="0" cellpadding="5" class="autoform">
            <auto:AutoForm1 ID="NewUserObjCtrl" runat="server" SkipHeader="true"/>
            <tr>
                <td><%=Utl.ContentHlp("Company.Password") %></td>
                <td><ad:AdmTextBox runat="server" ID="ATBNewUserPassword" /></td>
            </tr>  
            <tr>
                <td><%=Utl.ContentHlp("Company.ReEnterPassword") %></td>
                <td><ad:AdmTextBox runat="server" ID="ATBReEnterNewUserPassword" /></td>
            </tr>  
        </table>
    </ad:ContainerCtrl>
        
    <ad:ContainerCtrl ID="CCDeleteUser" ContentId="Company.DeleteUser" runat="server">
        <asp:ListView runat="server" ID="LVDeleteUser">
            <ItemTemplate>
                <asp:LinkButton ID="LBDeleteUser" runat="server" CommandArgument='<%#Eval("userId")%>' CommandName='<%#Eval("userName") %>' OnClick="LBDeleteUser_Click" Text='<%#Eval("firstName") + " " + Eval("lastName") + " - " + Eval("userName") %>' />  <br/>
            </ItemTemplate>
        </asp:ListView>
        <asp:Label ID="LblCompanyUsers" runat="server" Text=""></asp:Label>
    </ad:ContainerCtrl>

    <%-- Edit Company--%>
    <ad:ContainerCtrl ID="CCUserData" ContentId="Company.UserData" runat="server">
        <table border="0" cellpadding="5" class="autoform">
            <auto:AutoForm1 ID="UserObjCtrl" runat="server" SkipHeader="true"/>
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
                    <auto:AutoForm1 ID="CompanyInfoCtrl" runat="server" SkipHeader="true" OmitProperties="LastModifiedTs"/>
                </td>
            </tr>
        </table>
    </ad:ContainerCtrl>

    <div class="buttons">
        <ad:AdmButton ID="ABNewUser"       runat="server" CssClass="saveBtn" ContentId="Company.NewUser"                OnClick="ABNewUser_Click"/>
        <ad:AdmButton ID="ABUpdateCompany" runat="server" CssClass="saveBtn" ContentId="Company.UpdateUserAndCompany"   OnClick="ABUpdateCompany_Click"/>
    </div>
</asp:Content>