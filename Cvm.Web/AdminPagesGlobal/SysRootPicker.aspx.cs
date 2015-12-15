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
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Backend.Hibernate;

namespace Cvm.Web.AdminPagesGlobal
{
    public partial class SysRootPicker : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SysRootDropDown.DataSource = QueryMgr.instance.GetAllSysRoot().ListOrNull(HibernateMgr.Current.Session);
                SysRootDropDown.DataBind();                
            }
        }

        protected void OnClickChooseSysRoot(object sender, EventArgs e)
        {
            short sysId = short.Parse(SysRootDropDown.SelectedItem.Value);
            ContextObjectHelper.CurrentSysId.OverrideObject(new SysId(sysId));
        }
    }
}
