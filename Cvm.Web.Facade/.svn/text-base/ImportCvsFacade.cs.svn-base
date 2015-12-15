using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Util;
using Cvm.Backend.CvImport;
using Cvm.Backend.FileStore;
using Cvm.Web.Backend.FileStore;
using Napp.Backend.Business.Common;
using Napp.Backend.Business.Multisite;
using Napp.Common.MessageManager;
using Napp.VeryBasic;
using Napp.Web.AdminContentMgr;

namespace Cvm.Web.Facade
{
    public class ImportCvsFacade
    {
        
        /// <summary>
        /// Reads the archiveFile, gets all entries, builds up the list of CvImportItem objects,
        /// and finally updates the first name and last name fields according to the 
        /// resource name given in the resourceNamesText parameter (which is new-line seperated).
        /// </summary>
        /// <param name="archiveFile"></param>
        /// <param name="resourceNamesText"></param>
        /// <returns></returns>
        public IEnumerable<CvImportItem> GetPotentialImportItems(FilePath archiveFile, IList<String> resourceNamesBestGuess)
        {
            IList<CvImportItem> items = new CvZipArchive(archiveFile).ReadDocumentEntriesAsItems();
           
            //if (strings.Length>items.Count) throw new Exception(String.Format("Sanity check. Number of resource names {0} should not be bigger than the number of available entries in the archive {1}. ",strings.Length,items.Count));
            for (int i = 0; i < resourceNamesBestGuess.Count; i++)
            {
                items[i].SetDefaultValuesFromName(resourceNamesBestGuess[i]);
            }
            return items;
        }

        public List<string> SplitNames(string namesInFreeText)
        {
            return CvImportUtil.Instance.SplitNames(namesInFreeText);
        }

        public String[] GetPendingFilesNames(SysRoot root)
        {
            return FolderStructure.Instance.GetPendingFiles(root.SysCodeObj);
        }


        public CvImporter GetImporter(SysCode sysCode,string archiveName)
        {
            return new CvImporter(sysCode,new CvZipArchive(FolderStructure.Instance.GetPendingFile(sysCode,archiveName)));
        }

        public ICollection<string> GetArchiveEntryNames(FilePath archive)
        {
            return new CvZipArchive(archive).ReadDocumentEntries();
        }

        public string GetResourceNamesBestGuess(IEnumerable<string> fileNames, List<string> _ignoreFilters)
        {
            return CvImportUtil.Instance.GetResourceNamesBestGuess(fileNames, _ignoreFilters);
        }

        /// <summary>
        /// Returns names and emails 
        /// </summary>
        /// <param name="resourceIds">A commaseperated list of resourceIds</param>
        /// <returns></returns>
        public NamesAndEmails GetNamesAndEmail(string resourceIds)
        {
            List<Resource> res2 = GetResourcesByIds(resourceIds);
            NamesAndEmails namesAndEmail = new NamesAndEmails();
            int nrRes = res2.Count;
            res2.Sort();
            foreach (Resource r in res2)
            {
                namesAndEmail.AppendName(r.FullName);
                namesAndEmail.AppendEmail(ExtractEmail(r));
            }
            return namesAndEmail;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceIds">A commaseperated list of resourceIds</param>
        /// <returns></returns>
        public List<Resource> GetResourcesByIds(string resourceIds)
        {
            IList<Resource> res = QueryMgrDynamicHql.Instance.GetResourcesByIds(resourceIds);
            List<Resource> res2 = new List<Resource>(res);
            res2.Sort();
            return res2;
        }

        internal string ExtractEmail(Resource resource)
        {
            if (!String.IsNullOrEmpty(resource.Email)) return resource.Email;
            if (resource.RelatedResourceImport == null) return null;

            var importText = resource.RelatedResourceImport.ImportText;
            return ExtractEmailsFromStr(importText);
        }

        /// <summary>
        /// Extracts all emails from the string and returns them as a whitespace
        /// seperated list of emails.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal string ExtractEmailsFromStr(string str)
        {
            if (String.IsNullOrEmpty(str)) return null;
            //Replace tag-characters
            str = str.Replace("<", " ").Replace(">", " ");
            Regex emailMatch = EmailHelper.EmailRegex;
            MatchCollection match = emailMatch.Matches(str);
            HashSet<String> emails=new HashSet<string>();
            if (match.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Match c in match)
                {
                    string foundEmail = c.Value;
                    //avoid duplicates
                    if (!emails.Contains(foundEmail))
                    {
                        sb.Append(foundEmail).Append(" ");
                        emails.Add(foundEmail);
                    }
                }
                return sb.ToString().Trim();
            }
            return null;
        }
        /// <summary>
        /// Assigns email adressses from a list of emails to a list of resources.
        /// </summary>
        /// <param name="emails">A string with multiple email adresses seperated by linebreaks</param>
        /// <param name="res"></param>
        /// <returns></returns>
        public string PersistEmails(string emails, List<Resource> res)
        {
            String[] emailsArr=emails.Split('\n');
            int counter = 0;
            foreach (Resource r in res)
            {
                if (counter>=emailsArr.Count())
                {
                    //We don't have enough email-adresses, exit
                    break;
                }
                string email = emailsArr[counter].Trim();
                if (!String.IsNullOrEmpty(email))
                {
                    if (email!=r.Email)
                    {
                        bool isValid = r.SetEmailValidated(email);
                        if (isValid)
                        {
                            MessageManager.Current.PostMessage("ImportCvs.EmailAssigned", r.FullName, email);
                
                            //Resetting this email to allow for more rounds
                            emailsArr[counter] = "";
                        }
                    }
                }
                counter++;
            }
            return emailsArr.ConcatToString("\n");
        }

        public SkillMatchWrapperList GetSkillsToImport(Resource resource)
        {
            SkillMatchWrapperList skills = CvmFacade.ImportSkills.GetPotentialImportSkills(resource, false);
            return skills;

        }
    }

    public class NamesAndEmails
    {
        private readonly List<String> names = new List<string>();
        private readonly List<String> emails = new List<string>();

        public void AppendName(string fullName)
        {
            this.names.Add(fullName);
        }

        public void AppendEmail(string extractEmail)
        {
            this.emails.Add(extractEmail);
        }

        public String GetNamesNewlined()
        {
            return names.ToArray().ConcatToString("\n");
        }
        public String GetEmailsNewlined()
        {
            return emails.ToArray().ConcatToString("\n");
        }
    }
}