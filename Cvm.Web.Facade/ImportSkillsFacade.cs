using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Import;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Backend.Business.Skills;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Facade
{
    public class ImportSkillsFacade
    {
        /// <summary>
        /// Looks for import-data assigned to the resource, runs through all skills registered in the system
        /// and checks whether the import text contains any of these skills.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="usePartialMatches"></param>
        public SkillMatchWrapperList GetPotentialImportSkills(Resource resource, bool usePartialMatches)
        {
            List<SkillMatchWrapper> result = new List<SkillMatchWrapper>();
            ResourceImport resImport = resource.RelatedResourceImport;
            if (resImport != null)
            {
                if (!String.IsNullOrEmpty(resImport.ImportText)) AddSkillsFromDocImport(resImport, usePartialMatches, result);
                if (!String.IsNullOrEmpty(resImport.LinkedInImport)) { AddSkillsFromLinkedInImport(resource, result);}
            }
            result.Sort((s,t)=>s.Skill.SkillName.CompareTo(t.Skill.SkillName));
            return new SkillMatchWrapperList(result);
        }

        private void AddSkillsFromLinkedInImport(Resource resource, List<SkillMatchWrapper> result)
        {
            string[] skillNames = LinkedInImportMgr.Create(resource).GetSkillNames();
            foreach (string skillName in skillNames)
            {
                SkillMatch match = new SkillMatch(true,"LinkedIn" );
                if (match != null)
                {
                    result.Add(new SkillMatchWrapper(new Skill() {SkillName=skillName}, match));
                }
            }
        }

        /// <summary>
        /// Add skills from imported Word og PDF documents
        /// </summary>
        /// <param name="resImport"></param>
        /// <param name="usePartialMatches"></param>
        /// <param name="result"></param>
        private void AddSkillsFromDocImport(ResourceImport resImport, bool usePartialMatches, List<SkillMatchWrapper> result)
        {
            String import = resImport.ImportText;
            HiberQuery<Skill> skillsQuery = QueryMgr.instance.GetAllSkill();
            IList<Skill> skills2 = skillsQuery.ListOrNull(HibernateMgr.Current.Session);
            foreach (Skill s in skills2)
            {
                String skillName = s.SkillName;
                SkillMatch match = MatchSkill(skillName, import, usePartialMatches);
                if (match != null)
                {
                    result.Add(new SkillMatchWrapper(s, match));
                }
            }
        }

        /// <summary>
        /// Assigns the skill to the given resource if it is not already assigned.
        /// Returns true if it was assigned.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool AssignSkill(Resource res, Skill skill)
        {

            return res.AddSkillOrSkip(skill);
        }
        public SkillLevelEnum GetDefaultSkillLevel()
        {
            return ConfigMgr.defaultSkillLevel;
        }


        internal bool DoesContainSkill(string skillName, string importText)
        {
            Regex regex = new Regex(skillName, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return regex.IsMatch(importText);
        }

        /// <summary>
        /// Returns a SkillMatch containing the context text in which the skillName 
        /// was found found or null if no match was found.
        /// First an exact match is attempted, then a partial match.
        /// </summary>
        /// <param name="skillName"></param>
        /// <param name="importText"></param>
        /// <param name="usePartialMatches">If true, we will also match within words, e.g. RAC will match Oracle.</param>
        /// <returns></returns>
        internal SkillMatch MatchSkill(string skillName, string importText, bool usePartialMatches)
        {
            string skillNameLower = skillName.ToLower();
            //First check for exact match with word boundaries on both sides.
            string safeRegEx = Regex.Escape(skillNameLower);
            Regex reg=null;
            
            try
            {
                reg = new Regex(@"\W" + safeRegEx + @"\W", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            }
            catch (ArgumentException)
            {
                //Do nothing as there is a valid risk that certain names can introduce regex compile errors.
            }
            if (reg != null)
            {
                Match match = reg.Match(importText);
                if (match.Success)
                {
                    //If we have a match find the index using the matched text
                    int index2 = importText.IndexOf(match.Value);
                    Debug.Assert(index2 > -1);
                    return GetMatchResult(skillName, importText, index2, true);
                }
            }
            if (usePartialMatches)
            {
                int index = importText.ToLower().IndexOf(skillNameLower);
                if (index > -1)
                {
                    return GetMatchResult(skillName, importText, index, false);
                }
            }

            //We found neither exact matched nor partial matches.
            return null;
            
        }


        private SkillMatch GetMatchResult(string skillName, string importText, int startIndex, bool isFullMatch)
        {
            string contextString = SearchUtil.Instance.HighLightSearchString(importText,skillName, startIndex,true);
            return new SkillMatch(isFullMatch,contextString);
        }


        /// <summary>
        /// Checks to see if the skills are already found in the database.
        /// If not, a new is created and an id is assigned. 
        /// If it is, the id is assigned.
        /// A new list is returned.
        /// </summary>
        /// <param name="enumerable"></param>
        public IEnumerable<Skill> PrepareSkills(IEnumerable<Skill> ss)
        {
            return LinkedInImportMgr.PrepareSkillsUtil(ss);
        }
    }

    public class SkillMatchWrapper
    {
        public readonly Skill Skill;
        public readonly SkillMatch Match;

        public SkillMatchWrapper(Skill skill, SkillMatch match)
        {
            Skill = skill;
            Match = match;
        }
    }

    public class SkillMatchWrapperList
    {
        private IList<SkillMatchWrapper> list;
        private string listOfSkillsAsString;
        private Exception exception;

        private SkillMatchWrapperList()
        {
            
        }

        public SkillMatchWrapperList(IList<SkillMatchWrapper> list)
        {
            this.list = list;
        }

        public IList<SkillMatchWrapper> GetIList()
        {
            return list;
        }

        /// <summary>
        /// Returns a whitespace seperated list of skills to be imported
        /// </summary>
        /// <returns></returns>
        public string GetListOfSkillsAsString()
        {
            if (this.listOfSkillsAsString==null)
            {
                if (exception!=null)
                {
                    this.listOfSkillsAsString = "ERROR";
                } else
                {
                    StringBuilder sb = new StringBuilder(this.list.Count * 15);
                    foreach (var item in this.list)
                    {
                        sb.Append(item.Skill.SkillName).Append(" ");
                    }
                    this.listOfSkillsAsString = sb.ToString();                                    
                }
            }
            return listOfSkillsAsString;
        }

        public static SkillMatchWrapperList CreateError(Exception exception)
        {
            SkillMatchWrapperList list=new SkillMatchWrapperList();
            list.exception = exception;
            return list;
        }
        public bool HasError()
        {
            return exception != null;
        }
        public String GetErrorMessage()
        {
            return exception.Message;
        }
    }

    /// <summary>
    /// Encapsulates the result of matching a skillname against an import-text.
    /// </summary>
    public class SkillMatch
    {
        public readonly String ContextText;
        public readonly bool IsFullMatch;

        public SkillMatch(bool isFullMatch, string contextText)
        {
            IsFullMatch = isFullMatch;
            this.ContextText = contextText;
        }
    }
}