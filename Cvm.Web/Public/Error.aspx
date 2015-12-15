<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Cvm.Web.AdminPages.Error"
    Title="Untitled Page" %>

<%@ Import Namespace="Cvm.Web.Code" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Import Namespace="System.ComponentModel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Error in CVNavigator</title>
    <link href="~/reset.css" type="text/css" rel="stylesheet" />
    <link href="~/cvnstyle.css" type="text/css" rel="stylesheet" />
    <link href="~/ssadmin.css" type="text/css" rel="stylesheet" />
    <link href="~/ssadmin_print.css" media="print" type="text/css" rel="stylesheet" />
    <link href="~/ResultSetStyles.css" type="text/css" rel="stylesheet" />
</head>
<body style="">
    <div id="container">
        <form id="form1" runat="server">
        <div id="top">
            <div id="logo" onclick="location.href='../default.htm'" style="cursor: pointer;">
            </div>
            <div id="menu" class="noprint">
                <div class="noprint">
                </div>
            </div>
            <div id="menu-right">
            </div>
        </div>
        <div id="left">
            <div id="mainContainer" style="text-align: left">
                <asp:Panel runat="server" ID="ErrorPanel" Visible="true">
                    <h2>
                        <%=this.GetExplanation(this.GetFirstException())%> (<%=this.GetErrorCodeLink()%>)</h2>
                        <br />
                    <input type="button" onclick="javascript:window.history.back()" value="<%=AdminContentMgr.instance.GetContent("Error.GoBack")%>" />
                    <br /><br />
                    <input type="button" onclick="javascript:location.href='../default.htm'" value="<%=AdminContentMgr.instance.GetContent("Error.GotoStart")%>" />
                    <br /><br />
                    <input type="button" onclick="javascript:location.href='../Public/Login.aspx?logout=1'"
                        value="<%=AdminContentMgr.instance.GetContent("Error.ToLoginPage")%>" />
                    <br />
                    <br />
                    <span style="font-weight:bold;cursor:pointer;" onclick="javascript:document.getElementById('detailsDiv').style.display='block';"><%=AdminContentMgr.instance.GetContent("Error.Details") %></span>
                    <div id="detailsDiv" style="display:none">
                    <table border="0" width="800">
                        <asp:Repeater runat="server" DataSource="<%#GetExceptionChain() %>">
                            <ItemTemplate>
                                <tr>
                                    <td class="tdhead">
                                        <%=AdminContentMgr.instance.GetContent("Error.FriendlyMessage") %>
                                    </td>
                                    <td class="tdbody">
                                        <xmp><%#this.GetExplanation(Container.DataItem as Exception)%></xmp>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdhead">
                                        <%=AdminContentMgr.instance.GetContent("Error.TechMessage") %>
                                    </td>
                                    <td class="tdbody">
                                        <xmp><%#(Container.DataItem as Exception).Message%></xmp>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdhead">
                                        <%=AdminContentMgr.instance.GetContent("Error.Type") %>
                                    </td>
                                    <td class="tdbody">
                                        <xmp><%#(Container.DataItem as Exception).GetType()%></xmp>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <br />
        </form>
    </div>
    <div id="bottom" style="bottom: 0px;">
        <div class="noprint" id="bottom-inner">
            CV-Navigator Copyright 2010 <span class="delim"></span>
            <img style="position: relative; top: 12px;" src="../images/small_logo.png" alt="CVNavigator" />
        </div>
    </div>
</body>
</html>
