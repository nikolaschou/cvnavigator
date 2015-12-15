using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Sorting;
using Cvm.Backend.Business.Users;
using Cvm.Web.AdminPages.CommonCtrl;
using Cvm.Web.AdminPages.GenericCtrls;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.BusinessObject;
using Napp.Backend.DataFetcher;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.Web.AdmControl;
using Napp.Web.Auto;
using Napp.Web.AutoFormExt;
using Napp.Web.Navigation;
using Napp.Web.Session;
using Cvm.Backend.Business.Companies;
using Cvm.Backend.Business.DataAccess;

namespace Cvm.Web.AdminPages
{
    public partial class EditCv : System.Web.UI.Page
    {
        private const string OMIT_PROPERTIES = "LastModifiedTs;ResourceId;RelatedResource;RelatedResourceObj;linkedInId";
        protected readonly RequestObject<Resource> req = new RequestObject<Resource>(QueryParmCvm.id, EditCv.ResourceCreator);

        private IAutoBuildPopulateWithSourceCtrl innerCtrl;
        private readonly long resourceId = QueryStringHelper.Instance.GetParmOrDefault(QueryParmCvm.id, 0);
        private string _previousId, _nextId;
        private SysId sysId;

        /// <summary>
        /// This object is used hold a local cache while
        /// editing or creating new projects, educations or certifications.
        /// </summary>
        private Object listItem;

        private PageMode? _mode;
        private IEnumerable<IdTitlePair> _allResources;

        private PageMode mode
        {
            get
            {
                if (this._mode == null) 
                    this._mode = GetMode();
                
                return (PageMode)_mode;
            }
        }

        private PageMode GetMode()
        {
            int? id = QueryStringHelper.Instance.GetParmIntOrNull(QueryParmCvm.id);
            
            if (id == null || id == 0) 
                return PageMode.New;
            else 
                return QueryStringHelper.Instance.GetMode();
        }

        private const int TAB_WIDTH = 100;
        private const bool CREATE_ON_EVERY_CALL = true;

        private const int NEW_MODE_TAB = 0;

        protected override void OnPreInit(EventArgs e)
        {
            bool filterSysId = true;

            if (GetTabIndex() == EditCvTab.GrantedSites)
                filterSysId = false;

            sysId = ContextObjectHelper.FindExplicitSysIdForCurrentRequestOrNull();

            if (sysId != null)
                ContextObjectHelper.CurrentSysId.OverrideObject(sysId);

            MasterPageHelper.Instance.OnPageInit(filterSysId, true);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CvmFacade.Security.VerifyAccess(this.req.Current, AccessMode.readwrite);

            ContextObjectHelper.CurrentBusinessObject = req.Current;
            EditCvTab index = GetTabIndex();
            BuildTabs((EditCvTab) index);

            CvmFacade.Security.VerifyAccessToEditCvTabsSecureTabs(this.req.Current, index);

            //Build up main form
            innerCtrl = GetControlByTabIndex(index);
            innerCtrl.BuildForm();
            this.MainPanel.Controls.Add((Control)innerCtrl);

            //Build up resource drop down
            ResourceDropDown.DataSource = GetResourcesForDropDown();
            ResourceDropDown.DataBind();

            if (!IsPostBack)
            {
                this.innerCtrl.PopulateFront();
                if (resourceId != 0) 
                    this.ResourceDropDown.SelectedLong = resourceId;
                else 
                    this.TopPanel.Visible = false;
            }

            if (index != EditCvTab.BaseData && index != EditCvTab.SysData)
            { 
                this.SaveBtn.Visible = false;
                this.CancelBtn.Visible = false;
            }
            
            this.EditPrintLink.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ContextObjectHelper.History.GetObject().Add(PageNavigation.GetCurrentLink().IncludeExistingParms(), this.req.Current.FullName);

            if (Utl.Query.HasParm(QueryParmCvm.jobId))
            {
                long jobId = (long)Utl.Query.GetParmLongOrNull(QueryParmCvm.jobId);
                bool alreadyApplied = false;

                // Check if user already has applied for this job
                IList<JobResourceRelation> sql = QueryMgrDynamicHql.Instance.GetJobResourceRelationByResourceId(this.resourceId);

                for (int i = 0; i < sql.Count; i++)
                {
                    if (jobId == sql[i].JobId)
                    {
                        alreadyApplied = true;
                    }
                }

                if (alreadyApplied)
                {
                    LblPopup.Text = Utl.Content("EditCv.AlreadyAppliedForThisJob");
                }
                else
                {
                    JobResourceRelation jrr = new JobResourceRelation();
                    jrr.ResourceId = req.Current.ResourceId;
                    jrr.JobId = (long)jobId;
                    jrr.SysId = sysId.SysIdInt;

                    HibernateMgr.Current.SaveOrUpdate(jrr);

                    LblPopup.Text = Utl.Content("EditCv.YourApplicationHasBeenRegistered");
                }

                MPEUserInfo.Show();
            }
        }

