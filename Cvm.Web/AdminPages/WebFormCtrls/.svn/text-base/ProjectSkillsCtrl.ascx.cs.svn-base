using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Util;
using Cvm.Web.AdminPages.CommonCtrl;
using Cvm.Web.Code;
using Cvm.Web.CommonCtrl;
using Cvm.Web.Facade;
using Iesi.Collections.Generic;
using Napp.Backend.Hibernate;
using Napp.Web.Auto;
using Napp.Web.Auto.Annotations;
using Napp.Web.DialogCtrl;

namespace Cvm.Web.AdminPages.WebFormCtrls
{
    public partial class ProjectSkillsCtrl : System.Web.UI.UserControl, ICompositeWebFormWithParent
    {
        private Project _project;
        private ObjectSource projectObjectSource;
        private DialogInvokerViewState dialogInvokeState;

        public ProjectSkillsCtrl()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            dialogInvokeState = new DialogInvokerViewState(ViewState);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

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

        }

        private void BuildCheckBoxList()
        {
            SortedList<ISkill, bool> skills = GetSkills();
            this.SkillsCheckBoxList.DataSource = skills.Keys;
            this.SkillsCheckBoxList.DataBind();

        }

        private SortedList<ISkill, bool> GetSkills()
        {
            return EditCvFacade.instance.GetProjectSkillsAsSubsetOfResourceSkills(MyProject);
            
        }


        public void PopulateBack()
        {
            Dictionary<long, bool> containedSkillIds = MapToSetOfIds(GetSkills());
            foreach (ListItem item in this.SkillsCheckBoxList.Items)
            {
                long skillId = long.Parse(item.Value);
                if (item.Selected)
                {
                    //If not already contained, add it now
                    if (!containedSkillIds.ContainsKey(skillId))
                    {
                        EditCvFacade.instance.AddProjectSkill(skillId, MyProject);
                    } 
                } else
                {
                    //If contained, remove it
                    if (containedSkillIds.ContainsKey(skillId))
                    {
                        EditCvFacade.instance.RemoveProjectSkill(skillId, MyProject);
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
                if (_skills2[s])containedSkillIds.Add(s.SkillId,true);
            }
            return containedSkillIds;
        }



    }
}