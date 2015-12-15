<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master"
    AutoEventWireup="true" CodeBehind="EditProfileTypes.aspx.cs" Inherits="Cvm.Web.AdminPages.EditProfileTypes" %>

<%@ Register TagPrefix="my" TagName="ListFilterCtrl" Src="~/AdminPages/CommonCtrl/ListFilterCtrl.ascx" %>
<%@ Import Namespace="Cvm.Backend.Business.Skills" %>
<%@ Register TagPrefix="a" Namespace="Napp.Web.WebForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
    <my:ListFilterCtrl runat="server" ID="MyList" ObjectType="ProfileType" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
    <% if (this.HasSelectedProfileType())
       {%>
    <table>
        <tr>
            <td>
                <auto2:AutoFormExt2 runat="server" ID="ProfileTypeForm" MinimumWidth="250" IncludeDeleteLink="false"/>
                <div class="buttons">
                    <ad:AdmButton ID="AdmButton1" ContentId="Standard.Save" runat="server" CssClass="saveBtn"
                        OnClick="OnClickSaveForm" />
                    <ad:AdmButton ID="AdmButton2" ContentId="Standard.Cancel" runat="server" CssClass="cancelBtn" />
                </div>
            </td>
            <td style="padding-left:20px;">
                <h2>
                    <%=Utl.Content("EditProfileTypes.RelatedSkills")%></h2>
                <div class="subformDiv">
                <%=Utl.Content("EditProfileTypes.EnterNew") %>
                    <ad:AdmTextBox runat="server" ID="AddSkillTextBox" OnTextChanged="OnChangeAddSkill"
                        CausesValidation="false" autocallback="globalService" webService="GetSkills" />
                </div>
                <table class="niceTable" width="100%">
                    <asp:Repeater runat="server" ID="SkillRepeater">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#((ISkill)Container.DataItem).SkillName %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
    </table>
    <%
        }%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightPane" runat="server">
</asp:Content>
