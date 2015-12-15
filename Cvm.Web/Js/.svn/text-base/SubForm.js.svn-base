


/********************** 
DEFAULT ENTER:
These functions makes the default enter work correctly.
**********************/

//Returns true if the buttonRef has the defaultEnter-property.
var hasDefaultEnter=function(buttonRef) {
	if (buttonRef==null) return false;
	else {
		if (buttonRef.getAttribute("defaultEnter")==1) return true;
		else return false;
	}
}

//Takes a reference to a span element (or some other element spanning 
//a section of html) and a key press event.
//From this all buttons inside the span element are identified
//and the last of these is clicked. If one button has the defaultEnter-property
//then this button will have higher priority than buttons without this property.
function executeDefaultEnter(spanRef, eventRef) {
	if (!eventRef) eventRef=window.event;
	if(eventRef.srcElement.tagName.toLowerCase()!="textarea"){
		markDefaultEnterButton(spanRef, eventRef);
		if (didPressEnterBtn(eventRef)) {
			var myButton=getDefaultEnterButton(spanRef);
			//If we did find a candidate button, fire this and cancel any further processing of the event.
			if (myButton!=null) {
				myButton.click();
				stopEvent(eventRef);
				return false;
			}
			else { return true; }
		}
	}
	else return true;
}

function didPressEnterBtn(e) {
	 if (!e) e=window.event;
	 var key;
	 if (e.which) {
		  key=e.which;
	 }
	 else {
		  key=e.keyCode;
	 }
	 return (key==13);
}

function defaultEnter(myEvent,btnId) {
    if (didPressEnterBtn(myEvent)) {
        var fct=document.getElementById(btnId).onclick;
        fct();
        return false;
    }
    return true;
}

//Marks the default enter button inside a span-ref while unmarking the last marked button.
/*function markDefaultEnterButton(spanRef, eventRef) {
	if (!eventRef) eventRef=window.event;
	var myButton=getDefaultEnterButton(spanRef);
	var stylePostFix="-defaultEnter";
	if (GLOBAL_lastMarkedButton!=null) {
		GLOBAL_lastMarkedButton.className=GLOBAL_lastMarkedButton.className.replace(stylePostFix+compareButtons(GLOBAL_lastMarkedButton,getFirstButton()),"");
	}
	if (myButton!=null) {
		myButton.className += stylePostFix+compareButtons(myButton,getFirstButton()); 
		GLOBAL_lastMarkedButton=myButton;
		
		//stopEvent(eventRef);
		eventRef.cancelBubble=true;
		return false;
	}
	else return true;
}*/

function compareButtons(button1, button2) {
	return (button1==button2 ? "-first" : "-notFirst" );
}

var GLOBAL_firstButton=null;
//Gets the first submit button on the page.
function getFirstButton() {
	if (GLOBAL_firstButton==null) {
		var check=true;
		var inputs=document.getElementsByTagName("input");
		for (var i=0;i<inputs.length && check;i++) {
			if (inputs[i].type=="submit") {
				GLOBAL_firstButton=inputs[i];
				check=false;
			}
		}		
	}
	return GLOBAL_firstButton;
}

//Adjust all button borders except for the first one because this one is born with extra borders.
function removeButtonBorders() {
	var inputs=document.getElementsByTagName("input");
	var didFindFirst=false;
	for (var i=0;i<inputs.length;i++) {
		if (inputs[i].type=="button" || inputs[i].type=="submit") {
			if (inputs[i]!=getFirstButton()) { inputs[i].className += "-equal"; }
					
		}

	}	
}


//Keep a global reference to the last marked button.
var GLOBAL_lastMarkedButton=null;

//Finds the button which has default enter inside a certain span-Element.
function getDefaultEnterButton(spanRef) {
	var myButton;
	var inputs=spanRef.getElementsByTagName("input");
	for (var i=0;i<inputs.length;i++) {
		if (inputs[i].type=="button" || inputs[i].type=="submit") {
			//Only reassign if new button has stronger priority than old button.
			if (hasDefaultEnter(inputs[i]) || (!hasDefaultEnter(myButton))) myButton=inputs[i];
		}
	}	
	return myButton;
}
/********************** DEFAULT ENTER END**********************/
