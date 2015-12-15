using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Facade
{
    public class SysProfileFacade
    {
        public static SysProfileFacade Instance=new SysProfileFacade();
        private SysProfileFacade()
        {
        }


        public String[] GetSkillsBySearch(string skillSearch)
        {
            var query = from skill in Hiber.Q<Skill>() where skill.SkillName.StartsWith(skillSearch) select skill.SkillName;
            return query.ToArray();
        }

        public SysSkill AddSysSkill(string skillName)
        {
            Skill skill = CvmFacade.Skills.GetOrCreateSkill(skillName);
            SysSkill ss=new SysSkill();
            ss.RelatedSkillObj = skill;
            HibernateMgr.Current.Save(ss);
            return ss;
        }


        public ProfileType CreateProfileType(string profileName)
        {
            ProfileType p = new ProfileType();
            p.ProfileTypeName = profileName;
            HibernateMgr.Current.Save(p);
            return p;
        }

        public void RemoveSysSkill(long skillId)
        {
            var q = from ss in Hiber.Q<SysSkill>() where ss.SkillId == skillId select ss;
            var sysSkill = q.FirstOrDefault();
            if (sysSkill!=null) HibernateMgr.Current.Delete(sysSkill);
        }

        public void DeleteProfile(long profileTypeId)
        {
            var p=QueryMgr.instance.GetProfileTypeByIdOrNull(profileTypeId);
            if (p!=null)
            {
                using (IDbConnection conn = HibernateMgr.Current.GetDirectSqlConnection())
                {
                    conn.Open();
                    QueryMgrSql.Instance.DeleteProfileFromSysSkills(ContextObjectHelper.CurrentSysId.GetObject(), p.ProfileTypeId,
                                                                    conn);
                    conn.Close();
                }
                HibernateMgr.Current.Delete(p);
            }
            
        }
    }
}
