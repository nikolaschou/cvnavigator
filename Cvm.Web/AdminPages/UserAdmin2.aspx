<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="True" CodeBehind="UserAdmin2.aspx.cs" Inherits="Cvm.Web.AdminPages.UserAdmin2" Title="Untitled Page" %>
<%@ Import Namespace="Cvm.Backend.Business.DataAccess" %>
<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Napp.Backend.Hibernate" %>
<%@ Import namespace="Napp.Web.AdminContentMgr"%>
<%@ Import namespace="Cvm.Web.Navigation"%>
<%@ Import namespace="Napp.Web.Navigation"%>
<%@ Import Namespace="NHibernate.Linq" %>
<%@Register Src="~/CommonCtrl/EditUserCtrl.ascx" TagPrefix="uc" TagName="EditUserCtrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
<h2><%=AdminContentMgr.instance.GetContent("UserAdmin.ExistingUsers") %></h2><br />
<ad:AdmHyperLink ID="CreatenewLink" runat="server" PageLink="<%#PageNavigation.GetCurrentLink().SetMode(PageMode.New)%>" ContentId="UserAdmin2.CreateNew"/>
<br /><br />
<asp:Repeater runat="server" ID="UserList" DataSource="<%#this.GetUsersForList() %>">
<ItemTemplate>
<ad:AdmHyperLink runat="server" PageLink="<%#PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.userId, ((SysUserObj)Container.DataItem).UserId)%>" Text="<%#this.GetUserNameForList((SysUserObj)Container.DataItem)%>"/>
<br />
</ItemTemplate>
</asp:Repeater>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
<asp:Panel runat="server" ID="MainPanel">
<table border="0" width="800">
<tr>
<td >
<h2><%=Utl.ContentHlp("UserAdmin.UserData") %></h2>
<table border="0" >
<tr><td><%=Utl.ContentHlp("UserAdmin.UserName") %></td>
<td style="white-space:nowrap"><ad:AdmTextBox runat="server" ValidationType="UserName" FieldContentId="UserObj.UserName" Required="true" ID="UserNameTextBox"/><asp:Literal runat="server" ID="UserNameSuffixLit"/></td>
</tr>
<auto:AutoForm1 runat="server" ID="NewUserForm" SkipHeader="true" OmitProperties="PartialUserName"/>
<auto:AutoForm1 runat="server" ID="UpdateMembershipUserForm" SkipHeader="true" OmitProperties="IsLockedOut;IsApproved;IsOnline;UserName;PasswordQuestion;ProviderName;LastLockoutDate;CreationDate;LastLoginDate;LastActivityDate;LastPasswordChangedDate;"  AutoBind="false"/>
<auto:AutoForm1 runat="server" ID="UpdateUserObjForm" SkipHeader="true"  OmitProperties="UserName;LastModifiedTs" AutoBind="false"/>
<auto:AutoForm1 runat="server" ID="SysUserObjForm" SkipHeader="true" AutoBind="false" OmitProperties="LastModifiedTs"/>
</table>
<br />
<asp:Button ID="Button1" runat="server" Text="<%$Content:UserAdmin.Save %>" OnClick="OnClickSaveBtn"/>
<asp:Button ID="Button2" CausesValidation="false" runat="server" Text="<%$Content:UserAdmin.Cancel %>" OnClick="OnClickCancelBtn"/>
</td>
<td align="right" valign="top" style="border-left:5px solid #cccccc;padding-left:10px;">
<h2><%=Utl.ContentHlp("Standard.OtherActions") %></h2>
<ad:AdmLinkButton ID="AdmLinkButton1" runat="server" ContentId="UserAdmin.DeleteUser" AutoBind="true" OnClick="OnClickDeleteUserLink" Confirm='<%#Utl.Content("UserAdmin.ConfirmDelete",this.MyUserObj.UserName) %>' Visible="<%#this.IsUpdateMode() %>"/><br />
<ad:AdmLinkButton ID="AdmLinkButton3" runat="server" ContentId="UserAdmin.SimulateUser" AutoBind="true" OnClick="OnClickSimulateUserLink" Visible="<%#this.IsUpdateMode() && Utl.HasSysRole(SysRoleEnum.SysAdmin) %>"/><br />
<ad:AdmLinkButton ID="AdmLinkButton4" runat="server" ContentId="UserAdmin.InviteUserByEmail" AutoBind="true" OnClick="OnClickInviteUserByEmailLink" Visible="<%#this.IsUpdateMode()%>"/><br />
<ad:AdmLinkButton ID="AdmLinkButton2" runat="server" ContentId="UserAdmin.UnlockUser" AutoBind="true" OnClick="OnClickUnlockUserLink" Visible="<%#this.IsUpdateMode() && MyMembershipUser!=null && this.MyMembershipUser.IsLockedOut %>"/><br />
<ad:AdmLinkButton ID="AdmLinkButton5" runat="server" ContentId="UserAdmin.SetPassword" AutoBind="true" OnClick="OnClickSetPasswordLink"  Visible="<%#this.IsUpdateMode() && Utl.HasSysRole(SysRoleEnum.SysAdmin) %>"/><br />
</td>
</tr>
</table>
</asp:Panel>
<ad:AdmDialog ID="AdmDialog1" runat="server" Title="<%$Content:UserAdmin.ChangePassword %>">

<fieldset> 
<ad:AdmValidationArea runat="server" />
<%=Utl.ContentHlpBread("UserAdmin.EnterNewPassword") %><asp:TextBox runat="server" ID="Password1TextBox" TextMode="Password" />
<%=Utl.ContentHlpBread("UserAdmin.RepeatNewPassword") %><asp:TextBox runat="server" ID="Password2TextBox" TextMode="Password" />
<asp:Button CausesValidation="false" runat="server" OnClick="OnClickSetPasswordBtn" Text="<%$Content:UserAdmin.ChangePasswordBtn %>"/>

</fieldset>
</ad:AdmDialog>
</asp:Content>