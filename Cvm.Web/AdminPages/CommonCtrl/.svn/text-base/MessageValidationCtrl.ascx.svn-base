<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Control EnableViewState="false" Language="C#" AutoEventWireup="true" Codebehind="MessageValidationCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.MessageValidationCtrl" %>
<div>
        <div  id="messageBox" class="messages">
            <ul>
                <asp:Repeater runat="server" ID="MessageRep">
                    <ItemTemplate>
                        <li class="message">
                            <%#Container.DataItem.ToString() %>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <ul id="extraMessageList">
            </ul>
            <e:ExtValidationSummary ID="ExtValidationSummary1" runat="server"  />
        </div>

    <script language="javascript" type="text/javascript">
function showMessage(msg) {
    document.getElementById("refreshMessage").innerHTML='<%=AdminContentMgr.instance.GetContent("AdminMasterPage.PageUpdated") %>';
    var msg1='<%=AdminContentMgr.instance.GetContent("AdminMasterPage.PageUpdated") %>';
    var msg2='<%=AdminContentMgr.instance.GetContent("AdminMasterPage.PageUpdatedStatus") %>';
    window.status=msg2;
}
    </script>

    <div class="noprint">
        <div id="refreshMessage" style="color: Red">
        </div>
    </div>
</div>
