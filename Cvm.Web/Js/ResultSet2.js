
/***********Class ResultSet2************************
By Nikola Schou. Feel free to use, reuse or abuse.

This javascript component is used to set up a table with sorting on each column and free text search in the table.

Please note that the documentation below is not fully up to date!!!

Synopsis:
<script language="javascript" src="ResultSet2.js">
//Include the component.
</script>
<body onload="javascript:automaticallyDetectResultSet()">
<table width="100%" fontSize="10" resultSet_height="300">
<tr>
<td width="100">Attribute A</td>
<td elasticWidth="1" sort="no">Attribute B</td>
</tr>
<tr>
<td>a1</td>
<td>b1</td>
</tr> 
<tr>
<td>a2</td>
<td>b2</td>
</tr>
</table>
</body>

DESCRIPTION OF SPECIAL ATTRIBUTES:
-<table resultSet="1">: This is required on all tables which are to be presented as a ResultSet-instance.

-<table fontSize="...">: This should equal the actual font-size of the letters 
in the table. This font-size is not used to set the actual size of the
 letters (this is controlled by the css-stylesheet). It is used to give 
estimates on the width of table columns in cases where this is left out. 

-<table resultSet_height="...": This will give the height of the table. 

-<td width>: This will give the width of the column in which this td
 resides. Note that one should only give the width in the top row. 
Do not give widths in the rest of the rows. If one omits a width, a width
will be estimated by counting the maximum number of characters in the column.

-<td elasticWidth="1">: This says that the column in which this td resides
is chosen to be the one which has the flexibility. If the table-tag has 
width="100%" one must ensure that exactly one td in the top row has set
this attribute. If 0 or 2... has the attribute, the table will not be formatted correct.

-<td sort="...">: This gives the sorting criteria by which the column
 should be sorted. sort="no" omits sorting. By leaving out the 
sort-attribute, default sorting is given.

CSS-CLASSES USED FOR THIS COMPONENT:

-toprow: Used in <tr class="toprow"> in the top row.

-toprowtxt: used in <td class="toprowtxt"> in all cells in the top row.

-portlet: used in <table class="portlet"> the table containing the top row.

-textblx: Used in <input type="text" class="textbox"> in the search field.

COMMENTS:
- The component is developed to be compatible with W3C specification of Javascript 1.5 (EcmaScript 1.5), IE5.0+ and NN6+.

- Sorting is done in a simple way. If two entries are numbers 
(e.g. 2000.00 and 3.49 but not 2,000.00 or 3,49), they are sorted as numbers,
otherwise they are sorted lexically. If an attribute sort="no" is given for a cell in the toprow, then no sorting is offered on this row. 

- Special types of sorting such as amounts with format x.xxx,xx will be implemented later.

- One must have the same number of table cells in each rows. It does not matter if they are empty. The component inserts non-breakable-spaces in empty cells.
The component has not been thoroughly tested for tables in which there is not the same number of table cells in each row.

- Everything is left-aligned by default. One can make cells right or center 
aligned by setting align="..." in all table cells in the desired column 
(including the top row).

***************************************************/
    
    /*****************************************************************
		UTILITIES start
    *****************************************************************/
    
    
    
    //define getBrowsertype if not already defined elsewhere.
    if (typeof getBrowserType=="undefined" || ! getBrowserType instanceof Function) {
		  //Returns 1 for IE5, 2 for NN6, possibly higher numbers for other types.
		  //It is not yet fully working.
		  function getBrowserType() {
				if (navigator.appName.indexOf("xplorer")>-1) {
					 return 1;
				}
				if (navigator.appName.indexOf("etscape")>-1) {
					 return 2;
				}
		  }	 

    }
//gets the version of the browser, i.e. 5.5 for EI5.5
function getBrowserVersion() {
    var version;
    try {
		  match=navigator.appVersion.match(/MSIE[ ]*([0-9]*[.]?[0-9]*)/);
		  version=parseFloat(match[1]);
    }
    catch(err) {
		  version=0;
    }
    return version;
}

var GLOBAL_IS_DEVELOPMENT=true;


//Shortcut for the annoying getElementById-method.
function get(myid) { return document.getElementById(myid) }

//Cancels any further processing of an event.
//Only works for IE for now.
function stopEvent(e) {
	 if (!e) e=window.event;
	 e.cancelBubble=true;
	 e.returnValue=false;
	 return false;
}



//Shows an error object in a reasonable way.
function handleError(e,xtramessage) {
	if (GLOBAL_IS_DEVELOPMENT) {
		if (!(e instanceof Error)) {
			alert("function handleError() called without error");
		}
		else {
			alert("Error: "+e.message+" in l."+e.lineNumber+". "+(xtramessage?xtramessage:"(no message)"));
		}
	}
	else {
		//do nothing, report only in development environments.
	}
}


