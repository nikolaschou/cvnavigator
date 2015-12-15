<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true"
    CodeBehind="DbInstaller.aspx.cs" Inherits="Cvm.Web.AdminPages.DbVersions.DbInstaller"
    Title="Untitled Page" %>

<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table border="0">
        <tr>
            <td>
                <h2>
                    Pending versions:</h2>
                <ul>
                    <asp:Repeater runat="server" ID="rep1" OnItemCommand="OnClickSkipLinkBtn">
                        <ItemTemplate>
                            <li>
                                <%#(Container.ItemIndex==0 ? "<b>" : "") %>
                                <asp:LinkButton runat="server" Text="<%$Content:DbInstaller.Skip %>" />
                                <%#Container.DataItem.ToString() %>
                                <%#(Container.ItemIndex==0 ? "</b>" : "") %>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </td>
            <td style="border-left: 1px solid gray">
                <h2>
                    Installed versions:</h2>
                <ul>
                    <asp:Repeater runat="server" ID="rep2">
                        <ItemTemplate>
                            <li>
                                <%#(Container.ItemIndex==0 ? "<b>" : "") %>
                                <%#Container.DataItem.ToString() %>
                                <%#(Container.ItemIndex==0 ? "</b>" : "") %>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="PendingContentBox" 
                        runat="server" EnableViewState="false" TextMode="MultiLine" Columns="40" Rows="5"
                        Text="<%#this.GetPendingFileContent() %>"
                    />
            </td>
            <td>
                    <asp:TextBox  ID="InstalledContentBox" 
                        runat="server" EnableViewState="false" TextMode="MultiLine" Columns="40" Rows="5"
                        Text="<%#this.GetInstalledFileContent() %>"
                    />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left">
                <asp:Button runat="server" Text="<%$Content:DbInstaller.MoveAllPending %>" OnClick="OnClickMoveAllPendingBtn" />
            </td>
        </tr>
    </table>
    <hr />
    <br />
    <table border="0">
    <asp:Repeater runat="server" ID="SqlRep" OnItemCommand="OnClickRunSqlBtn">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="SqlTextBox" runat="server" TextMode="MultiLine" Font-Size="X-Small" Rows="<%#this.CountLines(Container.DataItem.ToString()) +1 %>" Columns="150"
                        Text="<%#Container.DataItem.ToString() %>" /> 
                </td>
            </tr>
            <tr>
                <td style="border-bottom: 1px solid gray">
                    <asp:Button runat="server" ID="runSqlBtn" Text="<%$Content:DbInstaller.RunSql %>" />
                    <asp:CheckBox runat="server" ID="AutoRunCheckBox" Text="<%$Content:DbInstaller.AutoRun %>"
                        Checked="true" />
                        <asp:Label runat="server" ID="MessageLabel" />
                        <br /><br />
                </td>
            </tr>
            
        </ItemTemplate>
    </asp:Repeater>
<tr>
<td colspan="2" align="left">
<asp:Button ID="Button1" runat="server" Text="<%$Content:DbInstaller.RunAllSql %>" OnClick="OnClickRunAllSqlBtn" />
    </td>
</tr>
    </table>
    <hr />
    <h2>
        <%=AdminContentMgr.instance.GetContent("DbInstaller.CustomSql") %></h2>
    <asp:TextBox runat="server" ID="CustomSqlTextBox" TextMode="MultiLine" Columns="80" Rows="5"/>
    <asp:Button runat="server" OnClick="OnClickRunCustomSqlBtn" Text="<%$Content:DbInstaller.RunCustomSql %>" />
    <br />
    <asp:GridView runat="server" ID="CustomSqlGrid" AutoGenerateColumns="true" />
</asp:Content>
