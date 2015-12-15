using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Cvm.Backend.Business.Externals;
using Cvm.Backend.Business.Print;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.Public
{
    public partial class ExternalPrint : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string externalId = Utl.Query.GetParmOrFail(QueryParmCvm.externalId);
            ExternalLink link = Utl.QueryMgr.GetExternalLinkByLinkGuidOrNull(externalId);
            this.MyPrintCvCtrl.MyResource = link.RelatedResourceObj;
            PrintDefinition printDef = link.RelatedPrintDefinitionObj;
            if (printDef == null) printDef = PrintDefinition.GetDefaultDefinition();
            this.MyPrintCvCtrl.MyPrintDefinition = printDef;
            this.MyPrintCvCtrl.DataBind();
        }
    }
}
