using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Web.Facade;

namespace Cvm.Web.Public
{
    public partial class AppWebServices : System.Web.UI.Page
    {

        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static String[] GetSkills(String arg0)
        {
            string[] skills = CvmFacade.SysProfile.GetSkillsBySearch(arg0);
            return skills;
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static String[] GetResources2(String skillSearch)
        {
            return CvmFacade.SysProfile.GetSkillsBySearch(skillSearch);
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static AppResourceReferenceVO[] GetResources(String skillName, String availableByStr)
        {
            DateTime availableBy;
            if (String.IsNullOrEmpty(availableByStr)) availableBy = DateTime.Now;
            else availableBy = DateTime.Parse(availableByStr);
            return new List<AppResourceReferenceVO>(CvmFacade.AppWebServices.GetResourcesBySkillAndAvailability(skillName, (DateTime)availableBy)).ToArray();
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static AppResourceVO GetResource(long resourceId)
        {
            return CvmFacade.AppWebServices.GetResourceVO(resourceId);
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static void BookMeeting(long resourceId, String contactEmail)
        {
            CvmFacade.AppWebServices.BookMeeting(resourceId, contactEmail);
        }
    }
}