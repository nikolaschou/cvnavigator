using System;
using System.Web;
using Cvm.Backend.Business.Users;
using Cvm.ErrorHandling;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;

namespace Cvm.Web.Public
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                CvmPages.WelcomePage.Redirect();
            }

            if (Utl.HasSysRole(SysRoleEnum.CompanyHR))
            {
                CvmPages.CompanyWelcome.Redirect();
            }
            else if (Utl.HasSysRole(SysRoleEnum.Client))
            {
                CvmPages.ClientWelcome.Redirect();
            }
            else if (Utl.HasSysRole(RoleSet.SalesMgrAtLeast))
            {
                CvmPages.MenuPage.Redirect();
            } 
            else
            {
                var  resource = ContextObjectHelper.CurrentUser.RelatedResource;

                if (resource == null)
                {
                    throw new AppException().SetCode(ErrorCode.ResourceUserHasNoAccessToResource, ContextObjectHelper.CurrentUserName);
                }

                if (Utl.Query.HasParm(QueryParmCvm.jobId))
                    CvmPages.EditCvLink(resource.ResourceId, EditCvTab.MySite).IncludeExistingParms().Redirect();
                    
                    //CvmPages.EditCvLink(resource.ResourceId, EditCvTab.MySite, Utl.Query.GetParmOrDefault(QueryParmCvm.jobId, 0)).Redirect();
                else
                    CvmPages.EditCvLink(resource.ResourceId, EditCvTab.MySite).Redirect();
            }
        }
    }
}