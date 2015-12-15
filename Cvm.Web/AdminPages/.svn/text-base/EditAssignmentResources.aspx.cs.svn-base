using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Assignments;
using Cvm.Backend.Business.Externals;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.Hibernate;
using Napp.Web.AdmControl;
using Napp.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.AdminPages
{
    public partial class EditAssignmentResources : System.Web.UI.Page
    {
        private IList<Resource> searchedResources;
        protected readonly BusinessRequestObject<Assignment> req = new BusinessRequestObject<Assignment>(QueryParmCvm.id);
        private List<AssignmentResource> _assignmentResourcesSorted;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);            
        }

        protected override void OnInit(EventArgs e)
        {
            if(Utl.Query.GetMode() == PageMode.New) 
                CvmPages.EditAssignments.IncludeExistingParms().Redirect();
            
            TabularCtrl1.SetupTabs(TabularCtrlHelper.AssignmentTabular, 2);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PopulateGrid();
            Utl.SetCurrentBusinessObject(req.Current);
            this.AuxCtrl2.MyAssignment = this.req.Current;
            this.AuxCtrl2.DataBind();
        }

        private List<AssignmentResource> GetAssignedResources()
        {
            if (this._assignmentResourcesSorted==null)
            {
                this._assignmentResourcesSorted = new List<AssignmentResource>(req.Current.AssignmentResources);
                CvmFacade.Assignment.ValidateResources(this._assignmentResourcesSorted);
                this._assignmentResourcesSorted.Sort();
            }
        
            return _assignmentResourcesSorted;
        }

        protected Resource GetResource(IDataItemContainer container)
        {
            return ((AssignmentResource)container.DataItem).RelatedResourceObj;
        }

        protected void OnClickSearchBtn(object sender, EventArgs e)
        {
            this.SearchGrid.DataSource = SearchedResources;
            this.SearchGrid.DataBind();
        }

        private IList<Resource> SearchedResources
        {
            get
            {
                if (this.searchedResources == null)
                {
                    this.searchedResources = SearchResources();
                }
         
                return searchedResources;
            }
        }

        private IList<Resource> SearchResources()
        {
            String searchStr = this.KeywordTextBox.Text;
            if (String.IsNullOrEmpty(searchStr))
            {
                Utl.Msg.PostMessage("EditAssignmentResources.MustEnterKeyword");
                return null;
            } 
            else
            {
                IList<Resource> res = CvmFacade.Search.SearchResourcesGloballyBySearchString(searchStr);
                var res2 = new List<Resource>(res);
                res2.Sort((a,b)=>b.ProfileStatusId.CompareTo(b.ProfileStatusId));
                
                return res;
            }
        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName=="Add")
            {
                int index=Convert.ToInt32(e.CommandArgument);
                Resource res=this.SearchedResources[index];
                //If we already have this resource, ignore it
                if (this.req.Current.AssignmentResources.Count(a=>a.ResourceId==res.ResourceId)>0)
                {
                    Utl.Msg.PostMessage("EditAssignmentResources.ResourceAlreadyAdded",res);
                } 
                else
                {
                    AssignmentResource r = this.req.Current.AddNewResource(res);
                    HibernateMgr.Current.Save(r);
                    this._assignmentResourcesSorted = null;
                    this.RefreshGrid();
                }
            } 
            else
            {
                throw new InvalidProgramException();
            }
        }

        protected void OnChangeSelectedStatus(object sender, EventArgs e)
        {
            AdmDropDown dd = (AdmDropDown)sender;
            AssignmentResource res = this.GetAssignedResources()[dd.DataItemIndex];
            res.AssignmentResourceStatusId = (long)dd.SelectedLong;
        }

 
        protected void OnClickCreateLink(object sender, EventArgs e)
        {
            AdmLinkButton btn = (AdmLinkButton) sender;
            AssignmentResource res = this.GetAssignedResources()[btn.DataItemIndex];
            ExternalLink link=res.CreateLink(); 
            RefreshGrid();
        }

        protected void OnClickRemoveResource(object sender, EventArgs e)
        {
            AdmLinkButton btn = (AdmLinkButton)sender;
            AssignmentResource res = this.GetAssignedResources()[btn.DataItemIndex];
         
            foreach(var v in this._assignmentResourcesSorted)
            {
                if (v.ResourceId == res.ResourceId)
                {
                    this._assignmentResourcesSorted.Remove(v);
                    break;
                }
            }
            
            HibernateMgr.Current.Delete(res);
            RefreshGrid();
        }

        protected void OnClickSendToClient(object sender, EventArgs e)
        {
            if (this.req.Current.RelatedClientSysUserObj==null)
            {
                Utl.Msg.PostMessage("EditAssignmentResources.MissingContactPerson");
            } 
            else
            {
                AdmLinkButton btn = (AdmLinkButton)sender;
                AssignmentResource res = this.GetAssignedResources()[btn.DataItemIndex];
            
                if (res.RelatedExternalLinkIsNull())
                {
                    Utl.Msg.PostMessage("EditAssignmentResources.MissingLink");
                } 
                else
                {
                    CvmFacade.Mail.SendPrintResource(this.req.Current.RelatedClientSysUserObj.RelatedUserObjObj.Email,
                                                          res.RelatedResourceObj, res.RelatedExternalLinkObj);
                    
                    RefreshGrid();
                }
            }
        }

        private void RefreshGrid()
        {
            this.ResourceGrid.Controls.Clear();
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            this.ResourceGrid.DataSource = GetAssignedResources();
            this.ResourceGrid.DataBind();
        }

        protected String GetInfo(Resource res)
        {
            StringBuilder warningText=new StringBuilder();

            if (res.ProfileStatusIdEnum==ProfileStatusEnum.UnderUpdate)
            {
                warningText.Append(Utl.Content("EditAssignmentResources.WarningUnderUpdate")).Append(" ");
            }
            
            if (res.AvailableBy == null)
            {
                warningText.Append(Utl.Content("EditAssignmentResources.WarningNullAvail")).Append(" ");
            }
            
            if (res.AvailableBy != null && res.AvailableBy>this.req.Current.ContractStartBy)
            {
                warningText.Append(Utl.Content("EditAssignmentResources.WarningNotAvail")).Append(" ");
            }
            
            String str = warningText.ToString();
            
            if (!String.IsNullOrEmpty(str))
            {
                return "<span class='warning-symbol' title='" + str + "'><img src='../images/master/warning.png'/></span>";
            } 
            else
            {
                return "";
            }
        }
    }
}