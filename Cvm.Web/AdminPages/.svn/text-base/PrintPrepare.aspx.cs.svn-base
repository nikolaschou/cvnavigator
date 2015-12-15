using System;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.AdminPages
{
    public partial class PrintPrepare : System.Web.UI.Page
    {
        protected readonly RequestObject<Resource> req = new RequestObject<Resource>(QueryParmCvm.id, EditCv.ResourceCreator);
        private PrintDefContext context = new PrintDefContext();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true,true);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.AutoForm.ObjectSource = context.GetCurrent;
            this.AutoForm.OmitProperties = "PrintDefinitionId;LastModifiedTs";
            this.AutoForm.BuildForm();
            this.AutoForm.PopulateFront();
            this.CustomerDropDown3.DataSource = this.req.Current.GetDistinctProjectCustomers();
            this.CustomerDropDown3.DataTextField = Tables.Customer.ColCustomerName.Name;
            this.CustomerDropDown3.DataValueField = Tables.Customer.ColCustomerId.Name;
            this.CustomerDropDown3.DataBind();
        }

        protected void OnClick_PrintBtn(object sender, EventArgs e)
        {
            this.AutoForm.PopulateBack();
            
            if (String.IsNullOrEmpty(this.CustomerDropDown3.SelectedValue)) 
                this.context.Current.CustomerId = null;
            else 
                this.context.Current.CustomerId = long.Parse(this.CustomerDropDown3.SelectedValue);
            
            PageLink link = CvmPages.PrintCvPage.IncludeExistingParms();
            context.AddRequestParmsToLink(link);
            link.Redirect();
        }
    }
}
