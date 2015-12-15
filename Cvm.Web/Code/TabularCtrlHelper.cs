using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;

namespace Cvm.Web.Code
{
    public class TabularCtrlHelper
    {
        /// <summary>
        /// Describes the tabs for the Assignment pages.
        /// </summary>
        /// <returns></returns>
        public static TabDescriptor AssignmentTabular
        {
            get
            {
                TabDescriptor tab = new TabDescriptor();
                tab.ContentPrefix = "Assignment";

                List<PageLink> tabLinks = new List<PageLink>();
                tabLinks.Add(CvmPages.EditAssignments);
                tabLinks.Add(CvmPages.EditAssignmentsSearch);
                tabLinks.Add(CvmPages.EditAssignmentResources);
                //tabLinks.Add(CvmPages.EditAssignmentPrintDef);

                tab.Links = tabLinks;
                return tab;
            }
        }

        /// <summary>
        /// A data structure to hold all data for tabulars.
        /// </summary>
        public class TabDescriptor
        {
            internal TabDescriptor() {}
            public IEnumerable<PageLink> Links;
            public String ContentPrefix;
        }
    }
}
