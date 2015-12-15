using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Files;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.CvImport;
using Iesi.Collections.Generic;
using Napp.Backend.DataFetcher;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;

namespace Cvm.Web.Facade
{
    public class EditCvFacade
    {
        internal EditCvFacade()
        {
        }

        public static readonly EditCvFacade instance = new EditCvFacade();
        /// <summary>
        /// Returns all skills in a table in which defined skills are merged in.
        /// </summary>
        /// <returns></returns>
        public IList<ResourceSkill> GetResourceSkillTable(Resource res, SkillTypeEnum skillTypeIds, long profileTypeIds, bool includeAllAssigned)
        {
            List<ResourceSkill> merged;
            if (includeAllAssigned || !ContextObjectHelper.CurrentSysIdIsSpecified())
            {
                merged = GetResourceSkillsFilterOnlyAssigned(res, skillTypeIds);
            }
            else
            {
                merged = GetResourcesSkillsAsPartOfAll(res, skillTypeIds, profileTypeIds);                
            }

            merged.Sort(delegate(ResourceSkill a, ResourceSkill b)
            {return String.Compare(a.SkillName, b.SkillName); });
            
            return merged;
        }

        private List<ResourceSkill> GetResourceSkillsFilterOnlyAssigned(Resource res, SkillTypeEnum skillTypeIds)
        {
            List<ResourceSkill> merged;
            List<ResourceSkill> temp = (List<ResourceSkill>) res.GetResourceSkillsAsList();
            merged = new List<ResourceSkill>();
            
            foreach (ResourceSkill s in temp)
            {
                if ((s.RelatedSkillObj.SkillTypeId & (long)skillTypeIds) != 0)
                {
                    merged.Add(s);
                }
            }
            
            return merged;
        }

        /// <summary>
        /// Returns the resource skills as part of a larger set of skills as determined by the parameters
        /// </summary>
        /// <param name="res"></param>
        /// <param name="skillTypeIds">A bitpattern describing which skill types to include</param>
        /// <param name="profileTypeIds">A bitpattern describing which profile types to include</param>
        /// <param name="includeOtherAssigned">If true, all skills to which the resource has assigned experience will be returned, though still filtered by the other criteria</param>
        /// <returns></returns>
        private List<ResourceSkill> GetResourcesSkillsAsPartOfAll(Resource res, SkillTypeEnum skillTypeIds, long profileTypeIds)
        {
            Dictionary<Skill, ResourceSkill> map = GetResourceSkillsAsMap(res);
            List<Skill> allSkills = new List<Skill>(SkillMgr.GetRelevantSkills(ContextObjectHelper.CurrentSysIdObj, profileTypeIds));
            List<ResourceSkill> merged = new List<ResourceSkill>(allSkills.Count);
            
            foreach (Skill skill in allSkills)
            {
                //Filter by skillType
                if (skill.IsAmong(skillTypeIds))
                {
                    //If the skill is specified, use that
                    if (map.ContainsKey(skill))
                    {
                        merged.Add(map[skill]);
                    }
                    else
                    {
                        //Create it and assign relations.
                        ResourceSkill newResourceSkill = new ResourceSkill();
                        newResourceSkill.RelatedSkillObj = skill;
                        newResourceSkill.RelatedResourceObj = res;
                        merged.Add(newResourceSkill);
                    }
                }
            }
            
            return merged;
        }

        private Dictionary<Skill, ResourceSkill> GetResourceSkillsAsMap(Resource res)
        {
            Iesi.Collections.Generic.ISet<ResourceSkill> definedSkills = res.ResourceSkills;
            Dictionary<Skill, ResourceSkill> map = new Dictionary<Skill, ResourceSkill>();
         
            foreach (ResourceSkill resSkill in definedSkills)
            {
                map[resSkill.RelatedSkillObj] = resSkill;
            }
            
            return map;
        }

        private List<Skill> GetAllSkills()
        {
            IList<Skill> skills = QueryMgr.instance.GetAllSkill().ListOrNull(HibernateMgr.Current.Session);
            return new List<Skill>(skills);
        }

