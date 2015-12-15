using System;
using Cvm.Web.Code;
using Cvm.Web.Facade;

namespace Cvm.Web.AdminPages
{
    public partial class ClientWelcome : System.Web.UI.Page
    {

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected bool CanListResources()
        {
            return ContextObjectHelper.CurrentSysUserObjOrFail.CanListResources();
        }
    }
}
