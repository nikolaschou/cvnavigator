<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCvImportItemCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.EditCvImportItemCtrl" %>
<tr>
<td style="border-top:1px solid gray;"><asp:Label runat="server" ID="StatusLabel" /></td>
<td style="border-top:1px solid gray;"><asp:Literal runat="server" ID="FileNameLabel" /></td>
<td style="border-top:1px solid gray;"><asp:CheckBox runat="server" ID="DoImportCheckBox"/></td>
<td style="border-top:1px solid gray;"><asp:TextBox runat="server" ID="FirstNameTextBox" /></td>
<td style="border-top:1px solid gray;"><asp:TextBox runat="server" ID="LastNameTextBox" /></td>
<td style="border-top:1px solid gray;"><asp:TextBox runat="server" ID="EmailTextBox" /></td>
</tr>
<tr>
<td colspan="2"></td>
<td colspan="6">
<asp:Literal runat="server" ID="MessageLiteral" />
<asp:Literal runat="server" ID="ResourceMessage" />
<asp:DropDownList runat="server" ID="ResourceDropDown" DataTextField="StandardObjectTitle" DataValueField="Id" Visible="false"/>

</td>
</tr>
