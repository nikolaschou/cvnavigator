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
    public class SearchCvFacadeTest : CvmTest
    {
        [Test]
        public void TestAll()
        {
            BitPatternEnum profileTypeEnum = (BitPatternEnum)3;
            IList<SysResource> list = SearchCvFacade.Instance.SearchCv((ProfileStatusEnum)3, profileTypeEnum, (EmployeeTypeEnum)3,
                                                          new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                                                          new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 10);
            foreach (var r in list) { Console.WriteLine(r.RelatedResourceObj.FirstName + " " + r.ProfileTypeId); }
            Console.WriteLine("Found " + list.Count + " resources.");
        }
        [Test]
        public void TesSearch()
        {
            base.SetSysId3ForContext();
            BitPatternEnum all = BitPatternEnum.BAll;
            IList<Resource> list = SearchCvFacade.Instance.SearchCvAndMap((ProfileStatusEnum)all, all, (EmployeeTypeEnum)all,
                                                          null,
                                                          null,null);
            foreach (var r in list) { Console.Write(r.FirstName + " " + r.SysResourceContext.ProfileTypeId+" "); }
            Console.WriteLine("Found " + list.Count + " resources.");
        }
        [Test]
        public void TestCast()
        {
            int? x = 3;
            object y = x;
            long? z = (long?) (int?)y;
        }

        [Test]
        public void TestAll2()
        {
            BitPatternEnum profileTypeEnum = (BitPatternEnum)3;
            IList<SysResource> list = SearchCvFacade.Instance.SearchCv((ProfileStatusEnum)3, profileTypeEnum, (EmployeeTypeEnum)3,
                                                          null,null,null);
            Console.WriteLine("Found " + list.Count + " resources.");
            foreach (var res in list)
            {
                Console.WriteLine(res.RelatedResourceObj.RelatedResourceImport != null);
                
            }
        }


        [Test]
        public void TestSearchByString()
        {
            base.SetSysIdForContext(4);
            HibernateMgr.Current.EnableSysIdFilter(base.GetSysIdForContext());
            doSearch("petersen");
            doSearch("java");
            doSearch("SDC");
            doSearch("sDc");
            doSearch("Danske Bank");
        }

        private void doSearch(string search)
        {
            IList<ISearchResult> result = SearchCvFacade.Instance.SearchCvByString(search);
            Console.WriteLine("Search-results for " + search);
            int maxCounter = 2;
            int counter = 0;
            foreach(ISearchResult r in result)
            {
                if(counter<maxCounter)
                {
                    Console.WriteLine(SearchResultUtil.MakeString(r));
                    counter++;
                }
                else break;
            }
        }

        [Test]
        public void TestSearchResourcesGloballyBySearchString()
        {
            base.SetSysId3ForContext();
            IList<Resource> result = CvmFacade.Search.SearchResourcesGloballyBySearchString("*");
            Console.WriteLine("Found " + result.Count + " resources");
            foreach (var r in result)
            {
                Console.Write(r.FirstName + " " + r.SysResourceContext.SysId);
            }
        }

    }
}
