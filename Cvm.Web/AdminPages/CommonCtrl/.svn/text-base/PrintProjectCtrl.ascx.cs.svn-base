using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Print;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class PrintProjectCtrl : System.Web.UI.UserControl
    {
        private readonly PrintDefContext printDefContext = new PrintDefContext();
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public String MakeAnonym(string name)
        {
            if (this.printDefContext.Current.HasPrintOptionsOne(CvPrintFlagEnum.Anonymous)) return GetContent("PrintCv.AnonymXXX");
            else return name;
        }

        public Project Project;
        public int FirstColWidth;
        public int PageWidth;

        protected String GetContent(string s)
        {
            return PrintHelper.Instance.GetContent(s);
        }
    }
}