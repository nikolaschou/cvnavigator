<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAssignmentsAuxCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.EditAssignmentsAuxCtrl" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<div>
<ad:AuxHyperLink runat="server" AutoBind="false" Text="<%$Content:EditAssignmentsAuxCtrl.LinkToUser %>" ID="LinkToUser" Visible="<%#this.MyAssignment.HasAssociatedSysUser()%>"  /> 
<ad:AuxLinkButton runat="server" AutoBind="false" Text="<%$Content:EditAssignmentsAuxCtrl.CreateUser %>" ID="CreateUser" Visible="<%#!this.MyAssignment.HasAssociatedSysUser()%>" OnClick="OnClickCreateUserBtn" />
</div>