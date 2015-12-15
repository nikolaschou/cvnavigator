<%@ Page Language="C#" MasterPageFile="~/Public/Public.Master" AutoEventWireup="true" CodeBehind="SearchCv.aspx.cs" Inherits="Cvm.Web.Pages.SearchCv" Title="Untitled Page" %>
<%@ Import namespace="Napp.Web.AdminContentMgr"%>
<%@ Import namespace="Cvm.Backend.Business.Search"%>

<%@ Register Assembly="Napp.Web.AdmControl" Namespace="Napp.Web.AdmControl" TagPrefix="cc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="margin:20px">
<cc3:AdmGridView runat="server" ID="ResultsGrid" ParseAsResultSet="true">
    <Columns>
        
                <asp:TemplateField HeaderText="<%$Content:SearchCv.ResultTitle %>" >
                    <ItemStyle Width="150" />
                    <ItemTemplate><a class="listlink" href="<%#GetResultLink((ISearchResult)Container.DataItem) %>"><%#((ISearchResult)Container.DataItem).ResultTitle %></a></ItemTemplate>        
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="<%$Content:SearchCv.ResultType %>" >
                    <ItemStyle Width="100" />
                    <ItemTemplate><%#AdminContentMgr.instance.GetContent(((ISearchResult)Container.DataItem).ResultType)%></ItemTemplate>        
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="<%$Content:SearchCv.ResultSubType %>" >
                    <ItemStyle Width="200" />
                    <ItemTemplate><%#((ISearchResult)Container.DataItem).ResultSubType%></ItemTemplate>        
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="<%$Content:SearchCv.ResultDescription %>" >
                    <ItemStyle Width="500" />
                    <ItemTemplate><%#((ISearchResult)Container.DataItem).ResultDescription%></ItemTemplate>        
                </asp:TemplateField>

    </Columns> 
</cc3:AdmGridView>
</div>
<asp:GridView runat="server">
<Columns>

<asp:TemplateField ItemStyle-Width="200"><ItemStyle Width="200" /><ItemTemplate></ItemTemplate></asp:TemplateField>
</Columns>
</asp:GridView>
</asp:Content>
