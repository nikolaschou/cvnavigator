<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true"
    CodeBehind="ReportCv.aspx.cs" Inherits="Cvm.Web.AdminPages.ReportCv" Title="Untitled Page" %>

<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Register Assembly="Napp.Web.AdmControl" Namespace="Napp.Web.AdmControl" TagPrefix="cc3" %>
<%@ Register Assembly="Napp.Web.AutoForm" Namespace="Napp.Web.AutoForm" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ad:AdmPanel runat="server" AutoBind="true" Visible="<%#Utl.HasSysRole(RoleSet.SalesMgrAtLeast) %>">
        <asp:CheckBox runat="server" AutoPostBack="true" Text="<%$Content:ReportCv.ExtendedView %>"
            ID="ExtendedViewCheckBox" />
        <br />
        <asp:CheckBox runat="server" Visible="<%#Utl.HasSysRole(SysRoleEnum.SysAdmin) %>"
            AutoPostBack="true" Text="<%$Content:ReportCv.AdvancedSearch %>" ID="AdvancedSearchCheckBox" />
        
        <ad:AdmPanel runat="server" ID="SearchPanel" AutoBind="true" Visible="<%#this.AdvancedSearchCheckBox.Checked %>">
            <fieldset>
            <table style="width: 273px" class="paddedTable">
                <tr>
                    <td valign="top">
                        <asp:UpdatePanel runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <table>
                                    <h3><%=Utl.ContentHlp("ReportCv.SearchCustomer") %></h3>
                                    <cc3:AdmListBox Width="200" ID="CustomerListBox" ContentId="SearchCv.Customers" SelectionMode="Multiple"
                                        runat="server" Rows="20" TableMode="Row" />
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <ad:AdmLinkButton ID="AdmLinkButton1" runat="server" ContentId="ReportCv.SearchAllCustomers"
                                                OnClick="OnClickSearchAllCustomers" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top">
                        
                            <h3><%=Utl.ContentHlp("ReportCv.SearchEmployeeType") %></h3>
                            <cc3:AdmCheckBoxList EntityName="EmployeeType" AutoActivate="false" ID="EmployeeTypeCheckList"
                                ContentId="SearchCv.ProfileType" runat="server"  />
                                <br />
                            <h3><%=Utl.ContentHlp("ReportCv.SearchStatus") %></h3>
                            <cc3:AdmEnumCheckBoxList ID="ProfileStatusCheckList" AutoActivate="false" ContentId="SearchCv.ProfileStatus"
                                runat="server"  />
                                <br />
                            <h3><%=Utl.ContentHlp("ReportCv.SearchProfile") %></h3>
                            <cc3:AdmCheckBoxList EntityName="ProfileType" AutoActivate="false" ID="ProfileTypeCheckList"
                                ContentId="SearchCv.ProfileType" runat="server"  RepeatDirection="Vertical"
                                RepeatLayout="Table" RepeatColumns="3" CssClass="checkBoxList" />
                                <br />
                            <h3><%=Utl.ContentHlp("ReportCv.SearchAvailableBy") %></h3>
                            <cc3:AdmTextBox ID="AvailableInDays" ContentId="SearchCv.AvailableInDays" ValidationType="Integer"
                                runat="server"  />
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <cc3:AdmButton ID="AdmButton1" CausesValidation="true" ContentId="SearchCv.Search"
                            OnClick="OnClick_SearchProfile" runat="server" />
                        <cc3:AdmClientButton ID="AdmButton2" ContentId="SearchCv.ShowEmailList" OnClientClick="$('#emailDiv').dialog('open')"
                            runat="server" />
                    </td>
                </tr>
            </table>
            </fieldset>
        </ad:AdmPanel>
    </ad:AdmPanel>
    <br />
    <ad:AuxLinkButton Visible="false" runat="server" Text="<%$Content:ReportCv.ToSpreadSheet %>"
        OnClick="OnClickToSpreadSheet" />
    <div class="nowrap">
        <cc3:AdmGridView EnableViewState="false" AutoGenerateColumns="false" OmitSortingInColumns="4;5;6"
            ID="ResourcesGrid" runat="server">
            <Columns>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.FirstName %>">
                    <ItemTemplate>
                        <a href="<%#Utl.LinkHelper.GetPrintLink(Container.DataItem as Resource).GetLinkAsPopupScript() %>">
                            <%# (Container.DataItem as Resource).FirstName%></a></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.LastName %>">
                    <ItemTemplate>
                        <a href="<%#Utl.LinkHelper.GetPrintLink(Container.DataItem as Resource).GetLinkAsPopupScript() %>">
                            <%# (Container.DataItem as Resource).LastName%></a></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.LastModified %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).LastModifiedTs%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.ProfessionalSince %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).ProfessionalSince%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.AvailableBy %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).AvailableBy.FormatDate()%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.ExpertSkillListStr %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).ExpertSkillListStr.FormatMore(25)%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.CustomerNameListStr %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).CustomerNameListStr.FormatMore(50)%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.ProfileTypeNameStr %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).ProfileTypeNamesStr.FormatMore(20)%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.ProfileStatus %>">
                    <ItemTemplate>
                        <%# Utl.Content((Container.DataItem as Resource).ProfileStatusIdEnum)%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.EmployeeType %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).EmployeeTypeName%></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc3:AdmGridView>
    </div>
    <div class="nowrap">
        <cc3:AdmGridView EnableViewState="false" AutoGenerateColumns="false" ID="ResourcesGridSimple"
            runat="server">
            <Columns>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.FirstName %>">
                    <ItemTemplate>
                        <a href="<%#Utl.LinkHelper.GetPrintLink(Container.DataItem as Resource).GetLinkAsPopupScript() %>">
                            <%# (ContextObjectHelper.OnlyAnonymousAccess() ? "" : (Container.DataItem as Resource).FirstName)%></a></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.LastName %>">
                    <ItemTemplate>
                        <a href="<%#Utl.LinkHelper.GetPrintLink(Container.DataItem as Resource).GetLinkAsPopupScript() %>">
                            <%# (ContextObjectHelper.OnlyAnonymousAccess() ? (Container.DataItem as Resource).GetInitials() : (Container.DataItem as Resource).LastName)%></a></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.ProfessionalSince %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).ProfessionalSince%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.AvailableBy %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).AvailableBy.FormatDate()%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.ProfileStatus %>">
                    <ItemTemplate>
                        <%# Utl.Content((Container.DataItem as Resource).ProfileStatusIdEnum)%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$Content:SearchCv.EmployeeType %>">
                    <ItemTemplate>
                        <%# (Container.DataItem as Resource).EmployeeTypeName%></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc3:AdmGridView>
    </div>
    <div class="dialog closed" style="width: 400px; display: none" id="emailDiv" title="<%=Utl.ContentHlp("ReportCv.EmailList") %>">
        <textarea cols="70" rows="10"><%=GetEmailAddresses() %></textarea><br />
        <%=Utl.ContentHlp("ReportCv.ResourcesWithoutEmail") %>
        <div style='width:400px'><%=GetResourcesWithoutEmailAddresses() %></div>
    </div>
</asp:Content>
