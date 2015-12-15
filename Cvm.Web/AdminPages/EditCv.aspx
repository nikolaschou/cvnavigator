<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="EditCv.aspx.cs" Inherits="Cvm.Web.AdminPages.EditCv" Title="Untitled Page" %>

<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Cvm.Web.Code" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Import Namespace="Napp.Web.Navigation" %>

<%@ Register Assembly="Napp.Web.AdmControl" Namespace="Napp.Web.AdmControl" TagPrefix="cc2" %>
<%@ Register TagPrefix="Ex" Namespace="Napp.Web.AdmControl" Assembly="Napp.Web.AdmControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="../../css/cvnstyle.css" type="text/css" id="style" runat="server" visible="false" />

    <asp:HiddenField ID="hidForModel" runat="server" />

    <asp:ModalPopupExtender ID="MPEUserInfo" runat="server"
        BehaviorID="mpeUserInfo"
        TargetControlID="hidForModel"
        PopupControlID="popUpPanelUserInfo"
        OkControlID="btnOk"
        BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>

    <asp:Panel ID="popUpPanelUserInfo" runat="server" BackColor="White" Height="100px" Width="300px" BorderWidth="2px" BorderStyle="Solid">
        <blockquote>
            <asp:Label ID="LblPopup" runat="server" Text=""></asp:Label>
        </blockquote>
        <div class="buttons">
            <asp:Button ID="btnOk" runat="server" Text="Continue" CssClass="saveBtn" />
        </div>
    </asp:Panel>

    <div class="activeLinks">
        <div style="text-align: left">
            <asp:Panel ID="TopPanel" runat="server">
                <img src="../images/master/previous.png" style='position: relative; top: 2px; display: <%=(Utl.HasSysRole(RoleSet.SalesMgrAtLeast) && this.GetPreviousResourceId()>0?"inline":"none") %>' alt="Previous resource" title="<%=Utl.Content("EditCv.Previous") %>" onclick="javascript:location.href='<%=PageNavigation.GetCurrentLink().IncludeExistingParms().SetParm(QueryParmCvm.id,this.GetPreviousResourceId()).GetLinkAsHref()%>'" />
                <Ex:AdmDropDown EnableViewState="true" runat="server" Visible="<%#Utl.HasSysRole(RoleSet.SalesMgrAtLeast) %>" AutoSort="true" AutoActivate="false" AutoPostBack="true" ID="ResourceDropDown" ContentId="EditCv.SelectResource" OnSelectedIndexChanged="OnSelectResource" />

                <img src="../images/master/next.png" style='position: relative; top: 2px; display: <%=(Utl.HasSysRole(RoleSet.SalesMgrAtLeast) && this.GetNextResourceId()>0?"inline":"none") %>' alt="Previous resource" title="<%=Utl.Content("EditCv.Previous") %>" onclick="javascript:location.href='<%=PageNavigation.GetCurrentLink().IncludeExistingParms().SetParm(QueryParmCvm.id,this.GetNextResourceId()).GetLinkAsHref()%>'" />

                <ad:AdmHyperLink  ID="EditPrintLink"        runat="server" PageLink="<%#CvmPages.PrintCvPage.IncludeExistingParms() %>"                 ContentId="EditCv.PreviewPrint" />
                <ad:AdmLinkButton ID="CreateResourceButton" runat="server" OnClick="OnClickCreateResourceUser"                                          ContentId="EditCv.CreateResourceUser"   AutoBind="true" Visible="<%#Utl.HasSysRole(RoleSet.SysAdminAtLeast) && this.req.Current.UserId==null %>" />
                <ad:AdmHyperLink  ID="LinkToUserButton"     runat="server" PageLink="<%#CvmPages.UserAdminLink((int?)this.req.Current.UserId ?? 0) %>"  ContentId="EditCv.LinkToUser"           AutoBind="true" Visible="false" />
                <ad:AdmLinkButton ID="AdmLinkButton2"       runat="server" OnClick="OnClickInviteUser"                                                  ContentId="EditCv.InviteUser"           AutoBind="true" Visible="<%#Utl.HasSysRole(RoleSet.SalesMgrAtLeast) %>" />
                <ad:AdmLinkButton ID="AdmLinkButton1"       runat="server" OnClick="OnClickSimulateUser"                                                ContentId="EditCv.SimulateUser"         AutoBind="true" Visible="<%#Utl.HasGlobalRole(GlobalRoleEnum.GlobalAdmin) %>" />
            </asp:Panel>
            <br />

            <asp:Table runat="server" BorderColor="Gray" BorderWidth="0" CellPadding="0" CellSpacing="1">
                <asp:TableRow ID="Tabs" runat="server">
                </asp:TableRow>
                <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell ColumnSpan="16" CssClass="tdclass1">
                        <table border="0">
                            <tr>
                                &nbsp;
                                <td valign="top">
                                    <ad:AdmValidationArea runat="server" />
                                    <div style="padding: 10px"><%=Utl.ContentHlpOrBlank("EditCv.Intro" + this.GetTabIndex().ToString()) %></div>
                                    <div id="editCvPanel">
                                        <asp:Panel runat="server" ID="MainPanel">
                                        </asp:Panel>
                                    </div>
                                    <div class="buttons">
                                        <asp:Button ID="SaveBtn" runat="server" CausesValidation="true" Text="Save" CssClass="saveBtn" OnClick="OnClick_SaveBtn" />
                                        <asp:Button ID="CancelBtn" runat="server" CausesValidation="false" Text="Cancel" CssClass="cancelBtn" OnClick="OnClick_CancelBtn" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

        </div>
    </div>
</asp:Content>
