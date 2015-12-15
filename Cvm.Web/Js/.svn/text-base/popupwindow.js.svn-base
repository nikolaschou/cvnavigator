var layerOuterDiv="layerOuterDiv";
var layerRootEl="layerRootEl";

function closePopup() {
    document.getElementById(layerOuterDiv).style.display="none";
}

/*
Requires that PopupLayerCtrl is added to the page.
*/
function showPopup(htmlFragment) {
    document.getElementById(layerRootEl).innerHTML=htmlFragment;
    document.getElementById(layerOuterDiv).style.display="block";
    
}

function openPopupWindow(url, title) {
    var options='toolbar=yes,menubar=yes,scrollbars=yes,resizable=yes,location=yes,dependent=yes,width=700,height=600';
    var win=window.open(url, title, options);
    win.focus();
}

function showZoomUtil(url,urlPrefix, minimumWidth) {
        if (urlPrefix) url = urlPrefix+url;
        if (minimumWidth) url = url+"&width="+minimumWidth;
        HTTP.getText(url, showPopup );

}


var currentTdEl;

//This global boolean detects whether data has changed on the page.
//For now we just assume data always has changed.
var DID_CHANGE_DATA=true;


function popupWindow(href) {
    var refWin=window.open(href,"_blank","toolbar=no,titlebar=no,menubar=no,dependent=yes,alwaysRaised=yes,left=50,top=50");
    //var newEl=refWin.document.createElement("div");
    //newEl.innerHTML="<a href='javascript:closeWindowUtil()'>[Luk vindue]</a>";
}

//Reassign this global variable to change the text when closing windows.
var CLOSE_WINDOW_WARNING="Changed data (if any) has not been saved. Do you still want to close the window?";

function closeWindowUtil(forceClose) {
    if (forceClose || !DID_CHANGE_DATA || confirm(CLOSE_WINDOW_WARNING)) {
        window.close();
    }
}

function registerDirtyData() {
    DID_CHANGE_DATA=true;
}

function closeWindowFocusParentUtil() {
    if (!DID_CHANGE_DATA || confirm(CLOSE_WINDOW_WARNING)) {
        if (window.opener!=null) window.opener.focus();
        window.close();
    }
}

function notifyOpenerAndClose(clientSideId) {
    if (clientSideId!=null && clientSideId.length>0) {
        if (window.opener!=null) {
            //Check that the parent window contains this method.
            if (window.opener.onObjectDeleted!=null) {
                //We expect it to be a method, call it now with the client-ID as an argument
                window.opener.onObjectDeleted(clientSideId);
            }
        }
    }
    closeWindowUtil(true);
}

function onObjectDeleted(clientSideId) {
    var el=document.getElementById(clientSideId);
    if (el!=null) {
        el.innerHTML="<div>Data was deleted</div>";
    }
}