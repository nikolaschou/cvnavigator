using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Napp.Web.AdmControl;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public class LayerCtrl2 : ContainerCtrl
    {
        protected override ControlCollection CreateControlCollection()
        {
            ControlCollection children = base.CreateControlCollection();
            return children;
        }
    }
}
