using System;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Customers;
using Cvm.Web.Facade;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.VeryBasic.GenericDelegates;
using Napp.Web.AdmControl;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;
using Napp.Web.AutoFormExt;
using Napp.Web.Navigation;
using Napp.Web.WebForm;

namespace Cvm.Web.AdminPages.GenericCtrls
{
    public class EditObjectFromListCtrl<T> : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl
        where T : IBusinessObject
    {
        /// <summary>
        /// Knows how to return a new instance to be created.
        /// </summary>
        private ObjectSource _objectSource;
        private T _current;
        private readonly AutoFormExt2 autoForm = new AutoFormExt2();
        private readonly AdmButton saveAndDoneButton = new AdmButton();
        private readonly AdmButton saveAndNewButton = new AdmButton();
        private readonly AdmButton cancelButton = new AdmButton();

        /// <summary>
        /// Is called when editing (new or existing) is done.
        /// </summary>
        public event GenericEventHandler OnReturn;
        public bool IsCreateNewMode = false;
        public string OmitProperties;

        /// <summary>
        /// Assign the object to which this list of objects belongs.
        /// </summary>
        public IBusinessObject ListOwnerObject;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            saveAndNewButton.Click += OnClick_SaveAndNewBtn;
            saveAndDoneButton.Click += OnClick_SaveAndDoneBtn;
            cancelButton.Click += OnClick_CancelBtn;

            saveAndNewButton.ContentId = "AddObjectCtrl.SaveAndNew";
            saveAndDoneButton.ContentId = "AddObjectCtrl.SaveAndDone";
            cancelButton.ContentId = "AddObjectCtrl.Cancel";

            saveAndNewButton.CssClass = "saveBtn";
            saveAndDoneButton.CssClass = "saveBtn";
            cancelButton.CssClass = "cancelBtn";

            saveAndNewButton.CausesValidation=true;
            saveAndDoneButton.CausesValidation = true;
            cancelButton.CausesValidation = false;

            //Show the save-and-new button iff we are in new-mode.
            saveAndNewButton.Visible = this.IsCreateNewMode;
        }

        private T CurrentObject
        {
            get
            {
                if (_current == null)
                {
                    _current = (T) _objectSource();
                }
                return _current;
            }
        }
        public void PopulateFront()
        {
            this.autoForm.PopulateFront();
        }

        public void PopulateBack()
        {
            this.autoForm.PopulateBack();
        }

        public ObjectSource ObjectSource
        {
            get { return _objectSource; }
            set
            {
                //We save the reference to the assigned
                //value and use the local wrapper to
                //assign to the inner autoForm.
                //In this way we are in control
                //of when to create new instances.
                _objectSource = value;
                this.autoForm.ObjectSource = GetCurrent;
            }
        }

        /// <summary>
        /// In case _objectSource is a source creating new instances every time
        /// it is called, we need to make this wrapper to allow for local caching.
        /// </summary>
        /// <returns></returns>
        private object GetCurrent()
        {
            return CurrentObject;
        }

        public void BuildForm()
        {
            //Build up all controls
            ContainerCtrl contCtrl = new ContainerCtrl();
            string contextTitle = (this.ListOwnerObject!=null ? this.ListOwnerObject.StandardObjectTitle : "parent object");
            contCtrl.Title = AdminContentMgr.instance.GetContent("EditObjectFromListCtrl.Title"+QueryStringHelper.Instance.GetMode(), AdminContentMgr.instance.GetContent("ObjectTypes." + this.CurrentObject.GetObjectType()), contextTitle);
            HtmlControl div=new HtmlGenericControl("div");
            HtmlControl buttonDiv = new HtmlGenericControl("div class='buttons'");
            div.Controls.Add(autoForm);
            contCtrl.Controls.Add(div);

            contCtrl.Controls.Add(buttonDiv); // Make sure buttons align to lower right
            buttonDiv.Controls.Add(saveAndNewButton);
            buttonDiv.Controls.Add(saveAndDoneButton);
            buttonDiv.Controls.Add(cancelButton);
            
            this.Controls.Add(contCtrl);

            //Build autoForm
            autoForm.EditMode = AutoFormEditMode.Edit;
            autoForm.IncludeDeleteLink = false;
            autoForm.OmitProperties = this.OmitProperties;
            autoForm.BuildForm();
        }


        protected void OnClick_SaveAndNewBtn(object sender, EventArgs e)
        {
            if (DoAddObject())
            {
                _current = null;
                this.autoForm.Controls.Clear();
                this.autoForm.BuildForm();
                this.autoForm.PopulateFront();
            }
        }

        private void OnClick_CancelBtn(object sender, EventArgs e)
        {
            MessageManager.Current.PostMessage("AddObjectCtrl.NothingAdded");
            if (OnReturn!=null) OnReturn();
        }


        private void OnClick_SaveAndDoneBtn(object sender, EventArgs e)
        {
            if (DoAddObject())
            {
                if (OnReturn != null) OnReturn();                
            }
        }

        private bool DoAddObject()
        {
            PopulateBack();
            T obj = CurrentObject;

            if (Validate(obj))
            {
                HibernateMgr.Current.SaveOrUpdate(obj);
                //Now add or remove project skills
                SaveProjectsSkills(obj);
                MessageManager.Current.PostMessage(
                    "AddObjectCtrl.Object" + (this.IsCreateNewMode ? "Added" : "Updated"), CurrentObject.GetObjectType(),
                    CurrentObject.ToString());
                return true;
            } else
            {
                HibernateMgr.Current.Session.Evict(obj);
                return false;
            }
        }

        private bool Validate(T obj)
        {
            return ((IBusinessObject) obj).ValidateWithMessages()==ValidationResultEnum.Valid;
        }

        private void SaveProjectsSkills(T obj)
        {
            Project prj = obj as Project;
            if (prj!=null)
            {
                foreach (long skillId in prj.SkillIdsToAdd)
                {
                    EditCvFacade.instance.AddProjectSkill(skillId, prj);
                }
                foreach (long skillId in prj.SkillIdsToDelete)
                {
                    EditCvFacade.instance.RemoveProjectSkill(skillId, prj);
                }
                prj.ClearProjectSkillAdjustments();
            }
        }
    }
}