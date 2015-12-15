using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Resources;
using Napp.VeryBasic.GenericDelegates;
using Napp.Web.Auto;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class ImportLinkedInCtrl : System.Web.UI.UserControl
    {
        /// <summary>
        /// A callback when import is done. The callback must take a string-argument which will be the JSON-string received from LinkedIn.
        /// </summary>
        public GenericEventHandler<String> OnImportDone;

        /// <summary>
        /// Returns the linkedIn text if any.
        /// </summary>
        public String LinkedInText
        {
            get { return this.LinkedInImportTxBox.Text; }
        }

        protected void OnChangeLinkedInImport(object sender, EventArgs e)
        {
            if (OnImportDone != null) OnImportDone(this.LinkedInImportTxBox.Text);
        }

        public bool AutoPostBackOnImport
        {
            set { this.LinkedInImportTxBox.AutoPostBack = value; }
        }

    }
}