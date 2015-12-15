using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Napp.Backend.Business.Meta;
using Napp.Backend.Hibernate;

namespace Cvm.Web.AdminPagesGlobal
{
    public partial class ListAllUsers : System.Web.UI.Page
    {
        protected DataTable table;
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }



    }
}