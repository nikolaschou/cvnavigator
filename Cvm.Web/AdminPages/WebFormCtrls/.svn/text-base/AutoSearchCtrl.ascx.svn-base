<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AutoSearchCtrl.ascx.cs"
    Inherits="Nshop.Web.AdminPages.WebFormCtrls.AutoSearchCtrl" %>
<%@ Register Assembly="Napp.Web.AutoForm" Namespace="Napp.Web.AutoForm" TagPrefix="auto" %>
<%@ Register TagPrefix="Ex" Namespace="Napp.Web.AdmControl" Assembly="Napp.Web.AdmControls" %>
<Ex:ContainerCtrl ID="ContainerCtrl1" runat="server">
    <table rules="rows" border="1">
        <tr>
            <td>
                <%=Utl.Content("EditAll.ChooseId") %>
                <Ex:AdmTextBox runat="server" ValidationType="Integer" ID="ObjectIdInput" />
                <Ex:AdmButton ID="AdmButton1" CausesValidation="true" runat="server" OnClick="OnClickLookupById"
                    ContentId="EditAll.LookupById" />
            </td>
        </tr>
        <tr>
                    <td>
                        <auto:AutoSearchSimple runat="server" ID="autoSearchCtrl" />
                        <Ex:AdmButton ID="AdmButton3" CausesValidation="false" runat="server" OnClick="OnClickSearchBtn"
                            ContentId="EditAll.Search" />
                        <Ex:AdmGridView runat="server" ID="SearchGrid" AutoGenerateColumns="true" AutoGenerateSelectButton="true"
                            OnSelectedIndexChanged="SearchListSelectChanged" />
                    </td>
        </tr>
    </table>
</Ex:ContainerCtrl>
