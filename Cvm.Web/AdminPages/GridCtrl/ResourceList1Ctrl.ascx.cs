using System;
using System.Collections.Generic;
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
using Cvm.Backend.Business.Resources;
using Napp.Web.AdmControl;

namespace Cvm.Web.AdminPages.GridCtrl
{
    public partial class ResourceList1Ctrl : System.Web.UI.UserControl
    {
        public AdmGridView Grid { get { return this.Grid1; } }
    }
}