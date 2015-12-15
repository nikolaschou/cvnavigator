using System;
using System.Collections.Generic;
using System.Linq;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Util;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Web.Auto;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class EditCvSkillsCtrl : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl
    {
        private const string ADD_SKILL_MODE = "addskill";
        private ObjectSource _objectSource;
        private Resource _res;
        private List<ResourceSkill> _mySkills;
        protected long CHOOSE_ALL_ASSIGNED = 0;
        private List<ProfileType> _assignedProfileTypes;
        private long? _currentProfileTypeId;
        
        protected override void OnInit(EventArgs e)
        {   
        }

        protected List<ProfileType> GetAssignedProfileTypes()
        {
           if (_assignedProfileTypes == null)
           {
               if (!ContextObjectHelper.CurrentSysIdIsSpecified()) 
                   _assignedProfileTypes = new List<ProfileType>(0);
               else  
                   _assignedProfileTypes = this.GetResource().SysResourceContext.GetAssignedProfileTypes();
            }
            
            return _assignedProfileTypes;
        }

        public void PopulateFront()
        {
            //Do nothing, each control knows how to edit itself.
        }

        public void PopulateBack()
        {
            //Do nothing, each control knows how to edit itself.
        }

        public ObjectSource ObjectSource
        {
            get 
            { 
                return _objectSource; 
            }
            set 
            { 
                _objectSource = value; 
            }
        }

        public void BuildForm()
        {
            if (ContextObjectHelper.CurrentSysIdIsSpecified())
            {
                if (this.GetAssignedProfileTypes().Count > 0)
                {
                    this.ProfileTypePanel.Visible = true;
                    this.ProfileRep.DataBind();
                } 
            }
            
            BuildSkillTable();
        }

        private void BuildSkillTable()
        {
            IList<ResourceSkill> skills = MyResourceSkills;

            int counter = 0;
         
            if (skills != null && skills.Count > 0)
            {
                foreach (ResourceSkill skill in skills)
                {
                    EditSkillCtrl ctrl = (EditSkillCtrl)this.LoadControl(CommonCtrls.EditSkillCtrl);
                    ctrl.ID = "SkillCtrl" + counter;
                    counter++;
                    ctrl.mySkill = skill;
                    this.SkillsPanel.Controls.Add(ctrl);
                    ctrl.PopulateFront();
                    ctrl.DoEnable(true);
                    this.OuterPanel.Visible = true;
                }
            }
            else
            {
                this.OuterPanel.Visible = false;
            }
        }

        protected IList<ResourceSkill> MyResourceSkills
        {
            get
            {
                if (_mySkills == null)
                {
                    long? skillTypes = GetSkillTypesToBuild();
            
                    if (ConfigMgr.IncludeSkillCategory) 
                        SkillTypesFilterDropDown.SelectedLong = skillTypes;

                    Resource resource = GetResource();
                    
                    SkillTypeEnum skillTypesEnum = skillTypes != null
                                                       ? (SkillTypeEnum) skillTypes
                                                       : (SkillTypeEnum) BitPatternConst.All;
                    
                    bool includeAllAssignedSkills = GetCurrentProfileTypeId()==CHOOSE_ALL_ASSIGNED;
                    long profileTypeIds;
                    
                    if (includeAllAssignedSkills)
                    {
                        profileTypeIds = BitPatternConst.All;
                    }
                    else
                    {
                        profileTypeIds = GetCurrentProfileTypeId();
                    }
                    
                    _mySkills = new List<ResourceSkill>(EditCvFacade.instance.GetResourceSkillTable(resource, skillTypesEnum, profileTypeIds, includeAllAssignedSkills));
                }

                return _mySkills;
            }
        }

        protected long GetCurrentProfileTypeId()
        {
            if (_currentProfileTypeId == null)
            {
                if (!ContextObjectHelper.CurrentSysIdIsSpecified()) 
                    this._currentProfileTypeId = CHOOSE_ALL_ASSIGNED;
                else
                {
                    string pid = Utl.Query.GetParmOrNull(QueryParmCvm.profileTypeId);
                    if (pid == null)
                    {
                        if (GetAssignedProfileTypes().Count > 0)
                        {
                            _currentProfileTypeId = GetAssignedProfileTypes()[0].ProfileTypeId;
                        }
                        else
                        {
                            _currentProfileTypeId = CHOOSE_ALL_ASSIGNED;
                        }
                    }
                    else
                    {
                        long pidL = long.Parse(pid);

                        if (GetAssignedProfileTypes().Any(p=>p.ProfileTypeId == pidL))
                        {
                            _currentProfileTypeId = pidL;
                        } 
                        else
                        {
                            _currentProfileTypeId = CHOOSE_ALL_ASSIGNED;
                        }
                    }
                }
            }
            
            return (long)_currentProfileTypeId;
        }

        private static long? GetSkillTypesToBuild()
        {
            long? skillTypes = QueryStringHelper.Instance.GetParmIntOrNull(QueryParmCvm.skillTypes);
            /*if (skillTypes==null)
            {
                skillTypes = QueryMgrDynamicHql.Instance.GetFirstSkillTypeId(this.res.SysIdObj);
                
            }*/
            return skillTypes;
        }


        private Resource GetResource()
        {
            if (_res == null) 
                _res = _objectSource() as Resource;
            
            return _res;
        }

        protected void OnSelectSkillTypes(object sender, EventArgs e)
        {
            long selectedSkillType = (this.SkillTypesFilterDropDown.SelectedLong ?? BitPatternConst.All);
            PageNavigation.GetCurrentLink().IncludeExistingParms().SetParm(QueryParmCvm.skillTypes, selectedSkillType).Redirect();
        }

        protected void OnClick_SaveBtn(object sender, EventArgs e)
        {
            SaveAll();
        }

        private void SaveAll()
        {
            foreach (var ctrl in SkillsPanel.Controls)
            {
                EditSkillCtrl ctrl2 = ctrl as EditSkillCtrl;
            
                if (ctrl2 != null) 
                    ctrl2.Save();
            }
            
            PageNavigation.GotoCurrentPageAgainWithParms();
        }

        protected void OnClick_CancelBtn(object sender, EventArgs e)
        {
            PageNavigation.GetCurrentLink().IncludeParm("id").Redirect();
        }

        protected void OnClickAddSkillBtn(object sender, EventArgs e)
        {
            string skillName = this.AddSkillTextBox.Text;
            string skillNameLower = skillName.ToLower();
            if (this.MyResourceSkills.Any(rs => rs.SkillName.ToLower().Equals(skillNameLower)))
            {
                Utl.Msg.PostMessage("EditCvSkillsCtrl.SkillAlreadyAssigned");
            }
            else
            {
                Skill s = CvmFacade.Skills.GetOrCreateSkill(skillName);

                if (s != null)
                {
                    CvmFacade.Skills.EnsureResourceSkill(s, this.GetResource());
                    PageNavigation.GetCurrentLink().IncludeExistingParms().SetParm(QueryParmCommon.submode, ADD_SKILL_MODE).Redirect();
                }
            }
        }

        protected void OnClickAdjustAllSkills(object sender, EventArgs e)
        {
            foreach(var s in this.MyResourceSkills)
            {
                bool didChange=s.AdjustYearsAccordingToProjectQualification();

                if (didChange)
                {
                    Utl.Msg.PostMessage("EditCvSkillsCtrl.AdjustedSkill",s.SkillName);
                }
            }
            
            PageNavigation.GotoCurrentPageAgainWithParms();
        }

        /// <summary>
        /// Determines whether at least one skill is unqualified.
        /// </summary>
        /// <returns></returns>
        protected bool HasUnqualifiedSkills()
        {
            foreach(var s in MyResourceSkills)
            {
                if (!s.IsAlignedWithProjectExperience()) 
                    return true;
            }

            return false;
        }
    }
}