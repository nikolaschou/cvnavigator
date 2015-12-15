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
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class DeleteObject : System.Web.UI.Page
    {

        protected override void OnPreInit(EventArgs e) {
            String type = QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.type);
            Type t= TypeParser.ParseType(type);
            MasterPageHelper.Instance.OnPageInit(TypeParser.IsMultisiteEntity(t));
        }

      

        protected void Page_Load(object sender, EventArgs e)
        {
            AdminPages_AdminMasterPage.IsPopup = true;
        }
    }
}
