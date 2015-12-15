function stripTables()
{
	var tables = document.getElementsByTagName("table");
	for (var i=0;i<tables.length;i++)
	if  (tables[i].getAttribute("stripped")=="1")
		{
			stripTable(tables[i]);
		}	
}
function stripTable(tablesRef)
{
  var rows=tablesRef.getElementsByTagName("tr");
  for(var i=0;i<rows.length;i++)
 {
		var modulo2 = (i%2);
		rows[i].className = "itemstyle" + modulo2;
	}	
}
           
