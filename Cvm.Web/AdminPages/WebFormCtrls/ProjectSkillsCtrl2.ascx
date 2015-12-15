<%@ Import Namespace="Napp.Web.AdminContentMgr" %>
<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ProjectSkillsCtrl2.ascx.cs"
    Inherits="Cvm.Web.AdminPages.WebFormCtrls.ProjectSkillsCtrl2" %>
<div>
    <ad:AdmLinkButton runat="server" CausesValidation="false" ContentId="ProjectSkillsCtrl.MarkAll"
        ID="MarkAllBtn" OnClick="OnClickMarkAllBtn" />
    <ad:AdmLinkButton runat="server" CausesValidation="false" ContentId="ProjectSkillsCtrl.MarkNone"
        ID="MarkNoneBtn" OnClick="OnClickMarkNoneBtn" />
    <ad:AdmLinkButton Visible="false" ID="AdmLinkButton1" runat="server" ContentId="ProjectSkillsCtrl2.AddNewSkillBtnTitle"
        CausesValidation="false" OnClick="OnClickOpenSkillWizardBtn" />
    <span class="subformSmart">
        <ad:AdmTextBox runat="server" ID="AddSkillTextBox" CausesValidation="false" autocallback="globalService" webService="GetSkills" />
        <ad:AdmButton UseSubmitBehavior="false"  runat="server"  OnClick="OnClickAddSkillBtn" CausesValidation="false" />
    </span>
    <br />
    <ad:AdmCheckBoxList EnableViewState="true" RepeatLayout="Table" RepeatColumns="2"
        RepeatDirection="Vertical" CellPadding="4" AutoActivate="false" DataTextField="ExtendedObjectTitle"
        DataValueField="SkillId" runat="server" ID="SkillsCheckBoxList" />
    <br />
    <asp:Panel ID="NewSkillPanel" Visible="false" runat="server">
        <div class="dialog" id="addSkillDiv" title='<%= Utl.Content("ProjectSkillsCtrl2.AddNewSkillTitle")%>'>
            <%=Utl.Content("ProjectSkillsCtrl2.EnterSkill") %>
            <ad:AdmValidationArea ID="AdmValidationArea1" runat="server" />
            <input type="text" />
            <div style="height: 220px">
                <asp:ListBox runat="server" Rows="10" ID="SkillListBox" Height="200" Width="400"
                    SelectionMode="Multiple" />
                <br />
            </div>
            <div id="btnDiv">
                <ad:AdmButton ID="AdmButton2" runat="server" CausesValidation="false" OnClientClick="javascript:submitSkillForm()"
                    ContentId="ProjectSkillsCtrl2.AddSelSkills2" /></div>
            <br />
            <br />
            <span id="span3" style="display: none;">
                <%=Utl.Content("ProjectSkillsCtrl2.CantFindSkill1") %>
                '<span id="span1"></span>'
                <%=Utl.Content("ProjectSkillsCtrl2.CantFindSkill2") %>
                <ad:AdmLinkButton ID="AdmButton1" CssClass="stdLink" runat="server" ContentId="ProjectSkillsCtrl2.AddNewSkill"
                    CausesValidation="false" OnClick="OnClickAddNewSkillBtn" />
            </span>
        </div>
        <span id="selectedSkillIds" style="display: none">
            <asp:TextBox ID="SelectedSkillIdsBx" runat="server" OnTextChanged="OnClickAddSelSkillsBtn"
                AutoPostBack="true" CausesValidation="true" />
            <asp:TextBox ID="SkillTextBox2" runat="server" />
        </span>
    </asp:Panel>
    <script language="javascript">

        function setVisibility(optionEl, filterText) {
            if (optionEl.text().toLowerCase().indexOf(filterText) > -1) {
                optionEl.show();
            } else {
                optionEl.hide();
            }
        }

        //Executed when "Add selected skills" button is clicked
        function submitSkillForm() {
            var selectedSkillIds = "";
            var selectedSkillOptions = $('#addSkillDiv select option:selected:visible');
            if (selectedSkillOptions.size() == 0) {
                alert('Please select at least one skill');
                return false;
            } else {
                selectedSkillOptions.each(
                        function () {
                            var optionEl = $(this);
                            var skillId = optionEl.val();
                            selectedSkillIds += skillId + ",";
                        }
                    );
            }
            $('#selectedSkillIds :text:eq(0)').val(selectedSkillIds);
            $('#selectedSkillIds :text:eq(0)').change();
        }


        var isFiltering = false;
        $(document).ready(
        function () {
            $('#addSkillDiv').dialog('open');
            var textBox = $('#addSkillDiv :text');
            textBox.focus();
            if (textBox.val() == '<%=Utl.Content("ProjectSkillsCtrl2.EnterSkill") %>') {
                textBox.val('');
            }
            //Bind onchange event for filter text box
            textBox.bind('keyup', function () {
                //Make primitive synch
                if (!isFiltering) {

                    isFiltering = true;
                    var filterText = textBox.val().toLowerCase();
                    if (filterText.length > 2) {
                        //Execute filter
                        $('#addSkillDiv select option').each(function () { setVisibility($(this), filterText); });
                        $('#span1').text(textBox.val());
                        var hasAnyMatch = ($('#addSkillDiv select option:visible').size() > 0);
                        if (hasAnyMatch) {
                            $('#btnDiv input').attr('disabled', false);
                        }
                        else {
                            $('#btnDiv input').attr('disabled', true);
                        }
                        $('#span3').slideDown('fast');
                    } else {
                        $('#addSkillDiv select option:hidden').show();
                        $('#btnDiv input').attr('disabled', false);
                        $('#span3').slideUp('fast');
                    }

                    isFiltering = false;
                }
            });
            //Simulate keyup event to initialize field
            textBox.keyup();
            textBox.change(function () {
                $('#selectedSkillIds :text:eq(1)').val($(this).val());
            });
        }
        );
    </script>
</div>
