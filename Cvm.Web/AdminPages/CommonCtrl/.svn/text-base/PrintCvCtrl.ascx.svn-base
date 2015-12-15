<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintCvCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.PrintCvCtrl" %>
<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Cvm.Web.Code" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Cvm.Backend.Business.Search" %>
<%@ Import Namespace="Napp.Web.WebForm" %>
<%@ Import Namespace="Napp.Web.AutoForm" %>
<%@ Import Namespace="Cvm.Backend.Business.Print" %>
<%@ Import Namespace="Cvm.Backend.Business.Skills" %>
<%@ Import Namespace="Cvm.Backend.Business.Customers" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Import Namespace="Cvm.Web.AdminPages" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Register Assembly="Napp.Web.AdmControl" Namespace="Napp.Web.AdmControl" TagPrefix="cc3" %>
<%@ Register TagPrefix="Auto" Namespace="Napp.Web.AutoFormExt" Assembly="Napp.Web.AutoFormExt" %>
<%@ Register TagPrefix="uc" Src="~/AdminPages/CommonCtrl/PrintProjectCtrl.ascx" TagName="PrintProjectCtrl" %>
<%@ Register TagPrefix="uc" TagName="DiscProfileResultCtrl" Src="~/AdminPages/CommonCtrl/DiscProfileResultCtrl.ascx" %>
<div>
    <style type="text/css">
        body {
            font-size: 12px;
            margin-left: 20px;
            background-color: White;
            background-image: none;
            text-align: left;
        }

        div {
            border: 0px solid black;
        }

        td {
            padding-left: 5px;
        }

        .printH2 {
            text-align: left;
            font-weight: bold;
            margin-top: 20px;
            font-size: 14px;
            margin-bottom: 10px;
        }

        .printH1 {
            font-size: 14px;
            font-weight: bold;
            margin-bottom: 20px;
            margin-top: 30px;
            text-align: center;
            width: 100px;
            border-bottom: 1px solid #aaa;
        }

        .printDarkcell {
            background-color: #eeeeee;
            font-weight: normal;
            vertical-align: top;
        }

        .printTable {
            border: 1px solid #aaa;
        }

        .printSkillHeader, .printBold {
            font-weight: bold;
        }
    </style>
    <div style="background-color: White; width: 650px; overflow: hidden">
        <div class="noprint" style="width: <%=this.PageWidth%>px; text-align: right">
            <%=Utl.HasSysRole(RoleSet.SalesMgrAtLeast) || Utl.IsResourceOwner(this.MyResource) ? PageFunctions.GetCvLink(this.MyResource) : ""%><br />
            <ad:AdmHyperLink ID="AdmHyperLink1" runat="server" Visible="<%#Utl.HasSysRole(RoleSet.SalesMgrAtLeast) || Utl.IsResourceOwner(this.MyResource)%>"
                PageLink="<%#CvmPages.EditCvLink() %>" ContentId="PrintCv.EditCv" />
            <ad:AdmHyperLink ID="AdmHyperLink2" runat="server" Visible="<%#Utl.HasSysRole(RoleSet.SalesMgrAtLeast) || Utl.IsResourceOwner(this.MyResource)%>"
                PageLink="<%#CvmPages.PrintPreparePage.IncludeExistingParms() %>" ContentId="PrintCv.ChangeSettings" />
            <br />
            <br />
        </div>
        <table border="0" width="<%=this.PageWidth%>">
            <tr>
                <td>
                    <% if (MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.IncludeLogo))
                       { %>
                    <asp:Image runat="server" ID="LogoImage" Visible="false" />
                    <% } %>
                </td>
                <td style="vertical-align: bottom; text-align: right">
                    <%if ((!String.IsNullOrWhiteSpace(MyResource.PhotoUrl) || MyResource.PhotoFileRefId != null) && !this.MyPrintDefinition.IsAnonymous())
                      {%>
                    <img src="<%=(MyResource.PhotoFileRefId != null ? MyResource.RelatedPhotoFileRefObj.GetAsUrl() :  MyResource.PhotoUrl )%>" />
                    <%
                      }%>
                </td>
            </tr>
        </table>
        <h1 class="printH1" style="width: <%=this.PageWidth%>px; display: <%=String.IsNullOrEmpty(MyResource.ProfileTitle)?"none":"block"%>">
            <%#MyResource.ProfileTitle%>
        </h1>
        <table width="<%=this.PageWidth%>" border="0" cellspacing="5">
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetContent("PrintCv.Name")%>
                </td>
                <td>
                    <%#MakeAnonymName(MyResource)%>
                </td>
            </tr>
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetAddressContent(MyResource, "PrintCv.Address1")%>
                </td>
                <td>
                    <%#GetContactInfo(MyResource, "PrintCv.Address1")%>
                </td>
            </tr>
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetAddressContent(MyResource, "PrintCv.Address2")%>
                </td>
                <td>
                    <%#GetContactInfo(MyResource, "PrintCv.Address2")%>
                </td>
            </tr>
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetAddressContent(MyResource, "PrintCv.PostalCode")%>
                </td>
                <td>
                    <%#GetContactInfo(MyResource, "PrintCv.PostalCode")%>
                </td>
            </tr>
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetAddressContent(MyResource, "PrintCv.City")%>
                </td>
                <td>
                    <%#GetContactInfo(MyResource, "PrintCv.City")%>
                </td>
            </tr>
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetAddressContent(MyResource, "PrintCv.PhoneNumber1")%>
                </td>
                <td>
                    <%#GetContactInfo(MyResource, "PrintCv.PhoneNumber1")%>
                </td>
            </tr>
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetAddressContent(MyResource, "PrintCv.PhoneNumber2")%>
                </td>
                <td>
                    <%#GetContactInfo(MyResource, "PrintCv.PhoneNumber2")%>
                </td>
            </tr>
            <tr>
                <td style="width: <%=this.FirstColWidth%>px" class="printBold">
                    <%#GetAddressContent(MyResource, "PrintCv.AIMInfo")%>
                </td>
                <td>
                    <%#GetContactInfo(MyResource, "PrintCv.AIMInfo")%>
                </td>
            </tr>
            <% if (MyResource.BirthDate != null)
               {%>
            <tr>
                <td class="printBold">
                    <%#GetContent("PrintCv.BornYear")%>
                </td>
                <td>
                    <%#((MyResource.BirthDate != null) ? MyResource.BirthDate.Value.Year.ToString() : "") %>
                </td>
            </tr>
            <% }%>
            <tr>
                <td class="printBold">
                    <%#GetContent("PrintCv.Gender")%>
                </td>
                <td>
                    <%#GetContent("PrintCv.Gender" + MyResource.GenderIdEnum.ToString())%>
                </td>
            </tr>
            <% if (MyResource.Educations.Any())
               {%>
            <tr>
                <td class="printBold">
                    <%#GetContent("PrintCv.Educations")%>
                </td>
                <td>
                    <%#MyResource.EducationsStr %>
                </td>
            </tr>
            <%
               }%>
            <%if (MyResource.ProfessionalSince != null)
              {%>
            <tr>
                <td class="printBold">
                    <%#GetContent("PrintCv.ProfessionalSince")%>
                </td>
                <td>
                    <%#MyResource.ProfessionalSince %>
                </td>
            </tr>
            <%
              }%>
            <%if (!String.IsNullOrWhiteSpace(MyResource.ProfileResume))
              {%>
            <tr>
                <td colspan="2">
                    <br />
                    <blockquote>
                        <%#MyResource.ProfileResume %>
                    </blockquote>
                </td>
            </tr>
            <%
              }%>
        </table>
        <asp:Panel ID="LanguagePanel" runat="server">
            <h2 class="printH2">
                <%#GetContent("PrintCv.WorkingLanguage")%>
            </h2>
            <table width="<%=this.PageWidth%>" class="printTable" border="0" cellspacing="2">
                <tr class="printBold">
                    <td style="width: <%=this.FirstColWidth%>px">
                        <%#GetContent("PrintCv.Language")%>
                    </td>
                    <td width="170">
                        <%#GetContent("PrintCv.LevelOfSpeaking")%>
                    </td>
                    <td width="170">
                        <%#GetContent("PrintCv.LevelOfWriting")%>
                    </td>
                </tr>
                <tr>
                    <td style="width: <%=this.FirstColWidth%>px">
                        <%#MyResource.MotherTounge %>
                    </td>
                    <td width="233">
                        <%#GetContent("PrintCv.MotherTongue")%>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="Repeater5" DataSource="<%#MyResource.GetLanguageSkillsAsList() %>">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#((LanguageSkill) Container.DataItem).RelatedLanguageObj.LanguageName%>
                            </td>
                            <td>
                                <%#((LanguageSkill) Container.DataItem).RelatedLanguageSpeakingLevelObj%>
                            </td>
                            <td style="white-space: nowrap">
                                <%#((LanguageSkill) Container.DataItem).RelatedLanguageWritingLevelObj%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:Panel>
        <asp:Panel ID="Panel5" runat="server" Visible="<%#this.MyPrintDefinition.HasPrintOptions(CvPrintFlagEnum.IncludeSkills) %>">
            <h2 class="printH2">
                <%# GetContent("PrintCv.HeadlineSkill")%>
            </h2>
            <%#GetContent("PrintCv.ListOfSkills")%>
            <%#GetSkillLevels() %>
            <br />
            <br />
            <table width="<%=this.PageWidth%>" class="printTable" border="0" cellspacing="2">
                <tr class="printBold">
                    <td style="width: <%=this.FirstColWidth%>px">
                        <%#GetContent("PrintCv.SkillName")%>
                    </td>
                    <td width="80">
                        <%#GetContent("PrintCv.LastUsed")%>
                    </td>
                    <td width="80">
                        <%#GetContent("PrintCv.UsedInTotal")%>
                    </td>
                    <td>
                        <%#GetContent("PrintCv.Level")%>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="Repeater1" DataSource="<%#MyResource.GetResourceSkillsFiltered(this.MyPrintDefinition.ProfileTypeIds) %>">
                    <ItemTemplate>
                        <tr id="Tr1" runat="server" class="printDarkcell" visible="<%#IsNewSkillType(((ResourceSkill) Container.DataItem).SkillTypeName) %>">
                            <td colspan="5">
                                <%#((ResourceSkill) Container.DataItem).SkillTypeName%>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 10px;">
                                <%#((ResourceSkill) Container.DataItem).SkillName%>
                            </td>
                            <td>
                                <%#((ResourceSkill) Container.DataItem).LastUsed%>
                            </td>
                            <td>
                                <%#((ResourceSkill) Container.DataItem).UsedInTotal%>
                            </td>
                            <td style="white-space: nowrap">
                                <%#Utl.Content(((ResourceSkill) Container.DataItem).LevelEnum)%>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" Visible="<%#this.MyResource.HasDiscProfile() &&this.MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.IncludeBehavior) %>">
            <br />
            <uc:DiscProfileResultCtrl runat="server" MyResource="<%#this.MyResource %>" />
            <br />
            <br />
        </asp:Panel>
        <asp:Panel runat="server" Visible="<%#this.MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.IncludeProjectsGraph) && (this.MyResource.Projects.Count()+this.MyResource.Educations.Count>2) %>">
            <h2 class="printH2"><%=Utl.Content("PrintCvCtrl.ProjectsDiagram") %></h2>
            <div id="canvas" style="width: 619px; height: 619px; border: 1px solid grey; z-index: 100; overflow: visible"></div>
            <script>
                var redraw;
                var height = 600;
                var width = 600;
                /* only do all this when document has finished loading (needed for RaphaelJS */
                $(document).ready(function () {
                    var g = new Graph();
                    g.addNode('myself', { color: 'rgb(0,0,0)', size: '20', label: '<%=this.MyResource.FullName %>' });
                    <%foreach (var c in this.MyResource.GetDistinctProjectCustomers())
                      {%>
                    g.addNode('<%=c.CustomerName %>', { color: 'rgb(0,0,128)', size: '20', label: '<%=c.CustomerName %>' });
                    g.addEdge('myself', '<%=c.CustomerName %>', { color: 'rgb(0,0,128)', label: 'Label' });
                    
                    <% } %>
                                        <%foreach (var c in this.MyResource.Educations)
                                          {%>
                    g.addNode('<%=c.EducationName %>', { color: 'rgb(0,128,0)', size: '20', label: '<%=c.EducationName %>' });
                    g.addEdge('myself', '<%=c.EducationName %>', { color: 'rgb(0,128,0)', label: 'Label' });
                    
                    <% } %>
                    /* layout the graph using the Spring layout implementation */
                    var layouter = new Graph.Layout.Spring(g);
                    layouter.layout();
                    /* draw the graph using the RaphaelJS draw implementation */
                    var renderer = new Graph.Renderer.Raphael('canvas', g, width, height);
                    renderer.draw();
                    redraw = function () {
                        layouter.layout();
                        renderer.draw();
                    };
                });

            </script>
        </asp:Panel>
        <asp:Panel ID="Panel4" runat="server" Visible="<%#this.MyPrintDefinition.HasPrintOptions(CvPrintFlagEnum.IncludeProjects) %>">
            <h2 class="printH2">
                <%#(this.HasCustomerProjects() ? GetContent("PrintCv.CustomerProjects") : "") %>
            </h2>
            <asp:Repeater runat="server" ID="Repeater4" DataSource="<%#this.GetCustomerProjects()%>">
                <ItemTemplate>
                    <uc:PrintProjectCtrl ID="PrintProjectCtrl1" runat="server" PageWidth="<%#PageWidth %>"
                        FirstColWidth="<%#FirstColWidth %>" Project="<%#Container.DataItem as Project %>" />
                    <br />
                </ItemTemplate>
            </asp:Repeater>
            <h2 class="printH2">
                <%#(this.HasCustomerProjects() ? GetContent("PrintCv.RemainingProjects") : GetContent("PrintCv.AllProjects")) %>
            </h2>
            <asp:Repeater runat="server" ID="Repeater3" DataSource="<%#this.GetRemainingProjects()%>">
                <ItemTemplate>
                    <uc:PrintProjectCtrl ID="PrintProjectCtrl2" runat="server" PageWidth="<%#PageWidth %>"
                        FirstColWidth="<%#FirstColWidth %>" Project="<%#Container.DataItem as Project %>" />
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
        <asp:Panel ID="BackgroundPanel" runat="server" Visible="<%#this.MyPrintDefinition.HasPrintOptions(CvPrintFlagEnum.IncludeBackground) %>">
            <h2 class="printH2">
                <%# GetContent("PrintCv.HeadlineBackground")%>
            </h2>
            <table width="<%=this.PageWidth%>" border="0" cellpadding="5" class="printTable">
                <tr>
                    <td colspan="2" style="width: <%=this.FirstColWidth%>px" class="printDarkcell">
                        <%#GetContent("PrintCv.BackgroundTitle")%>
                    </td>
                    <td class="printDarkcell">
                        <%#GetContent("PrintCv.BackgroundPeriod")%>
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="Repeater2" DataSource="<%#this.GetResourceQualifications() %>">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#GetContent("ObjectTypes." + ((ISearchResultWithDate)Container.DataItem).ResultType.ToString())%>:
                                <i>'<%#((ISearchResultWithDate) Container.DataItem).ResultTitle%>',</i>
                            </td>
                            <td>
                                <%#((ISearchResultWithDate) Container.DataItem).ResultDescription%>
                            </td>
                            <td>
                                <%#((ISearchResultWithDate) Container.DataItem).ResultPeriod%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:Panel>
    </div>
    <br />
</div>
