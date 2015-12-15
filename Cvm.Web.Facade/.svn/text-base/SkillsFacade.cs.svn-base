using System;
using System.Linq;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Napp.Backend.Hibernate;
using Napp.Backend.Hibernate.DataFetcher;
using Napp.Common.MessageManager;

namespace Cvm.Web.Facade
{
    public class SkillsFacade
    {
        public Skill CreateSkill(string newSkill)
        {
            Skill s = new Skill();
            s.SkillName = newSkill;
            //Need to figure out how this is done in general
            //s.ProfileTypeIds = MyProject.RelatedResourceObj.ProfileTypeId;
            s.SkillTypeId = ConfigMgr.defaultSkillTypeId;
            HibernateMgr.Current.Save(s);
            HibernateDataFetcher.ClearKeyValueByEntityNameCache(Tables.SkillTb.TableName);
            return s;
        }


        public Skill GetOrCreateSkill(string skillName)
        {
            if (String.IsNullOrEmpty(skillName) || skillName.Length <= 1)
            {
                MessageManager.Current.PostMessage("EditCvSkillsCtrl.SkillMustHaveTwoLettersOrMore");
                return null;
            }
            else
            {

                var skill = QueryMgr.instance.GetSkillBySkillNameOrNull(skillName);
                if (skill == null)
                {
                    skill = CreateSkill(skillName);
                }
                return skill;
            }
        }

        /// <summary>
        /// Gets or creates a resource skill corresponding to the given skill
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public ResourceSkill EnsureResourceSkill(Skill skill, Resource res)
        {

            var q = from rs in Hiber.Q<ResourceSkill>()
                    where rs.SkillId == skill.SkillId && rs.ResourceId == res.ResourceId
                    select rs;
            ResourceSkill rskill=q.FirstOrDefault();
            if (rskill==null)
            {
                rskill=new ResourceSkill();
                rskill.RelatedResourceObj = res;
                rskill.RelatedSkillObj = skill;
                rskill.LevelEnum = SkillLevelEnum.Least;
                HibernateMgr.Current.Save(rskill);
            }
            return rskill;
        }
    }
}