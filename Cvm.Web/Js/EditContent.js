var ContentEdit=new Object();

function getContentLoaderQuery() {
    return getRootOfWebApp();
}
function editElement(myevent, spanEl, contentElSer) {
    if(myevent.ctrlKey) {
        var textBefore=spanEl.innerHTML;
        if (textBefore.charAt(0)=='[' && textBefore.charAt(textBefore.length -1)==']') textBefore=textBefore.substring(1,textBefore.length-1);
        else alert("Text did not contain [ and ]:"+textBefore);
        var contentIdSpan=document.getElementById('contentEditorAreaContentId');
        //The content-element is serialized as the following example shows:
        //CE|1|SiteText|PageNavigation.Catalog.Sub
        var splited=contentElSer.split("|");
        contentIdSpan.innerHTML=splited[splited.length-1];
        var editorArea=document.getElementById('contentEditorArea');
        var editor=document.getElementById('contentEditor');
        editorArea.value=textBefore;
        editor.style.display='block';
        ContentEdit.contentElSer=contentElSer;
        ContentEdit.spanEl=spanEl;    
    }
}

//This global variable can be overriden in the pages if the root of the web-app is different
var ROOT_WEB_APP=null;

function getRootOfWebApp() {
    if (ROOT_WEB_APP!=null) {
        return ROOT_WEB_APP;
    } else {
        alert("The global javascript variable ROOT_WEB_APP must be defined to something like / or /Nshop.Web/.");
    }
}

//Call this to define the root of the web application so that the content editor can work.
function setRootOfWebApp(root) {
    ROOT_WEB_APP=root;
}

function updateContent() {
    var editorArea=document.getElementById('contentEditorArea');
    var editor=document.getElementById('contentEditor');
    var newText=editorArea.value;
    var query=getRootOfWebApp()+'AdminPages/Content/EditContentService.aspx?contentEl='+ContentEdit.contentElSer+"&newText="+encodeURIComponent(newText);
    HTTP.getText(query,callBack,error);
    ContentEdit.spanEl.innerHTML=newText;
    editor.style.display='none';
    
}


function cancelUpdateContent() {
    var editor=document.getElementById('contentEditor');
    editor.style.display='none';
    
}

function callBack(responseText) {
//    alert("OK: "+responseText);
}

function error(responseText) {
    alert("Error: "+responseText);
}