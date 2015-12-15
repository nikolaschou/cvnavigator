using System;
using System.Web.UI;
using Cvm.Web.Code;
using Cvm.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class SearchCtrl : System.Web.UI.UserControl
    {
       /* protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack) this.SearchTextBox.Text = SearchStringContext.Instance.SearchString;
        }

        protected void OnClick_SearchBtn(object sender, ImageClickEventArgs e)
        {
            CvmPages.SearchCvLink(this.SearchTextBox.Text).Redirect();
        }
        */
    }
}