<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EditLanguageSkillsCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.EditLanguageSkillsCtrl" %>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<div>
    <table border="0">
        <tr>
            <td>
                <%=AdminContentMgr.instance.GetContent("EditLanguageSkillsCtrl.MotherTounge") %>
            </td>
            <td>
                <ad:AdmDropDown IncludeBlank="true" runat="server" AutoPostBack="true" AutoActivate="true" EntityName="Language" ID="MotherToungeDropDown" OnSelectedIndexChanged="OnMotherTongueChosen" />
            </td>
        </tr>
        <tr>
            <td>
                <%=AdminContentMgr.instance.GetContent("EditLanguageSkillsCtrl.SecondLanguage") %>
            </td>
            <td>
                <ad:AdmDropDown IncludeBlank="true" runat="server" AutoPostBack="true" AutoActivate="false" Required="false" ID="SecondLangDropDown" OnSelectedIndexChanged="OnSecondLanguageChosen" />
            </td>
        </tr>
    </table>
    <auto:AutoList OmitProperties="LanguageSkillId;ResourceId" runat="server" ID="AutoList1" EditMode="Edit" />    
</div>

<asp:Button ID="btnUpdateSecondLanguageSkills" runat="server" Text="Button" OnClick="btnUpdateSecondLanguageSkills_Click" style="display:none"/>

<script>
    $(document).ready(function () {
        $("select").change(function () {
            $("#<%=btnUpdateSecondLanguageSkills.ClientID%>").trigger('click');
        });
    });
</script>