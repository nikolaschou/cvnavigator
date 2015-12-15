using System;
using System.Linq;
using System.Text.RegularExpressions;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.IdGeneration;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Backend.Business.Util;
using log4net;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;

namespace Cvm.Web.Facade
{
    public class NewSiteFacade
    {
        private static readonly object mutex = new object();
        private static ILog log = LogManager.GetLogger(typeof (NewSiteFacade)); 

        internal NewSiteFacade()
        {            
        }

        public bool CreateSite(SysRoot sysRoot, SysOwner sysOwner, UserObjWrap userObj)
        {
            sysRoot.SysCode = sysRoot.SysCode.ToLower();
            if (sysRoot.SysRootTypeId == BitPatternConst.None) 
                sysRoot.SysRootTypeIdEnum = SysRootTypeEnum.Consulting;
            
            SysRoot bySysName = QueryMgr.instance.GetSysRootBySysNameOrNull(sysRoot.SysName);
            
            if (bySysName != null)
            {
                MessageManager.Current.PostMessage("NewSiteFacade.SystemNameExist", sysRoot.SysName);
                return false;
            }
            
            SysRoot bySysCode = QueryMgr.instance.GetSysRootBySysCodeOrNull(sysRoot.SysCode);
            
            if (bySysCode != null)
            {
                MessageManager.Current.PostMessage("NewSiteFacade.SystemCodeExist", sysRoot.SysCode);
                return false;
            }
            //Only allow alpha-numerics.
            Regex alphanum = new Regex("^[a-z0-9]+$");
            
            if (!alphanum.IsMatch(sysRoot.SysCode))
            {
                MessageManager.Current.PostMessage("NewSiteFacade.SysCodeNotValid", sysRoot.SysCode);
                return false;
            }

            if (!CvmFacade.UserAdmin.ValidateUser(userObj)) 
                return false;
            
            SetupSite(userObj, sysRoot, sysOwner);
            
            return true;
        }

        private void SetupSite(UserObjWrap userObj, SysRoot sysRoot, SysOwner sysOwner)
        {
            HibernateMgr.Current.Save(sysRoot);
            HibernateMgr.Current.CommitAndReopenTransaction();
            //At this point we set the sysId at the session
            ContextObjectHelper.CurrentSysId.OverrideObject(new SysId(sysRoot.SysId));
            sysOwner.SysId = sysRoot.SysId;
            HibernateMgr.Current.Save(sysOwner);
            SysUserObj u = CvmFacade.UserAdmin.CreateUserWithMembership(userObj, sysRoot);
            //Add sysAdmin within sys
            CvmFacade.UserAdmin.MakeSysAdmin(u);
            //Now invite user
            CvmFacade.UserAdmin.InviteUser(u.RelatedUserObjObj);
        }

        public void ImportDataBlueVersion(long sysId)
        {
            CvmFacade.ImportData.ImportDataBlueVersion(sysId);
        }

        public void RollbackSiteCreation(SysRoot current, SysOwner sysOwner, UserObjWrap userObj)
        {
            try
            {
                UserObj user = QueryMgr.instance.GetUserObjByUserNameOrNull(userObj.FullUserName);
                SysUserObj sysU = user.SysUserObjs.FirstOrDefault(s => s.SysId == current.SysId);
                
                if (sysU != null)
                {
                    HibernateMgr.Current.Delete(sysU);
                }
                
                bool hasOtherSysUsers = user.SysUserObjs.Where(s => s.SysId != current.SysId).Count() > 0;
                
                if (!hasOtherSysUsers)
                {
                    HibernateMgr.Current.Delete(user);
                } 
                else
                {
                    MessageManager.Current.PostMessage("NewSiteFacade.UserStillHasRoles", user.UserName);
                }
                
                HibernateMgr.Current.CommitAndReopenTransaction();
                
                var owner = QueryMgr.instance.GetSysOwnerByIdOrNull((short)ContextObjectHelper.CurrentSysIdVal);
                
                if (owner != null) 
                    HibernateMgr.Current.Delete(owner);
                
                HibernateMgr.Current.CommitAndReopenTransaction();
                
                IdfrSequenceMgr.Instance.ClearSequences(current.SysIdObj);
                HibernateMgr.Current.CommitAndReopenTransaction();
                
                var root = QueryMgr.instance.GetSysRootBySysCodeOrNull(current.SysCode);
                
                if (root != null) 
                    HibernateMgr.Current.Delete(root);
                
                HibernateMgr.Current.CommitAndReopenTransaction();

                MessageManager.Current.PostMessage("NewSiteFacade.NoDataWasSaved");
            } 
            catch(Exception e)
            {
                MessageManager.Current.PostMessage("NewSiteFacade.RollbackFailed");
                log.Error("Could not roll back", e);
            }
        }
    }
}