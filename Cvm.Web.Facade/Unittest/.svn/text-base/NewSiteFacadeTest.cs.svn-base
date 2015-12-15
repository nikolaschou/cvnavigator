using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Unittest;
using Cvm.Backend.Business.Users;
using Napp.VeryBasic;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class NewSiteFacadeTest : CvmTest
    {
        [Test]
        public void TestAll()
        {
            var sysRoot = new SysRoot();
            String ticks = DateTime.Now.Ticks.GetHashCode()+"";
            sysRoot.SysName = "SysName" + ticks;
            sysRoot.SysCode = ticks.Substring(0, 5).Replace("-","X");

            var owner = new SysOwner();
            owner.SysOwnerContactEmail = "x" + ticks + "@ddd.dk";
            var userObj = new UserObjWrap(sysRoot.SysCodeObj);
            userObj.Email = owner.SysOwnerContactEmail;
            userObj.PartialUserName = owner.SysOwnerContactEmail;
            userObj.Password = "Vinder.15";
            userObj.PasswordConfirmed = "Vinder.15";
            bool ok = new NewSiteFacade().CreateSite(sysRoot, owner, userObj);
            Assert.IsTrue(ok);
            base.CommitAndNewSession();
            new ImportDataFacade().ImportDataBlueVersion(sysRoot.SysId);
            Console.WriteLine("Created new site: "+sysRoot.SysId);
        }
    }
}
