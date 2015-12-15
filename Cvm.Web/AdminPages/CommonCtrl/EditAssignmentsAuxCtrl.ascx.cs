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
using Cvm.Web.Facade;
using Cvm.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class EditAssignmentsAuxCtrl : System.Web.UI.UserControl
    {
        public Assignment MyAssignment { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void DataBind()
        {
            if (MyAssignment != null)
            {
                bool hasUser = MyAssignment.HasAssociatedSysUser();
                if (hasUser)
                {
                    this.LinkToUser.PageLink = CvmPages.UserAdminLink((long)MyAssignment.AssociatedSysUserId);
              
                }

            }
            base.DataBind();
        }

        protected void OnClickCreateUserBtn(object sender, EventArgs e)
        {
            if (MyAssignment.RelatedClientSysUserObj==null)
            {
                Utl.Msg.PostMessage("EditAssignmentsAuxCtrl.MissingContact");
            } else 
            {
                CvmFacade.UserAdmin.CreateNewMemberhipUserOrFail(this.MyAssignment.RelatedClientSysUserObj.RelatedUserObjObj);
            }
            this.DataBind();
        }
    }
}