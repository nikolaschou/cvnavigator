using System;
using System.Web.Security;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Business.Multisite;
using Napp.Backend.Hibernate;
using Napp.Web.Session;
using Nshop.Backend.Business.GenericDelegates;

namespace Cvm.Web.Public
{
    public partial class NewSite : System.Web.UI.Page
    {
        private const string ADMIN_USER_NAME = "admin";
        protected SimpleRequestObject<SysRoot> SysRoot = new SimpleRequestObject<SysRoot>();
        protected SimpleRequestObject<SysOwner> SysOwner = new SimpleRequestObject<SysOwner>();
        protected SimpleRequestObject<MyUserObj> UserObj = new SimpleRequestObject<MyUserObj>();

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MyUserObj.SysCodeGetter = delegate { return SysRoot.Current.SysCodeObj; };

            AdminPages_AdminMasterPage.IsLoginPage = true;
            this.DataBind();
            this.AutoForm1.BuildForm();
            this.AutoForm2.BuildForm();
            this.AutoForm3.BuildForm();
        }

        protected void OnClickOkBtn(object sender, EventArgs e)
        {
            bool success = false;
            this.AutoForm1.PopulateBack();
            this.AutoForm2.PopulateBack();
            this.AutoForm3.PopulateBack();

            try
            {
                this.UserObj.Current.PartialUserName = ADMIN_USER_NAME;
                this.SysOwner.Current.SysOwnerContactEmail = this.UserObj.Current.Email;
                this.SysRoot.Current.SysCode = this.SysRoot.Current.SysCode.ToLower();
                
                success = CvmFacade.NewSite.CreateSite(this.SysRoot.Current, this.SysOwner.Current, this.UserObj.Current);
                
                if (success)
                {
                    HibernateMgr.Current.CommitAndReopenTransaction();
                    //Not supported on the new web-platform
                    //CvmFacade.NewSite.ImportDataBlueVersion(this.SysRoot.Current.SysId);
                    FormsAuthentication.SetAuthCookie(this.UserObj.Current.FullUserName, false);

                    Utl.Msg.PostMessage("CvmWebFacade.SiteCreated", ContextObjectHelper.CurrentSysRoot.SysId, SysRoot.Current.SysName, UserObj.Current.FullUserName);
                }
            }
            catch (Exception)
            {
                CvmFacade.NewSite.RollbackSiteCreation(this.SysRoot.Current, this.SysOwner.Current, this.UserObj.Current);
                throw;
            }
            
            if (success) 
                CvmPages.RootPath.Redirect();
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
}