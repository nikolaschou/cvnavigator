<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SearchCtrl.ascx.cs"
    Inherits="Cvm.Web.AdminPages.CommonCtrl.SearchCtrl" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
    <%@ Register Assembly="Napp.Web.ExtControls" Namespace="Napp.Web.ExtControls" TagPrefix="Ex" %>
<div>
<Ex:SubForm runat="server">

            <div onkeypress="javascript:return defaultEnter(event,'searchButton')" style="white-space:nowrap">
                <input type="text" id="searchText" value="<%=QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.search) %>"/>
                <img src="../images/master/search-glass-small-black.PNG" id="searchButton" alt="search" onclick="javascript:doSearch()" />
                <script type="text/javascript" language="javascript">
                    function doSearch() {
                        window.location.href='../AdminPages/SearchCv.aspx?search='+document.getElementById('searchText').value;
                    }
                </script>
            
            </div>

</Ex:SubForm>
</div>