<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="NewSite.aspx.cs" Inherits="Cvm.Web.Public.NewSite" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ad:ContainerCtrl ContentId="NewSite.RootInfo" runat="server">
<auto:AutoForm1 OmitProperties="LastModifiedTs" ID="AutoForm1" runat="server" ObjectSourceInstance="<%#this.SysRoot.Current %>" />
</ad:ContainerCtrl>
<br />
<ad:ContainerCtrl ID="ContainerCtrl1" ContentId="NewSite.OwnerInfo" runat="server" Visible="false">
<auto:AutoForm1 OmitProperties="LastModifiedTs;SysOwnerContactEmail" ID="AutoForm2" runat="server" ObjectSourceInstance="<%#this.SysOwner.Current %>" />
</ad:ContainerCtrl>
<br />
<ad:ContainerCtrl ID="ContainerCtrl2" ContentId="NewSite.AdminUser" runat="server">
<auto:AutoForm1 OmitProperties="LastModifiedTs" ID="AutoForm3" runat="server" AutoBind="true" IncludeOnlyProperties="Password;PasswordConfirmed;Email" ObjectSourceInstance="<%#this.UserObj.Current %>" />
</ad:ContainerCtrl>
<br />
<div class="buttons">
    <ad:AdmButton runat="server" ContentId="NewSite.OkBtn" OnClick="OnClickOkBtn" />
</div>
</asp:Content>
