//Used as a global constant for a delimitier of id seperators
var ID_SEPERATOR=",";
var FILTER_VALUE="<filter>";


    
jQuery.fn.center = function () {
    //this.css("position","absolute");
    this.css("top", ( $(window).height() - this.height() ) / 2+$(window).scrollTop() + "px");
    this.css("left", ( $(window).width() - this.width() ) / 2+$(window).scrollLeft() + "px");
    return this;
}



$(document).ready(
    function () {
        //Initialize popupContainers
        $('.popupContainer').hide();
        $('.popupContainer').slideDown(300);
        $('.popupContainerOpener').click(function () { $('.popupContainer').slideDown(500); });

        //Initialize all date-pickers using jquery-ui
        $('.datefield').datepicker({
            closeAtTop: false, useShortYear: false,
            prevText: 'Previous', nextText: 'Next',
            yearRange: '1940:2030', firstDay: 1,
            numberOfMonths: [1, 1], rangeSelect: false,
            stepMonths: 1, dateFormat: 'dd-mm-yy',
            changeYear: true, showAnim: 'fadeIn',
            direction: 'right',
            showOn: 'both', buttonImageOnly: true, showOn: 'button',
            buttonImage: '../images/master/pickDate.PNG', buttonText: 'Calendar',
            gotoCurrent: true,
            showWeek: true,
            showButtonPanel: true,
        });


        //Initialize all date-pickers using jquery-tools
        /*$('.datefield').dateinput({
        format: 'dd-mm-yyyy',
        offset: [10, 0],
        selectors: true,             	// whether month/year dropdowns are shown
        speed: 'fast',               	// calendar reveal speed
        firstDay: 1,
        min: -36500,
        yearRange: [-70, 20]
        });*/

        /* Nicelinks
        ******************************************/
        $('.niceLink').click(function (e) {
            var els = this.getElementsByTagName("input");
            if (els.length > 0) els[0].click();
            var anchors = this.getElementsByTagName("a");
            if (anchors.length > 0) window.location.href = anchors[0].href;
        });


        /* Make div-based subforms work by preventing submit and keypress events to propagate up from the div-element.
        ******************************************/

        $('divsubform').keypress(function () {
            return false;
        });
        $('divsubform').submit(function () {
            return false;
        });

        /* Filter text box 
        ******************************************/

        var clearFilter = function (inputEl) {
            if ($(inputEl).val() == FILTER_VALUE) {
                $(inputEl).val('');
            }
        }
        $('.filter').click(function () {
            clearFilter(this);
        });

        $('.filter').keydown(function () {
            clearFilter(this);
        });
        //Avoid enter submits surrounding form
        $('.filter').keypress(function (event) {
            if (event.which == 13) {
                $(this).blur();
                return false;
            }
            else return true;
        });

        /*** Handle all input fields with an attribute 'autocallback' as autocomplete fields in a generic way 
        Assumes the input field looks like something this:
        input type='text' autocallback="globalService" webservice="SomeServerSideMethod"
        where autocallback can be either globalService or sysService.
        ***/

        $('input[autocallback]').each(
                function () {

                    //The name of the global function and the server side webservice is written as a DOM-attributes
                    var serviceFunction = window[$(this).attr('autocallback')];
                    var webservice = $(this).attr('webService');
                    $(this).autocomplete(
                        {
                            source: function (data, cb) {

                                if (data.term.length > 2) serviceFunction(webservice, data.term, cb);
                            }
                        }
                );
                }
        );



        /* AdmDialog Javascript code
        ******************************************/
        $('.dialog').dialog({ autoOpen: false, modal: true, width: "500px" }).parent().appendTo($("form:last"));

        $(".dialog.opened").dialog('open');
        $(".dialog.closed").dialog('close');

        /* Schedule moveValidationMessages */
        window.setTimeout("moveValidationMessages()", 50);

        /* Handle form-submitter buttons
        ******************************************/
        $(".formSubmitter").click(function () {
            $(this).closest('body').find('form').submit();
        });

        /* Handle subform divs, i.e. divs catching all enter *****/
        $('.subformDiv').keypress(
            function (e) {
                if (e.which == 13) {
                    document.forms[0].submit();
                }
            }
        );
        /* Smart subforms assumes that a button with server side events is present inside the form */
        $('.subformSmart').each(
                function () {
                    var subform = $(this);
                    var button = subform.find(':button:eq(0)');
                    if (button.length == 0) button = subform.find(':submit:eq(0)');
                    $(this).keypress(
                        function (e) {
                            if (e.which == 13) {
                                button.click();
                                e.stopImmediatePropagation();
                            }
                            return true;
                        }
                    );
                    button.hide();
                    var textBox = subform.find(':text:eq(0)');
                    textBox.after('<span style="display:none;color:gray"> Press enter</span>');
                    function handler() {
                        textBox.next().show();
                    }
                    function dehandler() {
                        textBox.next().hide();
                    }
                    textBox.focus(handler);
                    textBox.keypress(handler);
                    textBox.blur(dehandler);

                }
            );




        /* Handle datagrid tables
        ******************************************/
        $("table.datagrid1 tr:odd").addClass("odd");


        /********** Handle fields with autocompletion *********/
        $('input[autocallback]').each(
                function () {
                    var inputRef = $(this);
                    $(this).autocomplete(
                        {
                            source: function (data, cb) {
                                //The name of the global function is written as a DOM-attribute 
                                var serviceFunction = window[inputRef.attr('autocallback')];

                                if (data.term.length >= 2) serviceFunction(inputRef.attr('webService'), { arg0: data.term }, cb, inputRef);
                            }
                        }
                );
                }
        );

        //Handle history
        $('#searchText').focus(function () { $('#historyItems').slideDown(); }).blur(function () { $('#historyItems').slideUp(); });

    }
);

    /* Handle validation and messages
    *  Move the message-box to the last visible validation area
    ******************************************/
    function moveValidationMessages() {
        var validationAreas = $('.validationArea:visible');
        if (validationAreas.size() > 0) {
            $('#messageBox').prependTo(validationAreas.get(-1));
        }
    }

/* Handling hidden fields as a cheap container with ID-lists
*****************************************************/

function setIdInHiddenField(id, isSelected, hiddenEl) {
    var idToken=id+ID_SEPERATOR;
    var contains=hiddenEl.val().indexOf(idToken)>-1;
    if (isSelected && !contains) {
        hiddenEl.val( hiddenEl.val()+ idToken);
    }
    if (!isSelected && contains) {
        hiddenEl.val(hiddenEl.val().replace(idToken,""));      
    }

}

/* Handle warning signs */
$(document).ready(
    function () {
        $('.warningDiv').show();
    }
);

