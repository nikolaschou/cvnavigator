using System;
using System.Collections.Generic;
using System.IO;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class SearchCv : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
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
                    long resourceId;
                    if (result is Resource) resourceId = (result as Resource).ResourceId;
                    else if (result is ResourceImport) resourceId = (result as ResourceImport).ResourceId;
                    else throw new NotImplementedException("Sanity check");
                    return CvmPages.EditCvPage.SetParm(QueryParmCvm.id,resourceId).GetLinkAsPopupScript();
                default:
                    return null;
            }
        }
    }
}
