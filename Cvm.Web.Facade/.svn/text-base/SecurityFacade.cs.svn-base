using System;
using System.Security;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Cvm.Web.Navigation;
using log4net;
using Napp.Backend.BusinessObject;
using Napp.Common.MessageManager;
using Napp.Web.Navigation;

namespace Cvm.Web.Facade
{
    public class SecurityFacade
    {
        private static ILog log = LogManager.GetLogger(typeof(SecurityFacade));
        public void VerifyAccess(IBusinessObject obj)
        {
            VerifyAccess(obj, AccessMode.readwrite);
        }

        public void VerifyAccess(IBusinessObject obj, AccessMode accessMode)
        {
            if (obj is Resource)
            {
                VerifyOrFail(HasAccessToResource(obj as Resource, accessMode), obj, accessMode);
            } 
            else
            {
                throw new NotImplementedException("Access control not implemented for type "+obj.GetType());
            }
        }

        private void VerifyOrFail(bool hasAccessToObject, IBusinessObject obj, AccessMode accessMode)
        {
            if (!hasAccessToObject)
            {
                Fail(obj, accessMode, "");
            }
        }

        /// <summary>
        /// Fails with a security exception.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="accessMode"></param>
        /// <param name="reason"></param>
        private void Fail(IBusinessObject obj, AccessMode accessMode, string reason)
        {
            log.Info("User " + ContextObjectHelper.CurrentUserName + " does not have access " + accessMode + " to " + obj.StandardObjectTitle + " " + obj.IdStr + ". " + reason);
            throw new SecurityException("The user does not have access to the requested action.");
        }

        /// <summary>
        /// Returns true if the current user has access to the given resource.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        private bool HasAccessToResource(Resource resource, AccessMode accessMode)
        {
            if (ContextObjectHelper.IsResourceOwner(resource))
            {
                return true;
            }

            //Global administrators can see all resources
            if (ContextObjectHelper.CurrentUser.IsGlobalAdmin) 
                return true;
            
            SysRoleEnum role = ContextObjectHelper.CurrentSysUserRole;
            
            if ((role & RoleSet.ClientAtLeast) != 0)
            {
                //Verify that the resource is enrolled in the current sysId simply by getting a reference to it
                SysResource sysResource = resource.SysResourceContext;
                
                //SysAdmin can alway see all enrolled resources 
                if (role == SysRoleEnum.SysAdmin) 
                    return true;
            
                //For clients, write-mode should never be allowed
                if (role == SysRoleEnum.Client && accessMode == AccessMode.readwrite) 
                    return false; 

                UserObj user = ContextObjectHelper.CurrentUser;
                SysUserObj sysUser = ContextObjectHelper.CurrentSysUserObjOrFail;
            
                //Check if the filters contains the employee type and the status id
                if ((sysUser.FilterEmployeeTypeIds & sysResource.EmployeeTypeId) != 0) 
                    return true;
                
				if ((sysUser.FilterProfileStatusIds & resource.ProfileStatusId) != 0) 
                    return true;
                
                //For clients, check that the resource is related to an active Assignment to which this client has access
                if (role == SysRoleEnum.Client)
                {
                    if (QueryMgrDynamicHql.Instance.HasClientAssignedResource(user, resource)) 
                        return true;
                }
            }

            return false;
        }

        public void Logout()
        {
            ContextObjectHelper.ClearContext();
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Sets up the context so that the given user is simulated.
        /// Redirecting should be carried out by the web-layer.
        /// </summary>
        /// <param name="myUserName"></param>
        public void SimulateUser(UserObj u)
        {
            CvmFacade.UserAdmin.EnsureUserObjAndMemberhipAligned(u);
            ContextObjectHelper.PreSimulateUser = HttpContext.Current.User;
            ContextObjectHelper.PreSimulateUrl = PageNavigation.GetCurrentLink().IncludeExistingParms();
            ContextObjectHelper.CurrentSysId.Clear();
            ContextObjectHelper.CurrentSysId.InitObject();

            PerformLogin(u.UserName);
            CvmPages.DefaultPage.Redirect();
        }

        /// <summary>
        /// Sets the user on HttpContext.Current.User and writes a cookie
        /// </summary>
        /// <param name="myUserName"></param>
        public void PerformLogin(string myUserName)
        {
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(myUserName, "simulated login"), RoleMgr.Instance.GetRolesForUser(myUserName));
            FormsAuthentication.SetAuthCookie(myUserName, false);
        }

        /// <summary>
        /// Switches back to the original user.
        /// Redirecting should be carried out by the web-layer.
        /// </summary>
        public void StopSimulation()
        {
            if (ContextObjectHelper.PreSimulateUser == null)
            {
                MessageManager.Current.PostMessage("SecurityFacade.SimulationSessionNoLongerValid");
            }
            else
            {
                string userName = ContextObjectHelper.PreSimulateUser.Identity.Name;
                var page = ContextObjectHelper.PreSimulateUrl;
                HttpContext.Current.User = ContextObjectHelper.PreSimulateUser;
                ContextObjectHelper.ClearContext();
                ContextObjectHelper.CurrentSysId.InitObject();
                
                FormsAuthentication.SetAuthCookie(userName, false);
                ContextObjectHelper.PreSimulateUser = null;
                ContextObjectHelper.PreSimulateUrl = null;

                page.Redirect();
            }
        }

        /// <summary>
        /// Determines whether the current user is allowed to delete objects.
        /// The special rule for resources is that if they can access the object, then they can delete it.
        /// They cannot delete the whole CV, though.
        /// </summary>
        /// <param name="objectGetter"></param>
        public void CanDeleteVerify(object obj)
        {
            IBusinessObject bo = obj as IBusinessObject;
           
            if (obj == null) 
                throw new InvalidOperationException("Cannot delete null ") ;
            
            if (bo == null) 
                throw new NotImplementedException("Expected IBusinessObject, found " + obj.GetType());
            
            if (ContextObjectHelper.CurrentUserHasAnyRole(RoleSet.SalesMgrAtLeast))
            {
                //Allowed to delete anything if they have page-access
                return;
            } 
            
            if (bo is Resource && ContextObjectHelper.IsResourceOwner((Resource)bo))
            {
                //Resource cannot delete itself
                Fail(bo, AccessMode.delete, ""); 
            } 
            else if (bo is IHasRelatedResource)
            {
                Resource res = (bo as IHasRelatedResource).RelatedResourceObj;
                VerifyOrFail(HasAccessToResource(res,AccessMode.delete), res, AccessMode.delete);
                return;
            } 
            else
            {
                Fail(bo, AccessMode.delete, "");
            }
            
            //If we haven't returned with true yet, we won't allow it
            Fail(bo, AccessMode.delete, ""); 
        }

        public bool HasPermission(PermissionEnum p)
        {
            return PermissionMgr.Instance.HasPermission(ContextObjectHelper.CurrentSysUserRole, p);
        }

        /// <summary>
        /// Verifies that the current user has access to the given EditCvTab for the given resource.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="index"></param>
        public void VerifyAccessToEditCvTabsSecureTabs(Resource res, EditCvTab index)
        {
            //For now only the GrantedSites tab is protected
            if (index == EditCvTab.GrantedSites) 
            {
                if (!ContextObjectHelper.IsResourceOwner(res))
                {
                    Fail(res, AccessMode.readwrite, "Cannot enter page " + index);
                }
            } 
        }
    }
}