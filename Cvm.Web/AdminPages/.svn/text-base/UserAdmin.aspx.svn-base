<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master"
    AutoEventWireup="true" CodeBehind="UserAdmin.aspx.cs" Inherits="Cvm.Web.AdminPages.UserAdmin" %>

<%@ Register TagPrefix="Ex" Namespace="Napp.Web.AdmControl" %>
<%@ Import Namespace="Cvm.Backend.Business.Meta" %>
<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<%@ Import Namespace="Napp.Backend.BusinessObject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
    <ad:AdmHyperLink ID="CreateLink" runat="server"
        AutoBind="true" PageLink="<%#PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.type,this.UserRole()).SetMode(PageMode.New) %>" />
        <br /><br />
    <asp:Repeater EnableViewState="false" runat="server" ID="rep">
        <ItemTemplate>
            <nobr>
                <Ad:AdmHyperLink runat="server"  PageLink="<%#PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.type,this.UserRole()).SetParm(QueryParmCvm.userId,((IBusinessObject)Container.DataItem).Idfr.IdfrAsLong()) %>" Text="<%#this.GetListTitle((SysUserObj)(Container.DataItem))%>"  />
            </nobr>
            <br />
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
    <asp:Panel runat="server" ID="MainPanel">
    <div class="autoformDiv">
        <table border="0" cellpadding="5" class="autoform">
        <tr>
        <td><%=Utl.ContentHlp("CustomerContacts.UserName") %></td>
        <td><ad:AdmTextBox runat="server" ID="UserNameTxt" /></td>
        </tr>        
            <auto:AutoForm1 ID="UserObjCtrl" runat="server" SkipHeader="true" />
            <auto:AutoForm1 ID="SysUserObjCtrl" runat="server" SkipHeader="true" />
        </table>
        </div>
        <div class="buttons">
        <ad:AdmButton runat="server" OnClick="OnClickSaveBtn" CssClass="saveBtn" ContentId="Standard.Save" />
        <ad:AdmDialog ID="AdmDialog1" runat="server" Title="<%$Content:UserAdmin.ChangePassword %>">
        </div>
            <fieldset>
                <ad:AdmValidationArea ID="AdmValidationArea1" runat="server" />
                <%=Utl.ContentHlpBread("UserAdmin.EnterNewPassword") %><asp:TextBox runat="server" ID="Password1TextBox" TextMode=SingleLine />
                <%=Utl.ContentHlpBread("UserAdmin.RepeatNewPassword") %><asp:TextBox runat="server" ID="Password2TextBox" TextMode="SingleLine" />
                </br>
                <asp:Button ID="Button1" CausesValidation="false" runat="server" OnClick="OnClickSetPasswordBtn" Text="<%$Content:UserAdmin.ChangePasswordBtn %>" />
            </fieldset>
        </ad:AdmDialog>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightPane" runat="server">
    <ad:AuxLinkButton ID="AuxLinkButton1" runat="server" OnClick="OnClickInviteUser"
        ContentId="CustomerContacts.InviteUser" />
    <ad:AuxLinkButton ID="AuxLinkButton2" runat="server" OnClick="OnClickSimulateUser"
        Visible="<%#Utl.HasGlobalRole(GlobalRoleEnum.GlobalAdmin) %>" AutoBind="true"
        ContentId="CustomerContacts.SimulateUser" />
    <ad:AuxLinkButton ID="AuxLinkButton3" runat="server" OnClick="OnClickChangePassword"
        Visible="<%#Utl.HasGlobalRole(GlobalRoleEnum.GlobalAdmin) %>" AutoBind="true"
        ContentId="CustomerContacts.ChangePassword" />
</asp:Content>
