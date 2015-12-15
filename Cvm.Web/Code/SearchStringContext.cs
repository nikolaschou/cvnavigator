using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.Code
{
    /// <summary>
    /// Encapsulates the logic behind the sticky search string that will be stored on the session scope.
    /// </summary>
    public class SearchStringContext
    {
        private const string token = "mustInitSearchStr";
        private const string searchstr_token = "searchStr";
        private SearchStringContext() {}

        public static SearchStringContext Instance=new SearchStringContext();
        
        private bool mustInitSearchStr
        {
            get
            {
                return ((HttpContext.Current.Items[token] as bool?) ?? true);
            }
            set
            {
                HttpContext.Current.Items[token] = value;
            }
        }
        /// <summary>
        /// Returns the current search string which will be whatever was searched most recently.
        /// </summary>
        public String SearchString
        {
            get
            {
                if (mustInitSearchStr)
                {
                    //If provided on the query string, use that
                    string searchStr = QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.search);
                    if (searchStr != null && searchStr.Trim().Length > 0)
                    {
                        _SearchString = searchStr;
                    }
                    mustInitSearchStr = false;
                }
                return _SearchString;
            }
        }

        protected String _SearchString
        {
            get
            {
                return HttpContext.Current.Items[searchstr_token] as String;
            }
            set
            {
                HttpContext.Current.Items[searchstr_token] = value;
            }
        }
    }
}
