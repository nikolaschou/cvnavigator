<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscProfileResultCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.DiscProfileResultCtrl" %>
<%@ Import Namespace="Cvm.Web.Code" %>
<div>
    <h2 class="printH2">
        <%#Utl.Content("DiscProfileResultCtrl.BehavioralProfile") %></h2>
    <table border="0">
        <tr>
            <td valign="top" width="300">
                <%#Utl.Content("DiscProfileResultCtrl.Describe"+MyResource.DiscProfilePrimaryCharacteristic()) %><br /><br />
                <%#Utl.Content("DiscProfileResultCtrl.DescribeProfilesEIAP",MyResource.DiscEntrPrc.ToString(),MyResource.DiscIntegratorPrc.ToString(),MyResource.DiscAdminPrc.ToString(),MyResource.DiscProducerPrc.ToString()) %>
            </td>
            <td>
                <img runat="server" id="DiscImage" src="<%#ImageUtility.EstimateUrl(MyResource) %>" />
            </td>
        </tr>
    </table>
</div>
