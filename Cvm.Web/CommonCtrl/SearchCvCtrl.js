
function flipSkillIdSelected(skillId) {
    var leftEl=$('select#allSkillsList option[value='+skillId+']');
    var rightEl=$('select#selectedSkillsList option[value='+skillId+']');
    var wasSelected=(rightEl.length>0);
    if (wasSelected) {
        //Remove right element from DOM and remove class on left element
        rightEl.remove();
        if (leftEl.length>0) leftEl.removeClass('moved');    
    } else {
        leftEl.addClass('moved');
        leftEl.clone().removeClass('moved').prependTo($('select#selectedSkillsList'));
    }
    var isSelectedNow=!wasSelected;
    //Now update string-based idfr-list
    var hiddenEl=$('#skillsHiddenFieldSpan input');
    setIdInHiddenField(skillId,isSelectedNow,hiddenEl);      
}

$(document).ready(
function() {
    $('select#allSkillsList').live('change',
        function() {
            var list=$(this).find('option:selected');
            //alert(list.length);
            list.each(function() {
                var skillId=$(this).attr('value');
                $(this).attr('selected',false);
                flipSkillIdSelected(skillId);
            });
            return false;
        }
    );

    $('select#selectedSkillsList').live('change',
        function() {
            var list=$(this).find('option:selected');
            list.each(function() {
                var skillId=$(this).attr('value');
                flipSkillIdSelected(skillId);
            });
            return false;
        }
    );
}
);

