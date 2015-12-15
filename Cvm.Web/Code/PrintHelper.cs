using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Print;
using Napp.Web.AdminContentMgr;

namespace Cvm.Web.Code
{
    public class PrintHelper
    {
        public static PrintHelper Instance = new PrintHelper();
        private PrintHelper() { }
        public String GetContent(string contentId)
        {
            return AdminContentMgr.instance.GetContent(contentId);
        }


    }
}
