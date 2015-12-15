using System;
using System.Globalization;
using System.IO;
using System.Web;
using Cvm.Backend.FileStore;
using Cvm.Web.Backend.FileStore;
using Napp.Backend.Hibernate;
using Nshop.Backend.DataImport;

namespace Cvm.Web.Facade
{
    public class ImportDataFacade
    {
        private string[] importTables = {
"SiteConfig", 
"ProfileType", 
"EmployeeType", 
"Bureau", 
};

/*        private string[] importTables = {
"SiteConfig", 
"Country", 
"Language", 
"Industry", 
"ProfileType", 
"EmployeeType", 
"Resource",  
"ResourceImport", 
"Customer", 
"CustomerContact", 
"Project", 
"Education", 
"Bureau", 
"Certification", 
"Merit", 
"LanguageWritingLevel", 
"LanguageSpeakingLevel", 
"LanguageSkill", 
"SkillType", 
"Skill", 
"SkillLevel", 
"ResourceSkill", 
"ProjectSkill"
};*/

        private string startDataBlueVersion = "cvnavigator-start-data-1.0.xls";

        public void ImportDataBlueVersion(long sysId)
        {
            ImportData(sysId, startDataBlueVersion);
        }

        /// <summary>
        /// Imports data from any data file.
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="dataFile"></param>
        public void ImportData(long sysId, string dataFile)
        {
            String file;
            if (HttpContext.Current == null)
            {
                //Test case scenario
                file = AppDomain.CurrentDomain.BaseDirectory + "/../../Unittest/" +
                       dataFile;
            }
            else
            {
                file = HttpContext.Current.Server.MapPath(FolderStructure.ImportDataVirtual+dataFile);
            }
            if (!File.Exists(file)) throw new Exception("File not found: " + file);
             ImportMgr importMgr = new ImportMgr();
            importMgr.DeleteFromTables(importTables,sysId);
            importMgr.ImportFromSpreadSheet(file, importTables, sysId);
            HibernateSessionFactory.Instance.ClearSecondLevelCache();
        }
    }
}