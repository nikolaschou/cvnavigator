using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Cvm.Web.Code;
using Napp.Backend.Hibernate;

namespace Cvm.Web.AdminPagesGlobal
{
    public partial class CacheControl : System.Web.UI.Page
    {
        /// <summary>
        /// Insert this block at all pages.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnClickClearCaches(object sender, EventArgs e)
        {
            HibernateSessionFactory.Instance.ClearSecondLevelCache();
        }
    }
}
