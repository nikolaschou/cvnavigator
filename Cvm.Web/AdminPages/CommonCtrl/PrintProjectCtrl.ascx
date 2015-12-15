<%@ Import Namespace="Cvm.Backend.Business.Util" %>
<%@ Import Namespace="Cvm.Backend.Business.Customers" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintProjectCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.PrintProjectCtrl" %>
<%@ Import Namespace="Napp.Backend.Business.Common" %>
<%@ Import Namespace="Napp.VeryBasic" %>
<table width="<%=this.PageWidth%>" class="printTable" border="0" cellpadding="3"
    cellspacing="0">
    <%if (Project.ProjectName.NotEmpty())
      {%>
    <tr>
        <td style="width: <%=this.FirstColWidth%>px" class="printDarkcell">
            <%# GetContent("PrintCv.ProjectTitle")%>
        </td>
        <td>
            <%#Project.ProjectName %>
        </td>
    </tr>
    <tr>
        <td style="width: <%=this.FirstColWidth%>px" class="printDarkcell">
            <%# GetContent("PrintCv.Customer")%>
        </td>
        <td>
            <%#MakeAnonym(Project.CustomerName) %>
        </td>
    </tr>
    <%
                              }%>
    <tr>
        <td style="width: <%=this.FirstColWidth%>px" class="printDarkcell">
            <%# GetContent("PrintCv.Period")%>
        </td>
        <td>
            <%#DateHelper.Instance.FormatDuration(Project.StartedBy, Project.EndedBy)%>
        </td>
    </tr>
    <tr>
        <td style="width: <%=this.FirstColWidth%>px" class="printDarkcell">
            <%# GetContent("PrintCv.ProjectDescription")%>
        </td>
        <td>
            <%#(Project.ProjectDescription!=null ? StringUtil.MakeHtmlLineBreaks(Project.ProjectDescription) : "")%>
        </td>
    </tr>
    <% if (Project.ProjectSkillsStr.NotEmpty())
       {%>
    <tr>
        <td style="width: <%=this.FirstColWidth%>px" class="printDarkcell">
            <%# GetContent("PrintCv.Skills")%>
        </td>
        <td>
            <%#Project.ProjectSkillsStr %>
        </td>
    </tr>
    <%
                               }%>
    <% if (Project.Role.NotEmpty())
       {%>
    <tr>
        <td style="width: <%=this.FirstColWidth%>px" class="printDarkcell">
            <%# GetContent("PrintCv.Role")%>
        </td>
        <td>
            <%#Project.Role %>
        </td>
    </tr>
    <%
                               }%>
</table>
