using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Cvm.Backend.Business.Util;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Hibernate;
using Napp.Web.AdmControl;
using Napp.Web.AdminContentMgr;
using Napp.Web.ContentManagerExprBuilder;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class ReportCv : System.Web.UI.Page
    {
        private IList<Customer> allCustomers;
        private IList<Resource> allResources;
        private IList<Resource> _resourcesCached;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ProfileTypeCheckList.SelectAll();
                this.EmployeeTypeCheckList.SelectAll();
                this.ProfileStatusCheckList.SelectMultipleByBitPattern((long) (ProfileStatusEnum.Done | ProfileStatusEnum.UnderUpdate));

            }
            DoSearch();
            
        }

        protected void OnClick_SearchProfile(object sender, EventArgs e)
        {
            //Do nothing, is done on every reload
        }

        private void DoSearch()
        {
            IList<Resource> resources2 = GetResourcesCached();

            DataBind(resources2);
        }

        private IList<Resource> GetResourcesCached()
        {
            if (_resourcesCached == null)
            {
                ProfileStatusEnum status = (ProfileStatusEnum) this.ProfileStatusCheckList.GetSelectedInt();
                BitPatternEnum profileType = (BitPatternEnum) this.ProfileTypeCheckList.GetSelectedInt();
                EmployeeTypeEnum empType = (EmployeeTypeEnum) this.EmployeeTypeCheckList.GetSelectedInt();
                if (empType == 0) empType = (EmployeeTypeEnum) (-1);
                if (profileType == 0) profileType = (BitPatternEnum) (-1);
                long[] customerIds = GetIds(CustomerListBox);
                long[] resourceIds = new long[0];// GetIds(ResourceListBox);
                int? availInNoDays = this.AvailableInDays.ValueInt;
                _resourcesCached = SearchCvFacade.Instance.SearchCvAndMap(status, profileType, empType, customerIds,
                                                                          resourceIds, availInNoDays);
            }
            return _resourcesCached;
        }


        private void DataBind(IList<Resource> resources2)
        {
            if (this.ExtendedViewCheckBox.Checked)
            {
                this.ResourcesGrid.DataSource = resources2;
                this.ResourcesGrid.DataBind();
            } else
            {
                this.ResourcesGridSimple.DataSource = resources2;
                this.ResourcesGridSimple.DataBind();
                
            }
            bool b = Utl.HasSysRole(RoleSet.SalesMgrAtLeast);
            this.ResourcesGridSimple.Columns[4].Visible = b;
            this.ResourcesGridSimple.Columns[5].Visible = b;
        }

        private long[] GetIds(ListBox list)
        {
            long[] res=new long[list.GetSelectedIndices().Length];
            int counter = 0;
            foreach(ListItem item in list.Items)
            {
                if (item.Selected) res[counter++] = long.Parse(item.Value);
            }
            return res;
        }

        private long[] GetResourceIds(int[] ints)
        {
            long[] resIds=new long[ints.Length];
            int counter = 0;
            foreach (int i in ints)
            {
                resIds[counter++] = this.allResources[i].Idfr2.ObjId;
            }
            return resIds;
        }

        private long[] GetCustomerIds(int[] ints)
        {
            long[] cIds = new long[ints.Length];
            int counter = 0;
            foreach (int i in ints)
            {
                cIds[counter++] = allCustomers[i].Idfr2.ObjId;
            }
            return cIds;
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            allCustomers = QueryMgr.instance.GetAllCustomer().ListOrNull(HibernateMgr.Current.Session);
            allResources = QueryMgr.instance.GetAllResourceCached().ListOrNull(HibernateMgr.Current.Session);
            
            this.ProfileStatusCheckList.EnumType = typeof(ProfileStatusEnum);

            this.ProfileTypeCheckList.Activate();
            this.EmployeeTypeCheckList.Activate();
            this.ProfileStatusCheckList.Activate();

            this.CustomerListBox.DataSource = DoSort<Customer>(allCustomers, delegate(Customer c) { return c.StandardObjectTitle; });
            this.CustomerListBox.DataBind();
            //this.ResourceListBox.DataSource = DoSort<Resource>(allResources, delegate(Resource c) { return c.StandardObjectTitle; }); ;
            //this.ResourceListBox.DataBind();
            this.AdvancedSearchCheckBox.DataBind();

        }

        private List<T> DoSort<T>(IList<T> origList, Object2String<T> getter)
        {
            List<T> list = new List<T>(origList);
            list.Sort(GetComparer<T>(getter));
            return list;
        }

        private Comparison<T> GetComparer<T>(Object2String<T> stringGetter)
        {
            return delegate(T c1, T c2) { return (c1 == null ? -1 : stringGetter(c1).CompareTo(stringGetter(c2))); };
        }

       



        protected void OnClickSearchAllCustomers(object sender, EventArgs e)
        {
            ClearSelection(this.CustomerListBox);
        }

        private void ClearSelection(ListBox box)
        {
            box.SelectedIndex = -1;
        }

        

        protected void OnClickToSpreadSheet(object sender, EventArgs e)
        {
            this.Response.ContentType = "application/vnd.ms-excel";
            
        }

        protected void OnClick_ShowEmailList(object sender, EventArgs e)
        {

        }

        protected string GetEmailAddresses()
        {
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (var r in this.GetResourcesCached())
            {
                if (!String.IsNullOrEmpty(r.Email) && r.Email.Trim().Length > 0)
                {
                    if (!isFirst) sb.Append(", ");
                    sb.Append(r.Email);
                    isFirst = false;
                }
            }
            return sb.ToString();
        }



        protected string GetResourcesWithoutEmailAddresses()
        {
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (var r in this.GetResourcesCached())
            {
                if (String.IsNullOrEmpty(r.Email) || r.Email.Trim().Length == 0)
                {
                    if (!isFirst) sb.Append(", ");
                    sb.Append("<a href='")
                        .Append(CvmPages.EditCvLink(r.ResourceId,EditCvTab.BaseData)
                        .GetLinkAsHref())
                        .Append("'>")
                        .Append(r.FullName)
                        .Append("</a>");
                    isFirst = false;
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Knows how to get a string from an object.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    internal delegate String Object2String<T>(T x);
}
