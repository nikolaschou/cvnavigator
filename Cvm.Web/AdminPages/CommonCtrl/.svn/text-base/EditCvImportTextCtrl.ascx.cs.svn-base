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
using Cvm.Backend.Business.Resources;
using Napp.Backend.Hibernate;
using Nshop.Backend.Business.GenericDelegates;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class EditCvImportTextCtrl : System.Web.UI.UserControl
    {
        public ObjectGetter<ResourceImport> MyResourceImport;


        protected void OnTextChangedImportTextBox(object sender, EventArgs e)
        {
            ResourceImport imp = MyResourceImport();
            if (imp == null)
            {
                //Do nothing, it was deleted already
            }
            else
            {
                if (this.ImportTextBox.Text == null || this.ImportTextBox.Text.Trim().Length == 0)
                {
                    HibernateMgr.Current.Delete(imp);
                }
                else
                {
                    imp.ImportText = this.ImportTextBox.Text;
                }
            }
        }
    }
}