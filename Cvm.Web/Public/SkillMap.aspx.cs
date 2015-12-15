using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Web.Code;
using Napp.Backend.DataAccess;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Public
{
    public partial class SkillMap : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}