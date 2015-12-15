function clearInputFields(el) {
    var nodeList=el.getElementsByTagName("input");
    for (var i=0;i<nodeList.length;i++) {
        nodeList[i].value="";
    }
    var selectItems=el.getElementsByTagName("select");
    
    for (var i=0;i<selectItems.length;i++) {
        selectItems[i].selectedIndex=0;
    }
}

/*
Returns selected text.
*/
getSelected = function() {  
    var t = '';  
    if(window.getSelection){    
        t = window.getSelection();  
    }else if(document.getSelection){
        t = document.getSelection();  
    }else if(document.selection){    
        t = document.selection.createRange().text;  
    }  
    $("input").each(function(x,el) {alert(el.type)});
    return t;
    
}
