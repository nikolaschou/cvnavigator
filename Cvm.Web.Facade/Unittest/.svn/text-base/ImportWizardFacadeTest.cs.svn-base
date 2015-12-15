using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Backend.Business.Unittest;
using Cvm.Backend.Business.Util;
using Napp.Backend.Hibernate;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class ImportWizardFacadeTest : CvmTest
    {

        [Test]
        public void TestAll()
        {
            string s =
@"
Jeg kender en del 
Javascript, Java, c++, Ecmascript, Toracle, PL/1, SmoothWall firewall, abc (efg), abc [efg], Java Enterprise Bean og andet 
men også til XML.";

            Verify(s, "java", true, true);
            Verify(s, "Script", true, false);
            Verify(s, "Oracle", true, false);
            Verify(s, "java enterprise bean", true, true);
            Verify(s, "xml", true, true);
            Verify(s, "c++", true, true);
            Verify(s, "xml2", false, false);
            Verify(s, "Script", true, false);
            Verify(s, "abc (efg)", true, true);
            Verify(s, "abc [efg]", true, true);
            Verify(s, "SmoothWall (firewall)", false, false);
        }

        private void Verify(string importText, string skillName,bool mustMatch,bool isFullMatch)
        {
            ImportSkillsFacade fac = new ImportSkillsFacade();
            SkillMatch match = fac.MatchSkill(skillName, importText, true);
            bool didMatch = match!=null;
            Assert.AreEqual(mustMatch, didMatch);
            if (didMatch)
            {
                Assert.AreEqual(isFullMatch, match.IsFullMatch);
                Console.WriteLine(skillName + ": " + match.ContextText);                
            }
        }
    }
}
