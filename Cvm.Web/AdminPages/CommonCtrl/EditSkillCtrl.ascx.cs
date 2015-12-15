using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Qualification;
using Cvm.Backend.Business.Skills;
using Napp.Backend.Hibernate;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;
using Napp.Web.DialogCtrl;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class EditSkillCtrl : System.Web.UI.UserControl, IAutoPopulateBaseCtrl, IDialogCtrl
    {
        public ResourceSkill mySkill;

        private WebControl[] _inputControls;
        protected CvQualification MyQualification
        {
            get 
            { 
                return mySkill.RelatedResourceObj.GetCvQualification(); 
            }
        }
        /// <summary>
        /// Returns an array with all validators
        /// </summary>
        private BaseValidator[] _validationControls;

        private static readonly Random random = new Random();

        private readonly int rowId = random.Next();
        private bool? _isAlignedWithProjectExperience;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
 
            this.EnableValidationCtrls(false);
        }

        private BaseValidator[] ValidationControls
        {
            get
            {
                if (this._validationControls == null) 
                    _validationControls = new BaseValidator[] { LastUsedValidator,  UsedInTotalValidator,  SkillLevelValidator };
                
                return _validationControls;
            }
        }

        private WebControl[] InputControls
        {
            get
            {
                if (_inputControls == null)
                    _inputControls = new WebControl[] { LastUsedTextBox, UsedInTotalTextBox, SkillLevelDropDown3 };
                
                return _inputControls;
            }
        }

        private void EnableValidationCtrls(bool b)
        {
            foreach(BaseValidator val in this.ValidationControls)
            {
                val.Enabled = b;
            }
        }

        protected String GetLastUsed()
        {
            return GetUtil(mySkill.LastUsed);
        }

        private string GetUtil(int? used)
        {
            return "" + used;
        }

        protected String GetUsedInTotal()
        {
            return GetUtil(mySkill.UsedInTotal);
        }

        private string GetUtil(long? var)
        {
            if (mySkill.IsPersisted())
            {
                return "" + var;
            }
            else 
                return "";
        }


        public ResourceSkill ResourceSkill
        {
            set
            {
                this.mySkill = value;
            }
        }

        public void PopulateFront()
        {
            this.DataBind();
            this.SkillLevelDropDown3.EnumType = typeof (SkillLevelEnum);
            SkillLevelDropDown3.Select(mySkill.Level);
            this.EnableValidationCtrls(false);
        }

        public void PopulateBack()
        {
            mySkill.LastUsed = this.LastUsedTextBox.TextIntOrNull;
            mySkill.UsedInTotal = this.UsedInTotalTextBox.TextIntOrNull;
            //We can safely cast to long as we only get here if all values are specified.
            mySkill.Level = this.SkillLevelDropDown3.GetSelectedInt();
        }

        public void DoEnable(bool enabled)
        {
            foreach (WebControl ctrl in InputControls) 
                ctrl.Enabled = enabled;
        }

        protected void OnClick_Edit(object sender, EventArgs e)
        {
//            this.DoEnable(true);
//            SwapLinkButtons(true);
        }

        protected void OnClick_Save(object sender, EventArgs e)
        {
            this.Save();
            this.DialogConcluded(this.mySkill);
        }

        protected void OnClick_Cancel(object sender, EventArgs e)
        {
            this.DialogConcluded(null);
        }

        /// <summary>
        /// Determines if some values have been entered.
        /// </summary>
        /// <returns></returns>
        public bool HasValues()
        {
            return !this.SkillLevelDropDown3.IsNullSelected() ||
                   HasStringVal(this.UsedInTotalTextBox.Text) ||
                   HasStringVal(this.LastUsedTextBox.Text);

        }

        private bool HasStringVal(string text)
        {
            return text == null && text.Trim().Length > 0;
        }

        public void Save()
        {
            if (this.HasValues())
            {
                if (this.DidChange() && this.Validate())
                {
                    this.PopulateBack();
                    if (!this.mySkill.IsPersisted())
                    {
                        HibernateMgr.Current.Save(this.mySkill);
                        this.MsgLiteral.Text = AdminContentMgr.instance.GetContent("EditSkillCtrl.SkillCreated");
                    } 
                    else 
                    {
                        this.MsgLiteral.Text = AdminContentMgr.instance.GetContent("EditSkillCtrl.SkillUpdated");
                    }
                }
            }
            else
            {
                if (this.mySkill.IsPersisted())
                {
                    HibernateMgr.Current.Delete(this.mySkill);
                    this.MsgLiteral.Text = AdminContentMgr.instance.GetContent("EditSkillCtrl.SkillDeleted");
                }
            }
        }

        private bool DidChange()
        {
            return this.mySkill.Level != this.SkillLevelDropDown3.GetSelectedInt()
                   || this.mySkill.LastUsed != this.LastUsedTextBox.TextIntOrNull
                   || this.mySkill.UsedInTotal != this.UsedInTotalTextBox.TextIntOrNull;
        }

        /// <summary>
        /// Validates all input and returns true if all are valid.
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            bool isValid = true;
            
            foreach (BaseValidator ctrl in ValidationControls)
            {
                ctrl.Enabled = true;
                ctrl.Validate();
                isValid = isValid && ctrl.IsValid;
            }
            return isValid;
        }
        
        protected String GetRowId()
        {
            return "" + rowId;
        }

        public event DialogConcludedHandler DialogConcluded;

        public string DialogTitle
        {
            get 
            { 
                return "EditSkillCtrl." + (this.mySkill.IsPersisted() ? "UpdateResourceSkill" : "CreateResourceSkill"); 
            }
        }

        protected SkillProjectQualification SkillProjectQualification
        {
            get 
            { 
                return this.mySkill.GetProjectQualification(); 
            }
        }

        protected void OnClickAdjustYears(object sender, EventArgs e)
        {
            this.mySkill.AdjustYearsAccordingToProjectQualification();
            Utl.Msg.PostMessage("EditSkillCtrl.AdjustedSkill", this.mySkill.SkillName);
            PageNavigation.GotoCurrentPageAgainWithParms();
        }

        protected bool IsAlignedWithProjectExperience()
        {
            if (this._isAlignedWithProjectExperience == null)
            {
                this._isAlignedWithProjectExperience = this.mySkill.IsAlignedWithProjectExperience();
            }

            return (bool)this._isAlignedWithProjectExperience;
        }
    }
}