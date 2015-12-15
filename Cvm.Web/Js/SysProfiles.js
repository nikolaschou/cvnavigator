/* Runs through the matrix and builds a string representation of the selected skills
by building up an array of arrays, each entry having 1 for marked and 0 for unmarked.

*/
function collectState() {
    var result=new Array();
    $('#profileMatrix tr').each(
        function () {
            var row = new Array();
            $(this).find('td.selector').each(
                function () {
                    row.push($(this).hasClass('marked') ? 1 : 0);
                }
            );
            result.push(row);
        }
    );
    var resultJson = $.toJSON(result);
    $('#hiddenStateDiv :input').val(resultJson);
}

/*
Preparing page
*/
$(document).ready(
    function () {
        //Make sure all submits stores the current state in the hidden field so it is posted to the server
        $(document.forms[0]).bind('submit',
            function () {
                collectState();
            }
        );
        //Make autocompletion on skill-selector
        $('#addDiv :input:eq(0)').autocomplete(
            {
                source: function (data, cb) {
                    if (data.term.length>2) PageMethods.GetSkills(data.term, cb);
                }
            }
        );

        //Make all td's clickable
        $('#profileMatrix tbody tr')
            .find('td.selector')
            .click(
                function () {
                    $(this).toggleClass('marked');
                }
            ).mouseover(
                function (e) {
                    if (e.ctrlKey) {

                        $(this).addClass('marked');
                    } else if (e.shiftKey) {

                        $(this).removeClass('marked');
                    }
                }
            );
        //Handle deletion
        $('#profileMatrix img').click(
            function () {
                var skillId = $(this).attr('skillId');
                var outerThis = $(this);
                PageMethods.RemoveSysSkill(skillId,
                    function () {
                        var row = outerThis.closest('tr');
                        row.fadeOut('fast', 'linear', function () { row.remove(); });
                    },
                    function () {
                        alert("failure");
                    }
                );

            }
        );
    }
);

