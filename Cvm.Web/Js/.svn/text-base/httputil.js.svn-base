//Contains common app methods.
function callAspnet(serviceName, aspxPath, data, callback, element) {
    //Must redefine the callback to cater for the asp.net convention about returning actual result data inside the d-field of the result object.
    var aspnetCallback = function (aspnetResult) {
        callback(aspnetResult.d);
    };
    $.ajax({
        type: "POST",
        url: rootPath + aspxPath + "/"+serviceName,
        data: $.toJSON(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: aspnetCallback,
        error: function (err) {
            var msg = $.evalJSON(err.responseText).Message;
            showPopupMessage(msg, element);
        }
    });
}

function globalService(serviceName, data, callback, sourceElement) {
    callAspnet(serviceName, "Public/AppWebServices.aspx", data, callback, sourceElement);
}

function sysService(serviceName, data, callback, sourceElement) {
    callAspnet(serviceName, "AdminPages/AdminWebServices.aspx", data, callback, sourceElement);
}

