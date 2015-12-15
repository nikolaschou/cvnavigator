<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabularCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.TabularCtrl" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <asp:Repeater runat="server" DataSource="<%#this.Urls %>">
            <ItemTemplate>
                <td class='<%#"tab"+(this.ChosenIndex==Container.ItemIndex?"Chosen":"") %>' 
                onclick="<%#((PageLink)Container.DataItem).IncludeExistingParms().GetLinkAsJavascript()%>">
                    <%#Utl.Content(this.ContentPrefix+".Tab"+Container.ItemIndex)%>
                </td>
            </ItemTemplate>
        </asp:Repeater>
        <td class="tabFiller">&nbsp;</td>
        </tr>
</table>
