<%@ Import namespace="Napp.Web.AdminContentMgr"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EditObjectCtrl.ascx.cs"
    Inherits="Cvm.Web.CommonCtrl.EditObjectCtrl" EnableViewState="true" %>
<%@ Register Assembly="Napp.Web.AutoForm" TagPrefix="Uc" Namespace="Napp.Web.AutoForm" %>
<%@ Register TagPrefix="Ex" Namespace="Napp.Web.AdmControl" Assembly="Napp.Web.AdmControl" %>
<%@ Register TagPrefix="Auto" Namespace="Napp.Web.AutoFormExt" Assembly="Napp.Web.AutoFormExt" %>
<%@ Register Src="~/CommonCtrl/AutoFormExt.ascx" TagPrefix="Uc" TagName="AutoFormExt" %>
<div>
    <Auto2:AutoFormExt2 ID="AutoForm2" runat="server" MinimumWidth="580"/>
    <asp:MultiView runat="server" ID="Panels" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            
            <asp:Panel ID="BtnPanel" runat="server" Visible="false">
                <Ex:AdmButton ID="SaveBtn" CausesValidation="true" runat="server" ContentId="EditObjectCtrl.Save" OnClick="OnClickSaveBtn" />
                <Ex:AdmButton ID="CancelBtn" runat="server" CausesValidation="false" ContentId="EditObjectCtrl.Cancel" OnClick="OnClickCancelBtn" />
                <Ex:AdmButton ID="DeleteBtn" Visible="false" runat="server" ContentId="EditObjectCtrl.Delete"
                    OnClick="OnClickDeleteBtn" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="View2" runat="server">
        <div class="messages"><%=AdminContentMgr.instance.GetContent("EditObjectCtrl.FoundValidationForcable") %></div>
            <Ex:AdmButton ID="ForceYesBtn" runat="server" ContentId="EditObjectCtrl.ForceYes" 
                OnClick="OnClickForceYesBtn" />            
            <Ex:AdmButton ID="ForceNoBtn" runat="server" ContentId="EditObjectCtrl.ForceNo" 
                OnClick="OnClickForceNoBtn" />
        </asp:View>
    </asp:MultiView>
</div>