        public static Resource ResourceCreator(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                long resourceId = long.Parse(args[0]);
                return HibernateMgr.Current.GetById<Resource>(resourceId);
            }
            else
            {
                return ResourceMgr.Instance.CreateResourceWithSys();
            }
        }

        private IEnumerable<IdTitlePair> GetResourcesForDropDown()
        {
            if (_allResources == null) 
                this._allResources = CvmFacade.EditCv.GetResourcesForDropDown();
            
            return _allResources;
        }

        private IAutoBuildPopulateWithSourceCtrl GetControlByTabIndex(EditCvTab tabIndex)
        {
            switch (tabIndex)
            {
                case EditCvTab.MySite:
                    IAutoBuildPopulateWithSourceCtrl meeSiteCtrl = (IAutoBuildPopulateWithSourceCtrl)this.LoadControl(CommonCtrls.MeeSiteCtrl);
                    meeSiteCtrl.ObjectSource = GetCurrentSource;
                    
                    return meeSiteCtrl;

                case EditCvTab.BaseData:
                    //Stamdata
                    AutoFormExt2 form = new AutoFormExt2();
                    form.ObjectSource = GetCurrentSource;
                    form.IncludeDeleteLink = false;
                    form.OmitProperties = Tables.Resource.AddLanguageId.AddCvFileRefId.AddDiscAdminPrc.AddDiscEntrPrc.AddDiscIntegratorPrc.AddDiscProducerPrc.SelectedColumnsStr();
                    
                    return form;

                case EditCvTab.SysData:
                    //Sys-data
                    AutoFormExt2 form2 = new AutoFormExt2();
                    form2.ObjectSource = () => this.req.Current.SysResourceContext;
            
                    if (ContextObjectHelper.IsCurrentSysType(SysRootTypeEnum.HumanResource))
                    {
                        form2.OmitProperties =
                            Tables.SysResource.AddPrice.AddExpectExtension.AddCommercialMarketId.AddContractDuration.
                                AddSocialProfile.SelectedColumnsStr();
                    }
                    
                    return form2;
            
                case EditCvTab.LanguageSkills:
                    IAutoBuildPopulateWithSourceCtrl editLanguageCtrl = (IAutoBuildPopulateWithSourceCtrl)this.LoadControl(CommonCtrls.EditLanguageSkillsCtrl);
                    editLanguageCtrl.ObjectSource = GetCurrentSource;
                    
                    return editLanguageCtrl;
                
                case EditCvTab.Skills:
                    IAutoBuildPopulateWithSourceCtrl editSkillCtrl = (IAutoBuildPopulateWithSourceCtrl)this.LoadControl(CommonCtrls.EditCvSkillsCtrl);
                    editSkillCtrl.ObjectSource = GetCurrentSource;
                    
                    return editSkillCtrl;
                
                case EditCvTab.Projects:
                    List<Project> projects = (List<Project>)this.req.Current.GetProjectsAsList();
                    projects.Sort(SortMgr.projectSorterByStartDate);
                    
                    return GetListEditCtrl<Project>(() => projects, GetListItem<Project>, "", Tables.Project.AddCustomerId.SelectedColumnsStr());
                
                case EditCvTab.Education:
                    return GetListEditCtrl<Education>(this.req.Current.GetEducationsAsList, GetListItem<Education>);
                
                case EditCvTab.Certifications:
                    return GetListEditCtrl<Certification>(this.req.Current.GetCertificationsAsList, GetListItem<Certification>);
                
                case EditCvTab.Merits:
                    return GetListEditCtrl<Merit>(this.req.Current.GetMeritsAsList, GetListItem<Merit>);
                
                case EditCvTab.Import:
                    IAutoBuildPopulateWithSourceCtrl importCtrl = (ImportCvDataCtrl)this.LoadControl(CommonCtrls.ImportCvDataCtrl);
                    importCtrl.ObjectSource = GetCurrentSource;
                    
                    return importCtrl;
                
                case EditCvTab.DiscProfile:
                    IAutoBuildPopulateWithSourceCtrl discProfileCtrl = (DiscProfileCtrl)this.LoadControl(CommonCtrls.DiscProfileCtrl);
                    discProfileCtrl.ObjectSource = GetCurrentSource;
                    
                    return discProfileCtrl;
                
                case EditCvTab.GrantedSites:
                    IAutoBuildPopulateWithSourceCtrl grantedSitesCtrl = (IAutoBuildPopulateWithSourceCtrl)this.LoadControl(CommonCtrls.GrantedSitesCtrl);
                    grantedSitesCtrl.ObjectSource = GetCurrentSource;
                    
                    return grantedSitesCtrl;

                case EditCvTab.JobSite:
                    IAutoBuildPopulateWithSourceCtrl jobSiteCtrl = (IAutoBuildPopulateWithSourceCtrl)this.LoadControl(CommonCtrls.JobSiteCtrl);
                    jobSiteCtrl.ObjectSource = GetCurrentSource;

                    return jobSiteCtrl;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Generically edit lists of objects. 
        /// </summary>
        /// <returns></returns>
        private IAutoBuildPopulateWithSourceCtrl GetListEditCtrl<T>(ObjectSource list, ObjectSource itemSource, String omitFields = "", String omitFieldsReadMode = null) where T : IBusinessObject
        {
            if (omitFieldsReadMode == null) 
                omitFieldsReadMode = omitFields;
            
            PageMode pageMode = GetPageMode();
            
            if (pageMode == PageMode.AddListItem || pageMode == PageMode.Update)
            {
                EditObjectFromListCtrl<T> form2 = new EditObjectFromListCtrl<T>();
                form2.ObjectSource = itemSource;
                form2.IsCreateNewMode = (pageMode == PageMode.AddListItem);
                form2.OnReturn += ChangeToListMode;
                form2.ListOwnerObject = this.req.Current;
                form2.OmitProperties = omitFields + ";" + OMIT_PROPERTIES;
            
                return form2;
            }
            else
            {
                EditListCtrl<T> form2 = new EditListCtrl<T>();
                form2.ObjectSource = list;
                form2.OmitProperties = omitFieldsReadMode + ";" + OMIT_PROPERTIES;
                
                return form2;
            }
        }

        private PageMode GetPageMode()
        {
            return QueryStringHelper.Instance.GetMode();
        }

        private void ChangeToListMode()
        {
            PageNavigation.GetCurrentLink().IncludeExistingParms().SetMode(PageMode.List).Redirect();
        }

        /// <summary>
        /// Generic getter to be used as the data-source for add og update operations.
        /// </summary>
        /// <returns></returns>
        private T GetListItem<T>() where T : IHasRelatedResource
        {
            if (mode == PageMode.AddListItem)
            {
                if (this.listItem == null || CREATE_ON_EVERY_CALL)
                {
                    T itemTemp = (T)typeof(T).GetConstructor(new Type[0]).Invoke(new object[0]);
                    itemTemp.RelatedResourceObj = this.req.Current;
                    this.listItem = itemTemp;
                }
            }
            else if (mode == PageMode.Update)
            {
                if (this.listItem == null)
                {
                    long listItemId = long.Parse(QueryStringHelper.Instance.GetParmOrFail(QueryParmCvm.listItemId));
                    this.listItem = HibernateMgr.Current.LoadById(typeof(T), listItemId);
                }
            }
            return (T)this.listItem;
        }
        
        protected EditCvTab GetTabIndex()
        {
            return (EditCvTab) QueryStringHelperCvm.Instance.GetParmOrDefault(QueryParmCvm.index, 0);
        }

        private object GetCurrentSource()
        {
            return this.req.Current;
        }

        private void BuildTabs(EditCvTab chosenIndex)
        {
            //If mode new only show first tab.
            if (mode == PageMode.New)
            {
                BuildTabsUtil(NEW_MODE_TAB, chosenIndex);
            }
            else
            {
                BuildTabsUtil(EditCvTab.MySite, chosenIndex);

                BuildTabsUtil(EditCvTab.BaseData, chosenIndex);
                
                if (ContextObjectHelper.CurrentUserHasAnyRole(RoleSet.SalesMgrAtLeast)) 
                    BuildTabsUtil(EditCvTab.SysData, chosenIndex);
                
                BuildTabsUtil(EditCvTab.Skills, chosenIndex);
                BuildTabsUtil(EditCvTab.Projects, chosenIndex);

                BuildTabsUtil(EditCvTab.LanguageSkills, chosenIndex);
                BuildTabsUtil(EditCvTab.Education, chosenIndex);
                BuildTabsUtil(EditCvTab.Certifications, chosenIndex);
                BuildTabsUtil(EditCvTab.Merits, chosenIndex);
                BuildTabsUtil(EditCvTab.Import, chosenIndex);
                BuildTabsUtil(EditCvTab.DiscProfile, chosenIndex);
                
                if (Utl.IsResourceOwner(req.Current)) 
                    BuildTabsUtil(EditCvTab.GrantedSites, chosenIndex);

                BuildTabsUtil(EditCvTab.JobSite, chosenIndex);
            }
        }

        private void BuildTabsUtil(EditCvTab index, EditCvTab chosenIndex)
        {
            TableCell cell = new TableCell();
            cell.Width = TAB_WIDTH;
            EditCvTab tab = index;
            string contentId = "EditCv." + tab;
            cell.Style.Add(HtmlTextWriterStyle.BorderCollapse, "separate");

            AdmHyperLink link = new AdmHyperLink();
            link.ContentId = contentId;
            link.Attributes.Add("nonActiveLink", "");
            link.PageLink = GetLinkForThisPage();
            link.PageLink.SetParm(QueryParmCvm.index, (int)index);
            link.PageLink.IncludeParm(QueryParmCvm.skillTypes);
            link.PageLink.IncludeParm(QueryParmCvm.profileTypeId);

            if ((int)index == 0)
            {
                link.PageLink.ExcludeParm(QueryParmCommon.mode).ExcludeParm(QueryParmCvm.listItemId).ExcludeParm(QueryParmCvm.clientId);
            }
            else
                link.PageLink.SetMode(PageMode.List);

            cell.Controls.Add(link);

            cell.CssClass = (index == chosenIndex ? "tabChosen" : "tab");
            this.Tabs.Cells.Add(cell);
        }

        protected void OnClick_SaveBtn(object sender, EventArgs e)
        {
            DoSave();
        }

        private void DoSave()
        {
            innerCtrl.PopulateBack();

            if (innerCtrl is IControlWithSave)
            {
                ((IControlWithSave)innerCtrl).OnSave();
            }
            else
            {
                Resource resource = req.Current;
        
                if (!resource.IsPersisted())
                {
                    HibernateMgr.Current.SaveOrUpdate(resource.RelatedUserObjObj);
                    HibernateMgr.Current.SaveOrUpdate(resource);
                    HibernateMgr.Current.SaveOrUpdate(resource.SysResourceContext);
                }
            
                resource.SynchDenormalizedData();
                this.innerCtrl.PopulateFront();
                
                Debug.Assert(resource.Idfr.IdfrAsLong() != 0, "The Id of the object should not be 0 after creation or update.");
                
                if (resource.Idfr.IdfrAsLong() != QueryStringHelper.Instance.GetParmIntOrNull(QueryParmCvm.id))
                {
                    GetLinkForThisPage().SetParm(QueryParmCvm.id, resource.Idfr.IdfrAsLong()).Redirect();
                }
            }
        }

        protected void OnSelectResource(object sender, EventArgs e)
        {
            GetLinkForThisPage().IncludeParm(QueryParmCvm.index).SetParm(QueryParmCvm.id, (long)this.ResourceDropDown.SelectedInt).Redirect();
        }

        private PageLink GetLinkForThisPage()
        {
            return CvmPages.EditCvPage.IncludeParm(QueryParmCvm.id).IncludeParm(QueryParmCvm.isPopup);
        }

        protected void OnClick_CancelBtn(object sender, EventArgs e)
        {
            if (innerCtrl is IControlWithCancel)
            {
                ((IControlWithCancel)innerCtrl).OnCancel();
            }
            else
            {
                MessageManager.Current.PostMessage("Standard.NothingSaved");
                GetLinkForThisPage().Redirect();
            }
        }

        private string GetCvLink()
        {
            return PageFunctions.GetCvLink(this.req.Current);
        }

        protected void OnClickCreateResourceUser(object sender, EventArgs e)
        {
            DoSave();
            CvmFacade.UserAdmin.CreateNewMemberhipUserOrFail(this.req.Current.RelatedUserObjObj);
            this.CreateResourceButton.DataBind();
            this.LinkToUserButton.DataBind();
        }

        protected void OnClickInviteUser(object sender, EventArgs e)
        {
            CvmFacade.UserAdmin.InviteUser(this.req.Current.RelatedUserObjObj);
        }

        protected void OnClickSimulateUser(object sender, EventArgs e)
        {
            CvmFacade.Security.SimulateUser(this.req.Current.RelatedUserObjObj);
        }

        protected long GetPreviousResourceId()
        {
            EnsurePreviousNextFound();
            return long.Parse(_previousId);
        }

        protected long GetNextResourceId()
        {
            EnsurePreviousNextFound();
            return long.Parse(_nextId);
        }

        private void EnsurePreviousNextFound()
        {
            if (_nextId == null)
            {
                if (this.resourceId > 0)
                {
                    String current = this.resourceId + "";
                    bool didFind = false;
                
                    foreach (var r in this.GetResourcesForDropDown())
                    {
                        if (didFind)
                        {
                            _nextId = r.Id;
                            break;
                        }
                        else if (current.Equals(r.Id))
                        {
                            didFind = true;
                        }
                        else
                        {
                            _previousId = r.Id;
                        }
                    }
                    
                    if (_nextId == null) 
                        _nextId = "0";
                    
                    if (_previousId == null) 
                        _previousId = "0";
                }
            }
        }
    }
}