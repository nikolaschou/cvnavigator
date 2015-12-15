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
using Cvm.Web.Navigation;

namespace Cvm.Web.Public
{
    public partial class Public : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnClick_SearchBtn(object sender, ImageClickEventArgs e)
        {
            CvmPages.SearchCvPage.SetParm(QueryParmCvm.search,this.TextBox1.Text).Redirect();
        }
    }
}
