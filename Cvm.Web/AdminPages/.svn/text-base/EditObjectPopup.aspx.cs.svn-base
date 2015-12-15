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
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Web.AdminPages.GenericCtrls;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class EditObjectPopup : System.Web.UI.Page
    {
        private object obj;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Panel1.Controls.Add(GetEditCtrl());
        }

        private Control GetEditCtrl()
        {
            string type = QueryStringHelper.Instance.GetParmOrFail(QueryParmCvm.type);
            if (type.Equals("Project"))
            {
                return GetEditCtrlHelper<Project>();
            }
            else if (type.Equals("Education"))
            {
                return GetEditCtrlHelper<Education>();
            }

            else if (type.Equals("Certification"))
            {
                return GetEditCtrlHelper<Certification>();
            }
            else
            {
                throw new NotImplementedException(type);
            }
        }

        private EditObjectFromListCtrl<T> GetEditCtrlHelper<T>() where T : IBusinessObject
        {
            EditObjectFromListCtrl<T> ctrl = new EditObjectFromListCtrl<T>();
            ctrl.IsCreateNewMode = false;
            ctrl.ObjectSource = GetObject;
            ctrl.BuildForm();
            if (!this.IsPostBack)
            {
                ctrl.PopulateFront();
            }
            return ctrl;
        }

        private object GetObject()
        {
            if(obj==null)
            {
                Type type = TypeParser.ParseType(QueryStringHelper.Instance.GetParmOrFail(QueryParmCvm.type));
                this.obj = HibernateMgr.Current.LoadById(type, QueryStringHelper.Instance.GetParmIntOrFail(QueryParmCvm.id));
            }
            return obj;
        }
    }
}
