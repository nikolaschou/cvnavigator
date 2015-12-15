//Attaches an eventlistenter to a given object. 
 //Compatible with both explorer, w3c and NN6.
 function addEvent(myobject, eventName, func) {
 	if (myobject.attachEvent) {// Explorer
			myobject.attachEvent(eventName, func);
	}
	if (myobject.addEventListener) { //W3C
			myobject.addEventListener(eventName, func, true);
	} 
 }


function setFocus(el) {
    el.focus();
}

function setFocusToFirstInput() {
    var els=document.getElementsByTagName("input");
    for (var i=0;i<els.length;i++) {
        if (els[i].type=="text" && els[i].id!='searchText') {
            setFocus(els[i]);
            return;
        }
    }
}

//The interval in seconds by which the session will be kept alive.
var KEEP_ALIVE_INTERVAL_SECONDS=600;

//Keeps the session alive
function keepAlive() {
    jQuery.get("../Public/KeepAlive.aspx");
}

//Call this method at startup to keep the session alive.
function keepAliveSchedule() {
    window.setInterval("keepAlive()",1000*KEEP_ALIVE_INTERVAL_SECONDS);
}



function showMessage(msg) {
    document.getElementById("extraMessageList").innerHTML = '<li class="message">' + msg + '</li>';
}

function showPopupMessage(msg, element) {
    var el = $('#popupMessage');
    if (el.length == 0) {
        el = $("<div id='popupMessage' style='display:none' title='Press Escape to close or click outside'></div>")
            .appendTo('body')
            .draggable()
            .click(function (e) { e.stopImmediatePropagation(); return true; });
        $(document).click(function () { el.slideUp(); })
            .keypress(function (e) { if (e.which == 0) el.slideUp(); });
    }
    el.text(msg);
    if (false) {
        el.position({my: "left top", at:"left bottom", of: element, offset: "3 3", collision: "fit"});
    } else {
        el.position({my:"center top", at:"center center", of:$('body'), offset: "0 200", collision: "fit"})
    }
    el.slideDown();
}

