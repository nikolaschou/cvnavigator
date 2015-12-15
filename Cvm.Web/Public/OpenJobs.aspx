<%@ Page Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage2.master" AutoEventWireup="true" CodeBehind="OpenJobs.aspx.cs" Inherits="Cvm.Web.Public.OpenJobs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftPane" runat="server">
    <ad:ContainerCtrl ID="TextSearch" runat="server" ContentId="<%$Content:OpenAssignments.FreeTextSearch %>">
        <asp:Label ID="Label2" runat="server" Text="<%$Content:OpenAssignments.FreeTextSearch %>"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </ad:ContainerCtrl>

    <ad:ContainerCtrl ID="Region" runat="server" ContentId="<%$Content:OpenAssignments.Region %>">
        <asp:ListView runat="server" ID="LVRegion">
            <ItemTemplate>
                <asp:CheckBox ID="CBRegion" runat="server" OnClick="LBDeleteUser_Click" Text='<%#Eval("jobRegionFlagName") %>' />  <br/>
            </ItemTemplate>
        </asp:ListView>
    </ad:ContainerCtrl>

    <div class="buttons">
        <ad:AdmButton ID="ABUpdate" CssClass="saveBtn" runat="server" CausesValidation="true" Text="<%$Content:OpenAssignments.Update %>" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MiddlePane" runat="server">
    <asp:ListView runat="server" ID="ListView1">
        <ItemTemplate>
            <ad:ContainerCtrl ID="JobContainer" runat="server" ContentId='<%#Eval("title")%>'>
                <asp:LinkButton ID="LBApplyJob" runat="server" OnClick="LBApplyJob_Click" Text='<%#TruncateAtWord(Eval("description").ToString(), 100)%>' CommandArgument='<%#Eval("announcementId") %>' OnClientClick="aspnetForm.target ='_blank';" ></asp:LinkButton>
            </ad:ContainerCtrl>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>

