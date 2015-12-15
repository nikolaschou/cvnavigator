using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.Web.AutoForm;
using Napp.Web.Navigation;
using Napp.Web.WebForm;
using NHibernate.Linq;

namespace Cvm.Web.AdminPages
{
    public partial class UserAdmin2 : System.Web.UI.Page
    {

        private int? _sysUserId;
        private MembershipUser _membershipUser;
        private UserObj _sysUser;
        private PageMode? mode;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected override void OnInit(EventArgs e)
        {

            if (MyPageMode==PageMode.New || MyPageMode==PageMode.Update)
            {
                //Populate membership user form (depending on new or update mode)
                if (MyPageMode == PageMode.Update)
                {
                    if (MyMembershipUser == null)
                    {
                        Utl.Msg.PostMessage("UserAdmin.NoUserRecordFoundInUserDatabase", MyUserObj.UserName);
                        this.Button1.Enabled = false;
                        this.Button2.Enabled = false;
                    }
                    else
                    {

                        this.UserNameSuffixLit.Text = "";
                        this.UpdateMembershipUserForm.ObjectSourceInstance = MyMembershipUser;
                        this.UpdateMembershipUserForm.BuildForm();
                        this.UserNameTextBox.Text = MyMembershipUser.UserName;
                        this.UserNameTextBox.Enabled = false;

                        //Populate user form
                        this.UpdateUserObjForm.ObjectSourceInstance = this.MyUserObj;
                        if (!ContextObjectHelper.CurrentUser.IsGlobalAdmin)
                        {
                            this.UpdateUserObjForm.EditMode = AutoFormEditMode.View;
                        }
                        
                        this.UpdateUserObjForm.BuildForm();
                        if (!IsPostBack) this.UpdateUserObjForm.PopulateFront();

                        //Populate sysUser form
                        if (this.MySysUserObj != null)
                        {
                            this.SysUserObjForm.ObjectSourceInstance = this.MySysUserObj;
                            this.SysUserObjForm.BuildForm();
                            if (!IsPostBack) this.SysUserObjForm.PopulateFront();
                        }
                        else
                        {
                            this.SysUserObjForm.Visible = false;
                        }
                    }
                }
                else 
                {
                    this.NewUserForm.ObjectSourceInstance = new UserObjWrap(ContextObjectHelper.CurrentSysRoot.SysCodeObj);
                    this.NewUserForm.BuildForm();
                    this.UserNameSuffixLit.Text = CvmFacade.UserAdmin.GetUserNameSuffix();
                }
            }
            else
            {
                this.MainPanel.Visible = false;
            }
            //Refresh list of users
            this.UserList.DataBind();
            this.CreatenewLink.DataBind();

        }

        private SysUserObj _sysUserObj;
        /// <summary>
        /// Returns the SysUserObj of the chosen userObj assuming:
        /// 1. We are working in a sysId context
        /// 2. The chosen userObj is registered as a user within this sysId.
        /// If one of these conditions are not satisfied null is returned.
        /// </summary>
        protected SysUserObj MySysUserObj
        {
            get
            {
                if (!ContextObjectHelper.CurrentSysIdIsSpecified())
                {
                    //A sysUserObj can never be relevant
                    return null;
                }
                else if (_sysUserObj == null) 
                {
                    if (MyPageMode==PageMode.New)
                    {
                        _sysUserObj = new SysUserObj();
                    } else
                    {
                        //We might find null if the userObj is not registered within the current sysId.
                        _sysUserObj = this.MyUserObj.SysUserObjInContextOrNull;
                    }
                }
                return _sysUserObj;
            }
        }

        /// <summary>
        /// Determines whether the page is in mode New, Update or List.
        /// </summary>
        protected PageMode MyPageMode
        {
            get
            {
                if (this.mode==null)
                {
                    if (Utl.Query.GetMode()==PageMode.New) this.mode = PageMode.New;
                    else if (this.MyUserId != null)
                    {
                        this.mode = PageMode.Update;
                    } else
                    {
                        this.mode = PageMode.List;
                    }

                }
                return (PageMode)this.mode;
            }
        }

        /// <summary>
        /// Returns the sysUser object to be used on the page.
        /// A new SysUser-object is created if necessary.
        /// </summary>
        /// <value></value>
        /// <value></value>
        protected UserObj MyUserObj
        {
            get
            {
                if (this._sysUser == null)
                {
                    if (this.MyUserId != null)
                    {
                        this._sysUser = QueryMgr.instance.GetUserObjByIdOrNull((long)MyUserId);
                        System.Diagnostics.Debug.Assert(_sysUser != null, "User " + _sysUserId + " was not found or you don't have access to that user.");
                    }
                    else
                    {
                        this._sysUser = new UserObj();
                    }
                }
                return this._sysUser;
            }
        }

        public bool IsUpdateMode()
        {
            return MyPageMode==PageMode.Update;
        }


        protected MembershipUser MyMembershipUser
        {
            get
            {
                if (this._membershipUser == null && MyUserObj.UserName!=null)
                {
                    this._membershipUser = Membership.GetUser(MyUserObj.UserName);
                }
                return _membershipUser;
            }
        }

