using System;
using System.Web.UI;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Web.ExtControls.Services;

namespace Cvm.Web.AdminPages
{
    public partial class AdminMasterPage0 : System.Web.UI.MasterPage
    {
        private AppVersion versionInfo;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keepAlive", "keepAliveSchedule();", true);
            }
        }

        protected override void OnInit(EventArgs e)
        {           
            base.OnInit(e);
         
            if(!MasterPageHelper.Instance.DidInit)
            {
                throw new Exception("MasterPageHelper.Instance.DidInit must be called during phase OnPreInit.");
            }
        }

        protected void OnClickStopSimulation(object sender, EventArgs e)
        {
            long sysUserId = ContextObjectHelper.CurrentUser.UserId;
            string userName = ContextObjectHelper.CurrentUserName;
            CvmFacade.Security.StopSimulation();
            Utl.Msg.PostMessage("AdminMasterPage.SimulationStopped", userName);
            CvmPages.UserAdmin.SetParm(QueryParmCvm.userId, sysUserId).Redirect();
        }

        protected override void OnPreRender(EventArgs e)
        {
            //Push any validators into this panel as they need to be pushed into 
            //a control that spans all validated controls.
            new ValidationManager().PushDropDownValidators(this.form1);
        
            if (MasterPageHelper.Instance.AvoidAutoComplete)
            {
                this.form1.Attributes["autocomplete"] = "off";
            }
        }

        protected AppVersion GetLatestVersionInfo()
        {
            if (this.versionInfo == null)
            {
                this.versionInfo = MasterPageHelper.Instance.GetLatestVersionInfo();
            }

            return versionInfo;
        }
    }
}