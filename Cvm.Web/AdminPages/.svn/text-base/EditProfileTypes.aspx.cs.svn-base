using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class EditProfileTypes : System.Web.UI.Page
    {
        private ProfileType _profileType;
        private bool _profileTypeDidResolve=false;
        private List<SysSkill> _sysSkills;

        /**** Life cycle handlers ***/
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected override void OnInit(EventArgs e)
        {

            if (this.HasSelectedProfileType())
            {
                this.ProfileTypeForm.ObjectSourceInstance = this.MyProfileType;
                this.ProfileTypeForm.BuildForm();
                this.ProfileTypeForm.PopulateFront();
                this.MyList.CurrentObject = this.MyProfileType;
                UpdateList();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void UpdateList()
        {
            this.SkillRepeater.DataSource = GetProfileSkills();
            this.SkillRepeater.DataBind();
        }

        /******** Helper methods ********/

        private List<SysSkill> GetProfileSkills()
        {
            if (_sysSkills == null && HasSelectedProfileType())
            {
                _sysSkills = new List<SysSkill>(QueryMgrDynamicHql.Instance.GetSysSkillsByProfileTypeIds(this.MyProfileType.ProfileTypeId));
                _sysSkills.Sort((s, t) => String.Compare(s.SkillName, t.SkillName));
                
            }
            return _sysSkills;
        }

        protected ProfileType MyProfileType
        {
            get
            {
                if (!this._profileTypeDidResolve)
                {
                    if (Utl.Query.GetMode() == PageMode.New)
                    {
                        this._profileType = new ProfileType();
                    }
                    else
                    {
                        long? id = Utl.Query.GetParmLongOrNull(QueryParmCvm.id);
                        if (id != null)
                        {
                            this._profileType = Utl.QueryMgr.GetProfileTypeByIdOrNull((long) id);
                        }
                    }
                    this._profileTypeDidResolve = true;
                }
                return this._profileType;
            }
        }


        protected bool HasSelectedProfileType()
        {
            return this.MyProfileType != null;
        }

        protected void OnChangeAddSkill(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.AddSkillTextBox.Text))
            {
                SysSkill ss = new SysProfileHelper().AddSkillToProfileExclusively(this.AddSkillTextBox.Text, this.GetProfileSkills(),MyProfileType);

                if (ss!=null)
                {
                    this.AddSkillTextBox.Text = "";
                    UpdateList();
                }
            }
        }

        protected void OnClickSaveForm(object sender, EventArgs e)
        {
            this.ProfileTypeForm.PopulateBack();
            if (Utl.Query.GetMode()==PageMode.New) HibernateMgr.Current.Save(this.MyProfileType);
            PageNavigation.GetCurrentLink().SetMode(PageMode.Update).SetParm(QueryParmCvm.id,MyProfileType.Id).Redirect();
        }
    }
}