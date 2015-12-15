using System;
using System.Collections.Generic;
using System.Linq;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Web.Facade;

namespace Cvm.Web.Code
{
    public class SysProfileHelper
    {
        public SysSkill AddSkill(string skill, List<SysSkill> mySysSkills)
        {
            if (skill == null || skill.Trim().Length == 0)
            {
                Utl.Msg.PostMessage("SysProfiles.NoSkillSpecified");
                return null;
            }
            else
            {
                string skillLower = skill.ToLower();
                if (mySysSkills.Any((s => s.SkillName.ToLower().Equals(skillLower))))
                {
                    Utl.Msg.PostMessage("SysProfiles.SkillAlreadyAssigned", skill);
                    return null;
                }
                else
                {
                    SysSkill ss = CvmFacade.SysProfile.AddSysSkill(skill);
                    mySysSkills.Add(ss);
                    return ss;
                }
            }
        }

        /// <summary>
        /// Adds a SysSkill to the current SysId, relates it to the given profileType
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="relatedSkills"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public SysSkill AddSkillToProfileExclusively(string skill, List<SysSkill> relatedSkills, ProfileType p)
        {
            SysSkill ss = QueryMgrDynamicHql.Instance.GetSysSkillBySkillName(skill);
            if (ss != null)
            {
                relatedSkills.Add(ss);
            }
            else
            {
                ss = new SysProfileHelper().AddSkill(skill, relatedSkills);
            }
         
            if (ss != null)
            {
                ss.ProfileTypeId = p.ProfileTypeId;
            }

            relatedSkills.Sort((s,t)=>String.Compare(s.SkillName,t.SkillName));
            
            return ss;
        }
    }
}