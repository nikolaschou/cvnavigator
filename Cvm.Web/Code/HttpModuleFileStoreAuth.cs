using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Backend.Business.Files;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.FileStore;
using Cvm.Web.Facade;

namespace Cvm.Web.Code
{
    public class HttpModuleFileStoreAuth : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += OnAuthenticated;
        }

        private void OnAuthenticated(object sender, EventArgs e)
        {
            string path = HttpContext.Current.Request.Path;
            int index = path.ToLower().IndexOf(FolderStructure.FileStore.ToLower());
            if (index>-1)
            {
                SysRoot root = ContextObjectHelper.CurrentSysRoot;
                if (!FileRefMgr.Instance.ValidatePath(path, root.SysCodeObj))
                {
                    throw new UnauthorizedAccessException("Cannot access "+path+" for sys-code "+root.SysCode);
                }
            }
            
        }

        public void Dispose()
        {
            //Do nothing
        }
    }
}
