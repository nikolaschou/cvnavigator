<%@ Import Namespace="Cvm.Backend.Business.Config" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Import Namespace="Cvm.Backend.Business.Skills" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCvSkillsCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.EditCvSkillsCtrl" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<%@ Register Src="~/AdminPages/CommonCtrl/EditSkillCtrl.ascx" TagPrefix="uc" TagName="EditSkillCtrl" %>
<%@ Register Src="~/AdminPages/CommonCtrl/ImportSkillsCtrl.ascx" TagPrefix="uc" TagName="ImportSkillsCtrl" %>
<%@ Register Src="~/CommonCtrl/EditObjectCtrl.ascx" TagPrefix="uc" TagName="EditObjectCtrl" %>
<%@ Register TagPrefix="Adm" Namespace="Napp.Web.AdmControl" Assembly="Napp.Web.AdmControl" %>
<%@ Register TagPrefix="Ex" Namespace="Napp.Web.ExtControls" Assembly="Napp.Web.ExtControls" %>
<div>
    <%if (ConfigMgr.EditSkillCtrl_EnableQualificationEngine && this.HasUnqualifiedSkills())
      {%>
    <div class="warningDiv">
        <img src="../images/master/warning.png" alt="" />
        <%=Utl.ContentHlp("EditCvSkillsCtrls.FoundUnqualifiedSkills")%>
        <ad:AdmLinkButton ID="AdmLinkButton2" runat="server" ContentId="EditCvSkillsCtrl.AdjustAllSkills"
            OnClick="OnClickAdjustAllSkills" />
    </div>
    <%
        }
      if (ConfigMgr.IncludeSkillCategory)
      {
    %>
    <%=AdminContentMgr.instance.GetContent("EditCvSkillsCtrl.FilterByType")%>
    <Adm:AdmDropDown ID="SkillTypesFilterDropDown" IncludeBlank="true" BlankTextContentId="EditCvSkillsCtrl.ChooseSkillCategory"
        AutoPostBack="true" OnSelectedIndexChanged="OnSelectSkillTypes" runat="server"
        EntityName="SkillType" />
    <br />
    <%
        }%>
    <asp:Panel runat="server" ID="ProfileTypePanel" Visible="false">
        <%=Utl.Content("EditCvSkillsCtrl.ChooseProfiles" + (this.GetAssignedProfileTypes().Count>1?"Many":"Single"))%>
        <asp:Repeater runat="server" ID="ProfileRep" DataSource="<%#this.GetAssignedProfileTypes() %>">
            <ItemTemplate>
                <a href="<%#PageNavigation.GetCurrentLink().IncludeExistingParms().SetParm(QueryParmCvm.profileTypeId,((ProfileType)Container.DataItem).ProfileTypeId).GetLinkAsHref() %>"
                    class="<%#(this.GetCurrentProfileTypeId()==((ProfileType)Container.DataItem).ProfileTypeId ? "admLinkButton selected" :"admLinkButton") %>">
                    <%#((ProfileType)Container.DataItem).ProfileTypeName %>
                </a>
            </ItemTemplate>
            <SeparatorTemplate>
                |</SeparatorTemplate>
        </asp:Repeater>
        <% if (ConfigMgr.EditCvSkillsCtrl_ShowAllAssignedLink)
           {%>
        |
        <ad:AdmHyperLink ID="AdmHyperLink1" runat="server" CssClass='<%#this.GetCurrentProfileTypeId()==this.CHOOSE_ALL_ASSIGNED ? "admLinkButton selected" :"admLinkButton" %>'
            Text="<%$Content:EditCvSkillsCtrl.ShowAllAssignedSkills%>" AutoBind="true" PageLink="<%#PageNavigation.GetCurrentLink().IncludeExistingParms().SetParm(QueryParmCvm.profileTypeId,this.CHOOSE_ALL_ASSIGNED)%>" />
        <%
            }%>
    </asp:Panel>
    <% if (ConfigMgr.EditCvSkillsCtrl_IncludeAddSkill)
       {%>
    <br />
    <%=AdminContentMgr.instance.GetContent("EditCvSkillsCtrl.AddSkill")%>
    <span class="subformSmart">
    <ad:AdmTextBox runat="server" ID="AddSkillTextBox" CausesValidation="false" autocallback="globalService"
        webService="GetSkills" />
    <ad:AdmButton ID="AdmButton5" runat="server" ContentId="EditCvSkillsCtrl.AddSkillBtn"
        OnClick="OnClickAddSkillBtn" />
        </span>
    <%
        }%>
    <div style="display: none">
        <ad:AdmLinkButton ID="AdmLinkButton1" Visible="true" runat="server" ContentId="EditCvSkillsCtrl.AddSkillLink"
            OnClick="OnClickAddSkillBtn" />
        <%=AdminContentMgr.instance.GetContent("EditCvSkillsCtrl.AddSkill") %>
    </div>
    <asp:Panel runat="server" ID="OuterPanel">
        <ad:AdmPanel runat="server" AutoBind="true" Visible="<%#this.MyResourceSkills.Count>20 %>">
            <br />
            <Adm:AdmButton ID="AdmButton3" runat="server" ContentId="EditCvSkillsCtrl.Save" CssClass="saveBtn" OnClick="OnClick_SaveBtn" />
            <Adm:AdmButton ID="AdmButton4" runat="server" ContentId="EditCvSkillsCtrl.Cancel" CssClass="cancelBtn" OnClick="OnClick_CancelBtn" />
            <br />
        </ad:AdmPanel>
        <br />
        <div>
            <table class="datagrid1 boxed">
                <thead>
                    <tr>
                        <% if (ConfigMgr.IncludeSkillCategory)
                           {%>
                        <td>
                            <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.SkillCategory")%>
                        </td>
                        <%
                            }%>
                        <td>
                            <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.SkillName")%>
                        </td>
                        
                        <% if (ConfigMgr.EditSkillCtrl_SkillLastUsed) {%>
                        <td>
                            <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.SkillUsedLast")%>
                        </td>
                          <% }%>
                        <% if (ConfigMgr.EditSkillCtrl_SkillUsedInTotal) {%>
                        <td>
                            <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.SkillUsedInTotal")%>
                        </td>
                          <% }%>
                        <td>
                            <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.SkillLevel")%>
                        </td>
                        <% if (ConfigMgr.EditSkillCtrl_EnableQualificationEngine)
                           {%>
                        <td colspan="3" width="230">
                            <table>
                                <tr>
                                    <td colspan="3" align="center" style="background-color: #BBBBBB">
                                        <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.Qualification")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150">
                                        <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.QualifyStatus")%>
                                    </td>
                                    <td width="40">
                                        <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.QualifyLastUsed")%>
                                    </td>
                                    <td width="40">
                                        <%=Utl.ContentHlp("Content:EditCvSkillsCtrl.QualifyUsedInTotal")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <%
                            }%>
                        <td>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Panel runat="server" ID="SkillsPanel">
                    </asp:Panel>
                </tbody>
            </table>
        </div>
        <br />
        <div class="buttons">
            <Adm:AdmButton ID="AdmButton1" runat="server" ContentId="EditCvSkillsCtrl.Save" CssClass="saveBtn" OnClick="OnClick_SaveBtn" />
            <Adm:AdmButton ID="AdmButton2" runat="server" ContentId="EditCvSkillsCtrl.Cancel" CssClass="cancelBtn" OnClick="OnClick_CancelBtn" />
        </div>
    </asp:Panel>
</div>
