using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cvm.Backend.Business.Unittest;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class ContextObjectHelperTest : CvmTest
    {
        [Test]
        public void TestUtil()
        {
            Assert.AreEqual(4, ContextObjectHelperUtil.FindSysIdFromSubDomain("S4.cvnav2.dk").SysIdInt);
            Assert.AreEqual(4, ContextObjectHelperUtil.FindSysIdFromSubDomain("s4.cvnav.dk").SysIdInt);
            Assert.IsNull(ContextObjectHelperUtil.FindSysIdFromSubDomain("xxx.cvnav.dk"));

        }

        [Test]
        public void TestRoles()
        {
        }
    }
}
