<%@ Control Language="C#" EnableViewState="true" AutoEventWireup="True" CodeBehind="SearchCvCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.SearchCvCtrl" %>
<%@ Register Src="~/AdminPages/CommonCtrl/ProgressBar.ascx" TagPrefix="uc" TagName="ProgressBar" %>
<%@ Register Src="~/CommonCtrl/ResourceResultGrid.ascx" TagPrefix="uc2" TagName="ResourceResultGrid" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Cvm.Backend.Business.Skills" %>
<%@ Import Namespace="Cvm.Web.AdminPages.CommonCtrl" %>
<%@ Import Namespace="Cvm.Web.Code" %>
<div>
    <uc:ProgressBar runat="server" ID="bar" Titles="<%#this.GetBarTitles()%>" />
    <ad:AdmValidationArea runat="server"/>
    <asp:Wizard DisplaySideBar="false" runat="server" ID="SearchWiz">
        <WizardSteps>
            <asp:WizardStep AllowReturn="true" Title="<%$Content:SearchCvCtrl.Step1 %>">
                <%=Utl.ContentHlpBread("SearchCvCtrl.Step1Intro")%>
                <ad:AdmCheckBoxList runat="server" EnableViewState="true" AutoActivate="true" EntityName="ProfileType"
                    ID="ProfileTypeIdsCheckList2" RepeatColumns="3" RepeatDirection=Horizontal RepeatLayout="Table" />
            </asp:WizardStep>
            <asp:WizardStep OnActivate="OnActivateChooseSkills" AllowReturn="true" Title="<%$Content:SearchCvCtrl.Step2 %>">
                <%=Utl.ContentHlpBread("SearchCvCtrl.Step2Intro")%>
                <span id="skillsHiddenFieldSpan">
                    <asp:HiddenField runat="server" ID="SkillsHiddenField" />
                </span>
                <table class="niceTable2">
                    <thead>
                        <tr>
                            <td>
                                <%=Utl.ContentHlp("SearchCvCtrl.AvailSkills") %>
                            </td>
                            <td valign="top">
                                <%=Utl.ContentHlp("SearchCvCtrl.ChosenSkills") %>
                            </td>
                            <td valign="top" style="display:none">
                                <%=Utl.ContentHlp("SearchCvCtrl.MinimummSkillLevel") %>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                        <div class="divsubform">
                                            <asp:TextBox Width="80%" runat="server" AutoCompleteType="Search" CssClass="filter"
                                                ID="SkillFilterTextBox" AutoPostBack="true" OnTextChanged="OnFilterChanged" ToolTip="<%$Content:SearchCvCtrl.FilterHelp %>" />
                                                
                                        </div>
                                        <select class="niceList clickableList" id="allSkillsList" multiple="multiple" size="20">
                                            <asp:Repeater runat="server" ID="SkillsRep" EnableViewState="false">
                                                <ItemTemplate>
                                                    <option value="<%#((ISkill)Container.DataItem).SkillId %>" class='<%#this.HasSelected(((Skill)Container.DataItem).SkillId) ? "moved" : "" %>'>
                                                        <%#((Skill)Container.DataItem).ExtendedObjectTitle %></option>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </select>
                            </td>
                            <td style="vertical-align: bottom">
                                <select class="niceList clickableList" id="selectedSkillsList" multiple="multiple"
                                    size="20">
                                    <asp:Repeater runat="server" ID="SelectedSkillsRep" EnableViewState="false">
                                        <ItemTemplate>
                                            <option value="<%#((ISkill)Container.DataItem).SkillId %>">
                                                <%#((Skill)Container.DataItem).ExtendedObjectTitle%></option>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </select>
                            </td>
                            <td style="display: none">
                                <ad:EnumDropDown2 runat="server" ID="MinimumSkillLevel2" />
                            </td>
                        </tr>
                    </tbody>
                </table>

                <script language="javascript" type="text/javascript" src="../CommonCtrl/SearchCvCtrl.js"></script>

            </asp:WizardStep>
            <asp:WizardStep runat="server" OnActivate="OnActivateDoSearchStep" AllowReturn="true"
                Title="<%$Content:SearchCvCtrl.Step3 %>">
                <%=Utl.ContentHlpBread("SearchCvCtrl.Step3Intro") %><br />
                <uc2:ResourceResultGrid runat="server" ID="ResourceResultGrid" FieldsStr="FirstName;LastName;Price;AvailableBy;ProfileTypeNamesStr;EmployeeTypeName;ProfileStatusIdEnumName;CustomerNameListStr;ExpertSkillListStr;ProfessionalSince"/>
            </asp:WizardStep>
            <asp:WizardStep AllowReturn="false" StepType="Complete" Title="<%$Content:SearchCvCtrl.Step4 %>" OnActivate="OnActivateFinish">
                <asp:Panel runat="server" ID="SummaryPanel">
                <%=Utl.ContentHlpBread("SearchCvCtrl.Step4Intro") %>
                <ul class="niceList">
                    <asp:Repeater runat="server" ID="SummaryRep" EnableViewState="false">
                        <ItemTemplate>
                            <li>
                                <%#((Resource)Container.DataItem).FullName %></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                </asp:Panel>
                
                <ad:AdmLinkButton runat="server" ID="StartOverBtn" OnClick="OnClickStartOverBtn" Text="<%$Content:SearchCvCtrl.StartOver %>"/>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</div>
