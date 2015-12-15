using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;

namespace Cvm.Web.Facade
{
    /// <summary>
    /// Used as the global facade class for the whole of CvmWeb application.
    /// </summary>
    public class CvmFacade
    {
        public readonly static NewSiteFacade NewSite = new NewSiteFacade();
        public readonly static NewCompanyFacade NewCompany = new NewCompanyFacade();
        public static readonly UserAdminFacade UserAdmin = new UserAdminFacade();
        public static readonly ImportDataFacade ImportData = new ImportDataFacade();
        public static readonly EditCvFacade EditCv = new EditCvFacade();
        public static readonly ImportSkillsFacade ImportSkills = new ImportSkillsFacade();
        public static readonly StatisticsFacade Statistics = new StatisticsFacade();
        public static readonly SkillsFacade Skills = new SkillsFacade();

        public static ImportCvsFacade ImportCvs = new ImportCvsFacade();
        public static readonly SecurityFacade Security = new SecurityFacade();
        public static MailFacade Mail = new MailFacade();
        public static FileFacade File = new FileFacade();
        public static SearchCvFacade Search = SearchCvFacade.Instance;
        public static AssignmentFacade Assignment = AssignmentFacade.Instance;

        public static SignupFacade Signup = SignupFacade.Instance;

        public static SysProfileFacade SysProfile = SysProfileFacade.Instance;
        public static AppWebServicesFacade AppWebServices = AppWebServicesFacade.Instance;

    }
}
