using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Security;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Backend.Business.Util;
using Napp.Backend.Business.Meta;
using Napp.Backend.Business.Multisite;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.Web.ExtControls;

namespace Cvm.Web.Facade
{
    public class UserAdminFacade
    {
        public static readonly Regex EmailRegEx = new Regex(ExtTextBox.EmailRegEx);
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Creates a user from a UserObjWrap.
        /// This is the most generic way of creating a user.
        /// If 
        /// </summary>
        /// <param name="userObj"></param>
        /// <param name="sysRoot"></param>
        internal SysUserObj CreateUserWithMembership(UserObjWrap userObj, SysRoot sysRoot)
        {
            var user = new UserObj();
            user.UserName = userObj.FullUserName;
            HibernateMgr.Current.Save(user);
            user.Email = userObj.Email;
            SysUserObj sysUser = new SysUserObj();
            sysUser.SysId = sysRoot.SysId;
            sysUser.RoleIdEnum = SysRoleEnum.None;
            sysUser.RelatedUserObjObj = user;
            HibernateMgr.Current.Save(sysUser);

            CreateMemberhipUser(userObj);
            return sysUser;
        }

        /// <summary>
        /// Creates a membership user throwing an exception if it fails.
        /// </summary>
        /// <param name="userObj"></param>
        public void CreateMemberhipUser(UserObjWrap userObj)
        {
            MembershipCreateStatus status;
            Membership.CreateUser(userObj.FullUserName, userObj.Password, userObj.Email, null, null, true, out status);
            
            if (status != MembershipCreateStatus.Success) 
                throw new Exception("User could not be created: " + status);
        }
        /// <summary>
        /// Creates a membership user for the given userObj if it does not already exist.
        /// If a membership user already exist for the given userObj, an error will be thrown.
        /// </summary>
        /// <param name="userObj"></param>
        /// <param name="password"></param>
        public void CreateNewMemberhipUserOrFail(UserObj userObj, String password)
        {
            var users = Membership.FindUsersByName(userObj.UserName);
            
            if (users.Count > 0) 
                throw new InvalidProgramException("User " + userObj.UserName + " already exists.");
            
            MembershipCreateStatus status;
            Membership.CreateUser(userObj.Email, password, userObj.Email, null, null, true, out status);
            
            if (status != MembershipCreateStatus.Success) 
                throw new Exception("User " + userObj.UserName + "could not be created: " + status);
        }

        /// <summary>
        /// Creates a new membership user with a generated password.
        /// </summary>
        /// <param name="userObj"></param>
        public void CreateNewMemberhipUserOrFail(UserObj userObj)
        {
            CreateNewMemberhipUserOrFail(userObj, GenerateRandomPassword());
        }
        
        /// <summary>
        /// Verifies that a userObj with userName=email exists, creating it on the fly if not.
        /// If it is already found, firstName and lastName are verified. If they do not match,
        /// they will be corrected.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserObj EnsureUserObj(string firstName, string lastName, string email)
        {
            return UserObjMgr.Instance.CreateUserObjUtil(email, firstName, lastName, false);
        }

        public UserObj CreateNewUserObjOrFail(string firstName, string lastName, string email)
        {
            return UserObjMgr.Instance.CreateUserObjUtil(email, firstName, lastName, true);
        }

        /// <summary>
        /// Validates and creates a new user for the current sysId.
        /// Returns the created UserObj or null if nothing was created.
        /// </summary>
        /// <param name="userObj"></param>
        public UserObj ValidateAndCreateUser(UserObjWrap userObj)
        {
            if (ValidateUser(userObj))
            {
                return CreateUserWithMembership(userObj, ContextObjectHelper.CurrentSysRoot).RelatedUserObjObj;
            }
            else 
                return null;
        }

        public bool ValidateUser(UserObjWrap userObj)
        {
            return ValidateUser(userObj.FullUserName, userObj.Password, userObj.PasswordConfirmed);
        }

        /// <summary>
        /// Validates the user, returning false if something is wrong.
        /// </summary>
        /// <param name="fullUserName"></param>
        /// <param name="password"></param>
        /// <param name="passwordConfirmed"></param>
        /// <returns></returns>
        public bool ValidateUser(string fullUserName, string password, string passwordConfirmed)
        {
            if (!password.Equals(passwordConfirmed))
            {
                MessageManager.Current.PostMessage("CvmWebFacade.PasswordMustMatch");
                return false;
            }
            
            string regex = Membership.PasswordStrengthRegularExpression;
            if (!String.IsNullOrEmpty(regex) && !Regex.Match(regex, password).Success)
            {
                MessageManager.Current.PostMessage("CvmWebFacade.PasswordTooWeak");
                return false;
            }
            
            string userFullName = fullUserName;
            if (Membership.GetUser(userFullName) != null)
            {
                MessageManager.Current.PostMessage("CvmWebFacade.UserAlreadyExists1");
                return false;
            }

            if (QueryMgr.instance.GetUserObjByUserNameOrNull(userFullName) != null)
            {
                MessageManager.Current.PostMessage("CvmWebFacade.UserAlreadyExists2");
                return false;
            }
            
            return true;
        }

