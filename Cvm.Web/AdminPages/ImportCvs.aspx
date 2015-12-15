<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="True"
    CodeBehind="ImportCvs.aspx.cs" Inherits="Cvm.Web.AdminPages.ImportCvs" Title="Untitled Page" %>

<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="progressBar">
        <asp:Repeater runat="server" ID="ProgressBarRep" EnableViewState="false" DataSource="<%#this.Wizard1.WizardSteps %>">
            <ItemTemplate>
                <div class='<%#(Container.ItemIndex==Wizard1.ActiveStepIndex?"selected":"") %>'>
                    <%#Utl.ContentHlp("ImportCvs.Step"+ Container.ItemIndex) %>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Wizard runat="server" ID="Wizard1" OnFinishButtonClick="OnComplete" FinishCompleteButtonText="<%$Content:ImportCvs.FinishBtn %>"
        StepNextButtonText="<%$Content:ImportCvs.NextBtn %>" StepPreviousButtonText="<%$Content:ImportCvs.PreviousBtn %>"
        SideBarStyle-CssClass="wizardSideBarCss" DisplaySideBar="false">
        <WizardSteps>
            <asp:WizardStep AllowReturn="false" Title="<%$Content:ImportCvs.Step1 %>">
                <%=AdminContentMgr.instance.GetContent("ImportCvs.ChooseArchiveIntro")%>
                <table border="0">
                    <tr>
                        <td>
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.ChooseArchiveCell")%>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ArchiveDropDown" OnSelectedIndexChanged="OnChangedArchiveSelect" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.UploadNewArchive")%>
                        </td>
                        <td>
                            <asp:FileUpload runat="server" ID="FileUpload1" />
                            <asp:Button ID="Button1" runat="server" Text="<%$Content:ImportCvs.UploadArchive %>"
                                OnClick="OnClickUploadArchiveBtn" />
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep AllowReturn="false" Title="<%$Content:ImportCvs.Step2 %>" OnActivate="OnActivateRules">
                <table border="0">
                    <tr>
                        <td valign="top">
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.ChooseRulesIntro")%>
                        </td>
                        <td valign="top" style="width: 420px">
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.IgnoreRulesHeader")%><br />
                            <asp:PlaceHolder runat="server" ID="RulePlaceHolder" />
                        </td>
                    </tr>
                </table>
                <div style="overflow: auto; height: 400px;">
                    <table border="0">
                        <tr>
                            <td>
                                <%=AdminContentMgr.instance.GetContent("ImportCvs.FileNamesTextBox")%>
                            </td>
                            <td>
                                <%=AdminContentMgr.instance.GetContent("ImportCvs.ResourceNamesTextBox")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox Width="450" TextMode="MultiLine" Enabled="false" runat="server" ID="FileNamesTextBox" />
                            </td>
                            <td>
                                <asp:TextBox Width="450" TextMode="MultiLine" Enabled="true" runat="server" ID="ResourceNamesTextBox" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:WizardStep>
            <asp:WizardStep Title="<%$Content:ImportCvs.Step3 %>" OnActivate="OnActivateEditImportDetails">
                <%=AdminContentMgr.instance.GetContent("ImportCvs.EditImportDetailsIntro")%>
                <br />
                <br />
                <asp:CheckBox runat="server" Checked="true" ID="HideCompletedCheckBox" Text="<%$Content:ImportCvs.HideCompletedImports %>"
                    AutoPostBack="true" />
                <table border="0">
                    <thead class="headerStyle">
                    <tr>
                        <td >
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.HeaderStatus") %>
                        </td>
                        <td >
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.HeaderOrigFile") %>
                        </td>
                        <td >
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.HeaderDoImport") %>
                        </td>
                        <td >
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.HeaderFirstName") %>
                        </td>
                        <td >
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.HeaderLastName") %>
                        </td>
                        <td >
                            <%=AdminContentMgr.instance.GetContent("ImportCvs.HeaderEmail") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!-- Status -->
                        </td>
                        <td>
                            <!-- Orig file -->
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="AllCheckBox" OnCheckedChanged="OnChangedAllCheckBox"
                                Text="<%$Content:ImportCvs.ChangeAll %>" AutoPostBack="true" />
                        </td>
                        <td>
                            <!-- First name -->
                        </td>
                        <td>
                            <!-- Last name -->
                        </td>
                        <td>
                            <!-- Email -->
                        </td>
                    </tr>
                    </thead>
                    <tbody class="inbox">
                    <asp:PlaceHolder runat="server" ID="ItemPlaceHolder" />
                    </tbody>
                </table>
                <div style="text-align: right">
                    <asp:Button ID="Button2" runat="server" OnClick="OnClickImportResources" Text="<%$Content:ImportCvs.ImportResources %>" />
                </div>
            </asp:WizardStep>
            <asp:WizardStep Title="<%$Content:ImportCvs.Step4 %>" OnActivate="OnActivateSetupEmails">
                <div style="display:none">
                <%=Utl.Content("ImportCvs.IgnoreEmails") %><br />
                <asp:TextBox Width="200" AutoPostBack="true" OnTextChanged="OnChangeIgnoreEmails"
                    Rows="3" TextMode="MultiLine" Enabled="true" runat="server" ID="EmailsIgnoreTextBox" />
                <span style="cursor: pointer">
                    <%=Utl.ContentHlp("ImportCvs.Refresh") %></span>
                    </div>
                <div style="overflow: auto; height: 400px;">
                    <table border="0" cellpadding="5">
                        <tr>
                            <td>
                                <%=AdminContentMgr.instance.GetContentWithHelpTextAsHtml("ImportCvs.FullNamesTextBox")%>
                            </td>
                            <td>
                                <%=AdminContentMgr.instance.GetContentWithHelpTextAsHtml("ImportCvs.EmailsTextBox")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="position: relative; z-index: 0; width: 150px; overflow: auto">
                                    <asp:TextBox Rows="20" Width="500" TextMode="MultiLine" Enabled="false" runat="server"
                                        ID="FileNames2TextBox" CssClass="textAreaSpecial" />
                                </div>
                            </td>
                            <td>
                                <div style="width: 450px; overflow: auto; position: relative;">
                                    <asp:TextBox Rows="20" Width="1000" TextMode="MultiLine" Enabled="true" runat="server"
                                        ID="EmailsTextBox" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:WizardStep>
            <asp:WizardStep Title="<%$Content:ImportCvs.Step5 %>" OnActivate="OnActivateImportSkills">
                <div style="display:none">
                <%=Utl.Content("ImportCvs.IgnoreSkills") %><br />
                <asp:TextBox Width="450" AutoPostBack="true" OnTextChanged="OnChangeIgnoreSkills"
                    Rows="3" TextMode="MultiLine" Enabled="true" runat="server" ID="IgnoreSkillsTextBox" />
                <span style="cursor: pointer">
                    <%=Utl.Content("ImportCvs.Refresh") %></span>
                    </div>
                <table border="0" cellpadding="2" class="stdTable">
                    <asp:Repeater runat="server" ID="ImportSkillsRep">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#((Resource)Container.DataItem).FullName %>
                                </td>
                                <td>
                                    <%#this.GetSkillsAsStr(Container.ItemIndex) %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </asp:WizardStep>
            <asp:WizardStep Title="<%$Content:ImportCvs.Step6 %>" OnActivate="OnActivateInviteUsers">
            </asp:WizardStep>
            <asp:WizardStep StepType="Finish" Title="<%$Content:ImportCvs.Step7 %>" OnActivate="OnActivateSummary">
                <%=Utl.ContentHlp("ImportCvs.Summary") %>
                <table class="niceTable">
                    <thead>
                        <tr>
                            <td><%=Utl.Content("ImportCvs.ResourceId") %></td>
                            <td><%=Utl.Content("ImportCvs.FirstName") %></td>
                            <td><%=Utl.Content("ImportCvs.LastName") %></td>
                            <td><%=Utl.Content("ImportCvs.Email") %></td>
                            <td><%=Utl.Content("ImportCvs.NoSkills") %></td>
                            <td><%=Utl.Content("ImportCvs.Invited") %></td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="SummaryRep2">
                            <ItemTemplate>
                                <tr>
                                    <td><%#((Resource)Container.DataItem).ResourceId %></td>
                                    <td><%#((Resource)Container.DataItem).FirstName %></td>
                                    <td><%#((Resource)Container.DataItem).LastName %></td>
                                    <td><%#((Resource)Container.DataItem).Email %></td>
                                    <td><%#CountSkill(this.GetImportedSkills((Resource)Container.DataItem))%></td>
                                    <td><%#this.IsInvited((Resource)Container.DataItem) %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
