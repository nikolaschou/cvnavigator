using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Web.Facade;
using Napp.Common.MessageManager;
using Napp.Web.Auto;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class ImportSkillsCtrl : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl
    {
        public Resource MyResource
        {
            get 
            {
                return (Resource) Source();
            }
        }

        private ObjectSource Source;
        private IList<SkillMatchWrapper> skills;
        
        protected IList<SkillMatchWrapper> GetPotentialImportSkills(bool usePartialMatches)
        {
            return CvmFacade.ImportSkills.GetPotentialImportSkills(MyResource, usePartialMatches).GetIList();
        }

        /// <summary>
        /// Determines whether MyResource already has this skill
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        protected bool HasSkillAlready(Skill skill)
        {
            return MyResource.HasSkill(skill);
        }

        protected void OnClickDoImportBtn(object sender, EventArgs e)
        {
            var skills = GetSkillsToAdd();
            skills = CvmFacade.ImportSkills.PrepareSkills(skills);
            
            foreach(var skill in skills)
            {
                bool didAssign = CvmFacade.ImportSkills.AssignSkill(MyResource, skill);
                if (didAssign)
                {
                    MessageManager.Current.PostMessage("ImportSkillsCtrl.ImportedSkill", skill.SkillName);
                }
            }
        }

        private IEnumerable<Skill> GetSkillsToAdd()
        {
            foreach (RepeaterItem repeaterItem in this.SkillRep.Items)
            {
                CheckBox check = (CheckBox) repeaterItem.FindControl("DoImportCheckBox");

                if (check.Checked)
                {
                    check.Enabled = false;
                    SkillMatchWrapper skillMatch = skills[repeaterItem.ItemIndex];
                    Skill skill = skillMatch.Skill;
                    yield return skill;
                }
            }
        }
        
        public void PopulateFront()
        {
            //Do nothing
        }

        public void PopulateBack()
        {
            //Do nothing
        }

        public ObjectSource ObjectSource
        {
            get 
            { 
                return Source; 
            }
            set 
            { 
                this.Source = value;
            }
        }

        public void BuildForm()
        {
            if (MyResource.HasImportText())
            {
                this.SkillRep.Controls.Clear();
            
                if (this.MatchedSkills.Count == 0)
                {
                    HideCtrl();
                }
                else
                {
                    this.MainPanel.Visible = true;
                    this.SkillRep.DataSource = MatchedSkills;
                    this.SkillRep.Controls.Clear();
                    MyResource.ClearCachedSkillIds();
                    this.DataBind();
                }
            } 
            else
            {
                HideCtrl();
            }
        }

        private void HideCtrl()
        {
            this.MainPanel.Visible = false;
            this.MsgLit.Text = Utl.Content("ImportSkillsCtrl.ImportDataBeforeExtract");
        }

        private IList<SkillMatchWrapper> MatchedSkills
        {
            get
            {
                if (skills == null)
                {
                    skills = this.GetPotentialImportSkills(this.UsePartialMatchesCheckBox.Checked);
                }
            
                return skills;
            }
        }
    }
}