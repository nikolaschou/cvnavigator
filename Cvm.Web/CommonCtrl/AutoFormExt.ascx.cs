using System;
using System.Web.UI;
using Napp.Backend.Business.Common;
using Napp.Backend.BusinessObject;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;

namespace Nshop.Web.AdminPages.CommonCtrl
{
    public partial class AutoFormExt : System.Web.UI.UserControl, IAutoWithSourceCtrl
    {
        private static readonly string objectDeletedMethodName = "objectDeleted";


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool DoesIncludeProperty(string propName)
        {
            return this.autoForm.DoesIncludeProperty(propName);
        }

        public string OmitProperties
        {
            set { this.autoForm.OmitProperties = value; }
        }

        /// <summary>
        /// Set some properties to include only these.
        /// If this property is not assigned, all properties
        /// will be included by default.
        /// </summary>
        public string IncludeOnlyProperties
        {
            set { this.autoForm.IncludeOnlyProperties=value; }
        }

        public ObjectSource ObjectSource
        {
            get { return this.autoForm.ObjectSource; }
            set { this.autoForm.ObjectSource=value; }
        }

        public void PopulateFront()
        {
            IBusinessObject obj = ObjectSource() as IBusinessObject;
            if (obj != null)
            {
//                this.DeleteObjectLink.PageLink = AdminPage.DeleteAnyObjectPage(obj.GetObjectType(), obj.Id, objectDeletedMethodName);
                this.DeleteObjectLink.OpenAsPopup=true;
                this.DeleteObjectLink.Visible = false;
                string script = String.Format(OnDeleteCallBackScript,
                    objectDeletedMethodName,
                    "{",
                    GetClientIdToHide(),
                    AdminContentMgr.instance.GetContent("AutoFormExt.ObjectDeleted"),
                    "}"
                    );
                ScriptManager.RegisterClientScriptBlock(this.Page,typeof(AutoFormExt),"OnDeleteHandler",script,true);
            }
            else
            {
                this.DeleteObjectLink.Visible = false;
            }
            this.autoForm.PopulateFront();
        }

        private string GetClientIdToHide()
        {
            if (ClientIdToHide != null) return ClientIdToHide;   
            else return Panel1.ClientID;
        }

        /// <summary>
        /// Set this to the client-ID of the element that is to be hidden when deletion
        /// is done.
        /// </summary>
        public String ClientIdToHide;

        public void PopulateBack()
        {
            this.autoForm.PopulateBack();
        }

        /// <summary>
        /// Builds up the control tree for this form based on the object source.
        /// Be aware that no values are inserted.
        /// </summary>
        public void BuildForm()
        {

            this.autoForm.BuildForm();
        }

        /// <summary>
        /// Set this to make a name-space prefix other than the default 'Common'.
        /// </summary>
        public string ContentIdPrefix
        {
            set { this.autoForm.ContentIdPrefix=value; }
        }

        private readonly string OnDeleteCallBackScript =
            @"
            function {0}() {1}
                document.getElementById('{2}').innerHTML='{3}';
            {4}
            ";

    }
}