using System;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Companies;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;

namespace Cvm.Web.Public
{
    public partial class OpenJobs : System.Web.UI.Page
    {
        private SysId sysId = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();

            ListView1.DataSource = QueryMgrDynamicHql.Instance.GetOpenSystemJobs(sysId.SysIdInt);
            ListView1.DataBind();
        }
        
        protected override void OnPreInit(EventArgs e)
        {
            sysId = ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull();

            if (sysId != null)
            {
                ContextObjectHelper.CurrentSysId.OverrideObject(sysId);
                MasterPageHelper.Instance.OnPageInit(true);
            }
            else
            {
                ContextObjectHelper.CurrentSysId.Clear();
                MasterPageHelper.Instance.OnPageInit(false);
            }
        }

        protected void LBApplyJob_Click(Object sender, EventArgs e)
        {
            //Response.Redirect("~/Public/viewjob.aspx?jobId=" + ((LinkButton)(sender)).CommandArgument.ToString());
            CvmPages.ViewJob.IncludeParm(QueryParmCvm.jobId, ((LinkButton)(sender)).CommandArgument).IncludeExistingParms().Redirect();
        }

        protected string TruncateAtWord(string value, int length)
        {
            if (value == null || value.Trim().Length <= length)
                return value;

            int index = value.Trim().LastIndexOf(" ");

            while ((index + 3) > length)
                index = value.Substring(0, index).Trim().LastIndexOf(" ");

            if (index > 0)
                return value.Substring(0, index) + "...";

            return value.Substring(0, length - 3) + "...";
        }
    }
}