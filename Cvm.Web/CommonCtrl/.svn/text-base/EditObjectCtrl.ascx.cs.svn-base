using System;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.VeryBasic.GenericDelegates;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;
using Napp.Web.DialogCtrl;

namespace Cvm.Web.CommonCtrl
{
    public partial class EditObjectCtrl : System.Web.UI.UserControl, IDialogCtrl
    {

        /// <summary>
        /// Will be fired when a conclusion on a dialog has been found.
        /// If the dialog is cancelled, this event will be triggered with 
        /// null as the conclusion.
        /// </summary>
        public event DialogConcludedHandler DialogConcluded;

        public GenericEventHandler OnSaveHandler;
        public GenericEventHandler OnCreateHandler;
        /// <summary>
        /// This handler will be invoked in case of all ending events: 
        /// object saved, object created or action cancelled.
        /// </summary>
        public GenericEventHandler OnDoneHandler;
        private string entityName;
        private object obj;

        /// <summary>
        /// Call this statically from all pages.
        /// </summary>
        public ObjectSource ObjectGetter
        {
            set
            {
                this.AutoForm2.ObjectSource = value;
            }
            get
            {
                return this.AutoForm2.ObjectSource;
            }
        }

        /// <summary>
        /// Specify the entity name here to get easy create-new functionality.
        /// </summary>
        public String EntityName
        {
            set
            {
                this.entityName = value;
                this.ObjectGetter = GetNewByEntityName;
            }
        }

        /// <summary>
        /// If EntityName has been assigned this will be used as the object source.
        /// </summary>
        /// <returns></returns>
        private object GetNewByEntityName()
        {
            if (this.obj==null)
            {
                Type type = TypeParser.ParseType(entityName);
                this.obj = type.GetConstructor(new Type[0]).Invoke(new object[0]);                
            }
            return this.obj;
        }

        public String IncludeOnlyProperties
        {
            set
            {
                this.AutoForm2.IncludeOnlyProperties = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AutoForm2.IncludeDeleteLink = false;
        }

        public bool IsModeNew() {
            IBusinessObject _obj = (IBusinessObject) ObjectGetter();
            return !_obj.IsPersisted();
        }

        public void BuildForm()
        {
            this.AutoForm2.BuildForm();
            this.AutoForm2.IncludeDeleteLink = !this.IsModeNew();
            this.BtnPanel.Visible = true;
        }

        public void PopulateFront()
        {
            this.AutoForm2.PopulateFront();    
        }


        public string ObjType
        {
            set
            {
                this.AutoForm2.ContentIdPrefix = value;
            }
        }

        public string DialogTitle
        {
            get
            {
                IBusinessObject _obj = (IBusinessObject)ObjectGetter();
                string contentId = "EditObjectCtrl."+(_obj.IsPersisted()?"Update":"Create");
                string objTypeName = AdminContentMgr.instance.GetContent("ObjectTypes." + _obj.GetObjectType());
                return AdminContentMgr.instance.GetContent(contentId, objTypeName);
            }
        }


        protected void OnClickSaveBtn(object sender, EventArgs e)
        {
            IBusinessObject x = PopulateBackAndGetObject();
            ValidationResultEnum valResult = x.ValidateWithMessages();
            if (valResult==ValidationResultEnum.Valid)
            {
                SaveAndPopulate(x);
            }
            else if (valResult == ValidationResultEnum.Forcable)
            {

                SwitchView(true);

            } else
            {
                //Do nothing, error messages are already posted.
                //MessageManager.Current.PostMessage("EditObjectCgtrl.NothingSaved");
            }
        }

        private IBusinessObject PopulateBackAndGetObject()
        {
            AutoForm2.PopulateBack();
            return (IBusinessObject)ObjectGetter();
        }

        private void SaveAndPopulate(IBusinessObject x)
        {
            SaveObject(x);
            this.AutoForm2.PopulateFront();
        }

        protected void OnClickForceNoBtn(object sender, EventArgs e)
        {
            SwitchView(false);
        }

        private void SwitchView(bool b)
        {
            this.Panels.ActiveViewIndex = (b?1:0);
            //this.AutoForm2.Enabled=!b;
        }

        protected void OnClickForceYesBtn(object sender, EventArgs e)
        {
            IBusinessObject x = PopulateBackAndGetObject();
            SaveAndPopulate(x);
            SwitchView(false);
        }
        private void SaveObject(IBusinessObject x)
        {
            if (!x.IsPersisted())
            {
                //This will now be called from the interceptors.
                //x.OnFirstSave();
                //New mode
                HibernateMgr.Current.Save(x);
                HibernateMgr.Current.Session.Flush();
                MessageManager.Current.PostMessage("EditObjectCtrl.Created");
                if (OnCreateHandler != null) OnCreateHandler();

                if (this.DialogConcluded != null) this.DialogConcluded(x);
            }
            else
            {
                HibernateMgr.Current.Session.Flush();
                if (OnSaveHandler != null) OnSaveHandler();
                MessageManager.Current.PostMessage("EditObjectCtrl.Saved");
                if (this.DialogConcluded != null) this.DialogConcluded(x);
            }
            if (this.OnDoneHandler != null) OnDoneHandler();
        }


        protected void OnClickCancelBtn(object sender, EventArgs e)
        {
            AutoForm2.PopulateFront();
            MessageManager.Current.PostMessage("EditObjectCtrl.Cancelled");
            if (this.DialogConcluded != null) this.DialogConcluded(null);
            if (this.OnDoneHandler != null) OnDoneHandler();
        }

        protected void OnClickDeleteBtn(object sender, EventArgs e)
        {
            MessageManager.Current.PostMessage("EditObjectCtrl.NotImplemented");
            //            new AdminFacade().DeleteObject(this.ObjectGetter());
        }


        public void CancelDialogConcluded()
        {
            if (OnDoneHandler!=null) this.OnDoneHandler();
            this.DialogConcluded(null);
        }
    }
}