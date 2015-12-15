<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="True"
    CodeBehind="EditAssignmentResources.aspx.cs" Inherits="Cvm.Web.AdminPages.EditAssignmentResources"
    Title="Untitled Page" %>

<%@ Import Namespace="Cvm.Backend.Business.Assignments" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Register TagPrefix="my" TagName="TabularCtrl" Src="~/AdminPages/CommonCtrl/TabularCtrl.ascx" %>
<%@ Register TagPrefix="my" TagName="ListFilterCtrl" Src="~/AdminPages/CommonCtrl/ListFilterCtrl.ascx" %>
<%@ Register TagPrefix="my" TagName="EditAssignmentsAuxCtrl" Src="~/AdminPages/CommonCtrl/EditAssignmentsAuxCtrl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LeftPane" runat="server">
    <my:ListFilterCtrl runat="server" ObjectType="Assignment" ID="ListFilterCtrl1" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MiddlePane" runat="server">
    <div class="tabDiv">
        <my:TabularCtrl runat="server" ID="TabularCtrl1" />
        <div class="tabDivInner">

                    <ad:AdmGridView runat="server" ID="ResourceGrid" EnableViewState="false" OmitSortingInColumns="6;7">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.FirstName %>">
                                <ItemTemplate>
                                    <a href="<%#Utl.LinkHelper.GetPrintLink(GetResource(Container)).GetLinkAsPopupScript() %>">
                                        <%# (this.GetResource(Container)).FirstName%></a></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.LastName %>">
                                <ItemTemplate>
                                    <a href="<%#Utl.LinkHelper.GetPrintLink(GetResource(Container)).GetLinkAsPopupScript() %>">
                                        <%# (this.GetResource(Container)).LastName%></a></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.AvailableBy %>">
                                <ItemTemplate>
                                    <%# GetResource(Container).AvailableBy.FormatDate()%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.Info %>">
                                <ItemTemplate>
                                    <%# GetInfo(GetResource(Container))%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.Price%>">
                                <ItemTemplate>
                                    <%# GetResource(Container).SysResourceContext.Price%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="80px" HeaderText="<%$Content:EditAssignmentResources.ExpertSkillListStr %>">
                                <ItemTemplate>
                                    <%# GetResource(Container).ExpertSkillListStr.Comma2LineBreak()%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="80px" HeaderText="<%$Content:EditAssignmentResources.CustomerNameListStr %>">
                                <ItemTemplate>
                                    <%# GetResource(Container).CustomerNameListStr.Comma2LineBreak()%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.Remove %>">
                                <ItemTemplate>
                                    <ad:AdmLinkButton  runat="server" AutoBind="false" 
                                        Text="<%$Content:EditAssignmentResources.RemoveResource %>" 
                                         OnClick="OnClickRemoveResource"
                                         DataItemIndex="<%#Container.DataItemIndex %>"
                                        />
                                        
                                        </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.ExternalLink %>">
                                <ItemTemplate>
                                    <a target="_blank" href='<%# 
                                    ((AssignmentResource)Container.DataItem).RelatedExternalLinkObj!=null ? 
                                    CvmPages.
                                        ExternalPrintLink(((AssignmentResource)Container.DataItem).RelatedExternalLinkObj.LinkGuid).GetLinkAsHref()
                                        : ""
                                        %>'>
                                        <%# 
                                    ((AssignmentResource)Container.DataItem).RelatedExternalLinkObj!=null ? 
                                        Utl.Content("EditAssignmentResources.OpenLink")
                                            : ""
                                        %>
                                        </a>
                                        <ad:AdmLinkButton ID="AdmLinkButton1" runat="server" AutoBind="false" 
                                        Visible="<%#((AssignmentResource)Container.DataItem).RelatedExternalLinkIsNull() %>"  
                                        Text="<%$Content:EditAssignmentResources.CreateLink %>" 
                                         OnClick="OnClickCreateLink"
                                         DataItemIndex="<%#Container.DataItemIndex %>"
                                        />
                                        
                                        <ad:AdmLinkButton ID="AdmLinkButton2" runat="server" AutoBind="false" 
                                        Visible="<%#!((AssignmentResource)Container.DataItem).RelatedExternalLinkIsNull() %>"  
                                        Text="<%$Content:EditAssignmentResources.SendToClient %>" 
                                         OnClick="OnClickSendToClient"
                                         DataItemIndex="<%#Container.DataItemIndex %>"
                                        />
                                        </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </ad:AdmGridView>
            <h2>
                <%=Utl.ContentHlp("EditAssignmentResources.AddResource") %></h2>
            <%=Utl.ContentHlp("EditAssignmentResources.AddResourceIntro") %><br />
            <%=Utl.ContentHlp("EditAssignmentResources.EnterKeyword") %>

            <asp:TextBox runat="server" ID="KeywordTextBox" />
            <asp:Button runat="server" Text="<%$Content:EditAssignmentResources.Search %>" OnClick="OnClickSearchBtn" />
            <ad:AdmGridView runat="server" ID="SearchGrid" EnableViewState="true" OnRowCommand="OnRowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.FirstName %>">
                        <ItemTemplate>
                            <a href="<%#Utl.LinkHelper.GetPrintLink(Container.DataItem as Resource).GetLinkAsPopupScript() %>">
                                <%# (Container.DataItem as Resource).FirstName%></a></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.LastName %>">
                        <ItemTemplate>
                            <a href="<%#Utl.LinkHelper.GetPrintLink(Container.DataItem as Resource).GetLinkAsPopupScript() %>">
                                <%# (Container.DataItem as Resource).LastName%></a></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.AvailableBy %>">
                        <ItemTemplate>
                            <%# (Container.DataItem as Resource).AvailableBy.FormatDate()%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Content:EditAssignmentResources.Price%>">
                        <ItemTemplate>
                            <%# (Container.DataItem as Resource).SysResourceContext.Price%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="80px" HeaderText="<%$Content:EditAssignmentResources.ExpertSkillListStr %>">
                        <ItemTemplate>
                            <%# (Container.DataItem as Resource).ExpertSkillListStr.Comma2LineBreak()%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="80px" HeaderText="<%$Content:EditAssignmentResources.CustomerNameListStr %>">
                        <ItemTemplate>
                            <%# (Container.DataItem as Resource).CustomerNameListStr.Comma2LineBreak()%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="80px" HeaderText="<%$Content:EditAssignmentResources.Status %>">
                        <ItemTemplate>
                            <%# (Container.DataItem as Resource).ProfileStatusIdEnum.ToContent()%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField ButtonType="Link" CausesValidation="false" CommandName="Add" Text="<%$Content:EditAssignmentResources.AddResource %>" />
                </Columns>
            </ad:AdmGridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightPane" runat="server">
<my:EditAssignmentsAuxCtrl ID="AuxCtrl2" runat="server"/> 
</asp:Content>
