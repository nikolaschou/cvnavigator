<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownOrNewCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.WebFormCtrls.DropDownOrNewCtrl" %>
<div>
<ad:AdmDropDown ID="ObjectDropDown" runat="server" AutoActivate="false"/> 
<ad:AdmLinkButton CssClass="subDialogButton"  runat="server" CausesValidation="true" ContentId="ChooseCustomerCtrl.CreateNew" OnClick="OnClick_CreateNewBtn"/>
</div>