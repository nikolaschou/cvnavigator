<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master"
    AutoEventWireup="true" CodeBehind="Profiles.aspx.cs" Inherits="Cvm.Web.Public.Profiles" %>

<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%=Utl.Content("Profiles.EnterEmail") %>
    <ad:AdmTextBox ValidationType="Email" runat="server" ID="EmailInput" />
    <br />
    <br />
    
            <%=Utl.Content("Profiles.SearchProfile") %><asp:TextBox runat="server" ID="SearchInput" OnTextChanged="OnChangeResourceNameSearch"
                AutoPostBack="true" />
            <ul class="niceList">
                <asp:Repeater ID="Repeater1" runat="server" DataSource="<%#this.GetResources() %>">
                    <ItemTemplate>
                        <li>
                            <b><%#((Resource)Container.DataItem).FullName %><br /></b>
                            <p style='width:300px;'>'<%#(((Resource)Container.DataItem)).ProfileResume %></p><br />
                            
                            <asp:Button ID="Button1" runat="server" OnClick="OnClickBookMeeting" CssClass="saveBtn"
                                CommandArgument="<%#((Resource)Container.DataItem).ResourceId %>" Text="<%$Content:Profiles.SetupMeeting %>" />
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
</asp:Content>
