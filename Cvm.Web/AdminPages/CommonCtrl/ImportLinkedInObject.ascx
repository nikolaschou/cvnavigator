<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportLinkedInObject.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.ImportLinkedInObject" %>
<%@ Import Namespace="Cvm.Backend.Business.Customers" %>
<%@ Import Namespace="Cvm.Backend.Business.Import" %>
<%@ Import Namespace="Napp.Backend.BusinessObject" %>
<div>

<ad:ContainerCtrl runat="server"  ID="ContainerCtrl1">
<asp:Panel runat="server" ID="mainPanel">

    <% if (this._updatedObjects.Any())
       {%>
    <table class="niceTable">
        <asp:Repeater runat="server" ID="UpdatedLinkedInObjectsRep"> 
            <HeaderTemplate>
                <thead>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <%#Utl.Content("ImportCvDataCtrl.UpdatedLinkedIn" + this.ImportType + "Import")%>
                        </td>
                        <td>
                            <%#Utl.Content("ImportCvDataCtrl.UpdatedLinkedIn" + this.ImportType+"Origin")%>
                        </td>
                    </tr>
                </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked="true" />
                    </td>
                    <td>
                     <%#((IImportedObject)Container.DataItem).GetImportedSummaryHtml()%>
                    </td>
                    <td>
                     <%#((IImportedObject)Container.DataItem).GetOriginSummaryHtml()%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <br />
    <%
       }%>
    <% if (this._newObjects.Any())
       {%>
    <table class="niceTable">
        <asp:Repeater runat="server" ID="NewLinkedInObjectsRep">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <td colspan=2>
                            <%#Utl.Content("ImportCvDataCtrl.NewLinkedIn" + this.ImportType)%>
                        </td>
                    </tr>
                </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked="true" />
                    </td>
                    <td>
                     <%#((IImportedObject)Container.DataItem).GetImportedSummaryHtml()%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <br />
    <%
       }%>
    <ad:AdmButton ID="AdmButton1" CssClass="saveBtn" runat="server"  OnClick="OnClickImportObjects" />
    </asp:Panel>
</ad:ContainerCtrl>
    </div>