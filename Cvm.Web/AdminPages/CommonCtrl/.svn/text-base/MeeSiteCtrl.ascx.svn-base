<%@ Import Namespace="Napp.Web.AdminContentMgr" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeeSiteCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.MeeSiteCtrl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<link rel="stylesheet" href="../../css/cvnstyle.css" type="text/css" id="style" runat="server" visible="false" />

<asp:HiddenField ID="hidForModel" runat="server" />

<asp:ModalPopupExtender ID="MPENewsletter" runat="server"
    BehaviorID="mpeNewsletter"
    TargetControlID="hidForModel"
    PopupControlID="popUpPanelNewsletter"
    OkControlID="btnOk"
    BackgroundCssClass="modalBackground">
</asp:ModalPopupExtender>

<asp:ModalPopupExtender ID="MPEDeleteAccount" runat="server"
    BehaviorID="mpeDeleteAccount"
    TargetControlID="lkbtnDeleteAccount"
    PopupControlID="popupPanelDeleteAccount"
    OkControlID="btnOk"
    CancelControlID="btnCancel"
    BackgroundCssClass="modalBackground">
</asp:ModalPopupExtender>

<asp:Panel ID="popUpPanelNewsletter" runat="server" BackColor="White" Height="100px" Width="300px" BorderWidth="2px" BorderStyle="Solid">
    <blockquote>
        <asp:Label ID="LblPopup" runat="server" Text=""></asp:Label>
    </blockquote>
    <div class="buttons">
        <asp:Button ID="btnOk" runat="server" Text="Continue" CssClass="saveBtn" />
    </div>
</asp:Panel>

<asp:Panel ID="popupPanelDeleteAccount" runat="server" Height="100px" Width="300px" CssClass="popupPanel" >
    <blockquote>
        <asp:Literal ID="Literal2" runat="server" Text="<%$Content:EditCv.AccountWillBeDeleted%>" />
    </blockquote>
    <div class="buttons">
        <asp:Button ID="btnOkDelete" runat="server" Text="Ok" CssClass="saveBtn" OnClick="btnOkDelete_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancelBtn" />
    </div>
</asp:Panel>
<div>
    <table border="0">
        <tr>
            <td>
                <asp:Literal ID="litReceiveNewsletter" runat="server" Text="<%$Content:EditCv.ReceiveNewsletter %>" Visible="true" />
            </td>
            <td>
                <asp:CheckBox runat="server" Text="" ID="NewsLetterCheckbox" OnCheckedChanged="NewsLetterCheckbox_CheckedChanged" AutoPostBack="True" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Content:EditCv.SiteLanguage %>" Visible="true" />
            </td>
            <td>
                <asp:DropDownList ID="ddlUserLanguage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlUserLanguage_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <br />
                <br />
                <asp:LinkButton ID="lkbtnDeleteAccount" Text="<%$Content:EditCv.DeleteMyAccount %>" runat="server" />
            </td>
            <td></td>
        </tr>
    </table>
</div>
