using System;
using System.Linq;
using Cvm.Backend.Business.Import;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Facade;
using Napp.Web.Auto;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class ImportCvDataCtrl : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl
    {
        private ObjectSource _source;
        private LinkedInImportMgr _mgr;

        public ObjectSource ObjectSource
        {
            get 
            { 
                return _source; 
            }
            set 
            { 
                _source = value;
                this.MyImportSkillsCtrl.ObjectSource = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.LinkedInCtrl.OnImportDone = LinkedInImportDone;
        }

        private void LinkedInImportDone(String importTxt)
        {
            this.CleanLinkedInMgr();
            MyResource.RelatedResourceImportOrCreate.LinkedInImport = importTxt;
            this.BuildForm();
            this.PopulateFront();
        }

        public Resource MyResource
        {
            get
            {
                return (Resource)ObjectSource();
            }
        }

        protected void OnTextChangedImportTextBox(object sender, EventArgs e)
        {
            ResourceImport imp = MyResource.RelatedResourceImport;
            if (imp == null)
            {
                //Do nothing, it was deleted already
            }
            else
            {
                imp.ImportText = this.ImportTextBox.Text;                
            }
        }

        private void PopulateImportText()
        {
            Resource res = MyResource;

            ResourceImport imp = res.RelatedResourceImport;
            if (imp != null)
            {
                this.ImportTextBox.Text = imp.ImportText;                
            }
            else
            {
            }
        }

        public void BuildForm()
        {
            this.MyImportSkillsCtrl.BuildForm();
            BuildProjectCtrl();
            BuildEducationsCtrl();
            BuildCertificationCtrl();
            BuildImportLanguageCtrl();
        }

        private void BuildImportLanguageCtrl()
        {
            if (Mgr.GetLanguages() == null || Mgr.GetLanguages().Length > 0)
            {
                LangLitteral.Visible = false;
                Button3.Visible = true;
            }
            else
            {
                Button3.Visible = false;

                LangLitteral.Text = Utl.Content("ImportCvDataCtrl.NoLanguageToImport");
            }
        }

        private void BuildProjectCtrl()
        {
            var delta = this.Mgr.ProjectDelta;
            if (delta.AllImportedObjects.Any())
            {
                this.ImportProjectsCtrl.UpdatedObjects = delta.UpdatedObjects;
                this.ImportProjectsCtrl.NewObjects = delta.NewObjects;
            }
        }

        private void BuildEducationsCtrl()
        {
            var delta = this.Mgr.EducationDelta;
            if (delta.AllImportedObjects.Any())
            {
                this.ImportEducationsCtrl.UpdatedObjects = delta.UpdatedObjects;
                this.ImportEducationsCtrl.NewObjects = delta.NewObjects;
            }
        }

        private void BuildCertificationCtrl()
        {
            var delta = this.Mgr.CertificationDelta;
            if (delta.AllImportedObjects.Any())
            {
                this.ImportCertificationsCtrl.UpdatedObjects = delta.UpdatedObjects;
                this.ImportCertificationsCtrl.NewObjects = delta.NewObjects;
            }
        }

        public void PopulateFront()
        {
            this.PopulateImportText();
            this.MyImportSkillsCtrl.PopulateFront();
        }
       
        /// <summary>
        /// To clean any caches or prepared data, call this method.
        /// </summary>
        private void CleanLinkedInMgr()
        {
            _mgr = null;
        }

        protected LinkedInImportMgr Mgr
        {
            get
            {
                if (_mgr == null)
                {
                    this._mgr = LinkedInImportMgr.Create(MyResource);
                }
                return _mgr;
            }
        }

        public void PopulateBack()
        {
            this.MyImportSkillsCtrl.PopulateBack();
        }

        protected void OnClickUploadCv(object sender, EventArgs e)
        {
            if (this.CvFileUpload.HasFile)
            {
                CvmFacade.Signup.ImportDocumentToResource(this.CvFileUpload.FileName, this.MyResource, this.CvFileUpload.SaveAs);
                PageNavigation.GotoCurrentPageAgainWithParms();
            }
            else
            {
                Utl.Msg.PostMessage("ImportCvDataCtrl.MustSelectFile");
            }
        }

        protected void OnClickImportPhotoBtn(object sender, EventArgs e)
        {
            MyResource.PhotoUrl = Mgr.BaseData.PhotoUrl;
        }
        protected void OnClickImportHeadlineBtn(object sender, EventArgs e)
        {
            MyResource.ProfileTitle = Mgr.BaseData.Headline;
        }

        protected void OnClickImportAllBtn(object sender, EventArgs e)
        {
            Mgr.ImportAllNew();
            Utl.Msg.PostMessage("ImportCvDataCtrl.AllNewImported");
            PageNavigation.GotoCurrentPageAgainWithParms();
        }

        protected void OnClickImportLanguage(object sender, EventArgs e)
        {
            Mgr.ImportLanguages();
        }
    }
}