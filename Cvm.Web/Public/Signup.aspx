<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master"
    AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="Cvm.Web.Public.Signup" %>

<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Backend.Business.Multisite" %>
<%@ Register Src="~/AdminPages/CommonCtrl/ImportSkillsCtrl.ascx" TagPrefix="uc" TagName="ImportSkillsCtrl" %>
<%@ Register Src="~/AdminPages/CommonCtrl/ImportLinkedInCtrl.ascx" TagPrefix="uc"
    TagName="ImportLinkedInCtrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="MainPanel" Visible="true">
        <div class="activeLinks">
            <div style="vertical-align: top">
                <fieldset>
                    <legend>
                        <%=Utl.ContentHlpBread("Signup.SignupTo", ContextObjectHelper.GetSysNameFromExplicitSysCode()) %></legend>
                    <br />
                    <div id="emailDiv">
                        <label><%=Utl.ContentHlpBread("Signup.Email") %></label>
                        <ad:AdmTextBox FieldContentId="Signup.Email" runat="server" ID="EmailTB" ValidationType="Email" Required="True" />
                    </div>
                    <div id="firstNameDiv">
                        <label><%=Utl.ContentHlpBread("Signup.FirstName") %></label>
                        <ad:AdmTextBox FieldContentId="Signup.FirstName" runat="server" ID="FirstNameTB" Required="True" />
                    </div>
                    <div id="lastNameDiv">
                        <label><%=Utl.ContentHlpBread("Signup.LastName") %></label>
                        <ad:AdmTextBox FieldContentId="Signup.LastName" runat="server" ID="LastNameTB" Required="True" />
                    </div>
                    <div id="passwordDiv">
                        <label><%=Utl.ContentHlpBread("Signup.Password") %></label>
                        <ad:AdmTextBox FieldContentId="Signup.Password" runat="server" ID="PasswordTB" TextMode="Password" Required="True" />
                    </div>
                    <br />
                    <div id="docDiv">
                        <asp:Label runat="server" ID="LinkedInSignUpLabel"><%=Utl.ContentHlp("Signup.ImportLinkedIn") %></asp:Label>
                        <uc:ImportLinkedInCtrl runat="server" ID="ImportLinkedInCtrl" />
                        <br />
                        <asp:Label runat="server" ID="CvFileUploadLabel"><%=Utl.ContentHlpBread("Signup.Import") %></asp:Label>
                        <asp:FileUpload runat="server" ID="CvFileUpload" />
                        <br />
                    </div>
                    <div>
                        <br />
                        <asp:Label runat="server" ID="CaptchaLabel"> <%=Utl.ContentHlp("Signup.Captcha") %></asp:Label> <br />
                        <asp:Image ID="imgCaptcha" ImageUrl="~/public/Captcha.ashx" runat="server" /> <br />
                        <ad:AdmTextBox FieldContentId="Signup.CaptchaTextBox" runat="server" ID="AdmCaptchaTextBox" Required="True" />
                    </div>
                    <div>
                        <br />
                        <ad:AdmButton runat="server" OnClick="OnClickSubmitBtn" Text="<%$Content:Signup.Submit %>" />
                    </div>
                </fieldset>
                <br />
                <br />
                <a class="stdLink" href="#" onclick="javascript:$('#sendFriendDiv').dialog('open');"> <%=Utl.ContentHlpBread("Signup.SendToAFriend") %></a>
                <div class="dialog" id="sendFriendDiv">
                    <span class="activeLinks">
                        <%=Utl.ContentHlpBread("Signup.SendToAFriendMessage") %><br />
                        <asp:TextBox runat="server" ID="SendToFriendMessageTB" TextMode="MultiLine" /><br />
                        <%=Utl.ContentHlpBread("Signup.SendToAFriendEmail") %><br />
                        <ad:AdmTextBox FieldContentId="Signup.SendToFriendEmail" ValidationType="Email" runat="server" ID="SendToFriendTextBox" Required="False" /> <br />
                        <ad:AdmLinkButton ID="AdmButton1" runat="server" CausesValidation="false" OnClick="OnClickSendToFriendBtn" Text="<%$Content:Signup.SendToFriend %>" />
                    </span>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="SubPanel" Visible="false">
        <br />
        <%--<ad:AdmHyperLink runat="server" Text="<%$Content:Signup.SignupWithoutRelations %>" AutoBind="true" PageLink="<%#CvmPages.SignUp(SysCode.www.Value) %>" />--%>
        <ad:AdmLinkButton ID="AHLSignUpNoRelations" runat="server" Text="<%$Content:Signup.SignupWithoutRelations %>"  OnClick="AHLSignUpNoRelations_Click"/>
        <br />
        <br />
        <fieldset>
            <legend><%=Utl.ContentHlp("Signup.SignupUnderSite") %></legend>
            <table class="centeredTable">
                <tr>
                    <asp:Repeater runat="server" ID="SiteRepeater">
                        <ItemTemplate>
                            <td style="whitespace: nowrap">
                                <a href="<%#CvmPages.SignUp(((SysOwner)Container.DataItem).RelatedSysRoot.SysCode).GetLinkAsHref() %>"><%#((SysOwner)Container.DataItem).RelatedSysRoot.SysName%></a>
                            </td>
                            <%#(((Container.ItemIndex+1) % 2 == 0)?"</tr><tr>":"") %>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <script language="javascript">
        //A callback when linked in data has been received.
        function linkedInCallback(result) {
            user = result.values[0];

            //Insert name and photoe
            $('#firstNameDiv :text').val(user.firstName);
            $('#lastNameDiv :text').val(user.lastName);
            $('#docDiv').slideUp('slow');
        }
    </script>
</asp:Content>
