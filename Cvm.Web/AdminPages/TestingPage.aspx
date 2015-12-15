<%@ Page EnableViewState="true" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="TestingPage.aspx.cs" Inherits="Cvm.Web.AdminPages.TestingPage" Title="Untitled Page" %>
<%@ Register Src="~/AdminPages/CommonCtrl/MessageValidationCtrl.ascx" TagPrefix="uc" TagName="MessageValidationCtrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="OtherTextBox" >Specify value</asp:RequiredFieldValidator>
<asp:TextBox runat="server" ID="OtherTextBox"></asp:TextBox>

<asp:Button ID="Button1" runat="server" Text="Normal button" />

<asp:Button runat="server" ID="TestBtn" Text="Start dialog" CausesValidation="false" OnClick="OnClick_TestBtn"/>
<asp:Panel runat="server" Visible="false">
<a href="#" class="popupContainerOpener">Open</a>
<div class="popupContainer">

<asp:UpdatePanel runat="server" RenderMode="Block" ChildrenAsTriggers="true" UpdateMode="Always">
<ContentTemplate>
<ad:ContainerCtrl runat="server" Title="Testing">
<uc:MessageValidationCtrl runat="server" />

<asp:TextBox runat="server" ID="TestTxtInput" />
<ad:AdmTextBox ID="AdmBox" runat="server" Required="true" />
<br /><br /><br /><br /><br /><br />
</ad:ContainerCtrl>
</ContentTemplate>
</asp:UpdatePanel>

</div>


<div style="height:1000px"></div>
<a href="#" class="jqModal">view</a>
...
<div class="jqmWindow" id="dialog">
<ad:ContainerCtrl ID="ContainerCtrl1" runat="server">

</ad:ContainerCtrl>
</div>
</asp:Panel>

<asp:Panel runat="server" ID="checkBoxPanel">
<asp:CheckBox OnCheckedChanged="OnCheckBoxChange" ID="CheckBox1" runat="server" />
<asp:RadioButton  runat="server"  ID="radio1" />
<asp:TextBox runat="server" ID="TextBox3" />
<asp:DropDownList runat="server" ID="MyDropDown">
<asp:ListItem Value="1" Text="1" />
<asp:ListItem Value="2" Text="2" />
<asp:ListItem Value="3" Text="3" />
</asp:DropDownList>

<asp:ListBox ID="listbox1" runat="server" SelectionMode="Multiple">

<asp:ListItem Value="1" Text="1" />
<asp:ListItem Value="2" Text="2" />
<asp:ListItem Value="3" Text="3" />
</asp:ListBox>
</asp:Panel> 
<asp:Button ID="Button2" runat="server" Text="Switch" OnClick="OnClickSwitchBtn" />
</asp:Content>