        /// <summary>
        /// Gets the user name of the current request while verifying that it 
        /// is in fact a user of the current sysId.
        /// </summary>
        protected int? MyUserId
        {
            get
            {
                if (_sysUserId == null)
                {
                    _sysUserId = Utl.Query.GetParmIntOrNull(QueryParmCvm.userId);
                    //Validate that this is a user of the current sysId.
                    //By accessing the SysUser property this is implicity verified.
                    if (_sysUserId != null)
                    {
                        UserObj temp = QueryMgr.instance.GetUserObjByIdOrNull((long)_sysUserId);
                        System.Diagnostics.Debug.Assert(temp != null);
                    }
                }
                return _sysUserId;
            }
        }

        protected void OnClickSaveBtn(object sender, EventArgs e)
        {
            String tempUserName;
            if (MyPageMode==PageMode.Update)
            {
                this.UpdateMembershipUserForm.PopulateBack();
                MembershipUser user = (MembershipUser)this.UpdateMembershipUserForm.ObjectSourceInstance;
                Membership.UpdateUser(user);
                this.UpdateUserObjForm.PopulateBack();
                if (this.MySysUserObj!=null) this.SysUserObjForm.PopulateBack();
                tempUserName = user.UserName;
                Utl.Msg.PostMessage("UserAdmin.UserUpdated", tempUserName);
            }
            else if (MyPageMode == PageMode.New)
            {
                this.NewUserForm.PopulateBack();
                UserObjWrap user = (UserObjWrap) this.NewUserForm.ObjectSourceInstance;
                user.PartialUserName = this.UserNameTextBox.Text;
                UserObj userObj=CvmFacade.UserAdmin.ValidateAndCreateUser(user);
                if (userObj!=null)
                {
                    tempUserName = user.FullUserName;
                    Utl.Msg.PostMessage("UserAdmin.UserCreated", tempUserName);

                    //Redirect to update mode
                    PageLink current = PageNavigation.GetCurrentLink();
                    current.SetParm(QueryParmCvm.userId, userObj.UserId);
                    current.Redirect();
                }
            }
            else
            {
                throw new InvalidProgramException("Coding exception. This should never happen.");
            }

        }

        protected void OnClickCancelBtn(object sender, EventArgs e)
        {
            Utl.Msg.PostMessage("UserAdmin.NothingSaved");
            PageNavigation.GetCurrentLink().Redirect();
        }

        protected void OnClickDeleteUserLink(object sender, EventArgs e)
        {
            CvmFacade.UserAdmin.DeleteUser(MyUserObj.UserId,MyUserObj.UserName);
            PageNavigation.GetCurrentLink().Redirect();

        }

        protected void OnClickUnlockUserLink(object sender, EventArgs e)
        {
            MyMembershipUser.UnlockUser();
            Utl.Msg.PostMessage("UserAdmin.UserUnlocked", this.MyUserId);
            this.UpdateMembershipUserForm.PopulateFront();
        }

        protected void OnClickSimulateUserLink(object sender, EventArgs e)
        {
            CvmFacade.Security.SimulateUser(MyUserObj);
            Utl.Msg.PostMessage("AdminMasterPage.SimulationStarted", MyUserObj.UserName);
            CvmPages.StartPage.Redirect(); 
        }

        protected void OnClickInviteUserByEmailLink(object sender, EventArgs e)
        {
            CvmFacade.UserAdmin.InviteUser(MyUserObj);
        }

        protected void OnClickSetPasswordLink(object sender, EventArgs e)
        {
            this.AdmDialog1.OpenDialog();
        }

        protected void OnClickSetPasswordBtn(object sender, EventArgs e)
        {
            if (this.Password1TextBox.Text.Length==0 || this.Password2TextBox.Text.Length==0)
            {
                Utl.Msg.PostMessage("UserAdmin.EnterPassword");
                return;
            }
            if (!this.Password1TextBox.Text.Equals(this.Password2TextBox.Text))
            {
                Utl.Msg.PostMessage("UserAdmin.PasswordsMustMatch");
                return;
            }
            var regex = new Regex(Membership.PasswordStrengthRegularExpression);
            if (!regex.IsMatch(Password1TextBox.Text))
            {
                Utl.Msg.PostMessage("UserAdmin.PasswordTooWeak");
                return;
            }
            String oldpw=MyMembershipUser.GetPassword();
            try
            {
                MyMembershipUser.ChangePassword(oldpw, Password1TextBox.Text);
                this.AdmDialog1.CloseDialog();
            } catch(ArgumentException e2)
            {
                Utl.Msg.PostMessage("Standard.TechnicalMessage",e2.Message);
            }
            
        }

        protected IList<SysUserObj> GetUsersForList()
        {
            IList<SysUserObj> result = QueryMgrDynamicHql.Instance.GetSysUserList();
            return result;
        }


        protected String GetUserNameForList(SysUserObj x)
        {
            return x.RoleIdEnum + " - " + x.FullName;
        }
    }
}
