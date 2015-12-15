using System;
using System.Web.UI;
using Cvm.Backend.Business.Companies;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Business.Multisite;
using Nshop.Backend.Business.GenericDelegates;

namespace Cvm.Web.Public
{
    public partial class CompanySignup : Page
    {
        protected UserObj user = new UserObj();
        protected Company company = new Company();

        protected override void OnPreInit(EventArgs e)
        {
            SysId sysId = ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull();

            // Outcommetted this because it causes problems everytime we try to login, but why did I implement it in the first place?
            // it has something todo with the jobportal
            if (sysId != null)
            {
                ContextObjectHelper.CurrentSysId.OverrideObject(sysId);
                MasterPageHelper.Instance.OnPageInit(true);
            }
            else
                MasterPageHelper.Instance.OnPageInit(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();
            UserObjCtrl.BuildForm();
            CompanyInfoCtrl.BuildForm();

            UserObjCtrl.PopulateFront();
            CompanyInfoCtrl.PopulateFront();

            // Update which controls to show
            UpdateControlView();

            UpdateButtonsView();
        }

        protected void ABCreateCompany_Click(object sender, EventArgs e)
        {
            bool success = false;

            UserObjCtrl.PopulateBack();
            CompanyInfoCtrl.PopulateBack();

            try
            {
                // User has to have ID hence we add it before we commit the new user
                user.GenerateIdentifier();

                // Create the user and company
                if (CvmFacade.UserAdmin.ValidateUser(user.Email, ATBPassword.Text, ATBReEnterPassword.Text))
                    success = CvmFacade.NewCompany.CreateCompanyAndUser(user, company, ATBPassword.Text);

                if (success)
                {
                    Utl.Msg.PostMessage("Company.CompanyAndUserCreated", company.Name, user.FullName);
                }
            }
            catch (Exception)
            {
                CvmFacade.NewCompany.RollbackCompanyAndUserCreation(user, company);
                throw;
            }
            
            if (success) 
                CvmPages.RootPath.Redirect();
        }

        protected void ABReturn_Click(object sender, EventArgs e)
        {
            CvmPages.RootPath.Redirect();
        }

        private void UpdateControlView()
        {
            // Hide all controls
            CCUserData.Visible = false;
            CCCompanyData.Visible = false;

            CCUserData.Visible = true;
            CCCompanyData.Visible = true;
        }

        private void UpdateButtonsView()
        {
            ABCreateCompany.Visible = true;
        }
    }

    /// <summary>
    /// Extends UserObj using a SysCode that is dynamically resolved from what is defined on this page.
    /// </summary>
    public class MyUserObj : UserObjWrap
    {
        internal static ObjectGetter<SysCode> SysCodeGetter;

        public MyUserObj() : base()
        {
        }

        public override SysCode? ContextSysCode
        {
            get
            {
                return SysCodeGetter();
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }
    }
}