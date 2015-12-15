using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.Code
{
    /// <summary>
    /// Provides convenience methods to create links between pages.
    /// This layer sits on top of the services provided by the Navigation
    /// layer
    /// </summary>
    public class LinkHelper
    {
        private LinkHelper()
        {
        }
        public static readonly LinkHelper Instance=new LinkHelper();

        public PageLink GetPrintLink(Resource res)
        {
            return CvmPages.PrintCvPage.SetParm(QueryParmCvm.id, res.ResourceId).setAsPopup(MakePopupName(res));
        }
        public PageLink GetDeleteLink(Resource res)
        {
            return CvmPages.DeleteObjectLink(res.GetObjectType(), res.ResourceId, null);
        }
        private string MakePopupName(Resource res)
        {
            return "res" + res.ResourceId;
        }
        public PageLink GetEditLink(Resource res)
        {
            return CvmPages.EditCvPage.SetParm(QueryParmCvm.id, res.ResourceId).setAsPopup(MakePopupName(res));
        }

    }
}
