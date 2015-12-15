using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.CvImport;
using Cvm.Backend.FileStore;
using Cvm.Web.AdminPages.CommonCtrl;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Backend.Business.Multisite;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.VeryBasic;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class ImportCvs : System.Web.UI.Page
    {
        private const int MaxNoIgnoreRules = 20;
        private const string ResourceIdsToken = "resIds";
        private static readonly Regex WhitespaceRegex = new Regex(@"\s+",RegexOptions.Multiline);
        private IEnumerable<CvImportItem> items;
        protected readonly ImportCvsFacade facade = CvmFacade.ImportCvs;
        private ICollection<string> _fileNames;
        private List<string> filters;

        private List<String> resourceNames;
        private List<Resource> _importedResources;
        private string[] _ignoresArray;
        private HashSet<string> _ignoredSkillsSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateArchiveDropDown();
                //If we have been here before we should clear the session.
                ImportedResourceIds = null;
            }

            if (MustPopulateEditTable)
            {
                PopulateEditTable();
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            this.ProgressBarRep.DataBind();
        }

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        private void PopulateArchiveDropDown()
        {
            this.ArchiveDropDown.DataSource = facade.GetPendingFilesNames(ContextObjectHelper.CurrentSysRoot);
            this.ArchiveDropDown.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            AdjustRuleTextBoxes();
        }

        /// <summary>
        /// Rearranges the text values and visibility of the rule text boxes 
        /// so that there is always exactly one more empty text box than the number of specified
        /// ignore rules.
        /// </summary>
        private void AdjustRuleTextBoxes()
        {
            List<string> filters = this.GetIgnoreFilters();
            int i = 0;
            foreach (Control ctrl in this.RulePlaceHolder.Controls)
            {
                TextBox textBox = ctrl as TextBox;
                if (textBox != null)
                {
                    if (i < filters.Count)
                    {
                        textBox.Text = filters[i];
                        textBox.Visible = true;
                    }
                    else
                    {
                        textBox.Text = null;
                        //Exactly one text box more than what is currently
                        //having a filter should be visible
                        if (i == filters.Count) textBox.Visible = true;
                        else textBox.Visible = false;
                    }
                    i++;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            //Keep the ruleRepeater populated with a constant number of text-boxes
            //To allow view state to be preserve through all wizard steps.
            //They are not visible by default and will only become visible as needed.
            PopulateIgnoreFilterTextBoxes();
        }

        private void PopulateIgnoreFilterTextBoxes()
        {
            for (int i = 0; i < MaxNoIgnoreRules; i++)
            {
                TextBox box = new TextBox();
                box.Visible = false;
                box.TextChanged += this.OnRuleTextChanged;
                box.AutoPostBack = true;
                box.Width = 200;
                this.RulePlaceHolder.Controls.Add(box);
            }
        }

        private void OnRuleTextChanged(object sender, EventArgs e)
        {
            UpdateResourceNamesTextBox();
            this.AdjustRuleTextBoxes();
        }

        private void PopulateEditTable()
        {
            if (this.ItemPlaceHolder.Controls.Count == 0)
            {
                IEnumerable<CvImportItem> items = GetPotentialImportItems(this.ResourceNamesList);
                foreach (CvImportItem item in items)
                {
                    EditCvImportItemCtrl ctrl = (EditCvImportItemCtrl)LoadControl(CommonCtrls.EditCvImportItemCtrl);
                    ctrl.HideIfCompleted = this.HideCompletedCheckBox.Checked;
                    ctrl.ImportItem = item;
                    ctrl.PopulateFront();

                    this.ItemPlaceHolder.Controls.Add(ctrl);

                }
            }
        }

        /// <summary>
        /// Returns a list of CvImportItem each of which describes how a resource can be/is to be imported.
        /// Whether it is im
        /// </summary>
        /// <param name="resourceNames"></param>
        /// <returns></returns>
        private IEnumerable<CvImportItem> GetPotentialImportItems(IList<String> resourceNames)
        {
            if (items == null)
            {
                FilePath archiveFilePath = GetArchiveFileObj();
                items = facade.GetPotentialImportItems(archiveFilePath, resourceNames);
            }
            return items;
        }

        private FilePath GetArchiveFileObj()
        {
            string archiveFileName = this.ArchiveDropDown.SelectedItem.Value;
            return FolderStructure.Instance.GetPendingFile(SysCode(), archiveFileName);
        }

        private SysCode SysCode()
        {
            return ContextObjectHelper.CurrentSysRoot.SysCodeObj;
        }

        private bool MustPopulateEditTable
        {
            get
            {
                bool? b = this.ViewState["mustPop"] as bool?;
                return b == true;
            }
            set
            {
                this.ViewState["mustPop"] = value;
            }
        }

        protected void OnActivateEditImportDetails(object sender, EventArgs e)
        {
            if (ValidateImportRules())
            {
                MustPopulateEditTable = true;
                PopulateEditTable();
            }
            else
            {
                this.Wizard1.ActiveStepIndex--;
            }
        }

        private bool ValidateImportRules()
        {
            List<string> names = ResourceNamesList;
            if (names.Count != this.CvFileNames.Count)
            {
                MessageManager.Current.PostMessage("ImportCvs.NumberOfLinesMustMatch", names.Count, this.CvFileNames.Count);
                return false;
            }
            return true;
        }

        private List<string> ResourceNamesList
        {
            get
            {
                if (this.resourceNames == null)
                {
                    this.resourceNames =
                        facade.SplitNames(this.ResourceNamesTextBox.Text);
                }
                return resourceNames;
            }
        }



        protected void OnComplete(object sender, EventArgs e)
        {
            PageNavigation.GotoCurrentPageAgainWithParms();
        }

        private IList<CvImportItemStatus> DoImport()
        {
            IList<CvImportItem> items = new List<CvImportItem>();
            IList<CvImportItemStatus> stati = new List<CvImportItemStatus>();
            foreach (EditCvImportItemCtrl ctrl in GetEditCvImportItemCtrls())
            {
                if (!ctrl.IsCompleted)
                {
                    ctrl.PopulateBack();
                    CvImportItem _item = ctrl.ImportItem;
                    if (_item.DoImport)
                    {
                        CvImporter importer = facade.GetImporter(SysCode(), this.ArchiveDropDown.SelectedValue);
                        CvImportItemStatus status = importer.DoImport(_item);
                        stati.Add(status);
                        ctrl.SetStatus(status);
                        if (status.ChosenResource != null)
                        {
                            this.AddImportResource(status.ChosenResource.ResourceId);
                        }
                    }
                }
            }
            return stati;
        }

        private IEnumerable<EditCvImportItemCtrl> GetEditCvImportItemCtrls()
        {
            foreach (Control ctrl in this.ItemPlaceHolder.Controls)
            {
                if (ctrl is EditCvImportItemCtrl) yield return ctrl as EditCvImportItemCtrl;
            }
        }

        protected void OnActivateRules(object sender, EventArgs e)
        {
            UpdateFileNamesTextArea();

            UpdateResourceNamesTextBox();
        }

        private void UpdateResourceNamesTextBox()
        {
            ICollection<string> fileNames = this.CvFileNames;
            string resourceNamesString = facade.GetResourceNamesBestGuess(fileNames, this.GetIgnoreFilters());
            this.ResourceNamesTextBox.Text = resourceNamesString;
            this.ResourceNamesTextBox.Rows = fileNames.Count;
        }


        /// <summary>
        /// Returns the ignore-filters by reading from the strings posted 
        /// with the textboxes found in the RuleRepeater.
        /// </summary>
        /// <returns></returns>
        private List<String> GetIgnoreFilters()
        {
            if (this.filters == null)
            {
                this.filters = new List<string>();
                foreach (Control ctrl in this.RulePlaceHolder.Controls)
                {
                    TextBox box = ctrl as TextBox;
                    if (box != null)
                    {
                        if (!String.IsNullOrEmpty(box.Text)) filters.Add(box.Text);
                    }
                }
            }
            return filters;
        }


        private void UpdateFileNamesTextArea()
        {
            if (String.IsNullOrEmpty(this.FileNamesTextBox.Text))
            {
                ICollection<string> fileNames = CvFileNames;
                string text = JoinNewLines(fileNames);
                this.FileNamesTextBox.Text = text;
                this.FileNamesTextBox.Rows = fileNames.Count;
            }
        }

        private string JoinNewLines(IEnumerable<string> fileNames)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string f in fileNames) sb.Append(f).Append("\n");
            return sb.ToString();
        }


        private ICollection<string> CvFileNames
        {
            get
            {
                if (_fileNames == null) _fileNames = facade.GetArchiveEntryNames(this.GetArchiveFileObj());
                return _fileNames;
            }
        }

        protected void OnChangedArchiveSelect(object sender, EventArgs e)
        {
            this.FileNamesTextBox.Text = null;
            this.ResourceNamesTextBox.Text = null;
        }

        protected void OnIgnoreFilterChanged(object sender, EventArgs e)
        {
            this.UpdateResourceNamesTextBox();
            this.AdjustRuleTextBoxes();
        }

        protected void OnChangedAllCheckBox(object sender, EventArgs e)
        {
            foreach (var ctrl in GetEditCvImportItemCtrls())
            {
                if (!ctrl.IsCompleted)
                {
                    ctrl.DoImport = this.AllCheckBox.Checked;
                }
            }
        }

        protected void OnClickUploadArchiveBtn(object sender, EventArgs e)
        {
            FolderStructure.Instance.EnsurePendingFolder(this.SysCode());
            FilePath file = FolderStructure.Instance.GetPendingFile(this.SysCode(), FileUpload1.FileName);
            bool didChangeFileName = false;
            while (file.ExistsAsFile())
            {
                file = file.IncrementFileCounter();
                didChangeFileName = true;
            }
            if (didChangeFileName) MessageManager.Current.PostMessage("ImportCvs.SaveArchiveNewFileName", file.FileName);
            file.EnsureParentFolderExists();
            this.FileUpload1.SaveAs(file.AbsolutePath);
            PopulateArchiveDropDown();

        }
        private HashSet<long> ImportedResourceIds
        {
            get
            {
                if (Session[ResourceIdsToken]==null)
                {
                    Session[ResourceIdsToken] = new HashSet<long>();
                }
                return Session[ResourceIdsToken] as HashSet<long>;
            }
            set
            {
                Session[ResourceIdsToken] = value;
            }
        }

        private string ImportedResourceIdsAsStr
        {
            get
            {
                return this.ImportedResourceIds.ConcatToString();
            }
        }

        /// <summary>
        /// Contains a cached map from resourceIds to a whitespace seperated list of skills
        /// 
        /// </summary>
        private Dictionary<long, SkillMatchWrapperList> ImportedSkills
        {
            get
            {
                if (Session["skillsImp"] == null)
                {
                    Session["skillsImp"] = new Dictionary<long, SkillMatchWrapperList>();
                }
                return (Dictionary<long, SkillMatchWrapperList>)Session["skillsImp"];
            }
        }

        private void AddImportResource(long resId)
        {
            if (!this.ImportedResourceIds.Contains(resId))
            {
                this.ImportedResourceIds.Add(resId);
            }
        }


        /// <summary>
        /// Makes sure that the resource ID's being imported
        /// are persisted to the session
        /// </summary>
        /*private void EnsureResourceIdsPersisted()
        {
            if (this._importedResIds != null && this.importedResourceDirty)
            {
                StringBuilder sb = new StringBuilder();
                bool isFirst = true;
                foreach (int id in this._importedResIds)
                {
                    if (!isFirst) sb.Append(",");
                    sb.Append(id);
                    isFirst = false;
                }
                this.ImportedResourceIdsPersisted = sb.ToString();
                this.importedResourceDirty = false;
            }
        }*/

        /// <summary>
        /// Takes all successfully imported resources and 
        /// saves them by ID in a view state variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnActivateSetupEmails(object sender, EventArgs e)
        {
            bool hasImportedToSession = this.ImportedResourceIds.Count > 0;
            if (!hasImportedToSession)
            {
                this.Wizard1.ActiveStepIndex--;
                Utl.Msg.PostMessage("ImportCvs.NothingYetImported");
            }
            else
            {
                //Now build up names and emails correspondingly
                NamesAndEmails namesAndEmail = this.facade.GetNamesAndEmail(this.ImportedResourceIdsAsStr);
                this.FileNames2TextBox.Text = namesAndEmail.GetNamesNewlined();
                this.EmailsTextBox.Text = namesAndEmail.GetEmailsNewlined();                                       
            }
        }



        private bool ImportStatusHasErrors(IEnumerable<CvImportItemStatus> stati)
        {
            foreach (var s in stati)
            {
                if (s.HasErrors())
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// /Determines whether at least one resource was imported.
        /// </summary>
        /// <param name="stati"></param>
        /// <returns></returns>
        private bool ImportStatusDidImport(IEnumerable<CvImportItemStatus> stati)
        {
            foreach (var s in stati)
            {
                if (s.ChosenResource!=null)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ImportStatusMustChooseResource(IEnumerable<CvImportItemStatus> stati)
        {
            foreach (var s in stati)
            {
                if (s.GetPotentialResources() != null && s.GetPotentialResources().Count>0)
                {
                    return true;
                }
            }
            return false;
        }
        protected void OnActivateImportSkills(object sender, EventArgs e)
        {
            PersistEmailAdresses(ImportedResources);
            HibernateMgr.Current.CommitAndReopenTransaction();
            PopulateImportSkillsRep();
        }

        private void PopulateImportSkillsRep()
        {
            List<Resource> res = ImportedResources;
            this.ImportSkillsRep.Controls.Clear();
            this.ImportSkillsRep.DataSource = res;
            this.ImportSkillsRep.DataBind();
        }

        /// <summary>
        /// Returns the imported resources as a lazy property
        /// </summary>
        private List<Resource> ImportedResources
        {
            get
            {
                if (this._importedResources == null)
                {
                    if (ImportedResourceIds.Count==0)
                    {
                        this._importedResources=new List<Resource>();
                    } else
                    {
                        this._importedResources = this.facade.GetResourcesByIds(ImportedResourceIdsAsStr);
                    }
                }
                return this._importedResources;
            }
        }


        protected void OnActivateInviteUsers(object sender, EventArgs e)
        {
            SaveImportedSkills();
        }


        protected void OnActivateSummary(object sender, EventArgs e)
        {
            this.SummaryRep2.DataSource=this.ImportedResources;
            this.SummaryRep2.DataBind();
        }

        private void SaveImportedSkills()
        {
            List<Resource> res=this.ImportedResources;
            foreach (Resource r in res)
            {
                SkillMatchWrapperList skills=this.GetImportedSkills(r);
                if (!skills.HasError())
                {
                    foreach (SkillMatchWrapper s in skills.GetIList())
                    {
                        Skill skill = s.Skill;
                        if (!this.IgnoredSkillsSet.Contains(skill.SkillName))
                        {
                            if (CvmFacade.ImportSkills.AssignSkill(r, skill))
                            {
                                Utl.Msg.PostMessage("ImportCvs.AssignedSkill", r, skill.SkillName);
                            }
                        }
                    }
                }
            }
        }

        private void PersistEmailAdresses(List<Resource> res)
        {
            string emailsLinebreakedBefore = this.EmailsTextBox.Text;
            string emailsLinebreakedAfter = this.facade.PersistEmails(emailsLinebreakedBefore, res);
            this.EmailsTextBox.Text = emailsLinebreakedAfter;
        }



        protected void OnChangeIgnoreEmails(object sender, EventArgs e)
        {
            string ignores = this.EmailsIgnoreTextBox.Text;
            
            String[] ignoreArr = WhitespaceRegex.Split(ignores);
            StringBuilder sb = new StringBuilder(this.EmailsTextBox.Text);
            foreach (String ign in ignoreArr)
            {
                sb.Replace(ign, "");
            }
            this.EmailsTextBox.Text = sb.ToString();
        }


        protected void OnChangeIgnoreSkills(object sender, EventArgs e)
        {
            this.PopulateImportSkillsRep();
        }

        /// <summary>
        /// Returns the skills of the i'th resource in the ImportedResources arraylist.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected String GetSkillsAsStr(int i)
        {
            Resource r = this.ImportedResources[i];
            SkillMatchWrapperList skillMatchWrappers = GetImportedSkills(r);
            string skills = skillMatchWrappers.GetListOfSkillsAsString();
            skills = FilterSkillsStr(skills);
            return skills;
        }

        private string FilterSkillsStr(string skills)
        {
            skills = FilterIgnored(skills, IgnoredSkillsArray);
            return skills;
        }

        private string[] IgnoredSkillsArray
        {
            get
            {
                if (this._ignoresArray == null)
                {
                    string ignores = this.IgnoreSkillsTextBox.Text;
                    this._ignoresArray = WhitespaceRegex.Split(ignores);
                }
                return _ignoresArray;
            }
        }


        private HashSet<string> IgnoredSkillsSet
        {
            get
            {
                if (this._ignoredSkillsSet == null)
                {
                    this._ignoredSkillsSet = new HashSet<string>();
                    foreach (String ign in IgnoredSkillsArray)
                    {
                        this._ignoredSkillsSet.Add(ign.ToLower());
                    }
                }
                return _ignoredSkillsSet;
            }
        }

        private string FilterIgnored(string skills, string[] ignoresArr)
        {
            foreach(String ignore in ignoresArr)
            {
                Regex ign=new Regex("\b"+ignore+"\b",RegexOptions.IgnoreCase);
                if (!String.IsNullOrEmpty(ignore)) skills = ign.Replace(skills,"");
            }
            return skills;
        }

        /// <summary>
        /// Gets the imported skills corresponding to a give resource.
        /// It is processed the first time for the given resource and then
        /// cached on the session.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        protected SkillMatchWrapperList GetImportedSkills(Resource r)
        {
            long resId=r.ResourceId;
            if (!this.ImportedSkills.ContainsKey(resId)) {
                try
                {
                    this.ImportedSkills[resId] = this.facade.GetSkillsToImport(r);
                } catch(Exception e)
                {
                    this.ImportedSkills[resId]=SkillMatchWrapperList.CreateError(e);
                }
            }
            return this.ImportedSkills[resId];
        }

        protected void OnClickImportResources(object sender, EventArgs e)
        {
            IList<CvImportItemStatus> stati = DoImport();
            bool hasErrors = ImportStatusHasErrors(stati);
            bool didImport = this.ImportStatusDidImport(stati);
            bool mustSelect = this.ImportStatusMustChooseResource(stati);
            bool hasImportedToSession = this.ImportedResourceIds.Count > 0;

            if (hasErrors) Utl.Msg.PostMessage("ImportCvs.HasImportErrors");
            else if (mustSelect) Utl.Msg.PostMessage("ImportCvs.MustSelect");
            else if (!hasImportedToSession) Utl.Msg.PostMessage("ImportCvs.NothingYetImported");
            
        }

        /// <summary>
        /// Determines whether an invitation mail was sent.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected bool IsInvited(Resource resource)
        {
            return false;
        }

        protected int CountSkill(SkillMatchWrapperList skills)
        {
            if (skills==null || skills.GetIList()==null) return 0;
            return skills.GetIList().Count;
        }
    }
}
