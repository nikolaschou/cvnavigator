<%@ Import namespace="Napp.Backend.Contents"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentEditorCtrl.ascx.cs" Inherits="Nshop.Web.AdminPages.WebFormCtrls.ContentEditorCtrl" %>
<%@ Register TagPrefix="Uc" Namespace="Nshop.Web.AdminPages.WebFormCtrls" Assembly="Cvm.Web" %>
<table border="0">
<asp:Repeater runat="server" ID="Rep1">
<ItemTemplate>
<tr>

<td>
<Uc:ContentFieldTextBox runat="server" 
Text="<%#MyContentField[((ILanguage)Container.DataItem)]%>"
TextMode="<%#GetTextMode() %>" 
Rows="<%#this.GetNoRowsForField() %>" 
Columns="<%#this.GetNoColumnsForField() %>"
Language="<%#(ILanguage)Container.DataItem %>"
ContentField="<%#MyContentField %>"
/>
</td>
<td valign="top" align="right">
<%#((ILanguage)Container.DataItem).LanguageCode %>
</td>
</tr>
</ItemTemplate>
</asp:Repeater>
</table>