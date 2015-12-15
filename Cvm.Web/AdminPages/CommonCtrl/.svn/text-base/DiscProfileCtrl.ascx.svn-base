<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscProfileCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.DiscProfileCtrl" %>
<%@ Register TagPrefix="my" TagName="DiscProfileResultCtrl" Src="~/AdminPages/CommonCtrl/DiscProfileResultCtrl.ascx" %>
<asp:Panel runat="server" ID="Panel1">
    <ad:EnumDropDown2 runat="server" ID="LanguageDropDown" />
    <ad:AdmButton ID="StartWizardBtn" runat="server" CssClass="saveBtn" OnClick="OnClickStartWizard"
        ContentId="DiscProfileCtr.StartWizard" />
    <iframe runat="server" style="display: none" id="MyFrame" src="" scrolling="no" height="600"
        width="800"></iframe>
    <ad:AdmButton ID="CancelWizardBtn" Visible="false" runat="server" CssClass="cancelBtn"
        OnClick="OnClickCancelWizard" ContentId="DiscProfileCtr.CancelWizard" />
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Visible="false"> 
    <my:DiscProfileResultCtrl runat="server" ID="DiscProfileResultCtrl1"/>
    <ad:AdmButton ID="AdmButton1"  runat="server" CssClass="cancelBtn"
        OnClick="OnClickRemoveDiscProfile" ContentId="DiscProfileCtr.RemoveDiscProfile" />
</asp:Panel>
<div id="resultPane" style="display: none">
    <asp:HiddenField ID="DiscProfileResult" runat="server" OnValueChanged="OnChangeDiscProfileResult" />
</div>
<script type="text/javascript">
    function onDone() {
        var url = window.frames[0].location.href;
        var queryString = url.substring(url.indexOf('?') + 1);
        $('#resultPane input').val(queryString);
        document.forms[0].submit();
    }  
</script>
