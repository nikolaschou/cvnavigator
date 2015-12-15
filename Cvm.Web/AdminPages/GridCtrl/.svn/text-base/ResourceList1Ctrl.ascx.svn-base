<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ResourceList1Ctrl.ascx.cs" Inherits="Cvm.Web.AdminPages.GridCtrl.ResourceList1Ctrl" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<div>
<ad:AdmGridView ID="Grid1" runat="server" EnableViewState="false"  AutoGenerateColumns="false" OmitSortingInColumns="3;4" OmitSearchBox="true">
            <Columns>
                <asp:TemplateField ItemStyle-Width="180px" HeaderText="<%$Content:ResourceList.Name %>"><ItemTemplate><a href="<%#Utl.LinkHelper.GetPrintLink(Container.DataItem as Resource).GetLinkAsPopupScript() %>"><%# (ContextObjectHelper.OnlyAnonymousAccess() ? (Container.DataItem as Resource).GetInitials() : (Container.DataItem as Resource).FullName)%></a></ItemTemplate></asp:TemplateField>                
                <asp:TemplateField  ItemStyle-Width="50px" HeaderText="<%$Content:ResourceList.ProfessionalSince %>"><ItemTemplate><%# (Container.DataItem as Resource).ProfessionalSince%></ItemTemplate></asp:TemplateField>                
                <asp:TemplateField ItemStyle-Width="100px" HeaderText="<%$Content:ResourceList.AvailableBy %>"><ItemTemplate><%# (Container.DataItem as Resource).AvailableBy.FormatDate()%></ItemTemplate></asp:TemplateField>                
                <asp:TemplateField ItemStyle-Width="180px" HeaderText="<%$Content:ResourceList.ExpertSkillListStr %>"><ItemTemplate><%# (Container.DataItem as Resource).ExpertSkillListStr.Comma2LineBreak()%></ItemTemplate></asp:TemplateField>                
                <asp:TemplateField  ItemStyle-Width="180px" HeaderText="<%$Content:ResourceList.CustomerNameListStr %>"><ItemTemplate><%#Utl.CurrentSysUser.SysUserObjInContextOrFail.AnonymousPrint? "" : (Container.DataItem as Resource).CustomerNameListStr.Comma2LineBreak()%></ItemTemplate></asp:TemplateField>                
            </Columns>
</ad:AdmGridView>
</div>