//tests whether return was pressed on an event.
function didPressEnter(e) {
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


/*****************************************************************
		UTILITIES END
*****************************************************************/
var IMG_STYLE="inline";
//Assigns a number of attributes to the sorting pics.
function prepareSortingImg(imgRef, upOrDown, hidden) {
    imgRef.alt=(upOrDown==1?'Sort ascending':'Sorter descending');
    imgRef.border=0;
    imgRef.style.cursor="pointer";
    imgRef.tabIndex=-1;
    imgRef.style.display=(hidden?"none":IMG_STYLE);
}
	 
var IMAGES_DIR = "../images/master/resultSetImages/";
//images for sorting. This will be cloned later on.
var DOWN_ARROW_GIF=IMAGES_DIR+"arrow_down_01.gif";
var DOWN_ARROW_IMG=new Image();
DOWN_ARROW_IMG.src=DOWN_ARROW_GIF;
prepareSortingImg(DOWN_ARROW_IMG, 2);

var DOWN_ARROW_GIF_CHOSEN=IMAGES_DIR+"arrow_down_02.gif";
var DOWN_ARROW_IMG_CHOSEN=new Image();
DOWN_ARROW_IMG_CHOSEN.src=DOWN_ARROW_GIF_CHOSEN;
prepareSortingImg(DOWN_ARROW_IMG_CHOSEN, 2,true);

var UP_ARROW_GIF=IMAGES_DIR+"arrow_up_01.gif";
var UP_ARROW_IMG=new Image();
UP_ARROW_IMG.src=UP_ARROW_GIF;
prepareSortingImg(UP_ARROW_IMG, 1);

var UP_ARROW_GIF_CHOSEN=IMAGES_DIR+"arrow_up_02.gif";
var UP_ARROW_IMG_CHOSEN=new Image();
UP_ARROW_IMG_CHOSEN.src=UP_ARROW_GIF_CHOSEN;
prepareSortingImg(UP_ARROW_IMG_CHOSEN, 1, true);
	 
	 

var SEARCH_LOGO=IMAGES_DIR+"search_logo.gif";
var MAXIMIZE_LOGO=IMAGES_DIR+"icoExpand.gif";
var MINIMIZE_LOGO=IMAGES_DIR+"icoCollapse.gif";
	 
//classes
var CLASS_TD="headerStyle"; //class of table cells
var CLASS_TR="toprowtxt";//class of table rows
var CLASS_TABLE="datagrid";//class of tables
var CLASS_SEARCH_RESULT="resultsetsearch";//class of rows found by search
var CLASS_BUTTON="button";//class of search button
var CLASS_TEXTBOX="input";//class of search text box
var CLASS_STRIP="resultset"; //rowstyle0 and rowstyle1 are styles of the stripes.

//Master index for instances of ResultSet2.
var MASTER_INDEX_RESULT_SET=0;
var RESULT_SET_INSTANCES=new Array();
var DEFAULT_SORT="alpha";
var NO_SORT="no";

var STANDARD_TABLE_HEIGHT=-1;
var FONT_SIZE_TO_LINEHEIGHT_RATIO=1.75;
var FONT_SIZE_TO_CHARACTER_WIDTH_RATIO=0.65;
var SEARCH_CELL_WIDTH=12;
var INVISIBLE_SYMBOL; //non-breakable space.
if (getBrowserVersion()<5.5) {
    INVISIBLE_SYMBOL="\u00A0";
}
else {
    var INVISIBLE_SYMBOL="\uFEFF"; //zero width non-breakable space. This is truly invisible in IE5.5 but not in IE5.0.
}
	 


//Constructor for ResultSet2 class
function ResultSet2(tableRef) {
    if (tableRef==null) {
		  return null;
    }
    if (!(tableRef.tagName=='TABLE')) {
		  tableRef=document.getElementById(tableRef.toString());
    }
    //don't initialize for tables without a body.
    if (tableRef.tagName.toUpperCase()=='TABLE' && tableRef.rows.length>1) {
		  this.tableRef=tableRef;
		  this.preFormatTable();
		  this.tableRef.setAttribute("didDetect",1);
		  //Used to box the main table
		  this.BoxDivEl=document.createElement("div");
		  //False if the table top row is placed in the same table as the data-rows
		  //and true if the table top-row is isolated in a seperate table.
		  this.headerIsInBody=true;
		  this.attNames=new Array(this.getNumberOfCols()+1);
		  this.initializeAttNames();
		  this.rows=new Array(tableRef.rows.length);
		  this.sortTypes=new Array(this.getNumberOfCols()); //contains types of the columns.
		  this.mustPrepareColumn=new Array(this.getNumberOfCols());//contains true if column i must be preprocessed before sorting.
					 
		  this.initializeSortTypes();
		  this.validateSortTypes();
		  this.arrowUpImages=new Array(this.getNumberOfCols());
		  this.arrowDownImages=new Array(this.getNumberOfCols());
		  this.arrowImages=new Array(this.getNumberOfCols());
		  this.lastSort=new Array(null,null);
		  //this.initializeAllRows();
		  this.searchBoxContentRef=null;
		  this.searchBoxRef=null;//if the searchbox is shown, this contains the reference to the containing div-element.
		  this.initializeSearchBox();
		  //set ratio between number of characters and typical number of pixels.
		  RESULT_SET_INSTANCES.push(this);
		  this.masterIndex=RESULT_SET_INSTANCES.length-1;
					 
		  this.htmlID="RESULT_SET_INSTANCES_"+this.masterIndex;
		  this.htmlIDTableHeader="RESULT_SET_INSTANCES_"+this.masterIndex+"_tableheader";
		  this.htmlRef="RESULT_SET_INSTANCES["+this.masterIndex+"]";
		  this.topRowRef=null;
		  this.insertTableHeader();
		  this.widths=new Array(this.attNames[0].cells.length);
		  this.defaultwidth=100;
		  this.initializeWidths();
		  //Comment this in if you want to see the widths.
		  this.insertTableInDivElement();
		  this.searchLogoRef=null;
		  if (!(this.tableRef.getAttribute("omitSearchBox") == 1)){
				this.insertSearchBox();
		  }
					 
		  //It is possible to set this attribute to merge header and body
		  //In this way columns are always aligned.

		  if (this.tableRef.getAttribute("critical")==1 || !(this.exceedsHeight())) {
				this.moveHeaderToBody();
		  }

		  if(ResultSet2.developmentMode) {
				this.prepareForDevelopmentMode();
		  }
					 
		  if (!this.tableRef.getAttribute("nostrip")=="1") {
		
				this.stripTable();
		  }
          this.getTableHeaderRef().width=this.tableRef.width;
		  var outerThis=this;

    }
}

		
		  
		  


//functions are defined below, properties of the class comes below.
try { 
    var dummyresultset=new ResultSet2();

    ResultSet2.prototype.maximizeMinimize=function() {
		  if (this.headerIsInBody) {
				this.maximize();
		  }
		  else {
				this.minimize();
		  }
    }
		  
    ResultSet2.prototype.maximize=function() {
		  this.moveHeaderToBody();
		  this.setHeight(STANDARD_TABLE_HEIGHT);
    }
		  
    ResultSet2.prototype.minimize=function() {
		  this.moveHeaderBack();
		  this.setHeight(this.getTableHeight());
    }		  

    //This prepares the resultset for development mode which is convenient when developing and adjusting resultsets.
    ResultSet2.prototype.prepareForDevelopmentMode=function() {
		  var a=document.createElement("a");
		  a.innerHTML="ResultSet info";
		  var message="ResultSet development information:\n";
		  for (var i=1;i<this.attNames.length;i++) {
				message += this.attNames[i] + "(" + this.sortTypes[i-1]+", "+this.widths[i-1]+")\n";
		  }
		  var ruler="";
		  for (var i=1;i<this.attNames.length;i++) {
				ruler+="<td ";
				if (this.widths[i-1]>0) ruler+="width='"+this.widths[i-1]+"'";
				ruler+=">|</td>";
		  }
		  //A table column for the search box.
		  ruler+="<td width=15>|</td>";
		  var temp=document.createElement("span");
				
		  temp.innerHTML="<table border='0' width='100%'><tr height='5'>"+ruler+"</tr></table>";
		  var tableRef=this.tableRef.parentNode;
		  this.tableRef.ondblclick=function(event) {
				tableRef.parentNode.insertBefore(temp,tableRef);
				alert(message);
		  }
    }

    ResultSet2.prototype.preFormatTable=function() {
		  this.tableRef.className=CLASS_TABLE;
		  this.tableRef.rows[0].className=CLASS_TR;
		  /*
			 this.stripTable();
			 var cells=this.tableRef.rows[0].cells;
			 var n=cells.length;
			 for (var i=0;i<n;i++) {
			 cells[i].className=CLASS_TD;
			 }
		  */
    }
		  
    //Searches the table by a regular expression,
    //putting up the matching rows to the top.
    ResultSet2.prototype.doSearch=function(searchString,index) {
		  //move header away to avoid confusion in case of no scroll-bar.
		  if (! this.exceedsHeight()) {
				this.moveHeaderBack();
		  }
		  this.ensureInitializeRowArray();
		  this.ensurePrepareForSearch();
		  
		  searchString=searchString.toLowerCase();
		  var rows=this.tableRef.rows;
		  var parent=rows[0].parentNode;
		  var i=0;
		  var n=this.rows.length;
		  var counter=0;
		  for (i=0;i<n;i++) {
				rows[i].className="";
				if (this.rows[i][this.rows[i].length-1].indexOf(searchString)>=0) {
					 this.rows[i][0].className = CLASS_SEARCH_RESULT;
					 parent.insertBefore(this.rows[i][0],rows[counter++]);
				}
		  }
		  //Strip table after search to make sure the colors are right
		  JstripTable(this.tableRef);
		  if (! this.exceedsHeight()) {
				this.moveHeaderToBody();
		  }
    }
	 
    ResultSet2.prototype.ensurePrepareForSearch=function() {
		  if (this.didPrepareForSearch!=true) {
				this.didPrepareForSearch=true;
				var i=0;
				var n=this.rows.length;
				var last=this.getNumberOfCols()+1;
				for (i=0;i<n;i++) {
					 this.rows[i][last]=this.getTextValue(this.tableRef.rows[i]).toLowerCase();
				}
		  }
    }
    //makes a searchbox and assigns it to this.searchBoxRef
    ResultSet2.prototype.initializeSearchBox=function() {
		  //create input field...
		  var searchBox=document.createElement("input");
		  searchBox.className=CLASS_TEXTBOX;
		  searchBox.setAttribute("type","text");
		  searchBox.setAttribute("size","20");
		  var mythis=this;//to prevent shadowing in nested functions (eventhandlers).
		  searchBox.onkeypress=function(e) {
				if (!e) { e=window.event;}
				try {
					 if(didPressEnter(e)) {
						  mythis.doSearch(this.value);
						  mythis.hideSearchBox();
						  e.returnValue=false;
					 }
				}
				catch(e) {
					 handleError(e, "Error in key down event");
				}
				//e.cancelBubble=true;
		  }		  
		  //use this event if you want tab to fire a search.
		  searchBox.onblurxxx=function(myevent) {
				mythis.doSearch(this.value);
				this.focus();
		  }
		  //create button...
		  var searchButton=document.createElement("input");
		  searchButton.setAttribute("type","button");
		  searchButton.className=CLASS_BUTTON;
		  searchButton.value="Find";
		  searchButton.onclick=function(e) {
				if (!e) {e=window.event;}
				mythis.doSearch(searchBox.value);
		  }
		  searchButton.onkeypress=function() {
				if(!e) e=window.event;
				if(didPressEnter(e)) {
					 mythis.doSearch(searchBox.value);
					 searchBox.focus();
				}
		  }
		  var div=document.createElement("div");
		  div.style.textAlign="center";
		  div.onkeypress=function(e) {
				if(!e) e=window.event;
				if(didPressEnter(e)) {
					 //e.cancelBubble=true;
					 e.returnValue=false;
				}
		  }
		  div.appendChild(searchBox);
		  div.appendChild(searchButton);
		  this.searchBoxContentRef=div;
    }
    ResultSet2.prototype.getSearchBoxRef =function(){
		  return this.searchBoxRef;
    }
    ResultSet2.prototype.hideSearchBox =function(){
		  //this.getSearchBoxRef().parentNode.removeChild(this.getSearchBoxRef());				
		  this.getSearchBoxRef().style.display="none";	
		  if(typeof this.searchLogoRef.focus!="undefined") {
				this.searchLogoRef.focus();
		  }
		  return this;
    }
    //determines where to put the search symbol.
    ResultSet2.prototype.insertSearchBox=function() {
		  var myspan=document.createElement("span");
			
		  var mythis=this;
				
										
		  //Maximize-icon
		  if (this.exceedsHeight()) {		
					
				var img1=document.createElement("img");
				img1.style.cursor="pointer";
				img1.title="Expand/Collapse";
				this.setMaximizeImageUtil(img1);
				img1.onclick=function(myevent) {
					 mythis.maximizeMinimize();
					 mythis.setMaximizeImageUtil(img1);
				}

				myspan.appendChild(img1);
		  }
				
		  //Search-icon
		  var img=document.createElement("img");
		  img.src=SEARCH_LOGO;
		  img.width=SEARCH_CELL_WIDTH-1;
		  img.height=SEARCH_CELL_WIDTH-1;
		  img.onclick=function(myevent) {mythis.showSearchBox(myevent)};
				
		  img.style.border="none";
				
		  var anchor=document.createElement("a");
		  anchor.href="javascript:;";
		  anchor.style.border="none";
		  anchor.appendChild(img);
		  anchor.title="Find";
		  anchor.onkeypress=function(e) {
				if (!e) e=window.event;
				if (didPressEnter(e)) {
					 mythis.showSearchBox(e);
				}
		  }
		  myspan.appendChild(anchor);

				
		  var myelement=this.getTableHeaderRef().rows[0].cells[this.getTableHeaderRef().rows[0].cells.length-1];
				
		  myelement.appendChild(myspan);
		  this.searchLogoRef=myspan;

    }
		  
    ResultSet2.prototype.setMaximizeImageUtil=function(img1) {

		  if (!this.headerIsInBody) img1.src=MINIMIZE_LOGO;
		  else img1.src=MAXIMIZE_LOGO;
    }

    ResultSet2.prototype.showSearchBox=function(myevent) {
		  //try {
		  if (! this.getSearchBoxRef() || !(this.getSearchBoxRef().parentNode)) {
				var mythis=this;//to avoid shadowing in nested functions.
				if (!myevent) { myevent=window.event; }//either IE5+ or W3C, NN6 etc.
				//					 var x=myevent.clientX;
				//var y=myevent.clientY;
				var x,y;
				if (getBrowserType()==1) {
					 x=myevent.clientX+document.body.scrollLeft-myevent.offsetX;
					 y=myevent.clientY+document.body.scrollTop-myevent.offsetY-5;
				}
				else {
					 x=myevent.clientX+window.pageXOffset;
					 y=myevent.clientY+window.pageYOffset;
				}
				var mywidth=150;
				var myheight=50;
				var div=document.createElement("div");
				div.style.position="absolute";
				div.style.width=mywidth;
				div.style.height=myheight;
				div.style.backgroundColor="#D4D4D4";
				div.style.marginLeft="-158px";
				div.style.marginTop="-3px";
				div.style.zindex=100;
			//	div.style.left=x-parseInt(mywidth);
			//	div.style.top=y-0;
				div.style.border="1px solid #808080";
				div.style.textAlign="center";
				div.style.padding="3px";


				var anchor=document.createElement("a");
				anchor.href="javascript:;";
				anchor.appendChild(document.createTextNode("X"));
				anchor.style.fontWeight="normal";
				anchor.style.textDecoration="none";
				anchor.style.fontSize="8px";
				anchor.style.fontFamily="Helvetica";
				anchor.style.color="black";
						  
				//closeImage=document.createElement("img");
				//closeImage.src="../images/btn_close.gif";
				//closeImage.width=10;
				//closeImage.height=10;
				//closeSign.href="javascript:;";
				//closeImage.alt="Close";

				var closeImage=document.createElement("span");
				closeImage.style.textAlign="center";
				closeImage.appendChild(anchor);
				closeImage.style.border="1px solid black";						  
				closeImage.style.height="12px";
				closeImage.style.position="relative";
				closeImage.style.overflow="hidden";
				closeImage.style.width="12px";
						  
				closeImage.onclick=function(e) {
					 if (!e) e=window.event;
					 mythis.hideSearchBox();
					 e.cancelBubble=true;
				}
				var closeImageContainer=document.createElement("div");
				closeImageContainer.style.textAlign="right";
				closeImageContainer.appendChild(closeImage);

				div.appendChild(closeImageContainer);
				div.appendChild(this.searchBoxContentRef);
						  
				//document.body.insertBefore(div,document.body.firstChild);
				//document.body.appendChild(div);

				this.searchLogoRef.parentNode.appendChild(div);
				this.searchBoxRef=div;
		  }
		  this.getSearchBoxRef().style.display="";
		  this.searchBoxContentRef.childNodes[0].focus();
		  //}
		  //catch(e) {
		  //handleError(e,"Error in search");
		  //}
    }

		  
    //Initializes all the rows in the fastest way. However, in this way the also non-sorting
    //columns are parsed (which is a waste).
    ResultSet2.prototype.initializeAllRows=function() {
		  var mylength=this.rows.length;
		  var i=0;
		  var numberOfCols=this.getNumberOfCols();
		  var tempRef1;
		  for (i=0;i<mylength;i++) {
				tempRef1=this.tableRef.rows[i];
				this.rows[i]=new Array(numberOfCols+2);
				this.rows[i][numberOfCols+1]="";
				this.rows[i][0]="";
				try {
					 for (j=0;j<numberOfCols;j++) {
						  this.rows[i][j+1]=this.getTextValue(tempRef1.cells[j]).toLowerCase();
					 }
				}
				catch(e) {
					 this.alertError("",e);
				}
				this.rows[i][numberOfCols+1]=" "+this.rows[i].join(" ")+" ";
				this.rows[i][0]=tempRef1;
		  }
    }
		  
    //Initializes the j'th column, if it hasn't already been done.
    ResultSet2.prototype.ensureInitializeColumn=function(j) {
		  if (!this.didInitialize) this.didInitialize=new Array(this.getNumberOfCols);
		  if (this.didInitialize[j]!=true || this.isDynamicColumn(j)) {
				this.initializeColumn(j);
				this.didInitialize[j]=true;
		  }
		  return;
    }
    ResultSet2.prototype.initializeRows=function() {
		  var numberOfCols=this.getNumberOfCols();
		  for (j=0;j<numberOfCols;j++) {
				this.initializeColumn(j);
		  }
    }

    ResultSet2.prototype.isDynamicColumn=function(j) {
		  return (typeof this.getPreprocessFunction(j).dynamic!="undefined");
    }

    //Initializes the j'th column
    ResultSet2.prototype.initializeColumn=function(j) {
		  this.ensureInitializeRowArray();
		  var i=0;
		  var mylength=this.tableRef.rows.length;
		  var preprocessFunction=this.getPreprocessFunction(j);
		  var tableRef=this.tableRef;
		  this.mustPrepareColumn[j]=false;
		  if (preprocessFunction instanceof Function) {
				if (this.isDynamicColumn(j)){
					 for (i=0;i<mylength;i++) {
						  this.rows[i][j+1]=preprocessFunction(tableRef.rows[i].cells[j]);								
					 }
				}
				else {
					 for (i=0;i<mylength;i++) {
						  this.rows[i][j+1]=preprocessFunction(this.getTextValue(tableRef.rows[i].cells[j]).toLowerCase());								
					 }
				}
		  }
		  else {
				for (i=0;i<mylength;i++) {
					 this.rows[i][j+1]=this.getTextValue(tableRef.rows[i].cells[j]).toLowerCase();
				}				 
		  }
    }

    //Initializes the whole 2d-array which is supposed to hold all the table-data.
    ResultSet2.prototype.ensureInitializeRowArray=function() {
		  //Only do it once.
		  if (this.didInitializeRowArray!=true) {
				var mylength=this.rows.length;
				var i=0;
				var numberOfCols=this.getNumberOfCols()+2;
				for (i=0;i<mylength;i++) {
					 this.rows[i]=new Array(numberOfCols);
					 this.rows[i][0]=this.tableRef.rows[i];
				}		
				this.didInitializeRowArray=true;
		  }
    }
	 
    //returns the maximal number of columns in the table.
    ResultSet2.prototype.getNumberOfCols=function() {
		  if (!(this.didCalculateNumberOfCols==true)) {
				this.numberOfCols=0;
				var n=this.tableRef.rows.length;
				for (var i=0;i<n;i++) {
					 this.numberOfCols=Math.max(this.numberOfCols,this.tableRef.rows[i].cells.length);
				}
				if (this.attNames && this.attNames.length>0 && this.attNames[0].cells.length>0) {
					 this.numberOfCols=Math.max(this.numberOfCols,this.attNames[0].cells.length);
				}
				this.didCalculateNumberOfCols=true;
		  }
		  return this.numberOfCols;
    }
    ResultSet2.prototype.stripTable=function() {
		  var n=this.tableRef.rows.length;
		  /*for (var i=0;i<n;i++) {
			 this.tableRef.rows[i].style.backgroundColor="";
			 }*/
		  if (typeof stripTable!="undefined") {
				if(stripTable instanceof Function) {
					 stripTable(this.tableRef);
				}
		  }
    }
    //Return the maximum number of characters in columns number index.
    //Counted 1,...,n.
    ResultSet2.prototype.getMaxWidth=function(index) {
		  try {
				index++;//remember that index has been shifted.
				var M=0;
				var n=this.rows.length;
				for (var i=0;i<n;i++) {
					 
					 if (index<this.rows[i].length-1) {
						  M=Math.max(M,(new String(this.rows[i][index])).length);
					 }
				}
				M=Math.max(M,this.attNames[index].length+5);
				return M;
		  }
		  catch(e) {
				handleError(e,"Could not get max width for index="+index +" and row index="+i+" where this.rows[rowIndex][index]=");
		  }
    }
    //returns an array containing all the maxwidths
    ResultSet2.prototype.getMaxWidths=function() {
		  try {
				if (!(this.didGetMaxWidths==true)) {
					 this.maxwidths=new Array();
					 var n=this.getNumberOfCols()+1;
					 var m=this.rows.length;
					 for (var j=1;j<n;j++) {
						  var M=0;
						  for (var i=0;i<m;i++) {
								if (j<this.rows[i].length-2) {
									 M=Math.max(M,(new String(this.rows[i][j])).length);
								}
						  }
						  this.maxwidths.push(M);
					 }
					 this.didGetMaxWidths=true;
				}
				return this.maxwidths;
		  }
		  catch(e) {
				handleError(e,"Could not get max widths");
		  }
    }

    ResultSet2.prototype.initializeWidths=function() {
		  try {
				var i,truewidth;
				var n=this.getNumberOfCols();
				var m=this.attNames[0].cells.length;
				var didFindElasticWidth=false; 
				var didDetermine=false;

				for (i=0;i<n && i<m;i++) {
					 //1. Elastic width can be declared directly in the td.
					 if(!didFindElasticWidth && this.attNames[0].cells[i].getAttribute("elasticWidth")==1) {
						  truewidth=-1;
						  didDetermine=true;
						  didFindElasticWidth=true;
					 }
					 else if(this.getWidthFromTableTag(i)!=0) {
						  //2. Or width can be given in the table-tag in the tdWidths-attribute
						  truewidth=this.getWidthFromTableTag(i);
						  didDetermine=true;
						  //If we found -1:
						  if (truewidth==-1) {
								if (didFindElasticWidth==true) {
									 
									 truewidth="";
									 didDetermine=false;
									 //this.alertError("You have set width=-1 in two different columns. This is not valid.");
								}
								didFindElasticWidth=true;
						  }
					 }
					 if(!didDetermine){
						  //3. Or a width can be given on the td.
						  var mywidth=this.attNames[0].cells[i].width;
						  if (!(isNaN(mywidth)) && mywidth>0 && mywidth!=null && mywidth!="") {
								truewidth=parseInt(mywidth);
						  }
						  else {
								//3. Or a type specific width can be found.
								var typeWidth=parseInt(ResultSet2.typeWidths[this.sortTypes[i]]);
								if (!isNaN(typeWidth)) {
									 truewidth=typeWidth;
								}
								else {
									 //4. Otherwise count...
									 //Get width as the max-width by counting the maximal number of characters in the column.
										  
									 truewidth=parseInt(this.getMaxWidth(i)*this.getWidthScale());
								}
						  }
					 }
					 this.widths[i]=truewidth;
				}

				//If no elastic column is found from the data columns, search the table top bar.
				if (!didFindElasticWidth && this.tableRef.getAttribute("elasticColumn") && (! isNaN(this.tableRef.getAttribute("elasticColumn")))) {
					 this.widths[this.tableRef.getAttribute("elasticColumn")]=-1;
					 didFindElasticWidth=true;
				}
				//If no elastic column is found, default to the first column.
				if (!didFindElasticWidth) {
					 this.widths[0]=-1;
					 didFindElasticWidth=true;
				}
					 
				this.setAllWidths();
		  }
		  catch(e) {
				handleError(e,"Could not initialize widths.");
		  }
    }

    //Gets the width of the columnNr as encoded in the table-tag.
    //Returns 0 if no width is found here.
    ResultSet2.prototype.getWidthFromTableTag=function(columnNr) {
		  //Initialize local copy of widths.
		  if (!this.tdWidths) {
				try {
					 this.tdWidths=this.tableRef.getAttribute("tdWidths").split(";");
				}
				catch(e) { this.tdWidths=new Array();}
		  }
		  if (columnNr<this.tdWidths.length) {
				if (this.tdWidths[columnNr]==0) return -1;
				else if (!isNaN(parseInt(this.tdWidths[columnNr]))) return parseInt(this.tdWidths[columnNr]);
					 
		  }
		  return 0;
    }

    ResultSet2.prototype.reverseOrder=function() {
		  this.rows.reverse();
		  return this;
    }
    ResultSet2.prototype.moveHeaderToBody=function() {
		  this.headerIsInBody=false;
		  var firstRow=this.tableRef.rows[0];
		  if (firstRow) {
				firstRow.parentNode.insertBefore(this.getTopRowRef(), firstRow);
		  }

    }
    ResultSet2.prototype.moveHeaderBack=function() {
		  this.headerIsInBody=true;
		  if (this.getTableHeaderRef().rows.length<1) {
				this.getTableHeaderRef().insertRow(0);
		  }
		  var row0=this.getTableHeaderRef().rows[0];
		  row0.parentNode.replaceChild(this.getTopRowRef(),row0);
    }	 
    ResultSet2.prototype.getTopRowRef=function() {
		  return this.topRowRef;
    }
		  
    ResultSet2.prototype.getTableWidth=function()
		  {
				if (this.tableRef.width) return this.tableRef.width;
				else return this.tableRef.style.width;
		  }
		  
    ResultSet2.prototype.insertTableHeader=function() {
		  try {
				var resultSetThis=this;
				var newtable1=document.createElement('table');
				newtable1.setAttribute("id",this.htmlIDTableHeader);
				if (!(isNaN(this.tableRef.border)) && this.tableRef.border>0) {
					 newtable1.border=this.tableRef.border;
				}
				newtable1.width=this.getTableWidth();
				
				newtable1.insertRow(0);
				newtable1.rows[0].parentNode.replaceChild(this.attNames[0],newtable1.rows[0]);
				newtable1.rows[0].className=CLASS_TR;
				var resultSetThis=this;//to be used in nested functions.
				var n=newtable1.rows[0].cells.length;
				for(var i=0;i<n;i++){
					 var cell=newtable1.rows[0].cells[i];
					 if (this.hasSorting(i)) {
						  this.arrowImages[i]=new Array(4);
						  this.arrowImages[i][0]=UP_ARROW_IMG.cloneNode(false);
						  this.arrowImages[i][1]=UP_ARROW_IMG_CHOSEN.cloneNode(false);
						  this.arrowImages[i][2]=DOWN_ARROW_IMG.cloneNode(false);
						  this.arrowImages[i][3]=DOWN_ARROW_IMG_CHOSEN.cloneNode(false);
						  //var upsort=this.getSortElement(i,1);
						  //var downsort=this.getSortElement(i,2);
						  this.addOnclickEventsToImages(i);
						  //newtable1.rows[0].cells[i].appendChild(upsort);
						  //newtable1.rows[0].cells[i].appendChild(downsort);
						  for (var j=0;j<4;j++) {
								newtable1.rows[0].cells[i].appendChild(this.arrowImages[i][j]);
						  }
					 }
					 if (isNodeEmpty(newtable1.rows[0].cells[i])) {//if empty insert space.
						  newtable1.rows[0].cells[i].appendChild(document.createTextNode(INVISIBLE_SYMBOL));
					 }
					 newtable1.rows[0].cells[i].className=CLASS_TD;
				}
				this.topRowRef=newtable1.rows[0];
				var mydiv=document.createElement("div");
				newtable1.cellPadding="0";
				newtable1.cellSpacing="0";
				mydiv.appendChild(newtable1);
                mydiv.style.zindex=50;
				newtable1.className=CLASS_TABLE;
				this.tableRef.parentNode.insertBefore(mydiv,this.tableRef);
		  }
		  catch(e) {
				handleError(e,"Could not insert table header");
		  }
    }
    //Returns true if columnNr has sorting.
    //Sorting is turned off by setting an omitSorting="i_1;i_2;...;i_n" attribute 
    //on the table-tag. For instance, omitSorting="0,3" will turn off sorting
    //in column 0 and 3 (zero-based).
    ResultSet2.prototype.hasSorting=function(columnNr) {
		  //First cache the omitSorting attribute in a local array.
		  if (!this.omitSorting) {
				try {
					 this.omitSorting=this.tableRef.getAttribute("omitSorting").split(";");
				}
				catch(e) { this.omitSorting=new Array(); }
		  }
		  var i=0;
		  while(i<this.omitSorting.length) {
				if (this.omitSorting[i]==columnNr && this.omitSorting[i].toString().length>0) { 
					 return false;
				}
				i++;
		  }
		  return true;
				
		  //return (this.sortTypes[columnNr].toLowerCase()!=NO_SORT);
    }
    ResultSet2.prototype.getSortElement=function(i,upOrDown) {
		  var resultSetThis=this;
		  var sortElement=document.createElement('a');
		  sortElement.href="javascript:;";
		  sortElement.tabIndex=-1;
		  var sortElementPic=document.createElement("img");
		  sortElementPic.src=(upOrDown==1?UP_ARROW_GIF:DOWN_ARROW_GIF);
		  sortElementPic.alt=(upOrDown==1?'Sorter stigende':'Sorter faldende');
		  sortElementPic.border=0;
		  if (upOrDown==1) {
				sortElement.onclick=function(e) {
					 //resultSetThis.markArrow(i,upOrDown);
					 resultSetThis.sortByIndex(i+1).syncronizeTable().stripTable();
				}
				this.arrowUpImages[i]=sortElementPic;
		  }
		  else {
				sortElement.onclick=function(e) {
					 //resultSetThis.markArrow(i,upOrDown);
					 resultSetThis.sortByIndex(i+1).reverseOrder().syncronizeTable().stripTable();
				}
				this.arrowDownImages[i]=sortElementPic;
		  }	
		  sortElement.appendChild(sortElementPic);	
		  return sortElement;
    }
    ResultSet2.prototype.addOnclickEventsToImages=function(i) {
		  //Save reference to avoid shadowing in inner functions...
		  var resultSetThis=this;
		  //add events...
		  this.arrowImages[i][0].onclick=function(e) {
				resultSetThis.markArrow(i,1).sortByIndex(i+1).syncronizeTable().stripTable();
		  }
		  this.arrowImages[i][1].onclick=function(e) {
				resultSetThis.markArrow(i,1).sortByIndex(i+1).syncronizeTable().stripTable();
		  }
		  this.arrowImages[i][2].onclick=function(e) {
				resultSetThis.markArrow(i,2).sortByIndex(i+1).reverseOrder().syncronizeTable().stripTable();
		  }
		  this.arrowImages[i][3].onclick=function(e) {
				resultSetThis.markArrow(i,2).sortByIndex(i+1).reverseOrder().syncronizeTable().stripTable();
		  }
		  return this;
    }
    //Performs sorting and marks the sorted column.
    //Simply fires the onclick event of the up- or down-arrow of the i'th column.
    ResultSet2.prototype.sortColumn=function(i,ascending) {
		  var type= (ascending ? 1 : 2);
		  this.markArrow(i, type).sortByIndex(i+1);
		  if(! ascending) {
				this.reverseOrder();
		  }
		  this.syncronizeTable().stripTable();
    }
    //Performs an initial sort of the resultset.
    //This initial sorting is triggered by putting an attribute init_sort on 
    //the column for which sorting is desired.
    //It can also be triggered by putting an init_sort_label-attribute on the table
    //and letting the value of this match an attribute rs_label on one of the columns.
    ResultSet2.prototype.doInitialSort=function() {
		  var cells=this.getTableHeaderRef().rows[0].cells;
		  var n=cells.length;
		  var i;
		  var didSort=false;
		  if (this.tableRef.getAttribute("init_sort_ref")) {
				var mylabel=this.tableRef.getAttribute("init_sort_ref").toLowerCase();
				if (mylabel) {
					 //Search through to see if init_sort_label of <table> match rs_label of some <td>.
					 for (i=0;i<n;i++) {
						  if (cells[i].getAttribute("rs_label") && mylabel==cells[i].getAttribute("rs_label").toLowerCase()) {
								var ascending=(cells[i].getAttribute("init_sort_descending") ? false : true);
								this.sortColumn(i,ascending);
								didSort=true;
								break;
						  }
					 }
				}
		  }
		  //If previous attempt to make an initial sort failed, 
		  //look through cells to find an init_sort="1" attribute.
		  if(! didSort) {
				for(i=0;i<n;i++) {
					 if (cells[i].getAttribute("init_sort")==1) {
						  var ascending=(cells[i].getAttribute("init_sort_descending") ? false : true);
						  this.sortColumn(i, ascending);
						  break;
					 }
				}
		  }
    }
    ResultSet2.prototype.addHeaderTableCell=function() {
		  var header=this.getTableHeaderRef();
		  header.rows[0].cells[0].parentNode.appendChild(document.createElement('td'));
		  var lastCell=header.rows[0].cells[header.rows[0].cells.length-1];
		  lastCell.width=SEARCH_CELL_WIDTH;
		  lastCell.setAttribute("valign","center");
		  lastCell.align="right";
		  lastCell.appendChild(document.createTextNode(INVISIBLE_SYMBOL));
		  lastCell.className=CLASS_TD;

    }
    ResultSet2.prototype.setAllWidths=function() {
		  try{
				this.setWidthsInARow(this.getTableHeaderRef().rows[0]);
				this.setWidthsInARow(this.tableRef.rows[0]);
		  }
		  catch(e) {
				handleError(e,"Could not set table widths");
		  }
        
    }
    ResultSet2.prototype.setWidthsInARow=function(tableRow) {
		  var i=0;
		  try {
				//test that tableRow really is a table row.
				if (tableRow && tableRow.cells && tableRow.cells.length>0) { 
					 var n=tableRow.cells.length;
					 for (i=0;i<n;i++) {
						  mywidth=this.getWidth(i);
						  if (mywidth!=-1) {
								tableRow.cells[i].width=mywidth;
						  }
						  else {
								tableRow.cells[i].removeAttribute("width");
						  }
					 }
				}
		  }
		  catch(e) {
				handleError(e,"Error in setWidthsInARow. Last index value was "+i);
		  }
    }
    ResultSet2.prototype.getWidth=function(index) {
		  if (this.widths.length>index && index>=0 && this.widths[index]!=null) {
				return this.widths[index];
		  }
		  else {
				return this.defaultwidth;
		  }
    }
    ResultSet2.prototype.setWidths=function(widthsarray) {
		  this.widths=widthsarray;
    }
    ResultSet2.prototype.setWidth=function(width,index) {
		  this.widths[index-1]=width;
    }

    ResultSet2.prototype.sortByIndex=function(index) {
		  if (! this.exceedsHeight()) {
				this.moveHeaderBack();
		  }
		  var zeroBasedIndex=index-1;
		  this.ensureInitializeColumn(zeroBasedIndex);
				
		  var compareFunction=this.getCompareFunction(zeroBasedIndex);				
				
		  function myCompare(a,b) {
				return compareFunction(a[index],b[index]);
		  }
		  this.rows.sort(myCompare);
		  if (! this.exceedsHeight()) {
				this.moveHeaderToBody();
		  }

		  return this;
    }
    //Alerts error message for the resultset component.
    ResultSet2.prototype.alertError=function(message, exception) {
		  var mymessage=message;
		  if (exception instanceof Error) {
				mymessage += " "+"Error: "+exception.message+" in l."+exception.lineNumber+". ";
		  }
		  alert(mymessage);
    }
    //Returns the compare function to be used for sorting for the index'th column.
    ResultSet2.prototype.getCompareFunction=function(zeroBasedIndex) {
		  var func=ResultSet2.compareFunctions[this.sortTypes[zeroBasedIndex]];
		  if (func instanceof Function) {
				return func;
		  }
		  else {
				this.alertError("There was found no compare function for the sorting type '"+this.sortTypes[zeroBasedIndex]+"'. You must either choose a different sorting type or declare a compare function for this sorting type (is done in the resultSet2_customize.js file).");
		  }
    }
    //Returns the sort type of the index'th column in the resultset.
    ResultSet2.prototype.getSortType=function(zeroBasedIndex) {
		  switch(this.sortTypes[zeroBasedIndex]) {
				case "defaultsort": 
				return "alpha";
				default:
				return this.sortTypes[zeroBasedIndex]+"";
		  }
    }
    ResultSet2.prototype.setLastSort=function(index,upOrDown) {
		  this.lastSort[0]=index;
		  this.lastSort[1]=upOrDown; //1 for up, 2 for down.
    }

    //upOrDown=1 for up and 2 for down.
    //Marks arrow number index.
    ResultSet2.prototype.markArrow=function(index,upOrDown) {
		  if (this.lastSort[0]!=null && this.lastSort[1]!=null) {
					 
				if (this.lastSort[1]==1) {//sorted up
					 //						  this.arrowImages[this.lastSort[0]].src=UP_ARROW_GIF;
					 this.arrowImages[this.lastSort[0]][0].style.display=IMG_STYLE;
					 this.arrowImages[this.lastSort[0]][1].style.display="none";
				}
				else {
					 this.arrowImages[this.lastSort[0]][2].style.display=IMG_STYLE;
					 this.arrowImages[this.lastSort[0]][3].style.display="none";						  
				}
		  }
		  if (upOrDown==1) {
				this.arrowImages[index][0].style.display="none";
				this.arrowImages[index][1].style.display=IMG_STYLE;
		  }
		  else {
				this.arrowImages[index][2].style.display="none";
				this.arrowImages[index][3].style.display=IMG_STYLE;
		  }
		  this.setLastSort(index,upOrDown);
		  return this;
    }


    //Makes sure that the rows in the table is permuted according to the 
    //indices in the internal representation of the rows.
    ResultSet2.prototype.syncronizeTable=function() {
		  var parent=this.rows[0][0].parentNode;
		  var n=this.rows.length;
		  this.cacheCheckBoxStates();
		  for (var i=0;i<n;i++) {
				parent.appendChild(this.rows[i][0]);
				this.rows[i][0].style.color="";
		  }
		  this.refreshCheckBoxStates();
		  return this;
    }
    ResultSet2.prototype.cacheCheckBoxStates=function() {
		  var checkboxes=this.getCheckBoxArray();
		  var states=this.getCheckBoxStateArray();
		  for (var i=0;i<checkboxes.length;i++) {
				states[i]=(checkboxes[i].checked ? true : false);
		  }
    }
    ResultSet2.prototype.refreshCheckBoxStates=function() {
		  var checkboxes=this.getCheckBoxArray();
		  var states=this.getCheckBoxStateArray();
		  for (var i=0;i<checkboxes.length;i++) {
				checkboxes[i].checked=states[i];
		  }
    }
	 
    ResultSet2.prototype.getCheckBoxStateArray=function() {
		  if (typeof this.checkboxStates=="undefined") {
				this.checkboxStates=new Array(this.getCheckBoxArray().length);
		  }
		  return this.checkboxStates;
    }

    ResultSet2.prototype.getCheckBoxArray=function()
		  {
				if (typeof this.checkboxes=="undefined")
				{
					 var el=this.tableRef.getElementsByTagName("input");
					 this.checkboxes=new Array();
					 for (var i=0;i<el.length;i++) {
						  if (el[i].type.toLowerCase()=="checkbox") this.checkboxes.push(el[i]);
					 }
				}
				return this.checkboxes;
		  }


    //Returns the height declared in the table.
    ResultSet2.prototype.getTableHeight=function() {
		  var height=this.tableRef.getAttribute("resultSet_height");
		  if (!(isNaN(height)) && height!=null && height!="") {
				return height;
		  }
		  else {
				return STANDARD_TABLE_HEIGHT;
		  }
    }
    ResultSet2.prototype.insertTableInDivElement=function() {
		  try {
				var mydiv=this.BoxDivEl;
				mydiv.style.position="relative";
				if (this.tableRef.getAttribute("width")) {
					 mydiv.style.width=this.tableRef.getAttribute("width");
				}
				else {
					 mydiv.style.width="auto";
				}	
				if (this.exceedsHeight()) {
					 this.setHeight(this.getTableHeight());
					 mydiv.style.overflow="auto";
				}
				else {
					 var cells=this.getTableHeaderRef().rows[0].cells;
					 if (cells[cells.length-1].width) { 
						  cells[cells.length-1].width -= SEARCH_CELL_WIDTH;
					 }
				}
		  }
		  catch(e) {
				handleError(e,"Could not insert table in div element.");
		  }
		  try{

				this.addHeaderTableCell();
				mydiv.style.borderLeft="0px solid #6387A0";
				mydiv.style.borderRight="0px solid #6387A0";
				mydiv.style.borderBottom="0px solid #6387A0";
				this.tableRef.parentNode.insertBefore(mydiv,this.tableRef);
				mydiv.appendChild(this.tableRef);
		  }
		  catch(e) {
				handleError(e, "Could not insert table in div element.");
		  }
		  
    }
    //Returns true if the estimated height is larger than the resultSet_Height given in the
    //declaration of the table.
    ResultSet2.prototype.exceedsHeight=function() {
		  var height=this.getTableHeight();
		  var actualHeight=parseInt(this.rows.length*this.getHeightScale());
		  if (!(height>actualHeight || height==-1)) {
				return true;
		  }
		  else {
				return false;
		  }
    }
		  
    ResultSet2.prototype.setHeight=function(h) {
		  if (h!=STANDARD_TABLE_HEIGHT) {
				this.BoxDivEl.style.height=h;
		  }
		  else {
				this.BoxDivEl.style.height="";
		  }
    }



    ResultSet2.prototype.getTableHeaderRef=function() {
		  return document.getElementById(this.htmlIDTableHeader);
    }
    //tries to give an estimate of the ratio between number of characters and width.	 
    ResultSet2.prototype.getWidthScale=function() {
		  return parseInt(this.getFontSize()*FONT_SIZE_TO_CHARACTER_WIDTH_RATIO); //
		  
    }
    //tries to give an estimate of the ratio between number of lines and height.
    ResultSet2.prototype.getHeightScale=function() {
		  return parseInt(this.getFontSize()*FONT_SIZE_TO_LINEHEIGHT_RATIO);
		  
    }
    //returns the font-size as set by the fontSize-attribute set in the main table.
    ResultSet2.prototype.getFontSize=function() {
		  var fontSize=parseFloat(this.tableRef.getAttribute("fontSize"));
		  if (isNaN(fontSize)) {
				fontSize=10; //default
		  }		  
		  return parseInt(fontSize);
    }	 
    ResultSet2.prototype.initializeAttNames=function() {
		  try {
				var n=this.tableRef.rows[0].cells.length;
				for (var i=0;i<n;i++) {
					 try {
						  this.attNames[i+1]=this.getTextValue(this.tableRef.rows[0].cells[i]);
					 }
					 catch(e) {
						  this.alertError("2",e);  
					 }
				}

				this.attNames[0]=this.tableRef.rows[0].cloneNode(1);
				this.tableRef.deleteRow(0);
		  }
		  catch(e) {
				handleError(e,"Could not initialize attribute names.");
		  }
    }
    ResultSet2.prototype.resolveType=function(colno) {
		  var type = DEFAULT_SORT;
		  var cells = this.attNames[0].getElementsByTagName('td');
		  try {
				var content = this.getTextValue(cells[colno]);
		  }
		  catch(e) {
				this.alertError("Error in resolving type in column "+colno+".",e);
		  }
		  var mymatch=(new String(content)).match(/\{\{(.+)\}\}/);
		  if (mymatch && mymatch.length==2) {
				type = mymatch[1];
				cells[colno].innerHTML =cells[colno].innerHTML.replace(/\{\{.*\}\}/,''); 
		  } 
		  return type;
    }
    //Initializes the sort types for the resultset.
    ResultSet2.prototype.initializeSortTypes=function() {
		  if (this.tableRef.getAttribute("sortTypes")) 
		  this.initializeSortTypesFromTableTag();
		  else
		  this.initializeSortTypesFromTopRow();
		  //Set up array of booleans keeping track of which columns have been preprocessed.
		  for (var i=0;i<this.sortTypes.length;i++) {
				this.mustPrepareColumn[i]=true;
		  }
    }
    ResultSet2.prototype.validateSortTypes=function() {
		  for (var i=1;i<this.sortTypes.length;i++) {
				this.getCompareFunction(i);
				this.getPreprocessFunction(i);
		  }
    }
		  
    //Recognizes the type of a column
    //from regular expression patterns in the ResultSet2.typePatterns object.
    ResultSet2.prototype.recognizeType=function(i) {
		  var j=0;
		  while(j<this.rows.length) {
				var textValue=this.getTextValue(this.tableRef.rows[j].cells[i]);
				if ( textValue!="" && typeof textValue != "undefined") {
					 for (var regexpKey in ResultSet2.typePatterns) {
						  if (textValue.match(ResultSet2.typePatterns[regexpKey])) {
								return regexpKey;
						  }
					 }
				}
				j++;
		  }
    }
		  
		  
    //Finds the sort types from the attribute in the <table>-element named sortTypes as 
    //a semicolon seperated list.
    ResultSet2.prototype.initializeSortTypesFromTableTag=function() {
		  var typesString=this.tableRef.getAttribute("sortTypes");
		  var arrTypes=typesString.split(";");
		  for (var i=0;i<Math.min(arrTypes.length,this.getNumberOfCols());i++) {
				this.sortTypes[i]=this.getSortType(arrTypes[i], i);				
		  }
		  //If only a few of the sorttypes were explicitly given, make defaults for the rest.
		  if (arrTypes.length<this.sortTypes.length) {
				for (var i=arrTypes.length;i<this.sortTypes.length;i++) {
					 this.sortTypes[i]=DEFAULT_SORT;
				}
		  }
    }
		  
    //Initializes the sort types from information in the top row.
    ResultSet2.prototype.initializeSortTypesFromTopRow=function() {
		  var mysort="";
		  var cells=this.attNames[0].getElementsByTagName('td');
		  for (var i=0;i<cells.length;i++) {
				mysort=(new String(cells[i].getAttribute("sort"))).toLowerCase();
				this.sortTypes[i]=this.getSortType(mysort, i);
		  }

    }

    ResultSet2.prototype.getSortType=function(mysort, index) {
		  if (mysort=="" || mysort=="null") {
				return DEFAULT_SORT;
		  } 
		  else if (mysort=="auto") {
				return this.recognizeType(index);
		  }	
		  else return mysort;
    }
		  
    ResultSet2.prototype.prepareColumnForSort=function(colnr) {

		  var processFunc=this.getPreprocessFunction(colnr);//function used to process the column.

		  if (processFunc instanceof Function) {
				var n=this.rows.length;
				var i;
				try {
					 for (i=0; i<n; i++) {
						  this.rows[i][colnr+1]=processFunc(this.rows[i][colnr+1]);
					 }
				}
				catch(e) {
					 this.alertError("Error in preprocessing column nr. "+colnr+" with type "+this.sortTypes[colnr]+".",e);
				}
		  }
		  return this;
    }
    //Returns the preprocess function for the colnr'th column in the resultset.
    ResultSet2.prototype.getPreprocessFunction=function(colnr) {
		  var func=ResultSet2.preprocessFunctions[this.sortTypes[colnr]];
		  if (func instanceof Function || func==0) {
				return func;
		  }
		  else {
				this.alertError("There was found no preprocess function for the sorting type '"+this.sortTypes[colnr]+"'. You must either choose a different sorting type or declare a preprocess function for this sorting type (is done in the resultSet2_customize.js file).");
		  }
    }

}
catch(e) {
    handleError(e,"Could not define functions in result set.");
}	
	
//Takes a dom-element as argument and returns true if and only if 
//it is empty (0 childNodes) or contains a single childNode with 
//whitespace characters only.
function isNodeEmpty(mynode) {
    var bool=true;
    if (mynode.childNodes.length>1) {
		  bool=false;
    }
    else if (mynode.childNodes.length==1) {
		  if (mynode.childNodes[0].nodeType==3) {
				if (!(mynode.childNodes[0].nodeValue.match(/^[\s]*$/))) {
					 bool=false;
				}
		  }
    }
    return bool;
}
//this function will be defined below. 
ResultSet2.prototype.getTextValue=null;
	 
if (getBrowserType()==1) {
    //W3C-compatible function to extract text-content from a node. 
    ResultSet2.prototype.getTextValue=function(domNode) {
		  //If defined return inner text, else return empty string.
		  if (domNode) return domNode.innerText;
		  else return "";
    }
}
else {
    ResultSet2.prototype.getTextValue=function(domNode) {
		  var str="";
		  if (domNode) {
				if (domNode.nodeType==1) {
					 var i;
					 var n=domNode.childNodes.length;
					 for (var i=0;i<n;i++){
						  if (domNode.childNodes[i].nodeType==3) {
								str=str+domNode.childNodes[i].nodeValue;
						  }
						  else {
								str=str+this.getTextValue(domNode.childNodes[i]);
						  }
					 }
				}
				return str.replace(/^[\s]+/,"").replace(/[\s]+$/,"");
		  }
		  else return "";
    }				
		  
}


//runs through the html-page, finds all tables, looks up the 
function automaticallyDetectResultSet() {
    var tables=document.getElementsByTagName('TABLE');
    //           new ResultSet2(tables[1]);
    var tablesArray=new Array();
    var n=tables.length;
    for (var i=0;i<n;i++) {
		  if (tables[i].getAttribute("resultSet")!="" && tables[i].getAttribute("resultSet")!=null && tables[i].getAttribute("didDetect")!=1) {
				tablesArray.push(tables[i]);
				tables[i].setAttribute("didDetect",1);
		  }
    }
    n=tablesArray.length;
    for (var i=0;i<tablesArray.length;i++) {
		  new ResultSet2(tablesArray[i]);
    }
}

function JstripTable(tablesRef)
{
    var rows=tablesRef.getElementsByTagName("tr");
    var n=rows.length;
    for(var i=0;i<n;i++)
		  {
				var modulo2 = (i%2)+1; 
				if (rows[i].className != CLASS_SEARCH_RESULT){
					 rows[i].className = CLASS_STRIP + modulo2;
				}
		  }	
} 

function stripTable(tablesRef)
{
    var rows=tablesRef.getElementsByTagName("tr");
    for(var i=0;i<rows.length;i++)
		  {
				var modulo2 = (i%2)+1;
				rows[i].className = CLASS_STRIP + modulo2;
		  }	
}
		
function ResultSet2MarkRow(rowRef)
{
		
    rowRef.style.backgroundImage='url(../lib/images/ResultSetImages/selectedRow.gif)';
}
		           

