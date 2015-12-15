<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportCvDataCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.ImportCvDataCtrl" %>
<%@ Register Src="~/AdminPages/CommonCtrl/ImportSkillsCtrl.ascx" TagPrefix="uc" TagName="ImportSkillsCtrl" %>
<%@ Register Src="~/AdminPages/CommonCtrl/ImportLinkedInCtrl.ascx" TagPrefix="uc"
    TagName="LinkedInCtrl" %>
<%@ Register TagPrefix="uc" TagName="ImportLinkedInObject" Src="~/AdminPages/CommonCtrl/ImportLinkedInObject.ascx" %>
<%@ Import Namespace="Cvm.Backend.Business.Customers" %>
<%@ Import Namespace="Cvm.Backend.Business.Import" %>
<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Import Namespace="Cvm.Web.Code" %>

<ad:ContainerCtrl ID="ContainerCtrl1" runat="server" ContentId="ImportCvDataCtrl.DataImport">
    <fieldset>
        <legend>
            <%=Utl.Content("ImportCvDataCtrl.ImportWordPdfEtc") %></legend>
        <asp:FileUpload runat="server" ID="CvFileUpload" />
        <ad:AdmButton runat="server" CausesValidation="false" ContentId="ImportCvDataCtrl.Upload"
            OnClick="OnClickUploadCv" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Panel ID="ImportPanel" runat="server">
                    <%=AdminContentMgr.instance.GetContentWithHelpTextAsHtml("EditCv.ImportText") %><br />
                    <asp:TextBox TextMode="MultiLine" Columns="100" Rows="6" runat="server" ID="ImportTextBox"
                        AutoPostBack="true" OnTextChanged="OnTextChangedImportTextBox" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%=PageFunctions.GetCvLink(MyResource) %>
    </fieldset>
    <fieldset>
        <legend>
            <%=Utl.Content("ImportCvDataCtrl.ImportLinkedIn") %></legend>
        <%=Utl.ContentBread("ImportLinkedInCtrl.Intro"+(this.MyResource.HasImportedLinkedInData()? "WithPrevious" : "NoPrevious")) %>
        <uc:LinkedInCtrl runat="server" ID="LinkedInCtrl" />
        <%if (this.MyResource.HasImportedLinkedInData() && Mgr.HasNewData())
          {%>
        <%=Utl.Content("ImportCvDataCtrl.ImportAllLinkedIn")%>
        <asp:Button runat="server" Text="<%$Content:ImportCvDataCtrl.ImportAllBtn %>" OnClick="OnClickImportAllBtn" CssClass="saveBtn" />
        <%
          }%>
    </fieldset>
</ad:ContainerCtrl>
<ad:ContainerCtrl ID="ContainerCtrl3" runat="server" ContentId="ImportCvDataCtrl.ImportBaseData">
    <%
        var baseData = this.Mgr.BaseData;
        bool canImportPhoto = !String.IsNullOrWhiteSpace(baseData.PhotoUrl) && baseData.PhotoUrl != MyResource.PhotoUrl;
        bool canImportTitle = !String.IsNullOrWhiteSpace(baseData.Headline) &&
                              baseData.Headline != MyResource.ProfileTitle;
        %>
    <%
        if (canImportPhoto)
        {%>
    <b>
        <%=Utl.Content("ImportCvDataCtrl.Photo") %>:</b>
    <br />
    <img src="<%=baseData.PhotoUrl%>" />
    <br />
    <asp:Button ID="Button1" runat="server" CssClass="saveBtn" Text="<%$Content:ImportCvDataCtrl.ImportPhoto %>"
        OnClick="OnClickImportPhotoBtn" />
    <br />
    <br />
    <%}%>
    <%
        if (canImportTitle)
        {%>
    <b>
        <%=Utl.Content("ImportCvDataCtrl.ProfileTitle") %>:</b>
    <br />
    <em>
        <%=baseData.Headline%></em>
    <br />
    <asp:Button ID="Button2" runat="server" CssClass="saveBtn" Text="<%$Content:ImportCvDataCtrl.ImportHeadline %>"
        OnClick="OnClickImportHeadlineBtn" />
    <br />
    <%}%>
    <% if (!canImportTitle && !canImportPhoto)
       {%>
       <%=Utl.Content("ImportCvDataCtrl.NothingToImport") %>
    <%
       }%>
</ad:ContainerCtrl>
<ad:ContainerCtrl ID="ContainerCtrl2" runat="server" ContentId="ImportCvDataCtrl.ImportSkills">
    <uc:ImportSkillsCtrl runat="server" ID="MyImportSkillsCtrl" />
</ad:ContainerCtrl>
<uc:ImportLinkedInObject runat="server" ID="ImportProjectsCtrl" ImportType="Project" />
<uc:ImportLinkedInObject runat="server" ID="ImportEducationsCtrl" ImportType="Education" />
<uc:ImportLinkedInObject runat="server" ID="ImportCertificationsCtrl" ImportType="Certification" />
<ad:ContainerCtrl ID="ContainerCtrl4" runat="server" ContentId="ImportCvDataCtrl.ImportLanguages">
    <asp:Label EnableViewState="false" runat="server" ID="LangLitteral" />
    <asp:Button ID="Button3" runat="server" visible="false" CssClass="saveBtn" Text="<%$Content:ImportCvDataCtrl.ImportLanguage %>" OnClick="OnClickImportLanguage"/>
</ad:ContainerCtrl>