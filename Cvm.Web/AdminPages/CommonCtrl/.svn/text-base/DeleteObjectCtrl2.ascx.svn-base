<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteObjectCtrl2.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.DeleteObjectCtrl2" %>
<%@ Import Namespace="Napp.Backend.AutoDeleteForm.Annotation" %>
<%@ Import Namespace="Napp.Backend.Business.Common" %>
<%@ Import Namespace="Napp.Backend.BusinessObject" %>
<%@ Register Assembly="Napp.Backend.AutoDeleteForm" Namespace="Napp.Backend.AutoDeleteForm" TagPrefix="auto" %>

<div>
    <h2><%#Utl.ContentHlp("DeleteObjectCtrl.NowDeleting",this.MyDeletionObject.ExtendedObjectTitle) %> </h2>
    <asp:Panel runat="server" ID="Panel1">
    <%#MyDeletionDataList.Count>0 ? Utl.ContentHlp("DeleteObjectCtrl.FoundRelatedObjects") : Utl.ContentHlp("DeleteObjectCtrl.NoRelatedObjects")%>
    <br />
    <div style="max-height:400px; overflow:auto">
    
    <asp:Repeater ID="Rep1" runat="server" DataSource="<%#this.MyDeletionDataList %>" EnableViewState="false">
        <ItemTemplate>
            <div style="white-space:nowrap">
                <span class='<%#"deleteItem"+(long)(((DeletionData) Container.DataItem).ParentBehaviorType) %>'>
                    <%#this.GetIndent(((DeletionData) Container.DataItem).RecurseDepth)%>
                    <%#BusinessObjectUtil.MakeTitleWithContext((IBusinessObject)((DeletionData)Container.DataItem).GetWrappedObject())%>
                </span>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    
    </div>
    <br />
    <%#MyDeletionData.HasAnyConflicts() ? Utl.ContentHlp("DeleteObjectCtrl.MustRefactor") : Utl.ContentHlp("DeleteObjectCtrl.ConfirmDeletion")%>
    <br />
    <br />
    <div >

    <asp:Panel ID="RefactorPanel" runat="server">
        <e:ExtDropDown runat="server" ID="RefactorDropDown2" EnableViewState="true" DataTextField="ExtendedObjectTitle" DataValueField="Id"/>
        
        <asp:Button runat="server" Text="<%$Content:DeleteObjectCtrl.RefactorBtn %>" OnClick="OnClickRefactorBtn" />
    </asp:Panel>
    </asp:Panel>
    <asp:Label runat="server" ID="DeleteLiteral" Text="<%$Content:DeleteObjectCtrl.ConfirmDeletion %>" Visible="false"/>
    <asp:Button ID="DeleteButton" runat="server" Text="<%$Content:DeleteObjectCtrl.DeleteBtn %>" OnClick="OnClickDeleteBtn" />
    <br />
    
    <asp:Button ID="CancelButton" runat="server" Text="<%$Content:DeleteObjectCtrl.CancelBtn%>" OnClick="OnClickCancelBtn" />
</div>