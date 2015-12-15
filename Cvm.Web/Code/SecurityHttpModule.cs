using System;
using System.Collections;
using System.Security;
using System.Web;
using System.Web.Security;
using Cvm.Backend.Business.Users;
using Cvm.ErrorHandling;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;

namespace Cvm.Web.Code
{
    /// <summary>Security Http Module</summary>

    public class SecurityHttpModule : IHttpModule
    {
        public SecurityHttpModule() { }

        /// <summary>Initializes a module and prepares
        /// it to handle requests.</summary>
        /// <param name="context" 
        /// >An <see cref="T:System.Web.HttpApplication" />
        /// that provides access to the methods, properties,
        /// and events common to all application objects within
        /// an ASP.NET application </param>
        public void Init(System.Web.HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.AuthenticateRequest);

            context.AuthorizeRequest += this.AuthorizeRequest;
        }

        private void AuthorizeRequest(object sender, EventArgs e)
        {
        }

        /// <summary>Occurs when a security module
        /// has established the identity of the user.</summary>
        private void AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpApplication Application = (HttpApplication)sender;
            HttpRequest Request = Application.Context.Request;

            // Exit if we're on login.aspx or KeepAlive
            string lowerPath = Request.Url.AbsolutePath.ToLower();
            
            //Only validate aspx-pages
            if (!lowerPath.EndsWith(".aspx")) 
                return;

            if (lowerPath == FormsAuthentication.LoginUrl.ToLower() || lowerPath.EndsWith("keepalive.aspx")) 
                return;

            if (Utl.HasGlobalRole(GlobalRoleEnum.GlobalAdmin))
            {
                //Access to all
                return;
            }
            
            // not authenticated, or no siteMapNode exists
            if (Application.Context.User == null)
            {
                //unauthorized users are taken care of by other means.
                return;
            }
            
            //For error page, don't use security
            if (lowerPath.Contains("/error.aspx"))
            {
                return;
            }

            if (lowerPath.Contains("/public/") || lowerPath.Contains("/adminpagesglobal/") || lowerPath.Contains("/admin/"))
            {
                //These are protected by folder protection.
                return;
            }
            
            if (SiteMap.CurrentNode == null)
            {
                //Does not work for sub-parameters
                this.NotAllowed("The page cannot be found in the sitemap");
                return;
            }

            // Check if user is in roles
            IList roles = GetEffectiveRoles();
            if (roles == null || roles.Count == 0)
            {
                // No Roles found, we don't check anymore
            }
            else
            {
                // Loop through each role and check to see if user is in it.
                bool hasRole = false;

                foreach (string role in roles)
                {
                    if (ContextObjectHelper.CurrentUserHasRoleGlobalOrSys(role))
                    {
                        hasRole = true;
                        break;
                    }
                }

                if (!hasRole) 
                    NotAllowed("The user hasn't got a sufficient role for this page.");
            }

            //Resource users must approve the first time they use the system.
            if (ContextObjectHelper.CurrentUser.RelatedResource != null && !ContextObjectHelper.CurrentUser.AcceptedConditions)
            {
                string returnUrl = HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
                CvmPages.AcceptConditions.SetParm(QueryParmCvm.ReturnUrl, returnUrl).Redirect();
            }
        }

        private IList GetEffectiveRoles()
        {
            SiteMapNode node = SiteMap.CurrentNode;
            return GetEffectiveRolesUtil(node);
        }

        private IList GetEffectiveRolesUtil(SiteMapNode node)
        {
            if (node.Roles != null && node.Roles.Count > 0) 
                return node.Roles;
            else if (node.ParentNode != null) 
                return GetEffectiveRolesUtil(node.ParentNode);
            else 
                return null;
        }

        private void NotAllowed(String msg)
        {
            throw new SecurityException(msg).SetCode(ErrorCode.UserHasNoAccessToThisPage, HttpContext.Current.User.Identity.Name);
        }

        /// <summary>Disposes of the resources (other than memory)
        /// used by the module that implements
        /// <see cref="T:System.Web.IHttpModule" />.</summary>
        public void Dispose() { }
    }
}