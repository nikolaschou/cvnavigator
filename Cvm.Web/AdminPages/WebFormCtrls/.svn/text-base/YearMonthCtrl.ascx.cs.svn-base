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
using Napp.Common.MessageManager;
using Napp.Web.Auto.Annotations;

namespace Cvm.Web.AdminPages.WebFormCtrls
{
    public partial class YearMonthCtrl : System.Web.UI.UserControl, ISimpleWebControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object ValueOfControl
        {
            get
            {
                string text = this.TextBox0.Text;
                if (String.IsNullOrEmpty(text))
                {
                    return null;
                } else
                {
                    DateTime? yearMonth = StringToYearMonth(text);
                    this.TextBox0.Text =YearMonth.YearMonthToString(yearMonth);
                    return yearMonth;                    
                }
            }
            set
            {
                this.TextBox0.Text = YearMonth.YearMonthToString(value as DateTime?);
            }
        }

        public string FieldContentId
        {
            get { return this.TextBox0.FieldContentId; }
            set { this.TextBox0.FieldContentId = value; }
        }

        private DateTime? StringToYearMonth(String value)
        {
            if (String.IsNullOrEmpty(value)) return null; 
            else
            {
                string[] parts = this.TextBox0.Text.Split(new char[] { '.', '-', '/' });
                if (parts.Length!=2)
                {
                    MessageManager.Current.PostMessage("Validation.InvalidYearMonth",value);
                    return null;
                } else
                {
                    string year = parts[0];
                    string month = parts[1];
                    return new DateTime(int.Parse(year), int.Parse(month), 1);                                    
                }
            }
        }



    }
}