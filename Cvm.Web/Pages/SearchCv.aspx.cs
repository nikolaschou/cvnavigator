using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.Pages
{
    public partial class SearchCv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String searchString = QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.search);
            if (!String.IsNullOrEmpty(searchString))
            {
                IList<ISearchResult> result = SearchCvFacade.Instance.SearchCvByString(searchString);
                this.ResultsGrid.DataSource = result;
                this.ResultsGrid.DataBind();
            }
        }

        protected String GetResultLink(ISearchResult result)
        {
            switch(result.ResultType)
            {
                case(SearchResultTypeEnum.Resource):
                    return CvmPages.EditCvPage.SetParm(QueryParmCvm.id,(result as Resource).ResourceId).GetLinkAsPopupScript();
                default:
                    return null;
            }
        }
    }
}
