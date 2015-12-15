using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Backend.Business.Assignments;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.AdminPages
{
    public partial class EditAssignments1 : System.Web.UI.MasterPage
    {
        protected bool hasIdParm;
        protected bool isModeNew;

        protected readonly BusinessRequestObject<Assignment> req = new BusinessRequestObject<Assignment>(QueryParmCvm.id);
        protected void Page_Load(object sender, EventArgs e)
        {
            Utl.SetCurrentBusinessObject(req.Current);
            this.AuxCtrl2.MyAssignment = this.req.Current;
            this.AuxCtrl2.DataBind();
            if (hasIdParm)
            {
                TabularCtrl1.SetupTabs(TabularCtrlHelper.AssignmentTabular, ChosenIndex);
            }
        }

        public static int ChosenIndex
        {
            get
            {
                int? i = HttpContext.Current.Items["chosenIndex"] as int?;
                return i ?? 0;
            }
            set { HttpContext.Current.Items["chosenIndex"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            hasIdParm = Utl.Query.HasParm(QueryParmCvm.id);
            isModeNew = Utl.Query.GetMode() == PageMode.New;
            

        }

    }
}
