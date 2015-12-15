<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteObjectCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.DeleteObjectCtrl" %>
<%@ Register Assembly="Napp.Backend.AutoDeleteForm" Namespace="Napp.Backend.AutoDeleteForm" TagPrefix="auto" %>

<div>
    <auto:AutoDeleteFormCtrl runat="server" ID="AutoDeleteCtrl" />
    <asp:Panel runat="server" ID="RefactorPanel" Visible="false">
    <%=Utl.ContentHlp("DeleteObjectCtrl.Refactor") %> 
    <br />
    <ad:AdmDropDown AutoActivate="false" runat="server" ID="RefactorDropDown" />
    <asp:Button runat="server" Text="<%$Content:DeleteObjectCtrl.RefactorBtn %>" OnClick="OnClickRefactorBtn" />
    </asp:Panel>
</div>