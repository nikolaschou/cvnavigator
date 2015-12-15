<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="DataStatistics.aspx.cs" Inherits="Cvm.Web.AdminPagesGlobal.DataStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table class="niceTable">
<thead><tr><td></td><%for (int dayCounter = 0; dayCounter < this.TableCount.GetNoDays(); dayCounter++)
  {%>
  <td><span style="cursor:pointer" title='<%=this.TableCount.GetDateStr(dayCounter) %>'><%=this.TableCount.GetDateStrDayOnly(dayCounter)%></span></td>
<%
  }%>
  </tr></thead>
<tbody>
  <%for (int tableCounter = 0; tableCounter < this.TableCount.GetNoTables(); tableCounter++)
  {%>
  <tr>
    <td><%=this.TableCount.GetTableName(tableCounter) %></td>
    <%for (int dayCounter = 0; dayCounter < this.TableCount.GetNoDays(); dayCounter++)
{%>
    <td><%=this.TableCount.GetCount(tableCounter,dayCounter) %></td>
    <%
}%>
  </tr>
<%
  }%>
  </tbody>
</table>
</asp:Content>
