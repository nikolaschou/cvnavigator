<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true"
    CodeBehind="PrintPrepare.aspx.cs" Inherits="Cvm.Web.AdminPages.PrintPrepare"
    Title="Untitled Page" %>

<%@ Register TagPrefix="ad" Namespace="Napp.Web.ExtControls" Assembly="Napp.Web.ExtControls" %>
<%@ Import Namespace="Cvm.Backend.Business.Meta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel Visible="true" runat="server" ID="Panel1">
        <asp:Literal ID="Literal1" runat="server" Text="<%$Content:PrintCv.Instruction %>" />
        <br />
        <div class="autoformOuterDiv">
            <auto2:AutoFormExt2 runat="server" ID="AutoForm" IncludeDeleteLink="false" OmitProperties="CustomerId"
                RenderMode="Form" />
            <table>
                <tr>
                    <td style="font-weight: bolder">
                        <%=Utl.ContentHlp("PrintPrepare.EmphasizeCustomer") %>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <ad:AdmDropDown runat="server" ID="CustomerDropDown3" IncludeBlank="True" AutoActivate="False">
                        </ad:AdmDropDown>
                    </td>
                </tr>
            </table>
        </div>
        <ad:AdmButton runat="server" ContentId="PrintCv.Print" OnClick="OnClick_PrintBtn" />
    </asp:Panel>
</asp:Content>
