using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cvm.Backend.Business.Unittest;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class SignupFacadeTest : CvmTest
    {
        [Test]
        public void TestFacade()
        {
            //Cannot be testet by unittest.
           // SignupFacade.Instance.Signup("Nikola", "Schou", "xxx@hotmail.com", "abc.123", "Nikola Schou CV", SaveDoc);
        }

        private void SaveDoc(string fileName)
        {
            System.IO.StreamWriter writer=new StreamWriter(fileName);
            writer.Write("<html><body>Her kommer en tekst</body></html>");
        }
    }
}
