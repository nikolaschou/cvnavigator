<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="True"
    CodeBehind="Assignments.aspx.cs" Inherits="Cvm.Web.AdminPages.Assignments" Title="Untitled Page" %>

<%@ Register TagPrefix="my" TagName="ResourceList1Ctrl" Src="~/AdminPages/GridCtrl/ResourceList1Ctrl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="FullPanel">
        <asp:Literal runat="server" Text="<%$Content:Assignments.ChooseAssignment %>" Visible="<%#this.AssignmentList.Count>1 %>" />
        <asp:DropDownList runat="server" ID="AssignmentDropDown" DataTextField="StandardObjectTitle"
            DataValueField="AssignmentId" EnableViewState="true" Visible="<%#this.AssignmentList.Count>1 %>" />
        <br />
        <br />
        <h2><%#Utl.Content("Assignments.Title",CurrentAssignment.ExtendedObjectTitle) %></h2>
        <table border="0" cellpadding="3" class="stdTable">
        <tr>
        <td><%#Utl.ContentHlp("Assignments.StartBy")%></td><td><%#CurrentAssignment.ContractStartBy.FormatDate() %></td>
        <tr>
        <td><%#Utl.ContentHlp("Assignments.Description")%></td><td><i><%#CurrentAssignment.AssignmentDescription %></i></td>
        </tr>
        </tr>
        </table>
        <auto:AutoForm1 runat="server" ObjectSourceInstance="<%#CurrentAssignment %>" AutoBind="true"
            EditMode="View" />
        <br />
        <%#Utl.ContentHlp("Assignments.ResourceListIntro") %>
        <br />
        <my:ResourceList1Ctrl runat="server" ID="ResourceList" EnableViewState="false"/>
    </asp:Panel>
</asp:Content>
