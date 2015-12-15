using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Cvm.Backend.Business.Assignments;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.AdminPages
{
    public partial class EditAssignmentPrintDef : System.Web.UI.Page
    {
        protected readonly BusinessRequestObject<Assignment> req = new BusinessRequestObject<Assignment>(QueryParmCvm.id);
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
            EditAssignments1.ChosenIndex = 2;

        }

        protected override void OnInit(EventArgs e)
        {
            this.AutoForm.ObjectSourceInstance = this.req.Current.GetOrCreatePrintDefinitionObjSaved();
            this.AutoForm.BuildForm();
        }
    }
}
