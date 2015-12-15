using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Facade;
using Napp.Backend.Hibernate;
using Napp.Web.Auto;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class GrantedSitesCtrl : UserControl, IAutoBuildPopulateWithSourceCtrl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void OnClickRemoveRelation(object sender, EventArgs e)
        {
            LinkButton b = (LinkButton)sender;
            String sysIdStr = b.CommandArgument;

            long sysId = long.Parse(sysIdStr);
            EditCvFacade.instance.RemoveGrantedSite(MyResource, sysId);
            this.BuildForm();
        }

        private void PopulateDropDown()
        {
            List<SysRoot> sRoots = (List<SysRoot>)QueryMgr.instance.GetAllSysRoot().ListOrNull(HibernateMgr.Current.Session);
            sRoots.Sort();

            this.GrantedSitesDropDown.DataSource = sRoots;
            this.GrantedSitesDropDown.DataBind();
        }

        public void PopulateFront()
        {
            // Do nothing
        }

        public void PopulateBack()
        {
            // Do nothing
        }

        public ObjectSource ObjectSource
        {
            get;
            set;
        }

        protected Resource MyResource
        {
            get 
            { 
                return (Resource) this.ObjectSource(); 
            }
        }

        public void BuildForm()
        {
            PopulateDropDown();

            this.Rep1.Controls.Clear();
            this.Rep1.DataSource = EditCvFacade.instance.GetGrantedSites(MyResource);
            this.Rep1.DataBind();
        }

        protected void OnChangeGrantSiteTB(object sender, EventArgs e)
        {
            EditCvFacade.instance.GrantSiteAccessToResource(MyResource, this.GrantedSitesDropDown.SelectedValue);
            this.BuildForm();
        }
    }
}