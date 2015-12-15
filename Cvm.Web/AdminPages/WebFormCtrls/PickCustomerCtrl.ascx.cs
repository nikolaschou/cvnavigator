using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Meta;
using Napp.Backend.Hibernate;
using Napp.Backend.Hibernate.DataFetcher;
using Napp.Web.Auto.Annotations;

namespace Cvm.Web.AdminPages.WebFormCtrls
{
    public partial class PickCustomerCtrl : System.Web.UI.UserControl, ISimpleWebControl, IControlWithRequired
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object ValueOfControl
        {
            get
            {
                if (String.IsNullOrEmpty(this.NewCustomerNameBx.Text))
                {
                    return this.CustomerDropDown.SelectedLong;
                }
                else
                {
                    Customer c = new Customer();
                    c.CustomerName = this.NewCustomerNameBx.Text;
                    HibernateMgr.Current.Save(c);
                    HibernateDataFetcher.ClearKeyValueByEntityNameCache(Tables.CustomerTb.TableName);
                    this.CustomerDropDown.Controls.Clear();
                    this.CustomerDropDown.Activate();
                    return c.CustomerId;
                }
            }
            set { this.CustomerDropDown.SelectedLong = (long?)value; }
        }

        public string FieldContentId
        {
            get;
            set;
        }

        public bool Required
        {
            set
            {
                //Ignored, not used for now
            }
        }
    }
}