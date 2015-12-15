using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Cvm.Web.Navigation;
using log4net;
using Napp.Backend.Business.Multisite;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.VeryBasic;
using Napp.Web.AdminContentMgr;
using Napp.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.Facade
{
    /// <summary>
    /// Contains various helper methods to get context information.
    /// </summary>
    public class ContextObjectHelper
    {
        private const string ROLES = "roles";
        private const string _currentRolesToken = "curRoles";
        private static ILog log = LogManager.GetLogger(typeof(ContextObjectHelper));
        /// <summary>
        /// Allows for 11 roles, should be enough for now.
        /// </summary>
        private static int[] bits = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
        /// <summary>
        /// 
        /// </summary>
        public static SessionObjectAdv<SysId> CurrentSysId = new SessionObjectAdv<SysId>("sysId", SysIdGetter, false);

        public static readonly SessionObjectAdv<SiteHistory> History = new SessionObjectAdv<SiteHistory>("history", ()=>new SiteHistory(), true);

        /// <summary>
        /// Contains the original user if a user simulation has been started.
        /// </summary>
        public static IPrincipal PreSimulateUser
        {
            get
            {
                return HttpContext.Current.Session["PreSimulateUser"] as IPrincipal;
            }
            set
            {
                HttpContext.Current.Session["PreSimulateUser"] = value;
            }
        }

        public static PageLink PreSimulateUrl
        {
            get
            {
                return HttpContext.Current.Session["PreSimulateUrl"] as PageLink;
            }
            set
            {
                HttpContext.Current.Session["PreSimulateUrl"] = value;
            }
        }

        /// <summary>
        /// Finds the sysId for the current user.
        /// If no sysId is found, null is returned. In this case 
        /// the user can only access his/her own profiles.
        /// If more than one sysId is found, the first is used for now.
        /// If multiple sysId's should be supported, changes should be made here.
        /// </summary>
        /// <returns></returns>
        private static SysId SysIdGetter()
        {
            if (HttpContext.Current == null) 
                return new SysId(3);
            else
            {
                SysId sysId = null;
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    string name = HttpContext.Current.User.Identity.Name;
                    int[] sysIdArr = QueryMgrDynamicHql.Instance.SelectSysIdByUserName(name);
                    
                    if (sysIdArr.Length == 0)
                    {
                        sysId = SysId.Undefined;
                    }
                    else
                    {
                        //Take the first entry, we don't yet support multiple sysIds for a the same user
                        sysId = new SysId(sysIdArr[0]);
                    }
                }
                
                return sysId;
            }
        }

        /// <summary>
        /// Makes sure no subdomain is part of the URL of the current request. 
        /// In case it is, the user is redirected using the 
        /// </summary>
        public static void EnsureNoSubDomain()
        {
            if (WebConfigMgr.ForceIntoDomainAtLogin)
            {
                String host = HttpContext.Current.Request.Url.Host;
                String subdomain = ContextObjectHelperUtil.GetPrefix(host);
                
                if (subdomain != null)
                {
                    String newHost = host.Replace(subdomain + ".", "");
                    String newUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(host, newHost);
                    bool hasSysCode = SysCode.IsGlobal(subdomain) || QueryMgr.instance.GetSysRootBySysCodeOrNull(subdomain) != null;
                    
                    if (hasSysCode)
                    {
                        String query= "" + QueryParmCvm.sysCode + "=" + subdomain;
                        if (newUrl.Contains("?"))
                        {
                            if (newUrl.EndsWith("&")) 
                                newUrl += query;
                            else 
                                newUrl += "&" + query;
                        } 
                        else
                        {
                            newUrl += "?" + query;
                        }
                    }
                
                    HttpContext.Current.Response.Redirect(newUrl);
                }
            }
        }

        /// <summary>
        /// Finds the sysId for the current request using the sysCode as determined by 
        /// FindSysCodeForCurrentRequestOrNull.
        /// If no sysCode is found or the global SysCode.www value is found, then null is returned.
        /// </summary>
        /// <returns></returns>
        public static SysId FindExplicitSysIdForCurrentRequestOrNull()
        {
            SysCode sysCode = FindExplicitSysCodeForCurrentRequestOrGlobal();
            
            if (sysCode.IsGlobal())
            {
                return null;
            }
            else
            {
                return QueryMgr.instance.GetSysRootBySysCodeOrNull(sysCode.Value).SysIdObj;
            }
        }

        /// <summary>
        /// Returns the host for the current request.
        /// Either taken from a request parameter called host or from the actual host of the current URL.
        /// </summary>
        /// <returns></returns>
        private static string GetHost()
        {
            string host = HttpContext.Current.Request["host"];
            
            if (string.IsNullOrEmpty(host))
            {
                host = HttpContext.Current.Request.Url.Host;
            }
            
            return host;
        }

        /// <summary>
        /// Finds the sysCode for the current request following these rules:
        /// 1. If request parameter sysCode is specified, this is returned
        /// 2. If a request parameter host is specified on the form xxx.yyy.zzz, the first part xxx is taken
        /// 3. If the current URL has a host part on the form xxx.yyy.zzz, then xxx is taken
        /// If the sysCode is determined to be www or WWW from either of the above methods, then SysCode.www is returned.
        /// If no sysCode is found then SysCode.www is also returned.
        /// </summary>
        /// <returns></returns>
        public static SysCode FindExplicitSysCodeForCurrentRequestOrGlobal()
        {
            SysCode? sysCode = FindExplicitSysCodeForCurrentRequest();
            
            if (sysCode == null) 
                return SysCode.www;
            else 
                return (SysCode) sysCode;
        }

        /// <summary>
        /// See FindExplicitSysCodeForCurrentRequestStr()
        /// </summary>
        /// <returns></returns>
        public static SysCode? FindExplicitSysCodeForCurrentRequest()
        {
            string sysCode = FindExplicitSysCodeForCurrentRequestStr();

            return SysCode.CreateOrNull(sysCode);
        }

        /// <summary>
        /// Returns the sysCode as specified by either the host prefix or the sysCode request parameter.
        /// </summary>
        /// <returns></returns>
        private static string FindExplicitSysCodeForCurrentRequestStr()
        {
            string sysCode = HttpContext.Current.Request["sysCode"];
            
            if (String.IsNullOrEmpty(sysCode))
            {
                string host = GetHost();
                sysCode = ContextObjectHelperUtil.GetPrefix(host);
            }
            
            return sysCode;
        }

        private static SysCode? _sysCode
        {
            get 
            { 
                return HttpContext.Current.Items["_sysCode"] as SysCode?; 
            }
            set 
            { 
                HttpContext.Current.Items["_sysCode"] = value; 
            }
        }

        /// <summary>
        /// Returns the SysRoot of the currently logged in user.
        /// </summary>
        /// <value></value>
        public static SysRoot CurrentSysRoot
        {
            get
            {
                if (CurrentSysIdVal != null)
                {
                    return QueryMgr.instance.GetSysRootByIdOrNull((short)CurrentSysIdVal);
                }
                else
                {
                    return null;
                }
            }
        }

        public static SysOwner CurrentSysOwnerOrNull
        {
            get
            {
                if (CurrentSysIdVal == null) 
                    return null;
                
                return QueryMgr.instance.GetSysOwnerByIdOrNull((short)CurrentSysIdVal);
            }
        }
        
        public static SysOwner CurrentSysOwnerOrFail
        {
            get
            {
                SysOwner owner = CurrentSysOwnerOrNull;
                
                if (owner == null) 
                    throw new InvalidProgramException("Expected a sys owner to be defined at this point.");
                
                return owner;
            }
        }

        /// <summary>
        /// Get and set a context object.
        /// </summary>
        public static IBusinessObject CurrentBusinessObject
        {
            get
            {
                return HttpContext.Current.Items["cntxObj"] as IBusinessObject;
            }
            set
            {
                HttpContext.Current.Items["cntxObj"] = value;
            }
        }

        public static long? CurrentSysIdVal
        {
            get
            {
                try
                {
                    var sysId = CurrentSysId.GetObject();
        
                    if (sysId != null) 
                        return sysId.SysIdInt;
                    else 
                        return null;
                }
                catch (SysIdNotDefinedException)
                {
                    //Ignore
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the currently chosen SysId or fails if none is found.
        /// </summary>
        public static long CurrentSysIdValOrFail
        {
            get
            {
                var sysId = CurrentSysId.GetObject();
                
                if (sysId != null) 
                    return sysId.SysIdInt;
                else 
                    throw new SysIdNotDefinedException("SysId undefined at this point. Probably a programming error.");
            }
        }

        /// <summary>
        /// Returns the current user name or null if none is specified.
        /// </summary>
        public static String CurrentUserName
        {
            get
            {
                if (HttpContext.Current.User != null) 
                    return HttpContext.Current.User.Identity.Name;
                else 
                    return null;
            }
        }
        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        public static UserObj CurrentUser
        {
            get 
            {
                return QueryMgr.instance.GetUserObjByUserNameOrNullCached(CurrentUserName); 
            }
        }

        public static SysUserObj CurrentSysUserObjOrFail
        {
            get
            {
                //Should be changed to a more sensible implementation supporting
                //multiple roles per user.
                return CurrentUser.SysUserObjInContextOrFail;
            }
        }
        
        public static SysUserObj CurrentSysUserObjOrNull
        {
            get
            {
                //Should be changed to a more sensible implementation supporting
                //multiple roles per user.
                if (CurrentUser != null)
                    return CurrentUser.SysUserObjInContextOrNull;
                else
                    return null;
            }
        }
        
        /// <summary>
        /// Returns the role of the current user.
        /// </summary>
        public static SysRoleEnum CurrentSysUserRole
        {
            get
            {
                if (HttpContext.Current.Items[ROLES] == null)
                {
                    SysRoleEnum r = GetSysRolesUtil();
                    HttpContext.Current.Items[ROLES] = r;
                }
            
                return (SysRoleEnum)HttpContext.Current.Items[ROLES];
            }
        }

        public static String AllCurrentRolesStr
        {
            get 
            { 
                return AllCurrentRoles.ConcatToString(); 
            }
        }
        
        /// <summary>
        /// Gets all roles for the current user.
        /// If the user has selected a current sysId, sysRoles will be added to global roles.
        /// </summary>
        private static string[] AllCurrentRoles
        {
            get
            {
                if (HttpContext.Current.Items[_currentRolesToken] == null)
                {
                    HttpContext.Current.Items[_currentRolesToken] = RoleMgr.Instance.GetRolesForUser(ContextObjectHelper.CurrentUserName);
                }
                
                return (string[])HttpContext.Current.Items[_currentRolesToken];
            }
        }

        public static SysOwner CurrentSysOwnerOrCreate
        {
            get
            {
                SysOwner owner = CurrentSysOwnerOrNull;
                
                if (owner == null)
                {
                    owner = new SysOwner();
                    owner.SysOwnerContactEmail = "xxx@yy.dk";
                    owner.SysId = CurrentSysIdValOrFail;
                    HibernateMgr.Current.Save(owner);
                }
                
                return owner;
            }
        }

        public static SysId CurrentSysIdObj
        {
            get 
            { 
                return CurrentSysRoot.SysIdObj; 
            }
        }

        public static string CurrentSysRootNameOrNull
        {
            get
            {
                if (CurrentSysRoot != null) 
                    return CurrentSysRoot.SysName;
                else 
                    return null;
            }
        }

        private static SysRoleEnum GetSysRolesUtil()
        {
            if (CurrentUser != null && CurrentUser.SysUserObjInContextOrNull != null)
            {
                return CurrentUser.SysUserObjInContextOrFail.RoleIdEnum;
            }
            else 
                return SysRoleEnum.None;
        }

        public static long GetCurrentSysIdValOrFail()
        {
            return CurrentSysIdValOrFail;
        }

        /// <summary>
        /// Returns the current SysId-value or the Undefined value (SysId.Undefined) if none is found.
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentSysIdValOrUndefined()
        {
            long? sysId = CurrentSysIdVal;
            return (sysId != null ? (long)sysId : SysId.Undefined.SysIdInt);
        }

        /// <summary>
        /// Returns the short code for the current sysRoot.
        /// If none is found an exceptions is thrown.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentSysCodeOrFail()
        {
            return CurrentSysRoot.SysCode;
        }

        /// <summary>
        /// Determines whether this session is currently simulating a user.
        /// </summary>
        /// <returns></returns>
        public static bool IsSimulatingUser()
        {
            return PreSimulateUser != null;
        }

        /// <summary>
        /// Clears all context objects.
        /// Should be called when logging out or simulating users.
        /// </summary>
        public static void ClearContext()
        {
            CurrentBusinessObject = null;
            CurrentSysId.Clear();
        }

        /// <summary>
        /// Determines whether the current user has any of the roles contained in the list of roles supplied.
        /// </summary>
        /// <param name="roles">A list of roles made up of bit pattern flags.</param>
        /// <returns></returns>
        public static bool CurrentUserHasAnyRole(SysRoleEnum roles)
        {
            return (roles & CurrentSysUserRole) != 0;
        }

        /// <summary>
        /// Determines whether the current user has the given role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool CurrentUserHasSysRole(string role)
        {
            return CurrentUserHasAnyRole((SysRoleEnum)Enum.Parse(typeof(SysRoleEnum), role));
        }

        public static bool CurrentUserHasSysRole(SysRoleEnum role)
        {
            return CurrentUserHasAnyRole(role);
        }

        /// <summary>
        /// Determines whether access to resources should be anonymous.
        /// </summary>
        /// <returns></returns>
        public static bool OnlyAnonymousAccess()
        {
            return CurrentUserHasSysRole(SysRoleEnum.Client) && CurrentSysUserObjOrFail.AnonymousPrint;
        }

        /// <summary>
        /// Performs login on behalf of the given user.
        /// </summary>
        /// <param name="sysUser"></param>
        public static void PerformLogin(UserObj sysUser)
        {
            CvmFacade.Security.PerformLogin(sysUser.UserName);
        }

        /// <summary>
        /// Determines whether the current user owns the given resource object.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool IsResourceOwner(Resource resource)
        {
            if (CurrentUser != null)
            {
                return resource.UserId == CurrentUser.UserId;
            }
            else 
                return false;
        }

        /// <summary>
        /// Determines whether a sysId exists for the current session.
        /// </summary>
        /// <returns></returns>
        public static bool CurrentSysIdIsSpecified()
        {
            return CurrentSysIdVal != null && CurrentSysIdVal > 0;
        }

        /// <summary>
        /// Determines whether the current user has a role, either in the global space or in the 
        /// currently chosen sys-space.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool CurrentUserHasRoleGlobalOrSys(string role)
        {
            return AllCurrentRoles.Contains(role);
        }
        
        public static bool CurrentUserHasRoleGlobal(GlobalRoleEnum role)
        {
            return CurrentUserHasRoleGlobalOrSys(role.ToString());
        }

        /// <summary>
        /// Determines whether a sysId is associated with the current session, and if it is, whether that site has the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCurrentSysType(SysRootTypeEnum type)
        {
            if (ContextObjectHelper.CurrentSysIdIsSpecified())
            {
                return ContextObjectHelper.CurrentSysRoot.SysRootTypeIdEnum == type;
            } 
            else
            {
                return false;
            }
        }

        public static string GetSysNameFromExplicitSysCode()
        {
            SysCode sc = FindExplicitSysCodeForCurrentRequestOrGlobal();
            
            if (sc.IsGlobal()) 
                return AdminContentMgr.instance.GetContent("ContextObjectHelper.GlobalSysName");
            else
            {
                SysRoot root = QueryMgr.instance.GetSysRootBySysCodeOrNull(sc.Value);
                
                if (root == null) 
                    return "";
                else 
                    return root.SysName;
            }
        }

        public static string GetSignupLinkForCurrentSite()
        {
            return CvmPages.SignUp(ContextObjectHelper.GetCurrentSysCodeOrFail()).GetLinkAsFullHref();
            //"http://"+ContextObjectHelper.GetCurrentSysCodeOrFail() +"."+HttpContext.Current.Request.Url.DnsSafeHost+ VirtualPathUtility.ToAbsolute("~/Public/Signup.aspx");
        }

        public static string GetSearchAppForCurrentSite()
        {
            return "http://" + "cvnav2.dk" + "/App/Search?sysCode=" + ContextObjectHelper.GetCurrentSysCodeOrFail();
        }
        
        public static string GetSearchAppForGlobal()
        {
            return  "http://www.cvnav.dk/App/Search";
        }
    }
}