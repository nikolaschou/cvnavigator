using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Web.Code;

namespace Cvm.Web.Public
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.MySkillMapGraph.CurrentSkillName = this.SkillMap1.Top1SkillName;
        }
    }
}