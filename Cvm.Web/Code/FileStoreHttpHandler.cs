using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using Cvm.Backend.Business.Files;
using Cvm.Backend.FileStore;
using Cvm.Web.Facade;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Code
{
    /// <summary>
    /// Maps requests on the form cvnavigator/FileStore/CV/Peter Petersen.doc/view.axd
    /// to cvnavigator/FileStore/syscode/CV/Peter Petersen.doc
    /// </summary>
    public class FileStoreHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HibernateMgr.InitializeForRequestScope();
            HibernateMgr.Current.BeginSessionAndTransaction();
            try
            {
                MasterPageHelper.Instance.OnPageInit(false);
                String path = context.Request.Path;
                string token = "/FileViewer.axd/".ToLower();
                int index = path.ToLower().IndexOf(token);
                if (index == -1) 
                    throw new Exception("Expected token as part of the path: " + token);
                /*string substitute = "/"+FolderStructure.FileStoreNoDash+"/"+ContextObjectHelper.GetCurrentSysCodeOrFail()+"/";
                String docPath = path.Substring(0,index)+substitute+path.Substring(index+token.Length);*/
                String fileName = path.Substring(index + token.Length);
                    //Should be something like guid.doc where guid is a real guid
                FileRef f =
                    (from fref in HibernateMgr.Current.Query<FileRef>() where fref.FileName == fileName select fref).
                        FirstOrDefault();
                
                if (f == null)
                {
                    context.Response.Write("File " + fileName + " not found.");
                }
                else
                {
                    String docPath = f.GetAsFilePath().AbsolutePath;
                    string fileHandle = docPath;
                    
                    if (!File.Exists(fileHandle))
                    {
                        throw new Exception("Path cannot be accessed: " + docPath);
                    }
                    else
                    {
                        context.Response.BufferOutput = true;

                        //context.Response.CacheControl = "public, max-age=604800";
                        context.Response.ContentType = GetContentType(docPath);
                        context.Response.WriteFile(fileHandle);
                        context.Response.Flush();
                        //context.Response.Close();
                    }
                }
            } 
            finally
            {
                HibernateMgr.Current.CommitAndCloseTransactionAndSession();
            }
        }

        private string GetContentType(string docPath)
        {
            String lpath = docPath.ToLower();
            if (lpath.EndsWith(".pdf")) return "application/pdf";
            else if (lpath.EndsWith(".doc")) return "application/msword";
            else if (lpath.EndsWith(".docx")) return "application/octet-stream";
            else if (lpath.EndsWith(".xml")) return "text/xml";
            else if (lpath.EndsWith(".html")) return "text/html";
            else if (lpath.EndsWith(".jpg")) return "image/jpeg";
            else if (lpath.EndsWith(".jpeg")) return "image/jpeg";
            else if (lpath.EndsWith(".bmp")) return "image/bmp";
            else if (lpath.EndsWith(".gif")) return "image/gif";
            else if (lpath.EndsWith(".png")) return "image/png";
            else return "text/plain";
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
