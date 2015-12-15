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

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class ProgressBar : System.Web.UI.UserControl
    {
        public IEnumerable Titles { get; set; }
        public int ChosenIndex { get; set; }
        protected override void OnPreRender(EventArgs e)
        {
            base.DataBind();
        }

        public override void DataBind()
        {
            //Do nothing, base.DataBind() will be called explicitly
            //from PreRender. This is to allow the properties
            //of this control to be set during normal Databind
            //while leaving the rendering of the control to the PreRender phase
        }
    }
}