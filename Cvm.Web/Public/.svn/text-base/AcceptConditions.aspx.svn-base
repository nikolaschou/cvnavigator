<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="AcceptConditions.aspx.cs" Inherits="Cvm.Web.Public.AcceptConditions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Import Namespace="Cvm.Web.Facade" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link rel="stylesheet" href="../../css/cvnstyle.css" type="text/css" id="style" runat="server" visible="false" />
    <asp:HiddenField ID="hidForModel" runat="server" />

    <asp:ModalPopupExtender ID="Modalpop" runat="server"
        BehaviorID="modalPopupExtender1"
        TargetControlID="hidForModel"
        PopupControlID="popUpPanel"
        OkControlID="btnOk"
        BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>

    <asp:Panel ID="popUpPanel" runat="server" Width="300px" CssClass="popupPanel">
        <blockquote>
            <%=Utl.Content("AcceptConditions.MustAccept")%>
        </blockquote>
        <div class="buttons">
            <asp:Button ID="btnOk" runat="server" Text="Continue" CssClass="saveBtn" />
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="Panel1">
        <div class="activeLinks">
            <blockquote>
                <div id="cond_uk">
                    <%=Utl.Content("AcceptedConditions.Conditions.uk", "CVNavigator ApS")%>
                </div>
            </blockquote>
        </div>
        <br />
        <asp:CheckBox runat="server" Text="<%$Content:AcceptedConditions.CheckBoxText %>" ID="AcceptedCheckbox" /><br />
        <asp:CheckBox runat="server" Text="<%$Content:AcceptedConditions.ReceiveNewsLetter%>" ID="ChkBoxReceiveNewsLetter" /><br />
        <br />
        <div class="buttons">
            <asp:Button runat="server" Text="<%$Content:AcceptedConditions.Continue %>" OnClick="OnClickContinueBtn" />
        </div>

    </asp:Panel>
    <ad:AdmLinkButton ID="ContinueBtn" runat="server" Text="<%$Content:AcceptConditions.PassThrough %>" Visible="false" OnClick="OnClickPassThroughBtn" />
</asp:Content>
