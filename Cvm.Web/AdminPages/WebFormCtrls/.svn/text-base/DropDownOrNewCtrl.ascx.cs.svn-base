using System;
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
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Cvm.Web.CommonCtrl;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate.DataFetcher;
using Napp.VeryBasic;
using Napp.Web.Auto.Annotations;
using Napp.Web.DialogCtrl;

namespace Cvm.Web.AdminPages.WebFormCtrls
{
    public partial class DropDownOrNewCtrl : System.Web.UI.UserControl, ISimpleWebControl, IControlWithInit, IFilterSysIdControl
    {
        private DialogInvokerViewState invokerViewState;
        private string fieldContentId;
        private IBusinessObject newObject;
        private string entityName;
        private bool didSetupDialog = false;
        private string REQUIRED = "REQUIRED";
        private string NOT_REQUIRED = "NOT_REQUIRED";
        
        public object ValueOfControl
        {
            get { return this.ObjectDropDown.SelectedLong; }
            set { this.ObjectDropDown.SelectedLong = (long?)value; }
        }

        public string FieldContentId
        {
            get { return fieldContentId; }
            set { 
                fieldContentId = value;
                this.ObjectDropDown.FieldContentId = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetupOnPostback();
        }

        private void SetupOnPostback()
        {
            if (invokerViewState.IsInvokingDialog)
            {
                SetupDialog();

            }
        }


        protected override void OnInit(EventArgs e)
        {
            invokerViewState = new DialogInvokerViewState(ViewState);
            base.OnInit(e);
            this.ObjectDropDown.EntityName = this.entityName;
            this.ObjectDropDown.Activate();
//            this.Page.LoadComplete += delegate { this.SetupOnPostback(); };
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.ChildControlsCreated = true;
        }

        protected void OnClick_CreateNewBtn(object sender, EventArgs e)
        {
            SetupDialog();
        }

        private void SetupDialog()
        {
            if (!didSetupDialog)
            {
                IDialogCtrl ctrl = (IDialogCtrl) this.LoadControl(CommonCtrls.EditObjectCtrl);
                EditObjectCtrl editObjCtrl = (EditObjectCtrl) ctrl;
                editObjCtrl.ObjType = this.entityName;
                editObjCtrl.ObjectGetter = ObjectCreator;
                editObjCtrl.BuildForm();
                editObjCtrl.PopulateFront();
                ctrl.DialogConcluded += MyDialogConcludedHandler;
                DialogController.RequestInstance.InvokeDialog(ctrl, invokerViewState);
                didSetupDialog = true;
            }
        }

        private object ObjectCreator()
        {
            if (this.newObject == null)
            {
                Type type = TypeParser.ParseType(this.entityName);
                ConstructorInfo constructor = type.GetConstructor(new Type[0]);
                this.newObject = (IBusinessObject) constructor.Invoke(new object[0]);
            }
            return this.newObject;
        }

        private void MyDialogConcludedHandler(Object concl)
        {
            if (concl != null)
            {
                IBusinessObject x = ((IBusinessObject)concl);
                HibernateDataFetcher.ClearKeyValueByEntityNameCache(x.GetObjectType());
                this.ObjectDropDown.Items.Clear();
                this.ObjectDropDown.Activate();
                this.ObjectDropDown.SelectedLong = x.Idfr.IdfrAsLong();
            }
        }

        /// <summary>
        /// Must be initialized with the following parameters:
        /// 1. entityName, string, not optional
        /// 2. REQUIRED|NOT_REQUIRED, optional, default is true
        /// </summary>
        /// <param name="initParms"></param>
        public void Initialize(params string[] initParms)
        {
            this.entityName = initParms[0];
            //Always include blank by default
            this.ObjectDropDown.IncludeBlank = true;

            if (initParms.Length>1)
            {
                String requiredness = initParms[1];
                if (REQUIRED.Equals(requiredness))
                {
                    this.ObjectDropDown.Required = true;
                }
                else if (NOT_REQUIRED.Equals(requiredness)) this.ObjectDropDown.Required = false;
                else throw new ArgumentException("Expected " + requiredness+" to be either "+REQUIRED+" or "+NOT_REQUIRED);
            } else
            {
                this.ObjectDropDown.Required = false;
            }
        }

        /// <summary>
        /// For now this is not used.
        /// </summary>
        public bool FilterSysId
        { get; set;
        }
    }
}