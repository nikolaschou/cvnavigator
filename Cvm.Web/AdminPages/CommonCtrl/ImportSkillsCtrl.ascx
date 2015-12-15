<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportSkillsCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.ImportSkillsCtrl" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<asp:Literal runat="server" ID="MsgLit" />
<asp:Panel runat="server" ID="MainPanel">
<div>

<%=AdminContentMgr.instance.GetContent("ImportSkillsCtrl.Introduktion") %>
<br />
<asp:CheckBox runat="server" visible="false" ID="UsePartialMatchesCheckBox" AutoPostBack="true" Text="<%$Content:ImportSkillsCtrl.UsePartialMatches %>" />

<table border="1" class="niceTable">
<thead>
<tr>
<td><%=AdminContentMgr.instance.GetContent("ImportSkillsCtrl.DoImport") %></td>
<td><%=AdminContentMgr.instance.GetContent("ImportSkillsCtrl.SkillName") %></td>
<td><%=AdminContentMgr.instance.GetContent("ImportSkillsCtrl.ContextText") %></td>
</tr>
</thead>
<tbody>
<asp:Repeater ID="SkillRep" runat="server" >
<ItemTemplate>
<tr>
<td>
<asp:CheckBox ToolTip='<%#(HasSkillAlready(((SkillMatchWrapper)Container.DataItem ).Skill) ? AdminContentMgr.instance.GetContent("ImportSkillsCtrl.HasSkillAlready") : null)%>' ID="DoImportCheckBox" runat="server" Checked="true" Enabled="<%#!HasSkillAlready(((SkillMatchWrapper)Container.DataItem ).Skill) %>"/>
</td>
<td>
<%#((SkillMatchWrapper)Container.DataItem ).Skill.SkillName%>
</td>
<td>
<%#((SkillMatchWrapper)Container.DataItem ).Match.ContextText%>

</td>
</tr>
</ItemTemplate>
</asp:Repeater>
</tbody>
</table>
<br />
<asp:Button ID="ImportBtn" CssClass="saveBtn" runat="server" Text="<%$Content:ImportSkillsCtrl.DoImportBtn %>" OnClick="OnClickDoImportBtn" />

</div>

</asp:Panel>