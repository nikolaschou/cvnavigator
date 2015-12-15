<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceResultGrid.ascx.cs" Inherits="Cvm.Web.CommonCtrl.ResourceResultGrid" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<div id="resourceResultGridDiv" class="clickableTableRows">
    <ad:AdmGridView  EnableViewState="false" runat="server" ID="ResultGrid" OmitSortingInColumns="0">
        <Columns>
            <asp:TemplateField><ItemTemplate>
                <input type='checkbox' resourceId="<%#((SysResource)Container.DataItem).ResourceId%>" xxx=<%#this.ChosenResourceIds.Contains(((SysResource)Container.DataItem).ResourceId ) ? "\'\' checked" : "\'\'"%>/>
            </ItemTemplate></asp:TemplateField>
        </Columns>
    </ad:AdmGridView>
    <span id="selResourcesHiddenFieldSpan">
        <asp:HiddenField runat="server"  ID="SelResourcesHiddenField"/>
    </span>
    <script language="javascript" type="text/javascript" src="../CommonCtrl/ResourceResultGrid.js" ></script>
</div>