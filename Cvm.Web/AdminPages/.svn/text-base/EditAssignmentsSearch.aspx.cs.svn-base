using System;
using System.Collections;
using System.Collections.Generic;
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
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.AdminPages
{
    public partial class EditAssignmentsSearch : System.Web.UI.Page
    {
        protected readonly BusinessRequestObject<Assignment> req = new BusinessRequestObject<Assignment>(QueryParmCvm.id);

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected override void OnInit(EventArgs e)
        {
            EditAssignments1.ChosenIndex = 1;
            this.SearchCtrl.OnResourcesSelected = HandleResources;
            this.SearchCtrl.OmitResources = new IdfrStringList(req.Current.AssignmentResources.Select(r=>(long)r.ResourceId));
        }

        private void HandleResources(IList<Resource> rs)
        {
            foreach (Resource r in rs)
            {
                Utl.Msg.PostMessage("EditAssignmentsSearch",r.FullName);
                this.req.Current.AddNewResourceAndSave(r);
            }
        }
    }
}
