using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Business.Multisite;
using Napp.Web.Navigation;

namespace Cvm.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected bool hasJobIdParm;
        private SysId sysId = ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull();

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);

            hasJobIdParm = Utl.Query.HasParm(QueryParmCvm.jobId);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //We should force the user to login using no host prefix
            ContextObjectHelper.EnsureNoSubDomain();
            this.LoginForm.TitleText = "";
            this.LoginForm.UserNameLabelText = Utl.ContentHlp("Login.UserName");
            this.LoginForm.PasswordLabelText = Utl.ContentHlp("Login.Password");
            this.LoginForm.RememberMeText = Utl.ContentHlp("Login.RememberMe");
            
            AdminPages_AdminMasterPage.IsLoginPage = true;
            if (Utl.Query.GetParmOrNull(QueryParmCvm.logout) != null)
            {
                CvmFacade.Security.Logout();
                PageNavigation.GetCurrentLink().Redirect();
            }
        }

        protected void OnLoggedInHandler(object sender, EventArgs e)
        {
            if (hasJobIdParm)
                CvmPages.StartPage.IncludeExistingParms().Redirect();
            else
                CvmPages.StartPage.Redirect();

            //ContextObjectHelper.CurrentSysId.Clear();
        }

        protected void LBSignUp_Click(Object sender, EventArgs e)
        {
            if (hasJobIdParm)
                CvmPages.SignUpPage.IncludeExistingParms().Redirect();
            else
                CvmPages.SignUpPage.Redirect();
        }

        protected void LBCompanySignUp_Click(object sender, EventArgs e)
        {
            CvmPages.CompanySignup.IncludeExistingParms().Redirect();
        }
    }
}