        /// <summary>
        /// Returns a sorted list of key-value pairs with the key being an ISkill 
        /// and the value a boolean determining whether the skill is 
        /// related to the project.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public SortedList<ISkill, bool> GetProjectSkillsAsSubsetOfResourceSkills(Project p)
        {
            Iesi.Collections.Generic.ISet<ProjectSkill> pskills = p.ProjectSkills;
            Iesi.Collections.Generic.ISet<ResourceSkill> rskills = p.RelatedResourceObj.ResourceSkills;
            SortedList<ISkill, bool> result = new SortedList<ISkill, bool>(rskills.Count);
            //Build set of existing ID's
            Iesi.Collections.Generic.ISet<long> pskillIds = new HashedSet<long>();
            
            foreach (ProjectSkill pskill in pskills)
            {
                pskillIds.Add(pskill.SkillId);
            }
            
            foreach (ISkill s in rskills)
            {
                bool contains = pskillIds.Contains(s.SkillId);
                result.Add(s, contains);
                //Make sure to empty it as we want to find out if any project
                //skills are not also a resource skill
                if (contains) 
                    pskillIds.Remove(s.SkillId);
            }
            
            //Now, if resource skills does not cover project skills,
            //pskillIds will be non-empty. Clean up now
            foreach(ProjectSkill ps in pskills)
            {
                if (pskillIds.Contains(ps.SkillId))
                {
                    MessageManager.Current.PostMessage("EditCvFacade.RemoveProjectSkill", ps.GetObjectTitle());
                    HibernateMgr.Current.Delete(ps);
                }
            }
            
            return result;
        }

        public void RemoveProjectSkill(long skillId, Project _project)
        {
            ProjectSkill pskill = GetProjectSkillBySkillId(skillId, _project);
            HibernateMgr.Current.Delete(pskill);
        }

        private ProjectSkill GetProjectSkillBySkillId(long _skillId, Project project)
        {
            foreach (ProjectSkill pskill in project.ProjectSkills)
            {
                if (pskill.SkillId == _skillId) 
                    return pskill;
            }
            throw new Exception("Coding error: Expected to find skill with id " + _skillId);
        }

        public void AddProjectSkill(long skillId, Project prj)
        {
            //Add now
            ProjectSkill ps = new ProjectSkill();
            ps.RelatedProjectObj = prj;
            ps.RelatedResourceObj = prj.RelatedResourceObj;
            ps.SkillId = skillId;
            prj.ProjectSkills.Add(ps);
            if (prj.IsPersisted())
            {
                HibernateMgr.Current.SaveOrUpdate(ps);
            } else
            {
                //Do nothing, 
                //the caller must remember to save after the project is saved.
            }
        }

        public void CloseImport(Resource res)
        {
            FileRef fref = res.RelatedCvFileRefObj;
         
            if (fref!=null)
            {
                res.RelatedCvFileRefObj = null;
                FileRefMgr.Instance.DeleteFile(fref);
            
                if (res.RelatedResourceImport != null)
                {
                    HibernateMgr.Current.Delete(res.RelatedResourceImport);
                }

                MessageManager.Current.PostMessage("EditCvFacade.ImportDataRemoved", res.FullName);
            }
        }

        public IEnumerable<IdTitlePair> GetResourcesForDropDown()
        {
            return QueryMgrDynamicHql.Instance.GetResourcesForCurrentSysId();
        }

        /// <summary>
        /// Removes the relation between resource and the given sysId
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="sysId"></param>
        public void RemoveGrantedSite(Resource resource, long sysId)
        {
            var q = from r in Hiber.Q<SysResource>()
                    where r.SysId == sysId && r.ResourceId == resource.ResourceId
                    select r;
            SysResource sr=q.First();
            HibernateMgr.Current.Delete(sr);
        }

        public List<SysResource> GetGrantedSites(Resource resource)
        {
            var q = from r in Hiber.Q<SysResource>()
                    where r.ResourceId == resource.ResourceId
                    select r;
            return q.ToList();  
        }

        public void GrantSiteAccessToResource(Resource res, string code)
        {
            String sysCode = code;
            SysRoot root = QueryMgr.instance.GetSysRootBySysCodeOrNull(sysCode);

            if (root != null)
            {
                if (GetGrantedSites(res).Count(s=>s.SysId==root.SysId)==0) 
                    CvImporterMgr.Instance.GrantSiteAccessToResource(res, root.SysIdObj);
                else 
                    MessageManager.Current.PostMessage("EditCvFacade.SiteAlreadyGranted");
            }
            else
            {
                MessageManager.Current.PostMessage("EditCvFacade.SysCodeNotFound");
            }
        }
    }
}
