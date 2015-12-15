using System;
using System.Collections;
using System.Collections.Generic;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Localization;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Util;
using Cvm.Web.Code;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class EditLanguageSkillsCtrl : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl, IControlWithSave
    {
        private ObjectSource objectSource;
        private bool didInit = false;

        public void PopulateFront()
        {
            this.MotherToungeDropDown.SelectedLong = MyResource.LanguageId;
            this.AutoList1.PopulateFront();
        }

        public void PopulateBack()
        {
            MyResource.LanguageId = this.MotherToungeDropDown.SelectedLong;
            this.AutoList1.PopulateBack();
        }

        private Resource MyResource
        {
            get
            {
                return (Resource) this.objectSource();
            }
        }

        public ObjectSource ObjectSource
        {
            get 
            { 
                return objectSource; 
            }
            set 
            { 
                this.objectSource = value;
                this.AutoList1.ObjectSource = delegate { return MyResource.LanguageSkills; };
            }
        }

        public void BuildForm()
        {
            if (!this.didInit)
            {
                this.AutoList1.LinkButtonInclude = true;
                this.AutoList1.LinkButtonText = AdminContentMgr.instance.GetContent("EditLanguageSkillsCtrl.Remove");
                this.AutoList1.LinkClicked += RemoveLanguage;
                this.didInit = true;
            }
            
            SetupSecondLangDropDown();
            this.AutoList1.BuildForm();
        }

        private void RemoveLanguage(object eventData)
        {
            LanguageSkill lang = (LanguageSkill) eventData;
            MyResource.LanguageSkills.Remove(lang); 
            HibernateMgr.Current.Delete(lang);
            this.BuildForm();
            this.PopulateFront();
        }

        /// <summary>
        /// Populates the second language drop down with already contained languages filtered out.
        /// </summary>
        private void SetupSecondLangDropDown()
        {
            bool b = HibernateMgr.Current.IsSysIdFilterEnabled();
            IList<Language> langs = QueryMgr.instance.GetAllLanguage().ListOrNull(HibernateMgr.Current.Session);
            
            if (MyResource.RelatedLanguageObj != null) 
                langs.Remove(MyResource.RelatedLanguageObj);
            
            foreach (LanguageSkill s in MyResource.LanguageSkills)
            {
                langs.Remove(s.RelatedLanguageObj);
            }
            
            if (langs.Count == 0) 
                this.SecondLangDropDown.Visible = false; 
            else
            {
                List<Language> langs2 = new List<Language>(langs);
                langs2.Sort();
                this.SecondLangDropDown.DataSource = langs2;
                this.SecondLangDropDown.DataBind();
            }
        }

        protected void btnUpdateSecondLanguageSkills_Click(object sender, EventArgs e)
        {
            PopulateBack();
        }

        protected void OnMotherTongueChosen(object sender, EventArgs e)
        {
            PopulateBack();
        }
    
        protected void OnSecondLanguageChosen(object sender, EventArgs e)
        {
            LanguageSkill skill = new LanguageSkill();
            skill.LanguageId = (long)this.SecondLangDropDown.SelectedLong;
            skill.ResourceId = MyResource.ResourceId;
            LanguageSpeakingLevel speakLevel = QueryMgr.instance.GetLanguageSpeakingLevelByIdOrNull(BitPatternConst.First);
            LanguageWritingLevel writeLevel = QueryMgr.instance.GetLanguageWritingLevelByIdOrNull(BitPatternConst.First);
            
            if (speakLevel == null || writeLevel == null)
            {
                MessageManager.Current.PostMessage("EditLanguageSkillsCtrl.MustCreateLanguageLevels");
            } 
            else
            {
                skill.RelatedLanguageSpeakingLevelObj = speakLevel;
                skill.RelatedLanguageWritingLevelObj = writeLevel;
                MyResource.LanguageSkills.Add(skill);
                HibernateMgr.Current.Save(skill);
                this.BuildForm();
                this.PopulateFront();
            }
        }

        public void OnSave()
        {
            PopulateBack();
            ResourceMgr.Instance.ValidateLanguageSkills(MyResource);
            this.BuildForm();
            this.PopulateFront();
            //The rest will be saved automatically as added language skills
            //are initially saved when first added.
        }

    }
}