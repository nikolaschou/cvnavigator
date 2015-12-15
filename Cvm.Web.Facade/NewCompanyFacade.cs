using System.Linq;
using System.Web.Security;
using Cvm.Backend.Business.Companies;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Users;
using Cvm.Backend.CvImport;
using log4net;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;

namespace Cvm.Web.Facade
{        
    public class NewCompanyFacade
    {
        private static readonly object mutex = new object();
        private static ILog log = LogManager.GetLogger(typeof(NewCompanyFacade));
        private SysUserObj sysUserObj = new SysUserObj();
        private readonly CvImporterMgr _importMgr = CvImporterMgr.Instance;

        internal NewCompanyFacade()
        {            
        }

        public bool CreateCompanyAndUser(UserObj user, Company company, string password)
        {
            user.UserName = user.Email;

            // First create the company, so we get a companyId to add to the sys user
            HibernateMgr.Current.Save(company);

            // Add a new user and sysuser to the system
            HibernateMgr.Current.Save(user);
            CvmFacade.UserAdmin.CreateNewMemberhipUserOrFail(user, password);

            sysUserObj.RelatedUserObjObj = user;
            sysUserObj.RelatedCompanyObj = company;
            sysUserObj.RoleIdEnum = SysRoleEnum.CompanyHR;

            HibernateMgr.Current.Save(sysUserObj);

            ContextObjectHelper.PerformLogin(user);
            return true;
        }

        public bool UpdateCompanyAndUser(UserObj user, Company company, string password)
        {
            string oldPassword = Membership.Provider.GetPassword(user.UserName, "");
            user.UserName = user.Email;

            // Update the company
            HibernateMgr.Current.SaveOrUpdate(company);

            // Update a user for the system
            HibernateMgr.Current.SaveOrUpdate(user);

            if (oldPassword != password)
                Membership.Provider.ChangePassword(user.UserName, oldPassword, password);

            return true;
        }

        public bool CreateNewCompanyUser(UserObj user, Company company, string password)
        {
            // Add a new user and sysuser to the system
            user.UserName = user.Email;

            HibernateMgr.Current.Save(user);
            CvmFacade.UserAdmin.CreateNewMemberhipUserOrFail(user, password);

            sysUserObj = new SysUserObj();
            sysUserObj.RelatedUserObjObj = user;
            sysUserObj.RelatedCompanyObj = company;
            sysUserObj.RoleIdEnum = SysRoleEnum.CompanyHR;

            HibernateMgr.Current.Save(sysUserObj);

            //Now invite user
            CvmFacade.UserAdmin.InviteUser(user);


            return true;
        }
        
        public void RollbackCompanyAndUserCreation(UserObj user, Company company)
        {
            UserObj u = QueryMgr.instance.GetUserObjByUserNameOrNull(user.UserName);
            SysUserObj sysU = u.SysUserObjs.FirstOrDefault(s => s.SysId == ContextObjectHelper.CurrentSysRoot.SysId);

            if (sysU != null)
            {
                HibernateMgr.Current.Delete(sysU);
            }

            bool hasOtherSysUsers = u.SysUserObjs.Where(s => s.SysId != ContextObjectHelper.CurrentSysRoot.SysId).Count() > 0;

            if (!hasOtherSysUsers)
            {
                HibernateMgr.Current.Delete(u);
            }
            else
            {
                MessageManager.Current.PostMessage("CompanyFacade.UserStillHasRoles", u.UserName);
            }

            HibernateMgr.Current.Delete(company);
        }
    }
}
