<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.WebFormCtrls.ImageCtrl" %>
<asp:Image runat="server" ID="Image1" />
<asp:FileUpload runat="server" ID="FileUpload1" />
<ad:AuxLinkButton CausesValidation="false" runat="server" Text="<%$Content:ImageCtrl.Clear %>" OnClick="OnClickClearBtn" />
</br>
