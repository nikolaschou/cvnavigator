using System;
using System.Collections.Generic;
using System.Text;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Unittest;
using Cvm.Backend.Business.Util;
using Napp.Backend.Hibernate;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class EditCvFacadeTest : CvmTest
    {
        [Test]
        public void TestAll()
        {
            IList<Skill> allSkills = QueryMgr.instance.GetAllSkill().ListOrNull(HibernateMgr.Current.Session);
            Console.WriteLine("Found "+allSkills.Count+" skills.");
            IList<ResourceSkill> table = EditCvFacade.instance.GetResourceSkillTable(QueryMgr.instance.GetResourceById(16),(SkillTypeEnum)BitPatternConst.All,BitPatternConst.All,false);
            //Assert.AreEqual(allSkills.Count,table.Count);
            foreach (ResourceSkill resSkill in table)
            {
                Console.WriteLine(resSkill.SkillName+" "+resSkill.Level);                
            }
        }
    }
}
