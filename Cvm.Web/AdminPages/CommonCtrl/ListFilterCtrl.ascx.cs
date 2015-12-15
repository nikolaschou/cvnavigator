using System;
using System.Collections.Generic;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Web.AdminContentMgr;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class ListFilterCtrl : System.Web.UI.UserControl
    {
        public static string DefaultFilterText
        {
             get
             {
                 return Utl.Content("ListFilterCtrl.FilterDefault");
             }   
        }
        private QueryStringHelperCvm query = QueryStringHelperCvm.Instance;
        private List<IBusinessObject> _listData;

        protected void OnChangeFilterText(object sender, EventArgs e)
        {
            //Do nothing, will be handled during normal Page_Load
        }

        private void PopulateList()
        {
            List<IBusinessObject> all = GetListData();
            string listFilter = ListFilter.ToLower();
            
            if (!String.IsNullOrEmpty(listFilter)) 
                all.RemoveAll(a => a.ExtendedObjectTitle.ToLower().IndexOf(listFilter) == -1);
            
            all.Sort(delegate(IBusinessObject a, IBusinessObject b)
            { return String.Compare(a.ExtendedObjectTitle, b.ExtendedObjectTitle); });

            this.rep.DataSource = all;
            this.rep.DataBind();
        }

        protected List<IBusinessObject> GetListData()
        {
            Type type = TypeParser.ParseType(this.ObjectType);

            if (_listData == null)
            {
                if (TableName != null)
                    this._listData = HibernateMgr.Current.GetAllByTypeCached<IBusinessObject>(type, TableName, ContextObjectHelper.CurrentSysUserObjOrNull.CompanyId.ToString());
                else
                    this._listData = HibernateMgr.Current.GetAllByTypeCached<IBusinessObject>(type);
            }
            
            return _listData;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.rep.Controls.Clear();
            this.PopulateList();
        }

        protected String ListFilter
        {
            get
            {
                bool isDefault = DefaultFilterText.Equals(this.FilterTextBox.Text);
            
                if (isDefault)
                {
                    //Check query string
                    return QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.filter) ?? "";
                }
                else
                {
                    return this.FilterTextBox.Text;
                }
            }
        }

        protected String GetContent(string s)
        {
            return AdminContentMgr.instance.GetContent("ListFilterCtrl." + this.ObjectType + s);
        }

        /// <summary>
        /// Sets the name of the object type on which to do filtering.
        /// </summary>
        public string ObjectType
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public string TableValue
        {
            get;
            set;
        }

        public IBusinessObject CurrentObject
        {
            get;
            set;
        }

        protected bool IsSelected(IBusinessObject dataItem)
        {
            return dataItem.Equals(CurrentObject);
        }
    }
}