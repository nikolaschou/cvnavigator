using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Backend.DataAccess;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;

namespace Cvm.Web.Public
{
    public partial class SkillGraph : System.Web.UI.Page
    {

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }
        protected void OnSearchTextChange(object sender, EventArgs e)
        {
            PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.search, this.SearchStringBx.Text).Redirect();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(this.GetSkillName())) PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.search, "java").Redirect();
            }
            this.MySkillMap.CurrentSkillName = this.GetSkillName();

        }
        protected override void OnPreRender(EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.GetSkillName())) this.SearchStringBx.Text = this.GetSkillName();
        }

        protected string GetSkillName()
        {
            return QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.search);
        }
    }
}