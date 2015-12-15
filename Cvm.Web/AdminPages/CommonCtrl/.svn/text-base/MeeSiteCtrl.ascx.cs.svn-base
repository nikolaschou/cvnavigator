using System;
using System.Collections.Generic;
using System.Web.Security;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Localization;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using CVnavNewsLetterWrapper;
using Napp.Backend.Hibernate;
using Napp.Web.Auto;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class MeeSiteCtrl : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl, IControlWithSave
    {
        private ObjectSource objectSource;
        private bool didInit = false;
        private MailChimp cm = new MailChimp();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<SiteLanguage> langs = QueryMgr.instance.GetAllSiteLanguage().ListOrNull(HibernateMgr.Current.Session);
                this.ddlUserLanguage.DataSource = langs;
                this.ddlUserLanguage.DataTextField = "SiteLanguageCode";
                this.ddlUserLanguage.DataValueField = "SiteLanguageId";
                this.ddlUserLanguage.DataBind();

                if (ContextObjectHelper.CurrentUser.SiteLanguageId == null)
                {
                    bool found = false;

                    if (ContextObjectHelper.CurrentSysOwnerOrNull != null)
                        ContextObjectHelper.CurrentUser.SiteLanguageId = ContextObjectHelper.CurrentSysOwnerOrNull.SiteLanguageId;
                    else
                    {
                        foreach (SiteLanguage sl in langs)
                        {
                            if (sl.SiteLanguageCode.ToLower() == SiteLanguageConst.DefaultLanguage.ToLower())
                            {
                                ContextObjectHelper.CurrentUser.SiteLanguageId = sl.SiteLanguageId;
                                found = true;
                                break;
                            }
                        }

                        if (found == false)
                        {
                            // Ups not good, we set sitelanguage to 2 (which should be english)
                            ContextObjectHelper.CurrentUser.SiteLanguageId = 2;
                        }
                    }
                }

                // Select the right index in the dropdown list
                for (int i = 0; i <= ddlUserLanguage.Items.Count - 1; i++)
                {
                    if (ContextObjectHelper.CurrentUser.SiteLanguageId.ToString() == ddlUserLanguage.Items[i].Value)
                    {
                        ddlUserLanguage.SelectedIndex = i;
                    }
                }
            } 
        }

        public void PopulateFront()
        {
            if (!string.IsNullOrEmpty(MyResource.Email))
                NewsLetterCheckbox.Checked = cm.UserSubscribed(MyResource.Email);
        }

        public void PopulateBack()
        {
        }

        private Resource MyResource
        {
            get
            {
                return (Resource)this.objectSource();
            }
        }

        public ObjectSource ObjectSource
        {
            get
            {
                return objectSource;
            }
            set
            {
                this.objectSource = value;
            }
        }

        public void BuildForm()
        {
            if (!this.didInit)
            {
                this.didInit = true;
            }
        }

        public void OnSave()
        {
            PopulateBack();

            this.BuildForm();
            this.PopulateFront();
        }

        protected void NewsLetterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LblPopup.Text = string.Empty;

            if (NewsLetterCheckbox.Checked == true)
            {
                if (!string.IsNullOrEmpty(MyResource.Email))
                {
                    if (cm.SubscribeUser(MyResource.Email, MyResource.FirstName, MyResource.LastName) == false)
                    {
                        LblPopup.Text = Utl.Content("EditCv.CurrentlyNotPossibleDueToAnError");
                        NewsLetterCheckbox.Checked = false;
                    }
                    else
                        LblPopup.Text = Utl.Content("EditCv.ReceiveNewsletterAccept");
                }
                else
                {
                    LblPopup.Text = Utl.Content("EditCv.NotPossibleToSubscribeDueToMissingEmail");
                    NewsLetterCheckbox.Checked = false;
                }

                MPENewsletter.Show();
            }
            else
            {
                if (!string.IsNullOrEmpty(MyResource.Email))
                {
                    if (cm.UnSubscribeUser(MyResource.Email) == false)
                    {
                        LblPopup.Text = Utl.Content("EditCv.CurrentlyNotPossibleDueToAnError");
                        NewsLetterCheckbox.Checked = true;
                    }
                    else
                        LblPopup.Text = Utl.Content("EditCv.ReceiveNewsletterReject");
                }
                else
                {
                    LblPopup.Text = Utl.Content("EditCv.NotPossibleToUnsubscribeDueToMissingEmail");
                    NewsLetterCheckbox.Checked = true;
                }

                MPENewsletter.Show();
            }
        }

        protected void ddlUserLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContextObjectHelper.CurrentUser.SiteLanguageId = (long)Convert.ToDouble(ddlUserLanguage.SelectedValue);

            Response.Redirect(Request.RawUrl);
        }

        protected void btnOkDelete_Click(object sender, EventArgs e)
        {
            // Delete account and redirect to login page
            CvmFacade.UserAdmin.DeleteUser(MyResource.RelatedUserObjObj);

            // Sign out
            FormsAuthentication.SignOut();

            // Redirect to login page
            Response.Redirect("~/Public/Login.aspx");
        }
    }
}