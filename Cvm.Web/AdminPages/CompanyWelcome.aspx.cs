using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Companies;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.Public
{
    public partial class CompanyWelcome : System.Web.UI.Page
    {
        protected readonly BusinessRequestObject<JobAnnouncement> req = new BusinessRequestObject<JobAnnouncement>(QueryParmCvm.id);
        protected UserObj newUserObj = new UserObj();
        protected UserObj user = new UserObj();
        protected Company company = new Company();
        protected bool isModeNew;
        protected bool hasJobIdParm;
        private bool newUser = false;
        private bool deleteUser = false;
        private bool editCompany = false;
        private long? companyId;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected override void OnInit(EventArgs e)
        {
            hasJobIdParm = Utl.Query.HasParm(QueryParmCvm.id);
            isModeNew = Utl.Query.GetMode() == PageMode.New;

            // Job Description
            if (hasJobIdParm || isModeNew)
            {
                JobForm.OmitProperties = Tables.JobAnnouncement.AddCompanyId.AddLastModifiedTs.SelectedColumnsStr();
                JobForm.ObjectSourceInstance = req.Current;
                JobForm.BuildForm();
                JobForm.PopulateFront();
            }

            // New User
            NewUserObjCtrl.IncludeOnlyProperties = Tables.UserObj.AddEmail.AddFirstName.AddLastName.AddMobile.AddPhone.SelectedColumnsStr();
            NewUserObjCtrl.ObjectSourceInstance = newUserObj;
            NewUserObjCtrl.BuildForm();
            NewUserObjCtrl.PopulateFront();

            // Company
            //Company User data
            user = ContextObjectHelper.CurrentUser;
            UserObjCtrl.IncludeOnlyProperties = Tables.UserObj.AddEmail.AddFirstName.AddLastName.AddMobile.AddPhone.SelectedColumnsStr();
            UserObjCtrl.ObjectSourceInstance = user;
            UserObjCtrl.BuildForm();
            UserObjCtrl.PopulateFront();

            ATBReEnterPassword.Text = ATBPassword.Text = Membership.GetUser(ContextObjectHelper.CurrentUser.UserName).GetPassword();

            // Company data
            company = ContextObjectHelper.CurrentUser.SysUserObjInContextOrNull.RelatedCompanyObj;
            CompanyInfoCtrl.ObjectSourceInstance = company;
            CompanyInfoCtrl.BuildForm();
            CompanyInfoCtrl.PopulateFront();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();

            if (!IsPostBack)
            {
                UpdateControlView();
                UpdateButtonsView();
            }
        }

        private void UpdateControlView()
        {
            CCJobDescription.Visible = false;
            JobForm.Visible = false;
            CCNewUserData.Visible = false;
            CCDeleteUser.Visible = false;
            CCCompanyData.Visible = false;
            CCUserData.Visible = false;

            if (isModeNew || hasJobIdParm)
            {
                CCJobDescription.Visible = true;
                JobForm.Visible = true;
            }

            if (newUser)
            {
                #region Update Content
                #endregion // Update content

                CCNewUserData.Visible = true;
            }

            if (deleteUser)
            {
                #region Update Content
                LblCompanyUsers.Visible = false;
                LVDeleteUser.Visible = false;

                if (ContextObjectHelper.CurrentUser != null && ContextObjectHelper.CurrentUser.SysUserObjInContextOrNull != null)
                {
                    companyId = ContextObjectHelper.CurrentUser.SysUserObjInContextOrNull.CompanyId;
                }

                if (companyId != null)
                {
                    IList<UserObj> list = QueryMgrDynamicHql.Instance.GetUsersForCompany(companyId, ContextObjectHelper.CurrentUser.UserName);

                    if (list.Count == 0)
                    {
                        LblCompanyUsers.Visible = true;
                        LblCompanyUsers.Text = Utl.ContentHlp("Company.OnlyLoggedInUserIsUser");
                    }
                    else
                    {
                        LVDeleteUser.Visible = true;
                        LVDeleteUser.DataSource = list;
                        LVDeleteUser.DataBind();
                    }
                }
                #endregion // Update content

                CCDeleteUser.Visible = true;
            }

            if (editCompany)
            {
                #region Update Content
                #endregion // Update content

                CCCompanyData.Visible = true;
                CCUserData.Visible = true;
            }
        }

        private void UpdateButtonsView()
        {
            ABNewUser.Visible = false;
            ABUpdateCompany.Visible = false;

            if (newUser)
            {
                ABNewUser.Visible = true;
            }

            if (editCompany)
            {
                ABUpdateCompany.Visible = true;
            }

        }

        #region Keypress
        protected void ABUpdateCompany_Click(object sender, EventArgs e)
        {
            bool success = false;

            UserObjCtrl.PopulateBack();
            CompanyInfoCtrl.PopulateBack();

            try
            {
                // Create the user and company
                success = CvmFacade.NewCompany.UpdateCompanyAndUser(user, company, ATBPassword.Text);

                if (success)
                {
                    Utl.Msg.PostMessage("Company.CompanyAndUserUpdated", company.Name, user.FullName);
                }
            }
            catch (Exception)
            {
                throw;
            }

            UpdateControlView();

            UpdateButtonsView();
        }

        protected void ABCreateJobAnnouncement(object sender, EventArgs e)
        {
            JobForm.PopulateBack();

            SaveAndUpdateAnnouncement();

            // Hide job description control 
            hasJobIdParm = false;
            isModeNew = false;

            UpdateControlView();
        }

        protected void ABSaveAndPreview_Click(object sender, EventArgs e)
        {
            JobForm.PopulateBack();

            SaveAndUpdateAnnouncement();

            // Hide job description control 
            hasJobIdParm = false;
            isModeNew = false;

            UpdateControlView();

            Response.Redirect("~/Public/viewjob.aspx?jobId=" + req.Current.AnnouncementId);
        }

        protected void ABNewUser_Click(object sender, EventArgs e)
        {
            bool success = false;

            NewUserObjCtrl.PopulateBack();

            try
            {
                // User has to have ID hence we add it before we commit the new user
                newUserObj.GenerateIdentifier();

                // Create the user and company
                if (CvmFacade.UserAdmin.ValidateUser(newUserObj.Email, ATBNewUserPassword.Text, ATBReEnterNewUserPassword.Text))
                    success = CvmFacade.NewCompany.CreateNewCompanyUser(newUserObj, ContextObjectHelper.CurrentSysUserObjOrNull.RelatedCompanyObj, ATBNewUserPassword.Text);

                if (success)
                {
                    Utl.Msg.PostMessage("Company.CompanyNewUserCreated", newUserObj.FullName);
                }
            }
            catch (Exception ex)
            {
                Utl.Msg.PostMessage(ex.Message);
            }

            UpdateControlView();

            UpdateButtonsView();
        }

        protected void LBDeleteUser_Click(object sender, EventArgs e)
        {
            // Delete User
            CvmFacade.UserAdmin.DeleteUser((long)Convert.ToDouble(((LinkButton)(sender)).CommandArgument), ((LinkButton)(sender)).CommandName);

            // Show the delete view again
            deleteUser = true;

            UpdateControlView();

            UpdateButtonsView();
        }

        protected void LB_Click(object sender, EventArgs e)
        {
            switch (((Control)(sender)).ID)
            {
                case "LBNewUser":
                    newUser = true;
                    break;

                case "LBDeleteUser":
                    deleteUser = true;
                    break;

                case "LBEdit":
                    editCompany = true;
                    break;
            }

            // Hide job description control 
            hasJobIdParm = false;
            isModeNew = false;

            UpdateControlView();

            UpdateButtonsView();
        }
        #endregion

        protected string TableLookupValue
        {
            get
            {
                return ContextObjectHelper.CurrentSysUserObjOrNull.CompanyId.ToString();
            }
        }

        private void SaveAndUpdateAnnouncement()
        {
            // Add the right company ID
            this.req.Current.RelatedCompanyObj = ContextObjectHelper.CurrentSysUserObjOrNull.RelatedCompanyObj;

            HibernateMgr.Current.SaveOrUpdate(this.req.Current);
            HibernateMgr.Current.Session.Flush();
        }
    }
}