using System;
using System.Collections;
using System.Collections.Generic;
using Cvm.Backend.Business.Assignments;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Backend.DataFetcher;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.AdminPages
{
    public partial class EditAssignments : System.Web.UI.Page
    {
        protected readonly BusinessRequestObject<Assignment> req = new BusinessRequestObject<Assignment>(QueryParmCvm.id);
        protected bool hasIdParm;
        protected bool isModeNew;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.hasIdParm || isModeNew)
                {
                    this.AssignmentForm.PopulateFront();
                }
            }

            Utl.SetCurrentBusinessObject(req.Current);
            this.AuxCtrl2.MyAssignment = this.req.Current;
            this.AuxCtrl2.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            hasIdParm = Utl.Query.HasParm(QueryParmCvm.id);
            isModeNew = Utl.Query.GetMode() == PageMode.New;
        
            if (hasIdParm || isModeNew)
            {
                this.AssignmentForm.OmitProperties = Tables.Assignment.AddClientUserId.AddPrintDefinitionId.SelectedColumnsStr();
                this.AssignmentForm.ObjectSourceInstance = req.Current;
                this.AssignmentForm.BuildForm();           
                //If a customer has been chosen, load the relevant customer contact persons
                Customer customer = this.req.Current.RelatedCustomerObj;
                this.CustomerContactDropDown.Enabled = customer != null;
                
                if (customer != null)
                {
                    IEnumerable<IdTitlePair> cs = QueryMgrDynamicHql.Instance.GetCustomerContacts(customer.CustomerId);
                    this.CustomerContactDropDown.DataSource = cs;
                    this.CustomerContactDropDown.DataBind();
                }
            } 
            
            if (hasIdParm)
            {
                TabularCtrl1.SetupTabs(TabularCtrlHelper.AssignmentTabular,0);
            }
            
        }

        protected void OnClickSaveBtn(object sender, EventArgs e)
        {
            this.req.Current.ClientUserId = this.CustomerContactDropDown.SelectedLong;
            this.AssignmentForm.PopulateBack();
            this.req.Current.ValidateCustomerContactOrReset();

            if (this.isModeNew)
            {
                HibernateMgr.Current.Save(this.req.Current);
                HibernateMgr.Current.Session.Flush();
                PageNavigation.GetCurrentLink().IncludeExistingParms().ExcludeParm(QueryParmCommon.mode).SetParm(QueryParmCvm.id, this.req.Current.AssignmentId).Redirect();
            }

            this.AssignmentForm.PopulateFront();
            this.CustomerContactDropDown.SelectedLong = this.req.Current.ClientUserId;
            Utl.Msg.PostMessage("EditAssignments.AssignmentSaved", this.req.Current.StandardObjectTitle);
        }

        protected void OnClickCancelBtn(object sender, EventArgs e)
        {
            Utl.Msg.PostMessage("Standard.NothingSaved");
            PageNavigation.GotoCurrentPageAgainWithParms();
        }
    }
}