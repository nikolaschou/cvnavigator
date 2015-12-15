using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class UserAdmin : Page
    {
        private SysUserObj _user;
        private readonly QueryStringHelper helper = QueryStringHelper.Instance;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected override void OnInit(EventArgs e)
        {
            if (IsModeEditOrNew())
            {
                SetupFields();

                this.UserObjCtrl.DataBind();
                this.SysUserObjCtrl.DataBind();

                this.UserObjCtrl.ObjectSourceInstance = SysUser.RelatedUserObjObj;
                this.UserObjCtrl.BuildForm();

                this.SysUserObjCtrl.ObjectSourceInstance = SysUser;
                this.SysUserObjCtrl.BuildForm();
            } 
            else
            {
                this.MainPanel.Visible = false;
            }
            
            SetupList();

            this.CreateLink.ContentId = "CustomerContacts.CreateNew" + this.UserRole();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsModeEditOrNew() && !IsPostBack)
            {
                PopulateFront();
            }
            
            MasterPageHelper.Instance.PushTitle("UserAdmin.Edit" + UserRole());
        }

        private void SetupFields()
        {
            Tables.UserObjTb userFields = Tables.UserObj.AddEmail.AddFirstName.AddLastName.AddMobile.AddPhone;
            this.UserObjCtrl.IncludeOnlyProperties = userFields.SelectedColumnsStr();

            Tables.SysUserObjTb sysUserFields = Tables.SysUserObj;
            String sysUserFieldsStr = null;
            
            if (this.UserRole() == SysRoleEnum.Client)
            {
                sysUserFieldsStr = sysUserFields.AddAnonymousPrint.AddContactDepartment.AddContactPosition.AddCustomerId.AddFilterEmployeeTypeIds.AddFilterProfileStatusIds.AddInternalNotes.SelectedColumnsStr();
            }
            else if (this.UserRole() == SysRoleEnum.SysAdmin || this.UserRole() == SysRoleEnum.SalesManager)
            {
                sysUserFieldsStr = sysUserFields.AddFilterEmployeeTypeIds.AddFilterProfileStatusIds.SelectedColumnsStr();
            }

            this.SysUserObjCtrl.IncludeOnlyProperties = sysUserFieldsStr;
        }

        private void PopulateFront()
        {
            bool newMode = helper.GetMode() != PageMode.New;
            
            if (newMode)
            {
                CvmFacade.UserAdmin.EnsureUserObjAndMemberhipAligned(this.SysUser.RelatedUserObjObj);
            }
            
            this.UserNameTxt.Enabled = !newMode;
            this.SysUserObjCtrl.PopulateFront();
            this.UserObjCtrl.PopulateFront();
            this.UserNameTxt.Text = this.SysUser.RelatedUserObjObj.UserName;
        }

        private void SetupList()
        {
            this.rep.Controls.Clear();
            this.rep.DataSource = this.UserList;
            this.rep.DataBind();
        }

        protected SysUserObj SysUser
        {
            get
            {
                if (_user == null)
                {
                    if (IsModeNew())
                    {
                        var u = new UserObj();
                        u.GenerateIdentifier();
                        _user = new SysUserObj();
                        _user.RoleIdEnum = UserRole();
                        _user.RelatedUserObjObj = u;
                    } 
                    else
                    {
                        long? userId = helper.GetParmLongOrNull(QueryParmCvm.userId);
                        
                        if (userId != null)
                        {
                            _user = QueryMgr.instance.GetSysUserObjById((long) userId);
                        }
                    }
                }

                return _user;
            }
        }

        private bool IsModeNew()
        {
            return helper.GetMode() == PageMode.New;
        }

        private SysRoleEnum? _role;
        protected SysRoleEnum UserRole()
        {
            if (_role == null)
            {
                string roleStr = helper.GetParmOrNull(QueryParmCvm.type);
                if (roleStr != null)
                {
                    _role = (SysRoleEnum) Enum.Parse(typeof (SysRoleEnum), roleStr);
                    
                    //Verify that we are editing a user of that role
                    if (this.IsModeEditOrNew() && !this.IsModeNew())
                    {
                        if (this.SysUser.RoleIdEnum != _role)
                        {
                            throw new InvalidProgramException("User role must match selected role.");
                        }
                    }
                } 
                else
                {
                    //If not mode new we can derive the role from the currently chosen user
                    if (IsModeNew())
                    {
                        //Fail!
                        helper.GetParmOrFail(QueryParmCvm.type);
                    } 
                    else
                    {
                        _role = this.SysUser.RoleIdEnum;
                    }
                }
            }
            
            return (SysRoleEnum)_role;
        }

        private bool IsModeEditOrNew()
        {
            return helper.GetMode() == PageMode.New || helper.HasParm(QueryParmCvm.userId);
        }
        
        protected IList<SysUserObj> UserList
        {
            get
            {
                var query = QueryMgrDynamicHql.Instance.GetSysUsersSorted(this.UserRole());
                return query.ToList();
            }
        }

        protected string GetListTitle(SysUserObj sysUserObj)
        {
            Customer c = sysUserObj.RelatedCustomerObj;
            return (c != null ? c.CustomerName + " - " :"") + sysUserObj.FullName;
        }

        protected void OnClickSaveBtn(object sender, EventArgs e)
        {
            this.UserObjCtrl.PopulateBack();
            this.SysUserObjCtrl.PopulateBack();
            this.SysUser.RelatedUserObjObj.UserName = this.UserNameTxt.Text;

            if (!DoValidate(SysUser))
            {
                HibernateMgr.Current.RestartTransaction();
            }
            else
            {
                if (helper.GetMode() == PageMode.New)
                {
                    HibernateMgr.Current.Save(SysUser.RelatedUserObjObj);
                    HibernateMgr.Current.Save(SysUser);
                    PageNavigation.GetCurrentLink().SetParm(QueryParmCvm.type,this.UserRole()).SetParm(QueryParmCvm.userId, SysUser.UserId).Redirect();
                }
                CvmFacade.UserAdmin.EnsureUserObjAndMemberhipAligned(this.SysUser.RelatedUserObjObj);
                SetupList();
                PopulateFront();
            }
        }

        private bool DoValidate(SysUserObj su)
        {
            UserObj u = QueryMgr.instance.GetUserObjByUserNameOrNull(su.RelatedUserObjObj.UserName);
            if (u != null)
            {
                if (su.UserId != u.UserId)
                {
                    Utl.Msg.PostMessage("CustomerContacts.UserNameExists");
                    //User already exist);
                    return false;
                }
            }

            return true;
        }

        protected void OnClickInviteUser(object sender, EventArgs e)
        {
            CvmFacade.UserAdmin.InviteUser(this.SysUser.RelatedUserObjObj);
        }

        protected void OnClickSimulateUser(object sender, EventArgs e)
        {
            CvmFacade.Security.SimulateUser(this.SysUser.RelatedUserObjObj);
        }

        protected void OnClickChangePassword(object sender, EventArgs e)
        {
            this.AdmDialog1.OpenDialog();
        }

        protected void OnClickSetPasswordBtn(object sender, EventArgs e)
        {
            if (this.Password1TextBox.Text.Length == 0 || this.Password2TextBox.Text.Length == 0)
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
            
            String oldpw = MyMembershipUser.GetPassword();
            
            try
            {
                MyMembershipUser.ChangePassword(oldpw, Password1TextBox.Text);
                this.AdmDialog1.CloseDialog();
            }
            catch (ArgumentException e2)
            {
                Utl.Msg.PostMessage("Standard.TechnicalMessage", e2.Message);
            }
        }

        private MembershipUser _membershipUser;
        protected MembershipUser MyMembershipUser
        {
            get
            {
                if (this._membershipUser == null && this.SysUser.RelatedUserObjObj.UserName != null)
                {
                    this._membershipUser = Membership.GetUser(this.SysUser.RelatedUserObjObj.UserName);
                }
             
                return _membershipUser;
            }
        }
    }
}