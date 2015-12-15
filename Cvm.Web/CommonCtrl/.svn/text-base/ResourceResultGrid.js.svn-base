function setChosenResource(resourceId, isSelected) {
    var hiddenField=$('#selResourcesHiddenFieldSpan input');
    setIdInHiddenField(resourceId,isSelected,hiddenField);
}

$(document).ready(
//Makes sure that all checkboxes is assigned a click-event which will register all checked resourceIds.
function() {
    //Matches all table rows
    var trs=$('#resourceResultGridDiv tr');
    trs.click(
        function(event) {
            
            var cb;
            var didClickCheckbox=$(event.target).is(":checkbox");
            if (didClickCheckbox) cb=$(event.target);
            else cb=$(this).find(':checkbox');
            //Flip the checked-value if not automatically so
            if (!didClickCheckbox) cb.attr('checked',!cb.attr('checked'));
            setChosenResource(cb.attr('resourceId'),cb.attr('checked'));
            return true;
        }
    );
}
);
