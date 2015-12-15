<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="true" CodeBehind="EditSysOwner.aspx.cs" Inherits="Cvm.Web.AdminPages.EditSysOwner" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
<auto:AutoForm1 runat="server" ID="SysRootForm"/>
<auto:AutoForm1 runat="server" ID="SysOwnerForm"/>
<div class="buttons">
<ad:AdmButton ID="AdmButton1" Visible="true" runat="server" Text="<%$Content:Standard.Save %>" CssClass="saveBtn" OnClick="OnClickSaveBtn" />
<ad:AdmButton ID="AdmButton2" runat="server" Text="<%$Content:Standard.Cancel %>" CssClass="cancelBtn" OnClick="OnClickCancelBtn" />
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightPane" runat="server">
</asp:Content>
