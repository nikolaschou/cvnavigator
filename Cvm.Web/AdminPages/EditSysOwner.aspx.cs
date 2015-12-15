using System;
using Cvm.Web.Code;
using Cvm.Web.Facade;

namespace Cvm.Web.AdminPages
{
    public partial class EditSysOwner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateFront();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.SysOwnerForm.ObjectSourceInstance = ContextObjectHelper.CurrentSysOwnerOrCreate;
            this.SysRootForm.ObjectSourceInstance = ContextObjectHelper.CurrentSysRoot;
            this.SysOwnerForm.BuildForm();
            this.SysRootForm.BuildForm();
        }

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }

        protected void OnClickSaveBtn(object sender, EventArgs e)
        {
            this.SysOwnerForm.PopulateBack();
            this.SysRootForm.PopulateBack();

            this.PopulateFront();

        }

        protected void OnClickCancelBtn(object sender, EventArgs e)
        {
            PopulateFront();
        }

        private void PopulateFront()
        {
            this.SysOwnerForm.PopulateFront();
            this.SysRootForm.PopulateFront();
        }
    }
}
