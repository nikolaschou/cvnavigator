<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgressBar.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.ProgressBar" %>
    <div class="progressBar">
        <asp:Repeater runat="server" ID="ProgressBarRep" EnableViewState="false" DataSource="<%#this.Titles %>">
            <ItemTemplate>
                <div class='<%#(Container.ItemIndex==this.ChosenIndex?"selected":"") %>'>
                    <%#Container.DataItem %>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>