using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Backend.Business.Skills;
using Cvm.Web.Facade;
using Napp.Backend.Hibernate;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class SearchCvCtrl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Used to seperate ID's in a compact string format.
        /// We use ;
        /// </summary>
        private const char IdSeperator = ',';
        private IList<Skill> _relevantSkills;
        private HashSet<long> _hasSelected;
        protected ResourceSearch SearchObj { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.MinimumSkillLevel2.EnumType = typeof (SkillLevelEnum);
            this.MinimumSkillLevel2.Required = true;
        }

        protected override void OnPreRender(EventArgs e)
        {

            this.bar.ChosenIndex = this.SearchWiz.ActiveStepIndex;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                this.SkillFilterTextBox.Text=ListFilterCtrl.DefaultFilterText;
            }
            
        }

        protected IEnumerable<String>  GetBarTitles()
        {
            foreach(var s in this.SearchWiz.WizardSteps)
            {
                yield return ((WizardStep) s).Title;
            }
        }


        protected void OnFilterChanged(object sender, EventArgs e)
        {
            PopulateSkillLists();
        }

        protected void OnActivateDoSearchStep(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.SkillsHiddenField.Value))
            {
                Utl.Msg.PostMessage("SearchCvCtrl.MustSelectSkills");
            }
            else
            {
                IdfrStringList idfrs = new IdfrStringList(this.SkillsHiddenField.Value);
                if (!idfrs.IsEmpty())
                {
                    this.ResourceResultGrid.ResourceList = CvmFacade.Search.SearchCvBySkills(idfrs, OmitResources);
                } else
                {
                    Utl.Msg.PostMessage("SearchCvCtrl.NoResourceFound");
                }
            }

        }

        protected void OnActivateChooseSkills(object sender, EventArgs e)
        {
            PopulateSkillLists();
        }

        private void PopulateSkillLists()
        {
            IEnumerable<Skill> selectedSkills = RelevantSkills.Where(s => HasSelected(s.SkillId)).OrderBy(s=>s.SkillName);
            IEnumerable<Skill> unselectedSkills = RelevantSkills.OrderBy(s=>s.ExtendedObjectTitle);
            if (HasFilterString(this.SkillFilterTextBox.Text)) {
                unselectedSkills = unselectedSkills.Where(s => s.ExtendedObjectTitle.ToLower().Contains(this.SkillFilterTextBox.Text));
            }
            this.SkillsRep.DataSource = unselectedSkills;
            this.SelectedSkillsRep.DataSource = selectedSkills;
            this.SkillsRep.DataBind();
            this.SelectedSkillsRep.DataBind();
        }

        /// <summary>
        /// Determines whether the filter string is meant to be used as a filter.
        /// </summary>
        /// <param name="filterStr"></param>
        /// <returns></returns>
        private bool HasFilterString(string filterStr)
        {
            return !String.IsNullOrEmpty(filterStr) && filterStr.Trim().Length != 0 && !ListFilterCtrl.DefaultFilterText.Equals(filterStr.Trim());
        }

        /// <summary>
        /// Determines whether the given skillId has been selected.
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        protected bool HasSelected(long skillId)
        {
            if (this._hasSelected == null)
            {
                this._hasSelected = new HashSet<long>();
                String[] skillIds = this.SkillsHiddenField.Value.Split(IdSeperator);
                foreach (String id in skillIds)
                {
                    string trim = id.Trim();
                    if (trim.Length > 0)
                    {
                        this._hasSelected.Add(long.Parse(trim));
                    }
                }
            }
            return _hasSelected.Contains(skillId);
        }

        protected IList<Skill> RelevantSkills
        {
            get
            {
                if (this._relevantSkills==null)
                {
                    long profileTypeIds = this.ProfileTypeIdsCheckList2.GetSelectedInt();
                    this._relevantSkills =
                        QueryMgr.instance.FindRelevantSkills(ContextObjectHelper.CurrentSysIdValOrFail, profileTypeIds).ListOrNull(HibernateMgr.Current.Session);
                }
                return this._relevantSkills;
            }
        }

        protected void OnActivateFinish(object sender, EventArgs e)
        {
            if (OnResourcesSelected == null)
            {
                this.SummaryRep.DataSource = GetFinallySelectedResources();
                this.SummaryRep.DataBind();
            } else
            {
                OnResourcesSelected(GetFinallySelectedResources());
                this.SummaryPanel.Visible = false;
            }
        }

        /// <summary>
        /// Assign a delegate to handle the resources selected by this search flow.
        /// </summary>
        public Action<IList<Resource>> OnResourcesSelected;

        public IdfrStringList OmitResources { get; set; }

        /// <summary>
        /// Returns the resources selected by this search flow.
        /// </summary>
        /// <returns></returns>
        private IList<Resource> GetFinallySelectedResources()
        {
            return CvmFacade.Search.GetResourcesByIds(this.ResourceResultGrid.ChosenResourceIds);
        }

        protected void OnClickStartOverBtn(object sender, EventArgs e)
        {
            this.SearchWiz.ActiveStepIndex = 0;
            this.SkillsHiddenField.Value = "";
            this.ResourceResultGrid.ClearSelection();
        }
    }
}