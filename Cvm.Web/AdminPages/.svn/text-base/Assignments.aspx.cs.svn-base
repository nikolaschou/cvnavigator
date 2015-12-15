using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Cvm.Backend.Business.Assignments;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;
using Cvm.Web.Facade;

namespace Cvm.Web.AdminPages
{
    public partial class Assignments : System.Web.UI.Page
    {
        private List<Assignment> _assignments;
        private Assignment _currentAssignment;

        protected override void OnInit(EventArgs e)
        {
            if (this.AssignmentList.Count() > 0)
            {
                this.AssignmentDropDown.DataSource = this.AssignmentList;
                this.DataBind();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.AssignmentList.Count() == 0)
            {
                this.FullPanel.Visible = false;
                Utl.Msg.PostMessage("Assignments.FoundNone");
            }
            else
            {
                this.ResourceList.Grid.Controls.Clear();
                List<Resource> relatedResourceList = CurrentAssignment.GetRelatedResourceList();
                if (relatedResourceList.Count > 0)
                {
                    this.ResourceList.Grid.DataSource = relatedResourceList;
                    this.ResourceList.DataBind();
                } else
                {

                    Utl.Msg.PostMessage("Assignments.NoResourcesAssigned");
                }
            }
        }

        protected Assignment CurrentAssignment
        {
            get
            {
                if (AssignmentList.Count() == 0) return null;
                if (this._currentAssignment==null)
                {
                    int selectedIndex;
                    if (!IsPostBack)
                    {
                        selectedIndex = 0;
                    } else
                    {
                        selectedIndex = this.AssignmentDropDown.SelectedIndex;
                    }
                    this._currentAssignment = AssignmentList.ElementAtOrDefault(selectedIndex);
                }
                return this._currentAssignment;
            }

        }

        ///
        protected List<Assignment> AssignmentList
        {
            get
            {
                if (this._assignments==null)
                {
                    this._assignments=new List<Assignment>(CvmFacade.Assignment.GetAssigmentsForCurrentUser());
                }
                return this._assignments;
            } 


        }

        


        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
    }
}
