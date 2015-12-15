using System;
using System.Web;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Import;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using log4net;
using Napp.Backend.Business.Multisite;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Public
{
    public partial class Signup : System.Web.UI.Page
    {
        private static ILog logger = LogManager.GetLogger(typeof (Signup));
        private SysId _sysId;
        protected bool hasJobIdParm;

        protected override void OnPreInit(EventArgs e)
        {
            ContextObjectHelper.EnsureNoSubDomain();
            
            MasterPageHelper.Instance.OnPageInit(false);

            hasJobIdParm = Utl.Query.HasParm(QueryParmCvm.jobId);
        }

        protected SysId GetSysId()
        {
            if (_sysId == null) 
                _sysId = ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull();
            return _sysId;
        }

        protected SysRoot GetSysRoot()
        {
            return QueryMgr.instance.GetSysRootById(GetSysId().SysIdInt);
        }

        protected override void OnLoad(EventArgs e)
        {
            MasterPageHelper.Instance.AvoidAutoComplete = true;
            this.ImportLinkedInCtrl.AutoPostBackOnImport = false;
            SysCode? sysCode = ContextObjectHelper.FindExplicitSysCodeForCurrentRequest();
            
            if (!sysCode.HasValue)
            {
                this.MainPanel.Visible = false;
                this.SubPanel.Visible = true;
                this.SiteRepeater.DataSource = QueryMgrDynamicHql.Instance.GetSysOwners(SysOwnerPreferencesEnum.publicSignup);
                this.SiteRepeater.DataBind();
            } 
            else
            {
                string sysName = ContextObjectHelper.GetSysNameFromExplicitSysCode();
                MasterPageHelper.Instance.PushTitleByContent("Signup.Header", sysName);
            }

            if (!this.Page.IsPostBack)
            {
                this.CvFileUpload.ToolTip = Utl.ContentHlp("Signup.FileUpLoadHelp");
                this.CvFileUploadLabel.ToolTip = Utl.ContentHlp("Signup.FileUpLoadHelp");
                this.LinkedInSignUpLabel.ToolTip = Utl.ContentHlp("Signup.LinkedInSignUpHelp");
                SetCaptchaText();
            }
        }

        protected void OnClickSubmitBtn(object sender, EventArgs e)
        {
            if (Session["Captcha"].ToString() != AdmCaptchaTextBox.Text.Trim())
            {
                Utl.Msg.PostMessage("Signup.CaptchaValueWrong");
                SetCaptchaText();
                AdmCaptchaTextBox.Text = "";
            }
            else
            {
                DoSignup();
            }
        }

        private void DoSignup()
        {
            try
            {
                Resource res = CvmFacade.Signup.Signup(this.FirstNameTB.Text, this.LastNameTB.Text, this.EmailTB.Text,
                                                       this.PasswordTB.Text, this.CvFileUpload.FileName, this.CvFileUpload.SaveAs, this.CvFileUpload.HasFile, this.ImportLinkedInCtrl.LinkedInText);
                ContextObjectHelper.PerformLogin(res.RelatedUserObjObj);
                HibernateMgr.Current.CommitAndReopenTransaction();

                CvmFacade.UserAdmin.InviteUser(res.RelatedUserObjObj);
                
                if (res.HasImportedLinkedInData())
                {
                    LinkedInImportMgr.Create(res).ImportAllNew();
                    CvmPages.PrintCvPageLink(res.ResourceId).Redirect();                    
                } 
                else
                {
                    EditCvTab tab = (res.HasImportData() ? EditCvTab.Import : EditCvTab.BaseData);

                    if (hasJobIdParm)
                        CvmPages.EditCvLink(res.ResourceId, tab, Utl.Query.GetParmOrDefault(QueryParmCvm.jobId, 0) ).Redirect();
                    else
                        CvmPages.EditCvLink(res.ResourceId, tab).Redirect();
                }
            } 
            catch(UserNotCreatedException )
            {
                Utl.Msg.PostMessage("Signup.UserNotCreated");
            }
        }

        protected void OnClickSendToFriendBtn(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.SendToFriendTextBox.Text))
            {
                Utl.Msg.PostMessage("SignUp.EnterSendToFriendEmail");
            } 
            else
            {
                CvmFacade.Mail.SignupSendToFriend(this.SendToFriendTextBox.Text, this.SendToFriendMessageTB.Text, HttpContext.Current.Request.Url.AbsoluteUri);
            }
        }

        private void SetCaptchaText()
        {
            Random oRandom = new Random();
            int iNumber = oRandom.Next(100000, 999999);
            Session["Captcha"] = iNumber.ToString();
        }

        protected void AHLSignUpNoRelations_Click(object sender, EventArgs e)
        {
            if (hasJobIdParm)
                CvmPages.SignUp(SysCode.www.Value, Utl.Query.GetParmOrDefault(QueryParmCvm.jobId, 0).ToString() ).Redirect();
            else
                CvmPages.SignUp(SysCode.www.Value).Redirect(); ;
        }
    }
}