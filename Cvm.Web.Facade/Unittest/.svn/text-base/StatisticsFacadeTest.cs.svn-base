using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cvm.Backend.Business.Unittest;
using Napp.Backend.Business.Meta;
using NUnit.Framework;

namespace Cvm.Web.Facade.Unittest
{
    public class StatisticsFacadeTest : CvmTest
    {
        [Test]
        public void TestGetTableCounts()
        {
            var result = CvmFacade.Statistics.GetTableCounts(TableMgr.instance().GetAllTables());
            for (int i = 0; i < result.GetNoDays(); i++)
            {
                Console.Write(result.GetDateStr(i) + " | ");
            }
            Console.WriteLine();
            for (int i = 0; i < result.GetNoTables(); i++)
            {
                Console.Write(result.GetTableName(i) + ":");
                for (int j = 0; j < result.GetNoDays(); j++)
                {
                    Console.Write(result.GetCount(i, j) + " | ");
                }
                Console.WriteLine();
            }
        }
    }
}
