using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Skills;
using Cvm.Web.Code;
using Cvm.Web.CommonCtrl;
using Cvm.Web.Facade;
using Iesi.Collections.Generic;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Backend.Hibernate.DataFetcher;
using Napp.Web.AdmControl;
using Napp.Web.Auto;
using Napp.Web.Auto.Annotations;
using Napp.Web.DialogCtrl;

namespace Cvm.Web.AdminPages.WebFormCtrls
{
    public partial class ProjectSkillsCtrl2 : UserControl, ICompositeWebFormWithParent
    {
        private const int SMALL_NUMBER_OF_CHECKBOXES = 3;
        private Project _project;
        private ObjectSource projectObjectSource;
        /// <summary>
        /// Used for the subdialog for adding skills
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Browser.ToLower().Contains("ie"))
            {
                Utl.Msg.PostMessage("Standard.OptimizedForFirefox");
            }
        }

        public ObjectSource ObjectSource
        {
            set
            {
                //Do nothing
                //We don't need this because this control is specialized for 
                //project skills
            }
        }
        private Project MyProject
        {
            get
            {
                if (_project == null) _project = (Project)projectObjectSource();
                return _project;
            }
        }

        public ObjectSource ParentObjectSource
        {
            set { projectObjectSource = value; }
        }

        public PropertyInfo Property
        {
            set { 
                //Do nothing, we don't need this as this control is specialized 
            //for project skills
            }
        }

        public void BuildForm()
        {
            BuildCheckBoxList();
            if (!this.IsPostBack)
            {
                InitForm();
            }

        }

        /// <summary>
        /// Sets up intial stuff and should be called when a new skill have been created.
        /// </summary>
        private void InitForm()
        {
            this.SkillTextBox2.Text = Utl.Content("ProjectSkillsCtrl2.EnterSkill");
        }

        private void BuildCheckBoxList()
        {
            SortedList<ISkill, bool> skills = GetSkills();
            //this.SkillsCheckBoxList.Controls.Clear();
            List<ISkill> keys = new List<ISkill>(skills.Keys);
            keys.Sort((s,t)=>((IBusinessObject)s).ExtendedObjectTitle.CompareTo(((IBusinessObject)t).ExtendedObjectTitle));
            this.SkillsCheckBoxList.DataSource = keys;
            this.SkillsCheckBoxList.DataBind();
            bool showHelperBtns = keys.Count > SMALL_NUMBER_OF_CHECKBOXES;
            this.MarkAllBtn.Visible = showHelperBtns;
            this.MarkNoneBtn.Visible = showHelperBtns;
        }

        private void SetupSkillSelectList()
        {
            IList<Skill> skillsList = QueryMgrDynamicHql.Instance.GetSkillByCategory(null).ListOrNull(HibernateMgr.Current.Session);
            List<Skill> filteredSkills = FilterExistingSkills(skillsList, MyProject.RelatedResourceObj.ResourceSkills);
            filteredSkills.Sort((a, b) => String.Compare(a.ExtendedObjectTitle,
                                                         b.ExtendedObjectTitle));
            this.SkillListBox.DataTextField = "ExtendedObjectTitle";
            this.SkillListBox.DataValueField = AdmControlPluginConfig.NameOfIdField;

            this.SkillListBox.DataSource = filteredSkills;
            this.SkillListBox.DataBind();
        }

        /// <summary>
        /// Filters the list of skills such that existings skills are not repeated.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="existingSkills"></param>
        /// <returns></returns>
        private List<Skill> FilterExistingSkills(IList<Skill> list, Iesi.Collections.Generic.ISet<ResourceSkill> existingSkills)
        {
            List<Skill> result=new List<Skill>(list.Count);
            Iesi.Collections.Generic.ISet<long> existingIds = MapToIdSet(existingSkills);
            foreach (Skill s in list)
            {
                if (!existingIds.Contains(s.SkillId))
                {
                    result.Add(s);
                }
            }
            return result;
        }

        private Iesi.Collections.Generic.ISet<long> MapToIdSet(Iesi.Collections.Generic.ISet<ResourceSkill> skills)
        {
            Iesi.Collections.Generic.ISet<long> ids = new HashedSet<long>();
            foreach(ResourceSkill s in skills)
            {
                ids.Add(s.SkillId);
            }
            return ids;
        }

        private SortedList<ISkill, bool> GetSkills()
        {
            return EditCvFacade.instance.GetProjectSkillsAsSubsetOfResourceSkills(MyProject);
            
        }


        public void PopulateBack()
        {
            Dictionary<long, bool> containedSkillIds = MapToSetOfIds(GetSkills());
            MyProject.ClearProjectSkillAdjustments();
            foreach (ListItem item in this.SkillsCheckBoxList.Items)
            {
                long skillId = long.Parse(item.Value);
                if (item.Selected)
                {
                    //If not already contained, add it now
                    if (!containedSkillIds.ContainsKey(skillId))
                    {
                        MyProject.SkillIdsToAdd.Add(skillId);
                    } 
                } else
                {
                    //If contained, remove it
                    if (containedSkillIds.ContainsKey(skillId))
                    {
                        MyProject.SkillIdsToDelete.Add(skillId);
                    }
                }
            }
        }

        public void PopulateFront()
        {
            PopulateSelectedCheckBoxes();
        }

        /// <summary>
        /// Marks selected check boxes
        /// </summary>
        private void PopulateSelectedCheckBoxes()
        {
            SortedList<ISkill, bool> _skills2 = GetSkills();
            Dictionary<long, bool> containedSkillIds = MapToSetOfIds(_skills2);
            foreach (ListItem ctrl in this.SkillsCheckBoxList.Items)
            {
                ctrl.Selected = containedSkillIds.ContainsKey(long.Parse(ctrl.Value));
            }
        }

        /// <summary>
        /// Maps to a set containing only the skillIds contained 
        /// </summary>
        /// <param name="_skills2"></param>
        /// <returns></returns>
        private Dictionary<long, bool> MapToSetOfIds(SortedList<ISkill, bool> _skills2)
        {
            Dictionary<long,bool> containedSkillIds=new Dictionary<long, bool>(_skills2.Count);
            foreach (ISkill s in _skills2.Keys)
            {
                if (_skills2.ContainsKey(s) && _skills2[s])containedSkillIds.Add(s.SkillId,true);
            }
            return containedSkillIds;
        }




        protected void OnClickMarkAllBtn(object sender, EventArgs e)
        {
            SetSelectionMarkForAll(true);
        }

        protected void OnClickMarkNoneBtn(object sender, EventArgs e)
        {
            SetSelectionMarkForAll(false);
        }

        private void SetSelectionMarkForAll(bool mark)
        {
            foreach (ListItem item in this.SkillsCheckBoxList.Items)
            {
                item.Selected = mark;
            }
        }


        protected void OnClickOpenSkillWizardBtn(object sender, EventArgs e)
        {
            this.SetupSkillSelectList();

            this.NewSkillPanel.Visible = true;
            //Must popup dialog window using javascript
            //ScriptManager.RegisterStartupScript(this,typeof(ProjectSkillsCtrl2),"addSkill","addSkill()",true);
        }

        protected void OnClickAddNewSkillBtn(object sender, EventArgs e)
        {
            String newSkill=this.SkillTextBox2.Text;
            if (String.IsNullOrEmpty(this.SkillTextBox2.Text.Trim()))
            {
                Utl.Msg.PostMessage("ProjectSkillsCtrl2.SpecifySkillName");
            }
            else
            {
                var q = from sk in Hiber.Q<Skill>() where sk.SkillName==newSkill select sk;
                if (q.Count() > 0)
                {
                    Utl.Msg.PostMessage("ProjectSkillsCtrl2.SkillAlreadyExists",newSkill);
                }
                else
                {

                    Skill s=CvmFacade.Skills.CreateSkill(newSkill);
                    AddSkillToResource(s);
                    //this.SkillTextBox.Text = "";
                    ReturnToMainForm();
                    MarkSkill(s);
                }
            }
        }

        private void MarkSkill(Skill skill)
        {
            foreach (ListItem item in this.SkillsCheckBoxList.Items)
            {
                if (long.Parse(item.Value).Equals(skill.SkillId))
                {
                    item.Selected = true;
                }
            }
        }

        private void ReturnToMainForm()
        {
            this.BuildForm();
            this.PopulateFront();
            this.NewSkillPanel.Visible = false;
        }

        private void AddSkillToResource(Skill s)
        {
            ResourceSkill rsSkill=new ResourceSkill();
            rsSkill.RelatedSkillObj = s;
            rsSkill.LevelEnum = ConfigMgr.defaultSkillLevel;
            rsSkill.RelatedResourceObj = this.MyProject.RelatedResourceObj;
            HibernateMgr.Current.Save(rsSkill);
            //Add to collections
            this.MyProject.RelatedResourceObj.ResourceSkills.Add(rsSkill);


        }

        protected void OnClickAddSelSkillsBtn(object sender, EventArgs e)
        {
            //Pick out the comma-separated list of skill Ids
            String skillIds = this.SelectedSkillIdsBx.Text;
            IdfrStringList skillIdsObj=new IdfrStringList(skillIds);
            long[] longIdentifiers = skillIdsObj.GetLongIdentifiers();
            List<Skill> addedSkills=new List<Skill>(longIdentifiers.Length);
            foreach (var id in longIdentifiers)
            {
                Skill s=QueryMgr.instance.GetSkillById(id);
                AddSkillToResource(s);
                addedSkills.Add(s);
            }
            ReturnToMainForm();
            foreach(var s in addedSkills)
            {
                MarkSkill(s);
            }
        }

        protected void OnClickAddSkillBtn(object sender, EventArgs e)
        {
            String skillName = this.AddSkillTextBox.Text;
            String skillNameL = skillName.ToLower();
            if (this.MyProject.RelatedResourceObj.ResourceSkills.Any(ps => ps.SkillName.ToLower().Equals(skillNameL)))
            {
                Utl.Msg.PostMessage("ProjectSkillsCtrl2.SkillAlreadyAdded");
            }
            else
            {
                Skill s = CvmFacade.Skills.GetOrCreateSkill(skillName);
                if (s != null)
                {
                    AddSkillToResource(s);
                    ReturnToMainForm();
                    MarkSkill(s);
                }
            }
        }
    }
}