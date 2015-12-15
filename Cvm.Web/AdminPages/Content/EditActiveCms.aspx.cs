using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Security;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Users;
using Cvm.Web.Facade;

namespace Nshop.Web.AdminPages.Content
{
    public partial class EditActiveCms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utl.AllowContentEditing())
            {
                //Ok
            } else
            {
                throw new SecurityException("Not enough privileges for this page.");
            }
        }
    }
}
