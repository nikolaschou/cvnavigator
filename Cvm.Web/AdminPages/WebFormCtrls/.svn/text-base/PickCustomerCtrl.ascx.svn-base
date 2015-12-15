<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PickCustomerCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.WebFormCtrls.PickCustomerCtrl" %>
<div style="height:30px;position:relative;z-index:100">
    <div id="pickCustDiv" style="position:absolute;z-index:100">
    <ad:AdmDropDown ID="CustomerDropDown" IncludeBlank="true" runat="server" AutoActivate="true"
        EntityName="Customer" />
        <a href="javascript:createCust()" title="<%=Utl.ContentHlp("Standard.ClickToCreateANewEntry") %>">
                <%=Utl.ContentHlp("PickCustomerCtrl.CreateCustomer") %></a>
                </div>
    <div id="newCustSpan" style="display: none;position:absolute;z-index:100">
        <ad:AdmTextBox ID="NewCustomerNameBx" runat="server" />
        <a href="javascript:cancelCreateCust()">
            <%=Utl.ContentHlp("PickCustomerCtrl.Cancel") %></a> </div>
            
    <script language="javascript">
        function cancelCreateCust() {
            $('#newCustSpan').fadeOut('fast');
            $('#newCustSpan input').first().val('');
            $('#pickCustDiv').first().fadeIn('fast'); //.fadeIn('fast');
        }

        function createCust() {
            $('#pickCustDiv').first().fadeOut('fast'); //.fadeOut();
            $('#newCustSpan').fadeIn('fast');// fadeIn('fast');
            $('#newCustSpan input').first().focus();

        }
        $(document).ready(
        function () {
            if (!$('#newCustSpan input').val() == "") {
                createCust();
            }
            /*$('#newCustSpan input').first().bind('focus', function () {

            });*/
        }
    );
    </script>
</div>
