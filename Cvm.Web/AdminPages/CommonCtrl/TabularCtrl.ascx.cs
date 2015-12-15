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
using Cvm.Web.Code;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class TabularCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Assign the Urls you want to get tabbed
        /// </summary>
        public IEnumerable<PageLink> Urls
        {
            get; set;
        }

        /// <summary>
        /// The zero-based index which should be highlighted as chosen.
        /// </summary>
        public int ChosenIndex
        {
            get; set;
        }

        /// <summary>
        /// The content-ID which will be prefixed to Tab0, Tab1, ...
        /// to create content-ID's for the tabs.
        /// </summary>
        public String ContentPrefix
        {
            get; set;
        }

        /// <summary>
        /// Sets up this tab with the given tab descriptor and the given chosen index.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="chosenIndex"></param>
        public void SetupTabs(TabularCtrlHelper.TabDescriptor tab, int chosenIndex)
        {
            this.ContentPrefix = tab.ContentPrefix;
            this.Urls = tab.Links;
            this.ChosenIndex = chosenIndex;
            this.DataBind();
        }
    }
}