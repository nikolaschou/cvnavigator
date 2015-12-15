ResultSet2.developmentMode=false;

/*Make new objects to store compare functions, preprocessor functions, regular expressions 
and type specific widths.
*/

ResultSet2.compareFunctions=new Object();
ResultSet2.preprocessFunctions=new Object();
ResultSet2.typePatterns=new Object();
ResultSet2.typeWidths=new Object();

//Sort numbers
ResultSet2.compareFunctions.numeric = function(a,b) {
	 return a-b;    
}

//Sort numbers
/*ResultSet2.compareFunctions.no = function(a,b) {
	 return 0;    
	 }*/

//Sort alphanumerical strings
ResultSet2.compareFunctions.alpha = function(a,b) {
	 if (a=="") { return 1; }
	 else if (b=="") { return -1; }
	 if (a < b) { return -1; }
	 else if (a == b) { return 0; }
	 else { return  1; }
}

ResultSet2.compareFunctions.datetime = ResultSet2.compareFunctions.alpha;
ResultSet2.compareFunctions.date = ResultSet2.compareFunctions.alpha;
ResultSet2.compareFunctions.currency = ResultSet2.compareFunctions.numeric;
ResultSet2.compareFunctions.undefined = ResultSet2.compareFunctions.alpha;
ResultSet2.compareFunctions.checkbox = ResultSet2.compareFunctions.alpha;


/************************** PREPROCESS FUNCTIONS ********************/

//For alphanumerical, simply replace " " with "".
ResultSet2.preprocessFunctions.alpha = function(d) {
	 return (d!=" " ? d : "");
}	

//Transforms danish date time format to something which can be sorted alphanumerically.
ResultSet2.preprocessFunctions.datetime = function(d) {
	 return d.substring(6,10)+d.substring(3,5)+d.substring(0,2)+d.substring(10);
}

//Transforms danish date time format to something which can be sorted alphanumerically.
ResultSet2.preprocessFunctions.date = function(d) {
	 return d.substring(6,10)+d.substring(3,5)+d.substring(0,2);
}

ResultSet2.preprocessFunctions.currency = function(d) {
	 var n=parseFloat(d.replace(/\./g,"").replace(",","."));
	 return (isNaN(n)? Number.MAX_VALUE : n);
}

ResultSet2.preprocessFunctions.numeric = function(d) {
	 //Replace comma with dot
	 var n=parseFloat(d.replace(",","."));
	 return (isNaN(n) ? Number.MAX_VALUE : n);
}

ResultSet2.preprocessFunctions.undefined = ResultSet2.preprocessFunctions.alpha;

ResultSet2.preprocessFunctions.checkbox =function(cell) {
	 var elements=cell.getElementsByTagName("input");
   var element=null;
   var i=0;
   while(i<elements.length && element==null) {
      if (elements[i].type.toLowerCase()=="checkbox") element=elements[i];
      i++;
   }
	 if(element!=null) {
		  element.setAttribute("state",(element.checked?"1":"0"));
		  return (element.checked?1:2);
	 }
	 else return 0;
}

ResultSet2.preprocessFunctions.checkbox.dynamic=true;



/******************** TYPE PATTERNS *********************************/
/*
  Insert patterns to be matched against the content of the columns.
  To make a pattern associated with a type insert a new line below.
*/
//Currency matches things like 9.333.234,55
ResultSet2.typePatterns.currency = /^[0-9]{1,3}(\.[0-9]{3})*,[0-9]{2}$/;
//Numeric matches things like 9 and 9,332
ResultSet2.typePatterns.numeric = /^[0-9]*(,[0-9]*)?$/;
//Datetime matches things like 31-02-03 09:40
ResultSet2.typePatterns.datetime = /^[0-9]{2}-[0-9]{2}-[0-9]{2,4} [0-9]{2}:[0-9]{2}(\:[0-9]{2})?$/;
//Datetime matches things like 31-02-03 09:40
ResultSet2.typePatterns.date = /^[0-9]{2}-[0-9]{2}-[0-9]{2,4}$/;
//alpha is default, hence matching everything.
ResultSet2.typePatterns.alpha = /.*/;

/********************* TYPE SPECIFIC WIDTHS *************************/
/*
Give the default values of widths for the different types of columns.
 */

ResultSet2.typeWidths.currency=70;
ResultSet2.typeWidths.numeric=91;
ResultSet2.typeWidths.datetime=160;
ResultSet2.typeWidths.date=70;
ResultSet2.typeWidths.alpha=100;
ResultSet2.typeWidths.checkbox=100;

ResultSet2.typeWidths.undefined=100;
