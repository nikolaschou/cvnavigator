using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Web.Code;

namespace Cvm.Web.AdminPages
{
    public partial class Contact : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
