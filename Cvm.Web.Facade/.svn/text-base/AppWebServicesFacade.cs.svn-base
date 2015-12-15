using System;
using System.Collections.Generic;
using System.Linq;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Users;
using Cvm.Backend.Business.Util;
using Cvm.Web.Facade.Util;
using Cvm.Web.Public;
using Napp.Backend.Business.Multisite;
using Napp.Web.AdminContentMgr;

namespace Cvm.Web.Facade
{
    public class AppWebServicesFacade
    {
        private AppWebServicesFacade() { }
        public static AppWebServicesFacade Instance = new AppWebServicesFacade();


        public IEnumerable<AppResourceReferenceVO> GetResourcesBySkillAndAvailability(string skillName, DateTime availableBy)
        {
            SysId sysId=ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull();
            if (sysId!=null && !QueryMgr.instance.GetSysOwnerById(sysId.SysIdInt).HasPreference(SysOwnerPreferencesEnum.publicSearch))
            {
                throw new Exception("You are not allowed to search these resources.");
            }
            IList<ResourceSkill> ress = QueryMgrDynamicHql.Instance.GetResourcesBySkillAndAvailability(skillName, availableBy, sysId);
            foreach(var r in ress)
            {
                r.RelatedResourceObj.KeepAnonymous = true;
                yield return new AppResourceReferenceVO()
                                 {
                                     ResourceId = r.ResourceId,
                                     SkillLevelNumber = BitPatternHelper.GetBitPosition(r.Level),
                                     SkillLevelName = AdminContentMgr.instance.GetContent(r.LevelEnum),
                                     ProfileTitle = r.RelatedResourceObj.ProfileTitle,
                                     Initials = r.RelatedResourceObj.GetInitials(),
                                     AvailableBy = AppUtil.ToStr(r.RelatedResourceObj.AvailableBy)
                                 };

            }
        }

        public AppResourceVO GetResourceVO(long resourceId)
        {
            Resource r = QueryMgr.instance.GetResourceById(resourceId);
            return AppResourceVO.Create(r);
        }

        /// <summary>
        /// Sends an email to the administration user of the current SysId of the given resource
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="contactEmail"></param>
        public void BookMeeting(long resourceId, string contactEmail)
        {
            Resource r = QueryMgr.instance.GetResourceById(resourceId);
            SysResource sysEntry = r.GetSysResourcesAsList().FirstOrDefault(
                sr => sr.RelatedSysRoot.SysRootTypeIdEnum == SysRootTypeEnum.Consulting);
            if (sysEntry!=null)
            {
                SysRoot sysRoot = sysEntry.RelatedSysRoot;
                String sysEmail = sysRoot.RelatedSysOwner.SysOwnerContactEmail;
                CvmFacade.Mail.SendContactRequestedMail(sysEmail, contactEmail, r.ResourceId, r.FullName);
                CvmFacade.Mail.SendContactRequesteConfirmationEmail(contactEmail, r.ProfileTitle, r.GetInitials());
            }
        }
    }
}