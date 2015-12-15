<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs" Inherits="Cvm.Web.AdminPages.Menu" Title="Untitled Page" %>
<%@OutputCache Duration="3600" Location="Client" VaryByParam="None"%>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>

<script runat="server">
    
    private int MODULO_LEVEL1 = 4;
    private int MODULO_LEVEL2 = 4;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Returns null if node is above the given level, otherwise
    /// finds the ancestor-or-self of node at the given level.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private SiteMapNode FindAncestorOrSelf(SiteMapNode node, int level)
    {
        if (IsRoot(node)) return null;
        SiteMapNode temp = node;
        for (int i = 0; i < level - 1; i++)
        {
            temp = temp.ParentNode;
            if (IsRoot(temp))
            {
                //We are above level
                return null;
            }

        }
        if (IsRoot(temp.ParentNode)) return node;
        else return FindAncestorOrSelf(node.ParentNode, level);
    }

    private bool IsRoot(SiteMapNode node)
    {
        return node.Equals(node.RootNode);
    }

    protected String MakeMouseOver(SiteMapNode node)
    {
        return "javascript:document.getElementById('" + MakeUniqueId(node) + "').style.display='block'";
    }

    protected String MakeUniqueId(SiteMapNode node)
    {
        return "div" + Math.Abs(node.GetHashCode());
    }

    protected String MakeMouseOut(SiteMapNode node)
    {
        return "javascript:document.getElementById('" + MakeUniqueId(node) + "').style.display='none'";
    }

    protected IEnumerable<SiteMapNode> FilterByRolesAndVisibility(SiteMapNodeCollection childNodes)
    {
        foreach (SiteMapNode n in childNodes)
        {
            if ("1".Equals(n["hideInMenu"]))
            {
                //Don't return
            }
            else
            {
                if (n.Roles.Count == 0) yield return n;
                foreach (String r in n.Roles)
                {
                    if (ContextObjectHelper.CurrentUserHasRoleGlobalOrSys(r))
                    {
                        yield return n;
                        break;
                    }
                }
            }
        }
    }


    private string GetContent(SiteMapNode n0)
    {
        String contentId;
        if (n0["contentId"] != null) contentId = "Menu." + n0["contentId"];
        else contentId = "Menu." + n0.Title.Replace(" ", "");
        return Utl.Content(contentId);
    }

    private int GetColspan(SiteMapNode n0)
    {
        return
            n0["colspan"] != null ?
            int.Parse(n0["colspan"]) :
            1;
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newMenu">
     <div  style="text-align:center;background-color:white;border:1px solid gray;border-radius:5px;-moz-border-radius:5px 5px 0px 0px;">
                     <img style='display:<%=ContextObjectHelper.CurrentSysOwnerOrNull!=null && ContextObjectHelper.CurrentSysOwnerOrFail.RelatedLogoFileRefObj!=null?"block":"none"%>' src='<%= ContextObjectHelper.CurrentSysOwnerOrNull!=null && ContextObjectHelper.CurrentSysOwnerOrFail.RelatedLogoFileRefObj!=null ? ContextObjectHelper.CurrentSysOwnerOrFail.RelatedLogoFileRefObj.GetAsUrl() :"" %>'/>
                    <a href='<%=ContextObjectHelper.GetSignupLinkForCurrentSite()%>'><%=Utl.Content("Menu.SignupLink") %></a> | <a href='<%=ContextObjectHelper.GetSearchAppForCurrentSite()%>'><%=Utl.ContentHlp("Menu.SearchApp") %></a>
                    
                    </div>
        <table style="border:1px solid gray">
         
            <tr class="newMenuTable">
                <%
                    int counter = 1;
                    int counter2 = 1;
                    foreach (SiteMapNode n0 in FilterByRolesAndVisibility(SiteMap.RootNode.ChildNodes))
                    {
                %>
                <td colspan="<%=this.GetColspan(n0) %>">
                    <div class="menuOuterDiv">
                        <table><tr><td>
                        <div style="cursor:pointer;" onclick="javascript:location.href='<%=FilterByRolesAndVisibility(n0.ChildNodes).First().Url %>'">
                        <div style="height:110px;width:150px;text-align:center">
                        <img src='../images/menu/<%=n0["image"]%>' title="<%=this.GetContent(n0) %>"/>
                        
                        </div>
                        <%=n0["horizontal"]!=null ? "</td><td>" : "" %>
                        <div class="menuTitle"><%=n0.Title%></div>
                        </div>
                        <% foreach (SiteMapNode n1 in FilterByRolesAndVisibility(n0.ChildNodes))
                           {
                        %>
                        <%=String.IsNullOrEmpty(n1.Url) ? "" : "<a href='"+n1.Url+"'>" %>
                        <%=n1.Title %>
                        <%=String.IsNullOrEmpty(n1.Url) ? "" : "</a>" %>
                        <br />
                        <ul>
                            <% foreach (SiteMapNode n2 in FilterByRolesAndVisibility(n1.ChildNodes))
                               {
                            %>
                            <li><a href="<%=n2.Url %>">
                                <%=n2.Title %></a></li>
                            <%
                                } %>
                        </ul>
                        <%MODULO_LEVEL2 = 2;%><%=n0["horizontal"]!=null && counter2 % MODULO_LEVEL2 == 0 ? "</td><td>" : "" %>
                        <%
                               counter2++;%>
                        <%
                           } %>
                            </td></tr></table>
                    </div>
                </td>
                <%= counter % MODULO_LEVEL1 == 0 ? "</tr><tr class='newMenuTable'>" : ""%>
                <%
                        counter = counter + this.GetColspan(n0);%>
                <%
                    }%>
            </tr>
        </table>
    </div>
</asp:Content>
