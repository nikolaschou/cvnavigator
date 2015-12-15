using System;
using Cvm.Backend.Business.Files;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Cvm.Backend.CvImport;

namespace Cvm.Web.Facade
{
    public class SignupFacade
    {
        public static SignupFacade Instance = new SignupFacade();
        private readonly CvImporterMgr _importMgr = CvImporterMgr.Instance;

        /// <summary>
        /// Creates a new Resource object with the given properties
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="fileName"></param>
        /// <param name="saveFunction"></param>
        /// <param name="hasUploadedDocument"></param>
        /// <param name="linkedInText"></param>
        /// <returns></returns>
        public Resource Signup(string firstName, string lastName, string email, string password, string fileName, Action<string> saveFunction, bool hasUploadedDocument, string linkedInText)
        {
            if (CvmFacade.UserAdmin.ValidateUser(email, password, password))
            {
                UserObj usr = CvmFacade.UserAdmin.CreateNewUserObjOrFail(firstName, lastName, email);
                CvmFacade.UserAdmin.CreateNewMemberhipUserOrFail(usr, password);
                SysId sysId = ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull();
                Resource res = _importMgr.CreateNewResource(usr, sysId);
                
                if (hasUploadedDocument)
                {
                    ImportDocumentToResource(fileName, res, saveFunction);
                }
                
                if (!String.IsNullOrEmpty(linkedInText))
                {
                    res.RelatedResourceImportOrCreate.LinkedInImport = linkedInText;
                }
                
                return res;
            } 
            else 
            {
                throw new UserNotCreatedException();
            }
        }

        public void ImportDocumentToResource(string fileName, Resource res, Action<string> saveFunction)
        {
            FileRef f = FileRefMgr.Instance.CreateFileRef(FileCategoryEnum.CV, fileName);
            res.RelatedCvFileRefObj = f;
            saveFunction(f.AbsolutePath);
            //Now when the file is saved we can extract the content 
            String extract = WordExtracter.Instance.ParseDocumentGeneric(f.GetAsFilePath());
            _importMgr.SetImportText(res, extract);
        }
    }
}
