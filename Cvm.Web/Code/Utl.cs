using System;
using System.Web;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Localization;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Backend.BusinessObject;
using Napp.Common.MessageManager;
using Napp.Web.AdminContentMgr;
using Napp.Web.Navigation;

/// <summary>
/// A top-level util class with compact short cuts for frequent method calls.
/// </summary>
public class Utl
{
    /// <summary>
    /// Provides quick access to query string helper methods.
    /// </summary>
    public static QueryStringHelper Query 
    { 
        get 
        { 
            return QueryStringHelper.Instance; 
        } 
    }

    public static IMessageManager Msg
    {
        get 
        { 
            return MessageManager.Current; 
        }  
    }

    public static UserObj CurrentSysUser
    {
        get 
        { 
            return ContextObjectHelper.CurrentUser; 
        }
    }

    public static QueryMgr QueryMgr = Cvm.Backend.Business.DataAccess.QueryMgr.instance;

    public static String ContentHlp(String contentId, params string[] parms)
    {
        return AdminContentMgr.instance.GetContentWithHelpTextAsHtml(contentId, parms);
    }

    /// <summary>
    /// Returns the content text corresponding to the given contentId. If it is not already found,
    /// an empty string is returned and an empty string is inserted in the content database.
    /// </summary>
    /// <param name="contentId"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    public static String ContentHlpOrBlank(String contentId, params string[] parms)
    {
        DefaultToBlank(contentId);
        
        return ContentHlp(contentId);
    }

    /// <summary>
    /// Makes sure that the content found by the given contentId defaults to blank,
    /// i.e. if it is not found it is created with value ""
    /// </summary>
    /// <param name="contentId"></param>
    private static void DefaultToBlank(string contentId)
    {
        if (!AdminContentMgr.instance.HasContent(contentId))
        {
            AdminContentMgr.instance.UpdateContent(contentId, " ");
        }
    }

    public static String Content(String contentId, params string[] parms)
    {
        return AdminContentMgr.instance.GetContent(contentId, parms);
    }

    /// <summary>
    /// Returns the content by contentId or blank as default value.
    /// </summary>
    /// <param name="contentId"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    public static String ContentOrBlank(String contentId, params string[] parms)
    {
        DefaultToBlank(contentId);
        
        return AdminContentMgr.instance.GetContent(contentId, parms);
    }
    
    public static String Content(Enum e, params string[] parms)
    {
        return AdminContentMgr.instance.GetContent(e.GetType().Name + "." + e, parms);
    }

    public static string ContentHlpBread(string contentId, params string[] parms)
    {
        return "<span class='bread'>" + ContentHlp(contentId, parms) + "</span>";
    }
    
    public static string ContentBread(string contentId, params string[] parms)
    {
        return "<span class='bread'>" + Content(contentId, parms) + "</span>";
    }

    public static readonly LinkHelper LinkHelper = Cvm.Web.Code.LinkHelper.Instance;

    /// <summary>
    /// Determines whether the current user has any of the roles
    /// contained in the roles bit pattern.
    /// </summary>
    /// <param name="roleEnum"></param>
    /// <returns></returns>
    public static bool HasSysRole(SysRoleEnum roles)
    {
        return ContextObjectHelper.CurrentUserHasAnyRole(roles);
    }
    
    public static bool HasGlobalRole(GlobalRoleEnum roles)
    {
        return ContextObjectHelper.CurrentUserHasRoleGlobal(roles);
    }

    public static String Content(Enum e)
    {
        return AdminContentMgr.instance.GetContent(e);
    }

    public static void SetCurrentBusinessObject(IBusinessObject current)
    {
        ContextObjectHelper.CurrentBusinessObject = current;
    }

    public static bool HasPermission(PermissionEnum p)
    {
        return CvmFacade.Security.HasPermission(p);
    }

    /// <summary>
    /// Determines whether the current user owns the given resource object.
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    public static bool IsResourceOwner(Resource resource)
    {
        return ContextObjectHelper.IsResourceOwner(resource);
    }

    public static bool AllowContentEditing()
    {
        return Utl.HasSysRole(RoleSet.SysAdminAtLeast) || ContextObjectHelper.IsSimulatingUser() || WebConfigMgr.AlwaysContentEdit;
    }

    public static string GetLanguage()
    {
        if (CurrentSiteLanguage == null)
        {
            CurrentSiteLanguage = GetLanguageUtil();
        }
   
        return CurrentSiteLanguage;
    }

    private static string GetLanguageUtil()
    {
        UserObj user = ContextObjectHelper.CurrentUser;
        SiteLanguage lang = null;

        if (user != null && user.SiteLanguageId != null)
        {
            lang = QueryMgr.instance.GetSiteLanguageByIdOrNull((long)user.SiteLanguageId);

            if (lang != null)
                return lang.SiteLanguageCode;
        }
        else if (ContextObjectHelper.CurrentSysIdIsSpecified())
        {
            SysOwner owner = ContextObjectHelper.CurrentSysOwnerOrNull;

            if (owner != null)
            {
                lang = owner.RelatedSiteLanguageObj;
                
                if (lang != null) 
                    return lang.SiteLanguageCode;
            }
        }
        
        return SiteLanguageConst.DefaultLanguage;
    }

    private static String CurrentSiteLanguage
    {
        get 
        { 
            return HttpContext.Current.Items["CurrentSiteLanguage"] as String; 
        }
        set 
        { 
            HttpContext.Current.Items["CurrentSiteLanguage"] = value; 
        }
    }
}
