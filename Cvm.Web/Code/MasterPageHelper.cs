using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Backend.Business.Util;
using Cvm.ErrorHandling;
using Cvm.Web.Facade;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Web.AdminContentMgr;

namespace Cvm.Web.Code
{
    public class MasterPageHelper
    {
        private const string DEPLOY_INFO_FILE = "~/DeployInfo/deployInfo.txt";
        private const string VERSION_INFO_FILE = "~/DeployInfo/releases.txt";
        private const string MASTER_DID_INIT_PAGE = "MasterDidInitPage";
        private StringCollection subTitles = new StringCollection();
        private static string[] versionLines;
        private String AVOID_AUTO_COMPLETE = "AutoCmpl";

        private MasterPageHelper()
        {
        }

        public static MasterPageHelper Instance
        {
            get
            {
                if (HttpContext.Current.Items["master"] == null)
                {
                    HttpContext.Current.Items["master"] = new MasterPageHelper();
                }
        
                return HttpContext.Current.Items["master"] as MasterPageHelper;
            }
        }

        public void PushTitle(String subTitle)
        {
            this.subTitles.Add(subTitle);
        }

        public void PushTitleByContent(String contentId,params String[] args)
        {
            this.subTitles.Add(Utl.ContentHlp(contentId, args));
        }

        public void OnPageInit(bool filterSysId)
        {
            OnPageInit(filterSysId, false);
        }

        /// <summary>
        /// Initializes context. If filterSysId is true, a sysId-filter will be set up assuming a current sysId has
        /// been selected.
        /// </summary>
        /// <param name="filterSysId"></param>
        /// <param name="allowMissingSysId">If true, the sysId filtering will only be enabled if a current sysId is found.</param>
        public void OnPageInit(bool filterSysId, bool allowMissingSysId)
        {
            if (filterSysId)
            {
                ContextObjectHelper.CurrentSysId.InitObject();
         
                if (!ContextObjectHelper.CurrentSysIdIsSpecified())
                {
                    if (allowMissingSysId)
                    {
                        //Ok, ignore
                    } 
                    else
                    {
                        throw new InvalidProgramException("Expected a sysId at this point.");
                    }
                } 
                else 
                {
                    SysId idObj = ContextObjectHelper.CurrentSysId.GetObject();
                
                    if (idObj == null)
                    {
                        throw new AppException("Should not happen.").SetCode(ErrorCode.CvSystemUnspecified);
                    }

                    long sysId = idObj.SysIdInt;
                    HibernateMgr.Current.EnableSysIdFilter(sysId);
                    
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        SysUserObj userRole = ContextObjectHelper.CurrentSysUserObjOrNull;
                    
                        if (userRole == null)
                        {
                            //Hence it must be a resource user which does not have access to list pages
                        }
                        else
                        {
                            if (userRole.RoleIdEnum == SysRoleEnum.SysAdmin)
                            {
                                //Do nothing
                            }
                            else
                            {
                                if (userRole.FilterProfileStatusIds != (long) BitPatternEnum.BAll ||
                                    userRole.FilterEmployeeTypeIds != (long) BitPatternEnum.BAll)
                                {
                                    HibernateMgr.Current.EnableResourceAccessFilter(userRole.FilterProfileStatusIds, userRole.FilterEmployeeTypeIds);
                                }
                            }
                        }
                    }
                } 
            }
            
            this.DidInit = true;
        }

        public bool DidInit
        {
            get
            {
                bool? b = HttpContext.Current.Items[MASTER_DID_INIT_PAGE] as bool?;
                return b ?? false;
            }
            set 
            { 
                HttpContext.Current.Items[MASTER_DID_INIT_PAGE] = value; 
            }
        }

        public bool AvoidAutoComplete
        {
            get
            {
                bool? b = HttpContext.Current.Items[AVOID_AUTO_COMPLETE] as bool?;
                return b ?? false;
            }
            set 
            { 
                HttpContext.Current.Items[AVOID_AUTO_COMPLETE] = value; 
            }
        }

        public StringCollection GetSubTitles()
        {
            return this.subTitles;
        }

        public String GetPageTitle()
        {
            IBusinessObject obj = ContextObjectHelper.CurrentBusinessObject;
            string contentId = GetContentIdByPage("AdminMasterPage", null);
            
            if (obj != null) 
                return AdminContentMgr.instance.GetContent(contentId, obj.StandardObjectTitle);
            else 
                return AdminContentMgr.instance.GetContent(contentId);
        }

        /// <summary>
        /// Returns a content ID on the form 
        /// xxx.yyy_zzz
        /// where xxx is the given prefix and yyy is derived from the current URL
        /// and zzz is the given suffix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GetContentIdByPage(string prefix, string suffix)
        {
            string path = HttpContext.Current.Request.Path;
            return prefix + "." + path.Substring(path.LastIndexOf('/') + 1) + (suffix != null ? "_" + suffix: "");
        }

        public void DisableSysIdFilter(object sender, EventArgs e)
        {
            HibernateMgr.Current.DisableSysIdFilter();
        }

        public string GetDeployInfo()
        {
            string deployInfo = HttpContext.Current.Server.MapPath(DEPLOY_INFO_FILE);
            return ReadFileOrNull(deployInfo);
        }

        public string[] GetVersionInfo()
        {
            if (versionLines == null)
            {
                string versionFile = HttpContext.Current.Server.MapPath(VERSION_INFO_FILE);
                String versionTxt = ReadFileOrNull(versionFile);
                if (versionTxt != null) 
                    versionLines = versionTxt.Trim('\n').Split('\n');
            }

            return versionLines;
        }

        public AppVersion GetLatestVersionInfo()
        {
            string[] versions = GetVersionInfo();
            if (versions != null && versions.Length > 0) 
                return new AppVersion(versions[versions.Length - 1]);
            else 
                return AppVersion.Empty;
        }

        private string ReadFileOrNull(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var s = new StreamReader(fileStream);
                    String content = s.ReadToEnd();
                    fileStream.Close();
                    return content.Trim();
                }
            }
            else 
                return null;
        }
    }

    public class AppVersion
    {
        public string Version { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }

        public readonly static AppVersion Empty = new AppVersion(null, null, null, null);
        private static readonly Regex RegExWhiteSpace = new Regex(" +");

        public AppVersion(string version, string date, string time, string description)
        {
            Version = version;
            Date = date;
            Time = time;
            Description = description;
        }

        /// <summary>
        /// Expects the format 
        /// 1.0 15-06-2010 22:37:55,56 Here comes a release message
        /// </summary>
        /// <param name="line"></param>
        public AppVersion(String line)
        {
            line = RegExWhiteSpace.Replace(line, " ");
            Queue<String> words = new Queue<string>(line.Split(' '));
            Version = words.Dequeue();
            Date = words.Dequeue();
            string time = words.Dequeue();
            String[] timeparts = time.Split(':');
            Time = time[0] + ":" + time[1];
            StringBuilder sb = new StringBuilder();
        
            foreach (String s in words)
            {
                sb.Append(s).Append(" ");
            }
            
            Description = sb.ToString();
        }
    }
}