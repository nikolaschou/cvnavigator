using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class SysProfiles : System.Web.UI.Page
    {
        private List<SysSkill> _skills;
        private List<ProfileType> _profiles;
        private bool _didSave = false;
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetupProfileRepeater();
        }

        private void SetupProfileRepeater()
        {
            this.ProfileRep.Controls.Clear();
            this.ProfileRep.DataSource = MyProfileTypes;
            this.ProfileRep.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            bool hasSkills = MySysSkills.Count == 0;
            bool hasProfiles = MyProfileTypes.Count == 0;
            if (hasSkills || hasProfiles)
            {
                Utl.Msg.PostMessage("SysProfile.MustCreate" +
                                    (hasSkills && hasProfiles ? "Both" : (hasSkills ? "Skill" : "Profile")));
            }
        }

        protected List<SysSkill> MySysSkills
        {
            get
            {
                if (_skills==null)
                {
                    this._skills = new List<SysSkill>(QueryMgr.instance.GetAllSysSkill().ListOrNull(HibernateMgr.Current.Session));
                    SortSkills();
                }
                return _skills;
            }
        }

        private void SortSkills()
        {
            this._skills.Sort((s,t)=>String.Compare(s.SkillName,t.SkillName));
        }

        protected IList<ProfileType> MyProfileTypes
        {
            get
            {
                if (this._profiles==null)
                {
                    this._profiles = new List<ProfileType>(QueryMgr.instance.GetAllProfileType().ListOrNull(HibernateMgr.Current.Session));
                    SortProfiles();
                }
                return _profiles;
            }
        }

        private void SortProfiles()
        {
            this._profiles.Sort((p,q)=>String.Compare(p.ProfileTypeName,q.ProfileTypeName));
        }

        protected void OnChangeAddSkill(object sender, EventArgs e)
        {
            String skill = this.AddSkillTextBox.Text;
            SysSkill ss = new SysProfileHelper().AddSkill(skill, MySysSkills);
            if (ss!=null)
            {
                SortSkills();
                this.AddSkillTextBox.Text = "";

                GotoPageAgain();
            }
        }



        protected void OnClickSaveBtn(object sender, EventArgs e)
        {
            DoSave();
        }

        private void DoSave()
        {
            if (_didSave)
            {
                //Do nothing
            }
            else
            {
                string state = this.StateTextBx.Text;
                //Has dimension n x m where n is the number of SysSkills and m is the number of profiles
                int[][] stateArr = new JavaScriptSerializer().Deserialize<int[][]>(state);
                try {
                for (int i = 0; i < MySysSkills.Count; i++)
                {
                    for (int j = 0; j < MyProfileTypes.Count; j++)
                    {
                        
                            MySysSkills[i].MakeAssigned(MyProfileTypes[j], stateArr[i][j] == 1);
                       
                    }
                }
                _didSave = true;
                }
                catch (IndexOutOfRangeException)
                {
                    Utl.Msg.PostMessage("SysProfiles.ErrorDuringMatrixSave");
                }
            }
        }

        protected void OnClickCancelBtn(object sender, EventArgs e)
        {
            GotoPageAgain();
        }

        private void GotoPageAgain()
        {
            PageNavigation.GetCurrentLink().Redirect();
        }


        protected void OnChangeAddProfile(object sender, EventArgs e)
        {
            string profileName = this.AddProfileTextBox.Text;
            if (String.IsNullOrEmpty(profileName) || profileName.Trim().Length==0)
            {
                  Utl.Msg.PostMessage("SysProfiles.NoProfileSpecified");
            }
            else if (MyProfileTypes.Any((s => s.ProfileTypeName.ToLower().Equals(profileName.ToLower()))))
            {
                Utl.Msg.PostMessage("SysProfiles.ProfileAlreadyExists");
            }
            else
            {
                ProfileType p = CvmFacade.SysProfile.CreateProfileType(profileName);
                MyProfileTypes.Add(p);
                SortProfiles();
                this.AddProfileTextBox.Text = "";
                this.SetupProfileRepeater();
            }

            GotoPageAgain();
        }
        #region Ajax methods
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static String[] GetSkills(String skillSearch)
        {
            return CvmFacade.SysProfile.GetSkillsBySearch(skillSearch);
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static void RemoveSysSkill(long skillId)
        {
            CvmFacade.SysProfile.RemoveSysSkill(skillId);
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false)]
        public static void DeleteProfile(long profileTypeId)
        {
            CvmFacade.SysProfile.DeleteProfile(profileTypeId);
        }
        #endregion

        protected void OnClickDeleteProfile(object sender, ImageClickEventArgs e)
        {
            String arg=((ImageButton) sender).CommandArgument;
            long profileTypeId = long.Parse(arg);
            ProfileType pt = MyProfileTypes.FirstOrDefault(p => p.ProfileTypeId == profileTypeId);
            if (pt!=null)
            {
                String pname = pt.ProfileTypeName;
                DeleteProfile(profileTypeId);
                MyProfileTypes.Remove(pt);   
                Utl.Msg.PostMessage("SysProfile.ProfileRemoved",pname);
                SetupProfileRepeater();
            }
            GotoPageAgain();
        }
    }
}