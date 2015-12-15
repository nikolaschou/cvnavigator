using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Unittest;
using Cvm.Backend.Business.Users;
using Napp.Backend.Business.Multisite;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class UserAdminFacadeTest : CvmTest
    {
        private int next = new Random().Next(100000);

        [Test]
        public void TestSimplePassword()
        {
            
            CreateUtil("xy34"+next);
            CreateUtil("xyzw" + next);
            CreateUtil("1234" + next);
        }
        [Test]
        public void TestStrongPassword()
        {
            CreateUtil("xyzw.1234" + next);
            CreateUtil("xyzw1234" + next);
        }

        private void CreateUtil(string password)
        {
            UserObjWrap user = UserObjWrap.CreateWithExplicitUserName("Peterxxx"+new Random().Next(9999));
            user.Email = password+"someemail@hotmail.com";
            user.Password = password; user.PasswordConfirmed = password;
            CvmFacade.UserAdmin.CreateUserWithMembership(user,QueryMgr.instance.GetSysRootByIdOrNull(4));
        }
    }
}
