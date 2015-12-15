using System;
using System.Collections.Generic;
using System.Linq;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;

namespace Cvm.Web.Public
{
    public class AppResourceVO
    {
        public long ResourceId;
        public string Initials;

        public string ProfessionalSince;

        public string ProfileTitle;

        public string ProfileResume;

        public string CountryName;

        public string LanguageName;
        public string AvailableBy;

        public String[] ExpertSkills;
        public String[] OtherSkills;
        public String[] Customers;

        public static AppResourceVO Create(Resource resource)
        {
            string[] expertSkills = PrepareList(resource.ResourceSkills.Where(s => s.LevelEnum == SkillLevelEnum.Most));
            string[] otherSkills = PrepareList(resource.ResourceSkills.Where(s => s.LevelEnum == SkillLevelEnum.More));
            String[] customers = resource.Projects
                .Where(p => !p.RelatedCustomerIsNull())
                .Select((p, i) => p.RelatedCustomerObj.CustomerName)
                .OrderBy(s => s)
                .Distinct()
                .ToArray();
            var vo = new AppResourceVO()
                         {
                             CountryName = resource.RelatedCountryIsNull() ? "" : resource.RelatedCountryObj.CountryName,
                             Initials = resource.GetInitials(),
                             LanguageName =
                                 resource.RelatedLanguageIsNull() ? "" : resource.RelatedLanguageObj.LanguageName,
                             ProfileResume = resource.ProfileResume??"",
                             ProfessionalSince = ""+resource.ProfessionalSince,
                             ProfileTitle = resource.ProfileTitle??"",
                             ResourceId = resource.ResourceId,
                             ExpertSkills = expertSkills,
                             OtherSkills = otherSkills,
                             Customers = customers
                         };
            return vo;
        }

        private static string[] PrepareList(IEnumerable<ResourceSkill> subset)
        {
            return subset.
                OrderBy(s => s.SkillName).
                Select((s , i)=>s.SkillName).
                ToArray();
        }
    }
}