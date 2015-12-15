using System;

namespace Cvm.Web.Public
{
    public class AppResourceReferenceVO
    {
        public string ProfileTitle;
        /// <summary>
        /// Skill level as a number between 1 and 5.
        /// </summary>
        public long SkillLevelNumber;
        public long ResourceId;
        public string Initials;
        public String AvailableBy;
        public string SkillLevelName;
    }
}