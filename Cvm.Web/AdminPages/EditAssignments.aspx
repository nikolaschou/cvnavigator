<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="True"
    CodeBehind="EditAssignments.aspx.cs" Inherits="Cvm.Web.AdminPages.EditAssignments"
    Title="Untitled Page" %>

<%@ Register TagPrefix="my" TagName="ListFilterCtrl" Src="~/AdminPages/CommonCtrl/ListFilterCtrl.ascx" %>
<%@ Register TagPrefix="my" TagName="TabularCtrl" Src="~/AdminPages/CommonCtrl/TabularCtrl.ascx" %>
<%@ Register TagPrefix="my" TagName="EditAssignmentsAuxCtrl" Src="~/AdminPages/CommonCtrl/EditAssignmentsAuxCtrl.ascx" %>
<%@ Register Src="~/AdminPages/WebFormCtrls/DropDownOrNewCtrl.ascx" TagPrefix="uc" TagName="DropDownOrNewCtrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
    <my:ListFilterCtrl runat="server" ObjectType="Assignment" ID="ListFilterCtrl1" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
    <div class="tabDiv" style='display: <%=(this.hasIdParm || this.isModeNew ? "block" : "hidden")%>'>
        <my:TabularCtrl runat="server" ID="TabularCtrl1" />
            <div class="tabDivInner autoformDiv">
                <table>
                    <auto:AutoForm1 SkipHeader="true" runat="server" ID="AssignmentForm"/>
                    <ad:AdmDropDown TableMode="Row" ContentId="EditAssignments.ContactPerson" runat="server" AutoActivate="false" Required="false" IncludeBlank="true" ID="CustomerContactDropDown" />
                </table>
                <div class="buttons">
                    <ad:AdmButton ID="AdmButton1" CssClass="saveBtn" runat="server" CausesValidation="true" OnClick="OnClickSaveBtn" Text="<%$Content:EditAssignments.Save %>" />
                    <ad:AdmButton ID="AdmButton2" CssClass="cancelBtn" runat="server" CausesValidation="false" OnClick="OnClickCancelBtn" Text="<%$Content:EditAssignments.Cancel %>" />
                </div>
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightPane" runat="server">
    <my:EditAssignmentsAuxCtrl runat="server" ID="AuxCtrl2" />
</asp:Content>
