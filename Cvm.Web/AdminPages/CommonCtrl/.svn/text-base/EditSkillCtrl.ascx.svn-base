<%@ Import Namespace="Cvm.Backend.Business.Config" %>
<%@ Import Namespace="Cvm.Backend.Business.Skills" %>
<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Import namespace="Napp.Web.AdminContentMgr"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EditSkillCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.EditSkillCtrl" EnableViewState="true" %>
<%@ Register TagPrefix="Ex" Namespace="Napp.Web.AdmControl" Assembly="Napp.Web.AdmControl" %>
<asp:Panel runat="server" ID="Outer">
    <asp:Panel runat="server">
        <tr id='<%=GetRowId() %>'>
            <% if (ConfigMgr.IncludeSkillCategory) 
            {%>
                <td>
                    <asp:Literal ID="Literal1" EnableViewState="false" runat="server" Text="<%# mySkill.RelatedSkillObj.RelatedSkillTypeObj.SkillTypeName%>" />
                </td>
            <%}%>
            <td>
                <asp:Literal ID="Literal2" EnableViewState="false" runat="server" Text="<%# mySkill.RelatedSkillObj.SkillName %>" />
            </td>
                    
            <% if (ConfigMgr.EditSkillCtrl_SkillLastUsed)
            {%>
                <td valign="top" style="width:40px;">
                    <Ex:AdmTextBox Width="40" ID="LastUsedTextBox" runat="server" Text="<%#GetLastUsed() %>" />
                    <asp:RangeValidator EnableViewState="false"  EnableClientScript="false" ID="LastUsedValidator" runat="server"  ControlToValidate="LastUsedTextBox" MinimumValue="1900" MaximumValue="2100"><br /><%#Napp.Web.AdminContentMgr.AdminContentMgr.instance.GetContent("EditSkillCtrl.ValidateLastUsed") %></asp:RangeValidator>
                </td>
            <%}%>
            <% if (ConfigMgr.EditSkillCtrl_SkillUsedInTotal)
            {%>
                <td valign="top" style="width:40px;">
                    <Ex:AdmTextBox Width="40" ID="UsedInTotalTextBox" runat="server" Text="<%#GetUsedInTotal() %>" />
                    <asp:RangeValidator  EnableViewState="false" EnableClientScript="false" ID="UsedInTotalValidator" runat="server" ControlToValidate="UsedInTotalTextBox" Type="Integer" MinimumValue="1" MaximumValue="50"><br /><%#Napp.Web.AdminContentMgr.AdminContentMgr.instance.GetContent("EditSkillCtrl.ValidateUsedInTotal") %></asp:RangeValidator>
                </td>
            <%}%>
                <td valign="top" style="width:90px">
                    <Ex:EnumDropDown2 runat="server" Width="90"  ID="SkillLevelDropDown3" Required="false"/>
                    <asp:RequiredFieldValidator EnableViewState="false"  EnableClientScript="false" ID="SkillLevelValidator" runat="server" ControlToValidate="SkillLevelDropDown3"><br /><%#Napp.Web.AdminContentMgr.AdminContentMgr.instance.GetContent("EditSkillCtrl.ValidateSkillLevel")%></asp:RequiredFieldValidator>
                </td>
            <% if (ConfigMgr.EditSkillCtrl_EnableQualificationEngine)
            {%>
                <td width="150">
                <% if (this.IsAlignedWithProjectExperience())
                {%>
                    <img src="../images/master/status-ok.png" alt="" />
                <%}
                else
                {%>
                    <div title="<%#Utl.Content("EditSkillCtrl.AdjustYearsTooltip",SkillProjectQualification.UsedInTotalYears.ToString(),SkillProjectQualification.LastUsedYear.ToString()) %>">
                    <img src="../images/master/warning.png" alt="" />
                    <ad:AdmLinkButton runat="server" ContentId="EditSkillCtrl.AdjustYears" OnClick="OnClickAdjustYears" />
                    </div>
                <%}%>
                </td>        
                <td width="40">
                    <%#this.IsAlignedWithProjectExperience()?"":""+this.mySkill.GetProjectQualification().LastUsedYear %>
                </td>
                <td width="40">
                    <%#this.IsAlignedWithProjectExperience() ? "" : "" + this.mySkill.GetProjectQualification().UsedInTotalYears%>
                </td>
            <%}%>
            <td valign="top">                        
                <a href='javascript:clearInputFields(document.getElementById("<%=GetRowId() %>"))'><%#AdminContentMgr.instance.GetContent("EditSkillCtrl.Clear") %></a>
                <asp:Label EnableViewState="false" CssClass="message" runat="server" ID="MsgLiteral" />
            </td>
        </tr>
    </asp:Panel>
</asp:Panel>
