<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCvImportTextCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.EditCvImportTextCtrl" %>
<div>
<asp:TextBox TextMode="MultiLine" Columns="100" Rows="20" runat="server" ID="ImportTextBox" AutoPostBack="true" OnTextChanged="OnTextChangedImportTextBox"/>
</div>