//Contains common app methods.
function callAspnet(serviceName, data, handler) {
    $.ajax({
        type: "POST",
        url: rootPath + serviceName,
        data: $.toJSON(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: handler,
        error: function (err) {
            var msg = err.responseText.substring('{"Message":"'.length, err.responseText.indexOf('","'));
            alert(msg);
        }
    });
}
//Contains quick references for all elements with an id.
//Makes E.xxx a short cut for $('#xxx')
var E = new Object();
$(document).ready(
    function () {
        $('*[id]').each(
            function () {
                E[$(this).attr('id')] = $(this);
            }
        );
    }
);
    