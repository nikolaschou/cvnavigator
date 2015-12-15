using System;
using System.Collections.Generic;
using System.Text;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Backend.Business.Unittest;
using Cvm.Backend.Business.Util;
using Napp.Backend.Hibernate;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class ImportCvsFacadeTest : CvmTest
    {

        [Test]
        public void ExtractEmail()
        {
            string emails = new ImportCvsFacade().ExtractEmailsFromStr(@"
s lkjdfæls kjfædslkj æa kjfdsælkjfd åpicw jæo kjckjædsz kjåo
yyy0@hotmail.com ccc. dr.dk.js@live-raw.ddr<tr>
yyy_1@hotmail.com ccc. dr.dk.js2@live-raw.ddr<tr> 
 yyy_2@hotmail.com ccc. dr.dk.js@live-raw.ddr<tr> 
");
            Assert.AreEqual(
            @"yyy0@hotmail.com dr.dk.js@live-raw.ddr yyy_1@hotmail.com dr.dk.js2@live-raw.ddr yyy_2@hotmail.com"
            , emails);
        }
        [Test]
        public void TestAll()
        {
            string[] ignores = new String[] { "BPHX", "February", "cv","uk","Blue","Phoenix" };
            
            string[] fileNames = 
@"Asbj+©rnVernang_20100205.docx
Bengt Johannessen.doc
BPHX XXX - Gitte Steinbu¦êchel.docx
Christian Weait  Hansen UK 20100129.doc
CV - Inge Christensen.pdf
CV Gia February 2010.pdf
CV Martin Lorentsen.pdf
CV-BPHXKimAndersen.docx
CV-FrankJeppesen 20100426.doc
CV_BirgitHjoth.doc
Dangis_Sirmulis_BluePhoenix_CV.doc".Split('\n');
            string res = new ImportCvsFacade().GetResourceNamesBestGuess(
                fileNames, new List<string>(ignores));
            Console.WriteLine(res);
        }
    }
}
