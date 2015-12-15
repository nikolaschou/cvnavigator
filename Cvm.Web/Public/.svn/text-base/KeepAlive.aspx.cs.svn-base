using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using log4net;

namespace Cvm.Web.Public
{
    public partial class KeepAlive : System.Web.UI.Page
    {
        private static int counter = 1;
        private static ILog log = LogManager.GetLogger(typeof (KeepAlive));
        protected void Page_Load(object sender, EventArgs e)
        {
            log.Info("Keeping alive "+(counter++));
        }
    }
}