        public string MapUserNamePrefix()
        {
            return ContextObjectHelper.GetCurrentSysCodeOrFail() + "-";
        }

        /// <summary>
        /// Sets the role of the given userName.
        /// Returns true if the role was changed.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        public bool UpdateUserRole(string userName, SysRoleEnum role)
        {
            return RoleMgr.Instance.UpdateSysUserRole(userName, role);
        }

        public bool UpdateUserRole(UserObjWrap user, SysRoleEnum role)
        {
            return UpdateUserRole(user.FullUserName, role);
        }

        public void MakeSysAdmin(SysUserObj su)
        {
            su.RoleIdEnum = SysRoleEnum.SysAdmin;
            su.AnonymousPrint = false;
            su.FilterEmployeeTypeIds = BitPatternConst.All;
            su.FilterProfileStatusIds = BitPatternConst.All;
        }

        /// <summary>
        /// Returns the suffix to be appended to the user names of newly created users.
        /// </summary>
        /// <returns></returns>
        public string GetUserNameSuffix()
        {
            return UserObjWrap.GetUserNameSuffix(new SysCode(ContextObjectHelper.GetCurrentSysCodeOrFail()));
        }

        private String GenerateRandomPassword()
        {
            return "" + _random.Next(9) + _random.Next(9) + _random.Next(9) + _random.Next(9) + _random.Next(9) + _random.Next(9) + _random.Next(9) + _random.Next(9);
        }

        public void DeleteUser(UserObj myUser)
        {
            if (myUser.RelatedResource != null)
            {
                DeleteUser(myUser.UserId, myUser.UserName);
            }
        }

        public void DeleteUser(long userIdL, string userName)
        {
            var conn = HibernateMgr.Current.GetDirectSqlConnection();
            try
            {
                conn.Open();
                QueryMgrSql.Instance.MakeNullInTable(conn, Tables.Assignment, Tables.Assignment.ColClientUserId, userIdL);
                
                List<TableObj> objs = new List<TableObj>();
                objs.Add(Tables.AssignmentResource);
                objs.Add(Tables.ExternalLink);
                objs.Add(Tables.Certification);
                objs.Add(Tables.Merit);
                objs.Add(Tables.Education);
                objs.Add(Tables.ProjectSkill);
                objs.Add(Tables.Project);
                objs.Add(Tables.ResourceSkill);
                objs.Add(Tables.LanguageSkill);
                objs.Add(Tables.ResourceImport);
                objs.Add(Tables.SysResource);
                objs.Add(Tables.Resource);
            
                foreach (var t in objs)
                {
                    QueryMgrSql.Instance.DeleteFromTable(conn, t, Tables.Resource.ColResourceId, userIdL);
                }
                QueryMgrSql.Instance.DeleteFromTable(conn, Tables.SysUserObj, Tables.SysUserObj.ColUserId, userIdL);
                QueryMgrSql.Instance.DeleteFromTable(conn, Tables.UserObj, Tables.UserObj.ColUserId, userIdL);


               // Added in order to be able to recreate a deleted sysRoot
                if (userName.Contains("admin."))
                {
                    string usernameWithoutAdmin = userName.Replace("admin.", "");

                    SysRoot bySysCode = QueryMgr.instance.GetSysRootBySysCodeOrNull(usernameWithoutAdmin);

                    if (bySysCode != null)
                    {
                        QueryMgrSql.Instance.DeleteFromTable(conn, Tables.SysOwner, Tables.SysOwner.ColSysId, bySysCode.SysId);
                        QueryMgrSql.Instance.DeleteFromTable(conn, Tables.SysRoot, Tables.SysRoot.ColSysId, bySysCode.SysId);
                    }
                } 

                Membership.DeleteUser(userName);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Makes sure that the user can log in, is ready to be invited etc.
        /// Furthermore it makes sure that the email registered in the aspnet-database
        /// matches that registered in the cvnav-database.
        /// </summary>
        /// <param name="u"></param>
        public void EnsureUserObjAndMemberhipAligned(UserObj u)
        {
            MembershipUser mu = Membership.GetUser(u.UserName);
            if (mu == null)
            {
                mu = Membership.CreateUser(u.UserName, this.GenerateRandomPassword());
                MessageManager.Current.PostMessage("UserAdminFacade.CreatedMembership", mu.UserName);
                mu.Email = u.Email;
                mu.IsApproved = true;
                Membership.UpdateUser(mu);
            }
            else
            {
                if (u.Email == null)
                {
                    MessageManager.Current.PostMessage("UserAdminFacade.ShouldSpecifyEmail");
                }
                else if (mu.Email == null || !mu.Email.Equals(u.Email))
                {
                    //Verify that emails match
                    MessageManager.Current.PostMessage("UserAdminFacade.AdjustedDifferenceInEmail", mu.Email, u.Email);
                    mu.Email = u.Email;
                    Membership.UpdateUser(mu);
                }
            }
        }

        /// <summary>
        /// Invite user to login to the cvnav-system.
        /// </summary>
        /// <param name="u"></param>
        public void InviteUser(UserObj u)
        {
            EnsureUserObjAndMemberhipAligned(u);
            CvmFacade.Mail.SendInvitationMail(u);
        }
    }
}